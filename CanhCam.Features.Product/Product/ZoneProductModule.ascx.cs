/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-08-27
/// Last Modified:		    2014-08-27

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class ZoneProductModule : SiteModuleControl
    {
        protected ZoneProductConfiguration config = null;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;

        private SiteMapDataSource siteMapDataSource;
        private bool isAdmin = false;
        private bool isContentAdmin = false;
        private bool isSiteEditor = false;
        private string secureSiteRoot = string.Empty;
        private string insecureSiteRoot = string.Empty;
        private bool resolveFullUrlsForMenuItemProtocolDifferences = false;
        private string navigationSiteRoot = string.Empty;
        private bool useFullUrlsForWebPage = false;
        private bool isSecureRequest = false;
        private SiteMapNode rootNode = null;
        private SiteMapNode startingNode = null;
        private gbSiteMapNode currentNode = null;

        private int languageId = -1;
        private bool isMobileSkin = false;
        private int mobileOnly = (int)ContentPublishMode.MobileOnly;
        private int webOnly = (int)ContentPublishMode.WebOnly;

        private CmsBasePage basePage;
        private bool userCanUpdate = false;
        private SiteUser currentUser;

        //private List<Product> lstAllProducts = new List<Product>();
        //private List<DiscountAppliedToItem> lstAllDiscountItems = new List<DiscountAppliedToItem>();
        //private List<int> lstZoneIds = new List<int>();

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.EnableViewState = false;

            this.Load += new EventHandler(Page_Load);
        }

        #endregion OnInit

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (HttpContext.Current == null) { return; }

            if (rootNode == null) { return; }

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<ZoneList></ZoneList>");

            var root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "Title", GetZoneTitle(currentNode));
            XmlHelper.AddNode(doc, root, "Url", FormatUrl(currentNode));
            XmlHelper.AddNode(doc, root, "Target", (currentNode.OpenInNewWindow == true ? "_blank" : "_self"));
            XmlHelper.AddNode(doc, root, "ImageUrl", (currentNode != null ? currentNode.PrimaryImage : string.Empty));
            XmlHelper.AddNode(doc, root, "SecondImageUrl", (currentNode != null ? currentNode.SecondImage : string.Empty));

            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                List<string> lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                {
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
                }
            }

            SiteMapNodeCollection allNodes = null;
            if (config.IsSubZone)
                allNodes = startingNode.ChildNodes;
            else
                allNodes = startingNode.GetAllNodes();

            foreach (SiteMapNode childNode in allNodes)
            {
                var gbNode = childNode as gbSiteMapNode;
                if (gbNode == null) { continue; }

                RenderNode(doc, root, gbNode);
            }

            if (GoogleTrackingHelper.Enable && GoogleTrackingHelper.EnableProductZone)
                XmlHelper.AddNode(doc, root, "GoogleTrackingScript", GoogleTrackingHelper.BuildGoogleImpressions(doc, "ProductZoneImpressions.xslt"));
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        private void RenderNode(XmlDocument doc, XmlElement xmlElement, gbSiteMapNode gbNode)
        {
            if (!ShouldRender(gbNode)) { return; }

            var item = doc.CreateElement("Zone");
            xmlElement.AppendChild(item);

            XmlHelper.AddNode(doc, item, "ZoneId", gbNode.ZoneId.ToInvariantString());
            XmlHelper.AddNode(doc, item, "Depth", gbNode.Depth.ToInvariantString());
            XmlHelper.AddNode(doc, item, "ChildCount", gbNode.ChildNodes.Count.ToInvariantString());
            XmlHelper.AddNode(doc, item, "IsClickable", gbNode.IsClickable.ToString().ToLower());
            XmlHelper.AddNode(doc, item, "Url", FormatUrl(gbNode));
            XmlHelper.AddNode(doc, item, "Target", (gbNode.OpenInNewWindow == true ? "_blank" : "_self"));
            XmlHelper.AddNode(doc, item, "Title", GetZoneTitle(gbNode));
            XmlHelper.AddNode(doc, item, "Description", GetDescription(gbNode));
            XmlHelper.AddNode(doc, item, "ImageUrl", gbNode.PrimaryImage);
            XmlHelper.AddNode(doc, item, "SecondImage", gbNode.SecondImage);

            var lstProducts = new List<Product>();
            if (config.ShowAllProducts)
            {
                var zoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, gbNode.ZoneId);
                if (config.MaxItemsToGet == 0)
                {
                    int iCount = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: zoneIds, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone);
                    XmlHelper.AddNode(doc, item, "ProductCount", iCount.ToString());
                }
                else if (config.MaxItemsToGet > 0)
                    lstProducts = ProductCacheHelper.GetTopAdv(siteId: siteSettings.SiteId, zoneIds: zoneIds, publishStatus: 1, languageId: languageId, position: config.ProductPosition, top: config.MaxItemsToGet, searchProductZone: ProductConfiguration.EnableProductZone);
                else
                {
                    int iCount = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: zoneIds, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone);
                    XmlHelper.AddNode(doc, item, "ProductCount", iCount.ToString());

                    lstProducts = ProductCacheHelper.GetTopAdv(siteId: siteSettings.SiteId, zoneIds: zoneIds, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone, top: Math.Abs(config.MaxItemsToGet));
                }
            }
            else
            {
                if (config.MaxItemsToGet == 0)
                {
                    int iCount = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneId: gbNode.ZoneId, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone);
                    XmlHelper.AddNode(doc, item, "ProductCount", iCount.ToString());
                }
                else if (config.MaxItemsToGet > 0)
                    lstProducts = ProductCacheHelper.GetTopAdv(siteId: siteSettings.SiteId, zoneId: gbNode.ZoneId, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone, top: config.MaxItemsToGet);
                else
                {
                    int iCount = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneId: gbNode.ZoneId, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone);
                    XmlHelper.AddNode(doc, item, "ProductCount", iCount.ToString());

                    lstProducts = ProductCacheHelper.GetTopAdv(siteId: siteSettings.SiteId, zoneId: gbNode.ZoneId, publishStatus: 1, languageId: languageId, position: config.ProductPosition, searchProductZone: ProductConfiguration.EnableProductZone, top: Math.Abs(config.MaxItemsToGet));
                }
            }

            var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -10, lstProducts);
            foreach (Product product in lstProducts)
            {
                var productXml = doc.CreateElement("Product");
                item.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
            }

            if ((currentNode != null)
                && (currentNode.ZoneGuid == gbNode.ZoneGuid) // Selected
                )
            {
                XmlHelper.AddNode(doc, item, "IsActive", "true");
            }
            else
            {
                XmlHelper.AddNode(doc, item, "IsActive", "false");
            }

            if (gbNode.ChildNodes.Count > 0)
            {
                foreach (SiteMapNode childNode in gbNode.ChildNodes)
                {
                    gbSiteMapNode gbChildNode = childNode as gbSiteMapNode;
                    if (gbChildNode == null) { continue; }

                    RenderNode(doc, item, gbChildNode);
                }
            }
        }

        private string GetZoneTitle(gbSiteMapNode mapNode)
        {
            if (mapNode == null)
                return string.Empty;

            string title = mapNode.Title;
            if (languageId > 0 && mapNode["Title" + languageId.ToString()] != null)
                title = mapNode["Title" + languageId.ToString()];

            return title;
        }

        private string GetDescription(gbSiteMapNode mapNode)
        {
            if (mapNode == null)
                return string.Empty;

            string str = mapNode.Description;
            if (languageId > 0 && mapNode["Description" + languageId.ToString()] != null)
                str = mapNode["Description" + languageId.ToString()];

            return str;
        }

        private string FormatDate(object startDate, string format = "")
        {
            if (startDate == null)
                return string.Empty;

            if (timeZone != null)
            {
                return TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(startDate), timeZone).ToString(format);
            }

            return Convert.ToDateTime(startDate).AddHours(timeOffset).ToString(format);
        }

        private bool ShouldRender(gbSiteMapNode mapNode)
        {
            if (mapNode == null) { return false; }

            if (
                languageId > 0
                && mapNode["Title" + languageId.ToString()] == null
                )
                return false;

            if (mapNode.Roles == null)
            {
                if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor)) { return false; }
            }
            else
            {
                if ((!isAdmin) && (mapNode.Roles.Count == 1) && (mapNode.Roles[0].ToString() == "Admins")) { return false; }

                if ((!isAdmin) && (!isContentAdmin) && (!isSiteEditor) && (!WebUser.IsInRoles(mapNode.Roles))) { return false; }
            }

            //if (!mapNode.IncludeInMenu && config.IsSubMenu) { return false; }
            if (!mapNode.IsPublished) { return false; }

            if (config.ZonePosition > 0)
            {
                if ((mapNode.Position & config.ZonePosition) == 0)
                    return false;
            }

            if ((mapNode.HideAfterLogin) && (Page.Request.IsAuthenticated)) { return false; }

            if ((!isMobileSkin) && (mapNode.PublishMode == mobileOnly)) { return false; }

            if ((isMobileSkin) && (mapNode.PublishMode == webOnly)) { return false; }

            return true;
        }

        private bool GetClickable(gbSiteMapNode mapNode)
        {
            if (mapNode == null)
                return false;

            bool isClickable = false;
            if (languageId > 0)
            {
                if (mapNode["IsClickable" + languageId.ToString()] != null)
                    isClickable = Convert.ToBoolean(mapNode["IsClickable" + languageId.ToString()]);
            }
            else
                isClickable = mapNode.IsClickable;

            return isClickable;
        }

        private string FormatUrl(gbSiteMapNode mapNode)
        {
            if (!GetClickable(mapNode))
                return "#";

            string url = string.Empty;
            if (WebConfigSettings.EnableHierarchicalFriendlyUrls)
            {
                url = mapNode.UrlExpand;
                if (languageId > 0 && mapNode["UrlExpand" + languageId.ToString()] != null)
                    url = mapNode["UrlExpand" + languageId.ToString()];
            }
            else
            {
                url = mapNode.Url;
                if (languageId > 0 && mapNode["Url" + languageId.ToString()] != null)
                    url = mapNode["Url" + languageId.ToString()];
            }

            string itemUrl = Page.ResolveUrl(url);
            bool useFullUrls = false;

            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (isSecureRequest)
                {
                    if (
                        (!mapNode.UseSsl)
                        && (!siteSettings.UseSslOnAllPages)
                        && (url.StartsWith("~/"))
                        )
                    {
                        itemUrl = insecureSiteRoot + url.Replace("~/", "/");
                        useFullUrls = true;
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (url.StartsWith("~/"))
                        {
                            itemUrl = secureSiteRoot + url.Replace("~/", "/");
                            useFullUrls = true;
                        }
                    }
                }
            }

            if (
                !useFullUrls
                && useFullUrlsForWebPage
                && url.StartsWith("~/")
                )
                itemUrl = navigationSiteRoot + url.Replace("~/", "/");

            return itemUrl;
        }

        private void PopulateLabels()
        {
        }

        private void LoadSettings()
        {
            EnsureConfiguration();

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            languageId = WorkingCulture.LanguageId;

            isAdmin = WebUser.IsAdmin;
            if (!isAdmin) { isContentAdmin = WebUser.IsContentAdmin; }
            if ((!isAdmin) && (!isContentAdmin)) { isSiteEditor = SiteUtils.UserIsSiteEditor(); }

            basePage = Page as CmsBasePage;
            userCanUpdate = ProductPermission.CanUpdate;
            currentUser = SiteUtils.GetCurrentSiteUser();

            useFullUrlsForWebPage = WebConfigSettings.UseFullUrlsForWebPage;
            resolveFullUrlsForMenuItemProtocolDifferences = WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences;
            navigationSiteRoot = WebUtils.GetSiteRoot();
            if (resolveFullUrlsForMenuItemProtocolDifferences)
            {
                secureSiteRoot = WebUtils.GetSecureSiteRoot();
                insecureSiteRoot = secureSiteRoot.Replace("https", "http");
            }

            isSecureRequest = SiteUtils.IsSecureRequest();
            isMobileSkin = SiteUtils.UseMobileSkin();
            siteMapDataSource = new SiteMapDataSource
            {
                SiteMapProvider = "canhcamsite" + siteSettings.SiteId.ToInvariantString()
            };

            rootNode = siteMapDataSource.Provider.RootNode;
            currentNode = SiteUtils.GetCurrentZoneSiteMapNode(rootNode);
            startingNode = rootNode;

            if (config.IsSubZone)
            {
                startingNode = currentNode;
            }
        }

        private void EnsureConfiguration()
        {
            if (config == null)
                config = new ZoneProductConfiguration(Settings);
        }

        public override bool UserHasPermission()
        {
            if (!Request.IsAuthenticated)
                return false;

            bool hasPermission = false;
            bool isAdminOrContentAdmin = WebUser.IsAdminOrContentAdmin && SiteUtils.UserIsSiteEditor();
            EnsureConfiguration();

            if (config.ZonePosition > 0)
            {
                if (isAdminOrContentAdmin || WebUser.IsInRoles(siteSettings.RolesThatCanManageZones))
                {
                    this.LiteralExtraMarkup = "<dd><a class='ActionLink choosezonelink' href='"
                            + SiteRoot
                            + "/AdminCP/ZonePosition.aspx?pos=" + config.ZonePosition.ToString() + "'><i class='fa fa-hand-o-up'></i> " + ResourceHelper.GetResourceString("Resource", "ZoneSelectLabel") + "</a></dd>";

                    hasPermission = true;
                }
            }

            if (config.ProductPosition > 0)
            {
                if (isAdminOrContentAdmin)
                {
                    this.LiteralExtraMarkup += "<li><a class='ActionLink chooseproductlink' href='"
                            + SiteRoot
                            + "/Product/AdminCP/ProductSpecial.aspx?pos=" + config.ProductPosition.ToString() + "'><i class='fa fa-hand-o-up'></i> " + ProductResources.ProductSelectLabel + "</a></li>";

                    hasPermission = true;
                }
            }

            return hasPermission;
        }
    }

}
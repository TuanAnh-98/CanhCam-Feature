using CanhCam.Business;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: a904d9fa-634a-41a5-98af-2d1e76086df8
    public partial class PromotionDetailModule : SiteModuleControl
    {
        private PromotionDetailConfiguration config = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            var promotionId = WebUtils.ParseInt32FromQueryString("PromotionID", -1);
            var obj = new Discount(promotionId);
            if (obj == null
                || obj.DiscountId < 0
                || obj.PageId <= 0
                )
                return;

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<PromotionDetail></PromotionDetail>");
            var root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "Title", obj.Name);
            XmlHelper.AddNode(doc, root, "BriefContent", obj.BriefContent);
            XmlHelper.AddNode(doc, root, "FullContent", obj.FullContent);
            //XmlHelper.AddNode(doc, root, "EditLink", BuildEditLink(obj.ItemID));
            XmlHelper.AddNode(doc, root, "PromotionId", obj.DiscountId.ToString());

            var imageFolderPath = PromotionsHelper.ImagePath(siteSettings.SiteId, obj.DiscountId);
            if (!string.IsNullOrEmpty(obj.ImageFile))
                XmlHelper.AddNode(doc, root, "ImageUrl", imageFolderPath + obj.ImageFile);
            if (!string.IsNullOrEmpty(obj.BannerFile))
                XmlHelper.AddNode(doc, root, "BannerUrl", imageFolderPath + obj.BannerFile);
            XmlHelper.AddNode(doc, root, "EditLink", SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx?DiscountID=" + obj.DiscountId.ToString());

            var lstDiscountContent = DiscountContent.GetByDiscount(obj.DiscountId, WorkingCulture.LanguageId);
            if (lstDiscountContent.Count > 0)
            {
                //var siteMapDataSource = new SiteMapDataSource();
                //siteMapDataSource.SiteMapProvider = "canhcamsite" + siteSettings.SiteId.ToInvariantString();

                //var rootNode = siteMapDataSource.Provider.RootNode;
                //var allNodes = rootNode.ChildNodes;

                var timeOffset = SiteUtils.GetUserTimeOffset();
                var timeZone = SiteUtils.GetUserTimeZone();
                var languageId = WorkingCulture.LanguageId;
                var basePage = Page as CmsBasePage;
                var userCanUpdate = ProductPermission.CanUpdate;
                var currentUser = SiteUtils.GetCurrentSiteUser();

                foreach (var content in lstDiscountContent)
                {
                    var contentXml = doc.CreateElement("Content");
                    root.AppendChild(contentXml);

                    XmlHelper.AddNode(doc, contentXml, "Title", content.Title);
                    XmlHelper.AddNode(doc, contentXml, "Description", content.Description);

                    if (!string.IsNullOrEmpty(content.BannerFile))
                        XmlHelper.AddNode(doc, contentXml, "ImageUrl", imageFolderPath + content.BannerFile);

                    XmlHelper.AddNode(doc, contentXml, "LoadType", content.LoadType.ToString());

                    if (!string.IsNullOrEmpty(content.ZoneIDs))
                    {
                        var lstZoneIDs = content.ZoneIDs.SplitOnCharAndTrim(';');

                        var lstZoneIDsDistinct = lstZoneIDs.Distinct().ToList();
                        foreach (var sZoneID in lstZoneIDs)
                        {
                            var zoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, Convert.ToInt32(sZoneID)).SplitOnCharAndTrim(';');
                            if (zoneIds.Count > 0)
                                lstZoneIDsDistinct.AddRange(zoneIds);
                        }
                        lstZoneIDsDistinct = lstZoneIDsDistinct.Distinct().ToList();
                        if (lstZoneIDsDistinct.Count > 0)
                        {
                            var pageSize = 10;
                            var lstProducts = new List<Product>();
                            var propertyCondition = "EXISTS (SELECT 1 FROM gb_DiscountAppliedToItems di WHERE di.DiscountID=" + obj.DiscountId.ToString() + " AND ((di.AppliedType=0 AND di.ItemID=p.ZoneID) OR (di.AppliedType=1 AND (di.ItemID=-1 OR di.ItemID=p.ProductID))) )";

                            if (ConfigHelper.GetBoolProperty("PromotionSortByItem", false))
                                lstProducts = Product.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: string.Join(";", lstZoneIDsDistinct.ToArray()), publishStatus: 1, languageId: languageId, propertyCondition: propertyCondition, discountId: obj.DiscountId, pageNumber: 1, pageSize: pageSize + 1);
                            else
                                lstProducts = Product.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: string.Join(";", lstZoneIDsDistinct.ToArray()), publishStatus: 1, languageId: languageId, propertyCondition: propertyCondition, pageNumber: 1, pageSize: pageSize + 1);

                            if (lstProducts.Count > pageSize)
                            {
                                lstProducts.RemoveAt(lstProducts.Count - 1);
                                XmlHelper.AddNode(doc, contentXml, "NextPageNumber", "2");
                            }
                            var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -1, lstProducts);
                            foreach (var product in lstProducts)
                            {
                                var productXml = doc.CreateElement("Product");
                                contentXml.AppendChild(productXml);

                                ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
                            }
                        }

                        var zIds = string.Empty;
                        var sepa = string.Empty;
                        foreach (var sZoneID in lstZoneIDs)
                        {
                            var zoneXml = doc.CreateElement("Zone");
                            contentXml.AppendChild(zoneXml);

                            var node = SiteUtils.GetSiteMapNodeByZoneId(Convert.ToInt32(sZoneID));
                            if (node != null)
                            {
                                XmlHelper.AddNode(doc, zoneXml, "ZoneId", node.ZoneId.ToString());
                                XmlHelper.AddNode(doc, zoneXml, "Title", node.Title);

                                if (content.LoadType < 0)
                                {
                                    var zoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, node.ZoneId);
                                    var propertyCondition = "EXISTS (SELECT 1 FROM gb_DiscountAppliedToItems di WHERE di.DiscountID=" + obj.DiscountId.ToString() + " AND ((di.AppliedType=0 AND di.ItemID=p.ZoneID) OR (di.AppliedType=1 AND (di.ItemID=-1 OR di.ItemID=p.ProductID))) )";
                                    var pageSize = 12;
                                    var lstProducts = Product.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: zoneIds, publishStatus: 1,
                                        languageId: languageId, propertyCondition: propertyCondition, pageNumber: 1, pageSize: pageSize + 1);
                                    if (lstProducts.Count > pageSize)
                                    {
                                        lstProducts.RemoveAt(lstProducts.Count - 1);
                                        XmlHelper.AddNode(doc, zoneXml, "NextPageNumber", "2");
                                    }
                                    var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -1, lstProducts);
                                    foreach (var product in lstProducts)
                                    {
                                        var productXml = doc.CreateElement("Product");
                                        zoneXml.AppendChild(productXml);

                                        ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
                                    }
                                }

                                zIds += sepa + node.ZoneId.ToString();
                                sepa = ";";
                            }
                        }

                        XmlHelper.AddNode(doc, contentXml, "ZoneIds", zIds);
                    }
                }
            }

            if (config.PageSize > 0)
            {
                var pageNumber = 1;
                var pageSize = config.PageSize;
                var lstDiscount = Discount.GetPage(SiteId, -1, -1, 0, -1, -1, obj.DiscountId, -1, pageNumber, pageSize);
                foreach (var obj2 in lstDiscount)
                {
                    var discountXml = doc.CreateElement("OtherPromotion");
                    root.AppendChild(discountXml);

                    XmlHelper.AddNode(doc, discountXml, "Title", obj2.Name);
                    XmlHelper.AddNode(doc, discountXml, "BriefContent", obj2.BriefContent);
                    XmlHelper.AddNode(doc, discountXml, "FullContent", obj2.FullContent);

                    var imageFolderPath2 = PromotionsHelper.ImagePath(siteSettings.SiteId, obj2.DiscountId);
                    if (!string.IsNullOrEmpty(obj2.ImageFile))
                        XmlHelper.AddNode(doc, discountXml, "ImageUrl", imageFolderPath2 + obj2.ImageFile);
                    if (!string.IsNullOrEmpty(obj2.BannerFile))
                        XmlHelper.AddNode(doc, discountXml, "BannerUrl", imageFolderPath2 + obj2.BannerFile);
                    XmlHelper.AddNode(doc, discountXml, "Url", PromotionsHelper.FormatPromotionUrl(obj2.Url, obj2.DiscountId));
                }
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        protected virtual void LoadSettings()
        {
            EnsureConfiguration();

            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }

        private void EnsureConfiguration()
        {
            if (config == null)
                config = new PromotionDetailConfiguration(Settings);
        }

        public override bool UserHasPermission()
        {
            if (!Request.IsAuthenticated)
                return false;

            bool hasPermission = false;

            if (ProductPermission.CanManageManufacturer)
            {
                this.LiteralExtraMarkup = "<dd><a class='ActionLink manufacturerlistlink' href='"
                        + SiteRoot
                        + "/Product/AdminCP/Promotions/Discounts.aspx'>" + ProductResources.DiscountPageTitle + "</a></dd>";

                hasPermission = true;
            }

            return hasPermission;
        }
    }
}
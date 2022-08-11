using CanhCam.Business;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Linq;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: a904d9fa-634a-41a5-98af-2d1e76086df8
    public partial class ManufacturerDetailModule : SiteModuleControl
    {
        private ManufacturerDetailConfiguration config = null;

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
            var manufacturerId = WebUtils.ParseInt32FromQueryString("ManufacturerID", -1);
            var manufacturer = new Manufacturer(manufacturerId);
            if (manufacturer == null
                || manufacturer.ManufacturerId < 0
                || !manufacturer.IsPublished
                || manufacturer.IsDeleted)
                return;

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<ManufacturerDetail></ManufacturerDetail>");
            var root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);

            XmlHelper.AddNode(doc, root, "Title", manufacturer.Name);
            XmlHelper.AddNode(doc, root, "Description", manufacturer.Description);
            XmlHelper.AddNode(doc, root, "ImageUrl", manufacturer.PrimaryImage);
            XmlHelper.AddNode(doc, root, "SecondImageUrl", manufacturer.SecondImage);
            XmlHelper.AddNode(doc, root, "EditLink", SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx?ManufacturerID=" + manufacturer.ManufacturerId.ToString());

            var timeOffset = SiteUtils.GetUserTimeOffset();
            var timeZone = SiteUtils.GetUserTimeZone();
            var languageId = WorkingCulture.LanguageId;
            var basePage = Page as CmsBasePage;
            var userCanUpdate = ProductPermission.CanUpdate;
            var currentUser = SiteUtils.GetCurrentSiteUser();

            if (config.LoadZones)
            {
                var lstZoneGuids = ZoneItem.GetByItem(manufacturer.Guid).Select(s => s.ZoneGuid).Distinct().ToList();
                if (lstZoneGuids.Count > 0)
                {
                    var siteMapDataSource = new SiteMapDataSource
                    {
                        SiteMapProvider = "canhcamsite" + siteSettings.SiteId.ToInvariantString()
                    };

                    var rootNode = siteMapDataSource.Provider.RootNode;
                    var allNodes = rootNode.ChildNodes;
                    //var allNodes = rootNode.GetAllNodes();

                    foreach (var childNode in allNodes)
                    {
                        var gbNode = childNode as gbSiteMapNode;
                        if (gbNode == null) { continue; }

                        if (lstZoneGuids.Contains(gbNode.ZoneGuid))
                        {
                            XmlElement item = doc.CreateElement("Zone");
                            root.AppendChild(item);

                            XmlHelper.AddNode(doc, item, "Title", gbNode.Title);

                            if (config.LoadProducts)
                            {
                                var zoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, gbNode.ZoneId);
                                var lstProducts = Product.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: zoneIds, publishStatus: 1, languageId: languageId, manufactureId: manufacturer.ManufacturerId, pageNumber: 1, pageSize: config.PageSize);
                                var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProducts);
                                foreach (var product in lstProducts)
                                {
                                    var productXml = doc.CreateElement("Product");
                                    item.AppendChild(productXml);

                                    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
                                }
                            }
                        }
                    }
                }
            }
            else if (config.LoadProducts)
            {
                var pageNumber = WebUtils.ParseInt32FromQueryString(ProductHelper.QueryStringPageNumberParam, 1);
                var totalRows = Product.GetCountAdv(siteId: siteSettings.SiteId, publishStatus: 1, languageId: languageId, manufactureId: manufacturer.ManufacturerId);
                var lstProducts = Product.GetPageAdv(pageNumber: pageNumber, pageSize: config.PageSize, siteId: siteSettings.SiteId, publishStatus: 1, languageId: languageId, manufactureId: manufacturer.ManufacturerId);
                var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProducts);
                foreach (var product in lstProducts)
                {
                    var productXml = doc.CreateElement("Product");
                    root.AppendChild(productXml);

                    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
                }

                var pageUrl = ManufacturerHelper.FormatManufacturerUrl(manufacturer.Url, manufacturer.ManufacturerId);
                if (pageUrl.Contains("?"))
                    pageUrl += "&amp;" + ProductHelper.QueryStringPageNumberParam + "={0}";
                else
                    pageUrl += "?" + ProductHelper.QueryStringPageNumberParam + "={0}";

                pgr.PageURLFormat = pageUrl;
                pgr.ShowFirstLast = true;
                pgr.PageSize = config.PageSize;
                pgr.ItemCount = totalRows;
                pgr.CurrentIndex = pageNumber;
                divPager.Visible = (totalRows > config.PageSize);
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", "ManufacturerDetail.xslt"), doc);
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
                config = new ManufacturerDetailConfiguration(Settings);
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
                        + "/Product/AdminCP/Manufacturers.aspx'>" + ProductResources.ManufacturerListLink + "</a></dd>";

                hasPermission = true;
            }

            return hasPermission;
        }
    }
}

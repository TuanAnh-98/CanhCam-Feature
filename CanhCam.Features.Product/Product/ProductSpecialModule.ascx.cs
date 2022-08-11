/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-28
/// Last Modified:			2014-08-28

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductSpecialModule : SiteModuleControl
    {
        private ProductSpecialConfiguration config = null;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;

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
            XmlDocument doc = new XmlDocument
            {
                XmlResolver = null
            };

            doc.LoadXml("<ProductList></ProductList>");
            XmlElement root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "ViewMore", ProductResources.ViewMoreLabel);

            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                List<string> lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                {
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
                }
            }

            var basePage = Page as CmsBasePage;
            var userCanUpdate = ProductPermission.CanUpdate;
            var currentUser = SiteUtils.GetCurrentSiteUser();

            var lstProducts = new List<Product>();
            if (config.Position == 10)
            {
                var lstProductsInCart = Product.GetByShoppingCart(SiteId, CartHelper.GetCartSessionGuid(SiteId), ShoppingCartTypeEnum.ShoppingCart);
                if (lstProductsInCart.Count > 0)
                    foreach (var product in lstProductsInCart)
                    {
                        lstProducts = Product.GetRelatedProducts(basePage.SiteId, siteSettings.SiteGuid, lstProductsInCart[0].ProductGuid, false, ProductConfiguration.RelatedProductsTwoWayRelationship);
                        if (lstProducts.Count >= config.MaxProductsToGet)
                            break;
                    }
            }
            else if (ProductConfiguration.RecentlyViewedProductsEnabled && config.Position == 0)
                lstProducts = ProductHelper.GetRecentlyViewedProducts(config.MaxProductsToGet);
            else if (config.ZoneId > -1)
            {
                var zoneId = config.ZoneId;
                if (zoneId == 0)
                    zoneId = CurrentZone.ZoneId;

                var zoneRangeIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, zoneId);
                lstProducts = ProductCacheHelper.GetPageAdv(pageNumber: 1, pageSize: config.MaxProductsToGet, siteId: siteSettings.SiteId, zoneIds: zoneRangeIds, publishStatus: 1, languageId: WorkingCulture.LanguageId, position: config.Position, searchProductZone: ProductConfiguration.EnableProductZone);
            }
            else
                lstProducts = ProductCacheHelper.GetTopAdv(siteId: SiteId, publishStatus: 1, languageId: WorkingCulture.LanguageId, position: config.Position, searchProductZone: ProductConfiguration.EnableProductZone, top: config.MaxProductsToGet);

            var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -1, lstProducts);
            foreach (Product product in lstProducts)
            {
                var productXml = doc.CreateElement("Product");
                root.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
            }

            if (GoogleTrackingHelper.Enable && GoogleTrackingHelper.EnableProductSpecial)
                XmlHelper.AddNode(doc, root, "GoogleTrackingScript", GoogleTrackingHelper.BuildGoogleImpressions(doc));
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        protected virtual void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

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
                config = new ProductSpecialConfiguration(Settings);
        }

        public override bool UserHasPermission()
        {
            if (!Request.IsAuthenticated)
                return false;

            var hasPermission = false;
            EnsureConfiguration();

            if (config.Position > 0)
            {
                if (WebUser.IsAdminOrContentAdmin && SiteUtils.UserIsSiteEditor())
                {
                    this.LiteralExtraMarkup = "<dd><a class='ActionLink chooseproductlink' href='"
                            + SiteRoot
                            + "/Product/AdminCP/ProductSpecial.aspx?pos=" + config.Position.ToString() + "'><i class='fa fa-list'></i> " + ProductResources.ProductSelectLabel + "</a></dd>";

                    hasPermission = true;
                }
            }

            return hasPermission;
        }
    }
}
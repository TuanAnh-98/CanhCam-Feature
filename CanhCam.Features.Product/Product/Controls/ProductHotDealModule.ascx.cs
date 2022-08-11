/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-28
/// Last Modified:			2014-08-28

using CanhCam.Business;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections.Generic;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductHotDealModule : SiteModuleControl
    {
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

            var currentHotDeal = Discount.GetActive(siteSettings.SiteId, (int)DiscountType.Deal);
            if (currentHotDeal != null && currentHotDeal.DiscountId > 0)
            {
                var lstProducts = Product.GetPageAdv(publishStatus: 1, siteId: siteSettings.SiteId, discountId: currentHotDeal.DiscountId);
                var basePage = Page as CmsBasePage;
                var userCanUpdate = ProductPermission.CanUpdate;
                var currentUser = SiteUtils.GetCurrentSiteUser();
                var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -1, lstProducts);
                foreach (Product product in lstProducts)
                {
                    var productXml = doc.CreateElement("Product");
                    root.AppendChild(productXml);

                    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
                }
            }
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        protected virtual void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }
    }
}
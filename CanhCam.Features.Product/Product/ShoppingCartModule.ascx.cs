/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-07-20
/// Last Modified:			2015-07-20

using CanhCam.Business;
using CanhCam.Web.Framework;
using System;
using System.Collections;
using System.Web;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: 34fc44b5-99ac-4e22-96e9-c5f2778bfdd2
    public partial class ShoppingCartModule : SiteModuleControl
    {
        private ShoppingCartConfiguration config = null;

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
            var doc = GetShoppingCartXmlFromContext();
            var root = doc.DocumentElement;
            if (root != null)
            {
                root["ModuleTitle"].InnerText = this.Title;
                root["ZoneTitle"].InnerText = CurrentZone.Name;

                if (config.CheckoutZoneId > 0)
                {
                    if (root["CheckoutUrl"] != null)
                        root["CheckoutUrl"].InnerText = CartHelper.GetZoneUrl(config.CheckoutZoneId);
                }

                if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
                {
                    var lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');
                    foreach (string item in lstResourceKeys)
                    {
                        if (root[item] != null)
                            root[item].InnerText = ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item);
                        else
                            XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
                    }
                }
            }
            if (GoogleTrackingHelper.Enable 
                && GoogleTrackingHelper.EnableCheckoutPage 
                && GoogleTrackingHelper.CheckoutPageZoneId > 0
                && CurrentZone.ZoneId == GoogleTrackingHelper.CheckoutPageZoneId)
                XmlHelper.AddNode(doc, root, "GoogleTrackingScript", GoogleTrackingHelper.BuildGoogleCheckout(doc));

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        private XmlDocument GetShoppingCartXmlFromContext()
        {
            var contextCartKey = "CurrentShoppingCart";
            if (!config.IsWishlist)
            {
                if (HttpContext.Current.Items[contextCartKey] != null)
                    return (XmlDocument)HttpContext.Current.Items[contextCartKey];
            }

            var cartType = Business.ShoppingCartTypeEnum.ShoppingCart;
            if (config.IsWishlist)
                cartType = Business.ShoppingCartTypeEnum.Wishlist;

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            var root = CartHelper.BuildShoppingCartXml(SiteId, Guid.Empty, out doc, cartType);

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "IsAuthenticated", Request.IsAuthenticated.ToString());
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);

            if (config.CheckoutZoneId > 0)
                XmlHelper.AddNode(doc, root, "CheckoutUrl", CartHelper.GetZoneUrl(config.CheckoutZoneId));

            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                var lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');
                foreach (string item in lstResourceKeys)
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
            }
            var strMessages = ProductConfiguration.ShoppingPageMessages;
            if (!string.IsNullOrEmpty(strMessages))
            {
                var lstMessages = strMessages.SplitOnCharAndTrim(';');
                foreach (string item in lstMessages)
                    XmlHelper.AddNode(doc, root, item, MessageTemplate.GetMessage(item));
            }
            if (!config.IsWishlist)
                HttpContext.Current.Items[contextCartKey] = doc;
            XmlHelper.AddNode(doc, root, "VoucherEnable", VoucherHelper.Enable.ToString());
            if (VoucherHelper.Enable)
            {
                XmlHelper.AddNode(doc, root, "VoucherShoppingCartDescription", MessageTemplate.GetMessage("VoucherShoppingCartDescription"));
                var codes = VoucherHelper.GetVoucherCodeApplied();
                foreach (var code in codes)
                {
                    var cartItemXml = doc.CreateElement("Vouchers");
                    root.AppendChild(cartItemXml);
                    XmlHelper.AddNode(doc, cartItemXml, "VoucherCode", code);
                }
            }

            return doc;
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
                config = new ShoppingCartConfiguration(Settings);
        }
    }
}

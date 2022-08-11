/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-07-30
/// Last Modified:			2015-07-30

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: c27d7534-f00a-43eb-b7fd-b17e4e9d6e70
    public partial class CheckoutMethodModule : SiteModuleControl
    {
        private CheckoutMethodConfiguration config = null;
        private Order order = null;
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckoutMethodModule));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (CartHelper.GetShoppingCart(SiteId, ShoppingCartTypeEnum.ShoppingCart).Count == 0)
            {
                CartHelper.SetupRedirectToCartPage(this);
                return;
            }

            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<CheckoutMethod></CheckoutMethod>");
            XmlElement root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "CartPageUrl", CartHelper.GetCartUrl());
            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                List<string> lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                {
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
                }
            }

            XmlHelper.AddNode(doc, root, "CompanyNameText", ProductResources.CheckoutCompanyName);
            XmlHelper.AddNode(doc, root, "CompanyTaxCodeText", ProductResources.CheckoutCompanyTaxCode);
            XmlHelper.AddNode(doc, root, "CompanyAddressText", ProductResources.CheckoutCompanyAddress);
            XmlHelper.AddNode(doc, root, "OrderNoteText", ProductResources.CheckoutOrderNote);

            XmlHelper.AddNode(doc, root, "CartUrl", CartHelper.GetCartUrl());
            if (config.CheckoutNextZoneId > 0)
                XmlHelper.AddNode(doc, root, "NextPageUrl", CartHelper.GetZoneUrl(config.CheckoutNextZoneId));

            int languageId = WorkingCulture.LanguageId;
            string shippingMethod = string.Empty;
            string paymentMethod = string.Empty;
            var lstShippingMethods = ShippingMethod.GetByActive(siteSettings.SiteId, 1, languageId);
            foreach (ShippingMethod shipping in lstShippingMethods)
            {
                XmlElement shippingItemXml = doc.CreateElement("Shipping");
                root.AppendChild(shippingItemXml);

                XmlHelper.AddNode(doc, shippingItemXml, "Title", shipping.Name);
                XmlHelper.AddNode(doc, shippingItemXml, "Description", shipping.Description);
                XmlHelper.AddNode(doc, shippingItemXml, "ShippingProvider", shipping.ShippingProvider.ToString());
                XmlHelper.AddNode(doc, shippingItemXml, "Id", shipping.ShippingMethodId.ToString());

                if (order != null && shipping.ShippingMethodId == order.ShippingMethod)
                {
                    XmlHelper.AddNode(doc, shippingItemXml, "IsActive", "true");
                    shippingMethod = shipping.Name;
                }
                if (order != null)
                {
                    // Push all service list 
                    List<ShippingOption> options = new List<ShippingOption>();
                    string errorMessage = string.Empty;
                    if (shipping.ShippingProvider == (int)ShippingMethodProvider.VittelPost)
                    {
                        if (options != null)
                            foreach (var item in options)
                            {
                                XmlElement serviceXml = doc.CreateElement("Service");
                                shippingItemXml.AppendChild(serviceXml);
                                XmlHelper.AddNode(doc, serviceXml, "Name", item.Name);
                                XmlHelper.AddNode(doc, serviceXml, "Value", item.Value.ToString());
                            }
                    }

                }

                if (ShippingHelper.EnableFastShipping && shipping.ShippingMethodId == ShippingHelper.FastShipMethodId)
                    XmlHelper.AddNode(doc, shippingItemXml, "IsFastMethodId", "true");

            }

            var lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, 1, languageId);
            foreach (PaymentMethod payment in lstPaymentMethods)
            {
                XmlElement paymentItemXml = doc.CreateElement("Payment");
                root.AppendChild(paymentItemXml);

                XmlHelper.AddNode(doc, paymentItemXml, "Title", payment.Name);
                XmlHelper.AddNode(doc, paymentItemXml, "Description", payment.Description);
                XmlHelper.AddNode(doc, paymentItemXml, "Id", payment.PaymentMethodId.ToString());

                if (order != null && payment.PaymentMethodId == order.PaymentMethod)
                {
                    XmlHelper.AddNode(doc, paymentItemXml, "IsActive", "true");
                    paymentMethod = payment.Name;
                }
            }

            if (order != null)
            {
                XmlHelper.AddNode(doc, root, "CompanyName", order.InvoiceCompanyName);
                XmlHelper.AddNode(doc, root, "CompanyTaxCode", order.InvoiceCompanyTaxCode);
                XmlHelper.AddNode(doc, root, "CompanyAddress", order.InvoiceCompanyAddress);
                XmlHelper.AddNode(doc, root, "OrderNote", order.OrderNote);
                XmlHelper.AddNode(doc, root, "ShippingMethodId", order.ShippingMethod.ToString());
                XmlHelper.AddNode(doc, root, "ShippingMethod", shippingMethod);
                XmlHelper.AddNode(doc, root, "PaymentMethodId", order.PaymentMethod.ToString());
                XmlHelper.AddNode(doc, root, "PaymentMethod", paymentMethod);
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

            order = CartHelper.GetOrderSession(siteSettings.SiteId);
        }

        private void EnsureConfiguration()
        {
            if (config == null)
                config = new CheckoutMethodConfiguration(Settings);
        }
    }
}

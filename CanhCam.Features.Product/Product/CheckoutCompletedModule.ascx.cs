using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: 47612562-8774-4387-868c-f54c2c707367
    public partial class CheckoutCompletedModule : SiteModuleControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckoutCompletedModule));

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = true;
            this.Page.EnableViewState = true;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            var htmlBody = MessageTemplate.GetMessage("CheckoutCompletedMessage");
            var order = CartHelper.GetOrderSavedSession(siteSettings.SiteId);
            var paymentStatus = ProductResources.OrderSuccess;
            var failureReasons = string.Empty;
            if (order != null && !string.IsNullOrEmpty(order.OrderCode))
                order = Order.GetByCode(order.OrderCode);
            if (order == null)
            {
                //Try get by Order Guid
                int itemId = WebUtils.ParseInt32FromQueryString("itemId", -1);
                order = new Order(itemId);

                if (order == null || order.OrderId == -1)
                {
                    SiteUtils.RedirectToHomepage();
                    return;
                }
            }

            string orderCode = order.OrderCode;


            if (PaymentHelper.IsOnlinePayment(order.PaymentMethod) && ConfigHelper.GetBoolProperty("Order:EnablePaymentAgain", true))
            {
                if (order.PaymentStatus == (int)OrderPaymentStatus.Successful)
                {
                    paymentStatus = ProductResources.OrderAndPaymentSuccess;
                }
                else //if (order.PaymentStatus == (int)OrderPaymentStatus.NotSuccessful)
                {
                    if (ConfigHelper.GetBoolProperty("Order:CheckoutPaymentNotSuccessdTemplate", false))
                    {
                        lblContent.Text = string.Empty;
                        var doc = new XmlDocument
                        {
                            XmlResolver = null
                        };
                        doc.LoadXml("<OrderDetail></OrderDetail>");
                        var root = doc.DocumentElement;
                        var timeOffset = SiteUtils.GetUserTimeOffset();
                        var timeZone = SiteUtils.GetUserTimeZone();
                        ProductHelper.BuildOrderXml(doc, root, order, timeZone, timeOffset);

                        //get data saved in session
                        var orderSession = CartHelper.GetOrderSavedSession(siteSettings.SiteId);
                        if (orderSession != null)
                        {
                            order.BankCode = orderSession.BankCode;
                            order.DiscountPayment = orderSession.DiscountPayment;
                        }
                        //data in session
                        XmlHelper.AddNode(doc, root, "BankCode", order.BankCode);
                        XmlHelper.AddNode(doc, root, "DiscountPaymentTotal", ProductHelper.FormatPrice(order.DiscountPayment, true));
                        XmlHelper.AddNode(doc, root, "DiscountPaymentValue", ProductHelper.FormatPrice(order.DiscountPayment));


                        BuildPaymentByOtherMethods(order, doc, root);
                        this.ModuleConfiguration.CssClass = string.Empty;
                        XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", "PaymentAgainPage.xslt"), doc);

                        return;
                    }
                    else
                    {

                        htmlBody = MessageTemplate.GetMessage("CheckoutPaymentNotSuccessdMessage");
                        paymentStatus = ProductResources.OrderAndPaymentNotSuccess;

                        var doc = new XmlDocument
                        {
                            XmlResolver = null
                        };
                        doc.LoadXml("<PaymentMethod></PaymentMethod>");
                        XmlElement root = doc.DocumentElement;
                        XmlHelper.AddNode(doc, root, "OrderCode", order.OrderCode);

                        BuildPaymentByOtherMethods(order, doc, root);

                        XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", "PaymentAgain.xslt"), doc);

                        failureReasons = order.OrderNote;
                        htmlBody = htmlBody.Replace("{PaymentAgainButton}", "<a class='btn btn-pay' data-fancybox data-src='#payment-again' href='javascript:;'>" + ProductResources.ButtonPaymentAgain + "</a>");
                    }
                }
            }


            htmlBody = htmlBody.Replace("{BillingEmail}", order.BillingEmail);
            htmlBody = htmlBody.Replace("{BillingFirstName}", order.BillingFirstName);
            htmlBody = htmlBody.Replace("{BillingPhone}", order.BillingPhone);
            htmlBody = htmlBody.Replace("{OrderCode}", orderCode);
            htmlBody = htmlBody.Replace("{PaymentStatus}", paymentStatus);
            htmlBody = htmlBody.Replace("{FailureReasons}", failureReasons);
            // Replace other order infos...
            htmlBody = htmlBody.Replace("{PaymentAgainButton}", string.Empty);
            if (GoogleTrackingHelper.Enable && GoogleTrackingHelper.EnableCompletedPage)
            {

                var doc = new XmlDocument
                {
                    XmlResolver = null
                };
                doc.LoadXml("<OrderDetail></OrderDetail>");
                var root = doc.DocumentElement;
                var timeOffset = SiteUtils.GetUserTimeOffset();
                var timeZone = SiteUtils.GetUserTimeZone();
                ProductHelper.BuildOrderXml(doc, root, order, timeZone, timeOffset);
                htmlBody += GoogleTrackingHelper.BuildGoogleCompleted(doc);
            }
            lblContent.Text = htmlBody;
        }

        private void BuildPaymentByOtherMethods(Order order, XmlDocument doc, XmlElement root)
        {
            var methods = PaymentMethod.GetByActive(siteSettings.SiteId, 1, WorkingCulture.LanguageId);
            foreach (var payment in methods)
            {
                if (ConfigHelper.GetBoolProperty("PaymentAgainIsOnline", false) && !PaymentHelper.IsOnlinePayment(payment.PaymentMethodId))
                    continue;
                XmlElement paymentItemXml = doc.CreateElement("Payment");
                root.AppendChild(paymentItemXml);

                XmlHelper.AddNode(doc, paymentItemXml, "Title", payment.Name);
                XmlHelper.AddNode(doc, paymentItemXml, "Description", payment.Description);
                XmlHelper.AddNode(doc, paymentItemXml, "Id", payment.PaymentMethodId.ToString());

                if (order != null && payment.PaymentMethodId == order.PaymentMethod)
                    XmlHelper.AddNode(doc, paymentItemXml, "IsActive", "true");
            }
        }

        protected virtual void LoadSettings()
        {
            if (this.ModuleConfiguration != null)
            {
                this.Title = this.ModuleConfiguration.ModuleTitle;
                this.Description = this.ModuleConfiguration.FeatureName;
            }
        }

        public override bool UserHasPermission()
        {
            return false;
        }
    }
}
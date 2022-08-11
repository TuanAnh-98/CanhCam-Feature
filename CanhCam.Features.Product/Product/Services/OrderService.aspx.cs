using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public class OrderService : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderService));

        private string method = string.Empty;
        private NameValueCollection postParams = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/json";
            Encoding encoding = new UTF8Encoding();
            Response.ContentEncoding = encoding;

            try
            {
                LoadParams();

                if (
                    method != "TrackingOrder"
                    && method != "OrderCheckDiscount"
                    )
                {
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = "No method found with the " + method
                    }));
                    return;
                }

                if (method == "TrackingOrder")
                {
                    Response.Write(TrackingOrder());
                    return;
                }
                else if (method == "OrderCheckDiscount")
                {
                    Response.Write(OrderCheckDiscount());
                    return;
                }

            }
            catch (Exception ex)
            {
                log.Error(ex);

                Response.Write(StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Failed to process from the server. Please refresh the page and try one more time."
                }));
            }
        }
        private string OrderCheckDiscount()
        {

            var orderCode = postParams.Get("orderCode");

            var paymentMethod = postParams.Get("paymentMethod");
            int methodId = -1;
            int.TryParse(paymentMethod, out methodId);
            if (string.IsNullOrEmpty(orderCode) && methodId <= 0)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Vui lòng kiểm tra lại thông tin"
                });
            Order order = Order.GetByCode(orderCode);
            if (order == null || order.OrderId == -1)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Không tìm thất đơn hàng trong hệ thống"
                });
            List<OrderItem> items = OrderItem.GetByOrder(order.OrderId);
            var products = Product.GetByOrder(order.SiteId, order.OrderId);
            decimal paymentDiscount = 0;
            PromotionsHelper.CheckOrderHaveDiscountPayment(order, items, products, methodId, ref paymentDiscount);
            return StringHelper.ToJsonString(new
            {
                success = true,
                paymentDiscount,
                paymentDiscountValue = ProductHelper.FormatPrice(paymentDiscount),
                PaymentDiscountMessage = string.Format(ProductResources.PaymentDiscountMessage, ProductHelper.FormatPrice(paymentDiscount))
            });

        }
        private string TrackingOrder()
        {
            var orderCode = postParams.Get("orderCode");

            if (string.IsNullOrEmpty(orderCode))
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Vui lòng điền mã đơn hàng của bạn"
                });
            Order objOrder = Order.GetByCode(orderCode);

            if (objOrder == null || objOrder.OrderId == -1)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Không tìm thấy đơn hàng của bạn"
                });
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<OrderDetail></OrderDetail>");
            XmlElement root = doc.DocumentElement;
            if (objOrder != null && objOrder.OrderId > 0)
            {
                var timeOffset = SiteUtils.GetUserTimeOffset();
                var timeZone = SiteUtils.GetUserTimeZone();

                ProductHelper.BuildOrderXml(doc, root, objOrder, timeZone, timeOffset);

                XmlHelper.AddNode(doc, root, "OrderTotal", ProductHelper.FormatPrice(objOrder.OrderTotal, true));
                XmlHelper.AddNode(doc, root, "OrderSubTotal", ProductHelper.FormatPrice(objOrder.OrderSubtotal, true));
                XmlHelper.AddNode(doc, root, "OrderDiscount", ProductHelper.FormatPrice(objOrder.OrderDiscount, true));
                XmlHelper.AddNode(doc, root, "PaymentFee", ProductHelper.FormatPrice(objOrder.OrderTax, true));
                XmlHelper.AddNode(doc, root, "ShippingFee", ProductHelper.FormatPrice(objOrder.OrderShipping, true));

                XmlHelper.AddNode(doc, root, "BillingAddress", objOrder.BillingAddress);
                XmlHelper.AddNode(doc, root, "BillingEmail", objOrder.BillingEmail);
                XmlHelper.AddNode(doc, root, "BillingFirstName", objOrder.BillingFirstName);
                XmlHelper.AddNode(doc, root, "BillingPhone", objOrder.BillingPhone);
                XmlHelper.AddNode(doc, root, "BillingMobile", objOrder.BillingMobile);
                XmlHelper.AddNode(doc, root, "BillingFax", objOrder.BillingFax);
                var country = new GeoZone(objOrder.BillingCountryGuid);
                if (country != null)
                    XmlHelper.AddNode(doc, root, "BillingCountry", country.Name);
                var province = new GeoZone(objOrder.BillingProvinceGuid);
                if (province != null)
                    XmlHelper.AddNode(doc, root, "BillingProvince", province.Name);

                var district = new GeoZone(objOrder.BillingDistrictGuid);
                if (district != null)
                    XmlHelper.AddNode(doc, root, "BillingDistrict", district.Name);

                //Shipping
                XmlHelper.AddNode(doc, root, "ShippingAddress", objOrder.ShippingAddress);
                XmlHelper.AddNode(doc, root, "ShippingEmail", objOrder.ShippingEmail);
                XmlHelper.AddNode(doc, root, "ShippingFirstName", objOrder.ShippingFirstName);
                XmlHelper.AddNode(doc, root, "ShippingPhone", objOrder.ShippingPhone);
                XmlHelper.AddNode(doc, root, "ShippingMobile", objOrder.ShippingMobile);
                var shippingCountry = new GeoZone(objOrder.ShippingCountryGuid);
                if (shippingCountry != null)
                    XmlHelper.AddNode(doc, root, "ShippingCountry", shippingCountry.Name);
                var shippingProvince = new GeoZone(objOrder.ShippingProvinceGuid);
                if (shippingProvince != null)
                    XmlHelper.AddNode(doc, root, "ShippingProvince", shippingProvince.Name);

                var shippingDistrict = new GeoZone(objOrder.ShippingDistrictGuid);
                if (shippingDistrict != null)
                    XmlHelper.AddNode(doc, root, "ShippingDistrict", shippingDistrict.Name);

                var payment = new PaymentMethod(objOrder.PaymentMethod);
                if (payment != null)
                    XmlHelper.AddNode(doc, root, "PaymentMethod", payment.Name);

                var shipping = new ShippingMethod(objOrder.ShippingMethod);
                if (shipping != null)
                    XmlHelper.AddNode(doc, root, "ShippingMethod", shipping.Name);

                XmlHelper.AddNode(doc, root, "InvoiceCompanyAddress", objOrder.InvoiceCompanyAddress);
                XmlHelper.AddNode(doc, root, "InvoiceCompanyName", objOrder.InvoiceCompanyName);
                XmlHelper.AddNode(doc, root, "InvoiceCompanyTaxCode", objOrder.InvoiceCompanyTaxCode);
            }
            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Product", "TrackingOrder.xslt"), doc);
            return StringHelper.ToJsonString(new
            {
                success = true,
                data = result
            });
        }

        private void LoadParams()
        {
            // don't accept get requests
            if (Request.HttpMethod != "POST") { return; }

            postParams = HttpUtility.ParseQueryString(Request.GetRequestBody());

            if (postParams.Get("method") != null)
            {
                method = postParams.Get("method");
            }
        }
    }
}
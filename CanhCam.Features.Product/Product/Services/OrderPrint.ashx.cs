using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;

namespace CanhCam.Web.ProductUI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class OrderPrint : IHttpHandler
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderPrint));

        public void ProcessRequest(HttpContext context)
        {
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;
            var results = string.Empty;

            try
            {
                var orderId = WebUtils.ParseInt32FromQueryString("id", -1);
                var languageId = WebUtils.ParseInt32FromQueryString("languageId", -1);
                string cultuname = "vi-VN";
                if (languageId == 1)
                    cultuname = "en-US";
                CultureInfo ci = new CultureInfo(cultuname);
                Thread.CurrentThread.CurrentCulture = ci;
                Thread.CurrentThread.CurrentUICulture = ci;
                var order = new Order(orderId);
                if (order != null
                        && order.OrderId > 0
                        )
                {
                    var objTemplate = new MessageTemplate("OrderPrinterTemplate", cultuname);
                    if (objTemplate == null || string.IsNullOrEmpty(objTemplate.ContentText))
                    {
                        results = "OrderPrinterTemplate not found";
                    }
                    else
                    {
                        var template = objTemplate.ContentText;
                        //var template = strTemplate;

                        var siteSettings = CacheHelper.GetCurrentSiteSettings();

                        template = template.Replace("{CompanyName}", siteSettings.CompanyName);
                        template = template.Replace("{ExtraName}", ConfigHelper.GetStringProperty("PrintTemplateExtraName", ""));
                        template = template.Replace("{Phone}", siteSettings.CompanyPhone);
                        template = template.Replace("{Address}", siteSettings.CompanyStreetAddress);
                        template = template.Replace("{OrderCode}", order.OrderCode);
                        template = template.Replace("{ApiOrderCode}", order.ApiOrderCode);

                        template = template.Replace("{CreatedDay}", order.CreatedUtc.ToString("dd"));
                        template = template.Replace("{CreatedMonth}", order.CreatedUtc.ToString("MM"));
                        template = template.Replace("{CreatedYear}", order.CreatedUtc.ToString("yyyy"));

                        template = template.Replace("{Customer.Name}", order.BillingFirstName);
                        template = template.Replace("{Customer.Address}", order.BillingAddress);
                        template = template.Replace("{Customer.Phone}", order.BillingPhone);
                        template = template.Replace("{Customer.Email}", order.BillingEmail);

                        template = template.Replace("{Customer.Name}", order.BillingFirstName);
                        template = template.Replace("{Customer.Address}", order.BillingAddress);
                        template = template.Replace("{Customer.Phone}", order.BillingPhone);
                        template = template.Replace("{Customer.Email}", order.BillingEmail);

                        GeoZone province = new GeoZone(order.BillingProvinceGuid);
                        if (province != null)
                            template = template.Replace("{Customer.Province}", province.Name);
                        else
                            template = template.Replace("{Customer.Province}", string.Empty);

                        GeoZone city = new GeoZone(order.BillingDistrictGuid);
                        if (city != null)
                            template = template.Replace("{Customer.City}", city.Name);
                        else
                            template = template.Replace("{Customer.City}", string.Empty);

                        if (string.IsNullOrEmpty(order.ShippingFirstName))
                            order.ShippingFirstName = order.BillingFirstName;

                        if (string.IsNullOrEmpty(order.ShippingAddress))
                            order.ShippingAddress = order.BillingAddress;

                        if (string.IsNullOrEmpty(order.ShippingPhone))
                            order.ShippingPhone = order.BillingPhone;

                        if (string.IsNullOrEmpty(order.ShippingEmail))
                            order.ShippingEmail = order.BillingEmail;

                        if (order.ShippingProvinceGuid == Guid.Empty)
                            order.ShippingProvinceGuid = order.BillingProvinceGuid;
                        if (order.ShippingDistrictGuid == Guid.Empty)
                            order.ShippingDistrictGuid = order.BillingDistrictGuid;

                        template = template.Replace("{ShippingAddress.Name}", order.ShippingFirstName);
                        template = template.Replace("{ShippingAddress.Address}", order.ShippingAddress);
                        template = template.Replace("{ShippingAddress.Phone}", order.ShippingPhone);
                        template = template.Replace("{ShippingAddress.Email}", order.ShippingEmail);

                        GeoZone sprovince = new GeoZone(order.ShippingProvinceGuid);
                        if (sprovince != null)
                            template = template.Replace("{ShippingAddress.Province}", sprovince.Name);
                        else
                            template = template.Replace("{ShippingAddress.Province}", string.Empty);

                        GeoZone scity = new GeoZone(order.ShippingDistrictGuid);
                        if (scity != null)
                            template = template.Replace("{ShippingAddress.City}", scity.Name);
                        else
                            template = template.Replace("{ShippingAddress.City}", string.Empty);

                        template = template.Replace("{SubTotal}", ProductHelper.FormatPrice(order.OrderSubtotal, true));
                        template = template.Replace("{DiscountAmount}", ProductHelper.FormatPrice(order.OrderDiscount, true));
                        template = template.Replace("{ShippingAmount}", ProductHelper.FormatPrice(order.OrderShipping, true));
                        template = template.Replace("{Total}", ProductHelper.FormatPrice(order.OrderTotal, true));

                        var repaymentDate = string.Empty;

                        template = template.Replace("{RepaymentDate}", repaymentDate);

                        List<OrderItem> lstOrderItems = OrderItem.GetByOrder(order.OrderId);
                        List<Product> lstProductOrders = Product.GetByOrder(order.SiteId, order.OrderId, languageId == 2 ? -1 : languageId);
                        string orderDetail = "<table style='width:100%;margin-top:10px;margin-bottom:10px;'>";
                        orderDetail += "<tbody>";
                        orderDetail += "<tr>";
                        orderDetail += "<td style='width:35px;border:solid 1px #000;text-align:center;padding:3px;'>STT</td>";
                        orderDetail += "<td style='width:80px;border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.ProductCodeLabel + "</td>";
                        orderDetail += "<td style='width:80px;border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.BarCodeLabel + "</td>";
                        orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.ProductNameLabel + "</td>";
                        orderDetail += "<td style='width:50px;border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.QuantityText + "</td>";
                        orderDetail += "<td style='width:100px;border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.CartUnitPriceLabel + "</td>";
                        orderDetail += "<td style='width:100px;border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.DiscountAmountLabel + "</td>";
                        orderDetail += "<td style='width:100px;border:solid 1px #000;text-align:center;padding:3px;'>" + ProductResources.CartItemTotalLabel + "</td>";
                        orderDetail += "</tr>";
                        int i = 1;
                        decimal totalQuantity = 0;
                        foreach (OrderItem item in lstOrderItems)
                        {
                            var product = ProductHelper.GetProductFromList(lstProductOrders, item.ProductId);
                            orderDetail += "<tr>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px;'>" + i.ToString() + "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>" +
                                product.Code +
                                "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>" +
                                product.BarCode +
                                "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:left;padding:3px;'>" +
                                //(item.ProductId > 0 && product != null ? product.Title : item.ProductName) +
                                product.Title +
                                "</td>";

                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>" +
                                //ProductHelper.FormatNumber(item.Quantity, true) +
                                item.Quantity +
                                "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'>" + ProductHelper.FormatPrice(item.Price, true) + "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'>" + ProductHelper.FormatPrice(item.DiscountAmount, true) + "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'>" + ProductHelper.FormatPrice(item.Price * item.Quantity - item.DiscountAmount, true) + "</td>";
                            orderDetail += "</tr>";
                            i += 1;
                            totalQuantity += item.Quantity;
                        }

                        if (!string.IsNullOrEmpty(order.ProductGifts))
                        {
                            var gifts = PromotionsHelper.ParseProductByGifts(order.ProductGifts);
                            foreach (var gift in gifts)
                            {
                                Product product = new Product(siteSettings.SiteId, gift.Key);
                                if (product != null)
                                {
                                    orderDetail += "<tr>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px;'>" + i.ToString() + "</td>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>" +
                                        product.Code +
                                        "</td>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>" +
                                        product.BarCode +
                                        "</td>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:left;padding:3px;'>Quà tặng - " +
                                        //(item.ProductId > 0 && product != null ? product.Title : item.ProductName) +
                                        product.Title +
                                        "</td>";

                                    orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>" +
                                       //ProductHelper.FormatNumber(item.Quantity, true) +
                                       gift.Value +
                                        "</td>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'>0</td>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'>0</td>";
                                    orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'>0</td>";
                                    orderDetail += "</tr>";
                                    i += 1;
                                    totalQuantity += gift.Value;
                                }
                            }
                        }


                        if (!string.IsNullOrEmpty(order.GiftDescription))
                        {
                            orderDetail += "<tr>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px;'>Quà tặng</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'></td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'></td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:left;padding:3px;'>" +
                                order.GiftDescription.Replace("<img ", "<img width='0'") +
                                "</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:center;padding:3px'>1</td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'></td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'></td>";
                            orderDetail += "<td style='border:solid 1px #000;text-align:right;padding:3px;'></td>";
                            orderDetail += "</tr>";
                        }

                        #region sumary total

                        orderDetail += "<tr style='font-weight:bold'>";
                        orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>" + ProductResources.OrderSubTotalLabel + "</td>";
                        //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                        orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.OrderSubtotal, true) + "</td>";
                        orderDetail += "</tr>";

                        orderDetail += "<tr style='font-weight:bold'>";
                        orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>" + ProductResources.OrderShippingFeeLabel + "</td>";
                        //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                        orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.OrderShipping, true) + "</td>";
                        orderDetail += "</tr>";

                        if (order.OrderCouponAmount > 0)
                        {
                            orderDetail += "<tr style='font-weight:bold'>";
                            orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>Coupon</td>";
                            //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                            orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.OrderCouponAmount, true) + "</td>";
                            orderDetail += "</tr>";
                        }

                        orderDetail += "<tr style='font-weight:bold'>";
                        orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>" + ProductResources.DiscountAmountLabel + "</td>";
                        //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                        orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.OrderDiscount, true) + "</td>";
                        orderDetail += "</tr>";

                        if (order.VoucherAmount > 0)
                        {
                            orderDetail += "<tr style='font-weight:bold'>";
                            orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>Voucher</td>";
                            //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                            orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.VoucherAmount, true) + "</td>";
                            orderDetail += "</tr>";
                        }

                        if (order.OrderTax > 0)
                        {
                            orderDetail += "<tr style='font-weight:bold'>";
                            orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>VAT</td>";
                            //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                            orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.OrderTax, true) + "</td>";
                            orderDetail += "</tr>";
                        }

                        orderDetail += "<tr style='font-weight:bold'>";
                        orderDetail += "<td colspan='7' style='border:solid 1px #000;text-align: center; padding:3px'>" + ProductResources.CartTotalLabel + "</td>";
                        //orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'></td>";
                        orderDetail += "<td style='border:solid 1px #000; text-align:right;padding:3px'>" + ProductHelper.FormatPrice(order.OrderTotal, true) + "</td>";
                        orderDetail += "</tr>";

                        #endregion sumary total

                        orderDetail += "</tbody>";
                        orderDetail += "</table>";

                        template = template.Replace("{OrderDetail}", orderDetail);

                        results = template;
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            context.Response.Write(results);
            context.Response.End();
        }

        private string GetGift(OrderItem item)
        {
            var attributesXml = item.AttributesXml;
            var results = string.Empty;

            int productId = item.ProductId;
            List<ProductProperty> productProperties = new List<ProductProperty>();
            List<CustomField> customFields = new List<CustomField>();
            if (ProductConfiguration.EnableShoppingCartAttributes)
            {
                foreach (var a in customFields.Where(c => (c.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0))
                {
                    productProperties.ForEach(property =>
                    {
                        if (property.ProductId == productId
                            && property.CustomFieldId == a.CustomFieldId)
                            results += string.Format("<div><span>{0}</span>: {1}</div>", a.Name, property.OptionName);
                    });
                }
            }
            else if (!string.IsNullOrEmpty(attributesXml))
            {
                var attributes = ProductAttributeParser.ParseProductAttributeMappings(customFields, attributesXml);
                if (attributes.Count > 0)
                {
                    foreach (var a in attributes)
                    {
                        var values = ProductAttributeParser.ParseValues(attributesXml, a.CustomFieldId);
                        if (values.Count > 0)
                        {
                            productProperties.ForEach(property =>
                            {
                                if (property.ProductId == productId
                                    && property.CustomFieldId == a.CustomFieldId
                                    && values.Contains(property.CustomFieldOptionId))
                                    results += string.Format("<div><span>{0}</span>: {1}</div>", a.Name, property.OptionName);
                            });
                        }
                    }
                }
            }

            if (!string.IsNullOrEmpty(item.AttributeDescription))
                results += item.AttributeDescription;
            if (results.Length > 0)
                results = string.Format("<br/><div class='attributes'>{0}</div>", results);
            return results.Replace("<img ", "<img width='0'");
        }

        public bool IsReusable => false;
    }
}
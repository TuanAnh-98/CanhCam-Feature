using System;
using log4net;
using CanhCam.Web.Framework;
using CanhCam.Business;
using Resources;

namespace CanhCam.Web.ProductUI
{
    public class NganLuongHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NganLuongHelper));
        private static string MERCHANT_ID = ConfigHelper.GetStringProperty("Alepay.NganLuongMERCHANT_ID", "");
        private static string MERCHANT_PASSWORD = ConfigHelper.GetStringProperty("Alepay.NganLuongMERCHANT_PASSWORD", "");
        private static string RECEIVER_EMAIL = ConfigHelper.GetStringProperty("Alepay.NganLuongRECEIVER_EMAIL", "");


        public static string SendPaymentNganLuong(Order order)
        {
            if (order == null || order.OrderId <= 0)
                return string.Empty;

            if (order.PaymentMethod != (int)AlepayHelper.NganLuongATMPaymentMethod && order.PaymentMethod != (int)AlepayHelper.NganLuongIBPaymentMethod)
                return string.Empty;

            var payment_method = "ATM_ONLINE"; //NL, VISA, IB_ONLINE
            //if (order.PaymentMethod == (int)OnlinePaymentMethod.NganLuongIB)
            //    payment_method = "IB_ONLINE";

            var info = new RequestInfo
            {
                Merchant_id = MERCHANT_ID,
                Merchant_password = MERCHANT_PASSWORD,
                Receiver_email = RECEIVER_EMAIL,

                cur_code = "vnd",
                bank_code = order.PaymentAPICode,

                //if (!string.IsNullOrEmpty(order.OrderCode))
                //    info.Order_code = order.OrderCode;
                //else
                //    info.Order_code = order.OrderId.ToString();
                Order_code = order.OrderId.ToString()
            };

            var shipping = order.OrderShipping;
            var amount = order.OrderSubtotal;
            //if (order.PreOrder > 0)
            //{ 
            //    amount = order.PreOrder;
            //    shipping = 0;
            //}
            info.Total_amount = Convert.ToDouble(amount).ToString();
            info.fee_shipping = Convert.ToDouble(shipping).ToString();

            info.Discount_amount = Convert.ToDouble(order.OrderDiscount).ToString();
            info.tax_amount = Convert.ToDouble(order.OrderTax).ToString();
            info.order_description = "";
            info.return_url = SiteUtils.GetNavigationSiteRoot() + "/product/nganluong/result.aspx?id=" + order.OrderId.ToString();
            info.cancel_url = SiteUtils.GetNavigationSiteRoot() + "/product/nganluong/result.aspx?id=" + order.OrderId.ToString() + "&cancel=1";
            info.Buyer_fullname = order.BillingFirstName;
            info.Buyer_email = order.BillingEmail;
            info.Buyer_mobile = order.BillingPhone;

            var objNLChecout = new NganLuongApiV3();
            var result = objNLChecout.GetUrlCheckout(info, payment_method);

            if (result.Error_code == "00")
                return result.Checkout_url;

            log.Error(result.Description);

            return string.Empty;
        }

        public static Order ProcessCallbackData()
        {
            try
            {
                var token = WebUtils.ParseStringFromQueryString("token", string.Empty);
                var info = new RequestCheckOrder
                {
                    Merchant_id = MERCHANT_ID,
                    Merchant_password = MERCHANT_PASSWORD,
                    Token = token
                };

                var objNLChecout = new NganLuongApiV3();
                var result = objNLChecout.GetTransactionDetail(info);

                Order order = null;
                if (result.order_code.Length > 0)
                    order = new Order(Convert.ToInt32(result.order_code));
                if (order == null || order.OrderId < 0 || order.IsDeleted)
                    order = null;

                if (order != null)
                {
                    var message = string.Empty;
                    int oldStatus = order.PaymentStatus;
                    if (result.transactionStatus == "00" || result.transactionStatus == "01")
                    {
                        order.PaymentStatus = (int)OrderPaymentStatus.Successful;
                        message = "Giao dịch thanh toán thành công!";
                    }
                    else
                    {
                        order.PaymentStatus = (int)OrderPaymentStatus.NotSuccessful;
                        message = "Giao dịch thanh toán thất bại!";
                    }

                    order.Save();

                    order.OrderNote = message;
                    new OrderLog()
                    {
                        OrderId = order.OrderId,
                        Comment = OrderHelper.GetPaymentStatusResources(oldStatus) + " => " + OrderHelper.GetPaymentStatusResources(order.PaymentStatus),
                        UserEmail = "NGANLUONG",
                        CreatedOn = DateTime.Now,
                        TypeName = ProductResources.SysnOrderPayment
                    }.Save();
                }

                return order;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return null;
        }

    }
}
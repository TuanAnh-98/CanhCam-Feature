using System;
using log4net;
using CanhCam.Web.Framework;
using CanhCam.Business;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace CanhCam.Web.ProductUI
{
    public class AlepayHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AlepayHelper));

        public static int AlepayPaymentMethod => ConfigHelper.GetIntProperty("Alepay.PaymentID", 0);
        public static int NganLuongATMPaymentMethod => ConfigHelper.GetIntProperty("Alepay.NganLuongATM.PaymentID", 0);
        public static int NganLuongIBPaymentMethod => ConfigHelper.GetIntProperty("Alepay.NganLuongIB.PaymentID", 0);
        public static int CompletedZoneId
        {
            get { return ConfigHelper.GetIntProperty("Alepay.CompletedZoneId", 68); }
        }

        public static string SendPaymentAlepay(Order order)
        {
            if (order == null || order.OrderId <= 0)
                return string.Empty;
            if (order.PaymentMethod == (int)AlepayPaymentMethod
                || order.PaymentMethod == (int)NganLuongATMPaymentMethod
                || order.PaymentMethod == (int)NganLuongIBPaymentMethod)
            {
                string storeCode = "";
                Store store = StoreCacheHelper.GetStoreById(order.StoreId);
                if (store != null)
                    storeCode = store.Code;
                PaymentMethod payment = new PaymentMethod(order.PaymentMethod);
                if (payment == null
                    && payment.PaymentProvider != (int)PaymentMethodProvider.AleyPayType1
                    && payment.PaymentProvider != (int)PaymentMethodProvider.AleyPayType2
                    && payment.PaymentProvider != (int)PaymentMethodProvider.AleyPayType3
                    && payment.PaymentProvider != (int)PaymentMethodProvider.AleyPayType4
                    && payment.PaymentProvider != (int)PaymentMethodProvider.AleyPayType5
                    )//&& order.PaymentMethod != (int)OnlinePaymentMethod.Agribank)
                    return string.Empty;
                // Service require TLS 1.2 (not support in .NET Framework 4.0)
                // http://stackoverflow.com/questions/33761919/tls-1-2-in-net-framework-4-0
                ServicePointManager.ServerCertificateValidationCallback =
                   delegate (
                       object s,
                       X509Certificate certificate,
                       X509Chain chain,
                       SslPolicyErrors sslPolicyErrors
                   )
                   {
                       return true;
                   };
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                try
                {
                    var lstOrderItems = OrderItem.GetByOrder(order.OrderId);
                    var totalItem = 0;
                    lstOrderItems.ForEach(s => { totalItem += s.Quantity; });

                    var buyerCity = string.Empty;
                    var city = new GeoZone(order.BillingProvinceGuid);
                    if (city != null && city.ItemID > 0)
                        buyerCity = city.Name;

                    var amount = Convert.ToDouble(order.OrderTotal);
                    //if (order.PreOrder > 0)
                    //    amount = Convert.ToDouble(order.PreOrder);

                    var info = new AlepayRequestInfo
                    {
                        orderCode = order.OrderId.ToString(),
                        amount = amount,
                        currency = "VND",
                        orderDescription = "Payment for #" + order.OrderCode,
                        totalItem = totalItem,
                        returnUrl = SiteUtils.GetNavigationSiteRoot() + "/product/nganluong/alepaycallback.aspx?id=" + order.OrderId.ToString(),
                        cancelUrl = SiteUtils.GetNavigationSiteRoot() + "/product/nganluong/alepaycallback.aspx?id=" + order.OrderId.ToString() + "&cancel=1",
                        buyerName = order.BillingFirstName,
                        buyerEmail = order.BillingEmail,
                        buyerPhone = order.BillingPhone,
                        buyerAddress = order.BillingAddress,
                        buyerCity = buyerCity,
                        buyerCountry = "Việt Nam",
                        paymentHours = "1",
                        alowFomestic = true
                    };
                    //version 2 add checkoutType
                    SetAleyCheckoutType(payment, info);

                    var objNLChecout = new AlepayApi();
                    var result = objNLChecout.GetUrlCheckout(info, storeCode);

                    if (result.errorCode == "000")
                    {
                        var dataDecrypted = Crypter.Decrypt<AlepayResponseDecrypted>(result.data, storeCode);
                        if (dataDecrypted != null)
                            return dataDecrypted.checkoutUrl;
                    }

                    log.Error(result.errorDescription);
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }
            }
            return string.Empty;
        }
        private static void SetAleyCheckoutType(PaymentMethod payment, AlepayRequestInfo alepayInfo)
        {
            switch (payment.PaymentProvider)
            {
                case ((int)PaymentMethodProvider.AleyPayType1):
                    alepayInfo.checkoutType = 0;
                    alepayInfo.allowDomestic = false;
                    break;
                case ((int)PaymentMethodProvider.AleyPayType2):
                    alepayInfo.checkoutType = 1;
                    alepayInfo.allowDomestic = false;
                    break;
                case ((int)PaymentMethodProvider.AleyPayType3):
                    alepayInfo.checkoutType = 2;
                    alepayInfo.allowDomestic = false;
                    break;
                case ((int)PaymentMethodProvider.AleyPayType4):
                    alepayInfo.checkoutType = 3;
                    alepayInfo.allowDomestic = true;
                    break;
                case ((int)PaymentMethodProvider.AleyPayType5):
                    alepayInfo.checkoutType = 4;
                    alepayInfo.allowDomestic = true;
                    break;
                default:
                    alepayInfo.checkoutType = 2;
                    alepayInfo.allowDomestic = false;
                    break;
            }
        }
        public static Order ProcessCallbackData()
        {
            try
            {
                var id = WebUtils.ParseInt32FromQueryString("id", -1);
                var checksum = WebUtils.ParseStringFromQueryString("checksum", string.Empty);

                var order = new Order(id);
                if (order == null || order.OrderId < 0 || order.IsDeleted)
                    order = null;

                if (order != null)
                {
                    var message = string.Empty;
                    var data = WebUtils.ParseStringFromQueryString("data", string.Empty);
                    data = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(data));

                    string storeCode = "";
                    Store store = StoreCacheHelper.GetStoreById(order.StoreId);
                    if (store != null)
                        storeCode = store.Code;
                    var dataDecrypted = Crypter.Decrypt<AlepayResponseCallback>(data, storeCode);
                    if (dataDecrypted != null && (dataDecrypted.errorCode == "155" || dataDecrypted.errorCode == "150" || dataDecrypted.errorCode == "000"))
                    {
                        order.PaymentStatus = (int)OrderPaymentStatus.Successful;
                        order.ShippingMobile = dataDecrypted.data;

                        if (!string.IsNullOrEmpty(order.OrderNote))
                            order.OrderNote += "\n";

                        if (dataDecrypted.errorCode == "155")
                        {
                            message = "Giao Dịch Đã Thành Công! Bạn Nên Xác Nhận Phía Ngân Hàng Để Hoàn Thành Nốt Thủ Tục Giao Dịch";
                            order.OrderNote += dataDecrypted.errorCode + ": Đang chờ duyệt";
                        }
                        else if (dataDecrypted.errorCode == "150")
                        {
                            message = "Quý khách đã thanh toán thành công, tuy nhiên để xác thực lại thông tin thẻ quý khách vui lòng check email do phía cổng thanh toán gửi";
                            order.OrderNote += dataDecrypted.errorCode + ": Thẻ đang bị review";
                        }
                    }
                    else
                    {
                        order.PaymentStatus = (int)OrderPaymentStatus.NotSuccessful;
                        message = "Giao Dịch Thất Bại! Mã lỗi: " + dataDecrypted.errorCode;
                    }

                    order.Save();

                    order.OrderNote = message;
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
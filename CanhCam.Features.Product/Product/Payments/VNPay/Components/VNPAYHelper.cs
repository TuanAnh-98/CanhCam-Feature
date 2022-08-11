using CanhCam.Business;
using CanhCam.Web.Framework;

namespace CanhCam.Web.ProductUI
{
    public static class VNPAYHelper
    {
        public static int CompletedZoneId = ConfigHelper.GetIntProperty("VNPAY.CompletedZoneId", 0);
        public static int PaymentID = ConfigHelper.GetIntProperty("VNPAY.PaymentID", 0);
        public static string SecureHashType = ConfigHelper.GetStringProperty("VNPAY.SecureHashType", "MD5");
        public static string Version = ConfigHelper.GetStringProperty("VNPAY.Version", "2.0.0");
        public static string CurrencyCode = ConfigHelper.GetStringProperty("VNPAY.CurrencyCode", "VND");
        public static string Location = ConfigHelper.GetStringProperty("VNPAY.Location", "vn");
        public static string HashSecret = ConfigHelper.GetStringProperty("VNPAY.HashSecret", "LHUZCQJWGCNLSBHGZJHXGONMYJPUUZKJ");
        public static string TmnCode = ConfigHelper.GetStringProperty("VNPAY.TmnCode", "LINDA001");
        public static string PayUrl = ConfigHelper.GetStringProperty("VNPAY.PayUrl", "http://sandbox.vnpayment.vn/paymentv2/vpcpay.html");
        public static string ReturnUrl = ConfigHelper.GetStringProperty("VNPAY.ReturnUrl", "/Product/Payments/VNPay/Callback.aspx");
        public static bool EnableConfigByStore => ConfigHelper.GetBoolProperty("VNPAY.EnableConfigByStore", false);

        public static void LoadConfigByStoreCode(int storeId)
        {
            if (storeId <= 0) return;
            CompletedZoneId = ConfigHelper.GetIntProperty("VNPAY.CompletedZoneId" + "_" + storeId, CompletedZoneId);
            PaymentID = ConfigHelper.GetIntProperty("VNPAY.PaymentID" + "_" + storeId, PaymentID);
            SecureHashType = ConfigHelper.GetStringProperty("VNPAY.SecureHashType" + "_" + storeId, SecureHashType);
            Version = ConfigHelper.GetStringProperty("VNPAY.Version" + "_" + storeId, Version);
            CurrencyCode = ConfigHelper.GetStringProperty("VNPAY.CurrencyCode" + "_" + storeId, CurrencyCode);
            Location = ConfigHelper.GetStringProperty("VNPAY.Location" + "_" + storeId, Location);
            HashSecret = ConfigHelper.GetStringProperty("VNPAY.HashSecret" + "_" + storeId, HashSecret);
            TmnCode = ConfigHelper.GetStringProperty("VNPAY.TmnCode" + "_" + storeId, TmnCode);
            PayUrl = ConfigHelper.GetStringProperty("VNPAY.PayUrl" + "_" + storeId, PayUrl);
            ReturnUrl = ConfigHelper.GetStringProperty("VNPAY.ReturnUrl" + "_" + storeId, ReturnUrl);
        }

        //Build URL for VNPAY
        public static string BuildUrlPay(Order order)
        {
            VnPayLibrary vnpay = new VnPayLibrary();
            if (EnableConfigByStore)
                LoadConfigByStoreCode(order.StoreId);
            vnpay.AddRequestData("vnp_Version", Version);
            vnpay.AddRequestData("vnp_Command", "pay");
            if (!string.IsNullOrEmpty(order.BankCode))
                vnpay.AddRequestData("vnp_BankCode", order.BankCode);

            vnpay.AddRequestData("vnp_TmnCode", TmnCode);
            vnpay.AddRequestData("vnp_Amount", (order.OrderTotal * 100).ToString("#"));

            vnpay.AddRequestData("vnp_CreateDate", order.CreatedUtc.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", CurrencyCode);
            vnpay.AddRequestData("vnp_IpAddr", Utils.GetIpAddress());

            vnpay.AddRequestData("vnp_Locale", Location);
            vnpay.AddRequestData("vnp_OrderInfo", "Pay Order #" + order.OrderCode);
            //vnpay.AddRequestData("vnp_OrderType", orderCategory.SelectedItem.Value); //default value: other
            vnpay.AddRequestData("vnp_ReturnUrl", SiteUtils.GetNavigationSiteRoot() + ReturnUrl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString());
            order.PaymentStatus = (int)OrderPaymentStatus.Pending;
            order.Save();
            return vnpay.CreateRequestUrl(PayUrl, HashSecret);
        }
    }
}
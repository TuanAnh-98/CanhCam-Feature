using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using RestSharp;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public static class PayooHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayooHelper));
        public static int PaymentMethodId = ConfigHelper.GetIntProperty("Payoo.PaymentMethodId", 0);
        public static int CompletedZoneId = ConfigHelper.GetIntProperty("Payoo.CompletedZoneId", 0);

        public static string ProviderUrl = ConfigHelper.GetStringProperty("Payoo.ProviderUrl", "");
        public static int ShopID = ConfigHelper.GetIntProperty("Payoo.ShopID", 0);
        public static string ShopTitle = ConfigHelper.GetStringProperty("Payoo.ShopTitle", "");
        public static string ShopDomain = ConfigHelper.GetStringProperty("Payoo.ShopDomain", "");
        public static string BusinessUsername = ConfigHelper.GetStringProperty("Payoo.BusinessUsername", "");
        public static string ShopBackUrl = ConfigHelper.GetStringProperty("Payoo.ShopBackUrl", "");
        public static string NotifyUrl = ConfigHelper.GetStringProperty("Payoo.NotifyUrl", "");
        public static string ChecksumKey = ConfigHelper.GetStringProperty("Payoo.ChecksumKey", "");

        #region Helper

        public static string GenerateCheckSum(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (SHA512 hash = System.Security.Cryptography.SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte
                StringBuilder hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (byte b in hashedInputBytes)
                {
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                }
                return hashedInputStringBuilder.ToString();
            }
        }

        public static bool VerifyCheckSum(string Data, string CheckSum)
        {
            try
            {
                if (string.IsNullOrEmpty(Data) ||
                   string.IsNullOrEmpty(CheckSum))
                {
                    return false;
                }
                return CheckSum.Equals(GenerateCheckSum(Data), StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static string EncryptSHA512(string input)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (SHA512 hash = System.Security.Cryptography.SHA512.Create())
            {
                byte[] hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte
                StringBuilder hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (byte b in hashedInputBytes)
                {
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                }
                return hashedInputStringBuilder.ToString();
            }
        }

        #endregion Helper

        public static string GetPaymentUrlIfNeeded(Order order)
        {
            if (order == null || order.OrderId <= 0)
                return string.Empty;

            try
            {
                PayooOrder model = new PayooOrder
                {
                    Session = order.OrderId.ToString(),
                    BusinessUsername = BusinessUsername,
                    OrderCashAmount = (long)order.OrderTotal,
                    OrderNo = order.OrderCode,
                    ShippingDays = 1,
                    ShopBackUrl = ShopBackUrl,
                    ShopDomain = ShopDomain,
                    ShopID = ShopID,
                    ShopTitle = ShopTitle,
                    StartShippingDate = order.CreatedUtc.ToString("dd/MM/yyyy"),
                    NotifyUrl = NotifyUrl,
                    ValidityTime = order.CreatedUtc.AddDays(1).ToString("yyyyMMddHHmmss"),
                    // thong tin khach hang
                    CustomerName = order.BillingFirstName,
                    CustomerPhone = order.BillingPhone,
                    CustomerEmail = order.BillingEmail,
                    CustomerAddress = order.BillingAddress,
                    CustomerCity = "", // ninh thuan
                    OrderDescription = "Payment for order #" + order.OrderCode
                };
                string checksum = string.Empty;

                string xml = PaymentXMLFactory.GetPaymentXML(model);
                log.Info("Payment XML " + xml);
                checksum = EncryptSHA512(ChecksumKey + xml);
                var client = new RestClient(ProviderUrl);
                var request = new RestRequest(Method.POST);
                request.AddParameter("data", xml);
                request.AddParameter("checksum", checksum);
                request.AddParameter("refer", ShopDomain);
                request.AddParameter("method", "cc-payment");
                //request.AddParameter("restrict", "1");
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JavaScriptSerializer objJson = new JavaScriptSerializer();
                    CreateOrderResponse objResponse = objJson.Deserialize<CreateOrderResponse>(response.Content);
                    if (objResponse.result.Equals("success", StringComparison.OrdinalIgnoreCase))
                    {
                        return objResponse.order.payment_url;
                    }
                }
                else
                    log.Error("Send Payoo Error :" + response.ErrorException);
            }
            catch (Exception ex)
            {
                log.Error("Send Order to Payoo Error :" + ex.Message);
            }
            return string.Empty;
        }
    }

    public class CreateOrderResponse
    {
        public string result;
        public OrderResponse order;
    }

    public class OrderResponse
    {
        public string payment_url;
    }
}
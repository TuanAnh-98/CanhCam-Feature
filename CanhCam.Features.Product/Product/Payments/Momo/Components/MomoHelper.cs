using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public static class MomoHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MomoHelper));
        public static int MoMoPaymentMethodId => ConfigHelper.GetIntProperty("MOMO.PaymentMethodId", 0);
        public static string partnerCode => ConfigHelper.GetStringProperty("MOMO.PartnerCode", "MOMOYI0F20190924");
        public static string accessKey => ConfigHelper.GetStringProperty("MOMO.AccessKey", "Ck4NhZGRd1jTKZoT");
        public static string secretKey => ConfigHelper.GetStringProperty("MOMO.SecretKey", "ANlOKMEnvj6Ty4dZ7Lo2jajLu3sLoH4w");
        public static string endPoint => ConfigHelper.GetStringProperty("MOMO.EndPoint", "https://test-payment.momo.vn/gw_payment/transactionProcessor");
        public static string endPointRefun => ConfigHelper.GetStringProperty("MOMO.EndPointRefun", "https://payment.momo.vn/pay/refund");
        public static string notifyUrl => ConfigHelper.GetStringProperty("MOMO.NotifyUrl", "");//CallBack http://preview12689.canhcam.asia/Product/Payments/MoMo/MoMoIPN.aspx
        public static string returnUrl => ConfigHelper.GetStringProperty("MOMO.ReturnUrl", "");//Redirect  http://preview12689.canhcam.asia/Product/Payments/MoMo/MoMoRedirect.aspx
        public static int CompletedZoneId => ConfigHelper.GetIntProperty("MOMO.CompletedZoneId", -1);
        public static int FailedZoneId => ConfigHelper.GetIntProperty("MOMO.FailedZoneId", -1);

        #region Helper

        public static string TransformString(string mess)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(mess);
            return Encoding.UTF8.GetString(bytes);
        }

        public static string SignSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                string hex = BitConverter.ToString(hashmessage);
                hex = hex.Replace("-", "").ToLower();
                return hex;
            }
        }

        public static string GeneratorExtraData(Order order, List<ShoppingCartItem> cartItems)
        {
            if (ConfigHelper.GetBoolProperty("MoMo.ExtraDataKeyAndValueOnly", true))
                return $"full_name={order.BillingFirstName};email={order.BillingEmail}phone={order.BillingPhone};merchantId={order.ShippingAddress}";

            List<MoMoRequestOrderItem> lstOrderItems = new List<MoMoRequestOrderItem>();
            foreach (var item in cartItems)
            {
                lstOrderItems.Add(
                   new MoMoRequestOrderItem()
                   {
                       currency = "VNĐ",
                       name = item.ProductTitle,
                       quantity = item.Quantity.ToString(),
                       sku = item.ProductId.ToString()
                   });
            }
            var purchare = new MoMoRequestPurchaseUnits() { items = lstOrderItems };
            return StringHelper.ToJsonString((new
            {
                full_name = order.BillingFirstName,
                email = order.BillingEmail,
                phone = order.BillingPhone,
                merchantId = order.ShippingAddress,
                purchase_units = purchare
            }));
        }

        #endregion Helper

        #region APIs

        public static string GetPaymentUrlIfNeeded(Order order, List<ShoppingCartItem> cartItems)
        {
            if (order == null || order.OrderId <= 0 || cartItems == null)
                return string.Empty;

            string json = MomoHelper.GeneratorJsonOrderData(order, cartItems, partnerCode, accessKey, secretKey);
            string resultJson = MomoHelper.CreateOrder(MomoHelper.endPoint, json);
            var js = new JavaScriptSerializer();
            var jsondata = js.Deserialize<MomoResponsePayment>(resultJson);
            if (jsondata.errorCode == 0)
                return jsondata.payUrl;
            log.Error("MoMo Can't Payment :" + resultJson);
            return string.Empty;
        }

        public static string GeneratorJsonOrderData(Order order, List<ShoppingCartItem> cartItems, string MoMoPartnerCode, string MoMoAccessKey, string MoMoSecretKey)
        {
            var amount = (long)Convert.ToDouble(order.OrderTotal);

            string requestId = Guid.NewGuid().ToString();
            // string orderInfo = string.Format(ProductHelper.RemoveUnicode(ProductResources.DescriptionZalo), TransformString(ProductHelper.RemoveUnicode(order.ShippingAddress)));
            string strextraData = GeneratorExtraData(order, cartItems);
            string extraData = strextraData;
            if (ConfigHelper.GetBoolProperty("MoMo.EnableConvertExtraDataToBase64", false))
                if (!string.IsNullOrEmpty(strextraData))
                    extraData = Convert.ToBase64String(Encoding.UTF8.GetBytes(strextraData));//MoMo yêu cầu Convert Json Order extra Data to Base64
            string orderId = Guid.NewGuid().ToString();
            string orderInfo = order.OrderId.ToString();
            string rawHash = "partnerCode=" +
              MoMoPartnerCode + "&accessKey=" +
              MoMoAccessKey + "&requestId=" +
              requestId + "&amount=" +
              amount + "&orderId=" +
              orderId + "&orderInfo=" +
              orderInfo + "&returnUrl=" +
              returnUrl + "&notifyUrl=" +
              notifyUrl + "&extraData=" +
              extraData;

            string signature = SignSHA256(rawHash, MoMoSecretKey);
            string json = StringHelper.ToJsonString((new
            {
                accessKey = MoMoAccessKey,
                partnerCode = MoMoPartnerCode,
                requestType = "captureMoMoWallet",//Sử dụng API captureMoMoWallet để yêu cầu thanh toán bằng Ví MoMo
                notifyUrl,
                returnUrl,
                orderId,
                amount = amount.ToString(),
                orderInfo,
                requestId,
                extraData,
                signature
            }));
            return json;
        }

        public static string CreateOrder(Order order, List<ShoppingCartItem> cartItems, string MoMoPartnerCode, string MoMoAccessKey, string MoMoSecretKey)
        {
            var postJsonString = GeneratorJsonOrderData(order, cartItems, MoMoPartnerCode, MoMoAccessKey, MoMoSecretKey);
            return CreateOrder(endPoint, postJsonString);
        }

        public static string CreateOrder(string endpoint, string postJsonString)
        {
            try
            {
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

                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endpoint);

                var postData = postJsonString;
                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }

                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }

        public static string GeneraorJsonRefun(string partnerRefId, string momoTransId, long amount, string description, string MoMoPartnerCode, string MoMoAccessKey)
        {
            string json = StringHelper.ToJsonString((new
            {
                partnerCode = MoMoPartnerCode,
                partnerRefId,
                momoTransId,//Sử dụng API captureMoMoWallet để yêu cầu thanh toán bằng Ví MoMo
                amount,
                description
            }));
            byte[] data = Encoding.UTF8.GetBytes(json);
            string result = null;
            using (var rsa = new RSACryptoServiceProvider(4096)) // or 4096, base on key length
            {
                try
                {
                    // Client encrypting data with public key issued by server
                    // "publicKey" must be XML format, use https://superdry.apphb.com/tools/online-rsa-key-converter
                    // to convert from PEM to XML before hash
                    rsa.FromXmlString(MoMoAccessKey);
                    var encryptedData = rsa.Encrypt(data, false);
                    var base64Encrypted = Convert.ToBase64String(encryptedData);
                    result = base64Encrypted;
                }
                finally
                {
                    rsa.PersistKeyInCsp = false;
                }
            }
            string reuqestJson = StringHelper.ToJsonString((new
            {
                partnerCode = MoMoPartnerCode,
                requestId = Guid.NewGuid(),
                hash = result,//Sử dụng API captureMoMoWallet để yêu cầu thanh toán bằng Ví MoMo
                version = 2.0
            }));
            return reuqestJson;
        }

        public static string RefunOrder(string postJsonString)
        {
            try
            {
                // Service require TLS 1.2 (not support in .NET Framework 4.0)
                // http://stackoverflow.com/questions/33761919/tls-1-2-in-net-framework-4-0
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

                HttpWebRequest httpWReq = (HttpWebRequest)WebRequest.Create(endPointRefun);

                var postData = postJsonString;
                var data = Encoding.UTF8.GetBytes(postData);

                httpWReq.ProtocolVersion = HttpVersion.Version11;
                httpWReq.Method = "POST";
                httpWReq.ContentType = "application/json";

                httpWReq.ContentLength = data.Length;
                httpWReq.ReadWriteTimeout = 30000;
                httpWReq.Timeout = 15000;
                Stream stream = httpWReq.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();

                HttpWebResponse response = (HttpWebResponse)httpWReq.GetResponse();

                string jsonresponse = "";

                using (var reader = new StreamReader(response.GetResponseStream()))
                {
                    string temp = null;
                    while ((temp = reader.ReadLine()) != null)
                    {
                        jsonresponse += temp;
                    }
                }

                //todo parse it
                return jsonresponse;
                //return new MomoResponse(mtid, jsonresponse);
            }
            catch (WebException e)
            {
                return e.Message;
            }
        }

        #endregion APIs
    }
}
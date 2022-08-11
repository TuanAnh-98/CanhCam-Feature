using CanhCam.Web.Framework;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public class AlepayApi
    {
        public static bool ENABLE_KEY_FORSTORE = ConfigHelper.GetBoolProperty("Alepay.ENABLE_KEY_FORSTORE", false);
        private static string BASE_URL = ConfigHelper.GetStringProperty("Alepay.BASE_URL", "https://alepay-sandbox.nganluong.vn");
        public static readonly string CONFIG_TOKEN_KEY = "Alepay.TOKEN";
        private static string TOKEN = ConfigHelper.GetStringProperty(CONFIG_TOKEN_KEY, "kpJNeuqYuQNx1CbJcZfIBDhyY5JAIt");
        public static readonly string CONFIG_CHECKSUM_KEY = "Alepay.CHECKSUM_KEY";
        private static string CHECKSUM_KEY = ConfigHelper.GetStringProperty(CONFIG_CHECKSUM_KEY, "i6G9ZFf07mnFK4Yyxu6JhZLrrt3JIH");
        public static readonly string CONFIG_ENCRYPT_KEY = "Alepay.ENCRYPT_KEY";
        public static string ENCRYPT_KEY = ConfigHelper.GetStringProperty(CONFIG_ENCRYPT_KEY, "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQDGyDRSmdCZA31f8Wud01w8ySj2MWT8CwezYZo0kFnfg+LrMqzft3zLOdkiI/zQajWBSneYU7lNwC+btJNLC4myNflyViJoI3582CI4xM07sM9oJ1HTBf++4Lryys13e+ZO2IJyT/gWE4+04GGfnv/1ihdHM22e8KLrsdHCduOFEwIDAQAB");

        public AlepayResponseInfo GetUrlCheckout(AlepayRequestInfo requestContent, string storeCode = "")
        {
            var response = SendRequestToAlepay(requestContent, "/checkout/v1/request-order", storeCode);
            //var response = SendRequestToAlepay(requestContent, "");
            var js = new JavaScriptSerializer();
            return js.Deserialize<AlepayResponseInfo>(response);
        }

        private static string HttpPost(string uri, string postData)
        {
            var encoding = new ASCIIEncoding();
            var data = encoding.GetBytes(postData);

            // Prepare web request...
            var myRequest = (HttpWebRequest)WebRequest.Create(BASE_URL + uri);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/json";
            myRequest.ContentLength = data.Length;
            var newStream = myRequest.GetRequestStream();
            // Send the data.

            newStream.Write(data, 0, data.Length);
            newStream.Close();

            var response = (HttpWebResponse)myRequest.GetResponse();
            var reader = new StreamReader(response.GetResponseStream());
            var output = reader.ReadToEnd();
            response.Close();

            return output;
        }


        private static string SendRequestToAlepay(AlepayRequestInfo info, string uri, string storeCode = "")
        {
            var data = StringHelper.ToJsonString(info);
            var dataEncrypt = Crypter.Encrypt(data, storeCode);
            string token = TOKEN;
            string checksumKey = CHECKSUM_KEY;
            if (ENABLE_KEY_FORSTORE && !string.IsNullOrEmpty(storeCode))
            {
                checksumKey = ConfigHelper.GetStringProperty(CONFIG_CHECKSUM_KEY + "_" + storeCode, "");
                if (string.IsNullOrEmpty(checksumKey))
                    checksumKey = CHECKSUM_KEY;
                token = ConfigHelper.GetStringProperty(CONFIG_TOKEN_KEY + "_" + storeCode, "");
                if (string.IsNullOrEmpty(token))
                    token = TOKEN;
            }

            var checksum = CreateMD5Hash(dataEncrypt + checksumKey);

            var json = StringHelper.ToJsonString(new
            {
                token,
                data = dataEncrypt,
                checksum
            });

            return HttpPost(uri, json);
        }

        private static String CreateMD5Hash(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }

        //private static String GetErrorMessage(string _ErrorCode)
        //{
        //    String _Message = "";
        //    switch (_ErrorCode)
        //    {
        //        case "00":
        //            _Message = "Giao dịch thành công";
        //            break;
        //        case "01":
        //            _Message = "Lỗi, địa chỉ IP truy cập API của NgânLượng.vn bị từ chối";
        //            break;
        //        case "02":
        //            _Message = "Lỗi, tham số gửi từ merchant tới NgânLượng.vn chưa chính xác.";
        //            break;
        //        case "03":
        //            _Message = "Lỗi, mã merchant không tồn tại hoặc merchant đang bị khóa kết nối tới NgânLượng.vn";
        //            break;
        //        case "04":
        //            _Message = "Lỗi, mã checksum không chính xác";
        //            break;
        //        case "05":
        //            _Message = "Tài khoản nhận tiền nạp của merchant không tồn tại";
        //            break;
        //        case "06":
        //            _Message = "Tài khoản nhận tiền nạp của  merchant đang bị khóa hoặc bị phong tỏa, không thể thực hiện được giao dịch nạp tiền";
        //            break;
        //        case "07":
        //            _Message = "Thẻ đã được sử dụng";
        //            break;
        //        case "08":
        //            _Message = "Thẻ bị khóa";
        //            break;
        //        case "09":
        //            _Message = "Thẻ hết hạn sử dụng";
        //            break;
        //        case "10":
        //            _Message = "Thẻ chưa được kích hoạt hoặc không tồn tại";
        //            break;
        //        case "11":
        //            _Message = "Mã thẻ sai định dạng";
        //            break;
        //        case "12":
        //            _Message = "Sai số serial của thẻ";
        //            break;
        //        case "13":
        //            _Message = "Mã thẻ và số serial không khớp";
        //            break;
        //        case "14":
        //            _Message = "Thẻ không tồn tại";
        //            break;
        //        case "15":
        //            _Message = "Thẻ không sử dụng được";
        //            break;
        //        case "16":
        //            _Message = "Số lần tưử của thẻ vượt quá giới hạn cho phép";
        //            break;
        //        case "17":
        //            _Message = "Hệ thống Telco bị lỗi hoặc quá tải, thẻ chưa bị trừ";
        //            break;
        //        case "18":
        //            _Message = "Hệ thống Telco  bị lỗi hoặc quá tải, thẻ có thể bị trừ, cần phối hợp với nhà mạng để đối soát";
        //            break;
        //        case "19":
        //            _Message = "Kết nối NgânLượng với Telco bị lỗi, thẻ chưa bị trừ.";
        //            break;
        //        case "20":
        //            _Message = "Kết nối tới Telco thành công, thẻ bị trừ nhưng chưa cộng tiền trên NgânLượng.vn";
        //            break;
        //        case "99":
        //            _Message = "Lỗi tuy nhiên lỗi chưa được định nghĩa hoặc chưa xác định được nguyên nhân";
        //            break;
        //    }
        //    return _Message;
        //}
    }

    public class AlepayRequestInfo
    {
        public string orderCode { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public string orderDescription { get; set; }
        public int totalItem { get; set; }
        public int checkoutType { get; set; }

        //public bool installment { get; set; }
        //public int month { get; set; }
        //public string bankCode { get; set; }
        //public string paymentMethod { get; set; }
        public string returnUrl { get; set; }

        public string cancelUrl { get; set; }
        public string buyerName { get; set; }
        public string buyerEmail { get; set; }
        public string buyerPhone { get; set; }
        public string buyerAddress { get; set; }
        public string buyerCity { get; set; }
        public string buyerCountry { get; set; }
        public string paymentHours { get; set; }
        public bool allowDomestic { get; set; }
        public bool alowFomestic { get; set; }
    }

    public class AlepayResponseInfo
    {
        public string errorCode { get; set; }
        public string data { get; set; }
        public string checksum { get; set; }
        public string errorDescription { get; set; }
    }

    public class AlepayResponseDecrypted
    {
        public string token { get; set; }
        public string checkoutUrl { get; set; }
    }

    public class AlepayResponseCallback
    {
        public string errorCode { get; set; }
        public string data { get; set; }
        public string cancel { get; set; }
    }
}
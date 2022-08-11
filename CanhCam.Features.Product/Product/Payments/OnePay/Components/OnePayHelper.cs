using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public static class OnePayHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OnePayHelper));

        public static string URLPayment => ConfigHelper.GetStringProperty("OnePAY.URLPayment", string.Empty);
        public static string UrlQueryDR => ConfigHelper.GetStringProperty("OnePAY.UrlQueryDR", string.Empty);

        public static string Hashcode => ConfigHelper.GetStringProperty("OnePAY.Hashcode", string.Empty);
        public static string MerchantID => ConfigHelper.GetStringProperty("OnePAY.MerchantID", string.Empty);
        public static string Accesscode => ConfigHelper.GetStringProperty("OnePAY.Accesscode", string.Empty);
        public static string QueryDRUser => ConfigHelper.GetStringProperty("OnePAY.QueryDR.User", string.Empty);
        public static string QueryDRPassword => ConfigHelper.GetStringProperty("OnePAY.QueryDR.Password", string.Empty);

        public static int PaymentMethodId => ConfigHelper.GetIntProperty("OnePAY.PaymentMethodId", 0);
        public static int CompletedZoneId => ConfigHelper.GetIntProperty("OnePAY.CompletedZoneId", 0);

        public static Order ProcessCallbackData(ref string responsecode, ref string mess, bool saveDatabase = false)
        {
            var hashcode = string.Empty;
            if (hashcode.Length == 0)
            {
                responsecode = "0";
                mess = "invalid-hash";
                return null;
            }

            var hashvalidateResult = string.Empty;
            // Khoi tao lop thu vien
            VPCRequest conn = new VPCRequest("http://onepay.vn");
            conn.SetSecureSecret(hashcode);
            // Xu ly tham so tra ve va kiem tra chuoi du lieu ma hoa
            hashvalidateResult = conn.Process3PartyResponse(System.Web.HttpContext.Current.Request.QueryString);

            // Lay gia tri tham so tra ve tu cong thanh toan
            var responseCode = conn.GetResultField("vpc_TxnResponseCode", "Unknown").Trim();

            //string merchTxnRef = conn.GetResultField("vpc_MerchTxnRef", "0");
            string orderInfo = conn.GetResultField("vpc_OrderInfo", "");

            Order order = null;
            if (orderInfo.Length > 0)
                order = Order.GetByCode(orderInfo);
            if (order == null || order.OrderId < 0 || order.IsDeleted)
            {
                responsecode = "0";
                mess = "Order-Not-Found";
                return null;
            }

            responsecode = "1";
            mess = "confirm-success";
            if (saveDatabase)
            {
                // Sua lai ham check chuoi ma hoa du lieu
                if (hashvalidateResult == "CORRECTED" && responseCode == "0") // "Transaction was paid successful"
                {
                    if (order != null)
                    {
                        int oldStatus = order.PaymentStatus;
                        order.PaymentStatus = (int)OrderPaymentStatus.Successful;

                        OrderHelper.SendMailAfterOrder(order, CacheHelper.GetCurrentSiteSettings());
                        order.Save();
                        new OrderLog()
                        {
                            OrderId = order.OrderId,
                            Comment = OrderHelper.GetPaymentStatusResources(oldStatus) + " => " + OrderHelper.GetPaymentStatusResources(order.PaymentStatus),
                            UserEmail = "OnePay",
                            CreatedOn = DateTime.Now,
                            TypeName = ProductResources.SysnOrderPayment
                        }.Save();
                    }
                }
                else // "Transaction was not paid successful"
                {
                    if (order != null)
                    {
                        int oldStatus = order.PaymentStatus;
                        order.PaymentStatus = (int)OrderPaymentStatus.NotSuccessful;
                        order.OrderNote = GetResponseReson(responseCode);
                        order.Save();
                        new OrderLog()
                        {
                            OrderId = order.OrderId,
                            Comment = OrderHelper.GetPaymentStatusResources(oldStatus) + " => " + OrderHelper.GetPaymentStatusResources(order.PaymentStatus),
                            UserEmail = "OnePay",
                            CreatedOn = DateTime.Now,
                            TypeName = ProductResources.SysnOrderPayment
                        }.Save();
                    }
                }
            }

            return order;
        }

        private static string GetResponseReson(string statusCode)
        {
            switch (statusCode)
            {
                case "1":
                    return "Ngân hàng từ chối giao dịch. Liên hệ lại Ngân hàng theo số điện thoại đằng sau mặt thẻ hoặc kiểm tra lại thông tin OTP";

                case "7":
                    return "Lỗi hệ thống Ngân hàng phát hành thẻ. Liên hệ lại ngân hàng theo số điện thoại đằng sau mặt thẻ";

                case "8":
                case "9":
                    return "Sai thông tin số thẻ";

                case "10":
                    return "Thẻ hết hạn/ thẻ bị khóa. Liên hệ ngân hàng theo số điện thoại đằng sau mặt thẻ";

                case "11":
                    return "Thẻ chưa đăng ký sử dụng dịch vụ thanh toán qua Internet. ";

                case "12":
                    return "Thông tin ngày phát hành thẻ không đúng";

                case "13":
                    return "Vượt quá hạn mức thanh toán.";

                case "21":
                    return "Số dư không đủ để thanh toán";

                case "22":
                case "23":
                    return "Lỗi trả về trong tường hợp thanh toán bằng acc IB || MB";

                case "24":
                    return "Lỗi chung: Sai thông tin thẻ, không thể xác định thông tin chưa đúng";

                case "25":
                    return "Sai mã OTP";

                case "300":
                    return "Giao dịch pending tại Bank";

                case "255 || 7":
                    return "Lỗi liên quan đến request: Sai tham số";
            }

            return string.Empty;
        }

        public static string GetPaymentUrlIfNeeded(Order order)
        {
            if (order == null || order.OrderId <= 0)
                return string.Empty;

            if (order.PaymentMethod == OnePayHelper.PaymentMethodId) // Onepay
            {
                //https://mtf.onepay.vn/developer/?page=modul_noidia
                //https://mtf.onepay.vn/developer/?page=modul_quocte
                string returnUrl = SiteUtils.GetNavigationSiteRoot() + "/product/onepay/callback.aspx";

                order.PaymentAPICode = Guid.NewGuid().ToString().Replace("-", string.Empty);
                order.PaymentStatus = (int)OrderPaymentStatus.Pending;

                // Khoi tao lop thu vien va gan gia tri cac tham so gui sang cong thanh toan
                VPCRequest conn = new VPCRequest(URLPayment);
                conn.SetSecureSecret(Hashcode);
                // Add the Digital Order Fields for the functionality you wish to use
                // Core Transaction Fields
                if (order.PaymentMethod == OnePayHelper.PaymentMethodId)
                    conn.AddDigitalOrderField("AgainLink", "http://onepay.vn");
                conn.AddDigitalOrderField("Title", "onepay paygate");
                conn.AddDigitalOrderField("vpc_Locale", "vn");//Chon ngon ngu hien thi tren cong thanh toan (vn/en)
                conn.AddDigitalOrderField("vpc_Version", "2");
                conn.AddDigitalOrderField("vpc_Command", "pay");
                conn.AddDigitalOrderField("vpc_Merchant", MerchantID);
                conn.AddDigitalOrderField("vpc_AccessCode", Accesscode);
                conn.AddDigitalOrderField("vpc_MerchTxnRef", order.PaymentAPICode);
                conn.AddDigitalOrderField("vpc_OrderInfo", order.OrderCode.ToString());
                conn.AddDigitalOrderField("vpc_Amount", Convert.ToDouble(order.OrderTotal * 100).ToString()); //*100?
                conn.AddDigitalOrderField("vpc_Currency", "VND");
                conn.AddDigitalOrderField("vpc_ReturnURL", returnUrl);
                // Thong tin them ve khach hang. De trong neu khong co thong tin
                //conn.AddDigitalOrderField("vpc_SHIP_Street01", order.BillingAddress);
                //conn.AddDigitalOrderField("vpc_SHIP_Provice", billingProvinceName);
                //conn.AddDigitalOrderField("vpc_SHIP_City", billingDistrictName);
                //conn.AddDigitalOrderField("vpc_SHIP_Country", "Vietnam");
                conn.AddDigitalOrderField("vpc_Customer_Phone", order.BillingPhone);
                conn.AddDigitalOrderField("vpc_Customer_Email", order.BillingEmail);
                if (order.UserGuid != Guid.Empty)
                    conn.AddDigitalOrderField("vpc_Customer_Id", order.UserGuid.ToString());
                // Dia chi IP cua khach hang
                conn.AddDigitalOrderField("vpc_TicketNo", order.CreatedFromIP);
                // Chuyen huong trinh duyet sang cong thanh toan

                order.Save(); // Save Transaction Reference

                return conn.Create3PartyQueryString();
            }

            return string.Empty;
        }

        public static int GetPaymentStatus(Order order, ref string result)
        {
            if (order == null || order.OrderId <= 0 || order.PaymentAPICode.Length == 0)
            {
                result = "Đơn hàng không hợp lệ";
                return -1;
            }

            if (order.PaymentMethod != OnePayHelper.PaymentMethodId)
            {
                result = "Đơn hàng này không thanh toán online";
                return -1;
            }

            var response = DoQueryDR(order);
            if (string.IsNullOrEmpty(response))
            {
                result = "Kết nối tới Onepay bị lỗi";
                return -1;
            }

            if (response.Contains("vpc_DRExists=N"))
            {
                result = "Không tồn tại giao dịch: " + order.PaymentAPICode;
                return -1;
            }

            if (response.Contains("vpc_TxnResponseCode=0"))
            {
                result = "Giao dịch thanh toán thành công";
                return (int)OrderPaymentStatus.Successful;
            }

            result = "Giao dịch không thanh toán thành công";
            return (int)OrderPaymentStatus.NotSuccessful;
        }

        public static string DoQueryDR(Order order)
        {
            if (order == null || order.OrderId <= 0)
                return string.Empty;

            string responseFromServer = string.Empty;

            //https://mtf.onepay.vn/developer/?page=modul_noidia
            //https://mtf.onepay.vn/developer/?page=modul_quocte
            // Thông số tài khoản cổng thanh toán:

            var postData = string.Empty;
            var seperator = string.Empty;
            var paras = 8;
            string param = "vpc_AccessCode=" + Accesscode
                + "&vpc_Command=queryDR"
                + "&vpc_MerchTxnRef=" + order.PaymentAPICode
                + "&vpc_Merchant=" + MerchantID
                + "&vpc_Password=" + QueryDRPassword
                + "&vpc_User=" + QueryDRUser
                + "&vpc_Version=2";
            string[,] array =
            {
                    {"vpc_AccessCode", Accesscode},
                    {"vpc_Command", "queryDR"},
                    {"vpc_MerchTxnRef", order.PaymentAPICode},
                    {"vpc_Merchant", MerchantID},
                    {"vpc_Password", QueryDRPassword},
                    {"vpc_User", QueryDRUser},
                    {"vpc_Version", "2"},
                    {"vpc_SecureHash", SignSHA256(param,Hashcode)}
                };
            for (int i = 0; i < paras; i++)
            {
                postData = postData + seperator + HttpUtility.UrlEncode(array[i, 0]) + "=" + HttpUtility.UrlEncode(array[i, 1]);
                seperator = "&";
            }

            try
            {
                var request = (HttpWebRequest)WebRequest.Create(UrlQueryDR);

                // Service require TLS 1.2 (not support in .NET Framework 4.0)
                // http://stackoverflow.com/questions/33761919/tls-1-2-in-net-framework-4-0
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;

                request.Method = "POST";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.UserAgent = "HTTP Client";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return responseFromServer;
        }

        public static string SignSHA256(string message, string key)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = HexDecode(key);

            HMACSHA256 hmacsha256 = new HMACSHA256(keyByte);

            byte[] messageBytes = encoding.GetBytes(message);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return ByteToString(hashmessage);
        }

        private static byte[] HexDecode(string hex)
        {
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber);
            }
            return bytes;
        }

        public static string ByteToString(byte[] buff)
        {
            string sbinary = "";

            for (int i = 0; i < buff.Length; i++)
            {
                sbinary += buff[i].ToString("X2"); // hex format
            }
            return (sbinary);
        }
    }
}
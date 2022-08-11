/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2016-11-10
/// Last Modified:			2016-11-10

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public enum VietttelPostShippingStatus
    {
        DaGiaoChuyenPhat = 1,
        DaGiaoChoKhachHang = 2
    }

    public enum ViettelPostShippingMethodEnum
    {
        ECommerce = 1,
        Express = 2,
        International = 3
    }

    public static class ViettelPostHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ViettelPostHelper));
        private static string _urlService = string.Empty;
        private static string _apiKey = string.Empty;
        private static string _sender_District = string.Empty;
        private static string _sender_Province = string.Empty;
        private static string _sender_WARD = string.Empty;
        private static string _token = string.Empty;
        public static int ShippingMethodId = ConfigHelper.GetIntProperty("ViettelPost:ShippingMethodId", 0);
        public static int ProductWeightDefault = ConfigHelper.GetIntProperty("ViettelPost:ProductWeightDefault", 0);
        public static string EnableService = ConfigHelper.GetStringProperty("ViettelPost:EnableService", "");
        public static string DisableService = ConfigHelper.GetStringProperty("ViettelPost:DisableService", "");
        public static string WebhookToken = ConfigHelper.GetStringProperty("ViettelPost:WebhookToken", "cbf60ac1-9566-4c33-a121-8d6dae907458");

        private static bool CheckValidApi()
        {
            _urlService = ConfigHelper.GetStringProperty("ViettelPost:urlService", _urlService);
            _apiKey = ConfigHelper.GetStringProperty("ViettelPost:apiKey", _apiKey);
            _sender_District = ConfigHelper.GetStringProperty("ViettelPost:SenderDistrict", _sender_District);
            _sender_Province = ConfigHelper.GetStringProperty("ViettelPost:SenderProvince", _sender_Province);
            _sender_WARD = ConfigHelper.GetStringProperty("ViettelPost:SenderWARD", _sender_WARD);

            if (_urlService.Length == 0
                || _apiKey.Length == 0
                || _sender_District.Length == 0
                || _sender_Province.Length == 0
                || _sender_WARD.Length == 0
                )
                return false;

            return true;
        }

        public static List<ShippingOption> GetSeriveList(decimal totalWeight, Guid BillingDistrictGuid,
            decimal amout, int totalQuantities, Store store, ref string error)
        {
            List<ShippingOption> lstServices = new List<ShippingOption>();
            if (!CheckValidApi())
            {
                error = "API chưa được cấu hình hoặc Cấu hình không đúng.";
                return lstServices;
            }

            string functionName = "order/getPriceAll";
            GeoZone district = new GeoZone(BillingDistrictGuid);
            GeoZone province = new GeoZone(district.ParentGuid);
            ViettelPostProvince vittelProvince = ShippingCacheHelper.GetViettelPostProvince(province.Guid);
            ViettelPostCode vittelDistrict = ShippingCacheHelper.GetViettelPostCode(district.Code);
            string sender_Province = _sender_Province;
            string sender_District = _sender_District;

            if (store != null)
            {
                try
                {
                    GeoZone storeDistrict = new GeoZone(store.DistrictGuid);
                    GeoZone storeProvince = new GeoZone(store.ProvinceGuid);

                    ViettelPostProvince storeVittelProvince = ShippingCacheHelper.GetViettelPostProvince(storeProvince.Guid);
                    ViettelPostCode storeVittelDistrict = ShippingCacheHelper.GetViettelPostCode(storeDistrict.Code);

                    if (storeVittelProvince != null)
                        sender_Province = storeVittelProvince.ViettelPostProvinceCode;
                    if (storeVittelDistrict != null)
                        sender_District = storeVittelDistrict.ViettelPostCode2;
                }
                catch (Exception ex)
                {
                    log.Info("Error : " + ex.Message);
                }
            }

            if (vittelProvince == null || vittelDistrict == null)
            {
                if (vittelProvince == null)
                    error = "Tỉnh thành chưa cấu hình Vittel Post ";
                if (vittelDistrict == null)
                    error += "Quận huyện chưa cấu hình Vittel Post";
                return lstServices;
            }

            var json = StringHelper.ToJsonString(new
            {
                PRODUCT_WEIGHT = totalWeight, // Trọng lượng bưu phẩm/ weight
                PRODUCT_PRICE = (amout).ToString("#"), // Giá trị hàng hóa/ price
                MONEY_COLLECTION = (amout).ToString("#"), // Tiền thu hộ/ collection money
                //ORDER_SERVICE_ADD = "", //Dịch vụ cộng thêm (theo hợp đồng) dùng nhiều dv cộng thêm thì các dịch vụ cách nhau dấu “,”/ Additional Services (contract), additional services are separated by “,”
                //ORDER_SERVICE = serviceID, // Dịch vụ (theo hợp đồng)/ Services (contract)

                SENDER_PROVINCE = sender_Province, // ID Tỉnh gửi/ Sender province Id
                SENDER_DISTRICT = sender_District, // ID Huyện gửi / Sender district ID

                RECEIVER_PROVINCE = vittelProvince.ViettelPostProvinceCode, // ID Tỉnh nhận / Reciever province Id
                RECEIVER_DISTRICT = vittelDistrict.ViettelPostCode2, // ID Huyện nhận / Reciever district Id
                PRODUCT_TYPE = "HH", // Thư/Envelope: TH; Hàng hóa/ Goods: HH

                //PRODUCT_QUANTITY = totalQuantities, // Số lượng/ quantity
                TYPE = 1,// 1 : Trong nước/ inland; 0: Quốc tế/ international
            });
            if (ConfigHelper.GetBoolProperty("EnableVTPLog", false))
                log.Info(json);
            var response = GetResponseData(functionName, json, false);
            if (ConfigHelper.GetBoolProperty("EnableVTPLog", false))
                log.Info(response);
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var listResult = js.Deserialize<List<ViettelPostServiceGetPrice>>(response);
                var arrayServiceEnable = EnableService.Split(';');
                var arrayServiceDisable = DisableService.Split(';');

                if (listResult.Count > 0)
                {
                    foreach (ViettelPostServiceGetPrice iii in listResult)
                    {
                        if (!string.IsNullOrEmpty(EnableService)
                                     && arrayServiceEnable.Length > 0
                                     && !arrayServiceEnable.Contains(iii.MA_DV_CHINH))
                            continue;
                        if (!string.IsNullOrEmpty(DisableService)
                                     && arrayServiceDisable.Length > 0
                                     && arrayServiceDisable.Contains(iii.MA_DV_CHINH))
                            continue;
                        var option = new ShippingOption()
                        {
                            Description = iii.TEN_DICHVU,
                            Price = iii.GIA_CUOC,
                            Value = iii.MA_DV_CHINH,
                            Name = iii.TEN_DICHVU
                        };
                        string str = ResourceHelper.GetResourceString("ProductResources", "ViettelPostService" + option.Value);
                        if (!string.IsNullOrEmpty(str) && str != "ViettelPostService" + option.Value)
                            option.Name = str;

                        lstServices.Add(option);
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                try
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var Result = js.Deserialize<ViettelPostResponse>(response);
                    error = Result.message;
                }
                catch (Exception)
                {
                }
            }

            return lstServices;
        }

        public static int CalculateServiceFee(decimal totalWeight,
            Guid BillingDistrictGuid, decimal amout, string serviceID,
            int totalQuantities,
            Store store,
            ref string error,
            ref string expectedTime,
            decimal orderLength = 0,
            decimal orderWidth = 0,
            decimal orderHeight = 0)
        {
            if (!CheckValidApi())
            {
                error = "API chưa được cấu hình hoặc Cấu hình không đúng.";
                return 0;
            }

            string functionName = "order/getPriceAll";
            GeoZone district = new GeoZone(BillingDistrictGuid);

            GeoZone province = new GeoZone(district.ParentGuid);
            //var lst = ViettelPostCode.GetAll();
            //var lst2 = ViettelPostProvince.GetAll();

            //var vittelProvince = lst2.Where(it => it.ProvinceGuid == province.Guid).FirstOrDefault();
            var vittelProvince = ShippingCacheHelper.GetViettelPostProvince(province.Guid);
            var vittelDistrict = ShippingCacheHelper.GetViettelPostCode(district.Code);
            // Warehouse warehouse = new Warehouse(district.Code);

            string sender_Province = _sender_Province;
            string sender_District = _sender_District;
            string sender_Wards = _sender_WARD;

            if (store != null)
            {
                try
                {
                    GeoZone storeDistrict = new GeoZone(store.DistrictGuid);
                    GeoZone storeProvince = new GeoZone(store.ProvinceGuid);

                    ViettelPostProvince storeVittelProvince = ShippingCacheHelper.GetViettelPostProvince(storeProvince.Guid);
                    ViettelPostCode storeVittelDistrict = ShippingCacheHelper.GetViettelPostCode(storeDistrict.Code);

                    if (storeVittelProvince != null)
                        sender_Province = storeVittelProvince.ViettelPostProvinceCode;
                    if (storeVittelDistrict != null)
                        sender_District = storeVittelDistrict.ViettelPostCode2;
                }
                catch (Exception ex)
                {
                    log.Info("Error : " + ex.Message);
                }
            }

            //var vittelDistrict = lst.Where(it => it.GeoZoneCode == district.Code).FirstOrDefault();
            if (vittelProvince == null || vittelDistrict == null)
            {
                if (vittelProvince == null)
                    error = "Tỉnh thành chưa cấu hình Vittel Post ";
                if (vittelDistrict == null)
                    error += "Quận huyện chưa cấu hình Vittel Post";
                return 0;
            }

            var json = StringHelper.ToJsonString(new
            {
                PRODUCT_WEIGHT = totalWeight, // Trọng lượng bưu phẩm/ weight
                PRODUCT_PRICE = (amout).ToString("#"), // Giá trị hàng hóa/ price
                MONEY_COLLECTION = (amout).ToString("#"), // Tiền thu hộ/ collection money
                //ORDER_SERVICE_ADD = "", //Dịch vụ cộng thêm (theo hợp đồng) dùng nhiều dv cộng thêm thì các dịch vụ cách nhau dấu “,”/ Additional Services (contract), additional services are separated by “,”
                ORDER_SERVICE = serviceID, // Dịch vụ (theo hợp đồng)/ Services (contract)
                SENDER_PROVINCE = sender_Province, // ID Tỉnh gửi/ Sender province Id
                SENDER_DISTRICT = sender_District, // ID Huyện gửi / Sender district ID
                RECEIVER_PROVINCE = vittelProvince.ViettelPostProvinceCode, // ID Tỉnh nhận / Reciever province Id
                RECEIVER_DISTRICT = vittelDistrict.ViettelPostCode2, // ID Huyện nhận / Reciever district Id
                PRODUCT_TYPE = "HH", // Thư/Envelope: TH; Hàng hóa/ Goods: HH
                //PRODUCT_QUANTITY = totalQuantities, // Số lượng/ quantity
                TYPE = 1,// 1 : Trong nước/ inland; 0: Quốc tế/ international
                PRODUCT_WIDTH = orderWidth,
                PRODUCT_HEIGHT = orderHeight,
                PRODUCT_LENGTH = orderLength
            });

            if (ConfigHelper.GetBoolProperty("EnableVTPLog", false))
                log.Info("Json :" + json);
            var response = GetResponseData(functionName, json, false);
            if (ConfigHelper.GetBoolProperty("EnableVTPLog", false))
                log.Info("Response :" + response);
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var listResult = js.Deserialize<List<ViettelPostServiceGetPrice>>(response);
                if (listResult.Count > 0)
                {
                    foreach (ViettelPostServiceGetPrice iii in listResult)
                    {
                        if (iii.MA_DV_CHINH.ToUpper().Equals(serviceID.ToUpper()))
                        {
                            expectedTime = iii.THOI_GIAN;
                            return iii.GIA_CUOC;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                var Result = js.Deserialize<ViettelPostResponse>(response);
                error = Result.message;
            }

            return 0;
        }

        private static string GetResponseData(string functionName, string postJsonValue, bool returnObject = true, string meothd = "POST")
        {
            try
            {
                // Service require TLS 1.2 (not support in .NET Framework 4.0)
                // http://stackoverflow.com/questions/33761919/tls-1-2-in-net-framework-4-0
                ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
                var url = string.Format("{0}/{1}", _urlService, functionName);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = meothd;
                httpWebRequest.Timeout = 5000;
                httpWebRequest.ReadWriteTimeout = 32000;
                httpWebRequest.Headers.Add("Token", _apiKey);
                if (meothd == "POST")
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(postJsonValue);
                        streamWriter.Flush();
                    }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    if (returnObject)
                    {
                        var jsonSerializer = new JavaScriptSerializer();
                        var obj = jsonSerializer.Deserialize<ViettelPostResponseV2>(result);
                        //dynamic data = JObject.Parse(result);
                        return new JavaScriptSerializer().Serialize(obj.data);
                    }
                    else
                        return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return string.Empty;
        }

        public static ViettelPostResponse MakeOrderForm(Order order, Store store,
            List<OrderItem> orderItems,
            List<Product> products,
            string service,
            ref string error)
        {
            if (!CheckValidApi())
            {
                error = "API chưa được cấu hình hoặc Cấu hình không đúng.";
                return null;
            }

            string functionName = "order/createOrder";
            SiteSettings s = new SiteSettings();

            string receiverFullName = order.BillingFirstName + order.BillingLastName;
            if (order.ShippingFirstName.Length > 0 || order.ShippingLastName.Length > 0)
            {
                receiverFullName = order.ShippingFirstName + order.ShippingLastName;
            }

            GeoZone district = new GeoZone(order.BillingDistrictGuid);
            GeoZone province = new GeoZone(district.ParentGuid);

            //    var vittelProvince = new ViettelPostProvince(province.Guid);

            var vittelProvince = ShippingCacheHelper.GetViettelPostProvince(province.Guid);
            var vittelDistrict = ShippingCacheHelper.GetViettelPostCode(district.Code);

            string sender_Province = _sender_Province;
            string sender_District = _sender_District;
            string sender_WARD = _sender_WARD;

            int cusId = ConfigHelper.GetIntProperty("VittelPost_CUS_ID", 0);
            int groupAddressId = ConfigHelper.GetIntProperty("VittelPost_GroupAddress_ID", 0);
            string fullName = ConfigHelper.GetStringProperty("Sender:FullName", "");
            string address = ConfigHelper.GetStringProperty("Sender:Address", "");
            string phone = ConfigHelper.GetStringProperty("Sender:Phone", "");
            if (store != null)
            {
                try
                {
                    fullName = store.Name;
                    address = store.Address;
                    phone = store.Phone;
                    if (!string.IsNullOrEmpty(store.ContactPerson) && int.TryParse(store.ContactPerson, out int Id))
                        groupAddressId = Id;
                    GeoZone storeDistrict = new GeoZone(store.DistrictGuid);
                    GeoZone storeProvince = new GeoZone(store.ProvinceGuid);

                    ViettelPostProvince storeVittelProvince = ShippingCacheHelper.GetViettelPostProvince(storeProvince.Guid);
                    ViettelPostCode storeVittelDistrict = ShippingCacheHelper.GetViettelPostCode(storeDistrict.Code);
                    ViettelPostCode storeVittelWard = ShippingCacheHelper.GetVittelPostWardCodeByStore(store);

                    if (storeVittelProvince != null)
                        sender_Province = storeVittelProvince.ViettelPostProvinceCode;
                    if (storeVittelDistrict != null)
                        sender_District = storeVittelDistrict.ViettelPostCode2;
                    if (storeVittelWard != null)
                        sender_WARD = storeVittelWard.ViettelPostCode2;
                }
                catch (Exception ex)
                {
                    log.Info("Error : " + ex.Message);
                }
            }

            int order_Payment = 1;
            int collection = 0;

            if (order.PaymentStatus != (int)OrderPaymentStatus.Successful)
            {
                order_Payment = 2;
                collection = Convert.ToInt32(order.OrderTotal - order.OrderShipping);//- order.OrderDiscount), OrderTotal đã trừ Order Discount rồi nên bỏ
            }
            string title = string.Join(" ,", products.Select(p => p.Title));
            if (title.Length > 150)
                title = title.Substring(0, 149) + "....";

            if (collection < 0)
                collection = 0;

            var cob = collection;
            if (PaymentHelper.IsOnlinePayment(order.PaymentMethod) && order.PaymentStatus == (int)OrderPaymentStatus.Successful)
                cob = 0;
            var MONEY_FEEVAS = decimal.Zero;
            //if (order.VoucherAmount > 0)
            //    MONEY_FEEVAS += -order.VoucherAmount;
            //if (order.OrderCouponAmount > 0)
            //    MONEY_FEEVAS += -order.OrderCouponAmount;

            var json = StringHelper.ToJsonString(new
            {
                ORDER_NUMBER = order.OrderId,
                CUS_ID = cusId,
                GROUPADDRESS_ID = groupAddressId,
                DELIVERY_DATE = DateTime.Now.AddMinutes(10).ToString("dd/MM/yyyy H:m:s"),
                SENDER_FULLNAME = fullName,
                SENDER_ADDRESS = address,
                SENDER_PHONE = phone,
                SENDER_EMAIL = ConfigHelper.GetStringProperty("Sender:Email", ""),
                SENDER_DISTRICT = sender_District,
                SENDER_PROVINCE = sender_Province,
                SENDER_WARD = sender_WARD,
                RECEIVER_FULLNAME = receiverFullName,
                RECEIVER_ADDRESS = order.ShippingAddress.Length > 0 ? order.ShippingAddress : order.BillingAddress,
                RECEIVER_PHONE = order.ShippingPhone.Length > 0 ? order.ShippingPhone : order.BillingPhone,
                RECEIVER_EMAIL = order.ShippingEmail.Length > 0 ? order.ShippingEmail : order.BillingEmail,
                RECEIVER_PROVINCE = vittelProvince.ViettelPostProvinceCode, // ID Tỉnh nhận / Reciever province Id
                RECEIVER_DISTRICT = vittelDistrict.ViettelPostCode2, // ID Huyện nhận / Reciever district Id
                RECEIVER_WARD = 0,
                PRODUCT_NAME = title,
                PRODUCT_DESCRIPTION = "",
                PRODUCT_QUANTITY = orderItems.GetTotalProducts(),
                PRODUCT_PRICE = Convert.ToInt32(order.OrderTotal - order.OrderShipping),//- order.OrderDiscount), OrderTotal đã trừ Order Discount rồi nên bỏ
                PRODUCT_WEIGHT = Convert.ToInt32(order.TotalWeight),
                PRODUCT_LENGTH = Convert.ToInt32(order.TotalLength),
                PRODUCT_WIDTH = Convert.ToInt32(order.TotalWidth),
                PRODUCT_HEIGHT = Convert.ToInt32(order.TotalHeight),
                PRODUCT_TYPE = "HH",
                ORDER_PAYMENT = order_Payment,
                ORDER_SERVICE = service,
                MONEY_COLLECTION = Convert.ToInt32(cob),
                MONEY_FEEVAS = Convert.ToInt32(MONEY_FEEVAS),
                MONEY_TOTAL = Convert.ToInt32(collection),
                LIST_ITEM = BuildOrderItem(orderItems, products)
            });
            log.Info(json);
            var response = GetResponseData(functionName, json, false);

            JavaScriptSerializer js = new JavaScriptSerializer();
            var Result = js.Deserialize<ViettelPostResponse>(response);
            error = Result.message;
            return Result;
        }

        private static List<ViettelPostOrderItem> BuildOrderItem(List<OrderItem> lstOrderItem, List<Product> lstProduct)
        {
            List<ViettelPostOrderItem> lst = new List<ViettelPostOrderItem>();
            foreach (OrderItem item in lstOrderItem)
            {
                Product product = lstProduct.Where(p => p.ProductId == item.ProductId).FirstOrDefault();

                lst.Add(new ViettelPostOrderItem()
                {
                    PRODUCT_NAME = product.Title,
                    PRODUCT_PRICE = Convert.ToInt32(item.Price),
                    PRODUCT_QUANTITY = item.Quantity,
                    PRODUCT_WEIGHT = ProductWeightDefault > 0 ? ProductWeightDefault : Convert.ToInt32(product.Weight)
                });
            }

            return lst;
        }
    }
}
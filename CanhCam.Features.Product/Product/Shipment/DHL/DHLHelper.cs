using CanhCam.Business;
using CanhCam.Shipping.DHL.Model;
using CanhCam.Web.Framework;
using log4net;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public class DHLHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NinjavanHelper));
        public static int ShippingMethodId = ConfigHelper.GetIntProperty("DHL:ShippingMethodId", 0);
        private static string SendOrderUrl = ConfigHelper.GetStringProperty("DHL:SendOrderUrl", string.Empty);
        private static string OAuthAccessUrl = ConfigHelper.GetStringProperty("DHL:OAuthAccessUrl", string.Empty);
        private static string ClientId = ConfigHelper.GetStringProperty("DHL:ClientId", string.Empty);
        private static string Password = ConfigHelper.GetStringProperty("DHL:Password", string.Empty);

        private static string Pickup_AccountId = ConfigHelper.GetStringProperty("DHL:Pickup_AccountId", string.Empty);
        private static string SoldTo_AccountId = ConfigHelper.GetStringProperty("DHL:SoldTo_AccountId", string.Empty);

        private static string Pickup_Name = ConfigHelper.GetStringProperty("DHL:Pickup_Name", string.Empty);
        private static string Pickup_Address1 = ConfigHelper.GetStringProperty("DHL:Pickup_Address1", string.Empty);
        private static string Pickup_Address2 = ConfigHelper.GetStringProperty("DHL:Pickup_Address2", string.Empty);
        private static string Pickup_State = ConfigHelper.GetStringProperty("DHL:Pickup_State", string.Empty);
        private static string Pickup_District = ConfigHelper.GetStringProperty("DHL:Pickup_District", string.Empty);
        private static string Pickup_Country = ConfigHelper.GetStringProperty("DHL:Pickup_Country", string.Empty);
        private static string Pickup_Phone = ConfigHelper.GetStringProperty("DHL:Pickup_Phone", string.Empty);
        private static string Pickup_Email = ConfigHelper.GetStringProperty("DHL:Pickup_Email", string.Empty);

        public static string GetOAuthAccessToken()
        {
            var client = new RestClient(OAuthAccessUrl + "?clientId=" + ClientId + "&password=" + Password)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            var js = new JavaScriptSerializer();
            var jsondata = js.Deserialize<AccessTokenResponse>(response.Content);
            if (!string.IsNullOrEmpty(jsondata.token))
                return jsondata.token;

            return string.Empty;
        }

        public static DHLResponse SendOrder(Order order, List<OrderItem> orderItems, List<Product> products)
        {
            string json = BuildOderJson(order, orderItems, products);
            var client = new RestClient(SendOrderUrl)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            var js = new JavaScriptSerializer();
            var result = js.Deserialize<DHLResponse>(response.Content);
            return result;
        }

        public static string BuildOderJson(Order order, List<OrderItem> orderItems, List<Product> lstProducts)
        {
            string result = StringHelper.ToJsonString(new
            {
                manifestRequest = new
                {
                    hdr = new
                    {
                        messageType = "LABLE",
                        messageDateTime = DateTime.Now.ToString("T"),
                        accessToken = GetOAuthAccessToken(),
                        messageVersion = "1.4",
                        messageLanguage = "en"
                    },
                    bd = GetShipmentItems(order, orderItems, lstProducts)
                }
            });
            log.Info("Oder Json :" + result);
            return result;
        }

        private static List<object> GetShipmentItems(Order order, List<OrderItem> orderItems, List<Product> lstProducts)
        {
            List<object> objs = new List<object>();
            foreach (var item in orderItems)
            {
                var product = lstProducts.FirstOrDefault(it => it.ProductId == item.ProductId);
                if (product != null)
                    objs.Add(GetProductObject(order, item, product));
            }
            return objs;
        }

        private static object GetProductObject(Order order, OrderItem item, Product product)
        {
            return new
            {
                pickupAccountId = Pickup_AccountId,
                soldToAccountId = SoldTo_AccountId,
                handoverMethod = 2,
                pickupAddress = new
                {
                    name = Pickup_Name,
                    address1 = Pickup_Address1,
                    address2 = Pickup_Address2,
                    state = Pickup_State,
                    district = Pickup_District,
                    country = Pickup_Country,
                    phone = Pickup_Phone,
                    email = Pickup_Email
                },
                shipmentItems = new
                {
                    consigneeAddress = new
                    {
                        name = order.BillingFirstName + " " + order.BillingLastName,
                        address1 = order.BillingAddress,
                        address2 = order.BillingAddress,
                        state = GetGeoZone(order.BillingProvinceGuid),
                        district = GetGeoZone(order.BillingDistrictGuid),
                        country = "VN",
                        phone = order.BillingPhone,
                        email = order.BillingEmail
                    },
                    returnAddress = new
                    {
                        name = Pickup_Name,
                        address1 = Pickup_Address1,
                        address2 = Pickup_Address2,
                        state = Pickup_State,
                        district = Pickup_District,
                        country = Pickup_Country,
                        phone = Pickup_Phone,
                        email = Pickup_Email
                    },
                    shipmentID = order.OrderCode,
                    returnMode = "01",
                    totalWeightUOM = "g",
                    productCode = product.Code,
                    codValue = ((int)(item.Price * item.Quantity) - item.DiscountAmount),
                    currency = "VND",
                    packageDesc = product.Title,
                    totalWeight = ((int)product.Weight * item.Quantity),
                }
            };
        }

        private static string GetGeoZone(Guid guid)
        {
            return string.Empty;
        }
    }
}
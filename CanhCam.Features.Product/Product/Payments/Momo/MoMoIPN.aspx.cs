using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public partial class MoMoIPN : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MoMoIPN));

        private string MoMopartnerCode = MomoHelper.partnerCode;
        private string MoMoaccessKey = MomoHelper.accessKey;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private NameValueCollection postParams = null;

        private void LoadParams()
        {
            postParams = HttpUtility.ParseQueryString(Request.GetRequestBody());
        }

        private void Page_Load(object sender, EventArgs e)
        {
            // don't accept get requests
            if (Request.HttpMethod == "POST")
            {
                Response.ContentType = "text/json";
                Encoding encoding = new UTF8Encoding();
                Response.ContentEncoding = encoding;
                try
                {
                    LoadParams();

                    //var js = new JavaScriptSerializer();
                    //var jsondata = js.Deserialize<Dictionary<string, object>>(data);
                    string partnerCode = string.Empty;
                    if (postParams.Get("partnerCode") != null)
                        partnerCode = postParams.Get("partnerCode").ToString();

                    string accessKey = string.Empty;
                    if (postParams.Get("accessKey") != null)
                        accessKey = postParams.Get("accessKey").ToString();
                    int orderId = 0;
                    if (postParams.Get("orderInfo") != null)
                        orderId = Convert.ToInt32(postParams.Get("orderInfo"));

                    string transId = string.Empty;
                    if (postParams.Get("transId") != null)
                        transId = postParams.Get("transId").ToString();
                    int errorCode = 0;
                    if (postParams.Get("errorCode") != null)
                        errorCode = Convert.ToInt32(postParams.Get("errorCode"));

                    var order = new Order(orderId);
                    if (order == null || order.OrderId == -1)
                    {
                        Response.Write(StringHelper.ToJsonString(new
                        {
                            returncode = 58,
                            returnmessage = "Giao dịch không tồn tại"
                        }));
                        return;
                    }
                    else
                    {
                        if (errorCode != 0)
                        {
                            log.Error("MoMoIPN Error Code:" + errorCode);
                            if (errorCode == 49)
                            {
                                order.PaymentStatus = (int)OrderPaymentStatus.NotSuccessful;
                                order.Save();
                            }

                            Response.Write(GeneraorResponseMoMo(order, errorCode, ""));
                            return;
                        }
                        if (partnerCode != MoMopartnerCode || accessKey != MoMoaccessKey)
                        {
                            Response.Write(GeneraorResponseMoMo(order, 59, "Sai PartnerCode hoặc AccessKey"));
                            return;
                        }
                        int oldStatus = order.PaymentStatus;
                        order.PaymentStatus = (int)OrderPaymentStatus.Successful;
                        order.Save();
                        CartHelper.SetOrderSavedSession(order.SiteId, order);
                        OrderHelper.SendMailAfterOrder(order, siteSettings);
                        new OrderLog()
                        {
                            OrderId = order.OrderId,
                            Comment = OrderHelper.GetPaymentStatusResources(oldStatus) + " => " + OrderHelper.GetPaymentStatusResources(order.PaymentStatus),
                            UserEmail = "MOMO",
                            CreatedOn = DateTime.Now,
                            TypeName = ProductResources.SysnOrderPayment
                        }.Save();
                        Response.Write(GeneraorResponseMoMo(order, 0, "Thành công"));
                        return;
                    }
                }
                catch (Exception ex)
                {
                    log.Error("MoMoIPN Error:" + ex.Message);
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        returncode = 99,
                        returnmessage = "Lỗi không xác định (Lỗi hệ thống)"
                    }));
                    return;
                }
            }
        }

        private List<ShoppingCartItem> GetShoppingCartItemFromOrder(Order order)
        {
            var lstOrderItems = OrderItem.GetByOrder(order.OrderId);
            List<ShoppingCartItem> lstShoppingCartItems = new List<ShoppingCartItem>();
            foreach (var item in lstOrderItems)
            {
                lstShoppingCartItems.Add(new ShoppingCartItem()
                {
                    AttributesXml = item.AttributesXml,
                    ProductId = item.ProductId
                });
            }
            return lstShoppingCartItems;
        }

        private string GeneraorResponseMoMo(Order order, int errorCode, string message)
        {
            string requestId = Guid.NewGuid().ToString();
            string strextraData = GeneratorExtraData(order);
            string extraData = string.Empty;
            if (!string.IsNullOrEmpty(strextraData))
                extraData = Convert.ToBase64String(Encoding.UTF8.GetBytes(strextraData));//MoMo yêu cầu Convert Json Order extra Data to Base64
            string orderId = Guid.NewGuid().ToString();
            string orderInfo = order.OrderId.ToString();
            string rawHash = "partnerCode=" +
              MoMopartnerCode + "&accessKey=" +
              MoMoaccessKey + "&requestId=" +
              requestId + "&orderId=" +
              orderId + "&orderInfo=" +
              orderInfo + "&errorCode=" +
              errorCode + "&message=" +
              message + "&responseTime=" +
              DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss") + "&extraData=" +
              extraData;

            string signature = MomoHelper.SignSHA256(rawHash, MomoHelper.secretKey);
            string json = StringHelper.ToJsonString(new
            {
                accessKey = MoMoaccessKey,
                partnerCode = MoMopartnerCode,
                requestType = "captureMoMoWallet",//Sử dụng API captureMoMoWallet để yêu cầu thanh toán bằng Ví MoMo
                requestId,
                orderId,
                errorCode,
                message,
                responseTime = DateTime.Now.ToString("YYYY-MM-DD HH:mm:ss"),
                extraData,
                signature
            });
            return json;
        }

        private string GeneratorExtraData(Order order)
        {
            List<MoMoRequestOrderItem> lstOrderItems = new List<MoMoRequestOrderItem>();
            foreach (var item in GetShoppingCartItemFromOrder(order))
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
            return StringHelper.ToJsonString(new
            {
                full_name = order.BillingFirstName,
                email = order.BillingEmail,
                phone = order.BillingPhone,
                store_code = order.ShippingAddress,
                purchase_units = purchare
            });
        }
    }
}
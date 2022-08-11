using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public partial class ViettelPostWebhookPage : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ViettelPostWebhookPage));
        //Not approved = ["-100"]
        //Approved = ["100" ,"102","103", "104" , "-108"]
        //Sent at convenience store = ["-109", "-110"]
        //Canceled = ["107", "201"]
        //Has taken the goods = ["105"]
        //Being transported = ["200", "202", "300", "320", "400"]
        //{501, 504 ->EDIT = stay the same, opposite -> 202}
        //On delivery = ["500", "506", "570", "508", "509", "550"]
        //Delivery failed =["507"]
        //Approval to return = ["505","502", "515"]
        //Successful delivery destroyed = ["503"]
        //Successful return = ["504"]
        //Wait for approval to return = ["505"]
        //Successful delivery = ["501"]
        private List<int> StatusFail = new List<int>() { 107, 201, 507, 505, 502, 515, 503, 504, 505 };
        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {

            if (Request.HttpMethod == "POST")
            {
                Response.ContentType = "text/json";
                Encoding encoding = new UTF8Encoding();
                Response.ContentEncoding = encoding;
                string body = Request.GetRequestBody();

                if (string.IsNullOrEmpty(body)) return;
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                ViettelPostWebhookModel model = json_serializer.Deserialize<ViettelPostWebhookModel>(body);

                if (model == null || model.DATA == null || model.TOKEN == null) return;

                if (model.TOKEN != ViettelPostHelper.WebhookToken) return;

                if (!int.TryParse(model.DATA.ORDER_REFERENCE, out int orderID)) return;

                Order order = new Order(orderID);

                if (order == null || order.OrderId == -1) return;
                if (order.OrderStatus == (int)OrderStatus.Completed)
                {
                    if (IsDeliveryToCustomers(model.DATA.ORDER_STATUS))
                    {

                        new OrderLog()
                        {
                            OrderId = order.OrderId,
                            Comment = "ViettelPost gửi trạng thái vận chuyển 'Đã giao cho khách' (không cập nhật vì đơn hàng đã hoàn tất)",
                            UserEmail = "ViettelPost",
                            CreatedOn = DateTime.Now,
                            TypeName = ProductResources.SysnOrderShipment
                        }.Save();
                    }
                    else
                    {

                        new OrderLog()
                        {
                            OrderId = order.OrderId,
                            Comment = "ViettelPost gửi trạng thái vận chuyển thất bại (không cập nhật vì đơn hàng đã hoàn tất)",
                            UserEmail = "ViettelPost",
                            CreatedOn = DateTime.Now,
                            TypeName = ProductResources.SysnOrderShipment
                        }.Save();
                    }

                    return;
                }

                if (IsDeliveryToCustomers(model.DATA.ORDER_STATUS))
                {
                    int oldStatus = order.OrderStatus;
                    order.OrderStatus = (int)OrderStatus.DeliveryToCustomers;
                    order.ShippingStatus = (int)ShippingStatus.Sent;
                    order.Save();
                    new OrderLog()
                    {
                        OrderId = order.OrderId,
                        Comment = OrderHelper.GetOrderStatusResources(oldStatus) + " => " + OrderHelper.GetOrderStatusResources((int)OrderStatus.DeliveryToCustomers),
                        UserEmail = "ViettelPost",
                        CreatedOn = DateTime.Now,
                        TypeName = ProductResources.SysnOrderShipment
                    }.Save();
                }
                else if (IsShipmentFail(model.DATA.ORDER_STATUS))
                {
                    int oldStatus = order.OrderStatus;
                    order.OrderStatus = (int)OrderStatus.DeliveryFailed;
                    order.ShippingStatus = (int)ShippingStatus.DeliveryFailed;
                    order.Save();
                    new OrderLog()
                    {
                        OrderId = order.OrderId,
                        Comment = OrderHelper.GetOrderStatusResources(oldStatus) + " => " + OrderHelper.GetOrderStatusResources((int)OrderStatus.DeliveryFailed),
                        UserEmail = "ViettelPost",
                        CreatedOn = DateTime.Now,
                        TypeName = ProductResources.SysnOrderShipment
                    }.Save();
                }

            }
        }
        private bool IsShipmentFail(int shipmentStatus)
        {
            return (StatusFail.Contains(shipmentStatus));
        }
        private bool IsDeliveryToCustomers(int shipmentStatus)
        {
            return (shipmentStatus == 501);
        }
    }
}
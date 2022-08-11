using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class VNPAYCallbackipn : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VNPAYCallbackipn));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.Count > 0)
                {
                    var SecureHash = WebUtils.ParseStringFromQueryString("vnp_SecureHash", "");

                    var vnpayData = Request.QueryString;
                    VnPayLibrary vnpay = new VnPayLibrary();

                    foreach (string s in vnpayData)
                    {
                        //get all querystring data
                        if (!string.IsNullOrEmpty(s) && s.StartsWith("vnp_"))
                        {
                            vnpay.AddResponseData(s, vnpayData[s]);
                        }
                    }
                    var orderID = WebUtils.ParseInt32FromQueryString("vnp_TxnRef", -1);
                    Order order = new Order(orderID);
                    if (VNPAYHelper.EnableConfigByStore && order != null)
                        VNPAYHelper.LoadConfigByStoreCode(order.StoreId);
                    if (vnpay.ValidateSignature(SecureHash))
                    {
                        if (order != null && order.OrderId != -1)
                        {
                            if (decimal.TryParse(WebUtils.ParseStringFromQueryString("vnp_Amount", ""), out decimal vnp_Amount))
                            {
                                if ((vnp_Amount / 100) != order.OrderTotal)
                                {
                                    Response.Write(StringHelper.ToJsonString(new
                                    {
                                        RspCode = "04",
                                        Message = "invalid amount "
                                    }));
                                }
                                else
                                {
                                    if (order.PaymentStatus == (int)OrderPaymentStatus.Successful)
                                        Response.Write(StringHelper.ToJsonString(new
                                        {
                                            RspCode = "02",
                                            Message = "Order already confirmed"
                                        }));
                                    else
                                    {
                                        string returnCode = WebUtils.ParseStringFromQueryString("vnp_ResponseCode", string.Empty);
                                        int oldStatus = order.PaymentStatus;
                                        if (returnCode == "00")
                                        {
                                            order.PaymentStatus = (int)OrderPaymentStatus.Successful;
                                            OrderHelper.SendMailAfterOrder(order, siteSettings);
                                        }
                                        else
                                            order.PaymentStatus = (int)OrderPaymentStatus.NotSuccessful;
                                        order.Save();

                                        CartHelper.SetOrderSavedSession(order.SiteId, order);
                                        new OrderLog() 
                                        {
                                            OrderId = order.OrderId,
                                            Comment = OrderHelper.GetPaymentStatusResources(oldStatus) + " => " + OrderHelper.GetPaymentStatusResources(order.PaymentStatus),
                                            UserEmail = "VNPAY",
                                            CreatedOn = DateTime.Now,
                                            TypeName = ProductResources.SysnOrderPayment
                                        }.Save();
                                        Response.Write(StringHelper.ToJsonString(new
                                        {
                                            RspCode = "00",
                                            Message = "Confirm Success"
                                        }));
                                    }
                                }
                            }
                            else
                            {
                                Response.Write(StringHelper.ToJsonString(new
                                {
                                    RspCode = "04",
                                    Message = "invalid amount "
                                }));
                            }
                        }
                        else
                            Response.Write(StringHelper.ToJsonString(new
                            {
                                RspCode = "01",
                                Message = "Order not found"
                            }));
                    }
                    else
                    {
                        Response.Write(StringHelper.ToJsonString(new
                        {
                            RspCode = "97",
                            Message = "Invalid signature"
                        }));
                    }
                }
                else
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        RspCode = "99",
                        Message = "Input data required"
                    }));
            }
            catch (Exception ex)
            {
                log.Error("VNPay Ipn Error: " + ex.Message);
                Response.Write(StringHelper.ToJsonString(new
                {
                    RspCode = "99",
                    ex.Message
                }));
            }
        }
    }
}
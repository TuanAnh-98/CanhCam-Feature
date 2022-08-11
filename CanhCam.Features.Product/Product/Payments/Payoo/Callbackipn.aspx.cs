using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Text;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class PayooCallbackipn : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayooCallbackipn));

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
                string NotifyMessage = Request.Form.Get("NotifyData");

                PayooConnectionPackage NotifyPackage = GetPayooConnectionPackage(NotifyMessage);

                string strNotifyData = NotifyPackage.Data;
                string outCheckSum = NotifyPackage.Signature;
                string KeyFields = NotifyPackage.KeyFields;

                PaymentNotification invoice = GetPaymentNotify(strNotifyData);

                if (outCheckSum == PayooHelper.GenerateCheckSum(StringBuilder(invoice, KeyFields, PayooHelper.ChecksumKey)))
                {
                    Order order = Order.GetByCode(invoice.OrderNo);
                    if (order != null && order.OrderId != -1)
                    {
                        int oldStatus = order.PaymentStatus;
                        if (invoice.State.Equals("PAYMENT_RECEIVED", StringComparison.OrdinalIgnoreCase))
                        {
                            order.PaymentStatus = (int)OrderPaymentStatus.Successful;
                            OrderHelper.SendMailAfterOrder(order, siteSettings);
                        }
                        else
                            order.PaymentStatus = (int)OrderPaymentStatus.NotSuccessful;
                        new OrderLog()
                        {
                            OrderId = order.OrderId,
                            Comment = OrderHelper.GetPaymentStatusResources(oldStatus) + " => " + OrderHelper.GetPaymentStatusResources(order.PaymentStatus),
                            UserEmail = "Payoo",
                            CreatedOn = DateTime.Now,
                            TypeName = ProductResources.SysnOrderPayment
                        }.Save();

                    }
                    order.Save(); CartHelper.SetOrderSavedSession(order.SiteId, order);
                }
                else
                {
                    Response.Clear();
                    Response.Output.Write("Verified checksum is faillure!");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                log.Error("Payoo Ipn Error: " + ex.Message);

                Response.Clear();
                Response.Output.Write("Verified checksum is faillure!");
                Response.End();
            }
        }

        public PayooConnectionPackage GetPayooConnectionPackage(string strPackage)
        {
            try
            {
                string PackageData = strPackage;
                PayooConnectionPackage objPackage = new PayooConnectionPackage();
                XmlDocument Doc = new XmlDocument
                {
                    XmlResolver = null
                };
                Doc.LoadXml(PackageData);
                objPackage.Data = ReadNodeValue(Doc, "Data");
                objPackage.Signature = ReadNodeValue(Doc, "Signature");
                objPackage.KeyFields = ReadNodeValue(Doc, "KeyFields");
                return objPackage;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public PaymentNotification GetPaymentNotify(string NotifyData)
        {
            try
            {
                string Data = Encoding.UTF8.GetString(Convert.FromBase64String(NotifyData));
                PaymentNotification invoice = new PaymentNotification();
                XmlDocument Doc = new XmlDocument
                {
                    XmlResolver = null
                };
                Doc.LoadXml(Data);
                if (!string.IsNullOrEmpty(ReadNodeValue(Doc, "BillingCode")))
                {
                    // Pay at store
                    if (!string.IsNullOrEmpty(ReadNodeValue(Doc, "ShopId")))
                    {
                        invoice.ShopID = long.Parse(ReadNodeValue(Doc, "ShopId"));
                    }
                    invoice.OrderNo = ReadNodeValue(Doc, "OrderNo");
                    if (!string.IsNullOrEmpty(ReadNodeValue(Doc, "OrderCashAmount")))
                    {
                        invoice.OrderCashAmount = long.Parse(ReadNodeValue(Doc, "OrderCashAmount"));
                    }
                    invoice.State = ReadNodeValue(Doc, "State");
                    invoice.PaymentMethod = ReadNodeValue(Doc, "PaymentMethod");
                    invoice.BillingCode = ReadNodeValue(Doc, "BillingCode");
                    invoice.PaymentExpireDate = ReadNodeValue(Doc, "PaymentExpireDate");
                }
                else
                {
                    invoice.Session = ReadNodeValue(Doc, "session");
                    invoice.BusinessUsername = ReadNodeValue(Doc, "username");
                    invoice.ShopID = long.Parse(ReadNodeValue(Doc, "shop_id"));
                    invoice.ShopTitle = ReadNodeValue(Doc, "shop_title");
                    invoice.ShopDomain = ReadNodeValue(Doc, "shop_domain");
                    invoice.ShopBackUrl = ReadNodeValue(Doc, "shop_back_url");
                    invoice.OrderNo = ReadNodeValue(Doc, "order_no");
                    invoice.OrderCashAmount = long.Parse(ReadNodeValue(Doc, "order_cash_amount"));
                    invoice.StartShippingDate = ReadNodeValue(Doc, "order_ship_date");
                    invoice.ShippingDays = Convert.ToInt32(ReadNodeValue(Doc, "order_ship_days"));
                    invoice.OrderDescription = System.Web.HttpUtility.UrlDecode((ReadNodeValue(Doc, "order_description")));
                    invoice.NotifyUrl = ReadNodeValue(Doc, "notify_url");
                    invoice.State = ReadNodeValue(Doc, "State");
                    invoice.PaymentMethod = ReadNodeValue(Doc, "PaymentMethod");
                    invoice.PaymentExpireDate = ReadNodeValue(Doc, "validity_time");
                }
                return invoice;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string ReadNodeValue(XmlDocument Doc, string TagName)
        {
            XmlNodeList nodeList = Doc.GetElementsByTagName(TagName);
            string NodeValue = null;
            if (nodeList.Count > 0)
            {
                XmlNode node = nodeList.Item(0);
                NodeValue = (node == null) ? "" : node.InnerText;
            }
            return NodeValue == null ? "" : NodeValue;
        }

        public static string StringBuilder(object APIParam, string KeyFields, string strChecksumKey)
        {
            try
            {
                StringBuilder strData = new StringBuilder();
                strData.Append(strChecksumKey);
                string[] arrKeyFields = KeyFields.Split('|');
                foreach (string strKey in arrKeyFields)
                {
                    strData.Append("|" + APIParam.GetType().GetProperty(strKey).GetValue(APIParam, null));
                }
                return strData.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
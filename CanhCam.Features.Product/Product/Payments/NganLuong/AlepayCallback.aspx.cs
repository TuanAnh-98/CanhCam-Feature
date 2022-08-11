using log4net;
using CanhCam.Web.Framework;
using System;
using CanhCam.Business;

namespace CanhCam.Web.ProductUI
{

    public partial class AlepayCallback : CmsInitBasePage
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(AlepayCallback));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion

        private void Page_Load(object sender, EventArgs e)
        {

            var order = (Order)null;
            var cancel = WebUtils.ParseInt32FromQueryString("cancel", 0);
            if (cancel == 1)
            {
                var orderId = WebUtils.ParseInt32FromQueryString("id", -1);
                order = new Order(orderId);
                if (order == null || order.OrderId <= 0)
                {
                    SiteUtils.RedirectToHomepage();
                    return;
                }

                order.OrderNote = "Bạn đã hủy giao dịch thanh toán!";
            }
            else
            {
                order = AlepayHelper.ProcessCallbackData();
                if (order == null)
                {
                    SiteUtils.RedirectToHomepage();
                    return;
                }
            }

            CartHelper.SetOrderSavedSession(order.SiteId, order);
            // OnlinePaymentHelper.ProcessAfterPayment(siteSettings, order);

            OrderHelper.SendMailAfterOrder(order, siteSettings);
            var pageUrl = SiteUtils.GetZoneUrl(AlepayHelper.CompletedZoneId);
            if (!string.IsNullOrEmpty(pageUrl))
                WebUtils.SetupRedirect(this, pageUrl);
            else
                SiteUtils.RedirectToHomepage();
        }

    }
}
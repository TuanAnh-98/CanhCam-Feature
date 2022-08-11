using CanhCam.Web.Framework;
using log4net;
using System;
using System.Threading;

namespace CanhCam.Web.ProductUI
{
    public partial class OnePAYCallback1 : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OnePAYCallback1));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            Thread.Sleep(2000);
            string code = "";
            string mess = "";

            var order = OnePayHelper.ProcessCallbackData(ref code, ref mess);
            if (order == null)
            {
                SiteUtils.RedirectToHomepage();
                return;
            }

            CartHelper.SetOrderSavedSession(order.SiteId, order);
            var pageUrl = SiteUtils.GetZoneUrl(OnePayHelper.CompletedZoneId);
            if (!string.IsNullOrEmpty(pageUrl))
                WebUtils.SetupRedirect(this, pageUrl);
            else
                SiteUtils.RedirectToHomepage();
        }
    }
}
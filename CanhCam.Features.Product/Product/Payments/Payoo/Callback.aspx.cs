using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class PayooCallback : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PayooCallback));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString.Count > 0)
            {
                try
                {

                    var orderNo = WebUtils.ParseStringFromQueryString("order_no", "");
                    var session = WebUtils.ParseStringFromQueryString("session", "");
                    var status = WebUtils.ParseStringFromQueryString("status", "");
                    var checkSumRequest = WebUtils.ParseStringFromQueryString("checksum", "");
                    string formatChecksum = "{0}{1}.{2}.{3}";
                    if (PayooHelper.VerifyCheckSum(string.Format(formatChecksum, PayooHelper.ChecksumKey, session, orderNo, status), checkSumRequest))
                    {
                        Order order = Order.GetByCode(orderNo);
                        if (order != null)
                        {
                            var pageUrl = SiteUtils.GetZoneUrl(PayooHelper.CompletedZoneId);
                            if (!string.IsNullOrEmpty(pageUrl))
                                WebUtils.SetupRedirect(this, pageUrl);

                        }
                        else
                            SiteUtils.RedirectToHomepage(); //Order Null
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error Callback Payoo :" + ex.Message);
                }
                SiteUtils.RedirectToHomepage(); //Order Null
            }
        }
    }
}
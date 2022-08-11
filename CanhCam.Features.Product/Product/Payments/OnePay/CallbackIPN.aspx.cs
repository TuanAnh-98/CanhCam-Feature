using CanhCam.Web.Framework;
using log4net;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class OnePAYCallback2 : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OnePAYCallback2));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            string code = "1";
            string mess = "confirm-success";
            try
            {
                OnePayHelper.ProcessCallbackData( ref code, ref mess, true);//Fix OnePay v2 Grove not use global
            }
            catch (Exception ex)
            {
                code = "0";
                mess = "invalid-hash";
            }
            Response.Write("responsecode=" + code + "&desc=" + mess);
            return;
        }
    }
}
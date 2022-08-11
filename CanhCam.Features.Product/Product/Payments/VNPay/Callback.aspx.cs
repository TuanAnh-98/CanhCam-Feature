using CanhCam.Business;
using CanhCam.Web.Framework;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class VNPAYCallback : CmsInitBasePage
    {
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
                    if (orderID != -1)
                    {
                        if (order != null)
                        {

                            System.Threading.Thread.Sleep(1000);
                            var pageUrl = SiteUtils.GetZoneUrl(VNPAYHelper.CompletedZoneId);
                            if (!string.IsNullOrEmpty(pageUrl))
                                WebUtils.SetupRedirect(this, pageUrl);
                        }
                        else
                            SiteUtils.RedirectToHomepage(); //Order Null
                    }
                    else
                        SiteUtils.RedirectToHomepage(); //Order Null
                }
                else
                    SiteUtils.RedirectToHomepage(); //Order Null
            }
        }
    }
}
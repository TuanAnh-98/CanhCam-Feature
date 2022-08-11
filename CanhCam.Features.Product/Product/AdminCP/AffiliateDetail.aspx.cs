using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class AffiliateDetail : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateDetail));
        private SiteUser siteUser = null;
        #endregion

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnSubmit.Click += BtnSubmit_Click;
            
        }


        #endregion

        #region Events
        private void BtnSubmit_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadParam();
            if (!Page.IsPostBack)
                PopulateControl();
        }
        #endregion

        #region LoadParam
        private void LoadParam()
        {
            hlnkPayment.NavigateUrl = SiteRoot + "/affiliate-form";
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            var user = SiteUtils.GetCurrentSiteUser();
            if (user.UserId > 0)
            {
                siteUser = new SiteUser(siteSettings, user.UserId);
            }
        }
        #endregion

        #region Populate
        private void PopulateControl()
        {
            slbTotalCommission.Text = "0" + " đ";
            slbCommissionPay.Text = "0" + " đ";
            slbCommissionWait.Text = "0" + " đ";
            slbTotalOrder.Text = "0" + " đ";
            if (siteUser != null)
            {
                decimal orderTotal = 0;
                decimal expectedAmount = 0;
                int userid = SiteUtils.GetCurrentSiteUser().UserId;
                var orders = Affiliate.GetPage(1, 32727, out _, userid);
                foreach (var order in orders)
                {
                    log.Info(order.OrderStatus);
                    if(order.OrderStatus == 99)
                    {
                        orderTotal += (order.Price * order.Quantity * Convert.ToDecimal("0,3"));
                    }
                    if(order.OrderStatus == 5 
                        || order.OrderStatus == 10
                        || order.OrderStatus == 15
                        || order.OrderStatus == 20
                        || order.OrderStatus == 25
                        || order.OrderStatus == 0)
                    {
                        expectedAmount += (order.Price * order.Quantity * Convert.ToDecimal("0,3"));
                    }
                }
                var affiliatePayment = AffiliatePayment.GetPage(1, 32727, out _, userid);
                foreach(var payment in affiliatePayment)
                {
                    if(payment.Status == true)
                    {
                        orderTotal -= payment.WithdrawMoney;
                    }
                }
                slbTotalCommission.Text = ProductHelper.FormatPrice(orderTotal) + " đ";
                slbCommissionWait.Text = ProductHelper.FormatPrice(expectedAmount) + " đ";

            }
        }
        #endregion
    }
}
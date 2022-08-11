using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.AffiliateUI
{
    public partial class AffiliateDetail : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateDetail));
        private int affiID = -1;
        private AffiliateUser affiUser = null;
        private TimeZoneInfo timeZone = null;
        private double timeOffset = 0;
        #endregion

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //this.EnableViewState = true;
            //this.Page.EnableViewState = true;
            this.Load += new EventHandler(this.Page_Load);
            this.btnSubmit.Click += BtnSubmit_Click;
            //this.btnFileUp.Click += BtnFileUp_Click;
        }
        #endregion

        #region Events
        //private void BtnFileUp_Click(object sender, EventArgs e)
        //{
        //    if (FileUpload1.HasFile)
        //    {
        //        string fileExt =
        //           System.IO.Path.GetExtension(FileUpload1.FileName);

        //        if (fileExt == ".jpeg" || fileExt == ".jpg" || fileExt == ".png" || fileExt == ".gif")
        //        {
        //            //do what you want with this image
        //        }
        //        else
        //        {
        //            Label1.Text = "Only .jpeg, .jpg, .png, .gif files are allowed!";
        //        }
        //    }
        //}

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
            var user = SiteUtils.GetCurrentSiteUser();
            if (user.UserId > 0)
            {
                affiUser = new AffiliateUser(user.UserId);
            }
        }
        #endregion

        #region Populate
        private void PopulateControl()
        {
            slbTotalCommission.Text = "0" + " đ";
            slbCommissionPay.Text = "0" + " đ";
            slbCommissionWait.Text = "0" + " đ";
            slbTotalOrder.Text = "0";
            if (affiUser != null)
            {
                slbTotalCommission.Text = affiUser.TotalCommission.ToString() + " đ";
                slbCommissionPay.Text = affiUser.CommissionPay.ToString() + " đ";
                slbCommissionWait.Text = affiUser.CommissionWait.ToString() + " đ";
                slbTotalOrder.Text = affiUser.TotalOrder.ToString();
                
            }
        }
        #endregion
    }
}
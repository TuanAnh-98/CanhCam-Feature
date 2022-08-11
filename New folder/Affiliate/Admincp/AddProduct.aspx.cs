using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Editor;
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
    public partial class AddProduct : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AddProduct));
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
            SiteUtils.SetupEditor(edBriefContent, AllowSkinOverride, Page);
            SiteUtils.SetupEditor(edFullContent, AllowSkinOverride, Page);
            //this.btnFileUp.Click += BtnFileUp_Click;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                AffiliateProduct pd = new AffiliateProduct();
                pd.SiteId = siteSettings.SiteId;
                pd.Code = txtCode.Text;
                pd.Price = Convert.ToDecimal(txtPrice.Text);
                pd.OldPrice = Convert.ToDecimal(txtOldPrice.Text);
                pd.StockQuantity = Convert.ToInt32(txtQuantity.Text);
                pd.Weight = Convert.ToDecimal(txtWeight.Text);
                pd.Title = txtName.Text;
                var friendlyUrlString = SiteUtils.SuggestFriendlyUrl(pd.Title, siteSettings);
                if (friendlyUrlString.EndsWith("/"))
                    friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
                pd.Url = "~/" + friendlyUrlString;
                pd.BriefContent = txtBriefContent.Text;
                pd.FullContent = txtFullContent.Text;
                pd.FileAttachment = fileUpload1.UploadedFiles[0].FileName;
                pd.OpenInNewWindow = chbOpenNewWindow.Checked;
                pd.DisableBuyButton = chbDisableBuyButton.Checked;
                pd.IsPublished = chbIsPublished.Checked;
                pd.UserGuid = siteSettings.CurrencyGuid;
                pd.LastModUserGuid = SiteUtils.GetCurrentSiteUser().UserGuid;
                if (dpStartDate.Text.Trim().Length > 0)
                {
                    DateTime localTime = DateTime.Parse(dpStartDate.Text);
                    localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);
                    if (timeZone != null)
                        pd.StartDate = localTime.ToUtc(timeZone);
                    else
                        pd.StartDate = localTime.AddHours(-timeOffset);
                }
                pd.Save();
                message.SuccessMessage = "Thêm mới thành công";
            }
            catch
            {
                message.ErrorMessage = "Thêm mới thất bại";
            }
        }
        #endregion

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            LoadParam();
            if (!Page.IsPostBack)
                PopulateControl();
        }
        #endregion

        #region LoadParam
        private void LoadParam()
        {
            
        }
        #endregion

        #region Populate
        private void PopulateControl()
        {
            edBriefContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edBriefContent.WebEditor.Height = Unit.Pixel(300);
            edFullContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edFullContent.WebEditor.Height = Unit.Pixel(300);
        }
        #endregion

        #region LoadSettings
        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
        }
        #endregion
    }
}
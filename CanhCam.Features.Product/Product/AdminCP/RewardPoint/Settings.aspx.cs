using log4net;
using Resources;
using System;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class SettingsPage : CmsNonBasePage
    {
        private const string pointStringFormat = "{0}/{1}";
        private static readonly ILog log = LogManager.GetLogger(typeof(SettingsPage));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.RewardPointSettingsTitle);
            heading.Text = ProductResources.RewardPointSettingsTitle;
        }

        private void PopulateControls()
        {
            var isEnable = siteSettings.GetExpandoProperty("EnableRewardPoints");
            if (!string.IsNullOrEmpty(isEnable) && isEnable.ToLower() == "true")
                chkRewardPoint.Checked = true;

            var rewardPoint = siteSettings.GetExpandoProperty("SpendYGetXRewardPoints");
            if (!string.IsNullOrEmpty(rewardPoint))
            {
                var txtRewardPoint = rewardPoint.Split('/');
                txtFromAmount.Text = txtRewardPoint[0];
                txtToPoint.Text = txtRewardPoint[1];
            }

            var exchangeRate = siteSettings.GetExpandoProperty("PointCurrencyExchangeRate");
            if (!string.IsNullOrEmpty(exchangeRate))
            {
                var txtExchangeRate = exchangeRate.Split('/');
                txtFromPoint.Text = txtExchangeRate[0];
                txtForAmount.Text = txtExchangeRate[1];
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            siteSettings.SetExpandoProperty("EnableRewardPoints", chkRewardPoint.Checked.ToString().ToLower());
            if (Int32.TryParse(txtFromAmount.Text, out int fromAmount) && Int32.TryParse(txtToPoint.Text, out int toPoint) && fromAmount > 0 && toPoint > 0)
                siteSettings.SetExpandoProperty("SpendYGetXRewardPoints", string.Format(pointStringFormat, txtFromAmount.Text, txtToPoint.Text));
            if (Int32.TryParse(txtFromPoint.Text, out int fromPoint) && Int32.TryParse(txtForAmount.Text, out int forAmount) && fromPoint > 0 && forAmount > 0)
                siteSettings.SetExpandoProperty("PointCurrencyExchangeRate", string.Format(pointStringFormat, txtFromPoint.Text, txtForAmount.Text));

            siteSettings.SaveExpandoProperties();

            message.SuccessMessage = "Cập nhật thành công";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
        }
    }
}
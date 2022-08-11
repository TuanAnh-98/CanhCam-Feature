using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using System;
using System.Linq;

namespace CanhCam.Web.AccountUI
{
    public partial class DashboardPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DashboardPage));
        private SiteUser siteUser;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();
            PopulateLabels();

            if (siteUser == null || siteUser.SiteId <= 0)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (!IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            litFullName.Text = AccountHelper.GetFullNameFormat(siteUser);
            litEmail.Text = siteUser.Email;
            RewardPointsHelper.GetRewardPointsBalance(siteUser, out decimal rewardPointsBalance, out decimal pointsAvallable);
            litCurrentPoints.Text = string.Format("Bạn có <strong>{0}</strong> điểm thưởng.", pointsAvallable);

            rptAddress.DataSource = UserAddress.GetByUser(siteUser.UserId).Take(2); 
            rptAddress.DataBind();

            var doc = ProductHelper.BuildPurchaseHistoryXml(SiteId, siteUser.UserGuid, 1, 5);
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("Product", "PurchaseHistory.xslt"), doc);
        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, litHeading.Text);
            lnkProfileEdit.NavigateUrl = SiteRoot + "/Account/UserProfile.aspx";
            lnkAllOrders.NavigateUrl = SiteRoot + "/Product/PurchaseHistory.aspx";
            lnkAddress.NavigateUrl = SiteRoot + "/Account/Address.aspx";
        }
         
        protected string FormatAddress(string address, string provinceGuid, string districtGuid, string wardGuid)
        {
            return address;
        }
    }
}
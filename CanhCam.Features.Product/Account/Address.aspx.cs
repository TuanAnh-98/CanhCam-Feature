using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Web.UI.WebControls;

namespace CanhCam.Web.AccountUI
{
    public partial class AddressPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AddressPage));
        private SiteUser siteUser;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.rptAddress.ItemDataBound += new RepeaterItemEventHandler(rptAddress_ItemDataBound);
            this.rptAddress.ItemCommand += new RepeaterCommandEventHandler(rptAddress_ItemCommand);
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
            rptAddress.DataSource = UserAddress.GetByUser(siteUser.UserId);
            rptAddress.DataBind();
        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser != null && siteUser.UserId > 0)
                lnkInsert.Visible = AccountHelper.IsAllowedAddNewAddress(siteUser.UserId);
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, litHeading.Text);
            lnkInsert.NavigateUrl = SiteRoot + "/Account/AddressEdit.aspx";
        }

        private void rptAddress_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hdfIsDefault = (HiddenField)e.Item.FindControl("hdfIsDefault");
                var hplDeleteAddress = (LinkButton)e.Item.FindControl("hplDeleteAddress");
                UIHelper.AddConfirmationDialog(hplDeleteAddress, "Bạn có thực sự muốn xóa?");

                hplDeleteAddress.Visible = !Convert.ToBoolean(hdfIsDefault.Value);
            }
        }

        private void rptAddress_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "DeleteAddress":
                    var addressId = Convert.ToInt32(e.CommandArgument);
                    var address = new UserAddress(addressId);
                    if (address != null && address.AddressId > 0 && address.UserId == siteUser.UserId)
                        UserAddress.Delete(address.AddressId);

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                    break;
            }
        }

        protected string FormatAddress(string address, string provinceGuid, string districtGuid, string wardGuid)
        {
            return address;
        }
    }
}
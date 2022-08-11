using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.AccountUI
{
    public partial class AddressEditPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AddressEditPage));
        private SiteUser siteUser;
        private int addressId = -1;
        private UserAddress address;
        private List<UserAddress> lstAddress = new List<UserAddress>();

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.ddlProvince.SelectedIndexChanged += new EventHandler(ddlProvince_SelectedIndexChanged);
            this.ddlDistrict.SelectedIndexChanged += new EventHandler(ddlDistrict_SelectedIndexChanged);
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

        private void ddlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindList(ddlDistrict, ddlProvince.SelectedValue, ProductResources.CheckoutAddressDistrict);
            BindList(ddlWard, null, ProductResources.CheckoutAddressWard);
        }

        private void ddlDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindList(ddlWard, ddlDistrict.SelectedValue, ProductResources.CheckoutAddressWard);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (address == null)
            {
                if (!AccountHelper.IsAllowedAddNewAddress(siteUser.UserId))
                {
                    lblError.Text = "Sổ địa chỉ của bạn hiện tại đã quá nhiều.";
                    return;
                }

                address = new UserAddress
                {
                    UserId = siteUser.UserId
                };
            }

            try
            {
                address.FirstName = txtFirstName.Text.Trim();
                address.Company = txtCompany.Text.Trim();
                address.Phone = txtPhone.Text.Trim();
                address.Address = txtAddress.Text.Trim();

                bool updateAddress = false;
                if (lstAddress.Count == 0)
                    address.IsDefault = true;
                else
                {
                    if (chkIsDefault.Checked)
                        updateAddress = true;
                }

                if (ddlProvince.SelectedValue.Length == 36)
                    address.ProvinceGuid = new Guid(ddlProvince.SelectedValue);
                else
                    address.ProvinceGuid = Guid.Empty;

                if (ddlDistrict.SelectedValue.Length == 36)
                    address.DistrictGuid = new Guid(ddlDistrict.SelectedValue);
                else
                    address.DistrictGuid = Guid.Empty;

                if (ddlWard.SelectedValue.Length == 36)
                    address.WardGuid = new Guid(ddlWard.SelectedValue);
                else
                    address.WardGuid = Guid.Empty;

                if (address.Save() && updateAddress)
                    UserAddress.UpdateDefaultAddress(siteUser.UserId, address.AddressId);
                message.SuccessMessage = "Cập nhật thành công";
                WebUtils.SetupRedirect(this, SiteRoot + "/Account/AddressEdit.aspx?id=" + address.AddressId.ToString());
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void PopulateControls()
        {
            BindProvince();
            BindList(ddlDistrict, null, ProductResources.CheckoutAddressDistrict);
            BindList(ddlWard, null, ProductResources.CheckoutAddressWard);

            if (address != null)
            {
                txtFirstName.Text = address.FirstName;
                txtCompany.Text = address.Company;
                txtPhone.Text = address.Phone;
                txtAddress.Text = address.Address;
                chkIsDefault.Checked = address.IsDefault;

                var li = ddlProvince.Items.FindByValue(address.ProvinceGuid.ToString());
                if (li != null)
                {
                    ddlProvince.ClearSelection();
                    li.Selected = true;

                    BindList(ddlDistrict, li.Value, ProductResources.CheckoutAddressDistrict);
                }

                li = ddlDistrict.Items.FindByValue(address.DistrictGuid.ToString());
                if (li != null)
                {
                    ddlDistrict.ClearSelection();
                    li.Selected = true;

                    BindList(ddlWard, li.Value, ProductResources.CheckoutAddressWard);
                }

                li = ddlWard.Items.FindByValue(address.WardGuid.ToString());
                if (li != null)
                {
                    ddlWard.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        private void BindProvince()
        {
            var lst = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid, 1, WorkingCulture.LanguageId);
            ddlProvince.Items.Clear();
            ddlProvince.Items.Add(new ListItem(ProductResources.CheckoutAddressProvince, string.Empty));
            ddlProvince.DataSource = lst;
            ddlProvince.DataBind();
        }

        private void BindList(DropDownList ddl, string parentGuid, string emptyText)
        {
            var lst = GeoZone.GetByListParent(parentGuid, 1, WorkingCulture.LanguageId);
            ddl.Items.Clear();
            ddl.Items.Add(new ListItem(emptyText, string.Empty));
            if (!string.IsNullOrEmpty(parentGuid))
            {
                ddl.DataSource = lst;
                ddl.DataBind();
            }
        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null)
                return;

            lstAddress = UserAddress.GetByUser(siteUser.UserId);
            addressId = WebUtils.ParseInt32FromQueryString("id", addressId);
            if (addressId > 0)
            {
                address = lstAddress.Where(s => s.AddressId == addressId).FirstOrDefault();
                if (address == null || address.AddressId <= 0 || address.UserId != siteUser.UserId)
                    address = null;
            }

            chkIsDefault.Visible = false;
            if ((address == null || !address.IsDefault) && lstAddress.Count > 0)
                chkIsDefault.Visible = true;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, litHeading.Text);
        }
    }
}
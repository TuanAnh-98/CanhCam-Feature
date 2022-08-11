using CanhCam.Business;
using CanhCam.Web.UI;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.AccountUI
{
    public partial class ProvinceDistrictSetting : UserControl, ISettingControl
    {
        protected List<GeoZone> tblProvinceList = new List<GeoZone>();
        private string setProvince = string.Empty;
        private string setDistrict = string.Empty;
        private string setWard = string.Empty;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            EnsureControls();
            ddProvince.SelectedIndexChanged += new EventHandler(ddProvince_SelectedIndexChanged);
            ddDistrict.SelectedIndexChanged += new EventHandler(ddDistrict_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            if (!Page.IsPostBack)
            {
                BindProvinceList();
                BindDistrictList();
                BindWardList();
            }
        }

        private void BindProvinceList()
        {
            if (tblProvinceList.Count == 0)
            {
                Guid defaultCountryGuid = new Guid("99f791e7-7343-42e8-8c19-3c41068b5f8d");
                tblProvinceList = GeoZone.GetByCountry(defaultCountryGuid, WorkingCulture.LanguageId);
            }

            if (tblProvinceList.Count > 0)
            {
                ddProvince.Items.Clear();
                ddProvince.Items.Add(new ListItem(Resources.ProductResources.CheckoutAddressProvince, string.Empty));
                ddProvince.DataSource = tblProvinceList;
                ddProvince.DataBind();
            }

            ListItem item = null;

            if (setProvince.Length > 0)
            {
                item = ddProvince.Items.FindByValue(setProvince);
            }

            if (item != null)
            {
                ddProvince.ClearSelection();
                item.Selected = true;
            }
        }

        private void BindDistrictList()
        {
            ddDistrict.Items.Clear();
            ddDistrict.Items.Add(new ListItem(Resources.ProductResources.CheckoutAddressDistrict, string.Empty));

            if ((ddProvince.SelectedIndex > -1)
                && (ddProvince.SelectedValue.Length > 0)
                )
            {
                if (!string.IsNullOrEmpty(ddProvince.SelectedValue))
                {
                    ddDistrict.DataSource = GeoZone.GetByListParent(ddProvince.SelectedValue, WorkingCulture.LanguageId);
                    ddDistrict.DataBind();

                    if (setDistrict.Length > 0)
                    {
                        ListItem item = ddDistrict.Items.FindByValue(setDistrict);
                        if (item != null)
                        {
                            ddDistrict.ClearSelection();
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        private void BindWardList()
        {
            ddWard.Items.Clear();
            ddWard.Items.Add(new ListItem(Resources.ProductResources.CheckoutAddressWard, string.Empty));

            if ((ddDistrict.SelectedIndex > -1)
                && (ddDistrict.SelectedValue.Length > 0)
                )
            {
                if (!string.IsNullOrEmpty(ddDistrict.SelectedValue))
                {
                    ddWard.DataSource = GeoZone.GetByListParent(ddDistrict.SelectedValue, WorkingCulture.LanguageId);
                    ddWard.DataBind();

                    if (setWard.Length > 0)
                    {
                        ListItem item = ddWard.Items.FindByValue(setWard);
                        if (item != null)
                        {
                            ddWard.ClearSelection();
                            item.Selected = true;
                        }
                    }
                }
            }
        }

        private void ddProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrictList();
            UpdatePanel1.Update();
        }

        private void ddDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindWardList();
            UpdatePanel1.Update();
        }

        private void EnsureControls()
        {
            if (ddProvince == null)
            {
                ddProvince = new DropDownList
                {
                    DataValueField = "Guid",
                    DataTextField = "Name"
                };

                if (this.Controls.Count == 0) { this.Controls.Add(ddProvince); }
            }

            if (ddDistrict == null)
            {
                ddDistrict = new DropDownList
                {
                    DataValueField = "Guid",
                    DataTextField = "Name"
                };

                if (this.Controls.Count == 1) { this.Controls.Add(ddDistrict); }
            }

            if (ddWard == null)
            {
                ddWard = new DropDownList
                {
                    DataValueField = "Guid",
                    DataTextField = "Name"
                };

                if (this.Controls.Count == 1) { this.Controls.Add(ddWard); }
            }
        }

        #region ISettingControl

        public string GetValue()
        {
            return ddProvince.SelectedValue + "|" + ddDistrict.SelectedValue + "|" + ddWard.SelectedValue;
        }

        /// <summary>
        /// Expects pipe separated pair Country|State like US|TN
        /// </summary>
        /// <param name="val"></param>
        public void SetValue(string val)
        {
            string[] args = val.Split('|');
            if (args.Length < 3) { return; }

            setProvince = args[0];
            setDistrict = args[1];
            setWard = args[2];

            BindProvinceList();

            ListItem item = ddProvince.Items.FindByValue(setProvince);
            if (item != null)
            {
                ddProvince.ClearSelection();
                item.Selected = true;
            }

            BindDistrictList();
            item = ddDistrict.Items.FindByValue(setDistrict);
            if (item != null)
            {
                ddDistrict.ClearSelection();
                item.Selected = true;
            }

            BindWardList();
            item = ddWard.Items.FindByValue(setWard);
            if (item != null)
            {
                ddWard.ClearSelection();
                item.Selected = true;
            }

            UpdatePanel1.Update();
        }

        #endregion ISettingControl
    }
}
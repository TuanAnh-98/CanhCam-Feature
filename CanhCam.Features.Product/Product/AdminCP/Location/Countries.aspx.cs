using CanhCam.Business;
using CanhCam.Web.Framework;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class CountriesPage : CmsNonBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            PopulateLabels();
            if ((!Page.IsPostBack) && (!Page.IsCallback))
            {
                PopulateControls();
            }
        }

        private void PopulateControls()
        {
            ddlPovince.DataSource = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            ddlPovince.DataTextField = "Name";
            ddlPovince.DataValueField = "Guid";
            ddlPovince.Items.Insert(0, new ListItem() { Value = "", Text = "None" });
            ddlPovince.DataBind();
        }

        private void PopulateLabels()
        {
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.btnUpdateLocation.Click += BtnUpdate_Click;
            this.ddlPovince.SelectedIndexChanged += DdlPovince_SelectedIndexChanged;
        }

        private void DdlPovince_SelectedIndexChanged(object sender, EventArgs e) 
        {
            grid.Rebind();
        }

        #endregion OnInit

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            foreach (GridDataItem item in grid.Items)
            {
                int itemID = Convert.ToInt32(item.GetDataKeyValue("ItemID"));
                Guid itemGuid = new Guid(item.GetDataKeyValue("Guid").ToString());

                TextBox txtApiCode = (TextBox)item.FindControl("txtApiCode");

                GeoZone geo = new GeoZone(itemGuid);
                if (txtApiCode != null && geo.ApiCode != txtApiCode.Text)
                {
                    geo.ApiCode = txtApiCode.Text;
                    geo.Save();
                }
            }
            message.SuccessMessage = "Cập nhật thành công";
            grid.Rebind();
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.AllowPaging = false;
            string provinceVal = ddlPovince.SelectedValue;
            if (string.IsNullOrEmpty(provinceVal))
                grid.DataSource = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            else
                grid.DataSource = GeoZone.GetByListParent(provinceVal);
        }
    }
}
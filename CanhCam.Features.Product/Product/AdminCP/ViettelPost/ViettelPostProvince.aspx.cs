using System;
using CanhCam.Web.Framework;
using Resources;
using log4net;
using Telerik.Web.UI;
using CanhCam.Business;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Linq;
using CanhCam.Data;
using System.Data;

namespace CanhCam.Web.ProductUI
{
    public partial class ViettelPostProvincePage : CmsNonBasePage
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
        void PopulateControls()
        {

        }

        private void PopulateLabels()
        {
            if (ViettelPostProvince.GetCount() > 0)
                btnUpdate.Enabled = false;
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnUpdate.Click += BtnUpdate_Click;
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.btnUpdateLocation.Click += BtnUpdateLocation_Click;
        }

        private void BtnUpdateLocation_Click(object sender, EventArgs e)
        {

            bool updated = false;
            foreach (GridDataItem data in grid.Items)
            {
                TextBox txtCode = (TextBox)data.FindControl("txtProductCode");
                if (txtCode != null)
                {
                    int rowId = Convert.ToInt32(data.GetDataKeyValue("RowID"));
                    string oldCode = data.GetDataKeyValue("ViettelPostProvinceCode").ToString();
                    string newCode = txtCode.Text;
                    if (oldCode != newCode)
                    {
                        ViettelPostProvince viettel = new ViettelPostProvince(rowId);
                        if (viettel != null && viettel.RowID > 0)
                        {
                            viettel.ViettelPostProvinceCode = newCode;
                            if (viettel.Save())
                                updated = true;
                        }
                    }
                }
            }
            if (updated)
            {
                message.SuccessMessage = "Cập nhật thành công";
                grid.Rebind();
            }
        }
        #endregion
        protected string GetName(Guid ProvinceGuid)
        {
            GeoZone geo = new GeoZone(ProvinceGuid);
            if (geo != null)
                return geo.Name;
            else
                return string.Empty;
        }
        private void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            if (ViettelPostProvince.GetCount() == 0)
            {
                List<GeoZone> lstGeoZone = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid, 1);
                foreach (GeoZone item in lstGeoZone)
                {
                    ViettelPostProvince viettel = new ViettelPostProvince()
                    {
                        ProvinceGuid = item.Guid,
                        ViettelPostProvinceCode = string.Empty
                    };
                    viettel.Save();
                }
                message.SuccessMessage = "Đồng bộ thành công";
                grid.Rebind();
            }
        }
        void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.AllowPaging = false;
            grid.DataSource = ViettelPostProvince.GetAll();

        }
    }
}
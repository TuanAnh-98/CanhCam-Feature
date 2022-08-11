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
using System.Net;
using System.Web.Script.Serialization;

namespace CanhCam.Web.ProductUI
{
    public partial class ShippingLocation : CmsNonBasePage
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

            ddlProvince.DataSource = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid, 1, WorkingCulture.LanguageId);
            ddlProvince.DataBind();
        }

        private void PopulateLabels()
        {
            if (ViettelPostCode.GetCount() > 0)
                btnUpdate.Enabled = false;

            if (ddlProvince.SelectedValue.Length == 36)
            {
                Guid provinceguid = new Guid(ddlProvince.SelectedValue);
                ViettelPostProvince province = ViettelPostProvince.GetAll().Where(p => p.ProvinceGuid == provinceguid).FirstOrDefault();
                if (province != null)
                {
                    linkGetCode.NavigateUrl = @"https://api.viettelpost.vn/api/setting/listdistrictbyprovince/" + province.ViettelPostProvinceCode;
                    linkGetCode.Text = "Get Code";
                }
                else
                {
                    linkGetCode.NavigateUrl = @"/Product/AdminCP/Shipping/ViettelPostProvince.aspx";

                    linkGetCode.Text = "Chưa cấu hình tỉnh thành VittelCode";
                }
            }
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnUpdate.Click += BtnUpdate_Click;
            ddlProvince.SelectedIndexChanged += DdlProvince_SelectedIndexChanged;
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.btnUpdateLocation.Click += BtnUpdateLocation_Click;
            this.btnSyns.Click += BtnSyns_Click;
        }

        private void BtnSyns_Click(object sender, EventArgs e)
        {


            var lst = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid, 1, WorkingCulture.LanguageId);
            var lstProvince = ViettelPostProvince.GetAll();
            foreach (var item in lst)
            {
                ViettelPostProvince province = lstProvince.Where(p => p.ProvinceGuid == item.Guid).FirstOrDefault();
                if (province != null)
                {
                    var url = @"https://api.viettelpost.vn/api/setting/listdistrictbyprovince/" + province.ViettelPostProvinceCode;

                    var client = new WebClient();
                    var result = client.DownloadString(url);
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    var lstojb = js.Deserialize<List<DISTRICTVittelPost>>(result);

                    var lstCode = ViettelPostCode.GetAllByProvince(province.ProvinceGuid);
                    foreach (DISTRICTVittelPost it in lstojb)
                    {
                        
                        var viettelPostCode = lstCode.Where(c => c.ViettelPostCode2 == it.DISTRICT_VALUE).FirstOrDefault();
                        if (viettelPostCode != null)
                        {
                            viettelPostCode.ViettelPostCode2 = it.DISTRICT_ID.ToString();
                            viettelPostCode.Save();
                        }
                    }

                }

            }


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
                    string oldCode = data.GetDataKeyValue("ViettelPostCode2").ToString();
                    string newCode = txtCode.Text;
                    if (oldCode != newCode)
                    {
                        ViettelPostCode viettel = new ViettelPostCode(rowId);
                        if (viettel != null && viettel.RowID > 0)
                        {
                            viettel.ViettelPostCode2 = newCode;
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
        private void DdlProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            List<GeoZone> lstGeoZone = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            foreach (GeoZone item in lstGeoZone)
            {
                ViettelPostCode viettel = new ViettelPostCode()
                {
                    GeoZoneCode = item.Code,
                    GeoZoneName = item.Name,
                    ViettelPostCode2 = string.Empty
                };
                viettel.Save();
            }

        }
        void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.AllowPaging = false;

            if (ddlProvince.SelectedValue.Length == 36)
                grid.DataSource = ViettelPostCode.GetAllByProvince(new Guid(ddlProvince.SelectedValue));
            else
                grid.DataSource = ViettelPostCode.GetAllProvince(siteSettings.DefaultCountryGuid);

        }
    }


    public class DISTRICTVittelPost
    {
        public int DISTRICT_ID { get; set; }
        public string DISTRICT_VALUE { get; set; }
        public string DISTRICT_NAME { get; set; }
        public int PROVINCE_ID { get; set; }

    }
}
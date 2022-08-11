using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.StoreUI
{
    public partial class StorePriorityPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StorePriorityPage));

        private List<Store> lstAllStores = new List<Store>();
        private List<GeoZone> lstProvinces = new List<GeoZone>();
        private List<GeoZone> lstDistricts = new List<GeoZone>();
        private Guid selectGuid = Guid.Empty;

        private bool enabledStore = false;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            grid.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(grid_ItemDataBound);

            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            ddlArea.SelectedIndexChanged += new EventHandler(ddlArea_SelectedIndexChanged);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();
            //PopulateLabels();

            if (!enabledStore)
            {
                SiteUtils.RedirectToHomepage();
            }

            if (!WebUser.IsAdmin) // && !WebUser.IsInRole("Store Admin"))
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateLabels();
            if (!IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
            enabledStore = StoreHelper.EnabledStore;
        }

        #region GridEvents

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            lstAllStores = Store.GetAll();

            lstProvinces = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);

            //Get List<Guid>, no order
            List<Guid> lstDistrictGuids = StoreHelper.GetStoresGeoGuidsNoOrder(lstAllStores);

            //Guid Filter
            if (selectGuid != Guid.Empty)
                lstDistrictGuids = lstDistrictGuids.Where(x => x == selectGuid).ToList();

            //Change to List<GeoZone>, order by province
            lstDistricts = StoreHelper.GetGeoZonesOrderByProvince(lstDistrictGuids, lstProvinces);

            //Change to List<Guid>, order by province
            lstDistrictGuids = StoreHelper.GetGeoGuidsFromGeoZones(lstDistricts);

            if (!IsPostBack)
                BindAreaList();

            //Get stores in each districts, order by priority, then add to final store list
            List<Store> lstStores = StoreHelper.GetDistrictStoresOrderByPriority(lstAllStores, lstDistrictGuids);

            grid.DataSource = lstStores;
        }

        private void grid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                Literal litManagingArea = (Literal)item.FindControl("litManagingArea");

                Guid.TryParse(item.GetDataKeyValue("ManagingArea").ToString().ToUpper(), out Guid areaGuid);
                if (areaGuid != Guid.Empty)
                {
                    var district = lstDistricts.Where(x => x.Guid == areaGuid).FirstOrDefault();
                    var province = lstProvinces.Where(x => x.Guid == district.ParentGuid).FirstOrDefault();

                    if (province != null)
                        litManagingArea.Text = province.Name + " > " + district.Name;
                    else
                        litManagingArea.Text = district.Name;
                }
            }
        }

        #endregion GridEvents

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, breadcrumb.CurrentPageTitle);
        }

        private void PopulateControls()
        {
        }

        private void BindAreaList()
        {
            ddlArea.Items.Clear();
            ddlArea.Items.Add(new ListItem(StoreResources.StoreChooseDropdown, string.Empty));

            foreach (GeoZone district in lstDistricts)
            {
                string districtName = string.Empty;
                var province = lstProvinces.Where(x => x.Guid == district.ParentGuid).FirstOrDefault();

                if (province != null)
                    districtName = province.Name + " > " + district.Name;
                else
                    districtName = district.Name;

                ddlArea.Items.Add(new ListItem(districtName, district.Guid.ToString().ToUpper()));
            }

            ddlArea.DataBind();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            bool isUpdated = false;
            foreach (GridDataItem data in grid.Items)
            {
                TextBox txtPriority = (TextBox)data.FindControl("txtPriority");
                int districtPriority = Int32.TryParse(txtPriority.Text, out districtPriority) ? districtPriority : -1;
                if (districtPriority < 0)
                    continue;

                int oldPriority = Convert.ToInt32(data.GetDataKeyValue("Priority"));
                if (districtPriority == oldPriority)
                    continue;

                int storeID = Convert.ToInt32(data.GetDataKeyValue("StoreID"));
                Store store = new Store(storeID);
                if (store != null && store.StoreID > 0)
                {
                    string districtGuids = store.DistrictGuids;
                    string districtGuidsNew = string.Empty;
                    foreach (string gp in districtGuids.Split(';'))
                    {
                        string guid = gp.Split('_')[0];

                        string priority = "0";
                        if (gp.Split('_').Count() > 1)
                            priority = gp.Split('_')[1];
                        string areaGuid = data.GetDataKeyValue("ManagingArea").ToString().ToUpper();
                        if (guid.ToUpper() == areaGuid)
                            priority = districtPriority.ToString();

                        districtGuidsNew += guid + "_" + priority + ";";
                    }
                    if (districtGuidsNew != string.Empty)
                        districtGuidsNew = districtGuidsNew.Remove(districtGuidsNew.Length - 1, 1);
                    store.DistrictGuids = districtGuidsNew;
                    if (store.Save())
                        isUpdated = true;
                }
            }
            if (isUpdated)
            {
                if (ddlArea.SelectedValue == string.Empty)
                    selectGuid = Guid.Empty;
                else
                    Guid.TryParse(ddlArea.SelectedValue.ToUpper(), out selectGuid);
                grid.Rebind();
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
            }
        }

        private void ddlArea_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlArea.SelectedValue == string.Empty)
                selectGuid = Guid.Empty;
            else
                Guid.TryParse(ddlArea.SelectedValue.ToUpper(), out selectGuid);

            grid.Rebind();
        }
    }
}
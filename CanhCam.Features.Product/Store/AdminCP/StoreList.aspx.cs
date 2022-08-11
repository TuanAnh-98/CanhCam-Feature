using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.StoreUI
{
    public partial class StoreListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreListPage));
        private SiteUser siteUser;

        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private bool enabledStore = false;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            btnSearch.Click += new EventHandler(btnSearch_Click);

            ddProvince.SelectedIndexChanged += new EventHandler(ddProvince_SelectedIndexChanged);
            ddDistrict.SelectedIndexChanged += new EventHandler(ddDistrict_SelectedIndexChanged);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            //grid.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(grid_ItemDataBound);

            btnDelete.Click += new EventHandler(btnDelete_Click);

            gridPersister = new RadGridSEOPersister(grid);
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

            if (siteUser == null || siteUser.UserId <= 0)
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
            siteUser = SiteUtils.GetCurrentSiteUser();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            enabledStore = StoreHelper.EnabledStore;

            lnkInsert.NavigateUrl = SiteRoot + "/Store/AdminCP/StoreEdit.aspx";
            //if (siteUser != null && siteUser.UserId > 0)
            //    lnkInsert.Visible = true;//AccountHelper.IsAllowedAddNewAddress(siteUser.UserId);
        }

        #region GridEvents

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Guid provinceGuid = Guid.Empty;
            Guid districtGuid = Guid.Empty;

            if (ddProvince.SelectedValue != "")
            {
                provinceGuid = new Guid(ddProvince.SelectedValue);
            }

            if (ddDistrict.SelectedValue != "")
            {
                districtGuid = new Guid(ddDistrict.SelectedValue);
            }

            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;

            //int iCount = Store.GetCount();
            int iCount = Store.GetCountBySearch(siteUser.UserId, WebUser.IsAdmin, siteSettings.SiteId, txtNameKeyword.Text.Trim(), provinceGuid, districtGuid);
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;

            //List<Store> list = Store.GetAll();
            List<Store> list = Store.GetPageBySearch(siteUser.UserId, WebUser.IsAdmin, siteSettings.SiteId, txtNameKeyword.Text.Trim(), startRowIndex, maximumRows, provinceGuid, districtGuid);

            //Push default store to top
            Int32.TryParse(siteSettings.GetExpandoProperty("DefaultStore"), out int defaultStoreID);
            Store defaultStore = list.Where(x => x.StoreID == defaultStoreID).FirstOrDefault();
            if (defaultStore != null)
                list = list.OrderBy(x => x.StoreID != defaultStore.StoreID).ToList();

            grid.DataSource = list;
        }

        //void grid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        //{
        //    if (e.Item is Telerik.Web.UI.GridDataItem)
        //    {
        //        Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
        //        Literal litStoreName = (Literal)item.FindControl("litStoreName");
        //        Literal litStoreDefault = (Literal)item.FindControl("litStoreDefault");
        //        CheckBox cbDefault = (CheckBox)item.FindControl("cbDefault");

        //        int storeID = Convert.ToInt32(item.GetDataKeyValue("StoreID"));
        //        Store store = new Store(storeID);
        //        Int32.TryParse(siteSettings.GetExpandoProperty("DefaultStore"), out int defaultStoreID);
        //        bool isDefault = store.StoreID == defaultStoreID;
        //        if (store != null && store.StoreID > 0)
        //        {
        //            litStoreName.Text = store.Name;
        //            if (isDefault)
        //            {
        //                litStoreDefault.Text = "Mặc định";
        //                cbDefault.Checked = true;
        //            }
        //            else
        //                cbDefault.Checked = false;
        //        }
        //    }
        //}

        #endregion GridEvents

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, StoreResources.StoreListTitle);
            UIHelper.AddConfirmationDialog(btnDelete, StoreResources.StoreDeleteConfirmMessage);
        }

        private void PopulateControls()
        {
            BindProvinceList();
            BindDistrictList();
        }

        #region Geo

        private void ddProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrictList();

            grid.Rebind();
        }

        private void ddDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void BindProvinceList()
        {
            Guid countryGuid = siteSettings.DefaultCountryGuid;
            ddProvince.Items.Clear();
            ddProvince.Items.Add(new ListItem(StoreResources.StoreProvinceDropdown, string.Empty));

            if (countryGuid.ToString().Length == 36)
            {
                ddProvince.DataSource = GeoZone.GetByCountry(countryGuid);
                ddProvince.DataBind();
            }
        }

        private void BindDistrictList()
        {
            ddDistrict.Items.Clear();
            ddDistrict.Items.Add(new ListItem(StoreResources.StoreDistrictDropdown, string.Empty));

            if (ddProvince.SelectedValue.Length == 36)
            {
                ddDistrict.DataSource = GeoZone.GetByListParent(ddProvince.SelectedValue, 1);
                ddDistrict.DataBind();
            }
        }

        #endregion Geo

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int storeID = Convert.ToInt32(data.GetDataKeyValue("StoreID"));
                    Store store = new Store(storeID);

                    if (store != null && store.StoreID > 0 && store.SiteID == siteSettings.SiteId && !store.IsDeleted)
                    {
                        ContentDeleted.Create(siteSettings.SiteId, store.StoreID.ToString(), "Store", typeof(StoreDeleted).AssemblyQualifiedName, store.StoreID.ToString(), Page.User.Identity.Name);

                        store.IsDeleted = true;
                        store.Save();

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    LogActivity.Write("Delete " + iRecordDeleted.ToString() + " store(s)", "Store");
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected bool IsStoreDefault(int storeID)
        {
            Int32.TryParse(siteSettings.GetExpandoProperty("DefaultStore"), out int defaultStoreID);
            return storeID == defaultStoreID;
        }
    }
}

namespace CanhCam.Web.StoreUI
{
    public class StoreDeleted : IContentDeleted
    {
        public bool RestoreContent(string storeID)
        {
            try
            {
                Store store = new Store(Convert.ToInt32(storeID));

                if (store != null && store.StoreID > 0)
                {
                    store.IsDeleted = false;
                    store.Save();
                }
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool DeleteContent(string storeID)
        {
            try
            {
                Store.Delete(Convert.ToInt32(storeID));
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ManufacturersPage : CmsNonBasePage
    {
        #region Properties

        private RadGridSEOPersister gridPersister;
        private static readonly ILog log = LogManager.GetLogger(typeof(ManufacturersPage));

        #endregion Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (!ProductPermission.CanManageManufacturer)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateLabels()
        {
            heading.Text = breadcrumb.CurrentPageTitle;
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductTagsDeleteMultiWarning);
        }

        private void LoadSettings()
        {
            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx";

            AddClassToBody("admin-manufacturers");
        }

        private void PopulateControls()
        {
            PopulateZoneList();
        }

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, true, Product.FeatureGuid);
            ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), ""));
        }

        #endregion Populate Zone List

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        #region "RadGrid Event"

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            var zoneGuid = (Guid?)null;
            if (ddZones.SelectedValue.Length == 36)
                zoneGuid = new Guid(ddZones.SelectedValue);
            var iCount = Manufacturer.GetCount(siteSettings.SiteId, ManufacturerPublishStatus.NotSet, zoneGuid, txtKeyword.Text.Trim());

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = !isApplied;

            grid.DataSource = Manufacturer.GetPage(siteSettings.SiteId, ManufacturerPublishStatus.NotSet, zoneGuid, startRowIndex, maximumRows, txtKeyword.Text.Trim());

            if (iCount == 0)
                btnDelete.Visible = false;
        }

        #endregion "RadGrid Event"

        #region Events

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                bool isUpdated = false;
                foreach (GridDataItem data in grid.Items)
                {
                    var txtDisplayOrder = (TextBox)data.FindControl("txtDisplayOrder");
                    var manufacturerId = Convert.ToInt32(data.GetDataKeyValue("ManufacturerId"));
                    var displayOrder = Convert.ToInt32(data.GetDataKeyValue("DisplayOrder"));
                    var displayOrderNew = displayOrder;
                    int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                    if (displayOrder != displayOrderNew)
                    {
                        var manufacturer = new Manufacturer(manufacturerId);
                        if (manufacturer != null && manufacturer.ManufacturerId > 0)
                        {
                            manufacturer.DisplayOrder = displayOrderNew;
                            manufacturer.Save();

                            LogActivity.Write("Update manufacturer", manufacturer.Name);

                            isUpdated = true;
                        }
                    }
                }

                if (isUpdated)
                {
                    grid.Rebind();

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = false;

                foreach (GridDataItem data in grid.SelectedItems)
                {
                    var manufacturerId = Convert.ToInt32(data.GetDataKeyValue("ManufacturerId"));
                    var manufacturer = new Manufacturer(manufacturerId);

                    if (manufacturer != null && manufacturer.ManufacturerId != -1 && !manufacturer.IsDeleted)
                    {
                        ContentDeleted.Create(siteSettings.SiteId, manufacturer.Name, "Manufacturer", typeof(ManufacturerDeleted).AssemblyQualifiedName, manufacturer.ManufacturerId.ToString(), Page.User.Identity.Name);

                        manufacturer.IsDeleted = true;
                        manufacturer.Save();

                        LogActivity.Write("Delete manufacturer", manufacturer.Name);

                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    grid.Rebind();

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion Events

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnSearch.Click += new EventHandler(BtnSearch_Click);
            //this.ddZones.SelectedIndexChanged += new EventHandler(ddZones_SelectedIndexChanged);

            grid.NeedDataSource += new GridNeedDataSourceEventHandler(grid_NeedDataSource);

            gridPersister = new RadGridSEOPersister(grid);
        }

        #endregion OnInit

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetManufacturersForExport();

            string fileName = "manufacturers-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }
        private DataTable GetManufacturersForExport()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Url", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            dt.Columns.Add("Published", typeof(string));

            var zoneGuid = (Guid?)null;
            if (ddZones.SelectedValue.Length == 36)
                zoneGuid = new Guid(ddZones.SelectedValue);
            var manufacturers = Manufacturer.GetPage(siteSettings.SiteId,
                ManufacturerPublishStatus.NotSet, zoneGuid,
                1, 21474836, txtKeyword.Text.Trim());

            int i = 1;
            foreach (var manu in manufacturers)
            {

                DataRow row = dt.NewRow();

                row["Number"] = i;
                row["Id"] = manu.ManufacturerId;
                row["Name"] = manu.Name;
                row["Url"] = ManufacturerHelper.FormatManufacturerUrl(manu.Url, manu.ManufacturerId);
                row["Description"] = manu.Description;
                row["Published"] = manu.IsPublished ? "Hiển thị" : "Không hiển thị";
                i += 1;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}

namespace CanhCam.Web.ProductUI
{
    public class ManufacturerDeleted : IContentDeleted
    {
        public bool RestoreContent(string manufacturerId)
        {
            try
            {
                var manufacture = new Manufacturer(Convert.ToInt32(manufacturerId));
                if (manufacture != null && manufacture.ManufacturerId > 0)
                {
                    manufacture.IsDeleted = false;
                    manufacture.Save();
                }
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool DeleteContent(string manufacturerId)
        {
            try
            {
                var manufacture = new Manufacturer(Convert.ToInt32(manufacturerId));
                if (manufacture != null && manufacture.ManufacturerId != -1)
                {
                    ContentLanguage.DeleteByContent(manufacture.Guid);
                    FriendlyUrl.DeleteByPageGuid(manufacture.Guid);
                    RelatedItem.DeleteByItem(manufacture.Guid);

                    Manufacturer.Delete(manufacture.ManufacturerId);
                }
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
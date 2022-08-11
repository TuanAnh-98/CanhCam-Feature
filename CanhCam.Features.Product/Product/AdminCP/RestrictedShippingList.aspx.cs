using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Resources;
using System;
using System.Data;
using System.Linq;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public partial class RestrictedShippingListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RestrictedShippingListPage));
        private RadGridSEOPersister gridPersister;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            int iCount = RestrictedShipping.GetCount();

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = !isApplied;

            grid.DataSource = RestrictedShipping.GetPage(startRowIndex, maximumRows, out int _);
        }


        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int itemId = Convert.ToInt32(data.GetDataKeyValue("RowId").ToString());

                    RestrictedShipping voucher = new RestrictedShipping(itemId);
                    if (voucher != null && voucher.RowId > 0)
                    {
                        RestrictedShipping.Delete(voucher.RowId);

                        LogActivity.Write("Delete Restricted area shipping", voucher.GeoZoneGuid.ToString());

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.RestrictedShippingList);
            heading.Text = ProductResources.RestrictedShippingList;
            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));


            var column = grid.MasterTableView.Columns.FindByUniqueName("Store");
            if (column != null)
                column.Visible = ShippingHelper.RestrictedShippingForStore;
        }

        private void LoadSettings()
        {
            AddClassToBody("admin-deals");

        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.btnImport.Click += BtnImport_Click;
            this.btnDelete.Click += btnDelete_Click;
            this.btnExportData.Click += BtnExportData_Click;
            gridPersister = new RadGridSEOPersister(grid);
        }

        private void BtnExportData_Click(object sender, EventArgs e)
        {
            string fileName = "shipping-template-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";
            var dt = GetRestrictedShippingDataForExport();
            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);

        }

        private DataTable GetRestrictedShippingDataForExport()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add(ProductResources.ShippingFeeGeoZoneId, typeof(Guid));
            dt.Columns.Add(ProductResources.ShippingFeeGeoZoneName, typeof(string));
            dt.Columns.Add(ProductResources.OrderWeight, typeof(int));
            dt.Columns.Add(ProductResources.ShippingMethodsTitle, typeof(string));
            dt.Columns.Add(ProductResources.PaymentMethodsTitle, typeof(string));

            if (ShippingHelper.RestrictedShippingForStore)
                dt.Columns.Add(StoreResources.StoreCodeLabel, typeof(int));
            var geoZones = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            var lst = RestrictedShipping.GetAll();
            foreach (var item in geoZones)
            {
                DataRow row = dt.NewRow();
                row[ProductResources.ShippingFeeGeoZoneId] = item.Guid;
                row[ProductResources.ShippingFeeGeoZoneName] = item.Name;
                var rs = lst.FirstOrDefault(it => it.GeoZoneGuid == item.Guid);
                if (rs != null)
                {
                    row[ProductResources.OrderWeight] = rs.Weight;
                    row[ProductResources.ShippingMethodsTitle] = rs.ShippingMethodIds;
                    row[ProductResources.PaymentMethodsTitle] = rs.PaymentMethodIds;
                }
                else
                {
                    row[ProductResources.OrderWeight] = 0;
                    row[ProductResources.ShippingMethodsTitle] = "";
                    row[ProductResources.PaymentMethodsTitle] = "";
                }
                dt.Rows.Add(row);
            }
            return dt;
        }
        #endregion OnInit

        #region Import


        private void BtnImport_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (fileUpload.UploadedFiles.Count == 0)
            {
                message.ErrorMessage = "Vui lòng nhập tập tin.";
                return;
            }
            int i = 1;


            var GeoZoneIdColumnName = ProductResources.ShippingFeeGeoZoneId;
            var GeoZoneNameColumnName = ProductResources.ShippingFeeGeoZoneName;
            var WeightColumnName = ProductResources.OrderWeight;
            var ShippingMethodsColumnName = ProductResources.ShippingMethodsTitle;
            var PaymentMethodsColumnName = ProductResources.PaymentMethodsTitle;
            var StoreCodeColumnName = StoreResources.StoreCodeLabel;
            try
            {
                var workbook = new HSSFWorkbook(fileUpload.UploadedFiles[0].InputStream, true);
                var worksheet = workbook.GetSheetAt(0);
                if (worksheet == null)
                    return;

                var errorMessage = string.Empty;
                int geoZoneIdColumnNo = -1;
                int geoZoneNameColumnNo = -1;
                int weightColumnNo = -1;
                int shippingMethodsColumnNo = -1;
                int paymentMethodsColumnNo = -1;
                int storeCodeColumnNo = -1;

                var titleRowNumber = 0;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();

                    if (columName == GeoZoneIdColumnName)
                        geoZoneIdColumnNo = columNumber;
                    else if (columName == GeoZoneNameColumnName)
                        geoZoneNameColumnNo = columNumber;
                    else if (columName == WeightColumnName)
                        weightColumnNo = columNumber;
                    else if (columName == ShippingMethodsColumnName)
                        shippingMethodsColumnNo = columNumber;
                    else if (columName == PaymentMethodsColumnName)
                        paymentMethodsColumnNo = columNumber;
                    else if (columName == StoreCodeColumnName)
                        storeCodeColumnNo = columNumber;



                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));
                int readCount = 0;
                int importedCount = 0;
                int updatedCount = 0;
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        readCount++;
                        Guid geoZoneGuid = new Guid(GetValueFromExcel(dataRow.GetCell(geoZoneIdColumnNo)).Trim());
                        int weight = Convert.ToInt32(GetValueFromExcel(dataRow.GetCell(weightColumnNo)).Trim());
                        string shippngMethods = GetValueFromExcel(dataRow.GetCell(shippingMethodsColumnNo)).Trim();
                        string paymentMethods = GetValueFromExcel(dataRow.GetCell(paymentMethodsColumnNo)).Trim();
                        string storeCode = GetValueFromExcel(dataRow.GetCell(storeCodeColumnNo)).Trim();
                        int storeId = -1;
                        if (!string.IsNullOrEmpty(storeCode))
                        {
                            var store = StoreCacheHelper.GetStoreByCode(storeCode);
                            if (store != null)
                                storeId = store.StoreID;
                        }
                        bool isNew = false;
                        RestrictedShipping restrictedShipping = new RestrictedShipping(geoZoneGuid, storeId);
                        if (restrictedShipping == null || restrictedShipping.RowId == -1)
                        {
                            isNew = true;
                            restrictedShipping = new RestrictedShipping();
                        }

                        restrictedShipping.StoreId = storeId;
                        restrictedShipping.GeoZoneGuid = geoZoneGuid;
                        restrictedShipping.Weight = weight;
                        restrictedShipping.ShippingMethodIds = shippngMethods;
                        restrictedShipping.PaymentMethodIds = paymentMethods;
                        if (restrictedShipping.Save())
                        {
                            if (isNew)
                                importedCount++;
                            else
                                updatedCount++;
                        }
                    }
                }

                message.WarningMessage = readCount + " địa điểm được đọc thành công.";

                message.SuccessMessage = importedCount + " địa điểm được import thành công.";
                if (updatedCount > 0)
                    message.InfoMessage = updatedCount + " địa điểm được cập nhật thành công.";
                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;

                grid.Rebind();
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }
        protected string FormatStoreName(int storeId)
        {
            if (storeId == -1) return string.Empty;
            return new Store(storeId).Name;
        }
        public static string GetValueFromExcel(ICell obj)
        {
            if (obj == null)
                return string.Empty;

            switch (obj.CellType)
            {
                case CellType.String:
                    return obj.StringCellValue;

                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(obj))
                    {
                        if (obj.DateCellValue != null)
                        {
                            return obj.DateCellValue.ToString("yyyy/MM/dd HH:mm:00");
                        }

                        return string.Empty;
                    }

                    return obj.NumericCellValue.ToString();

                default:
                    obj.SetCellType(CellType.String);
                    return obj.StringCellValue;
            }
        }

        #endregion Import
    }
}
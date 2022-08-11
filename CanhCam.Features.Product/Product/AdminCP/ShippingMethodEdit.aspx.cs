/// Author:					Tran Quoc Vuong - itqvuong@gmail.com
/// Created:				2015-04-24
/// Last Modified:			2015-04-24

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class ShippingMethodEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShippingMethodEditPage));

        private int shippingMethodId = -1;
        private ShippingMethod method = null;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndNew.Click += new EventHandler(btnInsertAndNew_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnExportTemplate.Click += new EventHandler(btnExportTemplate_Click);
            this.btnExportData.Click += new EventHandler(btnExportData_Click);
            this.ddlShippingProvider.SelectedIndexChanged += new EventHandler(ddlShippingProvider_SelectedIndexChanged);
            this.chkFreeShippingOverXEnabled.CheckedChanged += new EventHandler(chkFreeShippingOverXEnabled_CheckedChanged);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);

            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, Page);
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

            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            BindShippingProvider();

            bool loadAllTab = false;
            if (method != null && method.ShippingMethodId > 0)
            {
                txtName.Text = method.Name;
                edDescription.Text = method.Description;
                chkIsActive.Checked = method.IsActive;
                txtShippingFee.Text = ProductHelper.FormatPrice(method.ShippingFee);
                chkFreeShippingOverXEnabled.Checked = method.FreeShippingOverXEnabled;
                txtFreeShippingOverXValue.Text = ProductHelper.FormatPrice(method.FreeShippingOverXValue);

                ListItem liItem = ddlShippingProvider.Items.FindByValue(method.ShippingProvider.ToString());
                if (liItem != null)
                {
                    ddlShippingProvider.ClearSelection();
                    liItem.Selected = true;
                    ddlShippingProvider.Enabled = false;
                }

                loadAllTab = true;
            }

            litShippingMethod.Text = string.Format(ProductResources.ShippingMethodFormat, ddlShippingProvider.SelectedItem.Text);
            PopulateShippingProvider();

            LanguageHelper.PopulateTab(tabLanguage, loadAllTab);
        }

        private void BindShippingProvider()
        {
            ddlShippingProvider.Items.Clear();
            foreach (var item in typeof(ShippingMethodProvider).GetFields())
            {
                if (item.FieldType == typeof(ShippingMethodProvider))
                    ddlShippingProvider.Items.Add(new ListItem { Text = ResourceHelper.GetResourceString("ProductResources", "ShippingProvider" + item.Name), Value = item.GetRawConstantValue().ToString() });
            }
        }

        private void PopulateShippingProvider()
        {
            ShippingMethodProvider shippingMethodId = (ShippingMethodProvider)Convert.ToInt32(ddlShippingProvider.SelectedValue);
            switch (shippingMethodId)
            {
                case ShippingMethodProvider.Free:
                    lblShippingFee.Text = ProductResources.ShippingFreeLabel;
                    divShippingFee.Visible = false;
                    break;

                case ShippingMethodProvider.Fixed:
                    lblShippingFee.Text = ProductResources.ShippingFeeLabel;
                    divShippingFee.Visible = true;
                    break;

                case ShippingMethodProvider.FixedPerItem:
                    lblShippingFee.Text = ProductResources.ShippingFeePerItemLabel;
                    divShippingFee.Visible = true;
                    break;

                default:
                    lblShippingFee.Text = ProductResources.ShippingFeeDefaultLabel;
                    divShippingFee.Visible = true;
                    break;
            }

            divImportExport.Visible = false;
            chkExportDistrict.Visible = false;
            switch (shippingMethodId)
            {
                case ShippingMethodProvider.ByOrderTotal:
                case ShippingMethodProvider.ByWeight:
                    divImportExport.Visible = true;
                    break;

                case ShippingMethodProvider.ByGeoZoneAndFixed:
                case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                case ShippingMethodProvider.ByGeoZoneAndWeight:
                    divImportExport.Visible = true;
                    chkExportDistrict.Visible = true;
                    break;
            }

            PopulateFreeShipping();
        }

        private void PopulateFreeShipping()
        {
            ShippingMethodProvider shippingMethodId = (ShippingMethodProvider)Convert.ToInt32(ddlShippingProvider.SelectedValue);
            switch (shippingMethodId)
            {
                case ShippingMethodProvider.Free:
                case ShippingMethodProvider.ByOrderTotal:
                case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                    divFreeShippingOverXEnabled.Visible = false;
                    divFreeShippingOverXValue.Visible = false;
                    break;

                case ShippingMethodProvider.Fixed:
                case ShippingMethodProvider.FixedPerItem:
                case ShippingMethodProvider.ByWeight:
                    divFreeShippingOverXEnabled.Visible = true;
                    divFreeShippingOverXValue.Visible = chkFreeShippingOverXEnabled.Checked;
                    break;

                default:
                    divFreeShippingOverXEnabled.Visible = true;
                    divFreeShippingOverXValue.Visible = false;
                    break;
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethodEdit.aspx?id=" + itemId.ToString());
            }
        }

        private void btnInsertAndClose_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethods.aspx");
            }
        }

        private void btnInsertAndNew_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethodEdit.aspx");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethodEdit.aspx?id=" + itemId.ToString());
            }
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethods.aspx");
            }
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethodEdit.aspx");
            }
        }

        private int SaveData()
        {
            if (!Page.IsValid) return -1;

            try
            {
                if (method == null || method.ShippingMethodId == -1)
                {
                    method = new ShippingMethod
                    {
                        SiteId = siteSettings.SiteId
                    };
                }

                if (!IsLanguageTab())
                {
                    method.Name = txtName.Text.Trim();
                    method.Description = edDescription.Text.Trim();
                }

                method.ShippingProvider = Convert.ToInt32(ddlShippingProvider.SelectedValue);
                method.IsActive = chkIsActive.Checked;

                switch ((ShippingMethodProvider)method.ShippingProvider)
                {
                    case ShippingMethodProvider.Free:
                        method.ShippingFee = 0;
                        break;

                    default:
                        decimal shippingFee = 0;
                        decimal.TryParse(txtShippingFee.Text, out shippingFee);
                        method.ShippingFee = shippingFee;
                        break;
                }

                switch ((ShippingMethodProvider)method.ShippingProvider)
                {
                    case ShippingMethodProvider.Free:
                    case ShippingMethodProvider.ByOrderTotal:
                    case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                        method.FreeShippingOverXEnabled = false;
                        method.FreeShippingOverXValue = 0;
                        break;

                    case ShippingMethodProvider.Fixed:
                    case ShippingMethodProvider.FixedPerItem:
                    case ShippingMethodProvider.ByWeight:
                        method.FreeShippingOverXEnabled = chkFreeShippingOverXEnabled.Checked;

                        decimal freeShippingOverXValue = 0;
                        decimal.TryParse(txtFreeShippingOverXValue.Text, out freeShippingOverXValue);
                        method.FreeShippingOverXValue = freeShippingOverXValue;

                        break;

                    default:
                        method.FreeShippingOverXEnabled = chkFreeShippingOverXEnabled.Checked;
                        method.FreeShippingOverXValue = 0;
                        break;
                }

                if (method.Save())
                {
                    SaveContentLanguage(method.Guid);
                    SaveTableRateIfNeeded(method);
                }

                if (shippingMethodId > 0)
                {
                    LogActivity.Write("Update shipping method", method.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
                else
                {
                    LogActivity.Write("Create new shipping method", method.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }

                return method.ShippingMethodId;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return -1;
        }

        private bool SaveTableRateIfNeeded(ShippingMethod method)
        {
            if (fileUpload.UploadedFiles.Count == 0)
                return false;

            return ImportData(method);
        }

        private bool ImportData(ShippingMethod method)
        {
            bool result = false;
            int i = 1;

            try
            {
                HSSFWorkbook workbook = new HSSFWorkbook(fileUpload.UploadedFiles[0].InputStream, true);
                ISheet worksheet = workbook.GetSheetAt(0);
                if (worksheet == null)
                    return false;

                int iFinished = 0;
                bool shippingByGeoZone = (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndFixed
                                          || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndOrderTotal
                                          || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight
                                            );

                List<ShippingTableRate> lstTableRates = new List<ShippingTableRate>();
                if (shippingMethodId > 0)
                    lstTableRates = ShippingTableRate.GetByMethod(method.ShippingMethodId);

                for (i = 1; i <= worksheet.LastRowNum; i++)
                {
                    IRow dataRow = worksheet.GetRow(i);

                    if (dataRow != null)
                    {
                        string excelGeoZoneGuid = GetValueFromExcel(dataRow.GetCell(0)).Trim();
                        string excelShippingFee = GetValueFromExcel(dataRow.GetCell(2)).Trim();
                        string excelFromValue = GetValueFromExcel(dataRow.GetCell(3)).Trim();
                        string excelOverXValue = GetValueFromExcel(dataRow.GetCell(4)).Trim();
                        string excelStoreCodeValue = GetValueFromExcel(dataRow.GetCell(5)).Trim();
                        bool isValid = true; 
                        if (string.IsNullOrEmpty(excelShippingFee))
                            isValid = false;
                        if (shippingByGeoZone)
                        {
                            if (excelGeoZoneGuid.Length != 36)
                                isValid = false;

                            if ((method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndOrderTotal || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight) && string.IsNullOrEmpty(excelFromValue))
                                isValid = false;
                        }
                        else if (string.IsNullOrEmpty(excelFromValue))
                            isValid = false;

                        if (!isValid)
                        {
                            iFinished += 1;
                            if (iFinished >= 2)
                                break;

                            continue;
                        }
                        iFinished = 0;

                        var lstShippingFee = excelShippingFee.SplitOnChar('+');
                        var lstFromValue = excelFromValue.SplitOnChar('+');

                        Guid geoZoneGuid = Guid.Empty;
                        decimal shippingFee = Convert.ToDecimal(lstShippingFee[0]);
                        decimal fromValue = decimal.Zero;
                        decimal additionalFee = decimal.Zero;
                        decimal additionalValue = decimal.Zero;

                        if (shippingByGeoZone)
                        {
                            geoZoneGuid = new Guid(excelGeoZoneGuid);
                            if ((method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndOrderTotal || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight))
                                fromValue = Convert.ToDecimal(lstFromValue[0]);
                        }
                        else
                            fromValue = Convert.ToDecimal(lstFromValue[0]);

                        if (lstShippingFee.Count == 2
                            && lstFromValue.Count == 2
                            && (method.ShippingProvider == (int)ShippingMethodProvider.ByWeight || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight))
                        {
                            additionalFee = Convert.ToDecimal(lstShippingFee[1]);
                            additionalValue = Convert.ToDecimal(lstFromValue[1]);
                        }

                        int shippingTableRateId = -1;
                        foreach (ShippingTableRate tblRate in lstTableRates)
                        {
                            if (shippingByGeoZone)
                            {
                                if (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndFixed)
                                {
                                    if (shippingFee == tblRate.ShippingFee && geoZoneGuid == tblRate.GeoZoneGuid)
                                    {
                                        tblRate.MarkAsDeleted = false;
                                        shippingTableRateId = tblRate.ShippingTableRateId;
                                        break;
                                    }
                                }
                                else if (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndOrderTotal)
                                {
                                    if (shippingFee == tblRate.ShippingFee && geoZoneGuid == tblRate.GeoZoneGuid && fromValue == tblRate.FromValue)
                                    {
                                        tblRate.MarkAsDeleted = false;
                                        shippingTableRateId = tblRate.ShippingTableRateId;
                                        break;
                                    }
                                }
                                else if (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight)
                                {
                                    if (shippingFee == tblRate.ShippingFee && geoZoneGuid == tblRate.GeoZoneGuid && fromValue == tblRate.FromValue)
                                    {
                                        tblRate.MarkAsDeleted = false;
                                        shippingTableRateId = tblRate.ShippingTableRateId;
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                if (shippingFee == tblRate.ShippingFee && fromValue == tblRate.FromValue)
                                {
                                    tblRate.MarkAsDeleted = false;
                                    shippingTableRateId = tblRate.ShippingTableRateId;
                                    break;
                                }
                            }
                        }

                        ShippingTableRate tableRate = null;
                        if (shippingTableRateId > 0)
                            tableRate = new ShippingTableRate(shippingTableRateId);
                        else
                        {
                            tableRate = new ShippingTableRate
                            {
                                ShippingMethodId = method.ShippingMethodId
                            };
                        }

                        if (shippingByGeoZone)
                        {
                            tableRate.GeoZoneGuid = geoZoneGuid;
                            if (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndOrderTotal || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight)
                                tableRate.FromValue = fromValue;
                        }
                        else
                            tableRate.FromValue = fromValue;

                        tableRate.FreeShippingOverXValue = 0;
                        if (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndFixed
                            || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight)
                        {
                            if (!string.IsNullOrEmpty(excelOverXValue))
                                tableRate.FreeShippingOverXValue = Convert.ToDecimal(excelOverXValue);
                        }

                        tableRate.ShippingFee = shippingFee;

                        tableRate.AdditionalFee = additionalFee;
                        tableRate.AdditionalValue = additionalValue;

                        if (!string.IsNullOrEmpty(excelStoreCodeValue))
                        {
                            var store = StoreCacheHelper.GetStoreByCode(excelStoreCodeValue);
                            if (store != null)
                                tableRate.StoreId = store.StoreID;
                        }

                        tableRate.Save();

                        result = true;
                    }
                }

                foreach (ShippingTableRate tblRate in lstTableRates)
                {
                    if (tblRate.MarkAsDeleted)
                        ShippingTableRate.Delete(tblRate.ShippingTableRateId);
                }
            }
            catch (Exception ex)
            {
                result = false;
                message.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }

            return result;
        }

        public static string GetValueFromExcel(ICell obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (method != null && method.ShippingMethodId > -1)
                {
                    ContentDeleted.Create(siteSettings.SiteId, method.ShippingMethodId.ToString(), "ShippingMethod", typeof(ShippingMethodDeleted).AssemblyQualifiedName, method.ShippingMethodId.ToString(), Page.User.Identity.Name);

                    method.IsDeleted = true;
                    method.Save();

                    LogActivity.Write("Delete shipping method", method.Name);

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ShippingMethods.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void ddlShippingProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            litShippingMethod.Text = string.Format(ProductResources.ShippingMethodFormat, ddlShippingProvider.SelectedItem.Text);
            PopulateShippingProvider();
        }

        private void chkFreeShippingOverXEnabled_CheckedChanged(object sender, EventArgs e)
        {
            PopulateFreeShipping();
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (method != null && method.ShippingMethodId > 0)
            {
                var lstTableRates = new List<ShippingTableRate>();
                switch ((ShippingMethodProvider)method.ShippingProvider)
                {
                    case ShippingMethodProvider.ByOrderTotal:
                    case ShippingMethodProvider.ByWeight:
                        lstTableRates = ShippingTableRate.GetByMethod(shippingMethodId);
                        grid.DataSource = lstTableRates;
                        grid.Columns.FindByUniqueName("GeoZoneName").Visible = false;
                        break;

                    case ShippingMethodProvider.ByGeoZoneAndFixed:
                    case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                    case ShippingMethodProvider.ByGeoZoneAndWeight:
                        lstTableRates = ShippingTableRate.GetByMethod(shippingMethodId);
                        grid.DataSource = lstTableRates;
                        grid.Columns.FindByUniqueName("GeoZoneName").Visible = true;
                        break;
                }
                grid.Columns.FindByUniqueName("FromValueOrderWeight").Visible = false;
                switch ((ShippingMethodProvider)method.ShippingProvider)
                {
                    case ShippingMethodProvider.ByOrderTotal:
                        grid.Columns.FindByUniqueName("FromValue").Visible = true;
                        grid.Columns.FindByUniqueName("FromValue").HeaderText = ProductResources.ShippingFeeByOrderTotal;
                        break;

                    case ShippingMethodProvider.ByWeight:
                        grid.Columns.FindByUniqueName("FromValue").Visible = true;
                        grid.Columns.FindByUniqueName("FromValue").HeaderText = ProductResources.ShippingFeeByOrderWeight;
                        break;

                    case ShippingMethodProvider.ByGeoZoneAndFixed:
                        grid.Columns.FindByUniqueName("FromValue").Visible = false;
                        break;

                    case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                        grid.Columns.FindByUniqueName("FromValue").Visible = true;
                        grid.Columns.FindByUniqueName("FromValue").HeaderText = ProductResources.ShippingFeeByOrderTotal;
                        break;

                    case ShippingMethodProvider.ByGeoZoneAndWeight:
                        grid.Columns.FindByUniqueName("FromValue").Visible = false;
                        grid.Columns.FindByUniqueName("FromValueOrderWeight").Visible = true;
                        //grid.Columns.FindByUniqueName("FromValue").HeaderText = ProductResources.ShippingFeeByOrderWeight;
                        break;
                }

                if (method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndFixed
                    || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight)
                    grid.Columns.FindByUniqueName("FreeShippingOverXValue").Visible = true;

                if (lstTableRates.Count > 0)
                    divExportData.Visible = true;
            }
        }

        #region Export

        private void btnExportTemplate_Click(object sender, EventArgs e)
        {
            string fileName = "shipping-template-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";
            ShippingMethodProvider provider = (ShippingMethodProvider)Convert.ToInt32(ddlShippingProvider.SelectedValue);

            switch (provider)
            {
                case ShippingMethodProvider.ByOrderTotal:
                    DataTable dt = GetShippingTemplateForExport(provider, false);
                    ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
                    break;

                case ShippingMethodProvider.ByWeight:
                    dt = GetShippingTemplateForExport(provider, false);
                    ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
                    break;

                case ShippingMethodProvider.ByGeoZoneAndFixed:
                case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                case ShippingMethodProvider.ByGeoZoneAndWeight:
                    dt = GetShippingTemplateForExport(provider);
                    ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
                    break;
            }
        }

        private DataTable GetShippingTemplateForExport(ShippingMethodProvider provider, bool exportGeoZone = true)
        {
            DataTable dt = new DataTable();

            string geoZoneIdColumn = ProductResources.ShippingFeeGeoZoneId;
            string geoZoneNameColumn = ProductResources.ShippingFeeGeoZoneName;
            if (!exportGeoZone)
            {
                geoZoneIdColumn = " ";
                geoZoneNameColumn = "  ";
            }

            string shippingFeeColumn = ProductResources.ShippingFeeLabel;
            string orderColumn = ProductResources.ShippingFeeByOrderTotal;
            if (provider == ShippingMethodProvider.ByWeight || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                orderColumn = ProductResources.ShippingFeeByOrderWeight;
            else if (provider == ShippingMethodProvider.ByGeoZoneAndFixed)
                orderColumn = "   ";

            string freeShippingOverXColumn = "    ";
            if (provider == ShippingMethodProvider.ByGeoZoneAndFixed || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                freeShippingOverXColumn = ProductResources.ShippingMethodFreeShippingOverX;

            dt.Columns.Add(geoZoneIdColumn, typeof(Guid));
            dt.Columns.Add(geoZoneNameColumn, typeof(string));
            dt.Columns.Add(shippingFeeColumn, typeof(string));
            dt.Columns.Add(orderColumn, typeof(string));
            dt.Columns.Add(freeShippingOverXColumn, typeof(string));
            string storeCodeColumn = StoreResources.StoreCodeLabel;
            dt.Columns.Add(storeCodeColumn, typeof(string));
            if (exportGeoZone)
            {
                var lstProvinces = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid, 1);
                foreach (GeoZone province in lstProvinces)
                {
                    DataRow row = dt.NewRow();

                    row[geoZoneIdColumn] = province.Guid;
                    row[geoZoneNameColumn] = province.Name;
                    row[shippingFeeColumn] = string.Empty;
                    if (provider != ShippingMethodProvider.ByGeoZoneAndFixed)
                        row[orderColumn] = string.Empty;
                    if (provider == ShippingMethodProvider.ByGeoZoneAndFixed || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                        row[freeShippingOverXColumn] = string.Empty;

                    row[storeCodeColumn] = string.Empty;

                    dt.Rows.Add(row);

                    if (chkExportDistrict.Checked)
                    {
                        var lstDistricts = GeoZone.GetByListParent(province.Guid.ToString(), 1);
                        foreach (GeoZone district in lstDistricts)
                        {
                            DataRow rowDistrict = dt.NewRow();

                            rowDistrict[geoZoneIdColumn] = district.Guid;
                            rowDistrict[geoZoneNameColumn] = province.Name + " >> " + district.Name;
                            rowDistrict[shippingFeeColumn] = string.Empty;
                            if (provider != ShippingMethodProvider.ByGeoZoneAndFixed)
                                rowDistrict[orderColumn] = string.Empty;
                            if (provider == ShippingMethodProvider.ByGeoZoneAndFixed || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                                rowDistrict[freeShippingOverXColumn] = string.Empty;
                            rowDistrict[storeCodeColumn] = string.Empty;
                            dt.Rows.Add(rowDistrict);
                        }
                    }
                }
            }

            return dt;
        }

        private void btnExportData_Click(object sender, EventArgs e)
        {
            string fileName = "shipping-table-rates-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";
            ShippingMethodProvider provider = (ShippingMethodProvider)Convert.ToInt32(ddlShippingProvider.SelectedValue);

            switch (provider)
            {
                case ShippingMethodProvider.ByOrderTotal:
                    DataTable dt = GetShippingTableRatesForExport(provider, false);
                    ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
                    break;

                case ShippingMethodProvider.ByWeight:
                    dt = GetShippingTableRatesForExport(provider, false);
                    ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
                    break;

                case ShippingMethodProvider.ByGeoZoneAndFixed:
                case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                case ShippingMethodProvider.ByGeoZoneAndWeight:
                    dt = GetShippingTableRatesForExport(provider, true);
                    ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
                    break;
            }
        }

        private DataTable GetShippingTableRatesForExport(ShippingMethodProvider provider, bool exportGeoZone)
        {
            DataTable dt = new DataTable();
            string storeCodeColumn = StoreResources.StoreCodeLabel;

            string geoZoneIdColumn = ProductResources.ShippingFeeGeoZoneId;
            string geoZoneNameColumn = ProductResources.ShippingFeeGeoZoneName;
            if (!exportGeoZone)
            {
                geoZoneIdColumn = " ";
                geoZoneNameColumn = "  ";
            }

            string shippingFeeColumn = ProductResources.ShippingFeeLabel;
            string orderColumn = ProductResources.ShippingFeeByOrderTotal;
            if (provider == ShippingMethodProvider.ByWeight || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                orderColumn = ProductResources.ShippingFeeByOrderWeight;

            dt.Columns.Add(geoZoneIdColumn, typeof(Guid));
            dt.Columns.Add(geoZoneNameColumn, typeof(string));
            dt.Columns.Add(shippingFeeColumn, typeof(string));
            if (provider != ShippingMethodProvider.ByGeoZoneAndFixed)
                dt.Columns.Add(orderColumn, typeof(string));

            string freeShippingOverXColumn = ProductResources.ShippingMethodFreeShippingOverX;
            if (provider == ShippingMethodProvider.ByGeoZoneAndFixed || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                dt.Columns.Add(freeShippingOverXColumn, typeof(string));

            dt.Columns.Add(storeCodeColumn, typeof(string));


            var lstTableRates = ShippingTableRate.GetByMethod(shippingMethodId);
            foreach (ShippingTableRate tableRate in lstTableRates)
            {
                DataRow row = dt.NewRow();

                if (exportGeoZone)
                {
                    row[geoZoneIdColumn] = tableRate.GeoZoneGuid;
                    row[geoZoneNameColumn] = tableRate.GeoZoneName;
                }

                bool isAddional = tableRate.AdditionalFee > 0
                    && tableRate.AdditionalValue > 0
                    && (method.ShippingProvider == (int)ShippingMethodProvider.ByWeight || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight);

                if (isAddional)
                    row[shippingFeeColumn] = Convert.ToDouble(tableRate.ShippingFee).ToString() + "+" + Convert.ToDouble(tableRate.AdditionalFee).ToString();
                else
                    row[shippingFeeColumn] = Convert.ToDouble(tableRate.ShippingFee);

                if (provider != ShippingMethodProvider.ByGeoZoneAndFixed)
                {
                    if (isAddional)
                        row[orderColumn] = Convert.ToDouble(tableRate.FromValue).ToString() + "+" + Convert.ToDouble(tableRate.AdditionalValue).ToString();
                    else
                        row[orderColumn] = Convert.ToDouble(tableRate.FromValue);
                }

                if (provider == ShippingMethodProvider.ByGeoZoneAndFixed || provider == ShippingMethodProvider.ByGeoZoneAndWeight)
                    row[freeShippingOverXColumn] = Convert.ToDouble(tableRate.FreeShippingOverXValue);

                if (tableRate.StoreId > 0)
                {
                    var store = StoreCacheHelper.GetStoreById(tableRate.StoreId);
                    if (store != null)
                        row[storeCodeColumn] = store.Code;
                }
                else
                    row[storeCodeColumn] = "";
                dt.Rows.Add(row);
            }

            return dt;
        }

        #endregion Export

        #region Language

        protected void tabLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            txtName.Text = string.Empty;
            edDescription.Text = string.Empty;
            btnDeleteLanguage.Visible = false;

            if (method != null && method.ShippingMethodId > 0)
            {
                if (e.Tab.Index == 0)
                {
                    txtName.Text = method.Name;
                    edDescription.Text = method.Description;
                }
                else
                {
                    ContentLanguage content = new ContentLanguage(method.Guid, Convert.ToInt32(e.Tab.Value));
                    if (content != null && content.Guid != Guid.Empty)
                    {
                        txtName.Text = content.Title;
                        edDescription.Text = content.MetaTitle;

                        btnDeleteLanguage.Visible = true;
                    }
                }
            }

            upButton.Update();
        }

        private bool IsLanguageTab()
        {
            if (tabLanguage.Visible && tabLanguage.SelectedIndex > 0)
                return true;

            return false;
        }

        private void SaveContentLanguage(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsLanguageTab())
                return;

            int languageID = -1;
            if (tabLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabLanguage.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);

            if (txtName.Text.Length > 0)
            {
                content.LanguageId = languageID;
                content.ContentGuid = contentGuid;
                content.SiteGuid = siteSettings.SiteGuid;
                content.Title = txtName.Text.Trim();

                content.Save();
            }
        }

        protected void btnDeleteLanguage_Click(object sender, EventArgs e)
        {
            if (!IsLanguageTab())
                return;

            if (tabLanguage.SelectedIndex > 0)
            {
                int languageId = Convert.ToInt32(tabLanguage.SelectedTab.Value);

                if (languageId > 0 && method != null && method.Guid != Guid.Empty)
                {
                    ContentLanguage.Delete(method.Guid, languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                }
            }
        }

        #endregion Language

        protected string GetStoreName(int storeId)
        {
            var store = StoreCacheHelper.GetStoreById(storeId);
            if (store != null)
                return store.Name;
            return string.Empty;
        }

        protected string GetAdditional(decimal additional)
        {
            if (additional > 0 && (method.ShippingProvider == (int)ShippingMethodProvider.ByWeight
                || method.ShippingProvider == (int)ShippingMethodProvider.ByGeoZoneAndWeight))
                return "+" + ProductHelper.FormatPrice(additional);

            return string.Empty;
        }

        private void PopulateLabels()
        {
            heading.Text = ProductResources.ShippingMethodEditTitle;
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));
            UIHelper.AddConfirmationDialog(btnDeleteLanguage, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edDescription.WebEditor.Height = Unit.Pixel(300);
        }

        private void LoadSettings()
        {
            shippingMethodId = WebUtils.ParseInt32FromQueryString("id", shippingMethodId);

            if (shippingMethodId > 0)
            {
                method = new ShippingMethod(shippingMethodId);
                if (
                   method == null
                   || method.ShippingMethodId <= 0
                   || method.SiteId != siteSettings.SiteId
                   ) method = null;
            }

            HideControls();

            AddClassToBody("shippingmethod-edit");
        }

        private void HideControls()
        {
            btnInsert.Visible = false;
            btnInsertAndNew.Visible = false;
            btnInsertAndClose.Visible = false;
            btnUpdate.Visible = false;
            btnUpdateAndNew.Visible = false;
            btnUpdateAndClose.Visible = false;
            btnDelete.Visible = false;

            if (method == null)
            {
                btnInsert.Visible = true;
                btnInsertAndNew.Visible = true;
                btnInsertAndClose.Visible = true;
            }
            else if (method != null && method.ShippingMethodId > 0)
            {
                btnUpdate.Visible = true;
                btnUpdateAndNew.Visible = true;
                btnUpdateAndClose.Visible = true;

                btnDelete.Visible = true;
            }
        }
    }
}
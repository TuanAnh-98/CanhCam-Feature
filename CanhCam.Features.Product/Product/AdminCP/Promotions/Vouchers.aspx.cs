using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Resources;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class VouchersPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VouchersPage));
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
            int.TryParse(ddlStatus.SelectedValue, out int type);
            int iCount = Voucher.GetCountByAdv(txtKeywords.Text.Trim(), type);

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = !isApplied;

            grid.DataSource = Voucher.GetPageByAdv(txtKeywords.Text.Trim(), type, startRowIndex, maximumRows);
        }

        protected string FormatDate(object objDt, string format = "")
        {
            if (objDt != null)
            {
                if (!string.IsNullOrEmpty(format))
                    return Convert.ToDateTime(objDt).ToString(format);

                return Convert.ToDateTime(objDt).ToString("dd/MM/yyyy HH:mm");
            }

            return string.Empty;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    Guid itemGuid = new Guid(data.GetDataKeyValue("ItemGuid").ToString());

                    Voucher voucher = new Voucher(itemGuid);
                    if (voucher != null && voucher.ItemGuid != Guid.Empty)
                    {
                        Voucher.Delete(voucher.ItemGuid);

                        LogActivity.Write("Delete Voucher", voucher.VoucherCode);

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

        protected string GetLimitation(int limitationTimes)
        {
            if (limitationTimes == 0)
                return "Không giới hạn";

            return limitationTimes.ToString();
        }

        protected string GetActive(bool isActive, object fromDate, object expiryDate)
        {
            if (isActive)
            {
                if (expiryDate != null && Convert.ToDateTime(expiryDate) < DateTime.Now)
                    return "<i class=\"fa fa-minus-circle text-danger\"></i> Hết hạn";

                return "<i class=\"fa fa-check text-success\"></i> " + Resources.ProductResources.CouponActiveLabel;
            }

            return "<i class=\"fa fa-pause text-danger\"></i> " + Resources.ProductResources.CouponInactiveLabel;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.DiscountPageTitle);
            heading.Text = ProductResources.DiscountPageTitle;
            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void LoadSettings()
        {
            AddClassToBody("admin-deals");

            string code = WebUtils.ParseStringFromQueryString("code", "");
            if (!string.IsNullOrEmpty(code))
                txtKeywords.Text = code.Trim();
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.btnSearch.Click += btnSearch_Click;
            this.btnImport.Click += BtnImport_Click;
            this.btnDelete.Click += btnDelete_Click;
            gridPersister = new RadGridSEOPersister(grid);
        }

        #endregion OnInit

        #region Import

        private const string VoucherCodeColumnName = "VoucherCode";
        private const string UseCountColumnName = "UsedCount";
        private const string LimitationTimesColumnName = "LimitationTimes";
        private const string MinimumOrderAmountColumnName = "MinimumOrderAmount";
        private const string StartDateColumnName = "StartDate";
        private const string EndDateColumnName = "EndDate";
        private const string AmountColumnName = "Amount";
        private const string UsePercentageColumnName = "UsePercentage";
        private const string MaxAmountColumnName = "MaxAmount";

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

            try
            {
                var workbook = new HSSFWorkbook(fileUpload.UploadedFiles[0].InputStream, true);
                var worksheet = workbook.GetSheetAt(0);
                if (worksheet == null)
                    return;

                var errorMessage = string.Empty;
                int voucherCodeColumnNo = -1;
                int useCountColumnNo = -1;
                int limitationTimesColumnNo = -1;
                int minimumOrderAmountColumnNo = -1;
                int startDateColumnNo = -1;
                int endDateColumnNo = -1;
                int amountColumnNo = -1;
                int usePercentageColumnNo = -1;
                int maxAmountColumnNo = -1;

                var titleRowNumber = 0;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case VoucherCodeColumnName:
                            voucherCodeColumnNo = columNumber;
                            break;

                        case UseCountColumnName:
                            useCountColumnNo = columNumber;
                            break;

                        case LimitationTimesColumnName:
                            limitationTimesColumnNo = columNumber;
                            break;

                        case MinimumOrderAmountColumnName:
                            minimumOrderAmountColumnNo = columNumber;
                            break;

                        case StartDateColumnName:
                            startDateColumnNo = columNumber;
                            break;

                        case EndDateColumnName:
                            endDateColumnNo = columNumber;
                            break;

                        case AmountColumnName:
                            amountColumnNo = columNumber;
                            break;

                        case UsePercentageColumnName:
                            usePercentageColumnNo = columNumber;
                            break;

                        case MaxAmountColumnName:
                            maxAmountColumnNo = columNumber;
                            break;
                    }

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
                        string voucherCode = GetValueFromExcel(dataRow.GetCell(voucherCodeColumnNo)).Trim();
                        string strUseCount = GetValueFromExcel(dataRow.GetCell(useCountColumnNo)).Trim();
                        string strLimitationTimes = GetValueFromExcel(dataRow.GetCell(limitationTimesColumnNo)).Trim();
                        string strMinimumOrderAmount = GetValueFromExcel(dataRow.GetCell(minimumOrderAmountColumnNo)).Trim();
                        string strStartDate = GetValueFromExcel(dataRow.GetCell(startDateColumnNo)).Trim();
                        string strEndDate = GetValueFromExcel(dataRow.GetCell(endDateColumnNo)).Trim();
                        string strAmount = GetValueFromExcel(dataRow.GetCell(amountColumnNo)).Trim();
                        string strUsePercentage = GetValueFromExcel(dataRow.GetCell(usePercentageColumnNo)).Trim();
                        string strMaxAmount = GetValueFromExcel(dataRow.GetCell(maxAmountColumnNo)).Trim();

                        if (string.IsNullOrEmpty(voucherCode))
                            continue;
                        readCount++;
                        bool isNew = true;
                        Voucher voucher = new Voucher(voucherCode);
                        if (voucher != null && voucher.ItemGuid != Guid.Empty)
                        {
                            if (!chkOverride.Checked)
                                continue;
                            isNew = false;
                        }

                        if (voucher == null)
                            voucher = new Voucher();
                        voucher.VoucherCode = voucherCode;
                        if (int.TryParse(strUseCount, out int useCount))
                            voucher.UseCount = useCount;
                        if (int.TryParse(strLimitationTimes, out int limitationTimes))
                            voucher.LimitationTimes = limitationTimes;
                        if (decimal.TryParse(strMinimumOrderAmount, out decimal minimumOrderAmount))
                            voucher.MinimumOrderAmount = minimumOrderAmount;
                        if (DateTime.TryParse(strStartDate, out DateTime startDate))
                            voucher.StartDate = startDate;
                        if (DateTime.TryParse(strEndDate, out DateTime endDate))
                            voucher.EndDate = endDate;

                        if (decimal.TryParse(strAmount, out decimal amount))
                            voucher.Amount = amount;

                        if (int.TryParse(strMaxAmount, out int maxAmount))
                            voucher.MaxAmount = maxAmount;
                        if (bool.TryParse(strUsePercentage, out bool usePercentage))
                            voucher.UsePercentage = usePercentage;

                        if (voucher.Save())
                        {
                            if (isNew)
                                importedCount++;
                            else
                                updatedCount++;
                        }
                    }
                }

                message.WarningMessage = readCount + " voucher được đọc thành công.";

                message.SuccessMessage = importedCount + " voucher được import thành công.";
                if (updatedCount > 0)
                    message.InfoMessage = updatedCount + " voucher được cập nhật thành công.";
                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;

                grid.Rebind();
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
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
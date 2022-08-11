using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportUpdateProductPrice : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportUpdateProductPrice));
        private IFileSystem fileSystem = null;
        private SiteUser siteUser;
          
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!WebUser.IsAdmin && !ProductPermission.CanImport)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
        }

        private void PopulateLabels()
        {
            heading.Text = breadcrumb.CurrentPageTitle;
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
        }

        private void LoadSettings()
        {
            var p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p != null) { fileSystem = p.GetFileSystem(); }

            siteUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("admin-importproducts");
        }

        #region Import

        private CustomField FindCustomFieldByName(List<CustomField> lstCustomFields, string name)
        {
            foreach (CustomField field in lstCustomFields)
            {
                if (field.Name.ToLower().Trim() == name.ToLower().Trim())
                    return field;
            }

            return null;
        }

        private const string ProductItemNoColumnName = "ItemNo";
        private const string ProductBarCodeColumnName = "Barcode";
        private const string ProductNameColumnName = "ProductName";
        private const string PriceColumnName = "Price";

        private void btnGetData_Click(object sender, EventArgs e)
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

                var productItemNoColumnNo = -1;
                var productBarNoColumnNo = -1;
                var productNameColumnNo = -1;
                var priceColumnNo = -1;
                var oldPriceColumnNo = -1;

                var titleRowNumber = 1; 
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case ProductItemNoColumnName:
                            productItemNoColumnNo = columNumber;
                            break;

                        case ProductBarCodeColumnName:
                            productBarNoColumnNo = columNumber;
                            break;

                        case ProductNameColumnName:
                            productNameColumnNo = columNumber;
                            break;

                        case PriceColumnName:
                            priceColumnNo = columNumber;
                            break;
                    }

                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));

                var existsColumnName = "Exists";
                var dt = new DataTable();
                dt.Columns.Add(ProductItemNoColumnName, typeof(string));
                dt.Columns.Add(ProductBarCodeColumnName, typeof(string));
                dt.Columns.Add(ProductNameColumnName, typeof(string));
                dt.Columns.Add(PriceColumnName, typeof(string));
                dt.Columns.Add(existsColumnName, typeof(bool));

                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string productCode = GetValueFromExcel(dataRow.GetCell(productItemNoColumnNo)).Trim();
                        string barCode = GetValueFromExcel(dataRow.GetCell(productBarNoColumnNo)).Trim();
                        string productName = GetValueFromExcel(dataRow.GetCell(productNameColumnNo)).Trim();
                        string sPrice = GetValueFromExcel(dataRow.GetCell(priceColumnNo)).Trim();
                        string sOldPrice = GetValueFromExcel(dataRow.GetCell(oldPriceColumnNo)).Trim();
                        if (string.IsNullOrEmpty(productCode))
                            continue;

                        var row = dt.NewRow();
                        row[ProductItemNoColumnName] = productCode;
                        row[ProductBarCodeColumnName] = barCode;
                        row[ProductNameColumnName] = productName;
                        row[PriceColumnName] = string.IsNullOrEmpty(sPrice) ? "0" : sPrice;

                        var product = Product.GetByCode(siteSettings.SiteId, productCode);
                        if (product != null)
                            row[existsColumnName] = true;
                        else
                            row[existsColumnName] = false;

                        dt.Rows.Add(row);
                    }
                }

                btnGetData.Visible = false;
                btnImport.Visible = true;
                grid.DataSource = dt;
                grid.DataBind();

                message.InfoMessage = "Số sản phẩm đọc được: " + dt.Rows.Count;
                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                var numberProductsImported = 0;
                foreach (GridDataItem data in grid.Items)
                {
                    var productCode = data.GetDataKeyValue(ProductItemNoColumnName).ToString();
                    var barCode = data.GetDataKeyValue(ProductBarCodeColumnName).ToString();
                    var sPrice = data.GetDataKeyValue(PriceColumnName).ToString();
                    var product = Product.GetByCode(siteSettings.SiteId, productCode);
                    if (product != null
                        && decimal.TryParse(sPrice, out decimal price)
                        )
                    {
                        product.LastModUserGuid = siteUser.UserGuid;
                        product.Price = price;
                        if (product.Save())
                            numberProductsImported++; 
                    }

                }

                message.SuccessMessage = string.Format("Đã import {0} sản phẩm thành công", numberProductsImported);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
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

        public static ICell CreateCell(ref IRow dataRow, int column, object value, ICellStyle style, IFont font)
        {
            ICell cell = dataRow.CreateCell(column);
            if (value != null)
                cell.SetCellValue(value.ToString());
            else
                cell.SetCellValue(string.Empty);

            if (style != null)
                cell.CellStyle = style;
            if (font != null)
                cell.CellStyle.SetFont(font);

            return cell;
        }

        #endregion Import

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnImport.Click += new EventHandler(btnImport_Click);
            this.btnGetData.Click += new EventHandler(btnGetData_Click);
        }

        #endregion OnInit
    }
}
using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportNewProductPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportNewProductPage));
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

            //ACCESS DENIED
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
            PopulateZoneList();
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

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);
            ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "SelectButton"), ""));
        }

        #endregion Populate Zone List

        private const string ProductItemNoColumnName = "ItemNo";
        private const string ProductBarCodeColumnName = "Barcode";
        private const string ProductNameColumnName = "ProductName";
        private const string PriceColumnName = "Price";
        private const string OldPriceColumnName = "OldPrice";

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (fileUpload.UploadedFiles.Count == 0)
            {
                message.ErrorMessage = "Vui lòng nhập tập tin.";
                return;
            }
            if (!int.TryParse(ddZones.SelectedValue, out int zoneId))
            {
                message.ErrorMessage = "Vui lòng chọn chuyên mục.";
                return;
            }

            var node = SiteUtils.GetSiteMapNodeByZoneId(zoneId);
            if (node == null)
            {
                message.ErrorMessage = "Chuyên mục không tồn tại.";
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

                        case OldPriceColumnName:
                            oldPriceColumnNo = columNumber;
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
                dt.Columns.Add(OldPriceColumnName, typeof(string));
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
                        row[OldPriceColumnName] = string.IsNullOrEmpty(sOldPrice) ? "0" : sOldPrice;
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

            if (!int.TryParse(ddZones.SelectedValue, out int zoneId))
            {
                message.ErrorMessage = "Vui lòng chọn chuyên mục.";
                return;
            }
            try
            {
                var numberProductsImported = 0;
                foreach (GridDataItem data in grid.Items)
                {
                    var productCode = data.GetDataKeyValue(ProductItemNoColumnName).ToString();
                    var productName = data.GetDataKeyValue(ProductNameColumnName).ToString();
                    var barCode = data.GetDataKeyValue(ProductBarCodeColumnName).ToString();
                    var sPrice = data.GetDataKeyValue(PriceColumnName).ToString();
                    var sOldPrice = data.GetDataKeyValue(OldPriceColumnName).ToString();
                    var product = Product.GetByCode(siteSettings.SiteId, productCode);

                    if (product != null && !chkOverride.Checked)
                        continue;
                    if (product != null && chkOverride.Checked
                        )
                    {
                        if (product.BarCode != barCode)
                            product.BarCode = barCode;

                        product.LastModUserGuid = siteUser.UserGuid;
                        if (decimal.TryParse(sPrice, out decimal price))
                            product.Price = price;
                        if (decimal.TryParse(sOldPrice, out decimal oldPrice))
                            product.OldPrice = oldPrice;
                        var changedZone = false;
                        if (product.ZoneId != zoneId)
                            changedZone = true;

                        if (product.Title != productName && product.Url.Length > 0)
                        {
                            var oldUrl = product.Url.Replace("~/", string.Empty);
                            var oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                            if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.PageGuid == product.ProductGuid))
                                FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
                        }
                        product.ZoneId = zoneId;
                        if (product.Save())
                            numberProductsImported++;
                        if (changedZone)
                        {
                            var friendlyUrls = FriendlyUrl.GetByPageGuid(product.ProductGuid);
                            foreach (var item in friendlyUrls)
                            {
                                item.RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                    + product.ZoneId.ToInvariantString()
                                    + "&ProductID=" + product.ProductId.ToInvariantString();

                                item.Save();
                            }
                        }
                        continue;
                    }

                    if (!string.IsNullOrEmpty(productName))
                    {
                        product = new Product()
                        {
                            SiteId = siteSettings.SiteId,
                            Title = productName,
                        };
                        product.Code = productCode;
                        product.BarCode = barCode;
                        product.SiteId = siteSettings.SiteId;
                        product.IsPublished = true;
                        product.OpenInNewWindow = false;
                        product.StartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0);
                        product.EndDate = DateTime.MaxValue;
                        product.UserGuid = siteUser.UserGuid;
                        product.ShowOption = 0;
                        product.Position = 0;
                        product.LastModUserGuid = siteUser.UserGuid;
                        product.ZoneId = zoneId;
                        if (decimal.TryParse(sPrice, out decimal price))
                            product.Price = price;
                        if (decimal.TryParse(sOldPrice, out decimal oldPrice))
                            product.OldPrice = oldPrice;
                        var friendlyUrlString = SiteUtils.SuggestFriendlyUrl(productName, siteSettings);
                        if (friendlyUrlString.EndsWith("/"))
                            friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

                        product.Url = "~/" + friendlyUrlString;
                        if (product.Save())
                        {
                            numberProductsImported += 1;

                            if (friendlyUrlString.Length > 0)
                            {
                                var newFriendlyUrl = new FriendlyUrl
                                {
                                    SiteId = siteSettings.SiteId,
                                    SiteGuid = siteSettings.SiteGuid,
                                    PageGuid = product.ProductGuid,
                                    Url = friendlyUrlString,
                                    RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                    + product.ZoneId.ToInvariantString()
                                    + "&ProductID=" + product.ProductId.ToInvariantString()
                                };

                                newFriendlyUrl.Save();
                            }
                        }
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
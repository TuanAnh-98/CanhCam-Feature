using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Resources;
using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.StoreUI
{
    public partial class ImportInventoryListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportInventoryListPage));
        private IFileSystem fileSystem = null;
        private SiteUser siteUser;

        private bool enabledStore = false;
        private bool enabledInventory = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!enabledStore || !enabledInventory)
            {
                SiteUtils.RedirectToHomepage();
            }

            if (!WebUser.IsAdmin && !WebUser.IsInRole("Store Admin"))
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
            PopulateStoreList();
        }

        private void PopulateStoreList()
        {
            ddStore.Items.Clear();
            ddStore.Items.Add(new ListItem(StoreResources.StoreSelectLabel, string.Empty));

            var lstStores = Store.GetAll();
            if (!WebUser.IsAdmin)
            {
                foreach (Store store in lstStores.Reverse<Store>())
                {
                    var isAdmin = false;
                    var lstAdmins = store.OrderUserIDs.Split(';');
                    foreach (string adminID in lstAdmins)
                    {
                        if (adminID == siteUser.UserId.ToString())
                            isAdmin = true;
                    }
                    if (!isAdmin)
                        lstStores.Remove(store);
                }
            }

            ddStore.DataSource = lstStores;
            ddStore.DataBind();
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

            enabledStore = StoreHelper.EnabledStore;
            enabledInventory = StoreHelper.EnabledInventory;
        }

        #region Import

        private const string ProductIDColumnName = "ProductID";
        private const string ProductCodeColumnName = "ProductCode";
        private const string ProductNameColumnName = "ProductName";
        private const string StoreIDColumnName = "StoreID";
        private const string StoreNameColumnName = "StoreName";

        //const string ProductPriceColumnName = "Price";
        private const string StockAPIProductIDColumnName = "APICode";

        private const string StockQuantityColumnName = "Quantity";
        private const string existsColumnName = "Exists";

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (fileUpload.UploadedFiles.Count == 0)
            {
                message.ErrorMessage = "Vui lòng nhập tập tin.";
                return;
            }

            Int32.TryParse(ddStore.SelectedValue, out int ddStoreValue);
            if (ddStoreValue < 1)
            {
                message.ErrorMessage = "Vui lòng chọn cửa hàng để nhập khẩu";
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

                var productIDColumnNo = -1;
                var productCodeColumnNo = -1;
                var productNameColumnNo = -1;
                var storeIDColumnNo = -1;
                var storeNameColumnNo = -1;
                //var productPriceColumnNo = -1;
                var stockAPIProductIDColumnNo = -1;
                var stockQuantityColumnNo = -1;

                var titleRowNumber = 0;
                var columnNumber = 0;
                var columnName = string.Empty;
                do
                {
                    //Get column number for each column
                    columnName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columnNumber)).Trim();
                    switch (columnName)
                    {
                        case ProductIDColumnName:
                            productIDColumnNo = columnNumber;
                            break;

                        case ProductCodeColumnName:
                            productCodeColumnNo = columnNumber;
                            break;

                        case ProductNameColumnName:
                            productNameColumnNo = columnNumber;
                            break;

                        case StoreIDColumnName:
                            storeIDColumnNo = columnNumber;
                            break;

                        case StoreNameColumnName:
                            storeNameColumnNo = columnNumber;
                            break;
                        //case ProductPriceColumnName:
                        //    productPriceColumnNo = columnNumber;
                        //    break;
                        case StockAPIProductIDColumnName:
                            stockAPIProductIDColumnNo = columnNumber;
                            break;

                        case StockQuantityColumnName:
                            stockQuantityColumnNo = columnNumber;
                            break;
                    }

                    columnNumber += 1;
                } while (!string.IsNullOrEmpty(columnName));

                var dt = new DataTable();
                dt.Columns.Add(ProductIDColumnName, typeof(int));
                dt.Columns.Add(ProductCodeColumnName, typeof(string));
                dt.Columns.Add(ProductNameColumnName, typeof(string));
                dt.Columns.Add(StoreIDColumnName, typeof(int));
                dt.Columns.Add(StoreNameColumnName, typeof(string));
                //dt.Columns.Add(ProductPriceColumnName, typeof(int));
                dt.Columns.Add(StockAPIProductIDColumnName, typeof(string));
                dt.Columns.Add(StockQuantityColumnName, typeof(string));
                dt.Columns.Add(existsColumnName, typeof(bool));

                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        //Get value from each row of excel
                        string productID = GetValueFromExcel(dataRow.GetCell(productIDColumnNo)).Trim();
                        string productCode = GetValueFromExcel(dataRow.GetCell(productCodeColumnNo)).Trim();
                        string productName = GetValueFromExcel(dataRow.GetCell(productNameColumnNo)).Trim();
                        string storeID = GetValueFromExcel(dataRow.GetCell(storeIDColumnNo)).Trim();
                        string storeName = GetValueFromExcel(dataRow.GetCell(storeNameColumnNo)).Trim();
                        //string price = GetValueFromExcel(dataRow.GetCell(productPriceColumnNo)).Trim();
                        string apiProductID = GetValueFromExcel(dataRow.GetCell(stockAPIProductIDColumnNo)).Trim();
                        int intQuantity = Int32.TryParse(GetValueFromExcel(dataRow.GetCell(stockQuantityColumnNo)).Trim(), out intQuantity) ? intQuantity : -1;
                        string quantity = intQuantity > -1 ? GetValueFromExcel(dataRow.GetCell(stockQuantityColumnNo)).Trim() : string.Empty;

                        //if (string.IsNullOrEmpty(productCode))
                        //    continue;

                        Int32.TryParse(storeID, out int storeId);
                        if (storeId != ddStoreValue)
                            continue;

                        int productId = Int32.TryParse(productID, out productId) ? productId : -1;
                        if (productId < 1)
                            continue;

                        //Assign value to DataTable for grid
                        var row = dt.NewRow();
                        row[ProductIDColumnName] = productID;
                        row[ProductCodeColumnName] = productCode;
                        row[ProductNameColumnName] = productName;
                        row[StoreIDColumnName] = storeID;
                        row[StoreNameColumnName] = storeName;
                        //row[ProductPriceColumnName] = price;
                        row[StockAPIProductIDColumnName] = apiProductID;
                        row[StockQuantityColumnName] = quantity;

                        //var product = Product.GetByCode(siteSettings.SiteId, productCode);
                        var product = new Product(siteSettings.SiteId, productId);
                        if (product != null && product.ProductId > 0)
                        {
                            var productInStore = StockInventory.GetProductInStore(product.ProductId, storeId);
                            if (productInStore != null)
                                row[existsColumnName] = true;
                            else
                                row[existsColumnName] = false;
                        }
                        else
                            row[existsColumnName] = false;

                        dt.Rows.Add(row);
                    }
                }

                btnGetData.Visible = false;
                fileUpload.Visible = false;
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
                    Int32.TryParse(data.GetDataKeyValue(ProductIDColumnName).ToString(), out int productID);
                    //var productCode = data.GetDataKeyValue(ProductCodeColumnName).ToString();
                    //var product = Product.GetByCode(siteSettings.SiteId, productCode);
                    var product = new Product(siteSettings.SiteId, productID);
                    Int32.TryParse(data.GetDataKeyValue(StoreIDColumnName).ToString(), out int storeID);
                    string apiProductID = data.GetDataKeyValue(StockAPIProductIDColumnName).ToString();
                    int quantity = Int32.TryParse(data.GetDataKeyValue(StockQuantityColumnName).ToString(), out quantity) ? quantity : -1;
                    if (quantity < 0)
                        continue;
                    Boolean.TryParse(data.GetDataKeyValue(existsColumnName).ToString(), out bool exists);

                    if (exists)
                    {
                        StockInventory stockInventory = StockInventory.GetProductInStore(product.ProductId, storeID);
                        stockInventory.ApiProductID = apiProductID;
                        stockInventory.Quantity = quantity;
                        if (stockInventory.Save(siteSettings.SiteId))
                            numberProductsImported++;
                    }
                    else
                    {
                        StockInventory stockInventory = new StockInventory
                        {
                            ProductID = product.ProductId,
                            StoreID = storeID,
                            ApiProductID = apiProductID,
                            Quantity = quantity
                        };
                        if (stockInventory.Save(siteSettings.SiteId))
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
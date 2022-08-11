using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.SearchIndex;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportProductsFixBugPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportProductsFixBugPage));
        private IFileSystem fileSystem = null;
        private SiteUser siteUser;
        private List<Manufacturer> manufacturers;

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

            manufacturers = Manufacturer.GetAll(siteSettings.SiteId, ManufacturerPublishStatus.Published, null, -1);
        }

        private void LoadSettings()
        {
            var p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p != null) { fileSystem = p.GetFileSystem(); }

            siteUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("admin-importproducts");
        }

        #region Import

        private const string ProductItemNoColumnName = "ItemNo";
        private const string ProductBarcodeColumnName = "Barcode";
        private const string ProductItemNo2ColumnName = "ItemNo2";
        private const string ProductBarcode2ColumnName = "Barcode2";
        private const string VaridateColumnName = "Varidate";
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

                int productItemNoColumnNo = -1;
                int productBarcodeColumnNo = -1;
                int productItemNo2ColumnNo = -1;
                int productBarcode2ColumnNo = -1;
                int varidateColumnNo = -1;
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

                        case ProductBarcodeColumnName:
                            productBarcodeColumnNo = columNumber;
                            break;

                        case ProductItemNo2ColumnName:
                            productItemNo2ColumnNo = columNumber;
                            break;

                        case ProductBarcode2ColumnName:
                            productBarcode2ColumnNo = columNumber;
                            break;
                        case VaridateColumnName:
                            varidateColumnNo = columNumber;
                            break;

                    }

                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));
                var products = Product.GetPageAdv();
                int count = 0;
                int deleted = 0;
                int deletedNotInERP = 0;
                List<int> productsUpdated = new List<int>();
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string productItemNo = GetValueFromExcel(dataRow.GetCell(productItemNoColumnNo)).Trim();
                        string productBarcode = GetValueFromExcel(dataRow.GetCell(productBarcodeColumnNo)).Trim();

                        string productItemNo2 = GetValueFromExcel(dataRow.GetCell(productItemNo2ColumnNo)).Trim();
                        string productBarcode2 = GetValueFromExcel(dataRow.GetCell(productBarcode2ColumnNo)).Trim();
                        string varidate = GetValueFromExcel(dataRow.GetCell(varidateColumnNo)).Trim();

                        //Remove product updated
                        products = products.Where(it => !productsUpdated.Contains(it.ProductId)).ToList();

                        if (productItemNo2 == "Không có trên phần mềm")
                        {
                            var results = products.Where(it => it.Code == productItemNo).ToList();
                            foreach (var item in results)
                            {
                                DeleteProduct(item);
                                productsUpdated.Add(item.ProductId);
                                deletedNotInERP++;
                            }
                        }
                        else
                        {
                            var results = products.Where(it =>
                                                           it.BarCode == productBarcode2
                                                        || it.Code == productBarcode2
                                                        || it.BarCode == productItemNo2
                                                        || it.Code == productItemNo2).OrderByDescending(it => it.LastModUtc).ToList();
                            int productFinal = -1;
                            // if (varidate == "Update ItemNo")
                            foreach (Product product in results)
                            {
                                if (product.Code == productBarcode2)
                                    productFinal = product.ProductId;

                                product.Code = productItemNo2;
                                product.BarCode = productBarcode2;
                                product.Save();

                                productsUpdated.Add(product.ProductId);
                                count++;
                            }
                            if (productFinal > 0)
                            {
                                foreach (var item in results)
                                {
                                    if (item.ProductId == productFinal) continue;
                                    DeleteProduct(item);
                                    productsUpdated.Add(item.ProductId);
                                    deleted++;
                                }
                            }
                            else if (results != null && results.Count > 1)
                                for (int y = 1; y < results.Count; y++)
                                {
                                    DeleteProduct(results[y]);
                                    productsUpdated.Add(results[y].ProductId);
                                    deleted++;
                                }


                        }
                    }
                }

                message.SuccessMessage = "Update " + count + " ----- Deleted " + deleted + "----- Deleted Not In ERP :" + deletedNotInERP;
                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }
        private void DeleteProduct(Product product)
        {
            ContentDeleted.Create(siteSettings.SiteId, product.Title, "Product",
                typeof(ProductDeleted).AssemblyQualifiedName, product.ProductId.ToString(), Page.User.Identity.Name);

            product.IsDeleted = true;

            product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

            product.SaveDeleted();

            SiteUtils.QueueIndexing();

            ShoppingCartItem.DeleteByProduct(product.ProductId);

            LogActivity.Write("Delete product", product.Title);
        }
        private void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["ProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
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

            this.btnGetData.Click += new EventHandler(btnGetData_Click);
        }

        #endregion OnInit
    }
}
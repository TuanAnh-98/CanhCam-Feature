using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.SearchIndex;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public partial class DeleteProductsToolPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DeleteProductsToolPage));
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

        private const string ProductItemNoColumnName = "ItemNo";

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
                    }

                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));
                int count = 0;
                int notFound = 0;
                List<int> productsUpdated = new List<int>();
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string productItemNo = GetValueFromExcel(dataRow.GetCell(productItemNoColumnNo)).Trim();
                        var product = Product.GetByCode(siteSettings.SiteId, productItemNo);

                        if (product == null || product.ProductId == -1)
                        {
                            notFound++;
                            continue;
                        }
                        if (!chkDeleteOver.Checked)
                            DeleteProduct(product);
                        else
                            DeleteProductDatabase(product);
                        count++;
                    }
                }
                message.SuccessMessage = count + " sản phẩm bị xóa <br/>" + notFound + " sản phẩm không tìm thấy";

                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }

        private void DeleteProductDatabase(Product product)
        {
            ProductHelper.DeleteFolder(siteSettings.SiteId, product.ProductId);

            ContentMedia.DeleteByContent(product.ProductGuid);
            ShoppingCartItem.DeleteByProduct(product.ProductId);

            var listAtributes = ContentAttribute.GetByContentAsc(product.ProductGuid);
            foreach (ContentAttribute item in listAtributes)
                ContentLanguage.DeleteByContent(item.Guid);

            ContentAttribute.DeleteByContent(product.ProductGuid);
            ContentLanguage.DeleteByContent(product.ProductGuid);
            ProductProperty.DeleteByProduct(product.ProductId);
            FriendlyUrl.DeleteByPageGuid(product.ProductGuid);
            ProductComment.DeleteByProduct(product.ProductId);
            TagItem.DeleteByItem(product.ProductGuid);
            FileAttachment.DeleteByItem(product.ProductGuid);
            RelatedItem.DeleteByItem(product.ProductGuid);
            ProductZone.DeleteByProduct(product.ProductId);

            product.Delete();
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
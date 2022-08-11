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
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportProductsPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportProductsPage));
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

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);
            ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "SelectButton"), ""));
        }

        #endregion Populate Zone List

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

        private const string ProductCodeColumnName = "ProductCode";
        private const string ProductNameColumnName = "ProductName";
        private const string BriefContentColumnName = "BriefContent";
        private const string FullContentColumnName = "FullContent";
        private const string PriceColumnName = "Price";
        private const string PictureColumnName = "Picture";
        private const string SubTitleColumnName = "SubTitle";
        private const string DynamicFieldColumnName = "DynamicField";

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
                var lstCustomFields = CustomField.GetActiveByZone(siteSettings.SiteId, Product.FeatureGuid, node.ZoneGuid);
                var lstCustomFieldOptions = new List<CustomFieldOption>();
                foreach (var field in lstCustomFields)
                    lstCustomFieldOptions.AddRange(CustomFieldOption.GetByCustomField(field.CustomFieldId));

                var productCodeColumnNo = -1;
                var productNameColumnNo = -1;
                var briefContentColumnNo = -1;
                var fullContentColumnNo = -1;
                var priceColumnNo = -1;
                var pictureColumnNo = -1;
                var subTitleColumnNo = -1;
                var dynamicFieldsData = new Dictionary<int, CustomField>();

                var titleRowNumber = 0;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case ProductCodeColumnName:
                            productCodeColumnNo = columNumber;
                            break;

                        case ProductNameColumnName:
                            productNameColumnNo = columNumber;
                            break;

                        case BriefContentColumnName:
                            briefContentColumnNo = columNumber;
                            break;

                        case FullContentColumnName:
                            fullContentColumnNo = columNumber;
                            break;

                        case PriceColumnName:
                            priceColumnNo = columNumber;
                            break;

                        case PictureColumnName:
                            pictureColumnNo = columNumber;
                            break;

                        case SubTitleColumnName:
                            subTitleColumnNo = columNumber;
                            break;

                        case DynamicFieldColumnName:
                            var columRawName = GetValueFromExcel(worksheet.GetRow(titleRowNumber - 1).GetCell(columNumber)).Trim();
                            var customField = GetCustomFieldByName(lstCustomFields, columRawName);
                            if (customField == null)
                            {
                                message.ErrorMessage = "DynamicField cột " + columNumber.ToString() + " không tồn tại.";
                                return;
                            }

                            dynamicFieldsData[columNumber] = customField;
                            break;
                    }

                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));

                var existsColumnName = "Exists";
                var dt = new DataTable();
                dt.Columns.Add(ProductCodeColumnName, typeof(string));
                dt.Columns.Add(ProductNameColumnName, typeof(string));
                dt.Columns.Add(BriefContentColumnName, typeof(string));
                dt.Columns.Add(FullContentColumnName, typeof(string));
                dt.Columns.Add(PriceColumnName, typeof(double));
                dt.Columns.Add(PictureColumnName, typeof(string));
                dt.Columns.Add(SubTitleColumnName, typeof(string));
                foreach (var dic in dynamicFieldsData)
                    dt.Columns.Add(DynamicFieldColumnName + "-" + dic.Value.Name, typeof(string));
                dt.Columns.Add(existsColumnName, typeof(bool));

                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string productCode = GetValueFromExcel(dataRow.GetCell(productCodeColumnNo)).Trim();
                        string productName = GetValueFromExcel(dataRow.GetCell(productNameColumnNo)).Trim();
                        string briefContent = GetValueFromExcel(dataRow.GetCell(briefContentColumnNo)).Trim();
                        string fullContent = GetValueFromExcel(dataRow.GetCell(fullContentColumnNo)).Trim();
                        string picture = GetValueFromExcel(dataRow.GetCell(pictureColumnNo)).Trim();
                        string sPrice = GetValueFromExcel(dataRow.GetCell(priceColumnNo)).Trim();
                        string subTitle = GetValueFromExcel(dataRow.GetCell(subTitleColumnNo)).Trim();

                        if (string.IsNullOrEmpty(productCode))
                            continue;

                        var row = dt.NewRow();
                        row[ProductCodeColumnName] = productCode;
                        row[ProductNameColumnName] = productName;
                        row[BriefContentColumnName] = briefContent;
                        row[FullContentColumnName] = fullContent;
                        row[PriceColumnName] = string.IsNullOrEmpty(sPrice) ? "0" : sPrice;
                        row[PictureColumnName] = picture;
                        row[SubTitleColumnName] = subTitle;

                        foreach (var dic in dynamicFieldsData)
                        {
                            var property = GetValueFromExcel(dataRow.GetCell(dic.Key)).Trim();
                            row[DynamicFieldColumnName + "-" + dic.Value.Name] = property;

                            if (errorMessage.Length == 0 && dic.Value.DataType == (int)CustomFieldDataType.SelectBox && property.Contains(";"))
                                errorMessage = "<div>DynamicField " + dic.Value.Name + " có kiểu Selecbox nên không thể cập nhật nhiều giá trị</div>";
                        }

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

            var node = SiteUtils.GetSiteMapNodeByZoneId(zoneId);
            if (node == null)
            {
                message.ErrorMessage = "Chuyên mục không tồn tại.";
                return;
            }

            try
            {
                var lstCustomFields = CustomField.GetActiveByZone(siteSettings.SiteId, Product.FeatureGuid, node.ZoneGuid);
                var lstCustomFieldOptions = new List<CustomFieldOption>();
                foreach (var field in lstCustomFields)
                    lstCustomFieldOptions.AddRange(CustomFieldOption.GetByCustomField(field.CustomFieldId));

                var vitualImagePaths = "/Data/Sites/1/media/dataimports/";
                var numberProductsImported = 0;
                foreach (GridDataItem data in grid.Items)
                {
                    var productCode = data.GetDataKeyValue(ProductCodeColumnName).ToString();
                    var productName = data.GetDataKeyValue(ProductNameColumnName).ToString();
                    var product = Product.GetByCode(siteSettings.SiteId, productCode);
                    var changedZone = false;
                    var isExists = false;
                    if (product != null)
                    {
                        isExists = true;
                        if (!chkOverride.Checked)
                            continue;

                        if (product.ZoneId != zoneId)
                            changedZone = true;

                        if (product.Title != productName && product.Url.Length > 0)
                        {
                            var oldUrl = product.Url.Replace("~/", string.Empty);
                            var oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                            if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.PageGuid == product.ProductGuid))
                                FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
                        }
                    }
                    else
                    {
                        product = new Product
                        {
                            Code = productCode,
                            SiteId = siteSettings.SiteId,
                            IsPublished = true,
                            OpenInNewWindow = false,
                            StartDate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, 0),
                            EndDate = DateTime.MaxValue,
                            UserGuid = siteUser.UserGuid,
                            ShowOption = 0,
                            Position = 0
                        };
                    }

                    product.LastModUserGuid = siteUser.UserGuid;
                    product.ZoneId = zoneId;

                    product.Title = productName;
                    product.SubTitle = data.GetDataKeyValue(SubTitleColumnName).ToString();
                    product.BriefContent = data.GetDataKeyValue(BriefContentColumnName).ToString();
                    product.FullContent = data.GetDataKeyValue(FullContentColumnName).ToString();

                    var price = decimal.Zero;
                    if (decimal.TryParse(data.GetDataKeyValue(PriceColumnName).ToString(), out price))
                        product.Price = price;

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

                        if (!isExists)
                        {
                            var pictures = data.GetDataKeyValue(PictureColumnName).ToString();
                            SaveImage(product, vitualImagePaths, pictures);
                        }

                        var deletedProperties = false;
                        foreach (GridColumn col in grid.MasterTableView.AutoGeneratedColumns)
                        {
                            if (col.UniqueName.StartsWith(DynamicFieldColumnName + "-"))
                            {
                                var columRawName = col.UniqueName.Replace(DynamicFieldColumnName + "-", string.Empty);
                                var customField = GetCustomFieldByName(lstCustomFields, columRawName);
                                if (customField == null)
                                {
                                    message.ErrorMessage = "DynamicField cột " + columRawName + " không tồn tại.";
                                    return;
                                }

                                var customFieldValue = data[col].Text;
                                var mustReloadOption = false;

                                if (!deletedProperties && ProductProperty.DeleteByProduct(product.ProductId))
                                    deletedProperties = true;

                                SaveProductProperties(product, node.ZoneGuid, customField, lstCustomFieldOptions, customFieldValue, ref mustReloadOption);
                                if (mustReloadOption)
                                {
                                    lstCustomFieldOptions.Clear();
                                    foreach (var field in lstCustomFields)
                                        lstCustomFieldOptions.AddRange(CustomFieldOption.GetByCustomField(field.CustomFieldId));
                                }
                            }
                        }
                    }

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
                }

                message.SuccessMessage = string.Format("Đã import {0} sản phẩm thành công", numberProductsImported);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public static CustomField GetCustomFieldByName(List<CustomField> lstCustomFields, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                foreach (CustomField field in lstCustomFields)
                {
                    if (StringHelper.IsCaseInsensitiveMatch(field.Name, name))
                        return field;
                }

            return null;
        }

        public static CustomFieldOption GetCustomFieldOptionByName(List<CustomFieldOption> lstCustomFieldOptions, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                foreach (CustomFieldOption option in lstCustomFieldOptions)
                {
                    if (StringHelper.IsCaseInsensitiveMatch(option.Name, name))
                        return option;
                }

            return null;
        }

        private void SaveImage(Product product, string vitualImagePaths, string pictures)
        {
            if (product.ProductGuid == Guid.Empty)
                return;

            var lstImagePaths = pictures.SplitOnCharAndTrim(';');
            if (lstImagePaths.Count > 0)
            {
                var imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, product.ProductId);
                ProductHelper.VerifyProductFolders(fileSystem, imageFolderPath);

                foreach (string imageName in lstImagePaths)
                {
                    var vitualImagePath = vitualImagePaths + imageName;
                    if (fileSystem.FileExists(vitualImagePath))
                    {
                        string physicsPath = Server.MapPath(vitualImagePath);
                        var fs = new FileInfo(physicsPath);

                        var media = new ContentMedia
                        {
                            SiteGuid = siteSettings.SiteGuid,
                            ContentGuid = product.ProductGuid,
                            DisplayOrder = 0,
                            ThumbNailHeight = 100000,
                            ThumbNailWidth = 320,

                            MediaFile = fs.Name.ToAsciiIfPossible()
                        };
                        media.ThumbnailFile = media.MediaFile;

                        string newImagePath = VirtualPathUtility.Combine(imageFolderPath, media.MediaFile);
                        fileSystem.CopyFile(vitualImagePath, newImagePath, true);

                        media.Save();
                        ProductHelper.ProcessImage(media, fileSystem, imageFolderPath, media.MediaFile, ProductHelper.GetColor(displaySettings.ResizeBackgroundColor));
                    }
                }
            }
        }

        private void SaveProductProperties(Product product, Guid zoneGuid, CustomField customField, List<CustomFieldOption> lstCustomFieldOptions, string customFieldValue, ref bool mustReloadOption)
        {
            if (product.ProductGuid == Guid.Empty)
                return;

            if (!string.IsNullOrEmpty(customFieldValue) && customFieldValue != "&nbsp;")
            {
                var lstProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);

                if (customField.DataType == (int)CustomFieldDataType.Text)
                {
                    //ProductProperty.DeleteByProduct(product.ProductId);
                    var property = new ProductProperty
                    {
                        ProductId = product.ProductId,
                        CustomFieldId = customField.CustomFieldId,
                        CustomValue = customFieldValue
                    };
                    property.Save();
                }
                else
                {
                    var customFieldValues = customFieldValue.SplitOnCharAndTrim(';');
                    var lstlstCustomFieldOptionsTemp = lstCustomFieldOptions.Where(s => s.CustomFieldId == customField.CustomFieldId).ToList();
                    for (int i = 0; i < customFieldValues.Count; i++)
                    {
                        var option = GetCustomFieldOptionByName(lstlstCustomFieldOptionsTemp, customFieldValues[i]);
                        if (option == null)
                        {
                            option = new CustomFieldOption
                            {
                                CustomFieldId = customField.CustomFieldId,
                                Name = customFieldValues[i]
                            };
                            option.Save();

                            mustReloadOption = true;
                        }

                        var property = new ProductProperty
                        {
                            ProductId = product.ProductId,
                            CustomFieldId = customField.CustomFieldId,
                            CustomFieldOptionId = option.CustomFieldOptionId
                        };
                        property.Save();

                        if (customField.DataType == (int)CustomFieldDataType.SelectBox)
                            break;
                    }
                }
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
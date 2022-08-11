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
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportProductsCustomPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportProductsCustomPage));
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
        private const string ProductIntroduceColumnName = "Introduce";
        private const string ProductDetailsColumnName = "Details";
        private const string ProductCatalogueColumnName = "Catalogue";
        private const string ProductDescriptionColumnName = "Description";
        private const string ProductImagesColumnName = "Images";
        private const string ProductCatalogColumnName = "Catalog";
        private const string ProductOtherCatalogColumnName = "OtherCatalog";
        private const string ProductManufacturerColumnName = "Manufacturer";
        private const string ProductBuyWithItemNosColumnName = "BuyWithItemNos";
        private const string ProductVideoColumnName = "Video";
        private const string PublishedColumnName = "Published";
        private const string DisplayOrderColumnName = "DisplayOrder";
        private const string EnableBuyButtonColumnName = "EnableBuyButton";
        private const string StartDateColumnName = "StartDate";
        private const string ShowOptionsColumnName = "ShowOptions";
        private const string WeightColumnName = "Weight";

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
                int productIntroduceColumnNo = -1;
                int productDetailsColumnNo = -1;
                int productCatalogueColumnNo = -1;
                int productDescriptionColumnNo = -1;
                int productImagesColumnNo = -1;
                int productCatalogColumnNo = -1;
                int productOtherCatalogColumnNo = -1;
                int productManufacturerColumnNo = -1;
                int productBuyWithItemNosColumnNo = -1;
                int productVideoColumnNo = -1;
                int publishedColumnNo = -1;

                int displayOrderColumnNo = -1;
                int enableBuyButtonColumnNo = -1;
                int startDateColumnNo = -1;
                int showOptionsColumnNo = -1;
                int weightColumnNo = -1;

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

                        case ProductIntroduceColumnName:
                            productIntroduceColumnNo = columNumber;
                            break;

                        case ProductDetailsColumnName:
                            productDetailsColumnNo = columNumber;
                            break;

                        case ProductCatalogueColumnName:
                            productCatalogueColumnNo = columNumber;
                            break;

                        case ProductDescriptionColumnName:
                            productDescriptionColumnNo = columNumber;
                            break;

                        case ProductImagesColumnName:
                            productImagesColumnNo = columNumber;
                            break;

                        case ProductCatalogColumnName:
                            productCatalogColumnNo = columNumber;
                            break;

                        case ProductOtherCatalogColumnName:
                            productOtherCatalogColumnNo = columNumber;
                            break;
                        case ProductManufacturerColumnName:
                            productManufacturerColumnNo = columNumber;
                            break;

                        case ProductBuyWithItemNosColumnName:
                            productBuyWithItemNosColumnNo = columNumber;
                            break;

                        case ProductVideoColumnName:
                            productVideoColumnNo = columNumber;
                            break;

                        case PublishedColumnName:
                            publishedColumnNo = columNumber;
                            break;

                        case DisplayOrderColumnName:
                            displayOrderColumnNo = columNumber;
                            break;

                        case EnableBuyButtonColumnName:
                            enableBuyButtonColumnNo = columNumber;
                            break;

                        case StartDateColumnName:
                            startDateColumnNo = columNumber;
                            break;

                        case ShowOptionsColumnName:
                            showOptionsColumnNo = columNumber;
                            break;

                        case WeightColumnName:
                            weightColumnNo = columNumber;
                            break;
                    }
                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));

                var existsColumnName = "Exists";
                var dt = new DataTable();
                dt.Columns.Add(ProductItemNoColumnName, typeof(string));
                dt.Columns.Add(ProductBarcodeColumnName, typeof(string));
                dt.Columns.Add(ProductIntroduceColumnName, typeof(string));
                dt.Columns.Add(ProductDetailsColumnName, typeof(string));
                dt.Columns.Add(ProductCatalogueColumnName, typeof(string));
                dt.Columns.Add(ProductDescriptionColumnName, typeof(string));
                dt.Columns.Add(ProductImagesColumnName, typeof(string));
                dt.Columns.Add(ProductCatalogColumnName, typeof(string));
                dt.Columns.Add(ProductOtherCatalogColumnName, typeof(string));
                dt.Columns.Add(ProductManufacturerColumnName, typeof(string));
                dt.Columns.Add(ProductBuyWithItemNosColumnName, typeof(string));
                dt.Columns.Add(ProductVideoColumnName, typeof(string));
                dt.Columns.Add(PublishedColumnName, typeof(string));
                dt.Columns.Add(DisplayOrderColumnName, typeof(string));
                dt.Columns.Add(EnableBuyButtonColumnName, typeof(string));
                dt.Columns.Add(StartDateColumnName, typeof(string));
                dt.Columns.Add(ShowOptionsColumnName, typeof(string));
                dt.Columns.Add(WeightColumnName, typeof(string));
                dt.Columns.Add(existsColumnName, typeof(bool));
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string productItemNo = GetValueFromExcel(dataRow.GetCell(productItemNoColumnNo)).Trim();
                        string productBarcode = GetValueFromExcel(dataRow.GetCell(productBarcodeColumnNo)).Trim();
                        string productIntroduce = GetValueFromExcel(dataRow.GetCell(productIntroduceColumnNo)).Trim();
                        string productDetails = GetValueFromExcel(dataRow.GetCell(productDetailsColumnNo)).Trim();
                        string productCatalogue = GetValueFromExcel(dataRow.GetCell(productCatalogueColumnNo)).Trim();
                        string productDescription = GetValueFromExcel(dataRow.GetCell(productDescriptionColumnNo)).Trim();
                        string productImages = GetValueFromExcel(dataRow.GetCell(productImagesColumnNo)).Trim();
                        string productCatalog = GetValueFromExcel(dataRow.GetCell(productCatalogColumnNo)).Trim();
                        string productOtherCatalog = GetValueFromExcel(dataRow.GetCell(productOtherCatalogColumnNo)).Trim();
                        string productManufacturer = GetValueFromExcel(dataRow.GetCell(productManufacturerColumnNo)).Trim();
                        string productBuyWithItemNos = GetValueFromExcel(dataRow.GetCell(productBuyWithItemNosColumnNo)).Trim();
                        string productVideo = GetValueFromExcel(dataRow.GetCell(productVideoColumnNo)).Trim();
                        string strPublished = GetValueFromExcel(dataRow.GetCell(publishedColumnNo)).Trim();
                        string strDisplayOrder = GetValueFromExcel(dataRow.GetCell(displayOrderColumnNo)).Trim();
                        string strEnableBuyButton = GetValueFromExcel(dataRow.GetCell(enableBuyButtonColumnNo)).Trim();
                        string strStartDate = GetValueFromExcel(dataRow.GetCell(startDateColumnNo)).Trim();
                        string strShowOptions = GetValueFromExcel(dataRow.GetCell(showOptionsColumnNo)).Trim();
                        string strWeight = GetValueFromExcel(dataRow.GetCell(weightColumnNo)).Trim();

                        if (string.IsNullOrEmpty(productItemNo))
                            continue;

                        var row = dt.NewRow();
                        row[ProductItemNoColumnName] = productItemNo;
                        row[ProductBarcodeColumnName] = productBarcode;
                        row[ProductIntroduceColumnName] = productIntroduce;
                        row[ProductDetailsColumnName] = productDetails;
                        row[ProductCatalogueColumnName] = productCatalogue;
                        row[ProductDescriptionColumnName] = productDescription;
                        row[ProductImagesColumnName] = productImages;
                        row[ProductCatalogColumnName] = productCatalog;
                        row[ProductOtherCatalogColumnName] = productOtherCatalog;
                        row[ProductManufacturerColumnName] = productManufacturer;
                        row[ProductBuyWithItemNosColumnName] = productBuyWithItemNos;
                        row[ProductVideoColumnName] = productVideo;
                        row[DisplayOrderColumnName] = strDisplayOrder;
                        row[EnableBuyButtonColumnName] = strEnableBuyButton;
                        row[StartDateColumnName] = strStartDate;
                        row[ShowOptionsColumnName] = strShowOptions;
                        row[WeightColumnName] = strWeight;

                        if (!string.IsNullOrEmpty(strPublished))
                            row[PublishedColumnName] = strPublished;
                        var product = Product.GetByCode(siteSettings.SiteId, productItemNo);
                        if (product != null)
                        {
                            row[existsColumnName] = true;
                            if (string.IsNullOrEmpty(strPublished))
                                row[PublishedColumnName] = strPublished;
                        }
                        else
                        {
                            row[existsColumnName] = false;
                            if (string.IsNullOrEmpty(strPublished))
                                row[PublishedColumnName] = strPublished;
                        }
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
                var options = EnumDefined.LoadFromConfigurationXml("product", "showoption", "value");
                foreach (GridDataItem data in grid.Items)
                {
                    string productItemNo = data.GetDataKeyValue(ProductItemNoColumnName).ToString();
                    string productBarcode = data.GetDataKeyValue(ProductBarcodeColumnName).ToString();
                    string productIntroduce = data.GetDataKeyValue(ProductIntroduceColumnName).ToString();
                    string productDetails = data.GetDataKeyValue(ProductDetailsColumnName).ToString();
                    string productCatalogue = data.GetDataKeyValue(ProductCatalogueColumnName).ToString();
                    string productDescription = data.GetDataKeyValue(ProductDescriptionColumnName).ToString();
                    string productImages = data.GetDataKeyValue(ProductImagesColumnName).ToString();
                    string productCatalog = data.GetDataKeyValue(ProductCatalogColumnName).ToString();
                    string productOtherCatalog = data.GetDataKeyValue(ProductOtherCatalogColumnName).ToString();
                    string productManufacturer = data.GetDataKeyValue(ProductManufacturerColumnName).ToString();
                    string productBuyWithItemNos = data.GetDataKeyValue(ProductBuyWithItemNosColumnName).ToString();
                    string productVideo = data.GetDataKeyValue(ProductVideoColumnName).ToString();
                    string productPublished = data.GetDataKeyValue(PublishedColumnName).ToString();
                    string productDisplayOrder = data.GetDataKeyValue(DisplayOrderColumnName).ToString();
                    string productEnableBuyButton = data.GetDataKeyValue(EnableBuyButtonColumnName).ToString();
                    string productStartDate = data.GetDataKeyValue(StartDateColumnName).ToString();
                    string productShowOptions = data.GetDataKeyValue(ShowOptionsColumnName).ToString();
                    string productWeight = data.GetDataKeyValue(WeightColumnName).ToString();

                    if (!string.IsNullOrEmpty(productItemNo))
                    {
                        Product product = Product.GetByCode(siteSettings.SiteId, productItemNo);
                        if (product != null && product.ProductId > 0)
                        {
                            var changedZone = false;
                            int oldZoneId = product.ZoneId;
                            product.BarCode = productBarcode;
                            product.BriefContent = FormatProductIntroduce(productIntroduce, product.BriefContent);
                            product.FullContent = FormatProductFullContent(productDescription, product.FullContent);

                            if (!string.IsNullOrEmpty(productPublished)
                                && productPublished.ToLower() != "noedit"
                                && bool.TryParse(productPublished, out bool published))
                                product.IsPublished = published;

                            if (!string.IsNullOrEmpty(productCatalog)
                                && int.TryParse(productCatalog, out int zoneId))
                            {
                                var gbNode = SiteUtils.GetSiteMapNodeByZoneId(zoneId);
                                if (gbNode != null)
                                {
                                    if (product.ZoneId != zoneId)
                                    {
                                        changedZone = true;
                                        product.ZoneId = zoneId;
                                    }


                                }
                            }
                            if (!string.IsNullOrEmpty(productManufacturer))
                            {
                                var manu = manufacturers.FirstOrDefault(m => m.Name.ToLower().Equals(productManufacturer, StringComparison.OrdinalIgnoreCase));
                                if (manu != null)
                                    product.ManufacturerId = manu.ManufacturerId;
                            }

                            if (!string.IsNullOrEmpty(productDisplayOrder)
                                && int.TryParse(productDisplayOrder, out int displayOrder))
                                product.DisplayOrder = displayOrder;

                            if (!string.IsNullOrEmpty(productEnableBuyButton)
                                && bool.TryParse(productEnableBuyButton, out bool enableBuyButton))
                                product.DisableBuyButton = !enableBuyButton;

                            if (!string.IsNullOrEmpty(productStartDate)
                                && DateTime.TryParse(productStartDate, out DateTime startDate))
                                product.StartDate = startDate;


                            if (!string.IsNullOrEmpty(productShowOptions)
                                && productShowOptions.ToLower() != "noedit")
                            {
                                string[] optionValues = productShowOptions.Split(
                                            new[] { Environment.NewLine, ";", "\n", "\r" },
                                            StringSplitOptions.None
                                        );
                                int showOptions = 0;
                                foreach (var item in optionValues)
                                {
                                    if (string.IsNullOrEmpty(item)) continue;
                                    var it = options.FirstOrDefault(op => op.Name.ToLower().Equals(item.Trim().ToLower()));
                                    if (it != null)
                                    {
                                        int.TryParse(it.Value, out int val);
                                        showOptions |= val;
                                    }
                                }
                                if (showOptions < 0)
                                    showOptions = 0;
                                product.ShowOption = showOptions;
                            }


                            if (!string.IsNullOrEmpty(productWeight)
                                && productWeight.ToLower() != "noedit"
                                && decimal.TryParse(productWeight, out decimal weight))
                            {
                                product.Weight = weight;
                            }


                            product.Save();

                            //Detail
                            SaveProductAttribute(product, productDetails);

                            SaveProductBuyWith(product, productBuyWithItemNos);

                            //Images
                            SaveImages(product, productImages);

                            //Videos
                            SaveVideos(product, productVideo);

                            if (changedZone)
                            {
                                var productZones = ProductZone.SelectAllByProductID(product.ProductId);

                                if (productZones.Any(p => p.ZoneID == oldZoneId))
                                    ProductZone.Delete(product.ProductId, oldZoneId);

                                if (!productZones.Any(p => p.ZoneID == product.ZoneId))
                                    new ProductZone()
                                    {
                                        ProductID = product.ProductId,
                                        ZoneID = product.ZoneId
                                    }.Save();

                                var friendlyUrls = FriendlyUrl.GetByPageGuid(product.ProductGuid);
                                foreach (var item in friendlyUrls)
                                {
                                    item.RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                        + product.ZoneId.ToInvariantString()
                                        + "&ProductID=" + product.ProductId.ToInvariantString();

                                    item.Save();
                                }
                            }

                            SaveProductZone(product, productOtherCatalog);

                            numberProductsImported++;
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
        private void SaveProductZone(Product product, string content)
        {
            if (product.ProductGuid == Guid.Empty)
                return;
            if (string.IsNullOrEmpty(content))
                return;
            if (content.ToLower() == "noedit")
                return;

            string[] lines = content.Split(
                        new[] { Environment.NewLine, ";", "\n", "\r" },
                        StringSplitOptions.None
                    );
            ProductZone.DeleteByProduct(product.ProductId);

            new ProductZone()
            {
                ProductID = product.ProductId,
                ZoneID = product.ZoneId
            }.Save();

            foreach (var catalog in lines)
            {
                if (int.TryParse(catalog, out int zoneId))
                {
                    var gbNode = SiteUtils.GetSiteMapNodeByZoneId(zoneId);
                    if (gbNode != null)
                    {
                        if (product.ZoneId == zoneId) continue;

                        new ProductZone()
                        {
                            ProductID = product.ProductId,
                            ZoneID = zoneId
                        }.Save();
                    }
                }
            }

        }
        private void SaveProductAttribute(Product product, string content)
        {
            if (product.ProductGuid == Guid.Empty)
                return;
            if (content.ToLower() == "noedit") return;

            var tabs = ContentAttribute.GetByContentDesc(product.ProductGuid);
            var tabDetail = tabs.FirstOrDefault(t => t.Title.ToLower().Equals("thông tin chi tiết", StringComparison.OrdinalIgnoreCase));
            if (tabDetail != null)
            {
                tabDetail.ContentText = FormatProductDetail(content);
                tabDetail.Save();
                return;
            }
            else
            {
                new ContentAttribute()
                {
                    ContentText = FormatProductDetail(content),
                    Title = "Thông tin chi tiết",
                    ContentGuid = product.ProductGuid,
                    CreatedUtc = DateTime.Now,
                    DisplayOrder = 0,
                    SiteGuid = siteSettings.SiteGuid,
                }.Save();
            }
        }

        private string FormatProductDetail(string content)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;

            string result = content;
            Regex tagRegex = new Regex(@"<[^>]+>");
            if (!tagRegex.IsMatch(content))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<div class='data-table'>");
                builder.Append("<table>");
                builder.Append("<tbody>");

                string[] lines = content.Split(
                        new[] { Environment.NewLine, ";", "\n", "\r" },
                            StringSplitOptions.None
                        );

                foreach (var item in lines)
                {
                    if (item.Contains(":"))
                        builder.Append("<tr><td>" + item.Split(':')[0] + "</td><td>" + item.Split(':')[1] + "</td></tr>");
                    else
                        builder.Append("<tr>" + item + "</tr>");
                }
                builder.Append("</tbody>");
                builder.Append("</table>");
                builder.Append("</div>");

                result = builder.ToString();
            }
            return result;
        }

        private void SaveProductBuyWith(Product product, string productCodes)
        {
            if (product.ProductGuid == Guid.Empty)
                return;

            if (productCodes.ToLower() == "noedit") return;

            RelatedItem.DeleteByItem(product.ProductGuid);

            if (string.IsNullOrEmpty(productCodes)) return;
            string[] lines = productCodes.Split(
                        new[] { Environment.NewLine, ";", "\n", "\r" },
                        StringSplitOptions.None
                    );
            foreach (var item in lines)
            {
                Product pro = Product.GetByCode(siteSettings.SiteId, item);
                if (pro != null && pro.ProductId > 0)
                    new RelatedItem
                    {
                        SiteGuid = siteSettings.SiteGuid,
                        ItemGuid1 = product.ProductGuid,
                        ItemGuid2 = pro.ProductGuid
                    }.Save();
            }
        }

        #region Images

        private string vitualImagePaths = "/Data/Sites/1/media/dataimports/";

        private void SaveVideos(Product product, string videos)
        {
            if (product.ProductGuid == Guid.Empty)
                return;
            if (string.IsNullOrEmpty(videos)) return;
            if (videos.ToLower() == "noedit") return;

            var medias = ContentMedia.GetByContentDesc(product.ProductGuid);
            foreach (var item in medias)
                if (item.MediaType == (int)ProductMediaType.Video)
                    ContentMedia.Delete(item.Guid);

            string[] lines = videos.Split(
                        new[] { Environment.NewLine, ";", "\n", "\r" },
                        StringSplitOptions.None
                    );
            var imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, product.ProductId);
            ProductHelper.VerifyProductFolders(fileSystem, imageFolderPath);

            foreach (var video in lines)
            {
                if (string.IsNullOrEmpty(video)) continue;
                bool result = Uri.TryCreate(video, UriKind.Absolute, out Uri uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

                string link = video;
                if (!result)
                {
                }
                else
                {
                    ContentMedia media = new ContentMedia
                    {
                        SiteGuid = siteSettings.SiteGuid,
                        ContentGuid = product.ProductGuid,
                        DisplayOrder = 0,
                        ThumbNailHeight = 100000,
                        ThumbNailWidth = 320,
                        MediaType = (int)ProductMediaType.Video,
                        MediaFile = link,
                        ThumbnailFile = string.Empty
                    };
                    media.Save();
                }
            }
        }

        private void SaveImages(Product product, string images)
        {
            if (product.ProductGuid == Guid.Empty)
                return;
            if (string.IsNullOrEmpty(images)) return;

            if (images.ToLower() == "noedit") return;

            var medias = ContentMedia.GetByContentDesc(product.ProductGuid);
            foreach (var item in medias)
                if (item.MediaType == (int)ProductMediaType.Image || item.MediaType == -1)
                    ContentMedia.Delete(item.Guid);
            string[] lines = images.Split(
                        new[] { Environment.NewLine, ";", "\n", "\r" },
                        StringSplitOptions.None
                    );
            var imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, product.ProductId);
            ProductHelper.VerifyProductFolders(fileSystem, imageFolderPath);

            foreach (var picture in lines)
            {
                if (string.IsNullOrEmpty(picture)) continue;
                bool result = Uri.TryCreate(picture, UriKind.Absolute, out Uri uriResult)
                    && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
                if (result)
                {
                    Uri uri = new Uri(picture);
                    DowloandAndSaveImage(product, picture, imageFolderPath, System.IO.Path.GetFileName(uri.LocalPath), 99);
                }
                else
                {
                    SaveImage(product, vitualImagePaths, picture, 99);
                }
            }
        }

        private void DowloandAndSaveImage(Product product, string linkImage, string folderPath, string fileName, int displayOrder = 0)
        {
            if (product.ProductGuid == Guid.Empty)
                return;

            using (WebClient client = new WebClient())
            {
                string fullFileName = Path.Combine(folderPath, fileName);
                client.DownloadFile(new Uri(linkImage), Server.MapPath(fullFileName));

                var media = new ContentMedia
                {
                    SiteGuid = siteSettings.SiteGuid,
                    ContentGuid = product.ProductGuid,
                    DisplayOrder = displayOrder,
                    ThumbNailHeight = 100000,
                    ThumbNailWidth = 320,

                    MediaFile = fileName
                };
                media.ThumbnailFile = media.MediaFile;
                media.Save();
                ProductHelper.ProcessImage(media,
                    fileSystem,
                    folderPath,
                    media.MediaFile,
                    ProductHelper.GetColor(displaySettings.ResizeBackgroundColor));
            }
        }

        private void SaveImage(Product product, string vitualImagePaths, string pictures, int displayOrder = 0)
        {
            var lstImagePaths = pictures.SplitOnCharAndTrim(';');
            if (lstImagePaths.Count > 0)
            {
                var imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, product.ProductId);
                ProductHelper.VerifyProductFolders(fileSystem, imageFolderPath);

                foreach (string imageName in lstImagePaths)
                {
                    if (string.IsNullOrEmpty(imageName)) continue;

                    var vitualImagePath = vitualImagePaths + imageName;
                    if (fileSystem.FileExists(vitualImagePath))
                    {
                        string physicsPath = Server.MapPath(vitualImagePath);
                        var fs = new FileInfo(physicsPath);

                        var media = new ContentMedia
                        {
                            SiteGuid = siteSettings.SiteGuid,
                            ContentGuid = product.ProductGuid,
                            DisplayOrder = displayOrder,
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

        #endregion Images

        private string FormatProductFullContent(string content, string oldContent)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;

            if (content.ToLower() == "noedit") return oldContent;
            string result = content;

            Regex tagRegex = new Regex(@"<[^>]+>");
            if (!tagRegex.IsMatch(content))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<div ");

                string[] lines = content.Split(
                            new[] { Environment.NewLine, "\n", "\r" },
                            StringSplitOptions.None
                        );

                foreach (var item in lines)
                {
                    if (item.Contains(":"))
                        builder.Append("<p><strong>" + item.Split(':')[0] + "</strong>  : " + item.Split(':')[1] + "</p>");
                    else
                        builder.Append("<p>" + item + "</p>");
                }

                builder.Append("</div>");

                result = builder.ToString();
            }

            return result;
        }

        private string FormatProductIntroduce(string content, string oldContent)
        {
            if (string.IsNullOrEmpty(content)) return string.Empty;

            if (content.ToLower() == "noedit") return oldContent;

            string result = content;
            Regex tagRegex = new Regex(@"<[^>]+>");
            if (!tagRegex.IsMatch(content))
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<div class='des-pro'>");
                builder.Append("<ul>");

                string[] lines = content.Split(
                            new[] { Environment.NewLine, "\n", "\r" },
                            StringSplitOptions.None
                        );

                foreach (var item in lines)
                    builder.Append("<li>" + item + "</li>");

                builder.Append("</ul>");
                builder.Append("</div>");

                result = builder.ToString();
            }
            return result;
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
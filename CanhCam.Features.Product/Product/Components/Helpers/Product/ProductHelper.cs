/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23
/// 2015-06-04: added QueryStringKeywordParam
/// 2015-10-23: Enable shopping cart with manufacturer attributes

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.Net;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public static class ProductHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductHelper));
        public static bool EnableProductGroup = ConfigHelper.GetBoolProperty("Product:EnableProductGroup", false);
        public static string ProductGroupKey = "a8da9a82-c2cf-4547-ad12-8416bed52d66";

        public static void ProcessImage(ContentMedia contentImage, IFileSystem fileSystem, string virtualRoot,
                                        string originalFileName, Color backgroundColor)
        {
            string originalPath = virtualRoot + contentImage.MediaFile;
            string thumbnailPath = virtualRoot + "thumbs/" + contentImage.ThumbnailFile;

            fileSystem.CopyFile(originalPath, thumbnailPath, true);

            CanhCam.Web.ImageHelper.ResizeImage(
                thumbnailPath,
                IOHelper.GetMimeType(Path.GetExtension(thumbnailPath)),
                contentImage.ThumbNailWidth,
                contentImage.ThumbNailHeight,
                backgroundColor);
        }

        public static void DeleteImages(ContentMedia contentImage, IFileSystem fileSystem, string virtualRoot)
        {
            string imageVirtualPath = virtualRoot + contentImage.MediaFile;

            if (!contentImage.MediaFile.ContainsCaseInsensitive("/"))
                fileSystem.DeleteFile(imageVirtualPath);

            if (!contentImage.ThumbnailFile.ContainsCaseInsensitive("/"))
            {
                imageVirtualPath = virtualRoot + "thumbs/" + contentImage.ThumbnailFile;
                fileSystem.DeleteFile(imageVirtualPath);
            }
        }

        public static string MediaFolderPath(int siteId)
        {
            return "~/Data/Sites/" + siteId.ToInvariantString() + "/Product/";
        }

        public static string MediaFolderPath(int siteId, int newsId)
        {
            return MediaFolderPath(siteId) + newsId.ToInvariantString() + "/";
        }

        public static string GetMediaFilePath(string mediaFolderPath, string mediaFile)
        {
            if (mediaFile.Length == 0)
                return string.Empty;

            if (mediaFile.Contains("/"))
                return mediaFile;

            return VirtualPathUtility.ToAbsolute(mediaFolderPath) + mediaFile;
        }

        public static string GetImageFilePath(int siteId, int productId, string imageFile, string thumbnail)
        {
            if (imageFile.Length == 0 && thumbnail.Length == 0)
                return string.Empty;

            if (thumbnail.Contains("/"))
                return thumbnail;

            string mediaFolderPath = System.Web.VirtualPathUtility.ToAbsolute(MediaFolderPath(siteId, productId));
            if (!string.IsNullOrEmpty(thumbnail))
                return mediaFolderPath + "thumbs/" + thumbnail;

            if (imageFile.Contains("/"))
                return imageFile;

            return mediaFolderPath + imageFile;
        }

        public static string AttachmentsPath(int siteId, int newsId)
        {
            return MediaFolderPath(siteId, newsId) + "Attachments/";
        }

        public static string FormatProductUrl(string url, int productId, int zoneId)
        {
            if (url.Length > 0)
            {
                if (url.StartsWith("http"))
                {
                    string siteRoot = WebUtils.GetSiteRoot();
                    return url.Replace("http://~", siteRoot).Replace("https://~", siteRoot.Replace("http:", "https"));
                }

                return SiteUtils.GetNavigationSiteRoot(zoneId) + url.Replace("~", string.Empty);
            }

            return SiteUtils.GetNavigationSiteRoot(zoneId) + "/Product/ProductDetail.aspx?zoneid=" + zoneId.ToInvariantString()
                   + "&ProductID=" + productId.ToInvariantString();
        }

        public static string BuildProductLink(Product product)
        {
            return BuildProductLink(product.Url, product.ProductId, product.ZoneId, product.Title);
        }

        public static string BuildProductLink(string url, int productId, int zoneId, string title)
        {
            return string.Format("<a href='{0}'>{1}</a>", ProductHelper.FormatProductUrl(url, productId, zoneId), title);
        }

        public static string BuildProductImageLink(Product product, int imageWidth = 0)
        {
            if (product.ImageFile.Length > 0)
            {
                if (imageWidth <= 0)
                    return string.Format("<a href='{0}'><img src='{1}' title='{2}' /></a>", ProductHelper.FormatProductUrl(product.Url,
                                         product.ProductId, product.ZoneId), VirtualPathUtility.ToAbsolute(ProductHelper.MediaFolderPath(product.SiteId,
                                                 product.ProductId) + product.ImageFile), product.Title);

                return string.Format("<a href='{0}'><img src='{1}' title='{2}' width='{3}' /></a>",
                                     ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId),
                                     VirtualPathUtility.ToAbsolute(ProductHelper.MediaFolderPath(product.SiteId, product.ProductId) + product.ImageFile),
                                     product.Title, imageWidth.ToString());
            }

            return string.Empty;
        }

        public static string BuildEditLink(Product product, CmsBasePage basePage, bool userCanUpdate, SiteUser currentUser)
        {
            if (PageView.UserViewMode == PageViewMode.View) return string.Empty;

            if (!userCanUpdate)
                return string.Empty;

            if (!product.IsPublished)
            {
                if (currentUser == null) return string.Empty;
                if (product.UserId != currentUser.UserId)
                    return string.Empty;
            }

            if (basePage == null || !basePage.UserCanAuthorizeZone(product.ZoneId))
                return string.Empty;

            return "<a title='" + Resources.ProductResources.ProductEditLink
                   + "' class='edit-link' href='" + SiteUtils.GetNavigationSiteRoot() + "/Product/AdminCP/ProductEdit.aspx?ProductID=" +
                   product.ProductId
                   + "'> <i class='fa fa-pencil'></i></a>";
        }

        public static string BuildEditLink(int productId)
        {
            return "<a href='" + SiteUtils.GetNavigationSiteRoot()
                   + "/Product/AdminCP/ProductEdit.aspx?ProductID="
                   + productId
                   + "'>" + Resources.ProductResources.OrderDetailLink + "</a>";
        }

        public static string GetWishlistUrl()
        {
            return SiteUtils.GetNavigationSiteRoot() + "/Product/Wishlist.aspx";
        }

        public static string GetShoppingCartUrl()
        {
            return SiteUtils.GetNavigationSiteRoot() + "/Product/Cart.aspx";
        }

        public static void DeleteFolder(int siteId, int newsId)
        {
            string virtualPath = MediaFolderPath(siteId, newsId);
            string fileSystemPath = HostingEnvironment.MapPath(virtualPath);

            try
            {
                DeleteDirectory(fileSystemPath);
            }
            catch (Exception)
            {
                try
                {
                    System.Threading.Thread.Sleep(0);
                    Directory.Delete(fileSystemPath, false);
                }
                catch (Exception)
                {
                }

                //log.Error(ex);
            }
        }

        public static void DeleteDirectory(string fileSystemPath)
        {
            if (Directory.Exists(fileSystemPath))
            {
                string[] files = Directory.GetFiles(fileSystemPath);
                string[] dirs = Directory.GetDirectories(fileSystemPath);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                while (Directory.GetFiles(fileSystemPath).Length > 0)
                    System.Threading.Thread.Sleep(10);

                foreach (string dir in dirs)
                    DeleteDirectory(dir);

                Directory.Delete(fileSystemPath, true);
            }
        }

        public static bool VerifyProductFolders(IFileSystem fileSystem, string virtualRoot)
        {
            bool result = false;

            string originalPath = virtualRoot;
            string thumbnailPath = virtualRoot + "thumbs/";

            try
            {
                if (!fileSystem.FolderExists(originalPath))
                    fileSystem.CreateFolder(originalPath);

                if (!fileSystem.FolderExists(thumbnailPath))
                    fileSystem.CreateFolder(thumbnailPath);

                result = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error("Error creating directories in ProductHelper.VerifyProductFolders", ex);
            }

            return result;
        }

        public static string GetRangeZoneIdsToSemiColonSeparatedString(int siteId, int parentZoneId)
        {
            SiteMapDataSource siteMapDataSource = new SiteMapDataSource
            {
                SiteMapProvider = "canhcamsite" + siteId.ToInvariantString()
            };

            SiteMapNode rootNode = siteMapDataSource.Provider.RootNode;
            SiteMapNode startingNode = null;

            if (rootNode == null) return null;

            string listChildZoneIds = parentZoneId + ";";

            if (parentZoneId > -1)
            {
                SiteMapNodeCollection allNodes = rootNode.GetAllNodes();
                foreach (SiteMapNode childNode in allNodes)
                {
                    if (!(childNode is gbSiteMapNode gbNode)) continue;

                    if (Convert.ToInt32(gbNode.Key) == parentZoneId)
                    {
                        startingNode = gbNode;
                        break;
                    }
                }
            }
            else
                startingNode = rootNode;

            if (startingNode == null)
                return string.Empty;

            SiteMapNodeCollection childNodes = startingNode.GetAllNodes();
            foreach (gbSiteMapNode childNode in childNodes)
                listChildZoneIds += childNode.Key + ";";

            return listChildZoneIds;
        }

        public static string GetProductTarget(object openInNewWindow)
        {
            if (openInNewWindow != null)
            {
                bool isBlank = (bool)openInNewWindow;

                if (isBlank)
                    return "_blank";
            }

            return "_self";
        }

        public static bool IsParent(Product product)
        {
            if (product == null)
                return true;

            var countChild = Product.GetCountAdv(product.SiteId, parentId: product.ProductId);
            if (countChild > 0)
                return false;

            return true;
        }

        public static Color GetColor(string resizeBackgroundColor)
        {
            try
            {
                return ColorTranslator.FromHtml(resizeBackgroundColor);
            }
            catch (Exception)
            {
            }

            return Color.White;
        }

        //public static Module FindProductModule(PageSettings currentPage)
        //{
        //    foreach (Module m in currentPage.Modules)
        //    {
        //        if (m.FeatureGuid == Product.FeatureGuid) return m;

        //        if (m.ControlSource == "Product/ProductModule.ascx") return m;

        //    }

        //    return null;
        //}

        public static void SendCommentNotification(
            SmtpSettings smtpSettings,
            Guid siteGuid,
            string fromAddress,
            string fromAlias,
            string toEmail,
            string replyEmail,
            string ccEmail,
            string bccEmail,
            string subject,
            string messageTemplate,
            string siteName,
            string messageToken)
        {
            if (string.IsNullOrEmpty(messageTemplate))
                return;

            StringBuilder message = new StringBuilder();
            message.Append(messageTemplate);
            message.Replace("{SiteName}", siteName);
            message.Replace("{Message}", messageToken);
            subject = subject.Replace("{SiteName}", siteName);

            EmailMessageTask messageTask = new EmailMessageTask(smtpSettings)
            {
                SiteGuid = siteGuid,
                EmailFrom = fromAddress,
                EmailFromAlias = fromAlias,
                EmailReplyTo = replyEmail,
                EmailTo = toEmail,
                EmailCc = ccEmail,
                EmailBcc = bccEmail,
                UseHtml = true,
                Subject = subject,
                HtmlBody = message.ToString()
            };
            messageTask.QueueTask();

            WebTaskManager.StartOrResumeTasks();
        }

        #region filter

        public static string BuildFilterUrlLeaveOutPageNumber(string rawUrl, bool fullUrl = false)
        {
            string pageUrl = SiteUtils.BuildUrlLeaveOutParam(rawUrl, ProductHelper.QueryStringPageNumberParam, false);
            return SiteUtils.BuildUrlLeaveOutParam(pageUrl, ProductHelper.QueryStringIsAjaxParam, fullUrl);
        }

        public static string BuildFilterUrlLeaveOutPageNumber(string rawUrl, string paramName, string paramValue)
        {
            string pageUrl = SiteUtils.BuildUrlLeaveOutParam(rawUrl, ProductHelper.QueryStringPageNumberParam, false);
            pageUrl = SiteUtils.BuildUrlLeaveOutParam(pageUrl, ProductHelper.QueryStringIsAjaxParam, false);
            pageUrl = SiteUtils.BuildUrlLeaveOutParam(pageUrl, paramName);

            if (paramValue.Length > 0)
            {
                if (pageUrl.Contains("?"))
                    pageUrl += string.Format("&{0}={1}", paramName, paramValue);
                else
                    pageUrl += string.Format("?{0}={1}", paramName, paramValue);
            }

            return pageUrl;
        }

        public static string GetPriceFromQueryString(out decimal? priceMin, out decimal? priceMax)
        {
            priceMin = null;
            priceMax = null;
            string priceRangeValue = WebUtils.ParseStringFromQueryString(ProductHelper.QueryStringPriceParam, string.Empty);
            if (priceRangeValue.Length > 0)
            {
                var priceRange = priceRangeValue.Split('-');
                if (priceRange.Length == 2)
                {
                    decimal.TryParse(priceRange[0], out decimal priceValue);
                    if (priceValue > 0)
                        priceMin = priceValue;
                    decimal.TryParse(priceRange[1], out priceValue);
                    if (priceValue > 0)
                        priceMax = priceValue;
                }
            }

            return priceRangeValue;
        }

        public const string QueryStringManufacturerParam = "b";
        public const string QueryStringViewModeParam = "view";
        public const string QueryStringSortModeParam = "sort";
        public const string QueryStringPriceParam = "price";
        public const string QueryStringFilterSingleParam = "f";
        public const string QueryStringFilterMultiParam = "mf";
        public const string QueryStringKeywordParam = "keyword";
        public const string QueryStringPageSizeParam = "pagesize";
        public const string QueryStringPageNumberParam = "pagenumber";
        public const string QueryStringIsAjaxParam = "isajax";

        public static string GetQueryStringFilter(string pageUrl, int filterType, int customFieldId, int customFieldOptionId)
        {
            string result = pageUrl;
            string paramName = string.Empty;
            string paramValue = customFieldOptionId.ToString();
            if (filterType == (int)CustomFieldFilterType.ByValue)
                paramName = ProductHelper.QueryStringFilterSingleParam + customFieldId.ToString();
            else
            {
                paramName = ProductHelper.QueryStringFilterMultiParam + customFieldId.ToString();
                paramValue = "/" + paramValue + "/";
            }

            if (pageUrl.Contains("?"))
                result += string.Format("&{0}={1}", paramName, paramValue);
            else
                result += string.Format("?{0}={1}", paramName, paramValue);

            return result;
        }

        #endregion filter

        #region Compare products

        private const string COMPARE_PRODUCTS_COOKIE = "CompareProducts";
        private const string COMPARE_PRODUCTS_COOKIE_VALUE = "CompareProductIds";

        /// <summary>
        /// Clears a "compare products" list
        /// </summary>
        public static void ClearCompareProducts()
        {
            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get(COMPARE_PRODUCTS_COOKIE);
            if (compareCookie != null)
            {
                compareCookie.Values.Clear();
                compareCookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(compareCookie);
            }
        }

        /// <summary>
        /// Gets a "compare products" list
        /// </summary>
        /// <returns>"Compare products" list</returns>
        public static List<Product> GetCompareProducts()
        {
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var productIds = GetCompareProductsIds();

            if (productIds.Count > 0)
            {
                var products = ProductCacheHelper.GetPageAdv(pageNumber: 1, pageSize: productIds.Count, siteId: siteSettings.SiteId, publishStatus: 1, languageId: WorkingCulture.LanguageId, productIds: string.Join(";", productIds.ToArray()), searchProductZone: ProductConfiguration.EnableProductZone);
                return products;
            }

            return new List<Product>();
        }

        /// <summary>
        /// Gets a "compare products" identifier list
        /// </summary>
        /// <returns>"compare products" identifier list</returns>
        public static List<int> GetCompareProductsIds()
        {
            var productIds = new List<int>();
            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get(COMPARE_PRODUCTS_COOKIE);
            if ((compareCookie == null) || (compareCookie.Values == null))
                return productIds;
            string[] values = compareCookie.Values.GetValues(COMPARE_PRODUCTS_COOKIE_VALUE);
            if (values == null)
                return productIds;
            foreach (string productId in values)
            {
                int prodId = int.Parse(productId);
                if (!productIds.Contains(prodId))
                    productIds.Add(prodId);
            }

            return productIds;
        }

        /// <summary>
        /// Removes a manufacturer from a "compare products" list
        /// </summary>
        /// <param name="manufacturerId">Product identifer</param>
        public static void RemoveProductFromCompareList(int productId)
        {
            var oldProductIds = GetCompareProductsIds();
            var newProductIds = new List<int>();
            newProductIds.AddRange(oldProductIds);
            newProductIds.Remove(productId);

            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get(COMPARE_PRODUCTS_COOKIE);
            if ((compareCookie == null) || (compareCookie.Values == null))
                return;
            compareCookie.Values.Clear();
            foreach (int newProductId in newProductIds)
                compareCookie.Values.Add(COMPARE_PRODUCTS_COOKIE_VALUE, newProductId.ToString());
            compareCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(compareCookie);
        }

        /// <summary>
        /// Adds a manufacturer to a "compare products" list
        /// </summary>
        /// <param name="manufacturerId">Product identifer</param>
        public static void AddProductToCompareList(int productId)
        {
            if (!ProductConfiguration.EnableComparing)
                return;

            var oldProductIds = GetCompareProductsIds();
            var newProductIds = new List<int>
            {
                productId
            };
            foreach (int oldProductId in oldProductIds)
                if (oldProductId != productId)
                    newProductIds.Add(oldProductId);

            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get(COMPARE_PRODUCTS_COOKIE);
            if (compareCookie == null)
            {
                compareCookie = new HttpCookie(COMPARE_PRODUCTS_COOKIE)
                {
                    HttpOnly = true
                };
            }
            compareCookie.Values.Clear();
            int maxProducts = ProductConfiguration.MaximumCompareItems;
            int i = 1;
            foreach (int newProductId in newProductIds)
            {
                compareCookie.Values.Add(COMPARE_PRODUCTS_COOKIE_VALUE, newProductId.ToString());
                if (i == maxProducts)
                    break;
                i++;
            }
            compareCookie.Expires = DateTime.Now.AddDays(1.0);
            HttpContext.Current.Response.Cookies.Set(compareCookie);
        }

        #endregion Compare products

        #region recently viewed products

        /// <summary>
        /// Gets a "recently viewed products" list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        public static List<Product> GetRecentlyViewedProducts(int number)
        {
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var products = new List<Product>();
            var productIds = GetRecentlyViewedProductsIds(number);
            if (productIds.Count > 0)
            {
                var lstProducts = Product.GetPageAdv(pageSize: number, siteId: siteSettings.SiteId, publishStatus: 1, productIds: string.Join(";", productIds));
                foreach (var id in productIds)
                {
                    var product = lstProducts.FirstOrDefault(p => p.ProductId == id);
                    if (product != null)
                        products.Add(product);
                }
            }

            return products;
        }

        /// <summary>
        /// Gets a "recently viewed products" identifier list
        /// </summary>
        /// <returns>"recently viewed products" list</returns>
        public static List<int> GetRecentlyViewedProductsIds()
        {
            return GetRecentlyViewedProductsIds(int.MaxValue);
        }

        private static string GetRecentlyViewedProductsCookieName()
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            return "Site-" + siteSettings.SiteId.ToString() + ".RecentlyViewedProducts";
        }

        /// <summary>
        /// Gets a "recently viewed products" identifier list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        public static List<int> GetRecentlyViewedProductsIds(int number)
        {
            var productIds = new List<int>();

            if (HttpContext.Current.Session[GetRecentlyViewedProductsCookieName()] == null)
                return productIds;

            List<int> values = (List<int>)HttpContext.Current.Session[GetRecentlyViewedProductsCookieName()];
            if (values == null)
                return productIds;
            foreach (int productId in values)
            {
                int prodId = productId;
                if (!productIds.Contains(prodId))
                {
                    productIds.Add(prodId);
                    if (productIds.Count >= number)
                        break;
                }
            }

            return productIds;
        }

        /// <summary>
        /// Adds a customer to a recently viewed products list
        /// </summary>
        /// <param name="productId">Product identifier</param>
        public static void AddProductToRecentlyViewedList(int productId)
        {
            if (!ProductConfiguration.RecentlyViewedProductsEnabled)
                return;

            int maxProducts = ProductConfiguration.RecentlyViewedProductCount;
            if (maxProducts <= 0)
                maxProducts = 10;

            var oldProductIds = GetRecentlyViewedProductsIds();
            var newProductIds = new List<int>
            {
                productId
            };
            foreach (int oldProductId in oldProductIds)
                if (oldProductId != productId)
                    newProductIds.Add(oldProductId);

            HttpContext.Current.Session[GetRecentlyViewedProductsCookieName()] = newProductIds;
        }

        #endregion recently viewed products

        public static bool IsAjaxRequest(HttpRequest request)
        { //public static bool IsAjaxRequest(this HttpRequest request)
            if (!WebUtils.ParseBoolFromQueryString(ProductHelper.QueryStringIsAjaxParam, false))
                return false;

            if (request == null)
                throw new ArgumentNullException("request");
            var context = HttpContext.Current;
            var isCallbackRequest = false;// callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page page)
            {
                isCallbackRequest = page.IsCallback;
            }

            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest")
                   || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

        public static string RenderControlToHtml(System.Web.UI.Control control)
        {
            var sb = new StringBuilder();

            try
            {
                var page = new System.Web.UI.Page();
                var form = new System.Web.UI.HtmlControls.HtmlForm();
                form.Controls.Add(control);
                page.Controls.Add(form);

                using (var writer = new StringWriter(sb))
                {
                    HttpContext.Current.Server.Execute(page, writer, false);
                }
            }
            catch (Exception) { }

            return sb.ToString();
        }

        #region Price

        public static decimal GetTotalPriceFromProductList(int siteId, string productIds, bool isPublished)
        {
            var products = Product.GetPageAdv(siteId: siteId, productIds: productIds, publishStatus: isPublished ? 1 : 0);

            var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteId, -10, products);
            decimal total = 0;
            foreach (Product product in products)
            {
                var discountApplied = (DiscountAppliedToItem)null;
                var lstDiscountItemsMayBeApplied = new List<DiscountAppliedToItem>();
                var productPrice = ProductHelper.GetPrice(product, -1, lstDiscountItems, ref discountApplied, ref lstDiscountItemsMayBeApplied);
                total += productPrice;
            }

            return total;
        }

        //public static string FormatPrice(Product manufacturer)
        //{
        //    return FormatPrice(GetPrice(manufacturer));
        //}

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="price">Price</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal price)
        {
            return FormatPrice(price, false);
        }

        public static string FormatPrice(decimal price, bool showCurrency)
        {
            string result = price.ToString(ProductConfiguration.CurrencyFormatting);

            if (showCurrency)
                result += (ProductConfiguration.WorkingCurrency.Length > 0 ? " " + ProductConfiguration.WorkingCurrency : string.Empty);

            return result;
        }

        public static decimal GetPrice(Product product, int productVariantId = -1)
        {
            var discountApplied = (DiscountAppliedToItem)null;
            var lstDiscountItemsMayBeApplied = new List<DiscountAppliedToItem>();
            var lstDiscountItems = DiscountAppliedToItem.GetActive(product.SiteId, -1, new List<Product>() { product });

            return GetPrice(product, productVariantId, lstDiscountItems, ref discountApplied, ref lstDiscountItemsMayBeApplied);
        }

        public static decimal GetPrice(Product product, int productVariantId, ref DiscountAppliedToItem discountApplied, ref List<DiscountAppliedToItem> lstDiscountItemsMayBeApplied)
        {
            var lstDiscountItems = DiscountAppliedToItem.GetActive(product.SiteId, -1, new List<Product>() { product });

            return GetPrice(product, productVariantId, lstDiscountItems, ref discountApplied, ref lstDiscountItemsMayBeApplied);
        }

        public static bool ContainsDiscount(List<DiscountAppliedToItem> lstApplied, int discountId)
        {
            return (lstApplied.Where(s => s.DiscountId == discountId).Count()) > 0;
        }

        public static decimal GetPrice(
            Product product,
            int productVariantId,
            List<DiscountAppliedToItem> lstDiscountItems,
            ref DiscountAppliedToItem discountApplied,
            ref List<DiscountAppliedToItem> lstDiscountItemsMayBeApplied
            )
        {
            var giftProducts = string.Empty;
            var giftProductsSepa = string.Empty;
            var giftCustomProducts = string.Empty;
            var giftCustomProductsSepa = string.Empty;
            var price = GetOriginalPrice(product);
            //var isFirst = true;

            if (productVariantId > 0)
            {
                var variant = new ProductVariant(productVariantId);
                if (variant != null && variant.ProductVariantId > 0)
                    if (variant.Price > 0)
                        price = variant.Price;
            }

            if (lstDiscountItems != null)
            {
                var minPrice = price;

                var lstDiscountItemByProduct = new List<DiscountAppliedToItem>();
                var lstDiscountItemByProductMaybeApplied = new List<DiscountAppliedToItem>();
                foreach (var item in lstDiscountItems)
                {
                    if (
                            (item.ItemId == -1 && (string.IsNullOrEmpty(item.ExcludedZoneIDs) || !(";" + item.ExcludedZoneIDs).Contains(";" + product.ZoneId.ToString() + ";")))
                            || (item.AppliedType == (int)DiscountAppliedType.ToCategories && product.ZoneId == item.ItemId)
                            || (item.AppliedType == (int)DiscountAppliedType.ToProducts && product.ProductId == item.ItemId)
                        )
                    {
                        if (item.DiscountType == (int)DiscountType.ByCatalog)
                        {
                            if (!ContainsDiscount(lstDiscountItemByProduct, item.DiscountId))
                                lstDiscountItemByProduct.Add(item);
                        }
                        else if (item.DiscountType == (int)DiscountType.Deal)
                        {
                            if (
                                    item.DiscountType == (int)DiscountType.Deal
                                    && item.AppliedType == (int)DiscountAppliedType.ToProducts
                                )
                            {
                                if (item.DealQty > 0 && item.SoldQty >= item.DealQty) continue;

                                if (item.FromDate.HasValue && item.FromDate.Value > DateTime.Now) continue;
                                if (item.ToDate.HasValue && item.ToDate.Value < DateTime.Now) continue;

                                if (!ContainsDiscount(lstDiscountItemByProduct, item.DiscountId))
                                    lstDiscountItemByProduct.Add(item);
                            }
                        }
                        else if (item.DiscountOptions > 0)
                        {
                            if (!ContainsDiscount(lstDiscountItemByProductMaybeApplied, item.DiscountId))
                                lstDiscountItemByProductMaybeApplied.Add(item);
                        }
                    }
                }
                if (lstDiscountItemByProduct.Count > 0)
                {
                    var maxPriority = lstDiscountItemByProduct.OrderByDescending(s => s.DiscountPriority).Select(s => s.DiscountPriority).FirstOrDefault();
                    lstDiscountItemByProduct = lstDiscountItemByProduct.Where(s => s.DiscountPriority == maxPriority || (s.DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0).ToList();

                    foreach (var item in lstDiscountItemByProduct)
                    {
                        if (discountApplied == null)
                        {
                            discountApplied = (DiscountAppliedToItem)item.Clone();
                            discountApplied.OriginalPrice = price;
                        }
                        else
                            discountApplied.GiftHtml += item.GiftHtml;

                        var newPrice = 0M;
                        var discountAmount = decimal.Zero;
                        var usePercentageTmp = item.UsePercentage;
                        var discountAmountTmp = item.DiscountAmount;
                        if (item.DiscountAmount <= 0 && item.DiscountAmountParent > 0)
                        {
                            usePercentageTmp = item.UsePercentageParent;
                            discountAmountTmp = item.DiscountAmountParent;
                        }

                        if (usePercentageTmp)
                        {
                            discountAmount = minPrice * discountAmountTmp * (decimal)0.01;
                            if (discountAmount > item.MaximumDiscount && item.MaximumDiscount > 0)
                                discountAmount = item.MaximumDiscount;

                            newPrice = (minPrice - discountAmount);
                        }
                        else
                        {
                            discountAmount = discountAmountTmp;

                            if (discountAmount > item.MaximumDiscount && item.MaximumDiscount > 0)
                                discountAmount = item.MaximumDiscount;

                            newPrice = minPrice - discountAmountTmp;
                        }

                        if (newPrice > 0 && newPrice < minPrice)
                            minPrice = newPrice;

                        if (minPrice < 0)
                            minPrice = 0;
                    }
                }
                if (lstDiscountItemByProductMaybeApplied.Count > 0)
                {
                    var maxPriority = lstDiscountItemByProductMaybeApplied.OrderByDescending(s => s.DiscountPriority).Select(s => s.DiscountPriority).FirstOrDefault();
                    lstDiscountItemByProductMaybeApplied = lstDiscountItemByProductMaybeApplied.Where(s => s.DiscountPriority == maxPriority).ToList();

                    foreach (var item in lstDiscountItemByProductMaybeApplied)
                    {
                        var itemTmp = (DiscountAppliedToItem)item.Clone();
                        itemTmp.OriginalPrice = price;

                        if (item.GiftProducts.Length > 0)
                        {
                            itemTmp.GiftProducts += giftProductsSepa + item.GiftProducts;
                            giftProductsSepa = ";";
                        }
                        if (item.GiftCustomProducts.Length > 0)
                        {
                            itemTmp.GiftCustomProducts += giftCustomProductsSepa + item.GiftCustomProducts;
                            giftCustomProductsSepa = "\n";
                        }

                        var newPrice = 0M;
                        var discountAmount = 0M;
                        if (item.UsePercentage)
                        {
                            discountAmount = minPrice * item.DiscountAmount * (decimal)0.01;
                            if (discountAmount > item.MaximumDiscount && item.MaximumDiscount > 0)
                                discountAmount = item.MaximumDiscount;

                            newPrice = (minPrice - discountAmount);
                        }
                        else
                        {
                            discountAmount = item.DiscountAmount;

                            if (discountAmount > item.MaximumDiscount && item.MaximumDiscount > 0)
                                discountAmount = item.MaximumDiscount;

                            newPrice = minPrice - item.DiscountAmount;
                        }

                        if (newPrice > 0 && newPrice < minPrice)
                        {
                            itemTmp.DiscountPrice = newPrice;
                            itemTmp.DiscountCaculator = minPrice - newPrice;
                            itemTmp.DiscountPercentage = PromotionsHelper.CalculatePercentage(itemTmp.DiscountCaculator, minPrice);
                        }

                        lstDiscountItemsMayBeApplied.Add(itemTmp);
                    }
                }

                //var isShared = (bool?)false;
                //foreach (var item in lstDiscountItems)
                //{
                //    if (item.DiscountType == (int)DiscountType.ByCatalog
                //        || item.DiscountType == (int)DiscountType.Deal
                //        )
                //    {
                //        if (
                //            item.ItemId == -1
                //            || (item.AppliedType == (int)CouponAppliedType.ToCategories && product.ZoneId == item.ItemId)
                //            || (item.AppliedType == (int)CouponAppliedType.ToProducts && product.ProductId == item.ItemId)
                //        )
                //        {
                //            if (
                //                item.DiscountType == (int)DiscountType.Deal
                //                && item.AppliedType == (int)CouponAppliedType.ToProducts
                //                )
                //            {
                //                if (item.DealQty > 0 && item.SoldQty >= item.DealQty) continue;

                //                if (item.FromDate.HasValue && item.FromDate.Value > DateTime.Now) continue;
                //                if (item.ToDate.HasValue && item.ToDate.Value < DateTime.Now) continue;
                //            }

                //            if (!isShared.HasValue) isShared = (item.DiscountShareType & (int)DiscountShareType.Share) > 0;

                //            if (item.GiftProducts.Length > 0)
                //            {
                //                if ((isShared.HasValue && isShared.Value) || (item.DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                //                {
                //                    giftProducts += giftProductsSepa + item.GiftProducts;
                //                    giftProductsSepa = ";";
                //                }
                //                else if (string.IsNullOrEmpty(giftProducts))
                //                    giftProducts = item.GiftProducts;
                //            }
                //            if (item.GiftCustomProducts.Length > 0)
                //            {
                //                if ((isShared.HasValue && isShared.Value) || (item.DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                //                {
                //                    giftCustomProducts += giftCustomProductsSepa + item.GiftCustomProducts;
                //                    giftCustomProductsSepa = "\n";
                //                }
                //                else if (string.IsNullOrEmpty(giftProducts))
                //                    giftCustomProducts = item.GiftCustomProducts;
                //            }

                //            if (discountApplied == null)
                //            {
                //                discountApplied = (DiscountAppliedToItem)item.Clone();
                //                discountApplied.OriginalPrice = price;
                //            }

                //            var newPrice = 0M;
                //            if (item.UsePercentage)
                //                newPrice = (price - price * item.DiscountAmount * (decimal)0.01);
                //            else
                //                newPrice = price - item.DiscountAmount;

                //            if (newPrice < minPrice && newPrice > 0)
                //            {
                //                if (isFirst)
                //                    minPrice = newPrice;
                //                else if ((isShared.HasValue && isShared.Value) || (item.DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                //                    minPrice -= newPrice;

                //                isFirst = false;
                //            }

                //            if (minPrice < 0)
                //                minPrice = 0;
                //        }
                //    }
                //    else if (item.DiscountOptions > 0)
                //    {
                //        if (
                //            item.ItemId == -1
                //            || (item.AppliedType == (int)CouponAppliedType.ToCategories && product.ZoneId == item.ItemId)
                //            || (item.AppliedType == (int)CouponAppliedType.ToProducts && product.ProductId == item.ItemId)
                //        )
                //        {
                //            //if (
                //            //    item.AppliedType == (int)CouponAppliedType.ToProducts
                //            //    && item.DealQty > 0
                //            //    && item.SoldQty >= item.DealQty
                //            //)
                //            //    continue;

                //            var itemTmp = (DiscountAppliedToItem)item.Clone();
                //            itemTmp.OriginalPrice = price;

                //            if (item.GiftProducts.Length > 0)
                //            {
                //                itemTmp.GiftProducts += giftProductsSepa + item.GiftProducts;
                //                giftProductsSepa = ";";
                //            }
                //            if (item.GiftCustomProducts.Length > 0)
                //            {
                //                itemTmp.GiftCustomProducts += giftCustomProductsSepa + item.GiftCustomProducts;
                //                giftCustomProductsSepa = "\n";
                //            }

                //            decimal newPrice = price;
                //            if (minPrice > 0 && (item.DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                //                newPrice = minPrice;
                //            if (item.UsePercentage)
                //                newPrice = (newPrice - newPrice * item.DiscountAmount * (decimal)0.01);
                //            else
                //                newPrice = newPrice - item.DiscountAmount;

                //            if (newPrice > 0 && newPrice < price)
                //            {
                //                itemTmp.DiscountPrice = newPrice;

                //                //if (item.UsePercentage)
                //                //{
                //                //    itemTmp.DiscountPercentage = (int)Math.Round(item.DiscountAmount);
                //                //    itemTmp.DiscountCaculator = (item.DiscountAmount * price) / 100;
                //                //}
                //                //else if (price > 0)
                //                //{
                //                //    itemTmp.DiscountPercentage = PromotionsHelper.CalculatePercentage(item.DiscountAmount, price);
                //                //    itemTmp.DiscountCaculator = item.DiscountAmount;
                //                //}
                //                itemTmp.DiscountCaculator = price - newPrice;
                //                itemTmp.DiscountPercentage = PromotionsHelper.CalculatePercentage(itemTmp.DiscountCaculator, price);
                //            }

                //            lstDiscountItemsMayBeApplied.Add(itemTmp);
                //        }
                //    }
                //}

                if (discountApplied != null)
                {
                    discountApplied.GiftProducts = giftProducts;
                    discountApplied.GiftCustomProducts = giftCustomProducts;
                    discountApplied.DiscountPrice = minPrice;

                    discountApplied.DiscountCaculator = price - minPrice;
                    discountApplied.DiscountPercentage = PromotionsHelper.CalculatePercentage(discountApplied.DiscountCaculator, price);
                }

                price = minPrice;
            }

            return price;
        }

        //public static decimal GetPrice(Product manufacturer, List<DiscountAppliedToItem> lstDiscountItems)
        //{
        //    DiscountAppliedToItem discountApplied = null;
        //    return GetPrice(manufacturer, lstDiscountItems, ref discountApplied);
        //}

        //public static decimal GetPrice(Product manufacturer, ref DiscountAppliedToItem discountApplied)
        //{
        //    List<DiscountAppliedToItem> lstDiscountItems = DiscountAppliedToItem.GetActive(manufacturer.SiteId, -1, new List<Product>() { manufacturer });

        //    return GetPrice(manufacturer, lstDiscountItems, ref discountApplied);
        //}

        public static decimal GetPrice(decimal price, decimal specialPrice, DateTime? specialPriceStartDate,
                                       DateTime? specialPriceEndDate)
        {
            if (specialPrice > 0 && specialPriceStartDate.HasValue && specialPriceEndDate.HasValue)
            {
                if (specialPriceStartDate.Value <= DateTime.UtcNow && DateTime.UtcNow <= specialPriceEndDate)
                    price = specialPrice;
            }

            return price;
        }

        public static decimal GetOriginalPrice(Product product)
        {
            return GetPrice(product.Price, product.SpecialPrice, product.SpecialPriceStartDate, product.SpecialPriceEndDate);
        }

        #endregion Price

        #region XmlData

        public static XmlDocument BuildProductDataXml(XmlDocument doc, XmlElement productXml, Product product,
                List<DiscountAppliedToItem> lstDiscountItems,
                TimeZoneInfo timeZone = null, double timeOffset = -1, string editLink = null, List<int> lstCompareProductIds = null)
        {
            XmlHelper.AddNode(doc, productXml, "Title", product.Title);
            XmlHelper.AddNode(doc, productXml, "SubTitle", product.SubTitle);
            XmlHelper.AddNode(doc, productXml, "Url", ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId));
            XmlHelper.AddNode(doc, productXml, "Target", ProductHelper.GetProductTarget(product.OpenInNewWindow));
            XmlHelper.AddNode(doc, productXml, "ProductId", product.ProductId.ToString());
            XmlHelper.AddNode(doc, productXml, "ZoneId", product.ZoneId.ToString());
            XmlHelper.AddNode(doc, productXml, "StockQuantity", product.StockQuantity.ToString());
            XmlHelper.AddNode(doc, productXml, "ShowOption", product.ShowOption.ToString());

            var productType = ProductTypeCacheHelper.GetById(product.ProductType);
            if (productType != null)
                XmlHelper.AddNode(doc, productXml, "ProductType", productType.Name.ToString());
            XmlHelper.AddNode(doc, productXml, "ProductType", "");
            XmlHelper.AddNode(doc, productXml, "Code", product.Code);
            XmlHelper.AddNode(doc, productXml, "BarCode", product.BarCode);

            SiteMapDataSource siteMapDataSource = new SiteMapDataSource
            {
                SiteMapProvider = "canhcamsite" + product.SiteId.ToInvariantString()
            };

            SiteMapNode rootNode = siteMapDataSource.Provider.RootNode;
            if (product.ZoneId > -1 && rootNode != null)
            {
                SiteMapNodeCollection allNodes = rootNode.GetAllNodes();
                foreach (SiteMapNode childNode in allNodes)
                {
                    if (!(childNode is gbSiteMapNode gbNode)) continue;

                    if (Convert.ToInt32(gbNode.Key) == product.ZoneId)
                    {
                        XmlHelper.AddNode(doc, productXml, "CategoryDescription", gbNode.Description);
                        XmlHelper.AddNode(doc, productXml, "Category", gbNode.Title);
                        break;
                    }
                }
            }
            if (product.ManufacturerId > 0)
            {
                var manufacturer = ManufacturerCacheHelper.GetById(product.ManufacturerId);
                if (manufacturer != null && manufacturer.ManufacturerId > 0)
                {
                    XmlHelper.AddNode(doc, productXml, "ManufacturerTitle", manufacturer.Name);
                    XmlHelper.AddNode(doc, productXml, "ManufacturerDescription", manufacturer.Description);
                    XmlHelper.AddNode(doc, productXml, "ManufacturerUrl", ManufacturerHelper.FormatManufacturerUrl(manufacturer.Url, manufacturer.ManufacturerId));
                    XmlHelper.AddNode(doc, productXml, "ManufacturerImageUrl", manufacturer.PrimaryImage);
                    XmlHelper.AddNode(doc, productXml, "ManufacturerThumnailUrl", manufacturer.SecondImage);
                }
            }

            var discountApplied = (DiscountAppliedToItem)null;
            var lstDiscountItemsMayBeApplied = new List<DiscountAppliedToItem>();
            var productPrice = ProductHelper.GetPrice(product, -1, lstDiscountItems, ref discountApplied, ref lstDiscountItemsMayBeApplied);
            if (productPrice > 0)
                XmlHelper.AddNode(doc, productXml, "Price", ProductHelper.FormatPrice(productPrice, true));
            if (productPrice < 0)
                productPrice = 0;
            XmlHelper.AddNode(doc, productXml, "PriceFormatWithoutCurrency", productPrice.ToString("#"));
            if (product.OldPrice > 0)
            {
                XmlHelper.AddNode(doc, productXml, "OldPrice", ProductHelper.FormatPrice(product.OldPrice, true));

                if (product.Price > 0 && product.OldPrice > product.Price)
                {
                    var discountPercent = PromotionsHelper.CalculatePercentage(product.OldPrice - product.Price, product.OldPrice);
                    XmlHelper.AddNode(doc, productXml, "OldPriceDiscountPercentage", discountPercent.ToString());
                }
            }

            if (discountApplied != null)
            {
                if (discountApplied.DiscountPercentage > 0)
                {
                    XmlHelper.AddNode(doc, productXml, "DiscountPercentage", discountApplied.DiscountPercentage.ToString());

                    if (discountApplied.OriginalPrice > productPrice)
                    {
                        if (productXml["OldPrice"] != null)
                            productXml["OldPrice"].InnerText = ProductHelper.FormatPrice(discountApplied.OriginalPrice, true);
                        else
                            XmlHelper.AddNode(doc, productXml, "OldPrice", ProductHelper.FormatPrice(discountApplied.OriginalPrice, true));
                    }
                }

                XmlHelper.AddNode(doc, productXml, "GiftDescription", discountApplied.GiftHtml);
                XmlHelper.AddNode(doc, productXml, "GiftDescription2", discountApplied.GiftDescription);
                XmlHelper.AddNode(doc, productXml, "DiscountAmount", ProductHelper.FormatPrice(discountApplied.DiscountCaculator, true));

                //int remainingQty = (discountApplied.DealQty - discountApplied.SoldQty);
                //if (discountApplied.SoldQty >= 0)
                //    XmlHelper.AddNode(doc, productXml, "SoldQty", discountApplied.SoldQty.ToString());
                //if (remainingQty >= 0)
                //    XmlHelper.AddNode(doc, productXml, "RemainingQty", remainingQty.ToString());

                if (discountApplied != null && discountApplied.DiscountType == (int)DiscountType.Deal)
                {
                    var remainingQty = (discountApplied.DealQty - discountApplied.SoldQty);
                    if (discountApplied.DealQty > 0 && discountApplied.SoldQty >= 0)
                    {
                        var soldQty = discountApplied.SoldQty;
                        if (discountApplied.SoldQty > discountApplied.DealQty)
                            soldQty = discountApplied.DealQty;
                        var percent = PromotionsHelper.CalculatePercentage((decimal)soldQty, (decimal)discountApplied.DealQty);

                        XmlHelper.AddNode(doc, productXml, "DealSoldQty", soldQty.ToString());
                        XmlHelper.AddNode(doc, productXml, "DealSoldPercent", percent.ToString());
                        XmlHelper.AddNode(doc, productXml, "DealTotalQty", discountApplied.DealQty.ToString());
                        XmlHelper.AddNode(doc, productXml, "DealRemainingQty", remainingQty.ToString());
                    }

                    var date = (DateTime?)null;
                    if (discountApplied.ToDate.HasValue)
                        date = discountApplied.ToDate.Value;
                    if (discountApplied.DiscountEndDate.HasValue)
                    {
                        if (date == null || date > discountApplied.DiscountEndDate.Value)
                            date = discountApplied.DiscountEndDate.Value;
                    }
                    if (date != null)
                        BuildDealDateXml(doc, productXml, "DealEnd", date.Value);

                    date = (DateTime?)null;
                    if (discountApplied.FromDate.HasValue)
                        date = discountApplied.FromDate.Value;
                    if (discountApplied.DiscountStartDate.HasValue)
                    {
                        if (date == null || date < discountApplied.DiscountStartDate.Value)
                            date = discountApplied.DiscountStartDate.Value;
                    }
                    if (date != null)
                        BuildDealDateXml(doc, productXml, "DealStart", date.Value);
                }
            }
            if (lstDiscountItemsMayBeApplied != null && lstDiscountItemsMayBeApplied.Count > 0)
            {
                foreach (var dia in lstDiscountItemsMayBeApplied)
                {
                    var promotionXml = doc.CreateElement("Promotion");
                    productXml.AppendChild(promotionXml);

                    XmlHelper.AddNode(doc, promotionXml, "Option", dia.DiscountOptions.ToString());
                    if (dia.DiscountPrice > 0)
                        XmlHelper.AddNode(doc, promotionXml, "DiscountPrice", ProductHelper.FormatPrice(dia.DiscountPrice, true));
                }
            }
            if (lstDiscountItems != null)
                foreach (var item in lstDiscountItems)
                {
                    if (item.DiscountType == (int)DiscountType.ComboSale && item.ItemId == product.ProductId)
                    {
                        XmlHelper.AddNode(doc, productXml, "HasCombo", "true");
                        break;
                    }
                }

            string imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
            string thumbnailImageFolderPath = imageFolderPath + "thumbs/";
            if (product.ImageFile.Length > 0)
                XmlHelper.AddNode(doc, productXml, "ImageUrl", ProductHelper.GetMediaFilePath(imageFolderPath, product.ImageFile));
            if (product.ThumbnailFile.Length > 0)
                XmlHelper.AddNode(doc, productXml, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath,
                                  product.ThumbnailFile));

            XmlHelper.AddNode(doc, productXml, "BriefContent", product.BriefContent);
            XmlHelper.AddNode(doc, productXml, "FullContent", product.FullContent);
            XmlHelper.AddNode(doc, productXml, "ViewCount", product.ViewCount.ToString());
            XmlHelper.AddNode(doc, productXml, "CommentCount", product.CommentCount.ToString());

            XmlHelper.AddNode(doc, productXml, "BarCode", product.BarCode);

            if (!string.IsNullOrEmpty(editLink))
                XmlHelper.AddNode(doc, productXml, "EditLink", editLink);

            if (timeZone != null && timeOffset > -1)
            {
                object startDate = product.StartDate;
                XmlHelper.AddNode(doc, productXml, "CreatedDate", FormatDate(startDate, timeZone, timeOffset,
                                  ResourceHelper.GetResourceString("ProductResources", "ProductDateFormat")));
                XmlHelper.AddNode(doc, productXml, "CreatedTime", FormatDate(startDate, timeZone, timeOffset,
                                  ResourceHelper.GetResourceString("ProductResources", "ProductTimeFormat")));
                XmlHelper.AddNode(doc, productXml, "CreatedDD", FormatDate(startDate, timeZone, timeOffset, "dd"));
                XmlHelper.AddNode(doc, productXml, "CreatedYY", FormatDate(startDate, timeZone, timeOffset, "yy"));
                XmlHelper.AddNode(doc, productXml, "CreatedYYYY", FormatDate(startDate, timeZone, timeOffset, "yyyy"));
                XmlHelper.AddNode(doc, productXml, "CreatedMM", FormatDate(startDate, timeZone, timeOffset, "MM"));
                if (System.Globalization.CultureInfo.CurrentCulture.Name.ToLower() == "vi-vn")
                {
                    string monthVI = "Tháng " + FormatDate(startDate, timeZone, timeOffset, "MM");
                    XmlHelper.AddNode(doc, productXml, "CreatedMMM", monthVI);
                    XmlHelper.AddNode(doc, productXml, "CreatedMMMM", monthVI);
                }
                else
                {
                    XmlHelper.AddNode(doc, productXml, "CreatedMMM", FormatDate(startDate, timeZone, timeOffset, "MMM"));
                    XmlHelper.AddNode(doc, productXml, "CreatedMMMM", FormatDate(startDate, timeZone, timeOffset, "MMMM"));
                }
            }

            XmlHelper.AddNode(doc, productXml, "AddedWishList", WishlistCacheHelper.ExistWishList(product.SiteId, product.ProductId).ToString());

            if (lstCompareProductIds != null
                && lstCompareProductIds.Count > 0)
            {
                if (lstCompareProductIds.Contains(product.ProductId))
                    XmlHelper.AddNode(doc, productXml, "AddedCompare", "true");
            }

            return doc;
        }

        public static void BuildDealDateXml(XmlDocument doc, XmlElement productXml, string prefix, DateTime date)
        {
            XmlHelper.AddNode(doc, productXml, prefix + "Day", date.ToString("dd"));
            XmlHelper.AddNode(doc, productXml, prefix + "Month", date.ToString("MM"));
            XmlHelper.AddNode(doc, productXml, prefix + "Year", date.ToString("yyyy"));
            XmlHelper.AddNode(doc, productXml, prefix + "Hour", date.ToString("HH"));
            XmlHelper.AddNode(doc, productXml, prefix + "Minute", date.ToString("mm"));
            XmlHelper.AddNode(doc, productXml, prefix + "Second", date.ToString("ss"));
        }

        public static string FormatDate(object startDate, TimeZoneInfo timeZone, double timeOffset, string format = "")
        {
            if (startDate == null)
                return string.Empty;

            if (timeZone != null)
                return TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(startDate), timeZone).ToString(format);

            return Convert.ToDateTime(startDate).AddHours(timeOffset).ToString(format);
        }

        #endregion XmlData

        public static Product GetProductFromList(List<Product> lstProducts, int productId)
        {
            foreach (Product pd in lstProducts)
            {
                if (pd.ProductId == productId)
                    return pd;
            }

            return null;
        }

        #region Orders

        public static string GenerateOrderCode(int siteId)
        {
            //var number = 0;
            //try
            //{
            //    DateTime fromdate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day);
            //    DateTime todate = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 23, 59, 59);

            //    var dateFormatLength = ProductConfiguration.OrderCodeDateFormat.Length;
            //    var order = Order.GetByDate(siteId, fromdate, todate);
            //    if (order != null && order.OrderCode.Length >= (dateFormatLength + ProductConfiguration.OrderCodeMinimumLength))
            //        number = Convert.ToInt32(order.OrderCode.Remove(0, dateFormatLength));
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);
            //}

            //return DateTime.Now.ToString(ProductConfiguration.OrderCodeDateFormat) + (number + 1).ToString().PadLeft(
            //           ProductConfiguration.OrderCodeMinimumLength, '0');

            var orderCode = string.Empty;
            var number = 0;
            var timeOffset = SiteUtils.GetUserTimeOffset();
            var dtNow = DateTime.UtcNow.AddHours(timeOffset);
            try
            {
                var fromdate = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 0, 0, 0).AddHours(-timeOffset);
                var todate = new DateTime(dtNow.Year, dtNow.Month, dtNow.Day, 23, 59, 59).AddHours(-timeOffset);

                var dateFormatLength = ProductConfiguration.OrderCodeDateFormat.Length;
                var order = Order.GetByDate(siteId, fromdate, todate);
                if (order != null && order.OrderCode.Length >= (dateFormatLength + ProductConfiguration.OrderCodeMinimumLength))
                    number = Convert.ToInt32(order.OrderCode.Remove(0, dateFormatLength));

                var maximumLoop = 0;
                do
                {
                    number += 1;
                    orderCode = dtNow.ToString(ProductConfiguration.OrderCodeDateFormat) + number.ToString().PadLeft(ProductConfiguration.OrderCodeMinimumLength, '0');

                    maximumLoop++;
                    if (maximumLoop >= 10) break;
                } while (Order.GetByCode(orderCode) != null);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return orderCode;

            //return DateTime.Now.ToString(ProductConfiguration.OrderCodeDateFormat) + (number + 1).ToString().PadLeft(
            //           ProductConfiguration.OrderCodeMinimumLength, '0');
        }

        public static bool SendOrderPlacedNotification(
            SiteSettings siteSettings,
            Order order,
            List<Product> lstProductInOrder,
            List<OrderItem> lstOrderItems,
            string emailTemplate,
            string billingProvinceName,
            string billingDistrictName,
            string shippingProvinceName,
            string shippingDistrictName,
            string toEmail = "",
            bool isRestricted = false)
        {
            if (order == null)
                return false;

            EmailTemplate template = EmailTemplate.Get(siteSettings.SiteId, emailTemplate, WorkingCulture.LanguageId);

            if (template == null || template.Guid == Guid.Empty)
                return false;

            string fromAlias = template.FromName;
            if (fromAlias.Length == 0)
                fromAlias = siteSettings.DefaultFromEmailAlias;

            StringBuilder message = new StringBuilder();
            message.Append(template.HtmlBody);

            message.Replace("{SiteName}", siteSettings.SiteName);

            message.Replace("{BillingFirstName}", order.BillingFirstName);
            message.Replace("{BillingLastName}", order.BillingLastName);
            message.Replace("{BillingEmail}", order.BillingEmail);
            message.Replace("{BillingMobile}", order.BillingMobile);
            message.Replace("{BillingPhone}", order.BillingPhone);
            message.Replace("{BillingAddress}", order.BillingAddress);

            message.Replace("{ShippingFirstName}", order.ShippingFirstName);
            message.Replace("{ShippingLastName}", order.ShippingLastName);
            message.Replace("{ShippingMobile}", order.ShippingMobile);
            message.Replace("{ShippingEmail}", order.ShippingEmail);
            message.Replace("{ShippingPhone}", order.ShippingPhone);
            message.Replace("{ShippingAddress}", order.ShippingAddress);

            message.Replace("{InvoiceCompanyName}", order.InvoiceCompanyName);
            message.Replace("{InvoiceCompanyAddress}", order.InvoiceCompanyAddress);
            message.Replace("{InvoiceCompanyTaxCode}", order.InvoiceCompanyTaxCode);

            message.Replace("{OrderStatus}", ProductHelper.GetOrderStatus(order.OrderStatus));

            string shippingMethod = string.Empty;
            if (order.ShippingMethod > 0)
            {
                ShippingMethod method = new ShippingMethod(order.ShippingMethod);
                if (method != null && method.ShippingMethodId > 0)
                    shippingMethod = method.Name;
            }
            string paymentMethod = string.Empty;
            if (order.PaymentMethod > 0)
            {
                PaymentMethod method = new PaymentMethod(order.PaymentMethod);
                if (method != null && method.PaymentMethodId > 0)
                    paymentMethod = method.Name;
            }
            message.Replace("{PaymentMethod}", paymentMethod);
            message.Replace("{ShippingMethod}", shippingMethod);

            var detail = string.Empty;
            if (lstOrderItems.Count > 0)
            {
                List<ProductProperty> productProperties = new List<ProductProperty>();
                List<CustomField> customFields = new List<CustomField>();
                if (ProductConfiguration.EnableShoppingCartAttributes)
                {
                    var lstProductIds = lstOrderItems.Select(x => x.ProductId).Distinct().ToList();
                    customFields = CustomField.GetActiveForCart(order.SiteId, Product.FeatureGuid);
                    if (customFields.Count > 0 && lstProductIds.Count > 0)
                        productProperties = ProductProperty.GetPropertiesByProducts(lstProductIds);
                }

                XmlDocument doc = new XmlDocument
                {
                    XmlResolver = null
                };

                doc.LoadXml("<OrderDetails></OrderDetails>");
                XmlElement root = doc.DocumentElement;

                XmlHelper.AddNode(doc, root, "OrderTotal", ProductHelper.FormatPrice(order.OrderTotal, true));
                XmlHelper.AddNode(doc, root, "OrderSubTotal", ProductHelper.FormatPrice(order.OrderSubtotal, true));
                XmlHelper.AddNode(doc, root, "OrderDiscount", ProductHelper.FormatPrice(-order.OrderDiscount, true));
                XmlHelper.AddNode(doc, root, "OrderCouponAmount", ProductHelper.FormatPrice(-order.OrderCouponAmount, true));
                XmlHelper.AddNode(doc, root, "ShippingFee", ProductHelper.FormatPrice(order.OrderShipping, true));
                XmlHelper.AddNode(doc, root, "RedeemedRewardPoints", order.RedeemedRewardPoints.ToString());
                XmlHelper.AddNode(doc, root, "RedeemedRewardPointsAmount", ProductHelper.FormatPrice(-order.RedeemedRewardPointsAmount, true));

                XmlHelper.AddNode(doc, root, "VoucherAmount", ProductHelper.FormatPrice(-order.VoucherAmount, true));
                XmlHelper.AddNode(doc, root, "PaymentFee", ProductHelper.FormatPrice(order.OrderTax, true));
                XmlHelper.AddNode(doc, root, "ServiceFee", ProductHelper.FormatPrice(order.OrderServiceFee, true));
                XmlHelper.AddNode(doc, root, "IsRestricted", isRestricted.ToString());

                var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProductInOrder);
                foreach (OrderItem orderItems in lstOrderItems)
                {
                    var product = GetProductFromList(lstProductInOrder, orderItems.ProductId);
                    if (product != null)
                    {
                        XmlElement orderItemsXml = doc.CreateElement("OrderItems");
                        root.AppendChild(orderItemsXml);

                        ProductHelper.BuildProductDataXml(doc, orderItemsXml, product, lstDiscountItems);

                        // Order detail
                        if (orderItemsXml["Price"] != null)
                            orderItemsXml["Price"].InnerText = ProductHelper.FormatPrice(orderItems.Price, true);
                        else
                            XmlHelper.AddNode(doc, orderItemsXml, "Price", ProductHelper.FormatPrice(orderItems.Price, true));

                        XmlHelper.AddNode(doc, orderItemsXml, "Quantity", orderItems.Quantity.ToString());
                        XmlHelper.AddNode(doc, orderItemsXml, "Discount", ProductHelper.FormatPrice(orderItems.DiscountAmount, true));
                        XmlHelper.AddNode(doc, orderItemsXml, "ItemSubTotal", ProductHelper.FormatPrice(orderItems.Quantity * orderItems.Price, true));
                        XmlHelper.AddNode(doc, orderItemsXml, "ItemTotal", ProductHelper.FormatPrice(orderItems.Quantity * orderItems.Price - orderItems.DiscountAmount, true));

                        if (!string.IsNullOrEmpty(orderItems.AttributesXml))
                        {
                            var attributes = ProductAttributeParser.ParseProductAttributeMappings(customFields, orderItems.AttributesXml);
                            if (attributes.Count > 0)
                            {
                                foreach (var a in attributes)
                                {
                                    XmlElement attributeXml = doc.CreateElement("Attributes");
                                    orderItemsXml.AppendChild(attributeXml);

                                    XmlHelper.AddNode(doc, attributeXml, "Title", a.Name);

                                    var values = ProductAttributeParser.ParseValues(orderItems.AttributesXml, a.CustomFieldId);
                                    foreach (ProductProperty property in productProperties)
                                    {
                                        if (property.ProductId == product.ProductId
                                            && property.CustomFieldId == a.CustomFieldId
                                            && values.Contains(property.CustomFieldOptionId))
                                        {
                                            XmlElement optionXml = doc.CreateElement("Options");
                                            attributeXml.AppendChild(optionXml);

                                            XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                detail += XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Product", "OrderDetailsTemplate.xslt"), doc);
            }

            message.Replace("{OrderDetails}", detail);
            message.Replace("{OrderUrl}", string.Empty);
            message.Replace("{OrderNumber}", order.OrderId.ToString());
            message.Replace("{OrderCode}", order.OrderCode);
            message.Replace("{OrderNote}", order.OrderNote);
            message.Replace("{CreatedOn}", DateTimeHelper.Format(Convert.ToDateTime(order.CreatedUtc), SiteUtils.GetUserTimeZone(),
                            "dd/MM/yyyy HH:mm", SiteUtils.GetUserTimeOffset()));

            message.Replace("{OrderTotal}", ProductHelper.FormatPrice(order.OrderTotal, true));
            message.Replace("{OrderSubTotal}", ProductHelper.FormatPrice(order.OrderSubtotal, true));
            message.Replace("{OrderDiscount}", ProductHelper.FormatPrice(order.OrderDiscount, true));
            message.Replace("{ShippingFee}", ProductHelper.FormatPrice(order.OrderShipping, true));
            message.Replace("{CouponCode}", order.CouponCode);

            message.Replace("{BillingProvinceName}", billingProvinceName);
            message.Replace("{BillingDistrictName}", billingDistrictName);
            message.Replace("{ShippingProvinceName}", shippingProvinceName);
            message.Replace("{ShippingDistrictName}", shippingDistrictName);

            SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();

            string subjectEmail = template.Subject.Replace("{SiteName}", siteSettings.SiteName).Replace("{OrderCode}",
                                  order.OrderCode).Replace("{OrderNumber}", order.OrderId.ToString());

            EmailMessageTask messageTask = new EmailMessageTask(smtpSettings)
            {
                EmailFrom = siteSettings.DefaultEmailFromAddress,
                EmailTo = toEmail + (template.ToAddresses.Length == 0 ? string.Empty : "," + template.ToAddresses),
                EmailCc = (template.CcAddresses.Length == 0 ? string.Empty : "," + template.CcAddresses),
                EmailBcc = (template.BccAddresses.Length == 0 ? string.Empty : "," + template.BccAddresses),
                EmailReplyTo = template.ReplyToAddress,
                EmailFromAlias = template.FromName,
                UseHtml = true,
                SiteGuid = siteSettings.SiteGuid,
                Subject = subjectEmail,
                HtmlBody = message.ToString()
            };
            messageTask.QueueTask();

            return true;
        }

        public static string GetOrderStatus(int orderStatus)
        {
            return OrderHelper.GetOrderStatusResources(orderStatus);
        }

        public static void PopulateOrderSource(DropDownList ddOrderSource, bool addAll)
        {
            ddOrderSource.Items.Clear();

            if (addAll)
                ddOrderSource.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "OrderSourceAll"), "-1"));

            ddOrderSource.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "OrderSourceWebsite"), "0"));
            ddOrderSource.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "OrderSourceHotline"), "10"));
            ddOrderSource.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "OrderSourceCallCentre"), "20"));
        }

        public static string GetOrderSource(string source)
        {
            switch (source)
            {
                case "0":
                    return ResourceHelper.GetResourceString("ProductResources", "OrderSourceWebsite");

                case "10":
                    return ResourceHelper.GetResourceString("ProductResources", "OrderSourceHotline");

                case "20":
                    return ResourceHelper.GetResourceString("ProductResources", "OrderSourceCallCentre");
            }
            return string.Empty;
        }

        #endregion Orders

        #region Shipping

        public static string GetShippingGeoZoneGuidsByOrderSession(Order order)
        {
            string geoZoneGuids = string.Empty;
            if (order != null)
            {
                if (order.ShippingProvinceGuid != Guid.Empty)
                    geoZoneGuids += order.ShippingProvinceGuid + ";";
                if (order.ShippingDistrictGuid != Guid.Empty)
                    geoZoneGuids += order.ShippingDistrictGuid + ";";

                if (geoZoneGuids.Length == 0)
                {
                    if (order.BillingProvinceGuid != Guid.Empty)
                        geoZoneGuids += order.BillingProvinceGuid + ";";
                    if (order.BillingDistrictGuid != Guid.Empty)
                        geoZoneGuids += order.BillingDistrictGuid + ";";
                }
            }

            if (geoZoneGuids.Length == 0)
                return null;

            return geoZoneGuids;
        }

        public static decimal GetShippingPrice(
            int shippingMethodId,
            decimal orderSubTotal,
            decimal orderWeight,
            int productTotalQty,
            string geoZoneGuids,
            Guid? BillingDistrictGuid,
            string shippingOption,
            int storeId,
            Store store,
            ref string expectedTime,
            decimal orderLength = 0,
            decimal orderWidth = 0,
            decimal orderHeight = 0)
        {
            ShippingMethod method = new ShippingMethod(shippingMethodId);
            if (!ConfigHelper.GetBoolProperty("Store:ShippngFeeForStore", false))
                storeId = -1;
            if (store != null && store.StoreID <= 0)
                store = null;

            return GetShippingPrice(method, orderSubTotal, orderWeight, productTotalQty,
                geoZoneGuids, ref expectedTime, BillingDistrictGuid, shippingOption, storeId, store, orderLength, orderWidth, orderHeight);
        }

        public static decimal GetShippingPrice(
            ShippingMethod method,
            decimal orderSubTotal,
            decimal orderWeight,
            int productTotalQty,
            string geoZoneGuids,
            ref string expectedTime,
            Guid? BillingDistrictGuid = null,
            string shippingOption = "",
            int storeId = -1,
            Store store = null,
            decimal orderLength = 0,
            decimal orderWidth = 0,
            decimal orderHeight = 0)
        {
            if (method != null && method.ShippingMethodId > 0 && !method.IsDeleted && method.IsActive)
            {
                switch ((ShippingMethodProvider)method.ShippingProvider)
                {
                    case ShippingMethodProvider.Free:
                        return decimal.Zero;

                    case ShippingMethodProvider.Fixed:
                        if (method.FreeShippingOverXEnabled && orderSubTotal >= method.FreeShippingOverXValue && method.FreeShippingOverXValue > 0)
                            return decimal.Zero;
                        return method.ShippingFee;

                    case ShippingMethodProvider.FixedPerItem:
                        if (method.FreeShippingOverXEnabled && orderSubTotal >= method.FreeShippingOverXValue && method.FreeShippingOverXValue > 0)
                            return decimal.Zero;
                        return method.ShippingFee * productTotalQty;

                    case ShippingMethodProvider.ByOrderTotal:
                        ShippingTableRate tableRate = ShippingTableRate.GetOneMaxValue(method.ShippingMethodId, orderSubTotal, null, storeId);
                        if (tableRate != null)
                            return tableRate.ShippingFee;

                        return method.ShippingFee;

                    case ShippingMethodProvider.ByWeight:
                        if (method.FreeShippingOverXEnabled && orderSubTotal >= method.FreeShippingOverXValue && method.FreeShippingOverXValue > 0)
                            return decimal.Zero;
                        tableRate = ShippingTableRate.GetOneMaxValue(method.ShippingMethodId, orderWeight, null, storeId);
                        if (tableRate != null)
                        {
                            if (tableRate.AdditionalFee >= 0 && tableRate.AdditionalValue > 0)
                            {
                                decimal fromValue = tableRate.FromValue;
                                decimal shippingFee = tableRate.ShippingFee;
                                while (orderWeight > fromValue)
                                {
                                    fromValue += tableRate.AdditionalValue;
                                    shippingFee += tableRate.AdditionalFee;
                                }

                                return shippingFee;
                            }

                            return tableRate.ShippingFee;
                        }

                        return method.ShippingFee;

                    case ShippingMethodProvider.ByGeoZoneAndFixed:
                        tableRate = ShippingTableRate.GetOneMaxValue(method.ShippingMethodId, -1, geoZoneGuids, storeId);
                        if (tableRate != null)
                        {
                            if (method.FreeShippingOverXEnabled && orderSubTotal >= tableRate.FreeShippingOverXValue
                                && tableRate.FreeShippingOverXValue > 0)
                                return decimal.Zero;

                            return tableRate.ShippingFee;
                        }

                        return method.ShippingFee;

                    case ShippingMethodProvider.ByGeoZoneAndOrderTotal:
                        tableRate = ShippingTableRate.GetOneMaxValue(method.ShippingMethodId, orderSubTotal, geoZoneGuids, storeId);
                        if (tableRate != null)
                            return tableRate.ShippingFee;

                        return method.ShippingFee;

                    case ShippingMethodProvider.ByGeoZoneAndWeight: // Addional value
                        tableRate = ShippingTableRate.GetOneMaxValue(method.ShippingMethodId, orderWeight, geoZoneGuids, storeId);
                        if (tableRate != null)
                        {
                            if (method.FreeShippingOverXEnabled && orderSubTotal >= tableRate.FreeShippingOverXValue
                                && tableRate.FreeShippingOverXValue > 0)
                                return decimal.Zero;

                            if (tableRate.AdditionalFee >= 0 && tableRate.AdditionalValue > 0)
                            {
                                decimal fromValue = tableRate.FromValue;
                                decimal shippingFee = tableRate.ShippingFee;
                                while (orderWeight > fromValue)
                                {
                                    fromValue += tableRate.AdditionalValue;
                                    shippingFee += tableRate.AdditionalFee;
                                }
                                return shippingFee;
                            }
                            return tableRate.ShippingFee;
                        }

                        return method.ShippingFee;

                    case ShippingMethodProvider.VittelPost:
                        if (method.FreeShippingOverXEnabled && orderSubTotal >= method.FreeShippingOverXValue && method.FreeShippingOverXValue > 0)
                            return decimal.Zero;
                        if (BillingDistrictGuid == null || BillingDistrictGuid == Guid.Empty)
                            return decimal.Zero;
                        string error = string.Empty;
                        return ViettelPostHelper.CalculateServiceFee(orderWeight,
                            BillingDistrictGuid.Value, orderSubTotal,
                            shippingOption, productTotalQty, store, ref error, ref expectedTime, orderLength, orderWidth, orderHeight);
                }
            }

            return decimal.Zero;
        }

        #endregion Shipping

        public static XmlDocument BuildPurchaseHistoryXml(int siteId, Guid userGuid, int pageNumber, int pageSize)
        {
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<OrderList></OrderList>");
            var root = doc.DocumentElement;

            var startDate = (DateTime?)null;
            //var lstOrderItems = OrderItem.GetPageBySearch(siteId, -1, -1, -1, -1, startDate, null, null, null, userGuid, null, pageNumber, pageSize);
            //var lstOrderItems = Order.GetPage(siteId, -1, -1, -1, -1, startDate, null, null, null, userGuid, null, pageNumber, pageSize);
            var lstOrderItems = Order.GetPage(siteId, -1, -1, -1, -1, null, null, null, null, userGuid, -1, string.Empty, pageNumber, pageSize);
            var timeOffset = SiteUtils.GetUserTimeOffset();
            var timeZone = SiteUtils.GetUserTimeZone();

            if (lstOrderItems.Count > 0)
            {
                foreach (var item in lstOrderItems)
                {
                    XmlElement orderXml = doc.CreateElement("Order");
                    root.AppendChild(orderXml);

                    // Order detail
                    XmlHelper.AddNode(doc, orderXml, "OrderCode", item.OrderCode);
                    XmlHelper.AddNode(doc, orderXml, "OrderDate", ProductHelper.FormatDate(item.CreatedUtc, timeZone, timeOffset, "dd/MM/yyyy"));
                    XmlHelper.AddNode(doc, orderXml, "OrderStatus", ProductHelper.GetOrderStatus(item.OrderStatus));
                    XmlHelper.AddNode(doc, orderXml, "OrderTotal", ProductHelper.FormatPrice(item.OrderTotal, true));

                    var orderItems = OrderItem.GetByOrder(item.OrderId);
                    if (orderItems.Count > 0)
                    {
                        foreach (OrderItem orderItem in orderItems)
                        {
                            Product product = new Product(siteId, orderItem.ProductId);

                            if (product != null)
                            {
                                XmlElement productXml = doc.CreateElement("Product");
                                orderXml.AppendChild(productXml);

                                ProductHelper.BuildProductDataXml(doc, productXml, product, null, null);
                            }
                        }
                    }
                }
            }
            return doc;
        }

        public static string GetPaymentStatus(int paymentStatus)
        {
            if (paymentStatus == (int)OrderPaymentStatus.Successful)
                return ResourceHelper.GetResourceString("ProductResources", "OrderPaymentStatusPaid");

            return ResourceHelper.GetResourceString("ProductResources", "OrderPaymentStatusUnpaid");
        }

        public static void BuildOrderXml(XmlDocument doc, XmlElement orderXml, Order order, TimeZoneInfo timeZone, double timeOffset)
        {
            try
            {
                XmlHelper.AddNode(doc, orderXml, "OrderId", order.OrderId.ToString());
                XmlHelper.AddNode(doc, orderXml, "OrderCode", order.OrderCode);
                XmlHelper.AddNode(doc, orderXml, "OrderDate", ProductHelper.FormatDate(order.CreatedUtc, timeZone, timeOffset, "dd/MM/yyyy"));
                XmlHelper.AddNode(doc, orderXml, "OrderStatus", ProductHelper.GetOrderStatus(order.OrderStatus));
                XmlHelper.AddNode(doc, orderXml, "OrderStatusValue", order.OrderStatus.ToString());
                XmlHelper.AddNode(doc, orderXml, "CreatedDate", ProductHelper.FormatDate(order.CreatedUtc, timeZone, timeOffset, "dd/MM/yyyy HH:mm:ss"));

                XmlHelper.AddNode(doc, orderXml, "OrderTotal", ProductHelper.FormatPrice(order.OrderTotal, true));
                XmlHelper.AddNode(doc, orderXml, "OrderTotalFormatWithoutCurrency", order.OrderTotal.ToString("#"));
                XmlHelper.AddNode(doc, orderXml, "OrderSubTotal", ProductHelper.FormatPrice(order.OrderSubtotal, true));
                XmlHelper.AddNode(doc, orderXml, "OrderSubTotalValue", ProductHelper.FormatPrice(order.OrderSubtotal, false));
                XmlHelper.AddNode(doc, orderXml, "RedeemedRewardPoints", order.RedeemedRewardPoints.ToString());
                XmlHelper.AddNode(doc, orderXml, "RedeemedRewardPointsAmount", ProductHelper.FormatPrice(-order.RedeemedRewardPointsAmount, true));
                XmlHelper.AddNode(doc, orderXml, "RedeemedRewardPointsAmountValue", ProductHelper.FormatPrice(order.RedeemedRewardPointsAmount, false));
                XmlHelper.AddNode(doc, orderXml, "OrderCouponAmount", ProductHelper.FormatPrice(-order.OrderCouponAmount, true));
                XmlHelper.AddNode(doc, orderXml, "VoucherAmount", ProductHelper.FormatPrice(-order.VoucherAmount, true));
                XmlHelper.AddNode(doc, orderXml, "OrderDiscount", ProductHelper.FormatPrice(-order.OrderDiscount, true));
                XmlHelper.AddNode(doc, orderXml, "PaymentFee", ProductHelper.FormatPrice(order.OrderTax, true));
                XmlHelper.AddNode(doc, orderXml, "ServiceFee", ProductHelper.FormatPrice(order.OrderServiceFee, true));
                XmlHelper.AddNode(doc, orderXml, "ShippingFee", ProductHelper.FormatPrice(order.OrderShipping, true));
                XmlHelper.AddNode(doc, orderXml, "ShippingFeeFormatWithoutCurrency", order.OrderShipping.ToString("#"));

                XmlHelper.AddNode(doc, orderXml, "VoucherAmountValue", ProductHelper.FormatPrice(order.VoucherAmount, false));
                XmlHelper.AddNode(doc, orderXml, "OrderTotalValue", ProductHelper.FormatPrice(order.OrderTotal, false));
                XmlHelper.AddNode(doc, orderXml, "OrderCouponAmountValue", ProductHelper.FormatPrice(order.OrderCouponAmount, false));
                XmlHelper.AddNode(doc, orderXml, "OrderDiscountValue", ProductHelper.FormatPrice(order.OrderDiscount, false));
                XmlHelper.AddNode(doc, orderXml, "PaymentFeeValue", ProductHelper.FormatPrice(order.OrderTax, false));
                XmlHelper.AddNode(doc, orderXml, "ShippingFeeValue", ProductHelper.FormatPrice(order.OrderShipping, false));

                XmlHelper.AddNode(doc, orderXml, "PaymentStatus", OrderHelper.GetPaymentStatusResources(order.PaymentStatus));
                XmlHelper.AddNode(doc, orderXml, "ShippingStatus", OrderHelper.GetShippingStatusResources(order.ShippingStatus));

                XmlHelper.AddNode(doc, orderXml, "BillingAddress", order.BillingAddress);
                XmlHelper.AddNode(doc, orderXml, "BillingEmail", order.BillingEmail);
                XmlHelper.AddNode(doc, orderXml, "BillingFirstName", order.BillingFirstName);
                XmlHelper.AddNode(doc, orderXml, "BillingPhone", order.BillingPhone);
                XmlHelper.AddNode(doc, orderXml, "BillingMobile", order.BillingMobile);
                XmlHelper.AddNode(doc, orderXml, "BillingFax", order.BillingFax);
                var country = new GeoZone(order.BillingCountryGuid);
                if (country != null)
                    XmlHelper.AddNode(doc, orderXml, "BillingCountry", country.Name);
                var province = new GeoZone(order.BillingProvinceGuid);
                if (province != null)
                    XmlHelper.AddNode(doc, orderXml, "BillingProvince", province.Name);

                var district = new GeoZone(order.BillingDistrictGuid);
                if (district != null)
                    XmlHelper.AddNode(doc, orderXml, "BillingDistrict", district.Name);

                //Shipping
                XmlHelper.AddNode(doc, orderXml, "ShippingAddress", order.ShippingAddress);
                XmlHelper.AddNode(doc, orderXml, "ShippingEmail", order.ShippingEmail);
                XmlHelper.AddNode(doc, orderXml, "ShippingFirstName", order.ShippingFirstName);
                XmlHelper.AddNode(doc, orderXml, "ShippingPhone", order.ShippingPhone);
                XmlHelper.AddNode(doc, orderXml, "ShippingMobile", order.ShippingMobile);
                var shippingCountry = new GeoZone(order.ShippingCountryGuid);
                if (shippingCountry != null)
                    XmlHelper.AddNode(doc, orderXml, "ShippingCountry", shippingCountry.Name);
                var shippingProvince = new GeoZone(order.ShippingProvinceGuid);
                if (shippingProvince != null)
                    XmlHelper.AddNode(doc, orderXml, "ShippingProvince", shippingProvince.Name);

                var shippingDistrict = new GeoZone(order.ShippingDistrictGuid);
                if (shippingDistrict != null)
                    XmlHelper.AddNode(doc, orderXml, "ShippingDistrict", shippingDistrict.Name);

                var payment = new PaymentMethod(order.PaymentMethod);
                if (payment != null)
                    XmlHelper.AddNode(doc, orderXml, "PaymentMethod", payment.Name);

                var shipping = new ShippingMethod(order.ShippingMethod);
                if (shipping != null)
                    XmlHelper.AddNode(doc, orderXml, "ShippingMethod", shipping.Name);

                XmlHelper.AddNode(doc, orderXml, "InvoiceCompanyAddress", order.InvoiceCompanyAddress);
                XmlHelper.AddNode(doc, orderXml, "InvoiceCompanyName", order.InvoiceCompanyName);
                XmlHelper.AddNode(doc, orderXml, "InvoiceCompanyTaxCode", order.InvoiceCompanyTaxCode);

                var lstOrderItems = OrderItem.GetByOrder(order.OrderId);
                var lstProducts = Product.GetByOrder(order.SiteId, order.OrderId);
                var total = decimal.Zero;
                List<ProductProperty> productProperties = new List<ProductProperty>();
                List<CustomField> customFields = new List<CustomField>();
                if (ProductConfiguration.EnableShoppingCartAttributes)
                {
                    var lstProductIds = lstOrderItems.Select(x => x.ProductId).Distinct().ToList();
                    customFields = CustomField.GetActiveForCart(order.SiteId, Product.FeatureGuid);
                    if (customFields.Count > 0 && lstProductIds.Count > 0)
                        productProperties = ProductProperty.GetPropertiesByProducts(lstProductIds);
                }
                foreach (var orderItem in lstOrderItems)
                {
                    var product = ProductHelper.GetProductFromList(lstProducts, orderItem.ProductId);
                    if (product != null)
                    {
                        var orderItemXml = doc.CreateElement("Product");
                        orderXml.AppendChild(orderItemXml);
                        ProductHelper.BuildProductDataXml(doc, orderItemXml, product, null);

                        if (orderItemXml["Price"] == null)
                            XmlHelper.AddNode(doc, orderItemXml, "Price", ProductHelper.FormatPrice(orderItem.Price, true));
                        else
                            orderItemXml["Price"].InnerText = ProductHelper.FormatPrice(orderItem.Price, true);
                        if (orderItemXml["PriceFormatWithoutCurrency"] == null)
                            XmlHelper.AddNode(doc, orderItemXml, "PriceFormatWithoutCurrency", orderItem.Price.ToString("#"));
                        else
                            orderItemXml["PriceFormatWithoutCurrency"].InnerText = orderItem.Price.ToString("#");
                        XmlHelper.AddNode(doc, orderItemXml, "Quantity", orderItem.Quantity.ToString());
                        XmlHelper.AddNode(doc, orderItemXml, "Discount", ProductHelper.FormatPrice(orderItem.DiscountAmount, true));

                        var itemTotal = orderItem.Price * orderItem.Quantity - orderItem.DiscountAmount;
                        total += itemTotal;

                        XmlHelper.AddNode(doc, orderItemXml, "ItemTotal", ProductHelper.FormatPrice(itemTotal, true));

                        if (ProductConfiguration.EnableShoppingCartAttributes)
                        {
                            foreach (var a in customFields.Where(c => (c.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0))
                            {
                                XmlElement attributeXml = doc.CreateElement("Attributes");
                                orderItemXml.AppendChild(attributeXml);
                                XmlHelper.AddNode(doc, attributeXml, "Title", a.Name);
                                foreach (ProductProperty property in productProperties)
                                {
                                    if (property.ProductId == product.ProductId && property.CustomFieldId == a.CustomFieldId)
                                    {
                                        XmlElement optionXml = doc.CreateElement("Options");
                                        attributeXml.AppendChild(optionXml);

                                        XmlHelper.AddNode(doc, optionXml, "FieldId", a.CustomFieldId.ToString());
                                        XmlHelper.AddNode(doc, optionXml, "OptionId", property.CustomFieldOptionId.ToString());
                                        XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #region Child products

        public static List<Product> GetChildProducts(Product product)
        {
            return Product.GetPageAdv(pageNumber: 1, pageSize: 100, siteId: product.SiteId, publishStatus: 1, parentId: product.ProductId, languageId: WorkingCulture.LanguageId);
        }

        public static string FormatSpecificationsText(string content)
        {
            var lstContents = content.SplitOnCharAndTrim('\n');
            if (lstContents.Count < 2)
                return content;

            var result = new StringBuilder();
            result.Append("<ul class='spec-list'>");
            foreach (var ct in lstContents)
            {
                result.Append("<li>");
                result.Append(ct);
                result.Append("</li>");
            }
            result.Append("</ul>");

            return result.ToString();
        }

        public static bool ShowParent(Product product)
        {
            if (product == null)
                return true;

            var countChild = Product.GetCountAdv(product.SiteId, parentId: product.ProductId);
            if (countChild > 0)
                return false;

            return true;
        }
         
        public static Product GetProductWithMinPrice(List<Product> lstChildProducts)
        {
            if (lstChildProducts.Count == 0) return null;
            var p = lstChildProducts[0];
            var lstNonOutStock = lstChildProducts.Where(t => t.StockQuantity > 0).ToList();
            if (lstNonOutStock != null && lstNonOutStock.Count > 0)
            {
                p = lstNonOutStock[0];
                foreach (var childProduct in lstNonOutStock)
                {
                    if (childProduct.Price < p.Price)
                        p = childProduct;
                }
                return p;
            }
            foreach (var childProduct in lstChildProducts)
            {
                if (childProduct.Price < p.Price)
                    p = childProduct;
            }

            return p;
        }

        #endregion Child products
    }
}
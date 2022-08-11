using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.NewsUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductViewControl : UserControl
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(ProductViewControl));

        private ProductConfiguration config = new ProductConfiguration();
        private SiteUser currentUser = null;
        private string virtualRoot;
        private Product product = null;
        private Module md;

        private int zoneId = -1;
        protected int productId = -1;
        private int languageId = -1;
        private int pageNumber = 1;
        private int totalPages = 1;

        private bool parametersAreInvalid = false;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;

        private bool userCanEditAsDraft = false;

        private string siteRoot = string.Empty;
        private CmsBasePage basePage;
        private bool userCanUpdate = false;

        public Module module
        {
            get { return md; }
            set { md = value; }
        }

        public ProductConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        private Product childProductWithMinPrice = null;
        private List<Product> lstChildProducts = new List<Product>();
        private bool isChildProductPage = false;

        #endregion Properties

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            base.OnInit(e);

            basePage = Page as CmsBasePage;
            siteRoot = basePage.SiteRoot;
        }

        #endregion OnInit

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadParams();

            if (parametersAreInvalid)
            {
                pnlInnerWrap.Visible = false;
                return;
            }

            LoadSettings();

            //SetupRssLink();
            PopulateLabels();

            if (!IsPostBack && productId > 0)
            { // && moduleId > 0)
                PopulateControls();
                basePage.LastPageVisited = Request.RawUrl;

                //ProductHelper.AddProductToRecentlyViewedList(productId);
                if (product.ParentId > 0)
                    ProductHelper.AddProductToRecentlyViewedList(product.ParentId);
                else
                    ProductHelper.AddProductToRecentlyViewedList(product.ProductId);
            }
        }

        protected void PopulateControls()
        {
            if (parametersAreInvalid)
            {
                pnlInnerWrap.Visible = false;
                return;
            }

            if (product.IsDeleted)
            {
                if (WebConfigSettings.Custom404Page.Length > 0)
                    Server.Transfer(WebConfigSettings.Custom404Page);
                else
                    Server.Transfer("~/PageNotFound.aspx");

                return;
            }

            if (product.IsPublished && product.EndDate < DateTime.UtcNow)
            {
                expired.Visible = true;
                //http://support.google.com/webmasters/bin/answer.py?hl=en&answer=40132
                // 410 means the resource is gone but once existed
                // google treats it as more permanent than a 404
                // and it should result in de-indexing the content
                Response.StatusCode = 410;
                Response.StatusDescription = "Content Expired";
                if (
                    !ProductPermission.CanUpdate
                    || !basePage.UserCanAuthorizeZone(product.ZoneId)
                )
                {
                    pnlInnerWrap.Visible = false;
                    return;
                }
            }

            // if not published only the editor can see it
            if ((!product.IsPublished) || (product.StartDate > DateTime.UtcNow))
            {
                bool stopRedirect = false;
                if (
                    (currentUser != null && currentUser.UserGuid == product.UserGuid)
                    || ((ProductPermission.CanViewList || ProductPermission.CanUpdate) && basePage.UserCanAuthorizeZone(product.ZoneId))
                )
                    stopRedirect = true;

                if (!stopRedirect)
                {
                    pnlInnerWrap.Visible = false;
                    WebUtils.SetupRedirect(this, SiteUtils.GetCurrentZoneUrl());
                    return;
                }
            }

            if (
                ConfigHelper.GetBoolProperty("ProductDetail:ForceRedirectToFriendlyUrl", ConfigHelper.GetBoolProperty("AlwaysRedirectToFriendlyUrl", false))
                && !string.IsNullOrEmpty(product.Url)
                )
            {
                var rawUrl = Request.RawUrl.ToLower();
                var currentUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);
                if (WebConfigSettings.UseFullUrlsForWebPage)
                    currentUrl = currentUrl.Replace(WebUtils.GetSiteRoot(), string.Empty);
                if (
                    rawUrl.Contains("/product/productdetail.aspx")
                    || (WebConfigSettings.EnableHierarchicalFriendlyUrls && currentUrl.EndsWith(rawUrl) && currentUrl != rawUrl)
                    || (!WebConfigSettings.EnableHierarchicalFriendlyUrls && rawUrl.EndsWith(currentUrl) && currentUrl != rawUrl)
                    )
                {
                    Response.Status = "301 Moved Permanently";
                    Response.AddHeader("Location", currentUrl);
                }
            }

            if (isChildProductPage && childProductWithMinPrice != null)
                SetupMetaTags(childProductWithMinPrice);
            else
                SetupMetaTags(product);

            var siteUtils = SiteUtils.GetCurrentSiteUser();
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<ProductDetail></ProductDetail>");
            XmlElement root = doc.DocumentElement;
            var affid = WebUtils.ParseInt32FromQueryString("affid", -1);
            XmlHelper.AddNode(doc, root, "AffiliateUserID", affid.ToString());
            XmlHelper.AddNode(doc, root, "ZoneTitle", basePage.CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "Title", product.Title);
            //XmlHelper.AddNode(doc, root, "SubTitle", product.SubTitle);
            //XmlHelper.AddNode(doc, root, "EnableBuyButton", (!product.DisableBuyButton).ToString());
            XmlHelper.AddNode(doc, root, "IsMobileDevice", basePage.IsMobileDevice.ToString());

            XmlHelper.AddNode(doc, root, "ZoneUrl", SiteUtils.GetCurrentZoneUrl());

            var strMessages = ProductConfiguration.ProductDetailPageMessages;
            if (!string.IsNullOrEmpty(strMessages))
            {
                var lstMessages = strMessages.SplitOnCharAndTrim(';');
                foreach (string item in lstMessages)
                    XmlHelper.AddNode(doc, root, item, MessageTemplate.GetMessage(item));
            }

            if (module.ResourceFileDef.Length > 0 && module.ResourceKeyDef.Length > 0)
            {
                List<string> lstResourceKeys = module.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(module.ResourceFileDef, item));
            }

            XmlHelper.AddNode(doc, root, "ShowOption", product.ShowOption.ToString());
            XmlHelper.AddNode(doc, root, "ProductType", product.ProductType.ToString());
            DiscountAppliedToItem discountApplied = null;
            var lstDiscountItemsMayBeApplied = new List<DiscountAppliedToItem>();
            decimal productPrice = 0;//ProductHelper.GetPrice(product, -1, ref discountApplied, ref lstDiscountItemsMayBeApplied);
            var giftDescription = string.Empty;
            var giftDescription2 = string.Empty;

            var pageUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);

            #region Parent or Child Details
            if (childProductWithMinPrice != null)
            {
                XmlHelper.AddNode(doc, root, "ProductId", childProductWithMinPrice.ProductId.ToString());
                XmlHelper.AddNode(doc, root, "ParentProductId", childProductWithMinPrice.ParentId.ToString());
                //XmlHelper.AddNode(doc, root, "Title", childProductWithMinPrice.Title);
                XmlHelper.AddNode(doc, root, "Code", childProductWithMinPrice.Code);
                XmlHelper.AddNode(doc, root, "BarCode", childProductWithMinPrice.BarCode);
                productPrice = ProductHelper.GetPrice(childProductWithMinPrice, -1, ref discountApplied, ref lstDiscountItemsMayBeApplied);
                if (productPrice > 0)
                    XmlHelper.AddNode(doc, root, "Price", ProductHelper.FormatPrice(productPrice, true));
                if (childProductWithMinPrice.OldPrice > 0)
                    XmlHelper.AddNode(doc, root, "OldPrice", ProductHelper.FormatPrice(childProductWithMinPrice.OldPrice, true));
                XmlHelper.AddNode(doc, root, "OutStock", (childProductWithMinPrice.StockQuantity < 1) ? "true" : "false");
                XmlHelper.AddNode(doc, root, "StockQuantity", childProductWithMinPrice.StockQuantity.ToString());
                XmlHelper.AddNode(doc, root, "EditLink", ProductHelper.BuildEditLink(childProductWithMinPrice, basePage, userCanUpdate, currentUser));
                XmlHelper.AddNode(doc, root, "EnableBuyButton", (!childProductWithMinPrice.DisableBuyButton).ToString());
                XmlHelper.AddNode(doc, root, "SubTitle", childProductWithMinPrice.SubTitle);

                pageUrl = ProductHelper.FormatProductUrl(childProductWithMinPrice.Url, childProductWithMinPrice.ProductId, childProductWithMinPrice.ZoneId);
            }
            else
            {
                XmlHelper.AddNode(doc, root, "ProductId", product.ProductId.ToString());
                //XmlHelper.AddNode(doc, root, "Title", product.Title);
                XmlHelper.AddNode(doc, root, "Code", product.Code);
                XmlHelper.AddNode(doc, root, "BarCode", product.BarCode);
                productPrice = ProductHelper.GetPrice(product, -1, ref discountApplied, ref lstDiscountItemsMayBeApplied);
                if (productPrice > 0)
                    XmlHelper.AddNode(doc, root, "Price", ProductHelper.FormatPrice(productPrice, true));
                if (product.OldPrice > 0)
                    XmlHelper.AddNode(doc, root, "OldPrice", ProductHelper.FormatPrice(product.OldPrice, true));
                XmlHelper.AddNode(doc, root, "StockQuantity", product.StockQuantity.ToString());
                XmlHelper.AddNode(doc, root, "OutStock", (product.StockQuantity < 1) ? "true" : "false");
                XmlHelper.AddNode(doc, root, "EditLink", ProductHelper.BuildEditLink(product, basePage, userCanUpdate, currentUser));
                XmlHelper.AddNode(doc, root, "EnableBuyButton", (!product.DisableBuyButton).ToString());
                XmlHelper.AddNode(doc, root, "SubTitle", product.SubTitle);
            }
            if (ConfigHelper.GetBoolProperty("ProductDetail:ShowParentTitle", false))
                root["Title"].InnerText = product.Title;


            //if (oldPrice > 0 && oldPrice != productPrice)
            //{
            //    XmlHelper.AddNode(doc, root, "OldPrice", ProductHelper.FormatPrice(oldPrice, true));

            //    if (productPrice > 0 && oldPrice > productPrice)
            //    {
            //        var discountPercent = PromotionsHelper.CalculatePercentage(oldPrice - productPrice, oldPrice);
            //        XmlHelper.AddNode(doc, root, "DiscountPercentage", discountPercent.ToString());
            //    }
            //}
            #endregion Parent or Child Details

            if (discountApplied != null)
            {
                if (discountApplied.DiscountPercentage > 0)
                {
                    XmlHelper.AddNode(doc, root, "DiscountPercentage", discountApplied.DiscountPercentage.ToString());

                    //log.Info("=====oldprice 2: " + discountApplied.OriginalPrice);
                    if (discountApplied.OriginalPrice > productPrice)
                        XmlHelper.AddNode(doc, root, "OldPrice", ProductHelper.FormatPrice(discountApplied.OriginalPrice, true));
                }

                giftDescription = discountApplied.GiftHtml;

                if (discountApplied.DiscountType == (int)DiscountType.Deal)
                {
                    var remainingQty = (discountApplied.DealQty - discountApplied.SoldQty);
                    if (discountApplied.DealQty > 0 && discountApplied.SoldQty >= 0)
                    {
                        var soldQty = discountApplied.SoldQty;
                        if (discountApplied.SoldQty > discountApplied.DealQty)
                            soldQty = discountApplied.DealQty;
                        var percent = PromotionsHelper.CalculatePercentage((decimal)soldQty, (decimal)discountApplied.DealQty);

                        XmlHelper.AddNode(doc, root, "DealSoldQty", soldQty.ToString());
                        XmlHelper.AddNode(doc, root, "DealSoldPercent", percent.ToString());
                        XmlHelper.AddNode(doc, root, "DealTotalQty", discountApplied.DealQty.ToString());
                        XmlHelper.AddNode(doc, root, "DealRemainingQty", remainingQty.ToString());
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
                        ProductHelper.BuildDealDateXml(doc, root, "DealEnd", date.Value);

                    date = (DateTime?)null;
                    if (discountApplied.FromDate.HasValue)
                        date = discountApplied.FromDate.Value;
                    if (discountApplied.DiscountStartDate.HasValue)
                    {
                        if (date == null || date < discountApplied.DiscountStartDate.Value)
                            date = discountApplied.DiscountStartDate.Value;
                    }
                    if (date != null)
                        ProductHelper.BuildDealDateXml(doc, root, "DealStart", date.Value);
                }

                giftDescription2 = discountApplied.GiftDescription;
            }

            //var comboItem = DiscountAppliedToItem.GetOneComboSale(product.SiteId, product.ProductId);
            //if (comboItem != null)
            //    giftDescription += comboItem.GiftDescription;
            XmlHelper.AddNode(doc, root, "GiftDescription", giftDescription);
            XmlHelper.AddNode(doc, root, "GiftDescription2", giftDescription2);

            if (lstDiscountItemsMayBeApplied.Count > 0)
            {
                foreach (var dia in lstDiscountItemsMayBeApplied)
                {
                    var promotionXml = doc.CreateElement("Promotion");
                    root.AppendChild(promotionXml);

                    XmlHelper.AddNode(doc, promotionXml, "Option", dia.DiscountOptions.ToString());
                    if (dia.DiscountPrice > 0)
                        XmlHelper.AddNode(doc, promotionXml, "DiscountPrice", ProductHelper.FormatPrice(dia.DiscountPrice, true));
                }
            }

            XmlHelper.AddNode(doc, root, "CreatedDate", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset,
                              ProductResources.ProductDateFormat));
            XmlHelper.AddNode(doc, root, "CreatedTime", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset,
                              ProductResources.ProductTimeFormat));
            XmlHelper.AddNode(doc, root, "CreatedDD", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "dd"));
            XmlHelper.AddNode(doc, root, "CreatedYY", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "yy"));
            XmlHelper.AddNode(doc, root, "CreatedYYYY", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "yyyy"));
            XmlHelper.AddNode(doc, root, "CreatedMM", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "MM"));
            if (WorkingCulture.DefaultName.ToLower() == "vi-vn")
            {
                string monthVI = "Tháng " + ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "MM");
                XmlHelper.AddNode(doc, root, "CreatedMMM", monthVI);
                XmlHelper.AddNode(doc, root, "CreatedMMMM", monthVI);
            }
            else
            {
                XmlHelper.AddNode(doc, root, "CreatedMMM", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "MMM"));
                XmlHelper.AddNode(doc, root, "CreatedMMMM", ProductHelper.FormatDate(product.StartDate, timeZone, timeOffset, "MMMM"));
            }

            XmlHelper.AddNode(doc, root, "Code", product.Code);
            XmlHelper.AddNode(doc, root, "BriefContent", product.BriefContent);
            XmlHelper.AddNode(doc, root, "FullContent", product.FullContent);
            XmlHelper.AddNode(doc, root, "ViewCount", product.ViewCount.ToString());

            var wishItemGuid = WishlistCacheHelper.GetWishListItemGuid(product.SiteId, product.ProductId);
            bool isWishListed = wishItemGuid != Guid.Empty;
            XmlHelper.AddNode(doc, root, "AddedWishList", isWishListed.ToString());
            if (isWishListed)
                XmlHelper.AddNode(doc, root, "WishItemGuid", wishItemGuid.ToString());
            Manufacturer manufacturer = null;
            if (product.ManufacturerId > 0)
            {
                manufacturer = ManufacturerCacheHelper.GetById(product.ManufacturerId);
                if (manufacturer != null && manufacturer.ManufacturerId > 0)
                {
                    XmlHelper.AddNode(doc, root, "ManufacturerTitle", manufacturer.Name);
                    XmlHelper.AddNode(doc, root, "ManufacturerDescription", manufacturer.Description);
                    XmlHelper.AddNode(doc, root, "ManufacturerUrl", ManufacturerHelper.FormatManufacturerUrl(manufacturer.Url, manufacturer.ManufacturerId));
                    XmlHelper.AddNode(doc, root, "ManufacturerImageUrl", manufacturer.PrimaryImage);
                    XmlHelper.AddNode(doc, root, "ManufacturerThumnailUrl", manufacturer.SecondImage);
                }
            }

            if (
                displaySettings.ShowNextPreviousLink
                && !config.LoadFirstProduct
            )
                BuildNextPreviousXml(doc, root);

            pageUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);

            XmlHelper.AddNode(doc, root, "FullUrl", pageUrl);

            XmlHelper.AddNode(doc, root, "RatingVotes", product.RatingVotes.ToString());
            if (product.RatingVotes > 0)
            {
                var ratingValue = Convert.ToDouble(product.RatingSum / product.RatingVotes);
                XmlHelper.AddNode(doc, root, "RatingValue", Math.Round(ratingValue, 1).ToString());
                XmlHelper.AddNode(doc, root, "RatingPercent", Math.Round(ratingValue * 20, 0).ToString());
            }
            else
            {
                XmlHelper.AddNode(doc, root, "RatingValue", "0");
                XmlHelper.AddNode(doc, root, "RatingPercent", "0");
            }
            var wishItem = CartHelper.ExistProductInWishlist(product);
            if (wishItem != null)
            {
                XmlHelper.AddNode(doc, root, "IsWished", "True");
                XmlHelper.AddNode(doc, root, "WishItemGuid", wishItem.Guid.ToString());
            }

            BuildProductRelatedXml(doc, root, languageId);

            if (ProductConfiguration.EnableComparing)
                XmlHelper.AddNode(doc, root, "CompareListUrl", siteRoot + "/product/compare.aspx");

            if (displaySettings.ShowAttribute)
                BuildProductAttributesXml(doc, root, languageId);

            BuildProductPropertiesXml(doc, root, languageId);

            BuildProductMediaXml(doc, root, out List<ContentMedia> medias);

            BuildProductOtherXml(doc, root, zoneId, pageNumber, config.OtherProductsPerPage, out totalPages);

            BuildProductViewedXml(doc, root);

            BuildProductStockInventory(doc, root);

            if (displaySettings.ShowRelatedNews)
            {
                var siteSetting = CacheHelper.GetCurrentSiteSettings();
                BuildNewsRelatedXml(doc, root, languageId, siteSetting.SiteGuid);
            }

            if (ProductConfiguration.EnableProductJsonSchema)
            {
                var lstComments = ProductComment.GetPage(product.SiteId, product.ProductId,
                    (int)ProductCommentType.Rating, 1, -1, -1, null, null, null, 0, 1, 214849);

                BuildProductJsonSchema(manufacturer, medias, lstComments);
            }


            if (config.LoadFirstProduct)
                pageUrl = SiteUtils.GetCurrentZoneUrl();

            if (config.HidePaginationOnDetailPage)
                divPager.Visible = false;
            else
            {
                if (pageUrl.Contains("?"))
                    pageUrl += "&amp;pagenumber={0}";
                else
                    pageUrl += "?pagenumber={0}";

                pgr.PageURLFormat = pageUrl;
                pgr.ShowFirstLast = true;
                pgr.PageSize = config.OtherProductsPerPage;
                pgr.PageCount = totalPages;
                pgr.CurrentIndex = pageNumber;
                divPager.Visible = (totalPages > 1);
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", config.XsltFileNameDetailPage), doc);

            if (Page.Header == null) return;

            string canonicalUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);
            if (SiteUtils.IsSecureRequest() && (!basePage.CurrentPage.RequireSsl) && (!basePage.SiteInfo.UseSslOnAllPages))
            {
                if (WebConfigSettings.ForceHttpForCanonicalUrlsThatDontRequireSsl)
                    canonicalUrl = canonicalUrl.Replace("https:", "http:");
            }

            //LoadWorkflow();
            if (ConfigHelper.GetBoolProperty("ProductDetail:EnableCanonical", true))
            {
                Literal link = new Literal
                {
                    ID = "producturl",
                    Text = "<link rel='canonical' href='" + canonicalUrl + "' />"
                };
                Page.Header.Controls.Add(link);
            }

            Product.IncrementViewCount(product.ProductId);
        }

        private void SetupMetaTags(Product p)
        {
            var strLanguageId = string.Empty;
            if (languageId > 0)
                strLanguageId = languageId.ToString();
            var metaTitle = basePage.SiteInfo.GetExpandoProperty("SEO.Product.MetaTitle" + strLanguageId);
            var metaDescription = basePage.SiteInfo.GetExpandoProperty("SEO.Product.MetaDescription" + strLanguageId);
            var additionalMetaTags = basePage.SiteInfo.GetExpandoProperty("SEO.Product.AdditionalMetaTags" + strLanguageId);

            var title = p.Title;
            if (p.MetaTitle.Length > 0)
            {
                basePage.Title = p.MetaTitle;
                title = p.MetaTitle;
            }
            else
            {
                if (!string.IsNullOrEmpty(metaTitle))
                {
                    basePage.Title = metaTitle.Replace("{Title}", product.Title)
                                              .Replace("{SubTitle}", product.SubTitle)
                                              .Replace("{Code}", product.Code)
                                                ;
                    title = basePage.Title;
                }
                else
                    basePage.Title = SiteUtils.FormatPageTitle(basePage.SiteInfo, p.Title);
            }

            if (p.MetaKeywords.Length > 0)
                basePage.MetaKeywordCsv = p.MetaKeywords;

            if (p.MetaDescription.Length > 0)
                basePage.MetaDescription = p.MetaDescription;
            else if (!string.IsNullOrEmpty(metaDescription))
            {
                basePage.MetaDescription = metaDescription.Replace("{Title}", product.Title)
                                                      .Replace("{SubTitle}", product.SubTitle)
                                                      .Replace("{Code}", product.Code);
                if (metaDescription.Contains("{BriefContent}"))
                    basePage.MetaDescription = basePage.MetaDescription
                                                      .Replace("{BriefContent}", UIHelper.CreateExcerpt(product.BriefContent, 156))
                                                        ;
                if (metaDescription.Contains("{FullContent}"))
                    basePage.MetaDescription = basePage.MetaDescription
                                                      .Replace("{FullContent}", UIHelper.CreateExcerpt(product.FullContent, 156))
                                                        ;
            }
            else if (p.BriefContent.Length > 0)
                basePage.MetaDescription = UIHelper.CreateExcerpt(p.BriefContent, 156);
            else if (p.FullContent.Length > 0)
                basePage.MetaDescription = UIHelper.CreateExcerpt(p.FullContent, 156);

            if (title.Length > 0)
                basePage.AdditionalMetaMarkup += "<meta property=\"og:title\" content=\"" + title + "\" />";
            if (basePage.MetaDescription.Length > 0)
                basePage.AdditionalMetaMarkup += "<meta property=\"og:description\" content=\"" + basePage.MetaDescription + "\" />";

            basePage.AdditionalMetaMarkup += "<meta property=\"og:url\" content=\"" + ProductHelper.FormatProductUrl(p.Url,
                                             p.ProductId, p.ZoneId) + "\" />";
            basePage.AdditionalMetaMarkup += "<meta property=\"og:site_name\" content=\"" + basePage.SiteInfo.SiteName + "\" />";
            basePage.AdditionalMetaMarkup += "<meta property=\"og:type\" content=\"product\" />";

            if (title.Length > 0)
                basePage.AdditionalMetaMarkup += "<meta itemprop=\"name\" content=\"" + title + "\" />";
            if (basePage.MetaDescription.Length > 0)
                basePage.AdditionalMetaMarkup += "<meta itemprop=\"description\" content=\"" + basePage.MetaDescription + "\" />";

            if (!string.IsNullOrEmpty(additionalMetaTags))
            {
                additionalMetaTags = additionalMetaTags.Replace("{Id}", product.ProductId.ToString())
                                                      .Replace("{Title}", product.Title)
                                                      .Replace("{SubTitle}", product.SubTitle)
                                                      .Replace("{Code}", product.Code);
                basePage.AdditionalMetaMarkup += additionalMetaTags;
            }
            else
            {
                var currentZone = CacheHelper.GetCurrentZone();
                if (basePage.SiteInfo.MetaAdditional.Length > 0)
                    basePage.AdditionalMetaMarkup += basePage.SiteInfo.MetaAdditional;
                if (currentZone.AdditionalMetaTags.Length > 0)
                    basePage.AdditionalMetaMarkup += currentZone.AdditionalMetaTags;
                if (p.AdditionalMetaTags.Length > 0)
                    basePage.AdditionalMetaMarkup += p.AdditionalMetaTags;
            }
        }

        #region GenXML
        public void BuildNewsRelatedXml(XmlDocument doc, XmlElement root, int languageId, Guid siteGuid, string elementName = "NewsRelated")
        {
            var lstNewss = NewsRelated.GetNewsRelated(basePage.SiteId, siteGuid, product.ProductGuid, true, false, languageId);
            foreach (News news in lstNewss)
            {
                XmlElement productXml = doc.CreateElement(elementName);
                root.AppendChild(productXml);

                NewsHelper.BuildNewsDataXml(doc, productXml, news, timeZone, timeOffset,
                                                  NewsHelper.BuildEditLink(news, basePage, userCanUpdate, currentUser));
                List<ContentMedia> listMedia = ContentMedia.GetByContentDesc(news.NewsGuid);

                string imageFolderPath = NewsHelper.MediaFolderPath(basePage.SiteId, news.NewsID);
                string thumbnailImageFolderPath = imageFolderPath + "thumbs/";
                foreach (ContentMedia media in listMedia)
                    BuildNewsImagesXml(doc, productXml, media, imageFolderPath, thumbnailImageFolderPath);
            }
        }

        public void BuildNewsImagesXml(
         XmlDocument doc,
         XmlElement root,
         ContentMedia media,
         string imageFolderPath,
         string thumbnailImageFolderPath)
        {
            XmlElement element = doc.CreateElement("NewsImages");
            root.AppendChild(element);

            XmlHelper.AddNode(doc, element, "Title", media.Title);
            XmlHelper.AddNode(doc, element, "DisplayOrder", media.DisplayOrder.ToString());
            XmlHelper.AddNode(doc, element, "ImageUrl", Page.ResolveUrl(imageFolderPath + media.MediaFile));
            XmlHelper.AddNode(doc, element, "ThumbnailUrl", Page.ResolveUrl(thumbnailImageFolderPath + media.ThumbnailFile));
        }

        private void BuildProductStockInventory(XmlDocument doc, XmlElement root)
        {
            var items = StockInventoryModel.GetByProductId(product.ProductId);
            foreach (var item in items)
            {
                //if (item.Quantity <= 0) continue; 
                var inventoryXml = doc.CreateElement("Inventory");
                root.AppendChild(inventoryXml);
                XmlHelper.AddNode(doc, inventoryXml, "InventoryID", item.InventoryID.ToString());
                XmlHelper.AddNode(doc, inventoryXml, "ProductID", item.ProductID.ToString());
                XmlHelper.AddNode(doc, inventoryXml, "StoreID", item.StoreID.ToString());
                XmlHelper.AddNode(doc, inventoryXml, "StoreCode", item.StoreCode);
                XmlHelper.AddNode(doc, inventoryXml, "StoreName", item.StoreName);
                XmlHelper.AddNode(doc, inventoryXml, "StoreAddress", item.StoreAddress);
                XmlHelper.AddNode(doc, inventoryXml, "StoreDescription", item.StoreDescription);
                XmlHelper.AddNode(doc, inventoryXml, "StoreMap", item.StoreMap);
                XmlHelper.AddNode(doc, inventoryXml, "Price", ProductHelper.FormatPrice(item.Price, true));
                XmlHelper.AddNode(doc, inventoryXml, "Quantity", item.Quantity.ToString());
            }
        }

        private void BuildProductViewedXml(XmlDocument doc, XmlElement root)
        {
            var lstProducts = ProductHelper.GetRecentlyViewedProducts(config.OtherProductsPerPage);
            var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(product.SiteId, -1, lstProducts);
            foreach (var item in lstProducts)
            {
                if (item.ProductId == product.ProductId) continue;

                var productXml = doc.CreateElement("ProductViewed");
                root.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, item, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(item, basePage, userCanUpdate, currentUser));
            }
        }

        private void BuildNextPreviousXml(
            XmlDocument doc,
            XmlElement root)
        {
            product.LoadNextPrevious(languageId);

            if ((product.PreviousProductId > -1) || (product.PreviousProductUrl.Length > 0))
            {
                XmlHelper.AddNode(doc, root, "PreviousLink", "<a href='" + ProductHelper.FormatProductUrl(product.PreviousProductUrl,
                                  product.PreviousProductId, product.PreviousZoneId) + "' title='" + product.PreviousProductTitle + "'>" +
                                  ProductResources.ProductPreviousLink + "</a>");
                XmlHelper.AddNode(doc, root, "PreviousUrl", ProductHelper.FormatProductUrl(product.PreviousProductUrl,
                                  product.PreviousProductId, product.PreviousZoneId));
                XmlHelper.AddNode(doc, root, "IsFirstProduct", product.IsFirstProduct.ToString().ToLower());
                XmlHelper.AddNode(doc, root, "IsLastProduct", product.IsLastProduct.ToString().ToLower());
            }
            if ((product.NextProductId > -1) || (product.NextProductUrl.Length > 0))
            {
                XmlHelper.AddNode(doc, root, "NextLink", "<a href='" + ProductHelper.FormatProductUrl(product.NextProductUrl,
                                  product.NextProductId, product.NextZoneId) + "' title='" + product.NextProductTitle + "'>" + ProductResources.ProductNextLink
                                  + "</a>");
                XmlHelper.AddNode(doc, root, "NextUrl", ProductHelper.FormatProductUrl(product.NextProductUrl, product.NextProductId,
                                  product.NextZoneId));
                XmlHelper.AddNode(doc, root, "IsFirstProduct", product.IsFirstProduct.ToString().ToLower());
                XmlHelper.AddNode(doc, root, "IsLastProduct", product.IsLastProduct.ToString().ToLower());
            }
        }

        private void BuildProductMediaXml(
            XmlDocument doc,
            XmlElement root, out List<ContentMedia> medias)
        {
            var imageFolderPath = ProductHelper.MediaFolderPath(basePage.SiteId, product.ProductId);

            var siteRoot = WebUtils.GetSiteRoot();

            var youtubeVideoRegex = new Regex("youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            var listMedia = new List<ContentMedia>();
            if (childProductWithMinPrice != null)
            {
                imageFolderPath = ProductHelper.MediaFolderPath(basePage.SiteId, childProductWithMinPrice.ProductId);
                listMedia = ContentMedia.GetByContentDesc(childProductWithMinPrice.ProductGuid);
            }
            else
            {
                imageFolderPath = ProductHelper.MediaFolderPath(basePage.SiteId, product.ProductId);
                listMedia = ContentMedia.GetByContentDesc(product.ProductGuid);
            }
            var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
            medias = listMedia;
            var mediaTypes = new List<int>();
            var listOptions = new List<CustomFieldOption>();
            foreach (ContentMedia media in listMedia)
            {
                if (media.MediaType > 0 && !mediaTypes.Contains(media.MediaType))
                    mediaTypes.Add(media.MediaType);
            }
            if (mediaTypes.Count > 0)
                listOptions = CustomFieldOption.GetByOptionIds(product.SiteId, string.Join(";", mediaTypes.ToArray()));

            if (listOptions.Count > 0)
            {
                foreach (CustomFieldOption option in listOptions)
                {
                    XmlElement element = doc.CreateElement("ProductColors");
                    root.AppendChild(element);
                    XmlHelper.AddNode(doc, element, "Title", option.Name);
                    XmlHelper.AddNode(doc, element, "Color", option.OptionColor);
                    XmlHelper.AddNode(doc, element, "ColorId", option.CustomFieldOptionId.ToString());

                    foreach (ContentMedia media in listMedia)
                    {
                        if (
                            (option.CustomFieldOptionId == media.MediaType)
                            && (media.LanguageId == -1 || media.LanguageId == languageId)
                            && (media.MediaType != (int)ProductMediaType.Video)
                        )
                            BuildProductImagesXml(doc, element, media, imageFolderPath, thumbnailImageFolderPath);
                    }
                }
            }

            foreach (ContentMedia media in listMedia)
            {
                if (media.LanguageId == -1 || media.LanguageId == languageId)
                {
                    if (media.MediaType != (int)ProductMediaType.Video)
                    {
                        BuildProductImagesXml(doc, root, media, imageFolderPath, thumbnailImageFolderPath);

                        string relativePath = siteRoot + ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile);
                        basePage.AdditionalMetaMarkup += "<meta property=\"og:image\" content=\"" + relativePath + "\" />";
                        basePage.AdditionalMetaMarkup += "<meta itemprop=\"image\" content=\"" + relativePath + "\" />";
                    }
                    else
                    {
                        XmlElement element = doc.CreateElement("ProductVideos");
                        root.AppendChild(element);

                        XmlHelper.AddNode(doc, element, "Title", media.Title);
                        XmlHelper.AddNode(doc, element, "DisplayOrder", media.DisplayOrder.ToString());
                        XmlHelper.AddNode(doc, element, "Type", media.MediaType.ToString());
                        XmlHelper.AddNode(doc, element, "VideoUrl", ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile));

                        string thumbnailPath = ProductHelper.GetMediaFilePath(thumbnailImageFolderPath, media.ThumbnailFile);
                        if (media.ThumbnailFile.Length == 0 && media.MediaFile.ContainsCaseInsensitive("youtu"))
                        {
                            Match youtubeMatch = youtubeVideoRegex.Match(media.MediaFile);
                            string videoId = string.Empty;
                            if (youtubeMatch.Success)
                                videoId = youtubeMatch.Groups[1].Value;

                            thumbnailPath = "http://img.youtube.com/vi/" + videoId + "/0.jpg";
                        }

                        XmlHelper.AddNode(doc, element, "ThumbnailUrl", thumbnailPath);
                    }

                    if (displaySettings.ShowVideo)
                    {
                        XmlElement element = doc.CreateElement("ProductMedia");
                        root.AppendChild(element);

                        XmlHelper.AddNode(doc, element, "Title", media.Title);
                        XmlHelper.AddNode(doc, element, "DisplayOrder", media.DisplayOrder.ToString());
                        XmlHelper.AddNode(doc, element, "Type", media.MediaType.ToString());
                        XmlHelper.AddNode(doc, element, "MediaUrl", ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile));
                        XmlHelper.AddNode(doc, element, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath,
                                          media.ThumbnailFile));
                    }
                }
            }
        }

        private void BuildProductImagesXml(
            XmlDocument doc,
            XmlElement root,
            ContentMedia media,
            string imageFolderPath,
            string thumbnailImageFolderPath)
        {
            XmlElement element = doc.CreateElement("ProductImages");
            root.AppendChild(element);

            XmlHelper.AddNode(doc, element, "Title", media.Title);
            XmlHelper.AddNode(doc, element, "Type", media.MediaType.ToString());
            XmlHelper.AddNode(doc, element, "ImageUrl", ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile));
            XmlHelper.AddNode(doc, element, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath,
                              media.ThumbnailFile));
        }

        private void BuildProductOtherXml(
            XmlDocument doc,
            XmlElement root,
            int zoneId,
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            XmlHelper.AddNode(doc, root, "ProductOtherText", ProductResources.OtherProductLabel);

            int siteId = CacheHelper.GetCurrentSiteSettings().SiteId;

            List<Product> lstProducts = new List<Product>();
            if (pageSize < 0)
            {
                pageSize = -pageSize;
                totalPages = 1;
                int totalRows = Product.GetCountAdv(siteId: basePage.SiteId, zoneId: zoneId, publishStatus: 1,
                    languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone); //CacheHelper

                if (pageSize > 0) totalPages = totalRows / pageSize;

                if (totalRows <= pageSize)
                    totalPages = 1;
                else if (pageSize > 0)
                {
                    Math.DivRem(totalRows, pageSize, out int remainder);
                    if (remainder > 0)
                        totalPages += 1;
                }

                lstProducts = Product.GetPageAdv(siteId: siteId, zoneId: zoneId, publishStatus: 1, languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone, pageNumber: pageNumber, pageSize: pageSize); //CacheHelper
            }
            else
            {
                totalPages = 1;
                lstProducts = Product.GetTopAdv(zoneId: zoneId, publishStatus: 1,
                    propertyCondition: "p.ProductID <>" + product.ProductId.ToString(), languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone, top: pageSize); //CacheHelper
            }

            var lstDiscountItems = DiscountAppliedToItem.GetActive(siteId, -10, lstProducts);
            foreach (Product productOther in lstProducts)
            {
                XmlElement productXml = doc.CreateElement("ProductOther");
                root.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, productOther, lstDiscountItems, timeZone, timeOffset,
                                                  ProductHelper.BuildEditLink(productOther, basePage, userCanUpdate, currentUser));

                if (productOther.ProductId == productId)
                    XmlHelper.AddNode(doc, productXml, "IsActive", "true");
                else
                    XmlHelper.AddNode(doc, productXml, "IsActive", "false");
            }

            if (pageNumber < totalPages)
            {
                string pageUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);

                if (config.LoadFirstProduct)
                    pageUrl = SiteUtils.GetCurrentZoneUrl();

                if (pageUrl.Contains("?"))
                    pageUrl += "&pagenumber=" + (pageNumber + 1).ToString();
                else
                    pageUrl += "?pagenumber=" + (pageNumber + 1).ToString();

                XmlHelper.AddNode(doc, root, "NextPageUrl", pageUrl);
            }
        }

        private void BuildProductRelatedXml(XmlDocument doc, XmlElement root, int languageId)
        {
            var lstProducts = Product.GetRelatedProducts(basePage.SiteId,
                CacheHelper.GetCurrentSiteSettings().SiteGuid,
                product.ProductGuid, true, false, languageId);
            var lstDiscountItems = DiscountAppliedToItem.GetActive(basePage.SiteId, -10, lstProducts);
            foreach (Product productRelated in lstProducts)
            {
                XmlElement productXml = doc.CreateElement("ProductRelated");
                root.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, productRelated, lstDiscountItems, timeZone, timeOffset,
                                                  ProductHelper.BuildEditLink(productRelated, basePage, userCanUpdate, currentUser));
            }
        }

        private void BuildProductPropertiesXml(
            XmlDocument doc,
            XmlElement root,
            int languageId)
        {
            BuildProductGroupSpecsXml(doc, root, languageId, product, childProductWithMinPrice);

            //Build cart attributes
            var lstChildProductIds = lstChildProducts.Select(s => s.ProductId).Distinct().ToList();
            if (lstChildProductIds.Count > 0)
            {
                //var productProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);
                var productProperties = ProductProperty.GetPropertiesByProducts(lstChildProductIds, languageId);
                if (productProperties.Count > 0)
                {
                    var customFields = new List<CustomField>();
                    var customFieldIds = new List<int>();

                    foreach (var property in productProperties)
                    {
                        if (!customFieldIds.Contains(property.CustomFieldId))
                            customFieldIds.Add(property.CustomFieldId);
                    }

                    var tmp = CustomField.GetActiveByFields(basePage.SiteId, Product.FeatureGuid, customFieldIds, languageId);
                    //customFields = CustomField.GetByOption(tmp, CustomFieldOptions.ShowInProductDetailsPage);
                    customFields = CustomField.GetByOption(tmp, CustomFieldOptions.EnableShoppingCart);
                    CartHelper.BuildProductAttributes(doc, root, lstChildProducts, customFields, productProperties, product, childProductWithMinPrice != null ? childProductWithMinPrice.ProductId : -1);
                    var tmpC = customFields.Select(c => c.CustomFieldId);
                    customFields = CustomField.GetByOption(tmp, CustomFieldOptions.ShowInProductDetailsPage).Where(c => !tmpC.Contains(c.CustomFieldId)).ToList();

                    foreach (CustomField field in customFields)
                    {
                        XmlElement groupXml = doc.CreateElement("ProductProperties");
                        root.AppendChild(groupXml);

                        XmlHelper.AddNode(doc, groupXml, "FieldId", field.CustomFieldId.ToString());
                        XmlHelper.AddNode(doc, groupXml, "FieldType", field.FieldType.ToString());
                        XmlHelper.AddNode(doc, groupXml, "DataType", field.DataType.ToString());
                        XmlHelper.AddNode(doc, groupXml, "FilterType", field.FilterType.ToString());
                        XmlHelper.AddNode(doc, groupXml, "Title", field.Name);
                        XmlHelper.AddNode(doc, groupXml, "DisplayName", field.DisplayName);
                        XmlHelper.AddNode(doc, groupXml, "EnableShoppingCart", "False");

                        foreach (ProductProperty property in productProperties)
                        {
                            if (property.ProductId == product.ProductId && property.CustomFieldId == field.CustomFieldId)
                            {
                                XmlElement optionXml = doc.CreateElement("Options");
                                groupXml.AppendChild(optionXml);

                                XmlHelper.AddNode(doc, optionXml, "FieldId", field.CustomFieldId.ToString());
                                XmlHelper.AddNode(doc, optionXml, "OptionId", property.CustomFieldOptionId.ToString());

                                if (property.CustomFieldOptionId > 0)
                                {
                                    XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);
                                    XmlHelper.AddNode(doc, optionXml, "Color", property.OptionColor); //TODO: not yet implemented
                                    XmlHelper.AddNode(doc, optionXml, "Price", ProductHelper.FormatPrice(product.Price + property.OverriddenPrice, true));
                                    XmlHelper.AddNode(doc, optionXml, "PriceAdjustment", ProductHelper.FormatPrice(property.OverriddenPrice, true));
                                }
                                else
                                    XmlHelper.AddNode(doc, optionXml, "Title", property.CustomValue);

                                string pageUrl = SiteUtils.GetCurrentZoneUrl();
                                XmlHelper.AddNode(doc, optionXml, "Url", ProductHelper.GetQueryStringFilter(pageUrl, field.FilterType, field.CustomFieldId,
                                                  property.CustomFieldOptionId));

                                if (field.FieldType == (int)CustomFieldType.Color)
                                {
                                    if (!string.IsNullOrEmpty(property.OptionColor))
                                        XmlHelper.AddNode(doc, optionXml, "Color", property.OptionColor);
                                    var lstProductHasColor = CartHelper.GetProductsHasAttribute(lstChildProducts, productProperties, property.CustomFieldId, property.CustomFieldOptionId);
                                    if (lstProductHasColor.Count > 0)
                                    {
                                        var imageFolderPath = ProductHelper.MediaFolderPath(lstProductHasColor[0].SiteId, lstProductHasColor[0].ProductId);
                                        var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
                                        XmlHelper.AddNode(doc, optionXml, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath, lstProductHasColor[0].ThumbnailFile));
                                        XmlHelper.AddNode(doc, optionXml, "ImageUrl", ProductHelper.GetMediaFilePath(imageFolderPath, lstProductHasColor[0].ImageFile));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                var productProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);

                if (productProperties.Count > 0)
                {
                    var customFields = new List<CustomField>();
                    var customFieldIds = new List<int>();

                    foreach (var property in productProperties)
                    {
                        if (!customFieldIds.Contains(property.CustomFieldId))
                            customFieldIds.Add(property.CustomFieldId);
                    }

                    var tmp = CustomField.GetActiveByFields(basePage.SiteId, Product.FeatureGuid, customFieldIds, languageId);
                    customFields = CustomField.GetByOption(tmp, CustomFieldOptions.ShowInProductDetailsPage);

                    foreach (CustomField field in customFields)
                    {
                        XmlElement groupXml = doc.CreateElement("ProductProperties");
                        root.AppendChild(groupXml);

                        XmlHelper.AddNode(doc, groupXml, "FieldId", field.CustomFieldId.ToString());
                        XmlHelper.AddNode(doc, groupXml, "FieldType", field.FieldType.ToString());
                        XmlHelper.AddNode(doc, groupXml, "DataType", field.DataType.ToString());
                        XmlHelper.AddNode(doc, groupXml, "FilterType", field.FilterType.ToString());
                        XmlHelper.AddNode(doc, groupXml, "Title", field.Name);
                        XmlHelper.AddNode(doc, groupXml, "DisplayName", field.DisplayName);
                        XmlHelper.AddNode(doc, groupXml, "EnableShoppingCart", (field.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0 ? "True" : "False");

                        foreach (ProductProperty property in productProperties)
                        {
                            if (property.ProductId == product.ProductId && property.CustomFieldId == field.CustomFieldId)
                            {
                                XmlElement optionXml = doc.CreateElement("Options");
                                groupXml.AppendChild(optionXml);

                                XmlHelper.AddNode(doc, optionXml, "FieldId", field.CustomFieldId.ToString());
                                XmlHelper.AddNode(doc, optionXml, "OptionId", property.CustomFieldOptionId.ToString());

                                if (property.CustomFieldOptionId > 0)
                                {
                                    XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);
                                    XmlHelper.AddNode(doc, optionXml, "Color", property.OptionColor); //TODO: not yet implemented
                                    XmlHelper.AddNode(doc, optionXml, "Price", ProductHelper.FormatPrice(product.Price + property.OverriddenPrice, true));
                                    XmlHelper.AddNode(doc, optionXml, "PriceAdjustment", ProductHelper.FormatPrice(property.OverriddenPrice, true));
                                }
                                else
                                    XmlHelper.AddNode(doc, optionXml, "Title", property.CustomValue);

                                string pageUrl = SiteUtils.GetCurrentZoneUrl();
                                XmlHelper.AddNode(doc, optionXml, "Url", ProductHelper.GetQueryStringFilter(pageUrl, field.FilterType, field.CustomFieldId,
                                                  property.CustomFieldOptionId));
                            }
                        }
                    }
                }
            }
        }

        private void BuildProductAttributesXml(
            XmlDocument doc,
            XmlElement root, int languageId)
        {
            List<ContentAttribute> listAttributes = ContentAttribute.GetByContentAsc(product.ProductGuid, languageId);
            foreach (ContentAttribute attribute in listAttributes)
            {
                XmlElement element = doc.CreateElement("ProductAttributes");
                root.AppendChild(element);

                XmlHelper.AddNode(doc, element, "Title", attribute.Title);
                XmlHelper.AddNode(doc, element, "Content", attribute.ContentText);
            }
        }

        #endregion GenXML

        private void PopulateLabels()
        {
        }

        private void LoadSettings()
        {
            if ((product == null || product.ProductId == -1) && productId > 0)
                product = new Product(basePage.SiteId, productId, languageId);

            currentUser = SiteUtils.GetCurrentSiteUser();
            userCanUpdate = ProductPermission.CanUpdate;
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", pageNumber);

            if (product != null && product.ProductId > 0)
            {
                if (product.ParentId > 0)
                {
                    childProductWithMinPrice = product;
                    product = new Product(basePage.SiteId, product.ParentId, languageId);
                    lstChildProducts = ProductHelper.GetChildProducts(product);
                    isChildProductPage = true;
                    //lstChildProducts.Add(product);
                }
                else
                {
                    lstChildProducts = ProductHelper.GetChildProducts(product);
                    childProductWithMinPrice = GetProductWithMinPrice(lstChildProducts);
                }
            }

            if (
                //(module.ModuleId == -1)
                //|| (manufacturer.ModuleID == -1)
                //|| (manufacturer.ModuleID != module.ModuleId)
                (basePage.SiteInfo == null)
            )
            {
                // query string params have been manipulated
                pnlInnerWrap.Visible = false;
                parametersAreInvalid = true;
                return;
            }

            userCanEditAsDraft = ProductPermission.CanCreate;

            SetupCommentSystem();

            productSoldOutletter.Product = product;
            productSoldOutletter.Visible = ProductConfiguration.EnableProductSoldOutLetter && product.StockQuantity == 0;
        }

        private void SetupCommentSystem()
        {
            comment.Config = config;
            comment.Product = product;
            comment.CommentType = ProductCommentType.Rating;
        }

        private void LoadParams()
        {
            virtualRoot = WebUtils.GetApplicationRoot();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            zoneId = WebUtils.ParseInt32FromQueryString("zoneId", -1);
            productId = WebUtils.ParseInt32FromQueryString("ProductID", -1);
            languageId = WorkingCulture.LanguageId;

            if (config.LoadFirstProduct)
            {
                product = Product.GetOneByZone(zoneId, languageId);
                if (product != null && product.ProductId > 0)
                    productId = Convert.ToInt32(product.ProductId);
            }

            if (zoneId == -1) parametersAreInvalid = true;
            if (productId == -1) parametersAreInvalid = true;
            //if (!basePage.UserCanViewPage(moduleId)) { parametersAreInvalid = true; }
        }



        private void BuildProductJsonSchema(Manufacturer manufacturer, List<ContentMedia> medias, List<ProductComment> comments)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<script type=\"application/ld+json\">");
            builder.Append("{");
            builder.Append("\"@context\": \"https://schema.org/\",");
            builder.Append("\"@type\": \"Product\",");
            builder.Append($"\"name\": \"{product.Title}\",");

            if (medias.Count > 0)
            {
                builder.Append("\"image\": [");
                var imageFolderPath = ProductHelper.MediaFolderPath(basePage.SiteId, product.ProductId);

                bool add = false;
                foreach (var media in medias)
                {
                    string relativePath = siteRoot + ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile);
                    if (add)
                        builder.Append(",\"" + relativePath + "\"");
                    else
                        builder.Append("\"" + relativePath + "\"");
                    add = true;
                }
                builder.Append("],");
            }

            builder.Append($"\"description\": \"{basePage.MetaDescription.Trim()}\",");
            builder.Append($"\"sku\": \"{product.Code}\",");
            builder.Append($"\"mpn\": \"{product.ProductId}\",");

            if (manufacturer != null && manufacturer.ManufacturerId > 0)
            {
                builder.Append("\"brand\": {");
                builder.Append("\"@type\": \"Brand\",");
                builder.Append($"\"name\": \"{manufacturer.Name}\"");
                builder.Append("},");
            }
            if (product.RatingVotes > 0 && comments.Count > 0)
            {
                builder.Append("\"review\": [");
                bool add = false;
                foreach (ProductComment comment in comments)
                {
                    if (add)
                        builder.Append(",{");
                    else
                        builder.Append("{");
                    builder.Append("\"@type\": \"Review\",");
                    builder.Append("\"reviewRating\": {");

                    builder.Append("\"@type\": \"Rating\",");
                    builder.Append($"\"ratingValue\": \"{comment.Rating}\",");
                    builder.Append("\"bestRating\": \"5\"");
                    builder.Append("},");
                    builder.Append("\"author\": {");
                    builder.Append(" \"@type\": \"Person\",");
                    builder.Append($"\"name\": \"{comment.FullName}\"");
                    builder.Append("}");

                    builder.Append("}");
                    add = true;
                }
                builder.Append("],");

                var ratingValue = Convert.ToDouble(product.RatingSum / product.RatingVotes);
                builder.Append("\"aggregateRating\": {");
                builder.Append("\"@type\": \"AggregateRating\",");
                builder.Append($"\"ratingValue\": \"{ratingValue}\",");
                builder.Append($"\"reviewCount\": \"{product.RatingVotes}\"");
                builder.Append("},");
            }
            string pageUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);
            builder.Append(" \"offers\": {");
            builder.Append("\"@type\": \"Offer\",");
            builder.Append($"\"url\": \"{pageUrl}\", ");
            builder.Append($"\"priceCurrency\": \"{ProductConfiguration.WorkingCurrency}\",");
            builder.Append($"\"price\": \"{ProductHelper.FormatPrice(product.Price)}\",");
            builder.Append($" \"priceValidUntil\": \"{DateTime.MaxValue.ToString("yyy-MM-dd")}\",");
            builder.Append("\"itemCondition\": \"https://schema.org/NewCondition\",");
            builder.Append("\"availability\": \"https://schema.org/InStock\"");
            builder.Append(" }");
            builder.Append(" }");
            builder.Append("</script>");
            basePage.AdditionalMetaMarkup += builder.ToString();
        }

        #region Child products

        public static void BuildSamePriceProductsXml(
            XmlDocument doc,
            XmlElement root,
            Product product,
            Product childProductWithMinPrice,
            List<CustomField> customFields,
            bool byZoneId = true,
            string elementName = "ProductSamePrice",
            List<int> productIdsExclude = null
        )
        {
            var priceAdditional = 1000000M;
            var pageSize = 4;
            var priceMin = product.Price - priceAdditional;
            var priceMax = product.Price + priceAdditional;
            if (childProductWithMinPrice != null)
            {
                priceMin = childProductWithMinPrice.Price - priceAdditional;
                priceMax = childProductWithMinPrice.Price + priceAdditional;
            }
            var propertyCondition = (string)null;// " p.ProductID <> " + product.ProductId.ToString() + " ";
            var parentId = 0;
            var o = customFields.Where(s => s.FieldType != (int)CustomFieldType.Color).FirstOrDefault();
            if (o != null)
                parentId = -1;


            string zoneIds = product.ZoneId.ToString();
            if (!byZoneId) zoneIds = string.Empty;
            var lstProducts = Product.GetPageAdv(pageNumber: 1, pageSize: pageSize * 2, siteId: product.SiteId,
                zoneIds: zoneIds, publishStatus: 1, languageId: WorkingCulture.LanguageId,
                priceMin: priceMin, priceMax: priceMax,
                propertyCondition: propertyCondition,
                parentId: parentId);
            if (childProductWithMinPrice != null)
                lstProducts = lstProducts.Where(s => s.ProductId != childProductWithMinPrice.ProductId && s.ProductId != product.ProductId).ToList();
            else
                lstProducts = lstProducts.Where(s => s.ProductId != product.ProductId).ToList();

            if (productIdsExclude != null)
                lstProducts = lstProducts.Where(s => !productIdsExclude.Contains(s.ProductId)).ToList();
            var lstTmp = new List<Product>();
            lstProducts.ForEach(s =>
            {
                if (s.ParentId > 0)
                    lstTmp.Add(s);
                else if (s.ParentId == 0)
                {
                    var obj = lstProducts.Where(p => p.ParentId == s.ProductId).FirstOrDefault();
                    if (obj == null)
                        lstTmp.Add(s);
                }
            });
            //lstProducts = lstProducts.Where(s => s.ParentId > 0 || (s.ParentId == 0 && lstProducts.Select(p => p.ParentId == s.ProductId).Count() == 0)).ToList();

            lstProducts = lstTmp.Take(pageSize).ToList();

            var siteRoot = SiteUtils.GetNavigationSiteRoot();
            var lstDiscountItems = DiscountAppliedToItem.GetActive(product.SiteId, -10, lstProducts);
            foreach (Product productOther in lstProducts)
            {
                XmlElement productXml = doc.CreateElement(elementName);
                root.AppendChild(productXml);

                var productIds = new List<int>();
                if (childProductWithMinPrice != null)
                    productIds.Add(childProductWithMinPrice.ProductId);
                else
                    productIds.Add(product.ProductId);
                productIds.Add(productOther.ProductId);

                ProductHelper.BuildProductDataXml(doc, productXml, productOther, lstDiscountItems);
            }
        }

        public static void BuildProductGroupSpecsXml(
                 XmlDocument doc,
                 XmlElement root,
                 int languageId, Product product, Product childProductWithMinPrice)
        {
            //Build Specifications
            var customFields = new List<CustomField>();
            var pProperties = new List<ProductProperty>();
            if (childProductWithMinPrice != null)
            {
                pProperties = ProductProperty.GetPropertiesByProduct(childProductWithMinPrice.ProductId);
                var customFieldIds = new List<int>();
                foreach (var property in pProperties)
                {
                    if (!customFieldIds.Contains(property.CustomFieldId))
                        customFieldIds.Add(property.CustomFieldId);
                }

                var tmp = CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds, languageId);
                customFields = CustomField.GetByOption(tmp, CustomFieldOptions.EnableComparing);
            }
            if (customFields.Count == 0)
            {
                pProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);
                var customFieldIds = new List<int>();
                foreach (var property in pProperties)
                {
                    if (!customFieldIds.Contains(property.CustomFieldId))
                        customFieldIds.Add(property.CustomFieldId);
                }

                var tmp = CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds, languageId);
                customFields = CustomField.GetByOption(tmp, CustomFieldOptions.EnableComparing);
            }
            if (customFields.Count > 0)
            {
                var groups = customFields.Where(s => !string.IsNullOrEmpty(s.InvalidMessage)).Select(s => s.InvalidMessage).Distinct().ToList();
                if (groups.Count > 0)
                {
                    var lstTemp = new List<CustomField>();
                    foreach (var group in groups)
                    {
                        var groupXml = doc.CreateElement("GroupSpecs");
                        root.AppendChild(groupXml);

                        XmlHelper.AddNode(doc, groupXml, "Title", group);

                        var customFieldsByGroup = customFields.Where(s => s.InvalidMessage == group).OrderBy(s => s.DisplayOrder).ToList();
                        if (customFieldsByGroup.Count > 0)
                            lstTemp.AddRange(customFieldsByGroup);
                    }

                    customFields.Clear();
                    customFields.AddRange(lstTemp);
                }
                foreach (var field in customFields)
                    BuildPropertyByGroup(doc, root, field, pProperties);
            }
        }

        private static void BuildPropertyByGroup(XmlDocument doc, XmlElement root, CustomField field, List<ProductProperty> pProperties)
        {
            var groupXml = doc.CreateElement("Specifications");
            root.AppendChild(groupXml);

            XmlHelper.AddNode(doc, groupXml, "Title", field.Name);
            XmlHelper.AddNode(doc, groupXml, "DisplayName", field.DisplayName);
            XmlHelper.AddNode(doc, groupXml, "Group", field.InvalidMessage);

            var property = pProperties.Where(s => s.CustomFieldId == field.CustomFieldId).FirstOrDefault();
            if (property != null)
            {
                if (property.CustomValue == "1")
                    XmlHelper.AddNode(doc, groupXml, "Value", "<i class=\"fa fa-check-circle\"></i>");
                else
                    XmlHelper.AddNode(doc, groupXml, "Value", ProductHelper.FormatSpecificationsText(property.CustomValue));
            }
        }

        private Product GetProductWithMinPrice(List<Product> lstChildProducts)
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
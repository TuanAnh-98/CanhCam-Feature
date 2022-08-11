/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23
/// 2015-06-04: enable keyword filtering

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductListControl : UserControl
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(ProductListControl));

        private SiteSettings siteSettings = null;
        private int pageNumber = 1;
        private int totalPages = 1;
        private bool showCommentCounts = true;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private CmsBasePage basePage = null;
        private Module module = null;
        protected ProductConfiguration config = new ProductConfiguration();
        private int zoneId = -1;
        private int moduleId = -1;

        private bool userCanUpdate = false;

        private string siteRoot = string.Empty;
        private string imageSiteRoot = string.Empty;
        private bool allowComments = false;

        private SiteUser currentUser = null;

        public string SiteRoot
        {
            get { return siteRoot; }
            set { siteRoot = value; }
        }

        public string ImageSiteRoot
        {
            get { return imageSiteRoot; }
            set { imageSiteRoot = value; }
        }

        public int ModuleId
        {
            get { return moduleId; }
            set { moduleId = value; }
        }

        public ProductConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        private string moduleTitle = string.Empty;

        public string ModuleTitle
        {
            get { return moduleTitle; }
            set { moduleTitle = value; }
        }

        #endregion Properties

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (config.LoadFirstProduct)
            {
                Visible = false;
                return;
            }

            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            int pageSize = config.PageSize;
            int pageSizeQueryString = WebUtils.ParseInt32FromQueryString("pagesize", 0);
            if (config.PageSizeOptions.Contains(pageSizeQueryString.ToString()))
                pageSize = pageSizeQueryString;

            var doc = GetPageXml(zoneId, pageNumber, pageSize, out totalPages);

            var pageUrl = ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl, false);
            if (pageUrl.Contains("?"))
                pageUrl += "&amp;" + ProductHelper.QueryStringPageNumberParam + "={0}";
            else
                pageUrl += "?" + ProductHelper.QueryStringPageNumberParam + "={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.PageSize = pageSize;
            pgr.PageCount = totalPages;
            pgr.CurrentIndex = pageNumber;
            divPager.Visible = (totalPages > 1);

            if (config.XsltFileName.Length > 0)
            {
                string viewMode = WebUtils.ParseStringFromQueryString("view", string.Empty);
                string xsltFileName = config.XsltFileName;
                if (viewMode.ToLower() == "list")
                    xsltFileName = xsltFileName.Substring(0, xsltFileName.LastIndexOf(".xslt",
                                                          StringComparison.InvariantCultureIgnoreCase)) + "_List.xslt";

                if (ProductHelper.IsAjaxRequest(Request))
                {
                    Response.Write("<div>");
                    Response.Write(XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", xsltFileName), doc));
                    if (divPager.Visible)
                    {
                        var pagerResponse = ProductHelper.RenderControlToHtml(divPager).Replace("&amp;" + ProductHelper.QueryStringPageNumberParam + "=", "&" + ProductHelper.QueryStringPageNumberParam + "=");
                        Response.Write(pagerResponse);
                    }
                    Response.Write("</div>");

                    if (HttpContext.Current.Items["IsAjaxResponse"] != null)
                        Response.End();

                    HttpContext.Current.Items["IsAjaxResponse"] = true;

                    return;
                }

                XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", xsltFileName), doc);
            }
        }

        private XmlDocument GetPageXml(int zoneId,
                                       int pageNumber,
                                       int pageSize,
                                       out int totalPages)
        {
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<ProductList></ProductList>");
            XmlElement root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.moduleTitle);
            XmlHelper.AddNode(doc, root, "ZoneTitle", basePage.CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "ZoneImageUrl", basePage.CurrentZone.PrimaryImage);
            XmlHelper.AddNode(doc, root, "ZoneSecondImageUrl", basePage.CurrentZone.SecondImage);
            XmlHelper.AddNode(doc, root, "ZoneDescription", basePage.CurrentZone.Description);
            XmlHelper.AddNode(doc, root, "ViewMore", ProductResources.ViewMoreLabel);
            XmlHelper.AddNode(doc, root, "SiteUrl", siteRoot);
            XmlHelper.AddNode(doc, root, "ZoneUrl", SiteUtils.GetCurrentZoneUrl());

            if (module != null && module.ResourceFileDef.Length > 0 && module.ResourceKeyDef.Length > 0)
            {
                List<string> lstResourceKeys = module.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(module.ResourceFileDef, item));
            }

            //Render view mode
            BuildViewModeXml(doc, root);

            //Render sort mode
            BuildSortModeXml(doc, root);

            var languageId = WorkingCulture.LanguageId;
            var lstProducts = new List<Product>();
            var propertyCondition = string.Empty;
            var andClause = " ";
            var numberGroup = 0;
            var lstCustomFields = CustomFieldHelper.GetCustomFieldsFromContext(siteSettings.SiteId, Product.FeatureGuid,
                                                basePage.CurrentZone.ZoneGuid, languageId);
            foreach (CustomField field in lstCustomFields)
            {
                if (
                    field.DataType == (int)CustomFieldDataType.CheckBox
                    || field.DataType == (int)CustomFieldDataType.SelectBox
                )
                {
                    if (field.FilterType == (int)CustomFieldFilterType.ByValue)
                    {
                        string paramName = ProductHelper.QueryStringFilterSingleParam + field.CustomFieldId.ToString();
                        int optionValue = WebUtils.ParseInt32FromQueryString(paramName, -1);

                        if (optionValue > 0)
                        {
                            propertyCondition += andClause + "(CustomFieldID=" + field.CustomFieldId + " AND CustomFieldOptionID=" +
                                                 optionValue.ToString() + ")";
                            andClause = " OR ";
                            numberGroup += 1;
                        }
                    }
                    else
                    {
                        string paramName = ProductHelper.QueryStringFilterMultiParam + field.CustomFieldId.ToString();
                        string optionValues = WebUtils.ParseStringFromQueryString(paramName, string.Empty);

                        // Split and validate data
                        List<int> lstValues = new List<int>();
                        optionValues.SplitOnCharAndTrim('/').ForEach(s =>
                        {
                            int value = -1;
                            if (int.TryParse(s, out value))
                                lstValues.Add(value);
                        });
                        if (lstValues.Count > 0)
                        {
                            propertyCondition += andClause + "(CustomFieldID=" + field.CustomFieldId + " AND CustomFieldOptionID IN (" + string.Join(",",
                                                 lstValues.ToArray()) + "))";
                            andClause = " OR ";
                            numberGroup += 1;
                        }
                    }
                }
            }

            if (numberGroup > 0)
                propertyCondition = "(SELECT COUNT(DISTINCT CustomFieldID) FROM gb_ProductProperties WHERE ProductID=p.ProductID AND (" +
                                    propertyCondition + ")) = " + numberGroup.ToString();

            string keyword = null;
            if (config.EnableKeywordFiltering)
                keyword = WebUtils.ParseStringFromQueryString(ProductHelper.QueryStringKeywordParam, null);

            int sort = WebUtils.ParseInt32FromQueryString(ProductHelper.QueryStringSortModeParam, 0);
            ProductHelper.GetPriceFromQueryString(out decimal? priceMin, out decimal? priceMax);

            var lstManufacturerId = ManufacturerHelper.ParseManufacturerFromQueryString();
            if (lstManufacturerId.Count > 0)
            {
                if (propertyCondition.Length > 0)
                    propertyCondition += " AND";
                propertyCondition += " ManufacturerID IN (" + string.Join(",", lstManufacturerId.ToArray()) + ")";
            }

            if (sort == 30)
            {
                if (propertyCondition.Length > 0)
                    propertyCondition += " AND";

                propertyCondition += " EXISTS (SELECT 1 FROM gb_DiscountAppliedToItems di INNER JOIN [dbo].[gb_Discount] d ON di.DiscountID=d.DiscountID WHERE d.DiscountType=" + ((int)DiscountType.ByCatalog).ToString() + " AND d.IsActive = 1 AND (getdate() > ISNULL(d.StartDate, '1/1/1900')) AND (getdate() < ISNULL(d.EndDate, '1/1/2999')) AND d.SiteID = " + siteSettings.SiteId.ToString() + " AND ((di.AppliedType=0 AND di.ItemID=p.ZoneID) OR (di.AppliedType=1 AND (di.ItemID=-1 OR di.ItemID=p.ProductID))) )";
            }

            int totalRows = 0;
            if (!string.IsNullOrEmpty(propertyCondition)
                || priceMin != null
                || priceMax != null
                || sort > 0
                || !string.IsNullOrEmpty(keyword)
               )
            {
                string rangeZoneIds = null;
                if (ProductConfiguration.FilterProductByTopLevelParentZones)
                {
                    var topLevelNode = SiteUtils.GetTopLevelParentNode(SiteUtils.GetSiteMapNodeByZoneId(zoneId));
                    if (topLevelNode != null)
                        rangeZoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, topLevelNode.ZoneId);
                }
                else
                {
                    if (config.ZoneIds.Length == 0)
                        rangeZoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, zoneId);
                    else
                        rangeZoneIds = config.ZoneIds;
                }

                totalRows = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: rangeZoneIds, publishStatus: 1, languageId: languageId, priceMin: priceMin, priceMax: priceMax, propertyCondition: propertyCondition, keyword: keyword, searchProductZone: ProductConfiguration.EnableProductZone);

                if (pageSize == -1)
                    pageSize = totalRows;

                totalPages = CaculateTotalPages(pageSize, totalRows);
                lstProducts = ProductCacheHelper.GetPageAdv(pageNumber: pageNumber, pageSize: pageSize, siteId: siteSettings.SiteId, zoneIds: rangeZoneIds, publishStatus: 1, languageId: languageId, priceMin: priceMin, priceMax: priceMax, propertyCondition: propertyCondition, keyword: keyword, searchCode: false, searchProductZone: ProductConfiguration.EnableProductZone, orderBy: sort);
            }
            else
            {
                if (config.ZoneIds.Length == 0)
                {
                    if (config.ShowAllProducts)
                    {
                        string rangeZoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, zoneId);
                        totalRows = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: rangeZoneIds, publishStatus: 1, languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone);

                        if (pageSize == -1)
                            pageSize = totalRows;

                        totalPages = CaculateTotalPages(pageSize, totalRows);
                        lstProducts = ProductCacheHelper.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: rangeZoneIds, publishStatus: 1, languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone, pageNumber: pageNumber, pageSize: pageSize);
                    }
                    else
                    {
                        totalRows = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneId: zoneId, publishStatus: 1, languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone);

                        if (pageSize == -1)
                            pageSize = totalRows;

                        totalPages = CaculateTotalPages(pageSize, totalRows);
                        lstProducts = ProductCacheHelper.GetPageAdv(siteId: siteSettings.SiteId, zoneId: zoneId, publishStatus: 1, languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone, pageNumber: pageNumber, pageSize: pageSize);
                    }
                }
                else
                {
                    totalRows = ProductCacheHelper.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: config.ZoneIds, publishStatus: 1, searchProductZone: ProductConfiguration.EnableProductZone, languageId: languageId);

                    if (pageSize == -1)
                        pageSize = totalRows;

                    totalPages = CaculateTotalPages(pageSize, totalRows);
                    lstProducts = ProductCacheHelper.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: config.ZoneIds, publishStatus: 1, languageId: languageId, searchProductZone: ProductConfiguration.EnableProductZone, pageNumber: pageNumber, pageSize: pageSize);
                }
            }

            XmlHelper.AddNode(doc, root, "TotalProducts", totalRows.ToString());
            BuildPageSizeXml(doc, root);

            var productIds = new List<int>();
            if (ProductConfiguration.EnableComparing)
            {
                productIds = ProductHelper.GetCompareProductsIds();

                XmlHelper.AddNode(doc, root, "CompareProductsCount", productIds.Count.ToString());
                XmlHelper.AddNode(doc, root, "CompareListUrl", siteRoot + "/product/compare.aspx");

                var compareProducts = ProductHelper.GetCompareProducts();
                var lstDiscountItemsCompare = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -1, lstProducts);
                foreach (Product product in compareProducts)
                {
                    XmlElement productXml = doc.CreateElement("Compared");
                    root.AppendChild(productXml);

                    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItemsCompare, timeZone, timeOffset, ProductHelper.BuildEditLink(product,
                                                      basePage, userCanUpdate, currentUser));
                }
            }

            var productProperties = new List<ProductProperty>();
            var customFields = new List<CustomField>();
            if (config.ShowCustomFieldsInProductList)
            {
                List<int> lstProductIds = new List<int>();
                foreach (Product product in lstProducts)
                    lstProductIds.Add(product.ProductId);

                if (lstProductIds.Count > 0)
                    productProperties = ProductProperty.GetPropertiesByProducts(lstProductIds);

                if (productProperties.Count > 0)
                {
                    var customFieldIds = new List<int>();

                    foreach (var property in productProperties)
                    {
                        if (!customFieldIds.Contains(property.CustomFieldId))
                            customFieldIds.Add(property.CustomFieldId);
                    }

                    var tmp = CustomField.GetActiveByFields(basePage.SiteId, Product.FeatureGuid, customFieldIds, languageId);
                    customFields = CustomField.GetByOption(tmp, CustomFieldOptions.ShowInCatalogPages);
                }
            }

            var lstDiscountItems = DiscountAppliedToItemHelper.GetActive(siteSettings.SiteId, -10, lstProducts);
            foreach (var product in lstProducts)
            {
                XmlElement productXml = doc.CreateElement("Product");
                root.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset, ProductHelper.BuildEditLink(product,
                                                  basePage, userCanUpdate, currentUser), productIds);

                if (config.ShowCustomFieldsInProductList)
                    BuildCustomFieldsXml(doc, productXml, product.ProductId, customFields, productProperties);
                if (config.ShowAllImagesInProductList)
                    BuildProductImages(doc, productXml, product);
            }

            if (pageNumber < totalPages)
            {
                string pageUrl = ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl);

                if (pageUrl.Contains("?"))
                    pageUrl += "&" + ProductHelper.QueryStringPageNumberParam + "=" + (pageNumber + 1).ToString();
                else
                    pageUrl += "?" + ProductHelper.QueryStringPageNumberParam + "=" + (pageNumber + 1).ToString();

                XmlHelper.AddNode(doc, root, "NextPageUrl", pageUrl);
            }
            if (GoogleTrackingHelper.Enable && GoogleTrackingHelper.EnableProductList)
                XmlHelper.AddNode(doc, root, "GoogleTrackingScript", GoogleTrackingHelper.BuildGoogleImpressions(doc));


            return doc;
        }

        private void BuildProductImages(XmlDocument doc, XmlElement root, Product product)
        {
            var imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
            var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
            var listMedia = ContentMedia.GetByContentDesc(product.ProductGuid);
            foreach (ContentMedia media in listMedia)
                if (media.MediaType != (int)ProductMediaType.Video)
                    BuildProductImagesXml(doc, root, media, imageFolderPath, thumbnailImageFolderPath);
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

        private int CaculateTotalPages(int pageSize, int totalRows)
        {
            int totalPages = 1;

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
                totalPages = 1;
            else if (pageSize > 0)
            {
                Math.DivRem(totalRows, pageSize, out int remainder);
                if (remainder > 0)
                    totalPages += 1;
            }

            return totalPages;
        }

        public void BuildCustomFieldsXml(
            XmlDocument doc,
            XmlElement productXml,
            int productId,
            List<CustomField> customFields,
            List<ProductProperty> productProperties)
        {
            string pageUrl = SiteUtils.GetCurrentZoneUrl();

            foreach (CustomField field in customFields)
            {
                XmlElement groupXml = doc.CreateElement("ProductProperties");
                productXml.AppendChild(groupXml);

                XmlHelper.AddNode(doc, groupXml, "FieldId", field.CustomFieldId.ToString());
                XmlHelper.AddNode(doc, groupXml, "FieldType", field.FieldType.ToString());
                XmlHelper.AddNode(doc, groupXml, "DataType", field.DataType.ToString());
                XmlHelper.AddNode(doc, groupXml, "FilterType", field.FilterType.ToString());
                XmlHelper.AddNode(doc, groupXml, "Title", field.Name);

                foreach (ProductProperty property in productProperties)
                {
                    if (property.ProductId == productId && property.CustomFieldId == field.CustomFieldId)
                    {
                        XmlElement optionXml = doc.CreateElement("Options");
                        groupXml.AppendChild(optionXml);

                        XmlHelper.AddNode(doc, optionXml, "OptionId", property.CustomFieldOptionId.ToString());

                        if (property.CustomFieldOptionId > 0)
                            XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);
                        else
                            XmlHelper.AddNode(doc, optionXml, "Title", property.CustomValue);

                        XmlHelper.AddNode(doc, optionXml, "Url", ProductHelper.GetQueryStringFilter(pageUrl, field.FilterType, field.CustomFieldId,
                                          property.CustomFieldOptionId));
                    }
                }
            }
        }

        public void BuildViewModeXml(
            XmlDocument doc,
            XmlElement root)
        {
            // table
            XmlElement element = doc.CreateElement("ViewAs");
            root.AppendChild(element);

            string pageUrl = ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl, ProductHelper.QueryStringViewModeParam,
                             "grid");
            XmlHelper.AddNode(doc, element, "Url", pageUrl);
            string viewMode = WebUtils.ParseStringFromQueryString(ProductHelper.QueryStringViewModeParam, string.Empty);
            if (viewMode.Length == 0 || viewMode.ToLower() == "grid")
                XmlHelper.AddNode(doc, element, "IsActive", "true");

            // list
            XmlElement listXml = doc.CreateElement("ViewAs");
            root.AppendChild(listXml);

            pageUrl = ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl, ProductHelper.QueryStringViewModeParam, "list");
            XmlHelper.AddNode(doc, listXml, "Url", pageUrl);
            if (viewMode.ToLower() == "list")
                XmlHelper.AddNode(doc, listXml, "IsActive", "true");
        }

        public void BuildSortModeXml(
            XmlDocument doc,
            XmlElement root)
        {
            int sortMode = WebUtils.ParseInt32FromQueryString(ProductHelper.QueryStringSortModeParam, 0);
            XmlHelper.AddNode(doc, root, "CurrentSort", sortMode.ToString());

            var sortUrl = SiteUtils.BuildUrlLeaveOutParam(Request.RawUrl, ProductHelper.QueryStringSortModeParam, false);
            XmlHelper.AddNode(doc, root, "SortUrl", sortUrl + (sortUrl.Contains("?") ? "&" : "?"));

            // 0 - position
            var element = doc.CreateElement("SortBy");
            root.AppendChild(element);
            XmlHelper.AddNode(doc, element, "Title", ProductResources.SortByNewest);
            XmlHelper.AddNode(doc, element, "Value", "0");
            XmlHelper.AddNode(doc, element, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl,
                              ProductHelper.QueryStringSortModeParam, "0"));
            if (sortMode == 0)
                XmlHelper.AddNode(doc, element, "IsActive", "true");

            //// 30 - discount
            //element = doc.CreateElement("SortBy");
            //root.AppendChild(element);
            //XmlHelper.AddNode(doc, element, "Title", ProductResources.SortByNewest);
            //XmlHelper.AddNode(doc, element, "Value", "0");
            //XmlHelper.AddNode(doc, element, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl,
            //                  ProductHelper.QueryStringSortModeParam, "30"));
            //if (sortMode == 30)
            //    XmlHelper.AddNode(doc, element, "IsActive", "true");

            //// 20 - view count
            //element = doc.CreateElement("SortBy");
            //root.AppendChild(element);
            //XmlHelper.AddNode(doc, element, "Title", ProductResources.SortByNewest);
            //XmlHelper.AddNode(doc, element, "Value", "20");
            //XmlHelper.AddNode(doc, element, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl,
            //                  ProductHelper.QueryStringSortModeParam, "0"));
            //if (sortMode == 20)
            //    XmlHelper.AddNode(doc, element, "IsActive", "true");

            // 10 - Price: Low to High
            element = doc.CreateElement("SortBy");
            root.AppendChild(element);
            XmlHelper.AddNode(doc, element, "Title", ProductResources.SortByPriceLowToHigh);
            XmlHelper.AddNode(doc, element, "Value", "10");
            XmlHelper.AddNode(doc, element, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl,
                              ProductHelper.QueryStringSortModeParam, "10"));
            if (sortMode == 10)
                XmlHelper.AddNode(doc, element, "IsActive", "true");

            // 11 - Price: High to Low
            element = doc.CreateElement("SortBy");
            root.AppendChild(element);
            XmlHelper.AddNode(doc, element, "Title", ProductResources.SortByPriceHighToLow);
            XmlHelper.AddNode(doc, element, "Value", "11");
            XmlHelper.AddNode(doc, element, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl,
                              ProductHelper.QueryStringSortModeParam, "11"));
            if (sortMode == 11)
                XmlHelper.AddNode(doc, element, "IsActive", "true");
        }

        private void BuildPageSizeXml(
            XmlDocument doc,
            XmlElement root)
        {
            if (config.PageSizeOptions.Count > 1)
            {
                int pageSizeQueryString = WebUtils.ParseInt32FromQueryString("pagesize", 0);
                foreach (string option in config.PageSizeOptions)
                {
                    XmlElement element = doc.CreateElement("PageSize");
                    root.AppendChild(element);

                    if (pageSizeQueryString.ToString() == option || (pageSizeQueryString == 0 && config.PageSize.ToString() == option))
                        XmlHelper.AddNode(doc, element, "IsActive", true.ToString().ToLower());
                    else
                        XmlHelper.AddNode(doc, element, "IsActive", false.ToString().ToLower());

                    XmlHelper.AddNode(doc, element, "Title", option);
                    XmlHelper.AddNode(doc, element, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl, "pagesize", option));
                }
            }
        }

        protected virtual void PopulateLabels()
        {
        }

        protected virtual void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            zoneId = WebUtils.ParseInt32FromQueryString("zoneid", zoneId);
            currentUser = SiteUtils.GetCurrentSiteUser();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            pageNumber = WebUtils.ParseInt32FromQueryString(ProductHelper.QueryStringPageNumberParam, pageNumber);

            if (Page is CmsBasePage)
            {
                basePage = Page as CmsBasePage;
                module = basePage.GetModule(moduleId, Product.FeatureGuid);
            }

            userCanUpdate = ProductPermission.CanUpdate;

            if (module == null) return;

            allowComments = displaySettings.ShowComments && showCommentCounts;

            if (displaySettings.ShowComments)
            {
                //if ((DisqusSiteShortName.Length > 0) && (config.CommentSystem == "disqus"))
                //{
                //    disqusFlag = "#disqus_thread";
                //    disqus.SiteShortName = DisqusSiteShortName;
                //    disqus.RenderCommentCountScript = true;
                //}

                //if ((IntenseDebateAccountId.Length > 0) && (config.CommentSystem == "intensedebate"))
                //{
                //    ShowCommentCounts = false;
                //}

                if (config.CommentSystem == "facebook")
                    showCommentCounts = false;
            }
        }
    }
}
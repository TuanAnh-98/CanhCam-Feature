/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-07-26
/// Last Modified:			2014-11-21

using CanhCam.Business;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Web;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// Query string structure: ?m=&view=&sort=&price=from-to=&fxxx=&mfxxx=&keyword=&pagesize=&pagenumber
    /// </summary>
    public partial class ProductFilterModule : SiteModuleControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            XmlDocument doc = new XmlDocument
            {
                XmlResolver = null
            };

            doc.LoadXml("<ProductFilter></ProductFilter>");
            XmlElement root = doc.DocumentElement;

            string rawUrl = Request.RawUrl;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "ClearAllUrl", WebUtils.GetUrlWithoutQueryString(rawUrl));

            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                var lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
            }

            var rawUrlLeaveOutPageNumber = ProductHelper.BuildFilterUrlLeaveOutPageNumber(rawUrl, false);
            var languageId = WorkingCulture.LanguageId;
            var lstCustomFields = CustomFieldHelper.GetCustomFieldsFromContext(siteSettings.SiteId, Product.FeatureGuid, CurrentZone.ZoneGuid, languageId);
            foreach (CustomField field in lstCustomFields)
            {
                if (
                    (field.DataType == (int)CustomFieldDataType.CheckBox
                     || field.DataType == (int)CustomFieldDataType.SelectBox)
                    && (field.FilterType == (int)CustomFieldFilterType.ByValue
                        || field.FilterType == (int)CustomFieldFilterType.ByMultipleValues)
                )
                {
                    XmlElement productXml = doc.CreateElement("Group");
                    root.AppendChild(productXml);

                    XmlHelper.AddNode(doc, productXml, "GroupId", field.CustomFieldId.ToString());
                    XmlHelper.AddNode(doc, productXml, "Title", field.Name);
                    XmlHelper.AddNode(doc, productXml, "FieldType", field.FieldType.ToString());
                    XmlHelper.AddNode(doc, productXml, "FilterType", field.FilterType.ToString());

                    string paramName = string.Empty;
                    bool isCheckBox = true;
                    if (field.FilterType == (int)CustomFieldFilterType.ByValue)
                    {
                        paramName = ProductHelper.QueryStringFilterSingleParam + field.CustomFieldId.ToString();

                        XmlHelper.AddNode(doc, productXml, "ClearUrl", SiteUtils.BuildUrlLeaveOutParam(rawUrlLeaveOutPageNumber, paramName));

                        isCheckBox = false;
                    }
                    else
                    {
                        paramName = ProductHelper.QueryStringFilterMultiParam + field.CustomFieldId.ToString();

                        XmlHelper.AddNode(doc, productXml, "ClearUrl", SiteUtils.BuildUrlLeaveOutParam(rawUrlLeaveOutPageNumber, paramName));

                        isCheckBox = true;
                    }

                    XmlHelper.AddNode(doc, productXml, "IsCheckBox", isCheckBox.ToString().ToLower());

                    var lstOptions = CustomFieldOption.GetByCustomField(field.CustomFieldId, languageId);
                    foreach (var option in lstOptions)
                    {
                        var optionXml = doc.CreateElement("Option");
                        productXml.AppendChild(optionXml);

                        XmlHelper.AddNode(doc, optionXml, "GroupId", option.CustomFieldId.ToString());
                        XmlHelper.AddNode(doc, optionXml, "OptionId", option.CustomFieldOptionId.ToString());
                        XmlHelper.AddNode(doc, optionXml, "Title", option.Name);
                        if (field.FieldType == (int)CustomFieldType.Color)
                            XmlHelper.AddNode(doc, optionXml, "Color", option.OptionColor);
                        XmlHelper.AddNode(doc, optionXml, "FieldType", field.FieldType.ToString());
                        XmlHelper.AddNode(doc, optionXml, "FilterType", field.FilterType.ToString());
                        XmlHelper.AddNode(doc, optionXml, "IsCheckBox", isCheckBox.ToString().ToLower());
                        XmlHelper.AddNode(doc, optionXml, "IsActive", CustomFieldHelper.IsActive(paramName, option.CustomFieldOptionId.ToString(), field.FilterType).ToString().ToLower());

                        //XmlHelper.AddNode(doc, optionXml, "QueryString", "");
                        var queryString = string.Empty;
                        var pageUrl = CustomFieldHelper.BuildFilterUrl(rawUrlLeaveOutPageNumber, paramName, option.CustomFieldOptionId, field.FilterType, out queryString);
                        //string rawUrlLeaveOutManufacturer = urlWithoutQueryString + BuildSpecFilterQueryString(queryString, paramName, option.CustomFieldOptionId, field.FilterType);
                        XmlHelper.AddNode(doc, optionXml, "QueryString", queryString);
                        XmlHelper.AddNode(doc, optionXml, "Url", pageUrl);

                        XmlHelper.AddNode(doc, optionXml, "ClearUrl", SiteUtils.BuildUrlLeaveOutParam(rawUrlLeaveOutPageNumber, paramName));
                    }
                }
            }

            //XmlElement priceXml = doc.CreateElement("Price");
            //root.AppendChild(priceXml);
            //XmlHelper.AddNode(doc, priceXml, "Title", string.Empty);
            //XmlHelper.AddNode(doc, priceXml, "Url", ProductHelper.BuildFilterUrlLeaveOutPageNumber(rawUrlLeaveOutPageNumber, ProductHelper.QueryStringPriceParam, string.Empty));
            XmlHelper.AddNode(doc, root, "UrlWithPrice", ProductHelper.BuildFilterUrlLeaveOutPageNumber(rawUrlLeaveOutPageNumber, true));
            XmlHelper.AddNode(doc, root, "UrlWithoutPrice",
                              SiteUtils.BuildUrlLeaveOutParam(ProductHelper.BuildFilterUrlLeaveOutPageNumber(rawUrlLeaveOutPageNumber),
                                      ProductHelper.QueryStringPriceParam));

            ProductHelper.GetPriceFromQueryString(out decimal? priceMin, out decimal? priceMax);
            if (priceMin.HasValue)
                XmlHelper.AddNode(doc, root, "PriceMin", priceMin.Value.ToString());
            if (priceMax.HasValue)
                XmlHelper.AddNode(doc, root, "PriceMax", priceMax.Value.ToString());

            //Render sort mode
            BuildSortModeXml(doc, root);


            if (ProductHelper.IsAjaxRequest(Request))
            {
                Response.Write(XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc));

                if (HttpContext.Current.Items["IsAjaxResponse"] != null)
                    Response.End();

                HttpContext.Current.Items["IsAjaxResponse"] = true;

                return;
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        //private string BuildSpecFilterQueryString(string queryString, string paramName, int optionId, int filterType)
        //{
        //    string paramValue = string.Empty;
        //    if (filterType == (int)CustomFieldFilterType.ByValue)
        //    {
        //        paramValue = optionId.ToString();
        //    }
        //    else
        //    {
        //        paramValue = WebUtils.ParseStringFromQueryString(paramName, string.Empty);

        //        if (paramValue.Contains("/" + optionId.ToString() + "/"))
        //            paramValue = paramValue.Replace(string.Format("/{0}/", optionId.ToString()), "/");
        //        else
        //        {
        //            if (!paramValue.EndsWith("/"))
        //                paramValue += "/";
        //            paramValue = paramValue + string.Format("{0}/", optionId.ToString());
        //        }
        //    }

        //    string result = WebUtils.BuildQueryString(queryString, paramName);
        //    if (result.StartsWith("&"))
        //        result = result.Remove(0, 1);
        //    if (result.Length > 0 && !result.StartsWith("?"))
        //        result = "?" + result;

        //    if (paramValue.Length > 0 && paramValue != "/")
        //    {
        //        if (result.Contains("?"))
        //            result += string.Format("&{0}={1}", paramName, paramValue);
        //        else
        //            result += string.Format("?{0}={1}", paramName, paramValue);
        //    }

        //    return result;
        //}
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

        protected virtual void LoadSettings()
        {
            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }
    }
}
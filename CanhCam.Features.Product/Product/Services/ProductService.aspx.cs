using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public class ProductService : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductService));

        private string method = string.Empty;
        private NameValueCollection postParams = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/json";
            Encoding encoding = new UTF8Encoding();
            Response.ContentEncoding = encoding;

            try
            {
                LoadParams();

                if (
                    method != "SearchProducts"
                    )
                {
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = "No method found with the " + method
                    }));
                    return;
                }

                if (method == "SearchProducts")
                {
                    Response.Write(SearchProducts());
                    return;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                Response.Write(StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Failed to process from the server. Please refresh the page and try one more time."
                }));
            }
        }

        private string SearchProducts()
        {
            var languageId = WorkingCulture.LanguageId;
            var pageSize = ConfigHelper.GetIntProperty("Product:MaximumProductResultsOnAjaxSearch", 10);
            var xsltFileName = ConfigHelper.GetStringProperty("Product:AjaxSearchXsltFileName", "AjaxProductSearchs.xslt");
            var rangeZoneIds = (string)null;
            var zoneId = -1;
            var position = -1;
            if (postParams.Get("position") != null)
                int.TryParse(postParams.Get("position"), out position);
            if (postParams.Get("zoneid") != null)
            {
                if (int.TryParse(postParams.Get("zoneid"), out zoneId) && zoneId > 0)
                    rangeZoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, zoneId);
            }

            if (string.IsNullOrEmpty(rangeZoneIds) && postParams.Get("zoneids") != null)
            {
                var zIds = postParams.Get("zoneids").ToString().SplitOnCharAndTrim(';');
                if (zIds.Count > 10)
                    zIds = zIds.Take(10).ToList();

                var sepa = string.Empty;
                foreach (var id in zIds)
                {
                    var idTmp = -1;
                    if (int.TryParse(id, out idTmp))
                    {
                        var str = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, idTmp);
                        if (!string.IsNullOrEmpty(str))
                        {
                            if (rangeZoneIds == null)
                                rangeZoneIds = string.Empty;
                            rangeZoneIds += sepa + str;
                            sepa = ";";
                        }
                    }
                }

                if (!string.IsNullOrEmpty(rangeZoneIds))
                    rangeZoneIds = rangeZoneIds.Replace(";;", ";").Replace(";;", ";");
            }

            var priceMin = (decimal?)null;
            var priceMax = (decimal?)null;
            ProductHelper.GetPriceFromQueryString(out priceMin, out priceMax);

            //if (postParams.Get("template") != null)
            //    xsltFileName = postParams.Get("template").ToString();
            //if (postParams.Get(ProductHelper.QueryStringPageSizeParam) != null)
            //{
            //    var tmp = 0;
            //    int.TryParse(postParams.Get(ProductHelper.QueryStringPageSizeParam), out tmp);
            //    if (tmp > 0 && tmp < pageSize)
            //        pageSize = tmp;
            //}

            var sort = 0;
            if (postParams.Get(ProductHelper.QueryStringSortModeParam) != null)
                int.TryParse(postParams.Get(ProductHelper.QueryStringSortModeParam), out sort);

            var pageNumber = 1;
            if (postParams.Get("page") != null)
                int.TryParse(postParams.Get("page"), out pageNumber);

            var ps = pageSize;
            if (postParams.Get("pagesize") != null)
                int.TryParse(postParams.Get("pagesize"), out ps);
            if (ps <= 50 && ps > 0)
                pageSize = ps;

            if (postParams.Get("template") != null)
            {
                var template = postParams.Get("template").ToString() + ".xslt";
                if (File.Exists(Server.MapPath(SiteUtils.GetXsltBasePath("product", template))))
                    xsltFileName = template;
            }

            //Keyword
            var keyword = (string)null;
            if (postParams.Get("keyword") != null)
                keyword = postParams.Get("keyword")
                             .Replace("\"", string.Empty)
                             .Replace("+", string.Empty)
                             .Replace("&&", string.Empty)
                             .Replace("||", string.Empty)
                             .Replace("!", string.Empty)
                             .Replace("(", string.Empty)
                             .Replace(")", string.Empty)
                             .Replace("{", string.Empty)
                             .Replace("}", string.Empty)
                             .Replace("[", string.Empty)
                             .Replace("]", string.Empty)
                             .Replace("^", string.Empty)
                             .Replace("\"", string.Empty)
                             .Replace("~", string.Empty)
                             .Replace("*", string.Empty)
                             .Replace("?", string.Empty)
                             .Replace(":", string.Empty)
                             .Replace("\\", string.Empty)
                             ;

            //Custom field
            var loadFilter = File.Exists(Server.MapPath(SiteUtils.GetXsltBasePath("product", "AjaxProductFilter.xslt")));
            var node = (gbSiteMapNode)null;
            var lstCustomFields = new List<CustomField>();
            if (zoneId > 0)
            {
                node = SiteUtils.GetSiteMapNodeByZoneId(zoneId);
                if (node != null && loadFilter)
                    lstCustomFields = CustomFieldHelper.GetCustomFieldsFromContext(siteSettings.SiteId, Product.FeatureGuid, node.ZoneGuid, languageId);
            }

            var propertyCondition = (string)null;
            var andClause = " ";
            var numberGroup = 0;
            foreach (CustomField field in lstCustomFields)
            {
                if (
                    field.DataType == (int)CustomFieldDataType.CheckBox
                    || field.DataType == (int)CustomFieldDataType.SelectBox
                )
                {
                    if (field.FilterType == (int)CustomFieldFilterType.ByValue)
                    {
                        var paramName = ProductHelper.QueryStringFilterSingleParam + field.CustomFieldId.ToString();
                        var optionValue = WebUtils.ParseInt32FromQueryString(paramName, -1);

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
                        var paramName = ProductHelper.QueryStringFilterMultiParam + field.CustomFieldId.ToString();
                        var optionValues = WebUtils.ParseStringFromQueryString(paramName, string.Empty);

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

            var discountId = -1;
            if (postParams.Get("promoid") != null)
                int.TryParse(postParams.Get("promoid"), out discountId);
            if (discountId == 0)
            {
                if (!string.IsNullOrEmpty(propertyCondition))
                    propertyCondition += " AND ";
                propertyCondition = "EXISTS (SELECT 1 FROM gb_DiscountAppliedToItems di INNER JOIN [dbo].[gb_Discount] d ON di.DiscountID=d.DiscountID WHERE d.DiscountType=" + ((int)DiscountType.ByCatalog).ToString() + " AND d.IsActive = 1 AND (getdate() > ISNULL(d.StartDate, '1/1/1900')) AND (getdate() < ISNULL(d.EndDate, '1/1/2999')) AND d.SiteID = " + siteSettings.SiteId.ToString() + " AND ((di.AppliedType=0 AND di.ItemID=p.ZoneID) OR (di.AppliedType=1 AND (di.ItemID=-1 OR di.ItemID=p.ProductID))) )";
            }
            else if (discountId > 0)
            {
                if (!string.IsNullOrEmpty(propertyCondition))
                    propertyCondition += " AND ";
                propertyCondition = "EXISTS (SELECT 1 FROM gb_DiscountAppliedToItems di INNER JOIN [dbo].[gb_Discount] d ON di.DiscountID=d.DiscountID  WHERE di.DiscountID=" + discountId.ToString() + " AND ((di.AppliedType=0 AND di.ItemID=p.ZoneID) OR (di.AppliedType=1 AND (di.ItemID=-1 OR di.ItemID=p.ProductID))) )";
            }

            if (sort == 30)
            {
                if (propertyCondition.Length > 0)
                    propertyCondition += " AND";

                propertyCondition += " EXISTS (SELECT 1 FROM gb_DiscountAppliedToItems di INNER JOIN [dbo].[gb_Discount] d ON di.DiscountID=d.DiscountID WHERE d.DiscountType=" + ((int)DiscountType.ByCatalog).ToString() + " AND d.IsActive = 1 AND (getdate() > ISNULL(d.StartDate, '1/1/1900')) AND (getdate() < ISNULL(d.EndDate, '1/1/2999')) AND d.SiteID = " + siteSettings.SiteId.ToString() + " AND ((di.AppliedType=0 AND di.ItemID=p.ZoneID) OR (di.AppliedType=1 AND (di.ItemID=-1 OR di.ItemID=p.ProductID))) )";
            }

            var iDiscountId = -1;
            if (ConfigHelper.GetBoolProperty("PromotionSortByItem", false) && discountId > 0)
                iDiscountId = discountId;

            var iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: rangeZoneIds, publishStatus: 1, languageId: languageId, priceMin: priceMin, priceMax: priceMax, position: position, propertyCondition: propertyCondition, searchCode: false, keyword: keyword, searchProductZone: ProductConfiguration.EnableProductZone);
            var lstProducts = Product.GetPageAdv(pageNumber: pageNumber, pageSize: pageSize, siteId: siteSettings.SiteId, zoneIds: rangeZoneIds, publishStatus: 1, languageId: languageId, priceMin: priceMin, priceMax: priceMax, position: position, propertyCondition: propertyCondition, searchCode: false, keyword: keyword, discountId: iDiscountId, orderBy: sort, searchProductZone: ProductConfiguration.EnableProductZone);

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<ProductList></ProductList>");
            var root = doc.DocumentElement;

            var hasNextPage = iCount > (pageSize * pageNumber);
            if (hasNextPage)
            {
                //lstProducts.RemoveAt(lstProducts.Count - 1);
                XmlHelper.AddNode(doc, root, "NextPageNumber", (pageNumber + 1).ToString());
            }

            var timeOffset = SiteUtils.GetUserTimeOffset();
            var timeZone = SiteUtils.GetUserTimeZone();
            var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -10, lstProducts);
            foreach (var product in lstProducts)
            {
                XmlElement productXml = doc.CreateElement("Product");
                root.AppendChild(productXml);

                ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset);
            }

            var filter = (string)null;
            if (
                loadFilter
                && node != null
                && numberGroup <= 0
                )
            {
                var docFilter = new XmlDocument();
                doc.XmlResolver = null;

                docFilter.LoadXml("<ProductFilter></ProductFilter>");
                var rootFilter = docFilter.DocumentElement;

                foreach (CustomField field in lstCustomFields)
                {
                    if (
                        (field.DataType == (int)CustomFieldDataType.CheckBox
                         || field.DataType == (int)CustomFieldDataType.SelectBox)
                        && (field.FilterType == (int)CustomFieldFilterType.ByValue
                            || field.FilterType == (int)CustomFieldFilterType.ByMultipleValues)
                    )
                    {
                        var productXml = docFilter.CreateElement("Group");
                        rootFilter.AppendChild(productXml);

                        XmlHelper.AddNode(docFilter, productXml, "GroupId", field.CustomFieldId.ToString());
                        XmlHelper.AddNode(docFilter, productXml, "Title", field.Name);
                        XmlHelper.AddNode(docFilter, productXml, "FieldType", field.FieldType.ToString());
                        XmlHelper.AddNode(docFilter, productXml, "FilterType", field.FilterType.ToString());

                        string paramName = string.Empty;
                        if (field.FilterType == (int)CustomFieldFilterType.ByValue)
                            paramName = ProductHelper.QueryStringFilterSingleParam + field.CustomFieldId.ToString();
                        else
                            paramName = ProductHelper.QueryStringFilterMultiParam + field.CustomFieldId.ToString();

                        var lstOptions = CustomFieldOption.GetByCustomField(field.CustomFieldId, languageId);
                        foreach (var option in lstOptions)
                        {
                            var optionXml = docFilter.CreateElement("Option");
                            productXml.AppendChild(optionXml);

                            XmlHelper.AddNode(docFilter, optionXml, "GroupId", option.CustomFieldId.ToString());
                            XmlHelper.AddNode(docFilter, optionXml, "OptionId", option.CustomFieldOptionId.ToString());
                            XmlHelper.AddNode(docFilter, optionXml, "Title", option.Name);
                            if (field.FieldType == (int)CustomFieldType.Color)
                                XmlHelper.AddNode(docFilter, optionXml, "Color", option.OptionColor);
                            XmlHelper.AddNode(docFilter, optionXml, "FieldType", field.FieldType.ToString());
                            XmlHelper.AddNode(docFilter, optionXml, "FilterType", field.FilterType.ToString());
                            //XmlHelper.AddNode(docFilter, optionXml, "IsActive", CustomFieldHelper.IsActive(paramName, option.CustomFieldOptionId, field.FilterType).ToString().ToLower());

                            XmlHelper.AddNode(docFilter, optionXml, "Url", paramName + "=" + option.CustomFieldOptionId.ToString());
                        }
                    }
                }

                filter = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", "AjaxProductFilter.xslt"), docFilter);
            }

            return StringHelper.ToJsonString(new
            {
                success = true,
                nextPage = hasNextPage ? (pageNumber + 1) : 0,
                data = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", xsltFileName), doc),
                filter
            });
        }

        private void LoadParams()
        {
            // don't accept get requests
            if (Request.HttpMethod != "POST") { return; }

            postParams = HttpUtility.ParseQueryString(Request.GetRequestBody());

            if (postParams.Get("method") != null)
            {
                method = postParams.Get("method");
            }
        }
    }
}
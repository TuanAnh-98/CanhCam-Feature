/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-10-24
/// Last Modified:			2015-10-24

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public static class ProductAttributeParser
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductAttributeParser));

        /// <summary>
        /// Parse manufacturer attributes on the manufacturer details page
        /// </summary>
        /// <param name="manufacturer">Product</param>
        /// <param name="form">Form</param>
        /// <returns>Parsed attributes</returns>
        public static string ParseProductAttributes(Product product, NameValueCollection postParams)
        {
            string attributesXml = "";

            #region Product attributes

            if (ProductConfiguration.EnableShoppingCartAttributes)
            {
                var productProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);
                if (productProperties.Count > 0)
                {
                    var customFieldIds = productProperties.Select(x => x.CustomFieldId).Distinct().ToList();
                    var productAttributes = CustomField.GetByOption(CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds), CustomFieldOptions.EnableShoppingCart);
                    foreach (var attribute in productAttributes)
                    {
                        string controlId = string.Format("product_attribute_{0}_{1}", product.ProductId, attribute.CustomFieldId);
                        var ctrlAttributes = postParams[controlId];
                        if (!String.IsNullOrEmpty(ctrlAttributes))
                        {
                            int selectedAttributeId = int.Parse(ctrlAttributes);
                            if (selectedAttributeId > 0)
                                attributesXml = CustomFieldHelper.AddProductAttribute(attributesXml, attribute, selectedAttributeId.ToString());
                        }
                    }
                }
            }

            #endregion Product attributes

            // Gift cards

            return attributesXml;
        }

        #region Parse

        /// <summary>
        /// Gets selected manufacturer attribute mappings
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Selected manufacturer attribute mappings</returns>
        public static IList<CustomField> ParseProductAttributeMappings(List<CustomField> allCustomFields, string attributesXml)
        {
            var result = new List<CustomField>();
            var ids = ParseProductAttributeMappingIds(attributesXml);
            foreach (int id in ids)
            {
                var attribute = allCustomFields.Where(x => x.CustomFieldId == id).FirstOrDefault();
                //var attribute = new CustomField(id);
                if (attribute != null && attribute.CustomFieldId > 0)
                    result.Add(attribute);
            }
            return result;
        }

        /// <summary>
        /// Gets selected manufacturer attribute mapping identifiers
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Selected manufacturer attribute mapping identifiers</returns>
        private static IList<int> ParseProductAttributeMappingIds(string attributesXml)
        {
            var ids = new List<int>();
            if (String.IsNullOrEmpty(attributesXml))
                return ids;

            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                xmlDoc.LoadXml(attributesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id))
                        {
                            ids.Add(id);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return ids;
        }

        /// <summary>
        /// Gets selected manufacturer attribute values
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="productAttributeMappingId">Product attribute mapping identifier</param>
        /// <returns>Product attribute values</returns>
        public static IList<int> ParseValues(string attributesXml, int productAttributeMappingId)
        {
            var selectedValues = new List<int>();
            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                xmlDoc.LoadXml(attributesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id))
                        {
                            if (id == productAttributeMappingId)
                            {
                                var nodeList2 = node1.SelectNodes(@"ProductAttributeValue/Value");
                                foreach (XmlNode node2 in nodeList2)
                                {
                                    int value = Convert.ToInt32(node2.InnerText.Trim());
                                    selectedValues.Add(value);
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
            return selectedValues;
        }

        #endregion Parse

        public static string ChangeAttributes(Product product, NameValueCollection form)
        {
            decimal price = ProductHelper.GetPrice(product);
            var productProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);
            if (productProperties.Count > 0)
            {
                var customFieldIds = productProperties.Select(x => x.CustomFieldId).Distinct().ToList();
                var productAttributes = CustomField.GetByOption(CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds), CustomFieldOptions.EnableShoppingCart);
                foreach (var attribute in productAttributes)
                {
                    string controlId = string.Format("product_attribute_{0}_{1}", product.ProductId, attribute.CustomFieldId);
                    var ctrlAttributes = form[controlId];
                    if (!String.IsNullOrEmpty(ctrlAttributes))
                    {
                        int selectedAttributeId = int.Parse(ctrlAttributes);
                        productProperties.ForEach(x =>
                        {
                            if (x.CustomFieldOptionId == selectedAttributeId)
                                price += x.OverriddenPrice;
                        });
                    }
                }
            }

            return Json(new
            {
                success = true,
                price = ProductHelper.FormatPrice(price, true)
            });
        }

        public static string Json(this object result)
        {
            return StringHelper.ToJsonString(result);
        }
    }
}
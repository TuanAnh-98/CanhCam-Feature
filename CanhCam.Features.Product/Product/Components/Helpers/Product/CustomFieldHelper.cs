/// Created:			    2013-01-01
/// Last Modified:		    2014-06-19

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;

namespace CanhCam.Web
{
    public static class CustomFieldHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomFieldHelper));

        public static List<CustomField> GetCustomFieldsFromContext(int siteId, Guid featureGuid, Guid zoneGuid,
                int languageId = -1)
        {
            if (HttpContext.Current == null) return CustomField.GetActiveByZone(siteId, featureGuid, zoneGuid, languageId);

            string contextKey = "CustomFields_" + siteId.ToInvariantString() + "_" + featureGuid.ToString() + "_" + zoneGuid.ToString() +
                                "_" + languageId.ToString();

            List<CustomField> lstCustomFields = HttpContext.Current.Items[contextKey] as List<CustomField>;
            if (lstCustomFields == null)
            {
                lstCustomFields = CustomField.GetActiveByZone(siteId, featureGuid, zoneGuid, languageId);
                HttpContext.Current.Items[contextKey] = lstCustomFields;
            }

            return lstCustomFields;
        }

        /// <summary>
        /// Adds an attribute
        /// </summary>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="productAttributeMapping">Product attribute mapping</param>
        /// <param name="value">Value</param>
        /// <returns>Attributes</returns>
        public static string AddProductAttribute(string attributesXml, CustomField productAttributeMapping, string value)
        {
            string result = string.Empty;
            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                if (String.IsNullOrEmpty(attributesXml))
                {
                    var element1 = xmlDoc.CreateElement("Attributes");
                    xmlDoc.AppendChild(element1);
                }
                else
                    xmlDoc.LoadXml(attributesXml);
                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Attributes");

                XmlElement attributeElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//Attributes/ProductAttribute");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id))
                        {
                            if (id == productAttributeMapping.CustomFieldId)
                            {
                                attributeElement = (XmlElement)node1;
                                break;
                            }
                        }
                    }
                }

                //create new one if not found
                if (attributeElement == null)
                {
                    attributeElement = xmlDoc.CreateElement("ProductAttribute");
                    attributeElement.SetAttribute("ID", productAttributeMapping.CustomFieldId.ToString());
                    rootElement.AppendChild(attributeElement);
                }

                var attributeValueElement = xmlDoc.CreateElement("ProductAttributeValue");
                attributeElement.AppendChild(attributeValueElement);

                var attributeValueValueElement = xmlDoc.CreateElement("Value");
                attributeValueValueElement.InnerText = value;
                attributeValueElement.AppendChild(attributeValueValueElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return result;
        }

        public static string BuildFilterUrl(string rawUrl, string paramName, int optionId, int filterType, out string queryString)
        {
            queryString = string.Empty;
            string paramValue = string.Empty;
            if (filterType == (int)CustomFieldFilterType.ByValue)
                paramValue = optionId.ToString();
            else
            {
                paramValue = WebUtils.ParseStringFromQueryString(paramName, string.Empty);

                if (paramValue.Contains("/" + optionId.ToString() + "/"))
                    paramValue = paramValue.Replace(string.Format("/{0}/", optionId.ToString()), "/");
                else
                {
                    if (!paramValue.EndsWith("/"))
                        paramValue += "/";
                    paramValue = paramValue + string.Format("{0}/", optionId.ToString());
                }
            }

            string pageUrl = SiteUtils.BuildUrlLeaveOutParam(rawUrl, paramName, false);
            if (paramName.Length > 0 && paramValue.Length > 0 && paramValue != "/")
            {
                if (pageUrl.Contains("?"))
                    pageUrl += string.Format("&{0}={1}", paramName, paramValue);
                else
                    pageUrl += string.Format("?{0}={1}", paramName, paramValue);

                queryString = string.Format("{0}={1}", paramName, paramValue);
            }

            return pageUrl;
        }

        public static bool IsActive(string paramName, string value, int filterType)
        {
            if (filterType == (int)CustomFieldFilterType.ByValue)
            {
                var optionValue = WebUtils.ParseStringFromQueryString(paramName, string.Empty);
                if (optionValue == value)
                    return true;
            }

            string optionValues = WebUtils.ParseStringFromQueryString(paramName, string.Empty);
            if (optionValues.Contains("/" + value + "/"))
                return true;

            return false;
        }
    }
}
/// Author:                 Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-08-30
/// Last Modified:		    2014-09-01

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SuggestSearch : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            var query = WebUtils.ParseStringFromQueryString("q", string.Empty);
            if (query.Length == 0)
                return;
            query = query.Replace("\"", string.Empty)
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

            var queryToSearch = query;
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var timeZone = SiteUtils.GetUserTimeZone();
            var timeOffset = SiteUtils.GetUserTimeOffset();

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<SearchResults></SearchResults>");
            var root = doc.DocumentElement;

            var queryAscii = ToAsciiIfPossible(queryToSearch);
            var startHtml = "<strong class=\"highlight\">";
            var endHtml = "</strong>";

            var pageSize = 20;
            var lstProducts = Product.GetTopAdv(top: pageSize, siteId: siteSettings.SiteId, publishStatus: 1,
                languageId: WorkingCulture.LanguageId, keyword: queryToSearch);
            if (lstProducts.Count > 0)
            {
                var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProducts);
                foreach (var product in lstProducts)
                {
                    var productXml = doc.CreateElement("Product");
                    root.AppendChild(productXml);

                    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset);

                    var titleAscii = ToAsciiIfPossible(product.Title);
                    var lstIndex = new List<int>();
                    var startindex = 0;
                    while (startindex < titleAscii.Length)
                    {
                        int wordstartIndex = titleAscii.IndexOf(queryAscii, startindex, StringComparison.CurrentCultureIgnoreCase);
                        if (wordstartIndex != -1)
                            lstIndex.Add(wordstartIndex);
                        else
                            break;

                        startindex += wordstartIndex + queryAscii.Length;
                    }
                    var title = product.Title;
                    for (var i = lstIndex.Count - 1; i >= 0; i--)
                    {
                        if (title.Length > lstIndex[i] + queryAscii.Length - 1 && title.Length > lstIndex[i] - 1)
                        {
                            title = title.Insert(lstIndex[i] + queryAscii.Length, endHtml);
                            title = title.Insert(lstIndex[i], startHtml);
                        }
                    }

                    //var titleAsciit8 = ToAsciiIfPossible(product.Title);
                    //var regex = @"\b(" + String.Join("|", queryAscii.SplitOnCharAndTrim(' ')) + @")\b";
                    //var titleResult = System.Text.RegularExpressions.Regex.Replace(titleAscii, regex, @"<match>$1</match>");
                    //var lstIndex = AllIndexesOf(titleResult, "<match>").ToList();
                    //var title = product.Title;
                    //for (var i = lstIndex.Count - 1; i >= 0; i--)
                    //{
                    //    if (title.Length > lstIndex[i] + queryAscii.Length - 1 && title.Length > lstIndex[i] - 1)
                    //    {
                    //        title = title.Insert(lstIndex[i] + queryAscii.Length, endHtml);
                    //        title = title.Insert(lstIndex[i], startHtml);
                    //    }
                    //}

                    XmlHelper.AddNode(doc, productXml, "TitleHighlight", title);
                }
            }

            XmlHelper.AddNode(doc, root, "Keyword", queryToSearch);
            XmlHelper.AddNode(doc, root, "SearchResultsPage", "/product/searchresults.aspx?q=" + HttpUtility.UrlEncode(queryToSearch));

            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", "ProductSuggestSearch.xslt"), doc);

            if (result.Length > 0)
                context.Response.Write(result);
            else
                context.Response.Write(" ");

            context.Response.End();
        }

        public static IEnumerable<int> AllIndexesOf(string str, string searchstring)
        {
            int minIndex = str.IndexOf(searchstring);

            while (minIndex != -1)
            {
                yield return minIndex;
                minIndex = str.IndexOf(searchstring, minIndex + searchstring.Length);
            }
        }

        private static string ToAsciiIfPossible(string s)
        {
            if (string.IsNullOrEmpty(s)) { return s; }

            int len = s.Length;
            StringBuilder sb = new StringBuilder(len);
            char c;

            for (int i = 0; i < len; i++)
            {
                c = s[i];

                if ((int)c >= 128)
                    sb.Append(RemapInternationalCharToAscii(c));
                else
                    sb.Append(c);
            }

            return sb.ToString();
        }

        private static string RemapInternationalCharToAscii(char c)
        {
            string s = c.ToString().ToLowerInvariant();
            if ("áàạảãâấầậẩẫăắằặẳ".Contains(s))
                return "a";
            else if ("éèẹẻẽêếềệểễ".Contains(s))
                return "e";
            else if ("íìịỉĩ".Contains(s))
                return "i";
            else if ("óòọỏõôốồộổỗơớờợở".Contains(s))
                return "o";
            else if ("úùụủũưứừựửữ".Contains(s))
                return "u";
            else if ("ýỳỵỷỹ".Contains(s))
                return "y";
            else if (s == "đ")
                return "d";

            return s;
        }

        //private int CaculateTotalPages(int pageSize, int totalRows)
        //{
        //    int totalPages = 1;

        //    if (pageSize > 0) totalPages = totalRows / pageSize;

        //    if (totalRows <= pageSize)
        //        totalPages = 1;
        //    else if (pageSize > 0)
        //    {
        //        int remainder;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //            totalPages += 1;
        //    }

        //    return totalPages;
        //}

        //private List<string> GetUserRoles(HttpContext context, int siteId)
        //{
        //    List<string> userRoles = new List<string>();

        //    userRoles.Add("All Users");
        //    if (context.User.Identity.IsAuthenticated)
        //    {
        //        SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
        //        if (currentUser != null)
        //        {
        //            using (IDataReader reader = SiteUser.GetRolesByUser(siteId, currentUser.UserId))
        //            {
        //                while (reader.Read())
        //                {
        //                    userRoles.Add(reader["RoleName"].ToString());
        //                }

        //            }

        //        }
        //    }

        //    return userRoles;
        //}

        public bool IsReusable => false;
    }
}
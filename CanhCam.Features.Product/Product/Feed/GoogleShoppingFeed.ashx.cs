/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2013-02-21
/// Last Modified:			2013-02-21

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Xml;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// Purpose: Renders a SiteMap as xml
    /// in google site map protocol format
    /// https://www.google.com/webmasters/tools/docs/en/protocol.html
    /// for news posts that have friendly urls
    ///
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class GoogleShoppingFeed : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            GenerateFeed(context);
        }

        private void GenerateFeed(HttpContext context)
        {
            context.Response.Cache.SetExpires(DateTime.Now.AddMinutes(20));
            context.Response.Cache.SetCacheability(HttpCacheability.Public);

            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            using (XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding))
            {
                xmlTextWriter.Formatting = Formatting.Indented;

                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("rss"); //start rss

                xmlTextWriter.WriteStartAttribute("version");
                xmlTextWriter.WriteValue("2.0");
                xmlTextWriter.WriteEndAttribute();

                xmlTextWriter.WriteStartAttribute("xmlns:g");
                xmlTextWriter.WriteValue("http://base.google.com/ns/1.0");
                xmlTextWriter.WriteEndAttribute();

                xmlTextWriter.WriteStartElement("channel"); //start channel

                var siteSettings = CacheHelper.GetCurrentSiteSettings();
                xmlTextWriter.WriteElementString("title", siteSettings.SiteName);
                xmlTextWriter.WriteElementString("description", siteSettings.MetaTitle);
                xmlTextWriter.WriteElementString("link", WebUtils.GetSiteRoot());

                BuildProductItem(context, xmlTextWriter, siteSettings.SiteId);

                xmlTextWriter.WriteEndElement(); //channel

                xmlTextWriter.WriteEndElement(); //rss

                //end of document
                xmlTextWriter.WriteEndDocument();
            }
        }

        private int GetGoogleCategoryId()
        {
            return ConfigHelper.GetIntProperty("GoogleFeedDefaultCategoryId", 222);
        }

        private string GetManufacter(List<Manufacturer> lstManufacturer, int productManufacterId)
        {
            var manu = lstManufacturer.Where(f => f.ManufacturerId == productManufacterId).FirstOrDefault();
            if (manu != null && manu.ManufacturerId > 0)
                return manu.Name;
            return ConfigHelper.GetStringProperty("GoogleFeedDefaultBrand", "Cello");
        }

        private void BuildProductItem(HttpContext context, XmlTextWriter xmlTextWriter, int siteId)
        {
            var zoneId = WebUtils.ParseInt32FromQueryString("zoneid", -1);
            var zoneIds = (string)null;
            if (zoneId > 0)
                zoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteId, zoneId);
            var lstManufacter = Manufacturer.GetAll(siteId, ManufacturerPublishStatus.Published, null, -1);

            var lstProducts = Product.GetPageAdv(siteId: siteId, zoneIds: zoneIds, publishStatus: 1, languageId: WorkingCulture.LanguageId);
            var siteRoot = WebUtils.GetSiteRoot();
            var lstDiscountItems = DiscountAppliedToItem.GetActive(siteId, -10, lstProducts);
            foreach (var product in lstProducts)
            {
                if (product.Url.StartsWith("~/"))
                {
                    xmlTextWriter.WriteStartElement("item");
                    xmlTextWriter.WriteElementString("g:id", product.ProductId.ToString());
                    xmlTextWriter.WriteElementString("title", Regex.Replace(product.Title, @"[\x00-\x08]|[\x0B\x0C]|[\x0E-\x19]|[\uD800-\uDFFF]|[\uFFFE\uFFFF]|[\u0000-\u001F]", " "));
                    var editor = new RadEditor
                    {
                        Content = string.IsNullOrEmpty(product.MetaDescription) ? product.FullContent : product.MetaDescription
                    };
                    xmlTextWriter.WriteElementString("description", Regex.Replace(UIHelper.CreateExcerpt(editor.Text.Trim(), 255, "..."), @"[\x00-\x08]|[\x0B\x0C]|[\x0E-\x19]|[\uD800-\uDFFF]|[\uFFFE\uFFFF]|[\u0000-\u001F]", " "));
                    xmlTextWriter.WriteElementString("link", SiteUtils.GetNavigationSiteRoot(product.ZoneId) + product.Url.Replace("~", string.Empty));

                    var discountApplied = (DiscountAppliedToItem)null;
                    var productPrice = ProductHelper.GetPrice(product, -1, ref discountApplied, ref lstDiscountItems);
                    xmlTextWriter.WriteElementString("g:price", Convert.ToDouble(productPrice).ToString() + " VND");
                    string availability = product.StockQuantity > 0 ? "in stock" : "out of stock";
                    if (product.SpecialPriceEndDate != null && product.SpecialPriceEndDate > DateTime.Now)
                    {
                        availability = "preorder";
                    }
                    xmlTextWriter.WriteElementString("g:availability", availability);
                    xmlTextWriter.WriteElementString("g:brand", GetManufacter(lstManufacter, product.ManufacturerId));
                    xmlTextWriter.WriteElementString("g:condition", "new");
                    xmlTextWriter.WriteElementString("g:google_product_category", GetGoogleCategoryId().ToString());
                    if (!string.IsNullOrEmpty(product.ImageFile))
                    {
                        var imageFolderPath = ProductHelper.MediaFolderPath(siteId, product.ProductId);
                        xmlTextWriter.WriteElementString("g:image_link", siteRoot + ProductHelper.GetMediaFilePath(imageFolderPath, product.ImageFile));
                    }
                    xmlTextWriter.WriteEndElement(); //item
                }
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
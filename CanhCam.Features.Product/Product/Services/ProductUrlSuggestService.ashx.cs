/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23

using CanhCam.Business.WebHelpers;
using Resources;
using System;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// A service to suggest a friendly url for a news
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class ProductUrlSuggestService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            SendResponse(context);
        }

        private void SendResponse(HttpContext context)
        {
            if (context == null) return;

            context.Response.Expires = -1;
            context.Response.ContentType = "application/xml";
            Encoding encoding = new UTF8Encoding();

            XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, encoding)
            {
                Formatting = Formatting.Indented
            };

            xmlTextWriter.WriteStartDocument();
            xmlTextWriter.WriteStartElement("DATA");
            string warning = string.Empty;

            if (context.Request.Params.Get("pn") != null)
            {
                var pageName = context.Request.Params.Get("pn");
                var siteSettings = CacheHelper.GetCurrentSiteSettings();

                if (siteSettings != null)
                {
                    var friendlyUrl = SiteUtils.SuggestFriendlyUrl(pageName, siteSettings);
                    if (WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrl))
                        warning = ProductResources.ProductUrlConflictWithPhysicalPageError;

                    xmlTextWriter.WriteStartElement("fn");
                    xmlTextWriter.WriteString("~/" + friendlyUrl);
                    xmlTextWriter.WriteEndElement();
                    xmlTextWriter.WriteStartElement("wn");
                    xmlTextWriter.WriteString(warning);
                    xmlTextWriter.WriteEndElement();
                }
            }
            else
            {
                if (context.Request.Params.Get("cu") != null)
                {
                    String enteredUrl = context.Server.UrlDecode(context.Request.Params.Get("cu"));
                    if (WebPageInfo.IsPhysicalWebPage(enteredUrl))
                        warning = ProductResources.ProductUrlConflictWithPhysicalPageError;

                    xmlTextWriter.WriteStartElement("wn");
                    xmlTextWriter.WriteString(warning);
                    xmlTextWriter.WriteEndElement();
                }
            }

            //end of document
            xmlTextWriter.WriteEndElement();
            xmlTextWriter.WriteEndDocument();

            xmlTextWriter.Close();
            //Response.End();
        }

        public bool IsReusable => false;
    }
}
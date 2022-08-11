using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public partial class MoMoRedirect : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MoMoRedirect));

        private string MoMopartnerCode = MomoHelper.partnerCode;
        private string MoMoaccessKey = MomoHelper.accessKey;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (Request.HttpMethod == "GET")
            {
                // log.Info("ReqQuest : " + Request.RawUrl);
                string partnerCode = WebUtils.ParseStringFromQueryString("partnerCode", string.Empty);
                string accessKey = WebUtils.ParseStringFromQueryString("accessKey", string.Empty);
                int orderId = WebUtils.ParseInt32FromQueryString("orderInfo", -1);
                string transId = WebUtils.ParseStringFromQueryString("transId", string.Empty);
                int errorCode = WebUtils.ParseInt32FromQueryString("errorCode", -1);

                int lang = WorkingCulture.LanguageId;

                var order = new Order(orderId);
                if (order == null || order.OrderId == -1)
                {
                    log.Info("MoMoRedirect :Order Not Found ");
                    var pageUrl = SiteUtils.GetZoneUrl(MomoHelper.FailedZoneId);
                    if (!string.IsNullOrEmpty(pageUrl))
                        WebUtils.SetupRedirect(this, pageUrl);
                    else
                        SiteUtils.RedirectToHomepage();
                    return;
                }
                try
                {
                    if (partnerCode != MoMopartnerCode || accessKey != MoMoaccessKey)
                    {
                        SiteUtils.RedirectToHomepage();
                        return;
                    }
                    if (errorCode == 0)
                    {
                        order.PaymentStatus = (int)OrderPaymentStatus.Successful;
                        Redirect(order, lang);
                        return;
                    }
                }
                catch (Exception)
                {
                }
                Redirect(null, lang);
            }
        }

        private void Redirect(Order order, int langId)
        {
            if (order == null || order.PaymentStatus != (int)OrderPaymentStatus.Successful)
            {
                var pageUrl = GetZoneUrl(MomoHelper.FailedZoneId, langId);
                if (!string.IsNullOrEmpty(pageUrl))
                    WebUtils.SetupRedirect(this, pageUrl);
                else
                    SiteUtils.RedirectToHomepage();
            }
            else
            {
                var pageUrl = GetZoneUrl(MomoHelper.CompletedZoneId, langId);
                if (!string.IsNullOrEmpty(pageUrl))
                    WebUtils.SetupRedirect(this, pageUrl);
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private string GetZoneUrl(int zoneId, int languageId)
        {
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            gbSiteMapNode gbNode = SiteUtils.GetSiteMapNodeByZoneId(siteSettings.SiteId, zoneId);

            return FormatUrl(siteSettings, gbNode, languageId);
        }

        private string FormatUrl(SiteSettings siteSettings, gbSiteMapNode mapNode, int languageId)
        {
            string url = string.Empty;
            if (WebConfigSettings.EnableHierarchicalFriendlyUrls)
            {
                url = mapNode.UrlExpand;
                if (languageId > 0 && mapNode["UrlExpand" + languageId.ToString()] != null)
                    url = mapNode["UrlExpand" + languageId.ToString()];
            }
            else
            {
                url = mapNode.Url;
                if (languageId > 0 && mapNode["Url" + languageId.ToString()] != null)
                    url = mapNode["Url" + languageId.ToString()];
            }
            string itemUrl = VirtualPathUtility.ToAbsolute(url);
            bool useFullUrls = false;
            string secureSiteRoot = WebUtils.GetSecureSiteRoot();
            string insecureSiteRoot = secureSiteRoot.Replace("https", "http");

            if (WebConfigSettings.ResolveFullUrlsForMenuItemProtocolDifferences)
            {
                if (SiteUtils.IsSecureRequest())
                {
                    if (
                        (!mapNode.UseSsl)
                        && (!siteSettings.UseSslOnAllPages)
                        && (url.StartsWith("~/") || url.StartsWith("/"))
                        )
                    {
                        itemUrl = insecureSiteRoot + url.Replace("~/", "/");
                        useFullUrls = true;
                    }
                }
                else
                {
                    if ((mapNode.UseSsl) || (siteSettings.UseSslOnAllPages))
                    {
                        if (url.StartsWith("~/") || url.StartsWith("/"))
                        {
                            itemUrl = secureSiteRoot + url.Replace("~/", "/");
                            useFullUrls = true;
                        }
                    }
                }
            }

            if (
                !useFullUrls
                && WebConfigSettings.UseFullUrlsForWebPage
                && (url.StartsWith("~/") || url.StartsWith("/"))
                )
                itemUrl = WebUtils.GetSiteRoot() + url.Replace("~/", "/");

            return itemUrl;
        }
    }
}
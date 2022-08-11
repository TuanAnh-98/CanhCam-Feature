/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-28
/// Last Modified:			2014-08-28

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public class ShoppingCartLink : WebControl
    {
        private string overrideText = string.Empty;

        public string OverrideText
        {
            get { return overrideText; }
            set { overrideText = value; }
        }

        private string overrideLink = string.Empty;

        public string OverrideLink
        {
            get { return overrideLink; }
            set { overrideLink = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            string urlToUse = Page.ResolveUrl("~/Product/Cart.aspx");

            if (overrideLink.Length > 0)
                urlToUse = Page.ResolveUrl(overrideLink);

            if (SiteUtils.SslIsAvailable())
                urlToUse = SiteUtils.GetSecureNavigationSiteRoot() + urlToUse;
            else
                urlToUse = SiteUtils.GetNavigationSiteRoot() + urlToUse;

            writer.WriteBeginTag("a");
            writer.WriteAttribute("class", CssClass);
            writer.WriteAttribute("href", Page.ResolveUrl(urlToUse));
            writer.Write(HtmlTextWriter.TagRightChar);

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            int cartCount = CartHelper.GetShoppingCart(siteSettings.SiteId, ShoppingCartTypeEnum.ShoppingCart).Count;

            if (overrideText.Length > 0)
                writer.Write(string.Format(overrideText, cartCount));
            else
                writer.Write(cartCount.ToString());

            writer.WriteEndTag("a");
        }
    }
}
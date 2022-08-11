/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2017-03-16
/// Last Modified:			2017-03-16

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Web;
using System.Web.UI;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class WishlistPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WishlistPage));
        private SiteUser siteUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();
            PopulateLabels();

            if (siteUser == null || siteUser.UserGuid == Guid.Empty)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, litTitle.Text);
        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("wishlist-page");
        }

        private void PopulateControls()
        {
            string xsltPath = SiteUtils.GetXsltBasePath("product", "Wishlist.xslt");
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(xsltPath)))
                return;

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            var root = CartHelper.BuildShoppingCartXml(SiteId, Guid.Empty, out doc, Business.ShoppingCartTypeEnum.Wishlist);

            XmlHelper.XMLTransform(xmlTransformer, xsltPath, doc);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }
    }
}
using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using System;
using System.Web.UI;
using System.Xml;

namespace CanhCam.Web.AccountUI
{
    public partial class OrderViewPage : CmsBasePage
    {
        private int pageNumber = 1;
        private string ordercode = string.Empty;
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
            Title = SiteUtils.FormatPageTitle(siteSettings, "Chi tiết đơn hàng");
        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("page", pageNumber);

            siteUser = SiteUtils.GetCurrentSiteUser();
            ordercode = WebUtils.ParseStringFromQueryString("ordercode", string.Empty);
        }

        private void PopulateControls()
        {
            //var doc = BuildOrderDetailXml(ordercode);

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<OrderDetail></OrderDetail>");
            var root = doc.DocumentElement;
            var objOrder = Order.GetByCode(ordercode);
            if (objOrder != null && objOrder.OrderId > 0)
            {
                var timeOffset = SiteUtils.GetUserTimeOffset();
                var timeZone = SiteUtils.GetUserTimeZone();
                ProductHelper.BuildOrderXml(doc, root, objOrder, timeZone, timeOffset);
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("Product", "OrderView.xslt"), doc);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }
    }
}
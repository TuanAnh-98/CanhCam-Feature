/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-09-18
/// Last Modified:			2014-09-18

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class PurchaseHistoryPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PurchaseHistoryPage));
        private int pageNumber = 1;
        private int pageSize = 20;
        private SiteUser siteUser;
        //var lstOptions = new List<CustomFieldOption>();

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
            pageNumber = WebUtils.ParseInt32FromQueryString("page", pageNumber);
            siteUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("purchase-history-page");
        }

        private void PopulateControls()
        {
            var doc = ProductHelper.BuildPurchaseHistoryXml(SiteId, siteUser.UserGuid, pageNumber, pageSize);
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("Product", "PurchaseHistory.xslt"), doc);

            var startDate = (DateTime?)null;
            var itemCount = OrderItem.GetCountBySearch(siteSettings.SiteId, -1, -1, -1, -1, startDate, null, null, null, siteUser.UserGuid, null);

            string pageUrl = SiteRoot + "/Product/PurchaseHistory.aspx";
            if (pageUrl.Contains("?"))
                pageUrl += "&amp;page={0}";
            else
                pageUrl += "?page={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.PageSize = pageSize;
            pgr.ItemCount = itemCount;
            pgr.CurrentIndex = pageNumber;
            divPager.Visible = (itemCount > pageSize);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }
    }
}
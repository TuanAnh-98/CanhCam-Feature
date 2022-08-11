using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using System;
using System.Web.UI;
using System.Xml;

namespace CanhCam.Web.AccountUI
{
    public partial class RewardHistoryPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(RewardHistoryPage));
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


            XmlDocument doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<RewardPoints></RewardPoints>");
            XmlElement root = doc.DocumentElement;
            RewardPointsHelper.GetRewardPointsBalance(siteUser, out decimal rewardPointsBalance, out decimal pointsAvallable);
            XmlHelper.AddNode(doc, root, "RewardPointsBalance", rewardPointsBalance.ToString());
            var totalRows = RewardPointsHistory.GetCountBySearch(siteSettings.SiteId, siteUser.UserId);
            var items = RewardPointsHistory.GetPageBySearch(siteSettings.SiteId, siteUser.UserId, pageNumber: pageNumber, pageSize: pageSize);

            var timeOffset = SiteUtils.GetUserTimeOffset();
            var timeZone = SiteUtils.GetUserTimeZone();
            foreach (RewardPointsHistory item in items)
            {
                var productXml = doc.CreateElement("RewardPoint");
                root.AppendChild(productXml);
                XmlHelper.AddNode(doc, productXml, "RowId", item.RowId.ToString());
                XmlHelper.AddNode(doc, productXml, "PointsBalance", item.PointsBalance.ToString());
                XmlHelper.AddNode(doc, productXml, "Type", item.Type.ToString());
                XmlHelper.AddNode(doc, productXml, "Points", item.Points.ToString());
                XmlHelper.AddNode(doc, productXml, "Message", item.Message);
                XmlHelper.AddNode(doc, productXml, "CreateDate", ProductHelper.FormatDate(item.CreatedOnUtc, timeZone, timeOffset, "dd/MM/yyyy"));
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("Product", "RewardPoint.xslt"), doc);

            string pageUrl = SiteRoot + "/Account/RewardHistory.aspx";
            if (pageUrl.Contains("?"))
                pageUrl += "&amp;page={0}";
            else
                pageUrl += "?page={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.PageSize = pageSize;
            pgr.ItemCount = totalRows;
            pgr.CurrentIndex = pageNumber;
            divPager.Visible = (totalRows > pageSize);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }
    }
}
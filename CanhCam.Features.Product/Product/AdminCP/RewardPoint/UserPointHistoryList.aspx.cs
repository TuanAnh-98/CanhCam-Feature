using CanhCam.Business;
using log4net;
using Resources;
using System;
using System.Data;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class UserPointHistoryListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserPointHistoryListPage));
        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            PopulateLabels();
            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.UserPointHistoryTitle);
            //heading.Text = ProductResources.UserPointHistoryTitle;

            //UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void PopulateControls()
        {
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.PagerStyle.EnableSEOPaging = false;
            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            DateTime? startDateTo = null;
            DateTime? endDateTo = null;

            if (dpStartDate.Text.Trim().Length > 0)
            {
                startDateTo = DateTime.Parse(dpStartDate.Text);
            }
            if (dpEndDate.Text.Trim().Length > 0)
            {
                endDateTo = DateTime.Parse(dpEndDate.Text);
            }

            //int iCount = UserPoint.GetCountBySearch(startDateTo, endDateTo, txtTitle.Text.Trim());
            int iCount = RewardPointsHistory.GetCountBySearch(siteSettings.SiteId, -1, (int)RewardPointType.Redeemed, txtTitle.Text.Trim(), startDateTo, endDateTo);
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.DataSource = RewardPointsHistory.GetPageBySearch(siteSettings.SiteId, -1, (int)RewardPointType.Redeemed, txtTitle.Text.Trim(), startDateTo, endDateTo, startRowIndex, maximumRows);
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            gridPersister = new RadGridSEOPersister(grid);
            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            btnSearch.Click += new EventHandler(BtnSearch_Click);
        }
    }
}
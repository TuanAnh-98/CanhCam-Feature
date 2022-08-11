/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2016-08-15
/// Last Modified:			2016-08-15

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class BestsellersReport : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BestsellersReport));

        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (!ProductPermission.CanManageOrders)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);

            ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), "-1"));
        }

        #endregion Populate Zone List

        private DataTable GetData(bool forExport)
        {
            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            int zoneId = Convert.ToInt32(ddZones.SelectedValue);
            int orderBy = Convert.ToInt32(ddOrderBy.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;

            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            return Order.GetReportBestsellers(SiteId, zoneId, status, fromdate, todate, 100, orderBy, forExport);
        }

        #region Event

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetData(true);
            string fileName = "report-bestseller-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";
            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        #endregion Event



        #region Populate

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
        }

        private void PopulateControls()
        {
            OrderHelper.PopulateOrderStatus(ddOrderStatus, true);
            PopulateZoneList();

            BindData();
        }

        private void BindData()
        {
            var dt = GetData(false);
            grid.DataSource = dt;

            chart.DataSource = dt;
            chart.DataBind();

            grid.DataSource = dt;
            grid.DataBind();

            grid.Visible = chkListReportType.Items[0].Selected == true;
            chart.Visible = chkListReportType.Items[1].Selected == true;
        }

        #endregion Populate

        #region LoadSettings

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
        }

        #endregion LoadSettings

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        #endregion OnInit
    }
}
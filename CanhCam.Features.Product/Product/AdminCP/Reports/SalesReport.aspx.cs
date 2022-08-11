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

namespace CanhCam.Web.ProductUI
{
    public partial class SalesReportPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SalesReportPage));

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

        private DataTable GetData(bool isExport)
        {
            int state = -1;
            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;

            if (dpFromDate.SelectedDate.HasValue)
            {
                DateTime localTime = dpFromDate.SelectedDate.Value;
                //localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);
                fromdate = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);
                //if (timeZone != null)
                //    fromdate = localTime.ToUtc(timeZone);
                //else
                //    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.SelectedDate.HasValue)
            {
                DateTime localTime = dpToDate.SelectedDate.Value;
               // localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);
                todate = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);
                //if (timeZone != null)
                //    todate = localTime.ToUtc(timeZone);
                //else
                //    todate = localTime.AddHours(-timeOffset);
            }

            return Order.GetReportSales(SiteId, state, status, fromdate, todate, Convert.ToInt32(ddReportBy.SelectedValue), isExport);
        }

        #region Event

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetData(true);
            string fileName = "report-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";
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
            dpFromDate.SelectedDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);

            var endDate = dpFromDate.SelectedDate.Value.AddMonths(1).AddDays(-1);
            dpToDate.SelectedDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            BindData();
        }

        private void BindData()
        {
            var dt = GetData(false);
            grid.DataSource = dt;

            chart.DataSource = dt;
            chart.DataBind();

            chart2.DataSource = dt;
            chart2.DataBind();

            grid.DataSource = dt;
            grid.DataBind();

            grid.Visible = chkListReportType.Items[0].Selected == true;
            chart.Visible = chkListReportType.Items[1].Selected == true;
            chart2.Visible = chkListReportType.Items[2].Selected == true;
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
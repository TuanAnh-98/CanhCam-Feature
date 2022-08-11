using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using Resources;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.AccountUI
{
    public partial class CustomersListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomersListPage));
        private SiteUser siteUser;

        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnExport.Click += new EventHandler(BtnExport_Click);
            gridPersister = new RadGridSEOPersister(grid);
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCustomersForExport();

            string fileName = "orders-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        private DataTable GetCustomersForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add(AccountResources.CustomerNameLabel, typeof(string));
            dt.Columns.Add(AccountResources.CustomerEmailLabel, typeof(string));
            dt.Columns.Add(AccountResources.CustomerPhoneLabel, typeof(string));
            dt.Columns.Add(AccountResources.CustomerTotalOrdersBoughtLabel, typeof(string));
            dt.Columns.Add(AccountResources.CustomerTotalMoneyLabel, typeof(string));
            dt.Columns.Add(AccountResources.CustomerTotalConfirmedMoneyLabel, typeof(string));
            dt.Columns.Add(AccountResources.CustomerTotalUnconfirmedMoneyLabel, typeof(string));
            //dt.Columns.Add(AccountResources.CustomerPhoneReferrerLabel, typeof(string));
            //dt.Columns.Add(AccountResources.CustomerEmailReferrerLabel, typeof(string));
            dt.Columns.Add(ProductResources.MemberRankViewPointLabel, typeof(string));
            dt.Columns.Add(ProductResources.MemberRankViewRankLabel, typeof(string));

            DateTime? fromDate = null;
            DateTime? toDate = null;
            var nameKeyword = txtNameKeyword.Text.Trim();
            var emailKeyword = txtEmailKeyword.Text.Trim();
            var phoneKeyword = txtPhoneKeyword.Text.Trim();

            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromDate = localTime.ToUtc(timeZone);
                else
                    fromDate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    toDate = localTime.ToUtc(timeZone);
                else
                    toDate = localTime.AddHours(-timeOffset);
            }
            int iCount = SiteUserCustom.GetCustomerCountBySearch(fromDate, toDate, nameKeyword, emailKeyword, phoneKeyword);
            var customers = SiteUserCustom.GetCustomerBySearch(fromDate, toDate, nameKeyword, emailKeyword, phoneKeyword, 1, iCount);
            int i = 1;
            foreach (SiteUserCustom user in customers)
            {
                DataRow row = dt.NewRow();
                row["Number"] = i;
                row[AccountResources.CustomerNameLabel] = user.Name;
                row[AccountResources.CustomerEmailLabel] = user.Email;
                row[AccountResources.CustomerPhoneLabel] = user.Phone;
                row[AccountResources.CustomerTotalOrdersBoughtLabel] = user.TotalOrdersBought;
                row[AccountResources.CustomerTotalMoneyLabel] = CanhCam.Web.ProductUI.ProductHelper.FormatPrice(user.TotalMoney, true);
                row[AccountResources.CustomerTotalConfirmedMoneyLabel] = CanhCam.Web.ProductUI.ProductHelper.FormatPrice(user.TotalConfirmedMoney, true);
                row[AccountResources.CustomerTotalUnconfirmedMoneyLabel] = CanhCam.Web.ProductUI.ProductHelper.FormatPrice(user.TotalUnconfirmedMoney, true);
                //string phoneRefer = string.Empty;
                //string emailRefer = string.Empty;

                //SiteUser u = new SiteUser(siteSettings, user.UserID);
                //if (!string.IsNullOrEmpty(u.GetCustomPropertyAsString("PhoneReferrer")))
                //    phoneRefer = u.GetCustomPropertyAsString("PhoneReferrer");

                //if (!string.IsNullOrEmpty(u.GetCustomPropertyAsString("EmailReferrer")))
                //    emailRefer = u.GetCustomPropertyAsString("EmailReferrer");
                //row[AccountResources.CustomerPhoneReferrerLabel] = phoneRefer;
                //row[AccountResources.CustomerEmailReferrerLabel] = emailRefer;
                row[ProductResources.MemberRankViewPointLabel] = user.TotalPosts;
                row[ProductResources.MemberRankViewRankLabel] = MemberHelper.GetMemberRank(user.UserID, ddlMemberRank.SelectedItem);
                i += 1;
                dt.Rows.Add(row);
            }
            return dt;
        }

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

            if (siteUser == null || siteUser.SiteId <= 0)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            if (!IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            ddlMemberRank.Items.Clear();
            ddlMemberRank.Items.Add(new ListItem("Tất cả", "-1"));
            var lstRank = MemberRank.GetAll();
            foreach (MemberRank rank in lstRank)
            {
                ddlMemberRank.Items.Add(new ListItem(rank.Name, rank.RankOrder.ToString()));
            }
        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/CustomerEdit.aspx";
            //if (siteUser != null && siteUser.UserId > 0)
            //    lnkInsert.Visible = true;//AccountHelper.IsAllowedAddNewAddress(siteUser.UserId);
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, AccountResources.CustomersListTitle);
        }

        #region GridEvents

        private void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int userId = Convert.ToInt32(item.GetDataKeyValue("UserID"));
                Literal litCustomerPhoneReferrer = (Literal)item.FindControl("litCustomerPhoneReferrer");
                Literal litCustomerEmailReferrer = (Literal)item.FindControl("litCustomerEmailReferrer");

                SiteUser user = new SiteUser(siteSettings, userId);
                if (user != null
                   && litCustomerPhoneReferrer != null
                   && litCustomerEmailReferrer != null)
                {
                    if (!string.IsNullOrEmpty(user.GetCustomPropertyAsString("PhoneReferrer")))
                        litCustomerPhoneReferrer.Text = user.GetCustomPropertyAsString("PhoneReferrer");

                    if (!string.IsNullOrEmpty(user.GetCustomPropertyAsString("EmailReferrer")))
                        litCustomerEmailReferrer.Text = user.GetCustomPropertyAsString("EmailReferrer");
                }
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;
            var nameKeyword = txtNameKeyword.Text.Trim();
            var emailKeyword = txtEmailKeyword.Text.Trim();
            var phoneKeyword = txtPhoneKeyword.Text.Trim();

            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromDate = localTime.ToUtc(timeZone);
                else
                    fromDate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    toDate = localTime.ToUtc(timeZone);
                else
                    toDate = localTime.AddHours(-timeOffset);
            }

            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            int memberRank = Convert.ToInt32(ddlMemberRank.SelectedValue);
            int iCount = SiteUserCustom.GetCountBySearch(fromDate, toDate, nameKeyword, emailKeyword, phoneKeyword, memberRank);
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;
            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;

            grid.DataSource = SiteUserCustom.GetPageBySearch(fromDate, toDate, nameKeyword, emailKeyword, phoneKeyword,
               memberRank, startRowIndex, maximumRows);
        }

        #endregion GridEvents

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int userID = Convert.ToInt32(data.GetDataKeyValue("UserID"));
                    SiteUser user = new SiteUser(siteSettings, userID);

                    if (user != null && user.UserId > 0 && user.SiteId == siteSettings.SiteId && !user.IsDeleted)
                    {
                        ContentDeleted.Create(siteSettings.SiteId, user.UserId.ToString(), "User", typeof(ContentDeleted).AssemblyQualifiedName, user.UserId.ToString(), Page.User.Identity.Name);

                        //user.IsDeleted = true;
                        user.Save();

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    LogActivity.Write("Delete " + iRecordDeleted.ToString() + " user(s)", "User");
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void btnSysnERP_Click(object sender, EventArgs e)
        {
            var users = SiteUser.GetPage(siteSettings.SiteId, 1, 21474836, "", 0, out int _);
            StringBuilder builder = new StringBuilder();
            StringBuilder newBuilder = new StringBuilder();
            foreach (SiteUser user in users)
            {
                try
                {
                    string erpCode = user.GetCustomPropertyAsString(ERPHelper.CustomerCodeKey);
                    if (!string.IsNullOrEmpty(erpCode))
                        continue;
                    if (string.IsNullOrEmpty(erpCode)
                        && ERPHelper.SysnMember(user))
                    {
                        newBuilder.Append("<br/>Synced  " + user.Email + "-Phone Number :" + user.GetCustomPropertyAsString("Phone") + "- ERP Code : " + user.GetCustomPropertyAsString("Phone"));
                    }
                    else
                    {
                        builder.Append("<br/>Can't Sysn " + user.Email);
                    }

                }
                catch (Exception)
                {
                }
            }
            message.SuccessMessage = "Sysn Member Success " + newBuilder.Length + "/" + users.Count + "<br/>" + newBuilder.ToString();
            if (builder.Length > 0)
                message.WarningMessage = builder.ToString();
        }
    }
}
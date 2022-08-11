using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class MemberRankViewPage : CmsNonBasePage
    {
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
            Title = SiteUtils.FormatPageTitle(siteSettings, breadcrumb.CurrentPageTitle);
            heading.Text = breadcrumb.CurrentPageTitle;

            //UIHelper.AddConfirmationDialog(btnDelete, ProductResources.OrderDeleteMultiWarning);
        }

        private void PopulateControls()
        {
            ddlMemberRank.Items.Add(new ListItem("Tất cả", "-1"));
            var lstRank = MemberRank.GetAll();
            foreach (MemberRank rank in lstRank)
            {
                ddlMemberRank.Items.Add(new ListItem(rank.Name, rank.Id.ToString()));
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.PagerStyle.EnableSEOPaging = false;

            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            //int iCount = SiteUserCustom.GetCountByAdminSearch(siteSettings.SiteId, txtTitle.Text.Trim());
            int iCount = SiteUserCustom.GetCountByMemberRank(siteSettings.SiteId, txtTitle.Text.Trim(), Convert.ToInt32(ddlMemberRank.SelectedValue));
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;
            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            //var lstUsers = SiteUserCustom.GetPageByAdminSearch(siteSettings.SiteId, txtTitle.Text.Trim(), startRowIndex, maximumRows);
            var lstUsers = SiteUserCustom.GetPageByMemberRank(siteSettings.SiteId, txtTitle.Text.Trim(), Convert.ToInt32(ddlMemberRank.SelectedValue), startRowIndex, maximumRows);

            //var lstUsers = SiteUserCustom.GetAll();
            //DataTable dt = GetUserTable(lstUsers, ddlMemberRank.SelectedItem);

            //bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            //int iCount = SiteUserCustom.GetCountByAdminSearch(siteSettings.SiteId, txtTitle.Text.Trim());

            //grid.VirtualItemCount = iCount;
            //grid.AllowCustomPaging = !isApplied;

            grid.DataSource = GetUserTable(lstUsers, ddlMemberRank.SelectedItem);
        }

        private DataTable GetUserTable(List<SiteUserCustom> lstUsers, ListItem item)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Point", typeof(string));
            dt.Columns.Add("Rank", typeof(string));

            {
                foreach (SiteUserCustom user in lstUsers)
                {
                    DataRow row = dt.NewRow();

                    row["Id"] = user.UserID;
                    row["Name"] = user.Name;
                    row["Email"] = user.Email;
                    row["Point"] = user.TotalPosts;
                    MemberRank rank = new MemberRank(SiteUser.GetByEmail(CacheHelper.GetCurrentSiteSettings(), user.Email), user.TotalPosts);
                    row["Rank"] = string.IsNullOrEmpty(rank.Name) ? string.Empty : rank.Name;
                    if (item.Value == "-1" || item.Text == rank.Name)
                        dt.Rows.Add(row);
                }
            }
            return dt;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            gridPersister = new RadGridSEOPersister(grid);
        }
    }
}
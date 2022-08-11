using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class MemberRankPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MemberRankPage));
        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private SiteUser siteUser = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            //if (!WebUser.IsAdminOrContentAdmin && !ProductPermission.CanManageOrders)
            //{
            //    SiteUtils.RedirectToAccessDeniedPage(this);
            //    return;
            //}

            LoadSettings();

            //if (siteUser == null || siteUser.UserId <= 0)
            //{
            //    SiteUtils.RedirectToAccessDeniedPage(this);
            //    return;
            //}

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/Member/MemberRankEdit.aspx";
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.MemberRankAdminTitle);
            heading.Text = ProductResources.MemberRankAdminTitle;

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.OrderDeleteMultiWarning);
        }

        private void PopulateControls()
        {
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int rankId = Convert.ToInt32(data.GetDataKeyValue("Id"));
                    MemberRank rank = new MemberRank(rankId);

                    if (rank != null && rank.Id > 0)
                    {
                        MemberRank.Delete(rank.Id);
                        LogActivity.Write("Delete member rank ", rank.Name);
                        // reiterate rank again because we need this (check TTCE ElectricTierPrice for reference)
                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    //LogActivity.Write("Delete " + iRecordDeleted.ToString() + " order(s)", "Order");
                    MemberHelper.ReorderMemberRankOrderList();
                    MemberHelper.RearrangeMemberPointRange();
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            List<MemberRank> list = MemberRank.GetAll().OrderBy(m => m.RankOrder).ToList();
            grid.DataSource = list;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            btnDelete.Click += new EventHandler(btnDelete_Click);
            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            //grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);

            gridPersister = new RadGridSEOPersister(grid);
        }
    }
}
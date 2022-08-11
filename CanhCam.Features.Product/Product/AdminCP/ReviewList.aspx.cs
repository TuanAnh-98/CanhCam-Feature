/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-07-13
/// Last Modified:		    2015-07-13

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ReviewListPage : CmsNonBasePage
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewListPage));

        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;

        private bool appliedFilter = false;
        private int pageNumber = 1;

        protected bool canUpdate = false;
        protected bool canDelete = false;
        protected bool canApprove = false;

        #endregion Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (
                !canUpdate
                && !canDelete
                && !canApprove
                )
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductReviewTitle);
            heading.Text = ProductResources.ProductReviewTitle;

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductReviewDeleteMultiWarning);
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            canUpdate = ProductPermission.CanUpdateComment;
            canDelete = ProductPermission.CanDeleteComment;
            canApprove = ProductPermission.CanApproveComment;
            pgr.PageSize = grid.PageSize;

            btnApprove.Visible = canApprove;
            btnNotApproved.Visible = canApprove;
            btnDelete.Visible = canDelete;

            CmsBasePage basePage = Page as CmsBasePage;
            if (basePage != null) { basePage.ScriptConfig.IncludeFancyBox = true; }

            AddClassToBody("admin-productreview");
        }

        protected override void OnPreRender(System.EventArgs e)
        {
            base.OnPreRender(e);

            if (HttpContext.Current == null) { return; }
            if (HttpContext.Current.Request == null) { return; }

            SetupScripts();
        }

        private void SetupScripts()
        {
            StringBuilder initAutoScript = new StringBuilder();
            string fancyBoxConfig = "{width:'550', height:'80%', 'padding': 0, type:'iframe', autoSize:false, title:{type:'outside'} }";
            initAutoScript.Append("$('a.review-popup').fancybox(" + fancyBoxConfig + ");");

            ScriptManager.RegisterStartupScript(this, typeof(Page),
                   "fancy-init", "\n<script type=\"text/javascript\" >"
                   + initAutoScript.ToString() + "</script>", false);
        }

        private void PopulateControls()
        {
            BindCommentType();
            BindGrid();
        }

        protected static string GetCommentStatus(int commentStatus)
        {
            switch (commentStatus)
            {
                case 0:
                    return "<span style='color:Blue;'>Mới</span>";
                case 1:
                    return "<span style='color:Green;'>Đã duyệt</span>";
                case -1:
                    return "<span style='color:Gray;'>Không duyệt</span>";
                case -2:
                    return "<span style='color:Red;'>Report</span>";
            }

            return string.Empty;
        }

        private void BindCommentType()
        {
            ddCommentType.Items.Clear();
            ddCommentType.Items.Add(new ListItem { Text = "Tất cả", Value = "-1" });
            foreach (var item in typeof(ProductCommentType).GetFields())
            {
                if (item.FieldType == typeof(ProductCommentType))
                    ddCommentType.Items.Add(new ListItem { Text = item.Name, Value = item.GetRawConstantValue().ToString() });
            }
        }

        #region "RadGrid Event"

        private void BindGrid()
        {
            int status = Convert.ToInt32(ddStatus.Text.Trim());
            int orderBy = Convert.ToInt32(ddOrderBy.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;
            string keyword = null;

            int position = -1;
            if (ckbPosition.Checked)
            {
                appliedFilter = true;
                position = 1;
            }
            if (txtKeyword.Text.Trim().Length > 0)
            {
                appliedFilter = true;
                keyword = txtKeyword.Text.Trim();
            }

            if (status != -10 || orderBy != -1)
                appliedFilter = true;

            int parentId = -1;

            if (dpFromDate.Text.Trim().Length > 0)
            {
                appliedFilter = true;
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                appliedFilter = true;
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            if (appliedFilter)
                parentId = 0;

            int commentType = Convert.ToInt32(ddCommentType.SelectedValue);
            int iCount = ProductComment.GetCount(siteSettings.SiteId, -1, commentType, status, parentId, position, fromdate, todate, keyword);
            var lstComments = ProductComment.GetPage(siteSettings.SiteId, -1, commentType, status, parentId, position, fromdate, todate, keyword, orderBy, pageNumber, pgr.PageSize);
            var lstResults = new List<ProductComment>();
            int i = 0;
            foreach (ProductComment comment in lstComments)
            {
                i += 1;
                comment.RowNumber = ((pgr.PageSize * (pageNumber - 1)) + i).ToString();
                lstResults.Add(comment);

                if (!appliedFilter)
                {
                    var lstChildComments = ProductComment.GetPage(siteSettings.SiteId, -1, commentType, status, comment.CommentId, position, fromdate, todate, keyword, 1, 1, 100);
                    if (lstChildComments.Count > 0)
                    {
                        foreach (ProductComment child in lstChildComments)
                            lstResults.Add(child);
                    }
                }
            }

            grid.DataSource = lstResults;
            grid.DataBind();

            pgr.ShowFirstLast = true;
            pgr.ItemCount = iCount;
            pgr.Visible = (iCount > pgr.PageSize);
        }

        private void pgr_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            pageNumber = Convert.ToInt32(e.CommandArgument);
            pgr.CurrentIndex = pageNumber;
            BindGrid();
        }

        protected bool IsParent(int parentId)
        {
            if (appliedFilter || parentId == -1)
                return true;

            return false;
        }

        private void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int parentId = Convert.ToInt32(item.GetDataKeyValue("ParentId"));

                if (!appliedFilter && parentId > 0)
                    item.Font.Italic = true;
            }
        }

        //void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        //{
        //    int status = Convert.ToInt32(ddStatus.Text.Trim());
        //    int orderBy = Convert.ToInt32(ddOrderBy.SelectedValue);
        //    DateTime? fromdate = null;
        //    DateTime? todate = null;
        //    string keyword = null;

        //    int position = -1;
        //    if (ckbPosition.Checked)
        //        position = 1;
        //    if (txtKeyword.Text.Trim().Length > 0)
        //        keyword = txtKeyword.Text.Trim();
        //    int parentId = -1;
        //    if (orderBy > -1)
        //        parentId = 0;

        //    if (dpFromDate.Text.Trim().Length > 0)
        //    {
        //        DateTime localTime = DateTime.Parse(dpFromDate.Text);
        //        localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

        //        if (timeZone != null)
        //            fromdate = localTime.ToUtc(timeZone);
        //        else
        //            fromdate = localTime.AddHours(-timeOffset);
        //    }
        //    if (dpToDate.Text.Trim().Length > 0)
        //    {
        //        DateTime localTime = DateTime.Parse(dpToDate.Text);
        //        localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

        //        if (timeZone != null)
        //            todate = localTime.ToUtc(timeZone);
        //        else
        //            todate = localTime.AddHours(-timeOffset);
        //    }

        //    bool isApplied = false;
        //    int iCount = ProductComment.GetCount(siteSettings.SiteId, -1, status, 0, position, fromdate, todate, keyword);

        //    int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
        //    int maximumRows = isApplied ? iCount : grid.PageSize;

        //    grid.VirtualItemCount = iCount;
        //    grid.AllowCustomPaging = !isApplied;

        //    var lstComments = ProductComment.GetPage(siteSettings.SiteId, -1, status, parentId, position, fromdate, todate, keyword, orderBy, startRowIndex, maximumRows);
        //    var lstResults = new List<ProductComment>();
        //    foreach (ProductComment comment in lstComments)
        //    {
        //        lstResults.Add(comment);

        //        if (orderBy == -1)
        //        {
        //            var lstChildComments = ProductComment.GetPage(siteSettings.SiteId, -1, status, comment.CommentId, position, fromdate, todate, keyword, orderBy, 1, 100);
        //            if (lstChildComments.Count > 0)
        //            {
        //                foreach (ProductComment child in lstChildComments)
        //                    lstResults.Add(child);
        //            }
        //        }
        //    }

        //    grid.DataSource = lstResults;
        //}

        #endregion "RadGrid Event"

        #region Events

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            if (canApprove)
                Approve(1);
        }

        private void btnNotApproved_Click(object sender, EventArgs e)
        {
            if (canApprove)
                Approve(-1);
        }

        private void Approve(int status)
        {
            try
            {
                bool isUpdated = false;
                foreach (GridDataItem data in grid.SelectedItems)
                {
                    int commentId = Convert.ToInt32(data.GetDataKeyValue("CommentId"));
                    ProductComment comment = new ProductComment(commentId);

                    if (comment != null && comment.CommentId > -1)
                    {
                        comment.Status = status;

                        if (comment.Save())
                        {
                            isUpdated = true;

                            LogActivity.Write("Approval comment", comment.FullName);
                        }
                    }
                }

                if (isUpdated)
                {
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canDelete)
                    return;

                bool isDeleted = false;

                foreach (GridDataItem data in grid.SelectedItems)
                {
                    int commentId = Convert.ToInt32(data.GetDataKeyValue("CommentId"));
                    ProductComment comment = new ProductComment(commentId);

                    if (comment != null && comment.CommentId > -1)
                    {
                        ProductComment.Delete(comment.CommentId);
                        LogActivity.Write("Delete comment", comment.FullName);

                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion Events

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnApprove.Click += new EventHandler(btnApprove_Click);
            this.btnNotApproved.Click += new EventHandler(btnNotApproved_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);

            grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);
            pgr.Command += new System.Web.UI.WebControls.CommandEventHandler(pgr_Command);
        }

        #endregion OnInit
    }
}
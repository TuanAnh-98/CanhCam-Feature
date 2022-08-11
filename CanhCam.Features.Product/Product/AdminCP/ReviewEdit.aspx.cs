/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-07-13
/// Last Modified:		    2015-07-13

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class ReviewEditPage : CmsDialogNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReviewEditPage));

        private int commentId = -1;
        private ProductComment comment = null;
        private bool reply = false;

        protected bool canUpdate = false;
        protected bool canDelete = false;
        protected bool canApprove = false;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            PopulateLabels();
            LoadSettings();

            if (
                !canUpdate
                && !canDelete
                && !canApprove
                )
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (comment == null || comment.CommentId == -1)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ReviewList.aspx");
                return;
            }

            if (comment.ParentId > 0 && reply)
                reply = false;

            btnInsert.Visible = reply && canApprove;
            //btnInsertAndClose.Visible = btnInsert.Visible;
            btnUpdate.Visible = !reply && canUpdate;
            //btnUpdateAndClose.Visible = btnUpdate.Visible;
            //btnDelete.Visible = !reply && canDelete;
            //divRating.Visible = !reply;

            if (btnInsert.Visible)
            {
                heading.Text = "Trả lời bình luận";
                lblComment.Text = "Trả lời";

                if (comment.ParentId == -1)
                {
                    divQuestion.Visible = true;
                    litQuestion.Text = Server.HtmlEncode(comment.ContentText);
                }
            }
            else
            {
                if (canUpdate)
                    divPosition.Visible = true;

                if (comment.ParentId > 0)
                {
                    ProductComment parent = new ProductComment(comment.ParentId);
                    if (parent != null && parent.CommentId > 0)
                    {
                        lblComment.Text = "Trả lời";

                        divQuestion.Visible = true;
                        litQuestion.Text = Server.HtmlEncode(comment.ContentText);
                    }
                }
            }

            if (
                comment.CommentType == (int)ProductCommentType.Rating
                && comment.ParentId <= 0
                )
                divRating.Visible = true;

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            if (reply)
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null)
                {
                    txtFullName.Text = siteUser.Name;
                    txtEmail.Text = siteUser.Email;
                }
            }
            else
            {
                ratRating.Value = comment.Rating;
                txtCommentTitle.Text = comment.Title;
                txtEmail.Text = comment.Email;
                txtFullName.Text = comment.FullName;
                txtComment.Text = comment.ContentText;

                //divCreatedDate.Visible = true;
                divIpAddress.Visible = true;

                if (comment.Position > 0)
                    ckbPosition.Checked = true;

                litIpAddress.Text = comment.CreatedFromIP;
                litCreatedDate.Text = DateTimeHelper.Format(comment.CreatedUtc, SiteUtils.GetUserTimeZone(), "dd/MM/yyyy HH:mm", SiteUtils.GetUserTimeOffset());
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int itemId = Create();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ReviewEdit.aspx?CommentID=" + itemId.ToString());
            }
        }

        private void btnInsertAndClose_Click(object sender, EventArgs e)
        {
            int itemId = Create();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ReviewList.aspx");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int itemId = Update();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int itemId = Update();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ReviewList.aspx");
            }
        }

        private int Create()
        {
            if (!Page.IsValid) return -1;
            if (!reply) return -1;

            if (!canApprove)
                return -1;

            try
            {
                var cm = new ProductComment
                {
                    ProductId = comment.ProductId,
                    ParentId = comment.CommentId,
                    FullName = txtFullName.Text.Trim(),
                    Email = txtEmail.Text.Trim(),
                    Title = txtCommentTitle.Text.Trim(),
                    Status = 1,
                    ContentText = txtComment.Text,
                    Rating = Convert.ToInt32(ratRating.Value),
                    CreatedFromIP = SiteUtils.GetIP4Address(),
                    CommentType = comment.CommentType
                };

                var siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null)
                    cm.UserId = siteUser.UserId;

                cm.Status = 1;
                cm.IsModerator = true;

                if (comment.Status != 1)
                {
                    comment.Status = 1;
                    comment.Save();
                }

                cm.Save();

                LogActivity.Write("Reply comment", comment.FullName);
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");

                return cm.CommentId;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return -1;
        }

        private int Update()
        {
            if (!Page.IsValid) return -1;

            if (!canUpdate)
                return -1;

            try
            {
                comment.FullName = txtFullName.Text.Trim();
                comment.Email = txtEmail.Text.Trim();
                comment.Title = txtCommentTitle.Text.Trim();
                //comment.IsApproved = !config.RequireApprovalForComments;
                comment.ContentText = txtComment.Text;
                comment.Rating = Convert.ToInt32(ratRating.Value);
                if (ckbPosition.Checked)
                    comment.Position = 1;

                comment.Save();

                LogActivity.Write("Update comment", comment.FullName);
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");

                return comment.CommentId;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canDelete)
                    return;

                if (comment != null && comment.CommentId > -1)
                {
                    ProductComment.Delete(comment.CommentId);
                    LogActivity.Write("Delete product comment", comment.FullName);

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ReviewList.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductReviewEditTitle);
            heading.Text = ProductResources.ProductReviewEditTitle;

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));
        }

        private void LoadSettings()
        {
            commentId = WebUtils.ParseInt32FromQueryString("CommentID", commentId);
            reply = WebUtils.ParseBoolFromQueryString("reply", false);
            canUpdate = ProductPermission.CanUpdateComment;
            canDelete = ProductPermission.CanDeleteComment;
            canApprove = ProductPermission.CanApproveComment;

            if (commentId > 0)
                comment = new ProductComment(commentId);
        }
    }
}
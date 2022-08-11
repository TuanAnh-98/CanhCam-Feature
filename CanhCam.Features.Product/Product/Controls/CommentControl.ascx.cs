/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-13
/// Last Modified:			2014-10-23

using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class CommentControl : UserControl, IRefreshAfterPostback
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(CommentControl));

        protected ProductConfiguration config = new ProductConfiguration();
        private SiteUser currentUser = null;
        private Product product = null;

        private int pageNumber = 1;
        protected bool allowComments = true;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private int approvedStatus = 1;

        public ProductConfiguration Config
        {
            get { return config; }
            set { config = value; }
        }

        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
            }
        }

        private ProductCommentType commentType = ProductCommentType.Rating;

        public ProductCommentType CommentType
        {
            get { return commentType; }
            set
            {
                commentType = value;
            }
        }

        #endregion Properties

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            base.OnInit(e);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();

            if (!allowComments || !displaySettings.ShowComments)
            {
                this.Visible = false;
                return;
            }

            SetupScript();
            PopulateLabels();
            PopulateControls();
        }

        protected virtual void PopulateControls()
        {
            SetupCommentSystem();
        }

        #region IRefreshAfterPostback

        public void RefreshAfterPostback()
        {
            PopulateControls();
        }

        #endregion IRefreshAfterPostback

        private void PopulateLabels()
        {
            divStar.Visible = true;

            pnlFeedback.Attributes.Add("class", "commentpanel commentpanel" + ((int)commentType).ToString());
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            currentUser = SiteUtils.GetCurrentSiteUser();
            pageNumber = WebUtils.ParseInt32FromQueryString("cmtpg", pageNumber);

            if (
                (!this.Visible)
                || (product == null)
                || (product.ProductId == -1)
                )
            {
                // query string params have been manipulated
                allowComments = false;
                return;
            }
        }

        private void SetupScript()
        {
            //var skinUrl = SiteUtils.DetermineSkinBaseUrl(SiteUtils.GetSkinName(false));
            //if (!Page.ClientScript.IsStartupScriptRegistered("commentplugin"))
            //{
            //    this.Page.ClientScript.RegisterStartupScript(this.GetType(), "commentplugin", "<script type=\"text/javascript\" src=\""
            //        + skinUrl + "js/comment.min.js" + "\"></script>");
            //}

            var scripts = new StringBuilder();

            scripts.Append("<script type=\"text/javascript\">"); // script

            scripts.Append("var journalOptions = {};"); // journalOptions
            scripts.Append("journalOptions.itemId=" + product.ProductId + ";");
            scripts.Append("journalOptions.pageSize=" + pgr.PageSize + ";");
            scripts.Append("journalOptions.siteRoot='" + SiteUtils.GetNavigationSiteRoot() + "';");

            if (Request.IsAuthenticated)
            {
                scripts.Append("journalOptions.fullName='" + currentUser.Name + "';");
                scripts.Append("journalOptions.email='" + currentUser.Email + "';");
            }
            else
            {
                scripts.Append("journalOptions.fullName='" + CommentHelper.GetNameCookieValue() + "';");
                scripts.Append("journalOptions.email='';");
            }

            scripts.Append("journalOptions.deleteConfirmText = 'Bạn có chắc chắn muốn xóa bình luận?';");
            scripts.Append("journalOptions.reportSuccessfullyText = 'Đã báo cáo vi phạm thành công!';");
            scripts.Append("journalOptions.contentRequiredText = 'Vui lòng nhập nội dung.';");
            scripts.Append("journalOptions.contentInvalidText = 'Nội dung không hợp lệ.';");
            scripts.Append("journalOptions.fullNameRequiredText = 'Vui lòng nhập tên của bạn.';");
            scripts.Append("journalOptions.emailInvalidText = 'Email không hợp lệ.';");
            scripts.Append("journalOptions.likeText = 'Thích';");
            scripts.Append("journalOptions.unlikeText = 'Không thích';"); // end journalOptions
            scripts.Append("var commentOpts = {};");

            //scripts.Append("$(document).ready(function () {"); // document ready

            //scripts.Append("pluginInit();");

            //scripts.Append("if($('.rating').length){"); // init rating
            //scripts.Append("$('.rating').raty({");
            //scripts.Append("path:'" + skinUrl + "js/rating/',");
            ////scripts.Append("readOnly: true,");
            //scripts.Append("score: function() { return $(this).attr('data-score'); }");
            //scripts.Append("});");
            //scripts.Append("}"); // end init rating

            //scripts.Append("if($('.rating-plg').length){"); // init rating
            //scripts.Append("$('.rating-plg').raty({");
            //scripts.Append("path:'" + skinUrl + "js/rating/',");
            //scripts.Append("readOnly: true,");
            //scripts.Append("score: function() { return $(this).attr('data-score'); }");
            //scripts.Append("});");
            //scripts.Append("}"); // end init rating

            scripts.Append("var jopts = {};");
            scripts.Append("jopts.maxLength=" + 2000 + ";");
            scripts.Append("jopts.placeHolder = '#" + pnlFeedback.ClientID + " .journalPlaceholder';");
            scripts.Append("jopts.shareButton = '#" + pnlFeedback.ClientID + " .btnShare';");
            scripts.Append("jopts.closeButton = '#" + pnlFeedback.ClientID + " .journalClose';");
            scripts.Append("jopts.info = '#" + pnlFeedback.ClientID + " .journalInfo';");
            scripts.Append("jopts.fullName = '#" + pnlFeedback.ClientID + " .txtFullName';");
            scripts.Append("jopts.email = '#" + pnlFeedback.ClientID + " .txtEmail';");
            scripts.Append("jopts.content = '#" + pnlFeedback.ClientID + " .journalContent';");
            scripts.Append("jopts.items = '#" + pnlFeedback.ClientID + " .journalItems';");
            scripts.Append("jopts.pager = '#" + pnlFeedback.ClientID + " .commentpager a';");
            //scripts.Append("$('body').journalTools(jopts);");

            //scripts.Append("});"); // end document ready
            scripts.Append("</script>"); // end script

            litScripts.Text = scripts.ToString();
            //this.Page.ClientScript.RegisterStartupScript(this.GetType(), "commentinit" + ((int)commentType).ToString(), scripts.ToString());
        }

        private void SetupCommentSystem()
        {
            if (!ShouldAllowComments())
            {
                return;
            }

            string commentSystem = DetermineCommentSystem();

            switch (commentSystem)
            {
                case "disqus":

                    break;

                case "intensedebate":

                    break;

                case "facebook":

                    break;

                case "internal":
                default:
                    if (ProductConfiguration.UseLegacyCommentSystem)
                    {
                        SetupLegacyProductComments();
                    }

                    break;
            }
        }

        private bool ShouldAllowComments()
        {
            //comments closed globally
            if (!displaySettings.ShowComments) { return false; }

            // comments not allowed on this post
            if (product.AllowCommentsForDays < 0) { return false; }

            return true;
        }

        //private bool CommentsAreOpen()
        //{
        //    //comments closed globally
        //    if (!displaySettings.ShowComments) { return false; }

        //    // comments not allowed on this product
        //    if (product.AllowCommentsForDays < 0) { return false; }

        //    //no limit on comments for this product
        //    if (product.AllowCommentsForDays == 0) { return true; }

        //    if (product.AllowCommentsForDays > 0) //limited to a specific number of days
        //    {
        //        DateTime endDate = product.StartDate.AddDays((double)product.AllowCommentsForDays);

        //        if (endDate > DateTime.UtcNow) { return true; }

        //    }

        //    return false;
        //}

        private string DetermineCommentSystem()
        {
            // don't use new external comment system for existing posts that already have comments
            if (product.CommentCount > 0) { return "internal"; }

            return config.CommentSystem;
        }

        private void SetupLegacyProductComments()
        {
            pnlFeedback.Visible = true;// ( && Request.IsAuthenticated);
            pnlComment.Visible = true;
            if (!ConfigHelper.GetBoolProperty("EnableAllowAnonymousComment", true) && !Request.IsAuthenticated)
                pnlComment.Visible = false;

            pnlFeedback.Attributes["data-type"] = ((int)commentType).ToString();

            if (!IsPostBack)
                BindComments();
        }

        private void BindComments()
        {
            var iCount = ProductComment.GetCount(product.SiteId, product.ProductId, (int)commentType, approvedStatus, -1, -1, null, null, null);

            string pageUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);
            pageUrl += "?cmtpg={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ItemCount = iCount;
            pgr.CurrentIndex = pageNumber;
            divPager.Visible = (iCount > pgr.PageSize);

            var lstComments = ProductComment.GetPage(product.SiteId, product.ProductId, (int)commentType, approvedStatus, -1, -1, null, null, null, 0, pageNumber, pgr.PageSize);
            if (lstComments.Count > 0)
            {
                rptComments.DataSource = lstComments;
                rptComments.DataBind();

                //var lstTopComments = ProductComment.GetPage(product.SiteId, product.ProductId, (int)commentType, 1, 0, 1, null, null, null, 0, 1, 3);
                //if (lstTopComments.Count > 0)
                //{
                //    rptCommentTop.DataSource = lstTopComments;
                //    rptCommentTop.DataBind();
                //}
            }
            else
            {
                rptComments.Visible = false;
            }

            int percent5 = 0;
            int percent4 = 0;
            int percent3 = 0;
            int percent2 = 0;
            int percent1 = 0;

            int ratingCount5 = 0;
            int ratingCount4 = 0;
            int ratingCount3 = 0;
            int ratingCount2 = 0;
            int ratingCount1 = 0;

            double avg = 0;
            if (product.RatingVotes > 0)
            {
                avg = Convert.ToDouble(product.RatingSum / product.RatingVotes);

                if (displaySettings.CaculatePercentageRating)
                {
                    ratingCount5 = ProductComment.GetCountByRating(product.ProductId, 5);
                    ratingCount4 = ProductComment.GetCountByRating(product.ProductId, 4);
                    ratingCount3 = ProductComment.GetCountByRating(product.ProductId, 3);
                    ratingCount2 = ProductComment.GetCountByRating(product.ProductId, 2);
                    ratingCount1 = ProductComment.GetCountByRating(product.ProductId, 1);

                    percent5 = (int)(((decimal)ratingCount5 / product.RatingVotes) * 100M);
                    percent4 = (int)(((decimal)ratingCount4 / product.RatingVotes) * 100M);
                    percent3 = (int)(((decimal)ratingCount3 / product.RatingVotes) * 100M);
                    percent2 = (int)(((decimal)ratingCount2 / product.RatingVotes) * 100M);
                    percent1 = (int)(((decimal)ratingCount1 / product.RatingVotes) * 100M);
                }
            }

            //if (lstComments.Count > 0)
            //{
            //    var lstRating = lstComments.Select(com => com.Rating);

            //    avg = lstRating.Average();
            //    var lstRating5 = lstComments.Where(com => com.Rating == 5).ToList();
            //    var lstRating4 = lstComments.Where(com => com.Rating == 4).ToList();
            //    var lstRating3 = lstComments.Where(com => com.Rating == 3).ToList();
            //    var lstRating2 = lstComments.Where(com => com.Rating == 2).ToList();
            //    var lstRating1 = lstComments.Where(com => com.Rating == 1).ToList();

            //    percent5 = (int)(0.5f + ((100f * lstRating5.Count) / lstComments.Count));
            //    percent4 = (int)(0.5f + ((100f * lstRating4.Count) / lstComments.Count));
            //    percent3 = (int)(0.5f + ((100f * lstRating3.Count) / lstComments.Count));
            //    percent2 = (int)(0.5f + ((100f * lstRating2.Count) / lstComments.Count));
            //    percent1 = (int)(0.5f + ((100f * lstRating1.Count) / lstComments.Count));

            //    ratingCount5 = lstRating5.Count;
            //    ratingCount4 = lstRating4.Count;
            //    ratingCount3 = lstRating3.Count;
            //    ratingCount2 = lstRating2.Count;
            //    ratingCount1 = lstRating1.Count;
            //}

            litPercent5.Text = string.Format(ProductResources.CommentHintText, percent5, ratingCount5);
            litPercent4.Text = string.Format(ProductResources.CommentHintText, percent4, ratingCount4);
            litPercent3.Text = string.Format(ProductResources.CommentHintText, percent3, ratingCount3);
            litPercent2.Text = string.Format(ProductResources.CommentHintText, percent2, ratingCount2);
            litPercent1.Text = string.Format(ProductResources.CommentHintText, percent1, ratingCount1);

            progress5.Attributes.Add("style", "width:" + percent5 + '%');
            progress4.Attributes.Add("style", "width:" + percent4 + '%');
            progress3.Attributes.Add("style", "width:" + percent3 + '%');
            progress2.Attributes.Add("style", "width:" + percent2 + '%');
            progress1.Attributes.Add("style", "width:" + percent1 + '%');
            litCommentCount.Text = "(" + iCount + " nhận xét)";
            litAVGRating.Text = Math.Round(avg, 1).ToString() + "/5";
            //divProductRating.Attributes.Add("data-score", avg.ToString());
            litRatingPercentage.Text = FormatRating(avg);
        }

        private string FormatRating(double rating)
        {
            if (rating > 5)
                rating = 5;

            return string.Format("<div style=\"width:{0}%\"></div>", ((int)(rating * 20)).ToString());
        }

        protected string FormatRating(int rating)
        {
            if (rating > 5)
                rating = 5;
            return string.Format("<div style=\"width:{0}%\"></div>", rating * 20);
        }

        protected List<ProductComment> GetChildComments(int parentId)
        {
            var lstComments = ProductComment.GetPage(product.SiteId, product.ProductId, (int)commentType, approvedStatus, parentId, -1, null, null, null, 1, 1, pgr.PageSize);

            return lstComments;
        }
    }
}
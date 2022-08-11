using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.ProductUI;
using Resources;
using System;
using System.Web.UI;

namespace CanhCam.Web.AccountUI
{
    public partial class AccountMenu : UserControl
    {
        private SiteUser siteUser = null;
        private SiteSettings siteSettings = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (siteUser == null)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateControls();

            if (!Page.IsPostBack)
            {
                var siteRoot = SiteUtils.GetNavigationSiteRoot();
                var rawUrl = Request.RawUrl.ToLower();

                litDashboard.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/Dashboard.aspx'>" + AccountResources.GeneralInformationLabel + "</a></li>", GetCssClass(rawUrl, "Dashboard"), siteRoot);
                litPurchaseHistory.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Product/PurchaseHistory.aspx'>" + AccountResources.OrderListText + "</a></li>", GetCssClass(rawUrl, "PurchaseHistory"), siteRoot);
                // litNotification.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/Notification.aspx'>" + AccountResources.UserProfileMyNotificationLabel + "</a></li>", GetCssClass(rawUrl, "Notification"), siteRoot);

                litUserProfile.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/UserProfile.aspx'>" + AccountResources.AccountInformationLabel + "</a></li>", GetCssClass(rawUrl, "UserProfile"), siteRoot);
                litAddress.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/Address.aspx'>" + AccountResources.AddessListLabel + "</a></li>", GetCssClass(rawUrl, "Address"), siteRoot);

                // litReviewsProductsPurchased.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/ReviewsProductsPurchased.aspx'>" + AccountResources.CommentProductBuyedLabel + "</a></li>", GetCssClass(rawUrl, "ReviewsProductsPurchased"), siteRoot);
                //// litProductViewed.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/ProductViewed.aspx'>" + AccountResources.ProductViewedLabel + "</a></li>", GetCssClass(rawUrl, "ProductViewed"), siteRoot);
                // litMyReviews.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/MyReviews.aspx'>" + AccountResources.MyCommentLabel + "</a></li>", GetCssClass(rawUrl, "MyReviews"), siteRoot);

                litWishlist.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Product/Wishlist.aspx'>" + AccountResources.WishListText + "</a></li>", GetCssClass(rawUrl, "Wishlist"), siteRoot);
                litChangePassword.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/ChangePassword.aspx'>" + AccountResources.ChangePasswordText + "</a></li>", GetCssClass(rawUrl, "ChangePassword"), siteRoot);
                
                litUserPointHistory.Visible = RewardPointsHelper.Enable;
                litUserPointHistory.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/RewardHistory.aspx'>" + AccountResources.UserPointHistoryLabel + "</a></li>", GetCssClass(rawUrl, "UserPoint"), siteRoot);

                // litOrderSchedule.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/OrderSchedule.aspx'>" + AccountResources.OrderScheduleLabel + "</a></li>", GetCssClass(rawUrl, "OrderSchedule.aspx"), siteRoot);
                // litOrderScheduleConfirmation.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/OrderScheduleConfirmation.aspx'>" + AccountResources.OrderScheduleConfirmationLabel + "</a></li>", GetCssClass(rawUrl, "OrderScheduleConfirmation.aspx"), siteRoot);
                // litRequestReturn.Text = string.Format("<li class='hvr-sweep-to-right{0}'><a href='{1}/Account/RequestReturnOrder.aspx'>" + AccountResources.RequestReturnOrderLabel + "</a></li>", GetCssClass(rawUrl, "OrderScheduleConfirmation.aspx"), siteRoot);
            }
        }

        private void PopulateControls()
        {
            if (siteUser != null)
            {
                lblUserName.Text = AccountHelper.GetFullNameFormat(siteUser);

                userAvatar.UseGravatar = (siteSettings.AvatarSystem == "gravatar");
                userAvatar.Email = siteUser.Email;
                userAvatar.UserName = siteUser.Name;
                userAvatar.UserId = siteUser.UserId;
                userAvatar.AvatarFile = siteUser.AvatarUrl;
                userAvatar.MaxAllowedRating = SiteUtils.GetMaxAllowedGravatarRating();
                userAvatar.Disable = (siteSettings.AvatarSystem != "gravatar" && siteSettings.AvatarSystem != "internal");
                userAvatar.SiteId = siteSettings.SiteId;
                userAvatar.UseLink = false;

                if (userAvatar.Disable) { userAvatar.Visible = false; }
            }
            else
            {
                lblUserName.Text = "User not found";
                userAvatar.Visible = false;
            }
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null || siteUser.UserId <= 0)
                siteUser = null;
        }

        private string GetCssClass(string rawUrl, string pageName)
        {
            var cssClass = string.Empty;
            if (rawUrl.Contains(pageName.ToLower()))
                cssClass = " active";

            return cssClass;
        }
    }
}
/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-07-28
/// Last Modified:			2015-07-28

using CanhCam.Business;
using CanhCam.Web.Framework;
using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: 0d5c6593-44c0-41b1-8b72-8ff4c1db482c
    public partial class CheckoutLoginModule : SiteModuleControl
    {
        private CheckoutLoginConfiguration config = null;

        private TextBox txtUserName;
        private TextBox txtPassword;
        private CheckBox chkRememberMe;
        private HyperLink lnkRecovery;
        private HyperLink lnkExtraLink;
        private Panel divCaptcha = null;
        private Telerik.Web.UI.RadCaptcha captcha = null;

        private Button btnLogin;
        private Button btnCheckoutAsGuest;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (CartHelper.GetShoppingCart(SiteId, ShoppingCartTypeEnum.ShoppingCart).Count == 0)
            {
                CartHelper.SetupRedirectToCartPage(this);
                return;
            }

            LoadSettings();

            if (Request.IsAuthenticated)
            {
                //// user is logged in
                if (config.CheckoutNextZoneId > 0 && config.CheckoutNextZoneId != CurrentZone.ZoneId)
                    WebUtils.SetupRedirect(this, CartHelper.GetZoneUrl(config.CheckoutNextZoneId));
                else
                    LoginCtrl.Visible = false;

                return;
            }

            PopulateControls();
        }

        private void PopulateControls()
        {
            if (siteSettings == null) { return; }
            if (siteSettings.DisableDbAuth) { this.Visible = false; return; }

            LoginCtrl.SetRedirectUrl = true;

            txtUserName = (TextBox)this.LoginCtrl.FindControl("UserName");
            txtPassword = (TextBox)this.LoginCtrl.FindControl("Password");
            chkRememberMe = (CheckBox)this.LoginCtrl.FindControl("RememberMe");
            lnkRecovery = (HyperLink)this.LoginCtrl.FindControl("lnkPasswordRecovery");
            lnkExtraLink = (HyperLink)this.LoginCtrl.FindControl("lnkRegisterExtraLink");
            divCaptcha = (Panel)LoginCtrl.FindControl("divCaptcha");
            captcha = (Telerik.Web.UI.RadCaptcha)LoginCtrl.FindControl("captcha");
            btnLogin = (Button)this.LoginCtrl.FindControl("Login");
            btnCheckoutAsGuest = (Button)this.LoginCtrl.FindControl("btnCheckoutAsGuest");

            if (!siteSettings.RequireCaptchaOnLogin)
            {
                if (divCaptcha != null) { divCaptcha.Visible = false; }
                if (captcha != null) { captcha.Enabled = false; }
            }

            if (lnkRecovery.Visible)
            {
                lnkRecovery.Visible = ((siteSettings.AllowPasswordRetrieval || siteSettings.AllowPasswordReset) && (!siteSettings.UseLdapAuth ||
                                                                               (siteSettings.UseLdapAuth && siteSettings.AllowDbFallbackWithLdap)));
                lnkRecovery.NavigateUrl = this.LoginCtrl.PasswordRecoveryUrl;
            }

            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            if (lnkExtraLink.Visible)
            {
                lnkExtraLink.Visible = siteSettings.AllowNewRegistration;
                lnkExtraLink.NavigateUrl = siteRoot + "/Secure/Register.aspx";

                string returnUrlParam = Page.Request.Params.Get("returnurl");
                if (!String.IsNullOrEmpty(returnUrlParam))
                {
                    //string redirectUrl = returnUrlParam;
                    lnkExtraLink.NavigateUrl += "?returnurl=" + returnUrlParam;
                }
            }

            if (chkRememberMe.Visible)
                chkRememberMe.Visible = siteSettings.AllowPersistentLogin;

            if (!Page.IsPostBack)
            {
                var checkoutAsGuestMessage = MessageTemplate.GetMessage("CheckoutAsGuestMessage", string.Empty);
                if (!string.IsNullOrEmpty(checkoutAsGuestMessage))
                {
                    divEmailInput.Visible = false;
                    divCheckoutAsGuestMessage.Visible = true;
                    litCheckoutAsGuestMessage.Text = checkoutAsGuestMessage;
                }
            }
        }

        protected void btnCheckoutAsGuest_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (rdbCheckoutAsGuest != null
                    && rdbCheckoutAsGuest.Checked)
                {
                    var order = CartHelper.GetOrderSession(siteSettings.SiteId);
                    if (order == null)
                        order = new Order();

                    order.BillingEmail = txtGuestEmail.Text.Trim();
                    CartHelper.SetOrderSession(siteSettings.SiteId, order);

                    if (config.CheckoutNextZoneId > 0)
                        WebUtils.SetupRedirect(this, CartHelper.GetZoneUrl(config.CheckoutNextZoneId));
                }
            }
        }

        private void LoginCtrl_LoggedIn(object sender, EventArgs e)
        {
            if (config.CheckoutNextZoneId > 0)
            {
                WebUtils.SetupRedirect(this, CartHelper.GetZoneUrl(config.CheckoutNextZoneId));
                Response.End();
            }
        }

        protected void rdbCheckout_CheckedChanged(object sender, EventArgs e)
        {
            LoginCtrl.Visible = rdbCheckoutAsReturningCustomer.Checked;
            pnlGuest.Visible = !LoginCtrl.Visible;

            if (rdbCheckoutAsGuest.Checked)
            {
                rdbCheckoutAsGuest.CssClass = "active";
                rdbCheckoutAsReturningCustomer.CssClass = "";
            }
            else
            {
                rdbCheckoutAsGuest.CssClass = "";
                rdbCheckoutAsReturningCustomer.CssClass = "active";
            }

            //if (rdbCheckoutAsGuest.Checked && config.CheckoutNextZoneId > 0)
            //    WebUtils.SetupRedirect(this, CartHelper.GetZoneUrl(config.CheckoutNextZoneId));
        }

        private void LoadSettings()
        {
            EnsureConfiguration();

            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            LoginCtrl.LoggedIn += new EventHandler(LoginCtrl_LoggedIn);
        }

        private void EnsureConfiguration()
        {
            if (config == null)
                config = new CheckoutLoginConfiguration(Settings);
        }
    }
}

using CanhCam.Business;
using CanhCam.Web.Framework;
using System;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ComicUI
{
    public partial class UpdatePassword : CmsDialogBasePage
    {
        private SiteUser siteUser = null;

        private void Page_Load(object sender, System.EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            if (
                siteUser == null
                || siteUser.IsDeleted
                || (siteSettings.RequireApprovalBeforeLogin && !siteUser.ApprovedForLogin)
                )
            {
                //SiteUtils.RedirectToHomepage();
                SetupScripts();
                return;
            }

            if (siteUser.ApprovedForLogin)
            {
                DoUserLogin(siteUser);
                SetupScripts();
            }

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void DoUserLogin(SiteUser siteUser)
        {
            if (siteUser.IsLockedOut)
                FailureText.Text = ResourceHelper.GetResourceString("Resource", "LoginAccountLockedMessage");
            else
            {
                if (siteSettings.UseEmailForLogin)
                    FormsAuthentication.SetAuthCookie(siteUser.Email, true);
                else
                    FormsAuthentication.SetAuthCookie(siteUser.LoginName, true);

                SiteUtils.CreateAndStoreSessionToken(siteUser);
            }
        }

        private void SetupScripts()
        {
            string hookupInputScript = "<script type=\"text/javascript\">"
                + "\nopener.location.reload(); "
                + "\nwindow.close(); "
                + "\n</script>";

            if (!Page.ClientScript.IsStartupScriptRegistered("closepopupscript"))
            {
                Page.ClientScript.RegisterStartupScript(
                    this.GetType(),
                    "closepopupscript", hookupInputScript, false);
            }
        }

        private void PopulateControls()
        {
            if (siteUser.Email.Length > 0 && !siteUser.Email.Contains(siteUser.OpenIdUri))
            {
                Email.Text = siteUser.Email;
                Email.Enabled = false;
            }
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            Page.Validate("ChangePassword1");
            if (Page.IsValid)
            {
                if (siteUser.Email.Contains(siteUser.OpenIdUri) && siteUser.Email != Email.Text.Trim())
                {
                    if (SiteUser.EmailExistsInDB(siteSettings.SiteId, Email.Text.Trim()))
                    {
                        FailureText.Text = ResourceHelper.GetResourceString("Resource", "RegisterDuplicateEmailMessage");
                        return;
                    }

                    siteUser.Email = Email.Text.Trim();
                    siteUser.LoginName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, siteUser.Email);
                    siteUser.Name = siteUser.LoginName;
                }

                siteUser.PasswordResetGuid = Guid.Empty;
                gbMembershipProvider m = Membership.Provider as gbMembershipProvider;
                siteUser.Password = m.EncodePassword(siteSettings, siteUser, txtNewPassword.Text);
                siteUser.MustChangePwd = false;
                siteUser.ApprovedForLogin = true;
                siteUser.Save();
                siteUser.UpdateLastPasswordChangeTime();

                DoUserLogin(siteUser);

                //if (!string.IsNullOrEmpty(Request.QueryString["gl"]))
                //    Response.Redirect("/cart", true);
                SetupScripts();
            }
        }

        private void NewPasswordRulesValidator_ServerValidate(
            object source,
            ServerValidateEventArgs args)
        {
            CustomValidator validator = source as CustomValidator;
            validator.ErrorMessage = string.Empty;

            if (args.Value.Length < Membership.MinRequiredPasswordLength)
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordMinimumLengthWarning")
                    + Membership.MinRequiredPasswordLength.ToInvariantString() + "<br />";
            }

            if (!HasEnoughNonAlphaNumericCharacters(args.Value))
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordMinNonAlphanumericCharsWarning")
                    + Membership.MinRequiredNonAlphanumericCharacters.ToInvariantString() + "<br />";
            }

            gbMembershipProvider m = Membership.Provider as gbMembershipProvider;
            if (siteUser.Password == m.EncodePassword(siteUser.PasswordSalt + txtNewPassword.Text, siteSettings))
            {
                args.IsValid = false;
                validator.ErrorMessage += ResourceHelper.GetResourceString("Resource", "ChangePasswordNewMatchesOldWarning") + "<br />";
            }
        }

        private bool HasEnoughNonAlphaNumericCharacters(string newPassword)
        {
            bool result = false;
            string alphanumeric = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] passwordChars = newPassword.ToCharArray();
            int nonAlphaNumericCharCount = 0;
            foreach (char c in passwordChars)
            {
                if (!alphanumeric.Contains(c.ToString()))
                {
                    nonAlphaNumericCharCount += 1;
                }
            }

            if (nonAlphaNumericCharCount >= Membership.MinRequiredNonAlphanumericCharacters)
            {
                result = true;
            }

            return result;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ResourceHelper.GetResourceString("Resource", "ProfileLink"));

            if (btnChangePassword.Text.Length == 0)
                btnChangePassword.Text = ResourceHelper.GetResourceString("Resource", "ChangePasswordButton");
            NewPasswordCompare.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordMustMatchConfirmMessage");
            ConfirmNewPasswordRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordConfirmPasswordRequiredMessage");
            NewPasswordRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordNewPasswordRequired");
            NewPasswordRegex.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordPasswordRegexFailureMessage");

            string passwordRegexWarning = MessageTemplate.GetMessage("PasswordStrengthErrorMessage");
            if (passwordRegexWarning.Length > 0)
            {
                NewPasswordRegex.ErrorMessage = passwordRegexWarning;
                NewPasswordRegex.ToolTip = passwordRegexWarning;
            }

            NewPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression;
            if (Membership.PasswordStrengthRegularExpression.Length == 0)
            {
                NewPasswordRegex.Enabled = false;
            }

            if (siteSettings.ShowPasswordStrengthOnRegistration)
            {
                txtNewPassword.PasswordStrengthSettings.ShowIndicator = true;
                txtNewPassword.PasswordStrengthSettings.RequiresUpperAndLowerCaseCharacters = true;
                txtNewPassword.PasswordStrengthSettings.MinimumLowerCaseCharacters = WebConfigSettings.PasswordStrengthMinimumLowerCaseCharacters;
                txtNewPassword.PasswordStrengthSettings.MinimumUpperCaseCharacters = WebConfigSettings.PasswordStrengthMinimumUpperCaseCharacters;
                txtNewPassword.PasswordStrengthSettings.MinimumSymbolCharacters = siteSettings.MinRequiredNonAlphanumericCharacters;
                txtNewPassword.PasswordStrengthSettings.PreferredPasswordLength = siteSettings.MinRequiredPasswordLength;

                txtNewPassword.PasswordStrengthSettings.TextStrengthDescriptions = ResourceHelper.GetResourceString("Resource", "PasswordStrengthDescriptions");
                txtNewPassword.PasswordStrengthSettings.CalculationWeightings = WebConfigSettings.PasswordStrengthCalculationWeightings;
            }
        }

        private void LoadSettings()
        {
            var openId = WebUtils.ParseStringFromQueryString("facebookid", string.Empty);
            var userGuid = Guid.Empty;
            if (openId.Length > 0)
                userGuid = SiteUser.GetUserGuidFromOpenId(siteSettings.SiteId, openId);
            else
            {
                openId = WebUtils.ParseStringFromQueryString("googleid", string.Empty);
                if (openId.Length > 0)
                    userGuid = SiteUser.GetUserGuidFromOpenId(siteSettings.SiteId, openId);
            }

            if (userGuid != Guid.Empty)
            {
                siteUser = new SiteUser(siteSettings, userGuid);
                if (siteUser == null || siteUser.UserId <= 0)
                    siteUser = null;
            }
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
            NewPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
            btnChangePassword.Click += new EventHandler(btnChangePassword_Click);
        }
    }
}
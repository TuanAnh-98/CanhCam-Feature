/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-07-25
/// Last Modified:			2014-07-25

using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using System;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.AccountUI
{
    public partial class ChangePasswordPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ChangePasswordPage));

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);
            ChangePassword1.ChangedPassword += new EventHandler(ChangePassword1_ChangedPassword);
        }

        #endregion OnInit

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            PopulateLabels();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ResourceHelper.GetResourceString("Resource", "ChangePasswordLabel"));

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Account/ChangePassword.aspx"
                    + "' class='selectedcrumb'>" + ResourceHelper.GetResourceString("Resource", "ChangePasswordLink")
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Button changePasswordButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ChangePasswordPushButton");
            Button cancelButton = (Button)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CancelPushButton");

            if (changePasswordButton != null)
            {
                changePasswordButton.Text = ResourceHelper.GetResourceString("Resource", "ChangePasswordButton");
            }
            else
            {
                log.Debug("couldn't find changepasswordbutton so couldn't set label");
            }

            if (cancelButton != null)
            {
                cancelButton.Text = ResourceHelper.GetResourceString("Resource", "ChangePasswordCancelButton");
            }
            else
            {
                log.Debug("couldn't find cancelbutton so couldn't set label");
            }

            this.ChangePassword1.CancelDestinationPageUrl
                = SiteUtils.GetNavigationSiteRoot() + "/Account/UserProfile.aspx";

            this.ChangePassword1.ChangePasswordFailureText
                = ResourceHelper.GetResourceString("Resource", "ChangePasswordFailureText");

            CompareValidator newPasswordCompare
                = (CompareValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordCompare");

            if (newPasswordCompare != null)
            {
                newPasswordCompare.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordMustMatchConfirmMessage");
                newPasswordCompare.ToolTip = ResourceHelper.GetResourceString("Resource", "ChangePasswordMustMatchConfirmMessage");
            }

            RequiredFieldValidator confirmNewPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("ConfirmNewPasswordRequired");

            if (confirmNewPasswordRequired != null)
            {
                confirmNewPasswordRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordConfirmPasswordRequiredMessage");
                confirmNewPasswordRequired.ToolTip = ResourceHelper.GetResourceString("Resource", "ChangePasswordConfirmPasswordRequiredMessage");
            }

            this.ChangePassword1.ContinueDestinationPageUrl
                = SiteUtils.GetNavigationSiteRoot();

            RequiredFieldValidator newPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRequired");

            if (newPasswordRequired != null)
            {
                newPasswordRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordNewPasswordRequired");
                newPasswordRequired.ToolTip = ResourceHelper.GetResourceString("Resource", "ChangePasswordNewPasswordRequired");
            }

            RequiredFieldValidator currentPasswordRequired
                = (RequiredFieldValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPasswordRequired");

            if (currentPasswordRequired != null)
            {
                currentPasswordRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordCurrentPasswordRequiredWarning");
                currentPasswordRequired.ToolTip = ResourceHelper.GetResourceString("Resource", "ChangePasswordCurrentPasswordRequiredWarning");
            }

            RegularExpressionValidator newPasswordRegex
                = (RegularExpressionValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRegex");

            if (newPasswordRegex != null)
            {
                newPasswordRegex.ErrorMessage = ResourceHelper.GetResourceString("Resource", "ChangePasswordPasswordRegexFailureMessage");
                newPasswordRegex.ToolTip = ResourceHelper.GetResourceString("Resource", "ChangePasswordPasswordRegexFailureMessage");

                string passwordRegexWarning = MessageTemplate.GetMessage("PasswordStrengthErrorMessage");
                if (passwordRegexWarning.Length > 0)
                {
                    newPasswordRegex.ErrorMessage = passwordRegexWarning;
                    newPasswordRegex.ToolTip = passwordRegexWarning;
                }

                newPasswordRegex.ValidationExpression = Membership.PasswordStrengthRegularExpression;

                if (Membership.PasswordStrengthRegularExpression.Length == 0)
                {
                    newPasswordRegex.Visible = false;
                    newPasswordRegex.ValidationGroup = "None";
                }
            }

            CustomValidator newPasswordRulesValidator
                = (CustomValidator)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPasswordRulesValidator");
            if (newPasswordRulesValidator != null)
            {
                newPasswordRulesValidator.ServerValidate += new ServerValidateEventHandler(NewPasswordRulesValidator_ServerValidate);
            }

            this.ChangePassword1.SuccessTitleText = String.Empty;
            this.ChangePassword1.SuccessText = ResourceHelper.GetResourceString("Resource", "ChangePasswordSuccessText");

            if (siteSettings.ShowPasswordStrengthOnRegistration)
            {
                Telerik.Web.UI.RadTextBox NewPassword = (Telerik.Web.UI.RadTextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPassword");
                if (NewPassword != null)
                {
                    NewPassword.PasswordStrengthSettings.ShowIndicator = true;
                    NewPassword.PasswordStrengthSettings.RequiresUpperAndLowerCaseCharacters = true;
                    NewPassword.PasswordStrengthSettings.MinimumLowerCaseCharacters = WebConfigSettings.PasswordStrengthMinimumLowerCaseCharacters;
                    NewPassword.PasswordStrengthSettings.MinimumUpperCaseCharacters = WebConfigSettings.PasswordStrengthMinimumUpperCaseCharacters;
                    NewPassword.PasswordStrengthSettings.MinimumSymbolCharacters = siteSettings.MinRequiredNonAlphanumericCharacters;
                    NewPassword.PasswordStrengthSettings.PreferredPasswordLength = siteSettings.MinRequiredPasswordLength;

                    NewPassword.PasswordStrengthSettings.TextStrengthDescriptions = ResourceHelper.GetResourceString("Resource", "PasswordStrengthDescriptions");
                    NewPassword.PasswordStrengthSettings.CalculationWeightings = WebConfigSettings.PasswordStrengthCalculationWeightings;
                }
            }

            AddClassToBody("changepasswordpage");
        }

        private void ChangePassword1_ChangedPassword(object sender, EventArgs e)
        {
            ValidationSummary vSummary = (ValidationSummary)ChangePassword1.ChangePasswordTemplateContainer.FindControl("vSummary");
            if (vSummary != null) { vSummary.Visible = false; }
            if (WebConfigSettings.LogIpAddressForPasswordChanges)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    log.Info("user " + currentUser.Name + " changed their password from ip address " + SiteUtils.GetIP4Address());
                }
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
                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordMinimumLengthWarning") + " "
                    + Membership.MinRequiredPasswordLength.ToInvariantString() + "<br />";
                validator.ToolTip
                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordMinimumLengthWarning") + " "
                    + Membership.MinRequiredPasswordLength.ToInvariantString() + "<br />";
            }

            if (!HasEnoughNonAlphaNumericCharacters(args.Value))
            {
                args.IsValid = false;
                validator.ErrorMessage
                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordMinNonAlphanumericCharsWarning")
                    + Membership.MinRequiredNonAlphanumericCharacters.ToInvariantString() + "<br />";
                validator.ToolTip
                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordMinNonAlphanumericCharsWarning")
                    + Membership.MinRequiredNonAlphanumericCharacters.ToInvariantString() + "<br />";
            }

            TextBox currentPassword
                    = (TextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("CurrentPassword");

            Telerik.Web.UI.RadTextBox newPassword
                    = (Telerik.Web.UI.RadTextBox)ChangePassword1.ChangePasswordTemplateContainer.FindControl("NewPassword");

            SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
            if (currentUser != null)
            {
                if (currentPassword != null)
                {
                    switch (Membership.Provider.PasswordFormat)
                    {
                        case MembershipPasswordFormat.Clear:
                            if (currentPassword.Text != currentUser.Password)
                            {
                                args.IsValid = false;
                                validator.ErrorMessage
                                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordCurrentPasswordIncorrectWarning") + "<br />";
                                validator.ToolTip
                                    += ResourceHelper.GetResourceString("Resource", "ChangePasswordCurrentPasswordIncorrectWarning") + "<br />";
                            }
                            break;

                        case MembershipPasswordFormat.Encrypted:

                            break;

                        case MembershipPasswordFormat.Hashed:

                            break;
                    }
                }
            }

            if ((newPassword != null) && (currentPassword != null))
            {
                if (newPassword.Text == currentPassword.Text)
                {
                    args.IsValid = false;
                    validator.ErrorMessage
                       += ResourceHelper.GetResourceString("Resource", "ChangePasswordNewMatchesOldWarning") + "<br />";
                    validator.ToolTip
                       += ResourceHelper.GetResourceString("Resource", "ChangePasswordNewMatchesOldWarning") + "<br />";
                }
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
    }
}
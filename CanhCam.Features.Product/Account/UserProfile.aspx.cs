/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-07-25
/// Last Modified:			2014-07-25

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Business.WebHelpers.ProfileUpdatedHandlers;
using CanhCam.FileSystem;
using CanhCam.Web.Configuration;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using System;
using System.Configuration;
using System.Text;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.AccountUI
{
    public partial class UserProfilePage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(UserProfilePage));
        private string userEmail = string.Empty;
        private SiteUser siteUser;
        private Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private Avatar.RatingType MaxAllowedGravatarRating = SiteUtils.GetMaxAllowedGravatarRating();
        private bool allowGravatars = false;
        private bool disableAvatars = true;
        private string rpxApiKey = string.Empty;
        private string rpxApplicationName = string.Empty;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnUpdate.Click += new EventHandler(this.btnUpdate_Click);
            btnUpdateAvartar.Click += new System.Web.UI.ImageClickEventHandler(btnUpdateAvartar_Click);

            SetupAvatarScript();
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (SiteUtils.SslIsAvailable()) SiteUtils.ForceSsl();
            SecurityHelper.DisableBrowserCache();

            if (!WebConfigSettings.AllowUserProfilePage)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            siteUser = SiteUtils.GetCurrentSiteUser();

            LoadSettings();
            PopulateProfileControls();
            PopulateLabels();

            if (!IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            this.lnkChangePassword.NavigateUrl = SiteRoot + "/Account/ChangePassword.aspx";
            this.lnkChangePassword.Text = ResourceHelper.GetResourceString("Resource", "UserChangePasswordLabel");
            ListItem listItem;

            if ((siteSettings.AllowUserEditorPreference) && (siteUser != null) && (siteUser.EditorPreference.Length > 0))
            {
                listItem = ddEditorProviders.Items.FindByValue(siteUser.EditorPreference);
                if (listItem != null)
                {
                    ddEditorProviders.ClearSelection();
                    listItem.Selected = true;
                }
            }

            if (siteUser != null)
            {
#if!MONO
                ISettingControl setting = timeZoneSetting as ISettingControl;
                if (setting != null)
                {
                    setting.SetValue(siteUser.TimeZoneId);
                }
#endif

                txtName.Text = SecurityHelper.RemoveMarkup(siteUser.Name);
                //txtName.Enabled = siteSettings.AllowUserFullNameChange;
                txtLoginName.Text = SecurityHelper.RemoveMarkup(siteUser.LoginName);
                txtEmail.Text = siteUser.Email;
                lblOpenID.Text = siteUser.OpenIdUri;
                txtPasswordQuestion.Text = siteUser.PasswordQuestion;
                txtPasswordAnswer.Text = siteUser.PasswordAnswer;
                lblCreatedDate.Text = siteUser.DateCreated.AddHours(timeOffset).ToString();
                lnkPubProfile.NavigateUrl = SiteRoot + "/Secure/ProfileView.aspx?userid=" + siteUser.UserId.ToInvariantString();

                if (divLiveMessenger.Visible)
                {
                    WindowsLiveLogin wl = WindowsLiveHelper.GetWindowsLiveLogin();
                    WindowsLiveMessenger m = new WindowsLiveMessenger(wl);

                    if (WebConfigSettings.TestLiveMessengerDelegation)
                    {
                        lnkAllowLiveMessenger.NavigateUrl = m.ConsentOptInUrl;
                    }
                    else
                    {
                        lnkAllowLiveMessenger.NavigateUrl = m.NonDelegatedSignUpUrl;
                    }

                    if (siteUser.LiveMessengerId.Length > 0)
                    {
                        chkEnableLiveMessengerOnProfile.Checked = siteUser.EnableLiveMessengerOnProfile;
                        chkEnableLiveMessengerOnProfile.Enabled = true;
                    }
                    else
                    {
                        chkEnableLiveMessengerOnProfile.Checked = false;
                        chkEnableLiveMessengerOnProfile.Enabled = false;
                    }
                }

                userAvatar.UseGravatar = allowGravatars;
                userAvatar.Email = siteUser.Email;
                userAvatar.UserName = siteUser.Name;
                userAvatar.UserId = siteUser.UserId;
                userAvatar.AvatarFile = siteUser.AvatarUrl;
                userAvatar.MaxAllowedRating = MaxAllowedGravatarRating;
                userAvatar.Disable = disableAvatars;
                userAvatar.SiteId = siteSettings.SiteId;
                userAvatar.UseLink = false;
            }
        }

        private void PopulateProfileControls()
        {
            if (siteUser == null) { return; }

            gbProfileConfiguration profileConfig = gbProfileConfiguration.GetConfig();
            if (profileConfig != null)
            {
                foreach (gbProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
#if!MONO
                    if (propertyDefinition.Name == gbProfilePropertyDefinition.TimeOffsetHoursKey) { continue; }
#endif
                    if (propertyDefinition.Name == gbProfilePropertyDefinition.TimeZoneIdKey) { continue; }

                    if (
                         (propertyDefinition.VisibleToUser)
                      && (
                            (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                            || (siteUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        object propValue = siteUser.GetProperty(propertyDefinition.Name, propertyDefinition.SerializeAs, propertyDefinition.LazyLoad);
                        if (propValue != null)
                        {
                            if (propertyDefinition.EditableByUser)
                            {
                                gbProfilePropertyDefinition.SetupPropertyControl(
                                    this,
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propValue.ToString(),
                                    timeOffset,
                                    timeZone,
                                    SiteRoot);
                            }
                            else
                            {
                                gbProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propValue.ToString(),
                                    timeOffset,
                                    timeZone);
                            }
                        }
                        else
                        {
                            if (propertyDefinition.EditableByUser)
                            {
                                gbProfilePropertyDefinition.SetupPropertyControl(
                                    this,
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propertyDefinition.DefaultValue,
                                    timeOffset,
                                    timeZone,
                                    SiteRoot);
                            }
                            else
                            {
                                gbProfilePropertyDefinition.SetupReadOnlyPropertyControl(
                                    pnlProfileProperties,
                                    propertyDefinition,
                                    propertyDefinition.DefaultValue,
                                    timeOffset,
                                    timeZone);
                            }
                        }
                    }
                }
            }
        }

        private void btnUpdate_Click(Object sender, EventArgs e)
        {
            if (siteUser != null)
            {
                Page.Validate("profile");
                if (Page.IsValid)
                {
                    UpdateUser();
                }
            }
        }

        private void UpdateUser()
        {
            userEmail = siteUser.Email;

            if (
                (siteUser.Email != txtEmail.Text)
                && (SiteUser.EmailExistsInDB(siteSettings.SiteId, txtEmail.Text))
                )
            {
                message.ErrorMessage = ResourceHelper.GetResourceString("Resource", "DuplicateEmailMessage");
                return;
            }

            if ((siteSettings.AllowUserEditorPreference) && (divEditorPreference.Visible))
            {
                siteUser.EditorPreference = ddEditorProviders.SelectedValue;
            }

            if (siteSettings.AllowUserFullNameChange)
            {
                siteUser.Name = txtName.Text;
            }
            siteUser.Email = txtEmail.Text;

            if (WebConfigSettings.LogIpAddressForEmailChanges)
            {
                if ((siteUser.UserId != -1) && (userEmail != siteUser.Email))
                {
                    log.Info("email for user changed from " + userEmail + " to " + siteUser.Email + " from ip address " + SiteUtils.GetIP4Address());
                }
            }

            if (pnlSecurityQuestion.Visible)
            {
                siteUser.PasswordQuestion = this.txtPasswordQuestion.Text;
                siteUser.PasswordAnswer = this.txtPasswordAnswer.Text;
            }
            else
            {
                if (siteUser.PasswordQuestion.Length == 0)
                {
                    siteUser.PasswordQuestion = ResourceHelper.GetResourceString("Resource", "ManageUsersDefaultSecurityQuestion");
                    siteUser.PasswordAnswer = ResourceHelper.GetResourceString("Resource", "ManageUsersDefaultSecurityAnswer");
                }
            }

            if (siteUser.LiveMessengerId.Length > 0)
            {
                siteUser.EnableLiveMessengerOnProfile = chkEnableLiveMessengerOnProfile.Checked;
            }
            else
            {
                siteUser.EnableLiveMessengerOnProfile = false;
            }

#if!MONO
            ISettingControl setting = timeZoneSetting as ISettingControl;
            if (setting != null)
            {
                siteUser.TimeZoneId = setting.GetValue();
            }
#endif

            if (siteUser.Save())
            {
                gbProfileConfiguration profileConfig = gbProfileConfiguration.GetConfig();

                foreach (gbProfilePropertyDefinition propertyDefinition in profileConfig.PropertyDefinitions)
                {
                    if (
                        (propertyDefinition.EditableByUser)
                         && (
                              (propertyDefinition.OnlyAvailableForRoles.Length == 0)
                              || (WebUser.IsInRoles(propertyDefinition.OnlyAvailableForRoles))
                            )
                        )
                    {
                        gbProfilePropertyDefinition.SaveProperty(
                            siteUser,
                            pnlProfileProperties,
                            propertyDefinition,
                            timeOffset,
                            timeZone);
                    }
                }

                siteUser.UpdateLastActivityTime();
                if ((userEmail != siteUser.Email) && (siteSettings.UseEmailForLogin) && (!siteSettings.UseLdapAuth))
                {
                    FormsAuthentication.SetAuthCookie(siteUser.Email, false);
                }

                ProfileUpdatedEventArgs u = new ProfileUpdatedEventArgs(siteUser, false);
                OnUserUpdated(u);

                WebUtils.SetupRedirect(this, Request.RawUrl);
                return;
            }
        }

        protected void OnUserUpdated(ProfileUpdatedEventArgs e)
        {
            foreach (ProfileUpdatedHandlerProvider handler in ProfileUpdatedHandlerProviderManager.Providers)
            {
                handler.ProfileUpdatedHandler(null, e);
            }
        }

        private void LoadSettings()
        {
#if!MONO
            divTimeZone.Visible = true;

#endif

            QuestionRequired.Enabled = siteSettings.RequiresQuestionAndAnswer;
            AnswerRequired.Enabled = siteSettings.RequiresQuestionAndAnswer;

            switch (siteSettings.AvatarSystem)
            {
                case "gravatar":
                    allowGravatars = true;
                    disableAvatars = false;
                    break;

                case "internal":
                    allowGravatars = false;
                    disableAvatars = false;

                    lblAvatar.ConfigKey = "Photo";
                    if (siteUser != null)
                    {
                        lnkAvatarUpld.NavigateUrl = SiteRoot + "/Dialog/AvatarUploadDialog.aspx?u=" + siteUser.UserId.ToInvariantString();
                    }

                    if (WebConfigSettings.AvatarsCanOnlyBeUploadedByAdmin)
                    {
                        lnkAvatarUpld.Visible = false;
                    }

                    break;

                case "none":
                default:
                    allowGravatars = false;
                    disableAvatars = true;
                    break;
            }

            if (siteSettings.UseLdapAuth && !siteSettings.AllowDbFallbackWithLdap)
            {
                this.lnkChangePassword.Visible = false;
            }

            divEditorPreference.Visible = siteSettings.AllowUserEditorPreference;

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            divOpenID.Visible = WebConfigSettings.EnableOpenIdAuthentication && siteSettings.AllowOpenIdAuth;

            rpxApiKey = siteSettings.RpxNowApiKey;
            rpxApplicationName = siteSettings.RpxNowApplicationName;

            if (WebConfigSettings.UseOpenIdRpxSettingsFromWebConfig)
            {
                if (WebConfigSettings.OpenIdRpxApiKey.Length > 0)
                {
                    rpxApiKey = WebConfigSettings.OpenIdRpxApiKey;
                }

                if (WebConfigSettings.OpenIdRpxApplicationName.Length > 0)
                {
                    rpxApplicationName = WebConfigSettings.OpenIdRpxApplicationName;
                }
            }

            if (rpxApiKey.Length > 0)
            {
                lnkOpenIDUpdate.Visible = false;
                rpxLink.Visible = divOpenID.Visible;
            }

            if (!gbSetup.RunningInFullTrust())
            {
                divOpenID.Visible = false;
            }

            if (
                (WebConfigSettings.GloballyDisableMemberUseOfWindowsLiveMessenger)
                || (!siteSettings.AllowWindowsLiveMessengerForMembers)
                || ((siteSettings.WindowsLiveAppId.Length == 0) && (ConfigurationManager.AppSettings["GlobalWindowsLiveAppKey"] == null))
                )
            {
                divLiveMessenger.Visible = false;
            }

            //commerceConfig = SiteUtils.GetCommerceConfig();
            //if (!commerceConfig.IsConfigured)
            //{
            //    liOrderHistory.Visible = false;
            //    tabOrderHistory.Visible = false;
            //}

            if ((WebConfigSettings.UserNameValidationExpression.Length > 0) && (siteSettings.AllowUserFullNameChange))
            {
                regexUserName.ValidationExpression = WebConfigSettings.UserNameValidationExpression;
                regexUserName.ErrorMessage = WebConfigSettings.UserNameValidationWarning;
                regexUserName.Enabled = true;
            }

            FailSafeUserNameValidator.ErrorMessage = ResourceHelper.GetResourceString("Resource", "UserNameHasInvalidCharsWarning");
            FailSafeUserNameValidator.ServerValidate += new ServerValidateEventHandler(FailSafeUserNameValidator_ServerValidate);
            regexEmail.ValidationExpression = SecurityHelper.RegexEmailValidationPattern;

            AddClassToBody("userprofilepage");
        }

        private void FailSafeUserNameValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (args.Value.Contains("<")) { args.IsValid = false; }
            if (args.Value.Contains(">")) { args.IsValid = false; }
            if (args.Value.Contains("/")) { args.IsValid = false; }
            if (args.Value.IndexOf("script", StringComparison.InvariantCultureIgnoreCase) > -1) { args.IsValid = false; }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ResourceHelper.GetResourceString("Resource", "ProfileLink"));

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/Account/UserProfile.aspx"
                    + "' class='selectedcrumb'>" + ResourceHelper.GetResourceString("Resource", "ProfileLink")
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            lnkAllowLiveMessenger.Text = ResourceHelper.GetResourceString("Resource", "EnableLiveMessengerLink");

            btnUpdate.Text = ResourceHelper.GetResourceString("Resource", "UserProfileUpdateButton");

            if ((siteSettings.AllowUserEditorPreference) && (ddEditorProviders.Items.Count == 0))
            {
                ddEditorProviders.DataSource = EditorManager.Providers;
                ddEditorProviders.DataBind();
                foreach (ListItem providerItem in ddEditorProviders.Items)
                {
                    providerItem.Text = providerItem.Text.Replace("Provider", string.Empty);
                }

                ListItem listItem = new ListItem
                {
                    Value = string.Empty,
                    Text = ResourceHelper.GetResourceString("Resource", "SiteDefaultEditor")
                };
                ddEditorProviders.Items.Insert(0, listItem);
            }

            lnkPubProfile.Text = ResourceHelper.GetResourceString("Resource", "PublicProfileLink");
            lnkPubProfile.ToolTip = ResourceHelper.GetResourceString("Resource", "PublicProfileLink");

            rfvName.ErrorMessage = ResourceHelper.GetResourceString("Resource", "UserProfileNameRequired");
            regexEmail.ErrorMessage = ResourceHelper.GetResourceString("Resource", "UserProfileEmailValidation");
            rfvEmail.ErrorMessage = ResourceHelper.GetResourceString("Resource", "UserProfileEmailRequired");

            QuestionRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "RegisterSecurityQuestionRequiredMessage");
            AnswerRequired.ErrorMessage = ResourceHelper.GetResourceString("Resource", "RegisterSecurityAnswerRequiredMessage");

            lnkAvatarUpld.Text = ResourceHelper.GetResourceString("Resource", "UploadAvatarLink");
            lnkAvatarUpld.ToolTip = ResourceHelper.GetResourceString("Resource", "UploadAvatarLink");

            btnUpdateAvartar.ImageUrl = Page.ResolveUrl("~/Data/SiteImages/1x1.gif");
            btnUpdateAvartar.Attributes.Add("tabIndex", "-1");

            if (allowGravatars)
            {
                lnkAvatarUpld.Visible = false;
            }
            else
            {
                if (disableAvatars)
                {
                    divAvatar.Visible = false;
                    lnkAvatarUpld.Visible = false;
                }
            }

            lnkOpenIDUpdate.Text = ResourceHelper.GetResourceString("Resource", "OpenIDUpdateButton");
            lnkOpenIDUpdate.ToolTip = ResourceHelper.GetResourceString("Resource", "OpenIDUpdateButton");
            lnkOpenIDUpdate.NavigateUrl = SiteRoot + "/Secure/UpdateOpenID.aspx";

            rpxLink.OverrideText = ResourceHelper.GetResourceString("Resource", "OpenIDUpdateButton");

            pnlSecurityQuestion.Visible = siteSettings.RequiresQuestionAndAnswer;
        }

        private void btnUpdateAvartar_Click(object sender, System.Web.UI.ImageClickEventArgs e)
        {
            if ((siteUser != null) && siteUser.AvatarUrl.Length > 0)
            {
                FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                if (p != null)
                {
                    IFileSystem fileSystem = p.GetFileSystem();
                    string avatarBasePath = "~/Data/Sites/" + siteSettings.SiteId.ToInvariantString() + "/useravatars/";
                    WebFile avatarFile = fileSystem.RetrieveFile(avatarBasePath + siteUser.AvatarUrl);
                    if (avatarFile != null)
                    {
                        if (avatarFile.Modified > DateTime.Today)
                        {
                            string newfileName = "user"
                                + siteUser.UserId.ToInvariantString()
                                + "-" + siteUser.Name.ToCleanFileName()
                                + "-" + DateTime.UtcNow.Millisecond.ToInvariantString()
                                + System.IO.Path.GetExtension(siteUser.AvatarUrl);

                            fileSystem.MoveFile(avatarBasePath + siteUser.AvatarUrl, avatarBasePath + newfileName, true);
                            siteUser.AvatarUrl = newfileName;
                            siteUser.Save();
                        }
                    }
                }
            }

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void SetupAvatarScript()
        {
            StringBuilder script = new StringBuilder();

            script.Append("<script type=\"text/javascript\">");
            string fancyBoxConfig = "{type:'iframe', width:'60%', title:{type:'outside'}, afterClose:CBCallback }";
            script.Append("$('#" + lnkAvatarUpld.ClientID + "').fancybox(" + fancyBoxConfig + "); ");

            script.Append("function CBCallback() {");

            script.Append("var btn = document.getElementById('" + this.btnUpdateAvartar.ClientID + "');  ");
            script.Append("btn.click(); ");

            script.Append("}");

            script.Append("</script>");

            this.Page.ClientScript.RegisterStartupScript(this.GetType(), "cbupinit", script.ToString());
        }
    }
}
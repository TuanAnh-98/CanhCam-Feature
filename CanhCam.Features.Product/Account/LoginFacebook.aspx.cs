using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Business.WebHelpers.UserRegisteredHandlers;
using CanhCam.Net;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace CanhCam.Web.AccountUI
{
    public partial class LoginFacebook : CmsDialogBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginFacebook));
        private string facebookAppId = string.Empty;
        private string facebookAppSecret = string.Empty;
        private string redirectUri = string.Empty;
        private string facebookCode = string.Empty;

        private void Page_Load(object sender, System.EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();
            LoadSettings();

            if (
                facebookAppId.Length == 0
                || facebookAppSecret.Length == 0
                || redirectUri.Length == 0
                )
            {
                FailureText.Text = "Không thể đăng nhập bằng Facebook vì chưa được cấu hình.";
                //SiteUtils.RedirectToHomepage();
                return;
            }

            var facebookVersion = "v2.7";
            var url = string.Format("https://www.facebook.com/{0}/dialog/oauth?client_id={1}&redirect_uri={2}&state={3}&sdk=php-sdk-4.0.23&scope=email,public_profile", facebookVersion, facebookAppId, redirectUri, Guid.NewGuid().ToString().ToLower());
            if (facebookCode.Length == 0)
            {
                WebUtils.SetupRedirect(this, url);
                return;
            }

            // Get access token info
            string accessTokenUri = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}", facebookAppId, redirectUri, facebookAppSecret, facebookCode);
            try
            {
                string response = HttpGet(accessTokenUri);
                if (response.Length > 0)
                {
                    FacebookResponse facebookResponse = new JavaScriptSerializer().Deserialize<FacebookResponse>(response);
                    // Get access token
                    if (facebookResponse.access_token != null)
                    {
                        string accessToken = facebookResponse.access_token;
                        // Get clientNo infomation
                        var userData = HttpGet(string.Format("https://graph.facebook.com/{0}/me?access_token={1}", facebookVersion, accessToken));
                        var oUser = new JavaScriptSerializer().Deserialize<FaceBookUser>(userData);

                        //string resPic = HttpGet("http://graph.facebook.com/" + oUser.Id + "/picture?type=large");
                        //log.Info("LoginFacebook: " + userData);
                        //log.Info("LoginFacebook: " + oUser.Email);
                        //log.Info("LoginFacebook: " + resPic);


                        if (oUser != null)
                        {
                            var userGuid = Guid.Empty;
                            if (oUser.Id.Length > 0)
                                userGuid = SiteUser.GetUserGuidFromOpenId(siteSettings.SiteId, oUser.Id);
                            SiteUser user = null;
                            if (userGuid != Guid.Empty)
                            {
                                user = new SiteUser(siteSettings, userGuid);
                                if (user == null || user.UserId <= 0)
                                    user = CreateUser(oUser);
                            }
                            else
                                user = CreateUser(oUser);

                            if (user != null && user.UserId > 0 && user.ApprovedForLogin)
                            {
                                DoUserLogin(user);
                                SetupScripts();
                            }
                            else
                            {
                                RedirectToUpdatePassword(oUser.Id);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            //// Get access token info
            //string accessTokenUri = string.Format("https://graph.facebook.com/oauth/access_token?client_id={0}&redirect_uri={1}&client_secret={2}&code={3}", facebookAppId, redirectUri, facebookAppSecret, facebookCode);
            //try
            //{
            //    string response = HttpGet(accessTokenUri);
            //    if (response.Length > 0)
            //    {
            //        // Get access token
            //        string responseToken = response.Split('&')[0];
            //        if (responseToken.Contains("access_token="))
            //        {
            //            string accessToken = responseToken.Replace("access_token=", "");

            //            // Get clientNo infomation
            //            var userData = HttpGet(string.Format("https://graph.facebook.com/{0}/me?access_token={1}", facebookVersion, accessToken));
            //            var oUser = new JavaScriptSerializer().Deserialize<FaceBookUser>(userData);

            //            if (oUser != null)
            //            {
            //                var userGuid = Guid.Empty;
            //                if (oUser.Id.Length > 0)
            //                    userGuid = SiteUser.GetUserGuidFromOpenId(siteSettings.SiteId, oUser.Id);
            //                SiteUser user = null;
            //                if (userGuid != Guid.Empty)
            //                {
            //                    user = new SiteUser(siteSettings, userGuid);
            //                    if (user == null || user.UserId <= 0)
            //                        user = CreateUser(oUser);
            //                }
            //                else
            //                    user = CreateUser(oUser);

            //                if (user != null && user.UserId > 0 && user.ApprovedForLogin)
            //                {
            //                    DoUserLogin(user);
            //                    SetupScripts();
            //                }
            //                else
            //                {
            //                    RedirectToUpdatePassword(oUser.Id);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex.Message);
            //}
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

        private void RedirectToUpdatePassword(string facebookId)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/Account/UpdatePassword.aspx?facebookid=" + facebookId);
        }

        private string HttpGet(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    return webClient.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return string.Empty;
        }

        private SiteUser CreateUser(FaceBookUser oUser)
        {
            if (string.IsNullOrEmpty(oUser.Email) || !Email.IsValidEmailAddressSyntax(oUser.Email))
                oUser.Email = oUser.Id + "@facebook.com";

            if (SiteUser.EmailExistsInDB(siteSettings.SiteId, oUser.Email))
            {
                var user = new SiteUser(siteSettings, oUser.Email);
                if (user != null && user.UserId > 0)
                {
                    if (string.IsNullOrEmpty(user.OpenIdUri))
                    {
                        user.OpenIdUri = oUser.Id;
                        //if (oUser.Email != user.Email)
                        //    user.ApprovedForLogin = true;
                        user.Save();
                    }

                    return user;
                }
            }

            SiteUser newUser = new SiteUser(siteSettings)
            {
                Email = oUser.Email
            };
            newUser.LoginName = SiteUtils.SuggestLoginNameFromEmail(siteSettings.SiteId, newUser.Email);
            newUser.Name = newUser.LoginName;
            gbMembershipProvider gbMembership = (gbMembershipProvider)Membership.Provider;
            newUser.Password = gbMembership.EncodePassword(siteSettings, newUser, SiteUser.CreateRandomPassword(7, WebConfigSettings.PasswordGeneratorChars));

            newUser.ApprovedForLogin = false;
            newUser.OpenIdUri = oUser.Id; //"http://www.facebook.com/profile.php?id=" + oUser.Id;
            if (!string.IsNullOrEmpty(oUser.First_Name))
                newUser.FirstName = oUser.First_Name;
            if (!string.IsNullOrEmpty(oUser.Last_Name))
            {
                newUser.FirstName = (oUser.Last_Name + " " + newUser.FirstName).Trim();
                //newUser.LastName = oUser.Last_Name;
            }

            if (string.IsNullOrEmpty(newUser.FirstName) && !string.IsNullOrEmpty(oUser.Name))
                newUser.FirstName = oUser.Name;

            //if (!string.IsNullOrEmpty(oUser.Bio))
            //    newUser.AuthorBio = oUser.Bio;
            if (!string.IsNullOrEmpty(oUser.Gender))
            {
                switch (oUser.Gender.ToLower())
                {
                    case "male":
                    case "nam":
                        newUser.Gender = "M";
                        break;

                    case "female":
                    case "nữ":
                        newUser.Gender = "F";
                        break;
                }

                //newUser.Gender = oUser.Gender.ToUpper();
            }
            //if (!string.IsNullOrEmpty(oUser.Link))
            //    newUser.WebSiteUrl = oUser.Link;
            //newUser.AvatarUrl = string.Format("https://graph.facebook.com/{0}/picture?width=160&height=160", oUser.Id);

            newUser.Save();

            //// track clientNo ip address
            //UserLocation userLocation = new UserLocation(newUser.UserGuid, SiteUtils.GetIP4Address());
            //userLocation.SiteGuid = siteSettings.SiteGuid;
            //userLocation.Hostname = Page.Request.UserHostName;
            //userLocation.Save();

            UserRegisteredEventArgs u = new UserRegisteredEventArgs(newUser);
            OnUserRegistered(u);

            CacheHelper.ClearMembershipStatisticsCache();

            return newUser;
        }

        protected void OnUserRegistered(UserRegisteredEventArgs e)
        {
            foreach (UserRegisteredHandlerProvider handler in UserRegisteredHandlerProviderManager.Providers)
            {
                handler.UserRegisteredHandler(null, e);
            }
        }

        private void LoadSettings()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, "Đăng nhập bằng tài khoản Facebook");

            facebookAppId = WebConfigSettings.FacebookAppId;
            facebookAppSecret = WebConfigSettings.FacebookAppSecret;
            redirectUri = HttpUtility.UrlEncode(SiteUtils.GetNavigationSiteRoot() + "/Account/LoginFacebook.aspx");

            // Get productCode value
            facebookCode = WebUtils.ParseStringFromQueryString("code", string.Empty);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        public class FaceBookUser
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string First_Name { get; set; }
            public string Last_Name { get; set; }
            public string Bio { get; set; }
            public string Gender { get; set; }
            public string Link { get; set; }
        }

        public class FacebookResponse
        {
            public string access_token;
            public string token_type;
            public int expires_in;
        }

    }
}
using System;
using System.Web.Security;
using CanhCam.Business;
using CanhCam.Web.Framework;
using System.Web.Script.Serialization;
using log4net;
using CanhCam.Business.WebHelpers;
using CanhCam.Business.WebHelpers.UserRegisteredHandlers;
using CanhCam.Web.Configuration;
using CanhCam.Net;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization;

namespace CanhCam.Web.AccountUI
{
    public partial class LoginGooglePlus : CmsDialogBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LoginGooglePlus));
        private string googleClientId = string.Empty;
        private string googleSecret = string.Empty;
        private string redirectUri = string.Empty;
        private string code = string.Empty;

        private void Page_Load(object sender, System.EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();
            LoadSettings();

            if (
                googleClientId.Length == 0
                || googleSecret.Length == 0
                || redirectUri.Length == 0
                )
            {
                FailureText.Text = "Không thể đăng nhập bằng Google vì chưa được cấu hình.";
                //SiteUtils.RedirectToHomepage();
                return;
            }

            var url = string.Format("https://accounts.google.com/o/oauth2/auth?scope={0}&state=1&redirect_uri={1}&client_id={2}&response_type=code&approval_prompt=auto&access_type=online",
                Server.UrlEncode("https://www.googleapis.com/auth/userinfo.email https://www.googleapis.com/auth/userinfo.profile"),
                redirectUri,
                googleClientId);

            if (code.Length == 0)
            {
                WebUtils.SetupRedirect(this, url);
                return;
            }

            try
            {
                var tokenUrl = "https://accounts.google.com/o/oauth2/token";
                var postData = string.Format("code={0}&client_id={1}&client_secret={2}&redirect_uri={3}&grant_type=authorization_code", code, googleClientId, googleSecret, redirectUri);
                string response = HttpPost(tokenUrl, postData);
                if (response.Length > 0)
                {
                    // Get access token
                    var oToken = (new JavaScriptSerializer()).Deserialize<GoogleToken>(response);
                    string profileUrl = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + oToken.access_token;
                    string result = HttpGet(profileUrl);

                    //and Deserialize the JSON response
                    var oUser = (new JavaScriptSerializer()).Deserialize<GoogleUser>(result);
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
                            RedirectToUpdatePassword(oUser.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
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

        private void RedirectToUpdatePassword(string googleId)
        {
            WebUtils.SetupRedirect(this, SiteRoot + "/Account/UpdatePassword.aspx?googleid=" + googleId);
        }

        private string HttpGet(string url)
        {
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    //wc.Headers.Add("Authorization", "OAuth " + accessToken);
                    return webClient.DownloadString(url);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return string.Empty;
        }

        private string HttpPost(string url, string postData)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();
                var readStream = response.GetResponseStream();
                var responseString = new StreamReader(readStream).ReadToEnd();

                response.Close();
                readStream.Close();

                return responseString;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return string.Empty;
        }

        private SiteUser CreateUser(GoogleUser oUser)
        {
            if (string.IsNullOrEmpty(oUser.Email) || !Email.IsValidEmailAddressSyntax(oUser.Email))
                oUser.Email = oUser.Id + "@google.com";

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
            newUser.OpenIdUri = oUser.Id; //"https://plus.google.com/" + oUser.Id;
            newUser.WindowsLiveId = "google";
            if (!string.IsNullOrEmpty(oUser.Given_Name))
                newUser.FirstName = oUser.Given_Name;
            if (!string.IsNullOrEmpty(oUser.Family_Name))
            {
                newUser.FirstName = (oUser.Family_Name + " " + newUser.FirstName).Trim();
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
            Title = SiteUtils.FormatPageTitle(siteSettings, "Đăng nhập bằng tài khoản Google plus");

            googleClientId = ConfigHelper.GetStringProperty("GoogleClientId", string.Empty);
            googleSecret = ConfigHelper.GetStringProperty("GoogleSecret", string.Empty);
            redirectUri = Server.UrlEncode(SiteUtils.GetNavigationSiteRoot() + "/Account/LoginGooglePlus.aspx");

            // Get productCode value
            code = WebUtils.ParseStringFromQueryString("code", string.Empty);
        }

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new System.EventHandler(this.Page_Load);
        }

        //[DataContract]
        public class GoogleToken
        {
            //[DataMember(Name = "access_token")]
            //public string AccessToken { get; set; }

            public string access_token { get; set; }
        }

        public class GoogleUser
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Given_Name { get; set; }
            public string Family_Name { get; set; }
            public string Picture { get; set; }
            public string Gender { get; set; }
            public string Locale { get; set; }
        }

    }
}

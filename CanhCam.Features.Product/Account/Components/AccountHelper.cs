using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System.Web;

namespace CanhCam.Web.AccountUI
{
    public static class AccountHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AccountHelper));

        //public static string GetWelcomeMessageFormat(SiteUser siteUser)
        //{
        //    var format = ResourceHelper.GetResourceString("AccountResources", "WelcomeMessageFormat");
        //    format = format.Replace("{Username}", HttpUtility.HtmlEncode(siteUser.LoginName));
        //    format = format.Replace("{DisplayName}", HttpUtility.HtmlEncode(siteUser.Name));
        //    format = format.Replace("{Email}", HttpUtility.HtmlEncode(siteUser.Email));
        //    if (siteUser.FirstName.Length > 0 || siteUser.LastName.Length > 0)
        //    {
        //        format = format.Replace("{FirstName}", HttpUtility.HtmlEncode(siteUser.FirstName));
        //        format = format.Replace("{LastName}", HttpUtility.HtmlEncode(siteUser.LastName));
        //    }
        //    else
        //    {
        //        format = format.Replace("{FirstName}", HttpUtility.HtmlEncode(siteUser.Name));
        //        format = format.Replace("{LastName}", string.Empty);
        //    }

        //    return format;
        //}

        public static bool IsAllowedAddNewAddress(int userId)
        {
            var lstAddress = UserAddress.GetByUser(userId);
            if (lstAddress.Count >= ConfigHelper.GetIntProperty("MaximumAddressPerUser", 12))
                return false;

            return true;
        }

        public static string GetIsDefaultText(bool isDefault)
        {
            if (isDefault)
                return "Mặc định";

            return string.Empty;
        }

        public static string GetFullNameFormat(SiteUser siteUser)
        {
            if (siteUser.FirstName.Length > 0 || siteUser.LastName.Length > 0)
                return string.Format("{0} {1}", HttpUtility.HtmlEncode(siteUser.LastName), HttpUtility.HtmlEncode(siteUser.FirstName)).Trim();

            return HttpUtility.HtmlEncode(siteUser.Name);
        }

        public static UserAddress GetDefaultUserAddress(int userId)
        {
            var lst = UserAddress.GetByUser(userId, -1);
            if (lst.Count > 0)
                return lst[0];

            return null;
        }
    }
}
using CanhCam.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CanhCam.Web.CustomUI
{
    public class AffiliatePermission
    {
        public const string ViewList = "Permissions.PhongBan.ViewList";
        public const string Create = "Permissions.PhongBan.Create";
        public const string Update = "Permissions.PhongBan.Update";
        public const string Delete = "Permissions.PhongBan.Delete";

        public static bool CanViewList
        {
            get { return SiteUtils.UserHasPermission(ViewList); }
        }
        public static bool CanCreate
        {
            get { return SiteUtils.UserHasPermission(Create); }
        }
        public static bool CanUpdate
        {
            get { return SiteUtils.UserHasPermission(Update); }
        }
        public static bool CanDelete
        {
            get { return SiteUtils.UserHasPermission(Delete); }
        }

    }
}
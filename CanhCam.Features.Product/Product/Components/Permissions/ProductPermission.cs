/// Created:				2014-06-23
/// Last Modified:			2014-08-25

namespace CanhCam.Web.ProductUI
{
    public static class ProductPermission
    {
        public const string StaffRoleName = "Store Admin";

        public const string ViewList = "Permissions.Product.ViewList";
        public const string Create = "Permissions.Product.Create";
        public const string Update = "Permissions.Product.Update";
        public const string Delete = "Permissions.Product.Delete";
        public const string Import = "Permissions.Product.Import";

        public const string ManageTags = "Permissions.Product.ManageTags";
        public const string ManageOrders = "Permissions.Product.ManageOrders";
        public const string DeleteOrders = "Permissions.Product.DeleteOrders";

        public const string DeleteComment = "Permissions.Product.DeleteComment";
        public const string ApproveComment = "Permissions.Product.ApproveComment";
        public const string UpdateComment = "Permissions.Product.UpdateComment";
        public const string ManageComment = "Permissions.Product.ManageComment";

        public const string ManageManufacturer = "Permissions.Product.ManageManufacturer";

        public static bool CanImport => SiteUtils.UserHasPermission(Import);
        public static bool CanViewList => SiteUtils.UserHasPermission(ViewList);

        public static bool CanCreate => SiteUtils.UserHasPermission(Create);

        public static bool CanUpdate => SiteUtils.UserHasPermission(Update);

        public static bool CanDelete => SiteUtils.UserHasPermission(Delete);

        public static bool CanManageTags => SiteUtils.UserHasPermission(ManageTags);

        public static bool CanManageOrders => SiteUtils.UserHasPermission(ManageOrders);
        public static bool CanDeleteOrders => SiteUtils.UserHasPermission(DeleteOrders);

        public static bool CanDeleteComment => SiteUtils.UserHasPermission(DeleteComment);

        public static bool CanApproveComment => SiteUtils.UserHasPermission(ApproveComment);

        public static bool CanUpdateComment => SiteUtils.UserHasPermission(UpdateComment);

        public static bool CanManageComment => SiteUtils.UserHasPermission(ManageComment);

        public static bool CanManageManufacturer => SiteUtils.UserHasPermission(ManageManufacturer);
    }
}
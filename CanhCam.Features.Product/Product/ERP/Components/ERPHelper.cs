using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public static class ERPHelper
    {
        public static string CustomerCodeKey = "ERPCustomerCode";
        public static bool EnableERP = ConfigHelper.GetBoolProperty("ERP:Enable", false);
        public static bool DisabledChooseStatusERP = ConfigHelper.GetBoolProperty("ERP:DisabledChooseStatusERP", true);
        public static bool RewardPointEnableERP = ConfigHelper.GetBoolProperty("ERP:RewardPointEnable", false);
        public static bool EnableValidVoucherInERP = ConfigHelper.GetBoolProperty("ERP:EnableValidVoucherInERP", false);
        public static bool EnableValidCouponInERP = ConfigHelper.GetBoolProperty("ERP:EnableValidCouponInERP", false);
        public static bool AutoSyncWhenOrderCompleted = ConfigHelper.GetBoolProperty("ERP:AutoSyncWhenOrderCompleted", false);
        private static readonly ILog log = LogManager.GetLogger(typeof(ERPHelper));

        #region RewardPoint

        public static void AddRewardPointsHistory(SiteUser user, Order order, Store store, int points)
        {
            if (!RewardPointEnableERP) return;
        }
        public static decimal GetRewardPointsBalance(SiteUser user, ref decimal pointsAvailable)
        {
            pointsAvailable = 0;
            return 0;
        }
        #endregion RewardPoint

        #region Member

        public static bool SysnMember(SiteUser siteUser)
        {
            return false;
        }

        #endregion Member

        #region Order

        public static bool SendOrder(Order order, Store store, List<OrderItem> orderItems, List<Product> products, ref string message)
        {
            return false;
        }

        public static void CancelOrder(Order order, Store store, ref string message)
        {
            message = string.Empty;
        }

        #endregion Order

        #region Coupon

        public static void UpdateCouponStatus(string couponCode, bool isActive, string orderCode = "")
        {
        }

        public static bool CouponIsValid(string couponCode, out string warning)
        {
            warning = string.Empty;

            return false;
        }

        #endregion Coupon

        #region Vouchers

        public static void UpdateVoucherStatus(string voucherCode, bool isActive, string orderCode = "")
        {
        }

        public static bool VoucherIsValid(string voucherCode, out string warning)
        {
            warning = string.Empty;

            return false;
        }

        #endregion Vouchers
    }
}
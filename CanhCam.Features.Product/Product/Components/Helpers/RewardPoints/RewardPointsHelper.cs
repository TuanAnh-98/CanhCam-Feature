using CanhCam.Business;
using CanhCam.Web.Framework;
using System;

namespace CanhCam.Web.ProductUI
{
    public class RewardPointsHelper
    {
        public static bool Enable = ConfigHelper.GetBoolProperty("RewardPoints:Enable", false);
        public static int ExchangeRate = ConfigHelper.GetIntProperty("RewardPoints:ExchangeRate", 1);
        public static int MinimumRewardPointsToUse = ConfigHelper.GetIntProperty("RewardPoints:MinimumRewardPointsToUse", 0);
        public static int PointsForPurchases_Amount = ConfigHelper.GetIntProperty("RewardPoints:PointsForPurchasesAmount", 1);
        public static int PointsForPurchases_Points = ConfigHelper.GetIntProperty("RewardPoints:PointsForPurchasesPoints", 1);
        public static int PointsForPurchases_Awarded = ConfigHelper.GetIntProperty("RewardPoints:PointsForPurchases_Awarded", (int)OrderStatus.Completed);
        public static bool ReduceRewardPointsAfterCancelOrder = ConfigHelper.GetBoolProperty("RewardPoints:ReduceRewardPointsAfterCancelOrder", true);

        public static decimal ConvertRewardPointsToAmount(int rewardPoints)
        {
            if (rewardPoints <= 0)
                return decimal.Zero;

            var result = rewardPoints * ExchangeRate;
            return result;
        }

        public static int CalculateRewardPoints(SiteUser siteUser, decimal amount)
        {
            if (!Enable)
                return 0;

            if (amount <= decimal.Zero)
                return 0;

            if (siteUser == null)
                return 0;

            var points = (int)Math.Truncate(amount / PointsForPurchases_Amount * PointsForPurchases_Points);
            return points;
        }

        public static void GetRewardPointsBalance(SiteUser user, out decimal points, out decimal pointsAvailable)
        {
            points = 0;
            pointsAvailable = 0;
            if (!Enable)
                return;
            if (ERPHelper.EnableERP && ERPHelper.RewardPointEnableERP)
            {
                points = ERPHelper.GetRewardPointsBalance(user, ref pointsAvailable);
                return;
            }
            points = RewardPointsHistory.GetRewardPointsBalance(user.UserId);
            pointsAvailable = RewardPointsHistory.GetRewardPointsBalance(user.UserId);
        }

        public static RewardPointsHistory AddRewardPointsHistory(int siteId, SiteUser user, int type, int points, string message = "",
           int usedWithOrderId = -1, decimal usedAmount = 0M)
        {
            GetRewardPointsBalance(user, out decimal rewardPointsBalance, out decimal pointsAvallable);
            var rewardPointsHistory = new RewardPointsHistory
            {
                SiteId = siteId,
                UserId = user.UserId,
                Type = type,
                UsedWithOrderId = usedWithOrderId,
                Points = points,
                PointsBalance = Convert.ToInt32(rewardPointsBalance) + points,
                UsedAmount = usedAmount,
                Message = message,
                CreatedOnUtc = DateTime.UtcNow
            };

            rewardPointsHistory.Save();

            return rewardPointsHistory;
        }
    }
}
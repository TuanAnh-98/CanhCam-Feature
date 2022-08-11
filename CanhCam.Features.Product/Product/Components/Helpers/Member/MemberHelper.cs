using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public static class MemberHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MemberHelper));

        public static string GetMemberRank(int userId, ListItem item = null)
        {
            SiteUser user = new SiteUser(CacheHelper.GetCurrentSiteSettings(), userId);
            var totalApprovedPoint = UserPoint.GetTotalApprovedPointByUser(userId);
            MemberRank rank = new MemberRank(user, totalApprovedPoint);
            if ((!string.IsNullOrEmpty(user.ICQ) && Convert.ToInt32(user.ICQ) > rank.RankOrder) || (rank == null && !string.IsNullOrEmpty(user.ICQ)))
                rank = new MemberRank(WorkingCulture.LanguageId, Convert.ToInt32(user.ICQ));
            if (!(string.IsNullOrEmpty(rank.Name)) && (item != null && (item.Value == "-1" || item.Text == rank.Name)))
            {
                return rank.Name;
            }
            return string.Empty;
        }

        public static string GetMemberRank(SiteUser user)
        {
            var totalApprovedPoint = UserPoint.GetTotalApprovedPointByUser(user.UserId);
            MemberRank rank = new MemberRank(user, totalApprovedPoint);
            if ((!string.IsNullOrEmpty(user.ICQ) && Convert.ToInt32(user.ICQ) > rank.RankOrder) || (rank == null && !string.IsNullOrEmpty(user.ICQ)))
                rank = new MemberRank(WorkingCulture.LanguageId, Convert.ToInt32(user.ICQ));
            return rank.Name;
        }

        public static void ReorderMemberRankOrderList()
        {
            var lstMemberRank = MemberRank.GetAll().OrderBy(m => m.RankOrder).ToList();

            for (int i = 1; i <= lstMemberRank.Count; i++)
            {
                if (lstMemberRank[i - 1].RankOrder != i)
                {
                    lstMemberRank[i - 1].RankOrder = i;
                    lstMemberRank[i - 1].Save();
                }
            }
        }

        public static void RearrangeMemberPointRange()
        {
            var lstMemberRank = MemberRank.GetAll().OrderBy(m => m.RankOrder).ToList();
            for (int i = lstMemberRank.Count; i > 0; i--)
            {
                if (i == lstMemberRank.Count)
                    lstMemberRank[i - 1].Note = "-1";
                else lstMemberRank[i - 1].Note = (lstMemberRank[i].Point - 1).ToString();
                lstMemberRank[i - 1].Save();
            }
        }

        public static int GetMaxRank()
        {
            var lstTierPrice = MemberRank.GetAll();
            return GetMaxRank(lstTierPrice);
        }

        public static int GetMaxRank(List<MemberRank> lstMemberRank)
        {
            int rank = 0;
            foreach (MemberRank memberRank in lstMemberRank)
            {
                if (rank < memberRank.RankOrder)
                    rank = memberRank.RankOrder;
            }
            return rank;
        }

        public static bool CheckValidRankPoint(MemberRank memberRank)
        {
            MemberRank lowerRank = new MemberRank(WorkingCulture.LanguageId, memberRank.RankOrder - 1);
            MemberRank upperRank = new MemberRank(WorkingCulture.LanguageId, memberRank.RankOrder + 1);
            if (!(upperRank != null && upperRank.Id > 0) && (lowerRank != null && lowerRank.Id > 0)) // upperRank not exist, lowerRank does exist
            {
                return memberRank.Point > lowerRank.Point ? true : false;
            }
            else if (!(lowerRank != null && lowerRank.Id > 0) && (upperRank != null && upperRank.Id > 0))  // lowerRank not exist, upperRank does exist
            {
                //return memberRank.Point > 0 ? true : false;
                return memberRank.Point < upperRank.Point ? true : false;
            }
            else if ((upperRank != null && upperRank.Id > 0) && (lowerRank != null && lowerRank.Id > 0))
            {
                return (memberRank.Point > lowerRank.Point && memberRank.Point < upperRank.Point) ? true : false;
            }
            else if (lowerRank != null && lowerRank.Id > 0)
            {
                return memberRank.Point > lowerRank.Point ? true : false;
            }

            return memberRank.Point > 0 ? true : false;
        }
    }
}
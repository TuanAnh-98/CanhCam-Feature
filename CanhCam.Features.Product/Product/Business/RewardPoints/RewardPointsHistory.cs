// Author:					Canh cam
// Created:					2021-1-18
// Last Modified:			2021-1-18

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum RewardPointType
    {
        Earned = 0,
        Reduce = 10,
        Redeemed = 20
    }
    public class RewardPointsHistory
    {
        #region Constructors

        public RewardPointsHistory()
        { }

        public RewardPointsHistory(
            int rowId)
        {
            this.GetRewardPointsHistory(
                rowId);
        }

        #endregion Constructors

        #region Private Properties

        private int rowId = -1;
        private int userId = -1;
        private int usedWithOrderId = -1;
        private int siteId = -1;
        private int points = -1;
        private int pointsBalance = -1;
        private int type = -1;
        private decimal usedAmount;
        private string message = string.Empty;
        private DateTime createdOnUtc = DateTime.UtcNow;
        private DateTime? availableStartDateTimeUtc = null;
        private DateTime? availableEndDateTimeUtc = null;
        private DateTime? approvedUtc = null;
        private Guid approvedUserGuid = Guid.Empty;
        private string approvedBy = string.Empty;


        //Extra Filed not in table
        private string orderCode = string.Empty;
        private string customerName = string.Empty;
        private string customerEmail = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int UsedWithOrderId
        {
            get { return usedWithOrderId; }
            set { usedWithOrderId = value; }
        }

        public int SiteId
        {
            get { return siteId; }
            set { siteId = value; }
        }

        public int Points
        {
            get { return points; }
            set { points = value; }
        }

        public int PointsBalance
        {
            get { return pointsBalance; }
            set { pointsBalance = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public decimal UsedAmount
        {
            get { return usedAmount; }
            set { usedAmount = value; }
        }

        public string Message
        {
            get { return message; }
            set { message = value; }
        }

        public DateTime CreatedOnUtc
        {
            get { return createdOnUtc; }
            set { createdOnUtc = value; }
        }

        public DateTime? AvailableStartDateTimeUtc
        {
            get { return availableStartDateTimeUtc; }
            set { availableStartDateTimeUtc = value; }
        }

        public DateTime? AvailableEndDateTimeUtc
        {
            get { return availableEndDateTimeUtc; }
            set { availableEndDateTimeUtc = value; }
        }

        public DateTime? ApprovedUtc
        {
            get { return approvedUtc; }
            set { approvedUtc = value; }
        }

        public Guid ApprovedUserGuid
        {
            get { return approvedUserGuid; }
            set { approvedUserGuid = value; }
        }

        public string ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        public string OrderCode
        {
            get { return orderCode; }
            set { orderCode = value; }
        }
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }
        public string CustomerEmail
        {
            get { return customerEmail; }
            set { customerEmail = value; }
        }
        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of RewardPointsHistory.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        private void GetRewardPointsHistory(
            int rowId)
        {
            using (IDataReader reader = DBRewardPointsHistory.GetOne(
                rowId))
            {
                if (reader.Read())
                {
                    this.rowId = Convert.ToInt32(reader["RowId"]);
                    this.userId = Convert.ToInt32(reader["UserId"]);
                    this.usedWithOrderId = Convert.ToInt32(reader["UsedWithOrderId"]);
                    this.siteId = Convert.ToInt32(reader["SiteId"]);
                    this.points = Convert.ToInt32(reader["Points"]);
                    this.pointsBalance = Convert.ToInt32(reader["PointsBalance"]);
                    this.type = Convert.ToInt32(reader["Type"]);
                    this.usedAmount = Convert.ToDecimal(reader["UsedAmount"]);
                    this.message = reader["Message"].ToString();
                    this.createdOnUtc = Convert.ToDateTime(reader["CreatedOnUtc"]);
                    if (reader["AvailableStartDateTimeUtc"] != DBNull.Value)
                        this.availableStartDateTimeUtc = Convert.ToDateTime(reader["AvailableStartDateTimeUtc"]);
                    if (reader["AvailableEndDateTimeUtc"] != DBNull.Value)
                        this.availableEndDateTimeUtc = Convert.ToDateTime(reader["AvailableEndDateTimeUtc"]);
                    if (reader["ApprovedUtc"] != DBNull.Value)
                        this.approvedUtc = Convert.ToDateTime(reader["ApprovedUtc"]);
                    this.approvedUserGuid = new Guid(reader["ApprovedUserGuid"].ToString());
                    this.approvedBy = reader["ApprovedBy"].ToString();
                }
            }
        }

        /// <summary>
        /// Persists a new instance of RewardPointsHistory. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBRewardPointsHistory.Create(
                this.userId,
                this.usedWithOrderId,
                this.siteId,
                this.points,
                this.pointsBalance,
                this.type,
                this.usedAmount,
                this.message,
                this.createdOnUtc,
                this.availableStartDateTimeUtc,
                this.availableEndDateTimeUtc,
                this.approvedUtc,
                this.approvedUserGuid,
                this.approvedBy);

            this.rowId = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of RewardPointsHistory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBRewardPointsHistory.Update(
                this.rowId,
                this.userId,
                this.usedWithOrderId,
                this.siteId,
                this.points,
                this.pointsBalance,
                this.type,
                this.usedAmount,
                this.message,
                this.createdOnUtc,
                this.availableStartDateTimeUtc,
                this.availableEndDateTimeUtc,
                this.approvedUtc,
                this.approvedUserGuid,
                this.approvedBy);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of RewardPointsHistory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowId > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Create();
            }
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of RewardPointsHistory. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            return DBRewardPointsHistory.Delete(
                rowId);
        }

        /// <summary>
        /// Gets a count of RewardPointsHistory.
        /// </summary>
        public static int GetCount()
        {
            return DBRewardPointsHistory.GetCount();
        }

        private static List<RewardPointsHistory> LoadListFromReader(IDataReader reader)
        {
            List<RewardPointsHistory> rewardPointsHistoryList = new List<RewardPointsHistory>();
            try
            {
                while (reader.Read())
                {
                    RewardPointsHistory rewardPointsHistory = new RewardPointsHistory
                    {
                        rowId = Convert.ToInt32(reader["RowId"]),
                        userId = Convert.ToInt32(reader["UserId"]),
                        usedWithOrderId = Convert.ToInt32(reader["UsedWithOrderId"]),
                        siteId = Convert.ToInt32(reader["SiteId"]),
                        points = Convert.ToInt32(reader["Points"]),
                        pointsBalance = Convert.ToInt32(reader["PointsBalance"]),
                        type = Convert.ToInt32(reader["Type"]),
                        usedAmount = Convert.ToDecimal(reader["UsedAmount"]),
                        message = reader["Message"].ToString(),
                        createdOnUtc = Convert.ToDateTime(reader["CreatedOnUtc"])
                    };
                    if (reader["AvailableStartDateTimeUtc"] != DBNull.Value)
                        rewardPointsHistory.availableStartDateTimeUtc = Convert.ToDateTime(reader["AvailableStartDateTimeUtc"]);
                    if (reader["AvailableEndDateTimeUtc"] != DBNull.Value)
                        rewardPointsHistory.availableEndDateTimeUtc = Convert.ToDateTime(reader["AvailableEndDateTimeUtc"]);
                    if (reader["ApprovedUtc"] != DBNull.Value)
                        rewardPointsHistory.approvedUtc = Convert.ToDateTime(reader["ApprovedUtc"]);
                    rewardPointsHistory.approvedUserGuid = new Guid(reader["ApprovedUserGuid"].ToString());
                    rewardPointsHistory.approvedBy = reader["ApprovedBy"].ToString();

                    if (reader["OrderCode"] != DBNull.Value)
                        rewardPointsHistory.orderCode = reader["OrderCode"].ToString();
                    if (reader["CustomerName"] != DBNull.Value)
                        rewardPointsHistory.customerName = reader["CustomerName"].ToString();
                    if (reader["CustomerEmail"] != DBNull.Value)
                        rewardPointsHistory.customerEmail = reader["CustomerEmail"].ToString();

                    rewardPointsHistoryList.Add(rewardPointsHistory);
                }
            }
            finally
            {
                reader.Close();
            }

            return rewardPointsHistoryList;
        }
        public static int GetCountBySearch(
            int siteId = -1,
            int userId = -1,
            int type = -1,
            string keyword = "",
            DateTime? startDate = null,
            DateTime? endDate = null)
        {
            return DBRewardPointsHistory.GetCountBySearch(siteId, userId, type, keyword, startDate, endDate);
        }

        public static List<RewardPointsHistory> GetPageBySearch(
            int siteId = -1,
            int userId = -1,
            int type = -1,
            string keyword = "",
            DateTime? startDate = null,
            DateTime? endDate = null,
            int pageNumber = 1,
            int pageSize = 414736)
        {
            IDataReader reader = DBRewardPointsHistory.GetPageBySearch(siteId, userId, type, keyword, startDate, endDate, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static int GetRewardPointsBalance(int userId)
        {
            int point = 0;
            IDataReader reader = DBRewardPointsHistory.GetRewardPointsBalance(userId);
            try
            {
                while (reader.Read())
                {
                    point = Convert.ToInt32(reader["PointsBalance"]);
                }
            }
            finally
            {
                reader.Close();
            }


            return point;
        }

        #endregion Static Methods
    }
}
/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-14
/// Last Modified:			2015-08-14

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class DiscountUsageHistory
    {
        #region Constructors

        public DiscountUsageHistory()
        { }

        public DiscountUsageHistory(int historyId)
        {
            this.GetDiscountUsageHistory(historyId);
        }

        #endregion Constructors

        #region Private Properties

        private int historyID = -1;
        private int discountID = -1;
        private int orderID = -1;
        private DateTime createdOn = DateTime.Now;
        private string couponCode = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int HistoryId
        {
            get { return historyID; }
            set { historyID = value; }
        }

        public int DiscountId
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public int OrderId
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public string CouponCode { get => couponCode; set => couponCode = value; }

        #endregion Public Properties

        #region Private Methods

        private void GetDiscountUsageHistory(int historyId)
        {
            using (IDataReader reader = DBDiscountUsageHistory.GetOne(historyId))
            {
                if (reader.Read())
                {
                    this.historyID = Convert.ToInt32(reader["HistoryID"]);
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.orderID = Convert.ToInt32(reader["OrderID"]);
                    this.createdOn = Convert.ToDateTime(reader["CreatedOn"]);

                    if (reader["CouponCode"] != DBNull.Value)
                        this.couponCode = reader["CouponCode"].ToString();
                }
            }
        }

        /// <summary>
        /// Persists a new instance of DiscountUsageHistory. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBDiscountUsageHistory.Create(
                this.discountID,
                this.orderID,
                this.createdOn,
                this.couponCode);

            this.historyID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of DiscountUsageHistory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBDiscountUsageHistory.Update(
                this.historyID,
                this.discountID,
                this.orderID,
                this.createdOn,
                this.couponCode);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of DiscountUsageHistory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.historyID > 0)
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

        public static bool Delete(int historyId)
        {
            return DBDiscountUsageHistory.Delete(historyId);
        }

        public static bool DeleteByDiscount(int discountId)
        {
            return DBDiscountUsageHistory.DeleteByDiscount(discountId);
        }

        public static bool DeleteByOrder(int orderId)
        {
            return DBDiscountUsageHistory.DeleteByOrder(orderId);
        }

        private static List<DiscountUsageHistory> LoadListFromReader(IDataReader reader)
        {
            List<DiscountUsageHistory> discountUsageHistoryList = new List<DiscountUsageHistory>();
            try
            {
                while (reader.Read())
                {
                    DiscountUsageHistory discountUsageHistory = new DiscountUsageHistory
                    {
                        historyID = Convert.ToInt32(reader["HistoryID"]),
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        orderID = Convert.ToInt32(reader["OrderID"]),
                        createdOn = Convert.ToDateTime(reader["CreatedOn"])
                    };

                    if (reader["CouponCode"] != DBNull.Value)
                        discountUsageHistory.couponCode = reader["CouponCode"].ToString();

                    discountUsageHistoryList.Add(discountUsageHistory);
                }
            }
            finally
            {
                reader.Close();
            }

            return discountUsageHistoryList;
        }

        public static IDataReader GetByDiscount(int discountId)
        {
            var reader = DBDiscountUsageHistory.GetByDiscount(discountId);
            return reader; //LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
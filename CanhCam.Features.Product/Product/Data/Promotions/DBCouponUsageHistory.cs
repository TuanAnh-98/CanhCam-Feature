/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-10-06
/// Last Modified:			2014-10-06

using System;
using System.Data;

namespace CanhCam.Data
{

    public static class DBCouponUsageHistory
    {

        /// <summary>
        /// Inserts a row in the gb_CouponUsageHistory table. Returns new integer id.
        /// </summary>
        /// <param name="couponID"> couponID </param>
        /// <param name="orderID"> orderID </param>
        /// <param name="createdOn"> createdOn </param>
        /// <returns>int</returns>
        public static int Create(
            int couponID,
            int orderID,
            DateTime createdOn)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponUsageHistory_Insert", 3);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponID);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the gb_CouponUsageHistory table. Returns true if row updated.
        /// </summary>
        /// <param name="historyID"> historyID </param>
        /// <param name="couponID"> couponID </param>
        /// <param name="orderID"> orderID </param>
        /// <param name="createdOn"> createdOn </param>
        /// <returns>bool</returns>
        public static bool Update(
            int historyID,
            int couponID,
            int orderID,
            DateTime createdOn)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponUsageHistory_Update", 4);
            sph.DefineSqlParameter("@HistoryID", SqlDbType.Int, ParameterDirection.Input, historyID);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponID);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static int GetCountByCoupon(int couponId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CouponUsageHistory_GetCountByCoupon", 1);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        /// <summary>
        /// Deletes a row from the gb_CouponUsageHistory table. Returns true if row deleted.
        /// </summary>
        /// <param name="historyID"> historyID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int historyID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponUsageHistory_Delete", 1);
            sph.DefineSqlParameter("@HistoryID", SqlDbType.Int, ParameterDirection.Input, historyID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool DeleteByCoupon(int couponId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponUsageHistory_DeleteByCoupon", 1);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByOrder(int orderID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponUsageHistory_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int historyID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CouponUsageHistory_SelectOne", 1);
            sph.DefineSqlParameter("@HistoryID", SqlDbType.Int, ParameterDirection.Input, historyID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByCoupon(int couponId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CouponUsageHistory_SelectByCoupon", 1);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponId);
            return sph.ExecuteReader();
        }

    }

}
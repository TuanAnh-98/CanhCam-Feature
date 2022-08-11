// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2015-08-14
// Last Modified:			2015-08-14

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscountUsageHistory
    {
        public static int Create(
            int discountID,
            int orderID,
            DateTime createdOn,
            string couponCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountUsageHistory_Insert", 4);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int historyID,
            int discountID,
            int orderID,
            DateTime createdOn,
            string couponCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountUsageHistory_Update", 5);
            sph.DefineSqlParameter("@HistoryID", SqlDbType.Int, ParameterDirection.Input, historyID);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int historyID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountUsageHistory_Delete", 1);
            sph.DefineSqlParameter("@HistoryID", SqlDbType.Int, ParameterDirection.Input, historyID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByDiscount(int discountID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountUsageHistory_DeleteByDiscount", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByOrder(int orderID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountUsageHistory_DeleteByOrder", 1);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int historyID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountUsageHistory_SelectOne", 1);
            sph.DefineSqlParameter("@HistoryID", SqlDbType.Int, ParameterDirection.Input, historyID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByDiscount(int discountId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountUsageHistory_SelectByDiscount", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            return sph.ExecuteReader();
        }
    }
}
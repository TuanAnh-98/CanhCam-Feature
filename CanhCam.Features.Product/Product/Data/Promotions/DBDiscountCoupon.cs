/// Author:					Tran Quoc Vuong
/// Created:				2020-11-09
/// Last Modified:			2020-11-09

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscountCoupon
    {
        /// <summary>
        /// Inserts a row in the gb_DiscountCoupon table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="discountID"> discountID </param>
        /// <param name="couponCode"> couponCode </param>
        /// <param name="useCount"> useCount </param>
        /// <param name="limitationTimes"> limitationTimes </param>
        /// <param name="maximumQtyDiscount"> maximumQtyDiscount </param>
        /// <param name="maximumQtyRequired"> maximumQtyRequired </param>
        /// <param name="createdDate"> createdDate </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            int discountID,
            string couponCode,
            int useCount,
            int limitationTimes,
            int maximumQtyDiscount,
            int maximumQtyRequired,
            DateTime createdDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountCoupon_Insert", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@UseCount", SqlDbType.Int, ParameterDirection.Input, useCount);
            sph.DefineSqlParameter("@LimitationTimes", SqlDbType.Int, ParameterDirection.Input, limitationTimes);
            sph.DefineSqlParameter("@MaximumQtyDiscount", SqlDbType.Int, ParameterDirection.Input, maximumQtyDiscount);
            sph.DefineSqlParameter("@MaximumQtyRequired", SqlDbType.Int, ParameterDirection.Input, maximumQtyRequired);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the gb_DiscountCoupon table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="discountID"> discountID </param>
        /// <param name="couponCode"> couponCode </param>
        /// <param name="useCount"> useCount </param>
        /// <param name="limitationTimes"> limitationTimes </param>
        /// <param name="maximumQtyDiscount"> maximumQtyDiscount </param>
        /// <param name="maximumQtyRequired"> maximumQtyRequired </param>
        /// <param name="createdDate"> createdDate </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            int discountID,
            string couponCode,
            int useCount,
            int limitationTimes,
            int maximumQtyDiscount,
            int maximumQtyRequired,
            DateTime createdDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountCoupon_Update", 8);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@UseCount", SqlDbType.Int, ParameterDirection.Input, useCount);
            sph.DefineSqlParameter("@LimitationTimes", SqlDbType.Int, ParameterDirection.Input, limitationTimes);
            sph.DefineSqlParameter("@MaximumQtyDiscount", SqlDbType.Int, ParameterDirection.Input, maximumQtyDiscount);
            sph.DefineSqlParameter("@MaximumQtyRequired", SqlDbType.Int, ParameterDirection.Input, maximumQtyRequired);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountCoupon_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByDiscount(int discountID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountCoupon_DeleteByDiscount", 2);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_DiscountCoupon table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountCoupon_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static int GetCount(int discountId, string couponCode)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountCoupon_GetCount", 2);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPage(
            int discountId,
            string couponCode,
            int pageNumber,
            int pageSize)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountCoupon_SelectPage", 4);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByCode(string couponCode)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountCoupon_SelectByCode", 1);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            return sph.ExecuteReader();
        }

        public static bool UpdateUseCount(string couponCode)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountCoupon_UpdateUseCount", 1);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool ExistCode(string couponCode)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountCoupon_Exists", 1);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            int foundRows = Convert.ToInt32(sph.ExecuteScalar());
            return (foundRows > 0);
        }
    }
}
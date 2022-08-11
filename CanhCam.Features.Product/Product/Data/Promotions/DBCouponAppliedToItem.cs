/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-11-24
/// Last Modified:			2014-11-24

using System;
using System.Data;

namespace CanhCam.Data
{

    public static class DBCouponAppliedToItem
    {

        /// <summary>
        /// Inserts a row in the gb_CouponAppliedToItems table. Returns rows affected count.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="couponID"> couponID </param>
        /// <param name="itemID"> itemID </param>
        /// <param name="appliedType"> appliedType </param>
        /// <param name="usePercentage"> usePercentage </param>
        /// <param name="discount"> discount </param>
        /// <param name="giftType"> giftType </param>
        /// <param name="giftProducts"> giftProducts </param>
        /// <param name="giftCustomProducts"> giftCustomProducts </param>
        /// <returns>int</returns>
        public static int Create(
            Guid guid,
            int couponID,
            int itemID,
            int appliedType,
            bool usePercentage,
            decimal discount,
            int giftType,
            string giftProducts,
            string giftCustomProducts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponAppliedToItems_Insert", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponID);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@GiftType", SqlDbType.Int, ParameterDirection.Input, giftType);
            sph.DefineSqlParameter("@GiftProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftProducts);
            sph.DefineSqlParameter("@GiftCustomProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftCustomProducts);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the gb_CouponAppliedToItems table. Returns true if row updated.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <param name="couponID"> couponID </param>
        /// <param name="itemID"> itemID </param>
        /// <param name="appliedType"> appliedType </param>
        /// <param name="usePercentage"> usePercentage </param>
        /// <param name="discount"> discount </param>
        /// <param name="giftType"> giftType </param>
        /// <param name="giftProducts"> giftProducts </param>
        /// <param name="giftCustomProducts"> giftCustomProducts </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid guid,
            int couponID,
            int itemID,
            int appliedType,
            bool usePercentage,
            decimal discount,
            int giftType,
            string giftProducts,
            string giftCustomProducts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponAppliedToItems_Update", 9);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponID);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@GiftType", SqlDbType.Int, ParameterDirection.Input, giftType);
            sph.DefineSqlParameter("@GiftProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftProducts);
            sph.DefineSqlParameter("@GiftCustomProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftCustomProducts);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponAppliedToItems_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        
        public static bool DeleteByCoupon(int couponId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CouponAppliedToItems_DeleteByCoupon", 1);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponId);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CouponAppliedToItems_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByCoupon(int couponId, int appliedType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CouponAppliedToItems_SelectByCoupon", 2);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponId);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            return sph.ExecuteReader();
        }

    }
}

/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-10-06
/// Last Modified:			2014-10-06

using System;
using System.Data;

namespace CanhCam.Data
{

    public static class DBCoupon
    {
        public static int Create(
            int siteID,
            string couponCode,
            string name,
            decimal discount,
            int discountType,
            decimal orderPurchaseFrom,
            decimal orderPurchaseTo,
            DateTime? fromDate,
            DateTime? expiryDate,
            decimal minPurchase,
            int limitationTimes,
            bool isActive,
            int appliedType,
            string appliedToProducts,
            string appliedToCategories,
            Guid guid, 
			DateTime createdOn,
            int discountQtyStep,
            int maximumQtyDiscount,
            string limitationStores)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Coupon_Insert", 20);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@OrderPurchaseFrom", SqlDbType.Decimal, ParameterDirection.Input, orderPurchaseFrom);
            sph.DefineSqlParameter("@OrderPurchaseTo", SqlDbType.Decimal, ParameterDirection.Input, orderPurchaseTo);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ExpiryDate", SqlDbType.DateTime, ParameterDirection.Input, expiryDate);
            sph.DefineSqlParameter("@MinPurchase", SqlDbType.Decimal, ParameterDirection.Input, minPurchase);
            sph.DefineSqlParameter("@LimitationTimes", SqlDbType.Int, ParameterDirection.Input, limitationTimes);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            sph.DefineSqlParameter("@AppliedToProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedToProducts);
            sph.DefineSqlParameter("@AppliedToCategories", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedToCategories);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            sph.DefineSqlParameter("@DiscountQtyStep", SqlDbType.Int, ParameterDirection.Input, discountQtyStep);
            sph.DefineSqlParameter("@MaximumQtyDiscount", SqlDbType.Int, ParameterDirection.Input, maximumQtyDiscount);
            sph.DefineSqlParameter("@LimitationStores", SqlDbType.NVarChar, -1, ParameterDirection.Input, limitationStores);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int couponID,
            int siteID,
            string couponCode,
            string name,
            decimal discount,
            int discountType,
            decimal orderPurchaseFrom,
            decimal orderPurchaseTo,
            DateTime? fromDate,
            DateTime? expiryDate,
            decimal minPurchase,
            int limitationTimes,
            bool isActive,
            int appliedType,
            string appliedToProducts,
            string appliedToCategories,
            Guid guid, 
			DateTime createdOn,
            int discountQtyStep,
            int maximumQtyDiscount,
            string limitationStores)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Coupon_Update", 21);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Discount", SqlDbType.Decimal, ParameterDirection.Input, discount);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@OrderPurchaseFrom", SqlDbType.Decimal, ParameterDirection.Input, orderPurchaseFrom);
            sph.DefineSqlParameter("@OrderPurchaseTo", SqlDbType.Decimal, ParameterDirection.Input, orderPurchaseTo);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ExpiryDate", SqlDbType.DateTime, ParameterDirection.Input, expiryDate);
            sph.DefineSqlParameter("@MinPurchase", SqlDbType.Decimal, ParameterDirection.Input, minPurchase);
            sph.DefineSqlParameter("@LimitationTimes", SqlDbType.Int, ParameterDirection.Input, limitationTimes);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            sph.DefineSqlParameter("@AppliedToProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedToProducts);
            sph.DefineSqlParameter("@AppliedToCategories", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedToCategories);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            sph.DefineSqlParameter("@DiscountQtyStep", SqlDbType.Int, ParameterDirection.Input, discountQtyStep);
            sph.DefineSqlParameter("@MaximumQtyDiscount", SqlDbType.Int, ParameterDirection.Input, maximumQtyDiscount);
            sph.DefineSqlParameter("@LimitationStores", SqlDbType.NVarChar, -1, ParameterDirection.Input, limitationStores);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int couponId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Coupon_Delete", 1);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Coupon_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int couponID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Coupon_SelectOne", 1);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, couponID);
            return sph.ExecuteReader();

        }

        public static IDataReader GetOneByCode(int siteID, string couponCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Coupon_SelectOneByCode", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            return sph.ExecuteReader();
        }

        public static int GetCount(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Coupon_GetCount", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPage(
            int siteId,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Coupon_SelectPage", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
        
        public static bool IsAvailable(int siteId, string zoneIds, string productIds)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Coupon_IsAvailable", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
            return Convert.ToInt32(sph.ExecuteScalar()) == 1;
        }

        public static int CopyCoupon(int couponRefID, string couponCode, DateTime? fromDate, DateTime? expiryDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Coupon_Copy", 4);
            sph.DefineSqlParameter("@CouponRefID", SqlDbType.Int, ParameterDirection.Input, couponRefID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ExpiryDate", SqlDbType.DateTime, ParameterDirection.Input, expiryDate);
            //int rowsAffected = sph.ExecuteNonQuery();
            //return (rowsAffected > -1);

            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

    }

}

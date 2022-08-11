/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-05
/// Last Modified:			2015-08-05

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscount
    {
        public static int Create(
            int siteID,
            string name,
            int discountType,
            bool isActive,
            DateTime? startDate,
            DateTime? endDate,
            bool calendarEnabled,
            string calendarDaily,
            bool usePercentage,
            decimal discountAmount,
            decimal minPurchase,
            int discountQtyStep,
            int specialProductId,
            Guid guid,
            DateTime createdOn,
            decimal maximumDiscount,
            int priority,
            int options,
            string url,
            string imageFile,
            string bannerFile,
            string briefContent,
            string fullContent,
            string contentTokens,
            //bool alwaysOnDisplay,
            bool appliedAllProducts,
            string appliedForStoreIDs,
            string appliedForPaymentIDs,
            string excludedZoneIDs,
            string excludedProductIDs,
            int shareType,
            int pageID,
            string createdBy,
            string code)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Discount_Insert", 33);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@CalendarEnabled", SqlDbType.Bit, ParameterDirection.Input, calendarEnabled);
            sph.DefineSqlParameter("@CalendarDaily", SqlDbType.NVarChar, -1, ParameterDirection.Input, calendarDaily);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@MinPurchase", SqlDbType.Decimal, ParameterDirection.Input, minPurchase);
            sph.DefineSqlParameter("@DiscountQtyStep", SqlDbType.Int, ParameterDirection.Input, discountQtyStep);
            sph.DefineSqlParameter("@SpecialProductID", SqlDbType.Int, ParameterDirection.Input, specialProductId);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);

            sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@ImageFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFile);
            sph.DefineSqlParameter("@BannerFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, bannerFile);
            sph.DefineSqlParameter("@BriefContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, briefContent);
            sph.DefineSqlParameter("@FullContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, fullContent);
            sph.DefineSqlParameter("@ContentTokens", SqlDbType.NVarChar, -1, ParameterDirection.Input, contentTokens);
            //sph.DefineSqlParameter("@AlwaysOnDisplay", SqlDbType.Bit, ParameterDirection.Input, alwaysOnDisplay);
            sph.DefineSqlParameter("@AppliedAllProducts", SqlDbType.Bit, ParameterDirection.Input, appliedAllProducts);
            sph.DefineSqlParameter("@AppliedForStoreIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedForStoreIDs);
            sph.DefineSqlParameter("@AppliedForPaymentIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedForPaymentIDs);
            sph.DefineSqlParameter("@ExcludedZoneIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, excludedZoneIDs);
            sph.DefineSqlParameter("@ExcludedProductIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, excludedProductIDs);
            sph.DefineSqlParameter("@ShareType", SqlDbType.Int, ParameterDirection.Input, shareType);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageID);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);

            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int discountID,
            int siteID,
            string name,
            int discountType,
            bool isActive,
            DateTime? startDate,
            DateTime? endDate,
            bool calendarEnabled,
            string calendarDaily,
            bool usePercentage,
            decimal discountAmount,
            decimal minPurchase,
            int discountQtyStep,
            int specialProductId,
            Guid guid,
            DateTime createdOn,
            decimal maximumDiscount,
            int priority,
            int options,
            string url,
            string imageFile,
            string bannerFile,
            string briefContent,
            string fullContent,
            string contentTokens,
            //bool alwaysOnDisplay,
            bool appliedAllProducts,
            string appliedForStoreIDs,
            string appliedForPaymentIDs,
            string excludedZoneIDs,
            string excludedProductIDs,
            int shareType,
            int pageID,
            string createdBy,
            string code)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Discount_Update", 34);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@CalendarEnabled", SqlDbType.Bit, ParameterDirection.Input, calendarEnabled);
            sph.DefineSqlParameter("@CalendarDaily", SqlDbType.NVarChar, -1, ParameterDirection.Input, calendarDaily);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@MinPurchase", SqlDbType.Decimal, ParameterDirection.Input, minPurchase);
            sph.DefineSqlParameter("@DiscountQtyStep", SqlDbType.Int, ParameterDirection.Input, discountQtyStep);
            sph.DefineSqlParameter("@SpecialProductID", SqlDbType.Int, ParameterDirection.Input, specialProductId);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);

            sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@ImageFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFile);
            sph.DefineSqlParameter("@BannerFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, bannerFile);
            sph.DefineSqlParameter("@BriefContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, briefContent);
            sph.DefineSqlParameter("@FullContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, fullContent);
            sph.DefineSqlParameter("@ContentTokens", SqlDbType.NVarChar, -1, ParameterDirection.Input, contentTokens);
            //sph.DefineSqlParameter("@AlwaysOnDisplay", SqlDbType.Bit, ParameterDirection.Input, alwaysOnDisplay);
            sph.DefineSqlParameter("@AppliedAllProducts", SqlDbType.Bit, ParameterDirection.Input, appliedAllProducts);
            sph.DefineSqlParameter("@AppliedForStoreIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedForStoreIDs);
            sph.DefineSqlParameter("@AppliedForPaymentIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, appliedForPaymentIDs);
            sph.DefineSqlParameter("@ExcludedZoneIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, excludedZoneIDs);
            sph.DefineSqlParameter("@ExcludedProductIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, excludedProductIDs);
            sph.DefineSqlParameter("@ShareType", SqlDbType.Int, ParameterDirection.Input, shareType);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageID);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int discountId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Discount_Delete", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int discountId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_SelectOne", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetActive(int siteId, int discountType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_SelectOneActive", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            return sph.ExecuteReader();
        }

        public static int GetCount(int siteId, int discountType, int status, int pageID, int productId, int zoneId, int excludedDiscountId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_GetCount", 7);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageID);
            sph.DefineSqlParameter("@ProducID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@ExcludedDiscountID", SqlDbType.Int, ParameterDirection.Input, excludedDiscountId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPage(
            int siteId,
            int discountType,
            int status,
            int pageID,
            int productId,
            int zoneId,
            int excludedDiscountId,
            int orderBy,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_SelectPage", 10);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageID);
            sph.DefineSqlParameter("@ProducID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@ExcludedDiscountID", SqlDbType.Int, ParameterDirection.Input, excludedDiscountId);
            sph.DefineSqlParameter("@OrderBy", SqlDbType.Int, ParameterDirection.Input, orderBy);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetDiscountCoupons(int siteId, string keyword, string discountIds, int discountType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_SelectDiscountCoupons", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@DiscountIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, discountIds);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            return sph.ExecuteReader();
        }

        //public static int GetCountProduct(
        //    int siteId,
        //    int discountId,
        //    int specialProductId,
        //    DateTime currentTime,
        //    int languageId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_GetCountProduct", 5);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
        //    sph.DefineSqlParameter("@SpecialProductID", SqlDbType.Int, ParameterDirection.Input, specialProductId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    return Convert.ToInt32(sph.ExecuteScalar());
        //}

        //public static IDataReader GetPageProduct(
        //    int siteId,
        //    int discountId,
        //    int specialProductId,
        //    DateTime currentTime,
        //    int languageId,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Discount_SelectPageProduct", 7);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
        //    sph.DefineSqlParameter("@SpecialProductID", SqlDbType.Int, ParameterDirection.Input, specialProductId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();
        //}
    }
}
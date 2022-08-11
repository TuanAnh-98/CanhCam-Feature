/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProduct
    {
        public static int Create(
            int siteID,
            int zoneID,
            string title,
            string subTitle,
            string url,
            string code,
            string briefContent,
            string fullContent,
            int productType,
            bool openInNewWindow,
            int position,
            int showOption,
            bool isPublished,
            DateTime startDate,
            DateTime endDate,
            int displayOrder,
            decimal price,
            decimal oldPrice,
            decimal specialPrice,
            DateTime? specialPriceStartDate,
            DateTime? specialPriceEndDate,
            int viewCount,
            int commentCount,
            string metaTitle,
            string metaKeywords,
            string metaDescription,
            string additionalMetaTags,
            string compiledMeta,
            int manufacturerID,
            int stockQuantity,
            bool disableBuyButton,
            string fileAttachment,
            Guid productGuid,
            Guid userGuid,
            DateTime createdUtc,
            DateTime lastModUtc,
            Guid lastModUserGuid,
            bool isDeleted,
            int? stateID,
            DateTime? approvedUtc,
            Guid? approvedUserGuid,
            string approvedBy,
            string rejectedNotes,
            int options,
            int parentID,
            int childCount,
            string apiProductID,
            decimal weight,
            decimal ratingSum,
            int ratingVotes,
            string groupProductIds,
            decimal groupPrice)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_Insert", 52);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@SubTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, subTitle);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@BriefContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, briefContent);
            sph.DefineSqlParameter("@FullContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, fullContent);
            sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
            sph.DefineSqlParameter("@OpenInNewWindow", SqlDbType.Bit, ParameterDirection.Input, openInNewWindow);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            if (endDate < DateTime.MaxValue)
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            else
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@OldPrice", SqlDbType.Decimal, ParameterDirection.Input, oldPrice);
            sph.DefineSqlParameter("@SpecialPrice", SqlDbType.Decimal, ParameterDirection.Input, specialPrice);
            sph.DefineSqlParameter("@SpecialPriceStartDate", SqlDbType.DateTime, ParameterDirection.Input, specialPriceStartDate);
            sph.DefineSqlParameter("@SpecialPriceEndDate", SqlDbType.DateTime, ParameterDirection.Input, specialPriceEndDate);
            sph.DefineSqlParameter("@ViewCount", SqlDbType.Int, ParameterDirection.Input, viewCount);
            sph.DefineSqlParameter("@CommentCount", SqlDbType.Int, ParameterDirection.Input, commentCount);
            sph.DefineSqlParameter("@MetaTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaTitle);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@AdditionalMetaTags", SqlDbType.NVarChar, -1, ParameterDirection.Input, additionalMetaTags);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@ManufacturerID", SqlDbType.Int, ParameterDirection.Input, manufacturerID);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            sph.DefineSqlParameter("@DisableBuyButton", SqlDbType.Bit, ParameterDirection.Input, disableBuyButton);
            sph.DefineSqlParameter("@FileAttachment", SqlDbType.NVarChar, 255, ParameterDirection.Input, fileAttachment);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateID);
            sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            sph.DefineSqlParameter("@ApprovedUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedUserGuid);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.NVarChar, 100, ParameterDirection.Input, approvedBy);
            sph.DefineSqlParameter("@RejectedNotes", SqlDbType.NVarChar, -1, ParameterDirection.Input, rejectedNotes);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentID);
            sph.DefineSqlParameter("@ChildCount", SqlDbType.Int, ParameterDirection.Input, childCount);
            sph.DefineSqlParameter("@ApiProductID", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiProductID);
            sph.DefineSqlParameter("@Weight", SqlDbType.Decimal, ParameterDirection.Input, weight);
            sph.DefineSqlParameter("@RatingSum", SqlDbType.Decimal, ParameterDirection.Input, ratingSum);
            sph.DefineSqlParameter("@RatingVotes", SqlDbType.Int, ParameterDirection.Input, ratingVotes);
            sph.DefineSqlParameter("@GroupProductIds", SqlDbType.VarChar, 255, ParameterDirection.Input, groupProductIds);
            sph.DefineSqlParameter("@GroupPrice", SqlDbType.Decimal, ParameterDirection.Input, groupPrice);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int productID,
            int siteID,
            int zoneID,
            string title,
            string subTitle,
            string url,
            string code,
            string briefContent,
            string fullContent,
            int productType,
            bool openInNewWindow,
            int position,
            int showOption,
            bool isPublished,
            DateTime startDate,
            DateTime endDate,
            int displayOrder,
            decimal price,
            decimal oldPrice,
            decimal specialPrice,
            DateTime? specialPriceStartDate,
            DateTime? specialPriceEndDate,
            int viewCount,
            int commentCount,
            string metaTitle,
            string metaKeywords,
            string metaDescription,
            string additionalMetaTags,
            string compiledMeta,
            int manufacturerID,
            int stockQuantity,
            bool disableBuyButton,
            string fileAttachment,
            Guid productGuid,
            Guid userGuid,
            DateTime createdUtc,
            DateTime lastModUtc,
            Guid lastModUserGuid,
            bool isDeleted,
            int? stateID,
            DateTime? approvedUtc,
            Guid? approvedUserGuid,
            string approvedBy,
            string rejectedNotes,
            int options,
            int parentID,
            int childCount,
            string apiProductID,
            decimal weight,
            decimal ratingSum,
            int ratingVotes,
            string groupProductIds,
            decimal groupPrice)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_Update", 53);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@SubTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, subTitle);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@BriefContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, briefContent);
            sph.DefineSqlParameter("@FullContent", SqlDbType.NVarChar, -1, ParameterDirection.Input, fullContent);
            sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
            sph.DefineSqlParameter("@OpenInNewWindow", SqlDbType.Bit, ParameterDirection.Input, openInNewWindow);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            if (endDate < DateTime.MaxValue)
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            else
                sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, DBNull.Value);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@OldPrice", SqlDbType.Decimal, ParameterDirection.Input, oldPrice);
            sph.DefineSqlParameter("@SpecialPrice", SqlDbType.Decimal, ParameterDirection.Input, specialPrice);
            sph.DefineSqlParameter("@SpecialPriceStartDate", SqlDbType.DateTime, ParameterDirection.Input, specialPriceStartDate);
            sph.DefineSqlParameter("@SpecialPriceEndDate", SqlDbType.DateTime, ParameterDirection.Input, specialPriceEndDate);
            sph.DefineSqlParameter("@ViewCount", SqlDbType.Int, ParameterDirection.Input, viewCount);
            sph.DefineSqlParameter("@CommentCount", SqlDbType.Int, ParameterDirection.Input, commentCount);
            sph.DefineSqlParameter("@MetaTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaTitle);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@AdditionalMetaTags", SqlDbType.NVarChar, -1, ParameterDirection.Input, additionalMetaTags);
            sph.DefineSqlParameter("@CompiledMeta", SqlDbType.NVarChar, -1, ParameterDirection.Input, compiledMeta);
            sph.DefineSqlParameter("@ManufacturerID", SqlDbType.Int, ParameterDirection.Input, manufacturerID);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            sph.DefineSqlParameter("@DisableBuyButton", SqlDbType.Bit, ParameterDirection.Input, disableBuyButton);
            sph.DefineSqlParameter("@FileAttachment", SqlDbType.NVarChar, 255, ParameterDirection.Input, fileAttachment);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@LastModUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, lastModUserGuid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateID);
            sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            sph.DefineSqlParameter("@ApprovedUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedUserGuid);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.NVarChar, 100, ParameterDirection.Input, approvedBy);
            sph.DefineSqlParameter("@RejectedNotes", SqlDbType.NVarChar, -1, ParameterDirection.Input, rejectedNotes);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentID);
            sph.DefineSqlParameter("@ChildCount", SqlDbType.Int, ParameterDirection.Input, childCount);
            sph.DefineSqlParameter("@ApiProductID", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiProductID);
            sph.DefineSqlParameter("@Weight", SqlDbType.Decimal, ParameterDirection.Input, weight);
            sph.DefineSqlParameter("@RatingSum", SqlDbType.Decimal, ParameterDirection.Input, ratingSum);
            sph.DefineSqlParameter("@RatingVotes", SqlDbType.Int, ParameterDirection.Input, ratingVotes);
            sph.DefineSqlParameter("@GroupProductIds", SqlDbType.VarChar, 255, ParameterDirection.Input, groupProductIds);
            sph.DefineSqlParameter("@GroupPrice", SqlDbType.Decimal, ParameterDirection.Input, groupPrice);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool UpdateState(int productId, int? stateId, DateTime? approvedUtc, Guid? approvedUserGuid, string approvedBy, string rejectedNotes)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateState", 6);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateId);
            sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            sph.DefineSqlParameter("@ApprovedUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedUserGuid);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.NVarChar, 100, ParameterDirection.Input, approvedBy);
            sph.DefineSqlParameter("@RejectedNotes", SqlDbType.NVarChar, ParameterDirection.Input, rejectedNotes);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateDeleted(int productId, bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateDeleted", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateStockQuantity(int productId, int stockQuantity)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateStockQuantity", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool IncrementViewCount(int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_IncrementViewCount", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateZone(int productId, int zoneId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateZone", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetNextPreviousProduct(
            int productId,
            int zoneId,
            int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectNextPrevious", 4);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static bool Delete(int productID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_Delete", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetOne(int siteId, int productId, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectOne", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOne(int zoneId, DateTime currentTime, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectOneByZone", 3);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        //public static int GetCount(
        //    int siteId,
        //    int zoneId,
        //    DateTime currentTime,
        //    int languageId,
        //    int position)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_GetCount", 5);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
        //    return Convert.ToInt32(sph.ExecuteScalar());
        //}

        //public static IDataReader GetPage(
        //    int siteId,
        //    int zoneId,
        //    DateTime currentTime,
        //    int languageId,
        //    int position,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectPage", 7);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();
        //}

        //public static int GetCountByListZone(
        //    int siteId,
        //    string listZoneId,
        //    int languageId,
        //    int position)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_GetCountByListZone", 5);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ListZoneID", SqlDbType.NVarChar, ParameterDirection.Input, listZoneId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
        //    return Convert.ToInt32(sph.ExecuteScalar());
        //}

        //public static IDataReader GetPageByListZone(
        //    int siteId,
        //    string listZoneId,
        //    int languageId,
        //    int position,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectPageByListZone", 7);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ListZoneID", SqlDbType.NVarChar, ParameterDirection.Input, listZoneId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();
        //}

        //public static int GetCountBySearch(
        //    int siteId = -1,
        //    string zoneIds = null,
        //    int publishStatus = -1,
        //    int languageId = -1,
        //    int manufactureId = -1,
        //    int productType = -1,
        //    decimal? priceMin = null,
        //    decimal? priceMax = null,
        //    int position = -1,
        //    int showOption = -1,
        //    string propertyCondition = null,
        //    string productIds = null,
        //    string keyword = null,
        //    bool searchCode = true,
        //    string stateIds = null,
        //    int orderBy = 0)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_GetCountBySearch", 15);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
        //    sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@ManufactureID", SqlDbType.Int, ParameterDirection.Input, manufactureId);
        //    sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
        //    sph.DefineSqlParameter("@PriceMin", SqlDbType.Decimal, ParameterDirection.Input, priceMin);
        //    sph.DefineSqlParameter("@PriceMax", SqlDbType.Decimal, ParameterDirection.Input, priceMax);
        //    sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
        //    sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
        //    sph.DefineSqlParameter("@PropertyCondition", SqlDbType.NVarChar, ParameterDirection.Input, propertyCondition);
        //    sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
        //    sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
        //    sph.DefineSqlParameter("@SearchCode", SqlDbType.Bit, ParameterDirection.Input, searchCode);
        //    sph.DefineSqlParameter("@StateIDs", SqlDbType.NVarChar, ParameterDirection.Input, stateIds);
        //    return Convert.ToInt32(sph.ExecuteScalar());
        //}

        //public static IDataReader GetPageBySearch(
        //    int pageNumber = 1,
        //    int pageSize = 32767,
        //    int siteId = -1,
        //    string zoneIds = null,
        //    int publishStatus = -1,
        //    int languageId = -1,
        //    int manufactureId = -1,
        //    int productType = -1,
        //    decimal? priceMin = null,
        //    decimal? priceMax = null,
        //    int position = -1,
        //    int showOption = -1,
        //    string propertyCondition = null,
        //    string productIds = null,
        //    string keyword = null,
        //    bool searchCode = true,
        //    string stateIds = null,
        //    int orderBy = 0)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectPageBySearch", 18);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
        //    sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@ManufactureID", SqlDbType.Int, ParameterDirection.Input, manufactureId);
        //    sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
        //    sph.DefineSqlParameter("@PriceMin", SqlDbType.Decimal, ParameterDirection.Input, priceMin);
        //    sph.DefineSqlParameter("@PriceMax", SqlDbType.Decimal, ParameterDirection.Input, priceMax);
        //    sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
        //    sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
        //    sph.DefineSqlParameter("@PropertyCondition", SqlDbType.NVarChar, ParameterDirection.Input, propertyCondition);
        //    sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
        //    sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
        //    sph.DefineSqlParameter("@SearchCode", SqlDbType.Bit, ParameterDirection.Input, searchCode);
        //    sph.DefineSqlParameter("@StateIDs", SqlDbType.NVarChar, ParameterDirection.Input, stateIds);
        //    sph.DefineSqlParameter("@OrderBy", SqlDbType.Int, ParameterDirection.Input, orderBy);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();
        //}

        public static int GetCountAdv(
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = true,
            bool searchProductZone = false,
            string stateIds = null)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_GetCountAdv", 18);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@ManufactureID", SqlDbType.Int, ParameterDirection.Input, manufactureId);
            sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
            sph.DefineSqlParameter("@PriceMin", SqlDbType.Decimal, ParameterDirection.Input, priceMin);
            sph.DefineSqlParameter("@PriceMax", SqlDbType.Decimal, ParameterDirection.Input, priceMax);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@PropertyCondition", SqlDbType.NVarChar, ParameterDirection.Input, propertyCondition);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@SearchCode", SqlDbType.Bit, ParameterDirection.Input, searchCode);
            sph.DefineSqlParameter("@SearchProductZone", SqlDbType.Bit, ParameterDirection.Input, searchProductZone);
            sph.DefineSqlParameter("@StateIDs", SqlDbType.NVarChar, ParameterDirection.Input, stateIds);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageAdv(
            int pageNumber = 1,
            int pageSize = 32767,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = true,
            bool searchProductZone = false,
            string stateIds = null,
            int discountId = -1,
            int orderBy = 0)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectPageAdv", 22);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@ManufactureID", SqlDbType.Int, ParameterDirection.Input, manufactureId);
            sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
            sph.DefineSqlParameter("@PriceMin", SqlDbType.Decimal, ParameterDirection.Input, priceMin);
            sph.DefineSqlParameter("@PriceMax", SqlDbType.Decimal, ParameterDirection.Input, priceMax);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@PropertyCondition", SqlDbType.NVarChar, ParameterDirection.Input, propertyCondition);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@SearchCode", SqlDbType.Bit, ParameterDirection.Input, searchCode);
            sph.DefineSqlParameter("@SearchProductZone", SqlDbType.Bit, ParameterDirection.Input, searchProductZone);
            sph.DefineSqlParameter("@StateIDs", SqlDbType.NVarChar, ParameterDirection.Input, stateIds);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@OrderBy", SqlDbType.Int, ParameterDirection.Input, orderBy);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetTopAdv(
            int top = 1,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = true,
            bool searchProductZone = false,
            string stateIds = null,
            int orderBy = 0)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectTopAdv", 19);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@ManufactureID", SqlDbType.Int, ParameterDirection.Input, manufactureId);
            sph.DefineSqlParameter("@ProductType", SqlDbType.Int, ParameterDirection.Input, productType);
            sph.DefineSqlParameter("@PriceMin", SqlDbType.Decimal, ParameterDirection.Input, priceMin);
            sph.DefineSqlParameter("@PriceMax", SqlDbType.Decimal, ParameterDirection.Input, priceMax);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@PropertyCondition", SqlDbType.NVarChar, ParameterDirection.Input, propertyCondition);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@SearchCode", SqlDbType.Bit, ParameterDirection.Input, searchCode);
            sph.DefineSqlParameter("@SearchProductZone", SqlDbType.Bit, ParameterDirection.Input, searchProductZone);
            sph.DefineSqlParameter("@StateIDs", SqlDbType.NVarChar, ParameterDirection.Input, stateIds);
            sph.DefineSqlParameter("@OrderBy", SqlDbType.Int, ParameterDirection.Input, orderBy);
            sph.DefineSqlParameter("@Top", SqlDbType.Int, ParameterDirection.Input, top);
            return sph.ExecuteReader();
        }

        #region ProductOther

        //private static int GetCountProductOther(int zoneId, int productId, DateTime currentTime, int languageId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_GetCountProductOther", 4);
        //    sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
        //    sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);

        //    return Convert.ToInt32(sph.ExecuteScalar());
        //}

        //public static IDataReader GetPageProductOther(
        //    int zoneId,
        //    int productId,
        //    DateTime currentTime,
        //    int languageId,
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    totalPages = 1;
        //    int totalRows = GetCountProductOther(zoneId, productId, currentTime, languageId);

        //    if (pageSize > 0) totalPages = totalRows / pageSize;

        //    if (totalRows <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else if (pageSize > 0)
        //    {
        //        int remainder;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectPageProductOther", 6);
        //    sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
        //    sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
        //    sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, currentTime);
        //    sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();
        //}

        #endregion ProductOther

        public static IDataReader GetRelatedProducts(
            int siteId,
            Guid siteGuid,
            Guid productGuid,
            bool showHidden = false,
            bool twoWayRelated = false,
            int languageId = -1)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectRelatedProduct", 6);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@ProductGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, productGuid);
            sph.DefineSqlParameter("@ShowHidden", SqlDbType.Bit, ParameterDirection.Input, showHidden);
            sph.DefineSqlParameter("@TwoWayRelated", SqlDbType.Bit, ParameterDirection.Input, twoWayRelated);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        //public static IDataReader GetByZone(int siteId, int zoneId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectByZone", 2);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneId);
        //    return sph.ExecuteReader();
        //}

        public static IDataReader GetByShoppingCart(
            int siteId,
            Guid cartGuid,
            int cartType,
            int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectByShoppingCart", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@CartGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, cartGuid);
            sph.DefineSqlParameter("@CartType", SqlDbType.Int, ParameterDirection.Input, cartType);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByTag(
           int siteId,
           int tagId,
           int publishStatus,
           int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectByTag", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@TagID", SqlDbType.Int, ParameterDirection.Input, tagId);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByGuids(
           int siteId,
           string productGuids,
           int publishStatus,
           int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectByGuids", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ProductGuids", SqlDbType.NVarChar, ParameterDirection.Input, productGuids);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByOrder(
            int siteId,
            int orderId,
            int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectByOrder", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderId);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetProductForSiteMap(int siteId, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectForSiteMap", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            sph.DefineSqlParameter("@CurrentTime", SqlDbType.DateTime, ParameterDirection.Input, DateTime.UtcNow);
            return sph.ExecuteReader();
        }

        public static bool UpdateCommentCount(int productId, int commentCount)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateCommentCount", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@CommentCount", SqlDbType.Int, ParameterDirection.Input, commentCount);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetByCode(int siteId, string productCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectOneByCode", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ProductCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, productCode);
            return sph.ExecuteReader();
        }

        // public static int GetCountByKeyword(
        //int siteId,
        //string keyword)
        // {
        //     SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_GetCountByKeyword", 2);
        //     sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //     sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, ParameterDirection.Input, keyword);
        //     return Convert.ToInt32(sph.ExecuteScalar());
        // }

        // public static IDataReader GetPageByKeyword(
        //     int siteId,
        //     string keyword,
        //     int pageNumber,
        //     int pageSize)
        // {
        //     SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectPageByKeyword", 4);
        //     sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //     sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, ParameterDirection.Input, keyword);
        //     sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //     sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //     return sph.ExecuteReader();
        // }

        public static IDataReader GetZoneByKeyword(
            int siteId,
            string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_SelectZoneByKeyword", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            return sph.ExecuteReader();
        }

        public static bool UpdateCommentStats(int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateCommentStats", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool UpdateInventory(int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Product_UpdateInventory", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        #region Child Products

        public static IDataReader ExistsChild(
            int productID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Product_ExistsChild", 1);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, productID);
            return sph.ExecuteReader();
        }

        #endregion Child Products
    }
}
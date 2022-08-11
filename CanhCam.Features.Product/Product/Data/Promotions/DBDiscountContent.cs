/// Author:					Tran Quoc Vuong
/// Created:				2020-11-03
/// Last Modified:			2020-11-03

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscountContent
    {
        /// <summary>
        /// Inserts a row in the gb_DiscountContent table. Returns new integer id.
        /// </summary>
        /// <param name="discountID"> discountID </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="bannerFile"> bannerFile </param>
        /// <param name="thumbnailFile"> thumbnailFile </param>
        /// <param name="contentType"> contentType </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="loadType"> loadType </param>
        /// <param name="fromPrice"> fromPrice </param>
        /// <param name="toPrice"> toPrice </param>
        /// <param name="zoneIDs"> zoneIDs </param>
        /// <param name="options"> options </param>
        /// <param name="guid"> guid </param>
        /// <returns>int</returns>
        public static int Create(
            int discountID,
            string title,
            string description,
            string bannerFile,
            string thumbnailFile,
            int contentType,
            int displayOrder,
            int loadType,
            decimal fromPrice,
            decimal toPrice,
            string zoneIDs,
            int options,
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountContent_Insert", 13);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@BannerFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, bannerFile);
            sph.DefineSqlParameter("@ThumbnailFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, thumbnailFile);
            sph.DefineSqlParameter("@ContentType", SqlDbType.Int, ParameterDirection.Input, contentType);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@LoadType", SqlDbType.Int, ParameterDirection.Input, loadType);
            sph.DefineSqlParameter("@FromPrice", SqlDbType.Decimal, ParameterDirection.Input, fromPrice);
            sph.DefineSqlParameter("@ToPrice", SqlDbType.Decimal, ParameterDirection.Input, toPrice);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, zoneIDs);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_DiscountContent table. Returns true if row updated.
        /// </summary>
        /// <param name="contentID"> contentID </param>
        /// <param name="discountID"> discountID </param>
        /// <param name="title"> title </param>
        /// <param name="description"> description </param>
        /// <param name="bannerFile"> bannerFile </param>
        /// <param name="thumbnailFile"> thumbnailFile </param>
        /// <param name="contentType"> contentType </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="loadType"> loadType </param>
        /// <param name="fromPrice"> fromPrice </param>
        /// <param name="toPrice"> toPrice </param>
        /// <param name="zoneIDs"> zoneIDs </param>
        /// <param name="options"> options </param>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Update(
            int contentID,
            int discountID,
            string title,
            string description,
            string bannerFile,
            string thumbnailFile,
            int contentType,
            int displayOrder,
            int loadType,
            decimal fromPrice,
            decimal toPrice,
            string zoneIDs,
            int options,
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountContent_Update", 14);
            sph.DefineSqlParameter("@ContentID", SqlDbType.Int, ParameterDirection.Input, contentID);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@BannerFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, bannerFile);
            sph.DefineSqlParameter("@ThumbnailFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, thumbnailFile);
            sph.DefineSqlParameter("@ContentType", SqlDbType.Int, ParameterDirection.Input, contentType);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@LoadType", SqlDbType.Int, ParameterDirection.Input, loadType);
            sph.DefineSqlParameter("@FromPrice", SqlDbType.Decimal, ParameterDirection.Input, fromPrice);
            sph.DefineSqlParameter("@ToPrice", SqlDbType.Decimal, ParameterDirection.Input, toPrice);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, zoneIDs);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_DiscountContent table. Returns true if row deleted.
        /// </summary>
        /// <param name="contentID"> contentID </param>
        /// <returns>bool</returns>
        public static bool Delete(int contentID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountContent_Delete", 1);
            sph.DefineSqlParameter("@ContentID", SqlDbType.Int, ParameterDirection.Input, contentID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByDiscount(int discountID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountContent_DeleteByDiscount", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_DiscountContent table.
        /// </summary>
        /// <param name="contentID"> contentID </param>
        public static IDataReader GetOne(
            int contentID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountContent_SelectOne", 1);
            sph.DefineSqlParameter("@ContentID", SqlDbType.Int, ParameterDirection.Input, contentID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByDiscount(int discountID, int languageID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountContent_SelectByDiscount", 2);
            sph.DefineSqlParameter("@ContentID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageID);
            return sph.ExecuteReader();
        }

        public static int GetMaxSortOrder(int discountID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountContent_GetMaxSortOrder", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            int pageOrder = Convert.ToInt32(sph.ExecuteScalar());
            return pageOrder;
        }
    }
}
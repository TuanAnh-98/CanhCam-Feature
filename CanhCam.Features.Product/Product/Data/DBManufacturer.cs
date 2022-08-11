/// Author:					Tran Quoc Vuong
/// Created:				2017-03-30
/// Last Modified:			2017-03-30

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBManufacturer
    {
        /// <summary>
        /// Inserts a row in the gb_Manufacturer table. Returns new integer id.
        /// </summary>
        /// <param name="pageID"> pageID </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="name"> name </param>
        /// <param name="url"> url </param>
        /// <param name="description"> description </param>
        /// <param name="primaryImage"> primaryImage </param>
        /// <param name="secondImage"> secondImage </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="showOption"> showOption </param>
        /// <param name="isPublished"> isPublished </param>
        /// <param name="guid"> guid </param>
        /// <param name="metaTitle"> metaTitle </param>
        /// <param name="metaKeywords"> metaKeywords </param>
        /// <param name="metaDescription"> metaDescription </param>
        /// <param name="createdDate"> createdDate </param>
        /// <param name="modifiedDate"> modifiedDate </param>
        /// <param name="isDeleted"> isDeleted </param>
        /// <returns>int</returns>
        public static int Create(
            int pageID,
            int siteID,
            string name,
            string url,
            string description,
            string primaryImage,
            string secondImage,
            int displayOrder,
            int showOption,
            bool isPublished,
            Guid guid,
            string metaTitle,
            string metaKeywords,
            string metaDescription,
            DateTime createdDate,
            DateTime modifiedDate,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Manufacturer_Insert", 17);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@PrimaryImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, primaryImage);
            sph.DefineSqlParameter("@SecondImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, secondImage);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@MetaTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaTitle);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 500, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 500, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@ModifiedDate", SqlDbType.DateTime, ParameterDirection.Input, modifiedDate);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_Manufacturer table. Returns true if row updated.
        /// </summary>
        /// <param name="manufacturerID"> manufacturerID </param>
        /// <param name="pageID"> pageID </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="name"> name </param>
        /// <param name="url"> url </param>
        /// <param name="description"> description </param>
        /// <param name="primaryImage"> primaryImage </param>
        /// <param name="secondImage"> secondImage </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="showOption"> showOption </param>
        /// <param name="isPublished"> isPublished </param>
        /// <param name="guid"> guid </param>
        /// <param name="metaTitle"> metaTitle </param>
        /// <param name="metaKeywords"> metaKeywords </param>
        /// <param name="metaDescription"> metaDescription </param>
        /// <param name="createdDate"> createdDate </param>
        /// <param name="modifiedDate"> modifiedDate </param>
        /// <param name="isDeleted"> isDeleted </param>
        /// <returns>bool</returns>
        public static bool Update(
            int manufacturerID,
            int pageID,
            int siteID,
            string name,
            string url,
            string description,
            string primaryImage,
            string secondImage,
            int displayOrder,
            int showOption,
            bool isPublished,
            Guid guid,
            string metaTitle,
            string metaKeywords,
            string metaDescription,
            DateTime createdDate,
            DateTime modifiedDate,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Manufacturer_Update", 18);
            sph.DefineSqlParameter("@ManufacturerID", SqlDbType.Int, ParameterDirection.Input, manufacturerID);
            sph.DefineSqlParameter("@PageID", SqlDbType.Int, ParameterDirection.Input, pageID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@PrimaryImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, primaryImage);
            sph.DefineSqlParameter("@SecondImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, secondImage);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@MetaTitle", SqlDbType.NVarChar, 255, ParameterDirection.Input, metaTitle);
            sph.DefineSqlParameter("@MetaKeywords", SqlDbType.NVarChar, 500, ParameterDirection.Input, metaKeywords);
            sph.DefineSqlParameter("@MetaDescription", SqlDbType.NVarChar, 500, ParameterDirection.Input, metaDescription);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@ModifiedDate", SqlDbType.DateTime, ParameterDirection.Input, modifiedDate);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_Manufacturer table. Returns true if row deleted.
        /// </summary>
        /// <param name="manufacturerID"> manufacturerID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int manufacturerID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Manufacturer_Delete", 1);
            sph.DefineSqlParameter("@ManufacturerID", SqlDbType.Int, ParameterDirection.Input, manufacturerID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_Manufacturer table.
        /// </summary>
        /// <param name="manufacturerID"> manufacturerID </param>
        public static IDataReader GetOne(
            int manufacturerID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Manufacturer_SelectOne", 1);
            sph.DefineSqlParameter("@ManufacturerID", SqlDbType.Int, ParameterDirection.Input, manufacturerID);
            return sph.ExecuteReader();
        }

        public static int GetCountByTitle(int siteId, string title)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Manufacturer_GetCountByTitle", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Title", SqlDbType.Int, ParameterDirection.Input, title);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static int GetCount(int siteId, int publishStatus, Guid? zoneGuid, string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Manufacturer_GetCount", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@ZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, zoneGuid);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetAll(int siteId, int publishStatus, Guid? zoneGuid, int showOption)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Manufacturer_Select", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@ZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, zoneGuid);
            sph.DefineSqlParameter("@ShowOption", SqlDbType.Int, ParameterDirection.Input, showOption);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPage(int siteId, int publishStatus, Guid? zoneGuid, int pageNumber, int pageSize, string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Manufacturer_SelectPage", 6);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@PublishStatus", SqlDbType.Int, ParameterDirection.Input, publishStatus);
            sph.DefineSqlParameter("@ZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, zoneGuid);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
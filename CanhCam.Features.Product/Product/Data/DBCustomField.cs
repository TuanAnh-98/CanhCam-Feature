// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2014-7-24
// Last Modified:			2014-7-24

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBCustomField
    {
        public static int Create(
            int siteID,
            string name,
            string displayName,
            int dataType,
            int fieldType,
            int filterType,
            bool isEnabled,
            bool isRequired,
            string validationExpression,
            string invalidMessage,
            bool allowComparing,
            int displayOrder,
            Guid featureGuid,
            Guid guid,
            int options)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomField_Insert", 15);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@DisplayName", SqlDbType.NVarChar, 255, ParameterDirection.Input, displayName);
            sph.DefineSqlParameter("@DataType", SqlDbType.Int, ParameterDirection.Input, dataType);
            sph.DefineSqlParameter("@FieldType", SqlDbType.Int, ParameterDirection.Input, fieldType);
            sph.DefineSqlParameter("@FilterType", SqlDbType.Int, ParameterDirection.Input, filterType);
            sph.DefineSqlParameter("@IsEnabled", SqlDbType.Bit, ParameterDirection.Input, isEnabled);
            sph.DefineSqlParameter("@IsRequired", SqlDbType.Bit, ParameterDirection.Input, isRequired);
            sph.DefineSqlParameter("@ValidationExpression", SqlDbType.NVarChar, -1, ParameterDirection.Input, validationExpression);
            sph.DefineSqlParameter("@InvalidMessage", SqlDbType.NVarChar, 255, ParameterDirection.Input, invalidMessage);
            sph.DefineSqlParameter("@AllowComparing", SqlDbType.Bit, ParameterDirection.Input, allowComparing);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int customFieldID,
            int siteID,
            string name,
            string displayName,
            int dataType,
            int fieldType,
            int filterType,
            bool isEnabled,
            bool isRequired,
            string validationExpression,
            string invalidMessage,
            bool allowComparing,
            int displayOrder,
            Guid featureGuid,
            Guid guid,
            int options)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomField_Update", 16);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@DisplayName", SqlDbType.NVarChar, 255, ParameterDirection.Input, displayName);
            sph.DefineSqlParameter("@DataType", SqlDbType.Int, ParameterDirection.Input, dataType);
            sph.DefineSqlParameter("@FieldType", SqlDbType.Int, ParameterDirection.Input, fieldType);
            sph.DefineSqlParameter("@FilterType", SqlDbType.Int, ParameterDirection.Input, filterType);
            sph.DefineSqlParameter("@IsEnabled", SqlDbType.Bit, ParameterDirection.Input, isEnabled);
            sph.DefineSqlParameter("@IsRequired", SqlDbType.Bit, ParameterDirection.Input, isRequired);
            sph.DefineSqlParameter("@ValidationExpression", SqlDbType.NVarChar, -1, ParameterDirection.Input, validationExpression);
            sph.DefineSqlParameter("@InvalidMessage", SqlDbType.NVarChar, 255, ParameterDirection.Input, invalidMessage);
            sph.DefineSqlParameter("@AllowComparing", SqlDbType.Bit, ParameterDirection.Input, allowComparing);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(
            int customFieldID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomField_Delete", 1);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int customFieldID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomField_SelectOne", 1);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByFeature(int siteId, Guid featureGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomField_SelectByFeature", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetActive(int siteId, Guid featureGuid, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomField_SelectActive", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetActiveByZone(int siteId, Guid featureGuid, Guid zoneGuid, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomField_SelectActiveByZone", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@ZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, zoneGuid);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetActiveByFields(int siteId, Guid featureGuid, string fieldIds, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomField_SelectActiveByFields", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@FeatureGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, featureGuid);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.NVarChar, ParameterDirection.Input, fieldIds);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }
    }
}
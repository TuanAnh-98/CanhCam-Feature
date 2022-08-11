// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2014-7-23
// Last Modified:			2014-7-23

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBCustomFieldOption
    {
        /// <summary>
        /// Inserts a row in the gb_CustomFieldOption table. Returns new integer id.
        /// </summary>
        /// <param name="customFieldID"> customFieldID </param>
        /// <param name="name"> name </param>
        /// <param name="optionType"> optionType </param>
        /// <param name="optionColor"> optionColor </param>
        /// <param name="fromValue"> fromValue </param>
        /// <param name="toValue"> toValue </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="guid"> guid </param>
        /// <returns>int</returns>
        public static int Create(
            int customFieldID,
            string name,
            int optionType,
            string optionColor,
            decimal fromValue,
            decimal toValue,
            int displayOrder,
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomFieldOption_Insert", 8);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@OptionType", SqlDbType.Int, ParameterDirection.Input, optionType);
            sph.DefineSqlParameter("@OptionColor", SqlDbType.NVarChar, 255, ParameterDirection.Input, optionColor);
            sph.DefineSqlParameter("@FromValue", SqlDbType.Decimal, ParameterDirection.Input, fromValue);
            sph.DefineSqlParameter("@ToValue", SqlDbType.Decimal, ParameterDirection.Input, toValue);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_CustomFieldOption table. Returns true if row updated.
        /// </summary>
        /// <param name="customFieldOptionID"> customFieldOptionID </param>
        /// <param name="customFieldID"> customFieldID </param>
        /// <param name="name"> name </param>
        /// <param name="optionType"> optionType </param>
        /// <param name="optionColor"> optionColor </param>
        /// <param name="fromValue"> fromValue </param>
        /// <param name="toValue"> toValue </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Update(
            int customFieldOptionID,
            int customFieldID,
            string name,
            int optionType,
            string optionColor,
            decimal fromValue,
            decimal toValue,
            int displayOrder,
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomFieldOption_Update", 9);
            sph.DefineSqlParameter("@CustomFieldOptionID", SqlDbType.Int, ParameterDirection.Input, customFieldOptionID);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@OptionType", SqlDbType.Int, ParameterDirection.Input, optionType);
            sph.DefineSqlParameter("@OptionColor", SqlDbType.NVarChar, 255, ParameterDirection.Input, optionColor);
            sph.DefineSqlParameter("@FromValue", SqlDbType.Decimal, ParameterDirection.Input, fromValue);
            sph.DefineSqlParameter("@ToValue", SqlDbType.Decimal, ParameterDirection.Input, toValue);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int customFieldOptionID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomFieldOption_Delete", 1);
            sph.DefineSqlParameter("@CustomFieldOptionID", SqlDbType.Int, ParameterDirection.Input, customFieldOptionID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteCustomField(int customFieldID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_CustomFieldOption_DeleteByField", 1);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int customFieldOptionID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomFieldOption_SelectOne", 1);
            sph.DefineSqlParameter("@CustomFieldOptionID", SqlDbType.Int, ParameterDirection.Input, customFieldOptionID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByCustomField(int customFieldId, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomFieldOption_SelectByField", 2);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldId);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByOptionIds(int siteId, string optionIds, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomFieldOption_SelectByOptionIDs", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@CustomFieldOptionIDs", SqlDbType.NVarChar, ParameterDirection.Input, optionIds);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// gets the max sort rank or 1 if null
        /// </summary>
        /// <param name="customFieldId"></param>
        /// <returns>int</returns>
        public static int GetMaxSortOrder(int customFieldId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_CustomFieldOption_GetMaxSortOrder", 1);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldId);
            int pageOrder = Convert.ToInt32(sph.ExecuteScalar());
            return pageOrder;
        }
    }
}
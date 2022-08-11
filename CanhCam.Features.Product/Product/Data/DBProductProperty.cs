// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2014-7-26
// Last Modified:			2014-7-26

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProductProperty
    {
        public static int Create(
            Guid guid,
            int productID,
            int customFieldID,
            int customFieldOptionID,
            string customValue,
            int stockQuantity,
            decimal overriddenPrice)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductProperties_Insert", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            sph.DefineSqlParameter("@CustomFieldOptionID", SqlDbType.Int, ParameterDirection.Input, customFieldOptionID);
            sph.DefineSqlParameter("@CustomValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, customValue);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            sph.DefineSqlParameter("@OverriddenPrice", SqlDbType.Decimal, ParameterDirection.Input, overriddenPrice);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        public static bool Update(
            Guid guid,
            int productID,
            int customFieldID,
            int customFieldOptionID,
            string customValue,
            int stockQuantity,
            decimal overriddenPrice)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductProperties_Update", 7);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldID);
            sph.DefineSqlParameter("@CustomFieldOptionID", SqlDbType.Int, ParameterDirection.Input, customFieldOptionID);
            sph.DefineSqlParameter("@CustomValue", SqlDbType.NVarChar, -1, ParameterDirection.Input, customValue);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            sph.DefineSqlParameter("@OverriddenPrice", SqlDbType.Decimal, ParameterDirection.Input, overriddenPrice);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByProduct(int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductProperties_DeleteByProduct", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByCustomField(int customFieldId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductProperties_DeleteByField", 1);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static bool DeleteByCustomFieldOption(int customFieldOptionId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductProperties_DeleteByFieldOption", 1);
            sph.DefineSqlParameter("@CustomFieldOptionID", SqlDbType.Int, ParameterDirection.Input, customFieldOptionId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }

        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductProperties_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPropertiesByProduct(int productId, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductProperties_SelectByProduct", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPropertiesByProducts(string productIds, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductProperties_SelectByProducts", 2);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPropertiesByField(int customFieldId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductProperties_SelectByField", 1);
            sph.DefineSqlParameter("@CustomFieldID", SqlDbType.Int, ParameterDirection.Input, customFieldId);
            return sph.ExecuteReader();
        }
    }
}
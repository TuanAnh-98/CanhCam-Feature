// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2014-06-30
// Last Modified:			2014-06-30

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBShoppingCartItem
    {
        public static int Create(
            Guid guid,
            int siteID,
            int productID,
            Guid userGuid,
            string attributesXml,
            int quantity,
            int shoppingCartType,
            string createdFromIP,
            DateTime createdUtc,
            DateTime lastModUtc,
            int affiliateUserID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_Insert", 10);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@AttributesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributesXml);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@ShoppingCartType", SqlDbType.Int, ParameterDirection.Input, shoppingCartType);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        public static bool Update(
            Guid guid,
            int siteID,
            int productID,
            Guid userGuid,
            string attributesXml,
            int quantity,
            int shoppingCartType,
            string createdFromIP,
            DateTime createdUtc,
            DateTime lastModUtc,
            int affiliateUserID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_Update", 10);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@AttributesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributesXml);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@ShoppingCartType", SqlDbType.Int, ParameterDirection.Input, shoppingCartType);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@LastModUtc", SqlDbType.DateTime, ParameterDirection.Input, lastModUtc);
            sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByProduct(int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_DeleteByProduct", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool MoveToUser(Guid userGuid, Guid newUserGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_MoveToUser", 2);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@NewUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, newUserGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteBySite(int siteId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_DeleteBySite", 1);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteOlderThan(int siteId, int shoppingCartType, DateTime olderThanUtc)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShoppingCartItem_DeleteOlderThan", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.DateTime, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ShoppingCartType", SqlDbType.Int, ParameterDirection.Input, shoppingCartType);
            sph.DefineSqlParameter("@OlderThanUtc", SqlDbType.DateTime, ParameterDirection.Input, olderThanUtc);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShoppingCartItem_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static int GetCount(int siteId, int shoppingCartType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShoppingCartItem_GetCount", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ShoppingCartType", SqlDbType.Int, ParameterDirection.Input, shoppingCartType);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetByUserGuid(int siteId, int shoppingCartType, Guid userGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShoppingCartItem_SelectByUserGuid", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ShoppingCartType", SqlDbType.Int, ParameterDirection.Input, shoppingCartType);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetPage(
            int siteId,
            int shoppingCartType,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShoppingCartItem_SelectPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ShoppingCartType", SqlDbType.Int, ParameterDirection.Input, shoppingCartType);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
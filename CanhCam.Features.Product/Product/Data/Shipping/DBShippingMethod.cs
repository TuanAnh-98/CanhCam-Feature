/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-04-24
/// Last Modified:			2015-04-24

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBShippingMethod
    {
        public static int Create(
            int siteID,
            int shippingProvider,
            string name,
            string description,
            decimal shippingFee,
            bool freeShippingOverXEnabled,
            decimal freeShippingOverXValue,
            int displayOrder,
            bool isActive,
            Guid guid,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingMethod_Insert", 11);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ShippingProvider", SqlDbType.Int, ParameterDirection.Input, shippingProvider);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@ShippingFee", SqlDbType.Decimal, ParameterDirection.Input, shippingFee);
            sph.DefineSqlParameter("@FreeShippingOverXEnabled", SqlDbType.Bit, ParameterDirection.Input, freeShippingOverXEnabled);
            sph.DefineSqlParameter("@FreeShippingOverXValue", SqlDbType.Decimal, ParameterDirection.Input, freeShippingOverXValue);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int shippingMethodID,
            int siteID,
            int shippingProvider,
            string name,
            string description,
            decimal shippingFee,
            bool freeShippingOverXEnabled,
            decimal freeShippingOverXValue,
            int displayOrder,
            bool isActive,
            Guid guid,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingMethod_Update", 12);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ShippingProvider", SqlDbType.Int, ParameterDirection.Input, shippingProvider);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@ShippingFee", SqlDbType.Decimal, ParameterDirection.Input, shippingFee);
            sph.DefineSqlParameter("@FreeShippingOverXEnabled", SqlDbType.Bit, ParameterDirection.Input, freeShippingOverXEnabled);
            sph.DefineSqlParameter("@FreeShippingOverXValue", SqlDbType.Decimal, ParameterDirection.Input, freeShippingOverXValue);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int shippingMethodId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingMethod_Delete", 1);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int shippingMethodId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShippingMethod_SelectOne", 1);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByActive(int siteId, int activeStatus, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShippingMethod_SelectByActive", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ActiveStatus", SqlDbType.Int, ParameterDirection.Input, activeStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);

            return sph.ExecuteReader();
        }
    }
}
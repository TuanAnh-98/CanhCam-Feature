/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-05-05
/// Last Modified:			2015-05-05

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBShippingTableRate
    {
        public static int Create(
            int shippingMethodID,
            Guid geoZoneGuid,
            decimal fromValue,
            decimal shippingFee,
            decimal additionalValue,
            decimal additionalFee,
            decimal freeShippingOverXValue,
            int storeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingTableRate_Insert", 8);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodID);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@FromValue", SqlDbType.Decimal, ParameterDirection.Input, fromValue);
            sph.DefineSqlParameter("@ShippingFee", SqlDbType.Decimal, ParameterDirection.Input, shippingFee);
            sph.DefineSqlParameter("@AdditionalValue", SqlDbType.Decimal, ParameterDirection.Input, additionalValue);
            sph.DefineSqlParameter("@AdditionalFee", SqlDbType.Decimal, ParameterDirection.Input, additionalFee);
            sph.DefineSqlParameter("@FreeShippingOverXValue", SqlDbType.Decimal, ParameterDirection.Input, freeShippingOverXValue);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int shippingTableRateID,
            int shippingMethodID,
            Guid geoZoneGuid,
            decimal fromValue,
            decimal shippingFee,
            decimal additionalValue,
            decimal additionalFee,
            decimal freeShippingOverXValue,
            int storeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingTableRate_Update", 9);
            sph.DefineSqlParameter("@ShippingTableRateID", SqlDbType.Int, ParameterDirection.Input, shippingTableRateID);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodID);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@FromValue", SqlDbType.Decimal, ParameterDirection.Input, fromValue);
            sph.DefineSqlParameter("@ShippingFee", SqlDbType.Decimal, ParameterDirection.Input, shippingFee);
            sph.DefineSqlParameter("@AdditionalValue", SqlDbType.Decimal, ParameterDirection.Input, additionalValue);
            sph.DefineSqlParameter("@AdditionalFee", SqlDbType.Decimal, ParameterDirection.Input, additionalFee);
            sph.DefineSqlParameter("@FreeShippingOverXValue", SqlDbType.Decimal, ParameterDirection.Input, freeShippingOverXValue);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int shippingTableRateId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingTableRate_Delete", 1);
            sph.DefineSqlParameter("@ShippingTableRateID", SqlDbType.Int, ParameterDirection.Input, shippingTableRateId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByMethod(int shippingMethodId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ShippingTableRate_DeleteByMethod", 1);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int shippingTableRateID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShippingTableRate_SelectOne", 1);
            sph.DefineSqlParameter("@ShippingTableRateID", SqlDbType.Int, ParameterDirection.Input, shippingTableRateID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByMethod(int shippingMethodId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShippingTableRate_SelectByMethod", 1);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOneMaxValue(int shippingMethodId, decimal fromValue, string geoZoneGuids, int storeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ShippingTableRate_SelectOneMaxValue", 4);
            sph.DefineSqlParameter("@ShippingMethodID", SqlDbType.Int, ParameterDirection.Input, shippingMethodId);
            sph.DefineSqlParameter("@FromValue", SqlDbType.Decimal, ParameterDirection.Input, fromValue);
            sph.DefineSqlParameter("@GeoZoneGuids", SqlDbType.NVarChar, -1, ParameterDirection.Input, geoZoneGuids);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            return sph.ExecuteReader();
        }
    }
}
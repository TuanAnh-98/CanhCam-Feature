// Author:					Canh cam
// Created:					2021-3-11
// Last Modified:			2021-3-11

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBRestrictedShipping
    {
        /// <summary>
        /// Inserts a row in the gb_RestrictedShipping table. Returns new integer id.
        /// </summary>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="weight"> weight </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="shippingMethodIds"> shippingMethodIds </param>
        /// <param name="paymentMethodIds"> paymentMethodIds </param>
        /// <param name="storeId"> storeId </param>
        /// <returns>int</returns>
        public static int Create(
            Guid geoZoneGuid,
            int weight,
            decimal orderTotal,
            string shippingMethodIds,
            string paymentMethodIds,
            int storeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_RestrictedShipping_Insert", 6);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@Weight", SqlDbType.Int, ParameterDirection.Input, weight);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@ShippingMethodIds", SqlDbType.VarChar, 255, ParameterDirection.Input, shippingMethodIds);
            sph.DefineSqlParameter("@PaymentMethodIds", SqlDbType.VarChar, 255, ParameterDirection.Input, paymentMethodIds);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_RestrictedShipping table. Returns true if row updated.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <param name="geoZoneGuid"> geoZoneGuid </param>
        /// <param name="weight"> weight </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="shippingMethodIds"> shippingMethodIds </param>
        /// <param name="paymentMethodIds"> paymentMethodIds </param>
        /// <param name="storeId"> storeId </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            Guid geoZoneGuid,
            int weight,
            decimal orderTotal,
            string shippingMethodIds,
            string paymentMethodIds,
            int storeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_RestrictedShipping_Update", 7);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@Weight", SqlDbType.Int, ParameterDirection.Input, weight);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@ShippingMethodIds", SqlDbType.VarChar, 255, ParameterDirection.Input, shippingMethodIds);
            sph.DefineSqlParameter("@PaymentMethodIds", SqlDbType.VarChar, 255, ParameterDirection.Input, paymentMethodIds);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_RestrictedShipping table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_RestrictedShipping_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_RestrictedShipping table.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        public static IDataReader GetOne(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RestrictedShipping_SelectOne", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }
        public static IDataReader GetOne(Guid geoZoneGuid, int storeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RestrictedShipping_SelectOneByGeoZone", 2);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            return sph.ExecuteReader();
        }
        public static IDataReader GetOne(Guid geoZoneGuid, int storeId, decimal orderWeight)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RestrictedShipping_SelectOneByGeoZoneAndWeight", 3);
            sph.DefineSqlParameter("@GeoZoneGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, geoZoneGuid);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            sph.DefineSqlParameter("@OrderWeight", SqlDbType.Decimal, ParameterDirection.Input, orderWeight);
            return sph.ExecuteReader();
        }
        public static IDataReader GetOne(string geoZoneGuids, int storeId, decimal orderWeight)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RestrictedShipping_SelectOneByGeoZonesAndWeight", 3);
            sph.DefineSqlParameter("@GeoZoneGuids", SqlDbType.NVarChar,255, ParameterDirection.Input, geoZoneGuids);
            sph.DefineSqlParameter("@StoreId", SqlDbType.Int, ParameterDirection.Input, storeId);
            sph.DefineSqlParameter("@OrderWeight", SqlDbType.Decimal, ParameterDirection.Input, orderWeight);
            return sph.ExecuteReader();
        }
        /// <summary>
        /// Gets a count of rows in the gb_RestrictedShipping table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_RestrictedShipping_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_RestrictedShipping table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_RestrictedShipping_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_RestrictedShipping table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static IDataReader GetPage(
            int pageNumber,
            int pageSize,
            out int totalPages)
        {
            totalPages = 1;
            int totalRows
                = GetCount();

            if (pageSize > 0) totalPages = totalRows / pageSize;

            if (totalRows <= pageSize)
            {
                totalPages = 1;
            }
            else
            {
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RestrictedShipping_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
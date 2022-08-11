// Author:					Vu Minh Tri
// Created:					2018-9-7
// Last Modified:			2018-9-7

using System;
using System.Data;

namespace CanhCam.Data
{

    public static class DBViettelPostCode
    {

        /// <summary>
        /// Inserts a row in the gb_ViettelPostCode table. Returns new integer id.
        /// </summary>
        /// <param name="geoZoneCode"> geoZoneCode </param>
        /// <param name="viettelPostCode"> viettelPostCode </param>
        /// <returns>int</returns>
        public static int Create(
            string geoZoneCode,
            string viettelPostCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ViettelPostCode_Insert", 2);
            sph.DefineSqlParameter("@GeoZoneCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, geoZoneCode);
            sph.DefineSqlParameter("@ViettelPostCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, viettelPostCode);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the gb_ViettelPostCode table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="geoZoneCode"> geoZoneCode </param>
        /// <param name="viettelPostCode"> viettelPostCode </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowID,
            string geoZoneCode,
            string viettelPostCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ViettelPostCode_Update", 3);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowID);
            sph.DefineSqlParameter("@GeoZoneCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, geoZoneCode);
            sph.DefineSqlParameter("@ViettelPostCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, viettelPostCode);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the gb_ViettelPostCode table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ViettelPostCode_Delete", 1);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_ViettelPostCode table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(
            int rowID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostCode_SelectOne", 1);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowID);
            return sph.ExecuteReader();

        }
        /// <summary>
        /// Gets an IDataReader with one row from the gb_ViettelPostCode table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(
            string geozoneCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostCode_SelectOneByGeoZoneCode", 1);
            sph.DefineSqlParameter("@GeoZoneCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, geozoneCode);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the gb_ViettelPostCode table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ViettelPostCode_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_ViettelPostCode table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ViettelPostCode_SelectAll",
                null);

        }

        /// <summary>
        /// Gets a page of data from the gb_ViettelPostCode table.
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
                Math.DivRem(totalRows, pageSize, out int remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostCode_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetAllByProvince(Guid provinceGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostCode_SelectAllByProvince", 1);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            return sph.ExecuteReader();

        }
        public static IDataReader GetAllProvinceByCountry(Guid countryGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostCode_SelectAllProvinceByCountry", 1);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            return sph.ExecuteReader();
        }
    }

}

// Author:					Vu Minh Tri
// Created:					2018-9-9
// Last Modified:			2018-9-9

using System;
using System.Data;

namespace CanhCam.Data
{

    public static class DBViettelPostProvince
    {

        /// <summary>
        /// Inserts a row in the gb_ViettelPostProvince table. Returns new integer id.
        /// </summary>
        /// <param name="provinceGuid"> provinceGuid </param>
        /// <param name="viettelPostProvinceCode"> viettelPostProvinceCode </param>
        /// <returns>int</returns>
        public static int Create(
            Guid provinceGuid,
            string viettelPostProvinceCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ViettelPostProvince_Insert", 2);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@ViettelPostProvinceCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, viettelPostProvinceCode);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the gb_ViettelPostProvince table. Returns true if row updated.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <param name="provinceGuid"> provinceGuid </param>
        /// <param name="viettelPostProvinceCode"> viettelPostProvinceCode </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowID,
            Guid provinceGuid,
            string viettelPostProvinceCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ViettelPostProvince_Update", 3);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowID);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@ViettelPostProvinceCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, viettelPostProvinceCode);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the gb_ViettelPostProvince table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ViettelPostProvince_Delete", 1);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_ViettelPostProvince table.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(
            int rowID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostProvince_SelectOne", 1);
            sph.DefineSqlParameter("@RowID", SqlDbType.Int, ParameterDirection.Input, rowID);
            return sph.ExecuteReader();

        }   /// <summary>
            /// Gets an IDataReader with one row from the gb_ViettelPostProvince table.
            /// </summary>
            /// <param name="rowID"> rowID </param>
        public static IDataReader GetOne(
          Guid provinceGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostProvince_SelectOneByProvinceGuid", 1);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the gb_ViettelPostProvince table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ViettelPostProvince_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_ViettelPostProvince table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ViettelPostProvince_SelectAll",
                null);

        }

        /// <summary>
        /// Gets a page of data from the gb_ViettelPostProvince table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ViettelPostProvince_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

    }

}

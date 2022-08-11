// Author:					Canh cam
// Created:					2021-1-26
// Last Modified:			2021-1-26

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProductType
    {
        /// <summary>
        /// Inserts a row in the gb_ProductType table. Returns new integer id.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="name"> name </param>
        /// <param name="description"> description </param>
        /// <param name="primaryImage"> primaryImage </param>
        /// <param name="secondImage"> secondImage </param>
        /// <returns>int</returns>
        public static int Create(
            Guid itemGuid,
            int siteID,
            string name,
            string description,
            string primaryImage,
            string secondImage)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductType_Insert", 6);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@PrimaryImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, primaryImage);
            sph.DefineSqlParameter("@SecondImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, secondImage);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_ProductType table. Returns true if row updated.
        /// </summary>
        /// <param name="productTypeId"> productTypeId </param>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="name"> name </param>
        /// <param name="description"> description </param>
        /// <param name="primaryImage"> primaryImage </param>
        /// <param name="secondImage"> secondImage </param>
        /// <returns>bool</returns>
        public static bool Update(
            int productTypeId,
            Guid itemGuid,
            int siteID,
            string name,
            string description,
            string primaryImage,
            string secondImage)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductType_Update", 7);
            sph.DefineSqlParameter("@ProductTypeId", SqlDbType.Int, ParameterDirection.Input, productTypeId);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@PrimaryImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, primaryImage);
            sph.DefineSqlParameter("@SecondImage", SqlDbType.NVarChar, 255, ParameterDirection.Input, secondImage);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_ProductType table. Returns true if row deleted.
        /// </summary>
        /// <param name="productTypeId"> productTypeId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int productTypeId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductType_Delete", 1);
            sph.DefineSqlParameter("@ProductTypeId", SqlDbType.Int, ParameterDirection.Input, productTypeId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_ProductType table.
        /// </summary>
        /// <param name="productTypeId"> productTypeId </param>
        public static IDataReader GetOne(
            int productTypeId, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductType_SelectOne", 2);
            sph.DefineSqlParameter("@ProductTypeId", SqlDbType.Int, ParameterDirection.Input, productTypeId);
            sph.DefineSqlParameter("@LanguageId", SqlDbType.Int, ParameterDirection.Input, languageId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_ProductType table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ProductType_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_ProductType table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ProductType_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_ProductType table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductType_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
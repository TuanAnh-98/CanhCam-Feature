// Author:					Canh cam
// Created:					2020-11-3
// Last Modified:			2020-11-3

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProductSoldOutLetter
    {
        /// <summary>
        /// Inserts a row in the gb_ProductSoldOutLetter table. Returns new integer id.
        /// </summary>
        /// <param name="productID"> productID </param>
        /// <param name="productName"> productName </param>
        /// <param name="quantity"> quantity </param>
        /// <param name="url"> url </param>
        /// <param name="email"> email </param>
        /// <param name="fullName"> fullName </param>
        /// <param name="phone"> phone </param>
        /// <param name="userId"> userId </param>
        /// <param name="status"> status </param>
        /// <param name="createDate"> createDate </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <returns>int</returns>
        public static int Create(
            int productID,
            string productName,
            int quantity,
            string url,
            string email,
            string fullName,
            string phone,
            int userId,
            int status,
            DateTime createDate,
            string ipAddress,
            bool isContacted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductSoldOutLetter_Insert", 12);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ProductName", SqlDbType.NVarChar, 255, ParameterDirection.Input, productName);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@FullName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fullName);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@CreateDate", SqlDbType.DateTime, ParameterDirection.Input, createDate);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@IsContacted", SqlDbType.Bit, ParameterDirection.Input, isContacted);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_ProductSoldOutLetter table. Returns true if row updated.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <param name="productID"> productID </param>
        /// <param name="productName"> productName </param>
        /// <param name="quantity"> quantity </param>
        /// <param name="url"> url </param>
        /// <param name="email"> email </param>
        /// <param name="fullName"> fullName </param>
        /// <param name="phone"> phone </param>
        /// <param name="userId"> userId </param>
        /// <param name="status"> status </param>
        /// <param name="createDate"> createDate </param>
        /// <param name="ipAddress"> ipAddress </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            int productID,
            string productName,
            int quantity,
            string url,
            string email,
            string fullName,
            string phone,
            int userId,
            int status,
            DateTime createDate,
            string ipAddress,
            bool isContacted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductSoldOutLetter_Update", 13);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ProductName", SqlDbType.NVarChar, 255, ParameterDirection.Input, productName);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@FullName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fullName);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@CreateDate", SqlDbType.DateTime, ParameterDirection.Input, createDate);
            sph.DefineSqlParameter("@IpAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, ipAddress);
            sph.DefineSqlParameter("@IsContacted", SqlDbType.Bit, ParameterDirection.Input, isContacted);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_ProductSoldOutLetter table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductSoldOutLetter_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_ProductSoldOutLetter table.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        public static IDataReader GetOne(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductSoldOutLetter_SelectOne", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_ProductSoldOutLetter table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ProductSoldOutLetter_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_ProductSoldOutLetter table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ProductSoldOutLetter_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_ProductSoldOutLetter table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductSoldOutLetter_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountySearch(DateTime? startDate,
            DateTime? endDate,
            string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductSoldOutLetter_GetCountBySearch", 3);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageBySearch(DateTime? startDate,
            DateTime? endDate,
            string keyword,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductSoldOutLetter_SelectPageBySearch", 5);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
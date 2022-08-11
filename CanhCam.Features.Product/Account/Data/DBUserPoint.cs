// Author:					a
// Created:					2020-3-3
// Last Modified:			2020-3-3

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBUserPoint
    {
        /// <summary>
        /// Inserts a row in the gb_UserPoint table. Returns new integer id.
        /// </summary>
        /// <param name="userID"> userID </param>
        /// <param name="point"> point </param>
        /// <param name="amount"> amount </param>
        /// <param name="type"> type </param>
        /// <param name="orderID"> orderID </param>
        /// <param name="createdDate"> createdDate </param>
        /// <param name="description"> description </param>
        /// <returns>int</returns>
        public static int Create(
            int userID,
            int point,
            int amount,
            int type,
            int orderID,
            DateTime createdDate,
            string description,
            int pointType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserPoint_Insert", 8);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@Point", SqlDbType.Int, ParameterDirection.Input, point);
            sph.DefineSqlParameter("@Amount", SqlDbType.Int, ParameterDirection.Input, amount);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@PointType", SqlDbType.Int, ParameterDirection.Input, pointType);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_UserPoint table. Returns true if row updated.
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="userID"> userID </param>
        /// <param name="point"> point </param>
        /// <param name="amount"> amount </param>
        /// <param name="type"> type </param>
        /// <param name="orderID"> orderID </param>
        /// <param name="createdDate"> createdDate </param>
        /// <param name="description"> description </param>
        /// <returns>bool</returns>
        public static bool Update(
            int id,
            int userID,
            int point,
            int amount,
            int type,
            int orderID,
            DateTime createdDate,
            string description,
            int pointType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserPoint_Update", 9);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@Point", SqlDbType.Int, ParameterDirection.Input, point);
            sph.DefineSqlParameter("@Amount", SqlDbType.Int, ParameterDirection.Input, amount);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@PointType", SqlDbType.Int, ParameterDirection.Input, pointType);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_UserPoint table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserPoint_Delete", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_UserPoint table.
        /// </summary>
        /// <param name="id"> id </param>
        public static IDataReader GetOne(
            int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_SelectOne", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByOrder(
                    int orderId, int type, int pointType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_SelectByOrder", 3);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@PointType", SqlDbType.Int, ParameterDirection.Input, pointType);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByUser(
            int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_SelectByUser", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_UserPoint table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_UserPoint_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_UserPoint table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_UserPoint_SelectAll",
                null);
        }

        public static int GetTotalApprovedPointByUser(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_GetTotalApprovedPointByUser", 1);
            sph.DefineSqlParameter("UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return Int32.TryParse(sph.ExecuteScalar().ToString(), out int sum) ? sum : sum;
        }

        public static int GetCountBySearch(
           DateTime? startDate,
           DateTime? endDate,
           string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_GetCountBySearch", 3);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageBySearch(
           DateTime? startDate,
           DateTime? endDate,
           string keyword,
           int pageNumber = 1,
           int pageSize = 32767)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_SelectPageBySearch", 5);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a page of data from the gb_UserPoint table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserPoint_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
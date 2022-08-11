// Author:
// Created:					2020-2-3
// Last Modified:			2020-2-3

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBUserNotification
    {
        /// <summary>
        /// Inserts a row in the gb_UserNotification table. Returns new integer id.
        /// </summary>
        /// <param name="userId"> userId </param>
        /// <param name="name"> name </param>
        /// <param name="description"> description </param>
        /// <param name="type"> type </param>
        /// <param name="readed"> readed </param>
        /// <param name="createDateUtc"> createDateUtc </param>
        /// <param name="createBy"> createBy </param>
        /// <returns>int</returns>
        public static int Create(
            int userId,
            string name,
            string description,
            int type,
            bool readed,
            DateTime createDateUtc,
            string createBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserNotification_Insert", 7);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Readed", SqlDbType.Bit, ParameterDirection.Input, readed);
            sph.DefineSqlParameter("@CreateDateUtc", SqlDbType.DateTime, ParameterDirection.Input, createDateUtc);
            sph.DefineSqlParameter("@CreateBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, createBy);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_UserNotification table. Returns true if row updated.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <param name="userId"> userId </param>
        /// <param name="name"> name </param>
        /// <param name="description"> description </param>
        /// <param name="type"> type </param>
        /// <param name="readed"> readed </param>
        /// <param name="createDateUtc"> createDateUtc </param>
        /// <param name="createBy"> createBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            int userId,
            string name,
            string description,
            int type,
            bool readed,
            DateTime createDateUtc,
            string createBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserNotification_Update", 8);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Readed", SqlDbType.Bit, ParameterDirection.Input, readed);
            sph.DefineSqlParameter("@CreateDateUtc", SqlDbType.DateTime, ParameterDirection.Input, createDateUtc);
            sph.DefineSqlParameter("@CreateBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, createBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_UserNotification table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserNotification_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_UserNotification table.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        public static IDataReader GetOne(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserNotification_SelectOne", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_UserNotification table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_UserNotification_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_UserNotification table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_UserNotification_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_UserNotification table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserNotification_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a page of data from the gb_UserNotification table by search.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static IDataReader GetPageBySearch(
            int userId,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserNotification_SelectPageBySearch", 3);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets count of data from the gb_UserNotification table by search.
        /// </summary>
        /// <param name="userId">userId.</param>
        public static int GetCountBySearch(
            int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserNotification_GetCountBySearch", 1);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }
    }
}
// Author:					Canh cam
// Created:					2021-6-4
// Last Modified:			2021-6-4

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBOrderLog
    {
        /// <summary>
        /// Inserts a row in the gb_OrderLog table. Returns new integer id.
        /// </summary>
        /// <param name="orderId"> orderId </param>
        /// <param name="userId"> userId </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="typeName"> typeName </param>
        /// <param name="comment"> comment </param>
        /// <param name="createdOn"> createdOn </param>
        /// <returns>int</returns>
        public static int Create(
            int orderId,
            int userId,
            string userEmail,
            string typeName,
            string comment,
            DateTime createdOn)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderLog_Insert", 6);
            sph.DefineSqlParameter("@OrderId", SqlDbType.Int, ParameterDirection.Input, orderId);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@UserEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, userEmail);
            sph.DefineSqlParameter("@TypeName", SqlDbType.NVarChar, 255, ParameterDirection.Input, typeName);
            sph.DefineSqlParameter("@Comment", SqlDbType.NVarChar, 255, ParameterDirection.Input, comment);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_OrderLog table. Returns true if row updated.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <param name="orderId"> orderId </param>
        /// <param name="userId"> userId </param>
        /// <param name="userEmail"> userEmail </param>
        /// <param name="typeName"> typeName </param>
        /// <param name="comment"> comment </param>
        /// <param name="createdOn"> createdOn </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            int orderId,
            int userId,
            string userEmail,
            string typeName,
            string comment,
            DateTime createdOn)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderLog_Update", 7);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@OrderId", SqlDbType.Int, ParameterDirection.Input, orderId);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@UserEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, userEmail);
            sph.DefineSqlParameter("@TypeName", SqlDbType.NVarChar, 255, ParameterDirection.Input, typeName);
            sph.DefineSqlParameter("@Comment", SqlDbType.NVarChar, 255, ParameterDirection.Input, comment);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_OrderLog table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderLog_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_OrderLog table.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        public static IDataReader GetOne(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderLog_SelectOne", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_OrderLog table.
        /// </summary>
        public static int GetCount(int orderId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderLog_GetCount", 1);
            sph.DefineSqlParameter("@OrderId", SqlDbType.Int, ParameterDirection.Input, orderId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_OrderLog table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_OrderLog_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_OrderLog table.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static IDataReader GetPage(
            int orderId,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderLog_SelectPage", 3);
            sph.DefineSqlParameter("@OrderId", SqlDbType.Int, ParameterDirection.Input, orderId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
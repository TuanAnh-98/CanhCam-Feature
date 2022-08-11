// Author:					cc
// Created:					2020-6-17
// Last Modified:			2020-6-17

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBMemberRank
    {
        /// <summary>
        /// Inserts a row in the gb_MemberRank table. Returns new integer id.
        /// </summary>
        /// <param name="name"> name </param>
        /// <param name="rankOrder"> rankOrder </param>
        /// <param name="point"> point </param>
        /// <param name="discountPercent"> discountPercent </param>
        /// <param name="description"> description </param>
        /// <param name="note"> note </param>
        /// <returns>int</returns>
        public static int Create(
            string name,
            int rankOrder,
            int point,
            decimal discountPercent,
            string description,
            string note)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_MemberRank_Insert", 6);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@RankOrder", SqlDbType.Int, ParameterDirection.Input, rankOrder);
            sph.DefineSqlParameter("@Point", SqlDbType.Int, ParameterDirection.Input, point);
            sph.DefineSqlParameter("@DiscountPercent", SqlDbType.Decimal, ParameterDirection.Input, discountPercent);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Note", SqlDbType.NVarChar, 255, ParameterDirection.Input, note);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_MemberRank table. Returns true if row updated.
        /// </summary>
        /// <param name="id"> id </param>
        /// <param name="name"> name </param>
        /// <param name="rankOrder"> rankOrder </param>
        /// <param name="point"> point </param>
        /// <param name="discountPercent"> discountPercent </param>
        /// <param name="description"> description </param>
        /// <param name="note"> note </param>
        /// <returns>bool</returns>
        public static bool Update(
            int id,
            string name,
            int rankOrder,
            int point,
            decimal discountPercent,
            string description,
            string note)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_MemberRank_Update", 7);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@RankOrder", SqlDbType.Int, ParameterDirection.Input, rankOrder);
            sph.DefineSqlParameter("@Point", SqlDbType.Int, ParameterDirection.Input, point);
            sph.DefineSqlParameter("@DiscountPercent", SqlDbType.Decimal, ParameterDirection.Input, discountPercent);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@Note", SqlDbType.NVarChar, 255, ParameterDirection.Input, note);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_MemberRank table. Returns true if row deleted.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_MemberRank_Delete", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_MemberRank table.
        /// </summary>
        /// <param name="id"> id </param>
        public static IDataReader GetOne(
            int id)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_MemberRank_SelectOne", 1);
            sph.DefineSqlParameter("@Id", SqlDbType.Int, ParameterDirection.Input, id);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOneByPoint(int point)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_MemberRank_SelectOneByPoint", 1);
            sph.DefineSqlParameter("@Point", SqlDbType.Int, ParameterDirection.Input, point);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOneByRankOrder(int rankOrder)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_MemberRank_SelectByRankOrder", 1);
            sph.DefineSqlParameter("@RankOrder", SqlDbType.Int, ParameterDirection.Input, rankOrder);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_MemberRank table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_MemberRank_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_MemberRank table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_MemberRank_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_MemberRank table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_MemberRank_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
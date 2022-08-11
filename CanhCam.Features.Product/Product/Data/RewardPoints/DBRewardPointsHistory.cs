// Author:					Canh cam
// Created:					2021-1-18
// Last Modified:			2021-1-18

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBRewardPointsHistory
    {
        /// <summary>
        /// Inserts a row in the gb_RewardPointsHistory table. Returns new integer id.
        /// </summary>
        /// <param name="userId"> userId </param>
        /// <param name="usedWithOrderId"> usedWithOrderId </param>
        /// <param name="siteId"> siteId </param>
        /// <param name="points"> points </param>
        /// <param name="pointsBalance"> pointsBalance </param>
        /// <param name="type"> type </param>
        /// <param name="usedAmount"> usedAmount </param>
        /// <param name="message"> message </param>
        /// <param name="createdOnUtc"> createdOnUtc </param>
        /// <param name="availableStartDateTimeUtc"> availableStartDateTimeUtc </param>
        /// <param name="availableEndDateTimeUtc"> availableEndDateTimeUtc </param>
        /// <param name="approvedUtc"> approvedUtc </param>
        /// <param name="approvedUserGuid"> approvedUserGuid </param>
        /// <param name="approvedBy"> approvedBy </param>
        /// <returns>int</returns>
        public static int Create(
            int userId,
            int usedWithOrderId,
            int siteId,
            int points,
            int pointsBalance,
            int type,
            decimal usedAmount,
            string message,
            DateTime createdOnUtc,
            DateTime? availableStartDateTimeUtc,
            DateTime? availableEndDateTimeUtc,
            DateTime? approvedUtc,
            Guid approvedUserGuid,
            string approvedBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_RewardPointsHistory_Insert", 14);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@UsedWithOrderId", SqlDbType.Int, ParameterDirection.Input, usedWithOrderId);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Points", SqlDbType.Int, ParameterDirection.Input, points);
            sph.DefineSqlParameter("@PointsBalance", SqlDbType.Int, ParameterDirection.Input, pointsBalance);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@UsedAmount", SqlDbType.Decimal, ParameterDirection.Input, usedAmount);
            sph.DefineSqlParameter("@Message", SqlDbType.NVarChar, 255, ParameterDirection.Input, message);
            sph.DefineSqlParameter("@CreatedOnUtc", SqlDbType.DateTime, ParameterDirection.Input, createdOnUtc);
            sph.DefineSqlParameter("@AvailableStartDateTimeUtc", SqlDbType.DateTime, ParameterDirection.Input, availableStartDateTimeUtc);
            sph.DefineSqlParameter("@AvailableEndDateTimeUtc", SqlDbType.DateTime, ParameterDirection.Input, availableEndDateTimeUtc);
            sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            sph.DefineSqlParameter("@ApprovedUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedUserGuid);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, approvedBy);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_RewardPointsHistory table. Returns true if row updated.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <param name="userId"> userId </param>
        /// <param name="usedWithOrderId"> usedWithOrderId </param>
        /// <param name="siteId"> siteId </param>
        /// <param name="points"> points </param>
        /// <param name="pointsBalance"> pointsBalance </param>
        /// <param name="type"> type </param>
        /// <param name="usedAmount"> usedAmount </param>
        /// <param name="message"> message </param>
        /// <param name="createdOnUtc"> createdOnUtc </param>
        /// <param name="availableStartDateTimeUtc"> availableStartDateTimeUtc </param>
        /// <param name="availableEndDateTimeUtc"> availableEndDateTimeUtc </param>
        /// <param name="approvedUtc"> approvedUtc </param>
        /// <param name="approvedUserGuid"> approvedUserGuid </param>
        /// <param name="approvedBy"> approvedBy </param>
        /// <returns>bool</returns>
        public static bool Update(
            int rowId,
            int userId,
            int usedWithOrderId,
            int siteId,
            int points,
            int pointsBalance,
            int type,
            decimal usedAmount,
            string message,
            DateTime createdOnUtc,
            DateTime? availableStartDateTimeUtc,
            DateTime? availableEndDateTimeUtc,
            DateTime? approvedUtc,
            Guid approvedUserGuid,
            string approvedBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_RewardPointsHistory_Update", 15);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@UsedWithOrderId", SqlDbType.Int, ParameterDirection.Input, usedWithOrderId);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Points", SqlDbType.Int, ParameterDirection.Input, points);
            sph.DefineSqlParameter("@PointsBalance", SqlDbType.Int, ParameterDirection.Input, pointsBalance);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@UsedAmount", SqlDbType.Decimal, ParameterDirection.Input, usedAmount);
            sph.DefineSqlParameter("@Message", SqlDbType.NVarChar, 255, ParameterDirection.Input, message);
            sph.DefineSqlParameter("@CreatedOnUtc", SqlDbType.DateTime, ParameterDirection.Input, createdOnUtc);
            sph.DefineSqlParameter("@AvailableStartDateTimeUtc", SqlDbType.DateTime, ParameterDirection.Input, availableStartDateTimeUtc);
            sph.DefineSqlParameter("@AvailableEndDateTimeUtc", SqlDbType.DateTime, ParameterDirection.Input, availableEndDateTimeUtc);
            sph.DefineSqlParameter("@ApprovedUtc", SqlDbType.DateTime, ParameterDirection.Input, approvedUtc);
            sph.DefineSqlParameter("@ApprovedUserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, approvedUserGuid);
            sph.DefineSqlParameter("@ApprovedBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, approvedBy);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_RewardPointsHistory table. Returns true if row deleted.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_RewardPointsHistory_Delete", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_RewardPointsHistory table.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        public static IDataReader GetOne(
            int rowId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RewardPointsHistory_SelectOne", 1);
            sph.DefineSqlParameter("@RowId", SqlDbType.Int, ParameterDirection.Input, rowId);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_RewardPointsHistory table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_RewardPointsHistory_GetCount",
                null));
        }
        public static int GetCountBySearch(int siteId,
            int userId,
            int type,
            string keyword,
            DateTime? startDate,
            DateTime? endDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(),
                "gb_RewardPointsHistory_GetCountBySearch", 6);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            return Convert.ToInt32(sph.ExecuteScalar());
        }
        public static IDataReader GetPageBySearch(
            int siteId,
            int userId,
            int type,
            string keyword,
            DateTime? startDate,
            DateTime? endDate,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(),
                "gb_RewardPointsHistory_SelectPageBySearch", 8);
            sph.DefineSqlParameter("@SiteId", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetRewardPointsBalance(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_RewardPointsHistory_GetRewardPointsBalance", 1);
            sph.DefineSqlParameter("@UserId", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }
    }
}
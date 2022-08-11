// Author:					t
// Created:					2020-4-16
// Last Modified:			2020-4-16

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBSiteUserCustom
    {
        /// <summary>
        /// Inserts a row in the gb_Users table. Returns new integer id.
        /// </summary>
        /// <param name="siteID"> siteID </param>
        /// <param name="name"> name </param>
        /// <param name="loginName"> loginName </param>
        /// <param name="email"> email </param>
        /// <param name="phone"> phone </param>
        /// <param name="displayInMemberList"> displayInMemberList </param>
        /// <param name="totalPosts"> totalPosts </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="firstName"> firstName </param>
        /// <param name="lastName"> lastName </param>
        /// <param name="totalOrdersBought"> userGuid </param>
        /// <param name="totalMoney"> siteGuid </param>
        /// <param name="totalConfirmedMoney"> firstName </param>
        /// <param name="totalUnconfirmedMoney"> lastName </param>
        /// <returns>int</returns>
        public static int Create(
            int siteID,
            string name,
            string loginName,
            string email,
            string phone,
            bool displayInMemberList,
            int totalPosts,
            Guid userGuid,
            Guid siteGuid,
            string firstName,
            string lastName,
            int totalOrdersBought,
            decimal totalMoney,
            decimal totalConfirmedMoney,
            decimal totalUnconfirmedMoney)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Users_Insert", 57);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@DisplayInMemberList", SqlDbType.Bit, ParameterDirection.Input, displayInMemberList);
            sph.DefineSqlParameter("@TotalPosts", SqlDbType.Int, ParameterDirection.Input, totalPosts);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@TotalOrdersBought", SqlDbType.Int, ParameterDirection.Input, totalOrdersBought);
            sph.DefineSqlParameter("@TotalMoney", SqlDbType.Decimal, ParameterDirection.Input, totalMoney);
            sph.DefineSqlParameter("@TotalConfirmedMoney", SqlDbType.Decimal, ParameterDirection.Input, totalConfirmedMoney);
            sph.DefineSqlParameter("@TotalUnconfirmedMoney", SqlDbType.Decimal, ParameterDirection.Input, totalUnconfirmedMoney);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_Users table. Returns true if row updated.
        /// </summary>
        /// <param name="userID"> userID </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="name"> name </param>
        /// <param name="loginName"> loginName </param>
        /// <param name="email"> email </param>
        /// <param name="phone"> phone </param>
        /// <param name="displayInMemberList"> displayInMemberList </param>
        /// <param name="totalPosts"> totalPosts </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="siteGuid"> siteGuid </param>
        /// <param name="firstName"> firstName </param>
        /// <param name="lastName"> lastName </param>
        /// <returns>bool</returns>
        public static bool Update(
            int userID,
            int siteID,
            string name,
            string loginName,
            string email,
            string phone,
            bool displayInMemberList,
            int totalPosts,
            Guid userGuid,
            Guid siteGuid,
            string firstName,
            string lastName,
            int totalOrdersBought,
            decimal totalMoney,
            decimal totalConfirmedMoney,
            decimal totalUnconfirmedMoney)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Users_Update", 58);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 100, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@LoginName", SqlDbType.NVarChar, 50, ParameterDirection.Input, loginName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 100, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@DisplayInMemberList", SqlDbType.Bit, ParameterDirection.Input, displayInMemberList);
            sph.DefineSqlParameter("@TotalPosts", SqlDbType.Int, ParameterDirection.Input, totalPosts);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@SiteGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, siteGuid);
            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 100, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 100, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@TotalOrdersBought", SqlDbType.Int, ParameterDirection.Input, totalOrdersBought);
            sph.DefineSqlParameter("@TotalMoney", SqlDbType.Decimal, ParameterDirection.Input, totalMoney);
            sph.DefineSqlParameter("@TotalConfirmedMoney", SqlDbType.Decimal, ParameterDirection.Input, totalConfirmedMoney);
            sph.DefineSqlParameter("@TotalUnconfirmedMoney", SqlDbType.Decimal, ParameterDirection.Input, totalUnconfirmedMoney);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_Users table. Returns true if row deleted.
        /// </summary>
        /// <param name="userID"> userID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int userID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Users_Delete", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_Users table.
        /// </summary>
        /// <param name="userID"> userID </param>
        public static IDataReader GetCustomerByID(
            int userID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_GetCustomerByID", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_Users table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_Users_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_Users table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_Users_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_Users table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCustomerCountBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_GetCustomerCountBySearch", 5);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@NameKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, nameKeyword);
            sph.DefineSqlParameter("@EmailKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailKeyword);
            sph.DefineSqlParameter("@PhoneKeyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, phoneKeyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetCustomerBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword, int pageNumber, int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_GetCustomerBySearch", 7);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@NameKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, nameKeyword);
            sph.DefineSqlParameter("@EmailKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailKeyword);
            sph.DefineSqlParameter("@PhoneKeyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, phoneKeyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword, int memberRank)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_GetCountBySearch", 6);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@NameKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, nameKeyword);
            sph.DefineSqlParameter("@EmailKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailKeyword);
            sph.DefineSqlParameter("@PhoneKeyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, phoneKeyword);
            sph.DefineSqlParameter("@MemberRank", SqlDbType.Int, ParameterDirection.Input, memberRank);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword, int memberRank, int pageNumber, int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_SelectPageBySearch", 8);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@NameKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, nameKeyword);
            sph.DefineSqlParameter("@EmailKeyword", SqlDbType.NVarChar, 100, ParameterDirection.Input, emailKeyword);
            sph.DefineSqlParameter("@PhoneKeyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, phoneKeyword);
            sph.DefineSqlParameter("@MemberRank", SqlDbType.Int, ParameterDirection.Input, memberRank);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountByAdminSearch(int siteId, string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_CountForAdminSearch", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageByAdminSearch(int siteId, string keyword, int pageNumber, int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_SelectAdminSearchPage", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountByMemberRank(int siteId, string keyword, int memberRank)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_GetCountByMemberRank", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@MemberRank", SqlDbType.Int, ParameterDirection.Input, memberRank);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageByMemberRank(int siteId, string keyword, int memberRank, int pageNumber, int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Users_SearchPageByMemberRank", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@SearchInput", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@MemberRank", SqlDbType.Int, ParameterDirection.Input, memberRank);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-13
/// Last Modified:			2014-08-13

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProductComment
    {
        public static int Create(
            int parentID,
            int productID,
            string title,
            string contentText,
            string fullName,
            string email,
            bool isApproved,
            int rating,
            int helpfulYesTotal,
            int helpfulNoTotal,
            int userID,
            DateTime createdUtc,
            int status,
            Guid? userGuid,
            int position,
            string createdFromIP,
            bool isModerator,
            string moderationReason,
            int commentType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductComments_Insert", 19);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@ContentText", SqlDbType.NVarChar, -1, ParameterDirection.Input, contentText);
            sph.DefineSqlParameter("@FullName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fullName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@IsApproved", SqlDbType.Bit, ParameterDirection.Input, isApproved);
            sph.DefineSqlParameter("@Rating", SqlDbType.Int, ParameterDirection.Input, rating);
            sph.DefineSqlParameter("@HelpfulYesTotal", SqlDbType.Int, ParameterDirection.Input, helpfulYesTotal);
            sph.DefineSqlParameter("@HelpfulNoTotal", SqlDbType.Int, ParameterDirection.Input, helpfulNoTotal);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 50, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@IsModerator", SqlDbType.Bit, ParameterDirection.Input, isModerator);
            sph.DefineSqlParameter("@ModerationReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, moderationReason);
            sph.DefineSqlParameter("@CommentType", SqlDbType.Int, ParameterDirection.Input, commentType);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int commentID,
            int parentID,
            int productID,
            string title,
            string contentText,
            string fullName,
            string email,
            bool isApproved,
            int rating,
            int helpfulYesTotal,
            int helpfulNoTotal,
            int userID,
            DateTime createdUtc,
            int status,
            Guid? userGuid,
            int position,
            string createdFromIP,
            bool isModerator,
            string moderationReason)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductComments_Update", 19);
            sph.DefineSqlParameter("@CommentID", SqlDbType.Int, ParameterDirection.Input, commentID);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
            sph.DefineSqlParameter("@ContentText", SqlDbType.NVarChar, -1, ParameterDirection.Input, contentText);
            sph.DefineSqlParameter("@FullName", SqlDbType.NVarChar, 255, ParameterDirection.Input, fullName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@IsApproved", SqlDbType.Bit, ParameterDirection.Input, isApproved);
            sph.DefineSqlParameter("@Rating", SqlDbType.Int, ParameterDirection.Input, rating);
            sph.DefineSqlParameter("@HelpfulYesTotal", SqlDbType.Int, ParameterDirection.Input, helpfulYesTotal);
            sph.DefineSqlParameter("@HelpfulNoTotal", SqlDbType.Int, ParameterDirection.Input, helpfulNoTotal);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 50, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@IsModerator", SqlDbType.Bit, ParameterDirection.Input, isModerator);
            sph.DefineSqlParameter("@ModerationReason", SqlDbType.NVarChar, 255, ParameterDirection.Input, moderationReason);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool UpdateYesTotal(int commentID, int helpfulYesTotal)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductComments_UpdateYesTotal", 2);
            sph.DefineSqlParameter("@CommentID", SqlDbType.Int, ParameterDirection.Input, commentID);
            sph.DefineSqlParameter("@HelpfulYesTotal", SqlDbType.Int, ParameterDirection.Input, helpfulYesTotal);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int commentID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductComments_Delete", 1);
            sph.DefineSqlParameter("@CommentID", SqlDbType.Int, ParameterDirection.Input, commentID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByProduct(int productID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductComments_DeleteByProduct", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int commentID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductComments_SelectOne", 1);
            sph.DefineSqlParameter("@CommentID", SqlDbType.Int, ParameterDirection.Input, commentID);
            return sph.ExecuteReader();
        }

        public static int GetCount(
            int siteId,
            int productId,
            int commentType,
            int approvedStatus,
            int parentId,
            int position,
            DateTime? fromDate,
            DateTime? toDate,
            string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductComments_GetCount", 9);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@CommentType", SqlDbType.Int, ParameterDirection.Input, commentType);
            sph.DefineSqlParameter("@ApprovedStatus", SqlDbType.Int, ParameterDirection.Input, approvedStatus);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@FromDate", SqlDbType.Int, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.Int, ParameterDirection.Input, toDate);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static int GetCountByRating(int productId, int rating)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductComments_GetCountByRating", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@Rating", SqlDbType.Int, ParameterDirection.Input, rating);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPage(
            int siteId,
            int productId,
            int commentType,
            int approvedStatus,
            int parentId,
            int position,
            DateTime? fromDate,
            DateTime? toDate,
            string keyword,
            int orderBy,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductComments_SelectPage", 12);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@CommentType", SqlDbType.Int, ParameterDirection.Input, commentType);
            sph.DefineSqlParameter("@ApprovedStatus", SqlDbType.Int, ParameterDirection.Input, approvedStatus);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentId);
            sph.DefineSqlParameter("@Position", SqlDbType.Int, ParameterDirection.Input, position);
            sph.DefineSqlParameter("@FromDate", SqlDbType.Int, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.Int, ParameterDirection.Input, toDate);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@OrderBy", SqlDbType.Int, ParameterDirection.Input, orderBy);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
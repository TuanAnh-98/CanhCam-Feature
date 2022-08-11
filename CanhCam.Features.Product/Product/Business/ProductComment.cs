/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-13
/// Last Modified:			2014-08-13

using CanhCam.Data;
using CanhCam.Web.ProductUI;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum ProductCommentType
    {
        Comment = 0,
        Rating = 1
    }

    public class ProductComment
    {
        #region Constructors

        public ProductComment()
        { }

        public ProductComment(int commentId)
        {
            this.GetProductComment(commentId);
        }

        #endregion Constructors

        #region Private Properties

        private int commentID = -1;
        private int parentID = -1;
        private int productID = -1;
        private string title = string.Empty;
        private string contentText = string.Empty;
        private string fullName = string.Empty;
        private string email = string.Empty;
        private bool isApproved = false;
        private int rating = 0;
        private int helpfulYesTotal = 0;
        private int helpfulNoTotal = 0;
        private int userID = -1;
        private DateTime createdUtc = DateTime.UtcNow;
        private string productCode = string.Empty;
        private string productTitle = string.Empty;
        private int zoneID = -1;
        private string productUrl = string.Empty;
        private int status = 0;
        private Guid? userGuid = null;
        private int position = 0;
        private string createdFromIP = string.Empty;
        private bool isModerator = false;
        private string moderationReason = string.Empty;
        private string rowNumber = string.Empty;

        private int commentType = (int)ProductCommentType.Comment;

        #endregion Private Properties

        #region Public Properties

        public int CommentId
        {
            get { return commentID; }
            set { commentID = value; }
        }

        public int ParentId
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public int ProductId
        {
            get { return productID; }
            set { productID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string ContentText
        {
            get { return contentText; }
            set { contentText = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public bool IsApproved
        {
            get { return isApproved; }
            set { isApproved = value; }
        }

        public int Rating
        {
            get { return rating; }
            set { rating = value; }
        }

        public int HelpfulYesTotal
        {
            get { return helpfulYesTotal; }
            set { helpfulYesTotal = value; }
        }

        public int HelpfulNoTotal
        {
            get { return helpfulNoTotal; }
            set { helpfulNoTotal = value; }
        }

        public int UserId
        {
            get { return userID; }
            set { userID = value; }
        }

        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }

        public string ProductCode
        {
            get { return productCode; }
            set { productCode = value; }
        }

        public string ProductTitle
        {
            get { return productTitle; }
            set { productTitle = value; }
        }

        public int ZoneId
        {
            get { return zoneID; }
            set { zoneID = value; }
        }

        public string ProductUrl
        {
            get { return productUrl; }
            set { productUrl = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public Guid? UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public string CreatedFromIP
        {
            get { return createdFromIP; }
            set { createdFromIP = value; }
        }

        public bool IsModerator
        {
            get { return isModerator; }
            set { isModerator = value; }
        }

        public string ModerationReason
        {
            get { return moderationReason; }
            set { moderationReason = value; }
        }

        public string RowNumber
        {
            get { return rowNumber; }
            set { rowNumber = value; }
        }

        public int CommentType
        {
            get { return commentType; }
            set { commentType = value; }
        }

        #endregion Public Properties

        #region Private Methods

        private void GetProductComment(int commentId)
        {
            using (IDataReader reader = DBProductComment.GetOne(commentId))
            {
                if (reader.Read())
                {
                    this.commentID = Convert.ToInt32(reader["CommentID"]);
                    this.parentID = Convert.ToInt32(reader["ParentID"]);
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.title = reader["Title"].ToString();
                    this.contentText = reader["ContentText"].ToString();
                    this.fullName = reader["FullName"].ToString();
                    this.email = reader["Email"].ToString();
                    this.isApproved = Convert.ToBoolean(reader["IsApproved"]);
                    this.rating = Convert.ToInt32(reader["Rating"]);
                    this.helpfulYesTotal = Convert.ToInt32(reader["HelpfulYesTotal"]);
                    this.helpfulNoTotal = Convert.ToInt32(reader["HelpfulNoTotal"]);
                    this.userID = Convert.ToInt32(reader["UserID"]);
                    this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    if (reader["Status"] != DBNull.Value)
                        this.status = Convert.ToInt32(reader["Status"]);
                    if (reader["UserGuid"] != DBNull.Value)
                        this.userGuid = new Guid(reader["UserGuid"].ToString());
                    if (reader["Position"] != DBNull.Value)
                        this.position = Convert.ToInt32(reader["Position"]);
                    if (reader["CreatedFromIP"] != DBNull.Value)
                        this.createdFromIP = reader["CreatedFromIP"].ToString();
                    if (reader["IsModerator"] != DBNull.Value)
                        this.isModerator = Convert.ToBoolean(reader["IsModerator"]);
                    if (reader["ModerationReason"] != DBNull.Value)
                        this.moderationReason = reader["ModerationReason"].ToString();
                    if (reader["CommentType"] != DBNull.Value)
                        this.commentType = Convert.ToInt32(reader["CommentType"]);
                }
            }
        }

        private bool Create()
        {
            int newID = 0;

            newID = DBProductComment.Create(
                this.parentID,
                this.productID,
                this.title,
                this.contentText,
                this.fullName,
                this.email,
                this.isApproved,
                this.rating,
                this.helpfulYesTotal,
                this.helpfulNoTotal,
                this.userID,
                this.createdUtc,
                this.status,
                this.userGuid,
                this.position,
                this.createdFromIP,
                this.isModerator,
                this.moderationReason,
                this.commentType);

            this.commentID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBProductComment.Update(
                this.commentID,
                this.parentID,
                this.productID,
                this.title,
                this.contentText,
                this.fullName,
                this.email,
                this.isApproved,
                this.rating,
                this.helpfulYesTotal,
                this.helpfulNoTotal,
                this.userID,
                this.createdUtc,
                this.status,
                this.userGuid,
                this.position,
                this.createdFromIP,
                this.isModerator,
                this.moderationReason);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            bool flag = false;
            if (this.commentID > 0)
                flag = this.Update();
            else
                flag = this.Create();

            if (
                flag
                && ProductConfiguration.AutoUpdateCommentStats
                && this.commentType == (int)ProductCommentType.Rating
                && this.parentID == -1 // Parent
                )
                Product.UpdateCommentStats(this.ProductId);

            return flag;
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int commentId)
        {
            var comment = new ProductComment(commentId);
            var updateStats = false;
            if (
                comment != null && comment.commentID > 0
                && ProductConfiguration.AutoUpdateCommentStats
                && comment.commentType == (int)ProductCommentType.Rating
                && comment.parentID == -1 // Parent
                )
                updateStats = true;

            var flag = DBProductComment.Delete(commentId);

            if (updateStats)
                Product.UpdateCommentStats(comment.ProductId);

            return flag;
        }

        public static bool DeleteByProduct(int productId)
        {
            return DBProductComment.DeleteByProduct(productId);
        }

        public static bool UpdateYesTotal(int commentId, int helpfulYesTotal)
        {
            return DBProductComment.UpdateYesTotal(commentId, helpfulYesTotal);
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
            return DBProductComment.GetCount(siteId, productId, commentType, approvedStatus, parentId, position, fromDate, toDate, keyword);
        }

        public static int GetCountByRating(int productId, int rating)
        {
            return DBProductComment.GetCountByRating(productId, rating);
        }

        private static List<ProductComment> LoadListFromReader(IDataReader reader)
        {
            List<ProductComment> productCommentList = new List<ProductComment>();
            try
            {
                while (reader.Read())
                {
                    ProductComment productComment = new ProductComment
                    {
                        commentID = Convert.ToInt32(reader["CommentID"]),
                        parentID = Convert.ToInt32(reader["ParentID"]),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        title = reader["Title"].ToString(),
                        contentText = reader["ContentText"].ToString(),
                        fullName = reader["FullName"].ToString(),
                        email = reader["Email"].ToString(),
                        isApproved = Convert.ToBoolean(reader["IsApproved"]),
                        rating = Convert.ToInt32(reader["Rating"]),
                        helpfulYesTotal = Convert.ToInt32(reader["HelpfulYesTotal"]),
                        helpfulNoTotal = Convert.ToInt32(reader["HelpfulNoTotal"]),
                        userID = Convert.ToInt32(reader["UserID"]),
                        createdUtc = Convert.ToDateTime(reader["CreatedUtc"])
                    };

                    if (reader["ZoneID"] != DBNull.Value)
                        productComment.zoneID = Convert.ToInt32(reader["ZoneID"]);
                    if (reader["ProductUrl"] != DBNull.Value)
                        productComment.productUrl = reader["ProductUrl"].ToString();
                    if (reader["ProductCode"] != DBNull.Value)
                        productComment.productCode = reader["ProductCode"].ToString();
                    if (reader["ProductTitle"] != DBNull.Value)
                        productComment.productTitle = reader["ProductTitle"].ToString();

                    if (reader["Status"] != DBNull.Value)
                        productComment.status = Convert.ToInt32(reader["Status"]);
                    if (reader["UserGuid"] != DBNull.Value)
                        productComment.userGuid = new Guid(reader["UserGuid"].ToString());
                    if (reader["Position"] != DBNull.Value)
                        productComment.position = Convert.ToInt32(reader["Position"]);
                    if (reader["CreatedFromIP"] != DBNull.Value)
                        productComment.createdFromIP = reader["CreatedFromIP"].ToString();
                    if (reader["IsModerator"] != DBNull.Value)
                        productComment.isModerator = Convert.ToBoolean(reader["IsModerator"]);
                    if (reader["ModerationReason"] != DBNull.Value)
                        productComment.moderationReason = reader["ModerationReason"].ToString();
                    if (reader["CommentType"] != DBNull.Value)
                        productComment.commentType = Convert.ToInt32(reader["CommentType"]);

                    productCommentList.Add(productComment);
                }
            }
            finally
            {
                reader.Close();
            }

            return productCommentList;
        }

        public static List<ProductComment> GetPage(int siteId, int productId, int commentType, int approvedStatus, int parentId, int position, DateTime? fromDate, DateTime? toDate, string keyword, int orderBy, int pageNumber, int pageSize = 32767)
        {
            IDataReader reader = DBProductComment.GetPage(siteId, productId, commentType, approvedStatus, parentId, position, fromDate, toDate, keyword, orderBy, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
// Author:					a
// Created:					2020-3-3
// Last Modified:			2020-3-3

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum UserPointType
    {
        Order = 0,
        NewAccount = 1,
        OrderComplete = 2,
        // >= 5: Not display
        OutOfStockOrder = 5,
        CancelledOrder = 6,
        DeletedOrder = 7,
        PointNotApproved = 8,
        NewOrder = 9,
        ProcessingOrder = 10,
    }

    public enum UserPointPointType
    {
        RewardPoint = 0,
        SpendingPoint = 1,
    }

    public class UserPoint
    {
        #region Constructors

        public UserPoint()
        { }

        public UserPoint(
            int id)
        {
            this.GetUserPoint(
                id);
        }

        #endregion Constructors

        #region Private Properties

        private int id = -1;
        private int userID = -1;
        private int point = -1;
        private int amount = -1;
        private int type = -1;
        private int orderID = -1;
        private DateTime createdDate = DateTime.Now;
        private string description = string.Empty;
        private int pointType = -1;

        #endregion Private Properties

        #region Public Properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public int Point
        {
            get { return point; }
            set { point = value; }
        }

        public int Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public int OrderID
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int PointType
        {
            get { return pointType; }
            set { pointType = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of UserPoint.
        /// </summary>
        /// <param name="id"> id </param>
        private void GetUserPoint(
            int id)
        {
            using (IDataReader reader = DBUserPoint.GetOne(
                id))
            {
                if (reader.Read())
                {
                    this.id = Convert.ToInt32(reader["Id"]);
                    this.userID = Convert.ToInt32(reader["UserID"]);
                    this.point = Convert.ToInt32(reader["Point"]);
                    this.amount = Convert.ToInt32(reader["Amount"]);
                    this.type = Convert.ToInt32(reader["Type"]);
                    this.orderID = Convert.ToInt32(reader["OrderID"]);
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.description = reader["Description"].ToString();
                    if (reader["PointType"] != DBNull.Value)
                        this.pointType = Convert.ToInt32(reader["PointType"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of UserPoint. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBUserPoint.Create(
                this.userID,
                this.point,
                this.amount,
                this.type,
                this.orderID,
                this.createdDate,
                this.description,
                this.pointType);

            this.id = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of UserPoint. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBUserPoint.Update(
                this.id,
                this.userID,
                this.point,
                this.amount,
                this.type,
                this.orderID,
                this.createdDate,
                this.description,
                this.pointType);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of UserPoint. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.id > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Create();
            }
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of UserPoint. Returns true on success.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int id)
        {
            return DBUserPoint.Delete(
                id);
        }

        /// <summary>
        /// Gets a count of UserPoint.
        /// </summary>
        public static int GetCount()
        {
            return DBUserPoint.GetCount();
        }

        private static List<UserPoint> LoadListFromReader(IDataReader reader)
        {
            List<UserPoint> userPointList = new List<UserPoint>();
            try
            {
                while (reader.Read())
                {
                    UserPoint userPoint = new UserPoint
                    {
                        id = Convert.ToInt32(reader["Id"]),
                        userID = Convert.ToInt32(reader["UserID"]),
                        point = Convert.ToInt32(reader["Point"]),
                        amount = Convert.ToInt32(reader["Amount"]),
                        type = Convert.ToInt32(reader["Type"]),
                        orderID = Convert.ToInt32(reader["OrderID"]),
                        createdDate = Convert.ToDateTime(reader["CreatedDate"]),
                        description = reader["Description"].ToString()
                    };
                    if (reader["PointType"] != DBNull.Value)
                        userPoint.pointType = Convert.ToInt32(reader["PointType"]);
                    userPointList.Add(userPoint);
                }
            }
            finally
            {
                reader.Close();
            }

            return userPointList;
        }

        /// <summary>
        /// Gets an IList with all instances of UserPoint.
        /// </summary>
        public static List<UserPoint> GetAll()
        {
            IDataReader reader = DBUserPoint.GetAll();
            return LoadListFromReader(reader);
        }

        public static int GetCountBySearch(
           DateTime? startDate = null,
           DateTime? endDate = null,
           string keyword = null)
        {
            return DBUserPoint.GetCountBySearch(startDate, endDate, keyword);
        }

        public static int GetTotalApprovedPointByUser(int userId)
        {
            return DBUserPoint.GetTotalApprovedPointByUser(userId);
        }

        public static List<UserPoint> GetPageBySearch(
           DateTime? startDate = null,
           DateTime? endDate = null,
           string keyword = null,
           int pageNumber = 1,
           int pageSize = 32767)
        {
            return LoadListFromReader(DBUserPoint.GetPageBySearch(startDate, endDate, keyword, pageNumber, pageSize));
        }

        /// <summary>
        /// Gets an IList with page of instances of UserPoint.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<UserPoint> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBUserPoint.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static List<UserPoint> GetByUser(int userId)
        {
            IDataReader reader = DBUserPoint.GetByUser(userId);
            return LoadListFromReader(reader);
        }

        public static UserPoint GetByOrder(int orderId, int type = -1, int pointType = -1)
        {
            UserPoint userPoint = new UserPoint();
            using (IDataReader reader = DBUserPoint.GetByOrder(
                orderId, type, pointType))
            {
                if (reader.Read())
                {
                    userPoint.id = Convert.ToInt32(reader["Id"]);
                    userPoint.userID = Convert.ToInt32(reader["UserID"]);
                    userPoint.point = Convert.ToInt32(reader["Point"]);
                    userPoint.amount = Convert.ToInt32(reader["Amount"]);
                    userPoint.type = Convert.ToInt32(reader["Type"]);
                    userPoint.orderID = Convert.ToInt32(reader["OrderID"]);
                    userPoint.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    userPoint.description = reader["Description"].ToString();
                    if (reader["PointType"] != DBNull.Value)
                        userPoint.pointType = Convert.ToInt32(reader["PointType"]);
                }
            }
            return userPoint;
        }

        #endregion Static Methods

        #region Comparison Methods

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareById(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.Id.CompareTo(userPoint2.Id);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByUserID(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.UserID.CompareTo(userPoint2.UserID);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByPoint(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.Point.CompareTo(userPoint2.Point);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByAmount(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.Amount.CompareTo(userPoint2.Amount);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByType(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.Type.CompareTo(userPoint2.Type);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByOrderID(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.OrderID.CompareTo(userPoint2.OrderID);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByCreatedDate(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.CreatedDate.CompareTo(userPoint2.CreatedDate);
        }

        /// <summary>
        /// Compares 2 instances of UserPoint.
        /// </summary>
        public static int CompareByDescription(UserPoint userPoint1, UserPoint userPoint2)
        {
            return userPoint1.Description.CompareTo(userPoint2.Description);
        }

        #endregion Comparison Methods
    }
}
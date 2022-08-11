// Author:					Canh cam
// Created:					2021-6-4
// Last Modified:			2021-6-4

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class OrderLog
    {
        #region Constructors

        public OrderLog()
        { }

        public OrderLog(
            int rowId)
        {
            this.GetOrderLog(
                rowId);
        }

        #endregion Constructors

        #region Private Properties

        private int rowId = -1;
        private int orderId = -1;
        private int userId = -1;
        private string userEmail = string.Empty;
        private string typeName = string.Empty;
        private string comment = string.Empty;
        private DateTime createdOn = DateTime.Now;


        private string userName = string.Empty;
        private string userFirstName = string.Empty;
        private string userLastName = string.Empty;
        #endregion Private Properties

        #region Public Properties

        public int RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        public int OrderId
        {
            get { return orderId; }
            set { orderId = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }

        public string Comment
        {
            get { return comment; }
            set { comment = value; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        public string UserFirstName
        {
            get { return userFirstName; }
            set { userFirstName = value; }
        }
        public string UserLastName
        {
            get { return userLastName; }
            set { userLastName = value; }
        }
        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of OrderLog.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        private void GetOrderLog(
            int rowId)
        {
            using (IDataReader reader = DBOrderLog.GetOne(
                rowId))
            {
                if (reader.Read())
                {
                    this.rowId = Convert.ToInt32(reader["RowId"]);
                    this.orderId = Convert.ToInt32(reader["OrderId"]);
                    this.userId = Convert.ToInt32(reader["UserId"]);
                    this.userEmail = reader["UserEmail"].ToString();
                    this.typeName = reader["TypeName"].ToString();
                    this.comment = reader["Comment"].ToString();
                    this.createdOn = Convert.ToDateTime(reader["CreatedOn"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of OrderLog. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBOrderLog.Create(
                this.orderId,
                this.userId,
                this.userEmail,
                this.typeName,
                this.comment,
                this.createdOn);

            this.rowId = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of OrderLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBOrderLog.Update(
                this.rowId,
                this.orderId,
                this.userId,
                this.userEmail,
                this.typeName,
                this.comment,
                this.createdOn);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of OrderLog. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowId > 0)
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
        /// Deletes an instance of OrderLog. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            return DBOrderLog.Delete(
                rowId);
        }

        /// <summary>
        /// Gets a count of OrderLog.
        /// </summary>
        public static int GetCount(int orderId)
        {
            return DBOrderLog.GetCount(orderId);
        }

        private static List<OrderLog> LoadListFromReader(IDataReader reader)
        {
            List<OrderLog> orderLogList = new List<OrderLog>();
            try
            {
                while (reader.Read())
                {
                    OrderLog orderLog = new OrderLog
                    {
                        rowId = Convert.ToInt32(reader["RowId"]),
                        orderId = Convert.ToInt32(reader["OrderId"]),
                        userId = Convert.ToInt32(reader["UserId"]),
                        userEmail = reader["UserEmail"].ToString(),
                        typeName = reader["TypeName"].ToString(),
                        comment = reader["Comment"].ToString(),
                        createdOn = Convert.ToDateTime(reader["CreatedOn"])
                    };
                    if (reader["UserName"] != DBNull.Value)
                        orderLog.userName = reader["UserName"].ToString();
                    if (reader["UserFirstName"] != DBNull.Value)
                        orderLog.userFirstName = reader["UserFirstName"].ToString();
                    if (reader["UserLastName"] != DBNull.Value)
                        orderLog.userLastName = reader["UserLastName"].ToString();

                    orderLogList.Add(orderLog);
                }
            }
            finally
            {
                reader.Close();
            }

            return orderLogList;
        }

        /// <summary>
        /// Gets an IList with all instances of OrderLog.
        /// </summary>
        public static List<OrderLog> GetAll()
        {
            IDataReader reader = DBOrderLog.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of OrderLog.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<OrderLog> GetPage(int orderId, int pageNumber, int pageSize)
        {
            IDataReader reader = DBOrderLog.GetPage(orderId, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
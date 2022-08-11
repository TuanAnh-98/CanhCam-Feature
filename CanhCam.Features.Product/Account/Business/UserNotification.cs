// Author:
// Created:					2020-2-3
// Last Modified:			2020-2-3

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum UserNotificationType
    {
        System = 1,
    }

    public class UserNotification
    {
        #region Constructors

        public UserNotification()
        { }

        public UserNotification(
            int rowId)
        {
            this.GetUserNotification(
                rowId);
        }

        #endregion Constructors

        #region Private Properties

        private int rowId = -1;
        private int userId = -1;
        private string name = string.Empty;
        private string description = string.Empty;
        private int type = -1;
        private bool readed = false;
        private DateTime createDateUtc = DateTime.Now;
        private string createBy = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public int Type
        {
            get { return type; }
            set { type = value; }
        }

        public bool Readed
        {
            get { return readed; }
            set { readed = value; }
        }

        public DateTime CreateDateUtc
        {
            get { return createDateUtc; }
            set { createDateUtc = value; }
        }

        public string CreateBy
        {
            get { return createBy; }
            set { createBy = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of UserNotification.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        private void GetUserNotification(
            int rowId)
        {
            using (IDataReader reader = DBUserNotification.GetOne(
                rowId))
            {
                if (reader.Read())
                {
                    this.rowId = Convert.ToInt32(reader["RowId"]);
                    this.userId = Convert.ToInt32(reader["UserId"]);
                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();
                    this.type = Convert.ToInt32(reader["Type"]);
                    this.readed = Convert.ToBoolean(reader["Readed"]);
                    this.createDateUtc = Convert.ToDateTime(reader["CreateDateUtc"]);
                    this.createBy = reader["CreateBy"].ToString();
                }
            }
        }

        /// <summary>
        /// Persists a new instance of UserNotification. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBUserNotification.Create(
                this.userId,
                this.name,
                this.description,
                this.type,
                this.readed,
                this.createDateUtc,
                this.createBy);

            this.rowId = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of UserNotification. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBUserNotification.Update(
                this.rowId,
                this.userId,
                this.name,
                this.description,
                this.type,
                this.readed,
                this.createDateUtc,
                this.createBy);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of UserNotification. Returns true on success.
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
        /// Deletes an instance of UserNotification. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            return DBUserNotification.Delete(
                rowId);
        }

        /// <summary>
        /// Gets a count of UserNotification.
        /// </summary>
        public static int GetCount()
        {
            return DBUserNotification.GetCount();
        }

        private static List<UserNotification> LoadListFromReader(IDataReader reader)
        {
            List<UserNotification> userNotificationList = new List<UserNotification>();
            try
            {
                while (reader.Read())
                {
                    UserNotification userNotification = new UserNotification
                    {
                        rowId = Convert.ToInt32(reader["RowId"]),
                        userId = Convert.ToInt32(reader["UserId"]),
                        name = reader["Name"].ToString(),
                        description = reader["Description"].ToString(),
                        type = Convert.ToInt32(reader["Type"]),
                        readed = Convert.ToBoolean(reader["Readed"]),
                        createDateUtc = Convert.ToDateTime(reader["CreateDateUtc"]),
                        createBy = reader["CreateBy"].ToString()
                    };
                    userNotificationList.Add(userNotification);
                }
            }
            finally
            {
                reader.Close();
            }

            return userNotificationList;
        }

        /// <summary>
        /// Gets an IList with all instances of UserNotification.
        /// </summary>
        public static List<UserNotification> GetAll()
        {
            IDataReader reader = DBUserNotification.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of UserNotification.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<UserNotification> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBUserNotification.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets a page of data from the gb_UserNotification table by search.
        /// </summary>
        /// <param name="userId">userId.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        public static List<UserNotification> GetPageBySearch(
            int userId,
            int pageNumber,
            int pageSize)
        {
            IDataReader reader = DBUserNotification.GetPageBySearch(userId, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets count of data from the gb_UserNotification table by search.
        /// </summary>
        /// <param name="userId">userId.</param>
        public static int GetCountBySearch(
            int userId)
        {
            return DBUserNotification.GetCountBySearch(userId);
        }

        #endregion Static Methods
    }
}
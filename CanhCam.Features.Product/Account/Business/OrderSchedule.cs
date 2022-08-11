// Author:					t
// Created:					2020-3-30
// Last Modified:			2020-3-30

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class OrderSchedule
    {
        #region Constructors

        public OrderSchedule()
        { }

        public OrderSchedule(
            int orderScheduleID)
        {
            this.GetOrderSchedule(
                orderScheduleID);
        }

        #endregion Constructors

        #region Private Properties

        private int orderScheduleID = -1;
        private string name = string.Empty;
        private string description = string.Empty;
        private int userID = -1;
        private string products = string.Empty;
        private bool enabled = false;
        private DateTime startDate = DateTime.Now;
        private DateTime? endDate = null;
        private int displayOrder = -1;
        private DateTime createdDate = DateTime.Now;
        private DateTime lastOrderDate = DateTime.Now;
        private int frequencyType = -1;
        private int dailyFrequency = -1;
        private int weeklyFrequency = -1;
        private string weeklyDayOfWeeks = string.Empty;
        private int monthlyFrequency = -1;
        private int monthlyDay = -1;
        private Guid guid = Guid.Empty;
        private string addressID = string.Empty;
        private int shippingMethod = -1;
        private int paymentMethod = -1;
        private int confirmationEmailAlarm = 0;

        #endregion Private Properties

        #region Public Properties

        public int OrderScheduleID
        {
            get { return orderScheduleID; }
            set { orderScheduleID = value; }
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

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public string Products
        {
            get { return products; }
            set { products = value; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set { enabled = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public DateTime LastOrderDate
        {
            get { return lastOrderDate; }
            set { lastOrderDate = value; }
        }

        public int FrequencyType
        {
            get { return frequencyType; }
            set { frequencyType = value; }
        }

        public int DailyFrequency
        {
            get { return dailyFrequency; }
            set { dailyFrequency = value; }
        }

        public int WeeklyFrequency
        {
            get { return weeklyFrequency; }
            set { weeklyFrequency = value; }
        }

        public string WeeklyDayOfWeeks
        {
            get { return weeklyDayOfWeeks; }
            set { weeklyDayOfWeeks = value; }
        }

        public int MonthlyFrequency
        {
            get { return monthlyFrequency; }
            set { monthlyFrequency = value; }
        }

        public int MonthlyDay
        {
            get { return monthlyDay; }
            set { monthlyDay = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string AddressID
        {
            get { return addressID; }
            set { addressID = value; }
        }

        public int ShippingMethod
        {
            get { return shippingMethod; }
            set { shippingMethod = value; }
        }

        public int PaymentMethod
        {
            get { return paymentMethod; }
            set { paymentMethod = value; }
        }

        public int ConfirmationEmailAlarm
        {
            get { return confirmationEmailAlarm; }
            set { confirmationEmailAlarm = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of OrderSchedule.
        /// </summary>
        /// <param name="orderScheduleID"> orderScheduleID </param>
        private void GetOrderSchedule(
            int orderScheduleID)
        {
            using (IDataReader reader = DBOrderSchedule.GetOne(
                orderScheduleID))
            {
                if (reader.Read())
                {
                    this.orderScheduleID = Convert.ToInt32(reader["OrderScheduleID"]);
                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();
                    this.userID = Convert.ToInt32(reader["UserID"]);
                    this.products = reader["Products"].ToString();
                    this.enabled = Convert.ToBoolean(reader["Enabled"]);
                    this.startDate = Convert.ToDateTime(reader["StartDate"]);
                    if (reader["EndDate"].ToString() == "")
                        this.endDate = null;
                    else
                        this.endDate = Convert.ToDateTime(reader["EndDate"]);
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.lastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]);
                    this.frequencyType = Convert.ToInt32(reader["FrequencyType"]);
                    this.dailyFrequency = Convert.ToInt32(reader["DailyFrequency"]);
                    this.weeklyFrequency = Convert.ToInt32(reader["WeeklyFrequency"]);
                    this.weeklyDayOfWeeks = reader["WeeklyDayOfWeeks"].ToString();
                    this.monthlyFrequency = Convert.ToInt32(reader["MonthlyFrequency"]);
                    this.monthlyDay = Convert.ToInt32(reader["MonthlyDay"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.addressID = reader["AddressID"].ToString();
                    this.shippingMethod = Convert.ToInt32(reader["ShippingMethod"]);
                    this.paymentMethod = Convert.ToInt32(reader["PaymentMethod"]);
                    this.confirmationEmailAlarm = Convert.ToInt32(reader["ConfirmationEmailAlarm"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of OrderSchedule. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBOrderSchedule.Create(
                this.name,
                this.description,
                this.userID,
                this.products,
                this.enabled,
                this.startDate,
                this.endDate,
                this.displayOrder,
                this.createdDate,
                this.lastOrderDate,
                this.frequencyType,
                this.dailyFrequency,
                this.weeklyFrequency,
                this.weeklyDayOfWeeks,
                this.monthlyFrequency,
                this.monthlyDay,
                this.guid,
                this.addressID,
                this.shippingMethod,
                this.paymentMethod,
                this.confirmationEmailAlarm);

            this.orderScheduleID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of OrderSchedule. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBOrderSchedule.Update(
                this.orderScheduleID,
                this.name,
                this.description,
                this.userID,
                this.products,
                this.enabled,
                this.startDate,
                this.endDate,
                this.displayOrder,
                this.createdDate,
                this.lastOrderDate,
                this.frequencyType,
                this.dailyFrequency,
                this.weeklyFrequency,
                this.weeklyDayOfWeeks,
                this.monthlyFrequency,
                this.monthlyDay,
                this.guid,
                this.addressID,
                this.shippingMethod,
                this.paymentMethod,
                this.confirmationEmailAlarm);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of OrderSchedule. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.orderScheduleID > 0)
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
        /// Deletes an instance of OrderSchedule. Returns true on success.
        /// </summary>
        /// <param name="orderScheduleID"> orderScheduleID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int orderScheduleID)
        {
            return DBOrderSchedule.Delete(
                orderScheduleID);
        }

        /// <summary>
        /// Gets a count of OrderSchedule.
        /// </summary>
        public static int GetCount()
        {
            return DBOrderSchedule.GetCount();
        }

        private static List<OrderSchedule> LoadListFromReader(IDataReader reader)
        {
            List<OrderSchedule> orderScheduleList = new List<OrderSchedule>();
            try
            {
                while (reader.Read())
                {
                    OrderSchedule orderSchedule = new OrderSchedule
                    {
                        orderScheduleID = Convert.ToInt32(reader["OrderScheduleID"]),
                        name = reader["Name"].ToString(),
                        description = reader["Description"].ToString(),
                        userID = Convert.ToInt32(reader["UserID"]),
                        products = reader["Products"].ToString(),
                        enabled = Convert.ToBoolean(reader["Enabled"]),
                        startDate = Convert.ToDateTime(reader["StartDate"])
                    };
                    if (reader["EndDate"].ToString() == "")
                        orderSchedule.endDate = null;
                    else
                        orderSchedule.endDate = Convert.ToDateTime(reader["EndDate"]);
                    orderSchedule.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    orderSchedule.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    orderSchedule.lastOrderDate = Convert.ToDateTime(reader["LastOrderDate"]);
                    orderSchedule.frequencyType = Convert.ToInt32(reader["FrequencyType"]);
                    orderSchedule.dailyFrequency = Convert.ToInt32(reader["DailyFrequency"]);
                    orderSchedule.weeklyFrequency = Convert.ToInt32(reader["WeeklyFrequency"]);
                    orderSchedule.weeklyDayOfWeeks = reader["WeeklyDayOfWeeks"].ToString();
                    orderSchedule.monthlyFrequency = Convert.ToInt32(reader["MonthlyFrequency"]);
                    orderSchedule.monthlyDay = Convert.ToInt32(reader["MonthlyDay"]);
                    orderSchedule.guid = new Guid(reader["Guid"].ToString());
                    orderSchedule.addressID = reader["AddressID"].ToString();
                    orderSchedule.shippingMethod = Convert.ToInt32(reader["ShippingMethod"]);
                    orderSchedule.paymentMethod = Convert.ToInt32(reader["PaymentMethod"]);
                    orderSchedule.confirmationEmailAlarm = Convert.ToInt32(reader["ConfirmationEmailAlarm"]);
                    orderScheduleList.Add(orderSchedule);
                }
            }
            finally
            {
                reader.Close();
            }

            return orderScheduleList;
        }

        /// <summary>
        /// Gets an IList with all instances of OrderSchedule.
        /// </summary>
        public static List<OrderSchedule> GetAll()
        {
            IDataReader reader = DBOrderSchedule.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of OrderSchedule.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<OrderSchedule> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBOrderSchedule.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static List<OrderSchedule> GetByUser(int userId)
        {
            return LoadListFromReader(DBOrderSchedule.GetByUser(userId));
        }

        #endregion Static Methods
    }
}
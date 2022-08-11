// Author:					t
// Created:					2020-3-30
// Last Modified:			2020-3-30

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBOrderSchedule
    {
        /// <summary>
        /// Inserts a row in the gb_OrderSchedule table. Returns new integer id.
        /// </summary>
        /// <param name="name"> name </param>
        /// <param name="description"> description </param>
        /// <param name="userID"> userID </param>
        /// <param name="products"> products </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="startDate"> startDate </param>
        /// <param name="endDate"> endDate </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="createdDate"> createdDate </param>
        /// <param name="lastOrderDate"> lastOrderDate </param>
        /// <param name="frequencyType"> frequencyType </param>
        /// <param name="dailyFrequency"> dailyFrequency </param>
        /// <param name="weeklyFrequency"> weeklyFrequency </param>
        /// <param name="weeklyDayOfWeeks"> weeklyDayOfWeeks </param>
        /// <param name="monthlyFrequency"> monthlyFrequency </param>
        /// <param name="monthlyDay"> monthlyDay </param>
        /// <param name="guid"> guid </param>
        /// <param name="addressID"> addressID </param>
        /// <param name="shippingMethod"> shippingMethod </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="confirmationEmailAlarm"> confirmationEmailAlarm </param>
        /// <returns>int</returns>
        public static int Create(
            string name,
            string description,
            int userID,
            string products,
            bool enabled,
            DateTime startDate,
            DateTime? endDate,
            int displayOrder,
            DateTime createdDate,
            DateTime lastOrderDate,
            int frequencyType,
            int dailyFrequency,
            int weeklyFrequency,
            string weeklyDayOfWeeks,
            int monthlyFrequency,
            int monthlyDay,
            Guid guid,
            string addressID,
            int shippingMethod,
            int paymentMethod,
            int confirmationEmailAlarm)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderSchedule_Insert", 21);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@Products", SqlDbType.NVarChar, -1, ParameterDirection.Input, products);
            sph.DefineSqlParameter("@Enabled", SqlDbType.Bit, ParameterDirection.Input, enabled);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@LastOrderDate", SqlDbType.DateTime, ParameterDirection.Input, lastOrderDate);
            sph.DefineSqlParameter("@FrequencyType", SqlDbType.Int, ParameterDirection.Input, frequencyType);
            sph.DefineSqlParameter("@DailyFrequency", SqlDbType.Int, ParameterDirection.Input, dailyFrequency);
            sph.DefineSqlParameter("@WeeklyFrequency", SqlDbType.Int, ParameterDirection.Input, weeklyFrequency);
            sph.DefineSqlParameter("@WeeklyDayOfWeeks", SqlDbType.NVarChar, -1, ParameterDirection.Input, weeklyDayOfWeeks);
            sph.DefineSqlParameter("@MonthlyFrequency", SqlDbType.Int, ParameterDirection.Input, monthlyFrequency);
            sph.DefineSqlParameter("@MonthlyDay", SqlDbType.Int, ParameterDirection.Input, monthlyDay);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@AddressID", SqlDbType.NVarChar, -1, ParameterDirection.Input, addressID);
            sph.DefineSqlParameter("@ShippingMethod", SqlDbType.Int, ParameterDirection.Input, shippingMethod);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.Int, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@ConfirmationEmailAlarm", SqlDbType.Int, ParameterDirection.Input, confirmationEmailAlarm);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_OrderSchedule table. Returns true if row updated.
        /// </summary>
        /// <param name="orderScheduleID"> orderScheduleID </param>
        /// <param name="name"> name </param>
        /// <param name="description"> description </param>
        /// <param name="userID"> userID </param>
        /// <param name="products"> products </param>
        /// <param name="enabled"> enabled </param>
        /// <param name="startDate"> startDate </param>
        /// <param name="endDate"> endDate </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="createdDate"> createdDate </param>
        /// <param name="lastOrderDate"> lastOrderDate </param>
        /// <param name="frequencyType"> frequencyType </param>
        /// <param name="dailyFrequency"> dailyFrequency </param>
        /// <param name="weeklyFrequency"> weeklyFrequency </param>
        /// <param name="weeklyDayOfWeeks"> weeklyDayOfWeeks </param>
        /// <param name="monthlyFrequency"> monthlyFrequency </param>
        /// <param name="monthlyDay"> monthlyDay </param>
        /// <param name="guid"> guid </param>
        /// <param name="addressID"> addressID </param>
        /// <param name="shippingMethod"> shippingMethod </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="confirmationEmailAlarm"> confirmationEmailAlarm </param>
        /// <returns>bool</returns>
        public static bool Update(
            int orderScheduleID,
            string name,
            string description,
            int userID,
            string products,
            bool enabled,
            DateTime startDate,
            DateTime? endDate,
            int displayOrder,
            DateTime createdDate,
            DateTime lastOrderDate,
            int frequencyType,
            int dailyFrequency,
            int weeklyFrequency,
            string weeklyDayOfWeeks,
            int monthlyFrequency,
            int monthlyDay,
            Guid guid,
            string addressID,
            int shippingMethod,
            int paymentMethod,
            int confirmationEmailAlarm)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderSchedule_Update", 22);
            sph.DefineSqlParameter("@OrderScheduleID", SqlDbType.Int, ParameterDirection.Input, orderScheduleID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@Products", SqlDbType.NVarChar, -1, ParameterDirection.Input, products);
            sph.DefineSqlParameter("@Enabled", SqlDbType.Bit, ParameterDirection.Input, enabled);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@LastOrderDate", SqlDbType.DateTime, ParameterDirection.Input, lastOrderDate);
            sph.DefineSqlParameter("@FrequencyType", SqlDbType.Int, ParameterDirection.Input, frequencyType);
            sph.DefineSqlParameter("@DailyFrequency", SqlDbType.Int, ParameterDirection.Input, dailyFrequency);
            sph.DefineSqlParameter("@WeeklyFrequency", SqlDbType.Int, ParameterDirection.Input, weeklyFrequency);
            sph.DefineSqlParameter("@WeeklyDayOfWeeks", SqlDbType.NVarChar, -1, ParameterDirection.Input, weeklyDayOfWeeks);
            sph.DefineSqlParameter("@MonthlyFrequency", SqlDbType.Int, ParameterDirection.Input, monthlyFrequency);
            sph.DefineSqlParameter("@MonthlyDay", SqlDbType.Int, ParameterDirection.Input, monthlyDay);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@AddressID", SqlDbType.NVarChar, -1, ParameterDirection.Input, addressID);
            sph.DefineSqlParameter("@ShippingMethod", SqlDbType.Int, ParameterDirection.Input, shippingMethod);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.Int, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@ConfirmationEmailAlarm", SqlDbType.Int, ParameterDirection.Input, confirmationEmailAlarm);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_OrderSchedule table. Returns true if row deleted.
        /// </summary>
        /// <param name="orderScheduleID"> orderScheduleID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int orderScheduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderSchedule_Delete", 1);
            sph.DefineSqlParameter("@OrderScheduleID", SqlDbType.Int, ParameterDirection.Input, orderScheduleID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_OrderSchedule table.
        /// </summary>
        /// <param name="orderScheduleID"> orderScheduleID </param>
        public static IDataReader GetOne(
            int orderScheduleID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderSchedule_SelectOne", 1);
            sph.DefineSqlParameter("@OrderScheduleID", SqlDbType.Int, ParameterDirection.Input, orderScheduleID);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_OrderSchedule table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_OrderSchedule_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_OrderSchedule table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_OrderSchedule_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_OrderSchedule table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderSchedule_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByUser(int userId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderSchedule_SelectByUser", 1);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            return sph.ExecuteReader();
        }
    }
}
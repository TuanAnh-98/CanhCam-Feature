// Author:					Canh cam
// Created:					2021-2-17
// Last Modified:			2021-2-17

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBVoucher
    {
        /// <summary>
        /// Inserts a row in the gb_Voucher table. Returns rows affected count.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="voucherCode"> voucherCode </param>
        /// <param name="useCount"> useCount </param>
        /// <param name="limitationTimes"> limitationTimes </param>
        /// <param name="minimumOrderAmount"> minimumOrderAmount </param>
        /// <param name="amount"> amount </param>
        /// <param name="usePercentage"> usePercentage </param>
        /// <param name="maxAmount"> maxAmount </param>
        /// <param name="startDate"> startDate </param>
        /// <param name="endDate"> endDate </param>
        /// <param name="createdDate"> createdDate </param>
        /// <returns>int</returns>
        public static int Create(
            Guid itemGuid,
            string voucherCode,
            int useCount,
            int limitationTimes,
            decimal minimumOrderAmount,
            decimal amount,
            bool usePercentage,
            decimal maxAmount,
            DateTime startDate,
            DateTime endDate,
            DateTime createdDate,
            string orderCodesUsed)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Voucher_Insert", 12);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@VoucherCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, voucherCode);
            sph.DefineSqlParameter("@UseCount", SqlDbType.Int, ParameterDirection.Input, useCount);
            sph.DefineSqlParameter("@LimitationTimes", SqlDbType.Int, ParameterDirection.Input, limitationTimes);
            sph.DefineSqlParameter("@MinimumOrderAmount", SqlDbType.Decimal, ParameterDirection.Input, minimumOrderAmount);
            sph.DefineSqlParameter("@Amount", SqlDbType.Decimal, ParameterDirection.Input, amount);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@MaxAmount", SqlDbType.Decimal, ParameterDirection.Input, maxAmount);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@OrderCodesUsed", SqlDbType.VarChar, 255, ParameterDirection.Input, orderCodesUsed);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        /// <summary>
        /// Updates a row in the gb_Voucher table. Returns true if row updated.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="voucherCode"> voucherCode </param>
        /// <param name="useCount"> useCount </param>
        /// <param name="limitationTimes"> limitationTimes </param>
        /// <param name="minimumOrderAmount"> minimumOrderAmount </param>
        /// <param name="amount"> amount </param>
        /// <param name="usePercentage"> usePercentage </param>
        /// <param name="maxAmount"> maxAmount </param>
        /// <param name="startDate"> startDate </param>
        /// <param name="endDate"> endDate </param>
        /// <param name="createdDate"> createdDate </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid itemGuid,
            string voucherCode,
            int useCount,
            int limitationTimes,
            decimal minimumOrderAmount,
            decimal amount,
            bool usePercentage,
            decimal maxAmount,
            DateTime startDate,
            DateTime endDate,
            DateTime createdDate,
            string orderCodesUsed)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Voucher_Update", 12);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@VoucherCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, voucherCode);
            sph.DefineSqlParameter("@UseCount", SqlDbType.Int, ParameterDirection.Input, useCount);
            sph.DefineSqlParameter("@LimitationTimes", SqlDbType.Int, ParameterDirection.Input, limitationTimes);
            sph.DefineSqlParameter("@MinimumOrderAmount", SqlDbType.Decimal, ParameterDirection.Input, minimumOrderAmount);
            sph.DefineSqlParameter("@Amount", SqlDbType.Decimal, ParameterDirection.Input, amount);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@MaxAmount", SqlDbType.Decimal, ParameterDirection.Input, maxAmount);
            sph.DefineSqlParameter("@StartDate", SqlDbType.DateTime, ParameterDirection.Input, startDate);
            sph.DefineSqlParameter("@EndDate", SqlDbType.DateTime, ParameterDirection.Input, endDate);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@OrderCodesUsed", SqlDbType.VarChar, 255, ParameterDirection.Input, orderCodesUsed);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_Voucher table. Returns true if row deleted.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Voucher_Delete", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_Voucher table.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        public static IDataReader GetOne(
            Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Voucher_SelectOne", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOneByCode(
            string code)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Voucher_SelectOneByCode", 1);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, code);
            return sph.ExecuteReader();
        }

        /// <summary>
        /// Gets a count of rows in the gb_Voucher table.
        /// </summary>
        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_Voucher_GetCount",
                null));
        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_Voucher table.
        /// </summary>
        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_Voucher_SelectAll",
                null);
        }

        /// <summary>
        /// Gets a page of data from the gb_Voucher table.
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

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Voucher_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }


        public static int GetCountByAdv(string keyword,
            int type)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Voucher_GetCountAdv", 2);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageByAdv(string keyword,
            int type,
            int pageNumber,
            int pageSize)
        {

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Voucher_SelectPageAdv", 4);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
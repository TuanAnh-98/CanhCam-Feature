// Author:					Canh cam
// Created:					2021-2-22
// Last Modified:			2021-2-22

using System;
using System.Data;

namespace CanhCam.Data
{

    public static class DBProductServicePrice
    {

        /// <summary>
        /// Inserts a row in the gb_ProductServicePrice table. Returns rows affected count.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="productId"> productId </param>
        /// <param name="type"> type </param>
        /// <param name="price"> price </param>
        /// <param name="maxPrice"> maxPrice </param>
        /// <param name="feeForOrderTotal"> feeForOrderTotal </param>
        /// <param name="freeForPaymentOnline"> freeForPaymentOnline </param>
        /// <returns>int</returns>
        public static int Create(
            Guid itemGuid,
            int productId,
            int type,
            decimal price,
            decimal maxPrice,
            decimal feeForOrderTotal,
            bool freeForPaymentOnline)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductServicePrice_Insert", 7);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ProductId", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@MaxPrice", SqlDbType.Decimal, ParameterDirection.Input, maxPrice);
            sph.DefineSqlParameter("@FeeForOrderTotal", SqlDbType.Decimal, ParameterDirection.Input, feeForOrderTotal);
            sph.DefineSqlParameter("@FreeForPaymentOnline", SqlDbType.Bit, ParameterDirection.Input, freeForPaymentOnline);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;

        }


        /// <summary>
        /// Updates a row in the gb_ProductServicePrice table. Returns true if row updated.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <param name="productId"> productId </param>
        /// <param name="type"> type </param>
        /// <param name="price"> price </param>
        /// <param name="maxPrice"> maxPrice </param>
        /// <param name="feeForOrderTotal"> feeForOrderTotal </param>
        /// <param name="freeForPaymentOnline"> freeForPaymentOnline </param>
        /// <returns>bool</returns>
        public static bool Update(
            Guid itemGuid,
            int productId,
            int type,
            decimal price,
            decimal maxPrice,
            decimal feeForOrderTotal,
            bool freeForPaymentOnline)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductServicePrice_Update", 7);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            sph.DefineSqlParameter("@ProductId", SqlDbType.Int, ParameterDirection.Input, productId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@MaxPrice", SqlDbType.Decimal, ParameterDirection.Input, maxPrice);
            sph.DefineSqlParameter("@FeeForOrderTotal", SqlDbType.Decimal, ParameterDirection.Input, feeForOrderTotal);
            sph.DefineSqlParameter("@FreeForPaymentOnline", SqlDbType.Bit, ParameterDirection.Input, freeForPaymentOnline);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Deletes a row from the gb_ProductServicePrice table. Returns true if row deleted.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductServicePrice_Delete", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_ProductServicePrice table.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        public static IDataReader GetOne(
            Guid itemGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductServicePrice_SelectOne", 1);
            sph.DefineSqlParameter("@ItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, itemGuid);
            return sph.ExecuteReader();

        }

        /// <summary>
        /// Gets a count of rows in the gb_ProductServicePrice table.
        /// </summary>
        public static int GetCount()
        {

            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ProductServicePrice_GetCount",
                null));

        }

        /// <summary>
        /// Gets an IDataReader with all rows in the gb_ProductServicePrice table.
        /// </summary>
        public static IDataReader GetAll()
        {

            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_ProductServicePrice_SelectAll",
                null);

        }

        /// <summary>
        /// Gets a page of data from the gb_ProductServicePrice table.
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
                int remainder;
                Math.DivRem(totalRows, pageSize, out remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductServicePrice_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();

        }

        public static IDataReader GetByProductId(
            int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductServicePrice_GetProductID", 1);
            sph.DefineSqlParameter("@ProductId", SqlDbType.Int, ParameterDirection.Input, productId);
            return sph.ExecuteReader();

        }

        public static IDataReader GetByProductIds(
            string productIds)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductServicePrice_ByProductIDs", 1);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar,255, ParameterDirection.Input, productIds);
            return sph.ExecuteReader();

        }
    }

}

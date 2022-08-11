/// Author:					Tran Quoc Vuong
/// Created:				2020-09-21
/// Last Modified:			2020-09-21

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscountRange
    {
        /// <summary>
        /// Inserts a row in the gb_DiscountRange table. Returns new integer id.
        /// </summary>
        /// <param name="discountID"> discountID </param>
        /// <param name="zoneID"> zoneID </param>
        /// <param name="productID"> productID </param>
        /// <param name="fromPrice"> fromPrice </param>
        /// <param name="toPrice"> toPrice </param>
        /// <param name="discountType"> discountType </param>
        /// <param name="discountAmount"> discountAmount </param>
        /// <param name="giftHtml"> giftHtml </param>
        /// <returns>int</returns>
        public static int Create(
            int discountID,
            int zoneID,
            int productID,
            decimal fromPrice,
            decimal toPrice,
            int discountType,
            decimal discountAmount,
            string giftHtml,
            decimal maximumDiscount,
            string productGifts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountRange_Insert", 10);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@FromPrice", SqlDbType.Decimal, ParameterDirection.Input, fromPrice);
            sph.DefineSqlParameter("@ToPrice", SqlDbType.Decimal, ParameterDirection.Input, toPrice);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@GiftHtml", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftHtml);
            sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
            sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_DiscountRange table. Returns true if row updated.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <param name="discountID"> discountID </param>
        /// <param name="zoneID"> zoneID </param>
        /// <param name="productID"> productID </param>
        /// <param name="fromPrice"> fromPrice </param>
        /// <param name="toPrice"> toPrice </param>
        /// <param name="discountType"> discountType </param>
        /// <param name="discountAmount"> discountAmount </param>
        /// <param name="giftHtml"> giftHtml </param>
        /// <returns>bool</returns>
        public static bool Update(
            int itemID,
            int discountID,
            int zoneID,
            int productID,
            decimal fromPrice,
            decimal toPrice,
            int discountType,
            decimal discountAmount,
            string giftHtml,
            decimal maximumDiscount,
            string productGifts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountRange_Update", 11);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@FromPrice", SqlDbType.Decimal, ParameterDirection.Input, fromPrice);
            sph.DefineSqlParameter("@ToPrice", SqlDbType.Decimal, ParameterDirection.Input, toPrice);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@GiftHtml", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftHtml);
            sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
            sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_DiscountRange table. Returns true if row deleted.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <returns>bool</returns>
        public static bool Delete(int itemID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountRange_Delete", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_DiscountRange table.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        public static IDataReader GetOne(
            int itemID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountRange_SelectOne", 1);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
            return sph.ExecuteReader();
        }

        ///// <summary>
        ///// Gets a count of rows in the gb_DiscountRange table.
        ///// </summary>
        //public static int GetCount()
        //{
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "gb_DiscountRange_GetCount",
        //        null));
        //}

        ///// <summary>
        ///// Gets an IDataReader with all rows in the gb_DiscountRange table.
        ///// </summary>
        //public static IDataReader GetAll()
        //{
        //    return SqlHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "gb_DiscountRange_SelectAll",
        //        null);
        //}

        //public static IDataReader GetByDiscountID(int discountID)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountRange_SelectByDiscountID", 1);
        //    sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
        //    return sph.ExecuteReader();
        //}

        //public static IDataReader GetByProductID(int productID, int discountID)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountRange_SelectByProductID", 2);
        //    sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
        //    sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
        //    return sph.ExecuteReader();
        //}

        public static IDataReader GetRange(int discountID, int productID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountRange_Select", 2);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetActive(int siteID, int productID, decimal value, int shareType)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountRange_SelectActive", 4);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Value", SqlDbType.Decimal, ParameterDirection.Input, value);
            sph.DefineSqlParameter("@ShareType", SqlDbType.Int, ParameterDirection.Input, shareType);
            return sph.ExecuteReader();
        }

        ///// <summary>
        ///// Gets a page of data from the gb_DiscountRange table.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static IDataReader GetPage(
        //    int pageNumber,
        //    int pageSize,
        //    out int totalPages)
        //{
        //    totalPages = 1;
        //    int totalRows
        //        = GetCount();

        //    if (pageSize > 0) totalPages = totalRows / pageSize;

        //    if (totalRows <= pageSize)
        //    {
        //        totalPages = 1;
        //    }
        //    else
        //    {
        //        int remainder;
        //        Math.DivRem(totalRows, pageSize, out remainder);
        //        if (remainder > 0)
        //        {
        //            totalPages += 1;
        //        }
        //    }

        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountRange_SelectPage", 2);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();

        //}

        public static bool DeleteByDiscount(int discountID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountRange_DeleteByDiscount", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }
    }
}
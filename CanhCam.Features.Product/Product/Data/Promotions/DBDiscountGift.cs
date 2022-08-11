/// Author:					Tran Quoc Vuong
/// Created:				2020-06-04
/// Last Modified:			2020-06-04

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscountGift
    {
        public static int Create(
            int discountID,
            Guid appliedItemGuid,
            int discountRangeID,
            string name,
            int productID,
            string imageFile,
            int displayOrder,
            int quantity,
            int giftCount,
            int giftTotal,
            decimal giftPrice,
            string giftNote,
            string url,
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountGift_Insert", 14);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@AppliedItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, appliedItemGuid);
            sph.DefineSqlParameter("@DiscountRangeID", SqlDbType.Int, ParameterDirection.Input, discountRangeID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ImageFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFile);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@GiftCount", SqlDbType.Int, ParameterDirection.Input, giftCount);
            sph.DefineSqlParameter("@GiftTotal", SqlDbType.Int, ParameterDirection.Input, giftTotal);
            sph.DefineSqlParameter("@GiftPrice", SqlDbType.Decimal, ParameterDirection.Input, giftPrice);
            sph.DefineSqlParameter("@GiftNote", SqlDbType.NVarChar, 255, ParameterDirection.Input, giftNote);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_PromotionGift table. Returns true if row updated.
        /// </summary>
        /// <param name="giftID"> giftID </param>
        /// <param name="promotionID"> promotionID </param>
        /// <param name="name"> name </param>
        /// <param name="productID"> productID </param>
        /// <param name="imageFile"> imageFile </param>
        /// <param name="displayOrder"> displayOrder </param>
        /// <param name="quantity"> quantity </param>
        /// <param name="giftCount"> giftCount </param>
        /// <param name="giftTotal"> giftTotal </param>
        /// <param name="giftPrice"> giftPrice </param>
        /// <param name="giftNote"> giftNote </param>
        /// <param name="url"> url </param>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Update(
            int giftID,
            int discountID,
            Guid appliedItemGuid,
            int discountRangeID,
            string name,
            int productID,
            string imageFile,
            int displayOrder,
            int quantity,
            int giftCount,
            int giftTotal,
            decimal giftPrice,
            string giftNote,
            string url,
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountGift_Update", 15);
            sph.DefineSqlParameter("@GiftID", SqlDbType.Int, ParameterDirection.Input, giftID);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@AppliedItemGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, appliedItemGuid);
            sph.DefineSqlParameter("@DiscountRangeID", SqlDbType.Int, ParameterDirection.Input, discountRangeID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ImageFile", SqlDbType.NVarChar, 255, ParameterDirection.Input, imageFile);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@GiftCount", SqlDbType.Int, ParameterDirection.Input, giftCount);
            sph.DefineSqlParameter("@GiftTotal", SqlDbType.Int, ParameterDirection.Input, giftTotal);
            sph.DefineSqlParameter("@GiftPrice", SqlDbType.Decimal, ParameterDirection.Input, giftPrice);
            sph.DefineSqlParameter("@GiftNote", SqlDbType.NVarChar, 255, ParameterDirection.Input, giftNote);
            sph.DefineSqlParameter("@Url", SqlDbType.NVarChar, 255, ParameterDirection.Input, url);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_PromotionGift table. Returns true if row deleted.
        /// </summary>
        /// <param name="giftID"> giftID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int giftID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountGift_Delete", 1);
            sph.DefineSqlParameter("@GiftID", SqlDbType.Int, ParameterDirection.Input, giftID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_PromotionGift table.
        /// </summary>
        /// <param name="giftID"> giftID </param>
        public static IDataReader GetOne(
            int giftID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountGift_SelectOne", 1);
            sph.DefineSqlParameter("@GiftID", SqlDbType.Int, ParameterDirection.Input, giftID);
            return sph.ExecuteReader();
        }

        ///// <summary>
        ///// Gets a count of rows in the gb_PromotionGift table.
        ///// </summary>
        //public static int GetCount()
        //{
        //    return Convert.ToInt32(SqlHelper.ExecuteScalar(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "gb_DiscountGift_GetCount",
        //        null));

        //}

        ///// <summary>
        ///// Gets an IDataReader with all rows in the gb_PromotionGift table.
        ///// </summary>
        //public static IDataReader GetAll()
        //{
        //    return SqlHelper.ExecuteReader(
        //        ConnectionString.GetReadConnectionString(),
        //        CommandType.StoredProcedure,
        //        "gb_DiscountGift_SelectAll",
        //        null);

        //}

        ///// <summary>
        ///// Gets a page of data from the gb_PromotionGift table.
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

        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountGift_SelectPage", 2);
        //    sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
        //    sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
        //    return sph.ExecuteReader();

        //}

        public static IDataReader GetByDiscount(int discountId, Guid? appliedItemGuid = null, int discountRangeID = -1)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountGift_Select", 3);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@AppliedItemGuid", SqlDbType.Int, ParameterDirection.Input, appliedItemGuid);
            sph.DefineSqlParameter("@AiscountRangeID", SqlDbType.Int, ParameterDirection.Input, discountRangeID);

            return sph.ExecuteReader();
        }

        //public static IDataReader GetByDiscounts(string promotionIDs)
        //{
        //    var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountGift_SelectByDiscounts", 1);
        //    sph.DefineSqlParameter("@PromotionIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, promotionIDs);

        //    return sph.ExecuteReader();
        //}

        public static bool DeleteByDiscount(int discountID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountGift_DeleteByDiscount", 1);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > -1);
        }
    }
}
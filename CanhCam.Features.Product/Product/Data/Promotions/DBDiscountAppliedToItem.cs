/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-05
/// Last Modified:			2015-08-05

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBDiscountAppliedToItem
    {
        //public static int Create(
        //    Guid guid,
        //    int discountID,
        //    int itemID,
        //    int appliedType,
        //    bool usePercentage,
        //    decimal discountAmount,
        //    int giftType,
        //    string giftProducts,
        //    string giftCustomProducts,
        //    string giftDescription,
        //    int soldQty,
        //    int dealQty,
        //    int comboSaleQty,
        //    string comboSaleRules,
        //    DateTime? fromDate,
        //    DateTime? toDate,
        //    string giftHtml,
        //    decimal maximumDiscount)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_Insert", 18);
        //    sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
        //    sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
        //    sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
        //    sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
        //    sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
        //    sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
        //    sph.DefineSqlParameter("@GiftType", SqlDbType.Int, ParameterDirection.Input, giftType);
        //    sph.DefineSqlParameter("@GiftProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftProducts);
        //    sph.DefineSqlParameter("@GiftCustomProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftCustomProducts);
        //    sph.DefineSqlParameter("@GiftDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftDescription);
        //    sph.DefineSqlParameter("@SoldQty", SqlDbType.Int, ParameterDirection.Input, soldQty);
        //    sph.DefineSqlParameter("@DealQty", SqlDbType.Int, ParameterDirection.Input, dealQty);
        //    sph.DefineSqlParameter("@ComboSaleQty", SqlDbType.Int, ParameterDirection.Input, comboSaleQty);
        //    sph.DefineSqlParameter("@ComboSaleRules", SqlDbType.NVarChar, -1, ParameterDirection.Input, comboSaleRules);
        //    sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
        //    sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
        //    sph.DefineSqlParameter("@GiftHtml", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftHtml);
        //    sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return rowsAffected;
        //}

        //public static bool Update(
        //    Guid guid,
        //    int discountID,
        //    int itemID,
        //    int appliedType,
        //    bool usePercentage,
        //    decimal discountAmount,
        //    int giftType,
        //    string giftProducts,
        //    string giftCustomProducts,
        //    string giftDescription,
        //    int soldQty,
        //    int dealQty,
        //    int comboSaleQty,
        //    string comboSaleRules,
        //    DateTime? fromDate,
        //    DateTime? toDate,
        //    string giftHtml,
        //    decimal maximumDiscount)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_Update", 18);
        //    sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
        //    sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
        //    sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
        //    sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
        //    sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
        //    sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
        //    sph.DefineSqlParameter("@GiftType", SqlDbType.Int, ParameterDirection.Input, giftType);
        //    sph.DefineSqlParameter("@GiftProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftProducts);
        //    sph.DefineSqlParameter("@GiftCustomProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftCustomProducts);
        //    sph.DefineSqlParameter("@GiftDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftDescription);
        //    sph.DefineSqlParameter("@SoldQty", SqlDbType.Int, ParameterDirection.Input, soldQty);
        //    sph.DefineSqlParameter("@DealQty", SqlDbType.Int, ParameterDirection.Input, dealQty);
        //    sph.DefineSqlParameter("@ComboSaleQty", SqlDbType.Int, ParameterDirection.Input, comboSaleQty);
        //    sph.DefineSqlParameter("@ComboSaleRules", SqlDbType.NVarChar, -1, ParameterDirection.Input, comboSaleRules);
        //    sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
        //    sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
        //    sph.DefineSqlParameter("@GiftHtml", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftHtml);
        //    sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
        //    int rowsAffected = sph.ExecuteNonQuery();
        //    return (rowsAffected > 0);
        //}

        public static bool Save(
            Guid guid,
            int discountID,
            int itemID,
            int appliedType,
            bool usePercentage,
            decimal discountAmount,
            int giftType,
            string giftProducts,
            string giftCustomProducts,
            string giftDescription,
            int soldQty,
            int dealQty,
            int comboSaleQty,
            string comboSaleRules,
            DateTime? fromDate,
            DateTime? toDate,
            string giftHtml,
            decimal maximumDiscount,
            DateTime createdDate,
            int displayOrder,
            string productGifts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_Save", 21);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountID);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemID);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@GiftType", SqlDbType.Int, ParameterDirection.Input, giftType);
            sph.DefineSqlParameter("@GiftProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftProducts);
            sph.DefineSqlParameter("@GiftCustomProducts", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftCustomProducts);
            sph.DefineSqlParameter("@GiftDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftDescription);
            sph.DefineSqlParameter("@SoldQty", SqlDbType.Int, ParameterDirection.Input, soldQty);
            sph.DefineSqlParameter("@DealQty", SqlDbType.Int, ParameterDirection.Input, dealQty);
            sph.DefineSqlParameter("@ComboSaleQty", SqlDbType.Int, ParameterDirection.Input, comboSaleQty);
            sph.DefineSqlParameter("@ComboSaleRules", SqlDbType.NVarChar, -1, ParameterDirection.Input, comboSaleRules);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            sph.DefineSqlParameter("@GiftHtml", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftHtml);
            sph.DefineSqlParameter("@MaximumDiscount", SqlDbType.Decimal, ParameterDirection.Input, maximumDiscount);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_DiscountAppliedToItems table. Returns true if row deleted.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_DiscountAppliedToItems table.
        /// </summary>
        /// <param name="guid"> guid </param>
        public static IDataReader GetOne(
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountAppliedToItems_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static bool DeleteByDiscount(int discountId, int type)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_DeleteByDiscount", 2);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByItem(int itemId, int discountId, int type)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_DeleteByItem", 3);
            sph.DefineSqlParameter("@ItemID", SqlDbType.Int, ParameterDirection.Input, itemId);
            sph.DefineSqlParameter("@DiscountID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@Type", SqlDbType.Int, ParameterDirection.Input, type);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetByDiscount(int discountId, int appliedType)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountAppliedToItems_SelectByDiscount", 2);
            sph.DefineSqlParameter("@CouponID", SqlDbType.Int, ParameterDirection.Input, discountId);
            sph.DefineSqlParameter("@AppliedType", SqlDbType.Int, ParameterDirection.Input, appliedType);
            return sph.ExecuteReader();
        }

        public static IDataReader GetActive(int siteId, int discountType, string zoneIds, string productIds, int options)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountAppliedToItems_SelectActive", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@DiscountType", SqlDbType.Int, ParameterDirection.Input, discountType);
            sph.DefineSqlParameter("@ZoneIDs", SqlDbType.NVarChar, ParameterDirection.Input, zoneIds);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, ParameterDirection.Input, productIds);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            return sph.ExecuteReader();
        }

        //public static IDataReader GetActiveComboSale(int siteId, int productId)
        //{
        //    SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_DiscountAppliedToItems_SelectComboSale", 2);
        //    sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
        //    sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
        //    return sph.ExecuteReader();
        //}

        public static bool UpdateSoldQty(Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_DiscountAppliedToItems_UpdateSoldQty", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
    }
}
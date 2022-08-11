// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2014-7-2
// Last Modified:			2014-7-2

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBOrderItem
    {
        public static int Create(
            Guid guid,
            int orderID,
            int productID,
            int quantity,
            decimal unitPrice,
            decimal price,
            decimal discountAmount,
            decimal couponAmount,
            decimal originalProductCost,
            string attributeDescription,
            string attributesXml,
            int? discountQuantity,
            Guid? discountAppliedGuid,
            int affiliateUserID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderItem_Insert", 14);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@UnitPrice", SqlDbType.Decimal, ParameterDirection.Input, unitPrice);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@CouponAmount", SqlDbType.Decimal, ParameterDirection.Input, couponAmount);
            sph.DefineSqlParameter("@OriginalProductCost", SqlDbType.Decimal, ParameterDirection.Input, originalProductCost);
            sph.DefineSqlParameter("@AttributeDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributeDescription);
            sph.DefineSqlParameter("@AttributesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributesXml);
            sph.DefineSqlParameter("@DiscountQuantity", SqlDbType.Int, ParameterDirection.Input, discountQuantity);
            sph.DefineSqlParameter("@DiscountAppliedGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, discountAppliedGuid);
            sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
            int rowsAffected = sph.ExecuteNonQuery();
            return rowsAffected;
        }

        public static bool Update(
            Guid guid,
            int orderID,
            int productID,
            int quantity,
            decimal unitPrice,
            decimal price,
            decimal discountAmount,
            decimal couponAmount,
            decimal originalProductCost,
            string attributeDescription,
            string attributesXml,
            int? discountQuantity,
            Guid? discountAppliedGuid,
            int affiliateUserID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderItem_Update", 14);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@UnitPrice", SqlDbType.Decimal, ParameterDirection.Input, unitPrice);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@DiscountAmount", SqlDbType.Decimal, ParameterDirection.Input, discountAmount);
            sph.DefineSqlParameter("@CouponAmount", SqlDbType.Decimal, ParameterDirection.Input, couponAmount);
            sph.DefineSqlParameter("@OriginalProductCost", SqlDbType.Decimal, ParameterDirection.Input, originalProductCost);
            sph.DefineSqlParameter("@AttributeDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributeDescription);
            sph.DefineSqlParameter("@AttributesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributesXml);
            sph.DefineSqlParameter("@DiscountQuantity", SqlDbType.Int, ParameterDirection.Input, discountQuantity);
            sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
            sph.DefineSqlParameter("@DiscountAppliedGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, discountAppliedGuid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_OrderItem_Delete", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(
            Guid guid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderItem_SelectOne", 1);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByOrder(int orderId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderItem_SelectByOrder", 1);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderId);
            return sph.ExecuteReader();
        }

        public static int GetCountBySearch(
            int siteID,
            int stateID,
            int orderStatus,
            int paymentMethod,
            int shippingMethod,
            DateTime? fromDate,
            DateTime? toDate,
            decimal? fromOrderTotal,
            decimal? toOrderTotal,
            Guid? userGuid,
            string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_OrderItem_GetCountBySearch",
                    11);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateID);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.Int, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@ShippingMethod", SqlDbType.Int, ParameterDirection.Input, shippingMethod);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            sph.DefineSqlParameter("@FromOrderTotal", SqlDbType.Decimal, ParameterDirection.Input, fromOrderTotal);
            sph.DefineSqlParameter("@ToOrderTotal", SqlDbType.Decimal, ParameterDirection.Input, toOrderTotal);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageBySearch(
            int siteID,
            int stateID,
            int orderStatus,
            int paymentMethod,
            int shippingMethod,
            DateTime? fromDate,
            DateTime? toDate,
            decimal? fromOrderTotal,
            decimal? toOrderTotal,
            Guid? userGuid,
            string keyword,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(),
                    "gb_OrderItem_SelectPageBySearch", 13);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateID);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.Int, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@ShippingMethod", SqlDbType.Int, ParameterDirection.Input, shippingMethod);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            sph.DefineSqlParameter("@FromOrderTotal", SqlDbType.Decimal, ParameterDirection.Input, fromOrderTotal);
            sph.DefineSqlParameter("@ToOrderTotal", SqlDbType.Decimal, ParameterDirection.Input, toOrderTotal);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetTotalItemsByCoupon(int productID, string couponCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(),
                    "gb_OrderItem_GetTotalItemsByCoupon", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            return Convert.ToInt32(sph.ExecuteScalar());
        }
    }
}
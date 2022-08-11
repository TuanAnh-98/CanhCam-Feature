// Author:					Tran Quoc Vuong - itqvuong@gmail.com
// Created:					2014-07-02
// Last Modified:			2014-07-02

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBOrder
    {
        /// <summary>
        /// Inserts a row in the gb_Order table. Returns new integer id.
        /// </summary>
        /// <param name="siteID"> siteID </param>
        /// <param name="staffId"> staffId </param>
        /// <param name="staffProcessId"> staffProcessId </param>
        /// <param name="taskmasterId"> taskmasterId </param>
        /// <param name="orderGuid"> orderGuid </param>
        /// <param name="orderCode"> orderCode </param>
        /// <param name="orderSubtotal"> orderSubtotal </param>
        /// <param name="orderShipping"> orderShipping </param>
        /// <param name="orderDiscount"> orderDiscount </param>
        /// <param name="voucherAmount"> voucherAmount </param>
        /// <param name="couponAmount"> couponAmount </param>
        /// <param name="orderTax"> orderTax </param>
        /// <param name="redeemedRewardPointsAmount"> redeemedRewardPointsAmount </param>
        /// <param name="orderServiceFee"> orderServiceFee </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="redeemedRewardPoints"> redeemedRewardPoints </param>
        /// <param name="voucherCodes"> voucherCodes </param>
        /// <param name="currencyCode"> currencyCode </param>
        /// <param name="couponCode"> couponCode </param>
        /// <param name="orderNote"> orderNote </param>
        /// <param name="billingFirstName"> billingFirstName </param>
        /// <param name="billingLastName"> billingLastName </param>
        /// <param name="billingEmail"> billingEmail </param>
        /// <param name="billingAddress"> billingAddress </param>
        /// <param name="billingPhone"> billingPhone </param>
        /// <param name="billingMobile"> billingMobile </param>
        /// <param name="billingFax"> billingFax </param>
        /// <param name="billingStreet"> billingStreet </param>
        /// <param name="billingWard"> billingWard </param>
        /// <param name="billingDistrictGuid"> billingDistrictGuid </param>
        /// <param name="billingProvinceGuid"> billingProvinceGuid </param>
        /// <param name="billingCountryGuid"> billingCountryGuid </param>
        /// <param name="shippingFirstName"> shippingFirstName </param>
        /// <param name="shippingLastName"> shippingLastName </param>
        /// <param name="shippingEmail"> shippingEmail </param>
        /// <param name="shippingAddress"> shippingAddress </param>
        /// <param name="shippingPhone"> shippingPhone </param>
        /// <param name="shippingMobile"> shippingMobile </param>
        /// <param name="shippingFax"> shippingFax </param>
        /// <param name="shippingWard"> shippingWard </param>
        /// <param name="shippingStreet"> shippingStreet </param>
        /// <param name="shippingDistrictGuid"> shippingDistrictGuid </param>
        /// <param name="shippingProvinceGuid"> shippingProvinceGuid </param>
        /// <param name="shippingCountryGuid"> shippingCountryGuid </param>
        /// <param name="orderStatus"> orderStatus </param>
        /// <param name="paymentStatus"> paymentStatus </param>
        /// <param name="shippingStatus"> shippingStatus </param>
        /// <param name="shippingMethod"> shippingMethod </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="invoiceCompanyName"> invoiceCompanyName </param>
        /// <param name="invoiceCompanyAddress"> invoiceCompanyAddress </param>
        /// <param name="invoiceCompanyTaxCode"> invoiceCompanyTaxCode </param>
        /// <param name="customValuesXml"> customValuesXml </param>
        /// <param name="stateID"> stateID </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="isDeleted"> isDeleted </param>
        /// <param name="storeID"> storeID </param>
        /// <param name="oldStoreID"> oldStoreID </param>
        /// <param name="source"> source </param>
        /// <param name="giftDescription"> giftDescription </param>
        /// <param name="discountIDs"> discountIDs </param>
        /// <param name="calcRewardPoints"> calcRewardPoints </param>
        /// <param name="isCMSCreate"> isCMSCreate </param>
        /// <param name="cancelNote"> cancelNote </param>
        /// <param name="cancelUtc"> cancelUtc </param>
        /// <param name="cancelUserId"> cancelUserId </param>
        /// <param name="affiliate"> affiliate </param>
        /// <returns>int</returns>
        public static int Create(
            int siteID,
            int staffId,
            int staffProcessId,
            int taskmasterId,
            Guid orderGuid,
            string orderCode,
            decimal orderSubtotal,
            decimal orderShipping,
            decimal orderDiscount,
            decimal voucherAmount,
            decimal couponAmount,
            decimal orderTax,
            decimal redeemedRewardPointsAmount,
            decimal orderServiceFee,
            decimal orderTotal,
            int redeemedRewardPoints,
            string voucherCodes,
            string currencyCode,
            string couponCode,
            string orderNote,
            string billingFirstName,
            string billingLastName,
            string billingEmail,
            string billingAddress,
            string billingPhone,
            string billingMobile,
            string billingFax,
            string billingStreet,
            string billingWard,
            Guid billingDistrictGuid,
            Guid billingProvinceGuid,
            Guid billingCountryGuid,
            string shippingFirstName,
            string shippingLastName,
            string shippingEmail,
            string shippingAddress,
            string shippingPhone,
            string shippingMobile,
            string shippingFax,
            string shippingWard,
            string shippingStreet,
            Guid shippingDistrictGuid,
            Guid shippingProvinceGuid,
            Guid shippingCountryGuid,
            int orderStatus,
            int paymentStatus,
            int shippingStatus,
            int shippingMethod,
            int paymentMethod,
            string invoiceCompanyName,
            string invoiceCompanyAddress,
            string invoiceCompanyTaxCode,
            string customValuesXml,
            int stateID,
            Guid userGuid,
            string createdFromIP,
            string createdBy,
            DateTime createdUtc,
            bool isDeleted,
            int storeID,
            int oldStoreID,
            string source,
            string giftDescription,
            string discountIDs,
            int calcRewardPoints,
            bool isCMSCreate,
            string cancelNote,
            DateTime? cancelUtc,
            int cancelUserId,
            string affiliate,
            decimal totalWeight,
            decimal totalLength,
            decimal totalWidth,
            decimal totalHeight,
            string productGifts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_Insert", 75);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@StaffId", SqlDbType.Int, ParameterDirection.Input, staffId);
            sph.DefineSqlParameter("@StaffProcessId", SqlDbType.Int, ParameterDirection.Input, staffProcessId);
            sph.DefineSqlParameter("@TaskmasterId", SqlDbType.Int, ParameterDirection.Input, taskmasterId);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderCode);
            sph.DefineSqlParameter("@OrderSubtotal", SqlDbType.Decimal, ParameterDirection.Input, orderSubtotal);
            sph.DefineSqlParameter("@OrderShipping", SqlDbType.Decimal, ParameterDirection.Input, orderShipping);
            sph.DefineSqlParameter("@OrderDiscount", SqlDbType.Decimal, ParameterDirection.Input, orderDiscount);
            sph.DefineSqlParameter("@VoucherAmount", SqlDbType.Decimal, ParameterDirection.Input, voucherAmount);
            sph.DefineSqlParameter("@CouponAmount", SqlDbType.Decimal, ParameterDirection.Input, couponAmount);
            sph.DefineSqlParameter("@OrderTax", SqlDbType.Decimal, ParameterDirection.Input, orderTax);
            sph.DefineSqlParameter("@RedeemedRewardPointsAmount", SqlDbType.Decimal, ParameterDirection.Input, redeemedRewardPointsAmount);
            sph.DefineSqlParameter("@OrderServiceFee", SqlDbType.Decimal, ParameterDirection.Input, orderServiceFee);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@RedeemedRewardPoints", SqlDbType.Int, ParameterDirection.Input, redeemedRewardPoints);
            sph.DefineSqlParameter("@VoucherCodes", SqlDbType.VarChar, 255, ParameterDirection.Input, voucherCodes);
            sph.DefineSqlParameter("@CurrencyCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, currencyCode);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@OrderNote", SqlDbType.NVarChar, -1, ParameterDirection.Input, orderNote);
            sph.DefineSqlParameter("@BillingFirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingFirstName);
            sph.DefineSqlParameter("@BillingLastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingLastName);
            sph.DefineSqlParameter("@BillingEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingEmail);
            sph.DefineSqlParameter("@BillingAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress);
            sph.DefineSqlParameter("@BillingPhone", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingPhone);
            sph.DefineSqlParameter("@BillingMobile", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingMobile);
            sph.DefineSqlParameter("@BillingFax", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingFax);
            sph.DefineSqlParameter("@BillingStreet", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingStreet);
            sph.DefineSqlParameter("@BillingWard", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingWard);
            sph.DefineSqlParameter("@BillingDistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, billingDistrictGuid);
            sph.DefineSqlParameter("@BillingProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, billingProvinceGuid);
            sph.DefineSqlParameter("@BillingCountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, billingCountryGuid);
            sph.DefineSqlParameter("@ShippingFirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingFirstName);
            sph.DefineSqlParameter("@ShippingLastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingLastName);
            sph.DefineSqlParameter("@ShippingEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingEmail);
            sph.DefineSqlParameter("@ShippingAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingAddress);
            sph.DefineSqlParameter("@ShippingPhone", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingPhone);
            sph.DefineSqlParameter("@ShippingMobile", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingMobile);
            sph.DefineSqlParameter("@ShippingFax", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingFax);
            sph.DefineSqlParameter("@ShippingWard", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingWard);
            sph.DefineSqlParameter("@ShippingStreet", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingStreet);
            sph.DefineSqlParameter("@ShippingDistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, shippingDistrictGuid);
            sph.DefineSqlParameter("@ShippingProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, shippingProvinceGuid);
            sph.DefineSqlParameter("@ShippingCountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, shippingCountryGuid);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@PaymentStatus", SqlDbType.Int, ParameterDirection.Input, paymentStatus);
            sph.DefineSqlParameter("@ShippingStatus", SqlDbType.Int, ParameterDirection.Input, shippingStatus);
            sph.DefineSqlParameter("@ShippingMethod", SqlDbType.Int, ParameterDirection.Input, shippingMethod);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.Int, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@InvoiceCompanyName", SqlDbType.NVarChar, 100, ParameterDirection.Input, invoiceCompanyName);
            sph.DefineSqlParameter("@InvoiceCompanyAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, invoiceCompanyAddress);
            sph.DefineSqlParameter("@InvoiceCompanyTaxCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, invoiceCompanyTaxCode);
            sph.DefineSqlParameter("@CustomValuesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, customValuesXml);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateID);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@OldStoreID", SqlDbType.Int, ParameterDirection.Input, oldStoreID);
            sph.DefineSqlParameter("@Source", SqlDbType.NVarChar, 50, ParameterDirection.Input, source);
            sph.DefineSqlParameter("@GiftDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftDescription);
            sph.DefineSqlParameter("@DiscountIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, discountIDs);
            sph.DefineSqlParameter("@CalcRewardPoints", SqlDbType.Int, ParameterDirection.Input, calcRewardPoints);
            sph.DefineSqlParameter("@IsCMSCreate", SqlDbType.Bit, ParameterDirection.Input, isCMSCreate);
            sph.DefineSqlParameter("@CancelNote", SqlDbType.NVarChar, -1, ParameterDirection.Input, cancelNote);
            sph.DefineSqlParameter("@CancelUtc", SqlDbType.DateTime, ParameterDirection.Input, cancelUtc);
            sph.DefineSqlParameter("@CancelUserId", SqlDbType.Int, ParameterDirection.Input, cancelUserId);
            sph.DefineSqlParameter("@Affiliate", SqlDbType.NVarChar, 255, ParameterDirection.Input, affiliate);
            sph.DefineSqlParameter("@TotalWeight", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalWeight);
            sph.DefineSqlParameter("@TotalLength", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalLength);
            sph.DefineSqlParameter("@TotalWidth", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalWidth);
            sph.DefineSqlParameter("@TotalHeight", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalHeight);
            sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }


        /// <summary>
        /// Updates a row in the gb_Order table. Returns true if row updated.
        /// </summary>
        /// <param name="orderID"> orderID </param>
        /// <param name="siteID"> siteID </param>
        /// <param name="staffId"> staffId </param>
        /// <param name="staffProcessId"> staffProcessId </param>
        /// <param name="taskmasterId"> taskmasterId </param>
        /// <param name="orderGuid"> orderGuid </param>
        /// <param name="orderCode"> orderCode </param>
        /// <param name="orderSubtotal"> orderSubtotal </param>
        /// <param name="orderShipping"> orderShipping </param>
        /// <param name="orderDiscount"> orderDiscount </param>
        /// <param name="voucherAmount"> voucherAmount </param>
        /// <param name="couponAmount"> couponAmount </param>
        /// <param name="orderTax"> orderTax </param>
        /// <param name="redeemedRewardPointsAmount"> redeemedRewardPointsAmount </param>
        /// <param name="orderServiceFee"> orderServiceFee </param>
        /// <param name="orderTotal"> orderTotal </param>
        /// <param name="redeemedRewardPoints"> redeemedRewardPoints </param>
        /// <param name="voucherCodes"> voucherCodes </param>
        /// <param name="currencyCode"> currencyCode </param>
        /// <param name="couponCode"> couponCode </param>
        /// <param name="orderNote"> orderNote </param>
        /// <param name="billingFirstName"> billingFirstName </param>
        /// <param name="billingLastName"> billingLastName </param>
        /// <param name="billingEmail"> billingEmail </param>
        /// <param name="billingAddress"> billingAddress </param>
        /// <param name="billingPhone"> billingPhone </param>
        /// <param name="billingMobile"> billingMobile </param>
        /// <param name="billingFax"> billingFax </param>
        /// <param name="billingStreet"> billingStreet </param>
        /// <param name="billingWard"> billingWard </param>
        /// <param name="billingDistrictGuid"> billingDistrictGuid </param>
        /// <param name="billingProvinceGuid"> billingProvinceGuid </param>
        /// <param name="billingCountryGuid"> billingCountryGuid </param>
        /// <param name="shippingFirstName"> shippingFirstName </param>
        /// <param name="shippingLastName"> shippingLastName </param>
        /// <param name="shippingEmail"> shippingEmail </param>
        /// <param name="shippingAddress"> shippingAddress </param>
        /// <param name="shippingPhone"> shippingPhone </param>
        /// <param name="shippingMobile"> shippingMobile </param>
        /// <param name="shippingFax"> shippingFax </param>
        /// <param name="shippingWard"> shippingWard </param>
        /// <param name="shippingStreet"> shippingStreet </param>
        /// <param name="shippingDistrictGuid"> shippingDistrictGuid </param>
        /// <param name="shippingProvinceGuid"> shippingProvinceGuid </param>
        /// <param name="shippingCountryGuid"> shippingCountryGuid </param>
        /// <param name="orderStatus"> orderStatus </param>
        /// <param name="paymentStatus"> paymentStatus </param>
        /// <param name="shippingStatus"> shippingStatus </param>
        /// <param name="shippingMethod"> shippingMethod </param>
        /// <param name="paymentMethod"> paymentMethod </param>
        /// <param name="invoiceCompanyName"> invoiceCompanyName </param>
        /// <param name="invoiceCompanyAddress"> invoiceCompanyAddress </param>
        /// <param name="invoiceCompanyTaxCode"> invoiceCompanyTaxCode </param>
        /// <param name="customValuesXml"> customValuesXml </param>
        /// <param name="stateID"> stateID </param>
        /// <param name="userGuid"> userGuid </param>
        /// <param name="createdFromIP"> createdFromIP </param>
        /// <param name="createdBy"> createdBy </param>
        /// <param name="createdUtc"> createdUtc </param>
        /// <param name="isDeleted"> isDeleted </param>
        /// <param name="storeID"> storeID </param>
        /// <param name="oldStoreID"> oldStoreID </param>
        /// <param name="source"> source </param>
        /// <param name="giftDescription"> giftDescription </param>
        /// <param name="discountIDs"> discountIDs </param>
        /// <param name="calcRewardPoints"> calcRewardPoints </param>
        /// <param name="isCMSCreate"> isCMSCreate </param>
        /// <param name="cancelNote"> cancelNote </param>
        /// <param name="cancelUtc"> cancelUtc </param>
        /// <param name="cancelUserId"> cancelUserId </param>
        /// <param name="affiliate"> affiliate </param>
        /// <returns>bool</returns>
        public static bool Update(
            int orderID,
            int siteID,
            int staffId,
            int staffProcessId,
            int taskmasterId,
            Guid orderGuid,
            string orderCode,
            decimal orderSubtotal,
            decimal orderShipping,
            decimal orderDiscount,
            decimal voucherAmount,
            decimal couponAmount,
            decimal orderTax,
            decimal redeemedRewardPointsAmount,
            decimal orderServiceFee,
            decimal orderTotal,
            int redeemedRewardPoints,
            string voucherCodes,
            string currencyCode,
            string couponCode,
            string orderNote,
            string billingFirstName,
            string billingLastName,
            string billingEmail,
            string billingAddress,
            string billingPhone,
            string billingMobile,
            string billingFax,
            string billingStreet,
            string billingWard,
            Guid billingDistrictGuid,
            Guid billingProvinceGuid,
            Guid billingCountryGuid,
            string shippingFirstName,
            string shippingLastName,
            string shippingEmail,
            string shippingAddress,
            string shippingPhone,
            string shippingMobile,
            string shippingFax,
            string shippingWard,
            string shippingStreet,
            Guid shippingDistrictGuid,
            Guid shippingProvinceGuid,
            Guid shippingCountryGuid,
            int orderStatus,
            int paymentStatus,
            int shippingStatus,
            int shippingMethod,
            int paymentMethod,
            string invoiceCompanyName,
            string invoiceCompanyAddress,
            string invoiceCompanyTaxCode,
            string customValuesXml,
            int stateID,
            Guid userGuid,
            string createdFromIP,
            string createdBy,
            DateTime createdUtc,
            bool isDeleted,
            int storeID,
            int oldStoreID,
            string source,
            string giftDescription,
            string discountIDs,
            int calcRewardPoints,
            bool isCMSCreate,
            string cancelNote,
            DateTime? cancelUtc,
            int cancelUserId,
            string affiliate,
            decimal totalWeight,
            decimal totalLength,
            decimal totalWidth,
            decimal totalHeight,
            string productGifts)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_Update", 76);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@StaffId", SqlDbType.Int, ParameterDirection.Input, staffId);
            sph.DefineSqlParameter("@StaffProcessId", SqlDbType.Int, ParameterDirection.Input, staffProcessId);
            sph.DefineSqlParameter("@TaskmasterId", SqlDbType.Int, ParameterDirection.Input, taskmasterId);
            sph.DefineSqlParameter("@OrderGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, orderGuid);
            sph.DefineSqlParameter("@OrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderCode);
            sph.DefineSqlParameter("@OrderSubtotal", SqlDbType.Decimal, ParameterDirection.Input, orderSubtotal);
            sph.DefineSqlParameter("@OrderShipping", SqlDbType.Decimal, ParameterDirection.Input, orderShipping);
            sph.DefineSqlParameter("@OrderDiscount", SqlDbType.Decimal, ParameterDirection.Input, orderDiscount);
            sph.DefineSqlParameter("@VoucherAmount", SqlDbType.Decimal, ParameterDirection.Input, voucherAmount);
            sph.DefineSqlParameter("@CouponAmount", SqlDbType.Decimal, ParameterDirection.Input, couponAmount);
            sph.DefineSqlParameter("@OrderTax", SqlDbType.Decimal, ParameterDirection.Input, orderTax);
            sph.DefineSqlParameter("@RedeemedRewardPointsAmount", SqlDbType.Decimal, ParameterDirection.Input, redeemedRewardPointsAmount);
            sph.DefineSqlParameter("@OrderServiceFee", SqlDbType.Decimal, ParameterDirection.Input, orderServiceFee);
            sph.DefineSqlParameter("@OrderTotal", SqlDbType.Decimal, ParameterDirection.Input, orderTotal);
            sph.DefineSqlParameter("@RedeemedRewardPoints", SqlDbType.Int, ParameterDirection.Input, redeemedRewardPoints);
            sph.DefineSqlParameter("@VoucherCodes", SqlDbType.VarChar, 255, ParameterDirection.Input, voucherCodes);
            sph.DefineSqlParameter("@CurrencyCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, currencyCode);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            sph.DefineSqlParameter("@OrderNote", SqlDbType.NVarChar, -1, ParameterDirection.Input, orderNote);
            sph.DefineSqlParameter("@BillingFirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingFirstName);
            sph.DefineSqlParameter("@BillingLastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingLastName);
            sph.DefineSqlParameter("@BillingEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingEmail);
            sph.DefineSqlParameter("@BillingAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingAddress);
            sph.DefineSqlParameter("@BillingPhone", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingPhone);
            sph.DefineSqlParameter("@BillingMobile", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingMobile);
            sph.DefineSqlParameter("@BillingFax", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingFax);
            sph.DefineSqlParameter("@BillingStreet", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingStreet);
            sph.DefineSqlParameter("@BillingWard", SqlDbType.NVarChar, 255, ParameterDirection.Input, billingWard);
            sph.DefineSqlParameter("@BillingDistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, billingDistrictGuid);
            sph.DefineSqlParameter("@BillingProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, billingProvinceGuid);
            sph.DefineSqlParameter("@BillingCountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, billingCountryGuid);
            sph.DefineSqlParameter("@ShippingFirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingFirstName);
            sph.DefineSqlParameter("@ShippingLastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingLastName);
            sph.DefineSqlParameter("@ShippingEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingEmail);
            sph.DefineSqlParameter("@ShippingAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingAddress);
            sph.DefineSqlParameter("@ShippingPhone", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingPhone);
            sph.DefineSqlParameter("@ShippingMobile", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingMobile);
            sph.DefineSqlParameter("@ShippingFax", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingFax);
            sph.DefineSqlParameter("@ShippingWard", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingWard);
            sph.DefineSqlParameter("@ShippingStreet", SqlDbType.NVarChar, 255, ParameterDirection.Input, shippingStreet);
            sph.DefineSqlParameter("@ShippingDistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, shippingDistrictGuid);
            sph.DefineSqlParameter("@ShippingProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, shippingProvinceGuid);
            sph.DefineSqlParameter("@ShippingCountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, shippingCountryGuid);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@PaymentStatus", SqlDbType.Int, ParameterDirection.Input, paymentStatus);
            sph.DefineSqlParameter("@ShippingStatus", SqlDbType.Int, ParameterDirection.Input, shippingStatus);
            sph.DefineSqlParameter("@ShippingMethod", SqlDbType.Int, ParameterDirection.Input, shippingMethod);
            sph.DefineSqlParameter("@PaymentMethod", SqlDbType.Int, ParameterDirection.Input, paymentMethod);
            sph.DefineSqlParameter("@InvoiceCompanyName", SqlDbType.NVarChar, 100, ParameterDirection.Input, invoiceCompanyName);
            sph.DefineSqlParameter("@InvoiceCompanyAddress", SqlDbType.NVarChar, 255, ParameterDirection.Input, invoiceCompanyAddress);
            sph.DefineSqlParameter("@InvoiceCompanyTaxCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, invoiceCompanyTaxCode);
            sph.DefineSqlParameter("@CustomValuesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, customValuesXml);
            sph.DefineSqlParameter("@StateID", SqlDbType.Int, ParameterDirection.Input, stateID);
            sph.DefineSqlParameter("@UserGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, userGuid);
            sph.DefineSqlParameter("@CreatedFromIP", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdFromIP);
            sph.DefineSqlParameter("@CreatedBy", SqlDbType.NVarChar, 255, ParameterDirection.Input, createdBy);
            sph.DefineSqlParameter("@CreatedUtc", SqlDbType.DateTime, ParameterDirection.Input, createdUtc);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@OldStoreID", SqlDbType.Int, ParameterDirection.Input, oldStoreID);
            sph.DefineSqlParameter("@Source", SqlDbType.NVarChar, 50, ParameterDirection.Input, source);
            sph.DefineSqlParameter("@GiftDescription", SqlDbType.NVarChar, -1, ParameterDirection.Input, giftDescription);
            sph.DefineSqlParameter("@DiscountIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, discountIDs);
            sph.DefineSqlParameter("@CalcRewardPoints", SqlDbType.Int, ParameterDirection.Input, calcRewardPoints);
            sph.DefineSqlParameter("@IsCMSCreate", SqlDbType.Bit, ParameterDirection.Input, isCMSCreate);
            sph.DefineSqlParameter("@CancelNote", SqlDbType.NVarChar, -1, ParameterDirection.Input, cancelNote);
            sph.DefineSqlParameter("@CancelUtc", SqlDbType.DateTime, ParameterDirection.Input, cancelUtc);
            sph.DefineSqlParameter("@CancelUserId", SqlDbType.Int, ParameterDirection.Input, cancelUserId);
            sph.DefineSqlParameter("@Affiliate", SqlDbType.NVarChar, 255, ParameterDirection.Input, affiliate);
            sph.DefineSqlParameter("@TotalWeight", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalWeight);
            sph.DefineSqlParameter("@TotalLength", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalLength);
            sph.DefineSqlParameter("@TotalWidth", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalWidth);
            sph.DefineSqlParameter("@TotalHeight", SqlDbType.NVarChar, 255, ParameterDirection.Input, totalHeight);
            sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);

            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);

        }

        public static bool UpdateCalcRewardPoints(
            int orderID,
            int calcRewardPoints)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_UpdateCalcRewardPoints", 2);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CalcRewardPoints", SqlDbType.Int, ParameterDirection.Input, calcRewardPoints);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        public static bool UpdateStatus(
          int orderID,
          int status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_UpdateStatus", 2);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@Status", SqlDbType.Int, ParameterDirection.Input, status);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        public static bool UpdateAPIStatus(
             int orderID,
             string code,
             int status)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_UpdateAPIInfo", 3);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@ApiOrderCode", SqlDbType.Int, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@ApiOrderStatus", SqlDbType.Int, ParameterDirection.Input, status);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        public static bool UpdateCompletioned(
             int orderID,
             DateTime? dateTime)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_UpdateCompletioned", 2);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            sph.DefineSqlParameter("@CompletionedUtc", SqlDbType.DateTime, ParameterDirection.Input, dateTime);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
        public static bool Delete(int orderID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_Delete", 1);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int orderID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectOne", 1);
            sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
            return sph.ExecuteReader();
        }

        public static int GetCount(
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
            int storeId,
            string keyword)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_GetCount", 12);
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
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeId);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPage(
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
            int storeId,
            string keyword,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectPage", 14);
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
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeId);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 50, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByCoupon(int siteID, string couponCode)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectByCoupon", 2);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@CouponCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, couponCode);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByCode(string orderCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectByCode", 1);
            sph.DefineSqlParameter("@OrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderCode);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByDate(int siteID, DateTime fromDate, DateTime toDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectByDate", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            return sph.ExecuteReader();
        }

        public static IDataReader GetReportBestsellers(
            int siteID,
            int zoneID,
            int orderStatus,
            DateTime? fromDate,
            DateTime? toDate,
            int totalRows,
            int orderBy)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectBestsellersReport", 7);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            sph.DefineSqlParameter("@Top", SqlDbType.Int, ParameterDirection.Input, totalRows);
            sph.DefineSqlParameter("@OrderBy", SqlDbType.Int, ParameterDirection.Input, orderBy);
            return sph.ExecuteReader();
        }

        public static IDataReader GetReportSales(
            int siteID,
            int state,
            int orderStatus,
            DateTime? fromDate,
            DateTime? toDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_ReportSales", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@State", SqlDbType.Int, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            return sph.ExecuteReader();
        }

        public static IDataReader GetReportSalesByWeek(
            int siteID,
            int state,
            int orderStatus,
            DateTime? fromDate,
            DateTime? toDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_ReportSalesByWeek", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@State", SqlDbType.Int, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            return sph.ExecuteReader();
        }

        public static IDataReader GetReportSalesByMonth(
            int siteID,
            int state,
            int orderStatus,
            DateTime? fromDate,
            DateTime? toDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_ReportSalesByMonth", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@State", SqlDbType.Int, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            return sph.ExecuteReader();
        }

        public static IDataReader GetReportSalesByYear(
            int siteID,
            int state,
            int orderStatus,
            DateTime? fromDate,
            DateTime? toDate)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_ReportSalesByYear", 5);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@State", SqlDbType.Int, ParameterDirection.Input, state);
            sph.DefineSqlParameter("@OrderStatus", SqlDbType.Int, ParameterDirection.Input, orderStatus);
            sph.DefineSqlParameter("@FromDate", SqlDbType.DateTime, ParameterDirection.Input, fromDate);
            sph.DefineSqlParameter("@ToDate", SqlDbType.DateTime, ParameterDirection.Input, toDate);
            return sph.ExecuteReader();
        }

        public static int GetCountByStores(
            int userID,
            bool isAdmin,
            int storeID,
            int defaultStoreID,
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
           int staffProcessId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_GetCountByStores", 16);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@DefaultStoreID", SqlDbType.Int, ParameterDirection.Input, defaultStoreID);
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
            sph.DefineSqlParameter("@StaffProcessId", SqlDbType.Int, ParameterDirection.Input, staffProcessId);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageByStores(
            int userID,
            bool isAdmin,
            int storeID,
            int defaultStoreID,
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
           int staffProcessId,
            int pageNumber,
            int pageSize)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectPageByStores", 18);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@DefaultStoreID", SqlDbType.Int, ParameterDirection.Input, defaultStoreID);
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
            sph.DefineSqlParameter("@StaffProcessId", SqlDbType.Int, ParameterDirection.Input, staffProcessId);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }
    }
}
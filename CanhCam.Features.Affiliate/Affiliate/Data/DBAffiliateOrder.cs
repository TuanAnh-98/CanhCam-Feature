// Author:					Tran Quoc Vuong
// Created:					2022-8-5
// Last Modified:			2022-8-5

using System;
using System.Data;

namespace CanhCam.Data
{

	public static class DBAffiliateOrder
	{

		/// <summary>
		/// Inserts a row in the gb_Order table. Returns new integer id.
		/// </summary>
		/// <param name="siteID"> siteID </param>
		/// <param name="staffId"> staffId </param>
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
		/// <param name="apiOrderCode"> apiOrderCode </param>
		/// <param name="apiOrderStatus"> apiOrderStatus </param>
		/// <param name="calcRewardPoints"> calcRewardPoints </param>
		/// <param name="completionedUtc"> completionedUtc </param>
		/// <param name="isCMSCreate"> isCMSCreate </param>
		/// <param name="staffProcessId"> staffProcessId </param>
		/// <param name="taskmasterId"> taskmasterId </param>
		/// <param name="cancelNote"> cancelNote </param>
		/// <param name="cancelUtc"> cancelUtc </param>
		/// <param name="affiliate"> affiliate </param>
		/// <param name="cancelUserId"> cancelUserId </param>
		/// <param name="productGifts"> productGifts </param>
		/// <param name="totalWeight"> totalWeight </param>
		/// <param name="totalLength"> totalLength </param>
		/// <param name="totalWidth"> totalWidth </param>
		/// <param name="totalHeight"> totalHeight </param>
		/// <returns>int</returns>
		public static int Create(
			int siteID,
			int staffId,
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
			string apiOrderCode,
			int apiOrderStatus,
			int calcRewardPoints,
			DateTime completionedUtc,
			bool isCMSCreate,
			int staffProcessId,
			int taskmasterId,
			string cancelNote,
			DateTime cancelUtc,
			string affiliate,
			int cancelUserId,
			string productGifts,
			decimal totalWeight,
			decimal totalLength,
			decimal totalWidth,
			decimal totalHeight)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_Insert", 78);
			sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
			sph.DefineSqlParameter("@StaffId", SqlDbType.Int, ParameterDirection.Input, staffId);
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
			sph.DefineSqlParameter("@ApiOrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, apiOrderCode);
			sph.DefineSqlParameter("@ApiOrderStatus", SqlDbType.Int, ParameterDirection.Input, apiOrderStatus);
			sph.DefineSqlParameter("@CalcRewardPoints", SqlDbType.Int, ParameterDirection.Input, calcRewardPoints);
			sph.DefineSqlParameter("@CompletionedUtc", SqlDbType.DateTime, ParameterDirection.Input, completionedUtc);
			sph.DefineSqlParameter("@IsCMSCreate", SqlDbType.Bit, ParameterDirection.Input, isCMSCreate);
			sph.DefineSqlParameter("@StaffProcessId", SqlDbType.Int, ParameterDirection.Input, staffProcessId);
			sph.DefineSqlParameter("@TaskmasterId", SqlDbType.Int, ParameterDirection.Input, taskmasterId);
			sph.DefineSqlParameter("@CancelNote", SqlDbType.NVarChar, -1, ParameterDirection.Input, cancelNote);
			sph.DefineSqlParameter("@CancelUtc", SqlDbType.DateTime, ParameterDirection.Input, cancelUtc);
			sph.DefineSqlParameter("@Affiliate", SqlDbType.NVarChar, 255, ParameterDirection.Input, affiliate);
			sph.DefineSqlParameter("@CancelUserId", SqlDbType.Int, ParameterDirection.Input, cancelUserId);
			sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);
			sph.DefineSqlParameter("@TotalWeight", SqlDbType.Decimal, ParameterDirection.Input, totalWeight);
			sph.DefineSqlParameter("@TotalLength", SqlDbType.Decimal, ParameterDirection.Input, totalLength);
			sph.DefineSqlParameter("@TotalWidth", SqlDbType.Decimal, ParameterDirection.Input, totalWidth);
			sph.DefineSqlParameter("@TotalHeight", SqlDbType.Decimal, ParameterDirection.Input, totalHeight);
			int newID = Convert.ToInt32(sph.ExecuteScalar());
			return newID;
		}


		/// <summary>
		/// Updates a row in the gb_Order table. Returns true if row updated.
		/// </summary>
		/// <param name="orderID"> orderID </param>
		/// <param name="siteID"> siteID </param>
		/// <param name="staffId"> staffId </param>
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
		/// <param name="apiOrderCode"> apiOrderCode </param>
		/// <param name="apiOrderStatus"> apiOrderStatus </param>
		/// <param name="calcRewardPoints"> calcRewardPoints </param>
		/// <param name="completionedUtc"> completionedUtc </param>
		/// <param name="isCMSCreate"> isCMSCreate </param>
		/// <param name="staffProcessId"> staffProcessId </param>
		/// <param name="taskmasterId"> taskmasterId </param>
		/// <param name="cancelNote"> cancelNote </param>
		/// <param name="cancelUtc"> cancelUtc </param>
		/// <param name="affiliate"> affiliate </param>
		/// <param name="cancelUserId"> cancelUserId </param>
		/// <param name="productGifts"> productGifts </param>
		/// <param name="totalWeight"> totalWeight </param>
		/// <param name="totalLength"> totalLength </param>
		/// <param name="totalWidth"> totalWidth </param>
		/// <param name="totalHeight"> totalHeight </param>
		/// <returns>bool</returns>
		public static bool Update(
			int orderID,
			int siteID,
			int staffId,
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
			string apiOrderCode,
			int apiOrderStatus,
			int calcRewardPoints,
			DateTime completionedUtc,
			bool isCMSCreate,
			int staffProcessId,
			int taskmasterId,
			string cancelNote,
			DateTime cancelUtc,
			string affiliate,
			int cancelUserId,
			string productGifts,
			decimal totalWeight,
			decimal totalLength,
			decimal totalWidth,
			decimal totalHeight)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_Update", 79);
			sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
			sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
			sph.DefineSqlParameter("@StaffId", SqlDbType.Int, ParameterDirection.Input, staffId);
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
			sph.DefineSqlParameter("@ApiOrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, apiOrderCode);
			sph.DefineSqlParameter("@ApiOrderStatus", SqlDbType.Int, ParameterDirection.Input, apiOrderStatus);
			sph.DefineSqlParameter("@CalcRewardPoints", SqlDbType.Int, ParameterDirection.Input, calcRewardPoints);
			sph.DefineSqlParameter("@CompletionedUtc", SqlDbType.DateTime, ParameterDirection.Input, completionedUtc);
			sph.DefineSqlParameter("@IsCMSCreate", SqlDbType.Bit, ParameterDirection.Input, isCMSCreate);
			sph.DefineSqlParameter("@StaffProcessId", SqlDbType.Int, ParameterDirection.Input, staffProcessId);
			sph.DefineSqlParameter("@TaskmasterId", SqlDbType.Int, ParameterDirection.Input, taskmasterId);
			sph.DefineSqlParameter("@CancelNote", SqlDbType.NVarChar, -1, ParameterDirection.Input, cancelNote);
			sph.DefineSqlParameter("@CancelUtc", SqlDbType.DateTime, ParameterDirection.Input, cancelUtc);
			sph.DefineSqlParameter("@Affiliate", SqlDbType.NVarChar, 255, ParameterDirection.Input, affiliate);
			sph.DefineSqlParameter("@CancelUserId", SqlDbType.Int, ParameterDirection.Input, cancelUserId);
			sph.DefineSqlParameter("@ProductGifts", SqlDbType.NVarChar, -1, ParameterDirection.Input, productGifts);
			sph.DefineSqlParameter("@TotalWeight", SqlDbType.Decimal, ParameterDirection.Input, totalWeight);
			sph.DefineSqlParameter("@TotalLength", SqlDbType.Decimal, ParameterDirection.Input, totalLength);
			sph.DefineSqlParameter("@TotalWidth", SqlDbType.Decimal, ParameterDirection.Input, totalWidth);
			sph.DefineSqlParameter("@TotalHeight", SqlDbType.Decimal, ParameterDirection.Input, totalHeight);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Deletes a row from the gb_Order table. Returns true if row deleted.
		/// </summary>
		/// <param name="orderID"> orderID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int orderID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Order_Delete", 1);
			sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Gets an IDataReader with one row from the gb_Order table.
		/// </summary>
		/// <param name="orderID"> orderID </param>
		public static IDataReader GetOne(
			int orderID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectOne", 1);
			sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
			return sph.ExecuteReader();

		}

		/// <summary>
		/// Gets a count of rows in the gb_Order table.
		/// </summary>
		public static int GetCount()
		{

			return Convert.ToInt32(SqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_Order_GetCount",
				null));

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the gb_Order table.
		/// </summary>
		public static IDataReader GetAll()
		{

			return SqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_Order_SelectAll",
				null);

		}

		/// <summary>
		/// Gets a page of data from the gb_Order table.
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

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Order_SelectPage", 2);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

	}

}

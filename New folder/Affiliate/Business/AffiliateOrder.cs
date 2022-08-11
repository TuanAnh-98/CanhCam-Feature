
// Author:					Tran Quoc Vuong
// Created:					2022-8-5
// Last Modified:			2022-8-5

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

	public class AffiliateOrder
	{

		#region Constructors

		public AffiliateOrder()
		{ }


		public AffiliateOrder(
			int orderID)
		{
			this.GetOrder(
				orderID);
		}

		#endregion

		#region Private Properties

		private int orderID = -1;
		private int siteID = -1;
		private int staffId = -1;
		private Guid orderGuid = Guid.Empty;
		private string orderCode = string.Empty;
		private decimal orderSubtotal;
		private decimal orderShipping;
		private decimal orderDiscount;
		private decimal voucherAmount;
		private decimal couponAmount;
		private decimal orderTax;
		private decimal redeemedRewardPointsAmount;
		private decimal orderServiceFee;
		private decimal orderTotal;
		private int redeemedRewardPoints = -1;
		private string voucherCodes = string.Empty;
		private string currencyCode = string.Empty;
		private string couponCode = string.Empty;
		private string orderNote = string.Empty;
		private string billingFirstName = string.Empty;
		private string billingLastName = string.Empty;
		private string billingEmail = string.Empty;
		private string billingAddress = string.Empty;
		private string billingPhone = string.Empty;
		private string billingMobile = string.Empty;
		private string billingFax = string.Empty;
		private string billingStreet = string.Empty;
		private string billingWard = string.Empty;
		private Guid billingDistrictGuid = Guid.Empty;
		private Guid billingProvinceGuid = Guid.Empty;
		private Guid billingCountryGuid = Guid.Empty;
		private string shippingFirstName = string.Empty;
		private string shippingLastName = string.Empty;
		private string shippingEmail = string.Empty;
		private string shippingAddress = string.Empty;
		private string shippingPhone = string.Empty;
		private string shippingMobile = string.Empty;
		private string shippingFax = string.Empty;
		private string shippingWard = string.Empty;
		private string shippingStreet = string.Empty;
		private Guid shippingDistrictGuid = Guid.Empty;
		private Guid shippingProvinceGuid = Guid.Empty;
		private Guid shippingCountryGuid = Guid.Empty;
		private int orderStatus = -1;
		private int paymentStatus = -1;
		private int shippingStatus = -1;
		private int shippingMethod = -1;
		private int paymentMethod = -1;
		private string invoiceCompanyName = string.Empty;
		private string invoiceCompanyAddress = string.Empty;
		private string invoiceCompanyTaxCode = string.Empty;
		private string customValuesXml = string.Empty;
		private int stateID = -1;
		private Guid userGuid = Guid.Empty;
		private string createdFromIP = string.Empty;
		private string createdBy = string.Empty;
		private DateTime createdUtc = DateTime.Now;
		private bool isDeleted = false;
		private int storeID = -1;
		private int oldStoreID = -1;
		private string source = string.Empty;
		private string giftDescription = string.Empty;
		private string discountIDs = string.Empty;
		private string apiOrderCode = string.Empty;
		private int apiOrderStatus = -1;
		private int calcRewardPoints = -1;
		private DateTime completionedUtc = DateTime.Now;
		private bool isCMSCreate = false;
		private int staffProcessId = -1;
		private int taskmasterId = -1;
		private string cancelNote = string.Empty;
		private DateTime cancelUtc = DateTime.Now;
		private string affiliate = string.Empty;
		private int cancelUserId = -1;
		private string productGifts = string.Empty;
		private decimal totalWeight;
		private decimal totalLength;
		private decimal totalWidth;
		private decimal totalHeight;

		#endregion

		#region Public Properties

		public int OrderID
		{
			get { return orderID; }
			set { orderID = value; }
		}
		public int SiteID
		{
			get { return siteID; }
			set { siteID = value; }
		}
		public int StaffId
		{
			get { return staffId; }
			set { staffId = value; }
		}
		public Guid OrderGuid
		{
			get { return orderGuid; }
			set { orderGuid = value; }
		}
		public string OrderCode
		{
			get { return orderCode; }
			set { orderCode = value; }
		}
		public decimal OrderSubtotal
		{
			get { return orderSubtotal; }
			set { orderSubtotal = value; }
		}
		public decimal OrderShipping
		{
			get { return orderShipping; }
			set { orderShipping = value; }
		}
		public decimal OrderDiscount
		{
			get { return orderDiscount; }
			set { orderDiscount = value; }
		}
		public decimal VoucherAmount
		{
			get { return voucherAmount; }
			set { voucherAmount = value; }
		}
		public decimal CouponAmount
		{
			get { return couponAmount; }
			set { couponAmount = value; }
		}
		public decimal OrderTax
		{
			get { return orderTax; }
			set { orderTax = value; }
		}
		public decimal RedeemedRewardPointsAmount
		{
			get { return redeemedRewardPointsAmount; }
			set { redeemedRewardPointsAmount = value; }
		}
		public decimal OrderServiceFee
		{
			get { return orderServiceFee; }
			set { orderServiceFee = value; }
		}
		public decimal OrderTotal
		{
			get { return orderTotal; }
			set { orderTotal = value; }
		}
		public int RedeemedRewardPoints
		{
			get { return redeemedRewardPoints; }
			set { redeemedRewardPoints = value; }
		}
		public string VoucherCodes
		{
			get { return voucherCodes; }
			set { voucherCodes = value; }
		}
		public string CurrencyCode
		{
			get { return currencyCode; }
			set { currencyCode = value; }
		}
		public string CouponCode
		{
			get { return couponCode; }
			set { couponCode = value; }
		}
		public string OrderNote
		{
			get { return orderNote; }
			set { orderNote = value; }
		}
		public string BillingFirstName
		{
			get { return billingFirstName; }
			set { billingFirstName = value; }
		}
		public string BillingLastName
		{
			get { return billingLastName; }
			set { billingLastName = value; }
		}
		public string BillingEmail
		{
			get { return billingEmail; }
			set { billingEmail = value; }
		}
		public string BillingAddress
		{
			get { return billingAddress; }
			set { billingAddress = value; }
		}
		public string BillingPhone
		{
			get { return billingPhone; }
			set { billingPhone = value; }
		}
		public string BillingMobile
		{
			get { return billingMobile; }
			set { billingMobile = value; }
		}
		public string BillingFax
		{
			get { return billingFax; }
			set { billingFax = value; }
		}
		public string BillingStreet
		{
			get { return billingStreet; }
			set { billingStreet = value; }
		}
		public string BillingWard
		{
			get { return billingWard; }
			set { billingWard = value; }
		}
		public Guid BillingDistrictGuid
		{
			get { return billingDistrictGuid; }
			set { billingDistrictGuid = value; }
		}
		public Guid BillingProvinceGuid
		{
			get { return billingProvinceGuid; }
			set { billingProvinceGuid = value; }
		}
		public Guid BillingCountryGuid
		{
			get { return billingCountryGuid; }
			set { billingCountryGuid = value; }
		}
		public string ShippingFirstName
		{
			get { return shippingFirstName; }
			set { shippingFirstName = value; }
		}
		public string ShippingLastName
		{
			get { return shippingLastName; }
			set { shippingLastName = value; }
		}
		public string ShippingEmail
		{
			get { return shippingEmail; }
			set { shippingEmail = value; }
		}
		public string ShippingAddress
		{
			get { return shippingAddress; }
			set { shippingAddress = value; }
		}
		public string ShippingPhone
		{
			get { return shippingPhone; }
			set { shippingPhone = value; }
		}
		public string ShippingMobile
		{
			get { return shippingMobile; }
			set { shippingMobile = value; }
		}
		public string ShippingFax
		{
			get { return shippingFax; }
			set { shippingFax = value; }
		}
		public string ShippingWard
		{
			get { return shippingWard; }
			set { shippingWard = value; }
		}
		public string ShippingStreet
		{
			get { return shippingStreet; }
			set { shippingStreet = value; }
		}
		public Guid ShippingDistrictGuid
		{
			get { return shippingDistrictGuid; }
			set { shippingDistrictGuid = value; }
		}
		public Guid ShippingProvinceGuid
		{
			get { return shippingProvinceGuid; }
			set { shippingProvinceGuid = value; }
		}
		public Guid ShippingCountryGuid
		{
			get { return shippingCountryGuid; }
			set { shippingCountryGuid = value; }
		}
		public int OrderStatus
		{
			get { return orderStatus; }
			set { orderStatus = value; }
		}
		public int PaymentStatus
		{
			get { return paymentStatus; }
			set { paymentStatus = value; }
		}
		public int ShippingStatus
		{
			get { return shippingStatus; }
			set { shippingStatus = value; }
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
		public string InvoiceCompanyName
		{
			get { return invoiceCompanyName; }
			set { invoiceCompanyName = value; }
		}
		public string InvoiceCompanyAddress
		{
			get { return invoiceCompanyAddress; }
			set { invoiceCompanyAddress = value; }
		}
		public string InvoiceCompanyTaxCode
		{
			get { return invoiceCompanyTaxCode; }
			set { invoiceCompanyTaxCode = value; }
		}
		public string CustomValuesXml
		{
			get { return customValuesXml; }
			set { customValuesXml = value; }
		}
		public int StateID
		{
			get { return stateID; }
			set { stateID = value; }
		}
		public Guid UserGuid
		{
			get { return userGuid; }
			set { userGuid = value; }
		}
		public string CreatedFromIP
		{
			get { return createdFromIP; }
			set { createdFromIP = value; }
		}
		public string CreatedBy
		{
			get { return createdBy; }
			set { createdBy = value; }
		}
		public DateTime CreatedUtc
		{
			get { return createdUtc; }
			set { createdUtc = value; }
		}
		public bool IsDeleted
		{
			get { return isDeleted; }
			set { isDeleted = value; }
		}
		public int StoreID
		{
			get { return storeID; }
			set { storeID = value; }
		}
		public int OldStoreID
		{
			get { return oldStoreID; }
			set { oldStoreID = value; }
		}
		public string Source
		{
			get { return source; }
			set { source = value; }
		}
		public string GiftDescription
		{
			get { return giftDescription; }
			set { giftDescription = value; }
		}
		public string DiscountIDs
		{
			get { return discountIDs; }
			set { discountIDs = value; }
		}
		public string ApiOrderCode
		{
			get { return apiOrderCode; }
			set { apiOrderCode = value; }
		}
		public int ApiOrderStatus
		{
			get { return apiOrderStatus; }
			set { apiOrderStatus = value; }
		}
		public int CalcRewardPoints
		{
			get { return calcRewardPoints; }
			set { calcRewardPoints = value; }
		}
		public DateTime CompletionedUtc
		{
			get { return completionedUtc; }
			set { completionedUtc = value; }
		}
		public bool IsCMSCreate
		{
			get { return isCMSCreate; }
			set { isCMSCreate = value; }
		}
		public int StaffProcessId
		{
			get { return staffProcessId; }
			set { staffProcessId = value; }
		}
		public int TaskmasterId
		{
			get { return taskmasterId; }
			set { taskmasterId = value; }
		}
		public string CancelNote
		{
			get { return cancelNote; }
			set { cancelNote = value; }
		}
		public DateTime CancelUtc
		{
			get { return cancelUtc; }
			set { cancelUtc = value; }
		}
		public string Affiliate
		{
			get { return affiliate; }
			set { affiliate = value; }
		}
		public int CancelUserId
		{
			get { return cancelUserId; }
			set { cancelUserId = value; }
		}
		public string ProductGifts
		{
			get { return productGifts; }
			set { productGifts = value; }
		}
		public decimal TotalWeight
		{
			get { return totalWeight; }
			set { totalWeight = value; }
		}
		public decimal TotalLength
		{
			get { return totalLength; }
			set { totalLength = value; }
		}
		public decimal TotalWidth
		{
			get { return totalWidth; }
			set { totalWidth = value; }
		}
		public decimal TotalHeight
		{
			get { return totalHeight; }
			set { totalHeight = value; }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of Order.
		/// </summary>
		/// <param name="orderID"> orderID </param>
		private void GetOrder(
			int orderID)
		{
			using (IDataReader reader = DBAffiliateOrder.GetOne(
				orderID))
			{
				if (reader.Read())
				{
					this.orderID = Convert.ToInt32(reader["OrderID"]);
					this.siteID = Convert.ToInt32(reader["SiteID"]);
					this.staffId = Convert.ToInt32(reader["StaffId"]);
					this.orderGuid = new Guid(reader["OrderGuid"].ToString());
					this.orderCode = reader["OrderCode"].ToString();
					this.orderSubtotal = Convert.ToDecimal(reader["OrderSubtotal"]);
					this.orderShipping = Convert.ToDecimal(reader["OrderShipping"]);
					this.orderDiscount = Convert.ToDecimal(reader["OrderDiscount"]);
					this.voucherAmount = Convert.ToDecimal(reader["VoucherAmount"]);
					this.couponAmount = Convert.ToDecimal(reader["CouponAmount"]);
					this.orderTax = Convert.ToDecimal(reader["OrderTax"]);
					this.redeemedRewardPointsAmount = Convert.ToDecimal(reader["RedeemedRewardPointsAmount"]);
					this.orderServiceFee = Convert.ToDecimal(reader["OrderServiceFee"]);
					this.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
					this.redeemedRewardPoints = Convert.ToInt32(reader["RedeemedRewardPoints"]);
					this.voucherCodes = reader["VoucherCodes"].ToString();
					this.currencyCode = reader["CurrencyCode"].ToString();
					this.couponCode = reader["CouponCode"].ToString();
					this.orderNote = reader["OrderNote"].ToString();
					this.billingFirstName = reader["BillingFirstName"].ToString();
					this.billingLastName = reader["BillingLastName"].ToString();
					this.billingEmail = reader["BillingEmail"].ToString();
					this.billingAddress = reader["BillingAddress"].ToString();
					this.billingPhone = reader["BillingPhone"].ToString();
					this.billingMobile = reader["BillingMobile"].ToString();
					this.billingFax = reader["BillingFax"].ToString();
					this.billingStreet = reader["BillingStreet"].ToString();
					this.billingWard = reader["BillingWard"].ToString();
					this.billingDistrictGuid = new Guid(reader["BillingDistrictGuid"].ToString());
					this.billingProvinceGuid = new Guid(reader["BillingProvinceGuid"].ToString());
					this.billingCountryGuid = new Guid(reader["BillingCountryGuid"].ToString());
					this.shippingFirstName = reader["ShippingFirstName"].ToString();
					this.shippingLastName = reader["ShippingLastName"].ToString();
					this.shippingEmail = reader["ShippingEmail"].ToString();
					this.shippingAddress = reader["ShippingAddress"].ToString();
					this.shippingPhone = reader["ShippingPhone"].ToString();
					this.shippingMobile = reader["ShippingMobile"].ToString();
					this.shippingFax = reader["ShippingFax"].ToString();
					this.shippingWard = reader["ShippingWard"].ToString();
					this.shippingStreet = reader["ShippingStreet"].ToString();
					this.shippingDistrictGuid = new Guid(reader["ShippingDistrictGuid"].ToString());
					this.shippingProvinceGuid = new Guid(reader["ShippingProvinceGuid"].ToString());
					this.shippingCountryGuid = new Guid(reader["ShippingCountryGuid"].ToString());
					this.orderStatus = Convert.ToInt32(reader["OrderStatus"]);
					this.paymentStatus = Convert.ToInt32(reader["PaymentStatus"]);
					this.shippingStatus = Convert.ToInt32(reader["ShippingStatus"]);
					this.shippingMethod = Convert.ToInt32(reader["ShippingMethod"]);
					this.paymentMethod = Convert.ToInt32(reader["PaymentMethod"]);
					this.invoiceCompanyName = reader["InvoiceCompanyName"].ToString();
					this.invoiceCompanyAddress = reader["InvoiceCompanyAddress"].ToString();
					this.invoiceCompanyTaxCode = reader["InvoiceCompanyTaxCode"].ToString();
					this.customValuesXml = reader["CustomValuesXml"].ToString();
					this.stateID = Convert.ToInt32(reader["StateID"]);
					this.userGuid = new Guid(reader["UserGuid"].ToString());
					this.createdFromIP = reader["CreatedFromIP"].ToString();
					this.createdBy = reader["CreatedBy"].ToString();
					this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
					this.storeID = Convert.ToInt32(reader["StoreID"]);
					this.oldStoreID = Convert.ToInt32(reader["OldStoreID"]);
					this.source = reader["Source"].ToString();
					this.giftDescription = reader["GiftDescription"].ToString();
					this.discountIDs = reader["DiscountIDs"].ToString();
					this.apiOrderCode = reader["ApiOrderCode"].ToString();
					this.apiOrderStatus = Convert.ToInt32(reader["ApiOrderStatus"]);
					this.calcRewardPoints = Convert.ToInt32(reader["CalcRewardPoints"]);
					this.completionedUtc = Convert.ToDateTime(reader["CompletionedUtc"]);
					this.isCMSCreate = Convert.ToBoolean(reader["IsCMSCreate"]);
					this.staffProcessId = Convert.ToInt32(reader["StaffProcessId"]);
					this.taskmasterId = Convert.ToInt32(reader["TaskmasterId"]);
					this.cancelNote = reader["CancelNote"].ToString();
					this.cancelUtc = Convert.ToDateTime(reader["CancelUtc"]);
					this.affiliate = reader["Affiliate"].ToString();
					this.cancelUserId = Convert.ToInt32(reader["CancelUserId"]);
					this.productGifts = reader["ProductGifts"].ToString();
					this.totalWeight = Convert.ToDecimal(reader["TotalWeight"]);
					this.totalLength = Convert.ToDecimal(reader["TotalLength"]);
					this.totalWidth = Convert.ToDecimal(reader["TotalWidth"]);
					this.totalHeight = Convert.ToDecimal(reader["TotalHeight"]);
				}
			}

		}

		/// <summary>
		/// Persists a new instance of Order. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			int newID = 0;

			newID = DBAffiliateOrder.Create(
				this.siteID,
				this.staffId,
				this.orderGuid,
				this.orderCode,
				this.orderSubtotal,
				this.orderShipping,
				this.orderDiscount,
				this.voucherAmount,
				this.couponAmount,
				this.orderTax,
				this.redeemedRewardPointsAmount,
				this.orderServiceFee,
				this.orderTotal,
				this.redeemedRewardPoints,
				this.voucherCodes,
				this.currencyCode,
				this.couponCode,
				this.orderNote,
				this.billingFirstName,
				this.billingLastName,
				this.billingEmail,
				this.billingAddress,
				this.billingPhone,
				this.billingMobile,
				this.billingFax,
				this.billingStreet,
				this.billingWard,
				this.billingDistrictGuid,
				this.billingProvinceGuid,
				this.billingCountryGuid,
				this.shippingFirstName,
				this.shippingLastName,
				this.shippingEmail,
				this.shippingAddress,
				this.shippingPhone,
				this.shippingMobile,
				this.shippingFax,
				this.shippingWard,
				this.shippingStreet,
				this.shippingDistrictGuid,
				this.shippingProvinceGuid,
				this.shippingCountryGuid,
				this.orderStatus,
				this.paymentStatus,
				this.shippingStatus,
				this.shippingMethod,
				this.paymentMethod,
				this.invoiceCompanyName,
				this.invoiceCompanyAddress,
				this.invoiceCompanyTaxCode,
				this.customValuesXml,
				this.stateID,
				this.userGuid,
				this.createdFromIP,
				this.createdBy,
				this.createdUtc,
				this.isDeleted,
				this.storeID,
				this.oldStoreID,
				this.source,
				this.giftDescription,
				this.discountIDs,
				this.apiOrderCode,
				this.apiOrderStatus,
				this.calcRewardPoints,
				this.completionedUtc,
				this.isCMSCreate,
				this.staffProcessId,
				this.taskmasterId,
				this.cancelNote,
				this.cancelUtc,
				this.affiliate,
				this.cancelUserId,
				this.productGifts,
				this.totalWeight,
				this.totalLength,
				this.totalWidth,
				this.totalHeight);

			this.orderID = newID;

			return (newID > 0);

		}


		/// <summary>
		/// Updates this instance of Order. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBAffiliateOrder.Update(
				this.orderID,
				this.siteID,
				this.staffId,
				this.orderGuid,
				this.orderCode,
				this.orderSubtotal,
				this.orderShipping,
				this.orderDiscount,
				this.voucherAmount,
				this.couponAmount,
				this.orderTax,
				this.redeemedRewardPointsAmount,
				this.orderServiceFee,
				this.orderTotal,
				this.redeemedRewardPoints,
				this.voucherCodes,
				this.currencyCode,
				this.couponCode,
				this.orderNote,
				this.billingFirstName,
				this.billingLastName,
				this.billingEmail,
				this.billingAddress,
				this.billingPhone,
				this.billingMobile,
				this.billingFax,
				this.billingStreet,
				this.billingWard,
				this.billingDistrictGuid,
				this.billingProvinceGuid,
				this.billingCountryGuid,
				this.shippingFirstName,
				this.shippingLastName,
				this.shippingEmail,
				this.shippingAddress,
				this.shippingPhone,
				this.shippingMobile,
				this.shippingFax,
				this.shippingWard,
				this.shippingStreet,
				this.shippingDistrictGuid,
				this.shippingProvinceGuid,
				this.shippingCountryGuid,
				this.orderStatus,
				this.paymentStatus,
				this.shippingStatus,
				this.shippingMethod,
				this.paymentMethod,
				this.invoiceCompanyName,
				this.invoiceCompanyAddress,
				this.invoiceCompanyTaxCode,
				this.customValuesXml,
				this.stateID,
				this.userGuid,
				this.createdFromIP,
				this.createdBy,
				this.createdUtc,
				this.isDeleted,
				this.storeID,
				this.oldStoreID,
				this.source,
				this.giftDescription,
				this.discountIDs,
				this.apiOrderCode,
				this.apiOrderStatus,
				this.calcRewardPoints,
				this.completionedUtc,
				this.isCMSCreate,
				this.staffProcessId,
				this.taskmasterId,
				this.cancelNote,
				this.cancelUtc,
				this.affiliate,
				this.cancelUserId,
				this.productGifts,
				this.totalWeight,
				this.totalLength,
				this.totalWidth,
				this.totalHeight);

		}





		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of Order. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.orderID > 0)
			{
				return this.Update();
			}
			else
			{
				return this.Create();
			}
		}




		#endregion

		#region Static Methods

		/// <summary>
		/// Deletes an instance of Order. Returns true on success.
		/// </summary>
		/// <param name="orderID"> orderID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int orderID)
		{
			return DBAffiliateOrder.Delete(
				orderID);
		}


		/// <summary>
		/// Gets a count of Order. 
		/// </summary>
		public static int GetCount()
		{
			return DBAffiliateOrder.GetCount();
		}

		private static List<AffiliateOrder> LoadListFromReader(IDataReader reader)
		{
			List<AffiliateOrder> orderList = new List<AffiliateOrder>();
			try
			{
				while (reader.Read())
				{
					AffiliateOrder order = new AffiliateOrder();
					order.orderID = Convert.ToInt32(reader["OrderID"]);
					order.siteID = Convert.ToInt32(reader["SiteID"]);
					order.staffId = Convert.ToInt32(reader["StaffId"]);
					order.orderGuid = new Guid(reader["OrderGuid"].ToString());
					order.orderCode = reader["OrderCode"].ToString();
					order.orderSubtotal = Convert.ToDecimal(reader["OrderSubtotal"]);
					order.orderShipping = Convert.ToDecimal(reader["OrderShipping"]);
					order.orderDiscount = Convert.ToDecimal(reader["OrderDiscount"]);
					order.voucherAmount = Convert.ToDecimal(reader["VoucherAmount"]);
					order.couponAmount = Convert.ToDecimal(reader["CouponAmount"]);
					order.orderTax = Convert.ToDecimal(reader["OrderTax"]);
					order.redeemedRewardPointsAmount = Convert.ToDecimal(reader["RedeemedRewardPointsAmount"]);
					order.orderServiceFee = Convert.ToDecimal(reader["OrderServiceFee"]);
					order.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
					order.redeemedRewardPoints = Convert.ToInt32(reader["RedeemedRewardPoints"]);
					order.voucherCodes = reader["VoucherCodes"].ToString();
					order.currencyCode = reader["CurrencyCode"].ToString();
					order.couponCode = reader["CouponCode"].ToString();
					order.orderNote = reader["OrderNote"].ToString();
					order.billingFirstName = reader["BillingFirstName"].ToString();
					order.billingLastName = reader["BillingLastName"].ToString();
					order.billingEmail = reader["BillingEmail"].ToString();
					order.billingAddress = reader["BillingAddress"].ToString();
					order.billingPhone = reader["BillingPhone"].ToString();
					order.billingMobile = reader["BillingMobile"].ToString();
					order.billingFax = reader["BillingFax"].ToString();
					order.billingStreet = reader["BillingStreet"].ToString();
					order.billingWard = reader["BillingWard"].ToString();
					order.billingDistrictGuid = new Guid(reader["BillingDistrictGuid"].ToString());
					order.billingProvinceGuid = new Guid(reader["BillingProvinceGuid"].ToString());
					order.billingCountryGuid = new Guid(reader["BillingCountryGuid"].ToString());
					order.shippingFirstName = reader["ShippingFirstName"].ToString();
					order.shippingLastName = reader["ShippingLastName"].ToString();
					order.shippingEmail = reader["ShippingEmail"].ToString();
					order.shippingAddress = reader["ShippingAddress"].ToString();
					order.shippingPhone = reader["ShippingPhone"].ToString();
					order.shippingMobile = reader["ShippingMobile"].ToString();
					order.shippingFax = reader["ShippingFax"].ToString();
					order.shippingWard = reader["ShippingWard"].ToString();
					order.shippingStreet = reader["ShippingStreet"].ToString();
					order.shippingDistrictGuid = new Guid(reader["ShippingDistrictGuid"].ToString());
					order.shippingProvinceGuid = new Guid(reader["ShippingProvinceGuid"].ToString());
					order.shippingCountryGuid = new Guid(reader["ShippingCountryGuid"].ToString());
					order.orderStatus = Convert.ToInt32(reader["OrderStatus"]);
					order.paymentStatus = Convert.ToInt32(reader["PaymentStatus"]);
					order.shippingStatus = Convert.ToInt32(reader["ShippingStatus"]);
					order.shippingMethod = Convert.ToInt32(reader["ShippingMethod"]);
					order.paymentMethod = Convert.ToInt32(reader["PaymentMethod"]);
					order.invoiceCompanyName = reader["InvoiceCompanyName"].ToString();
					order.invoiceCompanyAddress = reader["InvoiceCompanyAddress"].ToString();
					order.invoiceCompanyTaxCode = reader["InvoiceCompanyTaxCode"].ToString();
					order.customValuesXml = reader["CustomValuesXml"].ToString();
					order.stateID = Convert.ToInt32(reader["StateID"]);
					order.userGuid = new Guid(reader["UserGuid"].ToString());
					order.createdFromIP = reader["CreatedFromIP"].ToString();
					order.createdBy = reader["CreatedBy"].ToString();
					order.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
					order.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
					order.storeID = Convert.ToInt32(reader["StoreID"]);
					order.oldStoreID = Convert.ToInt32(reader["OldStoreID"]);
					order.source = reader["Source"].ToString();
					order.giftDescription = reader["GiftDescription"].ToString();
					order.discountIDs = reader["DiscountIDs"].ToString();
					order.apiOrderCode = reader["ApiOrderCode"].ToString();
					order.apiOrderStatus = Convert.ToInt32(reader["ApiOrderStatus"]);
					order.calcRewardPoints = Convert.ToInt32(reader["CalcRewardPoints"]);
					order.completionedUtc = Convert.ToDateTime(reader["CompletionedUtc"]);
					order.isCMSCreate = Convert.ToBoolean(reader["IsCMSCreate"]);
					order.staffProcessId = Convert.ToInt32(reader["StaffProcessId"]);
					order.taskmasterId = Convert.ToInt32(reader["TaskmasterId"]);
					order.cancelNote = reader["CancelNote"].ToString();
					order.cancelUtc = Convert.ToDateTime(reader["CancelUtc"]);
					order.affiliate = reader["Affiliate"].ToString();
					order.cancelUserId = Convert.ToInt32(reader["CancelUserId"]);
					order.productGifts = reader["ProductGifts"].ToString();
					order.totalWeight = Convert.ToDecimal(reader["TotalWeight"]);
					order.totalLength = Convert.ToDecimal(reader["TotalLength"]);
					order.totalWidth = Convert.ToDecimal(reader["TotalWidth"]);
					order.totalHeight = Convert.ToDecimal(reader["TotalHeight"]);
					orderList.Add(order);

				}
			}
			finally
			{
				reader.Close();
			}

			return orderList;

		}

		/// <summary>
		/// Gets an IList with all instances of Order.
		/// </summary>
		public static List<AffiliateOrder> GetAll()
		{
			IDataReader reader = DBAffiliateOrder.GetAll();
			return LoadListFromReader(reader);

		}

		/// <summary>
		/// Gets an IList with page of instances of Order.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static List<AffiliateOrder> GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliateOrder.GetPage(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}



		#endregion

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByOrderID(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.OrderID.CompareTo(order2.OrderID);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareBySiteID(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.SiteID.CompareTo(order2.SiteID);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByStaffId(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.StaffId.CompareTo(order2.StaffId);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByOrderCode(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.OrderCode.CompareTo(order2.OrderCode);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByRedeemedRewardPoints(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.RedeemedRewardPoints.CompareTo(order2.RedeemedRewardPoints);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByVoucherCodes(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.VoucherCodes.CompareTo(order2.VoucherCodes);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCurrencyCode(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CurrencyCode.CompareTo(order2.CurrencyCode);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCouponCode(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CouponCode.CompareTo(order2.CouponCode);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByOrderNote(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.OrderNote.CompareTo(order2.OrderNote);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingFirstName(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingFirstName.CompareTo(order2.BillingFirstName);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingLastName(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingLastName.CompareTo(order2.BillingLastName);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingEmail(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingEmail.CompareTo(order2.BillingEmail);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingAddress(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingAddress.CompareTo(order2.BillingAddress);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingPhone(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingPhone.CompareTo(order2.BillingPhone);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingMobile(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingMobile.CompareTo(order2.BillingMobile);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingFax(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingFax.CompareTo(order2.BillingFax);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingStreet(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingStreet.CompareTo(order2.BillingStreet);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByBillingWard(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.BillingWard.CompareTo(order2.BillingWard);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingFirstName(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingFirstName.CompareTo(order2.ShippingFirstName);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingLastName(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingLastName.CompareTo(order2.ShippingLastName);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingEmail(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingEmail.CompareTo(order2.ShippingEmail);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingAddress(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingAddress.CompareTo(order2.ShippingAddress);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingPhone(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingPhone.CompareTo(order2.ShippingPhone);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingMobile(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingMobile.CompareTo(order2.ShippingMobile);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingFax(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingFax.CompareTo(order2.ShippingFax);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingWard(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingWard.CompareTo(order2.ShippingWard);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingStreet(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingStreet.CompareTo(order2.ShippingStreet);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByOrderStatus(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.OrderStatus.CompareTo(order2.OrderStatus);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByPaymentStatus(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.PaymentStatus.CompareTo(order2.PaymentStatus);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingStatus(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingStatus.CompareTo(order2.ShippingStatus);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByShippingMethod(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ShippingMethod.CompareTo(order2.ShippingMethod);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByPaymentMethod(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.PaymentMethod.CompareTo(order2.PaymentMethod);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByInvoiceCompanyName(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.InvoiceCompanyName.CompareTo(order2.InvoiceCompanyName);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByInvoiceCompanyAddress(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.InvoiceCompanyAddress.CompareTo(order2.InvoiceCompanyAddress);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByInvoiceCompanyTaxCode(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.InvoiceCompanyTaxCode.CompareTo(order2.InvoiceCompanyTaxCode);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCustomValuesXml(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CustomValuesXml.CompareTo(order2.CustomValuesXml);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByStateID(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.StateID.CompareTo(order2.StateID);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCreatedFromIP(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CreatedFromIP.CompareTo(order2.CreatedFromIP);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCreatedBy(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CreatedBy.CompareTo(order2.CreatedBy);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCreatedUtc(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CreatedUtc.CompareTo(order2.CreatedUtc);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByStoreID(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.StoreID.CompareTo(order2.StoreID);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByOldStoreID(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.OldStoreID.CompareTo(order2.OldStoreID);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareBySource(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.Source.CompareTo(order2.Source);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByGiftDescription(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.GiftDescription.CompareTo(order2.GiftDescription);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByDiscountIDs(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.DiscountIDs.CompareTo(order2.DiscountIDs);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByApiOrderCode(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ApiOrderCode.CompareTo(order2.ApiOrderCode);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByApiOrderStatus(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ApiOrderStatus.CompareTo(order2.ApiOrderStatus);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCalcRewardPoints(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CalcRewardPoints.CompareTo(order2.CalcRewardPoints);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCompletionedUtc(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CompletionedUtc.CompareTo(order2.CompletionedUtc);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByStaffProcessId(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.StaffProcessId.CompareTo(order2.StaffProcessId);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByTaskmasterId(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.TaskmasterId.CompareTo(order2.TaskmasterId);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCancelNote(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CancelNote.CompareTo(order2.CancelNote);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCancelUtc(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CancelUtc.CompareTo(order2.CancelUtc);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByAffiliate(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.Affiliate.CompareTo(order2.Affiliate);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByCancelUserId(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.CancelUserId.CompareTo(order2.CancelUserId);
		}
		/// <summary>
		/// Compares 2 instances of Order.
		/// </summary>
		public static int CompareByProductGifts(AffiliateOrder order1, AffiliateOrder order2)
		{
			return order1.ProductGifts.CompareTo(order2.ProductGifts);
		}

		#endregion


	}

}

// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
// Created:					2014-07-22
// Last Modified:			2014-07-22

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum PaymentStatus
    {
        None = 0,
        Paid = 1,
        Pending = 2,
        NotYet = 3
    }

    public enum ShippingStatus
    {
        None = 0,
        DeliveryForDelivery = 10,
        Sent = 20,
        DeliveryFailed = 30,
    }

    public enum OrderERPStatus
    {
        None = 0,
        Synchronized = 10,
        SyncError = 20,
        Asynchronous = 30
    }

    public enum OrderSource
    {
        Website = 0,
        Hotline = 10,
        CallCenter = 20
    }

    public enum OrderStatus
    {
        New = 0,
        Confirmed = 5,
        PaymentInformationConfirmed = 10,
        HavePackedTheGoods = 15,
        DeliveryForDelivery = 20,
        DeliveryToCustomers = 25,
        Completed = 99,
        Cancelled = 95,
        DeliveryFailed = 90,
    }

    public enum OrderPaymentStatus
    {
        Successful = 1,
        Pending = 2,
        NotSuccessful = 3
    }

    [Serializable]
    public class Order
    {
        #region Constructors

        public Order()
        { }

        public Order(
            int orderID)
        {
            this.GetOrder(
                orderID);
        }

        #endregion Constructors

        #region Private Properties

        private int orderID = -1;
        private int siteID = -1;
        private int staffId = -1;
        private int staffProcessId = -1;
        private int taskmasterId = -1;
        private Guid orderGuid = Guid.Empty;
        private string orderCode = string.Empty;
        private decimal orderSubtotal;
        private decimal orderShipping;
        private decimal orderDiscount;
        private decimal orderTax;
        private decimal orderTotal;
        private string currencyCode = string.Empty;
        private string couponCode = string.Empty;
        private decimal couponAmount;
        private decimal orderServiceFee;
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
        private int orderStatus = 0;
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
        private DateTime createdUtc = DateTime.UtcNow;
        private bool isDeleted = false;

        private int redeemedRewardPoints = 0;
        private decimal redeemedRewardPointsAmount = 0;
        private string voucherCodes = string.Empty;
        private decimal voucherAmount = 0;

        private int storeID = -1;
        private int oldStoreID = -1;
        private string source = string.Empty;
        private string storeName = string.Empty;

        private string giftDescription = string.Empty;
        private string discountIDs = string.Empty;
        private string apiOrderCode = string.Empty;
        private int apiOrderStatus = 0;
        private int calcRewardPoints;

        private DateTime? completionedUtc = null;
        private bool isCMSCreate = false;
        private string cancelNote = string.Empty;
        private DateTime? cancelUtc = DateTime.Now;
        private int cancelUserId = -1;
        private string affiliate = string.Empty;
        private decimal totalWeight = decimal.Zero;
        private decimal totalLength = decimal.Zero;
        private decimal totalWidth = decimal.Zero;
        private decimal totalHeight = decimal.Zero;

        private string productGifts = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int OrderId
        {
            get { return orderID; }
            set { orderID = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public int StaffId
        {
            get { return staffId; }
            set { staffId = value; }
        }

        public int StaffProcessId
        {
            get { return staffProcessId; }
            set { staffProcessId = value; }
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

        public decimal OrderTax
        {
            get { return orderTax; }
            set { orderTax = value; }
        }

        public decimal OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
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

        public string PaymentAPICode
        {
            get { return shippingFax; }
            set { shippingFax = value; }
        }

        public string ShippingWard
        {
            get { return shippingWard; }
            set { shippingWard = value; }
        }

        public string ShippingOption
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

        public int StateId
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

        public int RedeemedRewardPoints
        {
            get { return redeemedRewardPoints; }
            set { redeemedRewardPoints = value; }
        }

        public decimal RedeemedRewardPointsAmount
        {
            get { return redeemedRewardPointsAmount; }
            set { redeemedRewardPointsAmount = value; }
        }

        public string VoucherCodes
        {
            get { return voucherCodes; }
            set { voucherCodes = value; }
        }

        public decimal VoucherAmount
        {
            get { return voucherAmount; }
            set { voucherAmount = value; }
        }

        public decimal OrderServiceFee
        {
            get { return orderServiceFee; }
            set { orderServiceFee = value; }
        }

        public decimal OrderCouponAmount
        {
            get { return couponAmount; }
            set { couponAmount = value; }
        }

        public int StoreId { get => storeID; set => storeID = value; }
        public int OldStoreId { get => oldStoreID; set => oldStoreID = value; }
        public string Source { get => source; set => source = value; }
        public string StoreName { get => storeName; set => storeName = value; }
        public string GiftDescription { get => giftDescription; set => giftDescription = value; }
        public string DiscountIDs { get => discountIDs; set => discountIDs = value; }
        public string ApiOrderCode { get => apiOrderCode; set => apiOrderCode = value; }
        public int ApiOrderstatus { get => apiOrderStatus; set => apiOrderStatus = value; }
        public int CalcRewardPoints { get => calcRewardPoints; set => calcRewardPoints = value; }
        public DateTime? CompletionedUtc { get => completionedUtc; set => completionedUtc = value; }

        public bool IsCMSCreate
        {
            get => isCMSCreate;
            set => isCMSCreate = value;
        }

        public string CancelNote
        {
            get { return cancelNote; }
            set { cancelNote = value; }
        }

        public DateTime? CancelUtc
        {
            get { return cancelUtc; }
            set { cancelUtc = value; }
        }

        public int CancelUserId
        {
            get { return cancelUserId; }
            set { cancelUserId = value; }
        }

        public string Affiliate
        {
            get { return affiliate; }
            set { affiliate = value; }
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

        public string ProductGifts { get => productGifts; set => productGifts = value; }

        /// <summary>
        /// Card for Payment online not save in database
        /// </summary>
        public string BankCode { get; set; }

        /// <summary>
        /// Card for DiscountPayment online not save in database
        /// </summary>
        public decimal DiscountPayment { get; set; }

        #endregion Public Properties

        #region Private Methods

        private void GetOrder(int orderID)
        {
            using (IDataReader reader = DBOrder.GetOne(orderID))
            {
                if (reader.Read())
                {
                    GetOrderFromReader(reader, this);
                }
            }
        }

        public static void GetOrderFromReader(IDataReader reader, Order order)
        {
            order.orderID = Convert.ToInt32(reader["OrderID"]);
            order.siteID = Convert.ToInt32(reader["SiteID"]);
            order.orderGuid = new Guid(reader["OrderGuid"].ToString());
            order.orderCode = reader["OrderCode"].ToString();
            order.orderSubtotal = Convert.ToDecimal(reader["OrderSubtotal"]);
            order.orderShipping = Convert.ToDecimal(reader["OrderShipping"]);
            order.orderDiscount = Convert.ToDecimal(reader["OrderDiscount"]);
            order.orderTax = Convert.ToDecimal(reader["OrderTax"]);
            order.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
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

            if (reader["StaffId"] != DBNull.Value)
                order.staffId = Convert.ToInt32(reader["StaffId"]);
            if (reader["CouponAmount"] != DBNull.Value)
                order.couponAmount = Convert.ToDecimal(reader["CouponAmount"]);
            if (reader["RedeemedRewardPoints"] != DBNull.Value)
                order.redeemedRewardPoints = Convert.ToInt32(reader["RedeemedRewardPoints"]);
            if (reader["RedeemedRewardPointsAmount"] != DBNull.Value)
                order.redeemedRewardPointsAmount = Convert.ToDecimal(reader["RedeemedRewardPointsAmount"]);
            if (reader["OrderServiceFee"] != DBNull.Value)
                order.orderServiceFee = Convert.ToDecimal(reader["OrderServiceFee"]);
            if (reader["VoucherCodes"] != DBNull.Value)
                order.voucherCodes = reader["VoucherCodes"].ToString();
            if (reader["VoucherAmount"] != DBNull.Value)
                order.voucherAmount = Convert.ToDecimal(reader["VoucherAmount"]);
            if (reader["StoreID"] != DBNull.Value)
                order.storeID = Convert.ToInt32(reader["StoreID"]);
            if (reader["OldStoreID"] != DBNull.Value)
                order.oldStoreID = Convert.ToInt32(reader["OldStoreID"]);
            if (reader["Source"] != DBNull.Value)
                order.source = reader["Source"].ToString();
            if (reader["GiftDescription"] != DBNull.Value)
                order.giftDescription = reader["GiftDescription"].ToString();
            if (reader["DiscountIDs"] != DBNull.Value)
                order.discountIDs = reader["DiscountIDs"].ToString();
            if (reader["ApiOrderCode"] != DBNull.Value)
                order.apiOrderCode = reader["ApiOrderCode"].ToString();
            if (reader["ApiOrderStatus"] != DBNull.Value)
                order.apiOrderStatus = Convert.ToInt32(reader["ApiOrderStatus"].ToString());
            if (reader["CalcRewardPoints"] != DBNull.Value)
                order.calcRewardPoints = Convert.ToInt32(reader["CalcRewardPoints"].ToString());
            if (reader["CompletionedUtc"] != DBNull.Value)
                order.completionedUtc = Convert.ToDateTime(reader["CompletionedUtc"].ToString());
            if (reader["IsCMSCreate"] != DBNull.Value)
                order.isCMSCreate = Convert.ToBoolean(reader["IsCMSCreate"]);
            if (reader["CancelNote"] != DBNull.Value)
                order.cancelNote = reader["CancelNote"].ToString();
            if (reader["CancelUtc"] != DBNull.Value)
                order.cancelUtc = Convert.ToDateTime(reader["CancelUtc"]);
            if (reader["CancelUserId"] != DBNull.Value)
                order.cancelUserId = Convert.ToInt32(reader["CancelUserId"]);
            if (reader["Affiliate"] != DBNull.Value)
                order.affiliate = reader["Affiliate"].ToString();

            if (reader["StaffProcessId"] != DBNull.Value)
                order.staffProcessId = Convert.ToInt32(reader["StaffProcessId"]);
            if (reader["TaskmasterId"] != DBNull.Value)
                order.taskmasterId = Convert.ToInt32(reader["TaskmasterId"]);
            if (reader["TotalWeight"] != DBNull.Value)
                order.totalWeight = Convert.ToDecimal(reader["TotalWeight"]);
            if (reader["TotalLength"] != DBNull.Value)
                order.totalLength = Convert.ToDecimal(reader["TotalLength"]);
            if (reader["TotalWidth"] != DBNull.Value)
                order.totalWidth = Convert.ToDecimal(reader["TotalWidth"]);
            if (reader["TotalHeight"] != DBNull.Value)
                order.totalHeight = Convert.ToDecimal(reader["TotalHeight"]);
            if (reader["ProductGifts"] != DBNull.Value)
                order.productGifts = reader["ProductGifts"].ToString();
        }

        /// <summary>
        /// Persists a new instance of Order. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBOrder.Create(
                this.siteID,
                this.staffId,
                this.staffProcessId,
                this.taskmasterId,
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
                this.calcRewardPoints,
                this.isCMSCreate,
                this.cancelNote,
                this.cancelUtc,
                this.cancelUserId,
                this.affiliate,
                this.totalWeight,
                this.totalLength,
                this.totalWidth,
                this.totalHeight,
                this.productGifts);

            this.orderID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of Order. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
    	private bool Update()
        {
            return DBOrder.Update(
                this.orderID,
                this.siteID,
                this.staffId,
                this.staffProcessId,
                this.taskmasterId,
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
                this.calcRewardPoints,
                this.isCMSCreate,
                this.cancelNote,
                this.cancelUtc,
                this.cancelUserId,
                this.affiliate,
                this.totalWeight,
                this.totalLength,
                this.totalWidth,
                this.totalHeight,
                this.productGifts);
        }

        public bool UpdateCalcRewardPoints()
        {
            return DBOrder.UpdateCalcRewardPoints(this.orderID, this.calcRewardPoints);
        }

        #endregion Private Methods

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
                this.orderGuid = Guid.NewGuid();
                this.createdUtc = DateTime.UtcNow; //Prevent conflict GenerateOrderCode
                return this.Create();
            }
        }

        public bool SaveERPStatus()
        {
            return DBOrder.UpdateAPIStatus(this.orderID, this.apiOrderCode, this.apiOrderStatus);
        }
        public bool UpdateCompletioned()
        {
            return DBOrder.UpdateCompletioned(this.orderID, this.completionedUtc);
        }
        

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int orderID)
        {
            return DBOrder.Delete(orderID);
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
            return DBOrder.GetCount(siteID, stateID, orderStatus, paymentMethod, shippingMethod, fromDate, toDate, fromOrderTotal, toOrderTotal, userGuid, storeId, keyword);
        }

        private static List<Order> LoadListFromReader(IDataReader reader, bool getStoreName = false)
        {
            var orderList = new List<Order>();
            try
            {
                while (reader.Read())
                {
                    Order order = new Order
                    {
                        orderID = Convert.ToInt32(reader["OrderID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        orderGuid = new Guid(reader["OrderGuid"].ToString()),
                        orderCode = reader["OrderCode"].ToString(),
                        orderSubtotal = Convert.ToDecimal(reader["OrderSubtotal"]),
                        orderShipping = Convert.ToDecimal(reader["OrderShipping"]),
                        orderDiscount = Convert.ToDecimal(reader["OrderDiscount"]),
                        orderTax = Convert.ToDecimal(reader["OrderTax"]),
                        orderTotal = Convert.ToDecimal(reader["OrderTotal"]),
                        currencyCode = reader["CurrencyCode"].ToString(),
                        couponCode = reader["CouponCode"].ToString(),
                        orderNote = reader["OrderNote"].ToString(),
                        billingFirstName = reader["BillingFirstName"].ToString(),
                        billingLastName = reader["BillingLastName"].ToString(),
                        billingEmail = reader["BillingEmail"].ToString(),
                        billingAddress = reader["BillingAddress"].ToString(),
                        billingPhone = reader["BillingPhone"].ToString(),
                        billingMobile = reader["BillingMobile"].ToString(),
                        billingFax = reader["BillingFax"].ToString(),
                        billingStreet = reader["BillingStreet"].ToString(),
                        billingWard = reader["BillingWard"].ToString(),
                        billingDistrictGuid = new Guid(reader["BillingDistrictGuid"].ToString()),
                        billingProvinceGuid = new Guid(reader["BillingProvinceGuid"].ToString()),
                        billingCountryGuid = new Guid(reader["BillingCountryGuid"].ToString()),
                        shippingFirstName = reader["ShippingFirstName"].ToString(),
                        shippingLastName = reader["ShippingLastName"].ToString(),
                        shippingEmail = reader["ShippingEmail"].ToString(),
                        shippingAddress = reader["ShippingAddress"].ToString(),
                        shippingPhone = reader["ShippingPhone"].ToString(),
                        shippingMobile = reader["ShippingMobile"].ToString(),
                        shippingFax = reader["ShippingFax"].ToString(),
                        shippingWard = reader["ShippingWard"].ToString(),
                        shippingStreet = reader["ShippingStreet"].ToString(),
                        shippingDistrictGuid = new Guid(reader["ShippingDistrictGuid"].ToString()),
                        shippingProvinceGuid = new Guid(reader["ShippingProvinceGuid"].ToString()),
                        shippingCountryGuid = new Guid(reader["ShippingCountryGuid"].ToString()),
                        orderStatus = Convert.ToInt32(reader["OrderStatus"]),
                        paymentStatus = Convert.ToInt32(reader["PaymentStatus"]),
                        shippingStatus = Convert.ToInt32(reader["ShippingStatus"]),
                        shippingMethod = Convert.ToInt32(reader["ShippingMethod"]),
                        paymentMethod = Convert.ToInt32(reader["PaymentMethod"]),
                        invoiceCompanyName = reader["InvoiceCompanyName"].ToString(),
                        invoiceCompanyAddress = reader["InvoiceCompanyAddress"].ToString(),
                        invoiceCompanyTaxCode = reader["InvoiceCompanyTaxCode"].ToString(),
                        customValuesXml = reader["CustomValuesXml"].ToString(),
                        stateID = Convert.ToInt32(reader["StateID"]),
                        userGuid = new Guid(reader["UserGuid"].ToString()),
                        createdFromIP = reader["CreatedFromIP"].ToString(),
                        createdBy = reader["CreatedBy"].ToString(),
                        createdUtc = Convert.ToDateTime(reader["CreatedUtc"]),
                        isDeleted = Convert.ToBoolean(reader["IsDeleted"])
                    };

                    if (reader["StaffId"] != DBNull.Value)
                        order.staffId = Convert.ToInt32(reader["StaffId"]);

                    if (reader["CouponAmount"] != DBNull.Value)
                        order.couponAmount = Convert.ToDecimal(reader["CouponAmount"]);
                    if (reader["RedeemedRewardPoints"] != DBNull.Value)
                        order.redeemedRewardPoints = Convert.ToInt32(reader["RedeemedRewardPoints"]);

                    if (reader["RedeemedRewardPointsAmount"] != DBNull.Value)
                        order.redeemedRewardPointsAmount = Convert.ToDecimal(reader["RedeemedRewardPointsAmount"]);
                    if (reader["OrderServiceFee"] != DBNull.Value)
                        order.orderServiceFee = Convert.ToDecimal(reader["OrderServiceFee"]);

                    if (reader["VoucherCodes"] != DBNull.Value)
                        order.voucherCodes = reader["VoucherCodes"].ToString();
                    if (reader["VoucherAmount"] != DBNull.Value)
                        order.voucherAmount = Convert.ToDecimal(reader["VoucherAmount"]);
                    if (reader["StoreID"] != DBNull.Value)
                        order.storeID = Convert.ToInt32(reader["StoreID"]);
                    if (reader["OldStoreID"] != DBNull.Value)
                        order.oldStoreID = Convert.ToInt32(reader["OldStoreID"]);
                    if (reader["Source"] != DBNull.Value)
                        order.source = reader["Source"].ToString();
                    if (getStoreName && reader["StoreName"] != DBNull.Value)
                        order.storeName = reader["StoreName"].ToString();

                    if (reader["GiftDescription"] != DBNull.Value)
                        order.giftDescription = reader["GiftDescription"].ToString();
                    if (reader["DiscountIDs"] != DBNull.Value)
                        order.discountIDs = reader["DiscountIDs"].ToString();
                    if (reader["ApiOrderCode"] != DBNull.Value)
                        order.apiOrderCode = reader["ApiOrderCode"].ToString();
                    if (reader["ApiOrderStatus"] != DBNull.Value)
                        order.apiOrderStatus = Convert.ToInt32(reader["ApiOrderStatus"].ToString());

                    if (reader["CalcRewardPoints"] != DBNull.Value)
                        order.calcRewardPoints = Convert.ToInt32(reader["CalcRewardPoints"].ToString());
                    if (reader["CompletionedUtc"] != DBNull.Value)
                        order.completionedUtc = Convert.ToDateTime(reader["CompletionedUtc"].ToString());

                    order.isCMSCreate = Convert.ToBoolean(reader["IsCMSCreate"]);
                    if (reader["CancelNote"] != DBNull.Value)
                        order.cancelNote = reader["CancelNote"].ToString();
                    if (reader["CancelUtc"] != DBNull.Value)
                        order.cancelUtc = Convert.ToDateTime(reader["CancelUtc"]);
                    if (reader["CancelUserId"] != DBNull.Value)
                        order.cancelUserId = Convert.ToInt32(reader["CancelUserId"]);
                    if (reader["Affiliate"] != DBNull.Value)
                        order.affiliate = reader["Affiliate"].ToString();

                    if (reader["StaffProcessId"] != DBNull.Value)
                        order.staffProcessId = Convert.ToInt32(reader["StaffProcessId"]);
                    if (reader["TaskmasterId"] != DBNull.Value)
                        order.taskmasterId = Convert.ToInt32(reader["TaskmasterId"]);
                    orderList.Add(order);
                }
            }
            finally
            {
                reader.Close();
            }

            return orderList;
        }

        public static List<Order> GetPage(
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
            IDataReader reader = DBOrder.GetPage(siteID, stateID, orderStatus, paymentMethod, shippingMethod, fromDate, toDate, fromOrderTotal, toOrderTotal, userGuid, storeId, keyword, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static List<Order> GetByCoupon(int siteId, string couponCode)
        {
            var reader = DBOrder.GetByCoupon(siteId, couponCode);
            return LoadListFromReader(reader);
        }

        public static Order GetByCode(string orderCode)
        {
            IDataReader reader = DBOrder.GetByCode(orderCode);
            var lst = LoadListFromReader(reader);
            if (lst.Count > 0)
                return lst[0];

            return null;
        }

        public static Order GetByDate(int siteID, DateTime fromDate, DateTime toDate)
        {
            IDataReader reader = DBOrder.GetByDate(siteID, fromDate, toDate);
            var lst = LoadListFromReader(reader);
            if (lst.Count > 0)
                return lst[0];

            return null;
        }

        public static int GetCountByStores(
           int userID = -1,
           bool isAdmin = false,
           int storeID = -1,
           int defaultStoreID = -1,
           int siteID = -1,
           int stateID = -1,
           int orderStatus = -1,
           int paymentMethod = -1,
           int shippingMethod = -1,
           DateTime? fromDate = null,
           DateTime? toDate = null,
           decimal? fromOrderTotal = null,
           decimal? toOrderTotal = null,
           Guid? userGuid = null,
           string keyword = "",
           int staffProcessId = -1)
        {
            return DBOrder.GetCountByStores(userID, isAdmin, storeID,
                defaultStoreID, siteID, stateID, orderStatus, paymentMethod,
                shippingMethod, fromDate, toDate, fromOrderTotal, toOrderTotal, userGuid, keyword, staffProcessId);
        }

        public static List<Order> GetPageByStores(
           int userID = -1,
           bool isAdmin = false,
           int storeID = -1,
           int defaultStoreID = -1,
           int siteID = -1,
           int stateID = -1,
           int orderStatus = -1,
           int paymentMethod = -1,
           int shippingMethod = -1,
           DateTime? fromDate = null,
           DateTime? toDate = null,
           decimal? fromOrderTotal = null,
           decimal? toOrderTotal = null,
           Guid? userGuid = null,
           string keyword = "",
           int staffProcessId = -1,
            int pageNumber = 1,
            int pageSize = 21474836,
            bool getStoreName = false)
        {
            IDataReader reader = DBOrder.GetPageByStores(userID, isAdmin, storeID,
                defaultStoreID, siteID, stateID, orderStatus, paymentMethod,
                shippingMethod, fromDate, toDate, fromOrderTotal, toOrderTotal, userGuid, keyword, staffProcessId, pageNumber, pageSize);
            return LoadListFromReader(reader, getStoreName);
        }

        #endregion Static Methods

        #region Reports

        public static DataTable GetReportBestsellers(int siteID, int zoneID, int orderStatus, DateTime? fromDate, DateTime? toDate, int totalRows, int orderBy, bool forExport)
        {
            //return DBOrder.GetReportBestsellers(siteID, zoneID, orderStatus, fromDate, toDate, totalRows, orderBy);
            var reader = DBOrder.GetReportBestsellers(siteID, zoneID, orderStatus, fromDate, toDate, totalRows, orderBy);
            var dt = new DataTable();
            dt.Columns.Add("Row", typeof(int));
            dt.Columns.Add("Code", typeof(string));
            dt.Columns.Add("Title", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            if (forExport)
                dt.Columns.Add("OrderTotal", typeof(double));
            else
            {
                dt.Columns.Add("ProductId", typeof(int));
                dt.Columns.Add("OrderTotal", typeof(decimal));
                dt.Columns.Add("OrderTotalFormat", typeof(string));
            }

            try
            {
                var timeOffset = CanhCam.Web.SiteUtils.GetUserTimeOffset();
                int i = 0;
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);

                    i++;
                    row["Row"] = i;
                    row["Code"] = reader["Code"];
                    row["Title"] = reader["Title"];
                    row["Quantity"] = reader["Quantity"];

                    if (forExport)
                    {
                        row["OrderTotal"] = Convert.ToDouble(reader["SubTotal"]);
                    }
                    else
                    {
                        row["ProductId"] = Convert.ToInt32(reader["ProductID"]);
                        row["OrderTotal"] = Convert.ToDecimal(reader["SubTotal"]);
                        row["OrderTotalFormat"] = CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(reader["SubTotal"]));
                    }
                }
            }
            finally
            {
                reader.Close();
            }

            return dt;
        }

        public static DataTable GetReportSales(int siteID, int state, int orderStatus, DateTime? fromDate, DateTime? toDate, int reportType, bool forExport)
        {
            IDataReader reader = null;
            if (reportType == 1)
                reader = DBOrder.GetReportSales(siteID, state, orderStatus, fromDate, toDate);
            else if (reportType == 2)
                reader = DBOrder.GetReportSalesByWeek(siteID, state, orderStatus, fromDate, toDate);
            else if (reportType == 3)
                reader = DBOrder.GetReportSalesByMonth(siteID, state, orderStatus, fromDate, toDate);
            else if (reportType == 4)
                reader = DBOrder.GetReportSalesByYear(siteID, state, orderStatus, fromDate, toDate);

            var dt = new DataTable();
            dt.Columns.Add("Row", typeof(int));
            dt.Columns.Add("Time", typeof(string));
            dt.Columns.Add("OrderCount", typeof(int));
            dt.Columns.Add("OrderTotal", typeof(double));
            dt.Columns.Add("OrderTotalFormat", typeof(string));

            try
            {
                int i = 0;
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);

                    i++;
                    row["Row"] = i;
                    if (reportType == 1)
                        row["Time"] = Convert.ToDateTime(reader["CreatedDate"]).ToShortDateString();
                    else if (reportType == 2)
                        row["Time"] = string.Format("Tuần {0}/{1}", reader["SalesWeek"].ToString(), reader["SalesYear"].ToString());
                    else if (reportType == 3)
                        row["Time"] = string.Format("Tháng {0}/{1}", reader["SalesMonth"].ToString(), reader["SalesYear"].ToString());
                    else if (reportType == 4)
                        row["Time"] = string.Format("Năm {0}", reader["SalesYear"].ToString());
                    row["OrderCount"] = reader["OrderCount"];
                    row["OrderTotal"] = Convert.ToDouble(reader["OrderTotal"]);
                    row["OrderTotalFormat"] = CanhCam.Web.ProductUI.ProductHelper.FormatPrice(Convert.ToDecimal(reader["OrderTotal"]));
                }
            }
            finally
            {
                reader.Close();
            }

            return dt;
        }

        #endregion Reports
    }
}
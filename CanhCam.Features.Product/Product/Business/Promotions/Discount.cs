/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-05
/// Last Modified:			2015-08-05

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum DiscountType
    {
        ByCatalog = 1,
        ByOrderRange = 2,
        ByProductRange = 4,
        Coupon = 8,
        ComboSale = 16,
        PaymentMethod = 32, 
        Deal = 64
    }

    public enum DiscountShareType
    {
        //Share = 1,
        AlwaysUse = 1
    }

    public enum DiscountAppliedType
    {
        ToCategories = 0,
        ToProducts = 1
    }

    public enum DiscountAmountType
    {
        PercentagePerProduct = 1,
        AmountPerProduct = 2
    }

    public class Discount
    {
        #region Constructors

        public Discount()
        { }

        public Discount(int discountId)
        {
            this.GetDiscount(discountId);
        }

        #endregion Constructors

        #region Private Properties

        private int discountID = -1;
        private int siteID = -1;
        private string name = string.Empty;
        private int discountType = -1;
        private bool isActive = false;
        private DateTime? startDate;
        private DateTime? endDate;
        private bool calendarEnabled = false;
        private string calendarDaily = string.Empty;
        private bool usePercentage = false;
        private decimal discountAmount;
        private decimal minPurchase;
        private int discountQtyStep = 0;
        private int specialProductID = -1;
        private Guid guid = Guid.Empty;
        private DateTime createdOn = DateTime.Now;
        private int orderCount = 0;

        private decimal maximumDiscount;
        private int priority = 0;
        private int options = 0;
        private string url = string.Empty;
        private string imageFile = string.Empty;
        private string bannerFile = string.Empty;
        private string briefContent = string.Empty;
        private string fullContent = string.Empty;
        private string contentTokens = string.Empty;

        //private bool alwaysOnDisplay = false;
        private bool appliedAllProducts = false;

        private string appliedForStoreIDs = string.Empty;
        private string appliedForPaymentIDs = string.Empty;
        private string excludedZoneIDs = string.Empty;
        private string excludedProductIDs = string.Empty;
        private int shareType = 0;
        private int pageID = -1;
        private string createdBy = string.Empty;
        private string code = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int DiscountId
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int DiscountType
        {
            get { return discountType; }
            set { discountType = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        public DateTime? StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime? EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public bool CalendarEnabled
        {
            get { return calendarEnabled; }
            set { calendarEnabled = value; }
        }

        public string CalendarDaily
        {
            get { return calendarDaily; }
            set { calendarDaily = value; }
        }

        public bool UsePercentage
        {
            get { return usePercentage; }
            set { usePercentage = value; }
        }

        public decimal DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }

        public decimal MinPurchase
        {
            get { return minPurchase; }
            set { minPurchase = value; }
        }

        public int DiscountQtyStep
        {
            get { return discountQtyStep; }
            set { discountQtyStep = value; }
        }

        public int SpecialProductId
        {
            get { return specialProductID; }
            set { specialProductID = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public int OrderCount
        {
            get { return orderCount; }
            set { orderCount = value; }
        }

        public decimal MaximumDiscount
        {
            get { return maximumDiscount; }
            set { maximumDiscount = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public int Options
        {
            get { return options; }
            set { options = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        public string BannerFile
        {
            get { return bannerFile; }
            set { bannerFile = value; }
        }

        public string BriefContent
        {
            get { return briefContent; }
            set { briefContent = value; }
        }

        public string FullContent
        {
            get { return fullContent; }
            set { fullContent = value; }
        }

        public string ContentTokens
        {
            get { return contentTokens; }
            set { contentTokens = value; }
        }

        //public bool AlwaysOnDisplay
        //{
        //    get { return alwaysOnDisplay; }
        //    set { alwaysOnDisplay = value; }
        //}
        public bool AppliedAllProducts
        {
            get { return appliedAllProducts; }
            set { appliedAllProducts = value; }
        }

        public string AppliedForStoreIDs
        {
            get { return appliedForStoreIDs; }
            set { appliedForStoreIDs = value; }
        }

        public string AppliedForPaymentIDs
        {
            get { return appliedForPaymentIDs; }
            set { appliedForPaymentIDs = value; }
        }

        public string ExcludedZoneIDs
        {
            get { return excludedZoneIDs; }
            set { excludedZoneIDs = value; }
        }

        public string ExcludedProductIDs
        {
            get { return excludedProductIDs; }
            set { excludedProductIDs = value; }
        }

        public int PageId
        {
            get { return pageID; }
            set { pageID = value; }
        }

        public int ShareType { get => shareType; set => shareType = value; }
        public string CreatedBy { get => createdBy; set => createdBy = value; }
        public string Code { get => code; set => code = value; }

        #endregion Public Properties

        #region Private Methods

        private void GetDiscount(int discountId)
        {
            using (IDataReader reader = DBDiscount.GetOne(discountId))
            {
                if (reader.Read())
                {
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.name = reader["Name"].ToString();
                    this.discountType = Convert.ToInt32(reader["DiscountType"]);
                    this.isActive = Convert.ToBoolean(reader["IsActive"]);
                    if (reader["StartDate"] != DBNull.Value)
                        this.startDate = Convert.ToDateTime(reader["StartDate"]);
                    if (reader["EndDate"] != DBNull.Value)
                        this.endDate = Convert.ToDateTime(reader["EndDate"]);
                    this.calendarEnabled = Convert.ToBoolean(reader["CalendarEnabled"]);
                    this.calendarDaily = reader["CalendarDaily"].ToString();
                    this.usePercentage = Convert.ToBoolean(reader["UsePercentage"]);
                    this.discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                    this.minPurchase = Convert.ToDecimal(reader["MinPurchase"]);
                    this.discountQtyStep = Convert.ToInt32(reader["DiscountQtyStep"]);
                    this.specialProductID = Convert.ToInt32(reader["SpecialProductID"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.createdOn = Convert.ToDateTime(reader["CreatedOn"]);

                    this.maximumDiscount = Convert.ToDecimal(reader["MaximumDiscount"]);
                    this.priority = Convert.ToInt32(reader["Priority"]);
                    this.options = Convert.ToInt32(reader["Options"]);
                    this.url = reader["Url"].ToString();
                    this.imageFile = reader["ImageFile"].ToString();
                    this.bannerFile = reader["BannerFile"].ToString();
                    this.briefContent = reader["BriefContent"].ToString();
                    this.fullContent = reader["FullContent"].ToString();
                    this.contentTokens = reader["ContentTokens"].ToString();
                    //this.alwaysOnDisplay = Convert.ToBoolean(reader["AlwaysOnDisplay"]);
                    this.appliedAllProducts = Convert.ToBoolean(reader["AppliedAllProducts"]);
                    this.appliedForStoreIDs = reader["AppliedForStoreIDs"].ToString();
                    this.appliedForPaymentIDs = reader["AppliedForPaymentIDs"].ToString();
                    this.excludedZoneIDs = reader["ExcludedZoneIDs"].ToString();
                    this.excludedProductIDs = reader["ExcludedProductIDs"].ToString();
                    this.shareType = Convert.ToInt32(reader["ShareType"]);
                    this.pageID = Convert.ToInt32(reader["PageID"]);

                    if (reader["CreatedBy"] != DBNull.Value)
                        this.createdBy = reader["CreatedBy"].ToString();
                    if (reader["Code"] != DBNull.Value)
                        this.code = reader["Code"].ToString();
                }
            }
        }

        private bool Create()
        {
            int newID = 0;

            this.guid = Guid.NewGuid();

            newID = DBDiscount.Create(
                this.siteID,
                this.name,
                this.discountType,
                this.isActive,
                this.startDate,
                this.endDate,
                this.calendarEnabled,
                this.calendarDaily,
                this.usePercentage,
                this.discountAmount,
                this.minPurchase,
                this.discountQtyStep,
                this.specialProductID,
                this.guid,
                this.createdOn,
                this.maximumDiscount,
                this.priority,
                this.options,
                this.url,
                this.imageFile,
                this.bannerFile,
                this.briefContent,
                this.fullContent,
                this.contentTokens,
                //this.alwaysOnDisplay,
                this.appliedAllProducts,
                this.appliedForStoreIDs,
                this.appliedForPaymentIDs,
                this.excludedZoneIDs,
                this.excludedProductIDs,
                this.shareType,
                this.pageID,
                this.createdBy,
                this.code);

            this.discountID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBDiscount.Update(
                this.discountID,
                this.siteID,
                this.name,
                this.discountType,
                this.isActive,
                this.startDate,
                this.endDate,
                this.calendarEnabled,
                this.calendarDaily,
                this.usePercentage,
                this.discountAmount,
                this.minPurchase,
                this.discountQtyStep,
                this.specialProductID,
                this.guid,
                this.createdOn,
                this.maximumDiscount,
                this.priority,
                this.options,
                this.url,
                this.imageFile,
                this.bannerFile,
                this.briefContent,
                this.fullContent,
                this.contentTokens,
                //this.alwaysOnDisplay,
                this.appliedAllProducts,
                this.appliedForStoreIDs,
                this.appliedForPaymentIDs,
                this.excludedZoneIDs,
                this.excludedProductIDs,
                this.shareType,
                this.pageID,
                this.createdBy,
                this.code);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.discountID > 0)
                return this.Update();

            return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int discountId)
        {
            return DBDiscount.Delete(discountId);
        }

        public static int GetCount(int siteId, int discountType, int status, int pageID, int productId, int zoneId, int excludedDiscountId)
        {
            return DBDiscount.GetCount(siteId, discountType, status, pageID, productId, zoneId, excludedDiscountId);
        }

        private static List<Discount> LoadListFromReader(IDataReader reader, bool loadOrderCount = true)
        {
            List<Discount> discountList = new List<Discount>();
            try
            {
                while (reader.Read())
                {
                    Discount discount = new Discount
                    {
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        name = reader["Name"].ToString(),
                        discountType = Convert.ToInt32(reader["DiscountType"]),
                        isActive = Convert.ToBoolean(reader["IsActive"])
                    };
                    if (reader["StartDate"] != DBNull.Value)
                        discount.startDate = Convert.ToDateTime(reader["StartDate"]);
                    if (reader["EndDate"] != DBNull.Value)
                        discount.endDate = Convert.ToDateTime(reader["EndDate"]);
                    discount.calendarEnabled = Convert.ToBoolean(reader["CalendarEnabled"]);
                    discount.calendarDaily = reader["CalendarDaily"].ToString();
                    discount.usePercentage = Convert.ToBoolean(reader["UsePercentage"]);
                    discount.discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                    discount.minPurchase = Convert.ToDecimal(reader["MinPurchase"]);
                    discount.discountQtyStep = Convert.ToInt32(reader["DiscountQtyStep"]);
                    discount.specialProductID = Convert.ToInt32(reader["SpecialProductID"]);
                    discount.guid = new Guid(reader["Guid"].ToString());
                    discount.createdOn = Convert.ToDateTime(reader["CreatedOn"]);
                    if (loadOrderCount)
                        discount.orderCount = Convert.ToInt32(reader["OrderCount"]);

                    discount.maximumDiscount = Convert.ToDecimal(reader["MaximumDiscount"]);
                    discount.priority = Convert.ToInt32(reader["Priority"]);
                    discount.options = Convert.ToInt32(reader["Options"]);
                    discount.url = reader["Url"].ToString();
                    discount.imageFile = reader["ImageFile"].ToString();
                    discount.bannerFile = reader["BannerFile"].ToString();
                    discount.briefContent = reader["BriefContent"].ToString();
                    discount.fullContent = reader["FullContent"].ToString();
                    discount.contentTokens = reader["ContentTokens"].ToString();
                    //discount.alwaysOnDisplay = Convert.ToBoolean(reader["AlwaysOnDisplay"]);
                    discount.appliedAllProducts = Convert.ToBoolean(reader["AppliedAllProducts"]);
                    discount.appliedForStoreIDs = reader["AppliedForStoreIDs"].ToString();
                    discount.appliedForPaymentIDs = reader["AppliedForPaymentIDs"].ToString();
                    discount.excludedZoneIDs = reader["ExcludedZoneIDs"].ToString();
                    discount.excludedProductIDs = reader["ExcludedProductIDs"].ToString();
                    discount.shareType = Convert.ToInt32(reader["ShareType"]);
                    discount.pageID = Convert.ToInt32(reader["PageID"]);

                    if (reader["CreatedBy"] != DBNull.Value)
                        discount.createdBy = reader["CreatedBy"].ToString();
                    if (reader["Code"] != DBNull.Value)
                        discount.code = reader["Code"].ToString();

                    discountList.Add(discount);
                }
            }
            finally
            {
                reader.Close();
            }

            return discountList;
        }

        public static Discount GetActive(int siteId, int discountType)
        {
            IDataReader reader = DBDiscount.GetActive(siteId, discountType);
            var lstActiveDeals = LoadListFromReader(reader, false);
            if (lstActiveDeals.Count > 0)
                return lstActiveDeals[0];

            return null;
        }

        public static List<Discount> GetPage(int siteId, int discountType, int status, int pageID, int productId, int zoneId, int excludedDiscountId, int orderBy, int pageNumber, int pageSize)
        {
            var reader = DBDiscount.GetPage(siteId, discountType, status, pageID, productId, zoneId, excludedDiscountId, orderBy, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static List<Discount> GetDiscountCoupons(int siteId, string keyword, string discountIds, int discountType = (int)CanhCam.Business.DiscountType.Coupon)
        {
            var reader = DBDiscount.GetDiscountCoupons(siteId, !string.IsNullOrWhiteSpace(keyword) ? keyword : null, !string.IsNullOrWhiteSpace(discountIds) ? discountIds : null, discountType);
            return LoadListFromReader(reader, false);
        }

        //public static int GetCountProduct(
        //    int siteId,
        //    int discountId,
        //    int specialProductId,
        //    int languageId)
        //{
        //    return DBDiscount.GetCountProduct(siteId, discountId, specialProductId, DateTime.UtcNow, languageId);
        //}

        //public static List<Product> GetPageProduct(
        //    int siteId,
        //    int discountId,
        //    int specialProductId,
        //    int languageId,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    return Product.LoadListFromReader(DBDiscount.GetPageProduct(siteId, discountId, specialProductId, DateTime.UtcNow, languageId, pageNumber, pageSize));
        //}

        #endregion Static Methods
    }
}
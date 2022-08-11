/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-05
/// Last Modified:			2015-08-05

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class DiscountAppliedToItem : ICloneable
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        #region Constructors

        public DiscountAppliedToItem()
        { }

        public DiscountAppliedToItem(Guid guid)
        {
            this.GetDiscountAppliedToItem(guid);
        }

        #endregion Constructors

        #region Private Properties

        private Guid guid = Guid.Empty;
        private int discountID = -1;
        private int itemID = -1;
        private int appliedType = -1;
        private bool usePercentage = false;
        private decimal discountAmount;
        private int giftType = -1;
        private string giftProducts = string.Empty;
        private string giftCustomProducts = string.Empty;
        private string giftDescription = string.Empty;
        private int soldQty = 0;
        private int dealQty = 0;
        private int comboSaleQty = 0;
        private string comboSaleRules = string.Empty;
        private DateTime? fromDate;
        private DateTime? toDate;
        private string giftHtml = string.Empty;
        private decimal maximumDiscount = decimal.Zero;
        private DateTime createdDate = DateTime.Now;
        private int displayOrder = 0;

        private decimal originalPrice = decimal.Zero;
        private decimal discountCaculator = decimal.Zero;
        private int discountPercentage = 0;
        private int discountType = -1;
        private int discountQtyStep = 0;

        private int discountOptions = 0;

        //private int discountPriority = 0;
        private int discountShareType = 0;

        private DateTime? discountStartDate;
        private DateTime? discountEndDate;
        private decimal discountPrice = 0;
        private int discountPriority = 0;
        private string appliedForPaymentIDs = string.Empty;
        private decimal discountAmountParent = 0;
        private bool usePercentageParent = false;
        private decimal maximumDiscountParent = 0;
        private string excludedZoneIDs = string.Empty;

        private int specialDiscountType = 0;
        private string productGifts = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int DiscountId
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public int ItemId
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int AppliedType
        {
            get { return appliedType; }
            set { appliedType = value; }
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

        public int GiftType
        {
            get { return giftType; }
            set { giftType = value; }
        }

        public string GiftProducts
        {
            get { return giftProducts; }
            set { giftProducts = value; }
        }

        public string GiftCustomProducts
        {
            get { return giftCustomProducts; }
            set { giftCustomProducts = value; }
        }

        public string GiftDescription
        {
            get { return giftDescription; }
            set { giftDescription = value; }
        }

        public int SoldQty
        {
            get { return soldQty; }
            set { soldQty = value; }
        }

        public int DealQty
        {
            get { return dealQty; }
            set { dealQty = value; }
        }

        public int ComboSaleQty
        {
            get { return comboSaleQty; }
            set { comboSaleQty = value; }
        }

        public string ComboSaleRules
        {
            get { return comboSaleRules; }
            set { comboSaleRules = value; }
        }

        public decimal OriginalPrice
        {
            get { return originalPrice; }
            set { originalPrice = value; }
        }

        public decimal DiscountCaculator
        {
            get { return discountCaculator; }
            set { discountCaculator = value; }
        }

        public int DiscountPercentage
        {
            get { return discountPercentage; }
            set { discountPercentage = value; }
        }

        public int DiscountType
        {
            get { return discountType; }
            set { discountType = value; }
        }

        public int DiscountQtyStep
        {
            get { return discountQtyStep; }
            set { discountQtyStep = value; }
        }

        public int DiscountOptions { get => discountOptions; set => discountOptions = value; }
        public decimal DiscountPrice { get => discountPrice; set => discountPrice = value; }
        public DateTime? FromDate { get => fromDate; set => fromDate = value; }
        public DateTime? ToDate { get => toDate; set => toDate = value; }
        public int DiscountShareType { get => discountShareType; set => discountShareType = value; }
        public DateTime? DiscountStartDate { get => discountStartDate; set => discountStartDate = value; }
        public DateTime? DiscountEndDate { get => discountEndDate; set => discountEndDate = value; }
        public int DiscountPriority { get => discountPriority; set => discountPriority = value; }
        public string GiftHtml { get => giftHtml; set => giftHtml = value; }
        public decimal MaximumDiscount { get => maximumDiscount; set => maximumDiscount = value; }
        public string AppliedForPaymentIDs { get => appliedForPaymentIDs; set => appliedForPaymentIDs = value; }
        public decimal DiscountAmountParent { get => discountAmountParent; set => discountAmountParent = value; }
        public bool UsePercentageParent { get => usePercentageParent; set => usePercentageParent = value; }
        public decimal MaximumDiscountParent { get => maximumDiscountParent; set => maximumDiscountParent = value; }
        public string ExcludedZoneIDs { get => excludedZoneIDs; set => excludedZoneIDs = value; }
        public int SpecialDiscountType { get => specialDiscountType; set => specialDiscountType = value; }
        public DateTime CreatedDate { get => createdDate; set => createdDate = value; }
        public int DisplayOrder { get => displayOrder; set => displayOrder = value; }
        public string ProductGifts { get => productGifts; set => productGifts = value; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of DiscountAppliedToItem.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetDiscountAppliedToItem(Guid guid)
        {
            using (IDataReader reader = DBDiscountAppliedToItem.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.itemID = Convert.ToInt32(reader["ItemID"]);
                    this.appliedType = Convert.ToInt32(reader["AppliedType"]);
                    this.usePercentage = Convert.ToBoolean(reader["UsePercentage"]);
                    this.discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                    this.giftType = Convert.ToInt32(reader["GiftType"]);
                    this.giftProducts = reader["GiftProducts"].ToString();
                    this.giftCustomProducts = reader["GiftCustomProducts"].ToString();
                    this.giftDescription = reader["GiftDescription"].ToString();
                    if (reader["SoldQty"] != DBNull.Value)
                        this.soldQty = Convert.ToInt32(reader["SoldQty"]);
                    this.dealQty = Convert.ToInt32(reader["DealQty"]);
                    this.comboSaleQty = Convert.ToInt32(reader["ComboSaleQty"]);
                    this.comboSaleRules = reader["ComboSaleRules"].ToString();
                    if (reader["FromDate"] != DBNull.Value)
                        this.fromDate = Convert.ToDateTime(reader["FromDate"]);
                    if (reader["ToDate"] != DBNull.Value)
                        this.toDate = Convert.ToDateTime(reader["ToDate"]);

                    if (reader["GiftHtml"] != DBNull.Value)
                        this.giftHtml = reader["GiftHtml"].ToString();
                    if (reader["MaximumDiscount"] != DBNull.Value)
                        this.maximumDiscount = Convert.ToDecimal(reader["MaximumDiscount"]);

                    if (reader["CreatedDate"] != DBNull.Value)
                        this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    if (reader["DisplayOrder"] != DBNull.Value)
                        this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    if (reader["ProductGifts"] != DBNull.Value)
                        this.productGifts = reader["ProductGifts"].ToString();
                }
            }
        }

        ///// <summary>
        ///// Persists a new instance of DiscountAppliedToItem. Returns true on success.
        ///// </summary>
        ///// <returns></returns>
        //private bool Create()
        //{
        //    this.guid = Guid.NewGuid();

        //    int rowsAffected = DBDiscountAppliedToItem.Create(
        //        this.guid,
        //        this.discountID,
        //        this.itemID,
        //        this.appliedType,
        //        this.usePercentage,
        //        this.discountAmount,
        //        this.giftType,
        //        this.giftProducts,
        //        this.giftCustomProducts,
        //        this.giftDescription,
        //        this.soldQty,
        //        this.dealQty,
        //        this.comboSaleQty,
        //        this.comboSaleRules,
        //        this.fromDate,
        //        this.ToDate,
        //        this.GiftHtml,
        //        this.MaximumDiscount);

        //    return (rowsAffected > 0);
        //}

        ///// <summary>
        ///// Updates this instance of DiscountAppliedToItem. Returns true on success.
        ///// </summary>
        ///// <returns>bool</returns>
        //private bool Update()
        //{
        //    return DBDiscountAppliedToItem.Update(
        //        this.guid,
        //        this.discountID,
        //        this.itemID,
        //        this.appliedType,
        //        this.usePercentage,
        //        this.discountAmount,
        //        this.giftType,
        //        this.giftProducts,
        //        this.giftCustomProducts,
        //        this.giftDescription,
        //        this.soldQty,
        //        this.dealQty,
        //        this.comboSaleQty,
        //        this.comboSaleRules,
        //        this.fromDate,
        //        this.ToDate,
        //        this.GiftHtml,
        //        this.MaximumDiscount);
        //}

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of DiscountAppliedToItem. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            //if (this.guid != Guid.Empty)
            //    return Update();

            //return Create();

            if (this.guid == Guid.Empty)
                this.guid = Guid.NewGuid();

            return DBDiscountAppliedToItem.Save(
                        this.guid,
                        this.discountID,
                        this.itemID,
                        this.appliedType,
                        this.usePercentage,
                        this.discountAmount,
                        this.giftType,
                        this.giftProducts,
                        this.giftCustomProducts,
                        this.giftDescription,
                        this.soldQty,
                        this.dealQty,
                        this.comboSaleQty,
                        this.comboSaleRules,
                        this.fromDate,
                        this.toDate,
                        this.giftHtml,
                        this.maximumDiscount,
                        this.createdDate,
                        this.displayOrder,
                        this.productGifts);
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of DiscountAppliedToItem. Returns true on success.
        /// </summary>
        /// <param name="guid"> guid </param>
        /// <returns>bool</returns>
        public static bool Delete(Guid guid)
        {
            return DBDiscountAppliedToItem.Delete(guid);
        }

        private static List<DiscountAppliedToItem> LoadListFromReader(IDataReader reader, bool loadCreatedDate = false)
        {
            List<DiscountAppliedToItem> discountAppliedToItemList = new List<DiscountAppliedToItem>();
            try
            {
                while (reader.Read())
                {
                    DiscountAppliedToItem discountAppliedToItem = new DiscountAppliedToItem
                    {
                        guid = new Guid(reader["Guid"].ToString()),
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        itemID = Convert.ToInt32(reader["ItemID"]),
                        appliedType = Convert.ToInt32(reader["AppliedType"]),
                        usePercentage = Convert.ToBoolean(reader["UsePercentage"])
                    };
                    if (reader["DiscountAmount"] != DBNull.Value)
                        discountAppliedToItem.discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                    discountAppliedToItem.giftType = Convert.ToInt32(reader["GiftType"]);
                    discountAppliedToItem.giftProducts = reader["GiftProducts"].ToString();
                    discountAppliedToItem.giftCustomProducts = reader["GiftCustomProducts"].ToString();
                    discountAppliedToItem.giftDescription = reader["GiftDescription"].ToString();
                    if (reader["SoldQty"] != DBNull.Value)
                        discountAppliedToItem.soldQty = Convert.ToInt32(reader["SoldQty"]);
                    discountAppliedToItem.dealQty = Convert.ToInt32(reader["DealQty"]);
                    discountAppliedToItem.comboSaleQty = Convert.ToInt32(reader["ComboSaleQty"]);
                    discountAppliedToItem.comboSaleRules = reader["ComboSaleRules"].ToString();
                    if (reader["FromDate"] != DBNull.Value)
                        discountAppliedToItem.fromDate = Convert.ToDateTime(reader["FromDate"]);
                    if (reader["ToDate"] != DBNull.Value)
                        discountAppliedToItem.toDate = Convert.ToDateTime(reader["ToDate"]);
                    discountAppliedToItem.discountType = Convert.ToInt32(reader["DiscountType"]);
                    discountAppliedToItem.discountQtyStep = Convert.ToInt32(reader["DiscountQtyStep"]);
                    discountAppliedToItem.discountOptions = Convert.ToInt32(reader["DiscountOptions"]);
                    if (reader["DiscountPriority"] != DBNull.Value)
                        discountAppliedToItem.discountPriority = Convert.ToInt32(reader["DiscountPriority"]);
                    discountAppliedToItem.discountShareType = Convert.ToInt32(reader["DiscountShareType"]);
                    if (reader["DiscountStartDate"] != DBNull.Value)
                        discountAppliedToItem.DiscountStartDate = Convert.ToDateTime(reader["DiscountStartDate"]);
                    if (reader["DiscountEndDate"] != DBNull.Value)
                        discountAppliedToItem.discountEndDate = Convert.ToDateTime(reader["DiscountEndDate"]);
                    if (reader["AppliedForPaymentIDs"] != DBNull.Value)
                        discountAppliedToItem.appliedForPaymentIDs = reader["AppliedForPaymentIDs"].ToString();
                    if (reader["DiscountAmountParent"] != DBNull.Value)
                        discountAppliedToItem.discountAmountParent = Convert.ToDecimal(reader["DiscountAmountParent"]);
                    if (reader["UsePercentageParent"] != DBNull.Value)
                        discountAppliedToItem.usePercentageParent = Convert.ToBoolean(reader["UsePercentageParent"]);
                    if (reader["MaximumDiscountParent"] != DBNull.Value)
                        discountAppliedToItem.maximumDiscountParent = Convert.ToDecimal(reader["MaximumDiscountParent"]);

                    if (reader["GiftHtml"] != DBNull.Value)
                        discountAppliedToItem.giftHtml = reader["GiftHtml"].ToString();
                    if (reader["MaximumDiscount"] != DBNull.Value)
                        discountAppliedToItem.maximumDiscount = Convert.ToDecimal(reader["MaximumDiscount"]);

                    if (reader["ExcludedZoneIDs"] != DBNull.Value)
                        discountAppliedToItem.excludedZoneIDs = reader["ExcludedZoneIDs"].ToString();
                    if (reader["ProductGifts"] != DBNull.Value)
                        discountAppliedToItem.productGifts = reader["ProductGifts"].ToString();
                    if (loadCreatedDate)
                    {
                        if (reader["CreatedDate"] != DBNull.Value)
                            discountAppliedToItem.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                        if (reader["DisplayOrder"] != DBNull.Value)
                            discountAppliedToItem.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    }

                    discountAppliedToItemList.Add(discountAppliedToItem);
                }
            }
            finally
            {
                reader.Close();
            }

            return discountAppliedToItemList;
        }

        public static List<DiscountAppliedToItem> GetByDiscount(int discountId, int appliedType)
        {
            IDataReader reader = DBDiscountAppliedToItem.GetByDiscount(discountId, appliedType);
            return LoadListFromReader(reader, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="siteId"></param>
        /// <param name="discountType">-10: All, -1: Without ComboSale</param>
        /// <param name="lstProducts"></param>
        /// <returns></returns>
        public static List<DiscountAppliedToItem> GetActive(int siteId, int discountType, List<Product> lstProducts, int options = -1)
        {
            var zoneIds = new List<int>();
            var productIds = new List<int>();
            foreach (var product in lstProducts)
            {
                if (!zoneIds.Contains(product.ZoneId))
                    zoneIds.Add(product.ZoneId);
                if (!productIds.Contains(product.ProductId))
                    productIds.Add(product.ProductId);
            }

            List<DiscountAppliedToItem> lstItems = new List<DiscountAppliedToItem>();
            if (zoneIds.Count > 0 || productIds.Count > 0)
                lstItems = GetActive(siteId, discountType, zoneIds, productIds, options);

            return lstItems;
        }

        public static List<DiscountAppliedToItem> GetActive(int siteId, int discountType, List<int> zoneIds, List<int> productIds, int options = -1)
        {
            IDataReader reader = DBDiscountAppliedToItem.GetActive(siteId, discountType, string.Join(";", zoneIds), string.Join(";", productIds), options);
            return LoadListFromReader(reader);
        }

        //public static DiscountAppliedToItem GetOneComboSale(int siteId, int productId)
        //{
        //    IDataReader reader = DBDiscountAppliedToItem.GetActiveComboSale(siteId, productId);
        //    var lstItems = LoadListFromReader(reader);

        //    if (lstItems.Count > 0)
        //        return lstItems[0];

        //    return null;
        //}

        public static bool DeleteByDiscount(int discountId, int type = -1)
        {
            return DBDiscountAppliedToItem.DeleteByDiscount(discountId, type);
        }

        public static bool DeleteByItem(int itemId, int discountId, int type = -1)
        {
            return DBDiscountAppliedToItem.DeleteByItem(itemId, discountId, type);
        }

        public static DiscountAppliedToItem FindFromList(List<DiscountAppliedToItem> lstItems, int itemId, int appliedType)
        {
            foreach (DiscountAppliedToItem item in lstItems)
            {
                if (item.itemID == itemId && appliedType == item.appliedType)
                    return item;
            }

            return null;
        }

        public static string GetItemIdsFromList(List<DiscountAppliedToItem> lstItems, int appliedType)
        {
            string result = string.Empty;
            string sepa = string.Empty;
            foreach (DiscountAppliedToItem item in lstItems)
            {
                if (item.appliedType == appliedType)
                {
                    result += sepa + item.itemID;
                    sepa = ";";
                }
            }

            return result;
        }

        public static bool UpdateSoldQty(Guid guid)
        {
            return DBDiscountAppliedToItem.UpdateSoldQty(guid);
        }

        #endregion Static Methods

    }
}
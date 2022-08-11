// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
// Created:					2014-06-30
// Last Modified:			2014-06-30

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    /// <summary>
    /// Represents a shoping cart type (need to be synchronize with [gb_ShoppingCartType] table
    /// </summary>
    public enum ShoppingCartTypeEnum : int
    {
        /// <summary>
        /// Shopping cart
        /// </summary>
        ShoppingCart = 1,

        /// <summary>
        /// Wishlist
        /// </summary>
        Wishlist = 2,
    }

    public class ShoppingCartItem
    {
        #region Constructors

        public ShoppingCartItem()
        { }

        public ShoppingCartItem(Guid guid)
        {
            this.GetShoppingCartItem(
                guid);
        }

        #endregion Constructors

        #region Private Properties

        private Guid guid = Guid.Empty;
        private int siteID = -1;
        private int productID = -1;
        private Guid userGuid = Guid.Empty;
        private string attributesXml = string.Empty;
        private int quantity = -1;
        private int shoppingCartType = -1;
        private string createdFromIP = string.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        private int affiliateUserID = -1;
        private string createdByName = string.Empty;
        private int userID = -1;
        private string productUrl = string.Empty;
        private int zoneID = -1;
        private string productTitle = string.Empty;

        private int productVariantID = -1;
        private bool appliedCoupon = false;
        private string giftMessage = string.Empty;
        private decimal itemDiscount = decimal.Zero;
        private int? discountQuantity;
        private Guid? discountAppliedGuid;
        private string variantCode = string.Empty;
        private decimal variantPrice = decimal.Zero;

        private decimal itemDiscountCoupon = decimal.Zero;
        private string productGifts = string.Empty;
        private int comboDiscountPriority = -1;

        #endregion Private Properties

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public int ProductId
        {
            get { return productID; }
            set { productID = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public string AttributesXml
        {
            get { return attributesXml; }
            set { attributesXml = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public int ShoppingCartType
        {
            get { return shoppingCartType; }
            set { shoppingCartType = value; }
        }

        public string CreatedFromIP
        {
            get { return createdFromIP; }
            set { createdFromIP = value; }
        }

        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }

        public DateTime LastModUtc
        {
            get { return lastModUtc; }
            set { lastModUtc = value; }
        }

        public string CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }

        public int UserId
        {
            get { return userID; }
            set { userID = value; }
        }

        public string ProductUrl
        {
            get { return productUrl; }
            set { productUrl = value; }
        }

        public int ZoneId
        {
            get { return zoneID; }
            set { zoneID = value; }
        }

        public int AffiliateUserID
        {
            get { return affiliateUserID; }
            set { affiliateUserID = value; }
        }

        public string ProductTitle
        {
            get { return productTitle; }
            set { productTitle = value; }
        }

        public Product Product
        {
            get
            {
                if (productID > 0 && siteID > 0)
                {
                    Product product = new Product(siteID, productID);
                    if (product != null && product.ProductId > 0)
                        return product;
                }

                return null;
            }
        }

        public int ProductVariantId
        {
            get { return productVariantID; }
            set { productVariantID = value; }
        }

        public bool AppliedCoupon
        {
            get { return appliedCoupon; }
            set { appliedCoupon = value; }
        }

        public string GiftMessage
        {
            get { return giftMessage; }
            set { giftMessage = value; }
        }

        public decimal ItemDiscount
        {
            get { return itemDiscount; }
            set { itemDiscount = value; }
        }

        public int? DiscountQuantity
        {
            get { return discountQuantity; }
            set { discountQuantity = value; }
        }

        public Guid? DiscountAppliedGuid
        {
            get { return discountAppliedGuid; }
            set { discountAppliedGuid = value; }
        }

        //public string VariantCode
        //{
        //    get { return variantCode; }
        //    set { variantCode = value; }
        //}
        public decimal VariantPrice
        {
            get { return variantPrice; }
            set { variantPrice = value; }
        }

        public decimal ItemDiscountCoupon { get => itemDiscountCoupon; set => itemDiscountCoupon = value; }
        public string ProductGifts { get => productGifts; set => productGifts = value; }
        public int ComboDiscountPriority { get => comboDiscountPriority; set => comboDiscountPriority = value; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of ShoppingCartItem.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetShoppingCartItem(Guid guid)
        {
            using (IDataReader reader = DBShoppingCartItem.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    this.attributesXml = reader["AttributesXml"].ToString();
                    this.quantity = Convert.ToInt32(reader["Quantity"]);
                    this.shoppingCartType = Convert.ToInt32(reader["ShoppingCartType"]);
                    this.createdFromIP = reader["CreatedFromIP"].ToString();
                    this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    this.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    this.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of ShoppingCartItem. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBShoppingCartItem.Create(
                this.guid,
                this.siteID,
                this.productID,
                this.userGuid,
                this.attributesXml,
                this.quantity,
                this.shoppingCartType,
                this.createdFromIP,
                this.createdUtc,
                this.lastModUtc, 
                this.affiliateUserID);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Updates this instance of ShoppingCartItem. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBShoppingCartItem.Update(
                this.guid,
                this.siteID,
                this.productID,
                this.userGuid,
                this.attributesXml,
                this.quantity,
                this.shoppingCartType,
                this.createdFromIP,
                this.createdUtc,
                this.lastModUtc,
                this.affiliateUserID);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of ShoppingCartItem. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.guid != Guid.Empty)
                return Update();

            return Create();
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(Guid guid)
        {
            return DBShoppingCartItem.Delete(guid);
        }

        public static bool DeleteByProduct(int productId)
        {
            return DBShoppingCartItem.DeleteByProduct(productId);
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBShoppingCartItem.DeleteBySite(siteId);
        }

        public static bool DeleteOlderThan(int siteId, int shoppingCartType, DateTime olderThanUtc)
        {
            return DBShoppingCartItem.DeleteOlderThan(siteId, shoppingCartType, olderThanUtc);
        }

        public static bool MoveToUser(Guid userGuid, Guid newUserGuid)
        {
            return DBShoppingCartItem.MoveToUser(userGuid, newUserGuid);
        }

        public static int GetCount(int siteId, int shoppingCartType)
        {
            return DBShoppingCartItem.GetCount(siteId, shoppingCartType);
        }

        private static List<ShoppingCartItem> LoadListFromReader(IDataReader reader, bool loadOtherInfo = false)
        {
            List<ShoppingCartItem> shoppingCartItemList = new List<ShoppingCartItem>();
            try
            {
                while (reader.Read())
                {
                    ShoppingCartItem shoppingCartItem = new ShoppingCartItem
                    {
                        guid = new Guid(reader["Guid"].ToString()),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        userGuid = new Guid(reader["UserGuid"].ToString()),
                        attributesXml = reader["AttributesXml"].ToString(),
                        quantity = Convert.ToInt32(reader["Quantity"]),
                        shoppingCartType = Convert.ToInt32(reader["ShoppingCartType"]),
                        createdFromIP = reader["CreatedFromIP"].ToString(),
                        createdUtc = Convert.ToDateTime(reader["CreatedUtc"]),
                        lastModUtc = Convert.ToDateTime(reader["LastModUtc"]),
                        affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]),
                    };

                    if (loadOtherInfo)
                    {
                        if (reader["UserID"] != null)
                            shoppingCartItem.userID = Convert.ToInt32(reader["UserID"]);
                        if (reader["CreatedByName"] != null)
                            shoppingCartItem.createdByName = reader["CreatedByName"].ToString();
                        if (reader["ZoneID"] != null)
                            shoppingCartItem.zoneID = Convert.ToInt32(reader["ZoneID"]);
                        if (reader["ProductUrl"] != null)
                            shoppingCartItem.productUrl = reader["ProductUrl"].ToString();
                        if (reader["ProductTitle"] != null)
                            shoppingCartItem.productTitle = reader["ProductTitle"].ToString();
                    }

                    shoppingCartItemList.Add(shoppingCartItem);
                }
            }
            finally
            {
                reader.Close();
            }

            return shoppingCartItemList;
        }

        public static List<ShoppingCartItem> GetByUserGuid(int siteId, ShoppingCartTypeEnum cartType, Guid userGuid)
        {
            IDataReader reader = DBShoppingCartItem.GetByUserGuid(siteId, (int)cartType, userGuid);
            return LoadListFromReader(reader);
        }

        public static List<ShoppingCartItem> GetPage(int siteId, int shoppingCartType, int pageNumber, int pageSize)
        {
            IDataReader reader = DBShoppingCartItem.GetPage(siteId, shoppingCartType, pageNumber, pageSize);
            return LoadListFromReader(reader, true);
        }

        #endregion Static Methods
    }
}
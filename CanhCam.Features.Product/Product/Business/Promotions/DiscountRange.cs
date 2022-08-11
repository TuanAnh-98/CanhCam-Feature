/// Author:					Tran Quoc Vuong
/// Created:				2020-09-21
/// Last Modified:			2020-09-21

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    [Serializable]
    public class DiscountRange
    {
        #region Constructors

        public DiscountRange()
        { }

        public DiscountRange(int itemID)
        {
            this.GetDiscountRange(itemID);
        }

        #endregion Constructors

        #region Private Properties

        private int itemID = -1;
        private int discountID = -1;
        private int zoneID = 0;
        private int productID = 0;
        private decimal fromPrice;
        private decimal toPrice;
        private int discountType = -1;
        private decimal discountAmount;
        private string giftHtml = string.Empty;
        private decimal maximumDiscount = decimal.Zero;

        //private int discountPriority = 0;
        private int discountShareType = 0;
        private string productGifts = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int ItemID
        {
            get { return itemID; }
            set { itemID = value; }
        }

        public int DiscountID
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public int ZoneID
        {
            get { return zoneID; }
            set { zoneID = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public decimal FromPrice
        {
            get { return fromPrice; }
            set { fromPrice = value; }
        }

        public decimal ToPrice
        {
            get { return toPrice; }
            set { toPrice = value; }
        }

        public int DiscountType
        {
            get { return discountType; }
            set { discountType = value; }
        }

        public decimal DiscountAmount
        {
            get { return discountAmount; }
            set { discountAmount = value; }
        }

        //public int DiscountPriority { get => discountPriority; set => discountPriority = value; }
        public string GiftHtml { get => giftHtml; set => giftHtml = value; }

        public decimal MaximumDiscount { get => maximumDiscount; set => maximumDiscount = value; }
        public string ProductGifts { get => productGifts; set => productGifts = value; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of DiscountRange.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        private void GetDiscountRange(
            int itemID)
        {
            using (IDataReader reader = DBDiscountRange.GetOne(itemID))
            {
                if (reader.Read())
                {
                    this.itemID = Convert.ToInt32(reader["ItemID"]);
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.zoneID = Convert.ToInt32(reader["ZoneID"]);
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.fromPrice = Convert.ToDecimal(reader["FromPrice"]);
                    this.toPrice = Convert.ToDecimal(reader["ToPrice"]);
                    this.discountType = Convert.ToInt32(reader["DiscountType"]);
                    this.discountAmount = Convert.ToDecimal(reader["DiscountAmount"]);
                    this.giftHtml = reader["GiftHtml"].ToString();
                    this.maximumDiscount = Convert.ToDecimal(reader["MaximumDiscount"]);
                    if (reader["ProductGifts"] != DBNull.Value)
                        this.productGifts = reader["ProductGifts"].ToString();
                }
            }
        }

        /// <summary>
        /// Persists a new instance of DiscountRange. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBDiscountRange.Create(
                this.discountID,
                this.zoneID,
                this.productID,
                this.fromPrice,
                this.toPrice,
                this.discountType,
                this.discountAmount,
                this.giftHtml,
                this.maximumDiscount,
                this.productGifts);

            this.itemID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of DiscountRange. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBDiscountRange.Update(
                this.itemID,
                this.discountID,
                this.zoneID,
                this.productID,
                this.fromPrice,
                this.toPrice,
                this.discountType,
                this.discountAmount,
                this.giftHtml,
                this.maximumDiscount,
                this.productGifts);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of DiscountRange. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.itemID > 0)
                return this.Update();

            return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of DiscountRange. Returns true on success.
        /// </summary>
        /// <param name="itemID"> itemID </param>
        /// <returns>bool</returns>
        public static bool Delete(int itemID)
        {
            return DBDiscountRange.Delete(itemID);
        }

        private static List<DiscountRange> LoadListFromReader(IDataReader reader, bool loadDiscountInfo = false)
        {
            List<DiscountRange> discountRangeList = new List<DiscountRange>();
            try
            {
                while (reader.Read())
                {
                    var discountRange = new DiscountRange
                    {
                        itemID = Convert.ToInt32(reader["ItemID"]),
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        zoneID = Convert.ToInt32(reader["ZoneID"]),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        fromPrice = Convert.ToDecimal(reader["FromPrice"]),
                        toPrice = Convert.ToDecimal(reader["ToPrice"]),
                        discountType = Convert.ToInt32(reader["DiscountType"]),
                        discountAmount = Convert.ToDecimal(reader["DiscountAmount"])
                    };

                    if (reader["GiftHtml"] != DBNull.Value)
                        discountRange.giftHtml = reader["GiftHtml"].ToString();
                    if (reader["MaximumDiscount"] != DBNull.Value)
                        discountRange.maximumDiscount = Convert.ToDecimal(reader["MaximumDiscount"]);
                    if (reader["ProductGifts"] != DBNull.Value)
                        discountRange.productGifts = reader["ProductGifts"].ToString();
                    if (loadDiscountInfo)
                    {
                        if (reader["DiscountShareType"] != DBNull.Value)
                            discountRange.discountShareType = Convert.ToInt32(reader["DiscountShareType"]);
                    }

                    discountRangeList.Add(discountRange);
                }
            }
            finally
            {
                reader.Close();
            }

            return discountRangeList;
        }

        ///// <summary>
        ///// Gets an IList with all instances of DiscountRange.
        ///// </summary>
        //public static List<DiscountRange> GetAll()
        //{
        //    IDataReader reader = DBDiscountRange.GetAll();
        //    return LoadListFromReader(reader);

        //}

        ///// <summary>
        ///// Gets an IList with page of instances of DiscountRange.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static List<DiscountRange> GetPage(int pageNumber, int pageSize, out int totalPages)
        //{
        //    totalPages = 1;
        //    IDataReader reader = DBDiscountRange.GetPage(pageNumber, pageSize, out totalPages);
        //    return LoadListFromReader(reader);
        //}

        //public static List<DiscountRange> GetByDiscountID(int discountID)
        //{
        //    var reader = DBDiscountRange.GetByDiscountID(discountID);
        //    return LoadListFromReader(reader);
        //}

        //public static List<DiscountRange> GetByProductID(int productID, int discountID = 0)
        //{
        //    var reader = DBDiscountRange.GetByProductID(productID, discountID);
        //    return LoadListFromReader(reader);
        //}

        public static List<DiscountRange> GetRange(int discountID, int productID = 0)
        {
            var reader = DBDiscountRange.GetRange(discountID, productID);
            return LoadListFromReader(reader);
        }

        public static DiscountRange GetActive(int siteID, int productID, decimal value, int shareType)
        {
            var reader = DBDiscountRange.GetActive(siteID, productID, value, shareType);
            var lst = LoadListFromReader(reader, true);
            if (lst.Count > 0)
                return lst[0];

            return null;
        }

        public static bool DeleteByDiscount(int promotionId)
        {
            return DBDiscountRange.DeleteByDiscount(promotionId);
        }

        #endregion Static Methods
    }
}
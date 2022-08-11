/// Author:					Tran Quoc Vuong
/// Created:				2020-06-04
/// Last Modified:			2020-06-04

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    [Serializable]
    public class DiscountGift
    {
        #region Constructors

        public DiscountGift()
        { }

        public DiscountGift(
            int giftID)
        {
            this.GetPromotionGift(
                giftID);
        }

        #endregion Constructors

        #region Private Properties

        private int giftID = -1;
        private int discountID = -1;
        private Guid appliedItemGuid = Guid.Empty;
        private int discountRangeID = -1;
        private string name = string.Empty;
        private int productID = -1;
        private string imageFile = string.Empty;
        private int displayOrder = -1;
        private int quantity = -1;
        private int giftCount = -1;
        private int giftTotal = -1;
        private decimal giftPrice;
        private string giftNote = string.Empty;
        private string url = string.Empty;
        private Guid guid = Guid.Empty;

        #endregion Private Properties

        #region Public Properties

        public int GiftID
        {
            get { return giftID; }
            set { giftID = value; }
        }

        public int DiscountID
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public int GiftCount
        {
            get { return giftCount; }
            set { giftCount = value; }
        }

        public int GiftTotal
        {
            get { return giftTotal; }
            set { giftTotal = value; }
        }

        public decimal GiftPrice
        {
            get { return giftPrice; }
            set { giftPrice = value; }
        }

        public string GiftNote
        {
            get { return giftNote; }
            set { giftNote = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public Guid AppliedItemGuid { get => appliedItemGuid; set => appliedItemGuid = value; }
        public int DiscountRangeID { get => discountRangeID; set => discountRangeID = value; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of PromotionGift.
        /// </summary>
        /// <param name="giftID"> giftID </param>
        private void GetPromotionGift(
            int giftID)
        {
            using (IDataReader reader = DBDiscountGift.GetOne(giftID))
            {
                if (reader.Read())
                {
                    this.giftID = Convert.ToInt32(reader["GiftID"]);
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.appliedItemGuid = new Guid(reader["AppliedItemGuid"].ToString());
                    this.discountRangeID = Convert.ToInt32(reader["DiscountRangeID"]);
                    this.name = reader["Name"].ToString();
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.imageFile = reader["ImageFile"].ToString();
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.quantity = Convert.ToInt32(reader["Quantity"]);
                    this.giftCount = Convert.ToInt32(reader["GiftCount"]);
                    this.giftTotal = Convert.ToInt32(reader["GiftTotal"]);
                    this.giftPrice = Convert.ToDecimal(reader["GiftPrice"]);
                    this.giftNote = reader["GiftNote"].ToString();
                    this.url = reader["Url"].ToString();
                    this.guid = new Guid(reader["Guid"].ToString());
                }
            }
        }

        /// <summary>
        /// Persists a new instance of PromotionGift. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;
            this.Guid = Guid.NewGuid();

            newID = DBDiscountGift.Create(
                this.discountID,
                this.appliedItemGuid,
                this.discountRangeID,
                this.name,
                this.productID,
                this.imageFile,
                this.displayOrder,
                this.quantity,
                this.giftCount,
                this.giftTotal,
                this.giftPrice,
                this.giftNote,
                this.url,
                this.guid);

            this.giftID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of PromotionGift. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBDiscountGift.Update(
                this.giftID,
                this.discountID,
                this.appliedItemGuid,
                this.discountRangeID,
                this.name,
                this.productID,
                this.imageFile,
                this.displayOrder,
                this.quantity,
                this.giftCount,
                this.giftTotal,
                this.giftPrice,
                this.giftNote,
                this.url,
                this.guid);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of PromotionGift. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.giftID > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Create();
            }
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of PromotionGift. Returns true on success.
        /// </summary>
        /// <param name="giftID"> giftID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int giftID)
        {
            return DBDiscountGift.Delete(
                giftID);
        }

        ///// <summary>
        ///// Gets a count of PromotionGift.
        ///// </summary>
        //public static int GetCount()
        //{
        //    return DBDiscountGift.GetCount();
        //}

        private static List<DiscountGift> LoadListFromReader(IDataReader reader)
        {
            List<DiscountGift> promotionGiftList = new List<DiscountGift>();
            try
            {
                while (reader.Read())
                {
                    var promotionGift = new DiscountGift
                    {
                        giftID = Convert.ToInt32(reader["GiftID"]),
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        appliedItemGuid = new Guid(reader["AppliedItemGuid"].ToString()),
                        discountRangeID = Convert.ToInt32(reader["DiscountRangeID"]),
                        name = reader["Name"].ToString(),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        imageFile = reader["ImageFile"].ToString(),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        quantity = Convert.ToInt32(reader["Quantity"]),
                        giftCount = Convert.ToInt32(reader["GiftCount"]),
                        giftTotal = Convert.ToInt32(reader["GiftTotal"]),
                        giftPrice = Convert.ToDecimal(reader["GiftPrice"]),
                        giftNote = reader["GiftNote"].ToString(),
                        url = reader["Url"].ToString(),
                        guid = new Guid(reader["Guid"].ToString())
                    };
                    promotionGiftList.Add(promotionGift);
                }
            }
            finally
            {
                reader.Close();
            }

            return promotionGiftList;
        }

        ///// <summary>
        ///// Gets an IList with all instances of PromotionGift.
        ///// </summary>
        //public static List<DiscountGift> GetAll()
        //{
        //    IDataReader reader = DBDiscountGift.GetAll();
        //    return LoadListFromReader(reader);

        //}

        ///// <summary>
        ///// Gets an IList with page of instances of PromotionGift.
        ///// </summary>
        ///// <param name="pageNumber">The page number.</param>
        ///// <param name="pageSize">Size of the page.</param>
        ///// <param name="totalPages">total pages</param>
        //public static List<DiscountGift> GetPage(int pageNumber, int pageSize, out int totalPages)
        //{
        //    totalPages = 1;
        //    IDataReader reader = DBDiscountGift.GetPage(pageNumber, pageSize, out totalPages);
        //    return LoadListFromReader(reader);
        //}

        public static List<DiscountGift> GetByDiscount(int discountId, Guid? appliedItemGuid = null, int discountRangeID = -1)
        {
            var reader = DBDiscountGift.GetByDiscount(discountId, appliedItemGuid, discountRangeID);
            return LoadListFromReader(reader);
        }

        //public static List<DiscountGift> GetByDiscounts(List<int> promotionIDs)
        //{
        //    if (promotionIDs.Count == 0) return new List<DiscountGift>();

        //    var reader = DBDiscountGift.GetByDiscounts(string.Join(";", promotionIDs.ToArray()));
        //    return LoadListFromReader(reader);
        //}

        public static bool DeleteByDiscount(int promotionId)
        {
            return DBDiscountGift.DeleteByDiscount(promotionId);
        }

        #endregion Static Methods
    }
}
/// Author:					Tran Quoc Vuong
/// Created:				2020-11-03
/// Last Modified:			2020-11-03

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class DiscountContent : IComparable
    {
        #region Constructors

        public DiscountContent()
        { }

        public DiscountContent(
            int contentID)
        {
            this.GetDiscountContent(
                contentID);
        }

        #endregion Constructors

        #region Private Properties

        private int contentID = -1;
        private int discountID = -1;
        private string title = string.Empty;
        private string description = string.Empty;
        private string bannerFile = string.Empty;
        private string thumbnailFile = string.Empty;
        private int contentType = 0;
        private int displayOrder = 0;
        private int loadType = 0;
        private decimal fromPrice = decimal.Zero;
        private decimal toPrice = decimal.Zero;
        private string zoneIDs = string.Empty;
        private int options = 0;
        private Guid guid = Guid.Empty;

        #endregion Private Properties

        #region Public Properties

        public int CompareTo(object value)
        {
            if (value == null) return 1;

            int compareOrder = ((DiscountContent)value).DisplayOrder;

            if (this.DisplayOrder == compareOrder) return 0;
            if (this.DisplayOrder < compareOrder) return -1;
            if (this.DisplayOrder > compareOrder) return 1;

            return 0;
        }

        public int ContentId
        {
            get { return contentID; }
            set { contentID = value; }
        }

        public int DiscountId
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string BannerFile
        {
            get { return bannerFile; }
            set { bannerFile = value; }
        }

        public string ThumbnailFile
        {
            get { return thumbnailFile; }
            set { thumbnailFile = value; }
        }

        public int ContentType
        {
            get { return contentType; }
            set { contentType = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public int LoadType
        {
            get { return loadType; }
            set { loadType = value; }
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

        public string ZoneIDs
        {
            get { return zoneIDs; }
            set { zoneIDs = value; }
        }

        public int Options
        {
            get { return options; }
            set { options = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of DiscountContent.
        /// </summary>
        /// <param name="contentID"> contentID </param>
        private void GetDiscountContent(
            int contentID)
        {
            using (IDataReader reader = DBDiscountContent.GetOne(
                contentID))
            {
                if (reader.Read())
                {
                    this.contentID = Convert.ToInt32(reader["ContentID"]);
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.title = reader["Title"].ToString();
                    this.description = reader["Description"].ToString();
                    this.bannerFile = reader["BannerFile"].ToString();
                    this.thumbnailFile = reader["ThumbnailFile"].ToString();
                    this.contentType = Convert.ToInt32(reader["ContentType"]);
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.loadType = Convert.ToInt32(reader["LoadType"]);
                    this.fromPrice = Convert.ToDecimal(reader["FromPrice"]);
                    this.toPrice = Convert.ToDecimal(reader["ToPrice"]);
                    this.zoneIDs = reader["ZoneIDs"].ToString();
                    this.options = Convert.ToInt32(reader["Options"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                }
            }
        }

        /// <summary>
        /// Persists a new instance of DiscountContent. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            this.guid = Guid.NewGuid();

            newID = DBDiscountContent.Create(
                this.discountID,
                this.title,
                this.description,
                this.bannerFile,
                this.thumbnailFile,
                this.contentType,
                this.displayOrder,
                this.loadType,
                this.fromPrice,
                this.toPrice,
                this.zoneIDs,
                this.options,
                this.guid);

            this.contentID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of DiscountContent. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBDiscountContent.Update(
                this.contentID,
                this.discountID,
                this.title,
                this.description,
                this.bannerFile,
                this.thumbnailFile,
                this.contentType,
                this.displayOrder,
                this.loadType,
                this.fromPrice,
                this.toPrice,
                this.zoneIDs,
                this.options,
                this.guid);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of DiscountContent. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.contentID > 0)
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
        /// Deletes an instance of DiscountContent. Returns true on success.
        /// </summary>
        /// <param name="contentID"> contentID </param>
        /// <returns>bool</returns>
        public static bool Delete(int contentID)
        {
            return DBDiscountContent.Delete(contentID);
        }

        public static bool DeleteByDiscount(int discountID)
        {
            return DBDiscountContent.DeleteByDiscount(discountID);
        }

        private static List<DiscountContent> LoadListFromReader(IDataReader reader)
        {
            List<DiscountContent> discountContentList = new List<DiscountContent>();
            try
            {
                while (reader.Read())
                {
                    DiscountContent discountContent = new DiscountContent
                    {
                        contentID = Convert.ToInt32(reader["ContentID"]),
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        title = reader["Title"].ToString(),
                        description = reader["Description"].ToString(),
                        bannerFile = reader["BannerFile"].ToString(),
                        thumbnailFile = reader["ThumbnailFile"].ToString(),
                        contentType = Convert.ToInt32(reader["ContentType"]),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        loadType = Convert.ToInt32(reader["LoadType"]),
                        fromPrice = Convert.ToDecimal(reader["FromPrice"]),
                        toPrice = Convert.ToDecimal(reader["ToPrice"]),
                        zoneIDs = reader["ZoneIDs"].ToString(),
                        options = Convert.ToInt32(reader["Options"]),
                        guid = new Guid(reader["Guid"].ToString())
                    };
                    discountContentList.Add(discountContent);
                }
            }
            finally
            {
                reader.Close();
            }

            return discountContentList;
        }

        public static List<DiscountContent> GetByDiscount(int discountID, int languageID = -1)
        {
            var reader = DBDiscountContent.GetByDiscount(discountID, languageID);
            return LoadListFromReader(reader);
        }

        public static int GetNextSortOrder(int discountID)
        {
            int nextSort = DBDiscountContent.GetMaxSortOrder(discountID) + 2;

            return nextSort;
        }

        public static void ResortContent(List<DiscountContent> attributeList)
        {
            int i = 1;
            attributeList.Sort();

            foreach (var m in attributeList)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                m.displayOrder = i;
                m.Save();

                i += 2;
            }
        }

        #endregion Static Methods
    }
}
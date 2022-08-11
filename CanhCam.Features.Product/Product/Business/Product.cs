using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum ProductMediaType
    {
        Image = 0,
        Video = -1
    }

    public class Product : IIndexableContent
    {
        private const string featureGuid = "6b34eb4d-c072-4331-ac3e-0c93f4c2c3dd";

        public static Guid FeatureGuid => new Guid(featureGuid);

        #region Constructors

        public Product()
        { }

        public Product(int siteId, int productId)
        {
            this.GetProduct(siteId, productId, -1);
        }

        public Product(int siteId, int productId, int languageId)
        {
            this.GetProduct(siteId, productId, languageId);
        }

        #endregion Constructors

        #region Private Properties

        private int productID = -1;
        private int siteID = -1;
        private int zoneID = -1;
        private string title = string.Empty;
        private string subTitle = string.Empty;
        private string url = string.Empty;
        private string code = string.Empty;
        private string briefContent = string.Empty;
        private string fullContent = string.Empty;
        private int productType = 0;
        private bool openInNewWindow = false;
        private int position = 0;
        private int showOption = 0;
        private bool isPublished = false;
        private DateTime startDate = DateTime.UtcNow;
        private DateTime endDate = DateTime.MaxValue;
        private int displayOrder = 0;
        private decimal price;
        private decimal oldPrice;
        private decimal specialPrice;
        private DateTime? specialPriceStartDate = null;
        private DateTime? specialPriceEndDate = null;
        private int viewCount = 0;
        private int commentCount = 0;
        private string metaTitle = string.Empty;
        private string metaKeywords = string.Empty;
        private string metaDescription = string.Empty;
        private string additionalMetaTags = string.Empty;
        private string compiledMeta = string.Empty;
        private int manufacturerID = -1;
        private int stockQuantity = 0;
        private bool disableBuyButton = false;
        private string fileAttachment = string.Empty;
        private Guid productGuid = Guid.Empty;
        private Guid userGuid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private DateTime lastModUtc = DateTime.UtcNow;
        private Guid lastModUserGuid = Guid.Empty;
        private bool isDeleted = false;
        private int? stateID = -1;
        private DateTime? approvedUtc = DateTime.UtcNow;
        private Guid? approvedUserGuid = Guid.Empty;
        private string approvedBy = string.Empty;
        private string rejectedNotes = string.Empty;

        private int userID = -1;

        private int previousProductId = -1;
        private int nextProductId = -1;
        private int nextZoneId = -1;
        private int previousZoneId = -1;
        private string previousProductUrl = string.Empty;
        private string previousProductTitle = string.Empty;
        private string nextProductUrl = string.Empty;
        private string nextProductTitle = string.Empty;
        private bool isLastProduct = false;
        private bool isFirstProduct = false;

        private string searchIndexPath = string.Empty;
        private string createdByName = string.Empty;

        private string imageFile = string.Empty;
        private string thumbnailFile = string.Empty;

        private int options = 0;
        private int parentID = 0;
        private int childCount = 0;
        private string apiProductID = string.Empty;
        private decimal weight = 0;
        private decimal ratingSum = 0;
        private int ratingVotes = 0;
        private string groupProductIDs = string.Empty;
        private decimal groupPrice = 0;

        #endregion Private Properties

        #region Public Properties

        public int ProductId
        {
            get { return productID; }
            set { productID = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public int ZoneId
        {
            get { return zoneID; }
            set { zoneID = value; }
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public string SubTitle
        {
            get { return subTitle; }
            set { subTitle = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
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

        public int ProductType
        {
            get { return productType; }
            set { productType = value; }
        }

        public bool OpenInNewWindow
        {
            get { return openInNewWindow; }
            set { openInNewWindow = value; }
        }

        public int Position
        {
            get { return position; }
            set { position = value; }
        }

        public int ShowOption
        {
            get { return showOption; }
            set { showOption = value; }
        }

        public bool IsPublished
        {
            get { return isPublished; }
            set { isPublished = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }

        public decimal OldPrice
        {
            get { return oldPrice; }
            set { oldPrice = value; }
        }

        public decimal SpecialPrice
        {
            get { return specialPrice; }
            set { specialPrice = value; }
        }

        public DateTime? SpecialPriceStartDate
        {
            get { return specialPriceStartDate; }
            set { specialPriceStartDate = value; }
        }

        public DateTime? SpecialPriceEndDate
        {
            get { return specialPriceEndDate; }
            set { specialPriceEndDate = value; }
        }

        public int ViewCount
        {
            get { return viewCount; }
            set { viewCount = value; }
        }

        public int CommentCount
        {
            get { return commentCount; }
            set { commentCount = value; }
        }

        public string MetaTitle
        {
            get { return metaTitle; }
            set { metaTitle = value; }
        }

        public string MetaKeywords
        {
            get { return metaKeywords; }
            set { metaKeywords = value; }
        }

        public string MetaDescription
        {
            get { return metaDescription; }
            set { metaDescription = value; }
        }

        public string AdditionalMetaTags
        {
            get { return additionalMetaTags; }
            set { additionalMetaTags = value; }
        }

        public string CompiledMeta
        {
            get { return compiledMeta; }
            set { compiledMeta = value; }
        }

        public int ManufacturerId
        {
            get { return manufacturerID; }
            set { manufacturerID = value; }
        }

        public int StockQuantity
        {
            get { return stockQuantity; }
            set { stockQuantity = value; }
        }

        public bool DisableBuyButton
        {
            get { return disableBuyButton; }
            set { disableBuyButton = value; }
        }

        public string BarCode
        {
            get { return fileAttachment; }
            set { fileAttachment = value; }
        }

        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
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

        public Guid LastModUserGuid
        {
            get { return lastModUserGuid; }
            set { lastModUserGuid = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public int? StateId
        {
            get { return stateID; }
            set { stateID = value; }
        }

        public DateTime? ApprovedUtc
        {
            get { return approvedUtc; }
            set { approvedUtc = value; }
        }

        public Guid? ApprovedUserGuid
        {
            get { return approvedUserGuid; }
            set { approvedUserGuid = value; }
        }

        public string ApprovedBy
        {
            get { return approvedBy; }
            set { approvedBy = value; }
        }

        public string RejectedNotes
        {
            get { return rejectedNotes; }
            set { rejectedNotes = value; }
        }

        public int UserId
        {
            get { return userID; }
            set { userID = value; }
        }

        private string userEmail = string.Empty;

        public string UserEmail
        {
            get { return userEmail; }
            set { userEmail = value; }
        }

        public string CreatedByName
        {
            get { return createdByName; }
            set { createdByName = value; }
        }

        public int PreviousProductId
        {
            get { return previousProductId; }
            set { previousProductId = value; }
        }

        public int PreviousZoneId
        {
            get { return previousZoneId; }
            set { previousZoneId = value; }
        }

        public int NextProductId
        {
            get { return nextProductId; }
            set { nextProductId = value; }
        }

        public int NextZoneId
        {
            get { return nextZoneId; }
            set { nextZoneId = value; }
        }

        public string PreviousProductUrl
        {
            get { return previousProductUrl; }
            set { previousProductUrl = value; }
        }

        public string NextProductUrl
        {
            get { return nextProductUrl; }
            set { nextProductUrl = value; }
        }

        public string PreviousProductTitle
        {
            get { return previousProductTitle; }
            set { previousProductTitle = value; }
        }

        public string NextProductTitle
        {
            get { return nextProductTitle; }
            set { nextProductTitle = value; }
        }

        public bool IsLastProduct
        {
            get { return isLastProduct; }
            set { isLastProduct = value; }
        }

        public bool IsFirstProduct
        {
            get { return isFirstProduct; }
            set { isFirstProduct = value; }
        }

        /// <summary>
        /// This is not persisted to the db. It is only set and used when indexing forum threads in the search index.
        /// Its a convenience because when we queue the task to index on a new thread we can only pass one object.
        /// So we store extra properties here so we don't need any other objects.
        /// </summary>
        public string SearchIndexPath
        {
            get { return searchIndexPath; }
            set { searchIndexPath = value; }
        }

        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        public string ThumbnailFile
        {
            get { return thumbnailFile; }
            set { thumbnailFile = value; }
        }

        public int AllowCommentsForDays => 0;

        public int Options
        {
            get { return options; }
            set { options = value; }
        }

        public int ParentId
        {
            get { return parentID; }
            set { parentID = value; }
        }

        public int ChildCount
        {
            get { return childCount; }
            set { childCount = value; }
        }

        public string ApiProductId
        {
            get { return apiProductID; }
            set { apiProductID = value; }
        }

        public decimal Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public decimal RatingSum
        {
            get { return ratingSum; }
            set { ratingSum = value; }
        }

        public int RatingVotes
        {
            get { return ratingVotes; }
            set { ratingVotes = value; }
        }

        public string GroupProductIDs
        {
            get { return groupProductIDs; }
            set { groupProductIDs = value; }
        }

        public decimal GroupPrice
        {
            get { return groupPrice; }
            set { groupPrice = value; }
        }

        #endregion Public Properties

        #region Private Methods

        protected void GetProduct(int siteId, int productId, int languageId)
        {
            PopulateProduct(this, DBProduct.GetOne(siteId, productId, languageId));
        }

        public static void PopulateProduct(Product product, IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    product.productID = Convert.ToInt32(reader["ProductID"]);
                    product.siteID = Convert.ToInt32(reader["SiteID"]);
                    product.zoneID = Convert.ToInt32(reader["ZoneID"]);
                    product.title = reader["Title"].ToString();
                    product.subTitle = reader["SubTitle"].ToString();
                    product.url = reader["Url"].ToString();
                    product.code = reader["Code"].ToString();
                    product.briefContent = reader["BriefContent"].ToString();
                    product.fullContent = reader["FullContent"].ToString();
                    product.productType = Convert.ToInt32(reader["ProductType"]);
                    product.openInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]);
                    product.position = Convert.ToInt32(reader["Position"]);
                    product.showOption = Convert.ToInt32(reader["ShowOption"]);
                    product.isPublished = Convert.ToBoolean(reader["IsPublished"]);
                    product.startDate = Convert.ToDateTime(reader["StartDate"]);
                    product.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    product.price = Convert.ToDecimal(reader["Price"]);
                    product.oldPrice = Convert.ToDecimal(reader["OldPrice"]);
                    product.specialPrice = Convert.ToDecimal(reader["SpecialPrice"]);
                    product.viewCount = Convert.ToInt32(reader["ViewCount"]);
                    product.commentCount = Convert.ToInt32(reader["CommentCount"]);
                    product.metaTitle = reader["MetaTitle"].ToString();
                    product.metaKeywords = reader["MetaKeywords"].ToString();
                    product.metaDescription = reader["MetaDescription"].ToString();
                    product.additionalMetaTags = reader["AdditionalMetaTags"].ToString();
                    product.compiledMeta = reader["CompiledMeta"].ToString();
                    product.manufacturerID = Convert.ToInt32(reader["ManufacturerID"]);
                    product.stockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                    product.disableBuyButton = Convert.ToBoolean(reader["DisableBuyButton"]);
                    product.fileAttachment = reader["FileAttachment"].ToString();
                    product.productGuid = new Guid(reader["ProductGuid"].ToString());
                    product.userGuid = new Guid(reader["UserGuid"].ToString());
                    product.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    product.userEmail = reader["Email"].ToString();
                    product.options = Convert.ToInt32(reader["Options"]);
                    product.parentID = Convert.ToInt32(reader["ParentID"]);
                    product.childCount = Convert.ToInt32(reader["ChildCount"]);
                    product.apiProductID = reader["ApiProductID"].ToString();
                    product.weight = Convert.ToDecimal(reader["Weight"]);
                    product.ratingSum = Convert.ToDecimal(reader["RatingSum"]);
                    product.ratingVotes = Convert.ToInt32(reader["RatingVotes"]);
                    product.lastModUserGuid = new Guid(reader["LastModUserGuid"].ToString());

                    if (reader["LastModUtc"] != DBNull.Value)
                        product.lastModUtc = Convert.ToDateTime(reader["LastModUtc"]);
                    if (reader["EndDate"] != DBNull.Value)
                        product.endDate = Convert.ToDateTime(reader["EndDate"]);

                    if (reader["SpecialPriceStartDate"] != DBNull.Value)
                        product.specialPriceStartDate = Convert.ToDateTime(reader["SpecialPriceStartDate"]);
                    if (reader["SpecialPriceEndDate"] != DBNull.Value)
                        product.specialPriceEndDate = Convert.ToDateTime(reader["SpecialPriceEndDate"]);

                    string var = reader["UserGuid"].ToString();
                    if (var.Length == 36) product.userGuid = new Guid(var);
                    var = reader["LastModUserGuid"].ToString();
                    if (var.Length == 36) product.lastModUserGuid = new Guid(var);

                    if (reader["IsDeleted"] != DBNull.Value)
                        product.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    if (reader["StateID"] != DBNull.Value)
                        product.stateID = Convert.ToInt32(reader["StateID"]);
                    if (reader["ApprovedUtc"] != DBNull.Value)
                        product.approvedUtc = Convert.ToDateTime(reader["ApprovedUtc"]);
                    if (reader["ApprovedUserGuid"] != DBNull.Value)
                        product.approvedUserGuid = new Guid(reader["ApprovedUserGuid"].ToString());
                    if (reader["ApprovedBy"] != DBNull.Value)
                        product.approvedBy = reader["ApprovedBy"].ToString();
                    if (reader["RejectedNotes"] != DBNull.Value)
                        product.rejectedNotes = reader["RejectedNotes"].ToString();
                    if (reader["GroupProductIds"] != DBNull.Value)
                        product.groupProductIDs = reader["GroupProductIds"].ToString();
                    if (reader["GroupPrice"] != DBNull.Value)
                        product.groupPrice = Convert.ToDecimal(reader["GroupPrice"].ToString());
                }
            }
            finally
            {
                reader.Close();
            }
        }

        private bool Create()
        {
            int newID = 0;
            productGuid = Guid.NewGuid();
            createdUtc = DateTime.UtcNow;

            newID = DBProduct.Create(
                this.siteID,
                this.zoneID,
                this.title,
                this.subTitle,
                this.url,
                this.code,
                this.briefContent,
                this.fullContent,
                this.productType,
                this.openInNewWindow,
                this.position,
                this.showOption,
                this.isPublished,
                this.startDate,
                this.endDate,
                this.displayOrder,
                this.price,
                this.oldPrice,
                this.specialPrice,
                this.specialPriceStartDate,
                this.specialPriceEndDate,
                this.viewCount,
                this.commentCount,
                this.metaTitle,
                this.metaKeywords,
                this.metaDescription,
                this.additionalMetaTags,
                this.compiledMeta,
                this.manufacturerID,
                this.stockQuantity,
                this.disableBuyButton,
                this.fileAttachment,
                this.productGuid,
                this.userGuid,
                this.createdUtc,
                this.lastModUtc,
                this.lastModUserGuid,
                this.isDeleted,
                this.stateID,
                this.approvedUtc,
                this.approvedUserGuid,
                this.approvedBy,
                this.rejectedNotes,
                this.options,
                this.parentID,
                this.childCount,
                this.apiProductID,
                this.weight,
                this.ratingSum,
                this.ratingVotes,
                this.groupProductIDs,
                this.groupPrice);

            this.productID = newID;

            bool result = (newID > 0);

            //IndexHelper.IndexItem(this);
            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;
        }

        private bool Update()
        {
            this.lastModUtc = DateTime.UtcNow;
            bool result = DBProduct.Update(
                this.productID,
                this.siteID,
                this.zoneID,
                this.title,
                this.subTitle,
                this.url,
                this.code,
                this.briefContent,
                this.fullContent,
                this.productType,
                this.openInNewWindow,
                this.position,
                this.showOption,
                this.isPublished,
                this.startDate,
                this.endDate,
                this.displayOrder,
                this.price,
                this.oldPrice,
                this.specialPrice,
                this.specialPriceStartDate,
                this.specialPriceEndDate,
                this.viewCount,
                this.commentCount,
                this.metaTitle,
                this.metaKeywords,
                this.metaDescription,
                this.additionalMetaTags,
                this.compiledMeta,
                this.manufacturerID,
                this.stockQuantity,
                this.disableBuyButton,
                this.fileAttachment,
                this.productGuid,
                this.userGuid,
                this.createdUtc,
                this.lastModUtc,
                this.lastModUserGuid,
                this.isDeleted,
                this.stateID,
                this.approvedUtc,
                this.approvedUserGuid,
                this.approvedBy,
                this.rejectedNotes,
                this.options,
                this.parentID,
                this.childCount,
                this.apiProductID,
                this.weight,
                this.ratingSum,
                this.ratingVotes,
                this.groupProductIDs,
                this.groupPrice);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs();
                OnContentChanged(e);
            }

            return result;
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.productID > 0)
                return this.Update();

            return this.Create();
        }

        public bool Delete()
        {
            bool result = DBProduct.Delete(this.productID);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs
                {
                    IsDeleted = true
                };
                OnContentChanged(e);
            }

            return result;
        }

        public bool SaveState()
        {
            bool result = DBProduct.UpdateState(this.productID, this.StateId, this.approvedUtc, this.approvedUserGuid, this.approvedBy, this.rejectedNotes);

            //if (result)
            //{
            //    if (this.StateId == ContentWorkflowStatus.Approved)
            //    {
            //        this.isPublished = true;
            //        this.Save();
            //    }

            //    ContentChangedEventArgs e = new ContentChangedEventArgs();
            //    OnContentChanged(e);
            //}

            return result;
        }

        public bool SaveStockQuantity()
        {
            bool result = DBProduct.UpdateStockQuantity(this.productID, this.stockQuantity);
            return result;
        }

        public bool SaveDeleted()
        {
            bool result = DBProduct.UpdateDeleted(this.productID, this.isDeleted);

            if (result)
            {
                ContentChangedEventArgs e = new ContentChangedEventArgs
                {
                    IsDeleted = this.isDeleted
                };
                OnContentChanged(e);
            }

            return result;
        }

        public void LoadNextPrevious()
        {
            LoadNextPrevious(-1);
        }

        public void LoadNextPrevious(int languageId)
        {
            using (IDataReader reader = DBProduct.GetNextPreviousProduct(this.productID, this.zoneID, languageId))
            {
                if (reader.Read())
                {
                    if (reader["PreviousProductID"] != DBNull.Value)
                        this.previousProductId = Convert.ToInt32(reader["PreviousProductID"].ToString());
                    if (reader["NextProductID"] != DBNull.Value)
                        this.nextProductId = Convert.ToInt32(reader["NextProductID"].ToString());
                    if (reader["PreviousZoneID"] != DBNull.Value)
                        this.previousZoneId = Convert.ToInt32(reader["PreviousZoneID"].ToString());
                    if (reader["NextZoneID"] != DBNull.Value)
                        this.nextZoneId = Convert.ToInt32(reader["NextZoneID"].ToString());
                    if (reader["PreviousProductUrl"] != DBNull.Value)
                        this.previousProductUrl = reader["PreviousProductUrl"].ToString();
                    if (reader["PreviousProductTitle"] != DBNull.Value)
                        this.previousProductTitle = reader["PreviousProductTitle"].ToString();
                    if (reader["NextProductUrl"] != DBNull.Value)
                        this.nextProductUrl = reader["NextProductUrl"].ToString();
                    if (reader["NextProductTitle"] != DBNull.Value)
                        this.nextProductTitle = reader["NextProductTitle"].ToString();

                    if (reader["IsFirstProduct"] != DBNull.Value)
                        this.isFirstProduct = Convert.ToBoolean(reader["IsFirstProduct"]);
                    if (reader["IsLastProduct"] != DBNull.Value)
                        this.isLastProduct = Convert.ToBoolean(reader["IsLastProduct"]);
                }
            }
        }

        #endregion Public Methods

        #region Static Methods

        public static Product GetOneByZone(int zoneId, int languageId)
        {
            Product news = new Product();
            PopulateProduct(news, DBProduct.GetOne(zoneId, DateTime.UtcNow, languageId));

            return news;
        }

        public static bool DeleteBySite(int siteId)
        {
            return DBProduct.DeleteBySite(siteId);
        }

        public static List<Product> LoadListFromReader(IDataReader reader, bool loadImage = true)
        {
            List<Product> productList = new List<Product>();
            try
            {
                while (reader.Read())
                {
                    Product product = new Product
                    {
                        productID = Convert.ToInt32(reader["ProductID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        zoneID = Convert.ToInt32(reader["ZoneID"]),
                        title = reader["Title"].ToString(),
                        subTitle = reader["SubTitle"].ToString(),
                        url = reader["Url"].ToString(),
                        code = reader["Code"].ToString(),
                        briefContent = reader["BriefContent"].ToString(),
                        fullContent = reader["FullContent"].ToString(),
                        productType = Convert.ToInt32(reader["ProductType"]),
                        openInNewWindow = Convert.ToBoolean(reader["OpenInNewWindow"]),
                        position = Convert.ToInt32(reader["Position"]),
                        showOption = Convert.ToInt32(reader["ShowOption"]),
                        isPublished = Convert.ToBoolean(reader["IsPublished"]),
                        startDate = Convert.ToDateTime(reader["StartDate"]),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        price = Convert.ToDecimal(reader["Price"]),
                        oldPrice = Convert.ToDecimal(reader["OldPrice"]),
                        specialPrice = Convert.ToDecimal(reader["SpecialPrice"]),
                        viewCount = Convert.ToInt32(reader["ViewCount"]),
                        commentCount = Convert.ToInt32(reader["CommentCount"]),
                        metaTitle = reader["MetaTitle"].ToString(),
                        metaKeywords = reader["MetaKeywords"].ToString(),
                        metaDescription = reader["MetaDescription"].ToString(),
                        additionalMetaTags = reader["AdditionalMetaTags"].ToString(),
                        compiledMeta = reader["CompiledMeta"].ToString(),
                        manufacturerID = Convert.ToInt32(reader["ManufacturerID"]),
                        stockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        disableBuyButton = Convert.ToBoolean(reader["DisableBuyButton"]),
                        fileAttachment = reader["FileAttachment"].ToString(),
                        productGuid = new Guid(reader["ProductGuid"].ToString()),
                        userGuid = new Guid(reader["UserGuid"].ToString()),
                        createdUtc = Convert.ToDateTime(reader["CreatedUtc"]),
                        lastModUtc = Convert.ToDateTime(reader["LastModUtc"]),
                        lastModUserGuid = new Guid(reader["LastModUserGuid"].ToString()),
                        options = Convert.ToInt32(reader["Options"]),
                        parentID = Convert.ToInt32(reader["ParentID"]),
                        childCount = Convert.ToInt32(reader["ChildCount"]),
                        apiProductID = reader["ApiProductID"].ToString(),
                        weight = Convert.ToDecimal(reader["Weight"]),
                        ratingSum = Convert.ToDecimal(reader["RatingSum"]),
                        ratingVotes = Convert.ToInt32(reader["RatingVotes"])
                    };

                    if (reader["EndDate"] != DBNull.Value)
                        product.endDate = Convert.ToDateTime(reader["EndDate"]);
                    if (reader["SpecialPriceStartDate"] != DBNull.Value)
                        product.specialPriceStartDate = Convert.ToDateTime(reader["SpecialPriceStartDate"]);
                    if (reader["SpecialPriceEndDate"] != DBNull.Value)
                        product.specialPriceEndDate = Convert.ToDateTime(reader["SpecialPriceEndDate"]);
                    if (reader["IsDeleted"] != DBNull.Value)
                        product.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    if (reader["StateID"] != DBNull.Value)
                        product.stateID = Convert.ToInt32(reader["StateID"]);
                    if (reader["GroupProductIds"] != DBNull.Value)
                        product.groupProductIDs = reader["GroupProductIds"].ToString();
                    if (reader["GroupPrice"] != DBNull.Value)
                        product.groupPrice = Convert.ToDecimal(reader["GroupPrice"].ToString());

                    if (loadImage && reader["ImageFile"] != DBNull.Value)
                        product.ImageFile = reader["ImageFile"].ToString();
                    if (loadImage && reader["ThumbnailFile"] != DBNull.Value)
                        product.ThumbnailFile = reader["ThumbnailFile"].ToString();

                    productList.Add(product);
                }
            }
            finally
            {
                reader.Close();
            }

            return productList;
        }

        //public static int GetCountByListZone(
        //    int siteId,
        //    string listZoneId,
        //    int languageId,
        //    int position)
        //{
        //    return DBProduct.GetCountByListZone(siteId, listZoneId, languageId, position);
        //}

        //public static List<Product> GetPageByListZone(
        //    int siteId,
        //    string listZoneId,
        //    int languageId,
        //    int position,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    return LoadListFromReader(DBProduct.GetPageByListZone(siteId, listZoneId, languageId, position, pageNumber, pageSize));
        //}

        //public static int GetCount(
        //    int siteId,
        //    int zoneId,
        //    int languageId,
        //    int position)
        //{
        //    return DBProduct.GetCount(siteId, zoneId, DateTime.UtcNow, languageId, position);
        //}

        //public static List<Product> GetPage(
        //    int siteId,
        //    int zoneId,
        //    int languageId,
        //    int position,
        //    int pageNumber,
        //    int pageSize)
        //{
        //    return LoadListFromReader(DBProduct.GetPage(siteId, zoneId, DateTime.UtcNow, languageId, position, pageNumber, pageSize));
        //}

        //public static int GetCountBySearch(
        //    int siteId = -1,
        //    string zoneIds = null,
        //    int publishStatus = -1,
        //    int languageId = -1,
        //    int manufactureId = -1,
        //    int productType = -1,
        //    decimal? priceMin = null,
        //    decimal? priceMax = null,
        //    int position = -1,
        //    int showOption = -1,
        //    string propertyCondition = null,
        //    string productIds = null,
        //    string keyword = null,
        //    bool searchCode = false,
        //    string stateIds = null,
        //    ProductSortingEnum orderBy = ProductSortingEnum.Position)
        //{
        //    return DBProduct.GetCountBySearch(siteId, zoneIds, publishStatus, languageId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, stateIds, (int)orderBy);
        //}

        //public static List<Product> GetPageBySearch(
        //    int pageNumber = 1,
        //    int pageSize = 32767,
        //    int siteId = -1,
        //    string zoneIds = null,
        //    int publishStatus = -1,
        //    int languageId = -1,
        //    int manufactureId = -1,
        //    int productType = -1,
        //    decimal? priceMin = null,
        //    decimal? priceMax = null,
        //    int position = -1,
        //    int showOption = -1,
        //    string propertyCondition = null,
        //    string productIds = null,
        //    string keyword = null,
        //    bool searchCode = false,
        //    string stateIds = null,
        //    int orderBy = (int)ProductSortingEnum.Position)
        //{
        //    return LoadListFromReader(DBProduct.GetPageBySearch(pageNumber, pageSize, siteId, zoneIds, publishStatus, languageId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, stateIds, orderBy));
        //}

        public static int GetCountAdv(
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = 0,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null)
        {
            return DBProduct.GetCountAdv(siteId, zoneId, zoneIds, publishStatus, languageId, parentId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, searchProductZone, stateIds);
        }

        public static List<Product> GetPageAdv(
            int pageNumber = 1,
            int pageSize = 32767,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = 0,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null,
            int discountId = -1,
            int orderBy = (int)ProductSortingEnum.Position)
        {
            return LoadListFromReader(DBProduct.GetPageAdv(pageNumber, pageSize, siteId, zoneId, zoneIds, publishStatus, languageId, parentId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, searchProductZone, stateIds, discountId, orderBy));
        }

        public static List<Product> GetTopAdv(
            int top = 1,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = 0,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null,
            int orderBy = (int)ProductSortingEnum.Position)
        {
            return LoadListFromReader(DBProduct.GetTopAdv(top, siteId, zoneId, zoneIds, publishStatus, languageId, parentId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, searchProductZone, stateIds, orderBy));
        }

        //public static List<Product> GetPageProductOther(
        //   int zoneId,
        //   int productId,
        //   int languageId,
        //   int pageNumber,
        //   int pageSize,
        //   out int totalPages)
        //{
        //    return LoadListFromReader(DBProduct.GetPageProductOther(zoneId, productId, DateTime.UtcNow, languageId, pageNumber, pageSize, out totalPages));
        //}

        /// <summary>
        /// Gets a related manufacturer collection by manufacturer identifier
        /// </summary>
        /// <param name="productGuid">The first manufacturer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="twoWayRelated">two-way relationship</param>
        /// <returns>Related manufacturer collection</returns>
        public static List<Product> GetRelatedProducts(
            int siteId,
            Guid siteGuid,
            Guid productGuid,
            bool showHidden = false,
            bool twoWayRelated = false,
            int languageId = -1)
        {
            return LoadListFromReader(DBProduct.GetRelatedProducts(siteId, siteGuid, productGuid, showHidden, twoWayRelated, languageId));
        }

        //public static List<Product> GetByZone(int siteId, int zoneId)
        //{
        //    return LoadListFromReader(DBProduct.GetByZone(siteId, zoneId), false);
        //}

        public static List<Product> GetByShoppingCart(
           int siteId,
           Guid cartGuid,
           ShoppingCartTypeEnum cartType,
           int languageId = -1)
        {
            return LoadListFromReader(DBProduct.GetByShoppingCart(siteId, cartGuid, (int)cartType, languageId));
        }

        public static List<Product> GetByTag(
           int siteId,
           int tagId,
           int publishStatus = -1,
           int languageId = -1)
        {
            return LoadListFromReader(DBProduct.GetByTag(siteId, tagId, publishStatus, languageId));
        }

        public static List<Product> GetByGuids(
           int siteId,
           string productGuids,
           int publishStatus = -1,
           int languageId = -1)
        {
            return LoadListFromReader(DBProduct.GetByGuids(siteId, productGuids, publishStatus, languageId));
        }

        public static List<Product> GetByOrder(
            int siteId,
            int orderId,
            int languageId = -1)
        {
            return LoadListFromReader(DBProduct.GetByOrder(siteId, orderId, languageId));
        }

        public static IDataReader GetProductForSiteMap(int siteId, int languageId)
        {
            return DBProduct.GetProductForSiteMap(siteId, languageId);
        }

        public static bool IncrementViewCount(int productId)
        {
            return DBProduct.IncrementViewCount(productId);
        }

        public static bool UpdateZone(int productId, int zoneId)
        {
            return DBProduct.UpdateZone(productId, zoneId);
        }

        public static bool UpdateCommentCount(int productId, int commentCount)
        {
            return DBProduct.UpdateCommentCount(productId, commentCount);
        }

        public static Product GetByCode(int siteId, string productCode)
        {
            var lst = LoadListFromReader(DBProduct.GetByCode(siteId, productCode), false);
            if (lst.Count > 0)
                return lst[0];

            return null;
        }

        //public static List<Product> GetPageByKeyword(
        // int siteId,
        // string keyword,
        // int pageNumber,
        // int pageSize)
        //{
        //    return LoadListFromReader(DBProduct.GetPageByKeyword(siteId, keyword, pageNumber, pageSize));
        //}
        //public static int GetCountByKeyword(
        //    int siteId,
        //    string keyword)
        //{
        //    return DBProduct.GetCountByKeyword(siteId, keyword);
        //}

        public static List<int> GetZoneByKeyword(
         int siteId,
         string keyword)
        {
            var reader = DBProduct.GetZoneByKeyword(siteId, keyword);
            var lst = new List<int>();

            try
            {
                while (reader.Read())
                {
                    lst.Add(Convert.ToInt32(reader["ZoneID"]));
                }
            }
            finally
            {
                reader.Close();
            }

            return lst;
        }

        public static bool UpdateCommentStats(int productId)
        {
            return DBProduct.UpdateCommentStats(productId);
        }

        public static bool UpdateInventory(int productId)
        {
            return DBProduct.UpdateInventory(productId);
        }

        #endregion Static Methods

        #region Child Products

        public static bool ExistsChild(
            int productID)
        {
            IDataReader reader = DBProduct.ExistsChild(productID);
            bool existsChild = false;
            try
            {
                while (reader.Read())
                {
                    existsChild = Convert.ToBoolean(reader["Value"]);
                }
            }
            finally
            {
                reader.Close();
            }
            return existsChild;
        }

        #endregion Child Products

        #region IIndexableContent

        public event ContentChangedEventHandler ContentChanged;

        protected void OnContentChanged(ContentChangedEventArgs e)
        {
            ContentChanged?.Invoke(this, e);
        }

        #endregion IIndexableContent
    }
}
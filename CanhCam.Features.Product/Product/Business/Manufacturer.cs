using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum ManufacturerPublishStatus
    {
        NotSet = -1,
        UnPublished = 0,
        Published = 1
    }

    public class Manufacturer
    {
        #region Constructors

        public Manufacturer()
        { }

        public Manufacturer(int manufacturerID)
        {
            this.GetManufacturer(manufacturerID);
        }

        #endregion Constructors

        #region Private Properties

        private int manufacturerID = -1;
        private int pageID = -1;
        private int siteID = -1;
        private string name = string.Empty;
        private string url = string.Empty;
        private string description = string.Empty;
        private string primaryImage = string.Empty;
        private string secondImage = string.Empty;
        private int displayOrder = 0;
        private int showOption = 0;
        private bool isPublished = false;
        private Guid guid = Guid.Empty;
        private string metaTitle = string.Empty;
        private string metaKeywords = string.Empty;
        private string metaDescription = string.Empty;
        private DateTime createdDate = DateTime.UtcNow;
        private DateTime modifiedDate = DateTime.UtcNow;
        private bool isDeleted = false;

        #endregion Private Properties

        #region Public Properties

        public int ManufacturerId
        {
            get { return manufacturerID; }
            set { manufacturerID = value; }
        }

        public int PageId
        {
            get { return pageID; }
            set { pageID = value; }
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

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string PrimaryImage
        {
            get { return primaryImage; }
            set { primaryImage = value; }
        }

        public string SecondImage
        {
            get { return secondImage; }
            set { secondImage = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
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

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
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

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return modifiedDate; }
            set { modifiedDate = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of Manufacturer.
        /// </summary>
        /// <param name="manufacturerID"> manufacturerID </param>
        private void GetManufacturer(
            int manufacturerID)
        {
            using (IDataReader reader = DBManufacturer.GetOne(
                manufacturerID))
            {
                if (reader.Read())
                {
                    this.manufacturerID = Convert.ToInt32(reader["ManufacturerID"]);
                    this.pageID = Convert.ToInt32(reader["PageID"]);
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.name = reader["Name"].ToString();
                    this.url = reader["Url"].ToString();
                    this.description = reader["Description"].ToString();
                    this.primaryImage = reader["PrimaryImage"].ToString();
                    this.secondImage = reader["SecondImage"].ToString();
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.showOption = Convert.ToInt32(reader["ShowOption"]);
                    this.isPublished = Convert.ToBoolean(reader["IsPublished"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.metaTitle = reader["MetaTitle"].ToString();
                    this.metaKeywords = reader["MetaKeywords"].ToString();
                    this.metaDescription = reader["MetaDescription"].ToString();
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.modifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                    this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of Manufacturer. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();

            newID = DBManufacturer.Create(
                this.pageID,
                this.siteID,
                this.name,
                this.url,
                this.description,
                this.primaryImage,
                this.secondImage,
                this.displayOrder,
                this.showOption,
                this.isPublished,
                this.guid,
                this.metaTitle,
                this.metaKeywords,
                this.metaDescription,
                this.createdDate,
                this.modifiedDate,
                this.isDeleted);

            this.manufacturerID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of Manufacturer. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBManufacturer.Update(
                this.manufacturerID,
                this.pageID,
                this.siteID,
                this.name,
                this.url,
                this.description,
                this.primaryImage,
                this.secondImage,
                this.displayOrder,
                this.showOption,
                this.isPublished,
                this.guid,
                this.metaTitle,
                this.metaKeywords,
                this.metaDescription,
                this.createdDate,
                this.modifiedDate,
                this.isDeleted);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.manufacturerID > 0)
                return this.Update();

            return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of Manufacturer. Returns true on success.
        /// </summary>
        /// <param name="manufacturerID"> manufacturerID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int manufacturerID)
        {
            return DBManufacturer.Delete(
                manufacturerID);
        }
        /// <summary>
        /// Gets a count of Manufacturer by Title.
        /// </summary>
        public static int GetCountByTitle(int siteId, string title)
        {
            return DBManufacturer.GetCountByTitle(siteId, title);
        }


        /// <summary>
        /// Gets a count of Manufacturer.
        /// </summary>
        public static int GetCount(int siteId, ManufacturerPublishStatus publishStatus, Guid? zoneGuid, string keyword = "")
        {
            return DBManufacturer.GetCount(siteId, (int)publishStatus, zoneGuid, keyword);
        }

        private static List<Manufacturer> LoadListFromReader(IDataReader reader)
        {
            List<Manufacturer> manufacturerList = new List<Manufacturer>();
            try
            {
                while (reader.Read())
                {
                    Manufacturer manufacturer = new Manufacturer
                    {
                        manufacturerID = Convert.ToInt32(reader["ManufacturerID"]),
                        pageID = Convert.ToInt32(reader["PageID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        name = reader["Name"].ToString(),
                        url = reader["Url"].ToString(),
                        description = reader["Description"].ToString(),
                        primaryImage = reader["PrimaryImage"].ToString(),
                        secondImage = reader["SecondImage"].ToString(),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        showOption = Convert.ToInt32(reader["ShowOption"]),
                        isPublished = Convert.ToBoolean(reader["IsPublished"]),
                        guid = new Guid(reader["Guid"].ToString()),
                        metaTitle = reader["MetaTitle"].ToString(),
                        metaKeywords = reader["MetaKeywords"].ToString(),
                        metaDescription = reader["MetaDescription"].ToString(),
                        createdDate = Convert.ToDateTime(reader["CreatedDate"]),
                        modifiedDate = Convert.ToDateTime(reader["ModifiedDate"]),
                        isDeleted = Convert.ToBoolean(reader["IsDeleted"])
                    };
                    manufacturerList.Add(manufacturer);
                }
            }
            finally
            {
                reader.Close();
            }

            return manufacturerList;
        }

        public static List<Manufacturer> GetAll(int siteId, ManufacturerPublishStatus publishStatus, Guid? zoneGuid, int showOption)
        {
            var reader = DBManufacturer.GetAll(siteId, (int)publishStatus, zoneGuid, showOption);
            return LoadListFromReader(reader);
        }

        public static List<Manufacturer> GetPage(int siteId, ManufacturerPublishStatus publishStatus,
            Guid? zoneGuid, int pageNumber, int pageSize, string keyword = "")
        {
            var reader = DBManufacturer.GetPage(siteId, (int)publishStatus, zoneGuid, pageNumber, pageSize, keyword);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
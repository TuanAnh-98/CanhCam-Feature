using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class ProductType
    {
        #region Constructors

        public ProductType()
        { }

        public ProductType(
            int productTypeId)
        {
            this.GetProductType(productTypeId, -1);
        }

        public ProductType(
            int productTypeId, int languageId)
        {
            this.GetProductType(productTypeId, languageId);
        }

        #endregion Constructors

        #region Private Properties

        private int productTypeId = -1;
        private Guid itemGuid = Guid.Empty;
        private int siteID = -1;
        private string name = string.Empty;
        private string description = string.Empty;
        private string primaryImage = string.Empty;
        private string secondImage = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int ProductTypeId
        {
            get { return productTypeId; }
            set { productTypeId = value; }
        }

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }

        public int SiteID
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
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

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of ProductType.
        /// </summary>
        /// <param name="productTypeId"> productTypeId </param>
        private void GetProductType(
            int productTypeId, int languageId)
        {
            using (IDataReader reader = DBProductType.GetOne(productTypeId, languageId))
            {
                if (reader.Read())
                {
                    this.productTypeId = Convert.ToInt32(reader["ProductTypeId"]);
                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();
                    this.primaryImage = reader["PrimaryImage"].ToString();
                    this.secondImage = reader["SecondImage"].ToString();
                }
            }
        }

        /// <summary>
        /// Persists a new instance of ProductType. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;
            this.itemGuid = Guid.NewGuid();
            newID = DBProductType.Create(
                this.itemGuid,
                this.siteID,
                this.name,
                this.description,
                this.primaryImage,
                this.secondImage);

            this.productTypeId = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of ProductType. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBProductType.Update(
                this.productTypeId,
                this.itemGuid,
                this.siteID,
                this.name,
                this.description,
                this.primaryImage,
                this.secondImage);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of ProductType. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.productTypeId > 0)
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
        /// Deletes an instance of ProductType. Returns true on success.
        /// </summary>
        /// <param name="productTypeId"> productTypeId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int productTypeId)
        {
            return DBProductType.Delete(
                productTypeId);
        }

        /// <summary>
        /// Gets a count of ProductType.
        /// </summary>
        public static int GetCount()
        {
            return DBProductType.GetCount();
        }

        private static List<ProductType> LoadListFromReader(IDataReader reader)
        {
            List<ProductType> productTypeList = new List<ProductType>();
            try
            {
                while (reader.Read())
                {
                    ProductType productType = new ProductType
                    {
                        productTypeId = Convert.ToInt32(reader["ProductTypeId"]),
                        itemGuid = new Guid(reader["ItemGuid"].ToString()),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        name = reader["Name"].ToString(),
                        description = reader["Description"].ToString(),
                        primaryImage = reader["PrimaryImage"].ToString(),
                        secondImage = reader["SecondImage"].ToString()
                    };
                    productTypeList.Add(productType);
                }
            }
            finally
            {
                reader.Close();
            }

            return productTypeList;
        }

        /// <summary>
        /// Gets an IList with all instances of ProductType.
        /// </summary>
        public static List<ProductType> GetAll()
        {
            IDataReader reader = DBProductType.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of ProductType.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<ProductType> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBProductType.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
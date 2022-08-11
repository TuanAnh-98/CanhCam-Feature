/// Created:				2017-09-20
/// Last Modified:			2017-09-20

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class ProductVariant
    {
        #region Constructors

        public ProductVariant()
        { }

        public ProductVariant(int productVariantID)
        {
            this.GetProductVariant(productVariantID);
        }

        #endregion Constructors

        #region Private Properties

        private int productVariantID = -1;
        private int productID = -1;
        private string code = string.Empty;
        private string name = string.Empty;
        private int displayOrder = 0;
        private decimal price;
        private int stockQuantity = 0;
        private int soldQuantity = 0;
        private DateTime? availableDate = null;
        private string attributesXml = string.Empty;
        private Guid guid = Guid.Empty;
        private bool isDeleted = false;
        private DateTime createdOn = DateTime.Now;
        private string photo = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int ProductVariantId
        {
            get { return productVariantID; }
            set { productVariantID = value; }
        }

        public int ProductId
        {
            get { return productID; }
            set { productID = value; }
        }

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
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

        public int StockQuantity
        {
            get { return stockQuantity; }
            set { stockQuantity = value; }
        }

        public int SoldQuantity
        {
            get { return soldQuantity; }
            set { soldQuantity = value; }
        }

        public DateTime? AvailableDate
        {
            get { return availableDate; }
            set { availableDate = value; }
        }

        public string AttributesXml
        {
            get { return attributesXml; }
            set { attributesXml = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
            set { createdOn = value; }
        }

        public string Photo
        {
            get { return photo; }
            set { photo = value; }
        }

        #endregion Public Properties

        #region Private Methods

        private void GetProductVariant(int productVariantID)
        {
            using (IDataReader reader = DBProductVariant.GetOne(productVariantID))
            {
                if (reader.Read())
                {
                    this.productVariantID = Convert.ToInt32(reader["ProductVariantID"]);
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.code = reader["Code"].ToString();
                    this.name = reader["Name"].ToString();
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.price = Convert.ToDecimal(reader["Price"]);
                    this.stockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                    this.soldQuantity = Convert.ToInt32(reader["SoldQuantity"]);
                    if (reader["AvailableDate"] != DBNull.Value)
                        this.availableDate = Convert.ToDateTime(reader["AvailableDate"]);
                    this.attributesXml = reader["AttributesXml"].ToString();
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    this.createdOn = Convert.ToDateTime(reader["CreatedOn"]);
                }
            }
        }

        private bool Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();

            newID = DBProductVariant.Create(
                this.productID,
                this.code,
                this.name,
                this.displayOrder,
                this.price,
                this.stockQuantity,
                this.soldQuantity,
                this.availableDate,
                this.attributesXml,
                this.guid,
                this.isDeleted,
                this.createdOn);

            this.productVariantID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of ProductVariant. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBProductVariant.Update(
                this.productVariantID,
                this.productID,
                this.code,
                this.name,
                this.displayOrder,
                this.price,
                this.stockQuantity,
                this.soldQuantity,
                this.availableDate,
                this.attributesXml,
                this.guid,
                this.isDeleted,
                this.createdOn);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.productVariantID > 0)
                return this.Update();

            return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int productVariantID)
        {
            return DBProductVariant.Delete(productVariantID);
        }

        private static List<ProductVariant> LoadListFromReader(IDataReader reader)
        {
            List<ProductVariant> productVariantList = new List<ProductVariant>();
            try
            {
                while (reader.Read())
                {
                    ProductVariant productVariant = new ProductVariant
                    {
                        productVariantID = Convert.ToInt32(reader["ProductVariantID"]),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        code = reader["Code"].ToString(),
                        name = reader["Name"].ToString(),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        price = Convert.ToDecimal(reader["Price"]),
                        stockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        soldQuantity = Convert.ToInt32(reader["SoldQuantity"])
                    };
                    if (reader["AvailableDate"] != DBNull.Value)
                        productVariant.availableDate = Convert.ToDateTime(reader["AvailableDate"]);
                    productVariant.attributesXml = reader["AttributesXml"].ToString();
                    productVariant.guid = new Guid(reader["Guid"].ToString());
                    productVariant.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    productVariant.createdOn = Convert.ToDateTime(reader["CreatedOn"]);
                    productVariantList.Add(productVariant);
                }
            }
            finally
            {
                reader.Close();
            }

            return productVariantList;
        }

        public static List<ProductVariant> GetByProduct(int productId)
        {
            IDataReader reader = DBProductVariant.GetByProduct(productId);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
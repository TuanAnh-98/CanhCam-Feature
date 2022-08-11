// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
// Created:					2014-07-26
// Last Modified:			2014-07-26

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class ProductProperty
    {
        #region Constructors

        public ProductProperty()
        { }

        public ProductProperty(Guid guid)
        {
            this.GetProductProperty(guid);
        }

        #endregion Constructors

        #region Private Properties

        private Guid guid = Guid.Empty;
        private int productID = -1;
        private int customFieldID = -1;
        private int customFieldOptionID = -1;
        private string customValue = string.Empty;
        private int stockQuantity = 0;
        private decimal overriddenPrice;
        private string optionName = string.Empty;
        private string optionColor = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int ProductId
        {
            get { return productID; }
            set { productID = value; }
        }

        public int CustomFieldId
        {
            get { return customFieldID; }
            set { customFieldID = value; }
        }

        public int CustomFieldOptionId
        {
            get { return customFieldOptionID; }
            set { customFieldOptionID = value; }
        }

        public string CustomValue
        {
            get { return customValue; }
            set { customValue = value; }
        }

        public int StockQuantity
        {
            get { return stockQuantity; }
            set { stockQuantity = value; }
        }

        public decimal OverriddenPrice
        {
            get { return overriddenPrice; }
            set { overriddenPrice = value; }
        }

        public string OptionName
        {
            get { return optionName; }
            set { optionName = value; }
        }

        public string OptionColor
        {
            get { return optionColor; }
            set { optionColor = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of ProductProperty.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetProductProperty(
            Guid guid)
        {
            using (IDataReader reader = DBProductProperty.GetOne(guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.customFieldID = Convert.ToInt32(reader["CustomFieldID"]);
                    this.customFieldOptionID = Convert.ToInt32(reader["CustomFieldOptionID"]);
                    this.customValue = reader["CustomValue"].ToString();
                    this.stockQuantity = Convert.ToInt32(reader["StockQuantity"]);
                    this.overriddenPrice = Convert.ToDecimal(reader["OverriddenPrice"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of ProductProperty. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBProductProperty.Create(
                this.guid,
                this.productID,
                this.customFieldID,
                this.customFieldOptionID,
                this.customValue,
                this.stockQuantity,
                this.overriddenPrice);

            return (rowsAffected > 0);
        }

        private bool Update()
        {
            return DBProductProperty.Update(
                this.guid,
                this.productID,
                this.customFieldID,
                this.customFieldOptionID,
                this.customValue,
                this.stockQuantity,
                this.overriddenPrice);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.guid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        #endregion Public Methods

        #region Static Methods

        public static bool DeleteByProduct(int productId)
        {
            return DBProductProperty.DeleteByProduct(productId);
        }

        public static bool DeleteByCustomField(int customFieldId)
        {
            return DBProductProperty.DeleteByCustomField(customFieldId);
        }

        public static bool DeleteByCustomFieldOption(int customFieldOptionId)
        {
            return DBProductProperty.DeleteByCustomFieldOption(customFieldOptionId);
        }

        private static List<ProductProperty> LoadListFromReader(IDataReader reader, bool loadOption = false)
        {
            List<ProductProperty> productPropertieList = new List<ProductProperty>();
            try
            {
                while (reader.Read())
                {
                    ProductProperty productProperty = new ProductProperty
                    {
                        guid = new Guid(reader["Guid"].ToString()),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        customFieldID = Convert.ToInt32(reader["CustomFieldID"]),
                        customFieldOptionID = Convert.ToInt32(reader["CustomFieldOptionID"]),
                        customValue = reader["CustomValue"].ToString(),
                        stockQuantity = Convert.ToInt32(reader["StockQuantity"]),
                        overriddenPrice = Convert.ToDecimal(reader["OverriddenPrice"])
                    };

                    if (loadOption)
                    {
                        if (reader["OptionName"] != DBNull.Value)
                            productProperty.optionName = reader["OptionName"].ToString();
                        //TODO: not yet implemented
                        if (reader["OptionColor"] != DBNull.Value)
                            productProperty.optionColor = reader["OptionColor"].ToString();
                    }

                    productPropertieList.Add(productProperty);
                }
            }
            finally
            {
                reader.Close();
            }

            return productPropertieList;
        }

        public static List<ProductProperty> GetPropertiesByProduct(int productId)
        {
            return GetPropertiesByProduct(productId, -1);
        }

        public static List<ProductProperty> GetPropertiesByProduct(int productId, int languageId)
        {
            IDataReader reader = DBProductProperty.GetPropertiesByProduct(productId, languageId);
            return LoadListFromReader(reader, true);
        }

        public static List<ProductProperty> GetPropertiesByProducts(List<int> productIds)
        {
            return GetPropertiesByProducts(productIds, -1);
        }

        public static List<ProductProperty> GetPropertiesByProducts(List<int> productIds, int languageId)
        {
            if (productIds.Count > 0)
                return LoadListFromReader(DBProductProperty.GetPropertiesByProducts(string.Join(";", productIds.ToArray()), languageId), true);

            return new List<ProductProperty>();
        }

        public static List<ProductProperty> GetPropertiesByField(int customFieldId)
        {
            IDataReader reader = DBProductProperty.GetPropertiesByField(customFieldId);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
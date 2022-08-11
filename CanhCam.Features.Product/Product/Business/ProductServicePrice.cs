
// Author:					Canh cam
// Created:					2021-2-22
// Last Modified:			2021-2-22

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

    public enum ProductServiceType
    {
        Installation = 0
    }

    public class ProductServicePrice
    {

        #region Constructors

        public ProductServicePrice()
        { }


        public ProductServicePrice(
            Guid itemGuid)
        {
            this.GetProductServicePrice(
                itemGuid);
        }

        #endregion

        #region Private Properties

        private Guid itemGuid = Guid.Empty;
        private int productId = -1;
        private int type = -1;
        private decimal price;
        private decimal maxPrice;
        private decimal feeForOrderTotal;
        private bool freeForPaymentOnline = false;

        #endregion

        #region Public Properties

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }
        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }
        public int Type
        {
            get { return type; }
            set { type = value; }
        }
        public decimal Price
        {
            get { return price; }
            set { price = value; }
        }
        public decimal MaxPrice
        {
            get { return maxPrice; }
            set { maxPrice = value; }
        }
        public decimal FeeForOrderTotal
        {
            get { return feeForOrderTotal; }
            set { feeForOrderTotal = value; }
        }
        public bool FreeForPaymentOnline
        {
            get { return freeForPaymentOnline; }
            set { freeForPaymentOnline = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ProductServicePrice.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        private void GetProductServicePrice(
            Guid itemGuid)
        {
            using (IDataReader reader = DBProductServicePrice.GetOne(
                itemGuid))
            {
                if (reader.Read())
                {
                    this.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    this.productId = Convert.ToInt32(reader["ProductId"]);
                    this.type = Convert.ToInt32(reader["Type"]);
                    this.price = Convert.ToDecimal(reader["Price"]);
                    this.maxPrice = Convert.ToDecimal(reader["MaxPrice"]);
                    this.feeForOrderTotal = Convert.ToDecimal(reader["FeeForOrderTotal"]);
                    this.freeForPaymentOnline = Convert.ToBoolean(reader["FreeForPaymentOnline"]);
                }
            }

        }

        /// <summary>
        /// Persists a new instance of ProductServicePrice. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.itemGuid = Guid.NewGuid();

            int rowsAffected = DBProductServicePrice.Create(
                this.itemGuid,
                this.productId,
                this.type,
                this.price,
                this.maxPrice,
                this.feeForOrderTotal,
                this.freeForPaymentOnline);

            return (rowsAffected > 0);

        }


        /// <summary>
        /// Updates this instance of ProductServicePrice. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBProductServicePrice.Update(
                this.itemGuid,
                this.productId,
                this.type,
                this.price,
                this.maxPrice,
                this.feeForOrderTotal,
                this.freeForPaymentOnline);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ProductServicePrice. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.itemGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of ProductServicePrice. Returns true on success.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid itemGuid)
        {
            return DBProductServicePrice.Delete(
                itemGuid);
        }


        /// <summary>
        /// Gets a count of ProductServicePrice. 
        /// </summary>
        public static int GetCount()
        {
            return DBProductServicePrice.GetCount();
        }

        private static List<ProductServicePrice> LoadListFromReader(IDataReader reader)
        {
            List<ProductServicePrice> productServicePriceList = new List<ProductServicePrice>();
            try
            {
                while (reader.Read())
                {
                    ProductServicePrice productServicePrice = new ProductServicePrice
                    {
                        itemGuid = new Guid(reader["ItemGuid"].ToString()),
                        productId = Convert.ToInt32(reader["ProductId"]),
                        type = Convert.ToInt32(reader["Type"]),
                        price = Convert.ToDecimal(reader["Price"]),
                        maxPrice = Convert.ToDecimal(reader["MaxPrice"]),
                        feeForOrderTotal = Convert.ToDecimal(reader["FeeForOrderTotal"]),
                        freeForPaymentOnline = Convert.ToBoolean(reader["FreeForPaymentOnline"])
                    };
                    productServicePriceList.Add(productServicePrice);

                }
            }
            finally
            {
                reader.Close();
            }

            return productServicePriceList;

        }

        /// <summary>
        /// Gets an IList with all instances of ProductServicePrice.
        /// </summary>
        public static List<ProductServicePrice> GetAll()
        {
            IDataReader reader = DBProductServicePrice.GetAll();
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of instances of ProductServicePrice.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<ProductServicePrice> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBProductServicePrice.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }


        public static List<ProductServicePrice> GetByProductId(int productId)
        {
            IDataReader reader = DBProductServicePrice.GetByProductId(productId);
            return LoadListFromReader(reader);
        }

        public static List<ProductServicePrice> GetByProductIds(string productIds)
        {
            IDataReader reader = DBProductServicePrice.GetByProductIds(productIds);
            return LoadListFromReader(reader);
        }
        #endregion


    }

}

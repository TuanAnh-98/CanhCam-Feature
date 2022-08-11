// Author:					Canh cam
// Created:					2020-11-5
// Last Modified:			2020-11-5

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class ProductZone
    {
        #region Constructors

        public ProductZone()
        { }

        public ProductZone(
            int productID,
            int zoneID)
        {
            this.GetProductZone(
                productID,
                zoneID);
        }

        #endregion Constructors

        #region Private Properties

        private int productID = -1;
        private int zoneID = -1;

        #endregion Private Properties

        #region Public Properties

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public int ZoneID
        {
            get { return zoneID; }
            set { zoneID = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of ProductZone.
        /// </summary>
        /// <param name="productID"> productID </param>
        /// <param name="zoneID"> zoneID </param>
        private void GetProductZone(
            int productID,
            int zoneID)
        {
            using (IDataReader reader = DBProductZone.GetOne(
                productID,
                zoneID))
            {
                if (reader.Read())
                {
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.zoneID = Convert.ToInt32(reader["ZoneID"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of ProductZone. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBProductZone.Create(
                    this.productID,
                    this.zoneID);

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of ProductZone. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBProductZone.Update(
                this.productID,
                this.zoneID);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of ProductZone. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int productID, int zoneID)
        {
            return DBProductZone.Delete(productID, zoneID);
        }

        public static bool DeleteByProduct(int productID)
        {
            return DBProductZone.DeleteByProduct(productID);
        }

        private static List<ProductZone> LoadListFromReader(IDataReader reader)
        {
            List<ProductZone> productZoneList = new List<ProductZone>();
            try
            {
                while (reader.Read())
                {
                    ProductZone productZone = new ProductZone
                    {
                        productID = Convert.ToInt32(reader["ProductID"]),
                        zoneID = Convert.ToInt32(reader["ZoneID"])
                    };
                    productZoneList.Add(productZone);
                }
            }
            finally
            {
                reader.Close();
            }

            return productZoneList;
        }

        public static List<ProductZone> SelectAllByProductID(int productId)
        {
            IDataReader reader = DBProductZone.SelectAllByProductID(productId);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
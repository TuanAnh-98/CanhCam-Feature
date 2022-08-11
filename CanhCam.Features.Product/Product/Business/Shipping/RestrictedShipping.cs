// Author:					Canh cam
// Created:					2021-3-11
// Last Modified:			2021-3-11

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class RestrictedShipping
    {
        #region Constructors

        public RestrictedShipping()
        { }

        public RestrictedShipping(
            int rowId)
        {
            this.GetRestrictedShipping(
                rowId);
        }

        public RestrictedShipping(
            Guid geoZoneGuid, int storeId)
        {
            this.GetRestrictedShipping(
                geoZoneGuid, storeId);
        }

        public RestrictedShipping(
            Guid geoZoneGuid, int storeId, decimal orderWeight)
        {
            this.GetRestrictedShipping(
                geoZoneGuid, storeId, orderWeight);
        }

        public RestrictedShipping(
            string geoZoneGuids, int storeId, decimal orderWeight)
        {
            this.GetRestrictedShipping(geoZoneGuids, storeId, orderWeight);
        }

        #endregion Constructors

        #region Private Properties

        private int rowId = -1;
        private Guid geoZoneGuid = Guid.Empty;
        private int weight = -1;
        private decimal orderTotal;
        private string shippingMethodIds = string.Empty;
        private string paymentMethodIds = string.Empty;
        private int storeId = -1;

        private string geoZoneName = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        public Guid GeoZoneGuid
        {
            get { return geoZoneGuid; }
            set { geoZoneGuid = value; }
        }

        public int Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public decimal OrderTotal
        {
            get { return orderTotal; }
            set { orderTotal = value; }
        }

        public string ShippingMethodIds
        {
            get { return shippingMethodIds; }
            set { shippingMethodIds = value; }
        }

        public string PaymentMethodIds
        {
            get { return paymentMethodIds; }
            set { paymentMethodIds = value; }
        }

        public int StoreId
        {
            get { return storeId; }
            set { storeId = value; }
        }

        public string GeoZoneName
        {
            get { return geoZoneName; }
            set { geoZoneName = value; }
        }

        #endregion Public Properties

        #region Private Methods

        public static void PopulateRestrictedShipping(RestrictedShipping restricted, IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    restricted.rowId = Convert.ToInt32(reader["RowId"]);
                    restricted.geoZoneGuid = new Guid(reader["GeoZoneGuid"].ToString());
                    restricted.weight = Convert.ToInt32(reader["Weight"]);
                    restricted.orderTotal = Convert.ToDecimal(reader["OrderTotal"]);
                    restricted.shippingMethodIds = reader["ShippingMethodIds"].ToString();
                    restricted.paymentMethodIds = reader["PaymentMethodIds"].ToString();
                    restricted.storeId = Convert.ToInt32(reader["StoreId"]);
                }
            }
            finally
            {
                reader.Close();
            }
        }

        private void GetRestrictedShipping(Guid geoZoneGuid, int storeId, decimal orderWeight)
        {
            PopulateRestrictedShipping(this, DBRestrictedShipping.GetOne(geoZoneGuid, storeId, orderWeight));
        }

        private void GetRestrictedShipping(string geoZoneGuids, int storeId, decimal orderWeight)
        {
            PopulateRestrictedShipping(this, DBRestrictedShipping.GetOne(geoZoneGuids, storeId, orderWeight));
        }

        private void GetRestrictedShipping(Guid geoZoneGuid, int storeId)
        {
            PopulateRestrictedShipping(this, DBRestrictedShipping.GetOne(geoZoneGuid, storeId));
        }

        /// <summary>
        /// Gets an instance of RestrictedShipping.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        private void GetRestrictedShipping(
            int rowId)
        {
            PopulateRestrictedShipping(this, DBRestrictedShipping.GetOne(rowId));
        }

        /// <summary>
        /// Persists a new instance of RestrictedShipping. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBRestrictedShipping.Create(
                this.geoZoneGuid,
                this.weight,
                this.orderTotal,
                this.shippingMethodIds,
                this.paymentMethodIds,
                this.storeId);

            this.rowId = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of RestrictedShipping. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBRestrictedShipping.Update(
                this.rowId,
                this.geoZoneGuid,
                this.weight,
                this.orderTotal,
                this.shippingMethodIds,
                this.paymentMethodIds,
                this.storeId);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of RestrictedShipping. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowId > 0)
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
        /// Deletes an instance of RestrictedShipping. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            return DBRestrictedShipping.Delete(
                rowId);
        }

        /// <summary>
        /// Gets a count of RestrictedShipping.
        /// </summary>
        public static int GetCount()
        {
            return DBRestrictedShipping.GetCount();
        }

        private static List<RestrictedShipping> LoadListFromReader(IDataReader reader)
        {
            List<RestrictedShipping> restrictedShippingList = new List<RestrictedShipping>();
            try
            {
                while (reader.Read())
                {
                    RestrictedShipping restrictedShipping = new RestrictedShipping
                    {
                        rowId = Convert.ToInt32(reader["RowId"]),
                        geoZoneGuid = new Guid(reader["GeoZoneGuid"].ToString()),
                        weight = Convert.ToInt32(reader["Weight"]),
                        orderTotal = Convert.ToDecimal(reader["OrderTotal"]),
                        shippingMethodIds = reader["ShippingMethodIds"].ToString(),
                        paymentMethodIds = reader["PaymentMethodIds"].ToString(),
                        storeId = Convert.ToInt32(reader["StoreId"]),
                        geoZoneName = reader["GeoZoneName"].ToString()
                    };
                    restrictedShippingList.Add(restrictedShipping);
                }
            }
            finally
            {
                reader.Close();
            }

            return restrictedShippingList;
        }

        /// <summary>
        /// Gets an IList with all instances of RestrictedShipping.
        /// </summary>
        public static List<RestrictedShipping> GetAll()
        {
            IDataReader reader = DBRestrictedShipping.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of RestrictedShipping.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<RestrictedShipping> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBRestrictedShipping.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
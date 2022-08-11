/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-05-05
/// Last Modified:			2015-05-05

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class ShippingTableRate
    {
        #region Constructors

        public ShippingTableRate()
        { }

        public ShippingTableRate(int shippingTableRateId)
        {
            this.GetShippingTableRate(shippingTableRateId);
        }

        #endregion Constructors

        #region Private Properties

        private int shippingTableRateID = -1;
        private int shippingMethodID = -1;
        private Guid geoZoneGuid = Guid.Empty;
        private decimal fromValue;
        private decimal shippingFee;
        private decimal additionalValue;
        private decimal additionalFee;
        private decimal freeShippingOverXValue;
        private string geoZoneName = string.Empty;
        private bool markAsDeleted = true;
        private int storeId = -1;

        #endregion Private Properties

        #region Public Properties

        public int ShippingTableRateId
        {
            get { return shippingTableRateID; }
            set { shippingTableRateID = value; }
        }

        public int ShippingMethodId
        {
            get { return shippingMethodID; }
            set { shippingMethodID = value; }
        }

        public Guid GeoZoneGuid
        {
            get { return geoZoneGuid; }
            set { geoZoneGuid = value; }
        }

        public decimal FromValue
        {
            get { return fromValue; }
            set { fromValue = value; }
        }

        public decimal ShippingFee
        {
            get { return shippingFee; }
            set { shippingFee = value; }
        }

        public decimal AdditionalValue
        {
            get { return additionalValue; }
            set { additionalValue = value; }
        }

        public decimal AdditionalFee
        {
            get { return additionalFee; }
            set { additionalFee = value; }
        }

        public decimal FreeShippingOverXValue
        {
            get { return freeShippingOverXValue; }
            set { freeShippingOverXValue = value; }
        }

        public string GeoZoneName
        {
            get { return geoZoneName; }
            set { geoZoneName = value; }
        }

        public bool MarkAsDeleted
        {
            get { return markAsDeleted; }
            set { markAsDeleted = value; }
        }

        public int StoreId
        {
            get { return storeId; }
            set { storeId = value; }
        }
        #endregion Public Properties

        #region Private Methods

        private void GetShippingTableRate(int shippingTableRateId)
        {
            using (IDataReader reader = DBShippingTableRate.GetOne(shippingTableRateId))
            {
                if (reader.Read())
                {
                    this.shippingTableRateID = Convert.ToInt32(reader["ShippingTableRateID"]);
                    this.shippingMethodID = Convert.ToInt32(reader["ShippingMethodID"]);
                    this.geoZoneGuid = new Guid(reader["GeoZoneGuid"].ToString());
                    this.fromValue = Convert.ToDecimal(reader["FromValue"]);
                    this.shippingFee = Convert.ToDecimal(reader["ShippingFee"]);
                    this.additionalValue = Convert.ToDecimal(reader["AdditionalValue"]);
                    this.additionalFee = Convert.ToDecimal(reader["AdditionalFee"]);
                    this.freeShippingOverXValue = Convert.ToDecimal(reader["FreeShippingOverXValue"]);
                    this.storeId = Convert.ToInt32(reader["StoreId"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of ShippingTableRate. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBShippingTableRate.Create(
                this.shippingMethodID,
                this.geoZoneGuid,
                this.fromValue,
                this.shippingFee,
                this.additionalValue,
                this.additionalFee,
                this.freeShippingOverXValue,
                this.storeId);

            this.shippingTableRateID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBShippingTableRate.Update(
                this.shippingTableRateID,
                this.shippingMethodID,
                this.geoZoneGuid,
                this.fromValue,
                this.shippingFee,
                this.additionalValue,
                this.additionalFee,
                this.freeShippingOverXValue,
                this.storeId);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.shippingTableRateID > 0)
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

        public static bool Delete(int shippingTableRateId)
        {
            return DBShippingTableRate.Delete(shippingTableRateId);
        }

        public static bool DeleteByMethod(int shippingMethodId)
        {
            return DBShippingTableRate.DeleteByMethod(shippingMethodId);
        }

        private static List<ShippingTableRate> LoadListFromReader(IDataReader reader, bool loadZoneName = false)
        {
            List<ShippingTableRate> shippingTableRateList = new List<ShippingTableRate>();
            try
            {
                while (reader.Read())
                {
                    ShippingTableRate shippingTableRate = new ShippingTableRate
                    {
                        shippingTableRateID = Convert.ToInt32(reader["ShippingTableRateID"]),
                        shippingMethodID = Convert.ToInt32(reader["ShippingMethodID"]),
                        geoZoneGuid = new Guid(reader["GeoZoneGuid"].ToString()),
                        fromValue = Convert.ToDecimal(reader["FromValue"]),
                        shippingFee = Convert.ToDecimal(reader["ShippingFee"]),
                        additionalValue = Convert.ToDecimal(reader["AdditionalValue"]),
                        additionalFee = Convert.ToDecimal(reader["AdditionalFee"]),
                        freeShippingOverXValue = Convert.ToDecimal(reader["FreeShippingOverXValue"]),
                        storeId = Convert.ToInt32(reader["StoreId"])
                    };

                    if (loadZoneName)
                        if (reader["GeoZoneName"] != DBNull.Value)
                            shippingTableRate.geoZoneName = reader["GeoZoneName"].ToString();

                    shippingTableRateList.Add(shippingTableRate);
                }
            }
            finally
            {
                reader.Close();
            }

            return shippingTableRateList;
        }

        public static List<ShippingTableRate> GetByMethod(int shippingMethodId)
        {
            IDataReader reader = DBShippingTableRate.GetByMethod(shippingMethodId);
            return LoadListFromReader(reader, true);
        }

        public static ShippingTableRate GetOneMaxValue(int shippingMethodId, decimal fromValue, string geoZoneGuids,int storeId)
        {
            if (string.IsNullOrEmpty(geoZoneGuids))
                geoZoneGuids = null;

            IDataReader reader = DBShippingTableRate.GetOneMaxValue(shippingMethodId, fromValue, geoZoneGuids, storeId);
            var lstTableRates = LoadListFromReader(reader);
            if (lstTableRates.Count > 0)
                return lstTableRates[0];

            return null;
        }

        #endregion Static Methods
    }
}
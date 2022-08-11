/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-04-24
/// Last Modified:			2015-04-24

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum ShippingMethodProvider
    {
        Free = 0,
        Fixed = 1,
        FixedPerItem = 2,
        ByOrderTotal = 3,
        ByWeight = 4,
        ByGeoZoneAndFixed = 11,
        ByGeoZoneAndOrderTotal = 13,
        ByGeoZoneAndWeight = 14,
        //GiaoHangNhanh = 15,
        VittelPost = 16
    }

    public class ShippingMethod
    {
        #region Constructors

        public ShippingMethod()
        { }

        public ShippingMethod(int shippingMethodId)
        {
            this.GetShippingMethod(shippingMethodId);
        }

        #endregion Constructors

        #region Private Properties

        private int shippingMethodID = -1;
        private int siteID = -1;
        private int shippingProvider = 0;
        private string name = string.Empty;
        private string description = string.Empty;
        private decimal shippingFee;
        private bool freeShippingOverXEnabled = false;
        private decimal freeShippingOverXValue = decimal.Zero;
        private int displayOrder = 0;
        private bool isActive = false;
        private Guid guid = Guid.Empty;
        private bool isDeleted = false;
        #endregion Private Properties


        #region Public Properties

        public int ShippingMethodId
        {
            get { return shippingMethodID; }
            set { shippingMethodID = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public int ShippingProvider
        {
            get { return shippingProvider; }
            set { shippingProvider = value; }
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

        public decimal ShippingFee
        {
            get { return shippingFee; }
            set { shippingFee = value; }
        }

        public bool FreeShippingOverXEnabled
        {
            get { return freeShippingOverXEnabled; }
            set { freeShippingOverXEnabled = value; }
        }

        public decimal FreeShippingOverXValue
        {
            get { return freeShippingOverXValue; }
            set { freeShippingOverXValue = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
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
        #endregion Public Properties

        #region Private Methods

        private void GetShippingMethod(int shippingMethodId)
        {
            using (IDataReader reader = DBShippingMethod.GetOne(shippingMethodId))
            {
                if (reader.Read())
                {
                    this.shippingMethodID = Convert.ToInt32(reader["ShippingMethodID"]);
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.shippingProvider = Convert.ToInt32(reader["ShippingProvider"]);
                    this.name = reader["Name"].ToString();
                    this.description = reader["Description"].ToString();
                    this.shippingFee = Convert.ToDecimal(reader["ShippingFee"]);
                    this.freeShippingOverXEnabled = Convert.ToBoolean(reader["FreeShippingOverXEnabled"]);
                    this.freeShippingOverXValue = Convert.ToDecimal(reader["FreeShippingOverXValue"]);
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.isActive = Convert.ToBoolean(reader["IsActive"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                }
            }
        }

        private bool Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();

            newID = DBShippingMethod.Create(
                this.siteID,
                this.shippingProvider,
                this.name,
                this.description,
                this.shippingFee,
                this.freeShippingOverXEnabled,
                this.freeShippingOverXValue,
                this.displayOrder,
                this.isActive,
                this.guid,
                this.isDeleted);

            this.shippingMethodID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBShippingMethod.Update(
                this.shippingMethodID,
                this.siteID,
                this.shippingProvider,
                this.name,
                this.description,
                this.shippingFee,
                this.freeShippingOverXEnabled,
                this.freeShippingOverXValue,
                this.displayOrder,
                this.isActive,
                this.guid,
                this.isDeleted);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.shippingMethodID > 0)
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

        public static bool Delete(int shippingMethodId)
        {
            return DBShippingMethod.Delete(shippingMethodId);
        }

        private static List<ShippingMethod> LoadListFromReader(IDataReader reader)
        {
            List<ShippingMethod> shippingMethodList = new List<ShippingMethod>();
            try
            {
                while (reader.Read())
                {
                    ShippingMethod shippingMethod = new ShippingMethod
                    {
                        shippingMethodID = Convert.ToInt32(reader["ShippingMethodID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        shippingProvider = Convert.ToInt32(reader["ShippingProvider"]),
                        name = reader["Name"].ToString(),
                        description = reader["Description"].ToString(),
                        shippingFee = Convert.ToDecimal(reader["ShippingFee"]),
                        freeShippingOverXEnabled = Convert.ToBoolean(reader["FreeShippingOverXEnabled"]),
                        freeShippingOverXValue = Convert.ToDecimal(reader["FreeShippingOverXValue"]),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        isActive = Convert.ToBoolean(reader["IsActive"]),
                        guid = new Guid(reader["Guid"].ToString()),
                        isDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                    };
                    shippingMethodList.Add(shippingMethod);
                }
            }
            finally
            {
                reader.Close();
            }

            return shippingMethodList;
        }

        public static List<ShippingMethod> GetByActive(int siteId, int activeStatus)
        {
            return GetByActive(siteId, activeStatus, -1);
        }

        public static List<ShippingMethod> GetByActive(int siteId, int activeStatus, int languageId)
        {
            IDataReader reader = DBShippingMethod.GetByActive(siteId, activeStatus, languageId);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
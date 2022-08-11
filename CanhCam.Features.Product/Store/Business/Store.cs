// Author:					t
// Created:					2020-5-4
// Last Modified:			2020-5-4

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum StoreOptions
    {
        NoOptions = 0,
        OnlineOrder = 1
    }

    public class Store
    {
        #region Constructors

        public Store()
        { }

        public Store(
            int storeID)
        {
            this.GetStore(
                storeID);
        }

        public Store(
            string code)
        {
            this.GetStore(code);
        }

        #endregion Constructors

        #region Private Properties

        private int storeID = -1;
        private int siteID = -1;
        private string name = string.Empty;
        private string code = string.Empty;
        private int displayOrder = -1;
        private int options = -1;
        private string address = string.Empty;
        private string phone = string.Empty;
        private string fax = string.Empty;
        private string email = string.Empty;
        private string contactPerson = string.Empty;
        private string map = string.Empty;
        private string description = string.Empty;
        private bool isPublished = false;
        private string latitude = string.Empty;
        private string longitude = string.Empty;
        private string provinceGuids = string.Empty;
        private string districtGuids = string.Empty;
        private int priority = 1;
        private Guid guid = Guid.Empty;
        private string aPIParams = string.Empty;
        private string orderUserIDs = string.Empty;
        private DateTime createdDate = DateTime.Now;
        private bool isDeleted = false;
        private string aPIDistrictCode = string.Empty;
        private Guid provinceGuid = Guid.Empty;
        private Guid districtGuid = Guid.Empty;
        private string aPIEndpoint = string.Empty;
        private Guid managingArea = Guid.Empty;

        #endregion Private Properties

        #region Public Properties

        public int StoreID
        {
            get { return storeID; }
            set { storeID = value; }
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

        public string Code
        {
            get { return code; }
            set { code = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public int Options
        {
            get { return options; }
            set { options = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string ContactPerson
        {
            get { return contactPerson; }
            set { contactPerson = value; }
        }

        public string Map
        {
            get { return map; }
            set { map = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public bool IsPublished
        {
            get { return isPublished; }
            set { isPublished = value; }
        }

        public string Latitude
        {
            get { return latitude; }
            set { latitude = value; }
        }

        public string Longitude
        {
            get { return longitude; }
            set { longitude = value; }
        }

        public string ProvinceGuids
        {
            get { return provinceGuids; }
            set { provinceGuids = value; }
        }

        public string DistrictGuids
        {
            get { return districtGuids; }
            set { districtGuids = value; }
        }

        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string APIParams
        {
            get { return aPIParams; }
            set { aPIParams = value; }
        }

        public string OrderUserIDs
        {
            get { return orderUserIDs; }
            set { orderUserIDs = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public string APIDistrictCode
        {
            get { return aPIDistrictCode; }
            set { aPIDistrictCode = value; }
        }

        public Guid ProvinceGuid
        {
            get { return provinceGuid; }
            set { provinceGuid = value; }
        }

        public Guid DistrictGuid
        {
            get { return districtGuid; }
            set { districtGuid = value; }
        }

        public string APIEndpoint
        {
            get { return aPIEndpoint; }
            set { aPIEndpoint = value; }
        }

        public Guid ManagingArea
        {
            get { return managingArea; }
            set { managingArea = value; }
        }

        #endregion Public Properties

        #region Private Methods

        private void GetStore(
            int storeID)
        {
            PopulateStore(this, DBStore.GetOne(storeID));
        }

        private void GetStore(
            string code)
        {
            PopulateStore(this, DBStore.GetOne(code));
        }

        public static void PopulateStore(Store store, IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    store.storeID = Convert.ToInt32(reader["StoreID"]);
                    store.siteID = Convert.ToInt32(reader["SiteID"]);
                    store.name = reader["Name"].ToString();
                    store.code = reader["Code"].ToString();
                    store.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    store.options = Convert.ToInt32(reader["Options"]);
                    store.address = reader["Address"].ToString();
                    store.phone = reader["Phone"].ToString();
                    store.fax = reader["Fax"].ToString();
                    store.email = reader["Email"].ToString();
                    store.contactPerson = reader["ContactPerson"].ToString();
                    store.map = reader["Map"].ToString();
                    store.description = reader["Description"].ToString();
                    store.isPublished = Convert.ToBoolean(reader["IsPublished"]);
                    store.latitude = reader["Latitude"].ToString();
                    store.longitude = reader["Longitude"].ToString();
                    store.provinceGuids = reader["ProvinceGuids"].ToString();
                    store.districtGuids = reader["DistrictGuids"].ToString();
                    store.priority = Convert.ToInt32(reader["Priority"]);
                    store.guid = new Guid(reader["Guid"].ToString());
                    store.aPIParams = reader["APIParams"].ToString();
                    store.orderUserIDs = reader["OrderUserIDs"].ToString();
                    store.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    store.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    store.aPIDistrictCode = reader["APIDistrictCode"].ToString();
                    store.provinceGuid = new Guid(reader["ProvinceGuid"].ToString());
                    store.districtGuid = new Guid(reader["DistrictGuid"].ToString());
                    if (reader["APIEndpoint"] != DBNull.Value)
                    {
                        store.aPIEndpoint = reader["APIEndpoint"].ToString();
                    }
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

            newID = DBStore.Create(
                this.siteID,
                this.name,
                this.code,
                this.displayOrder,
                this.options,
                this.address,
                this.phone,
                this.fax,
                this.email,
                this.contactPerson,
                this.map,
                this.description,
                this.isPublished,
                this.latitude,
                this.longitude,
                this.provinceGuids,
                this.districtGuids,
                this.priority,
                this.guid,
                this.aPIParams,
                this.orderUserIDs,
                this.createdDate,
                this.isDeleted,
                this.aPIDistrictCode,
                this.provinceGuid,
                this.districtGuid,
                this.aPIEndpoint);

            this.storeID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBStore.Update(
                this.storeID,
                this.siteID,
                this.name,
                this.code,
                this.displayOrder,
                this.options,
                this.address,
                this.phone,
                this.fax,
                this.email,
                this.contactPerson,
                this.map,
                this.description,
                this.isPublished,
                this.latitude,
                this.longitude,
                this.provinceGuids,
                this.districtGuids,
                this.priority,
                this.guid,
                this.aPIParams,
                this.orderUserIDs,
                this.createdDate,
                this.isDeleted,
                this.aPIDistrictCode,
                this.provinceGuid,
                this.districtGuid,
                this.aPIEndpoint);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of Store. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.storeID > 0)
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

        public static bool Delete(
            int storeID)
        {
            return DBStore.Delete(
                storeID);
        }

        public static int GetCount()
        {
            return DBStore.GetCount();
        }

        private static List<Store> LoadListFromReader(IDataReader reader)
        {
            List<Store> storeList = new List<Store>();
            try
            {
                while (reader.Read())
                {
                    Store store = new Store
                    {
                        storeID = Convert.ToInt32(reader["StoreID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        name = reader["Name"].ToString(),
                        code = reader["Code"].ToString(),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        options = Convert.ToInt32(reader["Options"]),
                        address = reader["Address"].ToString(),
                        phone = reader["Phone"].ToString(),
                        fax = reader["Fax"].ToString(),
                        email = reader["Email"].ToString(),
                        contactPerson = reader["ContactPerson"].ToString(),
                        map = reader["Map"].ToString(),
                        description = reader["Description"].ToString(),
                        isPublished = Convert.ToBoolean(reader["IsPublished"]),
                        latitude = reader["Latitude"].ToString(),
                        longitude = reader["Longitude"].ToString(),
                        provinceGuids = reader["ProvinceGuids"].ToString(),
                        districtGuids = reader["DistrictGuids"].ToString(),
                        priority = Convert.ToInt32(reader["Priority"]),
                        guid = new Guid(reader["Guid"].ToString()),
                        aPIParams = reader["APIParams"].ToString(),
                        orderUserIDs = reader["OrderUserIDs"].ToString(),
                        createdDate = Convert.ToDateTime(reader["CreatedDate"]),
                        isDeleted = Convert.ToBoolean(reader["IsDeleted"]),
                        aPIDistrictCode = reader["APIDistrictCode"].ToString(),
                        provinceGuid = new Guid(reader["ProvinceGuid"].ToString()),
                        districtGuid = new Guid(reader["DistrictGuid"].ToString())
                    };
                    if (reader["APIEndpoint"] != DBNull.Value)
                    {
                        store.aPIEndpoint = reader["APIEndpoint"].ToString();
                    }
                    storeList.Add(store);
                }
            }
            finally
            {
                reader.Close();
            }

            return storeList;
        }

        private static List<int> LoadListIntFromReader(IDataReader reader)
        {
            List<int> lstStoreIDs = new List<int>();
            try
            {
                while (reader.Read())
                {
                    int storeID = Convert.ToInt32(reader["StoreID"]);
                    lstStoreIDs.Add(storeID);
                }
            }
            finally
            {
                reader.Close();
            }

            return lstStoreIDs;
        }

        public static List<Store> GetAll()
        {
            IDataReader reader = DBStore.GetAll();
            return LoadListFromReader(reader);
        }

        public static List<Store> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBStore.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static int GetCountBySearch(
            int userID = -1,
            bool isAdmin = false,
            int siteId = -1,
            string keyword = "",
            Guid? provinceGuid = null,
            Guid? districtGuid = null)
        {
            return DBStore.GetCountBySearch(userID, isAdmin, siteId, keyword, provinceGuid, districtGuid);
        }

        public static List<Store> GetPageBySearch(
            int userID = -1,
            bool isAdmin = false,
            int siteId = -1,
            string keyword = "",
            int pageNumber = 1,
            int pageSize = 999999,
            Guid? provinceGuid = null,
            Guid? districtGuid = null)
        {
            return LoadListFromReader(DBStore.GetPageBySearch(userID, isAdmin, siteId, keyword, pageNumber, pageSize, provinceGuid, districtGuid));
        }

        public static List<int> GetAllStoreIDs(
            )
        {
            return LoadListIntFromReader(DBStore.GetAllStoreIDs());
        }

        public static Store GetDefaultStore(
            )
        {
            var lst = LoadListFromReader(DBStore.GetDefaultStore());
            if (lst.Count > 0)
                return lst[0];
            return null;
        }

        #endregion Static Methods
    }
}
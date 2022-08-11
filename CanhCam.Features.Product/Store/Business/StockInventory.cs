// Author:					t
// Created:					2020-5-8
// Last Modified:			2020-5-8

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace CanhCam.Business
{
    public class StockInventory
    {
        #region Constructors

        public StockInventory()
        { }

        public StockInventory(
            int inventoryID)
        {
            this.GetStockInventory(
                inventoryID);
        }

        #endregion Constructors

        #region Private Properties

        private int inventoryID = -1;
        private int productID = -1;
        private int storeID = -1;
        private int price = 0;
        private int quantity = 0;
        private Guid guid = Guid.Empty;
        private bool isPublished = true;
        private bool isDeleted = false;
        private string apiProductID = string.Empty;

        private string productURL = string.Empty;
        private int productZoneID = -1;
        private string productName = string.Empty;

        private string imageFile = string.Empty;
        private string thumbnailFile = string.Empty;
        private int parentID = 0;

        #endregion Private Properties

        #region Public Properties

        public int InventoryID
        {
            get { return inventoryID; }
            set { inventoryID = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public int StoreID
        {
            get { return storeID; }
            set { storeID = value; }
        }

        public int Price
        {
            get { return price; }
            set { price = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public bool IsPublished
        {
            get { return isPublished; }
            set { isPublished = value; }
        }

        public bool IsDeleted
        {
            get { return isDeleted; }
            set { isDeleted = value; }
        }

        public string ApiProductID
        {
            get { return apiProductID; }
            set { apiProductID = value; }
        }

        public string ProductURL
        {
            get { return productURL; }
            set { productURL = value; }
        }

        public int ProductZoneID
        {
            get { return productZoneID; }
            set { productZoneID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public string ImageFile
        {
            get { return imageFile; }
            set { imageFile = value; }
        }

        public string ThumbnailFile
        {
            get { return thumbnailFile; }
            set { thumbnailFile = value; }
        }

        public int ParentID
        {
            get { return parentID; }
            set { parentID = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of StockInventory.
        /// </summary>
        /// <param name="inventoryID"> inventoryID </param>
        private void GetStockInventory(
            int inventoryID, bool loadImage = false, bool getParent = false)
        {
            using (IDataReader reader = DBStockInventory.GetOne(
                inventoryID))
            {
                if (reader.Read())
                {
                    this.inventoryID = Convert.ToInt32(reader["InventoryID"]);
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.storeID = Convert.ToInt32(reader["StoreID"]);
                    this.price = Convert.ToInt32(reader["Price"]);
                    this.quantity = Convert.ToInt32(reader["Quantity"]);
                    if (reader["Guid"] != DBNull.Value)
                        this.guid = new Guid(reader["Guid"].ToString());
                    if (reader["isPublished"] != DBNull.Value)
                        this.isPublished = Convert.ToBoolean(reader["IsPublished"]);
                    if (reader["IsDeleted"] != DBNull.Value)
                        this.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    if (reader["ApiProductID"] != DBNull.Value)
                        this.apiProductID = reader["ApiProductID"].ToString();

                    if (loadImage)
                    {
                        if (reader["ImageFile"] != DBNull.Value)
                        {
                            this.ImageFile = reader["ImageFile"].ToString();
                        }
                        if (reader["ThumbnailFile"] != DBNull.Value)
                        {
                            this.ThumbnailFile = reader["ThumbnailFile"].ToString();
                        }
                    }
                    if (getParent)
                    {
                        if (reader["ParentID"] != DBNull.Value)
                        {
                            this.ParentID = Convert.ToInt32(reader["ParentID"]);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Persists a new instance of StockInventory. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();
            newID = DBStockInventory.Create(
                this.productID,
                this.storeID,
                this.price,
                this.quantity,
                this.guid,
                this.isPublished,
                this.isDeleted,
                this.apiProductID);

            this.inventoryID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of StockInventory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBStockInventory.Update(
                this.inventoryID,
                this.productID,
                this.storeID,
                this.price,
                this.quantity,
                this.guid,
                this.isPublished,
                this.isDeleted,
                this.apiProductID);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of StockInventory. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>

        public bool Save()
        {
            if (this.inventoryID > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Create();
            }
        }

        public bool Save(int siteID)
        {
            bool save = false;
            if (this.inventoryID > 0)
            {
                save = this.Update();
            }
            else
            {
                save = this.Create();
            }

            Web.StoreUI.StoreHelper.UpdateProductTotalQuantity(siteID, this.productID);
            return save;
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(
            int inventoryID)
        {
            return DBStockInventory.Delete(
                inventoryID);
        }

        private static List<StockInventory> LoadListFromReader(IDataReader reader, bool loadImage = false, bool getParent = false, bool getMoreInfo = false)
        {
            List<StockInventory> stockInventoryList = new List<StockInventory>();
            try
            {
                while (reader.Read())
                {
                    StockInventory stockInventory = new StockInventory();
                    if (reader["InventoryID"] != DBNull.Value)
                        stockInventory.inventoryID = Convert.ToInt32(reader["InventoryID"]);
                    stockInventory.productID = Convert.ToInt32(reader["ProductID"]);
                    if (reader["StoreID"] != DBNull.Value)
                        stockInventory.storeID = Convert.ToInt32(reader["StoreID"]);
                    if (reader["Price"] != DBNull.Value)
                        stockInventory.price = Convert.ToInt32(reader["Price"]);
                    if (reader["Quantity"] != DBNull.Value)
                        stockInventory.quantity = Convert.ToInt32(reader["Quantity"]);
                    if (reader["Guid"] != DBNull.Value)
                        stockInventory.guid = new Guid(reader["Guid"].ToString());
                    if (reader["isPublished"] != DBNull.Value)
                        stockInventory.isPublished = Convert.ToBoolean(reader["IsPublished"]);
                    if (reader["IsDeleted"] != DBNull.Value)
                        stockInventory.isDeleted = Convert.ToBoolean(reader["IsDeleted"]);
                    if (reader["ApiProductID"] != DBNull.Value)
                        stockInventory.apiProductID = reader["ApiProductID"].ToString();

                    if (loadImage)
                    {
                        if (reader["ImageFile"] != DBNull.Value)
                            stockInventory.ImageFile = reader["ImageFile"].ToString();
                        if (reader["ThumbnailFile"] != DBNull.Value)
                            stockInventory.ThumbnailFile = reader["ThumbnailFile"].ToString();
                    }

                    if (getParent && reader["ParentID"] != DBNull.Value)
                        stockInventory.ParentID = Convert.ToInt32(reader["ParentID"]);
                    if (getMoreInfo)
                    {
                        if (reader["ProductURL"] != DBNull.Value)
                            stockInventory.ProductURL = reader["ProductURL"].ToString();
                        if (reader["ProductZoneID"] != DBNull.Value)
                            stockInventory.ProductZoneID = Convert.ToInt32(reader["ProductZoneID"]);
                        if (reader["ProductName"] != DBNull.Value)
                            stockInventory.ProductName = reader["ProductName"].ToString();
                    }
                    stockInventoryList.Add(stockInventory);
                }
            }
            finally
            {
                reader.Close();
            }

            return stockInventoryList;
        }

        private static List<StockInventory> LoadListFromReader2(IDataReader reader)
        {
            List<StockInventory> lstStoreIDs = new List<StockInventory>();
            try
            {
                while (reader.Read())
                {
                    StockInventory inventory = new StockInventory
                    {
                        storeID = Convert.ToInt32(reader["StoreID"]),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        quantity = Convert.ToInt32(reader["Quantity"])
                    };
                    lstStoreIDs.Add(inventory);
                }
            }
            finally
            {
                reader.Close();
            }

            return lstStoreIDs;
        }

        public static int InventoryFilterCount(
            string keyword,
            int storeID,
            int fromPrice,
            int toPrice,
            int fromQuantity,
            int toQuantity,
            string status,
            int selectMode,
            int parentID = -1)
        {
            return DBStockInventory.InventoryFilterCount(keyword, storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, selectMode, parentID);
        }

        public static List<StockInventory> InventoryFilter(
            string keyword,
            int storeID,
            int fromPrice,
            int toPrice,
            int fromQuantity,
            int toQuantity,
            string status,
            int selectMode,
            int pageNumber,
            int pageSize,
            bool loadImage = false,
            bool getParent = false,
            bool getMoreInfo = false,
            int parentID = -1)
        {
            return LoadListFromReader(DBStockInventory.InventoryFilter(keyword, storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, selectMode, pageNumber, pageSize, parentID), loadImage, getParent, getMoreInfo);
        }

        public static StockInventory GetProductInStore(
            int productID,
            int storeID)
        {
            var lst = LoadListFromReader(DBStockInventory.GetProductInStore(productID, storeID));
            if (lst.Count > 0)
                return lst[0];
            return null;
        }

        public static List<StockInventory> GetByStore(
            int storeID)
        {
            return LoadListFromReader(DBStockInventory.GetByStore(storeID));
        }

        public static List<int> GetStoreIDsForCheckout(
             List<ShoppingCartItem> cartItems)
        {
            List<int> result = new List<int>();
            var products = cartItems.Select(c => c.ProductId).Distinct();
            string productIds = string.Join(";", products);
            int productCount = products.Count();
            var tmp = LoadListFromReader2(DBStockInventory.GetStockInventoriesForCheckout(productIds));//Get All Store have products
            foreach (var item in tmp)
            {
                if (tmp.Where(s => s.storeID == item.storeID).Count() != productCount)//Skip Store Not enough products
                    continue;
                var cartItem = cartItems.FirstOrDefault(c => c.ProductId == item.productID);
                if (cartItem == null)
                    continue;
                if (item.quantity >= cartItem.Quantity && !result.Contains(item.storeID))
                    result.Add(item.storeID);
            }
            return result;
        }

        #endregion Static Methods

        #region Reports

        public static DataTable GetReportStockInventory(
            int siteID,
            string keyword,
            int storeID,
            int fromPrice,
            int toPrice,
            int fromQuantity,
            int toQuantity,
            string status,
            int selectMode)
        {
            int pageNumber = 0;
            int pageSize = 0;

            IDataReader reader;

            reader = DBStockInventory.InventoryFilter(keyword, storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, selectMode, pageNumber, pageSize, -1);

            var dt = new DataTable();
            dt.Columns.Add("Row", typeof(int));
            dt.Columns.Add("ProductID", typeof(int));
            dt.Columns.Add("ProductCode", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("StoreID", typeof(int));
            dt.Columns.Add("StoreName", typeof(string));
            //dt.Columns.Add("Price", typeof(int));
            dt.Columns.Add("APICode", typeof(string));
            dt.Columns.Add("Quantity", typeof(string));

            try
            {
                int i = 0;
                while (reader.Read())
                {
                    DataRow row = dt.NewRow();
                    dt.Rows.Add(row);

                    i++;
                    row["Row"] = i;
                    int productID = Convert.ToInt32(reader["ProductID"]);
                    Product product = new Product(siteID, productID);
                    row["ProductID"] = product.ProductId;
                    row["ProductCode"] = product.Code;
                    row["ProductName"] = product.Title;
                    int readerStoreID = storeID; //displayInputted ? Convert.ToInt32(reader["StoreID"]) : storeID;
                    Store store = new Store(readerStoreID);
                    row["StoreID"] = store.StoreID;
                    row["StoreName"] = store.Name;
                    //row["Price"] = reader["Price"];
                    if (reader["ApiProductID"] != DBNull.Value)
                        row["APICode"] = reader["ApiProductID"];
                    else
                        row["APICode"] = string.Empty;
                    if (reader["Quantity"] != DBNull.Value)
                        row["Quantity"] = reader["Quantity"];
                    else
                        row["Quantity"] = string.Empty;
                }
            }
            finally
            {
                reader.Close();
            }
            return dt;
        }

        #endregion Reports

        #region TotalQuantity

        public static int GetProductTotalQuantity(
            int productID
        )
        {
            return DBStockInventory.GetProductTotalQuantity(productID);
        }

        #endregion TotalQuantity
    }
}
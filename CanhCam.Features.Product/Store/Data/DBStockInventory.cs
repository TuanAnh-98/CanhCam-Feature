// Author:					t
// Created:					2020-5-8
// Last Modified:			2020-5-8

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBStockInventory
    {
        public static int Create(
            int productID,
            int storeID,
            int price,
            int quantity,
            Guid guid,
            bool isPublished,
            bool isDeleted,
            string apiProductID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_StockInventory_Insert", 8);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@Price", SqlDbType.Int, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@ApiProductID", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiProductID);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int inventoryID,
            int productID,
            int storeID,
            int price,
            int quantity,
            Guid guid,
            bool isPublished,
            bool isDeleted,
            string apiProductID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_StockInventory_Update", 9);
            sph.DefineSqlParameter("@InventoryID", SqlDbType.Int, ParameterDirection.Input, inventoryID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@Price", SqlDbType.Int, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@Quantity", SqlDbType.Int, ParameterDirection.Input, quantity);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@ApiProductID", SqlDbType.NVarChar, 255, ParameterDirection.Input, apiProductID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(
            int inventoryID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_StockInventory_Delete", 1);
            sph.DefineSqlParameter("@InventoryID", SqlDbType.Int, ParameterDirection.Input, inventoryID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(
            int inventoryID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_SelectOne", 1);
            sph.DefineSqlParameter("@InventoryID", SqlDbType.Int, ParameterDirection.Input, inventoryID);
            return sph.ExecuteReader();
        }

        //public static int GetCount()
        //{
        //	return Convert.ToInt32(SqlHelper.ExecuteScalar(
        //		ConnectionString.GetReadConnectionString(),
        //		CommandType.StoredProcedure,
        //		"gb_StockInventory_GetCount",
        //		null));

        //}

        public static int InventoryFilterCount(
            string keyword,
            int storeID,
            int fromPrice,
            int toPrice,
            int fromQuantity,
            int toQuantity,
            string status,
            int selectMode,
            int parentID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_InventoryFilterCount", 9);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@FromPrice", SqlDbType.Int, ParameterDirection.Input, fromPrice);
            sph.DefineSqlParameter("@ToPrice", SqlDbType.Int, ParameterDirection.Input, toPrice);
            sph.DefineSqlParameter("@FromQuantity", SqlDbType.Int, ParameterDirection.Input, fromQuantity);
            sph.DefineSqlParameter("@ToQuantity", SqlDbType.Int, ParameterDirection.Input, toQuantity);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 50, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@SelectMode", SqlDbType.Int, ParameterDirection.Input, selectMode);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentID);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader InventoryFilter(
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
            int parentID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_InventoryFilter", 11);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@FromPrice", SqlDbType.Int, ParameterDirection.Input, fromPrice);
            sph.DefineSqlParameter("@ToPrice", SqlDbType.Int, ParameterDirection.Input, toPrice);
            sph.DefineSqlParameter("@FromQuantity", SqlDbType.Int, ParameterDirection.Input, fromQuantity);
            sph.DefineSqlParameter("@ToQuantity", SqlDbType.Int, ParameterDirection.Input, toQuantity);
            sph.DefineSqlParameter("@Status", SqlDbType.NVarChar, 50, ParameterDirection.Input, status);
            sph.DefineSqlParameter("@SelectMode", SqlDbType.Int, ParameterDirection.Input, selectMode);
            sph.DefineSqlParameter("@ParentID", SqlDbType.Int, ParameterDirection.Input, parentID);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static IDataReader GetProductInStore(
            int productID,
            int storeID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_GetProductInStore", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByStore(
            int storeID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_GetByStore", 1);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetStockInventoriesForCheckout(
            string productIDs)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_GetStoreIDsForCheckout", 1);
            sph.DefineSqlParameter("@ProductIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, productIDs);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByProductId(
            int productID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_GetByProductID", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            return sph.ExecuteReader();
        }

        #region TotalQuantity

        public static int GetProductTotalQuantity(
            int productID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_StockInventory_GetProductTotalQuantity", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        #endregion TotalQuantity
    }
}
using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class StockInventoryModel
    {
        public int InventoryID { get; set; }
        public int ProductID { get; set; }
        public int StoreID { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string StoreAddress { get; set; }
        public string StoreDescription { get; set; }
        public string StoreMap { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }



        private static List<StockInventoryModel> LoadListFromReader(IDataReader reader)
        {
            List<StockInventoryModel> items = new List<StockInventoryModel>();
            try
            {
                while (reader.Read())
                {
                    StockInventoryModel item = new StockInventoryModel
                    {
                        InventoryID = Convert.ToInt32(reader["InventoryID"]),
                        ProductID = Convert.ToInt32(reader["ProductID"]),
                        StoreID = Convert.ToInt32(reader["StoreID"]),
                        StoreCode = reader["StoreCode"].ToString(),
                        StoreName = reader["StoreName"].ToString(), 
                        StoreAddress = reader["StoreAddress"].ToString(),
                        StoreDescription = reader["StoreDescription"].ToString(),
                        StoreMap = reader["StoreMap"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"].ToString()),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    };
                    items.Add(item);
                }
            }
            finally
            {
                reader.Close();
            }

            return items;
        }





        public static List<StockInventoryModel> GetByProductId(int productId)
        {
            return LoadListFromReader(DBStockInventory.GetByProductId(productId));
        }
    }
}
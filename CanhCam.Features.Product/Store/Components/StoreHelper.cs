using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.StoreUI
{
    public class StoreHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreHelper));

        public static bool EnabledStore => ConfigHelper.GetBoolProperty("Store:Enabled", false);

        public static bool EnabledInventory => ConfigHelper.GetBoolProperty("Store:EnabledInventory", false);

        public static bool EnabledAutoSelectStoreDefaultWhenNotValidInventory => ConfigHelper.GetBoolProperty("Store:EnabledAutoSelectStoreDefaultWhenNotValidInventory", true);

        public static string GetStoreNameByID(int storeID)
        {
            Store store = new Store(storeID);
            if (store != null && store.StoreID > 0)
                return store.Name;
            else
                return string.Empty;
        }

        public static string GetAdminEmails(SiteSettings siteSettings, string lstUserIDs)
        {
            string adminEmails = string.Empty;
            foreach (string storeAdmin in StringHelper.SplitOnCharAndTrim(lstUserIDs, ';'))
            {
                Int32.TryParse(storeAdmin, out int adminID);
                SiteUser admin = new SiteUser(siteSettings, adminID);
                if (admin != null && admin.UserId > 0)
                {
                    adminEmails += admin.Email + ";";
                }
            }

            return adminEmails;
        }

        public static Dictionary<Guid, int> GetStoreDistricts(string districts)
        {
            var results = new Dictionary<Guid, int>();

            if (!string.IsNullOrEmpty(districts))
            {
                foreach (string district in districts.SplitOnCharAndTrim(';'))
                {
                    var part = district.SplitOnCharAndTrim('_');

                    Guid guid = Guid.Empty;
                    Guid.TryParse(part[0].ToUpper(), out guid);

                    int index = 0;
                    if (part.Count == 2)
                    {
                        int.TryParse(part[1], out index);
                    }
                    results.Add(guid, index);
                }
            }

            return results;
        }

        public static bool ContainsDistricGuid(string districts, string guid, ref int priority)
        {
            var dic = GetStoreDistricts(districts);
            foreach (var item in dic)
            {
                if (item.Key.ToString().ToUpper() == guid.ToUpper())
                {
                    priority = item.Value;
                    return true;
                }
            }
            return false;
        }

        public static List<Guid> GetStoresGeoGuidsNoOrder(List<Store> lstStores)
        {
            List<Guid> lstDistrictGuids = new List<Guid>();
            foreach (Store store in lstStores)
            {
                var dictDistrictGuids = GetStoreDistricts(store.DistrictGuids);
                foreach (var item in dictDistrictGuids)
                {
                    if (!lstDistrictGuids.Contains(item.Key))
                    {
                        lstDistrictGuids.Add(item.Key);
                    }
                }
            }

            return lstDistrictGuids;
        }

        public static List<GeoZone> GetGeoZonesOrderByProvince(List<Guid> lstDistrictGuids, List<GeoZone> lstProvinces)
        {
            List<GeoZone> lstDistricts = new List<GeoZone>();
            foreach (GeoZone province in lstProvinces)
            {
                if (lstDistrictGuids.Contains(province.Guid))
                    lstDistricts.Add(province);
                List<GeoZone> childs = GeoZone.GetByListParent(province.Guid.ToString());
                foreach (GeoZone child in childs)
                {
                    if (lstDistrictGuids.Contains(child.Guid))
                        lstDistricts.Add(child);
                }
            }
            return lstDistricts;
        }

        public static List<Guid> GetGeoGuidsFromGeoZones(List<GeoZone> lstDistricts)
        {
            List<Guid> lstDistrictGuids = lstDistricts.Select(s => s.Guid).Distinct().ToList();
            return lstDistrictGuids;
        }

        public static List<Store> GetDistrictStoresOrderByPriority(List<Store> lstAllStores, List<Guid> lstDistrictGuids)
        {
            List<Store> lstStores = new List<Store>();

            foreach (Guid districtGuid in lstDistrictGuids)
            {
                var lstDistrictStore = new List<Store>();
                var st = (Store)null;
                foreach (Store store in lstAllStores)
                {
                    int priority = 0;
                    if (StoreHelper.ContainsDistricGuid(store.DistrictGuids, districtGuid.ToString().ToUpper(), ref priority))
                    {
                        st = new Store()
                        {
                            StoreID = store.StoreID,
                            Name = store.Name,
                            Priority = priority,
                            ManagingArea = districtGuid
                        };
                        lstDistrictStore.Add(st);
                    }
                }
                lstDistrictStore = lstDistrictStore.OrderByDescending(x => x.Priority).ToList();
                lstStores.AddRange(lstDistrictStore);
            }
            return lstStores;
        }

        public static bool UpdateProductTotalQuantity(int siteID, int productID)
        {
            var product = new Product(siteID, productID);
            if (product != null && product.ProductId > 0)
            {
                product.StockQuantity = StockInventory.GetProductTotalQuantity(product.ProductId);
                product.SaveStockQuantity();
                return true;
            }
            else
                return false;
        }

        public static Store SettoreForOrder(Order order, List<ShoppingCartItem> shoppingCartItems)
        {
            Store storeForOrder = null;
            Guid shippingDistrictGuid = order.BillingDistrictGuid;
            if (order.ShippingDistrictGuid != null && order.ShippingDistrictGuid != Guid.Empty)
                shippingDistrictGuid = order.ShippingDistrictGuid;
            var storesAvailability = StoreCacheHelper.GetForLocationByPriority(order.SiteId, shippingDistrictGuid);
            var storeDefault = StoreCacheHelper.GetStoreDefault(order.SiteId);
            log.Info("storesAvailability : " + storesAvailability.Count + " Location :" + shippingDistrictGuid);
            if (storeDefault != null)
                log.Info("storeDefault : " + storeDefault.Name);

            if (storesAvailability.Count == 0 && storeDefault == null)
                return null;
            if (order.StoreId > 0)
            {
                storeForOrder = new Store(order.StoreId);
                if (storeForOrder == null || storeForOrder.IsDeleted || !storesAvailability.Any(s => s.StoreID == order.StoreId))
                    order.StoreId = -1; //reset Store when order get from session have storeid don't avaliable
            }

            //Set Store if not selected
            if (order.StoreId <= 0)
            {
                if (storesAvailability.Count > 0)
                    storeForOrder = storesAvailability[0];
                else if (storeDefault != null)
                    storeForOrder = storeDefault;
            }

            if (storeForOrder == null)//Not found Store
                return null;

            if (EnabledInventory)
            {
                //Get StoreIds that have all items in productIDs and each item quantity >= minQuantity
                List<int> storeIdsAvailabilityForInventory = StockInventory.GetStoreIDsForCheckout(shoppingCartItems);
                //log.Info("storeIdsAvailabilityForInventory : " + storeIdsAvailabilityForInventory.Count);
                if (storesAvailability.Any(s => storeIdsAvailabilityForInventory.Contains(s.StoreID)))
                {
                    storeForOrder = storesAvailability.Where(s => storeIdsAvailabilityForInventory.Contains(s.StoreID)).OrderByDescending(s => s.Priority).FirstOrDefault();
                }
                else if (EnabledAutoSelectStoreDefaultWhenNotValidInventory && storeDefault != null)
                {
                    storeForOrder = storeDefault;
                }
                //if (storeForOrder != null)
                //    log.Info("Inventory Result" + storeForOrder.Name);
            }
            return storeForOrder;
        }

        public static List<ValidateInventoryModel> ValidateInventoryOrder(Order order,
            List<ShoppingCartItem> shoppingCartItems,
            List<Product> products, ref Store store)
        {
            List<ValidateInventoryModel> models = new List<ValidateInventoryModel>();

            Guid shippingDistrictGuid = order.BillingDistrictGuid;
            if (order.ShippingDistrictGuid != null && order.ShippingDistrictGuid != Guid.Empty)
                shippingDistrictGuid = order.ShippingDistrictGuid;
            var storesAvailability = StoreCacheHelper.GetForLocationByPriority(order.SiteId, shippingDistrictGuid);

            if (storesAvailability.Count == 0)
                return null;
            store = storesAvailability[0];

            foreach (ShoppingCartItem item in shoppingCartItems)
            {
                var product = products.FirstOrDefault(it => it.ProductId == item.ProductId);
                if (product == null)
                    continue;
                var inventory = StockInventory.GetProductInStore(item.ProductId, store.StoreID);
                int stockQuantityAvailability;
                if (inventory == null || inventory.InventoryID == -1)
                    stockQuantityAvailability = 0;
                else
                    stockQuantityAvailability = inventory.Quantity;
                models.Add(new ValidateInventoryModel()
                {
                    ProductId = item.ProductId,
                    ProductGuid = item.Product.ProductGuid,
                    StockQuantityAvailability = stockQuantityAvailability,
                    IsValid = (stockQuantityAvailability >= item.Quantity),
                    ProductTitle = product.Title,
                    CartQuantity = item.Quantity,
                    CartItemGuid = item.Guid
                });
            }
            return models;
        }
    }
}
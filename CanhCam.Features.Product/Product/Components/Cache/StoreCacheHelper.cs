using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Caching;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public static class StoreCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreCacheHelper));
        private static string CacheKey_Store_All = "Store-{0}";
        private static string CacheKey_Store_Default = "Store-{0}"; 
        private static string CacheKey_Store_By_Id = "Store-ID-{0}";
        private static string CacheKey_Store_By_Code = "Store-Code-{0}";
        private static string CacheKey_Store_By_DistrictGuid = "Store-{0}-{1}";

        private static void ClearCacheByKey(string cachekey)
        {
            try
            {
                if (WebConfigSettings.UseCacheDependencyFiles)
                    CacheHelper.TouchCacheFile(CacheHelper.GetPathToCacheDependencyFile(cachekey));

                CacheManager.Cache.InvalidateCacheItem(cachekey);
            }
            catch (Exception ex)
            {
                log.Error("Clear Cache key '" + cachekey + "' Error : " + ex.Message);
            }
        }

        public static List<Store> GetStoresAll(int siteId)
        {
            var cachekey = string.Format(CacheKey_Store_All, siteId);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var rows = CacheManager.Cache.Get<object>(cachekey, expiration, () =>
                {
                    return Store.GetAll().Where(s => s.SiteID == siteId).ToList();
                });

                return (List<Store>)rows;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<Store>();
            }
        }

        public static List<Store> GetForLocationByPriority(int siteId, Guid districtGuid)
        {
            var lstStores = GetStoresAll(siteId);
            List<Store> result = new List<Store>();
            foreach (var store in lstStores)
            {
                var priorityNew = 0;
                if (StoreUI.StoreHelper.ContainsDistricGuid(store.DistrictGuids, districtGuid.ToString(), ref priorityNew))
                {
                    store.Priority = priorityNew;
                    result.Add(store);
                }
            }

            result = result.OrderByDescending(s => s.Priority).ToList();
            return result;
        }

        public static Store GetStoreDefault(int siteId)
        {
            var cachekey = string.Format(CacheKey_Store_Default, siteId);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var store = CacheManager.Cache.Get<Store>(cachekey, expiration, () =>
                {
                    return Store.GetDefaultStore();
                });

                return store;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public static Store GetStoreById(int storeId)
        {
            var cachekey = string.Format(CacheKey_Store_By_Id, storeId);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var store = CacheManager.Cache.Get<Store>(cachekey, expiration, () =>
                {
                    return new Store(storeId);
                });

                return store;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public static Store GetStoreByCode(string code)
        {
            var cachekey = string.Format(CacheKey_Store_By_Code, code);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var store = CacheManager.Cache.Get<Store>(cachekey, expiration, () =>
                {
                    return new Store(code);
                });

                return store;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public static void ClearCacheByDistrictGuid(int siteId, Guid districtGuid)
        {
            var cachekey = string.Format(CacheKey_Store_By_DistrictGuid, siteId, districtGuid);
            ClearCacheByKey(cachekey);
        }

        public static void ClearCacheById(int storeId)
        {
            var cachekey = string.Format(CacheKey_Store_By_Id, storeId);
            ClearCacheByKey(cachekey);
        }

        public static void ClearCacheGetAll(int siteId)
        {
            var cachekey = string.Format(CacheKey_Store_All, siteId);
            ClearCacheByKey(cachekey);
        }
    }
}
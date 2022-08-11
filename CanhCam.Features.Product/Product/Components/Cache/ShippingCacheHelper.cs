using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Caching;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public static class ShippingCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShippingCacheHelper));
        private static string CacheKey_Shipping_By_Id = "Shipping-ID-{0}";
        private static string CacheKey_Shipping_Tables_By_Id = "Shipping-Table-ID-{0}";
        private static string CacheKey_Shipping_Tables_By_GeoZone = "Shipping-Table-ID-{0}-{1}-{2}-{3}";
        private static string CacheKey_Shipping_VTP_By_GeoZone = "Shipping-VTP-GeoZone-{0}";
        private static string CacheKey_Shipping_VTP_By_Code = "Shipping-VTP-Code-{0}";
        private static string CacheKey_Shipping_VTP_By_Store = "Shipping-VTP-Code-Ward-Store-{0}";

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
        public static ViettelPostProvince GetViettelPostProvince(Guid guid)
        {
            var cachekey = string.Format(CacheKey_Shipping_VTP_By_GeoZone, guid);

            var expiration = DateTime.Now.AddDays(5);
            try
            {
                var tables = CacheManager.Cache.Get<ViettelPostProvince>(cachekey, expiration, () =>
                {
                    return new ViettelPostProvince(guid);
                });

                return tables;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public static ViettelPostCode GetViettelPostCode(string code)
        {
            var cachekey = string.Format(CacheKey_Shipping_VTP_By_Code, code);

            var expiration = DateTime.Now.AddDays(5);
            try
            {
                var tables = CacheManager.Cache.Get<ViettelPostCode>(cachekey, expiration, () =>
                {
                    return new ViettelPostCode(code);
                });

                return tables;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public static ViettelPostCode GetVittelPostWardCodeByStore(Store store)
        {
            var cachekey = string.Format(CacheKey_Shipping_VTP_By_Store, store.StoreID);

            var expiration = DateTime.Now.AddDays(5);
            try
            {
                var tables = CacheManager.Cache.Get<ViettelPostCode>(cachekey, expiration, () =>
                {
                    var wards = GeoZone.GetByListParent(store.DistrictGuid.ToString());
                    if (wards.Count > 0)
                    {
                        GeoZone geoZone = wards.FirstOrDefault();
                        foreach (GeoZone ward in wards)
                            if (store.Address.ToLower().Contains(ward.Name.Replace("X.", string.Empty).Replace("TT.", string.Empty).Trim().ToLower()))
                                geoZone = ward;
                        return GetViettelPostCode(geoZone.Code);
                    }
                    return null;
                });

                return tables;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public static ShippingMethod GetShippingMethodById(int methodId)
        {
            var cachekey = string.Format(CacheKey_Shipping_By_Id, methodId);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var tables = CacheManager.Cache.Get<ShippingMethod>(cachekey, expiration, () =>
                {
                    return new ShippingMethod(methodId);
                });

                return tables;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public static List<ShippingTableRate> GetShippingTableRatesById(int methodId)
        {
            var cachekey = string.Format(CacheKey_Shipping_Tables_By_Id, methodId);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var tables = CacheManager.Cache.Get<List<ShippingTableRate>>(cachekey, expiration, () =>
                {
                    return ShippingTableRate.GetByMethod(methodId);
                });

                return tables;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }

        public static ShippingTableRate GetShippingTable(int methodId, decimal fromValue, string geoZoneGuids, int storeId)
        {
            var cachekey = string.Format(CacheKey_Shipping_Tables_By_GeoZone, methodId, fromValue, geoZoneGuids, storeId);

            var expiration = DateTime.Now.AddMinutes(5);
            try
            {
                var tables = CacheManager.Cache.Get<ShippingTableRate>(cachekey, expiration, () =>
                {
                    return ShippingTableRate.GetOneMaxValue(methodId, fromValue, geoZoneGuids, storeId);
                });

                return tables;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return null;
            }
        }
        public static void ClearCacheById(int methodId)
        {
            var cachekey = string.Format(CacheKey_Shipping_By_Id, methodId);
            ClearCacheByKey(cachekey);
        }
    }
}
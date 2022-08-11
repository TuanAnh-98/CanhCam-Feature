using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Caching;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public static class GeoZoneCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GeoZoneCacheHelper));
        private static string CacheKey = "District-{0}-{1}";
        private static string GeoZoneCacheKey = "GeoZone-By-Id-{0}";
        public static GeoZone GetByGuid(
           Guid itemGuid)
        {
            var cachekey = string.Format(GeoZoneCacheKey, itemGuid);

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);
            try
            {
                var rows = CacheManager.Cache.Get<GeoZone>(cachekey, expiration, () =>
                {
                    return new GeoZone(itemGuid);
                });

                return (GeoZone)rows;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new GeoZone();
            }
        }
        public static List<GeoZone> GetDistrict(
            int languageId,
            Guid provinceGuid)
        {
            var cachekey = string.Format(CacheKey, languageId, provinceGuid);

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);
            try
            {
                var rows = CacheManager.Cache.Get<List<GeoZone>>(cachekey, expiration, () =>
                {
                    var items = GeoZone.GetByListParent(provinceGuid.ToString(), 1, languageId);
                    return items;
                });

                return (List<GeoZone>)rows;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<GeoZone>();
            }
        }

    }
}
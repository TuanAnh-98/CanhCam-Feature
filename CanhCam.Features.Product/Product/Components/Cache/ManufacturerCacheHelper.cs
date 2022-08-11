using CanhCam.Business;
using CanhCam.Web.Caching;
using log4net;
using System;

namespace CanhCam.Web.ProductUI
{
    public static class ManufacturerCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ManufacturerCacheHelper));

        public static Manufacturer GetById(int id)
        {
            var cachekey = "Manufacturer_Id_" + id + "_" + WorkingCulture.LanguageId;

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);

            try
            {
                var lstProducts = CacheManager.Cache.Get(cachekey, expiration, () =>
                {
                    return new Manufacturer(id);
                });

                return lstProducts;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new Manufacturer(); 
            }
        }
    }
}
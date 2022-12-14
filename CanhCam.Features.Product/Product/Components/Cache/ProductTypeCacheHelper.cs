using CanhCam.Business;
using CanhCam.Web.Caching;
using log4net;
using System;

namespace CanhCam.Web.ProductUI
{
    public static class ProductTypeCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductTypeCacheHelper));

        public static ProductType GetById(int id)
        {
            var cachekey = "Product_Type_Id_" + id + "_" + WorkingCulture.LanguageId;

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);

            try
            {
                var lstProducts = CacheManager.Cache.Get(cachekey, expiration, () =>
                {
                    return new ProductType(id);
                });

                return lstProducts;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new ProductType(); 
            }
        }
    }
}
using CanhCam.Business;
using CanhCam.Web.Caching;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public static class ProductCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductCacheHelper));

        private static string GetProductListCacheKey(
            string key,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null,
            int orderBy = (int)ProductSortingEnum.Position,
            int pageSize = 1,
            int pageNumber = 1
            )
        {
            return key + "-" + siteId.ToString() + "-"
                                + zoneId.ToString() + "-"
                                + (zoneIds != null ? zoneIds.ToString() + "-" : "")
                                + publishStatus.ToString() + "-"
                                + languageId.ToString() + "-"
                                + parentId.ToString() + "-"
                                + manufactureId.ToString() + "-"
                                + productType.ToString() + "-"
                                + (priceMin != null ? Convert.ToDouble(priceMin.Value).ToString() + "-" : "")
                                + (priceMax != null ? Convert.ToDouble(priceMax.Value).ToString() + "-" : "")
                                + position.ToString() + "-"
                                + showOption.ToString() + "-"
                                + (propertyCondition != null ? propertyCondition.ToAsciiIfPossible().Replace(" ", "+") + "-" : "")
                                + (productIds != null ? productIds.ToString() + "-" : "")
                                + (keyword != null ? keyword.ToAsciiIfPossible().Replace(" ", "+") + "-" : "")
                                + searchCode.ToString() + "-"
                                + searchProductZone.ToString() + "-"
                                + (stateIds != null ? stateIds.ToString() + "-" : "")
                                + ((int)orderBy).ToString() + "-"
                                + pageSize.ToString() + "-"
                                + pageNumber.ToString()
                            ;
        }

        public static List<Product> GetTopAdv(
            int top = 1,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
            int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null,
            int orderBy = (int)ProductSortingEnum.Position
            )
        {
            var cachekey = GetProductListCacheKey(
                    "TopProduct",
                    siteId,
                    zoneId,
                    zoneIds,
                    publishStatus,
                    languageId,
                    parentId,
                    manufactureId,
                    productType,
                    priceMin,
                    priceMax,
                    position,
                    showOption,
                    propertyCondition,
                    productIds,
                    keyword,
                    searchCode,
                    searchProductZone,
                    stateIds,
                    orderBy,
                    top
                );
            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);

            try
            {
                var lstProducts = CacheManager.Cache.Get<List<Product>>(cachekey, expiration, () =>
                {
                    var lst = Product.GetTopAdv(top, siteId, zoneId, zoneIds, publishStatus, languageId, parentId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, searchProductZone, stateIds, orderBy);

                    return lst;
                });

                return lstProducts;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<Product>();
            }
        }

        public static List<Product> GetPageAdv(
            int pageNumber = 1,
            int pageSize = 32767,
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
             int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null,
            int orderBy = (int)ProductSortingEnum.Position)
        {
            var cachekey = GetProductListCacheKey(
                    "PageAdv",
                    siteId,
                    zoneId,
                    zoneIds,
                    publishStatus,
                    languageId,
                    parentId,
                    manufactureId,
                    productType,
                    priceMin,
                    priceMax,
                    position,
                    showOption,
                    propertyCondition,
                    productIds,
                    keyword,
                    searchCode,
                    searchProductZone,
                    stateIds,
                    orderBy,
                    pageSize,
                    pageNumber
                );

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);

            try
            {
                var lstProducts = CacheManager.Cache.Get<List<Product>>(cachekey, expiration, () =>
                {
                    var lst = Product.GetPageAdv(pageNumber, pageSize, siteId, zoneId, zoneIds, publishStatus, languageId, parentId,
                        manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition,
                        productIds, keyword, searchCode, searchProductZone, stateIds, -1, orderBy);

                    return lst;
                });

                return lstProducts;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<Product>();
            }
        }

        public static int GetCountAdv(
            int siteId = -1,
            int zoneId = -1,
            string zoneIds = null,
            int publishStatus = -1,
            int languageId = -1,
             int parentId = -1,
            int manufactureId = -1,
            int productType = -1,
            decimal? priceMin = null,
            decimal? priceMax = null,
            int position = -1,
            int showOption = -1,
            string propertyCondition = null,
            string productIds = null,
            string keyword = null,
            bool searchCode = false,
            bool searchProductZone = false,
            string stateIds = null)
        {
            var cachekey = GetProductListCacheKey(
                    "ProductCount",
                    siteId,
                    zoneId,
                    zoneIds,
                    publishStatus,
                    languageId,
                    parentId,
                    manufactureId,
                    productType,
                    priceMin,
                    priceMax,
                    position,
                    showOption,
                    propertyCondition,
                    productIds,
                    keyword,
                    searchCode,
                    searchProductZone,
                    stateIds
                );

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);

            try
            {
                var numberRows = CacheManager.Cache.Get<object>(cachekey, expiration, () =>
                {
                    var iCount = Product.GetCountAdv(siteId, zoneId, zoneIds, publishStatus, languageId, parentId, manufactureId, productType, priceMin, priceMax, position, showOption, propertyCondition, productIds, keyword, searchCode, searchProductZone, stateIds);

                    return iCount;
                });

                return (int)numberRows;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return 0;
            }
        }
    }
}
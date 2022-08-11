/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2018-11-02
/// Last Modified:			2018-11-02

using CanhCam.Business;
using CanhCam.Web.Caching;
using log4net;
using System;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public static class DiscountAppliedToItemHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountAppliedToItemHelper));

        public static List<DiscountAppliedToItem> GetActive(int siteId, int discountType, List<Product> lstProducts, int options = -1)
        {
            var zoneIds = new List<int>();
            var productIds = new List<int>();
            foreach (var product in lstProducts)
            {
                if (!zoneIds.Contains(product.ZoneId))
                    zoneIds.Add(product.ZoneId);
                if (!productIds.Contains(product.ProductId))
                    productIds.Add(product.ProductId);
            }

            if (zoneIds.Count == 0 && productIds.Count == 0)
                return new List<DiscountAppliedToItem>();

            var cachekey = "DiscountAppliedToItemActive-"
                                + siteId.ToString() + "-"
                                + discountType.ToString() + "-"
                                + (zoneIds.Count > 0 ? string.Join("+", zoneIds.ToArray()) + "-" : "")
                                + (productIds.Count > 0 ? string.Join("+", productIds.ToArray()) + "-" : "")
                                + options.ToString()
                            ;
            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);

            try
            {
                var lstItems = CacheManager.Cache.Get<List<DiscountAppliedToItem>>(cachekey, expiration, () =>
                {
                    var lst = DiscountAppliedToItem.GetActive(siteId, discountType, zoneIds, productIds, options);

                    return lst;
                });

                return lstItems;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<DiscountAppliedToItem>();
            }
        }
    }
}
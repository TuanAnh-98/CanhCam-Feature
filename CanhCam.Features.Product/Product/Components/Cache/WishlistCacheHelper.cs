using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Caching;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public static class WishlistCacheHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(WishlistCacheHelper));
        private static string CacheKey = "Wishlist-{0}-{1}";

        public static List<ShoppingCartItem> GetWishLish(
            int siteId,
            Guid userGuid)
        {
            var cachekey = string.Format(CacheKey, siteId, userGuid);

            var expiration = DateTime.Now.AddSeconds(WebConfigSettings.DefaultCacheDurationInSeconds);
            try
            {
                var rows = CacheManager.Cache.Get<object>(cachekey, expiration, () =>
                {
                    var items = ShoppingCartItem.GetByUserGuid(siteId, ShoppingCartTypeEnum.Wishlist, userGuid);

                    return items;
                });

                return (List<ShoppingCartItem>)rows;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return new List<ShoppingCartItem>();
            }
        }

        public static void ClearCache(
            int siteId,
            Guid userGuid)
        {
            var cachekey = string.Format(CacheKey, siteId, userGuid);

            try
            {
                if (WebConfigSettings.UseCacheDependencyFiles)
                    CacheHelper.TouchCacheFile(CacheHelper.GetPathToCacheDependencyFile(cachekey));

                CacheManager.Cache.InvalidateCacheItem(cachekey);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public static bool ExistWishList(int siteId, int productId)
        {
            var items = GetWishLish(siteId, CartHelper.GetCartSessionGuid(siteId));
            return items.Any(item => item.ProductId == productId);
        }

        public static Guid GetWishListItemGuid(int siteId, int productId)
        {
            if (!ExistWishList(siteId, productId))
                return Guid.Empty;
            var items = GetWishLish(siteId, CartHelper.GetCartSessionGuid(siteId));
            return items.FirstOrDefault(item => item.ProductId == productId).Guid;
        }
    }
}
using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public static class ProductServiceHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductServiceHelper));
        public static bool Enable = ConfigHelper.GetBoolProperty("Shopping:ProductServiceEnable", false);

        public static decimal GetServiceFee(Product product, ShoppingCartItem cartItem, decimal subTotal)
        {
            var services = ProductServicePrice.GetByProductId(product.ProductId);
            decimal result = 0;
            foreach (var item in services)
            {
                if (item.Price <= 0)
                    continue;

                if (item.FeeForOrderTotal <= subTotal)
                    continue;
                result += item.Price * cartItem.Quantity;
            }
            return result;
        }

        public static decimal GetServiceFee(List<Product> products, List<ShoppingCartItem> cartItems, decimal subTotal)
        {
            string productIds = products.GetProductIds();
            var services = ProductServicePrice.GetByProductIds(productIds);
            decimal result = 0;
            foreach (var item in services)
            {
                if (item.Price <= 0)
                    continue;
                if (item.FeeForOrderTotal != 0 && item.FeeForOrderTotal <= subTotal)
                    continue;
                var cartItem = cartItems.FirstOrDefault(it => it.ProductId == item.ProductId);
                result += item.Price * cartItem.Quantity;
            }
            return result;
        }
    }
}
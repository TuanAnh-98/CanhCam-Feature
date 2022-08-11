using CanhCam.Business;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public static class ProductExtensions
    {
        #region Product

        public static string GetProductIds(this IList<Product> products)
        {
            if (!products.Any())
                return string.Empty;
            string result = string.Empty;
            string spec = "";
            foreach (var item in products)
            {
                result += spec + item.ProductId;
                spec = ";";
            }
            return result;
        }

        #endregion Product

        #region Shopping

        public static decimal GetSubTotal(this IList<ShoppingCartItem> shoppingCart, List<Product> lstProductsInCart)
        {
            var result = decimal.Zero;
            foreach (var sci in shoppingCart)
            {
                var product = ProductHelper.GetProductFromList(lstProductsInCart, sci.ProductId);
                if (product != null)
                {
                    var price = ProductHelper.GetOriginalPrice(product);
                    result += (sci.Quantity * price);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a number of manufacturer in the cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <returns>Result</returns>
        public static int GetTotalProducts(this IList<ShoppingCartItem> shoppingCart)
        {
            int result = 0;
            foreach (ShoppingCartItem sci in shoppingCart)
                result += sci.Quantity;

            return result;
        }

        public static int GetTotalProducts(this IList<OrderItem> orderItems)
        {
            int result = 0;
            foreach (OrderItem sci in orderItems)
                result += sci.Quantity;

            return result;
        }

        public static decimal GetTotalWeights(this IList<ShoppingCartItem> shoppingCart, List<Product> lstProductsInCart)
        {
            decimal result = 0;
            foreach (ShoppingCartItem sci in shoppingCart)
            {
                Product product = ProductHelper.GetProductFromList(lstProductsInCart, sci.ProductId);
                if (product != null)
                    result += (sci.Quantity * product.Weight);
            }
            return result;
        }

        public static decimal GetTotalWeights(this IList<OrderItem> orderItems, List<Product> lstProducts)
        {
            decimal result = 0;
            foreach (OrderItem sci in orderItems)
            {
                Product product = ProductHelper.GetProductFromList(lstProducts, sci.ProductId);
                if (product != null)
                    result += (sci.Quantity * product.Weight);
            }
            return result;
        }

        #endregion Shopping

        #region Order

        public static List<ShoppingCartItem> ToShoppingCartItems(this List<OrderItem> orderItems, int siteId)
        {
            List<ShoppingCartItem> shoppingCartItems = new List<ShoppingCartItem>();

            foreach (var item in orderItems)
            {
                shoppingCartItems.Add(new ShoppingCartItem()
                {
                    ItemDiscount = item.DiscountAmount,
                    ProductId = item.ProductId,
                    SiteId = siteId,
                    AttributesXml = item.AttributesXml,
                    ShoppingCartType = (int)ShoppingCartTypeEnum.ShoppingCart,
                });
            }
            return shoppingCartItems;
        }

        #endregion Order
    }
}
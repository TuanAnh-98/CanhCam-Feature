/// Created:			    2014-11-28
/// Last Modified:		    2015-10-23

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public class CartService : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CartService));

        private string method = string.Empty;
        private NameValueCollection postParams = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.ContentType = "text/json";
            Encoding encoding = new UTF8Encoding();
            Response.ContentEncoding = encoding;

            try
            {
                LoadParams();

                if (
                    method != "AddProductToCart_Catalog"
                    && method != "AddProductToWishlist"
                    && method != "AddProductToCart_Details"
                    && method != "AddProductToCart_Details_Multi"
                    && method != "GetTotalByProductIds"
                    && method != "UpdateCart"
                    && method != "RemoveFromCart"
                    && method != "ChangeAttributes"
                    && method != "ApplyCoupon"
                    && method != "RemoveCoupon"
                    && method != "ApplyVoucher"
                    && method != "RemoveVoucher"
                    && method != "CartToWishlist"
                    && method != "WishlistToCart"
                    && method != "RemoveFromWishlist"
                    && method != "SelectProductOption"
                    )
                {
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = "No method found with the " + method
                    }));
                    return;
                }

                if (method == "UpdateCart")
                {
                    Response.Write(CartHelper.UpdateCart(postParams));
                    return;
                }
                else if (method == "AddProductToCart_Details_Multi")
                {
                    string productIds = string.Empty;
                    if (postParams.Get("productids") != null)
                        productIds = postParams.Get("productids").ToString();
                    int affiliateUserIDs = -1;
                    if (postParams.Get("affiliateUserIDs") != null)
                        affiliateUserIDs = int.Parse(postParams.Get("affiliateUserIDs"));
                    if (!string.IsNullOrEmpty(productIds))
                    {
                        foreach (var id in productIds.Split(';'))
                        {
                            Product item = new Product(siteSettings.SiteId, Convert.ToInt32(id));
                            if (
                             item == null
                             || item.ProductId <= 0
                             || !item.IsPublished
                             || item.IsDeleted
                             )
                                continue;
                            CartHelper.AddProductToCart_Catalog(item, ShoppingCartTypeEnum.ShoppingCart, 1, forceredirection: false, affiliateUserIDs);
                        }

                        //Clear Context to get from database
                        HttpContext.Current.Items[CartHelper.GetContextKeyShoppingCart(siteSettings.SiteId, ShoppingCartTypeEnum.ShoppingCart)] = null;

                        var updateflyoutcartsectionhtml = ProductConfiguration.MiniShoppingCartEnabled
                            ? CartHelper.PrepareMiniShoppingCart(siteSettings.SiteId, Guid.Empty, ShoppingCartTypeEnum.ShoppingCart) : "";
                        var updatetopcartsectionhtml = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartQuantityFormat"),
                                                                       CartHelper.GetShoppingCart(siteSettings.SiteId, ShoppingCartTypeEnum.ShoppingCart)
                                                                       .GetTotalProducts());
                        Response.Write(StringHelper.ToJsonString(new
                        {
                            success = true,
                            updatetopcartsectionhtml,
                            updateflyoutcartsectionhtml,
                        }));
                    }
                    else
                        Response.Write(StringHelper.ToJsonString(new
                        {
                            success = false,
                            message = "No product found with the specified ID"
                        }));
                    return;
                }
                else if (method == "GetTotalByProductIds")
                {
                    string productIds = string.Empty;
                    if (postParams.Get("productids") != null)
                        productIds = postParams.Get("productids").ToString();
                    if (!string.IsNullOrEmpty(productIds))
                    {
                        Response.Write(StringHelper.ToJsonString(new
                        {
                            success = true,
                            data = ProductHelper.FormatPrice(ProductHelper.GetTotalPriceFromProductList(siteSettings.SiteId, productIds, true), true)
                        }));
                    }
                    else
                        Response.Write(StringHelper.ToJsonString(new
                        {
                            success = true,
                            data = ProductHelper.FormatPrice(0, true)
                        }));
                    return;
                }
                else if (method == "RemoveFromCart")
                {
                    Guid itemGuid = Guid.Empty;
                    if (postParams.Get("itemguid") != null)
                        Guid.TryParse(postParams.Get("itemguid"), out itemGuid);

                    Response.Write(CartHelper.RemoveFromCart(itemGuid, ShoppingCartTypeEnum.ShoppingCart));
                    return;
                }
                else if (method == "ApplyCoupon")
                {
                    string couponCode = string.Empty;
                    if (postParams.Get("couponcode") != null)
                        couponCode = postParams.Get("couponcode").ToString();
                    Response.Write(ApplyCoupon(couponCode));

                    return;
                }
                else if (method == "RemoveCoupon")
                {
                    Response.Write(RemoveCoupon());
                    return;
                }
                else if (method == "ApplyVoucher")
                {
                    Response.Write(ApplyVoucher());
                    return;
                }
                else if (method == "RemoveVoucher")
                {
                    Response.Write(RemoveVoucher());
                    return;
                }
                else if (method == "CartToWishlist")
                {
                    Response.Write(CartToWishlist());
                    return;
                }
                else if (method == "WishlistToCart")
                {
                    Response.Write(WishlistToCart());
                    return;
                }
                else if (method == "RemoveFromWishlist")
                {
                    Guid itemGuid = Guid.Empty;
                    if (postParams.Get("itemguid") != null)
                        Guid.TryParse(postParams.Get("itemguid"), out itemGuid);

                    Response.Write(CartHelper.RemoveFromCart(itemGuid, ShoppingCartTypeEnum.Wishlist));
                    return;
                }

                var productId = -1;
                var affiliateUserID = -1;
                var product = (Product)null;
                if (postParams.Get("productid") != null)
                    int.TryParse(postParams.Get("productid"), out productId);
                if (postParams.Get("affiliateuserid") != null)
                    int.TryParse(postParams.Get("affiliateuserid"), out affiliateUserID);
                product = new Product(siteSettings.SiteId, productId);

                if (
                    product == null
                    || product.ProductId <= 0
                    || !product.IsPublished
                    || product.IsDeleted
                    )
                {
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = "No product found with the specified ID"
                    }));
                    return;
                }

                switch (method)
                {
                    case "AddProductToWishlist":
                        Response.Write(CartHelper.AddProductToCart_Catalog(product, ShoppingCartTypeEnum.Wishlist, 1, forceredirection: false, affiliateUserID));
                        WishlistCacheHelper.ClearCache(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId, true));
                        break;

                    case "AddProductToCart_Catalog":
                        int quantity = 1;
                        if (postParams.Get("qty") != null)
                            int.TryParse(postParams.Get("qty"), out quantity);
                        Response.Write(CartHelper.AddProductToCart_Catalog(product, ShoppingCartTypeEnum.ShoppingCart, quantity, forceredirection: false, affiliateUserID));
                        break;

                    case "AddProductToCart_Details":
                        Response.Write(CartHelper.AddProductToCart_Details(product, ShoppingCartTypeEnum.ShoppingCart, postParams, forceredirection: false, affiliateUserID));
                        break;

                    case "ChangeAttributes":
                        Response.Write(ChangeAttributes(product, postParams));
                        break;

                    case "SelectProductOption":
                        Response.Write(CartHelper.SelectProductOption(product, postParams));
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);

                Response.Write(StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Failed to process from the server. Please refresh the page and try one more time."
                }));
            }
        }

        private string CartToWishlist()
        {
            var itemGuid = Guid.Empty;
            if (postParams.Get("itemguid") != null)
                Guid.TryParse(postParams.Get("itemguid"), out itemGuid);

            return CartHelper.ChangeCartType(siteSettings, itemGuid, ShoppingCartTypeEnum.ShoppingCart, ShoppingCartTypeEnum.Wishlist);
        }

        private string WishlistToCart()
        {
            var itemGuid = Guid.Empty;
            if (postParams.Get("itemguid") != null)
                Guid.TryParse(postParams.Get("itemguid"), out itemGuid);

            return CartHelper.ChangeCartType(siteSettings, itemGuid, ShoppingCartTypeEnum.Wishlist, ShoppingCartTypeEnum.ShoppingCart);
        }

        private string RemoveVoucher()
        {
            string voucherCode = string.Empty;
            if (postParams.Get("vouchercode") != null)
                voucherCode = postParams.Get("vouchercode").ToString();
            if (voucherCode.Length == 0)
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = Resources.ProductResources.VoucherCodeRequiredWarning
                }));
            }

            var codes = VoucherHelper.GetVoucherCodeApplied();
            if (!codes.Contains(voucherCode))
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = Resources.ProductResources.VoucherCodeNotExistsWarning
                }));
            }

            codes.Remove(voucherCode);

            VoucherHelper.SetVoucherCodeApply(codes);

            return StringHelper.ToJsonString((new
            {
                success = true,
                redirect = CartHelper.GetCartUrl()
            }));
        }

        private string ApplyVoucher()
        {
            string voucherCode = string.Empty;
            if (postParams.Get("vouchercode") != null)
                voucherCode = postParams.Get("vouchercode").ToString();
            if (voucherCode.Length == 0)
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = Resources.ProductResources.VoucherCodeRequiredWarning
                }));
            }

            VoucherHelper.VoucherIsValid(voucherCode, out string warning);
            if (warning.Length > 0)
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = warning
                }));
            }

            VoucherHelper.SetVoucherCodeApply(voucherCode);

            return StringHelper.ToJsonString((new
            {
                success = true,
                redirect = CartHelper.GetCartUrl()
            }));
        }

        private string RemoveCoupon()
        {
            CouponHelper.CouponCodeInput = null;

            return StringHelper.ToJsonString((new
            {
                success = true,
                redirect = CartHelper.GetCartUrl()
            }));
        }

        private string ApplyCoupon(string couponCode)
        {
            if (couponCode.Length == 0)
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = Resources.ProductResources.CouponCodeRequiredWarning
                }));
            }

            CouponHelper.CouponIsValid(couponCode, out string warning);
            if (warning.Length > 0)
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = warning
                }));
            }

            CouponHelper.CouponCodeInput = couponCode;

            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var cartType = ShoppingCartTypeEnum.ShoppingCart;
            var lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId), cartType);
            var cart = CartHelper.GetShoppingCart(siteSettings.SiteId, cartType);
            var coupon = (DiscountCoupon)null;
            var lstDiscountItems = (List<DiscountAppliedToItem>)null;
            var lstDiscountId = (List<int>)null;
            var giftDescription = string.Empty;
            var productGifts = string.Empty;
            var discountPayment = 0M;
            cart.GetDiscountTotal(lstProductsInCart, ref lstDiscountItems, ref lstDiscountId, ref giftDescription, ref productGifts, ref coupon, ref discountPayment);
            if (coupon == null)
            {
                return StringHelper.ToJsonString((new
                {
                    success = false,
                    message = "Mã khuyến mãi không đủ điều kiện áp dụng."
                }));
            }

            return StringHelper.ToJsonString((new
            {
                success = true,
                redirect = CartHelper.GetCartUrl()
            }));
        }

        private string ChangeAttributes(Product product, NameValueCollection form)
        {
            decimal price = ProductHelper.GetPrice(product);
            var productProperties = ProductProperty.GetPropertiesByProduct(product.ProductId);
            if (productProperties.Count > 0)
            {
                var customFieldIds = productProperties.Select(x => x.CustomFieldId).Distinct().ToList();
                var productAttributes = CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds);
                foreach (var attribute in productAttributes)
                {
                    if ((attribute.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0)
                    {
                        string controlId = string.Format("product_attribute_{0}_{1}", product.ProductId, attribute.CustomFieldId);
                        var ctrlAttributes = form[controlId];
                        if (!String.IsNullOrEmpty(ctrlAttributes))
                        {
                            int selectedAttributeId = int.Parse(ctrlAttributes);
                            productProperties.ForEach(x =>
                            {
                                if (x.CustomFieldOptionId == selectedAttributeId)
                                    price += x.OverriddenPrice;
                            });
                        }
                    }
                }
            }

            return StringHelper.ToJsonString(new
            {
                success = true,
                price = ProductHelper.FormatPrice(price, true)
            });
        }

        private void LoadParams()
        {
            // don't accept get requests
            if (Request.HttpMethod != "POST") { return; }

            postParams = HttpUtility.ParseQueryString(Request.GetRequestBody());

            if (postParams.Get("method") != null)
            {
                method = postParams.Get("method");
            }
        }
    }
}
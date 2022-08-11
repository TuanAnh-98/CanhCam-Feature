/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-30
/// Last Modified:			2015-07-17
/// 2015-10-22: Add manufacturer with attributes to cart

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public static class CartHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CartHelper));

        public static string GetContextKeyShoppingCart(int siteId, ShoppingCartTypeEnum shoppingCartType)
        {
            return "ShoppingCart_" + siteId.ToInvariantString() + "_" + shoppingCartType;
        }

        public static List<ShoppingCartItem> GetShoppingCart(int siteId, ShoppingCartTypeEnum shoppingCartType)
        {
            var cartSessionGuid = GetCartSessionGuid(siteId);

            if (HttpContext.Current == null) return ShoppingCartItem.GetByUserGuid(siteId, shoppingCartType, cartSessionGuid);

            string contextKey = GetContextKeyShoppingCart(siteId, shoppingCartType);

            if (!(HttpContext.Current.Items[contextKey] is List<ShoppingCartItem> lstShoppingCartItems))
            {
                lstShoppingCartItems = ShoppingCartItem.GetByUserGuid(siteId, shoppingCartType, cartSessionGuid);
                HttpContext.Current.Items[contextKey] = lstShoppingCartItems;
            }

            return lstShoppingCartItems;
        }

        public static Guid GetCartSessionGuid(int siteId, bool setCookieIfNotExists = false)
        {
            if (
                (HttpContext.Current != null)
                && (HttpContext.Current.Request.IsAuthenticated)
                )
            {
                SiteUser siteUser = SiteUtils.GetCurrentSiteUser();

                if (siteUser != null && siteUser.UserGuid != Guid.Empty)
                {
                    VerifyCartUser(siteUser);
                    return siteUser.UserGuid;
                }
            }

            string cartCookie = GetCartCookie(siteId);
            if (cartCookie.Length == 36)
                return new Guid(cartCookie);

            Guid cartGuid = Guid.NewGuid();
            if (setCookieIfNotExists)
                SetCartCookie(siteId, cartGuid);

            return cartGuid;
        }

        private static string GetOrderSessionKey(int siteId)
        {
            return "Order-" + siteId.ToString();
        }

        public static Order GetOrderSession(int siteId)
        {
            string orderKey = GetOrderSessionKey(siteId);
            if (HttpContext.Current.Session[orderKey] != null)
                return (Order)HttpContext.Current.Session[orderKey];

            return null;
        }

        public static void SetOrderSession(int siteId, Order order)
        {
            string orderKey = GetOrderSessionKey(siteId);
            if (order == null)
                HttpContext.Current.Session.Remove(orderKey);
            else
                HttpContext.Current.Session[orderKey] = order;
        }

        private static string GetOrderSavedSessionKey(int siteId)
        {
            return "OrderSaved-" + siteId.ToString();
        }

        public static Order GetOrderSavedSession(int siteId)
        {
            string orderKey = GetOrderSavedSessionKey(siteId);
            if (HttpContext.Current.Session[orderKey] != null)
                return (Order)HttpContext.Current.Session[orderKey];

            return null;
        }

        public static void SetOrderSavedSession(int siteId, Order order)
        {
            string orderKey = GetOrderSavedSessionKey(siteId);
            if (order == null)
            {
                HttpContext.Current.Session.Remove(orderKey);
                return;
            }

            var oldOrder = CartHelper.GetOrderSavedSession(order.SiteId);
            if (oldOrder != null && order != null)
                order.DiscountPayment = oldOrder.DiscountPayment;

            HttpContext.Current.Session[orderKey] = order;
        }

        private static string GetCartCookie(int siteId)
        {
            string cartKey = "cart" + siteId.ToString();

            // TODO: decrypt and verify?

            return CookieHelper.GetCookieValue(cartKey);
        }

        private static void SetCartCookie(int siteId, Guid cartGuid)
        {
            string cartKey = "cart" + siteId.ToString();

            // TODO encrypt, sign?

            CookieHelper.SetPersistentCookie(cartKey, cartGuid.ToString());
        }

        public static void ClearCartCookie(int siteId)
        {
            string cartKey = "cart" + siteId.ToString();

            CookieHelper.ExpireCookie(cartKey);
        }

        private static void VerifyCartUser(SiteUser currentUser)
        {
            string cartCookie = GetCartCookie(currentUser.SiteId);
            if (cartCookie.Length == 36)
            {
                Guid cartGuid = new Guid(cartCookie);

                ShoppingCartItem.MoveToUser(cartGuid, currentUser.UserGuid);
                ClearCartCookie(currentUser.SiteId);
            }
        }

        public static string AddProductToCart_Catalog(Product product, ShoppingCartTypeEnum cartType, int quantity, bool forceredirection = false, int affiliateUserID = -1)
        {
            //get standard warnings without attribute validations
            //first, try to find existing shopping cart order
            var cartSessionGuid = CartHelper.GetCartSessionGuid(product.SiteId, true);
            var cart = ShoppingCartItem.GetByUserGuid(product.SiteId, cartType, cartSessionGuid);
            var shoppingCartItem = CartHelper.FindShoppingCartItemInTheCart(cart, cartType, product);

            //if we already have the same manufacturer in the cart, then use the total quantity to validate
            var quantityToValidate = shoppingCartItem != null ? shoppingCartItem.Quantity + quantity : quantity;
            var addToCartWarnings = CartHelper.GetShoppingCartItemWarnings(cartSessionGuid, cartType,
                product, string.Empty, quantityToValidate, false, true, false);
            if (addToCartWarnings.Count > 0)
            {
                //cannot be added to the cart
                //let's display standard warnings
                return Json(new
                {
                    success = false,
                    message = addToCartWarnings.ToArray()
                });
            }

            //now let's try adding manufacturer to the cart (now including manufacturer attribute validation, etc)
            var shoppingCartItemGuid = Guid.Empty;
            addToCartWarnings = CartHelper.AddToCart(
                product: product,
                shoppingCartType: cartType,
                shoppingCartItemGuid: out shoppingCartItemGuid,
                quantity: quantity,
                affiliateUserID: affiliateUserID);
            if (addToCartWarnings.Count > 0)
            {
                //cannot be added to the cart
                //but we do not display attribute and gift card warnings here. let's do it on the manufacturer details page
                return Json(new
                {
                    redirect = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId),
                });
            }

            //added to the cart/wishlist
            switch (cartType)
            {
                case ShoppingCartTypeEnum.Wishlist:
                    {
                        string wishlistUrl = CartHelper.GetWishlistUrl();
                        if (ProductConfiguration.DisplayWishlistAfterAddingProduct || forceredirection)
                        {
                            //redirect to the wishlist page
                            return Json(new
                            {
                                redirect = wishlistUrl
                            });
                        }

                        //display notification message and update appropriate blocks
                        var updatetopwishlistsectionhtml = string.Format(ResourceHelper.GetResourceString("ProductResources", "WishlistQuantityFormat"),
                        CartHelper.GetShoppingCart(product.SiteId, ShoppingCartTypeEnum.Wishlist).GetTotalProducts());
                        return Json(new
                        {
                            success = true,
                            message = string.Format(ResourceHelper.GetResourceString("ProductResources", "WishlistProductHasBeenAddedFormat"), wishlistUrl),
                            updatetopwishlistsectionhtml,
                        });
                    }
                case ShoppingCartTypeEnum.ShoppingCart:
                default:
                    {
                        string cartUrl = CartHelper.GetCartUrl();
                        if (ProductConfiguration.DisplayCartAfterAddingProduct || forceredirection)
                        {
                            //redirect to the shopping cart page
                            return Json(new
                            {
                                redirect = cartUrl
                            });
                        }

                        //display notification message and update appropriate blocks
                        var updatetopcartsectionhtml = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartQuantityFormat"),
                        CartHelper.GetShoppingCart(product.SiteId, ShoppingCartTypeEnum.ShoppingCart)
                        .GetTotalProducts());

                        var updateflyoutcartsectionhtml = ProductConfiguration.MiniShoppingCartEnabled
                            ? PrepareMiniShoppingCart(product.SiteId, shoppingCartItemGuid, cartType) : "";

                        return Json(new
                        {
                            success = true,
                            message = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartProductHasBeenAddedFormat"), cartUrl),
                            updatetopcartsectionhtml,
                            updateflyoutcartsectionhtml,
                        });
                    }
            }
        }

        public static string AddProductToCart_Details(Product product, ShoppingCartTypeEnum cartType, NameValueCollection postParams, bool forceredirection = false, int affiliateUserID = -1)
        {
            ////we can add only simple products
            //if (manufacturer.ProductType != ProductType.SimpleProduct)
            //{
            //    return Json(new
            //    {
            //        success = false,
            //        message = "Only simple products could be added to the cart"
            //    });
            //}

            #region Update existing shopping cart order?

            Guid updatecartitemguid = Guid.Empty;
            foreach (string formKey in postParams)
                if (formKey.Equals(string.Format("addtocart_{0}.UpdatedShoppingCartItemId", product.ProductId), StringComparison.InvariantCultureIgnoreCase))
                {
                    if (postParams[formKey].Length == 36)
                        updatecartitemguid = new Guid(postParams[formKey]);
                    break;
                }
            ShoppingCartItem updatecartitem = null;
            if (ProductConfiguration.AllowCartItemEditing && updatecartitemguid != Guid.Empty)
            {
                var cart = CartHelper.GetShoppingCart(product.SiteId, ShoppingCartTypeEnum.ShoppingCart);
                updatecartitem = cart.FirstOrDefault(x => x.Guid == updatecartitemguid);
                //not found?
                if (updatecartitem == null)
                {
                    return Json(new
                    {
                        success = false,
                        message = "No shopping cart item found to update"
                    });
                }
                //is it this manufacturer?
                if (product.ProductId != updatecartitem.ProductId)
                {
                    return Json(new
                    {
                        success = false,
                        message = "This product does not match a passed shopping cart item identifier"
                    });
                }
            }

            #endregion Update existing shopping cart order?

            #region Customer entered price

            //decimal customerEnteredPriceConverted = decimal.Zero;
            //if (manufacturer.CustomerEntersPrice)
            //{
            //    foreach (string formKey in postParams)
            //    {
            //        if (formKey.Equals(string.Format("addtocart_{0}.CustomerEnteredPrice", manufacturerId), StringComparison.InvariantCultureIgnoreCase))
            //        {
            //            decimal customerEnteredPrice;
            //            if (decimal.TryParse(postParams[formKey], out customerEnteredPrice))
            //                customerEnteredPriceConverted = _currencyService.ConvertToPrimaryStoreCurrency(customerEnteredPrice, _workContext.WorkingCurrency);
            //            break;
            //        }
            //    }
            //}

            #endregion Customer entered price

            #region Quantity

            int quantity = 1;

            if (postParams.Get(string.Format("addtocart_{0}.EnteredQuantity", product.ProductId)) != null)
            {
                int.TryParse(postParams.Get(string.Format("addtocart_{0}.EnteredQuantity", product.ProductId)), out quantity);
            }

            #endregion Quantity

            //manufacturer and gift card attributes
            string attributes = ProductAttributeParser.ParseProductAttributes(product, postParams); //color, sizes

            ////rental attributes
            //DateTime? rentalStartDate = null;
            //DateTime? rentalEndDate = null;
            //if (manufacturer.IsRental)
            //{
            //    ParseRentalDates(manufacturer, form, out rentalStartDate, out rentalEndDate);
            //}

            //save order
            var addToCartWarnings = new List<string>();
            var shoppingCartItemGuid = Guid.Empty;
            if (updatecartitem == null)
            {
                //add to the cart
                addToCartWarnings.AddRange(CartHelper.AddToCart(
                    product, cartType, out shoppingCartItemGuid,
                    attributes, quantity, true, affiliateUserID));
            }
            else
            {
                var cart = CartHelper.GetShoppingCart(product.SiteId, ShoppingCartTypeEnum.ShoppingCart);
                var otherCartItemWithSameParameters = CartHelper.FindShoppingCartItemInTheCart(
                    cart, cartType, product, attributes);
                if (otherCartItemWithSameParameters != null &&
                    otherCartItemWithSameParameters.Guid == updatecartitem.Guid)
                {
                    //ensure it's other shopping cart cart order
                    otherCartItemWithSameParameters = null;
                }
                //update existing order
                addToCartWarnings.AddRange(UpdateShoppingCartItem(CartHelper.GetCartSessionGuid(product.SiteId),
                    updatecartitem.Guid, attributes, quantity));
                if (otherCartItemWithSameParameters != null && addToCartWarnings.Count == 0)
                {
                    //delete the same shopping cart order (the other one)
                    ShoppingCartItem.Delete(otherCartItemWithSameParameters.Guid);
                }
            }

            #region Return result

            if (addToCartWarnings.Count > 0)
            {
                //cannot be added to the cart/wishlist
                //let's display warnings
                return Json(new
                {
                    success = false,
                    message = addToCartWarnings.ToArray()
                });
            }

            //added to the cart/wishlist
            switch (cartType)
            {
                case ShoppingCartTypeEnum.Wishlist:
                    {
                        string wishlistUrl = CartHelper.GetWishlistUrl();
                        if (ProductConfiguration.DisplayWishlistAfterAddingProduct || forceredirection)
                        {
                            //redirect to the wishlist page
                            return Json(new
                            {
                                redirect = wishlistUrl
                            });
                        }

                        //display notification message and update appropriate blocks
                        var updatetopwishlistsectionhtml = string.Format(ResourceHelper.GetResourceString("ProductResources", "WishlistQuantityFormat"),
                        CartHelper.GetShoppingCart(product.SiteId, ShoppingCartTypeEnum.Wishlist).GetTotalProducts());
                        return Json(new
                        {
                            success = true,
                            message = string.Format(ResourceHelper.GetResourceString("ProductResources", "WishlistProductHasBeenAddedFormat"), wishlistUrl),
                            updatetopwishlistsectionhtml
                        });
                    }
                case ShoppingCartTypeEnum.ShoppingCart:
                default:
                    {
                        string cartUrl = CartHelper.GetCartUrl();
                        if (ProductConfiguration.DisplayCartAfterAddingProduct || forceredirection)
                        {
                            //redirect to the shopping cart page
                            return Json(new
                            {
                                redirect = cartUrl
                            });
                        }

                        //display notification message and update appropriate blocks
                        var updatetopcartsectionhtml = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartQuantityFormat"),
                        CartHelper.GetShoppingCart(product.SiteId, ShoppingCartTypeEnum.ShoppingCart).GetTotalProducts());

                        var updateflyoutcartsectionhtml = ProductConfiguration.MiniShoppingCartEnabled
                            ? PrepareMiniShoppingCart(product.SiteId, shoppingCartItemGuid, cartType) : "";

                        return Json(new
                        {
                            success = true,
                            message = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartProductHasBeenAddedFormat"), cartUrl),
                            updatetopcartsectionhtml,
                            updateflyoutcartsectionhtml,
                            cartpageurl = cartUrl
                        });
                    }
            }

            #endregion Return result
        }

        public static string PrepareMiniShoppingCart(int siteId, Guid cartItemGuid, ShoppingCartTypeEnum cartType)
        {
            string xsltPath = SiteUtils.GetXsltBasePath("product", "ShoppingCartFlyout.xslt");
            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(xsltPath)))
                return string.Empty;

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            BuildShoppingCartXml(siteId, cartItemGuid, out doc, cartType);

            return XmlHelper.TransformXML(xsltPath, doc);
        }

        #region XmlData

        public static XmlElement BuildShoppingCartXml(int siteId, Guid lastAddedItemGuid, out XmlDocument doc, ShoppingCartTypeEnum cartType)
        {
            doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<ShoppingCart></ShoppingCart>");
            XmlElement root = doc.DocumentElement;

            var lstProductsInCart = Product.GetByShoppingCart(siteId, CartHelper.GetCartSessionGuid(siteId), cartType, WorkingCulture.LanguageId);
            var cart = GetShoppingCart(siteId, cartType);
            var cartUrl = GetCartUrl();
            var cartEmptyText = Resources.ProductResources.CartIsEmptyLabel;
            var cartItemsText = string.Format(Resources.ProductResources.CartItemsFormat, cartUrl, cart.GetTotalProducts());

            XmlHelper.AddNode(doc, root, "CartPageUrl", cartUrl);
            XmlHelper.AddNode(doc, root, "CartEmptyText", cartEmptyText);
            XmlHelper.AddNode(doc, root, "CartItemsText", cartItemsText);
            XmlHelper.AddNode(doc, root, "CartText", Resources.ProductResources.CartLabel);
            XmlHelper.AddNode(doc, root, "SubTotalText", Resources.ProductResources.CartSubTotalLabel);
            XmlHelper.AddNode(doc, root, "TotalText", Resources.ProductResources.CartTotalLabel);
            XmlHelper.AddNode(doc, root, "QuantityText", Resources.ProductResources.CartQuantityLabel);
            XmlHelper.AddNode(doc, root, "UnitPriceText", Resources.ProductResources.CartUnitPriceLabel);
            XmlHelper.AddNode(doc, root, "PriceText", Resources.ProductResources.CartPriceLabel);
            XmlHelper.AddNode(doc, root, "RemoveText", Resources.ProductResources.CartRemoveLabel);
            XmlHelper.AddNode(doc, root, "ItemTotalText", Resources.ProductResources.CartItemTotalLabel);
            XmlHelper.AddNode(doc, root, "ImageText", Resources.ProductResources.CartImageLabel);
            XmlHelper.AddNode(doc, root, "ProductText", Resources.ProductResources.CartProductLabel);
            XmlHelper.AddNode(doc, root, "UpdateCartText", Resources.ProductResources.CartUpdateLabel);
            XmlHelper.AddNode(doc, root, "ContinueShoppingText", Resources.ProductResources.CartContinueShoppingLabel);
            XmlHelper.AddNode(doc, root, "ContinueShoppingUrl", GetContinueShopping());
            XmlHelper.AddNode(doc, root, "CouponCodeText", Resources.ProductResources.CartCouponCodeLabel);
            XmlHelper.AddNode(doc, root, "CouponApplyText", Resources.ProductResources.CartCouponApplyLabel);
            XmlHelper.AddNode(doc, root, "ShippingTotalText", Resources.ProductResources.CartShippingTotalLabel);
            XmlHelper.AddNode(doc, root, "CheckoutText", Resources.ProductResources.CartCheckoutLabel);
            XmlHelper.AddNode(doc, root, "CheckoutProcessText", Resources.ProductResources.CartCheckoutProcessLabel);
            XmlHelper.AddNode(doc, root, "DiscountText", Resources.ProductResources.CartDiscountLabel);

            if (cart.GetTotalProducts() > 0)
                XmlHelper.AddNode(doc, root, "CartSummaryText", cartItemsText);
            else
                XmlHelper.AddNode(doc, root, "CartSummaryText", cartEmptyText);

            var languageId = WorkingCulture.LanguageId;
            var subTotal = cart.GetSubTotal(lstProductsInCart);
            //var couponIsAvailable = false;
            var coupon = (DiscountCoupon)null;
            var lstDiscountItems = (List<DiscountAppliedToItem>)null;
            var lstDiscountId = (List<int>)null;
            var giftDescription = string.Empty;
            var productGifts = string.Empty;
            var discountPayment = 0M;
            var discountTotal = cart.GetDiscountTotal(lstProductsInCart, ref lstDiscountItems, ref lstDiscountId, ref giftDescription, ref productGifts, ref coupon, ref discountPayment);
            var discountCouponTotal = coupon != null ? coupon.DiscountAmount : 0;

            if (ProductConfiguration.EnableSaveDiscountPayment && discountPayment > 0)
                discountTotal += discountPayment;
            // Custom fields
            var productProperties = new List<ProductProperty>();
            var customFields = new List<CustomField>();
            if (ProductConfiguration.EnableShoppingCartAttributes)
            {
                var lstProductIds = cart.Select(x => x.ProductId).Distinct().ToList();
                customFields = CustomField.GetActiveForCart(siteId, Product.FeatureGuid);
                if (customFields.Count > 0 && lstProductIds.Count > 0)
                    productProperties = ProductProperty.GetPropertiesByProducts(lstProductIds);
            }

            var hasGift = !string.IsNullOrEmpty(giftDescription);

            foreach (var cartItem in cart)
            {
                var cartItemXml = doc.CreateElement("CartItem");
                root.AppendChild(cartItemXml);

                var product = ProductHelper.GetProductFromList(lstProductsInCart, cartItem.ProductId);
                if (product != null)
                {
                    ProductHelper.BuildProductDataXml(doc, cartItemXml, product, lstDiscountItems);
                    BuildCartItemDataXml(doc, cartItemXml, cartItem, product, cart,
                        customFields, productProperties, lastAddedItemGuid, languageId, subTotal, discountTotal);
                    if (cartItem.GiftMessage.Length > 0)
                        hasGift = true;
                }
            }

            XmlHelper.AddNode(doc, root, "HasGift", hasGift.ToString().ToLower());
            XmlHelper.AddNode(doc, root, "GiftDescription", giftDescription);

            #region Coupon

            if (coupon != null)
            {
                XmlHelper.AddNode(doc, root, "CouponCode", coupon.CouponCode);
                XmlHelper.AddNode(doc, root, "CouponTotal", ProductHelper.FormatPrice(-discountCouponTotal, true));
            }

            #endregion Coupon

            #region Voucher

            decimal voucherAmount = VoucherHelper.GetVoucherAmountTotal();

            #endregion Voucher

            decimal serviceFee = decimal.Zero;
            if (ProductServiceHelper.Enable)
                serviceFee = ProductServiceHelper.GetServiceFee(lstProductsInCart, cart, subTotal);

            var shippingTotal = decimal.Zero;

            var totalTemp = GetCartTotal(subTotal: subTotal,
              shippingTotal: shippingTotal,
              discountTotal: discountTotal,
              discountCouponTotal: discountCouponTotal,
              serviceFee: serviceFee);
            if (totalTemp <= voucherAmount)
                voucherAmount = totalTemp;

            var total = GetCartTotal(subTotal: subTotal,
                shippingTotal: shippingTotal,
                discountTotal: discountTotal,
                discountCouponTotal: discountCouponTotal,
                voucherAmount: voucherAmount,
                serviceFee: serviceFee);

            #region Points

            var total2 = GetCartTotal(subTotal, shippingTotal: 0, discountTotal: 0);
            bool pointIsAvailable = false;
            var siteUser = SiteUtils.GetCurrentSiteUser();
            decimal rewardPointTotal = 0;
            if (RewardPointsHelper.Enable && siteUser != null && siteUser.UserId > 0)
            {
                RewardPointsHelper.GetRewardPointsBalance(siteUser, out decimal pointBalance, out decimal pointsAvallable);
                if (pointsAvallable > 0)
                {
                    pointIsAvailable = true;

                    int maxPointCanUse = RewardPointsHelper.CalculateRewardPoints(siteUser, total - shippingTotal);
                    if (pointsAvallable > maxPointCanUse)
                        XmlHelper.AddNode(doc, root, "RewardPointsAvailable", maxPointCanUse.ToString());
                    else
                        XmlHelper.AddNode(doc, root, "RewardPointsAvailable", pointsAvallable.ToString());

                    XmlHelper.AddNode(doc, root, "RewardPointsBalance", pointBalance.ToString());
                    XmlHelper.AddNode(doc, root, "RewardPointsAvallable", pointsAvallable.ToString());
                    var order = CartHelper.GetOrderSession(siteId);
                    if (order != null)
                    {
                        XmlHelper.AddNode(doc, root, "RewardPointPoint", order.RedeemedRewardPoints.ToString());
                        rewardPointTotal = order.RedeemedRewardPointsAmount;
                        if (rewardPointTotal > 0)
                            total -= rewardPointTotal;
                        if (total < 0)
                            total = 0;
                    }
                }
                XmlHelper.AddNode(doc, root, "SpendingPointsInfoText", string.Format(ProductResources.SpendingPointsInfoText,
                   ProductHelper.FormatPrice(RewardPointsHelper.PointsForPurchases_Amount, true), 1));
                XmlHelper.AddNode(doc, root, "RewardPointInfoText", string.Format(ProductResources.RewardPointInfoText,
                     RewardPointsHelper.CalculateRewardPoints(siteUser, total - shippingTotal)));
            }
            XmlHelper.AddNode(doc, root, "RewardPointTotal", ProductHelper.FormatPrice(rewardPointTotal, true));
            XmlHelper.AddNode(doc, root, "PointIsAvailable", pointIsAvailable.ToString());

            #endregion Points

            XmlHelper.AddNode(doc, root, "TotalProducts", cart.GetTotalProducts().ToString());
            XmlHelper.AddNode(doc, root, "SubTotal", ProductHelper.FormatPrice(subTotal, true));

            XmlHelper.AddNode(doc, root, "ServiceFeeEnable", ProductServiceHelper.Enable.ToString());
            XmlHelper.AddNode(doc, root, "ServiceFee", ProductHelper.FormatPrice(serviceFee, true));
            XmlHelper.AddNode(doc, root, "ShippingTotal", ProductHelper.FormatPrice(shippingTotal, true));
            //if (discountCouponTotal > 0)
            //    discountTotal += discountCouponTotal;
            XmlHelper.AddNode(doc, root, "DiscountTotal", ProductHelper.FormatPrice(-discountTotal, true));
            XmlHelper.AddNode(doc, root, "DiscountPaymentTotal", ProductHelper.FormatPrice(-discountPayment, true));
            XmlHelper.AddNode(doc, root, "DiscountPaymentValue", ProductHelper.FormatPrice(discountPayment));
            if (voucherAmount > 0)
            {
                XmlHelper.AddNode(doc, root, "VoucherTotal", ProductHelper.FormatPrice(-voucherAmount, true));
                XmlHelper.AddNode(doc, root, "VoucherTotalValue", voucherAmount.ToString());
            }
            XmlHelper.AddNode(doc, root, "Total", ProductHelper.FormatPrice(total, true));
            XmlHelper.AddNode(doc, root, "TotalWeights", cart.GetTotalWeights(lstProductsInCart).ToString());
            return root;
        }

        public static decimal GetCartTotal(
            decimal subTotal, //Tiền hàng
            decimal shippingTotal = decimal.Zero, // Phí ship
            decimal paymentTotal = decimal.Zero, //Phí thanh toán
            decimal discountTotal = decimal.Zero, // giảm giá khuyến mãi
            decimal discountCouponTotal = decimal.Zero, // Giảm giá coupon
            decimal rewardPointAmount = decimal.Zero, // đổi điểm
            decimal voucherAmount = decimal.Zero, // thanh toán = voucher
            decimal serviceFee = decimal.Zero, // phí dịch vụ
            decimal tax = decimal.Zero  // Thuế
            )
        {
            decimal totalWithOutShippingFee = subTotal
                                              + tax
                                              + serviceFee
                                              + paymentTotal
                                              - discountTotal
                                              - discountCouponTotal
                                              - rewardPointAmount
                                              - voucherAmount;

            if (totalWithOutShippingFee < 0)
                totalWithOutShippingFee = 0;
            //phí ship không áp dụng voucher
            decimal total = totalWithOutShippingFee + shippingTotal;

            //decimal total = subTotal + tax + shippingTotal +
            //                  paymentTotal - discountTotal - discountCouponTotal - rewardPointAmount - voucherAmount
            //                + serviceFee;

            return total;
        }

        public static string GetContinueShopping()
        {
            string returnUrl = CartHelper.LastContinueShoppingPage;
            if (string.IsNullOrEmpty(returnUrl))
                returnUrl = SiteUtils.GetHomepageUrl();

            return returnUrl;
        }

        private static XmlDocument BuildCartItemDataXml(XmlDocument doc, XmlElement cartItemXml, ShoppingCartItem cartItem,
                Product product, List<ShoppingCartItem> cartList,
                List<CustomField> customFields, List<ProductProperty> productProperties,
                Guid cartItemGuid, int languageId, decimal subTotal, decimal discountTotal)
        {
            var originalPrice = ProductHelper.GetOriginalPrice(product);
            if (!string.IsNullOrEmpty(cartItem.AttributesXml))
            {
                var attributes = ProductAttributeParser.ParseProductAttributeMappings(customFields, cartItem.AttributesXml);
                if (attributes.Count > 0)
                {
                    foreach (var a in attributes)
                    {
                        XmlElement attributeXml = doc.CreateElement("Attributes");
                        cartItemXml.AppendChild(attributeXml);

                        XmlHelper.AddNode(doc, attributeXml, "FieldId", a.CustomFieldId.ToString());
                        XmlHelper.AddNode(doc, attributeXml, "ItemGuid", cartItem.Guid.ToString());
                        XmlHelper.AddNode(doc, attributeXml, "Title", a.Name);

                        var values = ProductAttributeParser.ParseValues(cartItem.AttributesXml, a.CustomFieldId);
                        foreach (ProductProperty property in productProperties)
                        {
                            if (property.ProductId == product.ProductId && property.CustomFieldId == a.CustomFieldId)
                            {
                                XmlElement optionXml = doc.CreateElement("Options");
                                attributeXml.AppendChild(optionXml);

                                XmlHelper.AddNode(doc, optionXml, "FieldId", a.CustomFieldId.ToString());
                                XmlHelper.AddNode(doc, optionXml, "ItemGuid", cartItem.Guid.ToString());
                                XmlHelper.AddNode(doc, optionXml, "OptionId", property.CustomFieldOptionId.ToString());
                                XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);

                                if (values.Contains(property.CustomFieldOptionId))
                                {
                                    originalPrice += property.OverriddenPrice;

                                    XmlHelper.AddNode(doc, optionXml, "IsActive", "true");
                                }
                            }
                        }
                    }
                }
            }

            XmlHelper.AddNode(doc, cartItemXml, "Quantity", cartItem.Quantity.ToString());
            XmlHelper.AddNode(doc, cartItemXml, "ItemGuid", cartItem.Guid.ToString());
            XmlHelper.AddNode(doc, cartItemXml, "LastAddedItem", (cartItem.Guid == cartItemGuid).ToString().ToLower());
            XmlHelper.AddNode(doc, cartItemXml, "OriginalPrice", ProductHelper.FormatPrice(originalPrice, true));
            XmlHelper.AddNode(doc, cartItemXml, "ItemSubTotal", ProductHelper.FormatPrice(originalPrice * cartItem.Quantity, true));

            decimal serviceFee = 0;
            if (ProductServiceHelper.Enable)
                serviceFee = ProductServiceHelper.GetServiceFee(product, cartItem, subTotal - discountTotal);
            XmlHelper.AddNode(doc, cartItemXml, "ServiceFee", ProductHelper.FormatPrice(serviceFee, true));
            XmlHelper.AddNode(doc, cartItemXml, "ItemTotal", ProductHelper.FormatPrice(originalPrice * cartItem.Quantity - cartItem.ItemDiscount + serviceFee, true));

            if (cartItem.GiftMessage.Length > 0)
                XmlHelper.AddNode(doc, cartItemXml, "GiftDescription", cartItem.GiftMessage);

            if (cartItem.ItemDiscount > 0)
            {
                //var itemDiscountAmount = cartItem.ItemDiscount / cartItem.Quantity;
                var itemDiscountAmount = cartItem.ItemDiscount;
                var discountMessage = PromotionsHelper.GetDiscountAmountMessage(itemDiscountAmount, originalPrice);
                var discountAmount = ProductHelper.FormatPrice(itemDiscountAmount, true);
                var discountPercentage = PromotionsHelper.CalculatePercentage(itemDiscountAmount, originalPrice).ToString();

                if (cartItemXml["DiscountMessage"] != null)
                    cartItemXml["DiscountMessage"].InnerText = discountMessage;
                else
                    XmlHelper.AddNode(doc, cartItemXml, "DiscountMessage", discountMessage);

                if (cartItemXml["DiscountAmount"] != null)
                    cartItemXml["DiscountAmount"].InnerText = discountAmount;
                else
                    XmlHelper.AddNode(doc, cartItemXml, "DiscountAmount", discountAmount);

                if (cartItemXml["DiscountPercentage"] != null)
                    cartItemXml["DiscountPercentage"].InnerText = discountPercentage;
                else
                    XmlHelper.AddNode(doc, cartItemXml, "DiscountPercentage", discountPercentage);
            }

            if (!string.IsNullOrEmpty(ProductConfiguration.AllowedQuantities))
            {
                ParseAllowedQuantities(ProductConfiguration.AllowedQuantities)
                    .ForEach(quantity =>
                    {
                        XmlElement quantityXml = doc.CreateElement("Quantities");
                        cartItemXml.AppendChild(quantityXml);
                        XmlHelper.AddNode(doc, cartItemXml, "Quantity", quantity.ToString());
                    });
            }

            return doc;
        }

        private static List<int> ParseAllowedQuantities(string allowedQuantities)
        {
            var result = new List<int>();
            if (!string.IsNullOrWhiteSpace(allowedQuantities))
            {
                allowedQuantities
                    .SplitOnCharAndTrim(';')
                    .ForEach(qtyStr =>
                    {
                        if (!qtyStr.Contains("-"))
                        {
                            if (int.TryParse(qtyStr.Trim(), out int qty))
                                result.Add(qty);
                        }
                        else
                        {
                            var allowedRanges = allowedQuantities.SplitOnCharAndTrim('-');
                            if (allowedRanges.Count == 2)
                            {
                                if (int.TryParse(allowedRanges[0], out int qty1) && int.TryParse(allowedRanges[2], out int qty2))
                                    for (int i = qty1; i <= qty2; i++)
                                    {
                                        result.Add(i);
                                    }
                            }
                        }
                    });
            }

            return result;
        }

        #endregion XmlData

        // using dynamic
        public static string Json(this object result)
        {
            return StringHelper.ToJsonString(result);
        }

        /// <summary>
        /// Updates the shopping cart order
        /// </summary>
        /// <param name="cartItemGuid">Cart Item Guid</param>
        /// <param name="shoppingCartItemGuid">Shopping cart order identifier</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="quantity">New shopping cart order quantity</param>
        /// <param name="resetCheckoutData">A value indicating whether to reset checkout data</param>
        /// <returns>Warnings</returns>
        public static IList<string> UpdateShoppingCartItem(
            Guid cartSessionGuid,
            Guid shoppingCartItemGuid, string attributesXml,
            int quantity = 1)
        {
            //    var warnings = new List<string>();

            //    var shoppingCartItem = new ShoppingCartItem(shoppingCartItemGuid);
            //    if (shoppingCartItem != null && shoppingCartItem.Guid != Guid.Empty)
            //    {
            //        if (quantity > 0)
            //        {
            //            ////check warnings
            //            //warnings.AddRange(GetShoppingCartItemWarnings(customer, shoppingCartItem.ShoppingCartType,
            //            //    shoppingCartItem.Product, shoppingCartItem.StoreId,
            //            //    selectedAttributes, customerEnteredPrice, quantity, false));
            //            if (warnings.Count == 0)
            //            {
            //                //if everything is OK, then update a shopping cart order
            //                shoppingCartItem.Quantity = quantity;
            //                shoppingCartItem.AttributesXml = selectedAttributes;
            //                shoppingCartItem.LastModUtc = DateTime.UtcNow;
            //                shoppingCartItem.Save();
            //            }
            //        }
            //        else
            //        {
            //            //delete a shopping cart order
            //            //DeleteShoppingCartItem(shoppingCartItem, true);
            //            ShoppingCartItem.Delete(shoppingCartItem.Guid);
            //        }
            //    }

            //    return warnings;

            if (cartSessionGuid == Guid.Empty)
                throw new ArgumentNullException("cartSessionGuid");

            var warnings = new List<string>();

            var shoppingCartItem = new ShoppingCartItem(shoppingCartItemGuid);
            if (shoppingCartItem != null && shoppingCartItem.UserGuid == cartSessionGuid)
            {
                if (quantity > 0)
                {
                    //check warnings
                    warnings.AddRange(GetShoppingCartItemWarnings(cartSessionGuid, (ShoppingCartTypeEnum)shoppingCartItem.ShoppingCartType,
                        shoppingCartItem.Product,
                        attributesXml, quantity, false));
                    if (warnings.Count == 0)
                    {
                        //if everything is OK, then update a shopping cart order
                        shoppingCartItem.Quantity = quantity;
                        shoppingCartItem.AttributesXml = attributesXml;
                        shoppingCartItem.LastModUtc = DateTime.UtcNow;

                        shoppingCartItem.Save();
                    }
                }
                else
                {
                    //delete a shopping cart order
                    ShoppingCartItem.Delete(shoppingCartItem.Guid);
                }
            }

            return warnings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="cartList"></param>
        /// <param name="lstProductsInCart"></param>
        /// <param name="lstDiscountItems"></param>
        /// <param name="lstDiscountId"></param>
        /// <param name="giftDescription">Nội dung HTML chỉ sử dụng để hiển thị ở product item, cart summary</param>
        /// <param name="productGifts">format có dạng {ProductId}x{Quantity}</param>
        /// <param name="coupon"></param>
        /// <param name="paymentMethodId"></param>
        /// <returns></returns>
        public static decimal GetDiscountTotal(this List<ShoppingCartItem> cartList, List<Product> lstProductsInCart,
                                               ref List<DiscountAppliedToItem> lstDiscountItems,
                                               ref List<int> lstDiscountId, ref string giftDescription, ref string productGifts,
                                               ref DiscountCoupon coupon, ref decimal paymentDiscount, int paymentMethodId = -1)
        {
            if (cartList == null || lstProductsInCart == null || cartList.Count == 0 || lstProductsInCart.Count == 0)
                return decimal.Zero;

            // discount catalog
            // hot deal
            // product range
            // combo sale
            // order range
            // coupon
            // payment

            var siteId = lstProductsInCart[0].SiteId;
            lstDiscountItems = DiscountAppliedToItem.GetActive(siteId, -1, lstProductsInCart);
            lstDiscountId = new List<int>();

            //var lstComboItems = lstDiscountItems.Where(s => s.DiscountType == (int)DiscountType.ComboSale).ToList();

            //var lstCouponAppliedToItems = new List<DiscountAppliedToItem>();
            //var coupon = (DiscountCoupon)null;
            var appliedCoupon = false;
            if (!string.IsNullOrEmpty(CouponHelper.CouponCodeInput))
            {
                var warning = string.Empty;
                coupon = CouponHelper.CouponIsValid(CouponHelper.CouponCodeInput, out warning);
                //if (coupon != null)
                //{
                //    //lstCouponAppliedToItems = lstDiscountItems.Where(s => s.DiscountId == coupon.DiscountID).ToList();
                //    if (lstCouponAppliedToItems.Count > 0 && coupon.Discount != null)
                //        coupon.Discount.DiscountAmount = 0;
                //}
            }

            var discountTotal = decimal.Zero;
            var lstPaymentItems = new List<DiscountAppliedToItem>();
            var discountItemApplied = false;
            var subTotalOriginal = cartList.GetSubTotal(lstProductsInCart);
            var hasItemCouponLimit = false;
            var subTotalCoupon = decimal.Zero;

            try
            {
                foreach (var sci in cartList)
                {
                    if ((sci.ItemDiscount <= 0 && string.IsNullOrEmpty(sci.GiftMessage)) || sci.ComboDiscountPriority >= 0)
                    {
                        var oldItemDiscount = sci.ItemDiscount;

                        var product = ProductHelper.GetProductFromList(lstProductsInCart, sci.ProductId);
                        if (product != null)
                        {
                            // by catalog, deal, product range, combo

                            var lstDiscountItemByProduct = new List<DiscountAppliedToItem>();
                            var lstCouponItemByProduct = new List<DiscountAppliedToItem>();
                            //var lstPaymentItemByProduct = new List<DiscountAppliedToItem>();
                            var lstDiscountItemByProductCombo = new List<DiscountAppliedToItem>();

                            //if (lstDiscountItemByProductCombo.Count > 0)
                            //{
                            //    lstDiscountItemByProductCombo.ForEach(s =>
                            //    {
                            //        if (s.ItemId == product.ProductId)
                            //            lstDiscountItemByProduct.Add(s);
                            //    });
                            //}

                            var price = ProductHelper.GetOriginalPrice(product);
                            var minPrice = price;
                            var itemDiscount = decimal.Zero;
                            var itemGift = string.Empty;
                            var isSpecialDiscount = false;
                            var comboCount = 1;
                            var productGift = string.Empty;

                            foreach (var item in lstDiscountItems)
                            {
                                if (IsDiscountAppliedItemValid(item, product))
                                {
                                    if (item.DiscountType == (int)DiscountType.ByCatalog)
                                    {
                                        if (!ProductHelper.ContainsDiscount(lstDiscountItemByProduct, item.DiscountId))
                                            lstDiscountItemByProduct.Add(item);
                                    }
                                    else if (item.DiscountType == (int)DiscountType.Deal)
                                    {
                                        if (
                                                item.DiscountType == (int)DiscountType.Deal
                                                && item.AppliedType == (int)DiscountAppliedType.ToProducts
                                            )
                                        {
                                            if (item.DealQty > 0 && item.SoldQty >= item.DealQty) continue;

                                            if (item.FromDate.HasValue && item.FromDate.Value > DateTime.Now) continue;
                                            if (item.ToDate.HasValue && item.ToDate.Value < DateTime.Now) continue;

                                            if (!ProductHelper.ContainsDiscount(lstDiscountItemByProduct, item.DiscountId))
                                                lstDiscountItemByProduct.Add(item);
                                        }
                                    }
                                    else if (item.DiscountType == (int)DiscountType.ByProductRange)
                                    {
                                        var productRange = DiscountRange.GetActive(siteId, product.ProductId, sci.Quantity, -1);
                                        if (productRange != null)
                                        {
                                            item.DiscountAmount = productRange.DiscountAmount;
                                            item.UsePercentage = productRange.DiscountType == 1;
                                            item.GiftHtml = productRange.GiftHtml;
                                            item.ProductGifts = productRange.ProductGifts;
                                            if (!ProductHelper.ContainsDiscount(lstDiscountItemByProduct, item.DiscountId))
                                            {
                                                lstDiscountItemByProduct.Add(item);
                                                //item.SpecialDiscountType = 2;
                                            }
                                        }
                                    }
                                    else if (item.DiscountType == (int)DiscountType.ComboSale)
                                    {
                                        if (sci.ComboDiscountPriority < 0 || (sci.ComboDiscountPriority > 0 && item.DiscountPriority > sci.ComboDiscountPriority))
                                        {
                                            var dicComboProduct = PromotionsHelper.ParseComboSaleValues(item.ComboSaleRules);
                                            dicComboProduct.Add(item);

                                            var comboItem2 = dicComboProduct.Where(x => x.ComboSaleQty <= sci.Quantity && x.ItemId == product.ProductId).FirstOrDefault();
                                            if (comboItem2 != null)
                                            {
                                                var maxComboCount = comboCount;
                                                if (comboItem2.ComboSaleQty > 0)
                                                    maxComboCount = (int)(sci.Quantity / comboItem2.ComboSaleQty);

                                                dicComboProduct.Remove(comboItem2);

                                                var found = true;
                                                var lstDiscountAppliedItem2 = new List<DiscountAppliedToItem>();
                                                dicComboProduct.ForEach(comboItem3 =>
                                                {
                                                    var obj = cartList.Where(cartItem => cartItem.ProductId == comboItem3.ItemId
                                                                                && comboItem3.ComboSaleQty <= cartItem.Quantity).FirstOrDefault();
                                                    if (obj == null)
                                                        found = false;
                                                    else if (comboItem3.DiscountAmount > 0)
                                                    {
                                                        comboItem3.ItemId = obj.ProductId;
                                                        comboItem3.DiscountId = item.DiscountId;
                                                        comboItem3.AppliedType = (int)DiscountAppliedType.ToProducts;
                                                        comboItem3.DiscountPriority = comboItem2.DiscountPriority;
                                                        comboItem3.SpecialDiscountType = 1;
                                                        lstDiscountAppliedItem2.Add(comboItem3);

                                                        if (comboItem3.ComboSaleQty > 0)
                                                        {
                                                            var maxComboCount2 = (int)(obj.Quantity / comboItem3.ComboSaleQty);
                                                            if (maxComboCount > maxComboCount2)
                                                                maxComboCount = maxComboCount2;
                                                        }
                                                    }
                                                });

                                                if (found)
                                                {
                                                    comboCount = maxComboCount;

                                                    var discountApplied = comboItem2;
                                                    discountApplied.DiscountId = item.DiscountId;

                                                    if (!ProductHelper.ContainsDiscount(lstDiscountItemByProduct, item.DiscountId))
                                                    {
                                                        discountApplied.SpecialDiscountType = 1;
                                                        lstDiscountItemByProduct.Add(discountApplied);
                                                    }

                                                    if (lstDiscountAppliedItem2.Count > 0)
                                                        lstDiscountItemByProductCombo.AddRange(lstDiscountAppliedItem2);
                                                }
                                            }
                                        }
                                    }
                                    else if (item.DiscountType == (int)DiscountType.Coupon)
                                    {
                                        if (coupon != null && coupon.DiscountID == item.DiscountId)
                                            if (!ProductHelper.ContainsDiscount(lstCouponItemByProduct, item.DiscountId))
                                                lstCouponItemByProduct.Add(item);
                                    }
                                    else if (item.DiscountType == (int)DiscountType.PaymentMethod)
                                    {
                                        if (paymentMethodId > 0 && (";" + item.AppliedForPaymentIDs + ";").Contains(";" + paymentMethodId.ToString() + ";"))
                                        {
                                            if (item.ItemId == -1)
                                                if (!ProductHelper.ContainsDiscount(lstPaymentItems, item.DiscountId))
                                                    lstPaymentItems.Add(item);

                                            //if (!ProductHelper.ContainsDiscount(lstPaymentItemByProduct, item.DiscountId))
                                            //    lstPaymentItemByProduct.Add(item);
                                        }
                                    }
                                }
                            }

                            if (lstDiscountItemByProduct.Count > 0)
                            {
                                var maxPriority = lstDiscountItemByProduct.OrderByDescending(s => s.DiscountPriority).Select(s => s.DiscountPriority).FirstOrDefault();
                                lstDiscountItemByProduct = lstDiscountItemByProduct.Where(s => s.DiscountPriority == maxPriority || (s.DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0).ToList();
                                minPrice = GetMinPriceByDiscountItem(lstDiscountItemByProduct, price, ref itemDiscount, ref itemGift, ref productGift);

                                var objItem = lstDiscountItemByProduct.Where(s => s.SpecialDiscountType > 0).FirstOrDefault();
                                var combosaleDiscountId = -1;
                                if (objItem != null)
                                {
                                    isSpecialDiscount = true;
                                    itemDiscount = itemDiscount * comboCount * objItem.ComboSaleQty;
                                    combosaleDiscountId = objItem.DiscountId;
                                    sci.ComboDiscountPriority = objItem.DiscountPriority;
                                }

                                objItem = lstDiscountItemByProduct.Where(s => s.DiscountType == (int)DiscountType.Deal && s.ComboSaleQty > 0 && s.ComboSaleQty < sci.Quantity).FirstOrDefault();
                                if (objItem != null)
                                {
                                    isSpecialDiscount = true;
                                    itemDiscount = itemDiscount * objItem.ComboSaleQty;
                                    sci.DiscountQuantity = objItem.ComboSaleQty;
                                }

                                objItem = lstDiscountItemByProduct.Where(s => s.DiscountType == (int)DiscountType.Deal && s.DealQty > 0 && (s.SoldQty + sci.Quantity) > s.DealQty).FirstOrDefault();
                                if (objItem != null)
                                {
                                    isSpecialDiscount = true;
                                    sci.DiscountQuantity = objItem.SoldQty + sci.Quantity - objItem.DealQty;
                                    itemDiscount = itemDiscount * sci.DiscountQuantity.Value;
                                }

                                objItem = lstDiscountItemByProduct.Where(s => s.DiscountType == (int)DiscountType.Deal).FirstOrDefault();
                                if (objItem != null)
                                    sci.DiscountAppliedGuid = objItem.Guid;

                                // Update combo products
                                if (lstDiscountItemByProductCombo.Count > 0 && lstDiscountItemByProduct.Where(s => s.SpecialDiscountType == 1).Count() > 0)
                                {
                                    lstDiscountItemByProductCombo.ForEach(s =>
                                    {
                                        foreach (var sci2 in cartList)
                                        {
                                            if (s.ItemId == sci2.ProductId && s.DiscountId == combosaleDiscountId && sci2.ProductId != sci.ProductId)
                                            {
                                                var product2 = ProductHelper.GetProductFromList(lstProductsInCart, sci2.ProductId);
                                                if (product2 != null)
                                                {
                                                    discountTotal -= sci2.ItemDiscount;
                                                    sci2.ItemDiscount = decimal.Zero;
                                                    sci2.GiftMessage = string.Empty;
                                                    sci2.ProductGifts = string.Empty;

                                                    var itemDiscount2 = decimal.Zero;
                                                    var itemGift2 = string.Empty;
                                                    var productGift2 = string.Empty;
                                                    GetMinPriceByDiscountItem(new List<DiscountAppliedToItem>() { s }, ProductHelper.GetOriginalPrice(product2), ref itemDiscount2, ref itemGift2, ref productGift2);
                                                    sci2.ItemDiscount = itemDiscount2 * comboCount * s.ComboSaleQty;
                                                    sci2.GiftMessage = itemGift2;
                                                    sci2.ProductGifts = productGift2;

                                                    sci2.ComboDiscountPriority = sci.ComboDiscountPriority;

                                                    discountTotal += sci2.ItemDiscount;
                                                }
                                            }
                                        }
                                    });
                                }

                                discountItemApplied = true;
                                var lstIds = lstDiscountItemByProduct.Select(s => s.DiscountId).Distinct().ToList();
                                if (lstIds.Count > 0)
                                    lstDiscountId.AddRange(lstIds);
                            }

                            // Coupon
                            if (lstCouponItemByProduct.Count > 0
                                && coupon.Discount != null && (coupon.Discount.MinPurchase <= 0 || subTotalOriginal > coupon.Discount.MinPurchase))
                            {
                                var itemDiscountCoupon = decimal.Zero;
                                //var yes = false;
                                if ((coupon.Discount.ShareType & (int)DiscountShareType.AlwaysUse) > 0)
                                    minPrice = GetMinPriceByDiscountItem(lstCouponItemByProduct, minPrice, ref itemDiscountCoupon, ref itemGift, ref productGift);
                                else
                                {
                                    itemDiscount = 0;
                                    itemGift = string.Empty;
                                    productGift = string.Empty;
                                    minPrice = GetMinPriceByDiscountItem(lstCouponItemByProduct, price, ref itemDiscountCoupon, ref itemGift, ref productGift);

                                    //yes = true;
                                }

                                itemDiscountCoupon = itemDiscountCoupon * sci.Quantity;

                                coupon.DiscountAmount += itemDiscountCoupon;

                                sci.ItemDiscountCoupon = itemDiscountCoupon;
                                if (coupon.DiscountAmount > 0 && coupon.Discount.MaximumDiscount > 0 && coupon.DiscountAmount > coupon.Discount.MaximumDiscount)
                                {
                                    //if (yes)
                                    hasItemCouponLimit = true;
                                    coupon.DiscountAmount = coupon.Discount.MaximumDiscount;
                                }

                                if (itemDiscountCoupon > 0 || !string.IsNullOrEmpty(itemGift))
                                {
                                    subTotalCoupon += price * sci.Quantity;

                                    coupon.Discount.DiscountAmount = 0;
                                    appliedCoupon = true;
                                    discountItemApplied = true;

                                    lstDiscountId.Add(coupon.DiscountID);
                                }
                            }

                            //// Payment
                            //if (lstPaymentItemByProduct.Count > 0)
                            //{
                            //    var lst = new List<DiscountAppliedToItem> { lstPaymentItemByProduct[0] };
                            //    var itemDiscountTmp = itemDiscount;
                            //    var itemGiftTmp = itemGift;

                            //    discountItemApplied = true;
                            //    lstDiscountId.Add(lstPaymentItemByProduct[0].DiscountId);

                            //    if (itemDiscount > 0 || !string.IsNullOrEmpty(itemGift))
                            //    {
                            //        if ((lstPaymentItemByProduct[0].DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                            //            minPrice = GetMinPriceByDiscountItem(lst, minPrice, ref itemDiscount, ref itemGift, ref productGift);
                            //    }
                            //    else
                            //        minPrice = GetMinPriceByDiscountItem(lst, price, ref itemDiscount, ref itemGift, ref productGift);

                            //    if (itemDiscountTmp != itemDiscount || itemGiftTmp != itemGift)
                            //        lstPaymentItems = new List<DiscountAppliedToItem>();
                            //}

                            if (itemDiscount > 0 || !string.IsNullOrEmpty(itemGift))
                            {
                                if (isSpecialDiscount)
                                    sci.ItemDiscount = itemDiscount;
                                else
                                    sci.ItemDiscount = itemDiscount * sci.Quantity;

                                sci.GiftMessage = itemGift;
                                sci.ProductGifts = productGift;
                            }

                            if (oldItemDiscount <= 0 || oldItemDiscount != sci.ItemDiscount)
                                discountTotal += sci.ItemDiscount;

                            if (sci.ItemDiscountCoupon > 0)
                                sci.ItemDiscount += sci.ItemDiscountCoupon;

                            sci.ComboDiscountPriority = -1;
                        }
                    }
                }

                // coupon
                if (hasItemCouponLimit && subTotalCoupon > 0)
                {
                    foreach (var sci in cartList)
                    {
                        var product = ProductHelper.GetProductFromList(lstProductsInCart, sci.ProductId);
                        if (product != null)
                        {
                            if ((coupon.Discount.ShareType & (int)DiscountShareType.AlwaysUse) > 0)
                            {
                                //if (sci.ItemDiscountCoupon > 0)
                                //{
                                //    var price = ProductHelper.GetOriginalPrice(product);
                                //    var itemCouponDiscount = price * (coupon.DiscountAmount / subTotalCoupon);
                                //    sci.ItemDiscount = itemCouponDiscount;
                                //}
                                //else if (sci.ItemDiscount > 0)
                                //{
                                //    discountTotal -= sci.ItemDiscount;
                                //    if (discountTotal < 0)
                                //        discountTotal = 0;

                                //    sci.ItemDiscount = 0;
                                //}
                            }
                            else
                            {
                                if (sci.ItemDiscountCoupon > 0)
                                {
                                    var price = ProductHelper.GetOriginalPrice(product);
                                    var itemCouponDiscount = sci.Quantity * price * (coupon.DiscountAmount / subTotalCoupon);
                                    //discountTotal -= sci.ItemDiscount;
                                    sci.ItemDiscount -= sci.ItemDiscountCoupon;
                                    sci.ItemDiscountCoupon = itemCouponDiscount;
                                    sci.ItemDiscount += sci.ItemDiscountCoupon;
                                    //discountTotal += sci.ItemDiscount;
                                }
                                //else if (sci.ItemDiscount > 0)
                                //{
                                //    discountTotal -= sci.ItemDiscount;
                                //    if (discountTotal < 0)
                                //        discountTotal = 0;

                                //    sci.ItemDiscount = 0;
                                //}
                            }
                        }
                    }
                }

                // discount by order range
                var subTotal = subTotalOriginal;
                //if (discountTotal > 0)
                //    subTotal -= discountTotal;

                var discountTotalAndCoupon = discountTotal;
                if (coupon != null && coupon.DiscountAmount > 0)
                    discountTotalAndCoupon += coupon.DiscountAmount;
                if (discountTotalAndCoupon > 0)
                    subTotal -= discountTotalAndCoupon;

                var orderRange = (DiscountRange)null;
                if (discountItemApplied)
                    orderRange = DiscountRange.GetActive(siteId, 0, subTotal, (int)DiscountShareType.AlwaysUse);
                else
                    orderRange = DiscountRange.GetActive(siteId, 0, subTotal, -1);

                var discountAmount = decimal.Zero;
                if (orderRange != null)
                {
                    discountAmount = GetDiscountAmount(subTotal, orderRange.DiscountAmount, orderRange.MaximumDiscount, orderRange.DiscountType == 1);
                    discountTotal += discountAmount;

                    //if (!lstDiscountId.Contains(orderRange.DiscountID))
                    lstDiscountId.Add(orderRange.DiscountID);

                    if (!string.IsNullOrEmpty(orderRange.GiftHtml))
                        giftDescription = orderRange.GiftHtml;
                    if (!string.IsNullOrEmpty(orderRange.ProductGifts))
                        productGifts = orderRange.ProductGifts;
                }
                // end order range

                // Coupon
                if (coupon != null && coupon.Discount != null && coupon.Discount.DiscountAmount > 0)
                {
                    var canApplied = true;
                    if (!string.IsNullOrEmpty(coupon.Discount.ExcludedZoneIDs) && coupon.Discount.MinPurchase > 0)
                    {
                        var sub = decimal.Zero;
                        foreach (var sci in cartList)
                        {
                            var product = ProductHelper.GetProductFromList(lstProductsInCart, sci.ProductId);
                            if (product != null && (";" + coupon.Discount.ExcludedZoneIDs).Contains(";" + product.ZoneId.ToString() + ";"))
                            {
                                var price = ProductHelper.GetOriginalPrice(product);
                                sub += sci.Quantity * price;
                            }
                        }

                        if (sub > 0 && (subTotalOriginal - sub) < coupon.Discount.MinPurchase)
                        {
                            subTotalOriginal -= sub;
                            subTotal -= sub;
                            canApplied = false;
                        }
                    }

                    if (canApplied)
                    {
                        if (discountItemApplied || orderRange != null)
                        {
                            if ((coupon.Discount.ShareType & (int)DiscountShareType.AlwaysUse) > 0)
                            {
                                if (coupon.Discount.MinPurchase <= 0 || (subTotal - discountAmount) > coupon.Discount.MinPurchase)
                                {
                                    var discountAmount2 = GetDiscountAmount(subTotal - discountAmount, coupon.Discount.DiscountAmount, coupon.Discount.MaximumDiscount, coupon.Discount.UsePercentage);
                                    //discountTotal += discountAmount2;
                                    coupon.DiscountAmount = discountAmount2;
                                    appliedCoupon = true;
                                }
                            }
                            else
                            {
                                if (coupon.Discount.MinPurchase <= 0 || subTotalOriginal > coupon.Discount.MinPurchase)
                                {
                                    discountAmount = decimal.Zero;
                                    discountTotal = decimal.Zero;
                                    giftDescription = string.Empty;
                                    productGifts = string.Empty;
                                    foreach (var sci in cartList)
                                    {
                                        sci.ItemDiscount = decimal.Zero;
                                        sci.GiftMessage = string.Empty;
                                        sci.ProductGifts = string.Empty;
                                    }

                                    var discountAmount2 = GetDiscountAmount(subTotalOriginal, coupon.Discount.DiscountAmount, coupon.Discount.MaximumDiscount, coupon.Discount.UsePercentage);
                                    coupon.DiscountAmount = discountAmount2;
                                    appliedCoupon = true;

                                    lstDiscountId.Clear();
                                }
                            }
                        }
                        else
                        {
                            if (coupon.Discount.MinPurchase <= 0 || subTotal > coupon.Discount.MinPurchase)
                            {
                                var discountAmount2 = GetDiscountAmount(subTotal, coupon.Discount.DiscountAmount, coupon.Discount.MaximumDiscount, coupon.Discount.UsePercentage);
                                //discountTotal += discountAmount2;
                                coupon.DiscountAmount = discountAmount2;
                                appliedCoupon = true;
                            }
                        }

                        if (appliedCoupon)
                            lstDiscountId.Add(coupon.DiscountID);
                    }
                }

                if (!appliedCoupon)
                {
                    if (CouponHelper.CouponCodeInput != null)
                        CouponHelper.CouponCodeInput = null;

                    coupon = null;
                }

                //Payment
                if (lstPaymentItems.Count > 0)
                {
                    if (discountTotal > 0 || !string.IsNullOrEmpty(giftDescription))
                    {
                        if ((lstPaymentItems[0].DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                        {
                            paymentDiscount = GetDiscountAmount(subTotal - discountAmount, lstPaymentItems[0].DiscountAmountParent, lstPaymentItems[0].MaximumDiscountParent, lstPaymentItems[0].UsePercentageParent);
                            //discountTotal += paymentDiscount;
                        }
                    }
                    else
                    {
                        paymentDiscount = GetDiscountAmount(subTotal, lstPaymentItems[0].DiscountAmountParent, lstPaymentItems[0].MaximumDiscountParent, lstPaymentItems[0].UsePercentageParent);
                        //discountTotal += paymentDiscount;
                    }

                    //if (!lstDiscountId.Contains(lstPaymentItems[0].DiscountId))
                    lstDiscountId.Add(lstPaymentItems[0].DiscountId);
                }
                foreach (var sci in cartList)
                {
                    if (!string.IsNullOrEmpty(sci.ProductGifts))
                    {
                        if (!string.IsNullOrEmpty(productGifts))
                            productGifts += "+";

                        productGifts += sci.ProductGifts;
                    }
                }

                lstDiscountId = lstDiscountId.Distinct().ToList();

                //  discountTotal += RewardPointHelper.CaculateSpendingPoints();
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return discountTotal;
        }

        public static bool IsDiscountAppliedItemValid(DiscountAppliedToItem item, Product product)
        {
            return ((item.ItemId == -1 && (string.IsNullOrEmpty(item.ExcludedZoneIDs) || !(";" + item.ExcludedZoneIDs).Contains(";" + product.ZoneId.ToString() + ";")))
                                        || (item.AppliedType == (int)DiscountAppliedType.ToCategories && product.ZoneId == item.ItemId)
                                        || (item.AppliedType == (int)DiscountAppliedType.ToProducts && product.ProductId == item.ItemId));
        }

        public static decimal GetDiscountAmount(decimal subTotal, decimal discountAmount, decimal maximumDiscount, bool usePercentage)
        {
            var results = decimal.Zero;
            if (subTotal <= 0) return results;

            if (usePercentage)
                results = subTotal * discountAmount * (decimal)0.01;
            else
                results = discountAmount;

            if (results > maximumDiscount && maximumDiscount > 0)
                results = maximumDiscount;

            if (results < 0)
                results = 0;

            return results;
        }

        private static decimal GetMinPriceByDiscountItem(List<DiscountAppliedToItem> lstDiscountItemByProduct, decimal minPrice, ref decimal itemDiscount, ref string itemGift, ref string productGifts)
        {
            foreach (var item in lstDiscountItemByProduct)
            {
                var newPrice = 0M;
                var discountAmount = decimal.Zero;
                var usePercentageTmp = item.UsePercentage;
                var discountAmountTmp = item.DiscountAmount;
                //if (item.DiscountAmount <= 0 && item.DiscountAmountParent > 0)
                //{
                //    usePercentageTmp = item.UsePercentageParent;
                //    discountAmountTmp = item.DiscountAmountParent;
                //}
                if (usePercentageTmp)
                {
                    discountAmount = minPrice * discountAmountTmp * (decimal)0.01;
                    if (discountAmount > item.MaximumDiscount && item.MaximumDiscount > 0)
                        discountAmount = item.MaximumDiscount;
                }
                else
                {
                    discountAmount = discountAmountTmp;

                    if (discountAmount > item.MaximumDiscount && item.MaximumDiscount > 0)
                        discountAmount = item.MaximumDiscount;
                }

                if (discountAmount < 0)
                    discountAmount = 0;

                newPrice = (minPrice - discountAmount);

                itemDiscount += discountAmount;
                itemGift += item.GiftHtml;

                if (!string.IsNullOrEmpty(item.ProductGifts))
                {
                    if (!string.IsNullOrEmpty(productGifts))
                        productGifts += "+";
                    productGifts += item.ProductGifts;
                }

                if (newPrice > 0 && newPrice < minPrice)
                    minPrice = newPrice;

                if (minPrice < 0)
                    minPrice = 0;
            }

            return minPrice;
        }

        #region Add to cart

        /// <summary>
        /// Add a manufacturer to shopping cart
        /// </summary>
        /// <param name="manufacturer">Product</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="automaticallyAddRequiredProductsIfEnabled">Automatically add required products if enabled</param>
        /// <returns>Warnings</returns>
        public static IList<string> AddToCart(Product product,
            ShoppingCartTypeEnum shoppingCartType, out Guid shoppingCartItemGuid, string attributesXml = null,
            int quantity = 1, bool automaticallyAddRequiredProductsIfEnabled = true)
        {
            var cartSessionGuid = GetCartSessionGuid(product.SiteId, true);
            shoppingCartItemGuid = Guid.Empty;

            if (cartSessionGuid == Guid.Empty)
                throw new ArgumentNullException("cartSessionGuid");

            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();
            if (shoppingCartType == ShoppingCartTypeEnum.ShoppingCart && !ProductConfiguration.EnableShoppingCart)
            {
                warnings.Add("Shopping cart is disabled");
                return warnings;
            }
            if (shoppingCartType == ShoppingCartTypeEnum.Wishlist && !ProductConfiguration.EnableWishlist)
            {
                warnings.Add("Wishlist is disabled");
                return warnings;
            }
            if (quantity <= 0)
            {
                warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartQuantityShouldPositive"));
                return warnings;
            }

            var cart = ShoppingCartItem.GetByUserGuid(product.SiteId, shoppingCartType, cartSessionGuid);

            var shoppingCartItem = FindShoppingCartItemInTheCart(cart,
                shoppingCartType, product, attributesXml);

            if (shoppingCartItem != null)
            {
                //update existing shopping cart order
                int newQuantity = shoppingCartItem.Quantity + quantity;
                warnings.AddRange(GetShoppingCartItemWarnings(cartSessionGuid, shoppingCartType, product,
                    attributesXml,
                    newQuantity, automaticallyAddRequiredProductsIfEnabled));

                if (warnings.Count == 0)
                {
                    shoppingCartItem.AttributesXml = attributesXml;
                    shoppingCartItem.Quantity = newQuantity;
                    shoppingCartItem.LastModUtc = DateTime.UtcNow;
                    shoppingCartItem.Save();
                }

                shoppingCartItemGuid = shoppingCartItem.Guid;
            }
            else
            {
                //new shopping cart order
                warnings.AddRange(GetShoppingCartItemWarnings(cartSessionGuid, shoppingCartType, product,
                    attributesXml,
                    quantity, automaticallyAddRequiredProductsIfEnabled));
                if (warnings.Count == 0)
                {
                    //maximum items validation
                    switch (shoppingCartType)
                    {
                        case ShoppingCartTypeEnum.ShoppingCart:
                            {
                                if (cart.Count >= ProductConfiguration.MaximumShoppingCartItems)
                                {
                                    warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartMaximumShoppingCartItems"), ProductConfiguration.MaximumShoppingCartItems));
                                    return warnings;
                                }
                            }
                            break;

                        case ShoppingCartTypeEnum.Wishlist:
                            {
                                if (cart.Count >= ProductConfiguration.MaximumWishlistItems)
                                {
                                    warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartMaximumWishlistItems"), ProductConfiguration.MaximumWishlistItems));
                                    return warnings;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    DateTime now = DateTime.UtcNow;
                    shoppingCartItem = new ShoppingCartItem
                    {
                        ShoppingCartType = (int)shoppingCartType,
                        SiteId = product.SiteId,
                        ProductId = product.ProductId,
                        AttributesXml = attributesXml,
                        Quantity = quantity,
                        CreatedUtc = now,
                        LastModUtc = now,
                        CreatedFromIP = SiteUtils.GetIP4Address(),
                        UserGuid = cartSessionGuid
                    };

                    shoppingCartItem.Save();
                    shoppingCartItemGuid = shoppingCartItem.Guid;
                }
            }

            return warnings;
        }


        public static IList<string> AddToCart(Product product,
            ShoppingCartTypeEnum shoppingCartType, out Guid shoppingCartItemGuid, string attributesXml = null,
            int quantity = 1, bool automaticallyAddRequiredProductsIfEnabled = true, int affiliateUserID = -1)
        {
            var cartSessionGuid = GetCartSessionGuid(product.SiteId, true);
            shoppingCartItemGuid = Guid.Empty;

            if (cartSessionGuid == Guid.Empty)
                throw new ArgumentNullException("cartSessionGuid");

            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();
            if (shoppingCartType == ShoppingCartTypeEnum.ShoppingCart && !ProductConfiguration.EnableShoppingCart)
            {
                warnings.Add("Shopping cart is disabled");
                return warnings;
            }
            if (shoppingCartType == ShoppingCartTypeEnum.Wishlist && !ProductConfiguration.EnableWishlist)
            {
                warnings.Add("Wishlist is disabled");
                return warnings;
            }
            if (quantity <= 0)
            {
                warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartQuantityShouldPositive"));
                return warnings;
            }

            var cart = ShoppingCartItem.GetByUserGuid(product.SiteId, shoppingCartType, cartSessionGuid);

            var shoppingCartItem = FindShoppingCartItemInTheCart(cart,
                shoppingCartType, product, attributesXml);

            if (shoppingCartItem != null)
            {
                //update existing shopping cart order
                int newQuantity = shoppingCartItem.Quantity + quantity;
                warnings.AddRange(GetShoppingCartItemWarnings(cartSessionGuid, shoppingCartType, product,
                    attributesXml,
                    newQuantity, automaticallyAddRequiredProductsIfEnabled));

                if (warnings.Count == 0)
                {
                    shoppingCartItem.AttributesXml = attributesXml;
                    shoppingCartItem.Quantity = newQuantity;
                    shoppingCartItem.LastModUtc = DateTime.UtcNow;
                    shoppingCartItem.AffiliateUserID = affiliateUserID;
                    shoppingCartItem.Save();
                }

                shoppingCartItemGuid = shoppingCartItem.Guid;
            }
            else
            {
                //new shopping cart order
                warnings.AddRange(GetShoppingCartItemWarnings(cartSessionGuid, shoppingCartType, product,
                    attributesXml,
                    quantity, automaticallyAddRequiredProductsIfEnabled));
                if (warnings.Count == 0)
                {
                    //maximum items validation
                    switch (shoppingCartType)
                    {
                        case ShoppingCartTypeEnum.ShoppingCart:
                            {
                                if (cart.Count >= ProductConfiguration.MaximumShoppingCartItems)
                                {
                                    warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartMaximumShoppingCartItems"), ProductConfiguration.MaximumShoppingCartItems));
                                    return warnings;
                                }
                            }
                            break;

                        case ShoppingCartTypeEnum.Wishlist:
                            {
                                if (cart.Count >= ProductConfiguration.MaximumWishlistItems)
                                {
                                    warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartMaximumWishlistItems"), ProductConfiguration.MaximumWishlistItems));
                                    return warnings;
                                }
                            }
                            break;

                        default:
                            break;
                    }

                    DateTime now = DateTime.UtcNow;
                    shoppingCartItem = new ShoppingCartItem
                    {
                        ShoppingCartType = (int)shoppingCartType,
                        SiteId = product.SiteId,
                        ProductId = product.ProductId,
                        AttributesXml = attributesXml,
                        Quantity = quantity,
                        CreatedUtc = now,
                        LastModUtc = now,
                        CreatedFromIP = SiteUtils.GetIP4Address(),
                        UserGuid = cartSessionGuid,
                        AffiliateUserID = affiliateUserID
                    };

                    shoppingCartItem.Save();
                    shoppingCartItemGuid = shoppingCartItem.Guid;
                }
            }

            return warnings;
        }
        /// <summary>
        /// Finds a shopping cart order in the cart
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="manufacturer">Product</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Found shopping cart order</returns>
        private static ShoppingCartItem FindShoppingCartItemInTheCart(
            IList<ShoppingCartItem> shoppingCart,
            ShoppingCartTypeEnum shoppingCartType,
            Product product,
            string attributesXml = "")
        {
            if (shoppingCart == null)
                throw new ArgumentNullException("shoppingCart");

            if (product == null)
                throw new ArgumentNullException("product");

            // Query customfields if attribute applied
            List<CustomField> customFields = new List<CustomField>();
            if (ProductConfiguration.EnableShoppingCartAttributes)
            {
                if (!string.IsNullOrEmpty(attributesXml))
                    customFields = CustomField.GetActiveForCart(product.SiteId, Product.FeatureGuid);
                else
                    foreach (var sci in shoppingCart.Where(a => a.ShoppingCartType == (int)shoppingCartType))
                    {
                        if (!string.IsNullOrEmpty(sci.AttributesXml))
                        {
                            customFields = CustomField.GetActiveForCart(product.SiteId, Product.FeatureGuid);
                            break;
                        }
                    }
            }

            foreach (var sci in shoppingCart.Where(a => a.ShoppingCartType == (int)shoppingCartType))
            {
                if (sci.ProductId == product.ProductId)
                {
                    //attributes
                    bool attributesEqual = AreProductAttributesEqual(customFields, sci.AttributesXml, attributesXml, false);

                    //gift cards
                    bool giftCardInfoSame = true;

                    //rental products
                    bool rentalInfoEqual = true;

                    //found?
                    if (attributesEqual && giftCardInfoSame && rentalInfoEqual)
                        return sci;
                }
            }

            return null;
        }

        /// <summary>
        /// Are attributes equal
        /// </summary>
        /// <param name="attributesXml1">The attributes of the first manufacturer</param>
        /// <param name="attributesXml2">The attributes of the second manufacturer</param>
        /// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        /// <returns>Result</returns>
        private static bool AreProductAttributesEqual(List<CustomField> customFields, string attributesXml1, string attributesXml2, bool ignoreNonCombinableAttributes)
        {
            var attributes1 = ProductAttributeParser.ParseProductAttributeMappings(customFields, attributesXml1);
            if (ignoreNonCombinableAttributes)
            {
                attributes1 = attributes1.Where(x => !x.IsNonCombinable).ToList();
            }
            var attributes2 = ProductAttributeParser.ParseProductAttributeMappings(customFields, attributesXml2);
            if (ignoreNonCombinableAttributes)
            {
                attributes2 = attributes2.Where(x => !x.IsNonCombinable).ToList();
            }
            if (attributes1.Count != attributes2.Count)
                return false;

            bool attributesEqual = true;
            foreach (var a1 in attributes1)
            {
                bool hasAttribute = false;
                foreach (var a2 in attributes2)
                {
                    if (a1.CustomFieldId == a2.CustomFieldId)
                    {
                        hasAttribute = true;
                        var values1Str = ProductAttributeParser.ParseValues(attributesXml1, a1.CustomFieldId);
                        var values2Str = ProductAttributeParser.ParseValues(attributesXml2, a2.CustomFieldId);
                        if (values1Str.Count == values2Str.Count)
                        {
                            foreach (int str1 in values1Str)
                            {
                                bool hasValue = false;
                                foreach (int str2 in values2Str)
                                {
                                    //case insensitive?
                                    //if (str1.Trim().ToLower() == str2.Trim().ToLower())
                                    if (str1 == str2)
                                    {
                                        hasValue = true;
                                        break;
                                    }
                                }

                                if (!hasValue)
                                {
                                    attributesEqual = false;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            attributesEqual = false;
                            break;
                        }
                    }
                }

                if (hasAttribute == false)
                {
                    attributesEqual = false;
                    break;
                }
            }

            return attributesEqual;
        }

        #endregion Add to cart

        #region Shopping cart warning

        public static IList<string> GetShoppingCartItemWarnings(Guid cartSessionGuid, ShoppingCartTypeEnum shoppingCartType,
            Product product,
            string attributesXml,
            int quantity = 1, bool automaticallyAddRequiredProductsIfEnabled = true,
            bool getStandardWarnings = true, bool getAttributesWarnings = true)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();

            //standard properties
            if (getStandardWarnings)
                warnings.AddRange(GetStandardWarnings(cartSessionGuid, shoppingCartType, product, attributesXml, quantity));

            //selected attributes
            if (getAttributesWarnings)
                warnings.AddRange(GetShoppingCartItemAttributeWarnings(cartSessionGuid, shoppingCartType, product, quantity, attributesXml));

            //gift cards

            //required products

            //rental products

            return warnings;
        }

        /// <summary>
        /// Validates a manufacturer for standard properties
        /// </summary>
        /// <param name="cartSessionGuid">cartSessionGuid</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="manufacturer">Product</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="quantity">Quantity</param>
        /// <returns>Warnings</returns>
        public static IList<string> GetStandardWarnings(Guid cartSessionGuid, ShoppingCartTypeEnum shoppingCartType,
            Product product, string attributesXml,
            int quantity)
        {
            if (cartSessionGuid == Guid.Empty)
                throw new ArgumentNullException("cartSessionGuid");

            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();

            //deleted
            if (product.IsDeleted)
            {
                warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartProductDeleted"));
                return warnings;
            }

            //published
            if (!product.IsPublished)
            {
                warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartProductUnpublished"));
            }

            ////we can add only simple products
            //if (manufacturer.ProductType != ProductType.SimpleProduct)
            //{
            //    warnings.Add("This is not simple manufacturer");
            //}

            //disabled "add to cart" button
            if (shoppingCartType == ShoppingCartTypeEnum.ShoppingCart && product.DisableBuyButton)
            {
                warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartBuyingDisabled"));
            }

            ////disabled "add to wishlist" button
            //if (shoppingCartType == ShoppingCartTypeEnum.Wishlist && manufacturer.DisableWishlistButton)
            //{
            //    warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartWishlistDisabled"));
            //}

            //call for price

            //customer entered price

            //quantity validation
            var hasQtyWarnings = false;
            //if (quantity < manufacturer.OrderMinimumQuantity)
            //{
            //    warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartMinimumQuantity"), manufacturer.OrderMinimumQuantity));
            //    hasQtyWarnings = true;
            //}
            if (quantity > ProductConfiguration.OrderMaximumQuantity)
            {
                warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartMaximumQuantity"), ProductConfiguration.OrderMaximumQuantity));
                hasQtyWarnings = true;
            }
            //var allowedQuantities = manufacturer.ParseAllowedQuantities();
            //if (allowedQuantities.Length > 0 && !allowedQuantities.Contains(quantity))
            //{
            //    warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartAllowedQuantities"), string.Join(", ", allowedQuantities)));
            //}

            //var validateOutOfStock = shoppingCartType == ShoppingCartType.ShoppingCart || !_shoppingCartSettings.AllowOutOfStockItemsToBeAddedToWishlist;
            //if (validateOutOfStock && !hasQtyWarnings)
            //{
            //    switch (manufacturer.ManageInventoryMethod)
            //    {
            //        case ManageInventoryMethod.DontManageStock:
            //            {
            //                //do nothing
            //            }
            //            break;
            //        case ManageInventoryMethod.ManageStock:
            //            {
            //                if (manufacturer.BackorderMode == BackorderMode.NoBackorders)
            //                {
            //                    int maximumQuantityCanBeAdded = manufacturer.GetTotalStockQuantity();
            //                    if (maximumQuantityCanBeAdded < quantity)
            //                    {
            //                        if (maximumQuantityCanBeAdded <= 0)
            //                            warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartOutOfStock"));
            //                        else
            //                            warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartQuantityExceedsStock"), maximumQuantityCanBeAdded));
            //                    }
            //                }
            //            }
            //            break;
            //        case ManageInventoryMethod.ManageStockByAttributes:
            //            {
            //                var combination = _productAttributeParser.FindProductAttributeCombination(manufacturer, attributesXml);
            //                if (combination != null)
            //                {
            //                    //combination exists
            //                    //let's check stock level
            //                    if (!combination.AllowOutOfStockOrders && combination.StockQuantity < quantity)
            //                    {
            //                        int maximumQuantityCanBeAdded = combination.StockQuantity;
            //                        if (maximumQuantityCanBeAdded <= 0)
            //                        {
            //                            warnings.Add(ResourceHelper.GetResourceString("ProductResources", "CartOutOfStock"));
            //                        }
            //                        else
            //                        {
            //                            warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartQuantityExceedsStock"), maximumQuantityCanBeAdded));
            //                        }
            //                    }
            //                }
            //                else
            //                {
            //                    //combination doesn't exist
            //                    if (manufacturer.AllowAddingOnlyExistingAttributeCombinations)
            //                    {
            //                        //maybe, is it better  to display something like "No such manufacturer/combination" message?
            //                        warnings.Add(ResourceHelper.GetResourceString("ProductResources", "ShoppingCart.OutOfStock"));
            //                    }
            //                }
            //            }
            //            break;
            //        default:
            //            break;
            //    }
            //}

            //availability Out Of Stock

            //availability dates

            return warnings;
        }

        /// <summary>
        /// Validates shopping cart order attributes
        /// </summary>
        /// <param name="cartSessionGuid">Cart Session Guid</param>
        /// <param name="shoppingCartType">Shopping cart type</param>
        /// <param name="manufacturer">Product</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        /// <returns>Warnings</returns>
        public static IList<string> GetShoppingCartItemAttributeWarnings(Guid cartSessionGuid,
            ShoppingCartTypeEnum shoppingCartType,
            Product product,
            int quantity = 1,
            string attributesXml = "",
            bool ignoreNonCombinableAttributes = false)
        {
            if (product == null)
                throw new ArgumentNullException("product");

            var warnings = new List<string>();

            ////ensure it's our attributes
            //var attributes1 = ParseProductAttributeMappings(attributesXml);
            //if (ignoreNonCombinableAttributes)
            //{
            //    attributes1 = attributes1.Where(x => !x.IsNonCombinable).ToList();
            //}
            //foreach (var attribute in attributes1)
            //{
            //    ProductProperty.GetPropertiesByField(attribute.CustomFieldId);
            //    if (attribute.Product != null)
            //    {
            //        if (attribute.Product.Id != manufacturer.ProductId)
            //        {
            //            warnings.Add("Attribute error");
            //        }
            //    }
            //    else
            //    {
            //        warnings.Add("Attribute error");
            //        return warnings;
            //    }
            //}

            ////validate required manufacturer attributes (whether they're chosen/selected/entered)
            //var attributes2 = _productAttributeService.GetProductAttributeMappingsByProductId(manufacturer.ProductId);
            //if (ignoreNonCombinableAttributes)
            //{
            //    attributes2 = attributes2.Where(x => !x.IsNonCombinable).ToList();
            //}
            //foreach (var a2 in attributes2)
            //{
            //    if (a2.IsRequired)
            //    {
            //        bool found = false;
            //        //selected manufacturer attributes
            //        foreach (var a1 in attributes1)
            //        {
            //            if (a1.Id == a2.Id)
            //            {
            //                var attributeValuesStr = ParseValues(attributesXml, a1.Id);
            //                foreach (string str1 in attributeValuesStr)
            //                {
            //                    if (!String.IsNullOrEmpty(str1.Trim()))
            //                    {
            //                        found = true;
            //                        break;
            //                    }
            //                }
            //            }
            //        }

            //        //if not found
            //        if (!found)
            //        {
            //            var notFoundWarning = !string.IsNullOrEmpty(a2.TextPrompt) ?
            //                a2.TextPrompt :
            //                string.Format(ResourceHelper.GetResourceString("ProductResources", "CartSelectAttribute"), a2.ProductAttribute.GetLocalized(a => a.Name));

            //            warnings.Add(notFoundWarning);
            //        }
            //    }

            //    if (a2.AttributeControlType == AttributeControlType.ReadonlyCheckboxes)
            //    {
            //        //customers cannot edit read-only attributes
            //        var allowedReadOnlyValueIds = _productAttributeService.GetProductAttributeValues(a2.Id)
            //            .Where(x => x.IsPreSelected)
            //            .Select(x => x.Id)
            //            .ToArray();

            //        var selectedReadOnlyValueIds = _productAttributeParser.ParseProductAttributeValues(attributesXml)
            //            .Where(x => x.ProductAttributeMappingId == a2.Id)
            //            .Select(x => x.Id)
            //            .ToArray();

            //        if (!CommonHelper.ArraysEqual(allowedReadOnlyValueIds, selectedReadOnlyValueIds))
            //        {
            //            warnings.Add("You cannot change read-only values");
            //        }
            //    }
            //}

            ////validation rules
            //foreach (var pam in attributes2)
            //{
            //    if (!pam.ValidationRulesAllowed())
            //        continue;

            //    //minimum length
            //    if (pam.ValidationMinLength.HasValue)
            //    {
            //        if (pam.AttributeControlType == CustomFieldDataType.Text ||
            //            pam.AttributeControlType == AttributeControlType.MultilineTextbox)
            //        {
            //            var valuesStr = _productAttributeParser.ParseValues(attributesXml, pam.Id);
            //            var enteredText = valuesStr.FirstOrDefault();
            //            int enteredTextLength = String.IsNullOrEmpty(enteredText) ? 0 : enteredText.Length;

            //            if (pam.ValidationMinLength.Value > enteredTextLength)
            //            {
            //                warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartTextboxMinimumLength"), pam.ProductAttribute.GetLocalized(a => a.Name), pam.ValidationMinLength.Value));
            //            }
            //        }
            //    }

            //    //maximum length
            //    if (pam.ValidationMaxLength.HasValue)
            //    {
            //        if (pam.AttributeControlType == AttributeControlType.TextBox ||
            //            pam.AttributeControlType == AttributeControlType.MultilineTextbox)
            //        {
            //            var valuesStr = _productAttributeParser.ParseValues(attributesXml, pam.Id);
            //            var enteredText = valuesStr.FirstOrDefault();
            //            int enteredTextLength = String.IsNullOrEmpty(enteredText) ? 0 : enteredText.Length;

            //            if (pam.ValidationMaxLength.Value < enteredTextLength)
            //            {
            //                warnings.Add(string.Format(ResourceHelper.GetResourceString("ProductResources", "CartTextboxMaximumLength"), pam.ProductAttribute.GetLocalized(a => a.Name), pam.ValidationMaxLength.Value));
            //            }
            //        }
            //    }
            //}

            //if (warnings.Count > 0)
            //    return warnings;

            ////validate bundled products
            //var attributeValues = _productAttributeParser.ParseProductAttributeValues(attributesXml);
            //foreach (var attributeValue in attributeValues)
            //{
            //    if (attributeValue.AttributeValueType == AttributeValueType.AssociatedToProduct)
            //    {
            //        if (ignoreNonCombinableAttributes && attributeValue.ProductAttributeMapping.IsNonCombinable())
            //            continue;

            //        //associated manufacturer (bundle)
            //        var associatedProduct = _productService.GetProductById(attributeValue.AssociatedProductId);
            //        if (associatedProduct != null)
            //        {
            //            var totalQty = quantity * attributeValue.Quantity;
            //            var associatedProductWarnings = GetShoppingCartItemWarnings(customer,
            //                shoppingCartType, associatedProduct, _storeContext.CurrentStore.Id,
            //                "", decimal.Zero, null, null, totalQty, false);
            //            foreach (var associatedProductWarning in associatedProductWarnings)
            //            {
            //                var attributeName = attributeValue.ProductAttributeMapping.ProductAttribute.GetLocalized(a => a.Name);
            //                var attributeValueName = attributeValue.GetLocalized(a => a.Name);
            //                warnings.Add(string.Format(
            //                    ResourceHelper.GetResourceString("ProductResources", "CartAssociatedAttributeWarning"),
            //                    attributeName, attributeValueName, associatedProductWarning));
            //            }
            //        }
            //        else
            //        {
            //            warnings.Add(string.Format("Associated manufacturer cannot be loaded - {0}", attributeValue.AssociatedProductId));
            //        }
            //    }
            //}

            return warnings;
        }

        #endregion Shopping cart warning

        public static string RemoveFromCart(Guid itemGuid, ShoppingCartTypeEnum cartType)
        {
            if (!ProductConfiguration.EnableShoppingCart)
                return Json(new
                {
                    success = false,
                    redirect = SiteUtils.GetHomepageUrl()
                });

            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var cart = GetShoppingCart(siteSettings.SiteId, cartType);

            foreach (var sci in cart)
            {
                if (sci.Guid == itemGuid)
                {
                    ShoppingCartItem.Delete(sci.Guid);

                    break;
                }
            }
            if (cartType == ShoppingCartTypeEnum.Wishlist)
                WishlistCacheHelper.ClearCache(siteSettings.SiteId, GetCartSessionGuid(siteSettings.SiteId));
            return Json(new
            {
                success = true,
                redirect = GetCartUrl()
            });
        }

        public static string UpdateCart(NameValueCollection form)
        {
            if (!ProductConfiguration.EnableShoppingCart)
                return Json(new
                {
                    success = false,
                    redirect = SiteUtils.GetHomepageUrl()
                });

            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var cartType = ShoppingCartTypeEnum.ShoppingCart;
            var cart = GetShoppingCart(siteSettings.SiteId, cartType);

            var allGuidsToRemove = form["removefromcart"] != null ? form["removefromcart"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => Guid.Parse(x)).ToList() : new List<Guid>();

            var cartSessionGuid = CartHelper.GetCartSessionGuid(siteSettings.SiteId, true);
            var lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, cartSessionGuid, cartType);

            var customFields = new List<CustomField>();
            if (ProductConfiguration.EnableShoppingCartAttributes)
                customFields = CustomField.GetActiveForCart(siteSettings.SiteId, Product.FeatureGuid);

            foreach (var sci in cart)
            {
                bool remove = allGuidsToRemove.Contains(sci.Guid);
                if (remove)
                    ShoppingCartItem.Delete(sci.Guid);
                else
                {
                    int newQuantity = sci.Quantity;
                    string quantityKey = string.Format("itemquantity{0}", sci.Guid);
                    if (form[quantityKey] != null)
                    {
                        int.TryParse(form[quantityKey], out newQuantity);

                        if (newQuantity <= 0)
                        {
                            ShoppingCartItem.Delete(sci.Guid);
                            continue;
                        }
                    }

                    string attributesXml = string.Empty;
                    if (ProductConfiguration.EnableShoppingCartAttributes)
                    {
                        foreach (var attribute in customFields)
                        {
                            string controlId = string.Format("product_attribute_{0}_{1}", sci.Guid, attribute.CustomFieldId);
                            var ctrlAttributes = form[controlId];
                            if (!String.IsNullOrEmpty(ctrlAttributes))
                            {
                                int selectedAttributeId = int.Parse(ctrlAttributes);
                                if (selectedAttributeId > 0)
                                    attributesXml = CustomFieldHelper.AddProductAttribute(attributesXml, attribute, selectedAttributeId.ToString());
                            }
                        }
                    }

                    Product product = ProductHelper.GetProductFromList(lstProductsInCart, sci.ProductId);
                    //check warnings
                    var warnings = CartHelper.GetShoppingCartItemWarnings(cartSessionGuid, ShoppingCartTypeEnum.ShoppingCart,
                        product, attributesXml, newQuantity, false, true, false);
                    if (warnings.Count == 0)
                    {
                        sci.AttributesXml = attributesXml;
                        sci.Quantity = newQuantity;
                        sci.Save();
                    }
                }
            }

            if (ProductConfiguration.EnableShoppingCartAttributes)
            {
                for (int i = cart.Count - 1; i >= 0; i--)
                {
                    var sci = cart[i];
                    for (int j = cart.Count - 1; j >= 0; j--)
                    {
                        var sci2 = cart[j];

                        if (
                            sci.Guid != sci2.Guid
                            && sci.ProductId == sci2.ProductId
                            && AreProductAttributesEqual(customFields, sci.AttributesXml, sci2.AttributesXml, false)
                            )
                        {
                            sci.Quantity += sci2.Quantity;
                            sci.Save();

                            ShoppingCartItem.Delete(sci2.Guid);
                            cart.Remove(sci2);

                            break;
                        }
                    }
                }
            }

            ////display notification message and update appropriate blocks
            //var updatetopcartsectionhtml = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartQuantityFormat"),
            //        CartHelper.GetShoppingCart(siteSettings.SiteId, ShoppingCartTypeEnum.ShoppingCart).GetTotalProducts());

            //var updateflyoutcartsectionhtml = ProductConfiguration.MiniShoppingCartEnabled
            //    ? PrepareShoppingCart(siteSettings.SiteId) : "";

            return Json(new
            {
                success = true,
                redirect = GetCartUrl()
                //message = string.Format(ResourceHelper.GetResourceString("ProductResources", "CartProductHasBeenAddedFormat"), GetCartUrl()),
                //updatetopcartsectionhtml = updatetopcartsectionhtml,
                //updateflyoutcartsectionhtml = updateflyoutcartsectionhtml
            });
        }

        public static string GetCartUrl()
        {
            var gbNode = SiteUtils.GetSiteMapNodeByZoneId(ProductConfiguration.CartPageId);
            if (gbNode != null)
                return FormatUrl(gbNode, WorkingCulture.LanguageId);
            else
                return "/cart";
        }

        private static string FormatUrl(gbSiteMapNode mapNode, int languageId)
        {
            string url = string.Empty;
            if (WebConfigSettings.EnableHierarchicalFriendlyUrls)
            {
                url = mapNode.UrlExpand;
                if (languageId > 0 && mapNode["UrlExpand" + languageId.ToString()] != null)
                    url = mapNode["UrlExpand" + languageId.ToString()];
            }
            else
            {
                url = mapNode.Url;
                if (languageId > 0 && mapNode["Url" + languageId.ToString()] != null)
                    url = mapNode["Url" + languageId.ToString()];
            }
            return url.Replace("~/", "/");
        }

        public static void SetupRedirectToCartPage(System.Web.UI.Control control)
        {
            string cartPage = GetCartUrl();
            string rawUrl = HttpContext.Current.Request.RawUrl;
            if (!cartPage.ContainsCaseInsensitive(rawUrl))
                WebUtils.SetupRedirect(control, cartPage);
        }

        public static string GetZoneUrl(int zoneId)
        {
            try
            {
                return SiteUtils.GetZoneUrl(zoneId);
            }
            catch (Exception) { }

            return SiteUtils.GetHomepageUrl();
        }

        public static string GetWishlistUrl()
        {
            return GetWishlistUrl(SiteUtils.GetNavigationSiteRoot());
        }

        public static string GetWishlistUrl(string siteRoot)
        {
            return siteRoot + ProductConfiguration.WishlistPageUrl;
        }

        /// <summary>
        /// Gets the last page for "Continue shopping" button on shopping cart page
        /// </summary>
        public static string LastContinueShoppingPage
        {
            get
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["LastContinueShoppingPage"] != null))
                {
                    return HttpContext.Current.Session["LastContinueShoppingPage"].ToString();
                }
                return string.Empty;
            }
            set
            {
                if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
                {
                    HttpContext.Current.Session["LastContinueShoppingPage"] = value;
                }
            }
        }

        public static string ChangeCartType(SiteSettings siteSettings, Guid itemGuid, ShoppingCartTypeEnum fromCartType, ShoppingCartTypeEnum toCartType)
        {
            var cart = GetShoppingCart(siteSettings.SiteId, fromCartType);

            foreach (var sci in cart)
            {
                if (sci.Guid == itemGuid)
                {
                    sci.ShoppingCartType = (int)toCartType;
                    sci.Save();

                    if (ProductConfiguration.EnableShoppingCartAttributes)
                    {
                        var customFields = CustomField.GetActiveForCart(siteSettings.SiteId, Product.FeatureGuid);
                        var cartTo = GetShoppingCart(siteSettings.SiteId, toCartType);
                        for (int i = cartTo.Count - 1; i >= 0; i--)
                        {
                            var sci1 = cart[i];
                            for (int j = cart.Count - 1; j >= 0; j--)
                            {
                                var sci2 = cart[j];

                                if (
                                    sci1.Guid != sci2.Guid
                                    && sci1.ProductId == sci2.ProductId
                                    && AreProductAttributesEqual(customFields, sci1.AttributesXml, sci2.AttributesXml, false)
                                    )
                                {
                                    sci1.Quantity += sci2.Quantity;
                                    sci1.Save();

                                    ShoppingCartItem.Delete(sci2.Guid);
                                    cart.Remove(sci2);

                                    break;
                                }
                            }
                        }
                    }

                    return Json(new
                    {
                        success = true
                    });
                }
            }

            return Json(new
            {
                success = false
            });
        }

        public static ShoppingCartItem ExistProductInWishlist(Product product)
        {
            var cartType = ShoppingCartTypeEnum.Wishlist;
            var cartSessionGuid = CartHelper.GetCartSessionGuid(product.SiteId, true);
            var cart = ShoppingCartItem.GetByUserGuid(product.SiteId, cartType, cartSessionGuid);
            var shoppingCartItem = CartHelper.FindShoppingCartItemInTheCart(cart, cartType, product);
            return shoppingCartItem;
        }

        #region Child products

        public static string SelectProductOption(Product product, NameValueCollection postParams)
        {
            var lstChildProducts = ProductHelper.GetChildProducts(product);

            // lstChildProducts.Add(Product.GetPageBySearch(productIds: product.ProductId.ToString()).FirstOrDefault());
            if (lstChildProducts.Count == 0)
                return Json(new
                {
                    success = true
                });

            var lstChildProductIds = lstChildProducts.Select(s => s.ProductId).Distinct().ToList();
            var productProperties = ProductProperty.GetPropertiesByProducts(lstChildProductIds);

            var forceHasAttribute = false;
            var customFieldIds = productProperties.Select(x => x.CustomFieldId).Distinct().ToList();
            var customFields = CustomField.GetByOption(CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds), CustomFieldOptions.EnableShoppingCart);
            if (customFields.Count > 0)
                forceHasAttribute = true;

            if (!forceHasAttribute)
                return Json(new
                {
                    success = true
                });

            var selectedOptionId = -1;
            if (postParams["optionid"] != null)
                int.TryParse(postParams["optionid"], out selectedOptionId);
            var selectedFieldId = productProperties.Where(s => s.CustomFieldOptionId == selectedOptionId).Select(s => s.CustomFieldId).Distinct().FirstOrDefault();

            var lstAvailableProducts = GetProductsHasAttribute(lstChildProducts, productProperties, selectedFieldId, selectedOptionId);
            if (lstAvailableProducts.Count == 0)
                return Json(new
                {
                    success = false
                });

            var lstAvailableOptionsIds = new List<int>();
            lstAvailableOptionsIds.Add(selectedOptionId);
            foreach (var attribute in customFields)
            {
                if (attribute.CustomFieldId != selectedFieldId)
                {
                    var lstOptionIds = productProperties.Where(s => s.CustomFieldId == attribute.CustomFieldId).Select(s => s.CustomFieldOptionId).ToList();
                    foreach (var optionId in lstOptionIds)
                    {
                        if (!lstAvailableOptionsIds.Contains(optionId))
                        {
                            var lstAvailableByOption = GetProductsHasAttribute(lstAvailableProducts, productProperties, attribute.CustomFieldId, optionId);
                            if (lstAvailableByOption.Count > 0)
                                lstAvailableOptionsIds.Add(optionId);
                        }
                    }
                }
            }

            foreach (var attribute in customFields)
            {
                if (attribute.CustomFieldId != selectedFieldId)
                {
                    var controlId = string.Format("product_option_{0}", attribute.CustomFieldId);
                    var ctrlAttributes = postParams[controlId];
                    if (!string.IsNullOrEmpty(ctrlAttributes))
                    {
                        int selectedAttributeId = -1;
                        if (int.TryParse(ctrlAttributes, out selectedAttributeId))
                        {
                            var lstTmp = GetProductsHasAttribute(lstAvailableProducts, productProperties, attribute.CustomFieldId, selectedAttributeId);
                            if (lstTmp.Count > 0)
                                lstAvailableProducts = lstTmp;
                        }
                    }
                }
            }

            var productPrice = (string)null;
            //var installmentPrice = (string)null;
            //var installmentPrice12Month = (string)null;
            //var installmentUrl1 = (string)null;
            //var installmentUrl2 = (string)null;
            var oldPrice = (string)null;
            var productName = (string)null;
            var productCode = (string)null;
            var briefContent = (string)null;
            var fullContent = (string)null;
            var lstSelectedOptionIds = new List<int>();
            var lstCustomFieldId = new List<int>();
            var stockQuantity = 0;
            var editLink = (string)null;
            var stockUpdateUrl = (string)null;
            var supplierStockQuantity = 0;
            var productId = (int?)null;
            var productUrl = (string)null;
            var childProductDetail = (string)null;
            string discountPercentage = null;
            string giftDescription = null;
            string giftProductTemplate = null;
            string metaTitle = null;
            string metaDescription = null;
            bool EnableTraGop = false;
            bool preOrder = false;
            string preOrderEndDate = string.Empty;
            if (lstAvailableProducts.Count > 0)
            {
                //FlashSale flashSale = FlashSale.GetActive(lstAvailableProducts[0].SiteId);
                //List<FlashSaleAppliedToItem> lstItems = null;//FlashSaleAppliedToItem.GetByFlashSale(flashSale.FlashSaleID);
                //if (flashSale != null && flashSale.FlashSaleID > 0)
                //lstItems = FlashSaleAppliedToItem.GetByFlashSale(flashSale.FlashSaleID);

                var discountApplied = (DiscountAppliedToItem)null;
                List<DiscountAppliedToItem> discountMayBeApplied = new List<DiscountAppliedToItem>();

                var dPrice = ProductHelper.GetPrice(lstAvailableProducts[0], -1, ref discountApplied, ref discountMayBeApplied);//, flashSale, lstItems);
                productPrice = ProductHelper.FormatPrice(dPrice, true);
                if (lstAvailableProducts[0].OldPrice == 0)
                    if (lstAvailableProducts[0].OldPrice > 0 && lstAvailableProducts[0].OldPrice > dPrice)
                        oldPrice = ProductHelper.FormatPrice(lstAvailableProducts[0].OldPrice, true);
                    else if (lstAvailableProducts[0].OldPrice == 0 && lstAvailableProducts[0].Price > dPrice)
                        oldPrice = ProductHelper.FormatPrice(lstAvailableProducts[0].Price, true);

                if (discountApplied != null)
                {
                    if (discountApplied.DiscountPercentage > 0)
                    {
                        discountPercentage = discountApplied.DiscountPercentage.ToString();

                        if (discountApplied.OriginalPrice > dPrice)
                            oldPrice = ProductHelper.FormatPrice(discountApplied.OriginalPrice, true);
                    }

                    //giftDescription = discountApplied.GiftDescription;
                    //giftProductTemplate = GenGiftProtions(discountApplied.GiftProducts, product);
                }

                productName = lstAvailableProducts[0].Title;
                productCode = lstAvailableProducts[0].Code;
                briefContent = lstAvailableProducts[0].BriefContent;
                fullContent = lstAvailableProducts[0].FullContent;
                productId = lstAvailableProducts[0].ProductId;
                productUrl = ProductHelper.FormatProductUrl(lstAvailableProducts[0].Url, lstAvailableProducts[0].ProductId, lstAvailableProducts[0].ZoneId);
                if (product.SpecialPriceEndDate != null && product.SpecialPriceEndDate > DateTime.Now)
                {
                    productPrice = ProductHelper.FormatPrice(product.SpecialPrice, true);
                    preOrderEndDate = product.SpecialPriceEndDate.Value.ToString("dd-MM-yyyy");
                    preOrder = true;
                }
                stockQuantity = lstAvailableProducts[0].StockQuantity;
                if (HttpContext.Current.Request.IsAuthenticated)
                {
                    if (WebUser.IsAdmin)
                    {
                        editLink = GetEditLink(lstAvailableProducts[0].ProductId);
                    }
                }

                lstSelectedOptionIds = productProperties.Where(x => x.ProductId == lstAvailableProducts[0].ProductId).Select(x => x.CustomFieldOptionId).Distinct().ToList();
                lstCustomFieldId = productProperties.Where(x => x.ProductId == lstAvailableProducts[0].ProductId).Select(x => x.CustomFieldId).Distinct().ToList();

                var doc = new XmlDocument();
                doc.LoadXml("<ProductDetail></ProductDetail>");
                var root = doc.DocumentElement;
                XmlHelper.AddNode(doc, root, "Description", lstAvailableProducts[0].FullContent);
                XmlHelper.AddNode(doc, root, "ProductId", product.ProductId.ToString());
                XmlHelper.AddNode(doc, root, "EnableBuyButton", (!(lstAvailableProducts[0].DisableBuyButton)).ToString());
                XmlHelper.AddNode(doc, root, "ACartText", ResourceHelper.GetResourceString("HtmlResources", "ACartText"));
                ProductViewControl.BuildSamePriceProductsXml(doc, root, product, lstAvailableProducts[0], customFields);
                ProductViewControl.BuildProductGroupSpecsXml(doc, root, WorkingCulture.LanguageId, product, lstAvailableProducts[0]);
                BuildProductImageXml(doc, root, lstAvailableProducts[0]);
                BuildProductAttributesXml(doc, root, lstAvailableProducts[0], WorkingCulture.LanguageId);
                BuildProductMediaXml(doc, root, out List<ContentMedia> medias, WorkingCulture.LanguageId, lstAvailableProducts[0], null);

                if (lstAvailableProducts[0].MetaTitle.Length > 0)
                    metaTitle = lstAvailableProducts[0].MetaTitle;
                else
                {
                    var siteSettings = CacheHelper.GetCurrentSiteSettings();
                    metaTitle = SiteUtils.FormatPageTitle(siteSettings, lstAvailableProducts[0].Title);
                }
                if (lstAvailableProducts[0].MetaDescription.Length > 0)
                    metaDescription = lstAvailableProducts[0].MetaDescription;
                if (dPrice > ConfigHelper.GetIntProperty("EnableTraGopWhenPriceBigger", 3000000))
                    EnableTraGop = true;
                XmlHelper.AddNode(doc, root, "OutStock", (lstAvailableProducts[0].StockQuantity < 1) ? "true" : "false");
                if (lstAvailableProducts[0].SpecialPriceEndDate != null && lstAvailableProducts[0].SpecialPriceEndDate > DateTime.Now)
                {
                    XmlHelper.AddNode(doc, root, "PreOrder", "true");
                    XmlHelper.AddNode(doc, root, "SpecialPricePreOrder", ProductHelper.FormatPrice(lstAvailableProducts[0].SpecialPrice, true));
                    XmlHelper.AddNode(doc, root, "PreOrderEndDate", lstAvailableProducts[0].SpecialPriceEndDate.Value.ToString("dd-MM-yyyy"));
                }
                if (dPrice > ConfigHelper.GetIntProperty("EnableTraGopWhenPriceBigger", 3000000))
                    XmlHelper.AddNode(doc, root, "EnableTraGop", "true");

                if (discountApplied != null)
                {
                    if (discountApplied.DiscountPercentage > 0)
                        XmlHelper.AddNode(doc, root, "DiscountPercentage", discountApplied.DiscountPercentage.ToString());
                }

                childProductDetail = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Product", "ProductDetailChild.xslt"), doc);
            }

            if (ConfigHelper.GetBoolProperty("ProductDetail:ShowParentTitle", false))
                productName = product.Title;
            return Json(new
            {
                success = true,
                customFieldIds = lstCustomFieldId,
                optionIds = lstAvailableOptionsIds,
                selectedOptionIds = lstSelectedOptionIds,
                price = productPrice,
                discountPercentage,
                //giftDescription,
                //giftProductTemplate,
                enableTraGop = EnableTraGop,
                outStock = (product.StockQuantity < 1),
                oldPrice,
                productName,
                productCode,
                briefContent,
                fullContent,
                stockQuantity,
                supplierStockQuantity,
                editLink,
                stockUpdateUrl,
                productId,
                productUrl,
                childProductDetail,
                metaTitle,
                metaDescription
            });
        }

        public static string SelectProductOption_Popup(Product product, NameValueCollection postParams)
        {
            var lstChildProducts = ProductHelper.GetChildProducts(product);
            var lstChildProductIds = lstChildProducts.Select(s => s.ProductId).Distinct().ToList();
            var productProperties = ProductProperty.GetPropertiesByProducts(lstChildProductIds);

            var forceHasAttribute = false;
            var customFieldIds = productProperties.Select(x => x.CustomFieldId).Distinct().ToList();
            var customFields = CustomField.GetByOption(CustomField.GetActiveByFields(product.SiteId, Product.FeatureGuid, customFieldIds), CustomFieldOptions.EnableShoppingCart);
            if (customFields.Count > 0)
                forceHasAttribute = true;

            if (!forceHasAttribute)
                return Json(new
                {
                    success = true
                });

            var selectedOptionId = -1;
            if (postParams["optionid"] != null)
                int.TryParse(postParams["optionid"], out selectedOptionId);

            var selectedFieldId = productProperties.Where(s => s.CustomFieldOptionId == selectedOptionId).Select(s => s.CustomFieldId).Distinct().FirstOrDefault();

            var lstAvailableProducts = GetProductsHasAttribute(lstChildProducts, productProperties, selectedFieldId, selectedOptionId);
            if (lstAvailableProducts.Count == 0)
                return Json(new
                {
                    success = false
                });

            var lstAvailableOptionsIds = new List<int>();
            lstAvailableOptionsIds.Add(selectedOptionId);
            foreach (var attribute in customFields)
            {
                if (attribute.CustomFieldId != selectedFieldId)
                {
                    var lstOptionIds = productProperties.Where(s => s.CustomFieldId == attribute.CustomFieldId).Select(s => s.CustomFieldOptionId).ToList();
                    foreach (var optionId in lstOptionIds)
                    {
                        if (!lstAvailableOptionsIds.Contains(optionId))
                        {
                            var lstAvailableByOption = GetProductsHasAttribute(lstAvailableProducts, productProperties, attribute.CustomFieldId, optionId);
                            if (lstAvailableByOption.Count > 0)
                                lstAvailableOptionsIds.Add(optionId);
                        }
                    }
                }
            }

            foreach (var attribute in customFields)
            {
                if (attribute.CustomFieldId != selectedFieldId)
                {
                    var controlId = string.Format("product_option_{0}", attribute.CustomFieldId);
                    var ctrlAttributes = postParams[controlId];
                    if (!string.IsNullOrEmpty(ctrlAttributes))
                    {
                        int selectedAttributeId = -1;
                        if (int.TryParse(ctrlAttributes, out selectedAttributeId))
                        {
                            var lstTmp = GetProductsHasAttribute(lstAvailableProducts, productProperties, attribute.CustomFieldId, selectedAttributeId);
                            if (lstTmp.Count > 0)
                                lstAvailableProducts = lstTmp;
                        }
                    }
                }
            }

            //JSON var
            var productId = (int?)null;
            var productName = (string)null;
            var productUrl = (string)null;
            var productPrice = (string)null;
            var oldPrice = (string)null;
            string discountPercentage = null;
            var productImage = (string)null;
            var lstSelectedOptionIds = new List<int>();
            bool preOrder = false;
            string preOrderEndDate = string.Empty;

            if (lstAvailableProducts.Count > 0)
            {
                productName = lstAvailableProducts[0].Title;

                productUrl = ProductHelper.FormatProductUrl(lstAvailableProducts[0].Url, lstAvailableProducts[0].ProductId, lstAvailableProducts[0].ZoneId);

                var discountApplied = (DiscountAppliedToItem)null;
                List<DiscountAppliedToItem> discountMayBeApplied = new List<DiscountAppliedToItem>();

                var dPrice = ProductHelper.GetPrice(lstAvailableProducts[0], -1, ref discountApplied, ref discountMayBeApplied);//, flashSale, lstItems);
                productPrice = ProductHelper.FormatPrice(dPrice, true);
                if (lstAvailableProducts[0].OldPrice == 0)
                    if (lstAvailableProducts[0].OldPrice > 0 && lstAvailableProducts[0].OldPrice > dPrice)
                        oldPrice = ProductHelper.FormatPrice(lstAvailableProducts[0].OldPrice, true);
                    else if (lstAvailableProducts[0].OldPrice == 0 && lstAvailableProducts[0].Price > dPrice)
                        oldPrice = ProductHelper.FormatPrice(lstAvailableProducts[0].Price, true);

                if (discountApplied != null)
                {
                    if (discountApplied.DiscountPercentage > 0)
                    {
                        discountPercentage = discountApplied.DiscountPercentage.ToString();

                        if (discountApplied.OriginalPrice > dPrice)
                            oldPrice = ProductHelper.FormatPrice(discountApplied.OriginalPrice, true);
                    }
                }
                if (product.SpecialPriceEndDate != null && product.SpecialPriceEndDate > DateTime.Now)
                {
                    productPrice = ProductHelper.FormatPrice(product.SpecialPrice, true);
                    preOrderEndDate = product.SpecialPriceEndDate.Value.ToString("dd-MM-yyyy");
                    preOrder = true;
                }

                var imageFolderPath = ProductHelper.MediaFolderPath(lstAvailableProducts[0].SiteId, lstAvailableProducts[0].ProductId);
                var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
                productImage = ProductHelper.GetMediaFilePath(imageFolderPath, lstAvailableProducts[0].ImageFile);

                lstSelectedOptionIds = productProperties.Where(x => x.ProductId == lstAvailableProducts[0].ProductId).Select(x => x.CustomFieldOptionId).Distinct().ToList();
            }
            if (ConfigHelper.GetBoolProperty("ProductDetail:ShowParentTitle", false))
                productName = product.Title;

            return Json(new
            {
                success = true,
                productId = lstAvailableProducts[0].ProductId,
                productName,
                productUrl,
                price = productPrice,
                oldPrice,
                discountPercentage,
                productImage,
                selectedOptionIds = lstSelectedOptionIds,
            });
        }

        public static List<Product> GetProductsHasAttribute(List<Product> lstChildProducts, List<ProductProperty> productProperties, int customFieldId, int customfieldOptionId)
        {
            var results = new List<Product>();
            foreach (var childProduct in lstChildProducts)
            {
                var iCount = productProperties.Where(s => s.ProductId == childProduct.ProductId && s.CustomFieldId == customFieldId && s.CustomFieldOptionId == customfieldOptionId).Count();
                if (iCount > 0)
                    results.Add(childProduct);
            }

            return results;
        }

        public static void BuildProductImageXml(XmlDocument doc, XmlElement root, Product product)
        {
            var imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
            var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
            var siteRoot = WebUtils.GetSiteRoot();

            var listMedia = ContentMedia.GetByContentDesc(product.ProductGuid);

            foreach (ContentMedia media in listMedia)
            {
                if (media.MediaType != (int)ProductMediaType.Video)
                {
                    XmlElement element = doc.CreateElement("ProductImages");
                    root.AppendChild(element);

                    XmlHelper.AddNode(doc, element, "Title", media.Title);
                    XmlHelper.AddNode(doc, element, "Type", media.MediaType.ToString());
                    XmlHelper.AddNode(doc, element, "ImageUrl", ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile));
                    XmlHelper.AddNode(doc, element, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath, media.ThumbnailFile));
                }
            }
        }

        private static void BuildProductAttributesXml(
            XmlDocument doc,
            XmlElement root, Product product, int languageId)
        {
            List<ContentAttribute> listAttributes = ContentAttribute.GetByContentAsc(product.ProductGuid, languageId);
            foreach (ContentAttribute attribute in listAttributes)
            {
                XmlElement element = doc.CreateElement("ProductAttributes");
                root.AppendChild(element);

                XmlHelper.AddNode(doc, element, "Title", attribute.Title);
                XmlHelper.AddNode(doc, element, "Content", attribute.ContentText);
            }
        }

        private static string GetEditLink(int productId)
        {
            return SiteUtils.GetNavigationSiteRoot()
                   + "/Product/AdminCP/ProductEdit.aspx?ProductID="
                   + productId;
        }

        //public static string GenGiftProtions(string productIds, Product product)
        //{
        //    if (string.IsNullOrEmpty(productIds))
        //        return string.Empty;
        //    var lstProudctsGift = Product.GetPageAdv(publishStatus: 1, productIds: productIds);
        //    var lstDiscountItems = DiscountAppliedToItem.GetActive(product.SiteId, -10, lstProudctsGift);
        //    if (lstProudctsGift.Count > 0)
        //    {
        //        StringBuilder str = new StringBuilder();
        //        foreach (Product it in lstProudctsGift)
        //        {
        //            string imageFolderPath = ProductHelper.MediaFolderPath(it.SiteId, it.ProductId);
        //            string thumbnailImageFolderPath = imageFolderPath + "thumbs/";
        //            str.Append("<div class='item'>");

        //            str.Append(" <div class='img'><img src='" + ProductHelper.GetMediaFilePath(thumbnailImageFolderPath,
        //                          it.ThumbnailFile) + "'></div>");

        //            str.Append("<div class='caption'>");
        //            str.Append("<p>" + BuildProductLink(it) + "</p>");
        //            DiscountAppliedToItem discountApplied = null;
        //            DiscountAppliedToItem discountMayBeApplied = null;
        //            decimal productPrice = ProductHelper.GetPrice(it, -1, ref discountApplied, ref discountMayBeApplied);
        //            str.Append("<div class='price'><span>Trị giá: </span><span>" + ProductHelper.FormatPrice(productPrice) + "</span></div>");
        //            str.Append("</div>");//Close caption

        //            str.Append("</div>");//Close item
        //        }
        //        return str.ToString();
        //    }
        //    else
        //        return string.Empty;
        //}

        public static void BuildProductAttributes(XmlDocument doc, XmlElement root, List<Product> lstChildProducts, List<CustomField> customFields, List<ProductProperty> productProperties, Product product, int childProductId = -1)
        {
            var addedProduct = false;
            if (childProductId > 0)
            {
                var childProduct = ProductHelper.GetProductFromList(lstChildProducts, childProductId);
                if (childProduct != null)
                {
                    var productXml = doc.CreateElement("Product");
                    root.AppendChild(productXml);

                    var lstDiscountItems = DiscountAppliedToItem.GetActive(product.SiteId, -1, new List<Product>() { childProduct });
                    ProductHelper.BuildProductDataXml(doc, productXml, childProduct, lstDiscountItems);
                    XmlHelper.AddNode(doc, productXml, "ParentProductId", childProduct.ParentId.ToString());

                    addedProduct = true;
                }
            }

            if (!addedProduct)
            {
                var productXml = doc.CreateElement("Product");
                root.AppendChild(productXml);

                var listMedia = ContentMedia.GetByContentDesc(product.ProductGuid);
                foreach (ContentMedia media in listMedia)
                {
                    if (media.MediaType != (int)ProductMediaType.Video)
                    {
                        product.ImageFile = media.MediaFile;
                        product.ThumbnailFile = media.ThumbnailFile;
                        break;
                    }
                }

                var lstDiscountItems = DiscountAppliedToItem.GetActive(product.SiteId, -1, new List<Product>() { product });
                ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems);
            }

            var lstOptionAdded = new List<int>();
            foreach (var field in customFields)
            {
                var groupXml = doc.CreateElement("ProductProperties");
                root.AppendChild(groupXml);

                XmlHelper.AddNode(doc, groupXml, "FieldId", field.CustomFieldId.ToString());
                XmlHelper.AddNode(doc, groupXml, "FieldType", field.FieldType.ToString());
                XmlHelper.AddNode(doc, groupXml, "DataType", field.DataType.ToString());
                XmlHelper.AddNode(doc, groupXml, "FilterType", field.FilterType.ToString());
                XmlHelper.AddNode(doc, groupXml, "EnableShoppingCart", "True");
                XmlHelper.AddNode(doc, groupXml, "Title", field.Name);
                XmlHelper.AddNode(doc, groupXml, "DisplayName", field.DisplayName);

                var activeProperty = (ProductProperty)null;
                if (childProductId > 0)
                {
                    activeProperty = productProperties.Where(s => s.ProductId == childProductId && s.CustomFieldId == field.CustomFieldId && s.CustomFieldOptionId > 0).FirstOrDefault();
                    if (activeProperty != null)
                        XmlHelper.AddNode(doc, groupXml, "ActiveOptionId", activeProperty.CustomFieldOptionId.ToString());
                }

                lstOptionAdded.Clear();
                foreach (ProductProperty property in productProperties)
                {
                    if (!lstOptionAdded.Contains(property.CustomFieldOptionId) && property.CustomFieldId == field.CustomFieldId)
                    {
                        lstOptionAdded.Add(property.CustomFieldOptionId);

                        XmlElement optionXml = doc.CreateElement("Options");
                        groupXml.AppendChild(optionXml);

                        XmlHelper.AddNode(doc, optionXml, "FieldId", field.CustomFieldId.ToString());
                        XmlHelper.AddNode(doc, optionXml, "OptionId", property.CustomFieldOptionId.ToString());

                        if (property.CustomFieldOptionId > 0)
                        {
                            XmlHelper.AddNode(doc, optionXml, "Title", property.OptionName);

                            if (activeProperty != null && activeProperty.CustomFieldOptionId == property.CustomFieldOptionId)
                                XmlHelper.AddNode(doc, optionXml, "IsActive", "true");
                        }
                        else
                            XmlHelper.AddNode(doc, optionXml, "Title", property.CustomValue);

                        string pageUrl = SiteUtils.GetCurrentZoneUrl();
                        XmlHelper.AddNode(doc, optionXml, "Url", ProductHelper.GetQueryStringFilter(pageUrl, field.FilterType, field.CustomFieldId,
                                          property.CustomFieldOptionId));

                        if (field.FieldType == (int)CustomFieldType.Color)
                        {
                            if (!string.IsNullOrEmpty(property.OptionColor))
                                XmlHelper.AddNode(doc, optionXml, "Color", property.OptionColor);
                            var lstProductHasColor = CartHelper.GetProductsHasAttribute(lstChildProducts, productProperties, property.CustomFieldId, property.CustomFieldOptionId);
                            if (lstProductHasColor.Count > 0)
                            {
                                var imageFolderPath = ProductHelper.MediaFolderPath(lstProductHasColor[0].SiteId, lstProductHasColor[0].ProductId);
                                var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
                                XmlHelper.AddNode(doc, optionXml, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath, lstProductHasColor[0].ThumbnailFile));
                                XmlHelper.AddNode(doc, optionXml, "ImageUrl", ProductHelper.GetMediaFilePath(imageFolderPath, lstProductHasColor[0].ImageFile));
                            }
                        }
                    }
                }
            }
        }

        private static void BuildProductMediaXml(
          XmlDocument doc,
          XmlElement root, out List<ContentMedia> medias,
          int languageId, Product product, Product childProductWithMinPrice)
        {
            var imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
            var thumbnailImageFolderPath = imageFolderPath + "thumbs/";
            var siteRoot = WebUtils.GetSiteRoot();

            var youtubeVideoRegex = new Regex("youtu(?:\\.be|be\\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            var listMedia = new List<ContentMedia>();
            if (childProductWithMinPrice != null)
            {
                imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, childProductWithMinPrice.ProductId);
                listMedia = ContentMedia.GetByContentDesc(childProductWithMinPrice.ProductGuid);
            }
            else
            {
                imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
                listMedia = ContentMedia.GetByContentDesc(product.ProductGuid);
            }
            medias = listMedia;
            var mediaTypes = new List<int>();
            var listOptions = new List<CustomFieldOption>();
            foreach (ContentMedia media in listMedia)
            {
                if (media.MediaType > 0 && !mediaTypes.Contains(media.MediaType))
                    mediaTypes.Add(media.MediaType);
            }
            if (mediaTypes.Count > 0)
                listOptions = CustomFieldOption.GetByOptionIds(product.SiteId, string.Join(";", mediaTypes.ToArray()));

            if (listOptions.Count > 0)
            {
                foreach (CustomFieldOption option in listOptions)
                {
                    XmlElement element = doc.CreateElement("ProductColors");
                    root.AppendChild(element);
                    XmlHelper.AddNode(doc, element, "Title", option.Name);
                    XmlHelper.AddNode(doc, element, "Color", option.OptionColor);
                    XmlHelper.AddNode(doc, element, "ColorId", option.CustomFieldOptionId.ToString());

                    foreach (ContentMedia media in listMedia)
                    {
                        if (
                            (option.CustomFieldOptionId == media.MediaType)
                            && (media.LanguageId == -1 || media.LanguageId == languageId)
                            && (media.MediaType != (int)ProductMediaType.Video)
                        )
                            BuildProductImagesXml(doc, element, media, imageFolderPath, thumbnailImageFolderPath);
                    }
                }
            }

            foreach (ContentMedia media in listMedia)
            {
                if (media.LanguageId == -1 || media.LanguageId == languageId)
                {
                    if (media.MediaType != (int)ProductMediaType.Video)
                    {
                        BuildProductImagesXml(doc, root, media, imageFolderPath, thumbnailImageFolderPath);

                        string relativePath = siteRoot + ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile);
                    }
                    else
                    {
                        XmlElement element = doc.CreateElement("ProductVideos");
                        root.AppendChild(element);

                        XmlHelper.AddNode(doc, element, "Title", media.Title);
                        XmlHelper.AddNode(doc, element, "DisplayOrder", media.DisplayOrder.ToString());
                        XmlHelper.AddNode(doc, element, "Type", media.MediaType.ToString());
                        XmlHelper.AddNode(doc, element, "VideoUrl", ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile));

                        string thumbnailPath = ProductHelper.GetMediaFilePath(thumbnailImageFolderPath, media.ThumbnailFile);
                        if (media.ThumbnailFile.Length == 0 && media.MediaFile.ContainsCaseInsensitive("youtu"))
                        {
                            Match youtubeMatch = youtubeVideoRegex.Match(media.MediaFile);
                            string videoId = string.Empty;
                            if (youtubeMatch.Success)
                                videoId = youtubeMatch.Groups[1].Value;

                            thumbnailPath = "http://img.youtube.com/vi/" + videoId + "/0.jpg";
                        }

                        XmlHelper.AddNode(doc, element, "ThumbnailUrl", thumbnailPath);
                    }
                }
            }
        }

        private static void BuildProductImagesXml(
            XmlDocument doc,
            XmlElement root,
            ContentMedia media,
            string imageFolderPath,
            string thumbnailImageFolderPath)
        {
            XmlElement element = doc.CreateElement("ProductImages");
            root.AppendChild(element);

            XmlHelper.AddNode(doc, element, "Title", media.Title);
            XmlHelper.AddNode(doc, element, "Type", media.MediaType.ToString());
            XmlHelper.AddNode(doc, element, "ImageUrl", ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile));
            XmlHelper.AddNode(doc, element, "ThumbnailUrl", ProductHelper.GetMediaFilePath(thumbnailImageFolderPath,
                              media.ThumbnailFile));
        }

        #endregion Child products
    }
}
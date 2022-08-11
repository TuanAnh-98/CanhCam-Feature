using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.StoreUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public class CheckoutService : CmsInitBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CheckoutService));

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
                    method != "SaveOrder"
                    && method != "PaymentOrder"
                    && method != "CancelOrder"
                    && method != "GetServiceList"
                    && method != "GetDistrictsByProvinceGuid"
                    && method != "GetShippingTotal"
                    && method != "SpendingPoint"
                    && method != "OrderCalculator"
                    && method != "CheckDistrictHaveFastShipping"
                    && method != "UpdateCartInventories"
                    )
                {
                    Response.Write(StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = "No method found with the " + method
                    }));
                    return;
                }

                switch (method)
                {
                    case "SaveOrder":
                        bool saveToDB = false;
                        if (postParams.Get("savetodb") != null)
                            bool.TryParse(postParams.Get("savetodb"), out saveToDB);
                        Response.Write(SaveOrder(saveToDB));
                        break;

                    case "PaymentOrder":
                        Response.Write(PaymentOrder());
                        break;

                    case "CancelOrder":
                        Response.Write(CancelOrder());
                        break;

                    case "GetDistrictsByProvinceGuid":
                        Response.Write(GetDistrictsByProvinceGuid());
                        break;

                    case "GetServiceList":
                        Response.Write(GetServiceList());
                        break;

                    case "GetShippingTotal":
                        Response.Write(GetShippingTotal());
                        break;

                    case "OrderCalculator":
                        Response.Write(OrderCalculator());
                        break;

                    case "SpendingPoint":
                        Response.Write(SpendingPoint());
                        break;

                    case "CheckDistrictHaveFastShipping":
                        Response.Write(CheckDistrictHaveFastShipping());
                        break;

                    case "UpdateCartInventories":
                        Response.Write(UpdateCartInventories());
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

        private string CancelOrder()
        {
            var orderCode = GetPostValue("ordercode", "");
            if (string.IsNullOrEmpty(orderCode))
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Vui lòng nhập mã đơn"
                });

            var order = Order.GetByCode(orderCode);
            if (order == null || order.OrderId == -1)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Không tìm thấy đơn hàng với mã #" + orderCode
                });

            order.CancelNote = "Khách hàng hủy khi thanh toán lại";
            order.OrderStatus = (int)OrderStatus.Cancelled;
            order.CancelUtc = DateTime.Now;
            if (order.UserGuid != Guid.Empty)
            {
                SiteUser siteUser = new SiteUser(siteSettings, order.UserGuid);
                if (siteUser != null)
                    order.CancelUserId = siteUser.UserId;
            }
            if (order.Save())
            {
                if (ERPHelper.EnableERP)
                {
                    string waring = string.Empty;
                    ERPHelper.CancelOrder(order, new Store(order.StoreId), ref waring);
                }
            }
            return StringHelper.ToJsonString(new
            {
                success = true,
                redirect = SiteUtils.GetHomepageUrl()
            });
        }

        private string CheckDistrictHaveFastShipping()
        {
            if (!ShippingHelper.EnableFastShipping)
                return StringHelper.ToJsonString(new
                {
                    success = false
                });

            var districtGuid = GetPostValue("districtGuid", "");
            ShippingMethod method = ShippingCacheHelper.GetShippingMethodById(ShippingHelper.FastShipMethodId);
            if (method != null
                && districtGuid.Length == 36
                && method.ShippingMethodId > 0
                && method.IsActive)
            {
                var tables = ShippingCacheHelper.GetShippingTable(method.ShippingMethodId, 0, districtGuid, -1);
                if (tables != null)
                    return StringHelper.ToJsonString(new
                    {
                        success = true,
                        enableFastMethod = true
                    });
                else
                    return StringHelper.ToJsonString(new
                    {
                        success = true,
                        enableFastMethod = false
                    });
            }
            return StringHelper.ToJsonString(new
            {
                success = true,
                enableFastMethod = false
            });
        }

        private string PaymentOrder()
        {
            var orderCode = GetPostValue("ordercode", "");
            int.TryParse(GetPostValue("paymentMethodId", "0"), out int paymentMethodID);
            if (string.IsNullOrEmpty(orderCode))
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Vui lòng nhập mã đơn"
                });

            var order = Order.GetByCode(orderCode);
            if (order == null || order.OrderId == -1)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Không tìm thấy đơn hàng với mã #" + orderCode
                });
            PaymentMethod payment = new PaymentMethod(paymentMethodID);

            if (payment == null || payment.PaymentMethodId == -1)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Không tìm thấy phương thức thanh toán phù hợp"
                });

            order.PaymentMethod = payment.PaymentMethodId;
            order.PaymentStatus = (int)PaymentStatus.None;
            order.Save();
            CartHelper.SetOrderSavedSession(order.SiteId, order);
            if (PaymentHelper.IsOnlinePayment(payment.PaymentMethodId))
            {
                var paymentBankCode = GetPostValue("BankCode");
                if (!string.IsNullOrEmpty(paymentBankCode))
                    order.BankCode = paymentBankCode;
                var redirect = PaymentHelper.GetOnlinePaymentRedirectUrl(order, null);
                if (!string.IsNullOrEmpty(redirect))
                {
                    return StringHelper.ToJsonString(new
                    {
                        success = true,
                        redirect
                    });
                }
            }
            else
                OrderHelper.SendMailAfterOrder(order, siteSettings);

            return StringHelper.ToJsonString(new
            {
                success = true,
            });
        }

        private string SaveOrder(bool saveToDB, bool validate = true)
        {
            try
            {
                //validation
                var cart = CartHelper.GetShoppingCart(siteSettings.SiteId, ShoppingCartTypeEnum.ShoppingCart);

                if (cart.Count == 0)
                {
                    return StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = ProductResources.CartIsEmptyLabel
                    });
                }
                if ((!Request.IsAuthenticated && !ProductConfiguration.AnonymousCheckoutAllowed))
                {
                    return StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = ProductResources.CheckoutAnonymousNotAllowed
                    });
                }
                var order = CartHelper.GetOrderSession(siteSettings.SiteId);
                if (order == null)
                    order = new Order();

                order.SiteId = siteSettings.SiteId;
                order.BillingFirstName = GetPostValue("Address_FirstName", order.BillingFirstName);
                order.BillingLastName = GetPostValue("Address_LastName", order.BillingLastName);
                order.BillingEmail = GetPostValue("Address_Email", order.BillingEmail);
                order.BillingAddress = GetPostValue("Address_Address", order.BillingAddress);
                order.BillingPhone = GetPostValue("Address_Phone", order.BillingPhone);
                order.BillingMobile = GetPostValue("Address_Mobile", order.BillingMobile);
                order.BillingFax = GetPostValue("Address_Fax", order.BillingFax);
                order.BillingStreet = GetPostValue("Address_Street", order.BillingStreet);
                order.BillingWard = GetPostValue("Address_Ward", order.BillingWard);
                string district = GetPostValue("Address_District", order.BillingDistrictGuid.ToString());
                if (district.Length == 36)
                    order.BillingDistrictGuid = new Guid(district);
                else
                    order.BillingDistrictGuid = Guid.Empty;

                string province = GetPostValue("Address_Province", order.BillingProvinceGuid.ToString());
                if (province.Length == 36)
                    order.BillingProvinceGuid = new Guid(province);
                else
                    order.BillingProvinceGuid = Guid.Empty;

                string country = GetPostValue("Address_Country", order.BillingCountryGuid.ToString());
                if (country.Length == 36)
                    order.BillingCountryGuid = new Guid(country);
                else
                    order.BillingCountryGuid = Guid.Empty;

                #region Shipping Address

                string cbShipToOtherAddress = GetPostValue("Address_SameBilling", "off");
                if (cbShipToOtherAddress == "on")
                {
                    order.ShippingFirstName = GetPostValue("ShippingAddress_FirstName", order.ShippingFirstName);
                    order.ShippingPhone = GetPostValue("ShippingAddress_Phone", order.ShippingPhone);
                    order.ShippingMobile = GetPostValue("ShippingAddress_Mobile", order.ShippingMobile);
                    order.ShippingEmail = GetPostValue("ShippingAddress_Email", order.ShippingEmail);
                    order.ShippingAddress = GetPostValue("ShippingAddress_Address", order.ShippingAddress);
                    string shippingCountry = GetPostValue("ShippingAddress_Country", order.ShippingCountryGuid.ToString());
                    if (shippingCountry.Length == 36)
                        order.ShippingCountryGuid = new Guid(shippingCountry);
                    else
                        order.ShippingCountryGuid = Guid.Empty;
                    string shippingProvince = GetPostValue("ShippingAddress_Province", order.ShippingProvinceGuid.ToString());
                    if (shippingProvince.Length == 36)
                        order.ShippingProvinceGuid = new Guid(shippingProvince);
                    else
                        order.ShippingProvinceGuid = Guid.Empty;
                    string shippingDistrict = GetPostValue("ShippingAddress_District", order.ShippingDistrictGuid.ToString());
                    if (shippingDistrict.Length == 36)
                        order.ShippingDistrictGuid = new Guid(shippingDistrict);
                    else
                        order.ShippingDistrictGuid = Guid.Empty;
                    string shippingWard = GetPostValue("ShippingAddress_Ward", order.ShippingWard);
                    if (shippingWard.Length == 36)
                        order.ShippingWard = shippingWard;
                    else
                        order.ShippingWard = string.Empty;
                }

                #endregion Shipping Address

                // Shipping method
                bool hasShipping = false;
                foreach (var key in postParams.AllKeys)
                {
                    if (key == "ShippingMethod")
                    {
                        hasShipping = true;
                        break;
                    }
                }
                if (hasShipping)
                {
                    order.ShippingMethod = -1;
                    string shippingMethod = GetPostValue("ShippingMethod");
                    var lstShippingMethods = ShippingMethod.GetByActive(siteSettings.SiteId, 1);
                    foreach (ShippingMethod shipping in lstShippingMethods)
                    {
                        if (shippingMethod == shipping.ShippingMethodId.ToString())
                        {
                            order.ShippingMethod = shipping.ShippingMethodId;
                            break;
                        }
                    }

                    if (order.ShippingMethod == -1)
                    {
                        return StringHelper.ToJsonString(new
                        {
                            success = false,
                            message = ProductResources.CheckoutShippingMethodRequired
                        });
                    }
                }

                // Payment method
                bool hasPayment = false;
                foreach (var key in postParams.AllKeys)
                {
                    if (key == "PaymentMethod")
                    {
                        hasPayment = true;
                        break;
                    }
                }
                if (hasPayment)
                {
                    order.PaymentMethod = -1;
                    var paymentMethod = GetPostValue("PaymentMethod");
                    var lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, 1);
                    foreach (PaymentMethod payment in lstPaymentMethods)
                    {
                        if (paymentMethod == payment.PaymentMethodId.ToString())
                        {
                            order.PaymentMethod = payment.PaymentMethodId;
                            break;
                        }
                    }
                    var paymentBankCode = GetPostValue("BankCode");
                    if (!string.IsNullOrEmpty(paymentBankCode))
                        order.BankCode = paymentBankCode;
                }

                order.InvoiceCompanyName = GetPostValue("Invoice_CompanyName", order.InvoiceCompanyName);
                order.InvoiceCompanyAddress = GetPostValue("Invoice_CompanyAddress", order.InvoiceCompanyAddress);
                order.InvoiceCompanyTaxCode = GetPostValue("Invoice_CompanyTaxCode", order.InvoiceCompanyTaxCode);
                order.OrderNote = GetPostValue("OrderNote", order.OrderNote);
                order.Affiliate = GetPostValue("AffiliateUserID", order.OrderNote);
                string result = string.Empty;
                if (validate)
                {
                    if (!IsBillingAddressValid(order, out result))
                        return result;
                    if (!IsShippingAddressValid(order, out result))
                        return result;
                }
                var siteUser = SiteUtils.GetCurrentSiteUser();

                if (siteUser != null && RewardPointsHelper.Enable)
                {
                    int.TryParse(GetPostValue("RedeemedRewardPoints", "0"), out int redeemedRewardPoints);
                    if (redeemedRewardPoints > 0)
                    {
                        order.RedeemedRewardPoints = redeemedRewardPoints;
                        order.RedeemedRewardPointsAmount = RewardPointsHelper.ConvertRewardPointsToAmount(redeemedRewardPoints);
                    }
                }

                if (saveToDB)
                {
                    #region Store

                    if (StoreHelper.EnabledStore)
                    {
                        int.TryParse(GetPostValue("Store_ID", order.StoreId.ToString()), out int storeID);
                        order.StoreId = storeID;
                        Store storeForOrder = null;
                        storeForOrder = StoreHelper.SettoreForOrder(order, cart);

                        if (storeForOrder == null)
                            return StringHelper.ToJsonString(new
                            {
                                success = false,
                                message = StoreResources.StoreNotFoundInValid
                            });
                        else
                            order.StoreId = storeForOrder.StoreID;
                    }

                    #endregion Store

                    order.OrderCode = ProductHelper.GenerateOrderCode(order.SiteId);
                    order.CreatedFromIP = SiteUtils.GetIP4Address();
                    order.Source = ((int)OrderSource.Website).ToString();

                    var address = (UserAddress)null;
                    if (siteUser != null)
                    {
                        order.UserGuid = siteUser.UserGuid;

                        var lstAddress = UserAddress.GetByUser(siteUser.UserId);
                        if (lstAddress.Count == 0)
                        {
                            address = new UserAddress
                            {
                                UserId = siteUser.UserId,
                                FirstName = order.BillingFirstName,
                                LastName = order.BillingLastName,
                                Company = order.InvoiceCompanyName,
                                Phone = order.BillingPhone,
                                Address = order.BillingAddress,
                                IsDefault = true,
                                ProvinceGuid = order.BillingProvinceGuid,
                                DistrictGuid = order.BillingDistrictGuid
                            };
                        }
                    }

                    order.Save();
                    if (SaveOrderSummary(order, cart))
                    {
                        if (address != null)
                            address.Save();
                        CouponHelper.CouponCodeInput = null;
                        CartHelper.ClearCartCookie(order.SiteId);
                        CartHelper.SetOrderSession(siteSettings.SiteId, null);
                        CartHelper.SetOrderSavedSession(order.SiteId, null);//clear old data (only save in session) at old Order
                        CartHelper.SetOrderSavedSession(order.SiteId, order);
                        var redirect = PaymentHelper.GetOnlinePaymentRedirectUrl(order, cart);
                        if (!string.IsNullOrEmpty(redirect))
                        {
                            return StringHelper.ToJsonString(new
                            {
                                success = true,
                                redirect
                            });
                        }
                    }
                }
                else
                    CartHelper.SetOrderSession(siteSettings.SiteId, order);

                return StringHelper.ToJsonString(new
                {
                    success = true,
                    redirect = GetPostValue("redirect", string.Empty)
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
                return StringHelper.ToJsonString(new { success = false, message = ex.Message });
            }
        }

        private bool SaveOrderSummary(Order order, List<ShoppingCartItem> cartList)
        {
            var lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId), ShoppingCartTypeEnum.ShoppingCart);

            var customFields = new List<CustomField>();
            var productProperties = new List<ProductProperty>();
            if (ProductConfiguration.EnableShoppingCartAttributes && ProductConfiguration.EnableAttributesPriceAdjustment)
            {
                var lstProductIds = lstProductsInCart.Select(x => x.ProductId).Distinct().ToList();
                productProperties = ProductProperty.GetPropertiesByProducts(lstProductIds);
                if (productProperties.Count > 0)
                {
                    var customFieldIds = productProperties.Select(x => x.CustomFieldId).Distinct().ToList();
                    customFields = CustomField.GetByOption(CustomField.GetActiveByFields(siteSettings.SiteId, Product.FeatureGuid, customFieldIds), CustomFieldOptions.EnableShoppingCart);
                }
            }

            //var couponIsAvailable = false;
            var coupon = (DiscountCoupon)null;
            var lstDiscountItems = (List<DiscountAppliedToItem>)null;
            var lstDiscountId = new List<int>();
            var giftDescription = string.Empty;
            var productGifts = string.Empty;
            var discountPayment = 0M;
            var discountTotal = cartList.GetDiscountTotal(lstProductsInCart,
                                    ref lstDiscountItems, ref lstDiscountId, ref giftDescription, ref productGifts, ref coupon, ref discountPayment, order.PaymentMethod);
            var discountCouponTotal = coupon != null ? coupon.DiscountAmount : 0;

            if (ProductConfiguration.EnableSaveDiscountPayment && discountPayment > 0)
                discountTotal += discountPayment;

            if (!string.IsNullOrEmpty(giftDescription))
                order.GiftDescription = giftDescription;

            var lstOrderItem = new List<OrderItem>();
            foreach (ShoppingCartItem itm in cartList)
            {
                var product = ProductHelper.GetProductFromList(lstProductsInCart, itm.ProductId);
                if (product != null)
                {
                    var orderItem = new OrderItem
                    {
                        DiscountQuantity = itm.DiscountQuantity,
                        DiscountAppliedGuid = itm.DiscountAppliedGuid,
                        OrderId = order.OrderId,
                        Price = ProductHelper.GetOriginalPrice(product),
                        //orderItem.UnitPrice = price;
                        DiscountAmount = itm.ItemDiscount - itm.ItemDiscountCoupon,
                        CouponAmount = itm.ItemDiscountCoupon,
                        ProductId = itm.ProductId,
                        Quantity = itm.Quantity,
                        AttributesXml = itm.AttributesXml,
                        AttributeDescription = string.Empty,
                        AffiliateUserID = itm.AffiliateUserID
                        
                        
                    };
                    if (itm.GiftMessage.Length > 0)
                        orderItem.AttributeDescription = itm.GiftMessage;

                    orderItem.Save();
                    lstOrderItem.Add(orderItem);
                }

                ShoppingCartItem.Delete(itm.Guid);
            }

            var shippingPrice = decimal.Zero;
            List<int> shippingMethods = new List<int>();
            List<int> paymentMethods = new List<int>();
            decimal orderWeight = cartList.GetTotalWeights(lstProductsInCart);
            bool isRestricted = ShippingHelper.IsRestrictedArea(order, orderWeight, ref shippingMethods, ref paymentMethods);

            if (order.ShippingMethod > 0 && !isRestricted)
            {
                decimal orderSubTotal = cartList.GetSubTotal(lstProductsInCart);
                int productTotalQty = cartList.GetTotalProducts();
                string geoZoneGuids = ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order);
                string strServiceID = GetPostValue("ShippingService");
                string expectedTime = string.Empty;
                shippingPrice = ProductHelper.GetShippingPrice(order.ShippingMethod, orderSubTotal, orderWeight,
                    productTotalQty, geoZoneGuids, order.BillingDistrictGuid, strServiceID, order.StoreId, StoreCacheHelper.GetStoreById(order.StoreId), ref expectedTime);
                order.ShippingOption = strServiceID;
            }
            var subTotal = cartList.GetSubTotal(lstProductsInCart);

            var siteUser = SiteUtils.GetCurrentSiteUser();
            //reward points history
            if (siteUser != null && siteUser.UserId > 0
                && order.RedeemedRewardPointsAmount > decimal.Zero)
            {
                int maxPointCanUse = RewardPointsHelper.CalculateRewardPoints(siteUser, CartHelper.GetCartTotal(subTotal, discountTotal: discountTotal, discountCouponTotal: discountCouponTotal));

                if (order.RedeemedRewardPoints > maxPointCanUse)
                {
                    order.RedeemedRewardPoints = maxPointCanUse;
                }

                RewardPointsHelper.AddRewardPointsHistory(siteUser.SiteId, siteUser, (int)RewardPointType.Redeemed,
                    -order.RedeemedRewardPoints, string.Format(ProductResources.RedeemedForOrder, order.OrderCode),
                    order.OrderId, order.RedeemedRewardPointsAmount);

                ERPHelper.AddRewardPointsHistory(siteUser, order, new Store(order.StoreId), -order.RedeemedRewardPoints);
            }
            decimal serviceFee = decimal.Zero;
            if (ProductServiceHelper.Enable)
                serviceFee = ProductServiceHelper.GetServiceFee(lstProductsInCart, cartList, subTotal - discountTotal);
            var voucherAmount = VoucherHelper.GetVoucherAmountTotal();

            var totalTemp = CartHelper.GetCartTotal(subTotal: subTotal,
                shippingTotal: shippingPrice,
                discountTotal: discountTotal,
                discountCouponTotal: discountCouponTotal,
                rewardPointAmount: order.RedeemedRewardPointsAmount,
                serviceFee: serviceFee);

            if (totalTemp <= voucherAmount)
                voucherAmount = totalTemp;

            var total = CartHelper.GetCartTotal(subTotal: subTotal,
                shippingTotal: shippingPrice,
                discountTotal: discountTotal,
                discountCouponTotal: discountCouponTotal,
                rewardPointAmount: order.RedeemedRewardPointsAmount,
                voucherAmount: voucherAmount,
                serviceFee: serviceFee);

            var codes = VoucherHelper.GetVoucherCodeApplied();
            if (codes.Count > 0)
            {
                order.VoucherCodes = string.Join(";", codes);

                foreach (var code in codes)
                {
                    Voucher voucher = new Voucher(code);
                    voucher.UseCount++;
                    if (string.IsNullOrEmpty(voucher.OrderCodesUsed))
                        voucher.OrderCodesUsed = order.OrderCode;
                    else
                        voucher.OrderCodesUsed += ";" + order.OrderCode;
                    voucher.Save();
                    if (ERPHelper.EnableERP && ERPHelper.EnableValidVoucherInERP)
                        ERPHelper.UpdateVoucherStatus(code, true, order.OrderCode);
                }
                VoucherHelper.ClearVoucherCodeApply();
            }
            order.DiscountPayment = discountPayment;//not save database only session
            order.OrderServiceFee = serviceFee;
            order.VoucherAmount = voucherAmount;
            order.OrderDiscount = discountTotal;
            order.OrderSubtotal = subTotal;
            order.OrderShipping = shippingPrice;
            order.OrderCouponAmount = discountCouponTotal;
            order.OrderTotal = total;
            order.TotalWeight = orderWeight;
            if (coupon != null)
            {
                order.CouponCode = coupon.CouponCode;
                if (ERPHelper.EnableERP && ERPHelper.EnableValidCouponInERP)
                    ERPHelper.UpdateCouponStatus(coupon.CouponCode, true, order.OrderCode);
            }

            try
            {
                if (lstDiscountId.Count > 0)
                {
                    foreach (var discountId in lstDiscountId)
                    {
                        var obj = new DiscountUsageHistory
                        {
                            OrderId = order.OrderId,
                            DiscountId = discountId
                        };
                        if (coupon != null && coupon.DiscountID == discountId)
                            obj.CouponCode = coupon.CouponCode;
                        obj.Save();
                    }

                    order.DiscountIDs = string.Join(";", lstDiscountId.ToArray());
                    order.ProductGifts = productGifts;

                    //Update sold quantity for Deal
                    var ids = lstOrderItem.Select(s => s.ProductId).ToList();
                    var lstDiscountItems2 = lstDiscountItems.Where(s => s.DiscountType == (int)DiscountType.Deal && s.ItemId > 0 && ids.Contains(s.ItemId) && s.AppliedType == (int)DiscountAppliedType.ToProducts).ToList();
                    if (lstDiscountItems2.Count > 0)
                    {
                        foreach (var item in lstDiscountItems2)
                        {
                            var obj = lstOrderItem.Where(s => s.ProductId == item.ItemId).FirstOrDefault();
                            if (obj != null)
                            {
                                var objSave = new DiscountAppliedToItem(item.Guid);
                                if (objSave != null && objSave.Guid != Guid.Empty)
                                {
                                    if (obj.DiscountQuantity.HasValue && obj.DiscountQuantity.Value > 0)
                                        objSave.SoldQty += obj.DiscountQuantity.Value;
                                    else
                                        objSave.SoldQty += obj.Quantity;
                                    objSave.Save();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            order.Save();

            if (coupon != null && !string.IsNullOrEmpty(order.CouponCode))
                DiscountCoupon.UpdateUseCount(order.CouponCode);
            var billingProvinceName = string.Empty;
            var billingDistrictName = string.Empty;
            var shippingProvinceName = string.Empty;
            var shippingDistrictName = string.Empty;
            if (order.BillingProvinceGuid != Guid.Empty)
            {
                var province = new GeoZone(order.BillingProvinceGuid);
                if (province != null && province.Guid != Guid.Empty)
                    billingProvinceName = province.Name;
            }
            if (order.BillingDistrictGuid != Guid.Empty)
            {
                var province = new GeoZone(order.BillingDistrictGuid);
                if (province != null && province.Guid != Guid.Empty)
                    billingDistrictName = province.Name;
            }
            if (order.ShippingProvinceGuid != Guid.Empty)
            {
                var province = new GeoZone(order.ShippingProvinceGuid);
                if (province != null && province.Guid != Guid.Empty)
                    shippingProvinceName = province.Name;
            }
            if (order.ShippingDistrictGuid != Guid.Empty)
            {
                var province = new GeoZone(order.ShippingDistrictGuid);
                if (province != null && province.Guid != Guid.Empty)
                    shippingDistrictName = province.Name;
            }

            var toEmail = order.BillingEmail.Trim();
            if (
                order.ShippingEmail.Length > 0
                && !string.Equals(toEmail, order.ShippingEmail, StringComparison.CurrentCultureIgnoreCase)
                )
                toEmail += "," + order.ShippingEmail;

            if (PaymentHelper.IsOnlinePayment(order.PaymentMethod))
                return true;

            ProductHelper.SendOrderPlacedNotification(siteSettings, order, lstProductsInCart, lstOrderItem,
                "OrderPlacedCustomerNotification", billingProvinceName, billingDistrictName,
                shippingProvinceName, shippingDistrictName,
                toEmail, isRestricted);

            if (order.StoreId > 0)
            {
                var store = StoreCacheHelper.GetStoreById(order.StoreId);
                if (store != null && !string.IsNullOrEmpty(store.Email))
                {
                    ProductHelper.SendOrderPlacedNotification(siteSettings, order, lstProductsInCart, lstOrderItem,
                        "OrderPlacedStoreNotification", billingProvinceName,
                        billingDistrictName, shippingProvinceName,
                        shippingDistrictName, store.Email, isRestricted);
                }
            }

            WebTaskManager.StartOrResumeTasks();

            return true;
        }

        #region IsBillingAddressValid

        private bool IsBillingAddressValid(Order order, out string result)
        {
            result = string.Empty;

            // Check billing address required
            string requiredFields = ProductConfiguration.OrderAddressRequiredFields;
            if (IsEmpty(requiredFields, "Address_FirstName", order.BillingFirstName))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressFirstNameRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_LastName", order.BillingLastName))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressLastNameRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Email", order.BillingEmail))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressEmailRequired
                });

                return false;
            }
            // Check email validate
            if (!string.IsNullOrWhiteSpace(order.BillingEmail))
            {
                if (!IsValidEmail(order.BillingEmail))
                {
                    result = StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = ProductResources.CheckoutAddressEmailInvalid
                    });

                    return false;
                }
            }
            if (IsEmpty(requiredFields, "Address_Address", order.BillingAddress))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Phone", order.BillingPhone))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressPhoneRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Mobile", order.BillingMobile))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressMobileRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Fax", order.BillingFax))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressFaxRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Street", order.BillingStreet))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressStreetRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Ward", order.BillingWard))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressWardRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_District", order.BillingDistrictGuid == Guid.Empty ? string.Empty : order.BillingDistrictGuid.ToString()))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressDistrictRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "Address_Province", order.BillingProvinceGuid == Guid.Empty ? string.Empty : order.BillingProvinceGuid.ToString()))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressProvinceRequired
                });

                return false;
            }

            return true;
        }

        #endregion IsBillingAddressValid

        #region IsShippingAddressValid

        private bool IsShippingAddressValid(Order order, out string result)
        {
            result = string.Empty;

            // Check shipping address required
            string requiredFields = ProductConfiguration.OrderAddressRequiredFields;
            if (IsEmpty(requiredFields, "ShippingAddress_FirstName", order.ShippingFirstName))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressFirstNameRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "ShippingAddress_LastName", order.ShippingLastName))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressLastNameRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "ShippingAddress_Email", order.ShippingEmail))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressEmailRequired
                });

                return false;
            }
            // Check email validate
            if (!string.IsNullOrWhiteSpace(order.ShippingEmail))
            {
                if (!IsValidEmail(order.ShippingEmail))
                {
                    result = StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = ProductResources.CheckoutAddressEmailInvalid
                    });

                    return false;
                }
            }
            if (IsEmpty(requiredFields, "ShippingAddress_Address", order.ShippingAddress))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "ShippingAddress_Phone", order.ShippingPhone))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressPhoneRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "ShippingAddress_Mobile", order.ShippingMobile))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressMobileRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "ShippingAddress_District", order.ShippingDistrictGuid == Guid.Empty ? string.Empty : order.ShippingDistrictGuid.ToString()))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressDistrictRequired
                });

                return false;
            }
            if (IsEmpty(requiredFields, "ShippingAddress_Province", order.ShippingProvinceGuid == Guid.Empty ? string.Empty : order.BillingProvinceGuid.ToString()))
            {
                result = StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CheckoutAddressProvinceRequired
                });

                return false;
            }

            return true;
        }

        #endregion IsShippingAddressValid

        private bool IsEmpty(string requiredFields, string name, string value)
        {
            if (string.IsNullOrWhiteSpace(value) && requiredFields.Contains(name))
            {
                return true;
            }

            return false;
        }

        private string GetPostValue(string name, string defaultValueIfEmpty = "")
        {
            if (postParams[name] != null)
                return postParams[name];

            return defaultValueIfEmpty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public string GetDistrictsByProvinceGuid()
        {
            string provinceGuid = GetPostValue("provinceGuid");
            var lstDistricts = new List<GeoZone>();
            if (provinceGuid.Length == 36)
                lstDistricts = GeoZoneCacheHelper.GetDistrict(WorkingCulture.LanguageId, new Guid(provinceGuid));
            var result = (from s in lstDistricts
                          select new { guid = s.Guid.ToString(), name = s.Name }).ToList();

            result.Insert(0, new { guid = "", name = ProductResources.CheckoutSelectDistrict });

            return StringHelper.ToJsonString(result);
        }

        public string GetShippingTotal()
        {
            try
            {
                string strShippingMethodId = GetPostValue("shippingMethodId");
                int.TryParse(strShippingMethodId, out int shippingMethodId);
                string VitetelServiceCode = GetPostValue("GHNServiceID");
                string strProvinceGuid = GetPostValue("ProvinceGuid");
                string strDistrictGuid = GetPostValue("DistrictGuid");

                if (shippingMethodId > 0)
                {
                    var cartType = ShoppingCartTypeEnum.ShoppingCart;
                    var lstProductsInCart = new List<Product>();
                    var cartList = CartHelper.GetShoppingCart(siteSettings.SiteId, cartType);
                    if (cartList.Count > 0) lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId), cartType);

                    var order = CartHelper.GetOrderSession(siteSettings.SiteId);
                    var clearSession = false;
                    if (order == null)
                    {
                        SaveOrder(false);
                        order = CartHelper.GetOrderSession(siteSettings.SiteId);
                        clearSession = true;
                    }

                    var orderSubTotal = cartList.GetSubTotal(lstProductsInCart);
                    var orderWeight = cartList.GetTotalWeights(lstProductsInCart);
                    var productTotalQty = cartList.GetTotalProducts();
                    var geoZoneGuids = ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order);
                    string expectedTime = string.Empty;
                    var shippingPrice = ProductHelper.GetShippingPrice(shippingMethodId, orderSubTotal, orderWeight, productTotalQty, geoZoneGuids,
                             order.BillingDistrictGuid, VitetelServiceCode, order.StoreId, StoreCacheHelper.GetStoreById(order.StoreId), ref expectedTime);
                    //var shippingPrice = ProductHelper.GetShippingPrice(shippingMethodId, orderSubTotal, orderWeight, productTotalQty, geoZoneGuids);
                    var subTotal = cartList.GetSubTotal(lstProductsInCart);

                    //var couponIsAvailable = false;
                    var coupon = (DiscountCoupon)null;
                    var lstDiscountItems = (List<DiscountAppliedToItem>)null;
                    var lstDiscountId = new List<int>();
                    var giftDescription = string.Empty;
                    var productGifts = string.Empty;
                    var discountPayment = 0M;
                    var discountTotal = cartList.GetDiscountTotal(lstProductsInCart,
                                            ref lstDiscountItems, ref lstDiscountId, ref giftDescription, ref productGifts, ref coupon, ref discountPayment, order.PaymentMethod);

                    var discountCouponTotal = coupon != null ? coupon.DiscountAmount : 0;

                    if (ProductConfiguration.EnableSaveDiscountPayment && discountPayment > 0)
                        discountTotal += discountPayment;

                    var total = CartHelper.GetCartTotal(subTotal, shippingPrice, discountTotal: discountTotal, discountCouponTotal: discountCouponTotal);

                    if (clearSession)
                        CartHelper.SetOrderSession(siteSettings.SiteId, null);

                    return StringHelper.ToJsonString(new
                    {
                        success = true,
                        shippingtotal = Convert.ToDouble(shippingPrice),
                        shippingtotalsectionhtml = ProductHelper.FormatPrice(shippingPrice, true),
                        totalsectionhtml = ProductHelper.FormatPrice(total, true)
                    });
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return StringHelper.ToJsonString(new
            {
                success = false
            });
        }

        private string UpdateCartInventories()
        {
            if (!ProductConfiguration.ValidateInventoryEnable
                || !StoreHelper.EnabledStore)
                return StringHelper.ToJsonString(new
                {
                    success = false
                });
            try
            {
                int.TryParse(GetPostValue("ShippingMethod"), out int shippingMethodId);
                int.TryParse(GetPostValue("PaymentMethod"), out int paymentMethod);
                string VitetelServiceCode = GetPostValue("ShippingService");
                string strProvinceGuid = GetPostValue("ProvinceGuid");
                string strDistrictGuid = GetPostValue("DistrictGuid");

                var cartType = ShoppingCartTypeEnum.ShoppingCart;
                var lstProductsInCart = new List<Product>();
                var cartList = CartHelper.GetShoppingCart(siteSettings.SiteId, cartType);
                if (cartList.Count > 0) lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId), cartType);
                var order = CartHelper.GetOrderSession(siteSettings.SiteId);
                if (order == null)
                {
                    SaveOrder(false);
                    order = CartHelper.GetOrderSession(siteSettings.SiteId);
                }
                int.TryParse(GetPostValue("Store_ID", order.StoreId.ToString()), out int storeID);
                order.StoreId = storeID;
                Store storeForOrder = null;
                storeForOrder = StoreHelper.SettoreForOrder(order, cartList);

                if (storeForOrder == null)
                    return StringHelper.ToJsonString(new
                    {
                        success = false,
                        message = StoreResources.StoreNotFoundInValid
                    });
                else
                    order.StoreId = storeForOrder.StoreID;
                var geoZoneGuids = ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order);
                Guid? billingGuid = null;
                if (order != null)
                    billingGuid = order.BillingDistrictGuid;
                else if (!string.IsNullOrEmpty(strDistrictGuid) && strDistrictGuid.Length == 36)
                    billingGuid = new Guid(strDistrictGuid);

                Store store = null;
                var validateInventories = StoreHelper.ValidateInventoryOrder(order, cartList, lstProductsInCart, ref store);

                if (validateInventories != null && validateInventories.Any(it => !it.IsValid))
                {
                    foreach (var item in validateInventories)
                    {
                        if (!item.IsValid)
                        {
                            var cartItem = cartList.FirstOrDefault(it => it.Guid == item.CartItemGuid);

                            if (item.StockQuantityAvailability == 0)
                                ShoppingCartItem.Delete(item.CartItemGuid);
                            else if (item.StockQuantityAvailability < cartItem.Quantity)
                            {
                                cartItem.Quantity = item.StockQuantityAvailability;
                                cartItem.Save();
                            }
                        }
                    }
                }
                return StringHelper.ToJsonString(new
                {
                    success = true
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return StringHelper.ToJsonString(new
            {
                success = false
            });
        }

        private string OrderCalculator()
        {
            try
            {
                int.TryParse(GetPostValue("ShippingMethod"), out int shippingMethodId);
                int.TryParse(GetPostValue("PaymentMethod"), out int paymentMethod);
                string VitetelServiceCode = GetPostValue("ShippingService");
                string strProvinceGuid = GetPostValue("ProvinceGuid");
                string strDistrictGuid = GetPostValue("DistrictGuid");

                var cartType = ShoppingCartTypeEnum.ShoppingCart;
                var lstProductsInCart = new List<Product>();
                var cartList = CartHelper.GetShoppingCart(siteSettings.SiteId, cartType);
                if (cartList.Count > 0)
                    lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId), cartType);
                var order = CartHelper.GetOrderSession(siteSettings.SiteId);
                var clearSession = false;
                if (order == null)
                {
                    SaveOrder(false, false);
                    order = CartHelper.GetOrderSession(siteSettings.SiteId);
                    clearSession = true;
                }
                if (!string.IsNullOrEmpty(strProvinceGuid) && strProvinceGuid.Length == 36)
                    order.BillingProvinceGuid = new Guid(strProvinceGuid);

                if (!string.IsNullOrEmpty(strDistrictGuid) && strDistrictGuid.Length == 36)
                    order.BillingDistrictGuid = new Guid(strDistrictGuid);
                if (order.SiteId <= 0)
                    order.SiteId = siteSettings.SiteId;
                if (StoreHelper.EnabledStore)
                {
                    int.TryParse(GetPostValue("Store_ID", order.StoreId.ToString()), out int storeID);
                    order.StoreId = storeID;
                    Store storeForOrder = null;
                    storeForOrder = StoreHelper.SettoreForOrder(order, cartList);

                    if (storeForOrder == null)
                        return StringHelper.ToJsonString(new
                        {
                            success = false,
                            message = StoreResources.StoreNotFoundInValid
                        });
                    else
                        order.StoreId = storeForOrder.StoreID;
                }
                var orderSubTotal = cartList.GetSubTotal(lstProductsInCart);
                var orderWeight = cartList.GetTotalWeights(lstProductsInCart);
                var productTotalQty = cartList.GetTotalProducts();
                var geoZoneGuids = ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order);
                Guid? billingGuid = null;
                if (order != null)
                    billingGuid = order.BillingDistrictGuid;
                else if (!string.IsNullOrEmpty(strDistrictGuid) && strDistrictGuid.Length == 36)
                    billingGuid = new Guid(strDistrictGuid);
                decimal shippingPrice = decimal.Zero;
                List<int> shippingMethods = new List<int>();
                List<int> paymentMethods = new List<int>();
                bool isRestricted = ShippingHelper.IsRestrictedArea(order, orderWeight, ref shippingMethods, ref paymentMethods);
                string expectedTime = string.Empty;
                if (!isRestricted)
                    shippingPrice = ProductHelper.GetShippingPrice(shippingMethodId,
                       orderSubTotal,
                       orderWeight,
                       productTotalQty,
                       geoZoneGuids,
                       billingGuid,
                       VitetelServiceCode, order.StoreId, StoreCacheHelper.GetStoreById(order.StoreId), ref expectedTime);
                var subTotal = cartList.GetSubTotal(lstProductsInCart);
                //var couponIsAvailable = false;
                var coupon = (DiscountCoupon)null;
                var lstDiscountItems = (List<DiscountAppliedToItem>)null;
                var lstDiscountId = new List<int>();
                var giftDescription = string.Empty;
                var productGifts = string.Empty;
                var discountPayment = 0M;
                var discountTotal = cartList.GetDiscountTotal(lstProductsInCart,
                                        ref lstDiscountItems, ref lstDiscountId,
                                        ref giftDescription, ref productGifts, ref coupon, ref discountPayment, paymentMethod);
                var discountCouponTotal = coupon != null ? coupon.DiscountAmount : 0;

                if (ProductConfiguration.EnableSaveDiscountPayment && discountPayment > 0)
                    discountTotal += discountPayment;
                decimal rewardPointAmount = 0;
                var siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null && RewardPointsHelper.Enable)
                {
                    int.TryParse(postParams.Get("RedeemedRewardPoints"), out int point);
                    RewardPointsHelper.GetRewardPointsBalance(siteUser, out decimal pointBalance, out decimal pointsAvallable);
                    if (point > 0)
                    {
                        rewardPointAmount = RewardPointsHelper.ConvertRewardPointsToAmount(point);
                        decimal totalWithOutShippingFee = CartHelper.GetCartTotal(subTotal, discountTotal: discountTotal);
                        if (rewardPointAmount > totalWithOutShippingFee)
                            rewardPointAmount = totalWithOutShippingFee;
                    }
                }
                decimal voucherAmount = VoucherHelper.GetVoucherAmountTotal();

                decimal serviceFee = decimal.Zero;
                if (ProductServiceHelper.Enable)
                    serviceFee = ProductServiceHelper.GetServiceFee(lstProductsInCart, cartList, subTotal - discountTotal);

                var total = CartHelper.GetCartTotal(subTotal, shippingPrice,
                    discountTotal: discountTotal,
                    discountCouponTotal: discountCouponTotal,
                    rewardPointAmount: rewardPointAmount,
                    voucherAmount: voucherAmount,
                    serviceFee: serviceFee);
                if (clearSession)
                    CartHelper.SetOrderSession(siteSettings.SiteId, null);
                string validateInventoriesHtml = string.Empty;
                bool isValidInventory = true;
                if (ProductConfiguration.ValidateInventoryEnable)
                {
                    Store store = null;
                    var validateInventories = StoreHelper.ValidateInventoryOrder(order, cartList, lstProductsInCart, ref store);

                    if (validateInventories == null || validateInventories.Any(it => !it.IsValid))
                    {
                        isValidInventory = false;
                        validateInventoriesHtml = BuildValidateInventories(validateInventories, store);
                    }
                }
                return StringHelper.ToJsonString(new
                {
                    success = true,
                    subTotal = ProductHelper.FormatPrice(subTotal, true),
                    shippingTotal = ProductHelper.FormatPrice(shippingPrice, true),
                    discountTotal = ProductHelper.FormatPrice(-discountTotal, true),
                    rewardPointTotal = ProductHelper.FormatPrice(-rewardPointAmount, true),
                    discountPaymentTotal = ProductHelper.FormatPrice(discountPayment, true),
                    discountPaymentValue = ProductHelper.FormatPrice(discountPayment),
                    discountPaymentMessage = string.Format(ProductResources.PaymentDiscountMessage, ProductHelper.FormatPrice(discountPayment)),
                    voucherAmount = ProductHelper.FormatPrice(-voucherAmount, true),
                    total = ProductHelper.FormatPrice(total, true),
                    isRestricted,
                    shippingMethods,
                    paymentMethods,
                    shippingMessage = ProductResources.ShippingFeeContactText,
                    expectedTime,
                    isValidInventory,
                    validateInventoriesHtml
                });
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return StringHelper.ToJsonString(new
            {
                success = false
            });
        }

        private string BuildValidateInventories(List<ValidateInventoryModel> models, Store store)
        {
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<Inventories></Inventories>");
            var root = doc.DocumentElement;

            if (store != null)
            {
                XmlHelper.AddNode(doc, root, "StoreName", store.Name);
                XmlHelper.AddNode(doc, root, "Address", store.Address);
            }
            if (models != null)
                foreach (var item in models)
                {
                    var productXml = doc.CreateElement("Inventory");
                    root.AppendChild(productXml);

                    XmlHelper.AddNode(doc, productXml, "Title", item.ProductTitle);
                    XmlHelper.AddNode(doc, productXml, "CartQuantity", item.CartQuantity.ToString());
                    XmlHelper.AddNode(doc, productXml, "StockQuantityAvailability", item.StockQuantityAvailability.ToString());
                    XmlHelper.AddNode(doc, productXml, "IsValid", item.IsValid.ToString());
                    XmlHelper.AddNode(doc, productXml, "ProductId", item.ProductId.ToString());
                    XmlHelper.AddNode(doc, productXml, "ProductGuid", item.ProductGuid.ToString());
                    XmlHelper.AddNode(doc, productXml, "CartItemGuid", item.CartItemGuid.ToString());
                    XmlHelper.AddNode(doc, productXml, "ProductGuid", item.ProductGuid.ToString());
                }
            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", "ValidateInventories.xslt"), doc);
            return result;
        }

        private string SpendingPoint()
        {
            var cart = CartHelper.GetShoppingCart(siteSettings.SiteId, ShoppingCartTypeEnum.ShoppingCart);
            if (cart.Count == 0)
            {
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = ProductResources.CartIsEmptyLabel
                });
            }

            var siteUser = SiteUtils.GetCurrentSiteUser();
            if (siteUser == null || siteUser.UserId <= 0)
            {
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "User không tồn tại."
                });
            }

            int point = 0;
            if (postParams.Get("point") != null)
                int.TryParse(postParams.Get("point"), out point);

            RewardPointsHelper.GetRewardPointsBalance(siteUser, out decimal pointBalance, out decimal pointsAvallable);
            if (point < 0 || point > pointsAvallable)
                return StringHelper.ToJsonString(new
                {
                    success = false,
                    message = "Điểm sử dụng không hợp lệ."
                });

            var lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId,
                CartHelper.GetCartSessionGuid(siteSettings.SiteId),
                ShoppingCartTypeEnum.ShoppingCart);
            var subTotal = cart.GetSubTotal(lstProductsInCart);

            //var couponIsAvailable = false;
            var coupon = (DiscountCoupon)null;
            var lstDiscountItems = (List<DiscountAppliedToItem>)null;
            var lstDiscountId = (List<int>)null;
            var giftDescription = string.Empty;
            var productGifts = string.Empty;
            var discountPayment = 0M;
            var discountTotal = cart.GetDiscountTotal(lstProductsInCart,
                ref lstDiscountItems, ref lstDiscountId,
                ref giftDescription, ref productGifts, ref coupon, ref discountPayment);
            var discountCouponTotal = coupon != null ? coupon.DiscountAmount : 0;

            if (ProductConfiguration.EnableSaveDiscountPayment && discountPayment > 0)
                discountTotal += discountPayment;

            var total = CartHelper.GetCartTotal(subTotal, discountTotal: discountTotal, discountCouponTotal: discountCouponTotal);

            return StringHelper.ToJsonString(new
            {
                success = true,
                discountTotal = ProductHelper.FormatPrice(discountTotal, true),
                total = ProductHelper.FormatPrice(total, true),
                rewardPointAmount = RewardPointsHelper.CalculateRewardPoints(siteUser, total)
            });
        }

        private string GetServiceList()
        {
            string strProvinceGuid = GetPostValue("provinceGuid");
            string strDistrictGuid = GetPostValue("districtGuid");
            string strShippingMethodId = GetPostValue("shippingMethodId");
            if (!int.TryParse(strShippingMethodId, out int shippingId))
                return StringHelper.ToJsonString(new
                {
                    success = false
                });
            var shipping = new ShippingMethod(shippingId);
            if (shipping == null || shipping.ShippingMethodId == -1)
                return StringHelper.ToJsonString(new
                {
                    success = false
                });

            var cartType = ShoppingCartTypeEnum.ShoppingCart;
            var order = CartHelper.GetOrderSession(siteSettings.SiteId);
            if (order == null)
                order = new Order();
            var lstProductsInCart = new List<Product>();
            var cartList = CartHelper.GetShoppingCart(siteSettings.SiteId, cartType);
            if (cartList.Count > 0) lstProductsInCart = Product.GetByShoppingCart(siteSettings.SiteId, CartHelper.GetCartSessionGuid(siteSettings.SiteId), cartType);
            var orderSubTotal = cartList.GetSubTotal(lstProductsInCart);
            var orderWeight = cartList.GetTotalWeights(lstProductsInCart);
            var productTotalQty = cartList.GetTotalProducts();
            var geoZoneGuids = ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order);
            if (!string.IsNullOrEmpty(strProvinceGuid))
                geoZoneGuids = strProvinceGuid;

            if (!string.IsNullOrEmpty(strDistrictGuid) && Guid.TryParse(strDistrictGuid, out Guid districtGuid))
                order.BillingDistrictGuid = districtGuid;
            string errorMessage = string.Empty;
            if (shipping.ShippingProvider == (int)ShippingMethodProvider.VittelPost)
            {
                #region Store

                Store store = null;
                if (StoreHelper.EnabledStore)
                {
                    int.TryParse(GetPostValue("Store_ID", order.StoreId.ToString()), out int storeID);
                    order.StoreId = storeID;
                    store = StoreHelper.SettoreForOrder(order, cartList);
                }

                #endregion Store

                var listServiceVT = ShippingHelper.GetShippingOptions(shipping.ShippingMethodId,
                    orderWeight,
                    order.BillingDistrictGuid,
                    orderSubTotal,
                    productTotalQty,
                    store,
                    ref errorMessage);
                StringBuilder str = new StringBuilder();
                foreach (var item in listServiceVT)
                    str.Append($"<option value='{item.Value}'>{item.Name}</option>");
                return StringHelper.ToJsonString(new
                {
                    success = true,
                    data = str.ToString()
                });
            }
            return StringHelper.ToJsonString(new
            {
                success = false
            });
        }
    }
}
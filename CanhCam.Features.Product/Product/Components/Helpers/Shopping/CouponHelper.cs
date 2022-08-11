using CanhCam.Business;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public static class CouponHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CouponHelper));
        public static string PROMOTION_WARNING_SHOPPINGCART = "PromotionWarning";

        #region Coupon

        public static string CouponCodeInput
        {
            get
            {
                if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session["CouponCodeInput"] != null))
                    return HttpContext.Current.Session["CouponCodeInput"].ToString();
                return string.Empty;
            }
            set
            {
                if ((HttpContext.Current != null) && (HttpContext.Current.Session != null))
                {
                    if (value == null && HttpContext.Current.Session["CouponCodeInput"] != null)
                    {
                        HttpContext.Current.Session.Remove("CouponCodeInput");
                        return;
                    }

                    HttpContext.Current.Session["CouponCodeInput"] = value;
                }
            }
        }

        //public static List<CouponAppliedToItem> GetAppliedItems(int siteId, List<ShoppingCartItem> cartList,
        //        List<Product> lstProducts, out Coupon coupon)
        //{
        //    coupon = null;
        //    List<CouponAppliedToItem> lstAppliedItems = new List<CouponAppliedToItem>();
        //    if (CouponHelper.CouponCodeInput.Length > 0)
        //    {
        //        var warning = string.Empty;
        //        coupon = CouponIsValid(siteId, CouponHelper.CouponCodeInput, out warning);
        //        if (coupon != null)
        //        {
        //            var lstItems = CouponAppliedToItem.GetByCoupon(coupon.CouponId, -1);

        //            foreach (ShoppingCartItem sci in cartList)
        //            {
        //                var product = ProductHelper.GetProductFromList(lstProducts, sci.ProductId);
        //                if (product != null)
        //                {
        //                    var item = CouponAppliedToItem.FindFromList(lstItems, product.ZoneId, (int)CouponAppliedType.ToCategories);
        //                    if (item == null)
        //                        item = CouponAppliedToItem.FindFromList(lstItems, sci.ProductId, (int)CouponAppliedType.ToProducts);

        //                    if (item != null)
        //                        lstAppliedItems.Add(item);
        //                }
        //            }
        //        }
        //    }

        //    return lstAppliedItems;
        //}

        //public static Coupon CouponIsValid(int siteId, string couponCode, out string warning)
        //{
        //    warning = string.Empty;
        //    var coupon = Coupon.GetOneByCode(siteId, couponCode);
        //    if (coupon == null || coupon.CouponId == -1 || !coupon.IsActive)
        //    {
        //        warning = ProductResources.CouponCodeNotExistsWarning;
        //        return null;
        //    }

        //    if (coupon.FromDate != null && DateTime.Now < coupon.FromDate)
        //    {
        //        warning = ProductResources.CouponCodeNotExistsWarning;
        //        return null;
        //    }

        //    if (
        //        (coupon.ExpiryDate != null && DateTime.Now > coupon.ExpiryDate)
        //        || (coupon.LimitationTimes > 0 && coupon.NumOfUses >= coupon.LimitationTimes)
        //    )
        //    {
        //        warning = ProductResources.CouponCodeHasExpiredWarning;
        //        return null;
        //    }

        //    return coupon;
        //}

        public static DiscountCoupon CouponIsValid(string couponCode, out string warning)
        {
            warning = string.Empty;
            var coupon = DiscountCoupon.GetByCode(couponCode);
            if (coupon == null || coupon.Guid == Guid.Empty)
            {
                warning = ProductResources.CouponCodeNotExistsWarning;
                return null;
            }
            if (ERPHelper.EnableERP && ERPHelper.EnableValidCouponInERP && !ERPHelper.CouponIsValid(couponCode, out warning))
                return null;

            if (coupon.LimitationTimes > 0 && coupon.UseCount >= coupon.LimitationTimes)
            {
                warning = "Mã khuyến mãi đã được sử dụng";
                return null;
            }

            var discount = new Discount(coupon.DiscountID);
            if (discount == null || discount.DiscountId <= 0 || !discount.IsActive)
            {
                warning = ProductResources.CouponCodeNotExistsWarning;
                return null;
            }

            if (discount.StartDate.HasValue && DateTime.Now < discount.StartDate.Value)
            {
                warning = ProductResources.CouponCodeNotExistsWarning;
                return null;
            }

            if (
                (discount.EndDate.HasValue && DateTime.Now > discount.EndDate.Value)
                || (coupon.LimitationTimes > 0 && coupon.UseCount >= coupon.LimitationTimes)
            )
            {
                warning = ProductResources.CouponCodeHasExpiredWarning;
                return null;
            }

            coupon.Discount = discount;

            return coupon;
        }

        public static int GetTotalProductsAppliedCoupon(List<ShoppingCartItem> cartList, int productId)
        {
            int totalQuantity = 0;
            foreach (ShoppingCartItem sci in cartList)
            {
                if (sci.ProductId == productId && sci.AppliedCoupon)
                    totalQuantity += sci.Quantity;
            }

            return totalQuantity;
        }

        #endregion Coupon
    }
}
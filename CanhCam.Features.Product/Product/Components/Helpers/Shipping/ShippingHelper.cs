using CanhCam.Business;
using CanhCam.Web.Framework;
using System;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public static class ShippingHelper
    {
        public static bool EnableFastShipping = ConfigHelper.GetBoolProperty("Shipping:EnableFastShipping", false);
        public static int FastShipMethodId = ConfigHelper.GetIntProperty("Shipping:FastShipMethodId", -1);
        public static bool EnableRestrictedShipping = ConfigHelper.GetBoolProperty("Shipping:EnableRestrictedShipping", false);
        public static bool RestrictedShippingForStore = ConfigHelper.GetBoolProperty("Shipping:RestrictedShippingForStore", false);

        public static bool IsShippingServiceProvider(int shippingMethodId)
        {
            if (ViettelPostHelper.ShippingMethodId == shippingMethodId)
                return true;
            if (DHLHelper.ShippingMethodId == shippingMethodId)
                return true;
            return false;
        }

        public static List<ShippingOption> GetShippingOptions(
            int shippingMethodId,
            decimal totalWeight,
            Guid BillingDistrictGuid,
            decimal amout,
            int totalQuantities,
            Store store,
            ref string error)
        {
            if (ViettelPostHelper.ShippingMethodId == shippingMethodId)
                return ViettelPostHelper.GetSeriveList(totalWeight, BillingDistrictGuid, amout, totalQuantities, store, ref error);

            return new List<ShippingOption>();
        }

        public static bool SendShippingment(Order order, List<OrderItem> orderitems, List<Product> products, ref string shipmentCode, ref string error)
        {
            if (ViettelPostHelper.ShippingMethodId == order.ShippingMethod)
            {
                var response = ViettelPostHelper.MakeOrderForm(order, StoreCacheHelper.GetStoreById(order.StoreId), orderitems, products, order.ShippingOption, ref error);
                if (response.status == 200 && response.data != null)
                {
                    shipmentCode = response.data.ORDER_NUMBER;
                    return true;
                }
                else
                    error = response.message;
            }
            if (DHLHelper.ShippingMethodId == order.ShippingMethod)
            {
                try
                {
                    var response = DHLHelper.SendOrder(order, orderitems, products);
                    if (response.bd.responseStatus.code == "200")
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        public static bool IsRestrictedArea(Order order, decimal orderWeight, ref List<int> shippingMethods, ref List<int> paymentMethods)
        {
            if (!ShippingHelper.EnableRestrictedShipping)
                return false;
            int storeId = order.StoreId;
            if (!RestrictedShippingForStore)
                storeId = -1;
            string geoZoneGuids = order.BillingProvinceGuid.ToString();
            if (order.BillingDistrictGuid != Guid.Empty)
                geoZoneGuids += ";" + order.BillingDistrictGuid;

            RestrictedShipping restrictedShipping = new RestrictedShipping(geoZoneGuids, storeId, orderWeight);
            if (restrictedShipping != null && restrictedShipping.RowId > 0)
            {
                if (!string.IsNullOrEmpty(restrictedShipping.ShippingMethodIds))
                    foreach (var item in restrictedShipping.ShippingMethodIds.Split(';'))
                        if (int.TryParse(item, out int methodId))
                            shippingMethods.Add(methodId);

                if (!string.IsNullOrEmpty(restrictedShipping.PaymentMethodIds))
                    foreach (var item in restrictedShipping.PaymentMethodIds.Split(';'))
                        if (int.TryParse(item, out int methodId))
                            paymentMethods.Add(methodId);
                return true;
            }
            return false;
        }
    }
}
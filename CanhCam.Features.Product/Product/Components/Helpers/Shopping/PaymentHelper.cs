using CanhCam.Business;
using log4net;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public static class PaymentHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PaymentHelper));

        public static string GetOnlinePaymentRedirectUrl(Order order, List<ShoppingCartItem> cartItems)
        {
            if (!IsOnlinePayment(order.PaymentMethod))
                return string.Empty;
            string results = string.Empty;
            if (order.PaymentMethod == OnePayHelper.PaymentMethodId)
                OnePayHelper.GetPaymentUrlIfNeeded(order);

            if (order.PaymentMethod == MomoHelper.MoMoPaymentMethodId)
                results = MomoHelper.GetPaymentUrlIfNeeded(order, cartItems);

            if (order.PaymentMethod == VNPAYHelper.PaymentID)
                results = VNPAYHelper.BuildUrlPay(order);

            if (order.PaymentMethod == AlepayHelper.AlepayPaymentMethod)
                results = AlepayHelper.SendPaymentAlepay(order);

            if (order.PaymentMethod == PayooHelper.PaymentMethodId)
                results = PayooHelper.GetPaymentUrlIfNeeded(order);

            if (!string.IsNullOrEmpty(results))
                return results;
            return string.Empty;
        }

        public static bool IsOnlinePayment(int paymentMethodId)
        {
            return (paymentMethodId == OnePayHelper.PaymentMethodId
                || paymentMethodId == MomoHelper.MoMoPaymentMethodId
                || paymentMethodId == AlepayHelper.AlepayPaymentMethod
                || paymentMethodId == AlepayHelper.NganLuongIBPaymentMethod
                || paymentMethodId == AlepayHelper.NganLuongATMPaymentMethod
                || paymentMethodId == PayooHelper.PaymentMethodId
                || paymentMethodId == VNPAYHelper.PaymentID);
        }

        public static string GetPaymentOnlinePendingText(int paymentMethodId)
        {
            if (paymentMethodId == OnePayHelper.PaymentMethodId)
                return "Chờ OnePay xác thực";
            else if (paymentMethodId == MomoHelper.MoMoPaymentMethodId)
                return "Chờ MoMo xác thực";
            else if (paymentMethodId == AlepayHelper.AlepayPaymentMethod)
                return "Chờ Alepay xác thực";
            else if (paymentMethodId == AlepayHelper.NganLuongIBPaymentMethod)
                return "Chờ Alepay xác thực";
            else if (paymentMethodId == AlepayHelper.NganLuongATMPaymentMethod)
                return "Chờ Alepay xác thực";
            else if (paymentMethodId == PayooHelper.PaymentMethodId)
                return "Chờ  Payoo xác thực";
            else if (paymentMethodId == VNPAYHelper.PaymentID)
                return "Chờ VNPAY xác thực";
            return string.Empty;
        }

        public static int GetPaymentStatusOnline(Order order, ref string result)
        {
            if (order.PaymentMethod == OnePayHelper.PaymentMethodId)
                return OnePayHelper.GetPaymentStatus(order, ref result);
            else if (order.PaymentMethod == MomoHelper.MoMoPaymentMethodId) return -1;
            else if (order.PaymentMethod == AlepayHelper.AlepayPaymentMethod) return -1;
            else if (order.PaymentMethod == AlepayHelper.NganLuongIBPaymentMethod) return -1;
            else if (order.PaymentMethod == AlepayHelper.NganLuongATMPaymentMethod) return -1;
            else if (order.PaymentMethod == PayooHelper.PaymentMethodId) return -1;
            else if (order.PaymentMethod == VNPAYHelper.PaymentID) return -1;
            return -1;
        }
    }
}
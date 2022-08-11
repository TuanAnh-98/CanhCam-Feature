using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;

namespace CanhCam.Web.ProductUI
{
    public static class VoucherHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(VoucherHelper));
        public static bool Enable = ConfigHelper.GetBoolProperty("Shopping:VoucherEnable", false);
        public static string VoucherCodeInputKey = "VoucherCodeInput";
        public static int MaximumVoucherApplyInCart = ConfigHelper.GetIntProperty("Shopping:MaximumVoucherApplyInCart", 5);
        public static bool AutoReplaceWhenMaximumVoucher = ConfigHelper.GetBoolProperty("Shopping:AutoReplaceWhenMaximumVoucher", false);

        #region Voucher

        public static List<string> GetVoucherCodeApplied()
        {
            var codes = new List<string>();

            if ((HttpContext.Current.Session != null) && (HttpContext.Current.Session[VoucherCodeInputKey] != null))
                codes = (List<string>)HttpContext.Current.Session[VoucherCodeInputKey];

            return codes;
        }

        public static void SetVoucherCodeApply(string code)
        {
            if (string.IsNullOrEmpty(code))
                return;

            var codes = GetVoucherCodeApplied();

            if (codes.Count >= MaximumVoucherApplyInCart && !AutoReplaceWhenMaximumVoucher)
                return;

            if (codes.Contains(code))
                return;

            codes.Add(code);

            if (codes.Count > MaximumVoucherApplyInCart && AutoReplaceWhenMaximumVoucher)//ex: 1, 2, 3, 4,5 (max = 4)
                codes.RemoveAt(0); //remove 1

            HttpContext.Current.Session[VoucherCodeInputKey] = codes;
        }

        public static void ClearVoucherCodeApply()
        {
            HttpContext.Current.Session[VoucherCodeInputKey] = null;
        }

        public static void SetVoucherCodeApply(List<string> codes)
        {
            if (codes == null || codes.Count == 0)
            {
                HttpContext.Current.Session[VoucherCodeInputKey] = null;
                return;
            }

            if (codes.Count <= MaximumVoucherApplyInCart)
            {
                HttpContext.Current.Session[VoucherCodeInputKey] = codes;
                return;
            }

            if (codes.Count > MaximumVoucherApplyInCart && AutoReplaceWhenMaximumVoucher)//ex: 1, 2, 3, 4,5 (max = 4)
            {
                List<string> tmp = new List<string>();
                for (int i = 0; i < MaximumVoucherApplyInCart; i++)
                    tmp.Add(codes[i]);
                HttpContext.Current.Session[VoucherCodeInputKey] = tmp;
            }
        }

        public static bool VoucherIsValid(string voucherCode, out string warning)
        {
            warning = string.Empty;

            var codes = GetVoucherCodeApplied();

            if (codes.Count >= MaximumVoucherApplyInCart && !AutoReplaceWhenMaximumVoucher)
            {
                warning = string.Format(ProductResources.VoucherMaximumVoucherApplyInCartFormat, MaximumVoucherApplyInCart);
                return false;
            }

            if (codes.Contains(voucherCode))
            {
                warning = ProductResources.VoucherCodeExistsWarning;
                return false;
            }

            var voucher = new Voucher(voucherCode);
            if (voucher == null || voucher.ItemGuid == Guid.Empty)
            {
                warning = ProductResources.VoucherCodeNotExistsWarning;
                return false;
            }

            if (ERPHelper.EnableERP && ERPHelper.EnableValidVoucherInERP && !ERPHelper.VoucherIsValid(voucherCode, out warning))
                return false;
            if (DateTime.Now < voucher.StartDate)
            {
                warning = ProductResources.VoucherCodeNotExistsWarning;
                return false;
            }

            if (DateTime.Now > voucher.EndDate)
            {
                warning = ProductResources.VoucherCodeHasExpiredWarning;
                return false;
            }

            if (voucher.LimitationTimes > 0 && voucher.UseCount >= voucher.LimitationTimes)
            {
                warning = ProductResources.VoucherCodeUsedWarning;
                return false;
            }

            return true;
        }

        #endregion Voucher

        public static decimal GetVoucherAmountTotal()
        {
            var codes = GetVoucherCodeApplied();
            if (codes.Count == 0) return 0;
            decimal amount = 0;
            foreach (var code in codes)
            {
                Voucher voucher = new Voucher(code);
                if (voucher == null || voucher.ItemGuid == Guid.Empty)
                    continue;

                if (DateTime.Now < voucher.StartDate)
                    continue;

                if (DateTime.Now > voucher.EndDate)
                    continue;

                if ((voucher.LimitationTimes > 0 && voucher.UseCount >= voucher.LimitationTimes))
                    continue;

                amount += voucher.Amount;
            }
            return amount;
        }
    }
}
using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class ShoppingCartConfiguration
    {
        public ShoppingCartConfiguration()
        { }

        public ShoppingCartConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            //if (settings["CheckoutPageUrl"] != null)
            //    nextPageUrl = settings["CheckoutPageUrl"].ToString();

            checkoutZoneId = WebUtils.ParseInt32FromHashtable(settings, "CheckoutPageUrl", checkoutZoneId);
            isWishlist = WebUtils.ParseBoolFromHashtable(settings, "IsWishlistSetting", isWishlist);
        }

        private bool isWishlist = false;

        public bool IsWishlist => isWishlist;

        private int checkoutZoneId = -1;

        public int CheckoutZoneId => checkoutZoneId;
    }
}
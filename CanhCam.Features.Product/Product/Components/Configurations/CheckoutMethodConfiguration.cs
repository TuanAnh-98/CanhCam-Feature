using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class CheckoutMethodConfiguration
    {
        public CheckoutMethodConfiguration()
        { }

        public CheckoutMethodConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            checkoutNextZoneId = WebUtils.ParseInt32FromHashtable(settings, "CheckoutNextPageUrl", checkoutNextZoneId);
        }

        private int checkoutNextZoneId = -1;

        public int CheckoutNextZoneId => checkoutNextZoneId;
    }
}
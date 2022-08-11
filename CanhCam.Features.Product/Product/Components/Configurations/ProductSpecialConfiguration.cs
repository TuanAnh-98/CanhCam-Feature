using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class ProductSpecialConfiguration
    {
        public ProductSpecialConfiguration()
        { }

        public ProductSpecialConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            maxProductsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxProductsToGetSetting", maxProductsToGet);
            position = WebUtils.ParseInt32FromHashtable(settings, "ProductPositionSetting", position);
            zoneId = WebUtils.ParseInt32FromHashtable(settings, "ParentZoneSetting", zoneId);
        }

        private int position = -1;

        public int Position => position;

        private int maxProductsToGet = 5;

        public int MaxProductsToGet => maxProductsToGet;

        private int zoneId = -1;

        public int ZoneId => zoneId;
    }
}
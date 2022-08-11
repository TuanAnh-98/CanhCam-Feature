using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class ZoneProductConfiguration
    {
        public ZoneProductConfiguration()
        { }

        public ZoneProductConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            isSubZone = WebUtils.ParseBoolFromHashtable(settings, "IsSubZoneSetting", isSubZone);
            maxItemsToGet = WebUtils.ParseInt32FromHashtable(settings, "MaxProductsToGetSetting", maxItemsToGet);
            productPosition = WebUtils.ParseInt32FromHashtable(settings, "ProductPositionSetting", productPosition);
            showAllProducts = WebUtils.ParseBoolFromHashtable(settings, "ShowAllProductsFromChildZoneSetting", showAllProducts);
            zonePosition = WebUtils.ParseInt32FromHashtable(settings, "ZonePositionSetting", zonePosition);
        }

        private int maxItemsToGet = 10;

        public int MaxItemsToGet => maxItemsToGet;

        private int productPosition = -1;

        public int ProductPosition => productPosition;

        private int zonePosition = -1;

        public int ZonePosition => zonePosition;

        private bool isSubZone = false;

        public bool IsSubZone => isSubZone;

        private bool showAllProducts = false;

        public bool ShowAllProducts => showAllProducts;

        private bool showAllImagesInNewsList = false;

        public bool ShowAllImagesInNewsList => showAllImagesInNewsList;
    }
}
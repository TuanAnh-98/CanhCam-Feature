using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class PromotionDetailConfiguration
    {
        public PromotionDetailConfiguration()
        { }

        public PromotionDetailConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            loadZones = WebUtils.ParseBoolFromHashtable(settings, "PromotionLoadZonesSetting", loadZones);
            loadProducts = WebUtils.ParseBoolFromHashtable(settings, "PromotionLoadProductsSetting", loadProducts);
            pageSize = WebUtils.ParseInt32FromHashtable(settings, "OtherPromotionsPerPageSetting", pageSize);
            loadZones = WebUtils.ParseBoolFromHashtable(settings, "PromotionLoadZonesSetting", loadZones);
        }

        private bool loadZones = false;

        public bool LoadZones => loadZones;

        private bool loadProducts = false;

        public bool LoadProducts => loadProducts;

        private int pageSize = 0;

        public int PageSize => pageSize;
    }
}
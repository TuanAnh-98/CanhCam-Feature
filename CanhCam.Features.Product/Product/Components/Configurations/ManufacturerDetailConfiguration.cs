using CanhCam.Web.Framework;
using System;
using System.Collections;
namespace CanhCam.Web.ProductUI
{
    public class ManufacturerDetailConfiguration
    {
        public ManufacturerDetailConfiguration()
        { }

        public ManufacturerDetailConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            loadZones = WebUtils.ParseBoolFromHashtable(settings, "ManufacturerLoadZonesSetting", loadZones);
            loadProducts = WebUtils.ParseBoolFromHashtable(settings, "ManufacturerLoadProductsSetting", loadProducts);
            pageSize = WebUtils.ParseInt32FromHashtable(settings, "ManufacturerItemsPerPageSetting", pageSize);
        }

        private bool loadZones = false;

        public bool LoadZones => loadZones;

        private bool loadProducts = false;

        public bool LoadProducts => loadProducts;

        private int pageSize = 0;

        public int PageSize => pageSize;
    }
}
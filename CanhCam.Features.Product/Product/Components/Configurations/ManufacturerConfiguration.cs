using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class ManufacturerConfiguration
    {
        public ManufacturerConfiguration()
        { }

        public ManufacturerConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            //showOptions = WebUtils.ParseInt32FromHashtable(settings, "ManufacturerShowOptionsSetting", showOptions);
            pageSize = WebUtils.ParseInt32FromHashtable(settings, "ManufacturerItemsPerPageSetting", pageSize);
            showPager = WebUtils.ParseBoolFromHashtable(settings, "ManufacturerShowPagerSetting", showPager);
            loadByZone = WebUtils.ParseBoolFromHashtable(settings, "ManufacturerLoadByZoneSetting", loadByZone);
        }

        private int pageSize = 0;

        public int PageSize => pageSize;

        private bool loadByZone = false;

        public bool LoadByZone => loadByZone;

        private bool showPager = false;

        public bool ShowPager => showPager;
    }
}
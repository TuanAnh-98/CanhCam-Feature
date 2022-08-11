using CanhCam.Web.Framework;
using System;
using System.Collections;

namespace CanhCam.Web.ProductUI
{
    public class DiscountConfiguration
    {
        public DiscountConfiguration()
        { }

        public DiscountConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            pageSize = WebUtils.ParseInt32FromHashtable(settings, "ItemsPerPageSetting", pageSize);
            loadType = WebUtils.ParseInt32FromHashtable(settings, "PromotionLoadTypeSetting", loadType);
        }

        private int pageSize = 10;

        public int PageSize => pageSize;

        private int loadType = 0;

        public int LoadType => loadType;
    }
}
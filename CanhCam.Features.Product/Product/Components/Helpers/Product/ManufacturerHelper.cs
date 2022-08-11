using CanhCam.Web.Framework;
using log4net;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public static class ManufacturerHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ManufacturerHelper));

        public static string FormatManufacturerUrl(string url, int manufacturerId)
        {
            if (url.Length > 0)
            {
                if (url.StartsWith("http"))
                {
                    string siteRoot = WebUtils.GetSiteRoot();
                    return url.Replace("http://~", siteRoot).Replace("https://~", siteRoot.Replace("http:", "https"));
                }

                return SiteUtils.GetNavigationSiteRoot() + url.Replace("~", string.Empty);
            }

            return SiteUtils.GetNavigationSiteRoot() + "/Product/Manufacturer.aspx?manufacturerid=" + manufacturerId.ToInvariantString();
        }

        /// <summary>
        /// Todo: Implement?
        /// </summary>
        public static bool EnableManufacturerFilterMultipleValues => true;

        public static List<int> ParseManufacturerFromQueryString()
        {
            var lstInt = new List<int>();
            var manufacturerValues = WebUtils.ParseStringFromQueryString(ProductHelper.QueryStringManufacturerParam, string.Empty);
            if (!string.IsNullOrEmpty(manufacturerValues))
            {
                var lstManufacturerValues = manufacturerValues.SplitOnCharAndTrim('/');
                foreach (var m in lstManufacturerValues)
                {
                    var mId = -1;
                    if (int.TryParse(m, out mId))
                    {
                        if (!lstInt.Contains(mId))
                            lstInt.Add(mId);
                    }
                }
            }

            return lstInt;
        }
    }
}
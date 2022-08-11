/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23
/// 2015-06-04: added QueryStringKeywordParam
/// 2015-10-23: Enable shopping cart with manufacturer attributes

using CanhCam.Web.Framework;
using log4net;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public static class GoogleTrackingHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GoogleTrackingHelper));
        public static bool Enable = ConfigHelper.GetBoolProperty("GoogleTracking:Enable", false);
        public static bool EnableProductList = ConfigHelper.GetBoolProperty("GoogleTracking:EnableProductList", false);
        public static bool EnableProductSpecial = ConfigHelper.GetBoolProperty("GoogleTracking:EnableProductSpecial", false);
        public static bool EnableProductZone = ConfigHelper.GetBoolProperty("GoogleTracking:EnableProductZone", false);
        public static bool EnableProductDetail = ConfigHelper.GetBoolProperty("GoogleTracking:EnableProductDetail", false);
        public static bool EnableCheckoutPage = ConfigHelper.GetBoolProperty("GoogleTracking:EnableCheckoutPage", false);
        public static int CheckoutPageZoneId = ConfigHelper.GetIntProperty("GoogleTracking:CheckoutPageZoneId", -1);
        public static bool EnableCompletedPage = ConfigHelper.GetBoolProperty("GoogleTracking:EnableCompletedPage", false);

        //Product list
        public static string BuildGoogleImpressions(XmlDocument doc, string fileXsltName = "Impressions.xslt")
        {
            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Tracking", fileXsltName), doc);
            return result;
        }

        //Product Detail
        public static string BuildGoogleDetail(XmlDocument doc)
        {
            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Tracking", "Detail.xslt"), doc);
            return result;
        }

        public static string BuildGoogleCheckout(XmlDocument doc)
        {
            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Tracking", "Checkout.xslt"), doc);
            return result;
        }

        public static string BuildGoogleCompleted(XmlDocument doc)
        {
            var result = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("Tracking", "Completed.xslt"), doc);
            return result;
        }
    }
}
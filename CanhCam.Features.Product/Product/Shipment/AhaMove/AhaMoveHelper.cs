using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using log4net;
using Newtonsoft.Json;
using RestSharp;

namespace CanhCam.Web.ProductUI
{
    public class AhaMoveHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AhaMoveHelper));

        public static readonly string AccountPhoneNumberKey = "AhaMove-AccountPhoneNumberKey";
        public static readonly string AccountNameKey = "AhaMove-AccountNameKey";
        public static readonly string AccountAPITokenKey = "AhaMove-AccountAPITokenKey";

        public static readonly string TokenKey = "AhaMove-TokenKey";

        public static string RegisterAccount(ref string message)
        {
            message = string.Empty;
            string result = string.Empty;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) return result;
            var client = new RestClient("https://apistg.ahamove.com/v1/partner/register_account?mobile=0968098865&name=Ahamove User&api_key=b5a780c740647d25ff6832a86ab56fbb4bff6eeb");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            if (!string.IsNullOrEmpty(response.Content))
            {
                var json = JsonConvert.DeserializeObject<JsonObject>(response.Content);
                if (json["token"] != null)
                    result = json["token"].ToString();
                if (json["title"] != null)
                    message = json["title"].ToString();
            }
            return result;
        }
    }
}
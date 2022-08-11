using CanhCam.Web.Framework;
using log4net;
using System;
using System.IO;
using System.Net;

namespace CanhCam.Web.ProductUI
{
    public class NinjavanHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NinjavanHelper));
        private static string _urlService = string.Empty;
        private static string _client_id = string.Empty;
        private static string _client_secret = string.Empty;
        private static string _sender_District = string.Empty;
        private static string _sender_Province = string.Empty;
        private static string _sender_WARD = string.Empty;

        private static bool CheckValidApi()
        {
            _urlService = ConfigHelper.GetStringProperty("Ninjavan:UrlService", _urlService);
            _client_id = ConfigHelper.GetStringProperty("Ninjavan:ClientId", _client_id);
            _client_secret = ConfigHelper.GetStringProperty("Ninjavan:ClientSecret", _client_secret);
            _sender_District = ConfigHelper.GetStringProperty("Ninjavan:SenderDistrict", _sender_District);
            _sender_Province = ConfigHelper.GetStringProperty("Ninjavan:SenderProvince", _sender_Province);
            _sender_WARD = ConfigHelper.GetStringProperty("Ninjavan:SenderWARD", _sender_WARD);

            if (_urlService.Length == 0
                || _client_id.Length == 0
                || _client_secret.Length == 0
                || _sender_District.Length == 0
                || _sender_Province.Length == 0
                || _sender_WARD.Length == 0
                )
                return false;
            return true;
        }

        public static string GetOAuthAccessToken()
        {
            if (!CheckValidApi())
            {
                return string.Empty;
            }
            var json = StringHelper.ToJsonString(new
            {
                client_id = _client_id,
                client_secret = _client_secret,
                grant_type = "client_credentials"
            });

            var response = GetResponseData("order/UpdateOrder", json);
            return string.Empty;
        }

        private static string GetResponseData(string functionName, string postJsonValue, string meothd = "POST", bool isGetToken = false)
        {
            try
            {
                var url = string.Format("{0}/{1}", _urlService, functionName);
                if (isGetToken)
                    url = functionName;
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = meothd;
                if (!isGetToken)
                    httpWebRequest.Headers.Add("Token", GetOAuthAccessToken());
                if (meothd == "POST")
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(postJsonValue);
                        streamWriter.Flush();
                    }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    string result = streamReader.ReadToEnd();
                    return result;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return string.Empty;
        }
    }
}
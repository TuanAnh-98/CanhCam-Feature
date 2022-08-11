﻿/// Author:                 Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-09-02
/// Last Modified:		    2014-09-02

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Services;

namespace CanhCam.Web.ProductUI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class SearchCountResult : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            string query = WebUtils.ParseStringFromQueryString("q", string.Empty);
            if (query.Length == 0)
                return;

            if (WebConfigSettings.EscapingSpecialCharactersInKeyword)
            {
                query = query.Replace("-", "\\-");
            }

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            int pageSize = 1;
            bool isSiteEditor = WebUser.IsAdminOrContentAdmin || (SiteUtils.UserIsSiteEditor());

            CanhCam.SearchIndex.IndexHelper.Search(
                siteSettings.SiteId,
                isSiteEditor,
                GetUserRoles(context, siteSettings.SiteId),
                Product.FeatureGuid,
                WorkingCulture.DefaultName,
                DateTime.MinValue,
                DateTime.MaxValue,
                query,
                false,
                WebConfigSettings.SearchResultsFragmentSize,
                1,
                pageSize,
                WebConfigSettings.SearchMaxClauseCount,
                out int totalHits,
                out bool queryErrorOccurred);

            context.Response.Write(totalHits.ToString());

            context.Response.End();
        }

        private List<string> GetUserRoles(HttpContext context, int siteId)
        {
            List<string> userRoles = new List<string>();

            userRoles.Add("All Users");
            if (context.User.Identity.IsAuthenticated)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    using (IDataReader reader = SiteUser.GetRolesByUser(siteId, currentUser.UserId))
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(reader["RoleName"].ToString());
                        }
                    }
                }
            }

            return userRoles;
        }

        public bool IsReusable => false;
    }
}
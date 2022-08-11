/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-08-02

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductEditPage : CmsNonBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static AutoCompleteBoxData GetProductTags(object context)
        {
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            List<Tag> lstTags = Tag.GetPage(siteSettings.SiteGuid, Product.FeatureGuid, searchString, -1, 1, 10);
            List<AutoCompleteBoxItemData> result = new List<AutoCompleteBoxItemData>();

            foreach (Tag tag in lstTags)
            {
                AutoCompleteBoxItemData childNode = new AutoCompleteBoxItemData
                {
                    Text = tag.TagText,
                    Value = tag.TagId.ToString()
                };
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };

            return res;
        }

        [WebMethod] 
        public static AutoCompleteBoxData GetProductParent(object context)
        {
            string searchString = ((Dictionary<string, object>)context)["Text"].ToString();
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var lst = Product.GetPageAdv(siteId: siteSettings.SiteId, parentId: 0, keyword: searchString);
            var result = new List<AutoCompleteBoxItemData>();

            foreach (var prd in lst)
            {
                var childNode = new AutoCompleteBoxItemData
                {
                    Text = prd.Title,
                    Value = prd.ProductId.ToString()
                };
                result.Add(childNode);
            }

            int resultLimit = ConfigHelper.GetIntProperty("Product:ParentSearchLimit", 20);
            if (result.Count > resultLimit)
                result = result.Take(resultLimit).ToList();

            var res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };
            return res;
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit
    }
}
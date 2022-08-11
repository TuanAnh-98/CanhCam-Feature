/// Created:			    2013-01-01
/// Last Modified:		    2016-04-11: Return Json format

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using System.Text;
using System.Web;
using System.Web.Services;

namespace CanhCam.Web.ProductUI
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class CompareService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "application/json";
            Encoding encoding = new UTF8Encoding();
            context.Response.ContentEncoding = encoding;

            int productId = WebUtils.ParseInt32FromQueryString("productid", -1);
            int type = WebUtils.ParseInt32FromQueryString("type", -1);
            if (productId > 0 && type > 0)
            {
                var productIds = ProductHelper.GetCompareProductsIds();
                if (type == 1)
                {
                    //AddProductToCompareList
                    SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
                    var product = new Product(siteSettings.SiteId, productId);
                    if (product != null
                        && product.ProductId > 0
                        && product.IsPublished
                        && !product.IsDeleted
                        && !productIds.Contains(product.ProductId))
                    {
                        ProductHelper.AddProductToCompareList(product.ProductId);

                        if (productIds.Count >= ProductConfiguration.MaximumCompareItems)
                            productIds.RemoveAt(productIds.Count - 1);

                        productIds.Add(product.ProductId);
                    }
                }
                else if (type == 2)
                {
                    //RemoveProductFromCompareList
                    ProductHelper.RemoveProductFromCompareList(productId);
                    if (productIds.Contains(productId))
                        productIds.Remove(productId);
                }

                context.Response.Write(StringHelper.ToJsonString(new
                {
                    success = true,
                    productIds
                }));
            }
        }

        public bool IsReusable => false;
    }
}
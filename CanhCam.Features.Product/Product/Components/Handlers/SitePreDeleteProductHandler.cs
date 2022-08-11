/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23

using CanhCam.Business.WebHelpers;

namespace CanhCam.Features
{
    public class SitePreDeleteProductHandler : SitePreDeleteHandlerProvider
    {
        public SitePreDeleteProductHandler()
        { }

        public override void DeleteSiteContent(int siteId)
        {
            CanhCam.Business.ShoppingCartItem.DeleteBySite(siteId);
            CanhCam.Business.Product.DeleteBySite(siteId);
        }
    }
}
//	Created:			    2012-04-23
//	Last Modified:		    2012-04-23

using System.Web.UI.WebControls;
using CanhCam.Business;
using CanhCam.Business.WebHelpers;

namespace CanhCam.Web.UI
{
    /// <summary>
    /// Renders literal text of the Company Address Region/State from Site Settings
    /// example usage: <portal:CompanyAddressRegionLiteral id="fax1" runat="server" />
    /// </summary>
    public class CompanyAddressRegionLiteral : Literal
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            Text = siteSettings.CompanyRegion;

        }
    }
}
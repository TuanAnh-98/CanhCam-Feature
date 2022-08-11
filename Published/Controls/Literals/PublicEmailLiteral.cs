//	Created:			    2012-03-11
//	Last Modified:		    2012-03-11

using System.Web.UI.WebControls;
using CanhCam.Business;
using CanhCam.Business.WebHelpers;

namespace CanhCam.Web.UI
{
    /// <summary>
    /// Renders literal text of the public email from Site Settings
    /// example usage: <portal:PublicEmailLiteral id="siteEmail" runat="server" />
    /// </summary>
    public class PublicEmailLiteral : Literal
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            Text = siteSettings.CompanyPublicEmail;
            
        }


    }
}

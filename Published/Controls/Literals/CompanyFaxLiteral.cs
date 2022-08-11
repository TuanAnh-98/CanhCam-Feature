//	Created:			    2012-03-11
//	Last Modified:		    2012-03-11

using System.Web.UI.WebControls;
using CanhCam.Business;
using CanhCam.Business.WebHelpers;

namespace CanhCam.Web.UI
{
    /// <summary>
    /// Renders literal text of the Company Fax from Site Settings
    /// example usage: <portal:CompanyFaxLiteral id="fax1" runat="server" />
    /// </summary>
    public class CompanyFaxLiteral : Literal
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            if (siteSettings == null) { return; }
            Text = siteSettings.CompanyFax;
           
        }
    }
}

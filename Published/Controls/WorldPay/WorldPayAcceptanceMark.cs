/// Created:			    2013-01-01
/// Last Modified:		    2013-01-01

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.UI
{
    public class WorldPayAcceptanceMark : WebControl
    {
        private string instId = string.Empty;
        [Themeable(false)]
        public string InstId
        {
            get { return instId; }
            set { instId = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if (instId.Length == 0) { return; }

            writer.Write("<script language=\"JavaScript\" src=\"https://secure.worldpay.com/wcc/logo?instId=" + instId + "\"></script>");

            //base.Render(writer);
        }

    }
}
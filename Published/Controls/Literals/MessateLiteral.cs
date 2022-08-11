//	Created:			    2012-03-11
//	Last Modified:		    2012-03-11

using System.Web.UI.WebControls;
using CanhCam.Business;
using CanhCam.Business.WebHelpers;

namespace CanhCam.Web.UI
{
    /// <summary>
    /// Renders literal text of the messate template
    /// example usage: <portal:MessateLiteral runat="server" />
    /// </summary>
    public class MessateLiteral : Literal
    {
        private string systemCode = string.Empty;
        public string SystemCode
        {
            get { return systemCode; }
            set { systemCode = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            EnableViewState = false;

            if (systemCode.Length == 0)
                return;

            Text = MessageTemplate.GetMessage(systemCode);
        }
    }
}

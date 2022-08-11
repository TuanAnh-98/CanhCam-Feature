/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-07-01
/// Last Modified:			2014-07-01

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// This control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class CartDisplaySettings : WebControl
    {
        private string resourceFile = string.Empty;

        public string ResourceFile
        {
            get { return resourceFile; }
            set { resourceFile = value; }
        }

        private string resourceKey = string.Empty;

        public string ResourceKey
        {
            get { return resourceKey; }
            set { resourceKey = value; }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }
    }
}
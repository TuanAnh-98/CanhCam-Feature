using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// This control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class ManufacturerDisplaySettings : WebControl
    {
        private bool showDescription = false;

        public bool ShowDescription
        {
            get { return showDescription; }
            set { showDescription = value; }
        }

        private bool hasDetailPage = false;

        public bool HasDetailPage
        {
            get { return hasDetailPage; }
            set { hasDetailPage = value; }
        }

        private bool showPrimaryImage = false;

        public bool ShowPrimaryImage
        {
            get { return showPrimaryImage; }
            set { showPrimaryImage = value; }
        }

        private bool showSecondImage = false;

        public bool ShowSecondImage
        {
            get { return showSecondImage; }
            set { showSecondImage = value; }
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
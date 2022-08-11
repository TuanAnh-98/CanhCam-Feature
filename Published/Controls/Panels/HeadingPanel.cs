/// Created:			    2015-01-15
/// Last Modified:		    2015-01-15

using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.UI
{
    /// <summary>
    /// A sub class of BasePanel so it can be configured differently from theme.skin than panels used in other scenarios
    /// </summary>
    public class HeadingPanel : BasePanel
    {
        private string text = string.Empty;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            RenderContentsOnly = true;

            //LiteralExtraTopContent = "<div class=\"row titlepage\">";
            //LiteralExtraTopContent += "<div class=\"col-md-12\">";
            //LiteralExtraTopContent += "<div class=\"row\">";
            //LiteralExtraTopContent += "<div class=\"col-lg-12\">";
            //LiteralExtraTopContent += "<div class=\"page-header\">";
            //LiteralExtraTopContent += "<div class=\"row\">";
            //LiteralExtraTopContent += "<div class=\"col-md-12\">";
            //LiteralExtraTopContent += "<div class=\"col-sm-4\">";
            //LiteralExtraTopContent += "<h1>" + text + "</h1>";
            //LiteralExtraTopContent += "</div>";
            //LiteralExtraTopContent += "<div class=\"col-sm-8\">";
            //LiteralExtraTopContent += "<div class=\"pull-right btngroups\">";

            //LiteralExtraBottomContent = "</div>";
            //LiteralExtraBottomContent += "</div>";
            //LiteralExtraBottomContent += "</div>"; //End col-md-12
            //LiteralExtraBottomContent += "</div>"; //End row
            //LiteralExtraBottomContent += "</div>"; //End page-header
            //LiteralExtraBottomContent += "</div>"; //End col-lg-12
            //LiteralExtraBottomContent += "</div>"; //End row
            //LiteralExtraBottomContent += "</div>"; //End col-md-12
            //LiteralExtraBottomContent += "</div>"; //End row titlepage


            LiteralExtraTopContent = "<div class=\"header-buttons\">";
            LiteralExtraBottomContent = "</div>";
        }
    }
}
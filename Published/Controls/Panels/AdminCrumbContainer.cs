/// Created:			    2013-01-01
/// Last Modified:		    2013-01-01

using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.UI
{
    /// <summary>
    /// A sub class of BasePanel so it can be configured differently from theme.skin than panels used in other scenarios
    /// </summary>
    public class AdminCrumbContainer : BasePanel
    {

        private string adminCrumbSeparator = "&nbsp;&gt;";
        /// <summary>
        ///this can be set from theme.skin and is used to set the text on any <portal:AdminCrumbSeparator inside this panel
        /// 
        /// </summary>
        public string AdminCrumbSeparator
        {
            get { return adminCrumbSeparator; }
            set { adminCrumbSeparator = value; }
        }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            foreach (Control c in Controls)
            {
                if (c is AdminCrumbSeparator)
                {
                    AdminCrumbSeparator s = c as AdminCrumbSeparator;
                    s.Text = AdminCrumbSeparator;
                }
            }
        }
    }

    public class AdminCrumbSeparator : Literal
    {
        
    }
}
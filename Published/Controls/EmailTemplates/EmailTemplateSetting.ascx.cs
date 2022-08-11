/// Created:			    2013-11-08
/// Last Modified:		    2013-11-08

using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CanhCam.Business;
using CanhCam.Business.WebHelpers;

namespace CanhCam.Web.UI
{

    public partial class EmailTemplateSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        private void EnsureItems()
        {
            lnkNotificationTemplate.NavigateUrl = SiteUtils.GetNavigationSiteRoot() + "/AdminCP/NotificationTemplate.aspx";
            lnkNotificationTemplate.Text = Resources.Resource.EmailTemplateManageLink;

            if (dd == null)
            {
                dd = new DropDownList();
                

                if (this.Controls.Count == 0) { this.Controls.Add(dd); }
            }

            if (dd.Items.Count > 0) { return; }

            dd.Items.Clear();
            dd.DataValueField = "SystemCode";
            dd.DataTextField = "Name";
            dd.DataSource = EmailTemplate.GetBySite(CacheHelper.GetCurrentSiteSettings().SiteId);
            dd.DataBind();

            dd.Items.Insert(0, new ListItem(Resources.Resource.EmailTemplateAutoGenerate, string.Empty));
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            return dd.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ListItem item = dd.Items.FindByValue(val);
            if (item != null)
            {
                dd.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion

    }
}
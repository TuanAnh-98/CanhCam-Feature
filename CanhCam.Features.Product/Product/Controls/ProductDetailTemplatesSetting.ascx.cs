/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-06-24
/// Last Modified:		    2014-06-24

using CanhCam.Web.UI;
using System;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductDetailTemplatesSetting : UserControl, ISettingControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region ISettingControl

        public string GetValue()
        {
            return ddlTemplates.SelectedValue;
        }

        public void SetValue(string val)
        {
            PopulateControls();

            var item = ddlTemplates.Items.FindByValue(val);
            if (item != null)
            {
                ddlTemplates.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion ISettingControl

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PopulateControls();
        }

        private void PopulateControls()
        {
            if (ddlTemplates.Items.Count > 0) { return; }

            ddlTemplates.DataValueField = "Value";
            ddlTemplates.DataTextField = "Name";
            ddlTemplates.DataSource = EnumDefined.LoadFromConfigurationXml("product", "templatedetail", "xsltname");
            ddlTemplates.DataBind();
        }
    }
}
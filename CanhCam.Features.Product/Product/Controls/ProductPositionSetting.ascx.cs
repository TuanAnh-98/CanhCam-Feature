/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-06-24
/// Last Modified:		    2014-06-24

using CanhCam.Web.UI;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductPositionSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        private void EnsureItems()
        {
            if (ddPosition == null)
            {
                ddPosition = new DropDownList
                {
                    DataValueField = "Value",
                    DataTextField = "Name"
                };

                if (this.Controls.Count == 0) { this.Controls.Add(ddPosition); }
            }

            if (ddPosition.Items.Count > 0) { return; }

            ddPosition.DataSource = EnumDefined.LoadFromConfigurationXml("product");
            ddPosition.DataBind();

            ddPosition.Items.Insert(0, new ListItem("", "-1"));
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            return ddPosition.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ListItem item = ddPosition.Items.FindByValue(val);
            if (item != null)
            {
                ddPosition.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion ISettingControl
    }
}
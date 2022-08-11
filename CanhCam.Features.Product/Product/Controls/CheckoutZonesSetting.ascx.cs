/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-07-29
/// Last Modified:		    2015-07-29

using CanhCam.Web.UI;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class CheckoutZonesSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        private void EnsureItems()
        {
            if (ddlZones == null)
            {
                ddlZones = new DropDownList();

                if (this.Controls.Count == 0) { this.Controls.Add(ddlZones); }
            }

            if (ddlZones.Items.Count > 0) { return; }

            gbSiteMapProvider.PopulateListControl(ddlZones, false, Guid.Empty);
            ddlZones.Items.Insert(0, new ListItem("", ""));
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();

            return ddlZones.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ddlZones.ClearSelection();

            if (val != null && val.Length > 0)
            {
                ListItem li = ddlZones.Items.FindByValue(val);
                if (li != null)
                    li.Selected = true;
            }
        }

        #endregion ISettingControl
    }
}
/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-08-28
/// Last Modified:		    2014-08-28

using CanhCam.Business;
using CanhCam.Web.UI;
using Resources;
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class ParentZoneSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        private void EnsureItems()
        {
            if (ddZones == null)
            {
                ddZones = new DropDownList();

                if (this.Controls.Count == 0) { this.Controls.Add(ddZones); }
            }

            if (ddZones.Items.Count > 0) { return; }

            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);

            ddZones.Items.Insert(0, new ListItem(ProductResources.AutomaticallySelectCategory, "0"));
            ddZones.Items.Insert(0, new ListItem("", ""));
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();
            return ddZones.SelectedValue;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            ListItem item = ddZones.Items.FindByValue(val);
            if (item != null)
            {
                ddZones.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion ISettingControl
    }
}
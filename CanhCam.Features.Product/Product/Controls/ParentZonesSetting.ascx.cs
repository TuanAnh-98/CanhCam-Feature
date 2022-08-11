/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-11-28
/// Last Modified:		    2014-11-28

using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using Resources;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class ParentZonesSetting : UserControl, ISettingControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (HttpContext.Current == null) { return; }
            EnsureItems();
        }

        private void EnsureItems()
        {
            if (cobZones == null)
            {
                cobZones = new ComboBox();

                if (this.Controls.Count == 0) { this.Controls.Add(cobZones); }
            }

            cobZones.DataPlaceHolder = ProductResources.AutomaticallySelectCategory;
            if (cobZones.Items.Count > 0) { return; }

            gbSiteMapProvider.PopulateListControl(cobZones, false, Product.FeatureGuid);
        }

        #region ISettingControl

        public string GetValue()
        {
            EnsureItems();

            string results = string.Empty;
            string sepa = string.Empty;
            foreach (ListItem li in cobZones.SelectedItems)
            {
                if (li.Value.Length > 0)
                {
                    results += sepa + li.Value;
                    sepa = ";";
                }
            }

            return results;
        }

        public void SetValue(string val)
        {
            EnsureItems();
            cobZones.ClearSelection();

            if (val != null && val.Length > 0)
            {
                List<string> lstVals = val.SplitOnCharAndTrim(';');

                foreach (ListItem li in cobZones.Items)
                {
                    if (lstVals.Contains(li.Value))
                        li.Selected = true;
                }
            }
        }

        #endregion ISettingControl
    }
}
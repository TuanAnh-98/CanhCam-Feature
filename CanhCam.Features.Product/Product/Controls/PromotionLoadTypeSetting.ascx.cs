/// Author:   			    Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2020-10-30
/// Last Modified:		    2020-10-30

using CanhCam.Web.UI;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.BannerUI
{
    public partial class PromotionLoadTypeSetting : UserControl, ISettingControl
    {
        #region ISettingControl

        public string GetValue()
        {
            return ddlLoadType.SelectedValue;
        }

        public void SetValue(string val)
        {
            ListItem item = ddlLoadType.Items.FindByValue(val);
            if (item != null)
            {
                ddlLoadType.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion ISettingControl
    }
}
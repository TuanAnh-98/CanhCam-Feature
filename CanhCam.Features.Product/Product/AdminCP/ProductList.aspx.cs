/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-08-03

using System;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductListPage : CmsNonBasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit
    }
}
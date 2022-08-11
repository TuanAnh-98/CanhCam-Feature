using CanhCam.Business;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class AffiliateWithdrawMoneyList : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateOrderList));
        private Order order;
        private Product product;
        private OrderItem orderItem;
        private SiteSettings siteSettings;
        #endregion

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grid.NeedDataSource += Grid_NeedDataSource;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnExportExcel.Click += BtnExportExcel_Click;
        }

        private void DdlCartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {


        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Events

        #endregion

        #region grid
        private void Grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bool isApplied = true;

            int iCount = AffiliatePayment.GetCount();

            int pageNumber = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int pageSize = isApplied ? iCount : grid.PageSize;


            int userid = SiteUtils.GetCurrentSiteUser().UserId;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = isApplied;
            grid.PagerStyle.EnableSEOPaging = false;
            grid.DataSource = AffiliatePayment.GetPage(pageNumber, pageSize, out _, userid);
        }
        #endregion grid

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            LoadControl();

            if (!Page.IsPostBack)
            {
                PopulateControl();
            }

        }
        #endregion

        #region LoadSettings
        private void LoadControl()
        {
        }

        private void LoadSettings()
        {
        }
        #endregion

        #region Populate
        private void PopulateControl()
        {
        }
        #endregion
    }
}
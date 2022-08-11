using CanhCam.Business;
using CanhCam.Business.WebHelpers;
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
    public partial class AffiliateOrderList : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateOrderList));
        #endregion

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grid.NeedDataSource += Grid_NeedDataSource;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnExportExcel.Click += BtnExportExcel_Click;
            this.ddlAddProduct.SelectedIndexChanged += new EventHandler(DdlCartType_SelectedIndexChanged);
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

            int iCount = Affiliate.GetCount();

            int pageNumber = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int pageSize = isApplied ? iCount : grid.PageSize;
            

            int userid = SiteUtils.GetCurrentSiteUser().UserId;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = isApplied;
            grid.PagerStyle.EnableSEOPaging = false;
            grid.DataSource = Affiliate.GetPage(pageNumber, pageSize, out _, userid);
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
                DRPFill();
            }

        }
        #endregion

        #region LoadSettings
        public void DRPFill()
        {
            if (!IsPostBack)
            {
                ddlAddProduct.Items.Add(new ListItem("", ""));
                var productlist = Product.GetPageAdv();
                foreach (Product product in productlist)
                {
                    ListItem item = new ListItem(product.Title, product.ProductId.ToString());
                    ddlAddProduct.Items.Add(item);
                }
            }
        }

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

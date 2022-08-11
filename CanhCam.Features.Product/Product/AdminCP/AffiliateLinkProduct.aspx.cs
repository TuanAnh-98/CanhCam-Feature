using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class AffiliateLinkProduct : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateLinkProduct));
        private TimeZoneInfo timeZone = null;
        private SiteSettings siteSettings;
        private double timeOffset = 0;
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
            int id = Convert.ToInt32(ddlAddProduct.SelectedItem.Value);
            if (id > 0)
            {
                Product product = new Product(siteSettings.SiteId, id);
                product.Save();
            }
            grid.Rebind();

        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridDataItem item in grid.SelectedItems)
            {
                int.TryParse(item.GetDataKeyValue("productID").ToString(), out int id);
                if (id > 0)
                {
                    Product product = new Product(siteSettings.SiteId, id);
                    product.Save();
                }
                grid.Rebind();
            }
        }

        #endregion

        #region Events

        #endregion

        #region grid
        private void Grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bool isApplied = true;

            int iCount = Product.GetCountAdv();

            int pageNumber = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int pageSize = isApplied ? iCount : grid.PageSize;
            int totalPages = -1;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = isApplied;
            grid.PagerStyle.EnableSEOPaging = false;
            grid.DataSource = Product.GetPageAdv();
            //grid.DataSource = NhanVien.GetPage(startRowIndex, maximumRows, out _);
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
                ddlAddProduct.Items.Add(new ListItem("",""));
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
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            siteSettings = CacheHelper.GetCurrentSiteSettings();
        }
        #endregion

        #region Populate
        private void PopulateControl()
        {
        }
        #endregion
    }
}
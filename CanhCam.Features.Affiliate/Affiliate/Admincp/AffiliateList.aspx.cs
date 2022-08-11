using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using log4net;
using System;

using Telerik.Web.UI;

namespace CanhCam.Web.AffiliateUI
{
    public partial class AffiliateList : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateList));
        #endregion

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grid.NeedDataSource += Grid_NeedDataSource;
            this.btnDelete.Click += BtnDelete_Click;
            this.btnExportExcel.Click += BtnExportExcel_Click;
            this.btnSearch.Click += BtnSearch_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Events
        #endregion

        #region grid
        private void Grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bool isApplied = true;
            int iCount = SiteUser.UserCount(1);

            int pageNumber = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int pageSize = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = isApplied;
            grid.PagerStyle.EnableSEOPaging = false;
            grid.DataSource = SiteUser.GetPage(1, pageNumber, pageSize," ",3,out _);
            //grid.DataSource = NhanVien.GetPage(startRowIndex, maximumRows, out _);
        }
        #endregion grid

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!PhongBanPermission.CanViewList)
            //{
            //    SiteUtils.RedirectToAccessDeniedPage(this);
            //    return;
            //}

            LoadSettings();
            LoadControl();

        }
        #endregion

        #region LoadSettings
        private void LoadControl()
        {
            //lnkInsert.Visible = PhongBanPermission.CanCreate;
            //btnUpdate.Visible = PhongBanPermission.CanUpdate;
            //btnDelete.Visible = PhongBanPermission.CanDelete;


            var user = SiteUtils.GetCurrentSiteUser();
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings(); }
        #endregion

        #region Populate
        #endregion
    }
}
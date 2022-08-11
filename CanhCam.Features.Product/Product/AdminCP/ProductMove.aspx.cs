/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-07-22
/// Last Modified:			2014-07-22

using System;
using log4net;
using CanhCam.SearchIndex;
using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using Telerik.Web.UI;
using CanhCam.Web.Framework;
using System.Collections.Generic;
using System.Linq;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductMovePage : CmsNonBasePage
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(ProductMovePage));
        RadGridSEOPersister gridPersister1;
        RadGridSEOPersister gridPersister2;

        #region Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!WebUser.IsAdminOrContentAdmin && !SiteUtils.UserIsSiteEditor())
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        #endregion

        #region "RadGrid Event"

        void grid1_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid1.PagerStyle.EnableSEOPaging = false;

            bool isApplied = gridPersister1.IsAppliedSortFilterOrGroup;
            int iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: ddZones1.SelectedValue, searchProductZone: displaySettings.ShowMultipleZones);
            int startRowIndex = isApplied ? 1 : grid1.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid1.PageSize;

            grid1.VirtualItemCount = iCount;
            grid1.AllowCustomPaging = !isApplied;

            grid1.DataSource = Product.GetPageAdv(pageNumber: startRowIndex, pageSize: maximumRows, siteId: siteSettings.SiteId, zoneIds: ddZones1.SelectedValue, searchProductZone: displaySettings.ShowMultipleZones);
        }

        void grid2_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid2.PagerStyle.EnableSEOPaging = false;

            bool isApplied = gridPersister2.IsAppliedSortFilterOrGroup;
            int iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: ddZones2.SelectedValue, searchProductZone: displaySettings.ShowMultipleZones);
            int startRowIndex = isApplied ? 1 : grid2.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid2.PageSize;

            grid2.VirtualItemCount = iCount;
            grid2.AllowCustomPaging = !isApplied;

            grid2.DataSource = Product.GetPageAdv(pageNumber: startRowIndex, pageSize: maximumRows, siteId: siteSettings.SiteId, zoneIds: ddZones2.SelectedValue, searchProductZone: displaySettings.ShowMultipleZones);
        }

        #endregion

        #region Event

        void btnLeft_Click(object sender, EventArgs e)
        {
            UpdateProductZone(false);
        }

        void btnRight_Click(object sender, EventArgs e)
        {
            UpdateProductZone(true);
        }

        private bool UpdateProductZone(bool moveRight)
        {
            if (
                ddZones1.SelectedValue.Length == 0
                || ddZones2.SelectedValue.Length == 0
                || ddZones1.SelectedValue == ddZones2.SelectedValue
                )
                return false;

            bool isUpdated = false;
            if (moveRight)
            {
                foreach (GridDataItem data in grid1.SelectedItems)
                {
                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    //int zoneId = Convert.ToInt32(data.GetDataKeyValue("ZoneId"));
                    var zones = ProductZone.SelectAllByProductID(productId);
                    int zoneIdNew = 0;
                    int.TryParse(ddZones2.SelectedValue, out zoneIdNew);

                    if (!zones.Any(it => it.ZoneID == zoneIdNew) //neu khac zone hien tai
                        && zoneIdNew > 0
                        //&& zoneId.ToString() == ddZones1.SelectedValue
                        )
                    {
                        Product product = new Product(SiteId, productId);

                        if (product != null && product.ProductId > 0)
                        {

                            if (zones.Count == 1)//Khong co chuyen muc phu
                            {
                                if (Product.UpdateZone(productId, zoneIdNew))
                                {
                                    List<FriendlyUrl> friendlyUrls = FriendlyUrl.GetByPageGuid(product.ProductGuid);
                                    foreach (FriendlyUrl item in friendlyUrls)
                                    {
                                        item.RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                            + zoneIdNew.ToInvariantString()
                                            + "&ProductID=" + product.ProductId.ToInvariantString();

                                        item.Save();
                                    }

                                    //Xoa nhung chuyen muc cu~
                                    foreach (var old in zones)
                                        ProductZone.Delete(old.ProductID, old.ZoneID);

                                }
                            }
                            new ProductZone() { ProductID = product.ProductId, ZoneID = zoneIdNew }.Save();

                            product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                            LogActivity.Write("Change product zone", product.Title);

                            isUpdated = true;
                            //if (Product.UpdateZone(productId, zoneIdNew))
                            //{
                            //    List<FriendlyUrl> friendlyUrls = FriendlyUrl.GetByPageGuid(product.ProductGuid);
                            //    foreach (FriendlyUrl item in friendlyUrls)
                            //    {
                            //        item.RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                            //            + zoneIdNew.ToInvariantString()
                            //            + "&ProductID=" + product.ProductId.ToInvariantString();

                            //        item.Save();
                            //    }

                            //    product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                            //    LogActivity.Write("Change product zone", product.Title);

                            //    isUpdated = true;
                            //}
                        }
                    }
                }
            }
            else
            {
                foreach (GridDataItem data in grid2.SelectedItems)
                {
                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    int zoneId = Convert.ToInt32(data.GetDataKeyValue("ZoneId"));
                    var zones = ProductZone.SelectAllByProductID(productId);

                    int.TryParse(ddZones2.SelectedValue, out int zoneIdNew);

                    if (zones.Any(it => it.ZoneID == zoneIdNew) //neu  zone dang chon de xoa' co' ton` tai.
                        && zoneId.ToString() == ddZones2.SelectedValue)
                    {
                        Product product = new Product(SiteId, productId);

                        if (product != null && product.ProductId > 0)
                        {

                            if (product.ZoneId == zoneIdNew)//la chuyen muc chinh
                            {

                                if (Product.UpdateZone(productId, zoneIdNew))
                                {
                                    List<FriendlyUrl> friendlyUrls = FriendlyUrl.GetByPageGuid(product.ProductGuid);
                                    foreach (FriendlyUrl item in friendlyUrls)
                                    {
                                        item.RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                            + zoneIdNew.ToInvariantString()
                                            + "&ProductID=" + product.ProductId.ToInvariantString();

                                        item.Save();
                                    }


                                }
                            }

                            foreach (var zone in zones)
                            {
                                if (zone.ZoneID == zoneIdNew)//can xoa
                                    ProductZone.Delete(zone.ProductID, zone.ZoneID);
                            }

                            int.TryParse(ddZones1.SelectedValue, out int zoneLeft);
                            new ProductZone() { ProductID = product.ProductId, ZoneID = zoneLeft }.Save();

                            product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                            LogActivity.Write("Change product zone", product.Title);

                            isUpdated = true;
                        }
                    }
                }
            }

            if (isUpdated)
            {
                SiteUtils.QueueIndexing();

                grid1.Rebind();
                grid2.Rebind();

                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");

                return true;
            }

            return false;
        }

        void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["ProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        void ddZones1_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid1.Rebind();
        }

        void ddZones2_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid2.Rebind();
        }

        #endregion

        #region Protected methods

        #endregion

        #region Populate

        private void PopulateLabels()
        {
            heading.Text = Resources.ProductResources.ProductMoveTilte;
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);

            //btnRight.ImageUrl = "~/Data/SiteImages/rt2.gif";
            //btnRight.AlternateText = Resources.ProductResources.ProductMoveRightAlternateText;
            btnRight.ToolTip = Resources.ProductResources.ProductMoveRightAlternateText;

            //btnLeft.ImageUrl = "~/Data/SiteImages/lt2.gif";
            //btnLeft.AlternateText = Resources.ProductResources.ProductMoveLeftAlternateText;
            btnLeft.ToolTip = Resources.ProductResources.ProductMoveLeftAlternateText;
        }

        private void PopulateControls()
        {
            PopulateZoneList();
        }

        #endregion

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones1, false, Product.FeatureGuid);
            gbSiteMapProvider.PopulateListControl(ddZones2, false, Product.FeatureGuid);
        }

        #endregion

        #region LoadSettings

        private void LoadSettings()
        {

        }

        #endregion

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);

            btnLeft.Click += new EventHandler(btnLeft_Click);
            btnRight.Click += new EventHandler(btnRight_Click);

            grid1.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid1_NeedDataSource);
            grid2.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid2_NeedDataSource);

            ddZones1.SelectedIndexChanged += new EventHandler(ddZones1_SelectedIndexChanged);
            ddZones2.SelectedIndexChanged += new EventHandler(ddZones2_SelectedIndexChanged);

            gridPersister1 = new RadGridSEOPersister(grid1);
            gridPersister2 = new RadGridSEOPersister(grid2);
        }

        #endregion

    }
}

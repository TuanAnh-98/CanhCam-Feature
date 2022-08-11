/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-08-11
/// Last Modified:		    2015-08-11

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class DiscountChooseGiftDialog : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountChooseGiftDialog));

        private Guid guid = Guid.Empty;
        private DiscountAppliedToItem item = null;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            gridRelated.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(gridRelated_NeedDataSource);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);

            SiteUtils.SetupEditor(edCustomValue, true, Page);
            SiteUtils.SetupEditor(edGiftDescription, true, Page);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (item == null || item.Guid == Guid.Empty)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            PopulateZoneList();

            edCustomValue.Text = item.GiftCustomProducts;
            edGiftDescription.Text = item.GiftDescription;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
        }

        private void LoadSettings()
        {
            guid = WebUtils.ParseGuidFromQueryString("Guid", guid);

            if (guid != Guid.Empty)
            {
                item = new DiscountAppliedToItem(guid);
                if (
                    item != null
                    && item.Guid != Guid.Empty
                )
                { }
                else
                    item = null;
            }

            edCustomValue.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edCustomValue.WebEditor.Height = Unit.Pixel(300);

            edGiftDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edGiftDescription.WebEditor.Height = Unit.Pixel(300);

            AddClassToBody("choosegift");
        }

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);

            if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
                ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), "-1"));
        }

        #endregion Populate Zone List

        #region Applied

        private string sZoneId
        {
            get
            {
                if (ddZones.SelectedValue.Length > 0)
                {
                    if (ddZones.SelectedValue == "-1")
                        return string.Empty;

                    return ddZones.SelectedValue;
                }

                return "0";
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!Page.IsPostBack)
                grid.DataSource = new List<Product>();
            else
            {
                bool isApplied = false;
                int iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: sZoneId, keyword: txtTitle.Text.Trim());
                int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
                int maximumRows = isApplied ? iCount : grid.PageSize;

                grid.VirtualItemCount = iCount;
                grid.AllowCustomPaging = !isApplied;

                grid.DataSource = Product.GetPageAdv(pageNumber: startRowIndex, pageSize: maximumRows, siteId: siteSettings.SiteId, zoneIds: sZoneId, keyword: txtTitle.Text.Trim());
            }
        }

        private List<Product> lstProducts = new List<Product>();

        private void gridRelated_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (item != null && item.Guid != Guid.Empty && item.GiftProducts.Length > 0)
                gridRelated.DataSource = Product.GetPageAdv(pageNumber: 1, pageSize: 100, siteId: siteSettings.SiteId, productIds: item.GiftProducts);
            else
                gridRelated.DataSource = new List<Product>();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                item.GiftProducts = string.Empty;
                item.GiftCustomProducts = edCustomValue.Text.Trim();
                item.GiftDescription = edGiftDescription.Text;

                item.GiftType = 0;
                if (item.GiftProducts.Length > 0 || item.GiftCustomProducts.Length > 0)
                    item.GiftType = 1;

                item.Save();

                if (chkCopy.Checked)
                {
                    List<DiscountAppliedToItem> lstItems = DiscountAppliedToItem.GetByDiscount(item.DiscountId, item.AppliedType);
                    lstItems.ForEach(s =>
                    {
                        if (s.Guid != item.Guid)
                        {
                            s.GiftProducts = item.GiftProducts;
                            s.GiftCustomProducts = item.GiftCustomProducts;
                            s.GiftDescription = item.GiftDescription;
                            s.GiftType = item.GiftType;

                            s.Save();
                        }
                    });
                }

                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MoveProducts();
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            MoveProducts(true);
        }

        private bool MoveProducts(bool isRemove = false)
        {
            var lstProductId = PromotionsHelper.GetProductIDsByGiftProducts(item.ComboSaleRules);
            if (!isRemove)
            {
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));

                    if (!lstProductId.Contains(productId))
                        lstProductId.Add(productId);
                }
            }
            else
            {
                foreach (Telerik.Web.UI.GridDataItem data in gridRelated.SelectedItems)
                {
                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    if (lstProductId.Contains(productId))
                        lstProductId.Remove(productId);
                }
            }

            item.GiftProducts = string.Join(";", lstProductId);

            item.GiftType = 0;
            if (item.GiftProducts.Length > 0 || item.GiftCustomProducts.Length > 0)
                item.GiftType = 1;

            item.Save();

            gridRelated.Rebind();

            return true;
        }

        #endregion Applied
    }
}
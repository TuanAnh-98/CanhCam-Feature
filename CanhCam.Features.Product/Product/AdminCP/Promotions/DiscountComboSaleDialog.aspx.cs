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
    public partial class DiscountComboSaleDialog : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountComboSaleDialog));

        private Guid guid = Guid.Empty;
        private DiscountAppliedToItem item = null;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            gridRelated.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(gridRelated_NeedDataSource);
            gridRelated.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(gridRelated_ItemDataBound);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnAdd.Click += btnAdd_Click;
            btnRemove.Click += btnRemove_Click;
            btnUpdate.Click += new EventHandler(btnUpdate_Click);

            SiteUtils.SetupEditor(edGiftCustomProducts, true, Page);
            SiteUtils.SetupEditor(edGiftProducts, true, Page);
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

            Discount discount = new Discount(item.DiscountId);
            if (discount == null || discount.SiteId != siteSettings.SiteId || discount.DiscountType != (int)DiscountType.ComboSale)
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

            edGiftCustomProducts.Text = item.GiftCustomProducts;
            edGiftProducts.Text = item.GiftProducts;
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

            edGiftCustomProducts.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edGiftCustomProducts.WebEditor.Height = Unit.Pixel(300);

            edGiftProducts.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edGiftProducts.WebEditor.Height = Unit.Pixel(300);

            edGiftDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edGiftDescription.WebEditor.Height = Unit.Pixel(300);

            AddClassToBody("combosale");
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

        private void gridRelated_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (item != null && item.Guid != Guid.Empty && item.ComboSaleRules.Length > 0)
                gridRelated.DataSource = PromotionsHelper.ParseComboSaleProducts(siteSettings.SiteId, item.ComboSaleRules);

            if (gridRelated.DataSource == null)
                gridRelated.DataSource = new List<DiscountAppliedToItem>();
        }

        private void gridRelated_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem data = (Telerik.Web.UI.GridDataItem)e.Item;
                int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                TextBox txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                TextBox txtComboSaleQty = (TextBox)data.FindControl("txtComboSaleQty");
                DropDownList ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                var appliedItem = PromotionsHelper.ParseComboSaleValue(item.ComboSaleRules, productId);
                if (appliedItem != null)
                {
                    txtDiscountAmount.Text = ProductHelper.FormatPrice(appliedItem.DiscountAmount, false);
                    txtComboSaleQty.Text = appliedItem.ComboSaleQty.ToString();

                    if (appliedItem.UsePercentage)
                        ddlDiscountType.SelectedIndex = 0;
                    else
                        ddlDiscountType.SelectedIndex = 1;
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
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
            string comboSaleRulesXml = string.Empty;
            if (!isRemove)
            { // add products
                comboSaleRulesXml = item.ComboSaleRules;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    comboSaleRulesXml = PromotionsHelper.AddComboSaleRules(comboSaleRulesXml, productId, 1, false, 0);
                }
            }
            else
            {
                foreach (Telerik.Web.UI.GridDataItem data in gridRelated.Items)
                {
                    if (!data.Selected)
                    {
                        int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                        TextBox txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                        TextBox txtComboSaleQty = (TextBox)data.FindControl("txtComboSaleQty");
                        DropDownList ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                        int quantity = -1;
                        int.TryParse(txtComboSaleQty.Text, out quantity);

                        decimal discountAmount = -1;
                        decimal.TryParse(txtDiscountAmount.Text, out discountAmount);

                        if (quantity > 0 && discountAmount >= 0)
                            comboSaleRulesXml = PromotionsHelper.AddComboSaleRules(comboSaleRulesXml, productId, quantity,
                                                ddlDiscountType.SelectedIndex == 0 ? true : false, discountAmount);
                    }
                }
            }

            item.ComboSaleRules = comboSaleRulesXml;
            item.Save();

            gridRelated.Rebind();
            message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");

            return true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string result = string.Empty;
            foreach (Telerik.Web.UI.GridDataItem data in gridRelated.Items)
            {
                int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                TextBox txtComboSaleQty = (TextBox)data.FindControl("txtComboSaleQty");
                TextBox txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                DropDownList ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                int quantity = -1;
                int.TryParse(txtComboSaleQty.Text, out quantity);

                decimal discountAmount = -1;
                decimal.TryParse(txtDiscountAmount.Text, out discountAmount);

                if (quantity > 0 && discountAmount >= 0)
                    result = PromotionsHelper.AddComboSaleRules(result, productId, quantity, ddlDiscountType.SelectedIndex == 0 ? true : false,
                             discountAmount);
            }

            item.ComboSaleRules = result;
            item.GiftProducts = edGiftProducts.Text.Trim();
            item.GiftCustomProducts = edGiftCustomProducts.Text.Trim();
            item.GiftDescription = edGiftDescription.Text;
            item.Save();

            gridRelated.Rebind();
            message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
        }

        #endregion Applied
    }
}
/// Author:                 Tran Quoc Vuong - itqvuong@gmail.com
/// Created:			    2014-07-01
/// Last Modified:		    2014-07-01
/// 2015-10-23: Enable shopping cart with manufacturer attributes

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class CartListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CartListPage));

        private bool canEdit = false;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private List<ProductProperty> productProperties = new List<ProductProperty>();
        private List<CustomField> customFields = new List<CustomField>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!canEdit)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (!Page.IsPostBack)
            {
                ddlCartType.Items.Clear();
                ddlCartType.Items.Add(new ListItem(ProductResources.CartTypeShoppingCart, ((int)ShoppingCartTypeEnum.ShoppingCart).ToString()));

                if (ProductConfiguration.EnableWishlist)
                    ddlCartType.Items.Add(new ListItem(ProductResources.CartTypeWishlist, ((int)ShoppingCartTypeEnum.Wishlist).ToString()));

                if (ddlCartType.Items.Count == 1)
                {
                    lblCartType.Visible = false;
                    ddlCartType.Visible = false;
                }
            }
        }

        private void ddlCartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bool isApplied = false;
            int iCount = ShoppingCartItem.GetCount(siteSettings.SiteId, Convert.ToInt32(ddlCartType.SelectedValue));

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = !isApplied;

            var list = ShoppingCartItem.GetPage(
                siteSettings.SiteId,
                Convert.ToInt32(ddlCartType.SelectedValue),
                startRowIndex,
                maximumRows);

            if (ProductConfiguration.EnableShoppingCartAttributes)
            {
                var lstProductIds = list.Select(x => x.ProductId).Distinct().ToList();
                customFields = CustomField.GetActiveForCart(siteSettings.SiteId, Product.FeatureGuid);
                if (customFields.Count > 0 && lstProductIds.Count > 0)
                    productProperties = ProductProperty.GetPropertiesByProducts(lstProductIds);
            }

            grid.DataSource = list;
        }

        private void grid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;

                Literal litAttributes = (Literal)item.FindControl("litAttributes");
                if (litAttributes != null)
                {
                    int productId = Convert.ToInt32(item.GetDataKeyValue("ProductId"));
                    string attributesXml = item.GetDataKeyValue("AttributesXml").ToString();
                    string results = string.Empty;

                    if (!string.IsNullOrEmpty(attributesXml))
                    {
                        var attributes = ProductAttributeParser.ParseProductAttributeMappings(customFields, attributesXml);
                        if (attributes.Count > 0)
                        {
                            foreach (var a in attributes)
                            {
                                var values = ProductAttributeParser.ParseValues(attributesXml, a.CustomFieldId);
                                if (values.Count > 0)
                                {
                                    productProperties.ForEach(property =>
                                    {
                                        if (property.ProductId == productId
                                            && property.CustomFieldId == a.CustomFieldId
                                            && values.Contains(property.CustomFieldOptionId))
                                            results += string.Format("<div><span>{0}</span>: {1}</div>", a.Name, property.OptionName);
                                    });
                                }
                            }
                        }
                    }

                    if (results.Length > 0)
                        litAttributes.Text = string.Format("<div class='attributes'>{0}</div>", results);
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                int.TryParse(txtOlderDays.Text, out int olderDays);
                olderDays = Math.Abs(olderDays);

                DateTime cutoffDate = DateTime.Now.AddDays(-olderDays);
                if (ShoppingCartItem.DeleteOlderThan(SiteId, Convert.ToInt32(ddlCartType.SelectedValue), cutoffDate))
                {
                    LogActivity.Write("Delete cart older " + cutoffDate + " day(s)", "Cart");
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    Guid guid = new Guid(data.GetDataKeyValue("Guid").ToString());

                    if (ShoppingCartItem.Delete(guid))
                        iRecordDeleted += 1;
                }

                if (iRecordDeleted > 0)
                {
                    LogActivity.Write("Delete " + iRecordDeleted.ToString() + " item(s)", "Cart");
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected string GetProductUrl(string productUrl, int productId, int zoneId, string productTitle)
        {
            return string.Format("<a href='{0}'>{1}</a>", ProductHelper.FormatProductUrl(productUrl, productId, zoneId), productTitle);
        }

        protected string GetCustomerName(int userId, string createdByName, string userGuid)
        {
            if (createdByName.Length > 0)
                return createdByName;

            return userGuid;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.CartAdminTitle);
            heading.Text = ProductResources.CartAdminTitle;

            UIHelper.AddConfirmationDialog(btnClear, ResourceHelper.GetResourceString("Resource", "ClearConfirmMessage"));
            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void LoadSettings()
        {
            canEdit = WebUser.IsAdmin;

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            AddClassToBody("admin-carts");
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnClear.Click += new EventHandler(btnClear_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);

            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.grid.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(grid_ItemDataBound);

            this.ddlCartType.SelectedIndexChanged += new EventHandler(ddlCartType_SelectedIndexChanged);
        }

        #endregion OnInit
    }
}
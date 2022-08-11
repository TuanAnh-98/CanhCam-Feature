using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class OrderAddNewProductDialog : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderAddNewProductDialog));
        private Order order = null;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            grid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(grid_ItemCommand);
            btnSearch.Click += new EventHandler(btnSearch_Click);
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

            if (!WebUser.IsAdminOrContentAdmin && !ProductPermission.CanManageOrders)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            if (order == null || order.OrderId <= 0)
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
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
        }

        private void LoadSettings()
        {
            var orderId = WebUtils.ParseInt32FromQueryString("orderid", -1);

            if (orderId > 0)
            {
                order = new Order(orderId);
                if (
                    order != null
                    && order.OrderId > 0
                    && !order.IsDeleted
                    )
                { }
                else
                    order = null;
            }
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
            {
                grid.DataSource = new List<Product>();
            }
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

        private void grid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "AddNew":
                        var productId = -1;
                        int.TryParse(e.CommandArgument.ToString(), out productId);

                        var product = new Product(SiteId, productId);
                        if (product != null && product.ProductId > 0 && !product.IsDeleted)
                        {
                            var orderItem = (OrderItem)null;
                            var lstOrderItems = OrderItem.GetByOrder(order.OrderId);
                            foreach (var item in lstOrderItems)
                            {
                                if (item.ProductId == product.ProductId)
                                {
                                    orderItem = item;
                                    break;
                                }
                            }

                            if (orderItem != null)
                            {
                                orderItem.Quantity += 1;
                                orderItem.Save();

                                order.OrderSubtotal += orderItem.Price;
                                order.OrderTotal = CartHelper.GetCartTotal(order.OrderSubtotal, 
                                    shippingTotal: order.OrderShipping,
                                    discountTotal: order.OrderDiscount,
                                    discountCouponTotal: order.OrderCouponAmount,
                                    rewardPointAmount: order.RedeemedRewardPointsAmount);
                                order.Save();
                            }
                            else
                            {
                                var newItem = new OrderItem
                                {
                                    OrderId = order.OrderId,
                                    Price = ProductHelper.GetOriginalPrice(product),
                                    UnitPrice = 0,
                                    DiscountAmount = 0,
                                    ProductId = product.ProductId,
                                    Quantity = 1,
                                    AttributesXml = string.Empty,
                                    AttributeDescription = string.Empty
                                };

                                newItem.Save();

                                order.OrderSubtotal += newItem.Price;
                                order.OrderTotal = CartHelper.GetCartTotal(order.OrderSubtotal,
                                    shippingTotal: order.OrderShipping,
                                    discountTotal: order.OrderDiscount,
                                    discountCouponTotal: order.OrderCouponAmount,
                                    rewardPointAmount: order.RedeemedRewardPointsAmount
                                    );
                                order.Save();
                            }

                            message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        #endregion Applied
    }
}
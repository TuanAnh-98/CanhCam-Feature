using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.StoreUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class OrderTrackingPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderListPage));

        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private SiteUser currentUser = null;

        private bool enabledStore = false;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            if (!WebUser.IsAdminOrContentAdmin)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();

            if (!enabledStore)
            {
                SiteUtils.RedirectToHomepage();
            }

            PopulateLabels();

            if (!IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            currentUser = SiteUtils.GetCurrentSiteUser();

            enabledStore = StoreHelper.EnabledStore;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.OrderTrackingTitle);
            heading.Text = ProductResources.OrderTrackingTitle;
        }

        private void PopulateControls()
        {
            PopulateStoreList(ddStore, true);
            OrderHelper.PopulateOrderStatus(ddOrderStatus, true);
        }

        #region "RadGrid Event"

        private List<Store> lstAllStores = new List<Store>();

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            lstAllStores = Store.GetAll();

            Int32.TryParse(ddStore.SelectedValue, out int storeID);
            Int32.TryParse(siteSettings.GetExpandoProperty("DefaultStore"), out int defaultStoreID);
            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;

            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            bool isApplied = false;
            int iCount = Order.GetCountByStores(currentUser.UserId, (WebUser.IsAdmin || !enabledStore), storeID, defaultStoreID,
                siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, null, txtKeyword.Text.Trim(), -1);

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;

            List<Order> list = Order.GetPageByStores(currentUser.UserId, (WebUser.IsAdmin || !enabledStore),
                storeID, defaultStoreID, siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, null, txtKeyword.Text.Trim(), -1, startRowIndex, maximumRows);
            grid.DataSource = list;
        }

        protected string GetStoreName(int storeId)
        {
            var store = lstAllStores.Where(s => s.StoreID == storeId).FirstOrDefault();
            if (store != null) return store.Name;

            return string.Empty;
        }

        private void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int orderID = Convert.ToInt32(item.GetDataKeyValue("OrderId"));
                int storeID = Convert.ToInt32(item.GetDataKeyValue("StoreID"));

                //Order Store
                DropDownList ddlStore = (DropDownList)item.FindControl("ddlStore");
                PopulateStoreList(ddlStore);
                ListItem li = ddlStore.Items.FindByValue(storeID.ToString());
                if (li != null)
                {
                    ddlStore.ClearSelection();
                    li.Selected = true;
                }

                //Order Products
                Literal litOrderProducts = (Literal)item.FindControl("litOrderProducts");
                List<OrderItem> lstOrderItems = OrderItem.GetByOrder(orderID);
                if (lstOrderItems.Count > 0)
                {
                    string strItems = "";
                    foreach (OrderItem orderItem in lstOrderItems)
                    {
                        Product product = new Product(siteSettings.SiteId, orderItem.ProductId);
                        strItems += string.Format("<div>{0} x {1}</div>", product.Title, orderItem.Quantity);
                    }
                    litOrderProducts.Text = strItems;
                }

                //Order Quick View
                HyperLink lnkQuickView = (HyperLink)item.FindControl("lnkQuickView");
                RadToolTipManager1.TargetControls.Add(lnkQuickView.ClientID, orderID.ToString(), true);
            }
        }

        #endregion "RadGrid Event"

        #region Event

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                bool isUpdated = false;
                foreach (GridDataItem item in grid.Items)
                {
                    DropDownList ddlStore = (DropDownList)item.FindControl("ddlStore");
                    int newStoreID = Int32.TryParse(ddlStore.SelectedValue, out newStoreID) ? newStoreID : 0;
                    int storeID = Int32.TryParse(item.GetDataKeyValue("StoreID").ToString(), out storeID) ? storeID : 0;

                    if (storeID != newStoreID)
                    {
                        int orderID = Convert.ToInt32(item.GetDataKeyValue("OrderId"));
                        Order order = new Order(orderID);
                        order.OldStoreId = order.StoreId;
                        order.StoreId = newStoreID;
                        if (order.Save())
                        {
                            //Modify stock quantity if 'Completed' order change store
                            OrderHelper.QuantityOrderStoreHandler(order, order.OldStoreId);
                            //Send change store email
                            OrderHelper.SendOrderStoreChangedEmail(siteSettings, order, storeID, newStoreID);

                            LogActivity.Write("Update order", order.OrderCode);
                            isUpdated = true;
                        }
                    }
                    else
                        continue;
                }
                if (isUpdated)
                {
                    grid.Rebind();
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
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

        #endregion Event

        #region Populate

        private void PopulateStoreList(DropDownList ddl, bool filter = false)
        {
            ddl.Items.Clear();
            if (filter)
            {
                ddl.Items.Add(new ListItem(StoreResources.AllStoreDropdown, string.Empty));
                //ddl.Items.Add(new ListItem(StoreResources.OrderNoStoreText, "-1"));
            }
            else
                ddl.Items.Add(new ListItem(StoreResources.InventoryStoreDropdown, string.Empty));

            var lstStores = Store.GetAll();
            if (!WebUser.IsAdmin)
            {
                foreach (Store store in lstStores.Reverse<Store>())
                {
                    var isAdmin = false;
                    var lstAdmins = store.OrderUserIDs.Split(';');
                    foreach (string adminID in lstAdmins)
                    {
                        if (adminID == currentUser.UserId.ToString())
                            isAdmin = true;
                    }
                    if (!isAdmin)
                        lstStores.Remove(store);
                }
            }

            ddl.DataSource = lstStores;
            ddl.DataBind();
        }

        #endregion Populate

        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            OrderDetailToolTipControl ctrl = Page.LoadControl("/Product/Controls/OrderDetailToolTipControl.ascx") as OrderDetailToolTipControl;
            ctrl.ID = "UcOrderDetail1";
            ctrl.OrderId = Convert.ToInt32(e.Value);
            e.UpdatePanel.ContentTemplateContainer.Controls.Add(ctrl);
        }
    }
}
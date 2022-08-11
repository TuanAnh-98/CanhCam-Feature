using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.StoreUI
{
    public partial class InventoryListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreListPage));
        private SiteUser siteUser;

        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private int displayMode = 2;
        private bool cannotDisplay = false;
        private int parentID = 0;

        private List<Store> lstStores = null;
        private bool enabledStore = false;
        private bool enabledInventory = false;

        //--Filters--//
        //Store
        private int storeID;

        //Price
        private bool fromPriceFill;

        private int fromPrice;
        private bool toPriceFill;
        private int toPrice;

        //Quantity
        private bool fromQuantityFill;

        private int fromQuantity;
        private bool toQuantityFill;
        private int toQuantity;

        //Status
        private string status;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            btnExport.Click += new EventHandler(btnExport_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnSearch.Click += new EventHandler(btnSearch_Click);

            //btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();
            //PopulateLabels();

            if (!enabledStore || !enabledInventory)
            {
                SiteUtils.RedirectToHomepage();
            }

            if (siteUser == null || siteUser.UserId <= 0)
            {
                SiteUtils.RedirectToAccessDeniedPage();
                return;
            }

            PopulateLabels();
            if (!IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            siteUser = SiteUtils.GetCurrentSiteUser();
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            lstStores = Store.GetAll();
            enabledStore = StoreHelper.EnabledStore;
            enabledInventory = StoreHelper.EnabledInventory;

            switch (ddDisplay.SelectedValue)
            {
                case "0": // Non-inputted
                    displayMode = 0;
                    divPriceFilter.Visible = false;
                    divQuantityFilter.Visible = false;
                    divStatusFilter.Visible = false;
                    break;

                case "1": //Inputted
                    displayMode = 1;
                    divPriceFilter.Visible = false;
                    divQuantityFilter.Visible = true;
                    divStatusFilter.Visible = true;
                    break;

                case "2": //All
                    displayMode = 2;
                    divPriceFilter.Visible = false;
                    divQuantityFilter.Visible = true;
                    divStatusFilter.Visible = true;
                    break;

                default:
                    cannotDisplay = true;
                    divPriceFilter.Visible = false;
                    break;
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, StoreResources.InventoryListTitle);
        }

        private void PopulateControls()
        {
            PopulateStoreList();
            PopulateStatusList();
            PopulateDisplayList();
        }

        private void PopulateStoreList()
        {
            ddStore.Items.Clear();
            //ddStore.Items.Add(new ListItem(StoreResources.InventoryStoreDropdown, string.Empty));

            var lstStores = Store.GetAll();
            if (!WebUser.IsAdmin)
            {
                foreach (Store store in lstStores.Reverse<Store>())
                {
                    var isAdmin = false;
                    var lstAdmins = store.OrderUserIDs.Split(';');
                    foreach (string adminID in lstAdmins)
                    {
                        if (adminID == siteUser.UserId.ToString())
                            isAdmin = true;
                    }
                    if (!isAdmin)
                        lstStores.Remove(store);
                }
            }

            ddStore.DataSource = lstStores;
            ddStore.DataBind();
        }

        private void PopulateStatusList()
        {
            ddStatus.Items.Clear();
            ddStatus.Items.Add(new ListItem(StoreResources.InventoryStatusDropdown, string.Empty));
            ddStatus.Items.Add(new ListItem(StoreResources.InventoryStockDropdown, "Stock"));
            ddStatus.Items.Add(new ListItem(StoreResources.InventoryStockoutDropdown, "Stockout"));
        }

        private void PopulateDisplayList()
        {
            ddDisplay.Items.Clear();
            ddDisplay.Items.Add(new ListItem(StoreResources.InventoryAllText, "2"));
            ddDisplay.Items.Add(new ListItem(StoreResources.InventoryInputtedText, "1"));
            ddDisplay.Items.Add(new ListItem(StoreResources.InventoryNonInputtedText, "0"));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                bool isUpdated = false;

                foreach (TreeListDataItem data in treeList.Items)
                {
                    CheckBox cbPublish = (CheckBox)data.FindControl("cbPublish");
                    TextBox txtApiProductID = (TextBox)data.FindControl("txtApiProductID");
                    TextBox txtQuantity = (TextBox)data.FindControl("txtQuantity");
                    int newQuantity = Int32.TryParse(txtQuantity.Text.Trim(), out newQuantity) ? newQuantity : -1;

                    int inventoryID = Convert.ToInt32(data.GetDataKeyValue("InventoryID"));
                    // Update non-inputted item
                    if (inventoryID == -1 && newQuantity > -1)
                    {
                        StockInventory inventory = new StockInventory();
                        Int32.TryParse(ddStore.SelectedValue, out int storeID);
                        inventory.ProductID = Convert.ToInt32(data.GetDataKeyValue("ProductID"));
                        inventory.StoreID = storeID;
                        inventory.IsPublished = cbPublish.Checked;
                        inventory.ApiProductID = txtApiProductID.Text.Trim();
                        inventory.Quantity = newQuantity;
                        inventory.Save(siteSettings.SiteId);
                        LogActivity.Write("Update stock inventory", inventory.InventoryID.ToString());
                        isUpdated = true;
                    }
                    // Update inputted item
                    else if (inventoryID > 0 && newQuantity > -1)
                    {
                        int quantity = Convert.ToInt32(data.GetDataKeyValue("Quantity"));
                        string APIProductID = data.GetDataKeyValue("ApiProductID").ToString();
                        bool isPublished = Convert.ToBoolean(data.GetDataKeyValue("IsPublished"));
                        if (quantity == newQuantity && APIProductID == txtApiProductID.Text.Trim() && cbPublish.Checked == isPublished)
                            continue;
                        StockInventory inventory = new StockInventory(inventoryID);
                        if (inventory != null && inventory.InventoryID > 0)
                        {
                            inventory.IsPublished = cbPublish.Checked;
                            inventory.ApiProductID = txtApiProductID.Text.Trim();
                            inventory.Quantity = newQuantity;
                            inventory.Save(siteSettings.SiteId);
                            LogActivity.Write("Update stock inventory", inventory.InventoryID.ToString());
                            isUpdated = true;
                        }
                    }
                }

                if (isUpdated)
                {
                    treeList.Rebind();
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            GetFilters();
            TotalRowCount = StockInventory.InventoryFilterCount(txtKeyword.Text.Trim(), storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, displayMode, parentID);
            RadDataPager1.CurrentPageIndex = 0;
            treeList.Rebind();
        }

        private DataTable GetData()
        {
            //--Filters--//
            if (!GetFilters())
                return null;

            return StockInventory.GetReportStockInventory(siteSettings.SiteId, txtKeyword.Text.Trim(), storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, displayMode);
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetData();
            if (dt == null) return;
            string fileName = "stock-inventory-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";
            treeList.Rebind();
            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    int iRecordDeleted = 0;
            //    foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
            //    {
            //        int inventoryID = Convert.ToInt32(data.GetDataKeyValue("InventoryID"));
            //        StockInventory stockInventory = new StockInventory(inventoryID);

            //        if (stockInventory != null && stockInventory.InventoryID > 0)
            //        {
            //            ContentDeleted.Create(siteSettings.SiteId, stockInventory.InventoryID.ToString(), "StockInventory", typeof(StockInventoryDeleted).AssemblyQualifiedName, stockInventory.InventoryID.ToString(), Page.User.Identity.Name);

            //            StockInventory.Delete(stockInventory.InventoryID);

            //            iRecordDeleted += 1;
            //        }
            //    }

            //    if (iRecordDeleted > 0)
            //    {
            //        LogActivity.Write("Delete " + iRecordDeleted.ToString() + " stock(s)", "Stock");
            //        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

            //        grid.Rebind();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //}
        }

        private bool GetFilters()
        {
            if (
                txtKeyword.Text.Trim().Length > 0
                || txtFromPrice.Text.Trim().Length > 0
                || txtToPrice.Text.Trim().Length > 0
                || txtFromQuantity.Text.Trim().Length > 0
                || txtToQuantity.Text.Trim().Length > 0
                || ddStatus.SelectedValue != ""
                || ddDisplay.SelectedValue != "2")
            {
                parentID = -1;
            }
            else
            {
                parentID = 0;
            }

            //--Filters--//
            //Store
            Int32.TryParse(ddStore.SelectedValue, out storeID);
            //Price
            fromPriceFill = Int32.TryParse(txtFromPrice.Text.Trim(), out fromPrice);
            fromPrice = fromPriceFill ? fromPrice : -1;
            fromPrice = divPriceFilter.Visible ? fromPrice : -1;
            toPriceFill = Int32.TryParse(txtToPrice.Text.Trim(), out toPrice);
            toPrice = toPriceFill ? toPrice : -1;
            toPrice = divPriceFilter.Visible ? toPrice : -1;
            if (fromPriceFill && toPriceFill && fromPrice > toPrice) return false;
            //Quantity
            fromQuantityFill = Int32.TryParse(txtFromQuantity.Text.Trim(), out fromQuantity);
            fromQuantity = fromQuantityFill ? fromQuantity : -1;
            fromQuantity = divQuantityFilter.Visible ? fromQuantity : -1;
            toQuantityFill = Int32.TryParse(txtToQuantity.Text.Trim(), out toQuantity);
            toQuantity = toQuantityFill ? toQuantity : -1;
            toQuantity = divQuantityFilter.Visible ? toQuantity : -1;
            if (fromQuantityFill && toQuantityFill && fromQuantity > toQuantity) return false;
            //Status
            status = ddStatus.SelectedValue;
            status = ddStatus.Visible ? status : "";

            return true;
        }

        #region TreeList

        protected void treeList_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            bindList(RadDataPager1.CurrentPageIndex + 1);
        }

        private void bindList(int pageNumber)
        {
            bool isApplied = false;

            //--Filters--//
            if (!GetFilters())
                return;

            List<StockInventory> list = new List<StockInventory>();

            int iCount = StockInventory.InventoryFilterCount(txtKeyword.Text.Trim(), storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, displayMode, parentID);
            int startRowIndex = isApplied ? 1 : pageNumber;
            int maximumRows = isApplied ? iCount : RadDataPager1.PageSize;

            list = StockInventory.InventoryFilter(txtKeyword.Text.Trim(), storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, displayMode, startRowIndex, maximumRows, true, true, true, parentID);
            treeList.DataSource = list;
        }

        private int TotalRowCount
        {
            get
            {
                if (ViewState["TotalRowCount"] != null)
                    return (int)ViewState["TotalRowCount"];
                int iCount = 0;
                if (!GetFilters())
                    return iCount;
                iCount = StockInventory.InventoryFilterCount(txtKeyword.Text.Trim(), storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, displayMode, parentID);
                ViewState["TotalRowCount"] = iCount;
                return iCount;
            }
            set
            {
                ViewState["TotalRowCount"] = value;
            }
        }

        protected void RadDataPager1_TotalRowCountRequest(object sender, RadDataPagerTotalRowCountRequestEventArgs e)
        {
            e.TotalRowCount = TotalRowCount;
        }

        protected void RadDataPager1_Command(object sender, RadDataPagerCommandEventArgs e)
        {
            if (e.CommandName == "PageSizeChange")
            {
                RadDataPager1.PageSize = Convert.ToInt32(e.CommandArgument);
                bindList(1);
            }
        }

        protected void RadDataPager1_PageIndexChanged(object sender, RadDataPagerPageIndexChangeEventArgs e)
        {
            bindList(e.NewPageIndex + 1);
        }

        protected void treeList_ChildItemsDataBind(object sender, TreeListChildItemsDataBindEventArgs e)
        {
            //--Filters--//
            if (!GetFilters())
                return;

            var id = Convert.ToInt32(e.ParentDataKeyValues["ProductID"].ToString());
            e.ChildItemsDataSource = StockInventory.InventoryFilter(txtKeyword.Text.Trim(), storeID, fromPrice, toPrice, fromQuantity, toQuantity, status, displayMode, 0, 0, true, true, true, id);
        }

        protected void treeList_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                var item = (TreeListDataItem)e.Item;
                var parentId = Convert.ToInt32(item.GetParentDataKeyValue("ParentID"));

                if (parentId > 0 || parentID == -1)
                {
                    var expandButton = item.FindControl("ExpandCollapseButton");
                    if (expandButton != null)
                    {
                        expandButton.Visible = false;
                    }
                }
            }
        }

        protected void treeList_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            if (e.Item is TreeListDataItem)
            {
                var item = (TreeListDataItem)e.Item;
                int productID = Convert.ToInt32(item.GetDataKeyValue("ProductID"));
                if (productID > 0)
                {
                    bool existsChild = false;//Product.ExistsChild(productID);
                    var expandButton = item.FindControl("ExpandCollapseButton");
                    if (expandButton != null && !existsChild)
                    {
                        expandButton.Visible = false;
                    }
                }

                Literal litProductName = (Literal)item.FindControl("litProductName");
                Literal litStoreName = (Literal)item.FindControl("litStoreName");
                TextBox txtQuantity = (TextBox)item.FindControl("txtQuantity");
                txtQuantity.Attributes.Add("class", "form-control");

                //Get product name
                string productURL = item.GetDataKeyValue("ProductURL").ToString();
                int productZoneID = Convert.ToInt32(item.GetDataKeyValue("ProductZoneID"));
                string productName = item.GetDataKeyValue("ProductName").ToString();
                litProductName.Text = string.Format("<a href='{0}'>{1}</a>", ProductHelper.FormatProductUrl(productURL, productID, productZoneID), productName);

                //Get store name
                int storeID = 0;
                if (item.GetDataKeyValue("StoreID").ToString() != "-1")
                    storeID = Convert.ToInt32(item.GetDataKeyValue("StoreID"));
                else
                    Int32.TryParse(ddStore.SelectedValue, out storeID);
                Store store = lstStores.Where(x => x.StoreID == storeID).FirstOrDefault();
                if (store != null && store.StoreID > 0)
                {
                    litStoreName.Text = store.Name;
                }

                //Empty non-inputted items quantity
                if (item.GetDataKeyValue("StoreID").ToString() == "-1")
                {
                    txtQuantity.Text = string.Empty;
                }

                //Color stockout items
                Int32.TryParse(txtQuantity.Text, out int quantity);
                if (quantity < 1)
                {
                    txtQuantity.Attributes.Add("class", "form-control stockout");
                }

                //Highlight inputted item
                int inventoryID = Convert.ToInt32(item.GetDataKeyValue("InventoryID"));
                if (inventoryID > 0)
                    item.Attributes["style"] = "background-color:lightyellow !important;";
                else if (inventoryID == -1)
                    item.Attributes["style"] = "background-color:white !important;";

                //Default IsPublished = true for non-inputted item
                if (displayMode == 0)
                {
                    CheckBox cbPublish = (CheckBox)item.FindControl("cbPublish");
                    cbPublish.Checked = true;
                }
                else if (displayMode == 2)
                {
                    if (inventoryID < 1)
                    {
                        CheckBox cbPublish = (CheckBox)item.FindControl("cbPublish");
                        cbPublish.Checked = true;
                    }
                }
            }
        }

        #endregion TreeList
    }
}

namespace CanhCam.Web.StoreUI
{
    public class StockInventoryDeleted : IContentDeleted
    {
        public bool RestoreContent(string inventoryID)
        {
            //try
            //{
            //    StockInventory stockInventory = new StockInventory(Convert.ToInt32(inventoryID));

            //    if (stockInventory != null && stockInventory.InventoryID > 0)
            //    {
            //        stockInventory.IsDeleted = false;
            //        stockInventory.Save();
            //    }
            //}
            //catch (Exception) { return false; }

            //return true;
            return false;
        }

        public bool DeleteContent(string inventoryID)
        {
            try
            {
                StockInventory.Delete(Convert.ToInt32(inventoryID));
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
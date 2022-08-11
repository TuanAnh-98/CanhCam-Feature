/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-02
/// Last Modified:			2014-08-02

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.SearchIndex;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class AdminProductControl : UserControl
    {
        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(AdminProductControl));

        private RadGridSEOPersister gridPersister;

        protected SiteSettings siteSettings;
        protected CmsBasePage basePage;
        protected string siteRoot = string.Empty;

        private bool isAllowedZone = false;
        protected Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private bool canEditAnything = false;
        private bool canUpdate = false;

        private SiteUser currentUser = null;
        private string startZone = string.Empty;

        private int productType = -1;
        private string pageTitle = ProductResources.ProductListTitle;
        private string pageUrl = "/Product/AdminCP/ProductList.aspx";
        private string editPageUrl = "/Product/AdminCP/ProductEdit.aspx";

        #endregion Private Properties

        #region Public Properties

        public int ProductType
        {
            get { return productType; }
            set { productType = value; }
        }

        public string PageTitle
        {
            get { return pageTitle; }
            set { pageTitle = value; }
        }

        public string PageUrl
        {
            get { return Page.ResolveUrl(pageUrl); }
            set { pageUrl = value; }
        }

        public string EditPageUrl
        {
            get { return Page.ResolveUrl(editPageUrl); }
            set { editPageUrl = value; }
        }

        #endregion Public Properties

        #region Load

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadParams();
            LoadSettings();

            if (!ProductPermission.CanViewList)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        #endregion Load

        #region "RadGrid Event"

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.PagerStyle.EnableSEOPaging = false;

            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            int iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: sZoneId, productType: productType, keyword: txtTitle.Text.Trim(), searchProductZone: ProductConfiguration.EnableProductZone);
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;

            grid.DataSource = Product.GetPageAdv(pageNumber: startRowIndex, pageSize: maximumRows, siteId: siteSettings.SiteId, zoneIds: sZoneId, productType: productType, keyword: txtTitle.Text.Trim(), searchProductZone: ProductConfiguration.EnableProductZone);
        }

        private string sZoneId
        {
            get
            {
                if (ddZones.SelectedValue.Length > 0)
                {
                    if (ddZones.SelectedValue == "-1")
                        return "";

                    return ddZones.SelectedValue;
                }

                return "0";
            }
        }

        #endregion "RadGrid Event"

        #region Event

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ProductPermission.CanDelete)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                bool isDeleted = false;

                foreach (GridDataItem data in grid.SelectedItems)
                {
                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    Product product = new Product(siteSettings.SiteId, productId);

                    if (product != null && product.ProductId != -1 && !product.IsDeleted)
                    {
                        ContentDeleted.Create(siteSettings.SiteId, product.Title, "Product", typeof(ProductDeleted).AssemblyQualifiedName, product.ProductId.ToString(), Page.User.Identity.Name);

                        product.IsDeleted = true;

                        product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                        product.SaveDeleted();
                        LogActivity.Write("Delete product", product.Title);

                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    SiteUtils.QueueIndexing();
                    grid.Rebind();

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["ProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ProductPermission.CanUpdate)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                bool isUpdated = false;
                foreach (GridDataItem data in grid.Items)
                {
                    TextBox txtDisplayOrder = (TextBox)data.FindControl("txtDisplayOrder");
                    TextBox txtViewCount = (TextBox)data.FindControl("txtViewCount");
                    TextBox txtProductCode = (TextBox)data.FindControl("txtProductCode");
                    TextBox txtPrice = (TextBox)data.FindControl("txtPrice");
                    TextBox txtOldPrice = (TextBox)data.FindControl("txtOldPrice");
                    CheckBox cbPublished = (CheckBox)data.FindControl("cbPublished");

                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    int displayOrder = Convert.ToInt32(data.GetDataKeyValue("DisplayOrder"));
                    int viewCount = Convert.ToInt32(data.GetDataKeyValue("ViewCount"));
                    string code = data.GetDataKeyValue("Code").ToString();
                    decimal price = Convert.ToDecimal(data.GetDataKeyValue("Price"));
                    decimal oldPrice = Convert.ToDecimal(data.GetDataKeyValue("OldPrice"));
                    bool isPublished = Convert.ToBoolean(data.GetDataKeyValue("IsPublished"));

                    int displayOrderNew = displayOrder;
                    int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                    int viewCountNew = viewCount;
                    int.TryParse(txtViewCount.Text, out viewCountNew);

                    decimal priceNew = price;
                    decimal.TryParse(txtPrice.Text, out priceNew);

                    decimal oldPriceNew = oldPrice;
                    decimal.TryParse(txtOldPrice.Text, out oldPriceNew);

                    if (
                        displayOrder != displayOrderNew
                        || viewCount != viewCountNew
                        || txtProductCode.Text.Trim() != code.Trim()
                        || priceNew != price
                        || oldPriceNew != oldPrice
                        || (cbPublished != null && cbPublished.Checked != isPublished)
                        )
                    {
                        Product product = new Product(siteSettings.SiteId, productId);
                        if (product != null && product.ProductId != -1)
                        {
                            product.DisplayOrder = displayOrderNew;
                            product.ViewCount = viewCountNew;
                            product.Code = txtProductCode.Text.Trim();
                            product.Price = priceNew;
                            product.OldPrice = oldPriceNew;
                            product.IsPublished = cbPublished.Checked;
                            product.Save();

                            LogActivity.Write("Update product", product.Title);

                            isUpdated = true;
                        }
                    }
                }

                if (isUpdated)
                {
                    grid.Rebind();

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
            grid.CurrentPageIndex = 0;
            grid.Rebind();
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetProductsForExport();

            string fileName = "products-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        private void BtnExportCategory_Click(object sender, EventArgs e)
        {
            DataTable dt = GetCategoriesForExport();

            string fileName = "categories-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        private DataTable GetCategoriesForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("Id", typeof(string));
            dt.Columns.Add("NameExpand", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            var siteMapDataSource = new SiteMapDataSource
            {
                SiteMapProvider = "canhcamsite" + siteSettings.SiteId.ToInvariantString()
            };
            var startingNode = siteMapDataSource.Provider.RootNode;
            var allNodes = startingNode.GetAllNodes();
            int i = 1;
            int languageId = WorkingCulture.LanguageId;
            foreach (SiteMapNode childNode in allNodes)
            {
                gbSiteMapNode gbNode = childNode as gbSiteMapNode;
                if (gbNode == null) { continue; }
                if (!string.IsNullOrEmpty(gbNode.FeatureGuids)
                    && gbNode.FeatureGuids.Split(';').Contains(Product.FeatureGuid.ToString()))
                {
                    DataRow row = dt.NewRow();
                    row["Number"] = i;
                    row["Id"] = gbNode.ZoneId;
                    row["NameExpand"] = gbNode.TitleExpand;
                    row["Name"] = GetZoneTitle(gbNode, languageId);
                    i += 1;
                    dt.Rows.Add(row);
                }
            }
            return dt;
        }

        private string GetZoneTitle(gbSiteMapNode mapNode, int languageId)
        {
            if (mapNode == null)
                return string.Empty;

            string title = mapNode.Title;
            if (languageId > 0 && mapNode["Title" + languageId.ToString()] != null)
                title = mapNode["Title" + languageId.ToString()];

            return title;
        }

        private DataTable GetProductsForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("ItemNo", typeof(string));
            dt.Columns.Add("BarCode", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("ProductUrl", typeof(string));
            dt.Columns.Add("CatalogName", typeof(string));
            dt.Columns.Add("CatalogId", typeof(string));
            dt.Columns.Add("CatalogOtherNames", typeof(string));
            dt.Columns.Add("CatalogOtherIds", typeof(string));
            dt.Columns.Add("ManufacturerName", typeof(string));
            dt.Columns.Add("BriefContent", typeof(string));
            dt.Columns.Add("FullContent", typeof(string));
            dt.Columns.Add("Images", typeof(string));
            dt.Columns.Add("Price", typeof(string));
            dt.Columns.Add("OldPrice", typeof(string));
            dt.Columns.Add("IsPublished", typeof(bool));
            dt.Columns.Add("EnableBuyButton", typeof(bool));
            dt.Columns.Add("LastUpdateDate", typeof(string));
            dt.Columns.Add("StartDate", typeof(string));
            dt.Columns.Add("ShowOptions", typeof(string));
            dt.Columns.Add("BuyWithItemNos", typeof(string));
            dt.Columns.Add("DisplayOrder", typeof(int));
            dt.Columns.Add("Weight", typeof(decimal));

            var products = Product.GetPageAdv(siteId: siteSettings.SiteId, zoneIds: sZoneId, productType: productType, keyword: txtTitle.Text.Trim(), searchProductZone: ProductConfiguration.EnableProductZone);
            int i = 1;
            var options = EnumDefined.LoadFromConfigurationXml("product", "showoption", "value");
            foreach (Product product in products)
            {
                if (ConfigHelper.GetBoolProperty("DisableExportProductNonCode", true)
                    && (string.IsNullOrEmpty(product.Code) || string.IsNullOrEmpty(product.BarCode))) continue;

                var zones = ProductZone.SelectAllByProductID(product.ProductId);
                

                DataRow row = dt.NewRow();
                row["Number"] = i;
                row["ItemNo"] = product.Code;
                row["BarCode"] = product.BarCode;
                row["ProductName"] = product.Title;
                row["ProductUrl"] = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);
                row["CatalogName"] = GetCatalogName(product);
                row["CatalogId"] = GetCatalogId(product);
                row["CatalogOtherNames"] = GetCatalogOtherNames(product, zones);
                row["CatalogOtherIds"] = GetCatalogOtherIds(product, zones);
                row["ManufacturerName"] = GetManufacturerName(product);
                row["BriefContent"] = product.BriefContent;
                row["FullContent"] = product.FullContent;
                row["Images"] = GetImages(product);
                row["Price"] = ProductHelper.FormatPrice(product.Price);
                row["OldPrice"] = ProductHelper.FormatPrice(product.OldPrice);
                row["IsPublished"] = product.IsPublished;
                row["EnableBuyButton"] = !product.DisableBuyButton;
                row["LastUpdateDate"] = product.LastModUtc.ToString("dd-MM-yyyy hh:mm");
                row["StartDate"] = product.StartDate.ToString("dd-MM-yyyy hh:mm");
                row["ShowOptions"] = GetProductShowOptions(product, options);
                row["BuyWithItemNos"] = GetProductRelatedItemNos(product);
                row["DisplayOrder"] = product.DisplayOrder;
                row["Weight"] = product.Weight;
                i += 1;
                dt.Rows.Add(row);
            }

            return dt;
        }

        private string GetProductShowOptions(Product product, List<EnumDefined> options)
        {
            string result = string.Empty;
            string spec = "";
            foreach (var option in options)
            {
                if ((product.ShowOption & Convert.ToInt32(option.Value)) > 0)
                {
                    result += spec + option.Name;
                    spec = Environment.NewLine;
                }
            }
            return result;
        }

        private string GetProductRelatedItemNos(Product product)
        {
            var items = RelatedItem.GetByItem(product.ProductGuid);
            if (items.Count == 0)
                return string.Empty;
            var products = Product.GetByGuids(product.SiteId, string.Join(";", items.Select(it => it.ItemGuid2)));

            return string.Join(Environment.NewLine, products.Select(it => it.Code));
        }

        #endregion Event

        #region Helper Export

        private string GetImages(Product product)
        {
            StringBuilder builder = new StringBuilder();
            var imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
            var medias = ContentMedia.GetByContentDesc(product.ProductGuid);
            string spec = "";
            foreach (ContentMedia media in medias)
            {
                string relativePath = siteRoot + ProductHelper.GetMediaFilePath(imageFolderPath, media.MediaFile);
                builder.Append(spec + relativePath);
                spec = ";";
            }

            return builder.ToString();
        }

        private string GetManufacturerName(Product product)
        {
            if (product.ManufacturerId == -1)
                return string.Empty;
            var manu = ManufacturerCacheHelper.GetById(product.ManufacturerId);
            if (manu != null)
                return manu.Name;
            return string.Empty;
        }

        private string GetCatalogName(Product product)
        {
            if (product.ZoneId == -1) return string.Empty;

            var zone = SiteUtils.GetSiteMapNodeByZoneId(product.ZoneId);
            if (zone != null)
                return zone.TitleExpand;

            return string.Empty;
        }

        private string GetCatalogId(Product product)
        {
            if (product.ZoneId == -1) return string.Empty;

            var zone = SiteUtils.GetSiteMapNodeByZoneId(product.ZoneId);
            if (zone != null)
                return zone.ZoneId.ToString();

            return string.Empty;
        }

        private string GetCatalogOtherNames(Product product, List<ProductZone> zones)
        {
            if (zones.Count == 0) return string.Empty;

            string result = string.Empty;
            foreach (ProductZone item in zones)
            {
                if (item.ZoneID == product.ZoneId) continue;
                var zone = SiteUtils.GetSiteMapNodeByZoneId(product.ZoneId);
                if (zone != null)
                    result += zone.Title + Environment.NewLine;
            }
            return result;
        }


        private string GetCatalogOtherIds(Product product, List<ProductZone> zones)
        {
            if (zones.Count == 0) return string.Empty;

            string result = string.Empty;
            foreach (ProductZone item in zones)
            {
                if (item.ZoneID == product.ZoneId) continue;
                var zone = SiteUtils.GetSiteMapNodeByZoneId(product.ZoneId);
                if (zone != null)
                    result += zone.ZoneId + Environment.NewLine;
            }
            return result;
        }

        #endregion Helper Export

        #region Protected methods

        protected bool CanEditProduct(int userId, bool isPublished, object oStateId)
        {
            // Should be check permission zone???
            if (canUpdate)
                return true;

            //if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow)
            //{
            //    if (oStateId != null)
            //    {
            //        int stateId = Convert.ToInt32(oStateId);

            //        if (stateId == firstWorkflowStateId)
            //        {
            //            if (currentUser == null) { return false; }
            //            return (userId == currentUser.UserId);
            //        }
            //    }
            //}

            return false;
        }

        #endregion Protected methods

        #region Populate

        private void PopulateLabels()
        {
            Page.Title = SiteUtils.FormatPageTitle(siteSettings, pageTitle);
            heading.Text = pageTitle;

            UIHelper.DisableButtonAfterClick(
                btnUpdate,
                ProductResources.ButtonDisabledPleaseWait,
                Page.ClientScript.GetPostBackEventReference(this.btnUpdate, string.Empty)
                );

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductDeleteMultiWarning);
        }

        private void PopulateControls()
        {
            PopulateZoneList();
        }

        #endregion Populate

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);

            if (canEditAnything)
                ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), "-1"));

            if (startZone.Length > 0)
            {
                ListItem li = ddZones.Items.FindByValue(startZone);
                if (li != null)
                {
                    ddZones.ClearSelection();
                    li.Selected = true;
                }
            }
        }

        #endregion Populate Zone List

        #region LoadSettings

        private void LoadSettings()
        {
            canUpdate = ProductPermission.CanUpdate;

            lnkInsert.Visible = ProductPermission.CanCreate;
            btnUpdate.Visible = canUpdate;
            btnDelete.Visible = ProductPermission.CanDelete;

            currentUser = SiteUtils.GetCurrentSiteUser();
            isAllowedZone = WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor();

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            basePage = Page as CmsBasePage;
            siteRoot = basePage.SiteRoot;

            canEditAnything = WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor();

            breadcrumb.CurrentPageTitle = pageTitle;
            breadcrumb.CurrentPageUrl = pageUrl;

            lnkInsert.NavigateUrl = siteRoot + EditPageUrl;
            if (ddZones.SelectedValue != "-1" && ddZones.SelectedValue.Length > 0)
                lnkInsert.NavigateUrl = siteRoot + EditPageUrl + "?start=" + ddZones.SelectedValue;
            else
            {
                if (startZone.Length > 0)
                    lnkInsert.NavigateUrl = siteRoot + EditPageUrl + "?start=" + startZone.ToString();
            }

            var column = grid.MasterTableView.Columns.FindByUniqueName("ProductCode");
            if (column != null)
                column.Visible = displaySettings.ShowProductCode;

            column = grid.MasterTableView.Columns.FindByUniqueName("Price");
            if (column != null)
                column.Visible = displaySettings.ShowPrice;

            column = grid.MasterTableView.Columns.FindByUniqueName("OldPrice");
            if (column != null)
                column.Visible = displaySettings.ShowOldPrice;
        }

        #endregion LoadSettings

        #region LoadParams

        private void LoadParams()
        {
            startZone = WebUtils.ParseStringFromQueryString("start", startZone);

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
        }

        #endregion LoadParams

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnExport.Click += new EventHandler(BtnExport_Click);
            btnExportCategory.Click += new EventHandler(BtnExportCategory_Click);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);

            gridPersister = new RadGridSEOPersister(grid);
        }

        #endregion OnInit
    }
}

namespace CanhCam.Web.ProductUI
{
    public class ProductDeleted : IContentDeleted
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductDeleted));

        public bool RestoreContent(string productId)
        {
            try
            {
                var siteSettings = CacheHelper.GetCurrentSiteSettings();
                var product = new Product(siteSettings.SiteId, Convert.ToInt32(productId));

                if (product != null && product.ProductId > 0)
                {
                    product.IsDeleted = false;

                    product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                    product.SaveDeleted();

                    SiteUtils.QueueIndexing();
                }
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool DeleteContent(string productId)
        {
            try
            {
                var siteSettings = CacheHelper.GetCurrentSiteSettings();
                var product = new Product(siteSettings.SiteId, Convert.ToInt32(productId));

                if (product != null && product.ProductId != -1)
                {
                    ProductHelper.DeleteFolder(siteSettings.SiteId, product.ProductId);

                    ContentMedia.DeleteByContent(product.ProductGuid);
                    ShoppingCartItem.DeleteByProduct(product.ProductId);

                    var listAtributes = ContentAttribute.GetByContentAsc(product.ProductGuid);
                    foreach (ContentAttribute item in listAtributes)
                        ContentLanguage.DeleteByContent(item.Guid);

                    ContentAttribute.DeleteByContent(product.ProductGuid);
                    ContentLanguage.DeleteByContent(product.ProductGuid);
                    ProductProperty.DeleteByProduct(product.ProductId);
                    FriendlyUrl.DeleteByPageGuid(product.ProductGuid);
                    ProductComment.DeleteByProduct(product.ProductId);
                    TagItem.DeleteByItem(product.ProductGuid);
                    FileAttachment.DeleteByItem(product.ProductGuid);
                    RelatedItem.DeleteByItem(product.ProductGuid);
                    ProductZone.DeleteByProduct(product.ProductId);

                    product.Delete();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);

                return false;
            }

            return true;
        }

        private void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            var indexBuilder = IndexBuilderManager.Providers["ProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }
    }
}
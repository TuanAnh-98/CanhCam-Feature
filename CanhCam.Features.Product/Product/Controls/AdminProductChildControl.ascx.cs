using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.SearchIndex;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class AdminProductChildControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AdminProductChildControl));

        #region Protected Properties

        protected SiteSettings siteSettings;
        protected CmsBasePage basePage;
        protected string siteRoot = string.Empty;
        protected Double timeOffset = 0;
        protected string imageFolderPath;
        protected string thumbnailImageFolderPath;

        #endregion Protected Properties

        #region Private Properties

        private bool isAllowedZone = false;
        private TimeZoneInfo timeZone = null;
        private bool canEditAnything = false;
        private bool canUpdate = false;

        private SiteUser currentUser = null;
        private string startZone = string.Empty;
        private Product product = null;
        private int productType = -1;
        private string pageTitle = ProductResources.ProductListTitle;
        private string pageUrl = "/Product/AdminCP/ProductList.aspx";
        private string editPageUrl = "/Product/AdminCP/ProductEdit.aspx";
        private int productId = -1;
        private List<ProductProperty> productProperties = new List<ProductProperty>();
        private List<CustomField> lstCustomFields = new List<CustomField>();
        private List<CustomFieldOption> lstColorOptions = new List<CustomFieldOption>();
        private IFileSystem fileSystem = null;
        private List<ProductProperty> listProductProperties = new List<ProductProperty>();

        #endregion Private Properties

        #region Public Properties

        public List<CustomField> CustomFields
        {
            get { return lstCustomFields; }
            set { lstCustomFields = value; }
        }

        public List<CustomFieldOption> ColorOptions
        {
            get { return lstColorOptions; }
            set { lstColorOptions = value; }
        }

        public List<ProductProperty> ProductProperties
        {
            get { return productProperties; }
            set { productProperties = value; }
        }

        public int ProductId
        {
            get { return productId; }
            set { productId = value; }
        }

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

        private void BtnAddProductChild_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtChildProductName.Text))
            {
                FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                if (p != null) { fileSystem = p.GetFileSystem(); }
                currentUser = SiteUtils.GetCurrentSiteUser();
                product = new Product(siteSettings.SiteId, productId);
                if (product == null || product.ProductId <= 0) return;
                int iProductId = product.ProductId;
                Guid productGuid = product.ProductGuid;
                Product childProduct = product;
                childProduct.ParentId = iProductId;
                childProduct.ProductId = -1;
                childProduct.ProductGuid = Guid.Empty;
                childProduct.StartDate = DateTime.UtcNow;
                childProduct.EndDate = DateTime.MaxValue;
                childProduct.CreatedUtc = DateTime.UtcNow;
                childProduct.LastModUtc = DateTime.UtcNow;
                childProduct.UserGuid = currentUser.UserGuid;
                childProduct.LastModUserGuid = currentUser.UserGuid;
                childProduct.ViewCount = 0;
                childProduct.DisplayOrder = 0;
                childProduct.CommentCount = 0;
                childProduct.Title = txtChildProductName.Text.Trim();
                childProduct.IsPublished = chkChildProductPublished.Checked;
                if (divSubTitle.Visible)
                    childProduct.SubTitle = txtSubTitle.Text;
                if (divProductCode.Visible)
                    childProduct.Code = txtProductCode.Text;
                if (divPrice.Visible)
                {
                    decimal.TryParse(txtPrice.Text, out decimal price);
                    childProduct.Price = price;
                }
                if (divOldPrice.Visible)
                {
                    decimal.TryParse(txtOldPrice.Text, out decimal oldprice);
                    childProduct.OldPrice = oldprice;
                }
                if (divStockQuantity.Visible)
                {
                    int.TryParse(txtStockQuantity.Text, out int stockQuantity);
                    childProduct.StockQuantity = stockQuantity;
                }

                string childUrl = SiteUtils.SuggestFriendlyUrl(childProduct.Title, siteSettings);
                var friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(childUrl);
                if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                    friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

                childProduct.Url = "~/" + friendlyUrlString;

                childProduct.ContentChanged += new ContentChangedEventHandler(Product_ContentChanged);

                #region Save

                if (childProduct.Save())
                {
                    //Copy zones
                    List<ProductZone> parentZones = ProductZone.SelectAllByProductID(iProductId);
                    foreach (ProductZone parentZone in parentZones)
                    {
                        ProductZone childZone = new ProductZone
                        {
                            ProductID = childProduct.ProductId,
                            ZoneID = parentZone.ZoneID
                        };
                        childZone.Save();
                    }

                    var friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);
                    if (!friendlyUrl.FoundFriendlyUrl)
                    {
                        if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
                        {
                            FriendlyUrl newFriendlyUrl = new FriendlyUrl
                            {
                                SiteId = siteSettings.SiteId,
                                SiteGuid = siteSettings.SiteGuid,
                                PageGuid = childProduct.ProductGuid,
                                Url = friendlyUrlString,
                                RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                + childProduct.ZoneId.ToInvariantString()
                                + "&ProductID=" + childProduct.ProductId.ToInvariantString()
                            };

                            newFriendlyUrl.Save();
                        }
                    }

                    ////Copy properties
                    SaveProductProperty(childProduct);

                    //Copy attributes
                    var lstProductAttributes = ContentAttribute.GetByContentAsc(productGuid);
                    if (lstProductAttributes.Count > 0)
                    {
                        var lstProductLanguages = ContentLanguage.GetByContent(productGuid);

                        foreach (ContentAttribute attribute in lstProductAttributes)
                        {
                            var attributeGuid = attribute.Guid;
                            ContentAttribute childAttribute = attribute;
                            childAttribute.Guid = Guid.Empty;
                            childAttribute.ContentGuid = childProduct.ProductGuid;
                            if (childAttribute.Save())
                            {
                                lstProductLanguages = ContentLanguage.GetByContent(attributeGuid);
                                foreach (ContentLanguage content in lstProductLanguages)
                                {
                                    ContentLanguage childContent = content;
                                    childContent.Guid = Guid.Empty;
                                    childContent.ContentGuid = childAttribute.Guid;
                                    childContent.Save();
                                }
                            }
                        }
                    }

                    //Copy images
                    if (chkChildProductChildImages.Checked)
                    {
                        var lstProductMedia = ContentMedia.GetByContentAsc(productGuid);
                        if (lstProductMedia.Count > 0)
                        {
                            imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, iProductId);
                            var childImageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, childProduct.ProductId);

                            ProductHelper.VerifyProductFolders(fileSystem, childImageFolderPath);

                            foreach (ContentMedia medium in lstProductMedia)
                            {
                                ContentMedia childContent = medium;
                                childContent.Guid = Guid.Empty;
                                childContent.ContentGuid = childProduct.ProductGuid;
                                if (childContent.Save() && medium.MediaType != (int)ProductMediaType.Video)
                                {
                                    if (fileSystem.FileExists(imageFolderPath + medium.MediaFile))
                                        fileSystem.CopyFile(imageFolderPath + medium.MediaFile, childImageFolderPath + medium.MediaFile, false);
                                    if (fileSystem.FileExists(imageFolderPath + "thumbs/" + medium.ThumbnailFile))
                                        fileSystem.CopyFile(imageFolderPath + "thumbs/" + medium.ThumbnailFile, childImageFolderPath + "thumbs/" + medium.ThumbnailFile, false);
                                }
                            }
                        }
                    }

                    LogActivity.Write("Copy product", childProduct.Title);

                    grid.Rebind();

                    //if (ConfigHelper.GetBoolProperty("Product:EnableSaveProductColor", false))
                    //{
                    //    product.ProductColors = ProductHelper.GenerateProductColorImagesJson(product);
                    //    product.UpdateProductColors();
                    //}
                }

                #endregion Save
            }
        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            if (lstCustomFields != null)
            {
                rptCustomFields.DataSource = lstCustomFields;
                rptCustomFields.DataBind();
                rptGenarationProduct.DataSource = lstCustomFields.Where(c => ((c.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0));
                rptGenarationProduct.DataBind();
            }

            txtChildProductName.Text = "Sub " + product.Title;
            txtProductCode.Text = product.Code;
            txtPrice.Text = ProductHelper.FormatPrice(product.Price);
            txtOldPrice.Text = ProductHelper.FormatPrice(product.OldPrice);
            txtStockQuantity.Text = product.StockQuantity.ToString();
        }

        private void RptCustomFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var customFieldId = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdfCustomFieldId")).Value);
                var dataType = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdfDataType")).Value);
                var txtField = (TextBox)e.Item.FindControl("txtField");
                var ddlField = (DropDownList)e.Item.FindControl("ddlField");
                var cblField = (CheckBoxList)e.Item.FindControl("cblField");

                switch (dataType)
                {
                    case (int)CustomFieldDataType.Text:
                        txtField.Visible = true;
                        ddlField.Visible = false;
                        cblField.Visible = false;

                        foreach (ProductProperty property in productProperties)
                        {
                            if (property.CustomFieldId == customFieldId)
                            {
                                txtField.Text = property.CustomValue;
                                break;
                            }
                        }
                        break;

                    case (int)CustomFieldDataType.SelectBox:
                        txtField.Visible = false;
                        cblField.Visible = false;

                        ddlField.Items.Clear();
                        ddlField.Items.Add(new ListItem(ProductResources.CustomFieldSelectLabel, ""));
                        ddlField.DataSource = CustomFieldOption.GetByCustomField(customFieldId);
                        ddlField.DataBind();

                        foreach (ProductProperty property in productProperties)
                        {
                            ListItem li = ddlField.Items.FindByValue(property.CustomFieldOptionId.ToString());
                            if (li != null)
                            {
                                ddlField.ClearSelection();
                                li.Selected = true;
                                break;
                            }
                        }

                        if (ddlField.Items.Count > 1)
                            ddlField.Visible = true;

                        break;

                    case (int)CustomFieldDataType.CheckBox:
                        txtField.Visible = false;
                        ddlField.Visible = false;

                        var lstCustomFieldOptions = CustomFieldOption.GetByCustomField(customFieldId);

                        var rptCustomFieldOptions = (Repeater)e.Item.FindControl("rptCustomFieldOptions");
                        if (rptCustomFieldOptions != null && ProductConfiguration.EnableAttributesPriceAdjustment)
                        {
                            var enableShoppingCart = (Convert.ToInt32(((HiddenField)e.Item.FindControl("hdfOptions")).Value) & (int)CustomFieldOptions.EnableShoppingCart) > 0;

                            cblField.Visible = false;
                            rptCustomFieldOptions.Visible = true;
                            rptCustomFieldOptions.DataSource = lstCustomFieldOptions;
                            rptCustomFieldOptions.DataBind();

                            foreach (RepeaterItem rptItem in rptCustomFieldOptions.Items)
                            {
                                CheckBox cbxField = (CheckBox)rptItem.FindControl("cbxField");
                                TextBox txtOverriddenPrice = (TextBox)rptItem.FindControl("txtOverriddenPrice");
                                HiddenField hdfCustomFieldOptionId = (HiddenField)rptItem.FindControl("hdfCustomFieldOptionId");

                                txtOverriddenPrice.Visible = enableShoppingCart;
                                txtOverriddenPrice.Attributes["placeholder"] = ResourceHelper.GetResourceString("ProductResources", "ProductEditPriceAdjustmentLabel");

                                foreach (ProductProperty property in productProperties)
                                {
                                    if (property.CustomFieldOptionId.ToString() == hdfCustomFieldOptionId.Value)
                                    {
                                        cbxField.Checked = true;
                                        txtOverriddenPrice.Text = ProductHelper.FormatPrice(property.OverriddenPrice);

                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            cblField.DataSource = lstCustomFieldOptions;
                            cblField.DataBind();
                            cblField.ClearSelection();

                            foreach (ProductProperty property in productProperties)
                            {
                                ListItem li = cblField.Items.FindByValue(property.CustomFieldOptionId.ToString());
                                if (li != null)
                                    li.Selected = true;
                            }

                            if (cblField.Items.Count > 0)
                                cblField.Visible = true;
                        }

                        break;
                }
            }
        }

        private void RptGenarationProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var customFieldId = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdfCustomFieldId")).Value);
                var dataType = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdfDataType")).Value);
                var txtField = (TextBox)e.Item.FindControl("txtField");
                var ddlField = (DropDownList)e.Item.FindControl("ddlField");
                var cblField = (CheckBoxList)e.Item.FindControl("cblField");

                switch (dataType)
                {
                    case (int)CustomFieldDataType.Text:
                        txtField.Visible = true;
                        ddlField.Visible = false;
                        cblField.Visible = false;

                        foreach (ProductProperty property in productProperties)
                        {
                            if (property.CustomFieldId == customFieldId)
                            {
                                txtField.Text = property.CustomValue;
                                break;
                            }
                        }
                        break;

                    case (int)CustomFieldDataType.SelectBox:
                    case (int)CustomFieldDataType.CheckBox:
                        txtField.Visible = false;
                        ddlField.Visible = false;

                        var lstCustomFieldOptions = CustomFieldOption.GetByCustomField(customFieldId);

                        var rptCustomFieldOptions = (Repeater)e.Item.FindControl("rptGenarationCustomFieldOptions");
                        if (rptCustomFieldOptions != null && ProductConfiguration.EnableAttributesPriceAdjustment)
                        {
                            var enableShoppingCart = (Convert.ToInt32(((HiddenField)e.Item.FindControl("hdfOptions")).Value) & (int)CustomFieldOptions.EnableShoppingCart) > 0;

                            cblField.Visible = false;
                            rptCustomFieldOptions.Visible = true;
                            rptCustomFieldOptions.DataSource = lstCustomFieldOptions;
                            rptCustomFieldOptions.DataBind();

                            foreach (RepeaterItem rptItem in rptCustomFieldOptions.Items)
                            {
                                CheckBox cbxField = (CheckBox)rptItem.FindControl("cbxField");
                                TextBox txtOverriddenPrice = (TextBox)rptItem.FindControl("txtOverriddenPrice");
                                HiddenField hdfCustomFieldOptionId = (HiddenField)rptItem.FindControl("hdfCustomFieldOptionId");

                                txtOverriddenPrice.Visible = enableShoppingCart;
                                txtOverriddenPrice.Attributes["placeholder"] = ResourceHelper.GetResourceString("ProductResources", "ProductEditPriceAdjustmentLabel");

                                foreach (ProductProperty property in productProperties)
                                {
                                    if (property.CustomFieldOptionId.ToString() == hdfCustomFieldOptionId.Value)
                                    {
                                        cbxField.Checked = true;
                                        txtOverriddenPrice.Text = ProductHelper.FormatPrice(property.OverriddenPrice);

                                        break;
                                    }
                                }
                            }
                        }
                        else
                        {
                            cblField.DataSource = lstCustomFieldOptions;
                            cblField.DataBind();
                            cblField.ClearSelection();

                            foreach (ProductProperty property in productProperties)
                            {
                                ListItem li = cblField.Items.FindByValue(property.CustomFieldOptionId.ToString());
                                if (li != null)
                                    li.Selected = true;
                            }

                            if (cblField.Items.Count > 0)
                                cblField.Visible = true;
                        }

                        break;
                }
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bool isApplied = false;
            int iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, parentId: productId);
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;
            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            var lstProduct = Product.GetPageAdv(pageNumber: startRowIndex, pageSize: maximumRows, siteId: siteSettings.SiteId, parentId: productId);
            listProductProperties = ProductProperty.GetPropertiesByProducts(lstProduct.Select(p => p.ProductId).ToList());
            grid.DataSource = lstProduct;
        }

        private void PopulateLabels()
        {
            litContentTab.Text = ProductResources.ContentTab;
            if (CustomFields != null)
            {
                liCustomField.Visible = true;
                tabCustomField.Visible = true;
                litCustomFieldTab.Text = "<a aria-controls='" + tabCustomField.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabCustomField.ClientID + "' class='nav-link'>" + ProductResources.ProductPropertiesTab + "</a>";
            }
            divProductCode.Visible = displaySettings.ShowProductCode;
            divPrice.Visible = displaySettings.ShowPrice;
            divOldPrice.Visible = displaySettings.ShowOldPrice;
            divSubTitle.Visible = displaySettings.ShowSubTitle;
            divStockQuantity.Visible = displaySettings.ShowStockQuantity;

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductDeleteWarning);
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            canUpdate = ProductPermission.CanUpdate;

            product = new Product(siteSettings.SiteId, productId);
            if (product != null && product.ParentId == 0)
            {
                btnAddChild.Visible = true;
                btnAddChild.Attributes["data-target"] = "#" + pnlAddProductChildModal.ClientID;
                pnlAddProductChildModal.Visible = true;

                btnAddProductChild.Click += new EventHandler(BtnAddProductChild_Click);

                btnGenarationProductChild.Visible = true;
                btnGenarationProductChild.Attributes["data-target"] = "#" + pnlGenarationProductChild.ClientID;
                pnlGenarationProductChild.Visible = true;

                btnGenarationProductChildByCustomFiled.Click += new EventHandler(BtnGenarationProductChildByCustomFiled_Click);
            }
        }

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
                        if (Product.ExistsChild(product.ProductId))
                            continue;

                        ContentDeleted.Create(siteSettings.SiteId, product.Title, "Product", typeof(ProductDeleted).AssemblyQualifiedName, product.ProductId.ToString(), Page.User.Identity.Name);

                        product.IsDeleted = true;

                        product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                        product.SaveDeleted();
                        //ProductHelper.SaveChildCodes(product);
                        LogActivity.Write("Delete product", product.Title);

                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    SiteUtils.QueueIndexing();
                    Response.Redirect(Request.RawUrl);
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

        private void BtnUpdate_Click(object sender, EventArgs e)
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
                    TextBox txtStockQuantity = (TextBox)data.FindControl("txtStockQuantity");
                    TextBox txtPrice = (TextBox)data.FindControl("txtPrice");
                    TextBox txtOldPrice = (TextBox)data.FindControl("txtOldPrice");

                    int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                    int displayOrder = Convert.ToInt32(data.GetDataKeyValue("DisplayOrder"));
                    int viewCount = Convert.ToInt32(data.GetDataKeyValue("ViewCount"));
                    string code = data.GetDataKeyValue("Code").ToString();
                    int stockQuantity = Convert.ToInt32(data.GetDataKeyValue("StockQuantity"));
                    decimal price = Convert.ToDecimal(data.GetDataKeyValue("Price"));
                    decimal oldPrice = Convert.ToDecimal(data.GetDataKeyValue("OldPrice"));

                    int displayOrderNew = displayOrder;
                    int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                    int viewCountNew = viewCount;
                    int.TryParse(txtViewCount.Text, out viewCountNew);

                    int stockQuantityNew = stockQuantity;
                    int.TryParse(txtStockQuantity.Text.Trim(), out stockQuantityNew);

                    decimal priceNew = price;
                    decimal.TryParse(txtPrice.Text, out priceNew);

                    decimal oldPriceNew = oldPrice;
                    decimal.TryParse(txtOldPrice.Text, out oldPriceNew);

                    if (
                        displayOrder != displayOrderNew
                        || viewCount != viewCountNew
                        || txtProductCode.Text.Trim() != code.Trim()
                        || stockQuantity != stockQuantityNew
                        || priceNew != price
                        || oldPriceNew != oldPrice
                        )
                    {
                        Product product = new Product(siteSettings.SiteId, productId);
                        if (product != null && product.ProductId != -1)
                        {
                            product.DisplayOrder = displayOrderNew;
                            product.ViewCount = viewCountNew;
                            product.Code = txtProductCode.Text.Trim();
                            product.StockQuantity = stockQuantityNew;
                            product.Price = priceNew;
                            product.OldPrice = oldPriceNew;
                            product.Save();

                            //ProductHelper.SaveChildCodes(product);

                            LogActivity.Write("Update product", product.Title);

                            isUpdated = true;
                        }
                    }
                }

                if (isUpdated)
                {
                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void BtnGenarationProductChildByCustomFiled_Click(object sender, EventArgs e)
        {
            var lstChildProduct = Product.GetPageAdv(siteId: siteSettings.SiteId, parentId: productId);
            listProductProperties = ProductProperty.GetPropertiesByProducts(lstChildProduct.Select(p => p.ProductId).ToList());
            List<SubProductProperty> listSubProductProperty = new List<SubProductProperty>();

            foreach (RepeaterItem item in rptGenarationProduct.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var customFieldId = Convert.ToInt32(((HiddenField)item.FindControl("hdfCustomFieldId")).Value);
                    var dataType = Convert.ToInt32(((HiddenField)item.FindControl("hdfDataType")).Value);
                    var txtField = (TextBox)item.FindControl("txtField");
                    var ddlField = (DropDownList)item.FindControl("ddlField");
                    var cblField = (CheckBoxList)item.FindControl("cblField");
                    var subProductProperty = new SubProductProperty() { CustomFieldID = customFieldId, ListCustomFieldOptionIDs = new List<int>() };
                    foreach (ListItem cbxField in cblField.Items)
                        if (cbxField.Selected)
                        {
                            int o = Convert.ToInt32(cbxField.Value);
                            subProductProperty.ListCustomFieldOptionIDs.Add(o);
                        }
                    if (subProductProperty.ListCustomFieldOptionIDs.Count > 0)
                        listSubProductProperty.Add(subProductProperty);
                }
            }
            bool updated = false;
            if (listSubProductProperty.Count > 0)
            {
                FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
                if (p != null) { fileSystem = p.GetFileSystem(); }
                currentUser = SiteUtils.GetCurrentSiteUser();
                product = new Product(siteSettings.SiteId, productId);
                Product parent = new Product(siteSettings.SiteId, productId); //in case product got changed
                if (listSubProductProperty.Count > 1)
                {
                    List<List<int>> allPropertiesCanMerge = new List<List<int>>();
                    List<CustomFieldOption> allCustomFieldOptions = new List<CustomFieldOption>();
                    //Get all Product Properties can merge
                    Dodge(listSubProductProperty.Select(sp => sp.ListCustomFieldOptionIDs).ToList(), ref allPropertiesCanMerge);
                    //Get all CustomFieldOptions
                    foreach (int customFieldId in listSubProductProperty.Select(sp => sp.CustomFieldID))
                        allCustomFieldOptions.AddRange(CustomFieldOption.GetByCustomField(customFieldId));

                    foreach (List<int> optionIds in allPropertiesCanMerge)
                    {
                        //Check exist Product have all optionIds
                        bool exist = false;
                        foreach (Product child in lstChildProduct)
                        {
                            var lstTmp = listProductProperties.Where(pp => pp.ProductId == child.ProductId);
                            List<int> tmp = new List<int>(optionIds);
                            foreach (var item in lstTmp)
                                if (tmp.Contains(item.CustomFieldOptionId))
                                    tmp.Remove(item.CustomFieldOptionId);
                            if (tmp.Count == 0)
                                exist = true;
                        }
                        //Insert if not exist
                        if (!exist)
                        {
                            //Get Custom Field Options by optionIds
                            List<CustomFieldOption> customFieldOptions = allCustomFieldOptions.Where(cfo => optionIds.Contains(cfo.CustomFieldOptionId)).ToList();
                            //Genaration Attributes Name append to Product Title
                            List<string> appendNames = new List<string>();
                            foreach (var option in customFieldOptions)
                            {
                                CustomField customField = new CustomField(option.CustomFieldId);
                                if ((customField.Options & (int)CustomFieldOptions.EnableGenerateInTitle) > 0)
                                    appendNames.Add(customField.DisplayName + ": " + option.Name);
                            }
                            string appendName = string.Join(ConfigHelper.GetStringProperty("Product:GenarationAttributesSeparatorJoinName", " - "), appendNames);

                            //Insert Product Child with Attributes Name
                            var childProduct = InsertProductChild(appendName, new Product(product.SiteId, product.ProductId));
                            //Save Product Properties
                            foreach (var option in customFieldOptions)
                            {
                                //Save Product Propertities
                                ProductProperty productProperty = new ProductProperty()
                                {
                                    ProductId = childProduct.ProductId,
                                    CustomFieldId = option.CustomFieldId,
                                    CustomFieldOptionId = option.CustomFieldOptionId,
                                };
                                productProperty.Save();
                            }
                            CopyProductParentContent(parent, childProduct);
                            updated = true;
                        }
                    }
                }
                else
                {
                    foreach (int optionId in listSubProductProperty[0].ListCustomFieldOptionIDs)
                    {
                        var existProductPropertie = listProductProperties.Where(pp => pp.CustomFieldOptionId == optionId).FirstOrDefault();
                        if (existProductPropertie == null)
                        {
                            CustomFieldOption option = new CustomFieldOption(optionId);
                            if (option != null)
                            {
                                var childProduct = InsertProductChild(option.Name, new Product(product.SiteId, product.ProductId));
                                //Save Product Propertities
                                ProductProperty productProperty = new ProductProperty()
                                {
                                    ProductId = childProduct.ProductId,
                                    CustomFieldId = option.CustomFieldId,
                                    CustomFieldOptionId = option.CustomFieldOptionId,
                                };
                                productProperty.Save();
                                CopyProductParentContent(parent, childProduct);
                                updated = true;
                            }
                        }
                    }
                }

                grid.Rebind();

                ScriptManager.RegisterClientScriptBlock(Page, GetType(), "CloseGenerateModal", "<script> alert('Inserted successfully');$('#" + pnlGenarationProductChild.ClientID + "').modal('toggle');</script>", true);
            }
        }

        private void Dodge(List<List<int>> domains, ref List<List<int>> allPropertiesCanMerge)
        {
            Fuski(domains, new List<int>(), ref allPropertiesCanMerge);
        }

        private void Fuski(List<List<int>> domains, List<int> vector, ref List<List<int>> allPropertiesCanMerge)
        {
            if (domains.Count == vector.Count)
            {
                allPropertiesCanMerge.Add(vector);
                return;
            }
            foreach (var value in domains[vector.Count])
            {
                var newVector = vector.ToList();
                newVector.Add(value);
                Fuski(domains, newVector, ref allPropertiesCanMerge);
            }
        }

        private Product InsertProductChild(string attributeName, Product product, bool isGenaration = false)
        {
            if (product == null || product.ProductId <= 0) return null;
            var parent = new Product(product.SiteId, product.ProductId);
            int iProductId = product.ProductId;
            Guid productGuid = product.ProductGuid;
            Product childProduct = product;
            childProduct.ParentId = iProductId;
            childProduct.ProductId = -1;
            childProduct.ProductGuid = Guid.Empty;
            childProduct.StartDate = DateTime.UtcNow;
            childProduct.EndDate = DateTime.MaxValue;
            childProduct.CreatedUtc = DateTime.UtcNow;
            childProduct.LastModUtc = DateTime.UtcNow;
            childProduct.UserGuid = currentUser.UserGuid;
            childProduct.LastModUserGuid = currentUser.UserGuid;
            childProduct.ViewCount = 0;
            childProduct.DisplayOrder = 0;
            childProduct.CommentCount = 0;
            childProduct.Title = ConfigHelper.GetStringProperty("ProductChild:FormatName", "{ProductName} - {AttributeName}")
                                                                .Replace("{ProductName}", product.Title)
                                                                .Replace("{AttributeName}", attributeName);
            childProduct.IsPublished = (chkChildProductPublished.Checked && !isGenaration) || (chkGenarationProductPublished.Checked && isGenaration);

            //Luggage
            childProduct.Options = product.Options;

            string childUrl = SiteUtils.SuggestFriendlyUrl(childProduct.Title, siteSettings);
            var friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(childUrl);
            if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

            childProduct.Url = "~/" + friendlyUrlString;

            childProduct.ContentChanged += new ContentChangedEventHandler(Product_ContentChanged);

            #region Save

            if (childProduct.Save())
            {
                //Copy zones
                List<ProductZone> parentZones = ProductZone.SelectAllByProductID(iProductId);
                foreach (ProductZone parentZone in parentZones)
                {
                    ProductZone childZone = new ProductZone
                    {
                        ProductID = childProduct.ProductId,
                        ZoneID = parentZone.ZoneID
                    };
                    childZone.Save();
                }

                var friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);
                if (!friendlyUrl.FoundFriendlyUrl)
                {
                    if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
                    {
                        FriendlyUrl newFriendlyUrl = new FriendlyUrl
                        {
                            SiteId = siteSettings.SiteId,
                            SiteGuid = siteSettings.SiteGuid,
                            PageGuid = childProduct.ProductGuid,
                            Url = friendlyUrlString,
                            RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                            + childProduct.ZoneId.ToInvariantString()
                            + "&ProductID=" + childProduct.ProductId.ToInvariantString()
                        };

                        newFriendlyUrl.Save();
                    }
                }

                //CopyProductParentContent(parent, childProduct);

                //Copy attributes
                var lstProductAttributes = ContentAttribute.GetByContentAsc(productGuid);
                if (lstProductAttributes.Count > 0)
                {
                    var lstProductLanguages = ContentLanguage.GetByContent(productGuid);

                    foreach (ContentAttribute attribute in lstProductAttributes)
                    {
                        var attributeGuid = attribute.Guid;
                        ContentAttribute childAttribute = attribute;
                        childAttribute.Guid = Guid.Empty;
                        childAttribute.ContentGuid = childProduct.ProductGuid;
                        if (childAttribute.Save())
                        {
                            lstProductLanguages = ContentLanguage.GetByContent(attributeGuid);
                            foreach (ContentLanguage content in lstProductLanguages)
                            {
                                ContentLanguage childContent = content;
                                childContent.Guid = Guid.Empty;
                                childContent.ContentGuid = childAttribute.Guid;
                                childContent.Save();
                            }
                        }
                    }
                }
                //Copy images
                if ((chkChildProductChildImages.Checked && !isGenaration) || (chkGenarationProductChildImages.Checked && isGenaration))
                {
                    var lstProductMedia = ContentMedia.GetByContentAsc(productGuid);
                    if (lstProductMedia.Count > 0)
                    {
                        imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, iProductId);
                        var childImageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, childProduct.ProductId);

                        ProductHelper.VerifyProductFolders(fileSystem, childImageFolderPath);

                        foreach (ContentMedia medium in lstProductMedia)
                        {
                            ContentMedia childContent = medium;
                            childContent.Guid = Guid.Empty;
                            childContent.ContentGuid = childProduct.ProductGuid;
                            if (childContent.Save() && medium.MediaType != (int)ProductMediaType.Video)
                            {
                                if (fileSystem.FileExists(imageFolderPath + medium.MediaFile))
                                    fileSystem.CopyFile(imageFolderPath + medium.MediaFile, childImageFolderPath + medium.MediaFile, false);
                                if (fileSystem.FileExists(imageFolderPath + "thumbs/" + medium.ThumbnailFile))
                                    fileSystem.CopyFile(imageFolderPath + "thumbs/" + medium.ThumbnailFile, childImageFolderPath + "thumbs/" + medium.ThumbnailFile, false);
                            }
                        }
                    }
                }

                LogActivity.Write("Copy product", childProduct.Title);
                return childProduct;
            }

            #endregion Save

            return null;
        }

        private void CopyProductParentContent(Product parent, Product product)
        {
            var contents = ContentLanguage.GetByContent(parent.ProductGuid);
            if (contents.Count == 0)
                return;
            foreach (var item in contents)
            {
                List<ProductProperty> properties = ProductProperty.GetPropertiesByProduct(product.ProductId);
                item.ContentGuid = product.ProductGuid;
                item.Guid = Guid.Empty;

                //Genaration Attributes Name append to Product Title
                List<string> appendNames = new List<string>();
                //foreach (var option in properties)
                //{
                //    CustomFieldOption op = new CustomFieldOption(option.CustomFieldOptionId);
                //    if (op != null)
                //        appendNames.Add(op.Name);
                //}
                string optionIDs = string.Empty;
                foreach (ProductProperty childProperty in properties)
                {
                    optionIDs += childProperty.CustomFieldOptionId + ";";
                }
                List<CustomFieldOption> customFieldOptions = CustomFieldOption.GetByOptionIds(siteSettings.SiteId, optionIDs, item.LanguageId);
                foreach (var option in customFieldOptions)
                {
                    CustomField customField = new CustomField(option.CustomFieldId);
                    if (customField != null && customField.CustomFieldId > 0 && ((customField.Options & (int)CustomFieldOptions.EnableGenerateInTitle) > 0))
                    {
                        CustomField customFieldLanguage = CustomField.GetActive(customField.SiteId, customField.FeatureGuid, item.LanguageId).Where(x => x.CustomFieldId == customField.CustomFieldId).FirstOrDefault();
                        if (customFieldLanguage != null && customFieldLanguage.CustomFieldId > 0 && ((customFieldLanguage.Options & (int)CustomFieldOptions.EnableGenerateInTitle) > 0))
                            appendNames.Add(customFieldLanguage.DisplayName + ": " + option.Name);
                    }
                }

                string appendName = string.Join(ConfigHelper.GetStringProperty("Product:GenarationAttributesSeparatorJoinName", "-"), appendNames);

                item.Title = ConfigHelper.GetStringProperty("ProductChild:FormatName", "{ProductName}-{AttributeName}")
                                                                    .Replace("{ProductName}", item.Title)
                                                                    .Replace("{AttributeName}", appendName);

                string childUrl = SiteUtils.SuggestFriendlyUrl(item.Title, siteSettings);
                var friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(childUrl);
                if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                    friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

                item.Url = "~/" + friendlyUrlString;

                if (item.Save())
                {
                    var friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);
                    if (!friendlyUrl.FoundFriendlyUrl)
                    {
                        if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
                        {
                            FriendlyUrl newFriendlyUrl = new FriendlyUrl
                            {
                                SiteId = siteSettings.SiteId,
                                SiteGuid = siteSettings.SiteGuid,
                                PageGuid = product.ProductGuid,
                                ItemGuid = item.Guid,
                                LanguageId = item.LanguageId,
                                Url = friendlyUrlString,
                                RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                                + product.ZoneId.ToInvariantString()
                                + "&ProductID=" + product.ProductId.ToInvariantString()
                            };

                            newFriendlyUrl.Save();
                        }
                    }
                }
            }
        }

        private void SaveProductProperty(Product iProduct)
        {
            ProductProperty.DeleteByProduct(iProduct.ProductId);
            foreach (RepeaterItem item in rptCustomFields.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var customFieldId = Convert.ToInt32(((HiddenField)item.FindControl("hdfCustomFieldId")).Value);
                    var dataType = Convert.ToInt32(((HiddenField)item.FindControl("hdfDataType")).Value);
                    var txtField = (TextBox)item.FindControl("txtField");
                    var ddlField = (DropDownList)item.FindControl("ddlField");
                    var cblField = (CheckBoxList)item.FindControl("cblField");

                    switch (dataType)
                    {
                        case (int)CustomFieldDataType.Text:
                            if (txtField.Text.Trim().Length > 0)
                            {
                                ProductProperty property = new ProductProperty
                                {
                                    ProductId = iProduct.ProductId,
                                    CustomFieldId = customFieldId,
                                    CustomValue = txtField.Text.Trim()
                                };
                                property.Save();
                            }
                            break;

                        case (int)CustomFieldDataType.SelectBox:
                            if (ddlField.SelectedValue.Length > 0)
                            {
                                ProductProperty property = new ProductProperty
                                {
                                    ProductId = iProduct.ProductId,
                                    CustomFieldId = customFieldId,
                                    CustomFieldOptionId = Convert.ToInt32(ddlField.SelectedValue)
                                };
                                property.Save();
                            }
                            break;

                        case (int)CustomFieldDataType.CheckBox:
                            var rptCustomFieldOptions = (Repeater)item.FindControl("rptCustomFieldOptions");
                            if (rptCustomFieldOptions != null && ProductConfiguration.EnableAttributesPriceAdjustment)
                            {
                                var enableShoppingCart = (Convert.ToInt32(((HiddenField)item.FindControl("hdfOptions")).Value) & (int)CustomFieldOptions.EnableShoppingCart) > 0;
                                foreach (RepeaterItem rptItem in rptCustomFieldOptions.Items)
                                {
                                    CheckBox cbxField = (CheckBox)rptItem.FindControl("cbxField");
                                    TextBox txtOverriddenPrice = (TextBox)rptItem.FindControl("txtOverriddenPrice");
                                    HiddenField hdfCustomFieldOptionId = (HiddenField)rptItem.FindControl("hdfCustomFieldOptionId");

                                    if (cbxField.Checked)
                                    {
                                        ProductProperty property = new ProductProperty
                                        {
                                            ProductId = iProduct.ProductId,
                                            CustomFieldId = customFieldId,
                                            CustomFieldOptionId = Convert.ToInt32(hdfCustomFieldOptionId.Value)
                                        };

                                        decimal overriddenPrice = 0;
                                        if (enableShoppingCart)
                                            decimal.TryParse(txtOverriddenPrice.Text.Trim(), out overriddenPrice);
                                        property.OverriddenPrice = overriddenPrice;

                                        property.Save();
                                    }
                                }
                            }
                            else
                                foreach (ListItem li in cblField.Items)
                                {
                                    if (li.Selected)
                                    {
                                        ProductProperty property = new ProductProperty
                                        {
                                            ProductId = iProduct.ProductId,
                                            CustomFieldId = customFieldId,
                                            CustomFieldOptionId = Convert.ToInt32(li.Value)
                                        };
                                        property.Save();
                                    }
                                }

                            break;
                    }
                }
            }
        }

        private void Product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["ProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            rptCustomFields.ItemDataBound += new RepeaterItemEventHandler(RptCustomFields_ItemDataBound);
            rptGenarationProduct.ItemDataBound += new RepeaterItemEventHandler(RptGenarationProduct_ItemDataBound);
            btnUpdate.Click += new EventHandler(BtnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        #endregion OnInit

        #region Protected methods

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        protected bool CanEditProduct(int userId, bool isPublished, object oStateId)
        {
            if (canUpdate)
                return true;

            return false;
        }

        protected string GetShoppingPropertites(int productId)
        {
            if (ProductConfiguration.EnableShoppingCartAttributes && listProductProperties.Count > 0)
            {
                StringBuilder str = new StringBuilder();
                var customFields = CustomField.GetActiveByFields(siteSettings.SiteId, Product.FeatureGuid, listProductProperties.Select(p => p.CustomFieldId).Distinct().ToList());
                foreach (var a in customFields.Where(c => (c.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0))
                {
                    foreach (ProductProperty property in listProductProperties)
                    {
                        if (property.ProductId == productId && property.CustomFieldId == a.CustomFieldId)
                        {
                            str.Append("<br/>");
                            str.Append(a.Name + ": " + property.OptionName);
                        }
                    }
                }
                return str.ToString();
            }
            else
                return string.Empty;
        }

        #endregion Protected methods
    }

    public class SubProductProperty
    {
        public int CustomFieldID { get; set; }
        public List<int> ListCustomFieldOptionIDs { get; set; }
    }
}
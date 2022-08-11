using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.SearchIndex;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class AdminProductEditControl : UserControl
    {
        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(AdminProductEditControl));

        protected SiteSettings siteSettings;
        protected CmsBasePage basePage;

        protected int productId = -1;

        //protected String cacheDependencyKey;
        protected string virtualRoot;

        protected Double timeOffset = 0;
        private TimeZoneInfo timeZone = null;
        private int pageNumber = 1;
        private int pageSize = 10;
        private int totalPages = 1;
        private Product product = null;
        protected bool isAdmin = false;

        //private string defaultCommentDaysAllowed = "90";
        private ContentMetaRespository metaRepository = new ContentMetaRespository();

        protected string imageFolderPath;
        protected string thumbnailImageFolderPath;
        private IFileSystem fileSystem = null;
        private SiteUser currentUser = null;
        private bool cancelRedirect = false;

        private bool canViewList = false;
        private bool canCreate = false;
        private bool canUpdate = false;
        private bool canDelete = false;
        private string startZone = string.Empty;

        private string listPageUrl = "/Product/AdminCP/ProductList.aspx";
        private string editPageUrl = "/Product/AdminCP/ProductEdit.aspx";

        private string pageTitle = string.Empty;
        private string listPageTitle = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public string EditPageUrl
        {
            get { return Page.ResolveUrl(editPageUrl); }
            set { editPageUrl = value; }
        }

        public string ListPageUrl
        {
            get { return Page.ResolveUrl(listPageUrl); }
            set { listPageUrl = value; }
        }

        public string PageTitle
        {
            get { return pageTitle; }
            set { pageTitle = value; }
        }

        public string ListPageTitle
        {
            get { return listPageTitle; }
            set { listPageTitle = value; }
        }

        #endregion Public Properties

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();
            LoadSettings();

            if (!canCreate && !canUpdate && !canDelete)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (product != null && product.ProductId != -1)
            {
                if (!basePage.UserCanAuthorizeZone(product.ZoneId))
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                LoadCustomFields();
            }

            PopulateLabels();
            SetupScripts();
            if ((!Page.IsPostBack) && (!Page.IsCallback))
            {
                BindEnum();
                PopulateManufacturerList();
                PopulateZoneList();
                PopulateControls();
                BindAttribute();
                PopulateAttributeControls();
            }
        }

        protected virtual void PopulateControls()
        {
            PopluateProductTypeList();
            if (product != null)
            {
                BindCustomFields(product.ProductId);

                ListItem li = ddZones.Items.FindByValue(product.ZoneId.ToString());
                if (li != null)
                {
                    ddZones.ClearSelection();
                    li.Selected = true;
                }
                li = ddProductType.Items.FindByValue(product.ProductType.ToString());
                if (li != null)
                {
                    ddProductType.ClearSelection();
                    li.Selected = true;
                }

                dpBeginDate.ShowTime = true;
                if (timeZone != null)
                {
                    dpBeginDate.Text = product.StartDate.ToLocalTime(timeZone).ToString("g");
                    if (product.EndDate < DateTime.MaxValue)
                    {
                        dpEndDate.Text = product.EndDate.ToLocalTime(timeZone).ToString("g");
                    }
                }
                else
                {
                    dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(product.StartDate.AddHours(timeOffset).ToString("g"));
                    if (product.EndDate < DateTime.MaxValue)
                    {
                        dpEndDate.Text = DateTimeHelper.LocalizeToCalendar(product.EndDate.AddHours(timeOffset).ToString("g"));
                    }
                }
                txtTitle.Text = product.Title;
                txtSubTitle.Text = product.SubTitle;
                txtUrl.Text = product.Url;
                edFullContent.Text = product.FullContent;
                edBriefContent.Text = product.BriefContent;
                txtMetaDescription.Text = product.MetaDescription;
                txtMetaKeywords.Text = product.MetaKeywords;
                txtMetaTitle.Text = product.MetaTitle;
                txtAdditionalMetaTags.Text = product.AdditionalMetaTags;
                txtFileAttachment.Text = product.BarCode;

                chkIsPublished.Checked = product.IsPublished;
                chkOpenInNewWindow.Checked = product.OpenInNewWindow;
                cbEnableBuyButton.Checked = !product.DisableBuyButton;
                txtProductCode.Text = product.Code;
                txtPrice.Text = ProductHelper.FormatPrice(product.Price);
                txtOldPrice.Text = ProductHelper.FormatPrice(product.OldPrice);
                txtGroupPrice.Text = ProductHelper.FormatPrice(product.GroupPrice);

                txtStockQuantity.Text = product.StockQuantity.ToString();
                txtWeight.Text = Convert.ToDouble(product.Weight).ToString();
                txtApiProductID.Text = product.ApiProductId.ToString();
                txtRatingSum.Text = Convert.ToDouble(product.RatingSum).ToString();
                txtRatingVotes.Text = product.RatingVotes.ToString();

                if (product.ParentId > 0)//is childs
                {
                    var parent = new Product(siteSettings.SiteId, product.ParentId);
                    if (parent != null && parent.ProductId > 0)
                    {
                        var entry = new AutoCompleteBoxEntry(parent.Title, parent.ProductId.ToString());
                        cobParent.Entries.Add(entry);
                    }
                    liRelatedProduct.Attributes.Add("style", "display:none");
                    tabRelatedProduct.Attributes.Add("style", "display:none");
                    tabChildProduct.Visible = false;
                    divProductTags.Visible = false;
                    divPosition.Visible = false;
                    divShowOption.Visible = false;
                }

                if (divShowOption.Visible)
                {
                    foreach (ListItem option in chlShowOption.Items)
                    {
                        option.Selected = ((product.ShowOption & Convert.ToInt32(option.Value)) > 0);
                    }
                }

                foreach (ListItem option in chlPosition.Items)
                {
                    option.Selected = ((product.Position & Convert.ToInt32(option.Value)) > 0);
                }

                lnkPreview.NavigateUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId);
                if (product.IsPublished)
                    lnkPreview.Visible = true;

                hdnTitle.Value = txtTitle.Text;

                if (divProductTags.Visible)
                {
                    List<TagItem> lstTagItems = TagItem.GetByItem(product.ProductGuid);

                    foreach (TagItem tagItem in lstTagItems)
                    {
                        AutoCompleteBoxEntry entry = new AutoCompleteBoxEntry(tagItem.TagText, tagItem.TagId.ToString());
                        autProductTags.Entries.Add(entry);
                    }
                }

                li = ddManufacturers.Items.FindByValue(product.ManufacturerId.ToString());
                if (li != null)
                {
                    ddManufacturers.ClearSelection();
                    li.Selected = true;
                }
                if (displaySettings.ShowMultipleZones)
                {
                    var lstzones = ProductZone.SelectAllByProductID(product.ProductId);

                    cobZoneList.ClearSelection();

                    if (lstzones != null && lstzones.Count > 0)
                    {
                        var lstid = new List<int>();
                        foreach (var zone in lstzones)
                        {
                            if (zone.ZoneID != product.ZoneId)
                            {
                                var id = zone.ZoneID;
                                if (id > 0)
                                    lstid.Add(id);
                            }
                        }
                        foreach (ListItem listItem in cobZoneList.Items)
                        {
                            if (lstid.Contains(Convert.ToInt32(listItem.Value)))
                                listItem.Selected = true;
                        }
                    }
                }
            }
            else
            {
                dpBeginDate.Text = DateTimeHelper.LocalizeToCalendar(DateTime.UtcNow.AddHours(timeOffset).ToString("g"));
                this.btnDelete.Visible = false;
            }

            if ((txtUrl.Text.Length == 0) && (txtTitle.Text.Length > 0))
            {
                String friendlyUrl = SiteUtils.SuggestFriendlyUrl(txtTitle.Text, siteSettings);

                txtUrl.Text = "~/" + friendlyUrl;
            }

            bool isLoadAll = (product != null && product.ProductId > 0);
            LanguageHelper.PopulateTab(tabLanguage, isLoadAll);
            LanguageHelper.PopulateTab(tabLanguageSEO, isLoadAll);
            pnlProductService.Visible = ProductServiceHelper.Enable;
            if (pnlProductService.Visible)//Enable
            {
                List<ProductServicePrice> productServicePrices = new List<ProductServicePrice>();

                if (product != null)
                {
                    productServicePrices = ProductServicePrice.GetByProductId(product.ProductId);
                }
                if (productServicePrices.Count == 0)
                {
                    foreach (ProductServiceType type in (ProductServiceType[])Enum.GetValues(typeof(ProductServiceType)))
                    {
                        ProductServicePrice servicePrice = new ProductServicePrice() { Type = (int)type };
                        productServicePrices.Add(servicePrice);
                    }
                }
                rpProductServiceFee.DataSource = productServicePrices;
                rpProductServiceFee.DataBind();
            }
        }

        private void PopluateProductTypeList()
        {
            ddProductType.Items.Clear();
            ddProductType.DataSource = ProductType.GetAll();
            ddProductType.Items.Insert(0, new ListItem()
            {
                Text = "None",
                Value = "0"
            });
            ddProductType.DataBind();
        }

        private void PopulateManufacturerList()
        {
            var lstManufacturers = Manufacturer.GetAll(siteSettings.SiteId, ManufacturerPublishStatus.NotSet, null, -1);
            divManufacturers.Visible = (lstManufacturers.Count > 0);
            if (lstManufacturers.Count > 0)
            {
                ddManufacturers.DataSource = Manufacturer.GetAll(siteSettings.SiteId, ManufacturerPublishStatus.NotSet, null, -1);
                ddManufacturers.DataBind();
            }
        }

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddZones, false, Product.FeatureGuid);

            if (startZone.Length > 0 && productId == -1)
            {
                ListItem li = ddZones.Items.FindByValue(startZone);
                if (li != null)
                {
                    ddZones.ClearSelection();
                    li.Selected = true;
                }
            }
            ddZones.Items.Insert(0, new ListItem() { Value = "-1", Text = " " });
            if (displaySettings.ShowMultipleZones)
            {
                gbSiteMapProvider.PopulateListControl(cobZoneList, false, Product.FeatureGuid);

                if (startZone.Length > 0 && productId == -1)
                {
                    var li = cobZoneList.Items.FindByValue(startZone);
                    if (li != null)
                    {
                        cobZoneList.ClearSelection();
                        li.Selected = true;
                    }
                }
            }
        }

        #endregion Populate Zone List

        private void BindEnum()
        {
            try
            {
                List<EnumDefined> list = EnumDefined.LoadFromConfigurationXml("product");
                list.RemoveAll(s => Convert.ToInt32(s.Value) <= 0);
                chlPosition.DataSource = list;
                chlPosition.DataBind();

                //if (chlPosition.Items.Count > 0)
                //    divPosition.Visible = true;

                chlShowOption.DataValueField = "Value";
                chlShowOption.DataTextField = "Name";
                chlShowOption.DataSource = EnumDefined.LoadFromConfigurationXml("product", "showoption", "value");
                chlShowOption.DataBind();

                if (chlShowOption.Items.Count > 0)
                    divShowOption.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (!canCreate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            int nId = Save();
            if (nId > 0)
            {
                if (canUpdate)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + editPageUrl + "?ProductID=" + nId.ToString());
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private void btnInsertAndClose_Click(object sender, EventArgs e)
        {
            if (!canCreate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            int nId = Save();
            if (nId > 0)
            {
                if (canViewList)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + listPageUrl + "?start=" + ddZones.SelectedValue);
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private void btnInsertAndNew_Click(object sender, EventArgs e)
        {
            if (!canCreate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            int nId = Save();
            if (nId > 0)
            {
                if (canCreate)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + editPageUrl + "?start=" + ddZones.SelectedValue);
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!canUpdate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            int nId = Save();
            if (nId > 0)
            {
                if (canUpdate)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + editPageUrl + "?ProductID=" + nId.ToString());
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            if (!canUpdate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            int nId = Save();
            if (nId > 0)
            {
                if (canViewList)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + listPageUrl + "?start=" + ddZones.SelectedValue);
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            if (!canUpdate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            int nId = Save();
            if (nId > 0)
            {
                if (canCreate)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + editPageUrl + "?start=" + ddZones.SelectedValue);
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        //2015-11-25: copy manufacturer
        private void btnCopyProduct_Click(object sender, EventArgs e)
        {
            if (!canCreate)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            Page.Validate("productCopy");

            if (!Page.IsValid)
                return;

            if (product == null || product.ProductId <= 0) return;

            TextBox txtCopyProductName = (TextBox)this.FindControl("txtCopyProductName");
            CheckBox chkCopyProductPublished = (CheckBox)this.FindControl("chkCopyProductPublished");
            CheckBox chkCopyProductCopyImages = (CheckBox)this.FindControl("chkCopyProductCopyImages");
            if (
                txtCopyProductName == null
                || txtCopyProductName.Text.Trim().Length == 0
                )
                return;

            int iProductId = product.ProductId;
            Guid productGuid = product.ProductGuid;

            Product copyProduct = product;
            copyProduct.ProductId = -1;
            copyProduct.ProductGuid = Guid.Empty;

            copyProduct.StartDate = DateTime.UtcNow;
            copyProduct.EndDate = DateTime.MaxValue;
            copyProduct.CreatedUtc = DateTime.UtcNow;
            copyProduct.LastModUtc = DateTime.UtcNow;
            copyProduct.UserGuid = currentUser.UserGuid;
            copyProduct.LastModUserGuid = currentUser.UserGuid;
            copyProduct.ViewCount = 0;
            copyProduct.CommentCount = 0;
            copyProduct.Title = txtCopyProductName.Text.Trim();
            copyProduct.IsPublished = chkCopyProductPublished.Checked;
            string copyUrl = SiteUtils.SuggestFriendlyUrl(copyProduct.Title, siteSettings);
            var friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(copyUrl);
            if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
            copyProduct.Url = "~/" + friendlyUrlString;

            copyProduct.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

            if (copyProduct.Save())
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
                            PageGuid = copyProduct.ProductGuid,
                            Url = friendlyUrlString,
                            RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                            + copyProduct.ZoneId.ToInvariantString()
                            + "&ProductID=" + copyProduct.ProductId.ToInvariantString()
                        };

                        newFriendlyUrl.Save();
                    }
                }

                //Copy properties
                var lstProductProperties = ProductProperty.GetPropertiesByProduct(iProductId);
                foreach (ProductProperty property in lstProductProperties)
                {
                    ProductProperty copyProperty = property;
                    copyProperty.Guid = Guid.Empty;
                    copyProperty.ProductId = copyProduct.ProductId;
                    copyProperty.Save();
                }

                //Copy related products
                var lstRelatedProducts = RelatedItem.GetByItem(productGuid);
                foreach (RelatedItem item in lstRelatedProducts)
                {
                    RelatedItem copyItem = new RelatedItem
                    {
                        ItemGuid1 = copyProduct.ProductGuid,
                        ItemGuid2 = item.ItemGuid2,
                        SiteGuid = item.SiteGuid,
                        DisplayOrder = item.DisplayOrder,
                        CreatedUtc = item.CreatedUtc
                    };
                    copyItem.Save();
                }

                //Copy attributes
                var lstProductAttributes = ContentAttribute.GetByContentAsc(productGuid);
                if (lstProductAttributes.Count > 0)
                {
                    var lstProductLanguages = ContentLanguage.GetByContent(productGuid);

                    foreach (ContentAttribute attribute in lstProductAttributes)
                    {
                        ContentAttribute copyAttribute = attribute;
                        copyAttribute.Guid = Guid.Empty;
                        copyAttribute.ContentGuid = copyProduct.ProductGuid;
                        if (copyAttribute.Save())
                        {
                            lstProductLanguages = ContentLanguage.GetByContent(copyAttribute.Guid);
                            foreach (ContentLanguage content in lstProductLanguages)
                            {
                                ContentLanguage copyContent = content;
                                copyContent.Guid = Guid.Empty;
                                copyContent.ContentGuid = copyAttribute.Guid;
                                copyContent.Save();
                            }
                        }
                    }
                }

                //Copy images
                if (chkCopyProductCopyImages.Checked)
                {
                    var lstProductMedia = ContentMedia.GetByContentAsc(productGuid);
                    if (lstProductMedia.Count > 0)
                    {
                        imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, iProductId);
                        var copyImageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, copyProduct.ProductId);

                        ProductHelper.VerifyProductFolders(fileSystem, copyImageFolderPath);

                        foreach (ContentMedia medium in lstProductMedia)
                        {
                            ContentMedia copyContent = medium;
                            copyContent.Guid = Guid.Empty;
                            copyContent.ContentGuid = copyProduct.ProductGuid;
                            if (copyContent.Save())
                            {
                                if (fileSystem.FileExists(imageFolderPath + medium.MediaFile))
                                    fileSystem.CopyFile(imageFolderPath + medium.MediaFile, copyImageFolderPath + medium.MediaFile, false);
                                if (fileSystem.FileExists(imageFolderPath + "thumbs/" + medium.ThumbnailFile))
                                    fileSystem.CopyFile(imageFolderPath + "thumbs/" + medium.ThumbnailFile, copyImageFolderPath + "thumbs/" + medium.ThumbnailFile, false);
                            }
                        }
                    }
                }

                //Product Zone
                if (copyProduct.ZoneId > 0)
                    new ProductZone() { ProductID = copyProduct.ProductId, ZoneID = copyProduct.ZoneId }.Save();

                LogActivity.Write("Copy product", copyProduct.Title);
                message.SuccessMessage = ResourceHelper.GetResourceString("ProductResources", "CopyProductSuccessMessage");

                WebUtils.SetupRedirect(this, basePage.SiteRoot + editPageUrl + (canUpdate ? "?ProductID=" + copyProduct.ProductId.ToString() : ""));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (product != null && !product.IsDeleted)
            {
                if (!canDelete)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                ContentDeleted.Create(siteSettings.SiteId, product.Title, "Product", typeof(ProductDeleted).AssemblyQualifiedName, product.ProductId.ToString(), Page.User.Identity.Name);

                product.IsDeleted = true;

                product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

                product.SaveDeleted();

                SiteUtils.QueueIndexing();

                ShoppingCartItem.DeleteByProduct(product.ProductId);

                LogActivity.Write("Delete product", product.Title);

                if (canViewList)
                    WebUtils.SetupRedirect(this, basePage.SiteRoot + listPageUrl);
                else
                    SiteUtils.RedirectToHomepage();
            }
        }

        private bool ParamsAreValid()
        {
            try
            {
                DateTime localTime = DateTime.Parse(dpBeginDate.Text);
            }
            catch (FormatException)
            {
                message.ErrorMessage = ProductResources.ParseDateFailureMessage;
                return false;
            }
            catch (ArgumentNullException)
            {
                message.ErrorMessage = ProductResources.ParseDateFailureMessage;
                return false;
            }
            return true;
        }

        private int Save()
        {
            Page.Validate("product");

            if (!Page.IsValid)
                return -1;

            if (ddZones.SelectedValue.Length == 0)
            {
                message.ErrorMessage = ProductResources.SelectZoneMessage;
                return -1;
            }
            product = new Product(siteSettings.SiteId, productId);

            if (currentUser == null) { return -1; }
            product.LastModUserGuid = currentUser.UserGuid;

            bool changedZone = false;
            if (product.ZoneId.ToString() != ddZones.SelectedValue && product.ProductId > 0)
                changedZone = true;

            product.ZoneId = Convert.ToInt32(ddZones.SelectedValue);
            product.SiteId = siteSettings.SiteId;
            product.ManufacturerId = Convert.ToInt32(ddManufacturers.SelectedValue);

            product.DisableBuyButton = !cbEnableBuyButton.Checked;
            product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);

            DateTime localTime = DateTime.Parse(dpBeginDate.Text);
            if (timeZone != null)
                product.StartDate = localTime.ToUtc(timeZone);
            else
                product.StartDate = localTime.AddHours(-timeOffset);

            if (dpEndDate.Text.Length == 0)
                product.EndDate = DateTime.MaxValue;
            else
            {
                DateTime localEndTime = DateTime.Parse(dpEndDate.Text);
                if (timeZone != null)
                {
                    product.EndDate = localEndTime.ToUtc(timeZone);
                }
                else
                {
                    product.EndDate = localEndTime.AddHours(-timeOffset);
                }
            }

            product.IsPublished = chkIsPublished.Checked;

            bool saveState = false;
            //if (WebConfigSettings.EnableContentWorkflow && siteSettings.EnableContentWorkflow) Note here
            //{
            //    if (userCanEdit)
            //    {
            //        if (manufacturerId > -1)
            //        {
            //            if (manufacturer.IsPublished && manufacturer.UserGuid != currentUser.UserGuid)
            //            {
            //                manufacturer.Status = ContentWorkflowStatus.Approved;
            //                if (!manufacturer.ApprovedUserGuid.HasValue) { manufacturer.ApprovedUserGuid = currentUser.UserGuid; }
            //                if (!manufacturer.ApprovedUtc.HasValue) { manufacturer.ApprovedUtc = DateTime.UtcNow; }
            //                if (!string.IsNullOrEmpty(manufacturer.ApprovedBy)) { manufacturer.ApprovedBy = Context.User.Identity.Name.Trim(); }

            //                saveState = true;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        if (manufacturerId == -1)
            //        {
            //            manufacturer.IsPublished = false;
            //            manufacturer.Status = ContentWorkflowStatus.Draft;

            //            saveState = true;
            //        }
            //        else if (!manufacturer.IsPublished)
            //        {
            //            if (manufacturer.Status == ContentWorkflowStatus.ApprovalRejected
            //                || manufacturer.Status == ContentWorkflowStatus.Cancelled
            //                || manufacturer.Status == ContentWorkflowStatus.None)
            //            {
            //                manufacturer.Status = ContentWorkflowStatus.Draft;
            //                saveState = true;
            //            }
            //        }
            //    }
            //}

            product.UserGuid = currentUser.UserGuid;
            product.OpenInNewWindow = chkOpenInNewWindow.Checked;

            product.Code = txtProductCode.Text.Trim();

            decimal price = decimal.Zero;
            decimal.TryParse(txtPrice.Text.Trim(), out price);
            if (price > 0)
                product.Price = price;
            else
                product.Price = decimal.Zero;

            decimal oldPrice = decimal.Zero;
            decimal.TryParse(txtOldPrice.Text.Trim(), out oldPrice);
            if (oldPrice > 0)
                product.OldPrice = oldPrice;
            else
                product.OldPrice = decimal.Zero;

            decimal.TryParse(txtGroupPrice.Text.Trim(), out decimal groupPrice);
            product.GroupPrice = groupPrice;

            int.TryParse(txtStockQuantity.Text.Trim(), out int stockQuantity);
            if (stockQuantity > 0)
                product.StockQuantity = stockQuantity;
            else
                product.StockQuantity = 0;
            product.ApiProductId = txtApiProductID.Text.Trim();

            var weight = decimal.Zero;
            decimal.TryParse(txtWeight.Text.Trim(), out weight);
            if (weight > 0)
                product.Weight = weight;
            else
                product.Weight = decimal.Zero;

            if (divRatingSum.Visible)
            {
                var ratingSum = decimal.Zero;
                decimal.TryParse(txtRatingSum.Text.Trim(), out ratingSum);
                if (ratingSum > 0)
                    product.RatingSum = ratingSum;
                else
                    product.RatingSum = decimal.Zero;
            }

            if (divRatingVotes.Visible)
            {
                int.TryParse(txtRatingVotes.Text.Trim(), out int ratingVotes);
                if (ratingVotes > 0)
                    product.RatingVotes = ratingVotes;
                else
                    product.RatingVotes = 0;
            }

            product.ProductType = Convert.ToInt32(ddProductType.SelectedValue);

            product.ShowOption = chlShowOption.Items.SelectedItemsToBinaryOrOperator();
            product.Position = chlPosition.Items.SelectedItemsToBinaryOrOperator();

            product.BarCode = txtFileAttachment.Text.Trim();
            string oldUrl = string.Empty;
            string newUrl = string.Empty;
            String friendlyUrlString = string.Empty;
            FriendlyUrl friendlyUrl = null;
            if (!IsLanguageTab())
            {
                product.Title = txtTitle.Text.Trim();
                product.SubTitle = txtSubTitle.Text.Trim();
                product.BriefContent = edBriefContent.Text;
                product.FullContent = edFullContent.Text;

                if (txtUrl.Text.Length == 0)
                {
                    txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(txtTitle.Text, siteSettings);
                }

                friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtUrl.Text.Replace("~/", String.Empty));

                if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                {
                    friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
                }

                friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

                if (
                    ((friendlyUrl.FoundFriendlyUrl) && (friendlyUrl.PageGuid != product.ProductGuid))
                    && (product.Url != txtUrl.Text.Trim())
                    && (!txtUrl.Text.StartsWith("http"))
                    )
                {
                    message.ErrorMessage = ProductResources.PageUrlInUseErrorMessage;
                    cancelRedirect = true;
                    return -1;
                }

                oldUrl = product.Url.Replace("~/", string.Empty);
                newUrl = friendlyUrlString;

                if (txtUrl.Text.Trim().StartsWith("http"))
                {
                    product.Url = txtUrl.Text.Trim();
                }
                else if (friendlyUrlString.Length > 0)
                {
                    product.Url = "~/" + friendlyUrlString;
                }
                else if (friendlyUrlString.Length == 0)
                {
                    product.Url = string.Empty;
                }
            }
            if (!IsLanguageSeoTab())
            {
                product.MetaDescription = txtMetaDescription.Text;
                product.MetaKeywords = txtMetaKeywords.Text;
                product.MetaTitle = txtMetaTitle.Text;
                product.AdditionalMetaTags = txtAdditionalMetaTags.Text;
            }

            if (product.Save())
            {
                SaveProductProperties(product.ProductId);
                SaveContentLanguage(product.ProductGuid);
                SaveContentLanguageSEO(product.ProductGuid);
                SaveProductServiceFee();
                SaveProductZone();

                if (saveState)
                    product.SaveState();

                //Save Image
                if (fileImage.UploadedFiles.Count > 0)
                {
                    imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, product.ProductId);
                    thumbnailImageFolderPath = imageFolderPath + "thumbs/";

                    ProductHelper.VerifyProductFolders(fileSystem, imageFolderPath);

                    foreach (UploadedFile file in fileImage.UploadedFiles)
                    {
                        string ext = file.GetExtension();
                        if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                        {
                            ContentMedia media = new ContentMedia
                            {
                                SiteGuid = siteSettings.SiteGuid,
                                ContentGuid = product.ProductGuid,
                                //image.Title = txtImageTitle.Text;
                                DisplayOrder = 0
                            };

                            string newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                            string newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (media.MediaFile == newFileName)
                            {
                                // an existing image delete the old one
                                fileSystem.DeleteFile(newImagePath);
                            }
                            else
                            {
                                // this is a new newsImage instance, make sure we don't use the same file name as any other instance
                                int i = 1;
                                while (fileSystem.FileExists(VirtualPathUtility.Combine(imageFolderPath, newFileName)))
                                {
                                    newFileName = i.ToInvariantString() + newFileName;
                                    i += 1;
                                }
                            }

                            newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            file.SaveAs(Server.MapPath(newImagePath));

                            media.MediaFile = newFileName;
                            media.ThumbnailFile = newFileName;
                            media.ThumbNailWidth = displaySettings.ThumbnailWidth;
                            media.ThumbNailHeight = displaySettings.ThumbnailHeight;
                            media.Save();

                            ProductHelper.ProcessImage(media, fileSystem, imageFolderPath, file.FileName, ProductHelper.GetColor(displaySettings.ResizeBackgroundColor));
                        }
                    }
                }

                if (productId > 0)
                {
                    LogActivity.Write("Update product", product.Title);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
                else
                {
                    LogActivity.Write("Create new product", product.Title);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }
            }

            if (!IsLanguageTab())
            {
                if (
                    (oldUrl.Length > 0)
                    && (newUrl.Length > 0)
                    && (!SiteUtils.UrlsMatch(oldUrl, newUrl))
                    )
                {
                    FriendlyUrl oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                    if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.PageGuid == product.ProductGuid))
                    {
                        FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
                    }
                }

                if (
                    ((txtUrl.Text.EndsWith(".aspx")) || siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName)
                    && (txtUrl.Text.StartsWith("~/"))
                    )
                {
                    if (!friendlyUrl.FoundFriendlyUrl)
                    {
                        if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
                        {
                            FriendlyUrl newFriendlyUrl = new FriendlyUrl
                            {
                                SiteId = siteSettings.SiteId,
                                SiteGuid = siteSettings.SiteGuid,
                                PageGuid = product.ProductGuid,
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

            if (changedZone)
            {
                List<FriendlyUrl> friendlyUrls = FriendlyUrl.GetByPageGuid(product.ProductGuid);
                foreach (FriendlyUrl item in friendlyUrls)
                {
                    item.RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                        + product.ZoneId.ToInvariantString()
                        + "&ProductID=" + product.ProductId.ToInvariantString();

                    item.Save();
                }
            }

            if (divProductTags.Visible)
                UpdateProductTags();

            SiteUtils.QueueIndexing();

            return product.ProductId;
        }

        private void SaveProductZone()
        {
            if (!displaySettings.ShowMultipleZones) return;

            List<int> lstZoneId = new List<int>();
            List<int> lstZoneId2 = new List<int>();
            ListItem li = cobZoneList.Items.FindByValue(product.ZoneId.ToString());
            if (li != null)
                li.Selected = true;
            foreach (var item in cobZoneList.SelectedItems)
            {
                lstZoneId.Add(Convert.ToInt32(item.Value)); // list of old zones
                lstZoneId2.Add(Convert.ToInt32(item.Value)); // copy of list of old zones
            }
            var lstProductZone = ProductZone.SelectAllByProductID(product.ProductId); // list of new zones
            List<ProductZone> lstProductZone2 = new List<ProductZone>(lstProductZone); // copy of list of new zones
            foreach (var item in lstZoneId2)
            {
                var it = lstProductZone2.Where(zone => zone.ZoneID == item).FirstOrDefault(); // find matches between new zones and old zones
                if (it != null)
                {
                    lstZoneId.Remove(item); // remove matches from old zones
                    var itZone = lstProductZone.Where(zone => zone.ZoneID == item).FirstOrDefault();
                    lstProductZone.Remove(itZone); // remove matches from new zones
                }
            }
            foreach (var productZone in lstProductZone)
            {
                ProductZone.Delete(productZone.ProductID, productZone.ZoneID); // delete old zones
            }
            foreach (var item in lstZoneId)
            {
                ProductZone productZone = new ProductZone()
                {
                    ProductID = product.ProductId,
                    ZoneID = item
                };
                productZone.Save(); // save new zones
            }

            ////Update child's [ProductZone]
            //List<Product> lstChildProducts = Product.GetChildProducts(productId);
            //foreach (Product childProduct in lstChildProducts)
            //{
            //    var lstChildProductZones = ProductZone.SelectAllByProductID(childProduct.ProductId); // list of child's zones
            //    var oldChildMainZone = lstChildProductZones.Where(zone => zone.ZoneID == childProduct.ZoneId).FirstOrDefault(); // get child's old main zone
            //    if (oldChildMainZone != null)
            //        ProductZone.Delete(oldChildMainZone.RowID); // delete child's old main zone
            //    var matchChildZone = lstChildProductZones.Where(zone => zone.ZoneID == product.ZoneId).FirstOrDefault();
            //    if (matchChildZone == null) // no need to save if child already have that zone
            //    {
            //        ProductZone newChildMainZone = new ProductZone()
            //        {
            //            ProductId = childProduct.ProductId,
            //            ZoneID = product.ZoneId
            //        };
            //        newChildMainZone.Save(); // save child's new main zone
            //    }
            //}
        }

        private void SaveProductServiceFee()
        {
            if (!ProductServiceHelper.Enable)
                return;
            foreach (RepeaterItem item in rpProductServiceFee.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var serviceFeeGuid = new Guid(((HiddenField)item.FindControl("hdServiceFeeGuid")).Value);
                    var type = Convert.ToInt32(((HiddenField)item.FindControl("hdType")).Value);
                    //var cbFreeForPaymentOnline = (CheckBox)item.FindControl("cbFreeForPaymentOnline");
                    var txtFeeForOrderTotal = (TextBox)item.FindControl("txtFeeForOrderTotal");
                    var txtServiceFee = (TextBox)item.FindControl("txtServiceFee");

                    decimal.TryParse(txtFeeForOrderTotal.Text, out decimal feeForOrderTotal);
                    decimal.TryParse(txtServiceFee.Text, out decimal serviceFee);

                    ProductServicePrice servicePrice = new ProductServicePrice(serviceFeeGuid)
                    {
                        Type = type,
                        FeeForOrderTotal = feeForOrderTotal,
                        //  FreeForPaymentOnline = cbFreeForPaymentOnline.Checked,
                        Price = serviceFee,
                        ProductId = product.ProductId
                    };
                    servicePrice.Save();
                }
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

        private void UpdateProductTags()
        {
            //TagItem.DeleteByItem(manufacturer.ProductGuid);
            var tagItems = new List<TagItem>();
            if (productId > 0)
            {
                tagItems = TagItem.GetByItem(product.ProductGuid);
                foreach (TagItem tagItem in tagItems)
                {
                    bool found = false;
                    foreach (AutoCompleteBoxEntry entry in autProductTags.Entries)
                    {
                        if (entry.Value == tagItem.TagId.ToString())
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        TagItem.Delete(tagItem.Guid);
                        Tag.UpdateItemCount(tagItem.TagId);
                    }
                }
            }

            foreach (AutoCompleteBoxEntry entry in autProductTags.Entries)
            {
                int newTagId = -1;
                if (entry.Value.Length == 0)
                {
                    Tag tag = new Tag
                    {
                        SiteGuid = siteSettings.SiteGuid,
                        FeatureGuid = Product.FeatureGuid,
                        TagText = entry.Text,
                        ItemCount = 1,
                        CreatedBy = currentUser.SiteGuid
                    };
                    newTagId = tag.Save();

                    TagItem.Create(newTagId, product.ProductGuid);
                }
                else
                {
                    newTagId = Convert.ToInt32(entry.Value);
                    bool found = false;

                    foreach (TagItem tagItem in tagItems)
                    {
                        if (tagItem.TagId == newTagId)
                        {
                            found = true;
                            break;
                        }
                    }

                    if (!found)
                    {
                        TagItem.Create(newTagId, product.ProductGuid);
                        Tag.UpdateItemCount(newTagId);
                    }
                }
            }
        }

        #region Language

        protected void tabLanguage_TabClick(object sender, RadTabStripEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                reqTitle.Visible = true;

                txtSubTitle.Text = product.SubTitle;
                txtTitle.Text = product.Title;
                txtUrl.Text = product.Url;
                edBriefContent.Text = product.BriefContent;
                edFullContent.Text = product.FullContent;
                //   txtFileAttachment.Text = product.FileAttachment;
            }
            else
            {
                reqTitle.Visible = false;

                txtSubTitle.Text = string.Empty;
                txtTitle.Text = string.Empty;
                txtUrl.Text = string.Empty;
                edBriefContent.Text = string.Empty;
                edFullContent.Text = string.Empty;
                //  txtFileAttachment.Text = string.Empty;

                ContentLanguage content = new ContentLanguage(product.ProductGuid, Convert.ToInt32(e.Tab.Value));
                if (content != null && content.Guid != Guid.Empty)
                {
                    txtSubTitle.Text = content.ExtraText1;
                    txtTitle.Text = content.Title;
                    txtUrl.Text = content.Url;
                    edBriefContent.Text = content.BriefContent;
                    edFullContent.Text = content.FullContent;
                    //   txtFileAttachment.Text = content.ExtraText2;
                }
            }

            upButton.Update();
            hdnTitle.Value = txtTitle.Text;
        }

        protected void tabLanguageSEO_TabClick(object sender, RadTabStripEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                txtMetaKeywords.Text = product.MetaKeywords;
                txtMetaDescription.Text = product.MetaDescription;
                txtMetaTitle.Text = product.MetaTitle;
                txtAdditionalMetaTags.Text = product.AdditionalMetaTags;
            }
            else
            {
                txtMetaKeywords.Text = "";
                txtMetaDescription.Text = "";
                txtMetaTitle.Text = "";
                txtAdditionalMetaTags.Text = "";

                ContentLanguage content = new ContentLanguage(product.ProductGuid, Convert.ToInt32(e.Tab.Value));
                if (content != null && content.Guid != Guid.Empty)
                {
                    txtMetaKeywords.Text = content.MetaKeywords;
                    txtMetaDescription.Text = content.MetaDescription;
                    txtMetaTitle.Text = content.MetaTitle;
                    txtAdditionalMetaTags.Text = content.ExtraContent1;
                }
            }

            upButton.Update();
        }

        private bool IsLanguageTab()
        {
            if (tabLanguage.Visible && tabLanguage.SelectedIndex > 0)
                return true;

            return false;
        }

        private bool IsLanguageSeoTab()
        {
            if (tabLanguageSEO.Visible && tabLanguageSEO.SelectedIndex > 0)
                return true;

            return false;
        }

        private void SaveContentLanguage(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsLanguageTab())
                return;

            int languageID = -1;
            if (tabLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabLanguage.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);
            var newsName = txtTitle.Text.Trim();

            if (newsName.Length == 0)
            {
                return;
            }

            if (txtUrl.Text.Length == 0)
            {
                txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(newsName, siteSettings);
            }

            if (txtUrl.Text.Length == 0 || (txtUrl.Text == "~/" && product.Url != "~/"))
            {
                txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(product.Title + " " + tabLanguage.SelectedTab.Text, siteSettings);
            }

            String friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtUrl.Text.Replace("~/", String.Empty));

            if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
            {
                friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
            }

            FriendlyUrl friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

            if (
                ((friendlyUrl.FoundFriendlyUrl) && (friendlyUrl.ItemGuid != content.Guid))
                && (content.Url != txtUrl.Text.Trim())
                && (!txtUrl.Text.StartsWith("http"))
                )
            {
                message.ErrorMessage = ProductResources.PageUrlInUseErrorMessage;
                //message.InfoMessage = ProductResources.NewsUrlInUseErrorMessage;
                return;
            }

            string oldUrl = content.Url.Replace("~/", string.Empty);
            string newUrl = friendlyUrlString;
            if ((txtUrl.Text.StartsWith("http")) || (txtUrl.Text.Trim() == "~/"))
            {
                content.Url = txtUrl.Text.Trim();
            }
            else if (friendlyUrlString.Length > 0)
            {
                content.Url = "~/" + friendlyUrlString;
            }
            else if (friendlyUrlString.Length == 0)
            {
                content.Url = string.Empty;
            }

            content.BriefContent = edBriefContent.Text.Trim();
            content.FullContent = edFullContent.Text.Trim();
            content.LanguageId = languageID;
            content.ContentGuid = contentGuid;
            content.SiteGuid = siteSettings.SiteGuid;
            content.Title = newsName;
            content.ExtraText1 = txtSubTitle.Text.Trim();
            //   content.ExtraText2 = txtFileAttachment.Text.Trim();
            content.Save();

            if (
                (oldUrl.Length > 0)
                && (newUrl.Length > 0)
                && (!SiteUtils.UrlsMatch(oldUrl, newUrl))
                )
            {
                FriendlyUrl oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.ItemGuid == content.Guid))
                {
                    FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
                }
            }

            if (
                ((txtUrl.Text.EndsWith(".aspx")) || siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName)
                && (txtUrl.Text.StartsWith("~/"))
                )
            {
                if (!friendlyUrl.FoundFriendlyUrl)
                {
                    if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
                    {
                        FriendlyUrl newFriendlyUrl = new FriendlyUrl
                        {
                            SiteId = siteSettings.SiteId,
                            SiteGuid = siteSettings.SiteGuid,
                            PageGuid = product.ProductGuid,
                            ItemGuid = content.Guid,
                            LanguageId = content.LanguageId,
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

        protected void btnDeleteLanguage_Click(object sender, EventArgs e)
        {
            if (!IsLanguageTab())
                return;

            int languageId = -1;
            if (tabLanguage.SelectedIndex > 0)
            {
                languageId = Convert.ToInt32(tabLanguage.SelectedTab.Value);

                if (languageId > 0)
                {
                    FriendlyUrl.DeleteByLanguage(product.ProductGuid, languageId);
                    ContentLanguage.Delete(product.ProductGuid, languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);
                    SiteUtils.QueueIndexing();

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                }
            }
        }

        private void SaveContentLanguageSEO(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsLanguageSeoTab())
                return;

            int languageID = -1;
            if (tabLanguageSEO.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabLanguageSEO.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);

            if (content == null || content.LanguageId == -1)
                return;

            content.MetaTitle = txtMetaTitle.Text.Trim();
            content.MetaKeywords = txtMetaKeywords.Text.Trim();
            content.MetaDescription = txtMetaDescription.Text.Trim();
            content.ExtraContent1 = txtAdditionalMetaTags.Text.Trim();
            content.Save();
        }

        #endregion Language

        private void PopulateLabels()
        {
            if (pageTitle.Length > 0)
            {
                Page.Title = SiteUtils.FormatPageTitle(siteSettings, pageTitle);
                heading.Text = pageTitle;
            }
            else
            {
                if (product == null || product.ProductId == -1)
                {
                    Page.Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductAddNewTitle);
                    heading.Text = ProductResources.ProductAddNewTitle;
                }
                else
                {
                    Page.Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductEditTitle);
                    heading.Text = ProductResources.ProductEditTitle;
                }
            }

            breadcrumb.CurrentPageUrl = editPageUrl;

            litContentTab.Text = ProductResources.ContentTab;
            litMetaTab.Text = ProductResources.MetaTab;

            litAttributeTab.Text = "<a aria-controls='" + tabAttribute.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabAttribute.ClientID + "'>" + ProductResources.ProductAttributesTab + "</a>";
            litRelatedProductTab.Text = "<a aria-controls='" + tabRelatedProduct.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabRelatedProduct.ClientID + "'>" + ProductResources.ProductEditRelatedProductTab + "</a>";
            litChildProductTab.Text = "<a aria-controls='" + tabChildProduct.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabChildProduct.ClientID + "'>" + ProductResources.ProductEditChildProductTab + "</a>";
            litRelatedNewsTab.Text = "<a aria-controls='" + tabRelatedNews.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabRelatedNews.ClientID + "'>" + ResourceHelper.GetResourceString("ProductResources", "ProductEditRelatedNewsTab") + "</a>";

            if (displaySettings.ShowVideo)
            {
                litImagesTab.Text = "<a aria-controls='" + tabImages.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabImages.ClientID + "'>" + ProductResources.ProductMediaTab + "</a>";

                var column = grid.MasterTableView.Columns.FindByUniqueName("MediaFile");
                if (column != null)
                    column.HeaderText = ProductResources.ProductMediaTab;
            }
            else
            {
                litImagesTab.Text = "<a aria-controls='" + tabImages.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabImages.ClientID + "'>" + ProductResources.ProductImagesTab + "</a>";

                divProductVideos.Visible = false;
            }

            litCustomFieldTab.Text = "<a aria-controls='" + tabCustomField.ClientID + "' role=\"tab\" data-toggle=\"tab\" href='#" + tabCustomField.ClientID + "'>" + ProductResources.ProductPropertiesTab + "</a>";

            //btnAttributeUp.AlternateText = ProductResources.AttributeUpAlternateText;
            btnAttributeUp.ToolTip = ProductResources.AttributeUpAlternateText;
            //btnAttributeUp.ImageUrl = basePage.ImageSiteRoot + "/Data/SiteImages/up.gif";

            //btnAttributeDown.AlternateText = ProductResources.AttributeDownAlternateText;
            btnAttributeDown.ToolTip = ProductResources.AttributeDownAlternateText;
            //btnAttributeDown.ImageUrl = basePage.ImageSiteRoot + "/Data/SiteImages/dn.gif";

            UIHelper.AddConfirmationDialog(btnDeleteImage, ProductResources.ImageDeleteConfirmMessage);

            //btnDeleteAttribute.AlternateText = ProductResources.AttributeDeleteSelectedButton;
            btnDeleteAttribute.ToolTip = ProductResources.AttributeDeleteSelectedButton;
            //btnDeleteAttribute.ImageUrl = basePage.ImageSiteRoot + "/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
            UIHelper.AddConfirmationDialog(btnDeleteAttribute, ProductResources.AttributeDeleteConfirmMessage);
            UIHelper.AddConfirmationDialog(btnDeleteAttributeLanguage, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));

            edFullContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edBriefContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edBriefContent.WebEditor.Height = Unit.Pixel(300);
            edAttributeContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            lnkPreview.Text = ProductResources.ProductEditPreviewLabel;

            btnDelete.Text = ProductResources.ProductEditDeleteButton;
            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductDeleteWarning);

            reqTitle.ErrorMessage = ProductResources.TitleRequiredWarning;
            reqStartDate.ErrorMessage = ProductResources.ProductBeginDateRequired;
            this.dpBeginDate.ClockHours = ConfigurationManager.AppSettings["ClockHours"];
            regexUrl.ErrorMessage = ProductResources.FriendlyUrlRegexWarning;

            //FileAttachmentBrowser.TextBoxClientId = txtFileAttachment.ClientID;
            //FileAttachmentBrowser.Text = ProductResources.BrowserOnServerLink;

            if (!displaySettings.ShowTags)
            {
                divProductTags.Visible = false;
            }

            VideoFileBrowser.TextBoxClientId = txtVideoPath.ClientID;

            divRatingSum.Visible = displaySettings.ShowRatingStats;
            divRatingVotes.Visible = displaySettings.ShowRatingStats;
            divGroupPrice.Visible = false;
            if (ProductHelper.EnableProductGroup)
            {
                divGroupPrice.Visible = true;
                litGroupProduct.Text = "<a aria-controls='"
                    + tabGroupProduct.ClientID
                    + "' role=\"tab\" data-toggle=\"tab\" href='#"
                    + tabGroupProduct.ClientID
                    + "'>Nhóm sản phẩm</a>";
            }

            divParent.Visible = ProductHelper.IsParent(product);
        }

        private void LoadSettings()
        {
            canViewList = ProductPermission.CanViewList;
            canCreate = ProductPermission.CanCreate;
            canUpdate = ProductPermission.CanUpdate;
            canDelete = ProductPermission.CanDelete;

            startZone = WebUtils.ParseStringFromQueryString("start", startZone);

            if ((WebUser.IsAdminOrContentAdmin) || (SiteUtils.UserIsSiteEditor())) { isAdmin = true; }

            siteSettings = CacheHelper.GetCurrentSiteSettings();
            currentUser = SiteUtils.GetCurrentSiteUser();

            if (productId > -1)
            {
                product = new Product(siteSettings.SiteId, productId);
                if (product != null && product.ProductId > 0)
                {
                    if (product.IsDeleted)
                    {
                        SiteUtils.RedirectToEditAccessDeniedPage();
                        return;
                    }
                    imageFolderPath = ProductHelper.MediaFolderPath(siteSettings.SiteId, product.ProductId);
                    thumbnailImageFolderPath = imageFolderPath + "thumbs/";

                    relatedProduct.ProductGuid = product.ProductGuid;
                    relatedProduct.SiteGuid = siteSettings.SiteGuid;

                    productChild.ProductId = product.ProductId;

                    if (ProductHelper.EnableProductGroup)
                        groupProduct.ProductId = product.ProductId;


                    relatedNews.SiteGuid = siteSettings.SiteGuid;
                    relatedNews.ProductGuid = product.ProductGuid;
                }
            }

            HideControls();

            if (canViewList)
            {
                if (listPageTitle.Length > 0)
                    breadcrumb.ParentTitle = listPageTitle;
                else
                    breadcrumb.ParentTitle = ProductResources.ProductListTitle;

                breadcrumb.ParentUrl = listPageUrl;
            }

            FileSystemProvider p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p != null) { fileSystem = p.GetFileSystem(); }

            liAttribute.Visible = (product != null & displaySettings.ShowAttribute);
            tabAttribute.Visible = liAttribute.Visible;

            liRelatedProduct.Visible = (product != null & displaySettings.ShowRelatedProduct);
            tabRelatedProduct.Visible = liRelatedProduct.Visible;

            liChildProduct.Visible = (product != null && product.ParentId == 0);

            liRelatedNews.Visible = (product != null & displaySettings.ShowRelatedNews);
            tabRelatedNews.Visible = liRelatedNews.Visible;

            liImages.Visible = (product != null & ProductConfiguration.UseImages);
            tabImages.Visible = liImages.Visible;
            divUploadImage.Visible = (!liImages.Visible);

            divProductCode.Visible = displaySettings.ShowProductCode;
            divPrice.Visible = displaySettings.ShowPrice;
            divOldPrice.Visible = displaySettings.ShowOldPrice;
            divSubTitle.Visible = displaySettings.ShowSubTitle;
            divFileAttachment.Visible = displaySettings.ShowAttachment;
            divStockQuantity.Visible = displaySettings.ShowStockQuantity;
            divApiProductID.Visible = displaySettings.ShowApiProductID;
            divOtherZone.Visible = displaySettings.ShowMultipleZones;

            try
            {
                // this keeps the action from changing during ajax postback in folder based sites
                SiteUtils.SetFormAction(Page, Request.RawUrl);
            }
            catch (MissingMethodException)
            {
                //this method was introduced in .NET 3.5 SP1
            }

            //if (userCanEdit) Note here
            //{
            //    chkIsPublished.Enabled = true;
            //    if (!Page.IsPostBack)
            //        chkIsPublished.Checked = true;
            //}

            //2015-11-25: copy manufacturer settings
            Button btnCopyProduct = (Button)this.FindControl("btnCopyProduct");
            Button btnCopyModal = (Button)this.FindControl("btnCopyModal");
            Panel pnlModal = (Panel)this.FindControl("pnlModal");
            if (btnCopyProduct != null && btnCopyModal != null && pnlModal != null)
            {
                TextBox txtCopyProductName = (TextBox)this.FindControl("txtCopyProductName");

                btnCopyModal.Visible = false;
                pnlModal.Visible = false;

                if (product != null && product.ProductId > 0 && canCreate)
                {
                    if (!Page.IsPostBack)
                        txtCopyProductName.Text = "Copy of " + product.Title;
                    btnCopyProduct.Text = ResourceHelper.GetResourceString("ProductResources", "CopyProductButton");
                    btnCopyModal.Text = ResourceHelper.GetResourceString("ProductResources", "CopyProductButton");
                    btnCopyModal.Attributes["data-target"] = "#" + pnlModal.ClientID;

                    btnCopyModal.Visible = true;
                    pnlModal.Visible = true;

                    btnCopyProduct.Click += new EventHandler(btnCopyProduct_Click);
                }
            }

            basePage.AddClassToBody("admin-productedit");
        }

        private void HideControls()
        {
            btnInsert.Visible = false;
            btnInsertAndNew.Visible = false;
            btnInsertAndClose.Visible = false;
            btnUpdate.Visible = false;
            btnUpdateAndNew.Visible = false;
            btnUpdateAndClose.Visible = false;
            btnDelete.Visible = false;

            btnAttributeUpdate.Visible = false;
            btnAttributeUp.Visible = false;
            btnAttributeDown.Visible = false;
            btnDeleteAttribute.Visible = false;
            btnUpdateImage.Visible = false;
            btnDeleteImage.Visible = false;

            if (product == null)
            {
                btnInsert.Visible = canCreate;
                btnInsertAndNew.Visible = canCreate;
                btnInsertAndClose.Visible = (canCreate && canViewList);
            }
            else if (product != null && product.ProductId > 0)
            {
                if (!basePage.UserCanAuthorizeZone(product.ZoneId))
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                btnUpdate.Visible = canUpdate;
                btnUpdateAndNew.Visible = (canUpdate && canCreate);
                btnUpdateAndClose.Visible = (canUpdate && canViewList);

                btnAttributeUpdate.Visible = canUpdate;
                btnAttributeUp.Visible = canUpdate;
                btnAttributeDown.Visible = canUpdate;
                btnDeleteAttribute.Visible = canUpdate;
                btnUpdateImage.Visible = canUpdate;
                btnDeleteImage.Visible = canUpdate;
                btnDelete.Visible = canDelete;
            }
        }

        private void LoadParams()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            productId = WebUtils.ParseInt32FromQueryString("ProductID", productId);
            virtualRoot = WebUtils.GetApplicationRoot();
        }

        private void SetupScripts()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa", "<script src=\""
                    + ResolveUrl("~/ClientScript/sarissa/sarissa.js") + "\" type=\"text/javascript\"></script>");
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa_ieemu_xpath"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa_ieemu_xpath", "<script src=\""
                    + ResolveUrl("~/ClientScript/sarissa/sarissa_ieemu_xpath.js") + "\" type=\"text/javascript\"></script>");
            }
            SetupUrlSuggestScripts(this.txtTitle.ClientID, this.txtUrl.ClientID, this.hdnTitle.ClientID, this.spnUrlWarning.ClientID);
        }

        private void SetupUrlSuggestScripts(string inputText, string outputText, string referenceText, string warningSpan)
        {
            if ((productId == -1) || (WebConfigSettings.AutoSuggestFriendlyUrlsOnZoneNameChanges))
            {
                if (!Page.ClientScript.IsClientScriptBlockRegistered("friendlyurlsuggest"))
                {
                    this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "friendlyurlsuggest", "<script src=\""
                        + ResolveUrl("~/ClientScript/friendlyurlsuggest_v2.js") + "\" type=\"text/javascript\"></script>");
                }

                string focusScript = string.Empty;
                if (productId == -1) { focusScript = "document.getElementById('" + inputText + "').focus();"; }

                string hookupInputScript = "new UrlHelper( "
                    + "document.getElementById('" + inputText + "'),  "
                    + "document.getElementById('" + outputText + "'), "
                    + "document.getElementById('" + referenceText + "'), "
                    + "document.getElementById('" + warningSpan + "'), "
                    + "\"" + basePage.SiteRoot + "/Product/Services/ProductUrlSuggestService.ashx" + "\""
                    + "); " + focusScript;

                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), inputText + "urlscript", hookupInputScript, true);
            }
        }

        #region Custom fields

        private List<ProductProperty> productProperties = new List<ProductProperty>();
        private List<CustomField> lstCustomFields = new List<CustomField>();
        private List<CustomFieldOption> lstColorOptions = new List<CustomFieldOption>();

        private void BindCustomFields(int productId)
        {
            if (lstCustomFields.Count > 0)
            {
                List<CustomField> lstCustomFieldsExcludedColorIfNeeded = new List<CustomField>();
                foreach (CustomField field in lstCustomFields)
                {
                    if (field.FieldType != (int)CustomFieldType.Color)
                        lstCustomFieldsExcludedColorIfNeeded.Add(field);
                    else if (
                            field.FilterType != (int)CustomFieldFilterType.NotAllowFiltering // allow filtering
                            || field.Options > 0
                            )
                        lstCustomFieldsExcludedColorIfNeeded.Add(field);
                }

                if (lstCustomFieldsExcludedColorIfNeeded.Count > 0)
                {
                    liCustomField.Visible = true;
                    tabCustomField.Visible = true;

                    productProperties = ProductProperty.GetPropertiesByProduct(productId);
                    rptCustomFields.DataSource = lstCustomFieldsExcludedColorIfNeeded;
                    rptCustomFields.DataBind();
                    if (tabChildProduct.Visible)
                    {
                        productChild.CustomFields = lstCustomFields;
                        productChild.ProductProperties = productProperties;
                        productChild.ColorOptions = lstColorOptions;
                    }
                }
            }
        }

        private void LoadCustomFields()
        {
            if (product == null)
                return;

            gbSiteMapNode gbNode = SiteUtils.GetSiteMapNodeByZoneId(siteSettings.SiteId, product.ZoneId);
            if (gbNode == null)
                return;

            lstCustomFields = CustomField.GetActiveByZone(siteSettings.SiteId, Product.FeatureGuid, gbNode.ZoneGuid);

            int fieldColorId = -1;
            foreach (CustomField field in lstCustomFields)
            {
                if (field.FieldType == (int)CustomFieldType.Color)
                {
                    fieldColorId = field.CustomFieldId;
                    break;
                }
            }

            if (fieldColorId > 0)
                lstColorOptions = CustomFieldOption.GetByCustomField(fieldColorId);
        }

        private void SaveProductProperties(int productId)
        {
            if (productId <= 0)
                return;

            ProductProperty.DeleteByProduct(product.ProductId);
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
                                    ProductId = productId,
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
                                    ProductId = productId,
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
                                            ProductId = productId,
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
                                            ProductId = productId,
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

        private void RpProductServiceFee_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var hdServiceFeeGuid = new Guid(((HiddenField)e.Item.FindControl("hdServiceFeeGuid")).Value);
                var type = Convert.ToInt32(((HiddenField)e.Item.FindControl("hdType")).Value);
                var gbLabelServiceTitle = (CanhCam.Web.Controls.SiteLabel)e.Item.FindControl("gbLabelServiceTitle");
                gbLabelServiceTitle.Text = ResourceHelper.GetResourceString("ProductResources", "ProductServiceType-" + type);
            }
        }

        private void rptCustomFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
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

        #endregion Custom fields

        #region Attribute

        private string selAttribute = string.Empty;

        private void BindAttribute()
        {
            btnAttributeUp.Visible = false;
            btnAttributeDown.Visible = false;
            btnDeleteAttribute.Visible = false;

            if (product != null)
            {
                lbAttribute.Items.Clear();

                lbAttribute.Items.Add(new ListItem(ProductResources.AttributeNewLabel, ""));
                lbAttribute.DataSource = ContentAttribute.GetByContentAsc(product.ProductGuid);
                lbAttribute.DataBind();

                ListItem li = lbAttribute.Items.FindByValue(selAttribute);
                if (li != null)
                {
                    lbAttribute.ClearSelection();
                    li.Selected = true;
                }

                LanguageHelper.PopulateTab(tabAttributeLanguage, false);
            }
        }

        private void MoveUpDown(string direction)
        {
            if (product == null || product.ProductGuid == Guid.Empty)
                return;

            List<ContentAttribute> listAttribute = ContentAttribute.GetByContentAsc(product.ProductGuid);
            ContentAttribute attribute = null;
            if (lbAttribute.SelectedIndex > 0)
            {
                int delta;

                if (direction == "down")
                    delta = 3;
                else
                    delta = -3;

                attribute = listAttribute[lbAttribute.SelectedIndex - 1];
                attribute.DisplayOrder += delta;

                ContentAttribute.ResortAttribute(listAttribute);

                selAttribute = attribute.Guid.ToString();

                BindAttribute();
                PopulateAttributeControls();
            }
        }

        private void btnUpDown_Click(Object sender, EventArgs e)
        {
            string direction = ((LinkButton)sender).CommandName;
            MoveUpDown(direction);
        }

        private void lbAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateAttributeControls();
        }

        private void btnAttributeUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canUpdate)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                if (product != null && product.ProductId > 0 && lbAttribute.SelectedIndex > -1)
                {
                    ContentAttribute attribute = null;

                    if (lbAttribute.SelectedValue.Length > 0)
                        attribute = new ContentAttribute(new Guid(lbAttribute.SelectedValue));

                    bool isUpdate = true;
                    if (attribute == null || attribute.Guid == Guid.Empty)
                    {
                        attribute = new ContentAttribute
                        {
                            SiteGuid = siteSettings.SiteGuid,
                            ContentGuid = product.ProductGuid,
                            DisplayOrder = ContentAttribute.GetNextSortOrder(product.ProductGuid)
                        };

                        isUpdate = false;
                    }

                    if (!IsAttributeLanguageTab())
                    {
                        attribute.Title = txtAttributeTitle.Text;
                        attribute.ContentText = edAttributeContent.Text;
                    }

                    if (attribute.Save())
                        SaveAttributeContentLanguage(attribute.Guid);

                    LogActivity.Write("Update product attribute", txtAttributeTitle.Text);

                    selAttribute = attribute.Guid.ToString();

                    BindAttribute();
                    PopulateAttributeControls();

                    product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);
                    SiteUtils.QueueIndexing();

                    if (isUpdate)
                        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                    else
                        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnDeleteAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canDelete)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                if (product != null && lbAttribute.SelectedValue.Length > 0)
                {
                    Guid guid = new Guid(lbAttribute.SelectedValue);
                    if (guid != Guid.Empty)
                    {
                        ContentLanguage.DeleteByContent(guid);
                        ContentAttribute.Delete(guid);
                        LogActivity.Write("Delete product attribute", lbAttribute.SelectedItem.Text);

                        product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);
                        SiteUtils.QueueIndexing();
                    }

                    selAttribute = lbAttribute.Items[lbAttribute.SelectedIndex - 1].Value;

                    BindAttribute();
                    PopulateAttributeControls();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void btnDeleteAttributeLanguage_Click(object sender, EventArgs e)
        {
            if (!IsAttributeLanguageTab())
                return;

            if (tabAttributeLanguage.SelectedIndex > 0 && lbAttribute.SelectedValue.Length == 36)
            {
                int languageId = Convert.ToInt32(tabAttributeLanguage.SelectedTab.Value);

                if (languageId > 0)
                {
                    ContentLanguage.Delete(new Guid(lbAttribute.SelectedValue), languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    PopulateAttributeControls();

                    product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);
                    SiteUtils.QueueIndexing();
                }
            }
        }

        protected void tabAttributeLanguage_TabClick(object sender, RadTabStripEventArgs e)
        {
            PopulateAttributeControls();
        }

        private void PopulateAttributeControls()
        {
            btnAttributeUp.Visible = false;
            btnAttributeDown.Visible = false;

            if (lbAttribute.SelectedValue.Length > 0)
            {
                if (lbAttribute.Items.Count > 2)
                {
                    if (lbAttribute.SelectedIndex == 1)
                        btnAttributeDown.Visible = canUpdate;
                    else if (lbAttribute.SelectedIndex == (lbAttribute.Items.Count - 1))
                        btnAttributeUp.Visible = canUpdate;
                    else
                    {
                        btnAttributeDown.Visible = canUpdate;
                        btnAttributeUp.Visible = canUpdate;
                    }
                }

                btnDeleteAttribute.Visible = canDelete;
                btnAttributeUpdate.Text = ResourceHelper.GetResourceString("Resource", "UpdateButton");

                if (tabAttributeLanguage.Visible && tabAttributeLanguage.Tabs.Count == 1)
                    LanguageHelper.PopulateTab(tabAttributeLanguage, lbAttribute.SelectedValue.Length > 0);
            }
            else
            {
                btnDeleteAttribute.Visible = false;
                btnAttributeUpdate.Text = ResourceHelper.GetResourceString("Resource", "InsertButton");

                if (tabAttributeLanguage.Visible && tabAttributeLanguage.Tabs.Count != 1)
                    LanguageHelper.PopulateTab(tabAttributeLanguage, lbAttribute.SelectedValue.Length > 0);
            }

            PopulateDataLanguage(tabAttributeLanguage.SelectedTab);
        }

        private void PopulateDataLanguage(RadTab tab)
        {
            txtAttributeTitle.Text = string.Empty;
            edAttributeContent.Text = string.Empty;
            btnDeleteAttributeLanguage.Visible = false;

            if (lbAttribute.SelectedValue.Length > 0)
            {
                ContentAttribute attribute = new ContentAttribute(new Guid(lbAttribute.SelectedValue));

                if (attribute == null || attribute.Guid == Guid.Empty)
                    return;

                if (IsAttributeLanguageTab())
                {
                    ContentLanguage content = new ContentLanguage(attribute.Guid, Convert.ToInt32(tab.Value));
                    if (content != null && content.Guid != Guid.Empty)
                    {
                        txtAttributeTitle.Text = content.Title;
                        edAttributeContent.Text = content.FullContent;
                        btnDeleteAttributeLanguage.Visible = true;
                    }
                }
                else
                {
                    txtAttributeTitle.Text = attribute.Title;
                    edAttributeContent.Text = attribute.ContentText;
                }
            }
        }

        private bool IsAttributeLanguageTab()
        {
            if (tabAttributeLanguage.Visible && tabAttributeLanguage.SelectedIndex > 0)
                return true;

            return false;
        }

        private void SaveAttributeContentLanguage(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsAttributeLanguageTab())
                return;

            int languageID = -1;
            if (tabAttributeLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabAttributeLanguage.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);

            if (txtAttributeTitle.Text.Length > 0 || edAttributeContent.Text.Length > 0)
            {
                content.LanguageId = languageID;
                content.ContentGuid = contentGuid;
                content.SiteGuid = siteSettings.SiteGuid;
                content.Title = txtAttributeTitle.Text.Trim();
                content.FullContent = edAttributeContent.Text;
                content.Save();
            }
        }

        #endregion Attribute

        #region Images

        protected void btnUpdateImage_Click(object sender, EventArgs e)
        {
            if (product == null) return;

            if (txtVideoPath.Text.ToLower().Contains("youtu") || txtVideoPath.Text.ToLower().EndsWith(".mp4"))
            {
                ContentMedia image = new ContentMedia
                {
                    SiteGuid = siteSettings.SiteGuid,
                    ContentGuid = product.ProductGuid,
                    DisplayOrder = 0,
                    MediaType = (int)ProductMediaType.Video,
                    MediaFile = txtVideoPath.Text.Trim()
                };
                image.Save();

                txtVideoPath.Text = string.Empty;
            }

            //txtImageTitle.Text = txtImageTitle.Text.Trim();
            ProductHelper.VerifyProductFolders(fileSystem, imageFolderPath);

            foreach (UploadedFile file in uplImageFile.UploadedFiles)
            {
                string ext = file.GetExtension();
                if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                {
                    ContentMedia media = new ContentMedia
                    {
                        SiteGuid = siteSettings.SiteGuid,
                        ContentGuid = product.ProductGuid,
                        //image.Title = txtImageTitle.Text;
                        DisplayOrder = 0
                    };

                    string newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                    string newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                    if (media.MediaFile == newFileName)
                    {
                        // an existing image delete the old one
                        fileSystem.DeleteFile(newImagePath);
                    }
                    else
                    {
                        // this is a new newsImage instance, make sure we don't use the same file name as any other instance
                        int i = 1;
                        while (fileSystem.FileExists(VirtualPathUtility.Combine(imageFolderPath, newFileName)))
                        {
                            newFileName = i.ToInvariantString() + newFileName;
                            i += 1;
                        }
                    }

                    newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                    file.SaveAs(Server.MapPath(newImagePath));

                    media.MediaFile = newFileName;
                    media.ThumbnailFile = newFileName;
                    media.ThumbNailWidth = displaySettings.ThumbnailWidth;
                    media.ThumbNailHeight = displaySettings.ThumbnailHeight;
                    media.Save();

                    ProductHelper.ProcessImage(media, fileSystem, imageFolderPath, file.FileName, ProductHelper.GetColor(displaySettings.ResizeBackgroundColor));
                }
            }

            foreach (GridDataItem data in grid.Items)
            {
                Guid guid = new Guid(data.GetDataKeyValue("Guid").ToString());
                int languageId = (int)data.GetDataKeyValue("LanguageId");
                int displayOrder = (int)data.GetDataKeyValue("DisplayOrder");
                string title = data.GetDataKeyValue("Title").ToString();
                string mediaFile = data.GetDataKeyValue("MediaFile").ToString();

                TextBox txtDisplayOrder = (TextBox)data.FindControl("txtDisplayOrder");
                DropDownList ddlLanguage = (DropDownList)data.FindControl("ddlLanguage");
                TextBox txtTitle = (TextBox)data.FindControl("txtTitle");
                TextBox txtVideoPathGrid = (TextBox)data.FindControl("txtVideoPath");
                RadAsyncUpload fupThumbnail = (RadAsyncUpload)data.FindControl("fupThumbnail");
                RadAsyncUpload fupImageFile = (RadAsyncUpload)data.FindControl("fupImageFile");

                DropDownList ddlMediaType = (DropDownList)data.FindControl("ddlMediaType");
                int mediaType = Convert.ToInt32(data.GetDataKeyValue("MediaType"));

                int displayOrderNew = displayOrder;
                int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                int languageIdNew = languageId;
                if (ddlLanguage.SelectedValue.Length > 0)
                    int.TryParse(ddlLanguage.SelectedValue, out languageIdNew);

                if (
                    displayOrder != displayOrderNew
                    || languageId != languageIdNew
                    || title != txtTitle.Text.Trim()
                    || fupImageFile.UploadedFiles.Count > 0
                    || fupThumbnail.UploadedFiles.Count > 0
                    || (txtVideoPathGrid.Text.Trim().Length > 0 && mediaFile != txtVideoPathGrid.Text)
                    || (ddlMediaType != null && ddlMediaType.Visible && ddlMediaType.SelectedValue.Length > 0 && mediaType.ToString() != ddlMediaType.SelectedValue)
                    )
                {
                    ContentMedia media = new ContentMedia(guid);
                    if (media != null && media.Guid != Guid.Empty)
                    {
                        media.Title = txtTitle.Text.Trim();
                        media.DisplayOrder = displayOrderNew;
                        media.LanguageId = languageIdNew;

                        if (ddlMediaType != null && ddlMediaType.Visible)
                            media.MediaType = Convert.ToInt32(ddlMediaType.SelectedValue);

                        if (txtVideoPathGrid.Text.ToLower().Contains("youtu") || txtVideoPathGrid.Text.ToLower().EndsWith(".mp4"))
                            media.MediaFile = txtVideoPathGrid.Text.Trim();

                        if (fupImageFile.UploadedFiles.Count > 0)
                        {
                            UploadedFile file = fupImageFile.UploadedFiles[0];

                            string ext = file.GetExtension();
                            if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                            {
                                string newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                                string newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                                if (media.MediaFile == newFileName)
                                {
                                    // an existing image delete the old one
                                    fileSystem.DeleteFile(newImagePath);
                                }
                                else
                                {
                                    // this is a new newsImage instance, make sure we don't use the same file name as any other instance
                                    int i = 1;
                                    while (fileSystem.FileExists(VirtualPathUtility.Combine(imageFolderPath, newFileName)))
                                    {
                                        newFileName = i.ToInvariantString() + newFileName;
                                        i += 1;
                                    }
                                }

                                newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                                //updating with a new image so delete the previous version
                                fileSystem.DeleteFile(imageFolderPath + media.MediaFile);

                                file.SaveAs(Server.MapPath(newImagePath));

                                media.MediaFile = newFileName;
                            }
                        }
                        if (fupThumbnail.UploadedFiles.Count > 0)
                        {
                            UploadedFile file = fupThumbnail.UploadedFiles[0];

                            string ext = file.GetExtension();
                            if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                            {
                                string newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                                string newImagePath = VirtualPathUtility.Combine(thumbnailImageFolderPath, newFileName);

                                if (media.ThumbnailFile == newFileName)
                                {
                                    // an existing image delete the old one
                                    fileSystem.DeleteFile(newImagePath);
                                }
                                else
                                {
                                    // this is a new newsImage instance, make sure we don't use the same file name as any other instance
                                    int i = 1;
                                    while (fileSystem.FileExists(VirtualPathUtility.Combine(thumbnailImageFolderPath, newFileName)))
                                    {
                                        newFileName = i.ToInvariantString() + newFileName;
                                        i += 1;
                                    }
                                }

                                newImagePath = VirtualPathUtility.Combine(thumbnailImageFolderPath, newFileName);

                                //updating with a new image so delete the previous version
                                fileSystem.DeleteFile(imageFolderPath + "thumbs/" + media.ThumbnailFile);

                                file.SaveAs(Server.MapPath(newImagePath));

                                media.ThumbnailFile = newFileName;
                            }
                        }

                        media.Save();
                    }
                }
            }

            grid.Rebind();
            updImages.Update();
        }

        protected void btnDeleteImage_Click(Object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = false;
                foreach (GridDataItem data in grid.SelectedItems)
                {
                    Guid guid = new Guid(data.GetDataKeyValue("Guid").ToString());

                    ContentMedia media = new ContentMedia(guid);
                    if (media != null && media.Guid != Guid.Empty)
                    {
                        ProductHelper.DeleteImages(media, fileSystem, imageFolderPath);
                        ContentMedia.Delete(guid);

                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    grid.Rebind();
                    updImages.Update();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<ContentMedia> listMedia = null;

            if (product != null)
            {
                if (WebConfigSettings.AllowMultiLanguage)
                {
                    listLanguages = LanguageHelper.GetPublishedLanguages();
                }

                listMedia = ContentMedia.GetByContentDesc(product.ProductGuid);
                grid.DataSource = listMedia;

                if (listMedia.Count > 0)
                    btnDeleteImage.Visible = true;
            }

            if (listLanguages.Count > 1)
                grid.MasterTableView.GetColumn("LanguageID").Visible = true;
            else
                grid.MasterTableView.GetColumn("LanguageID").Visible = false;

            if (lstColorOptions.Count > 0 && displaySettings.EnableColorSwitcher)
                grid.MasterTableView.GetColumn("MediaType").Visible = true;
            else
                grid.MasterTableView.GetColumn("MediaType").Visible = false;
        }

        private List<Language> listLanguages = new List<Language>();

        protected void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridDataItem)
            {
                var item = (GridDataItem)e.Item;

                DropDownList ddlLanguage = (DropDownList)item.FindControl("ddlLanguage");
                string languageId = item.GetDataKeyValue("LanguageId").ToString();
                if (ddlLanguage != null && listLanguages.Count > 1)
                {
                    ddlLanguage.Items.Clear();
                    ddlLanguage.DataSource = listLanguages;
                    ddlLanguage.DataBind();

                    ddlLanguage.Items.Insert(0, new ListItem(ProductResources.ProductEditLanguageNotCareLabel, "-1"));

                    ListItem li = ddlLanguage.Items.FindByValue(languageId);
                    if (li != null)
                    {
                        ddlLanguage.ClearSelection();
                        li.Selected = true;
                    }
                }

                MediaElement ml2 = (MediaElement)item.FindControl("ml2");
                RadAsyncUpload fupImageFile = (RadAsyncUpload)item.FindControl("fupImageFile");
                TextBox txtVideoPath = (TextBox)item.FindControl("txtVideoPath");
                FileBrowserTextBoxExtender videoFileBrowser = (FileBrowserTextBoxExtender)item.FindControl("VideoFileBrowser");
                if (videoFileBrowser != null && txtVideoPath != null)
                    videoFileBrowser.TextBoxClientId = txtVideoPath.ClientID;

                int mediaType = Convert.ToInt32(item.GetDataKeyValue("MediaType"));
                if (mediaType != (int)ProductMediaType.Video)
                {
                    ml2.Visible = true;
                    fupImageFile.Visible = true;
                    txtVideoPath.Visible = false;
                    videoFileBrowser.Visible = false;
                }
                else
                {
                    ml2.Visible = false;
                    fupImageFile.Visible = false;
                    txtVideoPath.Visible = true;
                    videoFileBrowser.Visible = true;
                }

                DropDownList ddlMediaType = (DropDownList)item.FindControl("ddlMediaType");
                if (
                    ddlMediaType != null
                    && lstColorOptions.Count > 0
                    && mediaType != (int)ProductMediaType.Video
                    )
                {
                    ddlMediaType.Visible = true;

                    ddlMediaType.Items.Clear();
                    ddlMediaType.DataSource = lstColorOptions;
                    ddlMediaType.DataBind();

                    ddlMediaType.Items.Insert(0, new ListItem(ProductResources.ProductColorLabel, "0"));

                    ListItem li = ddlMediaType.Items.FindByValue(mediaType.ToString());
                    if (li != null)
                    {
                        ddlMediaType.ClearSelection();
                        li.Selected = true;
                    }
                }
            }
        }

        #endregion Images

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            basePage = Page as CmsBasePage;
            SiteUtils.SetupEditor(edFullContent, basePage.AllowSkinOverride, Page);
            SiteUtils.SetupEditor(edBriefContent, basePage.AllowSkinOverride, Page);
            SiteUtils.SetupEditor(edAttributeContent, basePage.AllowSkinOverride, Page);

            this.Load += new EventHandler(this.Page_Load);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndNew.Click += new EventHandler(btnInsertAndNew_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);


            btnDeleteAttribute.Click += new EventHandler(btnDeleteAttribute_Click);
            btnAttributeUpdate.Click += new EventHandler(btnAttributeUpdate_Click);
            lbAttribute.SelectedIndexChanged += new EventHandler(lbAttribute_SelectedIndexChanged);
            btnAttributeUp.Click += new EventHandler(btnUpDown_Click);
            btnAttributeDown.Click += new EventHandler(btnUpDown_Click);

            rptCustomFields.ItemDataBound += new RepeaterItemEventHandler(rptCustomFields_ItemDataBound);
            rpProductServiceFee.ItemDataBound += new RepeaterItemEventHandler(RpProductServiceFee_ItemDataBound);
        }

        #endregion OnInit
    }
}
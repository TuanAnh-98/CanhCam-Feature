using CanhCam.Business;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class ManufacturerEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ManufacturerEditPage));

        private int manufacturerId = -1;
        private Manufacturer manufacturer = null;
        private List<ZoneItem> lstZoneItems = new List<ZoneItem>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadParams();
            LoadSettings();

            if (!ProductPermission.CanManageManufacturer)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            SetupScripts();

            if ((!Page.IsPostBack) && (!Page.IsCallback))
            {
                BindEnum();
                PopulatePageList();
                PopulateZoneList();
                PopulateControls();
            }
        }

        protected virtual void PopulateControls()
        {
            PrimaryImageFileBrowser.TextBoxClientId = txtPrimaryImage.ClientID;
            PrimaryImageFileBrowser.Text = ResourceHelper.GetResourceString("Resource", "FileBrowserLink");

            SecondImageFileBrowser.TextBoxClientId = txtSecondImage.ClientID;
            SecondImageFileBrowser.Text = ResourceHelper.GetResourceString("Resource", "FileBrowserLink");

            if (manufacturer != null)
            {
                txtName.Text = manufacturer.Name;
                txtUrl.Text = manufacturer.Url;
                edDescription.Text = manufacturer.Description;
                txtMetaDescription.Text = manufacturer.MetaDescription;
                txtMetaKeywords.Text = manufacturer.MetaKeywords;
                txtMetaTitle.Text = manufacturer.MetaTitle;
                chkIsPublished.Checked = manufacturer.IsPublished;
                txtPrimaryImage.Text = manufacturer.PrimaryImage;
                txtSecondImage.Text = manufacturer.SecondImage;

                if (manufacturer.PrimaryImage.Length > 0)
                {
                    imgPrimaryImage.ImageUrl = manufacturer.PrimaryImage;
                    imgPrimaryImage.Visible = true;
                }

                if (manufacturer.SecondImage.Length > 0)
                {
                    imgSecondImage.ImageUrl = manufacturer.SecondImage;
                    imgSecondImage.Visible = true;
                }

                if (divShowOption.Visible)
                {
                    foreach (ListItem option in chlShowOption.Items)
                    {
                        option.Selected = ((manufacturer.ShowOption & Convert.ToInt16(option.Value)) > 0);
                    }
                }

                hdnTitle.Value = txtName.Text;

                var listItem = ddPages.Items.FindByValue(manufacturer.PageId.ToString());
                if (listItem != null)
                {
                    ddPages.ClearSelection();
                    listItem.Selected = true;
                }

                if (lstZoneItems.Count > 0)
                {
                    cobZones.ClearSelection();

                    foreach (ListItem li in cobZones.Items)
                    {
                        foreach (ZoneItem item in lstZoneItems)
                        {
                            if (li.Value == item.ZoneGuid.ToString())
                                li.Selected = true;
                        }
                    }
                }
            }
            else
                this.btnDelete.Visible = false;

            if ((txtUrl.Text.Length == 0) && (txtName.Text.Length > 0))
            {
                String friendlyUrl = SiteUtils.SuggestFriendlyUrl(txtName.Text, siteSettings);

                txtUrl.Text = "~/" + friendlyUrl;
            }

            bool isLoadAll = (manufacturer != null && manufacturer.ManufacturerId > 0);
            LanguageHelper.PopulateTab(tabLanguage, isLoadAll);
            LanguageHelper.PopulateTab(tabLanguageSEO, isLoadAll);
        }

        private void PopulatePageList()
        {
            ddPages.Items.Clear();

            if (displaySettings.HasDetailPage)
            {
                var listPages = PageSettings.GetPageList(siteSettings.SiteId);
                foreach (var pageSettings in listPages)
                {
                    if (!pageSettings.IsPending)
                    {
                        var li = new ListItem(pageSettings.PageName, pageSettings.PageId.ToString());
                        ddPages.Items.Add(li);
                    }
                }
            }

            ddPages.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "PageSettingsChoosePage"), "-1"));
        }

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(cobZones, true, Product.FeatureGuid);
        }

        #endregion Populate Zone List

        private void BindEnum()
        {
            try
            {
                chlShowOption.DataValueField = "Value";
                chlShowOption.DataTextField = "Name";
                chlShowOption.DataSource = EnumDefined.LoadFromConfigurationXml("product", "manufactureroption", "value");
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
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx?ManufacturerID=" + nId.ToString());
        }

        private void btnInsertAndClose_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Manufacturers.aspx");
        }

        private void btnInsertAndNew_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx?ManufacturerID=" + nId.ToString());
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Manufacturers.aspx");
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ManufacturerEdit.aspx");
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (manufacturer != null && !manufacturer.IsDeleted)
            {
                ContentDeleted.Create(siteSettings.SiteId, manufacturer.Name, "Manufacturer", typeof(ManufacturerDeleted).AssemblyQualifiedName, manufacturer.ManufacturerId.ToString(), Page.User.Identity.Name);
                manufacturer.IsDeleted = true;
                manufacturer.Save();

                LogActivity.Write("Delete manufacturer", manufacturer.Name);

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Manufacturers.aspx");
            }
        }

        private int Save()
        {
            Page.Validate("manufacturer");

            if (!Page.IsValid)
                return -1;

            if (manufacturer == null)
            {
                manufacturer = new Manufacturer(manufacturerId)
                {
                    SiteId = siteSettings.SiteId
                };
            }
            if (manufacturer.ManufacturerId == -1
                && ConfigHelper.GetBoolProperty("Manufacturer:EnableValidateExistByTitle", false)
                && Manufacturer.GetCountByTitle(siteSettings.SiteId, txtName.Text.Trim()) > 0)
            {
                message.WarningMessage = "Thương hiệu '" + txtName.Text.Trim() + "' đã tồn tại trong hệ thống";
                return -1;
            }
            //warehouse.ZoneId = Convert.ToInt32(ddZones.SelectedValue);
            manufacturer.SiteId = siteSettings.SiteId;

            manufacturer.IsPublished = chkIsPublished.Checked;
            manufacturer.PrimaryImage = txtPrimaryImage.Text;
            manufacturer.SecondImage = txtSecondImage.Text;

            manufacturer.ShowOption = chlShowOption.Items.SelectedItemsToBinaryOrOperator();
            manufacturer.PageId = Convert.ToInt32(ddPages.SelectedValue);

            var oldUrl = string.Empty;
            var newUrl = string.Empty;
            var friendlyUrlString = string.Empty;
            var friendlyUrl = (FriendlyUrl)null;
            if (!IsLanguageTab())
            {
                manufacturer.Name = txtName.Text.Trim();
                manufacturer.Description = edDescription.Text;

                if (displaySettings.HasDetailPage)
                {
                    if (txtUrl.Text.Length == 0)
                        txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(txtName.Text, siteSettings);

                    friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtUrl.Text.Replace("~/", String.Empty));

                    if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                        friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

                    friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

                    if (
                        ((friendlyUrl.FoundFriendlyUrl) && (friendlyUrl.PageGuid != manufacturer.Guid))
                        && (manufacturer.Url != txtUrl.Text.Trim())
                        && (!txtUrl.Text.StartsWith("http"))
                        )
                    {
                        message.ErrorMessage = ProductResources.PageUrlInUseErrorMessage;
                        return -1;
                    }

                    oldUrl = manufacturer.Url.Replace("~/", string.Empty);
                    newUrl = friendlyUrlString;

                    if (txtUrl.Text.Trim().StartsWith("http"))
                        manufacturer.Url = txtUrl.Text.Trim();
                    else if (friendlyUrlString.Length > 0)
                        manufacturer.Url = "~/" + friendlyUrlString;
                    else if (friendlyUrlString.Length == 0)
                        manufacturer.Url = string.Empty;
                }
            }
            if (!IsLanguageSeoTab())
            {
                manufacturer.MetaDescription = txtMetaDescription.Text;
                manufacturer.MetaKeywords = txtMetaKeywords.Text;
                manufacturer.MetaTitle = txtMetaTitle.Text;
            }

            if (manufacturer.Save())
            {
                SaveContentLanguage(manufacturer.Guid);

                if (displaySettings.HasDetailPage)
                    SaveContentLanguageSEO(manufacturer.Guid);

                foreach (ListItem li in cobZones.Items)
                {
                    Guid itemGuid = Guid.Empty;
                    foreach (ZoneItem item in lstZoneItems)
                    {
                        if (li.Value == item.ZoneGuid.ToString())
                        {
                            itemGuid = item.ItemGuid;
                            break;
                        }
                    }

                    if (li.Selected)
                    {
                        if (itemGuid == Guid.Empty)
                        {
                            ZoneItem zoneItem = new ZoneItem
                            {
                                ItemGuid = manufacturer.Guid,
                                ZoneGuid = new Guid(li.Value)
                            };
                            zoneItem.Save();
                        }
                    }
                    else
                    {
                        if (itemGuid != Guid.Empty)
                            ZoneItem.Delete(new Guid(li.Value), itemGuid);
                    }
                }

                if (manufacturerId > 0)
                {
                    LogActivity.Write("Update manufacturer", manufacturer.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
                else
                {
                    LogActivity.Write("Create new manufacturer", manufacturer.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }
            }

            if (displaySettings.HasDetailPage && !IsLanguageTab())
            {
                if (
                    (oldUrl.Length > 0)
                    && (newUrl.Length > 0)
                    && (!SiteUtils.UrlsMatch(oldUrl, newUrl))
                    )
                {
                    FriendlyUrl oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                    if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.PageGuid == manufacturer.Guid))
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
                                PageGuid = manufacturer.Guid,
                                Url = friendlyUrlString,
                                RealUrl = "~/Product/Manufacturer.aspx"
                                + "?ManufacturerID=" + manufacturer.ManufacturerId.ToInvariantString()
                            };

                            newFriendlyUrl.Save();
                        }
                    }
                }
            }

            return manufacturer.ManufacturerId;
        }

        #region Language

        protected void tabLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                reqName.Visible = true;

                txtName.Text = manufacturer.Name;
                txtUrl.Text = manufacturer.Url;
                edDescription.Text = manufacturer.Description;
            }
            else
            {
                reqName.Visible = false;

                txtName.Text = string.Empty;
                txtUrl.Text = string.Empty;
                edDescription.Text = string.Empty;

                var content = new ContentLanguage(manufacturer.Guid, Convert.ToInt32(e.Tab.Value));
                if (content != null && content.Guid != Guid.Empty)
                {
                    txtName.Text = content.Title;
                    txtUrl.Text = content.Url;
                    edDescription.Text = content.FullContent;
                }
            }

            upButton.Update();
            hdnTitle.Value = txtName.Text;
        }

        protected void tabLanguageSEO_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                txtMetaKeywords.Text = manufacturer.MetaKeywords;
                txtMetaDescription.Text = manufacturer.MetaDescription;
                txtMetaTitle.Text = manufacturer.MetaTitle;
            }
            else
            {
                txtMetaKeywords.Text = "";
                txtMetaDescription.Text = "";
                txtMetaTitle.Text = "";

                ContentLanguage content = new ContentLanguage(manufacturer.Guid, Convert.ToInt32(e.Tab.Value));
                if (content != null && content.Guid != Guid.Empty)
                {
                    txtMetaKeywords.Text = content.MetaKeywords;
                    txtMetaDescription.Text = content.MetaDescription;
                    txtMetaTitle.Text = content.MetaTitle;
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
            var name = txtName.Text.Trim();

            if (name.Length == 0)
                return;

            var oldUrl = string.Empty;
            var newUrl = string.Empty;
            var friendlyUrlString = string.Empty;
            var friendlyUrl = (FriendlyUrl)null;
            if (displaySettings.HasDetailPage)
            {
                if (txtUrl.Text.Length == 0)
                    txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(name, siteSettings);

                if (txtUrl.Text.Length == 0 || (txtUrl.Text == "~/" && manufacturer.Url != "~/"))
                    txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(manufacturer.Name + " " + tabLanguage.SelectedTab.Text, siteSettings);

                friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtUrl.Text.Replace("~/", String.Empty));
                if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                    friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

                friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

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

                oldUrl = content.Url.Replace("~/", string.Empty);
                newUrl = friendlyUrlString;
                if ((txtUrl.Text.StartsWith("http")) || (txtUrl.Text.Trim() == "~/"))
                    content.Url = txtUrl.Text.Trim();
                else if (friendlyUrlString.Length > 0)
                    content.Url = "~/" + friendlyUrlString;
                else if (friendlyUrlString.Length == 0)
                    content.Url = string.Empty;
            }

            content.FullContent = edDescription.Text.Trim();
            content.LanguageId = languageID;
            content.ContentGuid = contentGuid;
            content.SiteGuid = siteSettings.SiteGuid;
            content.Title = name;
            content.Save();

            if (displaySettings.HasDetailPage)
            {
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
                                PageGuid = manufacturer.Guid,
                                ItemGuid = content.Guid,
                                LanguageId = content.LanguageId,
                                Url = friendlyUrlString,
                                RealUrl = "~/Product/Manufacturer.aspx"
                                + "?ManufacturerID=" + manufacturer.ManufacturerId.ToInvariantString()
                            };
                            newFriendlyUrl.Save();
                        }
                    }
                }
            }
        }

        protected void btnDeleteLanguage_Click(object sender, EventArgs e)
        {
            if (!IsLanguageTab())
                return;
            if (tabLanguage.SelectedIndex > 0)
            {
                int languageId = Convert.ToInt32(tabLanguage.SelectedTab.Value);

                if (languageId > 0)
                {
                    FriendlyUrl.DeleteByLanguage(manufacturer.Guid, languageId);
                    ContentLanguage.Delete(manufacturer.Guid, languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

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
            content.Save();
        }

        #endregion Language

        private void PopulateLabels()
        {
            if (manufacturer == null || manufacturer.ManufacturerId == -1)
            {
                heading.Text = ProductResources.ManufacturerAddNewTitle;
                Page.Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
            }
            else
            {
                heading.Text = ProductResources.ManufacturerEditTitle;
                Page.Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
            }

            litContentTab.Text = ProductResources.ContentTab;
            litMetaTab.Text = string.Format("<a aria-controls=\"tabMeta\" role=\"tab\" data-toggle=\"tab\" href=\"#{0}\">{1}</a>", tabMeta.ClientID, ProductResources.MetaTab);

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            btnDelete.Text = ProductResources.ProductEditDeleteButton;
            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductDeleteWarning);

            reqName.ErrorMessage = ProductResources.ManufacturerNameRequiredWarning;
            regexUrl.ErrorMessage = ProductResources.FriendlyUrlRegexWarning;
        }

        private void LoadSettings()
        {
            if (manufacturerId > -1)
            {
                manufacturer = new Manufacturer(manufacturerId);
                if (manufacturer != null && manufacturer.ManufacturerId > 0 && manufacturer.SiteId == siteSettings.SiteId)
                {
                    if (manufacturer.IsDeleted)
                    {
                        SiteUtils.RedirectToEditAccessDeniedPage();
                        return;
                    }

                    lstZoneItems = ZoneItem.GetByItem(manufacturer.Guid);
                }
            }

            HideControls();

            divDescription.Visible = displaySettings.ShowDescription;
            divPages.Visible = displaySettings.HasDetailPage;
            ulTabs.Visible = displaySettings.HasDetailPage;
            tabMeta.Visible = displaySettings.HasDetailPage;
            divUrl.Visible = displaySettings.HasDetailPage;
            divPrimaryImage.Visible = displaySettings.ShowPrimaryImage;
            divSecondImage.Visible = displaySettings.ShowSecondImage;

            AddClassToBody("admin-manufactureredit");
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

            if (manufacturer == null)
            {
                btnInsert.Visible = true;
                btnInsertAndNew.Visible = true;
                btnInsertAndClose.Visible = true;
            }
            else if (manufacturer != null && manufacturer.ManufacturerId > 0)
            {
                btnUpdate.Visible = true;
                btnUpdateAndNew.Visible = true;
                btnUpdateAndClose.Visible = true;
                btnDelete.Visible = true;
            }
        }

        private void LoadParams()
        {
            manufacturerId = WebUtils.ParseInt32FromQueryString("ManufacturerID", manufacturerId);
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

            SetupUrlSuggestScripts(this.txtName.ClientID, this.txtUrl.ClientID, this.hdnTitle.ClientID, this.spnUrlWarning.ClientID);
        }

        private void SetupUrlSuggestScripts(string inputText, string outputText, string referenceText, string warningSpan)
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("friendlyurlsuggest"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "friendlyurlsuggest", "<script src=\""
                    + ResolveUrl("~/ClientScript/friendlyurlsuggest_v2.js") + "\" type=\"text/javascript\"></script>");
            }

            string focusScript = string.Empty;
            if (manufacturerId == -1) { focusScript = "document.getElementById('" + inputText + "').focus();"; }

            string hookupInputScript = "new UrlHelper( "
                + "document.getElementById('" + inputText + "'),  "
                + "document.getElementById('" + outputText + "'), "
                + "document.getElementById('" + referenceText + "'), "
                + "document.getElementById('" + warningSpan + "'), "
                + "\"" + SiteRoot + "/Product/Services/ProductUrlSuggestService.ashx" + "\""
                + "); " + focusScript;

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), inputText + "urlscript", hookupInputScript, true);
        }

        #region OnInits

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, Page);

            this.Load += new EventHandler(this.Page_Load);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndNew.Click += new EventHandler(btnInsertAndNew_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        #endregion OnInit
    }
}
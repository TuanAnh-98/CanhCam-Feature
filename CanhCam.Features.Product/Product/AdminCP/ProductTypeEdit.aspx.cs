using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductTypeEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductTypeEditPage));

        private int productTypeId = -1;
        private ProductType productType = null;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            SiteUtils.SetupEditor(edDescription, AllowSkinOverride, Page);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndNew.Click += new EventHandler(btnInsertAndNew_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
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

            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            PrimaryImageFileBrowser.TextBoxClientId = txtPrimaryImage.ClientID;
            PrimaryImageFileBrowser.Text = ResourceHelper.GetResourceString("Resource", "FileBrowserLink");

            SecondImageFileBrowser.TextBoxClientId = txtSecondImage.ClientID;
            SecondImageFileBrowser.Text = ResourceHelper.GetResourceString("Resource", "FileBrowserLink");
            if (productType != null && productType.ProductTypeId > 0)
            {
                txtName.Text = productType.Name;
                edDescription.Text = productType.Description;
            }
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ProductTypeEdit.aspx?id=" + itemId.ToString());
            }
        }

        private void btnInsertAndClose_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFields.aspx");
            }
        }

        private void btnInsertAndNew_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ProductTypeEdit.aspx");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ProductTypeEdit.aspx?id=" + itemId.ToString());
            }
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFields.aspx");
            }
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/ProductTypeEdit.aspx");
            }
        }

        private int SaveData()
        {
            if (!Page.IsValid) return -1;

            try
            {
                if (productType == null || productType.ProductTypeId == -1)
                {
                    productType = new ProductType();
                }
                productType.PrimaryImage = txtPrimaryImage.Text;
                productType.SecondImage = txtSecondImage.Text;
                if (!IsLanguageTab())
                {
                    productType.Name = txtName.Text.Trim();
                    productType.Description = edDescription.Text;
                }
                if (productType.Save())
                {
                    SaveContentLanguage(productType.ItemGuid);
                    if (productTypeId > 0)
                    {
                        LogActivity.Write("Update product type", productType.Name);
                        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                    }
                    else
                    {
                        LogActivity.Write("Create new  product type", productType.Name);
                        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                    }
                }

                return productType.ProductTypeId;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return -1;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (productType != null && productType.ProductTypeId > -1)
                {
                    ProductType.Delete(productType.ProductTypeId);

                    LogActivity.Write("Delete product type", productType.Name);

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFields.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }


        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductTypeEditTitle);
            heading.Text = ProductResources.ProductTypeEditTitle;

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));
        }

        private void LoadSettings()
        {
            productTypeId = WebUtils.ParseInt32FromQueryString("id", productTypeId);

            if (productTypeId > 0)
                productType = new ProductType(productTypeId);

            HideControls();

            AddClassToBody("product-type-edit");
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


            if (productType == null)
            {
                btnInsert.Visible = true;
                btnInsertAndNew.Visible = true;
                btnInsertAndClose.Visible = true;
            }
            else if (productType != null && productType.ProductTypeId > 0)
            {
                btnUpdate.Visible = true;
                btnUpdateAndNew.Visible = true;
                btnUpdateAndClose.Visible = true;

                btnDelete.Visible = true;
            }


        }



        #region Language

        protected void tabLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            if (e.Tab.Index == 0)
            {

                txtName.Text = productType.Name;
                edDescription.Text = productType.Description;
            }
            else
            {

                txtName.Text = string.Empty;
                edDescription.Text = string.Empty;

                var content = new ContentLanguage(productType.ItemGuid, Convert.ToInt32(e.Tab.Value));
                if (content != null && content.Guid != Guid.Empty)
                {
                    txtName.Text = content.Title;
                    edDescription.Text = content.FullContent;
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
            content.FullContent = edDescription.Text.Trim();
            content.LanguageId = languageID;
            content.ContentGuid = contentGuid;
            content.SiteGuid = siteSettings.SiteGuid;
            content.Title = name;
            content.Save();

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
                    FriendlyUrl.DeleteByLanguage(productType.ItemGuid, languageId);
                    ContentLanguage.Delete(productType.ItemGuid, languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                }
            }
        }


        #endregion Language


    }
}
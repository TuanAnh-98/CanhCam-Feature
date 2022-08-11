/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-04-24
/// Last Modified:		    2015-07-16

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class CustomFieldEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomFieldEditPage));

        private int customFieldId = -1;
        private CustomField field = null;
        private List<ZoneItem> lstZoneItems = new List<ZoneItem>();

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndNew.Click += new EventHandler(btnInsertAndNew_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);

            btnDeleteOption.Click += new EventHandler(btnDeleteOption_Click);
            btnUpdateOption.Click += new EventHandler(btnUpdateOption_Click);
            lbOptions.SelectedIndexChanged += new EventHandler(lbOptions_SelectedIndexChanged);
            btnOptionUp.Click += new EventHandler(btnUpDown_Click);
            btnOptionDown.Click += new EventHandler(btnUpDown_Click);
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
            PopulateZoneList();
            BindDataType();
            BindFilterType();
            BindFieldType();
            BindOptionsEnum();

            bool loadAllTab = false;
            if (field != null && field.CustomFieldId > 0)
            {
                if (field.FieldType == (int)CustomFieldType.Color)
                {
                    divOptionType.Visible = true;
                    BindOptionType();
                }

                BindOptions();
                PopulateOptionControls();

                txtName.Text = field.Name;
                txtDisplayName.Text = field.DisplayName;
                txtInvalidMessage.Text = field.InvalidMessage;

                var liItem = ddlDataType.Items.FindByValue(field.DataType.ToString());
                if (liItem != null)
                {
                    ddlDataType.ClearSelection();
                    liItem.Selected = true;

                    ddlDataType.Enabled = false;
                }

                liItem = ddlFilterType.Items.FindByValue(field.FilterType.ToString());
                if (liItem != null)
                {
                    ddlFilterType.ClearSelection();
                    liItem.Selected = true;
                }

                liItem = ddlFieldType.Items.FindByValue(field.FieldType.ToString());
                if (liItem != null)
                {
                    ddlFieldType.ClearSelection();
                    liItem.Selected = true;

                    if (liItem.Value != "-1")
                        ddlFieldType.Enabled = false;
                }

                if (field.Options > 0)
                {
                    foreach (ListItem option in chlOptions.Items)
                    {
                        option.Selected = ((field.Options & Convert.ToInt16(option.Value)) > 0);
                    }
                }

                chkIsEnabled.Checked = field.IsEnabled;
                chkIsRequired.Checked = field.IsRequired;
                chkAllowComparing.Checked = field.AllowComparing;
                txtValidationExpression.Text = field.ValidationExpression;

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

                loadAllTab = true;
            }

            LanguageHelper.PopulateTab(tabLanguage, loadAllTab);
        }

        private void BindDataType()
        {
            ddlDataType.Items.Clear();
            foreach (var item in typeof(CustomFieldDataType).GetFields())
            {
                if (item.FieldType == typeof(CustomFieldDataType))
                    ddlDataType.Items.Add(new ListItem { Text = ResourceHelper.GetResourceString("ProductResources", "CustomField" + item.Name + "DataType"), Value = item.GetRawConstantValue().ToString() });
            }
        }

        private void BindFilterType()
        {
            ddlFilterType.Items.Clear();
            foreach (var item in typeof(CustomFieldFilterType).GetFields())
            {
                if (item.FieldType == typeof(CustomFieldFilterType))
                    ddlFilterType.Items.Add(new ListItem { Text = ResourceHelper.GetResourceString("ProductResources", "CustomField" + item.Name + "FilterType"), Value = item.GetRawConstantValue().ToString() });
            }

            //ddlFilterType.Items.Add(new ListItem { Text = "", Value = "-1" });
        }

        private void BindFieldType()
        {
            ddlFieldType.Items.Clear();
            foreach (var item in typeof(CustomFieldType).GetFields())
            {
                if (item.FieldType == typeof(CustomFieldType))
                    ddlFieldType.Items.Add(new ListItem { Text = ResourceHelper.GetResourceString("ProductResources", "CustomField" + item.Name + "Type"), Value = item.GetRawConstantValue().ToString() });
            }
        }

        private void BindOptionsEnum()
        {
            chlOptions.Items.Clear();
            foreach (var item in typeof(CustomFieldOptions).GetFields())
            {
                if (item.FieldType == typeof(CustomFieldOptions))
                {
                    int value = Convert.ToInt32(item.GetRawConstantValue().ToString());
                    if (!ProductConfiguration.EnableComparing && value == (int)CustomFieldOptions.EnableComparing)
                        continue;
                    if (!ProductConfiguration.EnableShoppingCartAttributes && value == (int)CustomFieldOptions.EnableShoppingCart)
                        continue;

                    if (!ProductConfiguration.ShowCustomFieldInDetailsPage && value == (int)CustomFieldOptions.ShowInProductDetailsPage)
                        continue;

                    if (!ProductConfiguration.ShowCustomFieldInCatalogPages && value == (int)CustomFieldOptions.ShowInCatalogPages)
                        continue;

                    chlOptions.Items.Add(new ListItem { Text = ResourceHelper.GetResourceString("ProductResources", "CustomField" + item.Name + "Option"), Value = value.ToString() });
                }
            }

            if (chlOptions.Items.Count == 0)
                divOptions.Visible = false;
        }

        #region Populate Zone List

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(cobZones, true, Product.FeatureGuid);
        }

        #endregion Populate Zone List

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFieldEdit.aspx?id=" + itemId.ToString());
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
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFieldEdit.aspx");
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFieldEdit.aspx?id=" + itemId.ToString());
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
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFieldEdit.aspx");
            }
        }

        private int SaveData()
        {
            if (!Page.IsValid) return -1;

            try
            {
                if (field == null || field.CustomFieldId == -1)
                {
                    field = new CustomField
                    {
                        SiteId = siteSettings.SiteId
                    };
                }

                if (!IsLanguageTab())
                {
                    field.Name = txtName.Text.Trim();
                    field.DisplayName = txtDisplayName.Text.Trim();
                    field.InvalidMessage = txtInvalidMessage.Text.Trim();
                }

                field.FeatureGuid = Product.FeatureGuid;
                field.DataType = Convert.ToInt32(ddlDataType.SelectedValue);
                field.FilterType = Convert.ToInt32(ddlFilterType.SelectedValue);
                field.FieldType = Convert.ToInt32(ddlFieldType.SelectedValue);
                field.Options = chlOptions.Items.SelectedItemsToBinaryOrOperator();
                field.IsEnabled = chkIsEnabled.Checked;
                field.IsRequired = chkIsRequired.Checked;
                field.AllowComparing = chkAllowComparing.Checked;
                field.ValidationExpression = txtValidationExpression.Text.Trim();

                if (field.Save())
                    SaveContentLanguage(field.Guid);

                if (customFieldId > 0)
                {
                    LogActivity.Write("Update custom field", field.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
                else
                {
                    LogActivity.Write("Create new custom field", field.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }

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
                                ItemGuid = field.Guid,
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

                return field.CustomFieldId;
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
                if (field != null && field.CustomFieldId > -1)
                {
                    ContentLanguage.DeleteByContent(field.Guid);
                    ProductProperty.DeleteByCustomField(field.CustomFieldId);
                    CustomFieldOption.DeleteCustomField(field.CustomFieldId);
                    CustomField.Delete(field.CustomFieldId);

                    LogActivity.Write("Delete custom field", field.Name);

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/CustomFields.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #region Language

        protected void tabLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            txtName.Text = string.Empty;
            txtDisplayName.Text = string.Empty;
            txtInvalidMessage.Text = string.Empty;
            btnDeleteLanguage.Visible = false;

            if (field != null && field.CustomFieldId > 0)
            {
                if (e.Tab.Index == 0)
                {
                    txtName.Text = field.Name;
                    txtDisplayName.Text = field.DisplayName;
                    txtInvalidMessage.Text = field.InvalidMessage;
                }
                else
                {
                    ContentLanguage content = new ContentLanguage(field.Guid, Convert.ToInt32(e.Tab.Value));
                    if (content != null && content.Guid != Guid.Empty)
                    {
                        txtName.Text = content.Title;
                        txtDisplayName.Text = content.MetaTitle;
                        txtInvalidMessage.Text = content.ExtraText1;

                        btnDeleteLanguage.Visible = true;
                    }
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

            if (txtName.Text.Length > 0)
            {
                content.LanguageId = languageID;
                content.ContentGuid = contentGuid;
                content.SiteGuid = siteSettings.SiteGuid;
                content.Title = txtName.Text.Trim();
                content.MetaTitle = txtDisplayName.Text.Trim();
                content.ExtraText1 = txtInvalidMessage.Text.Trim();

                content.Save();
            }
        }

        protected void btnDeleteLanguage_Click(object sender, EventArgs e)
        {
            if (!IsLanguageTab())
                return;

            if (tabLanguage.SelectedIndex > 0)
            {
                int languageId = Convert.ToInt32(tabLanguage.SelectedTab.Value);

                if (languageId > 0 && field != null && field.Guid != Guid.Empty)
                {
                    ContentLanguage.Delete(field.Guid, languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    WebUtils.SetupRedirect(this, Request.RawUrl);
                }
            }
        }

        #endregion Language

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.CustomFieldEditTitle);
            heading.Text = ProductResources.CustomFieldEditTitle;

            litTabContent.Text = "<a aria-controls=\"tabContent\" class=\"active\" role=\"tab\" data-toggle=\"tab\" href='#tabContent'>" + ProductResources.CustomFieldsTitle + "</a>";
            litTabOptions.Text = "<a aria-controls=\"" + tabOptions.ClientID + "\" role=\"tab\" data-toggle=\"tab\" href='#" + tabOptions.ClientID + "'>" + ProductResources.CustomFieldOptionsTab + "</a>";

            //btnOptionUp.AlternateText = ProductResources.CustomFieldOptionUpAlternateText;
            btnOptionUp.ToolTip = ProductResources.CustomFieldOptionUpAlternateText;
            //btnOptionUp.ImageUrl = ImageSiteRoot + "/Data/SiteImages/up.gif";

            //btnOptionDown.AlternateText = ProductResources.CustomFieldOptionDownAlternateText;
            btnOptionDown.ToolTip = ProductResources.CustomFieldOptionDownAlternateText;
            //btnOptionDown.ImageUrl = ImageSiteRoot + "/Data/SiteImages/dn.gif";

            //btnDeleteOption.AlternateText = ProductResources.CustomFieldOptionDeleteSelectedButton;
            btnDeleteOption.ToolTip = ProductResources.CustomFieldOptionDeleteSelectedButton;
            //btnDeleteOption.ImageUrl = ImageSiteRoot + "/Data/SiteImages/" + WebConfigSettings.DeleteLinkImage;
            UIHelper.AddConfirmationDialog(btnDeleteOption, ProductResources.CustomFieldOptionDeleteConfirmMessage);
            UIHelper.AddConfirmationDialog(btnDeleteOptionLanguage, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));
            UIHelper.AddConfirmationDialog(btnDeleteLanguage, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));
        }

        private void LoadSettings()
        {
            customFieldId = WebUtils.ParseInt32FromQueryString("id", customFieldId);

            if (customFieldId > 0)
            {
                field = new CustomField(customFieldId);
                if (
                    field != null
                    && field.CustomFieldId > 0
                    && field.SiteId == siteSettings.SiteId
                    )
                    lstZoneItems = ZoneItem.GetByItem(field.Guid);
                else
                    field = null;
            }

            HideControls();

            AddClassToBody("customfields-edit");
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

            ulTabs.Visible = false;
            tabOptions.Visible = false;
            divtabs.Attributes["class"] = string.Empty;

            if (field == null)
            {
                btnInsert.Visible = true;
                btnInsertAndNew.Visible = true;
                btnInsertAndClose.Visible = true;
            }
            else if (field != null && field.CustomFieldId > 0)
            {
                btnUpdate.Visible = true;
                btnUpdateAndNew.Visible = true;
                btnUpdateAndClose.Visible = true;

                btnDelete.Visible = true;

                if (
                    field.DataType == (int)CustomFieldDataType.SelectBox
                    || field.DataType == (int)CustomFieldDataType.CheckBox
                    )
                {
                    ulTabs.Visible = true;
                    tabOptions.Visible = true;
                    divtabs.Attributes["class"] = "tabs";
                }
            }

            //should implement???
            //divDisplayName.Visible = true;
            //divInvalidMessage.Visible = true;
            //divIsRequired.Visible = true;
            //divValidationExpression.Visible = true;
            //divAllowComparing.Visible = true;
        }

        #region Custom field options

        private string selOption = string.Empty;

        private void BindOptions()
        {
            btnOptionUp.Visible = false;
            btnOptionDown.Visible = false;
            btnDeleteOption.Visible = false;

            if (field != null)
            {
                lbOptions.Items.Clear();

                lbOptions.Items.Add(new ListItem(ProductResources.CustomFieldOptionNewOptionLabel, ""));
                lbOptions.DataSource = CustomFieldOption.GetByCustomField(field.CustomFieldId);
                lbOptions.DataBind();

                ListItem li = lbOptions.Items.FindByValue(selOption);
                if (li != null)
                {
                    lbOptions.ClearSelection();
                    li.Selected = true;
                }

                LanguageHelper.PopulateTab(tabOptionLanguage, false);
            }
        }

        private void BindOptionType()
        {
            ddlOptionType.Items.Clear();
            foreach (var item in typeof(CustomFieldOptionType).GetFields())
            {
                if (item.FieldType == typeof(CustomFieldOptionType))
                    ddlOptionType.Items.Add(new ListItem { Text = ResourceHelper.GetResourceString("ProductResources", "CustomFieldOption" + item.Name + "Type"), Value = item.GetRawConstantValue().ToString() });
            }
        }

        private void MoveUpDown(string direction)
        {
            if (field == null || field.Guid == Guid.Empty)
                return;

            List<CustomFieldOption> listOptions = CustomFieldOption.GetByCustomField(field.CustomFieldId);
            if (lbOptions.SelectedIndex > 0)
            {
                int delta;

                if (direction == "down")
                    delta = 3;
                else
                    delta = -3;

                CustomFieldOption option = listOptions[lbOptions.SelectedIndex - 1];
                option.DisplayOrder += delta;

                CustomFieldOption.ResortOptions(listOptions);

                selOption = option.CustomFieldOptionId.ToString();

                BindOptions();
                PopulateOptionControls();
            }
        }

        private void btnUpDown_Click(Object sender, EventArgs e)
        {
            string direction = ((LinkButton)sender).CommandName;
            MoveUpDown(direction);
        }

        private void lbOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateOptionControls();
        }

        private void btnUpdateOption_Click(object sender, EventArgs e)
        {
            try
            {
                if (field != null && field.CustomFieldId > 0 && lbOptions.SelectedIndex > -1)
                {
                    CustomFieldOption option = null;

                    if (lbOptions.SelectedValue.Length > 0)
                        option = new CustomFieldOption(Convert.ToInt32(lbOptions.SelectedValue));

                    bool isUpdate = true;
                    if (option == null || option.CustomFieldOptionId == -1)
                    {
                        option = new CustomFieldOption
                        {
                            CustomFieldId = field.CustomFieldId,
                            DisplayOrder = CustomFieldOption.GetMaxSortOrder(field.CustomFieldId)
                        };

                        isUpdate = false;
                    }

                    if (!IsAttributeLanguageTab())
                    {
                        option.Name = txtOptionName.Text;
                    }

                    if (divOptionType.Visible)
                    {
                        option.OptionType = Convert.ToInt32(ddlOptionType.SelectedValue);
                        option.OptionColor = ColorTranslator.ToHtml(colorPicker.SelectedColor);
                    }

                    if (option.Save())
                        SaveAttributeContentLanguage(option.Guid);

                    LogActivity.Write("Update custom field option", txtOptionName.Text);

                    selOption = option.CustomFieldOptionId.ToString();

                    BindOptions();
                    PopulateOptionControls();

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

        private void btnDeleteOption_Click(object sender, EventArgs e)
        {
            try
            {
                if (field != null && lbOptions.SelectedValue.Length > 0)
                {
                    CustomFieldOption option = null;

                    if (lbOptions.SelectedValue.Length > 0)
                        option = new CustomFieldOption(Convert.ToInt32(lbOptions.SelectedValue));

                    if (option != null && option.CustomFieldOptionId > 0)
                    {
                        ContentLanguage.DeleteByContent(option.Guid);
                        ProductProperty.DeleteByCustomFieldOption(option.CustomFieldOptionId);
                        CustomFieldOption.Delete(option.CustomFieldOptionId);

                        LogActivity.Write("Delete custom field option", lbOptions.SelectedItem.Text);

                        selOption = lbOptions.Items[lbOptions.SelectedIndex - 1].Value;
                    }

                    BindOptions();
                    PopulateOptionControls();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void btnDeleteOptionLanguage_Click(object sender, EventArgs e)
        {
            if (!IsAttributeLanguageTab())
                return;

            if (tabOptionLanguage.SelectedIndex > 0 && lbOptions.SelectedValue.Length == 36)
            {
                int languageId = Convert.ToInt32(tabOptionLanguage.SelectedTab.Value);

                if (languageId > 0)
                {
                    ContentLanguage.Delete(new Guid(lbOptions.SelectedValue), languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    PopulateOptionControls();
                }
            }
        }

        protected void tabOptionLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            PopulateOptionControls();
        }

        private void PopulateOptionControls()
        {
            btnOptionUp.Visible = false;
            btnOptionDown.Visible = false;

            if (lbOptions.SelectedValue.Length > 0)
            {
                if (lbOptions.Items.Count > 2)
                {
                    if (lbOptions.SelectedIndex == 1)
                        btnOptionDown.Visible = true;
                    else if (lbOptions.SelectedIndex == (lbOptions.Items.Count - 1))
                        btnOptionUp.Visible = true;
                    else
                    {
                        btnOptionDown.Visible = true;
                        btnOptionUp.Visible = true;
                    }
                }

                btnDeleteOption.Visible = true;
                btnUpdateOption.Text = ResourceHelper.GetResourceString("Resource", "UpdateButton");

                if (tabOptionLanguage.Visible && tabOptionLanguage.Tabs.Count == 1)
                    LanguageHelper.PopulateTab(tabOptionLanguage, lbOptions.SelectedValue.Length > 0);
            }
            else
            {
                btnDeleteOption.Visible = false;
                btnUpdateOption.Text = ResourceHelper.GetResourceString("Resource", "InsertButton");

                if (tabOptionLanguage.Visible && tabOptionLanguage.Tabs.Count != 1)
                    LanguageHelper.PopulateTab(tabOptionLanguage, lbOptions.SelectedValue.Length > 0);
            }

            PopulateOptionData(tabOptionLanguage.SelectedTab);
        }

        private void PopulateOptionData(Telerik.Web.UI.RadTab tab)
        {
            txtOptionName.Text = string.Empty;
            btnDeleteOptionLanguage.Visible = false;
            if (divOptionType.Visible)
            {
                ddlOptionType.ClearSelection();
                colorPicker.SelectedColor = ColorTranslator.FromHtml(string.Empty);
            }

            if (lbOptions.SelectedValue.Length > 0)
            {
                CustomFieldOption option = new CustomFieldOption(Convert.ToInt32(lbOptions.SelectedValue));

                if (option == null || option.Guid == Guid.Empty)
                    return;

                if (divOptionType.Visible)
                {
                    ListItem li = ddlOptionType.Items.FindByValue(option.OptionType.ToString());
                    if (li != null)
                    {
                        ddlOptionType.ClearSelection();
                        li.Selected = true;
                    }

                    if (option.OptionColor.Length == 7 && option.OptionColor.StartsWith("#"))
                        colorPicker.SelectedColor = ColorTranslator.FromHtml(option.OptionColor);
                }

                if (IsAttributeLanguageTab())
                {
                    ContentLanguage content = new ContentLanguage(option.Guid, Convert.ToInt32(tab.Value));
                    if (content != null && content.Guid != Guid.Empty)
                    {
                        txtOptionName.Text = content.Title;
                        btnDeleteOptionLanguage.Visible = true;
                    }
                }
                else
                {
                    txtOptionName.Text = option.Name;
                }
            }
        }

        private bool IsAttributeLanguageTab()
        {
            if (tabOptionLanguage.Visible && tabOptionLanguage.SelectedIndex > 0)
                return true;

            return false;
        }

        private void SaveAttributeContentLanguage(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsAttributeLanguageTab())
                return;

            int languageID = -1;
            if (tabOptionLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabOptionLanguage.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);

            if (txtOptionName.Text.Length > 0)
            {
                content.LanguageId = languageID;
                content.ContentGuid = contentGuid;
                content.SiteGuid = siteSettings.SiteGuid;
                content.Title = txtOptionName.Text.Trim();
                content.Save();
            }
        }

        #endregion Custom field options
    }
}
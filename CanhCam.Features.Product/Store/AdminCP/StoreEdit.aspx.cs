using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.StoreUI
{
    public partial class StoreEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StoreEditPage));

        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private int storeID = -1;
        private Store store = null;

        private SiteUser currentUser = null;

        private bool enabledStore = false;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var basePage = Page as CmsBasePage;
            SiteUtils.SetupEditor(edDescription, basePage.AllowSkinOverride, Page);

            this.Load += new EventHandler(this.Page_Load);

            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            btnInsert.Click += new EventHandler(btnUpdate_Click);
            btnInsertAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            btnInsertAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);

            ddProvince.SelectedIndexChanged += new EventHandler(ddProvince_SelectedIndexChanged);
        }

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

            if (!enabledStore)
            {
                SiteUtils.RedirectToHomepage();
            }

            if (storeID > 0)
            {
                var isStoreAdmin = false;
                var lstStoreAdmins = store.OrderUserIDs.Split(';');
                foreach (string adminID in lstStoreAdmins)
                {
                    if (adminID == currentUser.UserId.ToString())
                        isStoreAdmin = true;
                }

                if (!isStoreAdmin && !WebUser.IsAdmin)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }
            }

            PopulateLabels();

            if ((!Page.IsPostBack) && (!Page.IsCallback))
                PopulateControls();
        }

        private void LoadParams()
        {
            storeID = WebUtils.ParseInt32FromQueryString("id", storeID);
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
            currentUser = SiteUtils.GetCurrentSiteUser();

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            enabledStore = StoreHelper.EnabledStore;

            if (storeID > 0)
            {
                store = new Store(storeID);
            }

            //Google Map
            var litGoogleScripts = (Literal)MPContent.FindControl("litScripts");
            if (litGoogleScripts != null)
            {
                var key = ConfigHelper.GetStringProperty("GoogleMapsAPIKey", string.Empty);
                var keyWithParam = string.Empty;
                if (key.Length > 0)
                    keyWithParam = string.Format("&key={0}", key);
                litGoogleScripts.Text = string.Format("<script type=\"text/javascript\" src=\"https://maps.googleapis.com/maps/api/js?v=3.exp&sensor=false&libraries=places&language=vi-VN{0}\"></script>", keyWithParam);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, StoreResources.StoreEditTitle);
            UIHelper.AddConfirmationDialog(btnDelete, StoreResources.StoreDeleteConfirmMessage);

            edDescription.WebEditor.ToolBar = ToolBar.FullWithTemplates;
        }

        protected virtual void PopulateControls()
        {
            BindProvince();

            if (storeID > 0)
            {
                btnInsert.Visible = false;
                btnInsertAndClose.Visible = false;
                btnInsertAndNew.Visible = false;
                btnDelete.Visible = true;

                txtPhone.Text = store.Phone;
                txtFax.Text = store.Fax;
                txtEmail.Text = store.Email;
                txtContact.Text = store.ContactPerson;
                txtDistrictCode.Text = store.APIDistrictCode;
                txtCode.Text = store.Code;
                //OrderUserIDs
                var lstUserIDs = store.OrderUserIDs.Split(';');
                foreach (string userID in lstUserIDs)
                {
                    if (userID != "")
                    {
                        var siteUser = new SiteUser(siteSettings, Int32.Parse(userID));
                        if (siteUser != null && siteUser.UserId > 0)
                        {
                            var orderUser = new AutoCompleteBoxEntry(siteUser.Name, siteUser.UserGuid.ToString());
                            autAdmins.Entries.Add(orderUser);
                        }
                    }
                }
                chkTakeOnlineOrder.Checked = store.Options != 0;
                chkPublic.Checked = store.IsPublished;
                chkDefault.Checked = store.StoreID == Int32.Parse(siteSettings.GetExpandoProperty("DefaultStore"));

                txtName.Text = store.Name;
                edDescription.Text = store.Description;
                txtAPIEndpoint.Text = store.APIEndpoint;
                txtAddress.Text = store.Address;

                //Areas & Provinces & Districts
                List<ListItem> lstSelectedItems = new List<ListItem>();

                var dictDistrictGuids = StoreHelper.GetStoreDistricts(store.DistrictGuids);
                foreach (var districtGuid in dictDistrictGuids)
                {
                    var tmp = GeoZone.GetByGuids(districtGuid.Key.ToString()).FirstOrDefault();
                    if (tmp != null)
                        lstSelectedItems.Add(new ListItem(tmp.Name, districtGuid.Key.ToString()));
                }

                BindArea(cobAreas, lstSelectedItems);

                var item = ddProvince.Items.FindByValue(store.ProvinceGuid.ToString());
                if (item != null)
                {
                    ddProvince.ClearSelection();
                    item.Selected = true;
                    BindDistrict(ddProvince, ddDistrict);
                }

                item = ddDistrict.Items.FindByValue(store.DistrictGuid.ToString());
                if (item != null)
                {
                    ddDistrict.ClearSelection();
                    item.Selected = true;
                }

                txtLatitude.Text = store.Latitude;
                txtLongitude.Text = store.Longitude;
            }
            else
            {
                btnUpdate.Visible = false;
                btnUpdateAndClose.Visible = false;
                btnUpdateAndNew.Visible = false;
                btnDelete.Visible = false;

                List<ListItem> lstSelectedItems = new List<ListItem>();
                BindArea(cobAreas, lstSelectedItems);
            }
            bool isLoadAll = (store != null && store.StoreID > 0);
            LanguageHelper.PopulateTab(tabLanguage, isLoadAll);
        }

        private void BindArea(ListControl cob, List<ListItem> lstSelectedItems)
        {
            cob.Items.Clear();
            cob.ClearSelection();

            List<GeoZone> provinces = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            foreach (GeoZone province in provinces)
            {
                string provinceName = province.Name;
                cob.Items.Add(new ListItem() { Text = provinceName, Value = province.Guid.ToString() });
                List<GeoZone> districts = GeoZone.GetByListParent(province.Guid.ToString(), 1);
                foreach (GeoZone district in districts)
                {
                    cob.Items.Add(new ListItem() { Text = provinceName + " > " + district.Name, Value = district.Guid.ToString() });
                }
            }

            AddSelectedToComboBox(cob, lstSelectedItems);
            RenderComboBox(cob.ClientID);
        }

        private void BindProvince()
        {
            ddProvince.DataSource = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            ddProvince.DataBind();
        }

        private void BindDistrict(ListControl ddProv, ListControl ddDis)
        {
            ddDis.Items.Clear();
            if (ddProv.SelectedValue.Length == 36)
            {
                ddDis.DataSource = GeoZone.GetByListParent(ddProv.SelectedValue, 1);
                ddDis.DataBind();
            }

            ddDis.Items.Insert(0, new ListItem("", ""));
        }

        private void ddProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrict(ddProvince, ddDistrict);
        }

        private void AddSelectedToComboBox(ListControl cob, List<ListItem> lstSelectedItems)
        {
            if (lstSelectedItems.Count > 0)
            {
                List<ListItem> lstItemsNotInCob = new List<ListItem>(); // add trong foreach bị lỗi
                foreach (ListItem selectedItem in lstSelectedItems)
                {
                    bool inCob = false;
                    foreach (ListItem listItem in cob.Items)
                    {
                        if (listItem.Value == selectedItem.Value)
                        {
                            listItem.Selected = true;
                            inCob = true;
                            break;
                        }
                    }
                    if (!inCob)
                    {
                        lstItemsNotInCob.Add(selectedItem);
                    }
                }
                foreach (ListItem itemNotInCob in lstItemsNotInCob)
                {
                    itemNotInCob.Selected = true;
                    cob.Items.Add(itemNotInCob);
                }
            }
        }

        #region Language

        protected void tabLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            if (e.Tab.Index == 0)
            {
                txtName.Text = store.Name;
                edDescription.Text = store.Description;
                txtAddress.Text = store.Address;

                var item = ddProvince.Items.FindByValue(store.ProvinceGuid.ToString());
                if (item != null)
                {
                    ddProvince.ClearSelection();
                    item.Selected = true;
                    BindDistrict(ddProvince, ddDistrict);
                }

                txtLatitude.Text = store.Latitude;
                txtLongitude.Text = store.Longitude;
            }
            else
            {
                txtName.Text = store.Name;
                edDescription.Text = store.Description;
                txtAddress.Text = store.Address;

                var item = ddProvince.Items.FindByValue(store.ProvinceGuid.ToString());
                if (item != null)
                {
                    ddProvince.ClearSelection();
                    item.Selected = true;
                    BindDistrict(ddProvince, ddDistrict);
                }

                txtLatitude.Text = store.Latitude;
                txtLongitude.Text = store.Longitude;

                ContentLanguage content = new ContentLanguage(store.Guid, Convert.ToInt32(e.Tab.Value));
                if (content != null && content.Guid != Guid.Empty)
                {
                    txtName.Text = store.Name;
                    edDescription.Text = store.Description;
                    txtAddress.Text = store.Address;

                    if (item != null)
                    {
                        ddProvince.ClearSelection();
                        item.Selected = true;
                        BindDistrict(ddProvince, ddDistrict);
                    }

                    txtLatitude.Text = store.Latitude;
                    txtLongitude.Text = store.Longitude;
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
            var storeName = txtName.Text.Trim();

            if (storeName.Length == 0)
            {
                return;
            }

            content.Title = txtName.Text.Trim();
            content.BriefContent = edDescription.Text.Trim();

            content.LanguageId = languageID;
            content.ContentGuid = contentGuid;
            content.SiteGuid = siteSettings.SiteGuid;
            content.Save();
        }

        #endregion Language

        #region Save

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Store/AdminCP/StoreEdit.aspx?id=" + nId.ToString());
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Store/AdminCP/StoreList.aspx");
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Store/AdminCP/StoreEdit.aspx");
        }

        private int Save()
        {
            Page.Validate("Store");

            if (!Page.IsValid)
                return -1;

            if (autAdmins.Entries.Count < 1)
                return -1;

            if (cobAreas.SelectedItems.Count < 1)
            {
                return -1;
            }

            Store store = storeID > 0 ? new Store(storeID) : new Store();
            store.SiteID = siteSettings.SiteId;
            store.DisplayOrder = 1;
            store.Code = txtCode.Text;
            store.Phone = txtPhone.Text.Trim();
            store.Fax = txtFax.Text.Trim();
            store.Email = txtEmail.Text.Trim();
            store.ContactPerson = txtContact.Text.Trim();
            store.Options = Convert.ToInt32(chkTakeOnlineOrder.Checked);
            store.APIDistrictCode = txtDistrictCode.Text.Trim();

            var lstAdmins = string.Empty;
            foreach (AutoCompleteBoxEntry entry in autAdmins.Entries)
            {
                SiteUser user = new SiteUser(siteSettings, new Guid(entry.Value));
                lstAdmins += user.UserId + ";";
            }
            if (lstAdmins.Length > 0)
                lstAdmins = lstAdmins.Remove(lstAdmins.Length - 1, 1);

            store.OrderUserIDs = lstAdmins;
            store.IsPublished = chkPublic.Checked;

            //Managing Areas
            Dictionary<Guid, int> dictDistrictGuids = StoreHelper.GetStoreDistricts(store.DistrictGuids);
            var results = new Dictionary<Guid, int>();
            foreach (ListItem selectedItem in cobAreas.SelectedItems)
            {
                Guid guid = new Guid(selectedItem.Value);

                if (dictDistrictGuids.ContainsKey(guid))
                    results.Add(guid, dictDistrictGuids[guid]);
                else
                    results.Add(guid, 0);
            }

            string districtGuids = string.Empty;
            foreach (var item in results)
            {
                districtGuids += item.Key.ToString().ToUpper() + "_" + item.Value.ToString() + ";";
            }
            if (districtGuids != string.Empty)
                districtGuids = districtGuids.Remove(districtGuids.Length - 1, 1);
            else
                districtGuids += Guid.Empty.ToString().ToUpper() + "_0";

            store.DistrictGuids = districtGuids;
            store.APIEndpoint = txtAPIEndpoint.Text.Trim();

            if (!IsLanguageTab())
            {
                store.Name = txtName.Text.Trim();
                store.Description = edDescription.Text.Trim();
                store.Address = txtAddress.Text.Trim();

                if (ddProvince.SelectedValue == "" || ddProvince.SelectedValue.Length < 36)
                {
                    store.ProvinceGuid = Guid.Empty;
                }
                else
                {
                    store.ProvinceGuid = new Guid(ddProvince.SelectedValue);
                }

                if (ddDistrict.SelectedValue == "" || ddDistrict.SelectedValue.Length < 36)
                {
                    store.DistrictGuid = Guid.Empty;
                }
                else
                {
                    store.DistrictGuid = new Guid(ddDistrict.SelectedValue);
                }

                store.Latitude = txtLatitude.Text;
                store.Longitude = txtLongitude.Text;

                //if (store.DistrictGuids != "" && store.ProvinceGuids != Guid.Empty.ToString())
                //{
                //    store.Code = GeoZone.GetByGuids(store.DistrictGuids)[0].Code;
                //}
            }

            if (store.Save())
            {
                SaveContentLanguage(store.Guid);
                if (chkDefault.Checked)
                {
                    siteSettings.SetExpandoProperty("DefaultStore", store.StoreID.ToString());
                    siteSettings.SaveExpandoProperties();
                }
            }

            if (storeID > 0)
            {
                LogActivity.Write("Update store", store.Name);
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
            }
            else
            {
                LogActivity.Write("Create new store", store.Name);
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
            }

            return store.StoreID;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WebUser.IsAdmin)
                    return;

                ContentDeleted.Create(siteSettings.SiteId, store.StoreID.ToString(), "Store", typeof(StoreDeleted).AssemblyQualifiedName, store.StoreID.ToString(), Page.User.Identity.Name);

                store.IsDeleted = true;
                store.Save();
                LogActivity.Write("Delete store", store.StoreID.ToString());

                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                WebUtils.SetupRedirect(this, SiteRoot + "/Store/AdminCP/StoreList.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion Save

        private void RenderComboBox(string clientID)
        {
            StringBuilder strBuilder = new StringBuilder();
            string separator = string.Empty;
            strBuilder.AppendFormat(separator + "no_results_text: \"{0}\"", "Không tìm thấy");
            separator = ",\n";
            strBuilder.Append(separator + "search_contains: true");
            separator = ",\n";
            strBuilder.Append(separator + "single_backstroke_delete: false");
            separator = ",\n";
            strBuilder.Append(separator + "display_disabled_options: false");
            separator = ",\n";
            strBuilder.Append(separator + "display_selected_options: false");
            string script = string.Format(@"$('#{0}').chosen({{{1}}});", clientID, strBuilder.ToString());

            if (ScriptManager.GetCurrent(this.Page) != null)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "initchosen" + clientID, script, true);
            else
                this.Page.ClientScript.RegisterStartupScript(this.GetType(), "initchosen" + clientID, script, true);
        }

        #region WebMethod

        [WebMethod]
        public static AutoCompleteBoxData GetUserNames(object context)
        {
            var searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToAsciiIfPossible().ToLower();
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var lstUsers = SiteUser.GetUserAdminSearchPage(
                                    siteSettings.SiteId,
                                    1,
                                    30,
                                    searchString,
                                    0,
                                    out _);
            var result = new List<AutoCompleteBoxItemData>();
            foreach (SiteUser siteUser in lstUsers)
            {
                var childNode = new AutoCompleteBoxItemData
                {
                    Text = siteUser.Name,
                    Value = siteUser.UserGuid.ToString()
                };
                childNode.Attributes.Add("FirstName", siteUser.FirstName);
                childNode.Attributes.Add("LastName", siteUser.LastName);
                childNode.Attributes.Add("Email", siteUser.Email);

                result.Add(childNode);
                if (result.Count >= 50)
                    break;
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };

            return res;
        }

        #endregion WebMethod
    }
}
/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-08-29
/// Last Modified:		    2014-08-29

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class TagBulkEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(TagBulkEditPage));

        protected int pageNumber = 1;
        protected int pageSize = WebConfigSettings.DefaultPageSize;
        private string keyword = null;

        private List<ContentLanguage> listAllContents = new List<ContentLanguage>();
        private List<ContentLanguage> listContents = new List<ContentLanguage>();
        private List<Language> listLanguages = new List<Language>();

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (!ProductPermission.CanManageTags)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (WebConfigSettings.AllowMultiLanguage)
            {
                listLanguages = LanguageHelper.GetPublishedLanguages(true);
            }

            if (Page.IsPostBack) return;

            if (keyword != null)
                txtKeyword.Text = keyword;

            BindGrid();
        }

        private void BindGrid()
        {
            btnAddNew.Visible = true;

            List<Tag> tags = Tag.GetPage(siteSettings.SiteGuid, Product.FeatureGuid, keyword, -1, pageNumber, pageSize);

            string pageUrl = SiteRoot + "/Product/AdminCP/TagBulkEdit.aspx";
            if (keyword != null)
                pageUrl += "?keyword=" + keyword + "&pagenumber={0}";
            else
                pageUrl += "?pagenumber={0}";

            pgr.PageURLFormat = pageUrl;
            pgr.ShowFirstLast = true;
            pgr.CurrentIndex = pageNumber;
            pgr.PageSize = pageSize;
            pgr.ItemCount = Tag.GetCount(siteSettings.SiteGuid, Product.FeatureGuid, null, -1);

            if (WebConfigSettings.AllowMultiLanguage)
            {
                string listGuidString = string.Empty;
                string sepa = string.Empty;
                foreach (Tag item in tags)
                {
                    listGuidString += sepa + item.Guid.ToString();
                    sepa = ";";
                }

                listAllContents = ContentLanguage.GetByListContent(listGuidString);
                listLanguages = LanguageHelper.GetPublishedLanguages(true);
            }

            if (tags.Count > 0)
            {
                rpt.DataSource = tags;
                rpt.DataBind();
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("TagId", typeof(int));
            dataTable.Columns.Add("Guid", typeof(Guid));
            dataTable.Columns.Add("TagText", typeof(String));

            DataRow row = dataTable.NewRow();
            row["TagId"] = -1;
            row["Guid"] = Guid.Empty;
            row["TagText"] = string.Empty;
            dataTable.Rows.Add(row);

            btnAddNew.Visible = false;
            btnCancel.Visible = true;
            pgr.Visible = false;

            rpt.DataSource = dataTable;
            rpt.DataBind();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string pageUrl = SiteRoot + "/Product/AdminCP/TagBulkEdit.aspx";
            if (txtKeyword.Text.Length > 0)
                pageUrl += "?keyword=" + txtKeyword.Text.Trim();

            WebUtils.SetupRedirect(this, pageUrl);
        }

        private void DeleteTag(int tagId)
        {
            try
            {
                Tag tag = new Tag(tagId);

                if (tag != null && tag.TagId > -1)
                {
                    ContentLanguage.DeleteByContent(tag.Guid);
                    TagItem.DeleteByTag(tag.TagId);
                    Tag.Delete(tagId);

                    LogActivity.Write("Delete product tag", tag.TagText);
                    //message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (RepeaterItem item in rpt.Items)
                {
                    if ((item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem))
                    {
                        HiddenField hdfTagId = (HiddenField)item.FindControl("hdfTagId");
                        CheckBox chkDelete = (CheckBox)item.FindControl("chkDelete");

                        if (chkDelete.Checked)
                        {
                            DeleteTag(Convert.ToInt32(hdfTagId.Value));
                            continue;
                        }

                        Repeater rptLanguages = (Repeater)item.FindControl("rptLanguages");
                        TextBox txtName = (TextBox)item.FindControl("txtName");

                        int tagId = Convert.ToInt32(hdfTagId.Value.ToString());

                        Tag tag;
                        if (tagId == -1)
                        {
                            tag = new Tag
                            {
                                SiteGuid = siteSettings.SiteGuid,
                                FeatureGuid = Product.FeatureGuid
                            };
                        }
                        else
                            tag = new Tag(tagId);

                        tag.TagText = txtName.Text.Trim();

                        if (tagId <= 0)
                            LogActivity.Write("Insert tag", tag.TagText);
                        else
                            LogActivity.Write("Update tag", tag.TagText);

                        if (tag.Save() > 0)
                            SaveContentLanguage(rptLanguages, tag.Guid);
                    }
                }

                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductTagsTitle);
            heading.Text = ProductResources.ProductTagsTitle;

            lnkGridView.NavigateUrl = SiteRoot + "/Product/AdminCP/TagList.aspx";
        }

        private void LoadSettings()
        {
            pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
            keyword = WebUtils.ParseStringFromQueryString("keyword", keyword);

            AddClassToBody("admin-producttags tagbulkedit");
        }

        #region Language

        protected void rpt_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                listContents.Clear();
                if (listLanguages.Count > 0)
                {
                    HiddenField hdfTagGuid = (HiddenField)e.Item.FindControl("hdfTagGuid");
                    Guid guid = new Guid(hdfTagGuid.Value.ToString());
                    foreach (ContentLanguage item in listAllContents)
                    {
                        if (item.ContentGuid == guid)
                            listContents.Add(item);
                    }

                    Repeater rptLanguages = (Repeater)e.Item.FindControl("rptLanguages");
                    rptLanguages.DataSource = listLanguages;
                    rptLanguages.DataBind();
                }
            }
        }

        protected void rptLanguages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var txtName = (TextBox)e.Item.FindControl("txtName");
                var hdfLanguageId = (HiddenField)e.Item.FindControl("hdfLanguageId");
                int languageId = Convert.ToInt32(hdfLanguageId.Value);
                var content = ContentLanguage.GetOneFromList(listContents, languageId);

                if (content != null)
                    txtName.Text = content.Title;
            }
        }

        private void SaveContentLanguage(Repeater rptLanguages, Guid contentGuid)
        {
            if (!WebConfigSettings.AllowMultiLanguage)
                return;

            if (contentGuid == Guid.Empty)
                return;

            foreach (RepeaterItem item in rptLanguages.Items)
            {
                if ((item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem))
                {
                    var txtName = (TextBox)item.FindControl("txtName");
                    var hdfLanguageId = (HiddenField)item.FindControl("hdfLanguageId");
                    int languageId = Convert.ToInt32(hdfLanguageId.Value);
                    var strName = txtName.Text.Trim();

                    var content = new ContentLanguage(contentGuid, languageId);

                    if (strName.Length > 0)
                    {
                        content.LanguageId = languageId;
                        content.ContentGuid = contentGuid;
                        content.SiteGuid = siteSettings.SiteGuid;
                        content.Title = strName;
                        content.Save();
                    }
                    else if (content != null && content.Guid != Guid.Empty)
                        ContentLanguage.Delete(contentGuid, languageId);
                }
            }
        }

        #endregion Language

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnAddNew.Click += new EventHandler(btnAddNew_Click);
            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnCancel.Click += new EventHandler(btnCancel_Click);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        #endregion OnInit
    }
}
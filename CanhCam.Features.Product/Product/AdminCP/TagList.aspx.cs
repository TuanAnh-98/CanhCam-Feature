/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2014-08-25
/// Last Modified:		    2014-08-25

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class TagListPage : CmsNonBasePage
    {
        #region Properties

        private RadGridSEOPersister gridPersister;
        private static readonly ILog log = LogManager.GetLogger(typeof(TagListPage));

        #endregion Properties

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

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductTagsTitle);
            heading.Text = ProductResources.ProductTagsTitle;

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductTagsDeleteMultiWarning);
        }

        private void LoadSettings()
        {
            lnkBulkEdit.NavigateUrl = SiteRoot + "/Product/AdminCP/TagBulkEdit.aspx";

            AddClassToBody("admin-producttags");
        }

        private void PopulateControls()
        {
        }

        #region "RadGrid Event"

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            int iCount = Tag.GetCount(siteSettings.SiteGuid, Product.FeatureGuid, null);

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = !isApplied;

            grid.DataSource = Tag.GetPage(siteSettings.SiteGuid, Product.FeatureGuid, null, -1, startRowIndex, maximumRows);

            if (iCount == 0)
                btnDelete.Visible = false;
        }

        #endregion "RadGrid Event"

        #region Events

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                bool isDeleted = false;

                foreach (GridDataItem data in grid.SelectedItems)
                {
                    int tagId = Convert.ToInt32(data.GetDataKeyValue("TagId"));
                    Tag tag = new Tag(tagId);

                    if (tag != null && tag.TagId > -1)
                    {
                        ContentLanguage.DeleteByContent(tag.Guid);
                        TagItem.DeleteByTag(tag.TagId);
                        Tag.Delete(tagId);

                        LogActivity.Write("Delete product tag", tag.TagText);

                        isDeleted = true;
                    }
                }

                if (isDeleted)
                {
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                    WebUtils.SetupRedirect(this, Request.RawUrl);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        #endregion Events

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);

            grid.NeedDataSource += new GridNeedDataSourceEventHandler(grid_NeedDataSource);

            gridPersister = new RadGridSEOPersister(grid);
        }

        #endregion OnInit
    }
}
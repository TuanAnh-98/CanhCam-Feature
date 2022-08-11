/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-28
/// Last Modified:			2014-06-28

using System;
using System.Web.UI.WebControls;
using log4net;
using CanhCam.Business.WebHelpers;
using CanhCam.Business;
using CanhCam.Web.Framework;
using System.Web.UI;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public partial class NewsRelatedControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(NewsRelatedControl));

        protected SiteSettings siteSettings = null;

        private Guid productGuid = Guid.Empty;
        private Guid siteGuid = Guid.Empty;
        public Guid ProductGuid
        {
            get { return productGuid; }
            set { productGuid = value; }
        }
        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }


        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (Page.IsPostBack) return;

            gbSiteMapProvider.PopulateListControl(ddZones, false, News.FeatureGuid);

            if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
                ddZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), "-1"));
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

        void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!Page.IsPostBack)
            {
                grid.DataSource = new List<Product>();
            }
            else
            {
                bool isApplied = false;
                int iCount = News.GetCountBySearch2(siteSettings.SiteId, sZoneId, -1, -1, -1, -1, -1, null, null, null, null, txtTitle.Text.Trim());
                int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
                int maximumRows = isApplied ? iCount : grid.PageSize;

                grid.VirtualItemCount = iCount;
                grid.AllowCustomPaging = !isApplied;

                grid.DataSource = News.GetPageBySearch2(startRowIndex, maximumRows, siteSettings.SiteId, sZoneId, -1, -1, -1, -1, -1, null, null, null, null, txtTitle.Text.Trim());
            }
        }

        void gridRelated_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (productGuid != Guid.Empty)
                gridRelated.DataSource = NewsRelated.GetNewsRelated(siteSettings.SiteId, siteGuid, productGuid, true, false);
            else
                gridRelated.DataSource = new List<News>();
        }

        void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        void btnAdd_Click(object sender, EventArgs e)
        {
            UpdateRelatedProducts(grid);
        }

        void btnRemove_Click(object sender, EventArgs e)
        {
            UpdateRelatedProducts(gridRelated, true);
        }

        private bool UpdateRelatedProducts(Telerik.Web.UI.RadGrid grid, bool isRemove = false)
        {
            if (productGuid == Guid.Empty)
                return false;

            bool isUpdated = false;

            foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
            {
                Guid itemGuid2 = new Guid(data.GetDataKeyValue("NewsGuid").ToString());

                if (!isRemove)
                {
                    RelatedItem relatedItem = new RelatedItem();
                    relatedItem.SiteGuid = siteGuid;
                    relatedItem.ItemGuid1 = productGuid;
                    relatedItem.ItemGuid2 = itemGuid2;
                    relatedItem.Save();
                }
                else
                {
                    RelatedItem.Delete(productGuid, itemGuid2);
                }

                isUpdated = true;
            }

            if (isUpdated)
            {
                //message.SuccessMessage = Resource.UpdateSuccessMessage;
                gridRelated.Rebind();
            }

            return true;
        }

        private void PopulateLabels()
        {

        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnAdd.Click += new EventHandler(btnAdd_Click);
            this.btnRemove.Click += new EventHandler(btnRemove_Click);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.gridRelated.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(gridRelated_NeedDataSource);
            this.btnSearch.Click += new EventHandler(btnSearch_Click);
        }

        #endregion
    }
}
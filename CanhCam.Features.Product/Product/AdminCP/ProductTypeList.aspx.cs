using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductTypeListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductTypeListPage));

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            grid.DataSource = ProductType.GetAll();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int productTypeId = Convert.ToInt32(data.GetDataKeyValue("ProductTypeId"));

                    ProductType type = new ProductType(productTypeId);
                    if (type != null && type.ProductTypeId != -1)
                    {
                        ProductType.Delete(type.ProductTypeId);
                        LogActivity.Write("Delete Product Type", type.Name);

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductTypeListLabel);
            heading.Text = ProductResources.ProductTypeListLabel;

            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/ProductTypeEdit.aspx";

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void LoadSettings()
        {
            AddClassToBody("customfields-manager");
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
        }

        #endregion OnInit
    }
}
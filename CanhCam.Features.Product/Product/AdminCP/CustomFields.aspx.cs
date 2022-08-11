/// Author:                 Tran Quoc Vuong - itqvuong@gmail.com
/// Created:			    2014-07-23
/// Last Modified:		    2014-07-23

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class CustomFieldsPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CustomFieldsPage));

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
            grid.DataSource = CustomField.GetByFeature(siteSettings.SiteId, Product.FeatureGuid);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int customFieldId = Convert.ToInt32(data.GetDataKeyValue("CustomFieldId"));

                    CustomField field = new CustomField(customFieldId);
                    if (field != null && field.CustomFieldId != -1)
                    {
                        ContentLanguage.DeleteByContent(field.Guid);
                        ProductProperty.DeleteByCustomField(field.CustomFieldId);
                        CustomFieldOption.DeleteCustomField(field.CustomFieldId);
                        CustomField.Delete(field.CustomFieldId);

                        LogActivity.Write("Delete custom field", field.Name);

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    //LogActivity.Write("Delete " + iRecordDeleted.ToString() + " order(s)", "Custom field");
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
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
                bool isUpdated = false;
                foreach (GridDataItem data in grid.Items)
                {
                    TextBox txtDisplayOrder = (TextBox)data.FindControl("txtDisplayOrder");
                    int customFieldId = Convert.ToInt32(data.GetDataKeyValue("CustomFieldId"));
                    int displayOrder = Convert.ToInt32(data.GetDataKeyValue("DisplayOrder"));

                    int displayOrderNew = displayOrder;
                    int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                    if (displayOrder != displayOrderNew)
                    {
                        CustomField field = new CustomField(customFieldId);
                        if (field != null && field.CustomFieldId != -1)
                        {
                            field.DisplayOrder = displayOrderNew;
                            field.Save();

                            LogActivity.Write("Resort custom field", field.Name);

                            isUpdated = true;
                        }
                    }
                }

                if (isUpdated)
                {
                    grid.Rebind();

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.CustomFieldsTitle);
            heading.Text = ProductResources.CustomFieldsTitle;

            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/CustomFieldEdit.aspx";

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

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
        }

        #endregion OnInit

        protected void btnFix_Click(object sender, EventArgs e)
        {
            var products = Product.GetPageAdv();

            foreach (var product in products)
            {
                FriendlyUrl.DeleteByPageGuid(product.ProductGuid);

                if (product.Save())
                {
                    var newFriendlyUrl = new FriendlyUrl
                    {
                        SiteId = siteSettings.SiteId,
                        SiteGuid = siteSettings.SiteGuid,
                        PageGuid = product.ProductGuid,
                        Url = product.Url.Replace("~/", string.Empty),
                        RealUrl = "~/Product/ProductDetail.aspx?zoneid="
                        + product.ZoneId.ToInvariantString()
                        + "&ProductID=" + product.ProductId.ToInvariantString()
                    };

                    newFriendlyUrl.Save();
                }
            }

            message.SuccessMessage = "Done ";
        }
    }
}
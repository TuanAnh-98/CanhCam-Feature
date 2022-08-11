/// Author:					Tran Quoc Vuong - itqvuong@gmail.com
/// Created:				2015-04-22
/// Last Modified:			2015-04-24

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
    public partial class ShippingMethodsPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ShippingMethodsPage));

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
            grid.DataSource = ShippingMethod.GetByActive(siteSettings.SiteId, -1);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int shippingMethodId = Convert.ToInt32(data.GetDataKeyValue("ShippingMethodId"));

                    ShippingMethod method = new ShippingMethod(shippingMethodId);
                    if (method != null && method.ShippingMethodId > 0 && method.SiteId == siteSettings.SiteId && !method.IsDeleted)
                    {
                        ContentDeleted.Create(siteSettings.SiteId, method.ShippingMethodId.ToString(), "ShippingMethod", typeof(ShippingMethodDeleted).AssemblyQualifiedName, method.ShippingMethodId.ToString(), Page.User.Identity.Name);

                        method.IsDeleted = true;
                        method.Save();

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    LogActivity.Write("Delete " + iRecordDeleted.ToString() + " shipping method(s)", "Shipping method");
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
                    CheckBox chkIsActive = (CheckBox)data.FindControl("chkIsActive");
                    int shippingMethodId = Convert.ToInt32(data.GetDataKeyValue("ShippingMethodId"));
                    int displayOrder = Convert.ToInt32(data.GetDataKeyValue("DisplayOrder"));
                    bool isActive = Convert.ToBoolean(data.GetDataKeyValue("IsActive"));

                    int displayOrderNew = displayOrder;
                    int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                    if (displayOrder != displayOrderNew || isActive != chkIsActive.Checked)
                    {
                        ShippingMethod method = new ShippingMethod(shippingMethodId);
                        if (method != null && method.ShippingMethodId != -1)
                        {
                            method.IsActive = chkIsActive.Checked;
                            method.DisplayOrder = displayOrderNew;
                            method.Save();

                            LogActivity.Write("Update shipping method", method.Name);

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
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ShippingMethodsTitle);
            heading.Text = ProductResources.ShippingMethodsTitle;

            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/ShippingMethodEdit.aspx";

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void LoadSettings()
        {
            AddClassToBody("admin-shippingmethods");
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
    }
}

namespace CanhCam.Web.ProductUI
{
    public class ShippingMethodDeleted : IContentDeleted
    {
        public bool RestoreContent(string shippingMethodId)
        {
            try
            {
                ShippingMethod method = new ShippingMethod(Convert.ToInt32(shippingMethodId));
                if (method != null && method.ShippingMethodId > 0)
                {
                    method.IsDeleted = false;
                    method.Save();
                }
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool DeleteContent(string shippingMethodId)
        {
            try
            {
                ShippingMethod method = new ShippingMethod(Convert.ToInt32(shippingMethodId));
                if (method != null && method.ShippingMethodId > 0)
                {
                    ContentLanguage.DeleteByContent(method.Guid);
                    ShippingTableRate.DeleteByMethod(method.ShippingMethodId);
                    ShippingMethod.Delete(method.ShippingMethodId);
                }
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
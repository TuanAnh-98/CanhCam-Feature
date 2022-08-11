/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-08-11
/// Last Modified:		    2015-08-11

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class DiscountsPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountsPage));
        private RadGridSEOPersister gridPersister;

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

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
            PromotionsHelper.PopulateDiscountType(ddlDiscountType, true);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        //private void ddlDiscountType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grid.Rebind();
        //}

        //private void ddlDiscountStatus_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    grid.Rebind();
        //}

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var productId = -1;
            var zoneId = -1;

            if (autProducts.Entries.Count > 0)
            {
                productId = Convert.ToInt32(autProducts.Entries[0].Value);
                var product = new Product(siteSettings.SiteId, productId);
                if (product != null && product.ProductId > 0)
                {
                    productId = product.ProductId;
                    zoneId = product.ZoneId;
                }
            }

            var isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            var discountType = Convert.ToInt32(ddlDiscountType.SelectedValue);
            var status = Convert.ToInt32(ddlDiscountStatus.SelectedValue);
            var excludedDiscountId = -1;
            int iCount = Discount.GetCount(siteSettings.SiteId, discountType, status, -1, productId, zoneId, excludedDiscountId);

            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = !isApplied;

            grid.DataSource = Discount.GetPage(siteSettings.SiteId, discountType, status, -1, productId, zoneId, excludedDiscountId, 1, startRowIndex, maximumRows);
        }

        protected string FormatDate(object objDt, string format = "")
        {
            if (objDt != null)
            {
                if (!string.IsNullOrEmpty(format))
                    return Convert.ToDateTime(objDt).ToString(format);

                return Convert.ToDateTime(objDt).ToString("dd/MM/yyyy HH:mm");
            }

            return string.Empty;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int discountId = Convert.ToInt32(data.GetDataKeyValue("DiscountId"));

                    Discount discount = new Discount(discountId);
                    if (discount != null && discount.DiscountId > -1)
                    {
                        PromotionsHelper.DeleteFolder(siteSettings.SiteId, discount.DiscountId);

                        var lstContent = DiscountContent.GetByDiscount(discount.DiscountId);
                        foreach (var c in lstContent)
                            ContentAttribute.DeleteByContent(c.Guid);
                        DiscountContent.DeleteByDiscount(discount.DiscountId);
                        DiscountRange.DeleteByDiscount(discount.DiscountId);
                        DiscountAppliedToItem.DeleteByDiscount(discount.DiscountId);
                        DiscountUsageHistory.DeleteByDiscount(discount.DiscountId);
                        DiscountGift.DeleteByDiscount(discount.DiscountId);
                        DiscountCoupon.DeleteByDiscount(discount.DiscountId);
                        Discount.Delete(discount.DiscountId);

                        LogActivity.Write("Delete discount", discount.Name);

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

        protected string GetLimitation(int limitationTimes)
        {
            if (limitationTimes == 0)
                return "Không giới hạn";

            return limitationTimes.ToString();
        }

        protected string GetActive(bool isActive, object fromDate, object expiryDate)
        {
            if (isActive)
            {
                if (expiryDate != null && Convert.ToDateTime(expiryDate) < DateTime.Now)
                    return "<i class=\"fa fa-minus-circle text-danger\"></i> Hết hạn";

                return "<i class=\"fa fa-check text-success\"></i> " + Resources.ProductResources.CouponActiveLabel;
            }

            return "<i class=\"fa fa-pause text-danger\"></i> " + Resources.ProductResources.CouponInactiveLabel;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.DiscountPageTitle);
            heading.Text = ProductResources.DiscountPageTitle;

            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx";

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void LoadSettings()
        {
            AddClassToBody("admin-deals");
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            //this.ddlDiscountType.SelectedIndexChanged += new EventHandler(ddlDiscountType_SelectedIndexChanged);
            //this.ddlDiscountStatus.SelectedIndexChanged += ddlDiscountStatus_SelectedIndexChanged;
            this.btnSearch.Click += btnSearch_Click;

            gridPersister = new RadGridSEOPersister(grid);
        }

        #endregion OnInit
    }
}
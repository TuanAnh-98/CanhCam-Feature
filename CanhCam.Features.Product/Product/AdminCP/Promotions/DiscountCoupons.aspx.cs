/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2021-03-17
/// Last Modified:		    2021-03-17

using System;
using log4net;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using Resources;
using CanhCam.Business;
using Telerik.Web.UI;
using System.Web.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class DiscountCouponsPage : CmsNonBasePage
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountCouponsPage));
        private static List<Discount> lstDiscounts = new List<Discount>();

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

        void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            var isApplied = false;
            var discountId = -1;

            if (autDiscounts.Entries.Count > 0)
                discountId = Convert.ToInt32(autDiscounts.Entries[0].Value);

            var iCount = DiscountCoupon.GetCount(discountId, txtCouponCode.Text);

            var startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            var maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.PagerStyle.EnableSEOPaging = false;

            var lst = DiscountCoupon.GetPage(discountId, txtCouponCode.Text, startRowIndex, maximumRows);

            lstDiscounts = new List<Discount>();
            if (lst.Count > 0)
            {
                var discountIds = lst.Select(s => s.DiscountID).Distinct().ToList();
                if (discountIds.Count > 0)
                    lstDiscounts = Discount.GetDiscountCoupons(siteSettings.SiteId, null, string.Join(";", discountIds.ToArray()));
            }

            grid.DataSource = lst;
        }

        private void grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ViewOrders")
            {
                var couponCode = e.CommandArgument.ToString();
                if (!string.IsNullOrEmpty(couponCode))
                {
                    var lstOrders = Order.GetByCoupon(siteSettings.SiteId, couponCode);
                    if (lstOrders.Count > 0)
                    {
                        var item = e.Item as GridDataItem;
                        var btnViewOrders = item.FindControl("btnViewOrders") as LinkButton;
                        var litViewOrders = item.FindControl("litViewOrders") as Literal;

                        var results = string.Empty;
                        var sepa = string.Empty;
                        foreach (var obj in lstOrders)
                        {
                            results += sepa + string.Format("<a href=\"{0}/Product/AdminCP/OrderEdit.aspx?OrderID={1}\">{2}</a>", SiteRoot, obj.OrderId, obj.OrderCode);
                            sepa = "; ";
                        }

                        litViewOrders.Text = results;
                        btnViewOrders.Visible = false;
                    }
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    var guid = new Guid(data.GetDataKeyValue("Guid").ToString());

                    var coupon = new DiscountCoupon(guid);
                    if (coupon != null && coupon.Guid != Guid.Empty)
                    {
                        DiscountCoupon.Delete(guid);
                        LogActivity.Write("Delete coupon", coupon.CouponCode);

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

        protected string GetDiscountName(int discountId)
        {
            var obj = lstDiscounts.Where(s => s.DiscountId == discountId).FirstOrDefault();
            if (obj != null) return obj.Name;

            return string.Empty;
        }

        protected string GetActive(int discountId, int useCount, int limitationTimes)
        {
            var obj = lstDiscounts.Where(s => s.DiscountId == discountId).FirstOrDefault();
            if (obj != null)
            {
                if (obj.IsActive)
                {
                    if (obj.EndDate != null && Convert.ToDateTime(obj.EndDate) < DateTime.Now)
                        return "<i class=\"fa fa-minus-circle text-danger\"></i> Hết hạn";

                    if (limitationTimes > 0 && useCount >= limitationTimes)
                        return "Đã sử dụng";

                    return "<i class=\"fa fa-check text-success\"></i> " + Resources.ProductResources.CouponActiveLabel;
                }

                return "<i class=\"fa fa-pause text-danger\"></i> " + Resources.ProductResources.CouponInactiveLabel;
            }

            return string.Empty;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.CouponsPageTitle);
            heading.Text = ProductResources.CouponsPageTitle;
            //lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/Promotions/CouponEdit.aspx";
            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteSelectedConfirmMessage"));
        }

        private void LoadSettings()
        {
            AddClassToBody("admin-coupons");
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnSearch.Click += btnSearch_Click;
            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.grid.ItemCommand += grid_ItemCommand;
        }

        #endregion

        [WebMethod]
        public static AutoCompleteBoxData GetDiscounts(object context)
        {
            var searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToAsciiIfPossible().ToLower();
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var lst = new List<Discount>();

            if (!string.IsNullOrEmpty(searchString))
                lst = Discount.GetDiscountCoupons(siteSettings.SiteId, searchString, null);

            var result = new List<AutoCompleteBoxItemData>();
            foreach (var obj in lst)
            {
                var childNode = new AutoCompleteBoxItemData
                {
                    Text = obj.Name,
                    Value = obj.DiscountId.ToString()
                };
                childNode.Attributes.Add("Code", obj.Code);
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };

            return res;
        }

    }
}

using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.ProductUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.AccountUI
{
    public partial class CustomerEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderListPage));

        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private int userID = -1;
        private SiteUserCustom user = null;

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);

            this.Load += new EventHandler(this.Page_Load);

            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            btnInsert.Click += new EventHandler(btnUpdate_Click);
            btnInsertAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            btnInsertAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            //this.btnDelete.Click += new EventHandler(btnDelete_Click);
            btnResetRank.Click += BtnResetRank_Click;
            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);
            gridPersister = new RadGridSEOPersister(grid);
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

            PopulateLabels();

            if ((!Page.IsPostBack) && (!Page.IsCallback))
            {
                PopulateControls();
            }
        }

        private void LoadParams()
        {
            userID = WebUtils.ParseInt32FromQueryString("id", userID);
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            if (userID > 0)
            {
                user = new SiteUserCustom(userID);
            }
            divERPCode.Visible = ERPHelper.EnableERP;
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, AccountResources.CustomerEditTitle);
        }

        protected virtual void PopulateControls()
        {
            if (userID > 0)
            {
                btnInsert.Visible = false;
                btnInsertAndClose.Visible = false;
                btnInsertAndNew.Visible = false;

                txtName.Text = user.Name;
                txtLoginName.Text = user.LoginName;
                txtFirstName.Text = user.FirstName;
                txtLastName.Text = user.LastName;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.Phone;
                txtTotalOrders.Text = user.TotalOrdersBought.ToString();
                txtTotalMoney.Text = ProductHelper.FormatPrice(user.TotalMoney);
                txtConfirmedMoney.Text = ProductHelper.FormatPrice(user.TotalConfirmedMoney);
                txtUnconfirmedMoney.Text = ProductHelper.FormatPrice(user.TotalUnconfirmedMoney);

                SiteUser user2 = new SiteUser(siteSettings, user.UserID);
                if (divERPCode.Visible)
                {
                    string code = user2.GetCustomPropertyAsString(ERPHelper.CustomerCodeKey);
                    if (code != null)
                        txtCustomerCode.Text = code;
                }
                txtRank.Text = CanhCam.Web.ProductUI.MemberHelper.GetMemberRank(user2);
                txtTotalPoint.Text = user.TotalPosts.ToString();
                txtTotalApprovedPoint.Text = CanhCam.Business.UserPoint.GetTotalApprovedPointByUser(user.UserID).ToString();
            }
            else
            {
                btnUpdate.Visible = false;
                btnUpdateAndClose.Visible = false;
                btnUpdateAndNew.Visible = false;
                divOrderHistory.Visible = false;
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DateTime? fromdate = null;
            DateTime? todate = null;
            int status = -1;
            string txtKeyword = "";

            if (userID > 0)
            {
                bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;//Order.GetCount(siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, user.UserGuid, txtKeyword.Trim());
                int iCount = Order.GetCount(siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, user.UserGuid, -1, txtKeyword.Trim());
                int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
                int maximumRows = isApplied ? iCount : grid.PageSize;

                grid.VirtualItemCount = iCount;
                grid.AllowCustomPaging = !isApplied;
                grid.PagerStyle.EnableSEOPaging = !isApplied;

                List<Order> list = Order.GetPage(siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, user.UserGuid, -1, txtKeyword.Trim(), startRowIndex, maximumRows);
                grid.DataSource = list;
            }
        }

        private void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                HyperLink lnkQuickView = (HyperLink)item.FindControl("lnkQuickView");
                int orderId = Convert.ToInt32(item.GetDataKeyValue("OrderId"));

                RadToolTipManager1.TargetControls.Add(lnkQuickView.ClientID, orderId.ToString(), true);
            }
        }

        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            OrderDetailToolTipControl ctrl = Page.LoadControl("/Product/Controls/OrderDetailToolTipControl.ascx") as OrderDetailToolTipControl;
            ctrl.ID = "UcOrderDetail1";
            ctrl.OrderId = Convert.ToInt32(e.Value);
            e.UpdatePanel.ContentTemplateContainer.Controls.Add(ctrl);
        }

        #region Save

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Member/CustomerEdit.aspx?id=" + nId.ToString());
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Member/CustomersList.aspx");
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            int nId = Save();
            if (nId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Member/CustomerEdit.aspx");
        }

        private int Save()
        {
            Page.Validate("customer");

            if (!Page.IsValid)
                return -1;

            SiteUser siteUser = userID > 0 ? new SiteUser(siteSettings, user.UserGuid) : new SiteUser(siteSettings);
            siteUser.Name = txtName.Text.Trim();
            siteUser.Email = txtEmail.Text.Trim();
            siteUser.LoginName = txtLoginName.Text.Trim();
            siteUser.FirstName = txtFirstName.Text.Trim();
            siteUser.LastName = txtLastName.Text.Trim();
            siteUser.Yahoo = txtPhone.Text.Trim();
            if (divERPCode.Visible)
                siteUser.SetProperty(ERPHelper.CustomerCodeKey, txtCustomerCode.Text);
            if (userID < 1)
            {
                siteUser.ProfileApproved = true;
                siteUser.ApprovedForLogin = true;
                siteUser.Trusted = false;
                siteUser.TotalPosts = 0;
                siteUser.TimeOffsetHours = 0;
            }
            if (siteUser.Save())
            {
                if (userID > 0)
                {
                    LogActivity.Write("Update customer", siteUser.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
                else
                {
                    LogActivity.Write("Create new customer", siteUser.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }
            }

            return siteUser.UserId;
        }

        private void BtnResetRank_Click(object sender, EventArgs e)
        {
            var totalApprovedUserPoint = UserPoint.GetTotalApprovedPointByUser(user.UserID);
            MemberRank memberRank = new MemberRank(null, Convert.ToInt32(txtTotalPoint.Text));
            SiteUser siteUser = userID > 0 ? new SiteUser(siteSettings, user.UserGuid) : new SiteUser(siteSettings);
            siteUser.ICQ = memberRank.RankOrder.ToString();
            if (siteUser.Save())
            {
                txtRank.Text = CanhCam.Web.ProductUI.MemberHelper.GetMemberRank(new SiteUser(siteSettings, user.UserID));
                LogActivity.Write("Update customer", siteUser.Name);
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
            }
        }

        #endregion Save
    }
}
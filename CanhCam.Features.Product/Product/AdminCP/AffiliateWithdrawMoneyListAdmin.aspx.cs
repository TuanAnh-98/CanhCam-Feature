using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class AffiliateWithdrawMoneyListAdmin : CmsNonBasePage
    {
        #region Properties
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateOrderList));
        private Order order;
        private Product product;
        private OrderItem orderItem;
        private SiteSettings siteSettings;
        #endregion

        #region OnInit
        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.grid.NeedDataSource += Grid_NeedDataSource;
            this.grid.ItemCommand += Grid_ItemCommand;
            this.grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);
            this.btnDelete.Click += BtnDelete_Click;
            this.btnUpdate.Click += BtnUpdate_Click;
            this.btnExportExcel.Click += BtnExportExcel_Click;
        }

        private void DdlCartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void BtnExportExcel_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {


        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region Events

        #endregion

        #region grid
        private void Grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            bool isApplied = true;

            int iCount = AffiliatePayment.GetCount();

            int pageNumber = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int pageSize = isApplied ? iCount : grid.PageSize;


            int userid = SiteUtils.GetCurrentSiteUser().UserId;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = isApplied;
            grid.PagerStyle.EnableSEOPaging = false;
            grid.DataSource = AffiliatePayment.GetPage(pageNumber, pageSize, out _);
        }

        private void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;

                string orderStatus = item.GetDataKeyValue("Status").ToString();
                Button btnProcessOrder = (Button)item.FindControl("btnPayment");
                Label lbPaymentSatus = (Label)item.FindControl("lbPaymentStatus");
                if (btnProcessOrder != null)
                {
                    btnProcessOrder.Visible = false;
                    if (orderStatus != "True")
                    {
                        btnProcessOrder.Visible = true;
                        lbPaymentSatus.Visible = false;
                    }
                }
            }
        }

        private void Grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ProcessPayment")
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                Guid paymentId = Guid.Parse(item.GetDataKeyValue("AffGuid").ToString());
                var payment = new AffiliatePayment(paymentId);
                if (payment != null
                    && payment.AffID > 0)
                {
                    decimal orderTotal = 0;
                    decimal expectedAmount = 0;
                    var orders = Affiliate.GetPage(1, 32727, out _, payment.AffUser);
                    foreach (var order in orders)
                    {
                        log.Info(order.OrderStatus);
                        if (order.OrderStatus == 99)
                        {
                            orderTotal += (order.Price * order.Quantity * Convert.ToDecimal("0,3"));
                        }
                        if (order.OrderStatus == 5
                            || order.OrderStatus == 10
                            || order.OrderStatus == 15
                            || order.OrderStatus == 20
                            || order.OrderStatus == 25
                            || order.OrderStatus == 0)
                        {
                            expectedAmount += (order.Price * order.Quantity * Convert.ToDecimal("0,3"));
                        }
                    }
                    if(payment.WithdrawMoney >= 200000 && (orderTotal - payment.WithdrawMoney) >= 0)
                    {
                        payment.DateRespond = DateTime.UtcNow;
                        payment.Status = true;
                        payment.Save();
                        NotifyMessage1.SuccessMessage = "Nhận thanh toán thành công";
                        grid.Rebind();
                    }
                    else
                    {
                        NotifyMessage1.ErrorMessage = "Thanh toán thất bại do không đủ tiền trong tài khoản";
                    }
                    
                }
                return;
            }
        }
        #endregion grid

        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {

            LoadSettings();
            LoadControl();

            if (!Page.IsPostBack)
            {
                PopulateControl();
            }

        }
        #endregion

        #region LoadSettings
        private void LoadControl()
        {
        }

        private void LoadSettings()
        {
        }
        #endregion

        #region Populate
        private void PopulateControl()
        {
        }
        #endregion
    }
}
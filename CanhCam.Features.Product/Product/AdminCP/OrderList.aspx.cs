/// Created:				2014-07-22
/// Last Modified:			2014-07-22

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.StoreUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class OrderListPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderListPage));

        //  private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        private SiteUser currentUser = null;
        private bool enabledStore = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (!WebUser.IsAdminOrContentAdmin && !ProductPermission.CanManageOrders)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        #region "RadGrid Event"

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Int32.TryParse(ddStore.SelectedValue, out int storeID);
            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;

            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            bool isApplied = false;
            int staffProcessId = -1;
            if (OrderHelper.OrderAutoHiddenOrderByStaffApplyed)
                staffProcessId = currentUser.UserId;
            int iCount = Order.GetCountByStores(userID: currentUser.UserId, isAdmin: (WebUser.IsAdmin || !enabledStore),
                storeID: storeID, siteID: siteSettings.SiteId,
                orderStatus: status, fromDate: fromdate, toDate: todate,
                keyword: txtKeyword.Text.Trim(), staffProcessId: staffProcessId);
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;

            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;

            List<Order> list = Order.GetPageByStores(userID: currentUser.UserId, isAdmin: (WebUser.IsAdmin || !enabledStore),
                storeID: storeID, siteID: siteSettings.SiteId,
                orderStatus: status, fromDate: fromdate, toDate: todate,
                keyword: txtKeyword.Text.Trim(), staffProcessId: staffProcessId,
                pageNumber: startRowIndex, pageSize: maximumRows, getStoreName: true);
            grid.DataSource = list;
        }

        private void grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                HyperLink lnkQuickView = (HyperLink)item.FindControl("lnkQuickView");
                DropDownList ddOrderStatus = (DropDownList)item.FindControl("ddOrderStatus");
                int orderId = Convert.ToInt32(item.GetDataKeyValue("OrderId"));
                int orderStatus = Convert.ToInt32(item.GetDataKeyValue("OrderStatus"));
                int apiOrderStatus = Convert.ToInt32(item.GetDataKeyValue("ApiOrderStatus"));
                RadToolTipManager1.TargetControls.Add(lnkQuickView.ClientID, orderId.ToString(), true);
                if (ddOrderStatus != null)
                {
                    OrderHelper.PopulateOrderStatus(ddOrderStatus, false);
                    ListItem li = ddOrderStatus.Items.FindByValue(orderStatus.ToString());
                    if (li != null)
                    {
                        ddOrderStatus.ClearSelection();
                        li.Selected = true;
                    }
                }
                int staffProcessId = Convert.ToInt32(item.GetDataKeyValue("StaffProcessId"));
                Button btnProcessOrder = (Button)item.FindControl("btnProcessOrder");
                if (btnProcessOrder != null)
                {
                    btnProcessOrder.Visible = false;
                    if (staffProcessId == -1 && OrderHelper.OrderStaffProcessEnable
                        && orderStatus != (int)OrderStatus.Cancelled
                        && orderStatus != (int)OrderStatus.Completed)
                    {
                        btnProcessOrder.Visible = true;
                        HyperLink editLink = (HyperLink)item.FindControl("EditLink");
                        if (editLink != null)
                            editLink.Visible = false;
                    }
                }

                if (orderStatus == (int)OrderStatus.Completed && apiOrderStatus != (int)OrderERPStatus.Synchronized)
                    e.Item.Attributes.Add("style", "background-color: #ffc10745;");
            }
        }


        private void Grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName == "ProcessOrder")
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                int orderId = Convert.ToInt32(item.GetDataKeyValue("OrderId"));
                var order = new Order(orderId);
                if (order != null
                    && order.OrderId > 0)
                {
                    if (order.StaffProcessId == -1)
                    {
                        order.StaffProcessId = SiteUtils.GetCurrentSiteUser().UserId;
                        order.StaffId = SiteUtils.GetCurrentSiteUser().UserId;
                        order.Save();
                        message.SuccessMessage = "Nhận order thành công";
                        WebUtils.SetupRedirect(this, "/Product/AdminCP/OrderEdit.aspx?OrderID=" + order.OrderId);
                    }
                    else
                    {
                        SiteUser user = new SiteUser(siteSettings, order.StaffProcessId);
                        string staffProcessName = user.Name;
                        message.WarningMessage = "Đơn hàng đang được xử lý bởi " + staffProcessName;
                        WebUtils.SetupRedirect(this, "/Product/AdminCP/OrderEdit.aspx?OrderID=" + order.OrderId);
                    }
                }
                return;
            }
        }


        protected void OnAjaxUpdate(object sender, ToolTipUpdateEventArgs e)
        {
            OrderDetailToolTipControl ctrl = Page.LoadControl("/Product/Controls/OrderDetailToolTipControl.ascx") as OrderDetailToolTipControl;
            ctrl.ID = "UcOrderDetail1";
            ctrl.OrderId = Convert.ToInt32(e.Value);
            e.UpdatePanel.ContentTemplateContainer.Controls.Add(ctrl);
        }

        #endregion "RadGrid Event"

        #region Event

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.CurrentPageIndex = 0;
            grid.Rebind();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int iRecordDeleted = 0;
                foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                {
                    int orderId = Convert.ToInt32(data.GetDataKeyValue("OrderId"));
                    Order order = new Order(orderId);

                    if (order != null && order.OrderId > 0 && order.SiteId == siteSettings.SiteId && !order.IsDeleted)
                    {
                        ContentDeleted.Create(siteSettings.SiteId, order.OrderId.ToString(), "Order", typeof(OrderDeleted).AssemblyQualifiedName, order.OrderId.ToString(), Page.User.Identity.Name);

                        order.IsDeleted = true;
                        order.Save();

                        iRecordDeleted += 1;
                    }
                }

                if (iRecordDeleted > 0)
                {
                    LogActivity.Write("Delete " + iRecordDeleted.ToString() + " order(s)", "Order");
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void ddOrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                DropDownList ddOrderStatus = (DropDownList)sender;
                if (ddOrderStatus != null && ddOrderStatus.SelectedValue.Length > 0)
                {
                    GridDataItem dataItem = (GridDataItem)ddOrderStatus.NamingContainer;
                    int orderId = Convert.ToInt32(dataItem.GetDataKeyValue("OrderId"));

                    Order order = new Order(orderId);
                    if (order != null && order.OrderId > 0)
                    {
                        order.OrderStatus = Convert.ToInt32(ddOrderStatus.SelectedValue);
                        order.Save();

                        LogActivity.Write("Update order's status", order.OrderId.ToString());
                        message.SuccessMessage = ProductResources.OrderStatusUpdateSuccessMessage;
                        grid.Rebind();
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetOrdersForExport();

            string fileName = "orders-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        protected void btnExportForERP_Click(object sender, EventArgs e)
        {
            DataTable dt = GetOrdersERPForExport();

            string fileName = "orders-data-BC-EC-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);

        }
        protected void btnExportByProduct_Click(object sender, EventArgs e)
        {
            DataTable dt = GetOrderByProductForExport();

            string fileName = "orders-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        #endregion Event

        private DataTable GetOrderByProductForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("OrderCode", typeof(string));
            dt.Columns.Add("OrderStatus", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("CreatedTime", typeof(string));
            dt.Columns.Add("ProductCode", typeof(string));
            dt.Columns.Add("ProductName", typeof(string));
            dt.Columns.Add("Quantity", typeof(int));
            dt.Columns.Add("Price", typeof(double));
            dt.Columns.Add("SubTotal", typeof(double));
            dt.Columns.Add("OrderDiscount", typeof(double));
            dt.Columns.Add("OrderShipping", typeof(double));
            dt.Columns.Add("OrderTotal", typeof(double));

            List<ShippingMethod> lstShippingMethods = ShippingMethod.GetByActive(siteSettings.SiteId, 1);
            List<PaymentMethod> lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, 1);
            if (lstShippingMethods.Count > 0)
                dt.Columns.Add("ShippingMethod", typeof(string));
            if (lstPaymentMethods.Count > 0)
                dt.Columns.Add("PaymentMethod", typeof(string));

            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Address", typeof(string));

            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;
            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            var iCount = OrderItem.GetCountBySearch(siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, null, txtKeyword.Text.Trim());
            var lstOrderItems = OrderItem.GetPageBySearch(siteSettings.SiteId, -1, status, -1, -1, fromdate, todate, null, null, null, txtKeyword.Text.Trim(), 1, iCount);
            if (lstOrderItems.Count > 0)
            {
                List<Guid> lstProductGuids = new List<Guid>();
                List<Guid> lstProvinceGuids = new List<Guid>();
                List<Guid> lstDistrictGuids = new List<Guid>();
                foreach (OrderItem orderItem in lstOrderItems)
                {
                    if (!lstProductGuids.Contains(orderItem.ProductGuid))
                        lstProductGuids.Add(orderItem.ProductGuid);
                    if (!lstProvinceGuids.Contains(orderItem.Order.BillingProvinceGuid))
                        lstProvinceGuids.Add(orderItem.Order.BillingProvinceGuid);
                    if (!lstDistrictGuids.Contains(orderItem.Order.BillingDistrictGuid))
                        lstDistrictGuids.Add(orderItem.Order.BillingDistrictGuid);
                }

                List<GeoZone> lstProvinces = new List<GeoZone>();
                List<GeoZone> lstDistricts = new List<GeoZone>();
                if (lstProvinceGuids.Count > 0)
                    lstProvinces = GeoZone.GetByGuids(string.Join(";", lstProvinceGuids.ToArray()), 1);
                if (lstDistrictGuids.Count > 0)
                    lstDistricts = GeoZone.GetByGuids(string.Join(";", lstDistrictGuids.ToArray()), 1);
                if (lstProvinces.Count > 0)
                    dt.Columns.Add("Province", typeof(string));
                if (lstDistricts.Count > 0)
                    dt.Columns.Add("District", typeof(string));

                int i = 1;
                List<Product> lstProducts = Product.GetByGuids(siteSettings.SiteId, string.Join(";", lstProductGuids.ToArray()), -1);
                foreach (OrderItem orderItem in lstOrderItems)
                {
                    foreach (Product product in lstProducts)
                    {
                        if (orderItem.ProductId == product.ProductId)
                        {
                            DataRow row = dt.NewRow();

                            row["Number"] = i;
                            row["OrderCode"] = orderItem.Order.OrderCode;
                            row["OrderStatus"] = ProductHelper.GetOrderStatus(orderItem.Order.OrderStatus);
                            row["Date"] = ProductHelper.FormatDate(orderItem.Order.CreatedUtc, timeZone, timeOffset, "dd/MM/yyyy");
                            row["CreatedTime"] = ProductHelper.FormatDate(orderItem.Order.CreatedUtc, timeZone, timeOffset, "HH:mm");

                            row["ProductCode"] = product.Code;
                            row["ProductName"] = product.Title;
                            row["Quantity"] = orderItem.Quantity;
                            row["Price"] = Convert.ToDouble(orderItem.Price);

                            row["SubTotal"] = Convert.ToDouble(orderItem.Order.OrderSubtotal);
                            row["OrderDiscount"] = Convert.ToDouble(orderItem.Order.OrderDiscount);
                            row["OrderShipping"] = Convert.ToDouble(orderItem.Order.OrderShipping);
                            row["OrderTotal"] = Convert.ToDouble(orderItem.Order.OrderTotal);

                            if (lstShippingMethods.Count > 0)
                            {
                                string name = string.Empty;
                                if (orderItem.Order.ShippingMethod > 0)
                                {
                                    foreach (ShippingMethod method in lstShippingMethods)
                                    {
                                        if (method.ShippingMethodId == orderItem.Order.ShippingMethod)
                                        {
                                            name = method.Name;
                                            break;
                                        }
                                    }
                                }

                                row["ShippingMethod"] = name;
                            }
                            if (lstPaymentMethods.Count > 0)
                            {
                                string name = string.Empty;
                                if (orderItem.Order.PaymentMethod > 0)
                                {
                                    foreach (PaymentMethod method in lstPaymentMethods)
                                    {
                                        if (method.PaymentMethodId == orderItem.Order.PaymentMethod)
                                        {
                                            name = method.Name;
                                            break;
                                        }
                                    }
                                }

                                row["PaymentMethod"] = name;
                            }

                            row["FirstName"] = orderItem.Order.BillingFirstName;
                            row["LastName"] = orderItem.Order.BillingLastName;
                            row["Email"] = orderItem.Order.BillingEmail;
                            row["Phone"] = orderItem.Order.BillingPhone;
                            row["Address"] = orderItem.Order.BillingAddress;

                            if (lstProvinces.Count > 0)
                            {
                                string name = string.Empty;
                                if (orderItem.Order.BillingProvinceGuid != Guid.Empty)
                                {
                                    foreach (GeoZone gz in lstProvinces)
                                    {
                                        if (gz.Guid == orderItem.Order.BillingProvinceGuid)
                                        {
                                            name = gz.Name;
                                            break;
                                        }
                                    }
                                }

                                row["Province"] = name;
                            }
                            if (lstDistricts.Count > 0)
                            {
                                string name = string.Empty;
                                if (orderItem.Order.BillingDistrictGuid != Guid.Empty)
                                {
                                    foreach (GeoZone gz in lstDistricts)
                                    {
                                        if (gz.Guid == orderItem.Order.BillingDistrictGuid)
                                        {
                                            name = gz.Name;
                                            break;
                                        }
                                    }
                                }

                                row["District"] = name;
                            }

                            i += 1;
                            dt.Rows.Add(row);

                            break;
                        }
                    }
                }
            }

            return dt;
        }

        private DataTable GetOrdersForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Number", typeof(int));
            dt.Columns.Add("OrderCode", typeof(string));
            dt.Columns.Add("OrderStatus", typeof(string));
            dt.Columns.Add("CancelNote", typeof(string));
            dt.Columns.Add("Date", typeof(string));
            dt.Columns.Add("CreatedTime", typeof(string));
            dt.Columns.Add("Products", typeof(string));
            dt.Columns.Add("SubTotal", typeof(double));
            dt.Columns.Add("OrderDiscount", typeof(double));
            dt.Columns.Add("OrderShipping", typeof(double));
            dt.Columns.Add("OrderTotal", typeof(double));

            List<ShippingMethod> lstShippingMethods = ShippingMethod.GetByActive(siteSettings.SiteId, 1);
            List<PaymentMethod> lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, 1);
            if (lstShippingMethods.Count > 0)
                dt.Columns.Add("ShippingMethod", typeof(string));
            if (lstPaymentMethods.Count > 0)
                dt.Columns.Add("PaymentMethod", typeof(string));

            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Address", typeof(string));

            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;
            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            int staffProcessId = -1;
            if (OrderHelper.OrderAutoHiddenOrderByStaffApplyed)
                staffProcessId = currentUser.UserId;
            int.TryParse(ddStore.SelectedValue, out int storeID);
            List<Order> lstOrders = Order.GetPageByStores(userID: currentUser.UserId, isAdmin: (WebUser.IsAdmin || !enabledStore),
                storeID: storeID, siteID: siteSettings.SiteId,
                orderStatus: status, fromDate: fromdate, toDate: todate,
                keyword: txtKeyword.Text.Trim(), staffProcessId: staffProcessId, getStoreName: true);
            if (lstOrders.Count > 0)
            {
                List<Guid> lstProvinceGuids = new List<Guid>();
                List<Guid> lstDistrictGuids = new List<Guid>();
                foreach (Order o in lstOrders)
                {
                    if (!lstProvinceGuids.Contains(o.BillingProvinceGuid))
                        lstProvinceGuids.Add(o.BillingProvinceGuid);
                    if (!lstDistrictGuids.Contains(o.BillingDistrictGuid))
                        lstDistrictGuids.Add(o.BillingDistrictGuid);
                }

                List<GeoZone> lstProvinces = new List<GeoZone>();
                List<GeoZone> lstDistricts = new List<GeoZone>();
                if (lstProvinceGuids.Count > 0)
                    lstProvinces = GeoZone.GetByGuids(string.Join(";", lstProvinceGuids.ToArray()), 1);
                if (lstDistrictGuids.Count > 0)
                    lstDistricts = GeoZone.GetByGuids(string.Join(";", lstDistrictGuids.ToArray()), 1);
                if (lstProvinces.Count > 0)
                    dt.Columns.Add("Province", typeof(string));
                if (lstDistricts.Count > 0)
                    dt.Columns.Add("District", typeof(string));

                int i = 1;
                foreach (Order o in lstOrders)
                {
                    DataRow row = dt.NewRow();

                    row["Number"] = i;
                    row["OrderCode"] = o.OrderCode;
                    row["OrderStatus"] = ProductHelper.GetOrderStatus(o.OrderStatus);
                    row["CancelNote"] = o.CancelNote;
                    row["Date"] = ProductHelper.FormatDate(o.CreatedUtc, timeZone, timeOffset, "dd/MM/yyyy");
                    row["CreatedTime"] = ProductHelper.FormatDate(o.CreatedUtc, timeZone, timeOffset, "HH:mm");

                    var lstProducts = Product.GetByOrder(siteSettings.SiteId, o.OrderId);
                    string products = string.Empty;
                    string sepa = string.Empty;
                    foreach (Product product in lstProducts)
                    {
                        products += sepa + product.Title;
                        sepa = ";" + System.Environment.NewLine;
                    }
                    row["Products"] = products;

                    row["SubTotal"] = Convert.ToDouble(o.OrderSubtotal);
                    row["OrderDiscount"] = Convert.ToDouble(o.OrderDiscount);
                    row["OrderShipping"] = Convert.ToDouble(o.OrderShipping);
                    row["OrderTotal"] = Convert.ToDouble(o.OrderTotal);

                    if (lstShippingMethods.Count > 0)
                    {
                        string name = string.Empty;
                        if (o.ShippingMethod > 0)
                        {
                            foreach (ShippingMethod method in lstShippingMethods)
                            {
                                if (method.ShippingMethodId == o.ShippingMethod)
                                {
                                    name = method.Name;
                                    break;
                                }
                            }
                        }

                        row["ShippingMethod"] = name;
                    }
                    if (lstPaymentMethods.Count > 0)
                    {
                        string name = string.Empty;
                        if (o.PaymentMethod > 0)
                        {
                            foreach (PaymentMethod method in lstPaymentMethods)
                            {
                                if (method.PaymentMethodId == o.PaymentMethod)
                                {
                                    name = method.Name;
                                    break;
                                }
                            }
                        }

                        row["PaymentMethod"] = name;
                    }

                    row["FirstName"] = o.BillingFirstName;
                    row["LastName"] = o.BillingLastName;
                    row["Email"] = o.BillingEmail;
                    row["Phone"] = o.BillingPhone;
                    row["Address"] = o.BillingAddress;

                    if (lstProvinces.Count > 0)
                    {
                        string name = string.Empty;
                        if (o.BillingProvinceGuid != Guid.Empty)
                        {
                            foreach (GeoZone gz in lstProvinces)
                            {
                                if (gz.Guid == o.BillingProvinceGuid)
                                {
                                    name = gz.Name;
                                    break;
                                }
                            }
                        }

                        row["Province"] = name;
                    }
                    if (lstDistricts.Count > 0)
                    {
                        string name = string.Empty;
                        if (o.BillingDistrictGuid != Guid.Empty)
                        {
                            foreach (GeoZone gz in lstDistricts)
                            {
                                if (gz.Guid == o.BillingDistrictGuid)
                                {
                                    name = gz.Name;
                                    break;
                                }
                            }
                        }

                        row["District"] = name;
                    }

                    i += 1;
                    dt.Rows.Add(row);
                }
            }

            return dt;
        }


        private DataTable GetOrdersERPForExport()
        {

            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Mã đơn hàng", typeof(string));
            dt.Columns.Add("Mã đơn đồng bộ", typeof(string));
            dt.Columns.Add("Ngày đặt hàng", typeof(string));
            dt.Columns.Add("Ngày hoàn tất", typeof(string));
            dt.Columns.Add("Trạng thái", typeof(string));
            dt.Columns.Add("Điểm thưởng đã đổi", typeof(string));
            dt.Columns.Add("Vouchers", typeof(string));
            dt.Columns.Add("Tổng đơn hàng", typeof(string));
            dt.Columns.Add("Hình thức thanh toán", typeof(string));
            dt.Columns.Add("Hình thức giao hàng", typeof(string));
            dt.Columns.Add("Nhân viên xử lý", typeof(string));


            int status = Convert.ToInt32(ddOrderStatus.SelectedValue);
            DateTime? fromdate = null;
            DateTime? todate = null;
            if (dpFromDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpFromDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpToDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpToDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            int staffProcessId = -1;
            if (OrderHelper.OrderAutoHiddenOrderByStaffApplyed)
                staffProcessId = currentUser.UserId;
            int.TryParse(ddStore.SelectedValue, out int storeID);
            List<Order> orders = Order.GetPageByStores(userID: currentUser.UserId, isAdmin: (WebUser.IsAdmin || !enabledStore),
                storeID: storeID, siteID: siteSettings.SiteId,
                orderStatus: status, fromDate: fromdate, toDate: todate,
                keyword: txtKeyword.Text.Trim(), staffProcessId: staffProcessId, getStoreName: true);
            int i = 1;
            List<ShippingMethod> lstShippingMethods = ShippingMethod.GetByActive(siteSettings.SiteId, 1);
            List<PaymentMethod> lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, 1);
            foreach (var o in orders)
            {
                DataRow row = dt.NewRow();

                row["STT"] = i;
                row["Mã đơn hàng"] = o.OrderCode;
                row["Mã đơn đồng bộ"] = o.ApiOrderCode;
                row["Ngày đặt hàng"] = ProductHelper.FormatDate(o.CreatedUtc, timeZone, timeOffset, "dd-MM-yyyy HH:mm");
                if (o.CompletionedUtc.HasValue)
                    row["Ngày hoàn tất"] = ProductHelper.FormatDate(o.CompletionedUtc.Value, timeZone, timeOffset, "dd-MM-yyyy HH:mm");
                else
                    row["Ngày hoàn tất"] = string.Empty;
                row["Trạng thái"] = OrderHelper.GetOrderStatusResources(o.OrderStatus);
                row["Điểm thưởng đã đổi"] = ProductHelper.FormatPrice(o.RedeemedRewardPointsAmount, true);
                row["Vouchers"] = ProductHelper.FormatPrice(o.VoucherAmount, true);
                row["Tổng đơn hàng"] = ProductHelper.FormatPrice(o.OrderTotal, true);

                var paymentMethod = lstPaymentMethods.FirstOrDefault(m => m.PaymentMethodId == o.PaymentMethod);
                if (paymentMethod != null)
                    row["Hình thức thanh toán"] = paymentMethod.Name;
                else
                    row["Hình thức thanh toán"] = string.Empty;
                var shippingMethod = lstShippingMethods.FirstOrDefault(m => m.ShippingMethodId == o.ShippingMethod);
                if (shippingMethod != null)
                    row["Hình thức giao hàng"] = shippingMethod.Name;
                else
                    row["Hình thức giao hàng"] = string.Empty;
                string staffName = string.Empty;
                if (o.StaffId > 0)
                {
                    SiteUser siteUser = new SiteUser(siteSettings, o.StaffId);
                    if (siteUser != null)
                        staffName = siteUser.Name + "-" + siteUser.GetCustomPropertyAsString("StaffCode");
                }
                row["Nhân viên xử lý"] = staffName;
                i += 1;
                dt.Rows.Add(row);
            }

            return dt;
        }

        #region Populate

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.OrderAdminTitle);
            heading.Text = ProductResources.OrderAdminTitle;

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.OrderDeleteMultiWarning);
        }

        private void PopulateControls()
        {
            OrderHelper.PopulateOrderStatus(ddOrderStatus, true);
            PopulateStoreList();
            var column = grid.MasterTableView.Columns.FindByUniqueName("StaffProcess");
            if (column != null)
                column.Visible = OrderHelper.OrderStaffProcessEnable;

        }

        private void PopulateStoreList()
        {
            ddStore.Items.Clear();
            ddStore.Items.Add(new ListItem(StoreResources.InventoryStoreDropdown, string.Empty));

            var lstStores = Store.GetAll();
            if (!WebUser.IsAdmin)
            {
                foreach (Store store in lstStores.Reverse<Store>())
                {
                    var isAdmin = false;
                    var lstAdmins = store.OrderUserIDs.Split(';');
                    foreach (string adminID in lstAdmins)
                    {
                        if (adminID == currentUser.UserId.ToString())
                            isAdmin = true;
                    }
                    if (!isAdmin)
                        lstStores.Remove(store);
                }
            }

            ddStore.DataSource = lstStores;
            ddStore.DataBind();
        }

        #endregion Populate

        #region LoadSettings

        private void LoadSettings()
        {
            currentUser = SiteUtils.GetCurrentSiteUser();

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            btnExportByProduct.Visible = true;
            lnkInsert.NavigateUrl = SiteRoot + "/Product/AdminCP/OrderAdd.aspx";

            enabledStore = StoreHelper.EnabledStore;
            if (enabledStore)
                divStoreFilter.Visible = true;
            else
                divStoreFilter.Visible = false;

            btnDelete.Visible = ProductPermission.CanDeleteOrders;
        }

        #endregion LoadSettings

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            grid.ItemDataBound += new GridItemEventHandler(grid_ItemDataBound);
            grid.ItemCommand += Grid_ItemCommand;
            // gridPersister = new RadGridSEOPersister(grid);
        }
        #endregion OnInit

        #region Protected Method
        protected string GetUserName(int userId)
        {
            if (userId <= 0)
                return string.Empty;

            SiteUser user = new SiteUser(siteSettings, userId);
            if (user != null)
                return user.Name;

            return string.Empty;
        }
        #endregion

    }
}

namespace CanhCam.Web.ProductUI
{
    public class OrderDeleted : IContentDeleted
    {
        public bool RestoreContent(string orderId)
        {
            try
            {
                Order order = new Order(Convert.ToInt32(orderId));

                if (order != null && order.OrderId > 0)
                {
                    order.IsDeleted = false;
                    order.Save();
                }
            }
            catch (Exception) { return false; }

            return true;
        }

        public bool DeleteContent(string orderId)
        {
            try
            {
                Order.Delete(Convert.ToInt32(orderId));
            }
            catch (Exception) { return false; }

            return true;
        }
    }
}
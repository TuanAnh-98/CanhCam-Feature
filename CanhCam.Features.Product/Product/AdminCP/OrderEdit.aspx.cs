using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.StoreUI;
using CanhCam.Web.UI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class OrderEditPage : CmsNonBasePage
    {
        #region Private Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(OrderEditPage));

        private Order order;
        private int orderID;
        private List<Product> lstProducts = new List<Product>();
        private List<OrderItem> lstOrderItems = new List<OrderItem>();
        private decimal orderSubtotal = 0;
        private Store store;
        private bool isStoreAdmin = false;

        private UserAddress address;
        private List<UserAddress> lstAddress = new List<UserAddress>();

        private List<ProductProperty> productProperties = new List<ProductProperty>();
        private List<CustomField> customFields = new List<CustomField>();

        private SiteUser siteUser = null;

        private bool enabledStore = false;

        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        #endregion Private Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            //ACCESS DENIED
            bool accessDenied = false;

            if (!WebUser.IsAdminOrContentAdmin && !ProductPermission.CanManageOrders)
                accessDenied = true;

            LoadSettings();

            if (siteUser == null || siteUser.UserId <= 0)
                accessDenied = true;

            if (enabledStore)
            {
                orderID = WebUtils.ParseInt32FromQueryString("orderid", -1);
                if (orderID > 0 && order.StoreId < 1)
                {
                    if (!WebUser.IsAdmin)
                    {
                        accessDenied = true;
                    }
                }

                if (orderID > 0 && order.StoreId > 0)
                {
                    store = new Store(order.StoreId);
                    var lstStoreAdmins = store.OrderUserIDs.Split(';');
                    foreach (string adminID in lstStoreAdmins)
                    {
                        if (adminID == siteUser.UserId.ToString())
                            isStoreAdmin = true;
                    }
                    if (!isStoreAdmin && !WebUser.IsAdmin)
                    {
                        accessDenied = true;
                    }
                }
                else
                    store = new Store();
            }

            if (accessDenied)
            {
                SiteUtils.RedirectToAccessDeniedPage(this);
                return;
            }
            //END ACCESS DENIED

            PopulateLabels();
            if (!Page.IsPostBack)
                PopulateControls();
        }

        #region Product

        private List<OrderItem> SelectedItems
        {
            get
            {
                if (ViewState["SelectedOrderItems"] != null)
                    return ViewState["SelectedOrderItems"] as List<OrderItem>;

                return new List<OrderItem>();
            }
            set
            {
                ViewState["SelectedOrderItems"] = value;
            }
        }

        private void autCustomers_EntryAdded(object sender, Telerik.Web.UI.AutoCompleteEntryEventArgs e)
        {
            if (autCustomers.Entries.Count > 1)
                autCustomers.Entries.RemoveAt(0);

            var userGuid = new Guid(autCustomers.Entries[0].Value);
            var siteUser = new SiteUser(siteSettings, userGuid);

            lstAddress = UserAddress.GetByUser(siteUser.UserId);
            if (lstAddress == null || lstAddress.Count < 1)
            {
                txtBillingFirstName.Text = siteUser.FirstName;
                txtBillingLastName.Text = siteUser.LastName;
                txtBillingEmail.Text = siteUser.Email;
                return;
            }
            address = lstAddress.Where(s => s.IsDefault == true).FirstOrDefault();
            if (address == null || address.AddressId < 1)
                address = lstAddress.First();
            if (address != null && address.AddressId > 0)
            {
                txtBillingFirstName.Text = address.FirstName;
                txtBillingLastName.Text = address.LastName;
                txtBillingEmail.Text = address.Email;
                txtBillingPhone.Text = address.Phone;
                txtBillingAddress.Text = address.Address;

                var item = ddBillingProvince.Items.FindByValue(address.ProvinceGuid.ToString());
                if (item != null)
                {
                    ddBillingProvince.ClearSelection();
                    item.Selected = true;
                    BindDistrict(ddBillingProvince, ddBillingDistrict);
                    litBillingProvince.Text = ddBillingProvince.SelectedItem.Text;
                }

                item = ddBillingDistrict.Items.FindByValue(address.DistrictGuid.ToString());
                if (item != null)
                {
                    ddBillingDistrict.ClearSelection();
                    item.Selected = true;
                    litBillingDistrict.Text = ddBillingDistrict.SelectedItem.Text;
                }
            }
            else
            {
                txtBillingFirstName.Text = siteUser.FirstName;
                txtBillingLastName.Text = siteUser.LastName;
                txtBillingEmail.Text = siteUser.Email;
            }
        }

        private void autCustomers_EntryRemoved(object sender, AutoCompleteEntryEventArgs e)
        {
            txtBillingFirstName.Text = String.Empty;
            txtBillingLastName.Text = String.Empty;
            txtBillingEmail.Text = String.Empty;
        }

        private void autProducts_EntryAdded(object sender, Telerik.Web.UI.AutoCompleteEntryEventArgs e)
        {
            var lstOrderItems = SelectedItems;
            var productId = Convert.ToInt32(autProducts.Entries[0].Value);

            var orderItem = FindOrderItemByProductId(lstOrderItems, productId);
            if (orderItem != null)// Exists
            {
                orderItem.Quantity += 1;

                SelectedItems = lstOrderItems;
            }
            else// Not Exists
            {
                var product = new Product(siteSettings.SiteId, productId);
                if (product != null && product.ProductId > 0)
                {
                    var item = new OrderItem
                    {
                        OrderId = -1,
                        ProductGuid = product.ProductGuid,
                        ProductId = product.ProductId,
                        OriginalProductCost = product.OldPrice,
                        Price = product.Price,
                        Quantity = 1
                    };

                    lstOrderItems.Add(item);
                    SelectedItems = lstOrderItems;
                }
            }

            grid.Rebind();
            autProducts.Entries.Clear();
        }

        private OrderItem FindOrderItemByProductId(List<OrderItem> lstOrderItems, int productId)
        {
            foreach (var orderItem in lstOrderItems)
            {
                if (orderItem.ProductId == productId)
                    return orderItem;
            }

            return null;
        }

        #endregion Product

        #region "RadGrid Event"

        private void gridLogs_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            bool isApplied = false;
            orderID = WebUtils.ParseInt32FromQueryString("orderid", -1);
            int iCount = OrderLog.GetCount(orderID);
            int startRowIndex = isApplied ? 1 : gridLogs.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : gridLogs.PageSize;
            gridLogs.VirtualItemCount = iCount;
            gridLogs.AllowCustomPaging = !isApplied;
            gridLogs.DataSource = OrderLog.GetPage(orderID, startRowIndex, maximumRows);
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            orderSubtotal = 0;

            var lstOrderItems = SelectedItems;
            var productIds = GetSelectedProductIds(lstOrderItems);
            if (productIds.Length > 0)
                lstProducts = Product.GetPageAdv(productIds: productIds, siteId: siteSettings.SiteId);

            if (lstOrderItems.Count > 0)
            {
                var itemDiscount = lstOrderItems.Sum(s => s.DiscountAmount);
                litDiscountItems.Text = ProductHelper.FormatPrice(-itemDiscount, true);
                if (order.OrderDiscount > itemDiscount
                    && (order.OrderDiscount - itemDiscount) > 0)
                    litDiscountOrder.Text = ProductHelper.FormatPrice(-(order.OrderDiscount - itemDiscount), true);
                else
                    litDiscountOrder.Text = ProductHelper.FormatPrice(0, true);
            }

            grid.DataSource = lstOrderItems;
        }

        private string GetSelectedProductIds(List<OrderItem> lstOrderItems)
        {
            string productIds = string.Empty;
            foreach (OrderItem item in lstOrderItems)
            {
                productIds += item.ProductId.ToString() + ";";
            }

            return productIds;
        }

        private void grid_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            //var btnDelete = e.Item.FindControl("btnDelete") as Button;
            //if (btnDelete != null)
            //    UIHelper.AddConfirmationDialog(btnDelete, "Bạn có chắc chắn muốn hủy sản phẩm này?");
            if (e.Item is Telerik.Web.UI.GridDataItem item)
            {
                var litProductCode = (Literal)item.FindControl("litProductCode");
                var litProductName = (Literal)item.FindControl("litProductName");
                var ml = (MediaElement)item.FindControl("ml");
                var productId = Convert.ToInt32(item.GetDataKeyValue("ProductId"));
                litProductCode.Visible = displaySettings.ShowProductCode;

                var product = ProductHelper.GetProductFromList(lstProducts, productId);
                if (product != null)
                {
                    litProductCode.Text = product.Code;
                    litProductName.Text = string.Format("<a href='{0}'>{1}</a>", ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId), product.Title);

                    if (ml != null && !string.IsNullOrEmpty(product.ImageFile))
                        ml.FileUrl = ProductHelper.GetImageFilePath(siteSettings.SiteId, product.ProductId, product.ImageFile, product.ThumbnailFile);

                    var litAttributes = (Literal)item.FindControl("litAttributes");
                    if (litAttributes != null)
                    {
                        var attributesXml = item.GetDataKeyValue("AttributesXml").ToString();
                        var results = string.Empty;

                        if (ProductConfiguration.EnableShoppingCartAttributes)
                        {
                            foreach (var a in customFields.Where(c => (c.Options & (int)CustomFieldOptions.EnableShoppingCart) > 0))
                            {
                                productProperties.ForEach(property =>
                                {
                                    if (property.ProductId == productId
                                        && property.CustomFieldId == a.CustomFieldId)
                                        results += string.Format("<div><span>{0}</span>: {1}</div>", a.Name, property.OptionName);
                                });
                            }
                        }
                        else if (!string.IsNullOrEmpty(attributesXml))
                        {
                            var attributes = ProductAttributeParser.ParseProductAttributeMappings(customFields, attributesXml);
                            if (attributes.Count > 0)
                            {
                                foreach (var a in attributes)
                                {
                                    var values = ProductAttributeParser.ParseValues(attributesXml, a.CustomFieldId);
                                    if (values.Count > 0)
                                    {
                                        productProperties.ForEach(property =>
                                        {
                                            if (property.ProductId == productId
                                                && property.CustomFieldId == a.CustomFieldId
                                                && values.Contains(property.CustomFieldOptionId))
                                                results += string.Format("<div><span>{0}</span>: {1}</div>", a.Name, property.OptionName);
                                        });
                                    }
                                }
                            }
                        }

                        if (results.Length > 0)
                            litAttributes.Text = string.Format("<div class='attributes'>{0}</div>", results);
                    }

                    //Dynamic order total price
                    //int productQuantity = Convert.ToInt32(item.GetDataKeyValue("Quantity"));
                    var productQuantity = Int32.Parse(((TextBox)item.FindControl("txtQuantity")).Text);
                    orderSubtotal += product.Price * productQuantity;
                    litOrderSubtotal.Text = ProductHelper.FormatPrice(orderSubtotal, true);
                    var orderTotal = (order != null) ? (CartHelper.GetCartTotal(
                        subTotal: order.OrderSubtotal,
                        discountTotal: order.OrderDiscount,
                        tax: order.OrderTax,
                        shippingTotal: order.OrderShipping,
                        rewardPointAmount: order.RedeemedRewardPointsAmount,
                        discountCouponTotal: order.OrderCouponAmount,
                        voucherAmount: order.VoucherAmount,
                        serviceFee: order.OrderServiceFee
                        )) : orderSubtotal;

                    litOrderTotal.Text = ProductHelper.FormatPrice(orderTotal, true);
                }
            }
        }

        #endregion "RadGrid Event"

        #region Event

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Page.Validate("OrderEdit");
            if (!Page.IsValid) return;

            try
            {
                var lstOrderItems = SelectedItems; // new order items
                var lstItems = new List<OrderItem>();
                if (lstOrderItems.Count == 0)
                {
                    message.ErrorMessage = "Vui lòng chọn sản phẩm.";
                    return;
                }
                int orderStaffProcessId = -1;
                if (cbStaffProcess.Entries.Count > 0)
                    int.TryParse(cbStaffProcess.Entries[0].Value, out orderStaffProcessId);

                if (ConfigHelper.GetBoolProperty("Order:ValidateStatusWhenCompleted", false)
                    && Convert.ToInt32(ddOrderStatus.SelectedValue) == (int)OrderStatus.Completed
                    )
                {
                    bool valid = true;
                    string builder = string.Empty;
                    if (Convert.ToInt32(ddlPaymentStatus.SelectedValue) != (int)PaymentStatus.Paid)
                    {
                        builder = "<p>+ Trạng thái thanh toán không hợp lệ</p>";
                        valid = false;
                    }
                    if (Convert.ToInt32(ddlShippingStatus.SelectedValue) != (int)ShippingStatus.Sent)
                    {
                        builder = "<p>+ Trạng thái giao hàng không hợp lệ</p>";
                        valid = false;
                    }
                    if (orderStaffProcessId <= 0)
                    {
                        builder = "<p>+ Vui lòng chọn nhân viên xử lý đơn</p>";
                        valid = false;
                    }
                    if (!valid)
                    {
                        message.ErrorMessage = "<p>Vui lòng kiểm tra thông tin và trạng thái để cập nhật thành đơn hàng hoàn tất!</p>" + builder;
                        return;
                    }
                }

                if (order == null)
                {
                    order = new Order
                    {
                        OrderCode = ProductHelper.GenerateOrderCode(siteSettings.SiteId),
                        SiteId = siteSettings.SiteId,

                        //order.UserGuid = siteUser.UserGuid;
                        CreatedBy = siteUser.Name
                    };
                }
                else
                {
                    lstItems = OrderItem.GetByOrder(order.OrderId); // old order items
                    foreach (OrderItem item in lstItems)
                    {
                        bool found = false;
                        foreach (OrderItem item2 in lstOrderItems)
                        {
                            if (item.Guid == item2.Guid)
                            {
                                found = true;
                                break;
                            }
                        }

                        if (!found)
                        {
                            OrderItem.Delete(item.Guid);
                            //if (item.ProductId > 0)
                            //    Product.UpdateStockQuantity(item.ProductId);
                        }
                    }
                }
                StringBuilder builderLog = new StringBuilder();

                order.BillingFirstName = txtBillingFirstName.Text.Trim();
                order.BillingLastName = txtBillingLastName.Text.Trim();
                order.BillingEmail = txtBillingEmail.Text.Trim();
                order.BillingAddress = txtBillingAddress.Text.Trim();
                order.BillingPhone = txtBillingPhone.Text.Trim();
                if (ddBillingProvince.SelectedValue.Length == 36)
                    order.BillingProvinceGuid = new Guid(ddBillingProvince.SelectedValue);
                if (ddBillingDistrict.SelectedValue.Length == 36)
                    order.BillingDistrictGuid = new Guid(ddBillingDistrict.SelectedValue);

                if (chkSameAddress.Checked)
                {
                    order.ShippingFirstName = txtShippingFirstName.Text.Trim();
                    order.ShippingLastName = txtShippingLastName.Text.Trim();
                    order.ShippingEmail = txtShippingEmail.Text.Trim();
                    order.ShippingAddress = txtShippingAddress.Text.Trim();
                    order.ShippingPhone = txtShippingPhone.Text.Trim();
                    if (ddShippingProvince.SelectedValue.Length == 36)
                        order.ShippingProvinceGuid = new Guid(ddShippingProvince.SelectedValue);
                    if (ddShippingDistrict.SelectedValue.Length == 36)
                        order.ShippingDistrictGuid = new Guid(ddShippingDistrict.SelectedValue);
                }
                else
                {
                    order.ShippingFirstName = string.Empty;
                    order.ShippingLastName = string.Empty;
                    order.ShippingEmail = string.Empty;
                    order.ShippingAddress = string.Empty;
                    order.ShippingPhone = string.Empty;
                    order.ShippingProvinceGuid = Guid.Empty;
                    order.ShippingDistrictGuid = Guid.Empty;
                }
                if (ddlERPStatus.SelectedIndex > -1)
                {
                    int apiStatus = Convert.ToInt32(ddlERPStatus.SelectedValue);
                    if (apiStatus != order.ApiOrderstatus)
                    {
                        builderLog.Append(string.Format(ResourceHelper.GetResourceString("ProductResources", "ChangeAPIStatusLogFormatText")
                                                        , OrderHelper.GetOrderERPStatusResources(order.ApiOrderstatus)
                                                        , OrderHelper.GetOrderERPStatusResources(apiStatus)));
                        order.ApiOrderstatus = Convert.ToInt32(ddlERPStatus.SelectedValue);
                    }
                }
                order.InvoiceCompanyName = txtInvoiceCompanyName.Text.Trim();
                order.InvoiceCompanyAddress = txtInvoiceCompanyAddress.Text.Trim();
                order.InvoiceCompanyTaxCode = txtInvoiceCompanyTaxCode.Text.Trim();

                order.OrderNote = txtOrderNote.Text.Trim();

                order.ShippingMethod = -1;
                order.OrderShipping = decimal.Zero;

                decimal.TryParse(txtTotalHeight.Text, out decimal orderHeight);
                decimal.TryParse(txtTotalLength.Text, out decimal orderLength);
                decimal.TryParse(txtTotalWeight.Text, out decimal orderWeight);
                decimal.TryParse(txtTotalWidth.Text, out decimal orderWidth);

                order.TotalWeight = orderWeight;
                order.TotalHeight = orderHeight;
                order.TotalLength = orderLength;
                order.TotalWidth = orderWidth;

                if (ddlShippingMethod.SelectedIndex > -1)
                {
                    order.ShippingMethod = Convert.ToInt32(ddlShippingMethod.SelectedValue);
                    string shippingOption = "";
                    if (ddlShippingOptions.SelectedIndex > -1)
                        shippingOption = ddlShippingOptions.SelectedValue;
                    int storeId = -1;
                    if (ddStore.SelectedIndex > -1)
                        int.TryParse(ddStore.SelectedValue, out storeId);
                    if (storeId == -1 && order.StoreId > 0)
                        storeId = order.StoreId;
                    string expectedTime = "";
                    order.ShippingOption = shippingOption;
                    order.OrderShipping = ProductHelper.GetShippingPrice(
                        order.ShippingMethod,
                        order.OrderSubtotal,
                        orderWeight,
                        lstOrderItems.GetTotalProducts(),
                        ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order),
                        order.BillingDistrictGuid,
                        shippingOption,
                        storeId, StoreCacheHelper.GetStoreById(storeId),
                        ref expectedTime, orderLength, orderWidth, orderHeight);
                }

                //order.PaymentMethod = -1;
                //if (ddlPaymentMethod.SelectedIndex > -1)
                //    order.PaymentMethod = Convert.ToInt32(ddlPaymentMethod.SelectedValue);

                //order.PaymentStatus = -1;
                if (ddlPaymentStatus.SelectedIndex > -1)
                {
                    int paymentStatus = Convert.ToInt32(ddlPaymentStatus.SelectedValue);
                    if (order.PaymentStatus != paymentStatus)
                    {
                        builderLog.Append(string.Format(ResourceHelper.GetResourceString("ProductResources", "ChangePaymentStatusLogFormatText")
                                                        , OrderHelper.GetPaymentStatusResources(order.PaymentStatus)
                                                        , OrderHelper.GetPaymentStatusResources(paymentStatus)));
                        order.PaymentStatus = Convert.ToInt32(ddlPaymentStatus.SelectedValue);
                    }
                }

                order.OrderTotal = CartHelper.GetCartTotal(order.OrderSubtotal,
                    order.OrderShipping,
                    discountTotal: order.OrderDiscount,
                    discountCouponTotal: order.OrderCouponAmount,
                    rewardPointAmount: order.RedeemedRewardPointsAmount,
                    voucherAmount: order.VoucherAmount,
                    serviceFee: order.OrderServiceFee,
                    tax: order.OrderTax);
                bool sendEmail = false;

                int.TryParse(ddOrderStatus.SelectedValue, out int newStatus);
                if (order.OrderStatus != newStatus)
                    sendEmail = true;

                //OrderStatus
                int oldOrderStatus = order.OrderStatus;
                order.OrderStatus = newStatus;
                //Modify stock quantity if an order is 'Completed' or revert from 'Completed'
                OrderHelper.QuantityOrderStatusHandler(order, oldOrderStatus);
                if (oldOrderStatus != newStatus)
                {
                    builderLog.Append(string.Format(ResourceHelper.GetResourceString("ProductResources", "ChangeOrderStatusLogFormatText")
                                                    , OrderHelper.GetOrderStatusResources(oldOrderStatus)
                                                    , OrderHelper.GetOrderStatusResources(newStatus)));
                }

                //StoreID
                int oldStoreID = order.StoreId;
                if (ddStore.Visible == true && enabledStore)
                {
                    Int32.TryParse(ddStore.SelectedValue, out int ddStoreValue);
                    if (order.StoreId != ddStoreValue)
                    {
                        //Send change store email
                        OrderHelper.SendOrderStoreChangedEmail(siteSettings, order, order.StoreId, ddStoreValue);

                        order.OldStoreId = order.StoreId;
                    }
                    order.StoreId = ddStoreValue;
                }
                //Modify stock quantity if 'Completed' order change store
                OrderHelper.QuantityOrderStoreHandler(order, oldStoreID);

                //Source
                order.Source = ddlSource.SelectedValue;

                int.TryParse(ddlShippingStatus.SelectedValue, out int shippingStatus);

                if (order.ShippingStatus != shippingStatus)
                {
                    builderLog.Append(string.Format(ResourceHelper.GetResourceString("ProductResources", "ChangeShippingStatusLogFormatText")
                                                    , OrderHelper.GetShippingStatusResources(order.ShippingStatus)
                                                    , OrderHelper.GetShippingStatusResources(shippingStatus)));
                    order.ShippingStatus = shippingStatus;
                }

                if (cbStaff.Entries.Count > 0)
                {
                    int.TryParse(cbStaff.Entries[0].Value, out int staffId);
                    order.StaffId = staffId;
                }
                else if (order.StaffId == -1)
                {
                    var currentUser = SiteUtils.GetCurrentSiteUser();
                    order.StaffId = currentUser.UserId;
                }

                if (cbStaffProcess.Entries.Count > 0)
                {
                    int.TryParse(cbStaffProcess.Entries[0].Value, out int staffProcessId);
                    order.StaffProcessId = staffProcessId;
                }
                order.Save();

                //Order's subtotal and total
                if (OrderHelper.OrderItemsDifferent(lstOrderItems, lstItems))
                {
                    decimal subTotal = decimal.Zero;
                    decimal orderCost = decimal.Zero;
                    int i = 1;
                    foreach (var item in lstOrderItems)
                    {
                        Product product = null;
                        //decimal adjustmentValue = 0;
                        if (item.Guid != Guid.Empty)
                        {
                            var saveItem = new OrderItem(item.Guid)
                            {
                                Quantity = item.Quantity,
                                Price = item.Price,
                                DiscountAmount = item.DiscountAmount
                            };

                            if (item.ProductId < 0)
                                saveItem.OriginalProductCost = 0;

                            subTotal += saveItem.Quantity * saveItem.Price - saveItem.DiscountAmount;
                            orderCost += saveItem.Quantity * saveItem.OriginalProductCost;

                            saveItem.Save();
                        }
                        else
                        {
                            product = new Product(siteSettings.SiteId, item.ProductId);

                            var saveItem = new OrderItem
                            {
                                OrderId = order.OrderId,
                                ProductId = item.ProductId,
                                Quantity = item.Quantity,
                                Price = item.Price,
                                OriginalProductCost = product.OldPrice,
                                DiscountAmount = item.DiscountAmount
                            };

                            if (item.ProductId < 0)
                            {
                                saveItem.OriginalProductCost = 0;
                            }

                            subTotal += saveItem.Quantity * saveItem.Price - saveItem.DiscountAmount;
                            orderCost += saveItem.Quantity * saveItem.OriginalProductCost;

                            saveItem.Save();
                        }

                        i += 1;
                    }

                    order.OrderSubtotal = subTotal;
                    order.OrderTotal = CartHelper.GetCartTotal(order.OrderSubtotal,
                        shippingTotal: order.OrderShipping,
                        discountTotal: order.OrderDiscount,
                        discountCouponTotal: order.OrderCouponAmount,
                        rewardPointAmount: order.RedeemedRewardPointsAmount,
                        voucherAmount: order.VoucherAmount,
                        serviceFee: order.OrderServiceFee,
                        tax: order.OrderTax
                        );
                    //order.OrderTotal = subTotal;
                    //if (order.OrderId > -1)
                    //    ProductHelper.UpdateUserPointFromOrder(order, 1);

                    order.Save();

                    SelectedItems = null;
                }

                LogActivity.Write("Update order's status", order.OrderCode + (builderLog.Length > 0 ? "<br/>" + builderLog.ToString() : ""));
                if (builderLog.Length > 0)
                    new OrderLog()
                    {
                        OrderId = order.OrderId,
                        Comment = builderLog.ToString(),
                        UserEmail = siteUser.Email,
                        UserId = siteUser.UserId,
                        CreatedOn = DateTime.Now,
                        TypeName = "Update order"
                    }.Save();

                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");

                if (sendEmail)
                {
                    string orderStatusName = GetOrderStatusName(order.OrderStatus);
                    if (orderStatusName.Length > 0)
                    {
                        string billingProvinceName = string.Empty;
                        string billingDistrictName = string.Empty;
                        string shippingProvinceName = string.Empty;
                        string shippingDistrictName = string.Empty;
                        if (order.BillingProvinceGuid != Guid.Empty)
                        {
                            var province = new GeoZone(order.BillingProvinceGuid);
                            if (province != null && province.Guid != Guid.Empty)
                                billingProvinceName = province.Name;
                        }
                        if (order.BillingDistrictGuid != Guid.Empty)
                        {
                            var province = new GeoZone(order.BillingDistrictGuid);
                            if (province != null && province.Guid != Guid.Empty)
                                billingDistrictName = province.Name;
                        }
                        if (order.ShippingProvinceGuid != Guid.Empty)
                        {
                            var province = new GeoZone(order.ShippingProvinceGuid);
                            if (province != null && province.Guid != Guid.Empty)
                                shippingProvinceName = province.Name;
                        }
                        if (order.ShippingDistrictGuid != Guid.Empty)
                        {
                            var province = new GeoZone(order.ShippingDistrictGuid);
                            if (province != null && province.Guid != Guid.Empty)
                                shippingDistrictName = province.Name;
                        }

                        string toEmail = order.BillingEmail.Trim();
                        if (
                            order.ShippingEmail.Length > 0
                            && !string.Equals(toEmail, order.ShippingEmail, StringComparison.CurrentCultureIgnoreCase)
                            )
                            toEmail += "," + order.ShippingEmail;

                        //order.OrderStatus
                        lstOrderItems = OrderItem.GetByOrder(order.OrderId);
                        lstProducts = Product.GetByOrder(order.SiteId, order.OrderId);
                        string templateName = "ChangeOrder-" + orderStatusName + "-CustomerNotification";

                        if (ProductHelper.SendOrderPlacedNotification(siteSettings, order, lstProducts, lstOrderItems, templateName, billingProvinceName, billingDistrictName, shippingProvinceName, shippingDistrictName, toEmail))
                            WebTaskManager.StartOrResumeTasks();

                        //if (order.UserGuid != Guid.Empty)
                        //{
                        //    SiteUser siteUser = new SiteUser(siteSettings, order.UserGuid);
                        //    if (siteUser != null)
                        //    {
                        //        string messageTemplate = MessageTemplate.GetMessage("UserNotificationChangeOrderStatus" + orderStatusName);
                        //        if (!string.IsNullOrEmpty(messageTemplate))
                        //        {
                        //            UserNotification userNotification = new UserNotification()
                        //            {
                        //                CreateBy = "System",
                        //                CreateDateUtc = DateTime.Now,
                        //                Name = ProductResources.UserNotificationChangeOrderStatus,
                        //                Type = (int)UserNotificationType.System,
                        //                Readed = false,
                        //            };

                        //            messageTemplate = messageTemplate.Replace("{FullName}", order.BillingFirstName)
                        //                                             .Replace("{CreatedUtc}", order.CreatedUtc.ToString("dd-MM-yyyy"))
                        //                                             .Replace("{OrderStatus}", ProductHelper.GetOrderStatus(order.OrderStatus))
                        //                                             .Replace("{OrderTotal}", ProductHelper.FormatPrice(order.OrderTotal))
                        //                                             .Replace("{OrderCode}", order.OrderCode);
                        //            userNotification.Description = messageTemplate;
                        //            userNotification.UserId = siteUser.UserId;
                        //            userNotification.Save();

                        //        }
                        //    }
                        //}
                    }
                }
                if (oldOrderStatus != order.OrderStatus)
                    OrderHelper.ChangeOrderStatusEvent(siteSettings, order);
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (!WebUser.IsAdmin)
                    return;

                ContentDeleted.Create(siteSettings.SiteId, order.OrderId.ToString(), "Order", typeof(OrderDeleted).AssemblyQualifiedName, order.OrderId.ToString(), Page.User.Identity.Name);

                order.IsDeleted = true;
                order.Save();
                LogActivity.Write("Delete order", order.OrderId.ToString());

                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/OrderList.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnGetPaymentStatus_Click(object sender, EventArgs e)
        {
            if (!PaymentHelper.IsOnlinePayment(order.PaymentMethod))
                return;

            var result = string.Empty;
            var status = PaymentHelper.GetPaymentStatusOnline(order, ref result);
            if (status > 0)
            {
                order.PaymentStatus = status;
                order.Save();
            }

            if (!string.IsNullOrEmpty(result))
                message.SuccessMessage = result;

            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void BtnSendShippingment_Click(object sender, EventArgs e)
        {
            string shipmentCode = string.Empty;
            string error = string.Empty;
            bool status = ShippingHelper.SendShippingment(order, OrderItem.GetByOrder(order.OrderId), Product.GetByOrder(order.SiteId, order.OrderId), ref shipmentCode, ref error);
            if (status)
            {
                order.ShippingStatus = (int)ShippingStatus.DeliveryForDelivery;
                order.OrderStatus = (int)OrderStatus.DeliveryForDelivery;
                order.Save();
                string result = "Đã gửi qua đơn vị vận chuyển thành công";
                if (!string.IsNullOrEmpty(shipmentCode))
                    result += "với mã giao vận là " + shipmentCode;
                message.SuccessMessage = result;
            }
            else if (!string.IsNullOrEmpty(error))
                message.ErrorMessage = error;
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Page.Validate("OrderCancel");
            if (!Page.IsValid) return;
            order.CancelNote = txtOrderCancelNote.Text;
            order.OrderStatus = (int)OrderStatus.Cancelled;
            order.CancelUtc = DateTime.Now;
            order.CancelUserId = SiteUtils.GetCurrentSiteUser().UserId;

            if (!string.IsNullOrEmpty(order.DiscountIDs))
            {
                var lstDiscountIds = order.DiscountIDs.SplitOnCharAndTrim(';');
                lstOrderItems = OrderItem.GetByOrder(order.OrderId);
                foreach (var item in lstOrderItems)
                {
                    if (item.DiscountAppliedGuid.HasValue)
                    {
                        var appliedItem = new DiscountAppliedToItem(item.DiscountAppliedGuid.Value);
                        if (appliedItem != null
                            && appliedItem.Guid != Guid.Empty
                            && appliedItem.SoldQty > 0
                            //&& lstDiscountIds.Contains(appliedItem.DiscountId.ToString())
                            )
                        {
                            //var discount = new Discount(appliedItem.DiscountId);
                            //if (discount != null && discount.DiscountId > 0 && discount.DiscountType == (int)DiscountType.Deal)
                            //{ }
                            if (item.DiscountQuantity.HasValue && item.DiscountQuantity.Value > 0)
                                appliedItem.SoldQty -= item.DiscountQuantity.Value;
                            else
                                appliedItem.SoldQty -= item.Quantity;
                            if (appliedItem.SoldQty < 0)
                                appliedItem.SoldQty = 0;
                            appliedItem.Save();
                        }
                    }
                }
            }

            if (order.Save())
            {
                if (ERPHelper.EnableERP)
                {
                    string waring = string.Empty;
                    ERPHelper.CancelOrder(order, new Store(order.StoreId), ref waring);
                    if (waring.Length > 0)
                    {
                        message.WarningMessage = waring;
                    }
                }
                message.SuccessMessage = "Hủy đơn hàng thành công";
                new OrderLog()
                {
                    OrderId = order.OrderId,
                    Comment = "Hủy đơn hàng thành công",
                    UserEmail = siteUser.Email,
                    UserId = siteUser.UserId,
                    CreatedOn = DateTime.Now,
                    TypeName = "Cancel order"
                }.Save();
                WebUtils.SetupRedirect(this, Request.RawUrl);
            }
        }

        private void BtnSendERP_Click(object sender, EventArgs e)
        {
            if (order == null || order.OrderId == -1) return;
            var items = OrderItem.GetByOrder(order.OrderId);
            Store store = new Store(order.StoreId);
            if (store == null || store.StoreID == -1 || store.IsDeleted)
            {
                message.WarningMessage = "Cửa hàng đã xóa. Không thể gửi xuống ERP.";
                return;
            }

            var products = Product.GetByOrder(order.SiteId, order.OrderId);
            string result = string.Empty;
            if (ERPHelper.SendOrder(order, store, items, products, ref result))
                message.SuccessMessage = "Đã gửi thành công qua ERP";
            else
                message.WarningMessage = result;
            WebUtils.SetupRedirect(this, Request.RawUrl);
        }

       
        private void chkSameAddress_CheckedChanged(object sender, EventArgs e)
        {
            divShippingInfo.Visible = chkSameAddress.Checked;
        }

        protected void txtQuantity_TextChanged(object sender, EventArgs e)
        {
            foreach (GridDataItem item in grid.Items)
            {
                int.TryParse(((TextBox)item.FindControl("txtQuantity")).Text, out int quantity);
                if (quantity <= 0)
                {
                    //message.ErrorMessage = "Số lượng sản phẩm không hợp lệ";
                    continue;
                }

                var productID = int.Parse(item.GetDataKeyValue("ProductId").ToString());

                var lstOrderItems = SelectedItems;
                var orderItem = FindOrderItemByProductId(lstOrderItems, productID);
                orderItem.Quantity = quantity;
                SelectedItems = lstOrderItems;
            }

            grid.Rebind();
        }

        private void grid_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (grid.Items.Count <= 1)
                    return;

                switch (e.CommandName)
                {
                    case "Delete":
                        var lstOrderItems = SelectedItems;
                        var productID = Int32.Parse(e.CommandArgument.ToString());
                        var orderItem = FindOrderItemByProductId(lstOrderItems, productID);
                        lstOrderItems.Remove(orderItem);
                        SelectedItems = lstOrderItems;
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion Event

        #region Populate

        private void BindOrderProcessBar()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<div class=\"tracking-progressbar\">");

            //item new
            builder.Append("<div class=\"progress-item done\">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusNew + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-shopping-basket\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\" fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item Confirmed
            string strClass = "progress-item ";
            if (order.OrderStatus == 5)
                strClass = "progress-item active";
            if (order.OrderStatus > 5)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusConfirmed + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-check\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item PaymentInformationConfirmed
            strClass = "progress-item ";
            if (order.OrderStatus == 10)
                strClass = "progress-item active";
            if (order.OrderStatus > 10)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusPaymentInformationConfirmed + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-check-double\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item HavePackedTheGoods
            strClass = "progress-item ";
            if (order.OrderStatus == 15)
                strClass = "progress-item active";
            if (order.OrderStatus > 15)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusHavePackedTheGoods + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-archive\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item DeliveryForDelivery
            strClass = "progress-item ";
            if (order.OrderStatus == 20)
                strClass = "progress-item active";
            if (order.OrderStatus > 20)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusDeliveryForDelivery + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-shipping-fast\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item DeliveryToCustomers
            strClass = "progress-item ";
            if (order.OrderStatus == 25)
                strClass = "progress-item active";
            if (order.OrderStatus > 25)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusDeliveryToCustomers + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-cube\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item
            strClass = "progress-item ";
            if (order.OrderStatus == 99)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>" + ProductResources.OrderStatusComplete + "</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-star\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");
            //End
            builder.Append("</div>");

            litProcessBar.Text = builder.ToString();
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.OrderDetailTitle);
            heading.Text = ProductResources.OrderDetailTitle;

            litTabContent.Text = "<a aria-controls=\"tabContent\" class=\"active\" role=\"tab\" data-toggle=\"tab\" href='#tabContent'>" + ProductResources.OrderContentTab + "</a>";
            litTabLogs.Text = "<a aria-controls=\"" + tabLogs.ClientID + "\" role=\"tab\" data-toggle=\"tab\" href='#" + tabLogs.ClientID + "'>" + ProductResources.OrderLogTab + "</a>";

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.OrderDeleteMultiWarning);
        }

        private void PopulateControls()
        {
            if (order.OrderStatus != (int)OrderStatus.Cancelled)
                BindOrderProcessBar();

            OrderHelper.PopulateOrderStatus(ddOrderStatus, false);
            OrderHelper.PopulateOrderERPStatus(ddlERPStatus, false);
            ProductHelper.PopulateOrderSource(ddlSource, false);
            BindShippingMethod();
            BindPaymentMethod();
            BindProvince();
            if (enabledStore)
            {
                litOrderStore.Visible = false;
                ddStore.Visible = true;
                PopulateStoreList();
            }

            if (order == null)
            {
                SelectedItems = null;
                litOrderSubtotal.Text = ProductHelper.FormatPrice(0, true);
                litShippingFee.Text = ProductHelper.FormatPrice(0, true);
                litOrderDiscount.Text = ProductHelper.FormatPrice(0, true);
                litOrderTotal.Text = ProductHelper.FormatPrice(0, true);
                litVoucherAmount.Text = ProductHelper.FormatPrice(0, true);
                litRewardPointAmount.Text = ProductHelper.FormatPrice(0, true);
                litRewardPoint.Text = "";
                litVoucherCodes.Text = "";
                return;
            }
            SelectedItems = OrderItem.GetByOrder(order.OrderId);
            txtOrderNote.Text = order.OrderNote;
            txtBillingFirstName.Text = order.BillingFirstName;
            txtBillingLastName.Text = order.BillingLastName;
            txtBillingAddress.Text = order.BillingAddress;
            txtBillingPhone.Text = order.BillingPhone;
            txtBillingEmail.Text = order.BillingEmail;
            litBillingFirstName.Text = Server.HtmlEncode(order.BillingFirstName);
            litBillingLastName.Text = Server.HtmlEncode(order.BillingLastName);
            litBillingAddress.Text = Server.HtmlEncode(order.BillingAddress);
            litBillingPhone.Text = Server.HtmlEncode(order.BillingPhone);
            litBillingEmail.Text = Server.HtmlEncode(order.BillingEmail);
            if (!string.IsNullOrEmpty(order.ApiOrderCode))
                litERPOrderCode.Text = "Mã đơn bên ERP :" + order.ApiOrderCode;
            var item = ddBillingProvince.Items.FindByValue(order.BillingProvinceGuid.ToString());
            if (item != null)
            {
                ddBillingProvince.ClearSelection();
                item.Selected = true;
                BindDistrict(ddBillingProvince, ddBillingDistrict);

                litBillingProvince.Text = ddBillingProvince.SelectedItem.Text;
            }

            item = ddBillingDistrict.Items.FindByValue(order.BillingDistrictGuid.ToString());
            if (item != null)
            {
                ddBillingDistrict.ClearSelection();
                item.Selected = true;

                litBillingDistrict.Text = ddBillingDistrict.SelectedItem.Text;
            }

            item = ddlSource.Items.FindByValue(order.Source.ToString());
            if (item != null)
            {
                ddlSource.ClearSelection();
                item.Selected = true;
            }

            item = ddlERPStatus.Items.FindByValue(order.ApiOrderstatus.ToString());
            if (item != null)
            {
                ddlERPStatus.ClearSelection();
                item.Selected = true;
            }

            if (ERPHelper.DisabledChooseStatusERP)
                ddlERPStatus.Attributes.Add("disabled", "disabled");
            item = ddlShippingStatus.Items.FindByValue(order.ShippingStatus.ToString());
            if (item != null)
            {
                ddlShippingStatus.ClearSelection();
                item.Selected = true;
            }
            //Shipping info
            if (
                !string.IsNullOrEmpty(order.ShippingFirstName)
                || !string.IsNullOrEmpty(order.ShippingLastName)
                || !string.IsNullOrEmpty(order.ShippingAddress)
                || !string.IsNullOrEmpty(order.ShippingPhone)
                || !string.IsNullOrEmpty(order.ShippingEmail)
                || order.ShippingProvinceGuid != Guid.Empty
                || order.ShippingDistrictGuid != Guid.Empty
                )
                chkSameAddress.Checked = true;
            else
                chkSameAddress.Checked = false;

            divShippingInfo.Visible = chkSameAddress.Checked;

            txtShippingFirstName.Text = order.ShippingFirstName;
            txtShippingLastName.Text = order.ShippingLastName;
            txtShippingAddress.Text = order.ShippingAddress;
            txtShippingPhone.Text = order.ShippingPhone;
            txtShippingEmail.Text = order.ShippingEmail;

            litShippingFirstName.Text = Server.HtmlEncode(order.ShippingFirstName);
            litShippingLastName.Text = Server.HtmlEncode(order.ShippingLastName);
            litShippingAddress.Text = Server.HtmlEncode(order.ShippingAddress);
            litShippingPhone.Text = Server.HtmlEncode(order.ShippingPhone);
            litShippingEmail.Text = Server.HtmlEncode(order.ShippingEmail);

            txtInvoiceCompanyName.Text = order.InvoiceCompanyName;
            txtInvoiceCompanyAddress.Text = order.InvoiceCompanyAddress;
            txtInvoiceCompanyTaxCode.Text = order.InvoiceCompanyTaxCode;

            litInvoiceCompanyName.Text = Server.HtmlEncode(order.InvoiceCompanyName);
            litInvoiceCompanyAddress.Text = Server.HtmlEncode(order.InvoiceCompanyAddress);
            litInvoiceCompanyTaxCode.Text = Server.HtmlEncode(order.InvoiceCompanyTaxCode);

            txtTotalWeight.Text = ProductHelper.FormatPrice(order.TotalWeight);
            txtTotalLength.Text = ProductHelper.FormatPrice(order.TotalLength);
            txtTotalWidth.Text = ProductHelper.FormatPrice(order.TotalWidth);
            txtTotalHeight.Text = ProductHelper.FormatPrice(order.TotalHeight);

            item = ddShippingProvince.Items.FindByValue(order.ShippingProvinceGuid.ToString());
            if (item != null)
            {
                ddShippingProvince.ClearSelection();
                item.Selected = true;
                BindDistrict(ddShippingProvince, ddShippingDistrict);

                litShippingProvince.Text = ddShippingProvince.SelectedItem.Text;
            }

            item = ddShippingDistrict.Items.FindByValue(order.ShippingDistrictGuid.ToString());
            if (item != null)
            {
                ddShippingDistrict.ClearSelection();
                item.Selected = true;
            }
            //End shipping info

            item = ddOrderStatus.Items.FindByValue(order.OrderStatus.ToString());
            if (item != null)
            {
                ddOrderStatus.ClearSelection();
                item.Selected = true;
                litOrderStatus.Text = ddOrderStatus.SelectedItem.Text;
                litOrderStatus.ForeColor = GetForeColor(order.OrderStatus);
            }
            if (order.OrderStatus != (int)OrderStatus.Cancelled)
            {
                item = ddOrderStatus.Items.FindByValue(((int)OrderStatus.Cancelled).ToString());
                if (item != null)
                    ddOrderStatus.Items.Remove(item);
            }

            item = ddlShippingMethod.Items.FindByValue(order.ShippingMethod.ToString());
            if (item != null)
            {
                ddlShippingMethod.ClearSelection();
                item.Selected = true;
            }

            LoadShippingService(order.BillingDistrictGuid, order.OrderSubtotal, order.ShippingOption);

            item = ddlPaymentMethod.Items.FindByValue(order.PaymentMethod.ToString());
            if (item != null)
            {
                ddlPaymentMethod.ClearSelection();
                item.Selected = true;
            }
            ddlPaymentStatus.Items.Clear();
            ddlPaymentStatus.Items.Add(new ListItem() { Text = PaymentHelper.GetPaymentOnlinePendingText(order.PaymentMethod), Value = "-1" });
            ddlPaymentStatus.Items.Add(new ListItem() { Text = "Đã thanh toán", Value = "1" });
            ddlPaymentStatus.Items.Add(new ListItem() { Text = "Pending", Value = "2" });
            ddlPaymentStatus.Items.Add(new ListItem() { Text = "Chưa thanh toán", Value = "3" });
            if (PaymentHelper.IsOnlinePayment(order.PaymentMethod) && (order.PaymentStatus == -1 || order.PaymentStatus == 0))
            {
                item = ddlPaymentStatus.Items.FindByValue("-1");
                if (item != null)
                {
                    ddlPaymentStatus.ClearSelection();
                    item.Selected = true;
                }
            }
            else
            {
                item = ddlPaymentStatus.Items.FindByValue(order.PaymentStatus.ToString());
                if (item != null)
                {
                    ddlPaymentStatus.ClearSelection();
                    item.Selected = true;
                }
            }
            ddlPaymentStatus.DataBind();
            litOrderCode.Text = order.OrderCode;
            litOrderSubtotal.Text = ProductHelper.FormatPrice(order.OrderSubtotal, true);
            litShippingFee.Text = ProductHelper.FormatPrice(order.OrderShipping, true);
            litOrderDiscount.Text = ProductHelper.FormatPrice(-order.OrderDiscount, true);
            litCouponDiscount.Text = ProductHelper.FormatPrice(-order.OrderCouponAmount, true);
            litOrderTotal.Text = ProductHelper.FormatPrice(order.OrderTotal, true);
            litVoucherAmount.Text = ProductHelper.FormatPrice(-order.VoucherAmount, true);
            litOrderServiceFee.Text = ProductHelper.FormatPrice(order.OrderServiceFee, true);
            litVoucherCodes.Text = order.VoucherCodes.Replace(";", "<br/>");
            if (!string.IsNullOrEmpty(order.VoucherCodes))
            {
                StringBuilder builder = new StringBuilder();
                foreach (var code in order.VoucherCodes.Split(';'))
                {
                    builder.Append("<a href='#'>" + code + "</a><br/>");
                }
            }
            litRewardPointAmount.Text = ProductHelper.FormatPrice(-order.RedeemedRewardPointsAmount, true);
            litRewardPoint.Text = "(" + order.RedeemedRewardPoints + " point)";

            litCreatedOn.Text = DateTimeHelper.Format(order.CreatedUtc, SiteUtils.GetUserTimeZone(), Resources.ProductResources.OrderCreatedDateFormat, SiteUtils.GetUserTimeOffset());
            if (order.CompletionedUtc.HasValue)
            {
                litTimeProcessOrder.Text = DateTimeHelper.Format(order.CompletionedUtc.Value, SiteUtils.GetUserTimeZone(), Resources.ProductResources.OrderCreatedDateFormat, SiteUtils.GetUserTimeOffset()) + " --- " + Math.Truncate((order.CompletionedUtc.Value - order.CreatedUtc).TotalHours) + " tiếng";
            }
            if (order.OrderCouponAmount > 0)
                pnCouponDiscount.Visible = true;

            if (!string.IsNullOrEmpty(order.GiftDescription))
            {
                divOrderGift.Visible = true;
                litOrderGift.Text = order.GiftDescription;
            }

            if (!string.IsNullOrEmpty(order.DiscountIDs))
            {
                litDiscounts.Text = string.Empty;
                divDiscounts.Visible = true;
                var lstDiscountIds = order.DiscountIDs.SplitOnCharAndTrim(';');
                foreach (var discountId in lstDiscountIds)
                {
                    var idTmp = -1;
                    int.TryParse(discountId, out idTmp);
                    if (idTmp > 0)
                    {
                        var discount = new Discount(idTmp);
                        if (discount != null)
                            litDiscounts.Text += string.Format("<div><a href=\"{0}/Product/AdminCP/Promotions/DiscountEdit.aspx?DiscountID={1}\">{2}</a></div>", SiteRoot, discount.DiscountId, discount.Name);
                    }
                }
            }

            //Order Store
            if (enabledStore)
            {
                int storeID = order.StoreId;
                if (storeID > 0)
                {
                    litOrderStore.Text = store.Name;
                    ddStore.SelectedValue = order.StoreId.ToString();
                }

                int oldStoreID = order.OldStoreId;
                Store oldStore = new Store(oldStoreID);
                if (divOrderOldStore.Visible && oldStore != null && oldStore.StoreID > 0)
                {
                    litOrderOldStore.Text = oldStore.Name;
                }
            }

            pnlRewardPoints.Visible = RewardPointsHelper.Enable;
            pnlVouchers.Visible = VoucherHelper.Enable;
            pnlServiceFee.Visible = ProductServiceHelper.Enable;

            //Order User
            //if (autCustomers.Entries.Count > 1)
            //    autCustomers.Entries.RemoveAt(0);
            var siteUser = new SiteUser(siteSettings, order.UserGuid);
            if (siteUser != null && siteUser.UserId > 0)
            {
                var orderUser = new AutoCompleteBoxEntry(siteUser.Name, siteUser.UserGuid.ToString());
                autCustomers.Entries.Add(orderUser);
            }
            siteUser = new SiteUser(siteSettings, order.StaffId);
            if (siteUser != null && siteUser.UserId > 0)
            {
                var orderUser = new AutoCompleteBoxEntry(siteUser.Name, siteUser.UserId.ToString());
                cbStaff.Entries.Add(orderUser);
            }
            siteUser = new SiteUser(siteSettings, order.StaffProcessId);
            if (siteUser != null && siteUser.UserId > 0)
            {
                var orderUser = new AutoCompleteBoxEntry(siteUser.Name, siteUser.UserId.ToString());
                cbStaffProcess.Entries.Add(orderUser);
            }
            if (order.OrderStatus == (int)OrderStatus.Cancelled)
            {
                StringBuilder builder = new StringBuilder();
                builder.Append("<p>Lí do hủy đơn : " + order.CancelNote + "</p>");
                if (order.CancelUtc.HasValue)
                    builder.Append("<p>Ngày hủy đơn : " + order.CancelUtc.Value.ToString("dd-MM-yyyy hh:mm") + "</p>");

                if (order.CancelUserId > 0)
                {
                    SiteUser user = new SiteUser(siteSettings, order.CancelUserId);
                    if (user != null)
                        builder.Append("<p>Người hủy đơn : " + user.FirstName + " " + user.LastName + "</p>");
                }

                litOrderCancelInfo.Text = builder.ToString();
            }

            divStaff.Visible = OrderHelper.OrderStaffEnable;
            divStaffProcess.Visible = OrderHelper.OrderStaffProcessEnable;
        }

        private void PopulateStoreList()
        {
            ddStore.Items.Clear();
            ddStore.Items.Add(new ListItem(ResourceHelper.GetResourceString("StoreResources", "InventoryStoreDropdown"), string.Empty));

            //var lstStores = Store.GetPageBySearch();
            //ddStore.DataSource = lstStores.Where(s => s.Options != 0).ToList();
            //ddStore.DataBind();

            var lstStores = Store.GetAll();
            if (!WebUser.IsAdmin)
            {
                foreach (Store store in lstStores.Reverse<Store>())
                {
                    var isAdmin = false;
                    var lstAdmins = store.OrderUserIDs.Split(';');
                    foreach (string adminID in lstAdmins)
                    {
                        if (adminID == siteUser.UserId.ToString())
                            isAdmin = true;
                    }
                    if (!isAdmin)
                        lstStores.Remove(store);
                }
            }

            ddStore.DataSource = lstStores;
            ddStore.DataBind();
        }

        protected string GetUserName(string userName, string userEmail)
        {
            return string.IsNullOrEmpty(userName) ? userEmail : userName;
        }

        protected System.Drawing.Color GetForeColor(int orderStatus)
        {
            return OrderHelper.GetForeColor(orderStatus);
        }

        private string GetOrderStatusName(int orderStatus)
        {
            return OrderHelper.GetOrderStatusName(orderStatus);
        }

        private void BindShippingMethod()
        {
            var lstShippingMethods = ShippingMethod.GetByActive(siteSettings.SiteId, -1);
            if (lstShippingMethods.Count > 0)
            {
                ddlShippingMethod.DataSource = lstShippingMethods;
                ddlShippingMethod.DataBind();
            }
            else
                divShippingMethod.Visible = false;
        }

        private void BindPaymentMethod()
        {
            var lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, -1);
            if (lstPaymentMethods.Count > 0)
            {
                ddlPaymentMethod.DataSource = lstPaymentMethods;
                ddlPaymentMethod.DataBind();
            }
            else
                divPaymentMethod.Visible = false;
        }

        private void BindProvince()
        {
            ddBillingProvince.DataSource = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid);
            ddBillingProvince.DataBind();

            ddShippingProvince.DataSource = ddBillingProvince.DataSource;
            ddShippingProvince.DataBind();
        }

        private void BindDistrict(ListControl ddProvince, ListControl dd)
        {
            dd.Items.Clear();
            if (ddProvince.SelectedValue.Length == 36)
            {
                dd.DataSource = GeoZone.GetByListParent(ddProvince.SelectedValue, 1);
                dd.DataBind();
            }

            dd.Items.Insert(0, new ListItem(ProductResources.OrderSelectLabel, ""));
        }

        private string GetListGeoZoneGuid()
        {
            string listGeoZoneGuid = string.Empty;
            if (!string.IsNullOrEmpty(ddShippingProvince.SelectedValue))
            {
                listGeoZoneGuid = ddShippingProvince.SelectedValue;
                var lstDistrict = GeoZone.GetByListParent(listGeoZoneGuid, 1);
                foreach (GeoZone item in lstDistrict)
                    listGeoZoneGuid += ";" + item.Guid.ToString();
            }

            return listGeoZoneGuid;
        }

        private void ddBillingProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrict(ddBillingProvince, ddBillingDistrict);
        }

        private void ddShippingProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrict(ddShippingProvince, ddShippingDistrict);
        }

        private void DdlShippingOptions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadShippingOptions(false);
        }

        private void BtnCalShippingFee_Click(object sender, EventArgs e)
        {
            ReloadShippingOptions(false);
        }

        private void ddlShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadShippingOptions();
        }

        private void LoadShippingService(Guid billingDistrictGuid, decimal subTotal, string shippingOptionSelected = "")
        {
            int shippingMethodId = Convert.ToInt32(ddlShippingMethod.SelectedValue);
            ddlShippingOptions.Items.Clear();
            if (ShippingHelper.IsShippingServiceProvider(shippingMethodId))
            {
                string error = string.Empty;
                var options = ShippingHelper.GetShippingOptions(shippingMethodId, lstOrderItems.GetTotalWeights(lstProducts), billingDistrictGuid, subTotal, lstOrderItems.GetTotalProducts(), StoreCacheHelper.GetStoreById(order.StoreId), ref error);
                if (options != null)
                {
                    foreach (var option in options)
                        ddlShippingOptions.Items.Add(new ListItem() { Value = option.Value, Text = option.Name, Selected = (option.Value == shippingOptionSelected) });
                    ddlShippingOptions.DataBind();
                }
                if (!string.IsNullOrEmpty(error))
                {
                    if (ViettelPostHelper.ShippingMethodId == shippingMethodId)
                        message.WarningMessage = "Hệ thống kết nối với ViettelPost đang không ổn định. Xin vui lòng chọn phương thức vận chuyển khác";
                }
            }
        }

        private void DdBillingDistrict_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReloadShippingOptions();
        }

        private void ReloadShippingOptions(bool loadOptions = true)
        {
            try
            {
                var subTotal = decimal.Zero;
                var billingDistrictGuid = Guid.Empty;
                var orderDiscount = decimal.Zero;
                if (order != null)
                {
                    subTotal = order.OrderSubtotal;
                    billingDistrictGuid = order.BillingDistrictGuid;
                    orderDiscount = order.OrderDiscount;
                }
                else
                {
                    //calculte subTotal
                    foreach (GridDataItem item in grid.Items)
                    {
                        var product = new Product(siteSettings.SiteId, Int32.Parse(item.GetDataKeyValue("ProductId").ToString()));
                        int productPrice = Decimal.ToInt32(product.Price);
                        int productQuantity = Int32.Parse(item.GetDataKeyValue("Quantity").ToString());
                        subTotal += productPrice * productQuantity;
                    }
                }
                if (ddBillingDistrict.SelectedValue.Length == 36)
                    billingDistrictGuid = new Guid(ddBillingDistrict.SelectedValue);
                if (loadOptions)
                    LoadShippingService(billingDistrictGuid, subTotal);
                string shippingOption = string.Empty;
                if (ddlShippingOptions.SelectedIndex > 0)
                    shippingOption = ddlShippingOptions.SelectedValue;
                else if (ddlShippingOptions.Items.Count > 0)
                    shippingOption = ddlShippingOptions.Items[0].Value;
                int shippingMethodId = Convert.ToInt32(ddlShippingMethod.SelectedValue);

                int storeId = -1;
                if (ddStore.SelectedIndex > -1)
                    storeId = Convert.ToInt32(ddStore.SelectedValue);
                if (storeId == -1 && order.StoreId > 0)
                    storeId = order.StoreId;
                string expectedTime = "";

                decimal.TryParse(txtTotalHeight.Text, out decimal orderHeight);
                decimal.TryParse(txtTotalLength.Text, out decimal orderLength);
                decimal.TryParse(txtTotalWeight.Text, out decimal orderWeight);
                decimal.TryParse(txtTotalWidth.Text, out decimal orderWidth);

                decimal shippingPrice = ProductHelper.GetShippingPrice(
                    shippingMethodId,
                    subTotal,
                    orderWeight == 0 ? lstOrderItems.GetTotalWeights(lstProducts) : orderWeight,
                    lstOrderItems.GetTotalProducts(),
                    ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order),
                    billingDistrictGuid,
                    shippingOption,
                    storeId, StoreCacheHelper.GetStoreById(storeId),
                    ref expectedTime,
                    orderLength,
                    orderWidth,
                    orderHeight
                    );
                decimal total = CartHelper.GetCartTotal(order.OrderSubtotal, shippingPrice, discountTotal: orderDiscount,
                                    discountCouponTotal: order.OrderCouponAmount,
                                    rewardPointAmount: order.RedeemedRewardPointsAmount,
                                    voucherAmount: order.VoucherAmount,
                                    serviceFee: order.OrderServiceFee);
                litShippingFee.Text = ProductHelper.FormatPrice(shippingPrice, true);
                litOrderTotal.Text = ProductHelper.FormatPrice(total, true);
            }
            catch (Exception)
            {
            }
        }

        #endregion Populate

        #region LoadSettings

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();

            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            int orderId = WebUtils.ParseInt32FromQueryString("orderid", -1);
            if (orderId > 0)
            {
                order = new Order(orderId);

                if (order == null
                    || order.OrderId == -1
                    || order.SiteId != siteSettings.SiteId
                    || order.IsDeleted)
                    order = null;
            }

            HiddenControl();

            btnDelete.Visible = ProductPermission.CanDeleteOrders;
            litUserCancle.Text = siteUser.FirstName + " " + siteUser.LastName;
            litCancleDate.Text = DateTime.Now.ToString("dd-MM-yyyy hh:mm");
        }

        private void HiddenControl()
        {
            btnDelete.Visible = WebUser.IsAdmin;
            if (order != null && PaymentHelper.IsOnlinePayment(order.PaymentMethod))
                btnGetPaymentStatus.Visible = (order.PaymentAPICode.Length > 0);

            enabledStore = StoreHelper.EnabledStore;
            if (enabledStore)
            {
                divOrderStore.Visible = true;
                if (order.OldStoreId > 0)
                {
                    divOrderOldStore.Visible = true;
                    lblOrderStore.Visible = false;
                    lblOrderNewStore.Visible = true;
                }
                else
                {
                    divOrderOldStore.Visible = false;
                    lblOrderStore.Visible = true;
                    lblOrderNewStore.Visible = false;
                }
            }
            else
            {
                divOrderStore.Visible = false;
            }

            if (order != null
                && (order.OrderStatus == (int)OrderStatus.Completed || order.OrderStatus == (int)OrderStatus.Cancelled))
            {
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
                divTimeProcessOrder.Visible = true;
                litTimeProcessOrder.Visible = true;
                linkCancelButton.Visible = false;
            }
            if (order != null
                && order.ApiOrderstatus == (int)OrderERPStatus.Synchronized
                || order.OrderStatus == (int)OrderStatus.Cancelled)
                btnSendERP.Visible = false;

            if (order != null
                && order.OrderStatus == (int)OrderStatus.Completed
                && order.ApiOrderstatus != (int)OrderERPStatus.Synchronized)
                btnSendERP.Visible = true;

            btnSendShippingment.Visible = false;
            if (order != null
                && ShippingHelper.IsShippingServiceProvider(order.ShippingMethod)
                && (order.OrderStatus == (int)OrderStatus.New
                || order.OrderStatus == (int)OrderStatus.Confirmed
                || order.OrderStatus == (int)OrderStatus.PaymentInformationConfirmed
                || order.OrderStatus == (int)OrderStatus.HavePackedTheGoods)
                )
                btnSendShippingment.Visible = true;

            ////cbStaff.Visible = false;
            ////if (WebUser.IsAdmin)
            ////    cbStaff.Visible = true;
            //if (order != null
            //    && ShippingHelper.IsShippingServiceProvider(order.ShippingMethod)
            //    && order.ShippingStatus != (int)ShippingStatus.DeliveryForDelivery
            //    && order.OrderStatus != (int)OrderStatus.Cancelled)
            //    btnSendShippingment.Visible = true;
        }

        #endregion LoadSettings

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            gridLogs.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(gridLogs_NeedDataSource);
            grid.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(grid_ItemDataBound);
            grid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(grid_ItemCommand);

            //btnCancel.Click += new EventHandler(btnCancel_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
            btnSendERP.Click += new EventHandler(BtnSendERP_Click);
            btnCancel.Click += new EventHandler(BtnCancel_Click);
            btnCalShippingFee.Click += new EventHandler(BtnCalShippingFee_Click);
            ddBillingProvince.SelectedIndexChanged += new EventHandler(ddBillingProvince_SelectedIndexChanged);
            ddBillingDistrict.SelectedIndexChanged += new EventHandler(DdBillingDistrict_SelectedIndexChanged);
            ddShippingProvince.SelectedIndexChanged += new EventHandler(ddShippingProvince_SelectedIndexChanged);
            ddlShippingMethod.SelectedIndexChanged += new EventHandler(ddlShippingMethod_SelectedIndexChanged);
            ddlShippingOptions.SelectedIndexChanged += new EventHandler(DdlShippingOptions_SelectedIndexChanged);
            btnGetPaymentStatus.Click += new EventHandler(btnGetPaymentStatus_Click);
            chkSameAddress.CheckedChanged += chkSameAddress_CheckedChanged;

            autCustomers.EntryAdded += autCustomers_EntryAdded;
            autCustomers.EntryRemoved += autCustomers_EntryRemoved;
            autProducts.EntryAdded += autProducts_EntryAdded;
            btnSendShippingment.Click += new EventHandler(BtnSendShippingment_Click);
        }

        #endregion OnInit

        #region WebMethod

        private static bool Contains(List<string> lstTitles, List<string> lstKeywords)
        {
            foreach (var s in lstKeywords)
            {
                var iCount = lstTitles.Where(k => k.StartsWith(s)).Count();
                if (iCount == 0)
                    return false;
            }

            return true;
        }

        [WebMethod]
        public static AutoCompleteBoxData GetProductNames(object context)
        {
            var searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToAsciiIfPossible().ToLower();
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var lstProducts = new List<Product>();

            if (!string.IsNullOrEmpty(searchString))
                lstProducts = Product.GetTopAdv(top: 50, siteId: siteSettings.SiteId, keyword: searchString);

            var result = new List<AutoCompleteBoxItemData>();
            foreach (var product in lstProducts)
            {
                var childNode = new AutoCompleteBoxItemData
                {
                    Text = product.Title,
                    Value = product.ProductId.ToString()
                };
                childNode.Attributes.Add("Price", ProductHelper.FormatPrice(product.Price, true));
                result.Add(childNode);
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };

            return res;
        }

        [WebMethod]
        public static AutoCompleteBoxData GetUserNames(object context)
        {
            var searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToAsciiIfPossible().ToLower();
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var lstUsers = SiteUser.GetUserAdminSearchPage(
                                    siteSettings.SiteId,
                                    1,
                                    30,
                                    searchString,
                                    0,
                                    out _);
            var result = new List<AutoCompleteBoxItemData>();
            foreach (SiteUser siteUser in lstUsers)
            {
                var childNode = new AutoCompleteBoxItemData
                {
                    Text = siteUser.Name,
                    Value = siteUser.UserGuid.ToString()
                };
                childNode.Attributes.Add("FirstName", siteUser.FirstName);
                childNode.Attributes.Add("LastName", siteUser.LastName);
                childNode.Attributes.Add("Email", siteUser.Email);

                result.Add(childNode);
                if (result.Count >= 50)
                    break;
            }

            AutoCompleteBoxData res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };

            return res;
        }

        [WebMethod]
        public static AutoCompleteBoxData GetStaff(object context)
        {
            var searchString = ((Dictionary<string, object>)context)["Text"].ToString().ToAsciiIfPossible().ToLower();
            var siteSettings = CacheHelper.GetCurrentSiteSettings();
            var lstUsers = SiteUser.GetUserAdminSearchPage(
                                    siteSettings.SiteId,
                                    1,
                                    23600,
                                    searchString,
                                    0,
                                    out int totalPages);
            //Store store = new Store();
            var result = new List<AutoCompleteBoxItemData>();
            var role = Role.GetRoleByName(1, ProductPermission.StaffRoleName);
            if (role != null && role.RoleId > 0)
            {
                var reader = Role.GetUsersInRole(1, role.RoleId, 1, 232321, out _);

                try
                {
                    while (reader.Read())
                    {
                        string name = reader["Name"].ToString();
                        if (name.ToLower().Contains(searchString))
                        {
                            var childNode = new AutoCompleteBoxItemData
                            {
                                Text = name,
                                Value = reader["UserID"].ToString()
                            };
                            childNode.Attributes.Add("FirstName", reader["FirstName"].ToString());
                            childNode.Attributes.Add("LastName", reader["LastName"].ToString());
                            childNode.Attributes.Add("Email", reader["Email"].ToString());

                            result.Add(childNode);
                            if (result.Count >= 50)
                                break;
                        }
                    }
                }
                catch (Exception)
                {
                }
                finally
                {
                    reader.Close();
                }
            }
            AutoCompleteBoxData res = new AutoCompleteBoxData
            {
                Items = result.ToArray()
            };

            return res;
        }

        #endregion WebMethod
    }
}
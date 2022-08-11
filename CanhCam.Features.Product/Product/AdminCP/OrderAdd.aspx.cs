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
    public partial class OrderAddPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderAddPage));

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

            //if (order.CouponCode.Length > 0)
            //{
            //    var col = grid.Columns.FindByUniqueNameSafe("DiscountAmount");
            //    if (col != null)
            //        col.Visible = true;
            //}

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
                litDiscountOrder.Text = ProductHelper.FormatPrice(-(order.OrderDiscount - itemDiscount), true);
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
                }

                if (e.Item.DataSetIndex == e.Item.OwnerTableView.DataSourceCount - 1)
                {
                    litOrderSubtotal.Text = ProductHelper.FormatPrice(orderSubtotal, true);
                    //var orderTotal = (order != null) ? (orderSubtotal - order.OrderDiscount + order.OrderTax + order.OrderShipping - order.RedeemedRewardPointsAmount - order.OrderCouponAmount - order.VoucherAmount) : orderSubtotal;


                    var orderTotal = CartHelper.GetCartTotal(orderSubtotal, order.OrderShipping, 0, order.OrderDiscount, order.OrderCouponAmount, order.RedeemedRewardPointsAmount, order.VoucherAmount);
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
                order = new Order
                {
                    OrderCode = ProductHelper.GenerateOrderCode(siteSettings.SiteId),
                    SiteId = siteSettings.SiteId,

                    //order.UserGuid = siteUser.UserGuid;
                    CreatedBy = siteUser.Name
                };
                order.Save();//GetId



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
                    order.ApiOrderstatus = Convert.ToInt32(ddlERPStatus.SelectedValue);
                order.InvoiceCompanyName = txtInvoiceCompanyName.Text.Trim();
                order.InvoiceCompanyAddress = txtInvoiceCompanyAddress.Text.Trim();
                order.InvoiceCompanyTaxCode = txtInvoiceCompanyTaxCode.Text.Trim();

                order.OrderNote = txtOrderNote.Text.Trim();

                var coupon = (DiscountCoupon)null;
                var lstDiscountItems = (List<DiscountAppliedToItem>)null;
                var lstDiscountId = new List<int>();
                var giftDescription = string.Empty;
                var productGifts = string.Empty;
                var discountPayment = 0M;
                var orderItems = OrderItem.GetByOrder(order.OrderId);
                var discountTotal = orderItems.ToShoppingCartItems(order.SiteId).GetDiscountTotal(Product.GetByOrder(order.SiteId, order.OrderId),
                                        ref lstDiscountItems, ref lstDiscountId, ref giftDescription, ref productGifts, ref coupon, ref discountPayment, order.PaymentMethod);
                var discountCouponTotal = coupon != null ? coupon.DiscountAmount : 0;

                if (!string.IsNullOrEmpty(giftDescription))
                    order.GiftDescription = giftDescription;

                order.ShippingMethod = -1;
                order.OrderShipping = decimal.Zero;
                if (ddlShippingMethod.SelectedIndex > -1)
                {
                    order.ShippingMethod = Convert.ToInt32(ddlShippingMethod.SelectedValue);
                    string shippingOption = "";
                    if (ddlShippingOptions.SelectedIndex > -1)
                        shippingOption = ddlShippingOptions.SelectedValue;
                    int storeId = -1;
                    if (ddStore.SelectedIndex > -1)
                        storeId = Convert.ToInt32(ddStore.SelectedValue);
                    string expectedTime = string.Empty;
                    order.OrderShipping = ProductHelper.GetShippingPrice(
                        order.ShippingMethod,
                        order.OrderSubtotal,
                        lstOrderItems.GetTotalWeights(lstProducts),
                        lstOrderItems.GetTotalProducts(),
                        ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order),
                        order.BillingDistrictGuid, shippingOption, storeId, StoreCacheHelper.GetStoreById(storeId), ref expectedTime);
                }

                order.PaymentMethod = -1;
                if (ddlPaymentMethod.SelectedIndex > -1)
                    order.PaymentMethod = Convert.ToInt32(ddlPaymentMethod.SelectedValue);

                order.PaymentStatus = -1;
                if (ddlPaymentStatus.SelectedIndex > -1)
                    order.PaymentStatus = Convert.ToInt32(ddlPaymentStatus.SelectedValue);


                order.OrderTotal = CartHelper.GetCartTotal(order.OrderSubtotal,
                    order.OrderShipping,
                    rewardPointAmount: order.RedeemedRewardPointsAmount,
                    discountCouponTotal: order.OrderCouponAmount,
                    discountTotal: order.OrderDiscount
                    );

                bool sendEmail = false;
                if (order.OrderStatus != Convert.ToInt32(ddOrderStatus.SelectedValue))
                    sendEmail = true;

                //OrderStatus
                int oldOrderStatus = order.OrderStatus;
                order.OrderStatus = Convert.ToInt32(ddOrderStatus.SelectedValue);
                //Modify stock quantity if an order is 'Completed' or revert from 'Completed'
                OrderHelper.QuantityOrderStatusHandler(order, oldOrderStatus);

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

                order.ShippingStatus = Convert.ToInt32(ddlShippingStatus.SelectedValue);

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

                if (oldOrderStatus != order.OrderStatus && order.OrderStatus == (int)OrderStatus.Completed)
                    order.CompletionedUtc = DateTime.UtcNow;
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
                    order.OrderTotal = CartHelper.GetCartTotal(order.OrderSubtotal, shippingTotal: order.OrderShipping, discountTotal: order.OrderDiscount);
                    //order.OrderTotal = subTotal;
                    //if (order.OrderId > -1)
                    //    ProductHelper.UpdateUserPointFromOrder(order, 1);

                    order.Save();

                    SelectedItems = null;
                }

                LogActivity.Write("Update order's status", order.OrderCode);
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
                WebUtils.SetupRedirect(this, "/Product/AdminCP/OrderEdit.aspx?OrderID=" + order.OrderId);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnGetPaymentStatus_Click(object sender, EventArgs e)
        {
            var result = string.Empty;
            var status = OnePayHelper.GetPaymentStatus(order, ref result);
            if (status > 0)
            {
                order.PaymentStatus = status;
                order.Save();
            }

            if (!string.IsNullOrEmpty(result))
                message.SuccessMessage = result;

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

            //item
            builder.Append("<div class=\"progress-item done\">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>Đơn hàng đã đặt</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-shopping-basket\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\" fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item
            string strClass = "progress-item ";
            if (order.OrderStatus == 10)
                strClass = "progress-item active";
            if (order.OrderStatus > 10)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>Đã xác nhận <br/>thông tin thanh toán</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-dollar-sign\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item
            strClass = "progress-item ";
            if (order.OrderStatus == 20)
                strClass = "progress-item active";
            if (order.OrderStatus > 20)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>Đã giao cho <br/>đơn vị vận chuyển</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-truck\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item
            strClass = "progress-item ";
            if (order.OrderStatus == 20)
                strClass = "progress-item active";
            if (order.OrderStatus > 20)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>Đơn hàng đã nhận</span>");
            builder.Append("</div>");
            builder.Append("<div class=\"circle\">");
            builder.Append("<em class=\"fa fa-clipboard-check\"></em>");
            builder.Append("</div>");
            builder.Append("<div class=\"bar\">");
            builder.Append("<div class=\"fill-color\"></div>");
            builder.Append("</div>");
            builder.Append("</div>");

            //item
            strClass = "progress-item ";
            if (order.OrderStatus == 25)
                strClass = "progress-item active";
            if (order.OrderStatus == 99)
                strClass = "progress-item done";
            builder.Append("<div class=\" " + strClass + " \">");
            builder.Append("<div class=\"text\">");
            builder.Append("<span>Đơn hàng đã hoàn tất</span>");
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

        }

        private void PopulateControls()
        {
            BindOrderProcessBar();
            OrderHelper.PopulateOrderStatus(ddOrderStatus, false);
            ProductHelper.PopulateOrderSource(ddlERPStatus, false);
            BindShippingMethod();
            BindPaymentMethod();
            BindProvince();
            if (enabledStore && (orderID < 1 || WebUser.IsAdmin))
            {
                litOrderStore.Visible = false;
                ddStore.Visible = true;
                PopulateStoreList();
            }

            SelectedItems = null;
            litOrderSubtotal.Text = ProductHelper.FormatPrice(0, true);
            litShippingFee.Text = ProductHelper.FormatPrice(0, true);
            litOrderDiscount.Text = ProductHelper.FormatPrice(0, true);
            litOrderTotal.Text = ProductHelper.FormatPrice(0, true);
            litVoucherAmount.Text = ProductHelper.FormatPrice(0, true);
            litVoucherCodes.Text = "";
            chkSameAddress.Checked = false;


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


        private void ddBillingProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrict(ddBillingProvince, ddBillingDistrict);
        }

        private void ddShippingProvince_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDistrict(ddShippingProvince, ddShippingDistrict);
        }

        private void ddlShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
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

                    if (ddBillingDistrict.SelectedValue.Length == 36)
                        billingDistrictGuid = new Guid(ddBillingDistrict.SelectedValue);
                }

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
                string expectedTime = string.Empty;
                decimal shippingPrice = ProductHelper.GetShippingPrice(
                    shippingMethodId,
                    subTotal,
                    lstOrderItems.GetTotalWeights(lstProducts),
                    lstOrderItems.GetTotalProducts(),
                    ProductHelper.GetShippingGeoZoneGuidsByOrderSession(order),
                    billingDistrictGuid,
                    shippingOption,
                    storeId, StoreCacheHelper.GetStoreById(storeId), ref expectedTime
                    );
                decimal total = CartHelper.GetCartTotal(order.OrderSubtotal, shippingPrice, discountTotal: orderDiscount,
                                    discountCouponTotal: order.OrderCouponAmount,
                                    rewardPointAmount: order.RedeemedRewardPointsAmount, voucherAmount: order.VoucherAmount);
                litShippingFee.Text = ProductHelper.FormatPrice(shippingPrice, true);
                litOrderTotal.Text = ProductHelper.FormatPrice(total, true);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void LoadShippingService(Guid billingDistrictGuid, decimal subTotal)
        {
            int shippingMethodId = Convert.ToInt32(ddlShippingMethod.SelectedValue);
            ddlShippingOptions.Items.Clear();
            if (ShippingHelper.IsShippingServiceProvider(shippingMethodId))
            {
                string error = string.Empty;
                var options = ShippingHelper.GetShippingOptions(shippingMethodId, lstOrderItems.GetTotalWeights(lstProducts), billingDistrictGuid, subTotal, lstOrderItems.GetTotalProducts(), null, ref error);
                foreach (var option in options)
                    ddlShippingOptions.Items.Add(new ListItem() { Value = option.Value, Text = option.Name });
                ddlShippingOptions.DataBind();
            }
        }

        #endregion Populate

        #region LoadSettings

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();

            order = new Order();

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


            cbStaff.Visible = false;
            if (WebUser.IsAdmin)
                cbStaff.Visible = true;
        }

        #endregion LoadSettings

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            grid.ItemDataBound += new Telerik.Web.UI.GridItemEventHandler(grid_ItemDataBound);
            grid.ItemCommand += new Telerik.Web.UI.GridCommandEventHandler(grid_ItemCommand);

            //btnCancel.Click += new EventHandler(btnCancel_Click);
            btnUpdate.Click += new EventHandler(btnUpdate_Click);

            ddBillingProvince.SelectedIndexChanged += new EventHandler(ddBillingProvince_SelectedIndexChanged);
            ddShippingProvince.SelectedIndexChanged += new EventHandler(ddShippingProvince_SelectedIndexChanged);
            ddlShippingMethod.SelectedIndexChanged += new EventHandler(ddlShippingMethod_SelectedIndexChanged);

            chkSameAddress.CheckedChanged += chkSameAddress_CheckedChanged;

            autCustomers.EntryAdded += autCustomers_EntryAdded;
            autCustomers.EntryRemoved += autCustomers_EntryRemoved;
            autProducts.EntryAdded += autProducts_EntryAdded;
        }


        #endregion OnInit

        #region WebMethod


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
using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.StoreUI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public static class OrderHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderHelper));

        public static bool OrderStaffEnable = ConfigHelper.GetBoolProperty("Order:StaffEnable", false);
        public static bool OrderAutoHiddenOrderByStaffApplyed = ConfigHelper.GetBoolProperty("Order:AutoHiddenOrderByStaffApplyed", false);
        public static bool OrderStaffProcessEnable = ConfigHelper.GetBoolProperty("Order:StaffProcessEnable", false);

        public static int OrderStatusNew = ConfigHelper.GetIntProperty("Order:StatusNew", 0);
        public static int OrderStatusCancelled = ConfigHelper.GetIntProperty("Order:StatusCancelled", 95);
        public static int OrderStatusOutOfStock = ConfigHelper.GetIntProperty("Order:StatusOutOfStock", 90);
        public static int OrderStatusComplete = ConfigHelper.GetIntProperty("Order:StatusComplete", 99);

        public static void SendOrderStoreChangedEmail(SiteSettings siteSettings, Order order, int storeID, int newStoreID)
        {
            Store store = new Store(storeID);
            string storeEmails = string.Empty;
            if (store != null && store.StoreID > 0)
                storeEmails = StoreHelper.GetAdminEmails(siteSettings, store.OrderUserIDs);

            Store newStore = new Store(newStoreID);
            string newStoreEmails = string.Empty;
            if (newStore != null && newStore.StoreID > 0)
                newStoreEmails = StoreHelper.GetAdminEmails(siteSettings, newStore.OrderUserIDs);

            OrderHelper.SendOrderStoreChangedNotification(siteSettings, order, store.Name, newStore.Name, newStoreEmails, storeEmails);
        }

        public static string GetCustomer(string firstName, string lastName)
        {
            var format = ResourceHelper.GetResourceString("ProductResources", "OrderCustomerFirstLastFormat");

            format = format.Replace("{FirstName}", firstName);
            format = format.Replace("{LastName}", lastName);

            return HttpUtility.HtmlEncode(format);
        }

        public static string FormatOrders(string codes, bool appendLinks)
        {
            if (string.IsNullOrEmpty(codes))
                return string.Empty;

            StringBuilder builder = new StringBuilder();
            if (appendLinks)
            {
                foreach (var code in codes.Split(';'))
                {
                    Order order = Order.GetByCode(code);
                    if (order == null || order.OrderId <= 0)
                        continue;
                    builder.Append("<a href='/Product/AdminCP/OrderEdit.aspx?OrderID=" + order.OrderId + "'>" + order.OrderCode + "</a>");
                }
            }
            else
                foreach (var code in codes.Split(';'))
                    builder.Append("<p>" + code + "</p>");

            return builder.ToString();
        }

        #region Order Status

        public static void PopulateOrderERPStatus(DropDownList ddOrderStatus, bool addAll)
        {
            ddOrderStatus.Items.Clear();

            if (addAll)
                ddOrderStatus.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "OrderStatusAll"), "-1"));

            foreach (var item in typeof(OrderERPStatus).GetFields())
            {
                if (item.FieldType == typeof(OrderERPStatus))
                    ddOrderStatus.Items.Add(new ListItem { Text = GetOrderERPStatusResources(Convert.ToInt32(item.GetRawConstantValue().ToString())), Value = item.GetRawConstantValue().ToString() });
            }
        }

        public static void PopulateOrderStatus(DropDownList ddOrderStatus, bool addAll, bool addCancelled = true)
        {
            ddOrderStatus.Items.Clear();

            if (addAll)
                ddOrderStatus.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "OrderStatusAll"), "-1"));

            foreach (var item in typeof(OrderStatus).GetFields())
            {
                if (item.FieldType == typeof(OrderStatus))
                {
                    string val = item.GetRawConstantValue().ToString();
                    if (!addCancelled && Convert.ToInt32(val) == (int)OrderStatus.Cancelled)
                        continue;
                    ddOrderStatus.Items.Add(new ListItem { Text = GetOrderStatusResources(Convert.ToInt32(val)), Value = val });
                }
            }
        }

        public static bool IsOrderProcessing(int orderStatus)
        {
            return orderStatus == (int)OrderStatus.Confirmed
                || orderStatus == (int)OrderStatus.PaymentInformationConfirmed
                || orderStatus == (int)OrderStatus.HavePackedTheGoods
                || orderStatus == (int)OrderStatus.DeliveryForDelivery
                || orderStatus == (int)OrderStatus.DeliveryToCustomers;
        }

        public static string GetPaymentStatusResources(int paymentStatus)
        {
            switch ((PaymentStatus)paymentStatus)
            {
                case PaymentStatus.Paid:
                    return Resources.ProductResources.PaymentStatusPaid;

                case PaymentStatus.Pending:
                    return Resources.ProductResources.PaymentStatusPending;

                case PaymentStatus.NotYet:
                    return Resources.ProductResources.PaymentStatusNotYet;
            }
            return Resources.ProductResources.PaymentStatusNotYet;
        }

        public static string GetShippingStatusResources(int shippingStatus)
        {
            switch ((ShippingStatus)shippingStatus)
            {
                case ShippingStatus.DeliveryForDelivery:
                    return Resources.ProductResources.ShipingStatusDeliveryForDelivery;

                case ShippingStatus.Sent:
                    return Resources.ProductResources.ShipingStatusSent;

                case ShippingStatus.DeliveryFailed:
                    return Resources.ProductResources.OrderStatusDeliveryFailed;
            }
            return Resources.ProductResources.ShipingStatusNone;
        }

        public static string GetOrderERPStatusResources(int orderStatus)
        {
            switch ((OrderERPStatus)orderStatus)
            {
                case OrderERPStatus.None:
                    return Resources.ProductResources.StatusNone;

                case OrderERPStatus.Synchronized:
                    return Resources.ProductResources.StatusSynchronized;

                case OrderERPStatus.SyncError:
                    return Resources.ProductResources.StatusSyncError;

                case OrderERPStatus.Asynchronous:
                    return Resources.ProductResources.StatusAsynchronous;
            }

            return string.Empty;
        }

        public static string GetOrderStatusResources(int orderStatus)
        {
            switch ((OrderStatus)orderStatus)
            {
                case OrderStatus.New:
                    return Resources.ProductResources.OrderStatusNew;

                case OrderStatus.Confirmed:
                    return Resources.ProductResources.OrderStatusConfirmed;

                case OrderStatus.PaymentInformationConfirmed:
                    return Resources.ProductResources.OrderStatusPaymentInformationConfirmed;

                case OrderStatus.HavePackedTheGoods:
                    return Resources.ProductResources.OrderStatusHavePackedTheGoods;

                case OrderStatus.DeliveryForDelivery:
                    return Resources.ProductResources.OrderStatusDeliveryForDelivery;

                case OrderStatus.DeliveryToCustomers:
                    return Resources.ProductResources.OrderStatusDeliveryToCustomers;

                case OrderStatus.Completed:
                    return Resources.ProductResources.OrderStatusComplete;

                case OrderStatus.DeliveryFailed:
                    return Resources.ProductResources.OrderStatusDeliveryFailed;

                case OrderStatus.Cancelled:
                    return Resources.ProductResources.OrderStatusCancelled;
            }

            return string.Empty;
        }

        public static System.Drawing.Color GetForeColor(int orderStatus)
        {
            switch ((OrderStatus)orderStatus)
            {
                case OrderStatus.New:
                    return System.Drawing.Color.Red;

                case OrderStatus.Confirmed:
                    return System.Drawing.Color.SlateBlue;

                case OrderStatus.PaymentInformationConfirmed:
                    return System.Drawing.Color.SlateBlue;

                case OrderStatus.HavePackedTheGoods:
                    return System.Drawing.Color.DarkBlue;

                case OrderStatus.DeliveryForDelivery:
                    return System.Drawing.Color.Brown;

                case OrderStatus.DeliveryToCustomers:
                    return System.Drawing.Color.Blue;

                case OrderStatus.Completed:
                    return System.Drawing.Color.Green;

                case OrderStatus.DeliveryFailed:
                    return System.Drawing.Color.OliveDrab;

                case OrderStatus.Cancelled:
                    return System.Drawing.Color.SaddleBrown;
            }

            return System.Drawing.Color.Red;
        }

        public static string GetOrderStatusName(int orderStatus)
        {
            switch ((OrderStatus)orderStatus)
            {
                case OrderStatus.New:
                    return "New";

                case OrderStatus.Confirmed:
                    return "Confirmed";

                case OrderStatus.PaymentInformationConfirmed:
                    return "PaymentInformationConfirmed";

                case OrderStatus.HavePackedTheGoods:
                    return "HavePackedTheGoods";

                case OrderStatus.DeliveryForDelivery:
                    return "DeliveryForDelivery";

                case OrderStatus.DeliveryToCustomers:
                    return "DeliveryToCustomers";

                case OrderStatus.Completed:
                    return "Completed";

                case OrderStatus.Cancelled:
                    return "Cancelled";

                case OrderStatus.DeliveryFailed:
                    return "DeliveryFailed";
            }

            return string.Empty;
        }

        public static string GetOrderSourceName(string strSource)
        {
            int.TryParse(strSource, out int source);
            switch ((OrderSource)source)
            {
                case OrderSource.Website:
                    return "Website";

                case OrderSource.Hotline:
                    return "Hotline";

                case OrderSource.CallCenter:
                    return "Call center";
            }
            return string.Empty;
        }

        #endregion Order Status

        //using OrderList.aspx
        public static string BuildOrderProcessing(int orderStatus, DateTime createdDate, object oProcessedDate, object oCompletedDate)
        {
            if (orderStatus == OrderHelper.OrderStatusCancelled || orderStatus == OrderHelper.OrderStatusOutOfStock)
                return string.Empty;

            var results = "<div class='order-processing'>";

            var processedDate = DateTime.UtcNow;
            if (oProcessedDate != null)
                processedDate = Convert.ToDateTime(oProcessedDate);

            var value = Math.Floor((processedDate - createdDate).TotalHours);
            var cssClass = "new-processing red";
            if (value <= 24)
                cssClass = "new-processing green";
            else if (value <= 48)
                cssClass = "new-processing yellow";

            results += "<span class='" + cssClass + "'></span>";

            if (oProcessedDate != null)
            {
                var completedDate = DateTime.UtcNow;
                if (oCompletedDate != null)
                    completedDate = Convert.ToDateTime(oCompletedDate);

                value = Math.Floor((completedDate - processedDate).TotalDays);
                cssClass = "processing-completed red";
                if (value <= 5)
                    cssClass = "processing-completed green";

                results += "<span class='" + cssClass + "'></span>";
            }
            else if (oCompletedDate != null)
            {
                results = "<div class='order-processing'>";
                var completedDate = Convert.ToDateTime(oCompletedDate);
                value = Math.Floor((completedDate - createdDate).TotalDays);
                cssClass = "new-completed red";
                if (value <= 6)
                    cssClass = "new-completed green";

                results += "<span class='" + cssClass + "'></span>";
            }

            results += "</div>";

            return results;
        }

        public static bool OrderItemsDifferent(List<OrderItem> orderItems1, List<OrderItem> orderItems2)
        {
            bool different = true;
            foreach (OrderItem orderItem in orderItems1)
            {
                different = true;
                if (orderItems2.Any(x => x.ProductGuid == orderItem.ProductGuid && x.Quantity == orderItem.Quantity))
                {
                    different = false;
                    continue;
                }
            }
            return different;
        }

        public static bool SendOrderStoreChangedNotification(
            SiteSettings siteSettings,
            Order order,
            string oldStoreName = "",
            string newStoreName = "",
            string toEmail = "",
            string ccEmail = "",
            string bccEmail = "")
        {
            if (order == null || order.OrderId < 1)
                return false;
            var template = EmailTemplate.Get(siteSettings.SiteId, "OrderStoreChangedNotification", WorkingCulture.LanguageId);

            if (template == null || template.Guid == Guid.Empty)
                return false;

            string fromAlias = template.FromName;
            if (fromAlias.Length == 0)
                fromAlias = siteSettings.DefaultFromEmailAlias;

            var message = new StringBuilder();
            message.Append(template.HtmlBody);

            message.Replace("{SiteName}", siteSettings.SiteName);

            message.Replace("{BillingFirstName}", order.BillingFirstName);
            message.Replace("{BillingLastName}", order.BillingLastName);
            message.Replace("{BillingEmail}", order.BillingEmail);
            message.Replace("{BillingMobile}", order.BillingMobile);
            message.Replace("{BillingPhone}", order.BillingPhone);
            message.Replace("{BillingAddress}", order.BillingAddress);

            message.Replace("{ShippingFirstName}", order.ShippingFirstName);
            message.Replace("{ShippingLastName}", order.ShippingLastName);
            message.Replace("{ShippingMobile}", order.ShippingMobile);
            message.Replace("{ShippingEmail}", order.ShippingEmail);
            message.Replace("{ShippingPhone}", order.ShippingPhone);
            message.Replace("{ShippingAddress}", order.ShippingAddress);

            message.Replace("{InvoiceCompanyName}", order.InvoiceCompanyName);
            message.Replace("{InvoiceCompanyAddress}", order.InvoiceCompanyAddress);
            message.Replace("{InvoiceCompanyTaxCode}", order.InvoiceCompanyTaxCode);

            message.Replace("{OrderStatus}", ProductHelper.GetOrderStatus(order.OrderStatus));

            message.Replace("{OldStoreName}", oldStoreName);
            message.Replace("{NewStoreName}", newStoreName);

            string shippingMethod = string.Empty;
            if (order.ShippingMethod > 0)
            {
                ShippingMethod method = new ShippingMethod(order.ShippingMethod);
                if (method != null && method.ShippingMethodId > 0)
                    shippingMethod = method.Name;
            }
            string paymentMethod = string.Empty;
            if (order.PaymentMethod > 0)
            {
                PaymentMethod method = new PaymentMethod(order.PaymentMethod);
                if (method != null && method.PaymentMethodId > 0)
                    paymentMethod = method.Name;
            }
            message.Replace("{PaymentMethod}", paymentMethod);
            message.Replace("{ShippingMethod}", shippingMethod);

            message.Replace("{OrderUrl}", string.Empty);
            message.Replace("{OrderNumber}", order.OrderId.ToString());
            message.Replace("{OrderCode}", order.OrderCode);
            message.Replace("{OrderNote}", order.OrderNote);
            message.Replace("{CreatedOn}", DateTimeHelper.Format(Convert.ToDateTime(order.CreatedUtc), SiteUtils.GetUserTimeZone(),
                            "dd/MM/yyyy HH:mm", SiteUtils.GetUserTimeOffset()));

            message.Replace("{OrderTotal}", ProductHelper.FormatPrice(order.OrderTotal, true));
            message.Replace("{OrderSubTotal}", ProductHelper.FormatPrice(order.OrderSubtotal, true));
            message.Replace("{OrderDiscount}", ProductHelper.FormatPrice(order.OrderDiscount, true));
            message.Replace("{ShippingFee}", ProductHelper.FormatPrice(order.OrderShipping, true));
            message.Replace("{CouponCode}", order.CouponCode);

            var smtpSettings = SiteUtils.GetSmtpSettings();

            string subjectEmail = template.Subject.Replace("{SiteName}", siteSettings.SiteName).Replace("{OrderCode}",
                                  order.OrderCode).Replace("{OrderNumber}", order.OrderId.ToString());

            EmailMessageTask messageTask = new EmailMessageTask(smtpSettings)
            {
                EmailFrom = siteSettings.DefaultEmailFromAddress,
                EmailTo = toEmail + (template.ToAddresses.Length == 0 ? string.Empty : "," + template.ToAddresses),
                EmailCc = ccEmail + (template.CcAddresses.Length == 0 ? string.Empty : "," + template.CcAddresses),
                EmailBcc = bccEmail + (template.BccAddresses.Length == 0 ? string.Empty : "," + template.BccAddresses),
                EmailReplyTo = template.ReplyToAddress,
                EmailFromAlias = template.FromName,
                UseHtml = true,
                SiteGuid = siteSettings.SiteGuid,
                Subject = subjectEmail,
                HtmlBody = message.ToString()
            };
            messageTask.QueueTask();

            return true;
        }

        #region Update product total from order

        public static bool QuantityOrderStatusHandler(Order order, int oldOrderStatus)
        {
            if (order.OrderStatus == OrderHelper.OrderStatusComplete && order.OrderStatus != oldOrderStatus) // whatever -> Complete => Decrease stock quantity
            {
                ModifyStockQuantityByOrder(order);
                return true;
            }
            else if (oldOrderStatus == OrderHelper.OrderStatusComplete && order.OrderStatus != oldOrderStatus) // Complete -> whatever => Increase stock quantity
            {
                ModifyStockQuantityByOrder(order, true);
                return true;
            }

            return false;
        }

        public static bool QuantityOrderStoreHandler(Order order, int oldStoreID)
        {
            if (order.OrderStatus == OrderHelper.OrderStatusComplete && order.StoreId != oldStoreID) // If 'Completed' order change store => Decrease new store stock & Increase old store stock
            {
                ModifyStockQuantityByOrder(order, true, oldStoreID); // Increase old store stock
                ModifyStockQuantityByOrder(order, false, order.StoreId); // Decrease new store stock
                return true;
            }
            return false;
        }

        public static void ModifyStockQuantityByOrder(Order order, bool addBack = false, int storeID = -1)
        {
            bool enabledStore = StoreHelper.EnabledStore;
            bool enabledInventory = StoreHelper.EnabledInventory;
            int multiplier = addBack ? 1 : -1;

            List<OrderItem> lstOrderItems = OrderItem.GetByOrder(order.OrderId);
            foreach (OrderItem orderItem in lstOrderItems)
            {
                //Modify product total quantity (if inventory is not used)
                int productID = orderItem.ProductId;
                Product product = new Product(order.SiteId, productID);
                if (product != null && product.ProductId > 0 && !enabledInventory)
                {
                    product.StockQuantity += (orderItem.Quantity * multiplier);
                    product.Save();
                }
                //Modify product individual quantity in specific store
                if (enabledStore && enabledInventory)
                {
                    if (storeID > -1)
                    {
                        StockInventory inventory = StockInventory.GetProductInStore(productID, storeID);
                        if (inventory != null && inventory.InventoryID > 0)
                        {
                            inventory.Quantity += (orderItem.Quantity * multiplier);
                            inventory.Save(order.SiteId);
                        }
                    }
                    else
                    {
                        StockInventory inventory = StockInventory.GetProductInStore(productID, order.StoreId);
                        if (inventory != null && inventory.InventoryID > 0)
                        {
                            inventory.Quantity += (orderItem.Quantity * multiplier);
                            inventory.Save(order.SiteId);
                        }
                    }
                }
            }
        }

        #endregion Update product total from order

        #region Events

        public static void ChangeOrderStatusEvent(SiteSettings siteSettings, Order order)
        {
            SiteUser user = null;
            if (order.UserGuid != Guid.Empty)
                user = new SiteUser(siteSettings, order.UserGuid);

            //reward points
            if (RewardPointsHelper.Enable && user != null
                && RewardPointsHelper.PointsForPurchases_Awarded == order.OrderStatus)
            {
                //await _mediator.Send(new AwardRewardPointsCommand() { Order = request.Order });

                var points = RewardPointsHelper.CalculateRewardPoints(user, order.OrderTotal - order.OrderShipping);
                if (points > 0)
                {
                    //add reward points
                    RewardPointsHelper.AddRewardPointsHistory(order.SiteId, user, (int)RewardPointType.Earned, points,
                        string.Format(ProductResources.EarnedForOrder, order.OrderCode));

                    order.CalcRewardPoints += points;
                    order.UpdateCalcRewardPoints();
                }
            }

            if (RewardPointsHelper.Enable && user != null
                && RewardPointsHelper.ReduceRewardPointsAfterCancelOrder
                && order.OrderStatus == (int)OrderStatus.Cancelled)
            {
                var points = RewardPointsHelper.CalculateRewardPoints(user, order.OrderTotal - order.OrderShipping);
                if (points > 0)
                {
                    //add reward points
                    RewardPointsHelper.AddRewardPointsHistory(order.SiteId, user, (int)RewardPointType.Reduce, points,
                        string.Format(ProductResources.RedeemedForOrder, order.OrderCode));
                    if (order.CalcRewardPoints >= points)
                    {
                        order.CalcRewardPoints -= points;
                        order.UpdateCalcRewardPoints();
                    }
                }
            }

            if (ERPHelper.EnableERP)
            {
                string result = string.Empty;
                if (ERPHelper.AutoSyncWhenOrderCompleted
                && order.ApiOrderstatus != (int)OrderERPStatus.Synchronized
                && order.OrderStatus == (int)OrderStatus.Completed)
                    try
                    {
                        var items = OrderItem.GetByOrder(order.OrderId);
                        Store store = new Store(order.StoreId);
                        var products = Product.GetByOrder(order.SiteId, order.OrderId);
                        if (ERPHelper.SendOrder(order, store, items, products, ref result))
                            new OrderLog()
                            {
                                OrderId = order.OrderId,
                                Comment = ProductResources.OrderSynced + " - " + order.ApiOrderCode,
                                UserEmail = "System",
                                CreatedOn = DateTime.Now,
                                TypeName = ProductResources.SysnOrderERP
                            }.Save();
                        else
                            new OrderLog()
                            {
                                OrderId = order.OrderId,
                                Comment = ProductResources.OrderSyncFailed,
                                UserEmail = "System",
                                CreatedOn = DateTime.Now,
                                TypeName = ProductResources.SysnOrderERP
                            }.Save();
                    }
                    catch (Exception ex)
                    {
                        log.Info("Error Sysnc ERP Result:" + result + "----Error :" + ex.Message);
                    }
                if (order.OrderStatus == (int)OrderStatus.Cancelled)
                    ERPHelper.AddRewardPointsHistory(user, order, new Store(order.StoreId), -order.RedeemedRewardPoints);
            }
            if (order.OrderStatus == (int)OrderStatus.Completed
                && !order.CompletionedUtc.HasValue)
            {
                order.CompletionedUtc = DateTime.UtcNow;
                order.UpdateCompletioned();
            }
        }

        #endregion Events

        public static void SendMailAfterOrder(Order order, SiteSettings siteSettings)
        {
            var lstProductsInCart = Product.GetByOrder(order.SiteId, order.OrderId);
            var lstOrderItem = OrderItem.GetByOrder(order.OrderId);
            var billingProvinceName = string.Empty;
            var billingDistrictName = string.Empty;
            var shippingProvinceName = string.Empty;
            var shippingDistrictName = string.Empty;
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

            var toEmail = order.BillingEmail.Trim();
            if (
                order.ShippingEmail.Length > 0
                && !string.Equals(toEmail, order.ShippingEmail, StringComparison.CurrentCultureIgnoreCase)
                )
                toEmail += "," + order.ShippingEmail;
            ProductHelper.SendOrderPlacedNotification(siteSettings, order, lstProductsInCart, lstOrderItem,
                "OrderPlacedCustomerNotification",
                billingProvinceName,
                billingDistrictName,
                shippingProvinceName,
                shippingDistrictName,
                toEmail);
            if (order.StoreId > 0)
            {
                var store = StoreCacheHelper.GetStoreById(order.StoreId);
                if (store != null && !string.IsNullOrEmpty(store.Email))
                {
                    ProductHelper.SendOrderPlacedNotification(siteSettings, order, lstProductsInCart,
                        lstOrderItem, "OrderPlacedStoreNotification",
                        billingProvinceName, billingDistrictName,
                        shippingProvinceName, shippingDistrictName, store.Email);
                }
            }
        }
    }
}
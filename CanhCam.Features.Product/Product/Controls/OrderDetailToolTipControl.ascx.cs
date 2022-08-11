/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-07-01
/// Last Modified:			2014-09-10

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class OrderDetailToolTipControl : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(OrderDetailToolTipControl));
        private SiteSettings siteSettings;
        private List<Product> lstProductsInOrder = new List<Product>();

        private int orderID = -1;

        public int OrderId
        {
            get { return orderID; }
            set { orderID = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabel();
            PopulateControls();
        }

        private void PopulateLabel()
        {
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();
        }

        private void PopulateControls()
        {
            if (orderID <= 0)
                return;

            Order order = new Order(orderID);

            if (order == null || order.OrderId == -1)
                return;

            var timeOffset = SiteUtils.GetUserTimeOffset();
            var timeZone = SiteUtils.GetUserTimeZone();

            litCustomerInfo.Text = GetCustomer(order.BillingFirstName, order.BillingLastName, order.BillingAddress, order.BillingProvinceGuid, order.BillingDistrictGuid);
            litBillingEmail.Text = Server.HtmlEncode(order.BillingEmail);
            litBillingPhone.Text = Server.HtmlEncode(order.BillingPhone);
            litCreatedOn.Text = DateTimeHelper.Format(order.CreatedUtc, timeZone, "g", timeOffset);
            litPaymentMethod.Text = string.Empty;

            litConsigneeInfo.Text = GetCustomer(order.ShippingFirstName, order.ShippingLastName, order.ShippingAddress, order.ShippingProvinceGuid, order.ShippingDistrictGuid);
            litShippingEmail.Text = Server.HtmlEncode(order.ShippingEmail);
            litShippingPhone.Text = Server.HtmlEncode(order.ShippingPhone);
            litShippingMethod.Text = string.Empty;

            lstProductsInOrder = Product.GetByOrder(siteSettings.SiteId, order.OrderId);
            rptOrderItems.DataSource = OrderItem.GetByOrder(order.OrderId);
            rptOrderItems.DataBind();

            litSubTotal.Text = ProductHelper.FormatPrice(order.OrderSubtotal, true);
            litShippingFee.Text = ProductHelper.FormatPrice(order.OrderShipping, true);
            litOrderTotal.Text = ProductHelper.FormatPrice(order.OrderTotal, true);
        }

        private string GetCustomer(string firstName, string lastName, string address, Guid provinceGuid, Guid districtGuid)
        {
            string results = ProductResources.OrderCustomerFirstLastFormat.Replace("{FirstName}", Server.HtmlEncode(firstName)).Replace("{LastName}", Server.HtmlEncode(lastName));

            if (address.Length > 0)
            {
                results += "<br />" + Server.HtmlEncode(address);

                if (districtGuid != Guid.Empty)
                {
                    GeoZone province = new GeoZone(districtGuid);
                    if (province != null && province.Guid != Guid.Empty)
                        results += " " + province.Name;
                }
                if (provinceGuid != Guid.Empty)
                {
                    GeoZone province = new GeoZone(provinceGuid);
                    if (province != null && province.Guid != Guid.Empty)
                        results += " " + province.Name;
                }
            }

            return results;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Load += new EventHandler(Page_Load);
            rptOrderItems.ItemDataBound += new System.Web.UI.WebControls.RepeaterItemEventHandler(rptOrderItems_ItemDataBound);
        }

        private void rptOrderItems_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (
                e.Item.ItemType == ListItemType.Item
                || e.Item.ItemType == ListItemType.AlternatingItem
                )
            {
                Literal litProduct = (Literal)e.Item.FindControl("litProduct");
                Literal litSubTitle = (Literal)e.Item.FindControl("litSubTitle");
                int productId = ((OrderItem)e.Item.DataItem).ProductId;
                Product product = ProductHelper.GetProductFromList(lstProductsInOrder, productId);

                if (product != null)
                {
                    litProduct.Text = ProductHelper.BuildProductLink(product);
                    litSubTitle.Text = product.SubTitle;
                }
            }
        }
    }
}
using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;

namespace CanhCam.Web.ProductUI
{
    public partial class OrderPrintPage : CmsNonBasePage
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(OrderPrintPage));
        private string productRowFormat = "";
        private Order obj = null;

        #endregion Properties

        protected void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (obj == null)
            {
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/OrderList.aspx");
                return;
            }

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateLabels()
        {
            Title = "In đơn hàng";
            litOrderPrintNoteBottom.Text = ResourceHelper.GetResourceString("ProductResources", "OrderPrintNoteBottom" + siteSettings.SiteId);
            if (siteSettings.SiteId > 1)
            {
                imgLogo1.Visible = false;
                imgLogo2.Visible = true;
            }
            AddClassToBody("client-site");
        }

        private void LoadSettings()
        {
            var keyId = WebUtils.ParseInt32FromQueryString("ID", -1);
            if (keyId > 0)
            {
                obj = new Order(keyId);
                //if (obj == null || obj.InvoiceId <= 0 || obj.Type != (int)InvoiceType.ThuTien || !obj.StudentId.HasValue)
                //    obj = null;
                filename1.Text = bindfilename();
            }
            productRowFormat += "<tr>";

            productRowFormat += "<td class='tdtable'>";
            productRowFormat += "<p><strong>{0}</strong></p>";
            productRowFormat += "</td>";

            productRowFormat += "<td class='tdtable' style='text-align: right;white-space: nowrap; ' >";
            productRowFormat += "<p>{1}</p>";
            productRowFormat += "</td>";

            productRowFormat += "<td class='tdtable' style='text-align: right;white-space: nowrap; ' >";
            productRowFormat += "<p>{2}</p>";
            productRowFormat += "</td>";

            productRowFormat += "<td class='tdtable' style='text-align: right;white-space: nowrap; ' >";
            productRowFormat += "<p>{3}</p>";
            productRowFormat += "</td>";

            productRowFormat += "<td class='tdtable' style='text-align: center;white-space: nowrap;'>";
            productRowFormat += "<p>{4}</p>";
            productRowFormat += "</td>";

            productRowFormat += "<td class='tdtable' style='text-align: right;white-space: nowrap;'>";
            productRowFormat += " <p>{5}</p>";
            productRowFormat += "</td>";

            productRowFormat += "</tr>";
        }

        private string bindfilename()
        {
            return "Order" + String.Format("{0:MMddyy}", DateTime.Now);
        }

        private void PopulateControls()
        {
            if (obj != null && obj.OrderId > 0)
            {
                var lstOrderItems = OrderItem.GetByOrder(obj.OrderId);
                string htmltable = "";
                if (lstOrderItems != null && lstOrderItems.Count > 0)
                {
                    var lstProducts = Product.GetByOrder(siteSettings.SiteId, obj.OrderId);

                    foreach (var item in lstOrderItems)
                    {
                        Product product = ProductHelper.GetProductFromList(lstProducts, item.ProductId);

                        decimal unitPrice = item.UnitPrice;
                        string giabia = string.Empty;
                        giabia = ProductHelper.FormatPrice(item.Price, true);
                        if (item.UnitPrice == 0 && product.OldPrice > 0 && product.OldPrice > item.Price)
                            unitPrice = product.OldPrice;

                        if (unitPrice > 0)
                            giabia = ProductHelper.FormatPrice(unitPrice, true);
                        string giamgia = string.Empty;
                        giamgia = "0%";
                        if (unitPrice > item.Price)
                        {
                            giamgia = ProductHelper.FormatPrice(unitPrice, true);
                            if ((((unitPrice - item.Price) / product.OldPrice) * 100) > 0)
                                giamgia = ProductHelper.FormatPrice(((unitPrice - item.Price) / product.OldPrice) * 100) + "%";
                        }

                        htmltable += string.Format(productRowFormat,
                            product.Title,
                            giabia,
                            giamgia,
                            ProductHelper.FormatPrice(Convert.ToDecimal(item.Price), true),
                            item.Quantity,
                            ProductHelper.FormatPrice(Convert.ToDecimal(item.Price) * Convert.ToInt32(item.Quantity) - Convert.ToDecimal(item.DiscountAmount), true)
                            );
                    }
                }
                lbExtraServices.Text = ProductHelper.FormatPrice(obj.OrderTax, true);
                PaymentMethod payMethod = new PaymentMethod(obj.PaymentMethod);
                if (payMethod != null)
                    lbPaymentMethod.Text = payMethod.Name;
                ltrProductList.Text = htmltable;
                lbOrderCode.Text = obj.OrderCode;
                lbFistName.Text = obj.BillingFirstName;
                lbAddress.Text = obj.BillingAddress;
                if (obj.BillingDistrictGuid != Guid.Empty)
                {
                    GeoZone district = new GeoZone(obj.BillingDistrictGuid);
                    if (district != null)
                    {
                        lbAddress.Text += ", " + district.Name;
                    }
                }

                if (obj.BillingProvinceGuid != Guid.Empty)
                {
                    GeoZone province = new GeoZone(obj.BillingProvinceGuid);
                    if (province != null)
                    {
                        lbAddress.Text += ", " + province.Name;
                    }
                }
                lbPhone.Text = obj.BillingPhone;
                lbEmail.Text = obj.BillingEmail;
                lbFeeShipping.Text = ProductHelper.FormatPrice(obj.OrderShipping, true);
                lbNode.Text = obj.OrderNote;
                lbDiscount.Text = string.Format("-{0}", ProductHelper.FormatPrice(obj.OrderDiscount, true));
                lbSubTotal.Text = ProductHelper.FormatPrice(obj.OrderSubtotal, true);
                lbTotal.Text = ProductHelper.FormatPrice(obj.OrderTotal, true);
            }
        }

        #region OnInit

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
        }

        #endregion OnInit
    }
}
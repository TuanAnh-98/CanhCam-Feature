using System.Collections.Generic;

namespace CanhCam.Web.ProductUI.APIs
{
    public class OrderModel
    {
        public OrderModel()
        {
            billingAddress = new AddressModel();
            shippingAddress = new AddressModel();
            items = new List<OrderItemModel>();
            promotionNames = new List<string>();
        }
        public string order_type { get; set; }
        public int orderId { get; set; }
        public string orderCode { get; set; }
        public string storeCode { get; set; }
        public string storeName { get; set; }
        public string orderStatus { get; set; }
        public decimal orderSubtotal { get; set; }
        public decimal orderShipping { get; set; }
        public decimal orderDiscount { get; set; }
        public decimal voucherAmount { get; set; }
        public string voucherCodes { get; set; }
        public decimal couponAmount { get; set; }
        public string couponCode { get; set; }
        public decimal orderTax { get; set; }
        public decimal orderService { get; set; }
        public decimal redeemedRewardPointsAmount { get; set; }
        public int redeemedRewardPoints { get; set; }
        public decimal orderTotal { get; set; }
        public AddressModel billingAddress { get; set; }
        public AddressModel shippingAddress { get; set; }
        public string invoiceCompanyName { get; set; }
        public string invoiceCompanyAddress { get; set; }
        public string invoiceCompanyTaxCode { get; set; }
        public string paymentMethod { get; set; }
        public string paymentStatus { get; set; }
        public string shippingMethod { get; set; }
        public string orderNote { get; set; }
        public string source { get; set; }
        public List<string> promotionNames { get; set; }
        public string createDate { get; set; }

        public List<OrderItemModel> items { get; set; }
    }
}
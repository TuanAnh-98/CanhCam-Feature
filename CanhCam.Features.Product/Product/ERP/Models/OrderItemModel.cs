namespace CanhCam.Web.ProductUI.APIs
{
    public class OrderItemModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string itemNo { get; set; }
        public string barcode { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal itemDiscountAmount { get; set; }
        public decimal itemTotal { get; set; }
    }
}
namespace CanhCam.Web.ProductUI
{
    public class transSalesEntryModel
    {
        public string barcodeNo { get; set; }
        public string itemNo { get; set; }
        public decimal price { get; set; }
        public int quantity { get; set; }
        public decimal netAmount { get; set; }
        public decimal lineDiscount { get; set; }
        public decimal discountAmount { get; set; }
        public decimal vatAmount { get; set; }
        public decimal totalRoundedAmt { get; set; }
        public string variantCode { get; set; }
        public string serialNo { get; set; }
        public string promotionNo { get; set; }
        public string couponNo { get; set; }
    }
}
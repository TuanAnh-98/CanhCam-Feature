using System;

namespace CanhCam.Web.ProductUI
{
    public class VoucherModel
    {
        public string storeNo { get; set; }
        public string voucherNo { get; set; }
        public string barcode { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime endingDate { get; set; }
        public decimal amount { get; set; }
        public string voucherType { get; set; }
        public DateTime dateApplied { get; set; }
        public bool isActive { get; set; }
    }
}
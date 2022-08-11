using System;

namespace CanhCam.Web.ProductUI
{
    public class CouponModel
    {
        public string storeNo { get; set; }
        public string couponCode { get; set; }
        public string barcode { get; set; }
        public DateTime issueDate { get; set; }
        public DateTime closedDate { get; set; }
        public int status { get; set; }
        public decimal amount { get; set; }
        public int voucherType { get; set; }
        public string accountNo { get; set; }
        public bool isActive { get; set; }
    }
}
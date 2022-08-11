using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public class transactionOrderModel
    {
        public transactionOrderModel()
        {
            transSalesEntry = new List<transSalesEntryModel>();
            transPaymentEntry = new List<transPaymentEntryModel>();
            transInfocodeEntry = new List<transInfocodeEntryModel>();
        }

        public string storeNo { get; set; }
        public string posTerminalNo { get; set; }
        public int transactionNo { get; set; }
        public int transactionType { get; set; }
        public string receiptNo { get; set; }
        public string staffId { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public decimal netAmount { get; set; }
        public decimal costAmount { get; set; }
        public decimal grossAmount { get; set; }
        public decimal payment { get; set; }
        public decimal discountAmount { get; set; }
        public decimal totalDiscount { get; set; }
        public int noOfItems { get; set; }
        public decimal amountToAccount { get; set; }
        public int rounded { get; set; }
        public string comment { get; set; }
        public int sourceType { get; set; }
        public string memberAccountNo { get; set; }
        public string memberContact { get; set; }
        public string customerName { get; set; }
        public string phoneNo { get; set; }
        public string email { get; set; }
        public string postCode { get; set; }
        public string district { get; set; }
        public string ward { get; set; }
        public string street { get; set; }
        public string orderNo { get; set; }
        public string SalesStaff { get; set; }
        public decimal ShippingMoney { get; set; }
        public List<transSalesEntryModel> transSalesEntry { get; set; }
        public List<transPaymentEntryModel> transPaymentEntry { get; set; }
        public List<transInfocodeEntryModel> transInfocodeEntry { get; set; }
    }
}
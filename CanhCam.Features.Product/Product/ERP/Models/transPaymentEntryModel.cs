namespace CanhCam.Web.ProductUI
{
    public class transPaymentEntryModel
    {
        public string statementCode { get; set; }
        public string cardNo { get; set; }
        public int exchangeRate { get; set; }
        public string tenderType { get; set; }
        public decimal amountTendered { get; set; }
        public string currencyCode { get; set; }
        public decimal amountInCurrency { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string createdByStaffId { get; set; }
        public string orderNo { get; set; }
    }
}
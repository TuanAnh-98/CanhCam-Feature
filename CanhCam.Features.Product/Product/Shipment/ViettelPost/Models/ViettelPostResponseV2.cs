namespace CanhCam.Web.ProductUI
{
    public class ViettelPostResponseV2
    {
        public int status { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }

    public class ViettelPostWarehouse
    {
        public int groupaddressId { get; set; }
        public int cusId { get; set; }
        public string name { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int provinceId { get; set; }
        public int districtId { get; set; }
        public int wardsId { get; set; }
        public string merchant { get; set; }
    }
}
namespace CanhCam.Web.ProductUI
{
    public class SakukoMemberResponse
    {
        public string success { get; set; }
        public string message { get; set; }
        public MemberModel data { get; set; }
    }
    public class MemberModel
    {
        public string account_No { get; set; }
        public string status { get; set; }
        public string contactNo { get; set; }
        public string cardNo { get; set; }
        public string clubCode { get; set; }
        public string schemeCode { get; set; }
        public int mainContact { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string postCode { get; set; }
        public string email { get; set; }
    }
}
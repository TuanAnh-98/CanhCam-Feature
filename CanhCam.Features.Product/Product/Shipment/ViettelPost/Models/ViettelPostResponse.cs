namespace CanhCam.Web.ProductUI
{
    public class ViettelPostResponse
    {
        public int status { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public ViettelPostOrderResponse data { get; set; }
    }

    public class ViettelPostOrderResponse
    {
        public string ORDER_NUMBER { get; set; }
    }
}
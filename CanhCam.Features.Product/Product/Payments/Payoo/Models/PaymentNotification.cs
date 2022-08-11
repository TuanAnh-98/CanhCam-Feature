namespace CanhCam.Web.ProductUI
{
    public class PaymentNotification : PayooOrder
    {
        public string PaymentMethod { get; set; } = "";

        public string State { set; get; } = "";
    }
}
using System;

namespace CanhCam.Web.ProductUI
{
    public class ServiceInfo
    {
        public int ServiceID { get; set; }
        public string ServiceName { get; set; }
        public int ServiceFee { get; set; }
        public DateTime ExpectedDeliveryTime { get; set; }
    }
}
using System;

namespace CanhCam.Web.ProductUI
{
    public class ShippingOption
    {
        public string Value { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime ExpectedDeliveryTime { get; set; }
    }
}
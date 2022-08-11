using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public class MoMoRequestPurchaseUnits
    {
        public List<MoMoRequestOrderItem> items { get; set; }
    }

    public class MoMoRequestOrderItem
    {
        public string name { get; set; }
        public string sku { get; set; }
        public string price { get; set; }
        public string currency { get; set; }
        public string quantity { get; set; }
    }
}
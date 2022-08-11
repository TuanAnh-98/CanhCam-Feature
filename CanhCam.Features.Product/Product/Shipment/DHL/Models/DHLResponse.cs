using System.Collections.Generic;

namespace CanhCam.Shipping.DHL.Model
{
    public class DHLResponse
    {
        public DHLResponsebd bd { get; set; }
        public DHLResponsehdr hdr { get; set; }
    }

    public class DHLResponsebd
    {
        public List<object> labels { get; set; }
        public DHLResponseStatus responseStatus { get; set; }
    }

    public class DHLResponseStatus
    {
        public string code { get; set; }
        public string message { get; set; }
        public string messageDetails { get; set; }
    }

    public class DHLResponsehdr
    {
        public string messageType { get; set; }
        public string messageDateTime { get; set; }
        public string messageVersion { get; set; }
    }
}
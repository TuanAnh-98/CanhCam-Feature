using System.Collections.Generic;

namespace CanhCam.Web.ProductUI.APIs
{
    public class JYSKResponsive
    {
        public string jsonrpc { get; set; }
        public string id { get; set; }
        public JYSKResponsive2 result { get; set; }
    }
    public class JYSKResponsive2
    {
        public string status { get; set; }
        public JYSKResponsive3 data { get; set; }
    }
    public class JYSKResponsive3
    {
        public int count { get; set; }
        public List<JYSKResponsive4> data { get; set; }
    }
    public class JYSKResponsive4
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}
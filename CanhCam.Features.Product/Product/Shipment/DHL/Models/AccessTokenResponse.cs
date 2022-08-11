namespace CanhCam.Shipping.DHL.Model
{
    public class AccessTokenResponse
    {
        public string token { get; set; }
        public string token_type { get; set; }
        public string expires_in_seconds { get; set; }
        public string client_id { get; set; }
        public AccessTokenStatusResponse responseStatus { get; set; }
    }

    public class AccessTokenStatusResponse
    {
        public string code { get; set; }
        public string message { get; set; }
        public string messageDetails { get; set; }
    }
}
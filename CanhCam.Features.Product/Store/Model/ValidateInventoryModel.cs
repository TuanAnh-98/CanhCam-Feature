using System;

namespace CanhCam.Business
{
    public class ValidateInventoryModel
    {

        public int ProductId { get; set; }
        public Guid ProductGuid { get; set; }
        public Guid CartItemGuid { get; set; }
        public string ProductTitle { get; set; }
        public int CartQuantity { get; set; }
        public int StockQuantityAvailability { get; set; }
        public bool IsValid { get; set; }
    }
}
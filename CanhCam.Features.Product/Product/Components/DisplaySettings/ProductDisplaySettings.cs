/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// This control doesn't render anything, it is used only as a themeable collection of settings for things we would like to be able to configure from theme.skin
    /// </summary>
    public class ProductDisplaySettings : WebControl
    {
        public bool ShowAttribute { get; set; } = true;

        public bool ShowRelatedProduct { get; set; } = false;

        public bool ShowNextPreviousLink { get; set; } = false;

        public string ResizeBackgroundColor { get; set; } = "#FFFFFF";

        public bool ShowComments { get; set; } = false;
        public bool CaculatePercentageRating { get; set; } = false;

        public string CommentDateFormat { get; set; } = "(dd/MM/yyyy)";

        public string CommentResourceFile { get; set; } = "ProductResources";

        public bool CommentUsingPlaceholder { get; set; } = false;

        public bool ShowTags { get; set; } = false;

        public bool ShowProductCode { get; set; } = false;

        public bool ShowPrice { get; set; } = true;

        public bool ShowOldPrice { get; set; } = false;

        public bool ShowSubTitle { get; set; } = false;

        public bool ShowVideo { get; set; } = false;

        public bool ShowAttachment { get; set; } = false;

        public bool ShowStockQuantity { get; set; } = false;

        public int CartPageId { get; set; } = -1;

        public int CheckoutPageId { get; set; } = -1;

        public int ComparePageId { get; set; } = -1;

        public int ThumbnailWidth { get; set; } = 320;

        public int ThumbnailHeight { get; set; } = 100000;

        public bool EnableColorSwitcher { get; set; } = false;

        public bool ShowMultipleZones { get; set; } = false;

        public bool ShowApiProductID { get; set; } = false;

        public bool ShowRatingStats { get; set; } = false;

        protected override void Render(HtmlTextWriter writer)
        {
            if (HttpContext.Current == null)
            {
                writer.Write("[" + this.ID + "]");
                return;
            }

            // nothing to render
        }

        public bool ShowRelatedNews { get; set; } = false;

    }
}
using CanhCam.Web.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    /// <summary>
    /// Encapsulates the feature instance configuration loaded from module settings into a more friendly object
    /// </summary>
    public partial class ProductConfiguration
    {
        public ProductConfiguration()
        { }

        public ProductConfiguration(Hashtable settings)
        {
            LoadSettings(settings);
        }

        private void LoadSettings(Hashtable settings)
        {
            if (settings == null || settings.Count == 0) { return; throw new ArgumentException("must pass in a hashtable of settings"); }

            if (settings.Contains("ProductsPerPageSetting"))
            {
                string pageSizeOptionsString = settings["ProductsPerPageSetting"].ToString().Trim();
                pageSizeOptions = pageSizeOptionsString.SplitOnCharAndTrim(';');

                if (pageSizeOptions.Count > 0)
                    int.TryParse(pageSizeOptions[0], out pageSize);
            }

            showLeftContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageLeftContentSetting", showLeftContent);
            showRightContent = WebUtils.ParseBoolFromHashtable(settings, "ShowPageRightContentSetting", showRightContent);
            showAllProducts = WebUtils.ParseBoolFromHashtable(settings, "ShowAllProductsFromChildZoneSetting", showAllProducts);

            relatedProductsToShow = WebUtils.ParseInt32FromHashtable(settings, "RelatedProductsToShow", relatedProductsToShow);
            otherProductsPerPage = WebUtils.ParseInt32FromHashtable(settings, "OtherProductsPerPageSetting", otherProductsPerPage);

            if (settings["XsltFileName"] != null)
                xsltFileName = settings["XsltFileName"].ToString();
            if (settings["XsltFileNameDetailPage"] != null)
                xsltFileNameDetailPage = settings["XsltFileNameDetailPage"].ToString();

            if (settings["LoadFirstProductSetting"] != null)
                loadFirstProduct = WebUtils.ParseBoolFromHashtable(settings, "LoadFirstProductSetting", loadFirstProduct);

            if (settings["ParentZonesSetting"] != null)
                zoneIds = settings["ParentZonesSetting"].ToString().Trim();

            showCustomFieldsInProductList = WebUtils.ParseBoolFromHashtable(settings, "ShowCustomFieldsInProductList", showCustomFieldsInProductList);
            showHiddenContents = WebUtils.ParseBoolFromHashtable(settings, "ShowHiddenContentsOnDetailPage", showHiddenContents);
            showAllImagesInProductList = WebUtils.ParseBoolFromHashtable(settings, "ShowAllImagesInProductList", showAllImagesInProductList);
            hideOtherContentsOnDetailPage = WebUtils.ParseBoolFromHashtable(settings, "HideOtherContentsOnDetailPage", hideOtherContentsOnDetailPage);
            enableKeywordFiltering = WebUtils.ParseBoolFromHashtable(settings, "EnableKeywordFiltering", enableKeywordFiltering);
            hidePaginationOnDetailPage = WebUtils.ParseBoolFromHashtable(settings, "HidePaginationOnDetailPage", hidePaginationOnDetailPage);
        }

        private bool hidePaginationOnDetailPage = true;

        public bool HidePaginationOnDetailPage => hidePaginationOnDetailPage;

        private bool enableKeywordFiltering = false;

        public bool EnableKeywordFiltering => enableKeywordFiltering;

        private bool showHiddenContents = false;

        public bool ShowHiddenContents => showHiddenContents;

        private bool hideOtherContentsOnDetailPage = false;

        public bool HideOtherContentsOnDetailPage => hideOtherContentsOnDetailPage;

        private bool showCustomFieldsInProductList = false;

        public bool ShowCustomFieldsInProductList => showCustomFieldsInProductList;

        private string xsltFileName = "ProductList.xslt";

        public string XsltFileName => xsltFileName;

        private string xsltFileNameDetailPage = "ProductDetail.xslt";

        public string XsltFileNameDetailPage => xsltFileNameDetailPage;

        private bool loadFirstProduct = false;

        public bool LoadFirstProduct => loadFirstProduct;

        private int relatedProductsToShow = 12;

        public int RelatedProductsToShow => relatedProductsToShow;

        private int otherProductsPerPage = 9;

        public int OtherProductsPerPage => otherProductsPerPage;

        #region Comment System

        private bool notifyOnComment = false;

        public bool NotifyOnComment => notifyOnComment;

        //private bool allowComments = false;
        //public bool AllowComments
        //{
        //    get { return allowComments; }
        //}

        public static bool UseLegacyCommentSystem => ConfigHelper.GetBoolProperty("Product:UseLegacyCommentSystem", true);

        private bool allowCommentTitle = true;

        public bool AllowCommentTitle => allowCommentTitle;

        private bool useCaptcha = false;

        public bool UseCaptcha => useCaptcha;

        private bool requireAuthenticationForComments = false;

        public bool RequireAuthenticationForComments => requireAuthenticationForComments;

        private string commentSystem = "internal";

        public string CommentSystem => commentSystem;

        private string zoneIds = string.Empty;

        public string ZoneIds => zoneIds;

        #endregion Comment System

        private string notifyEmail = string.Empty;

        public string NotifyEmail => notifyEmail;

        private bool enableContentVersioning = false;

        public bool EnableContentVersioning => enableContentVersioning;

        private bool showAllProducts = false;

        public bool ShowAllProducts => showAllProducts;

        private bool showLeftContent = true;

        public bool ShowLeftContent => showLeftContent;

        private bool showRightContent = true;

        public bool ShowRightContent => showRightContent;

        private int pageSize = 12;

        public int PageSize => pageSize;

        private bool showAllImagesInProductList = false;

        public bool ShowAllImagesInProductList => showAllImagesInProductList;

        private List<string> pageSizeOptions = new List<string>();

        public List<string> PageSizeOptions => pageSizeOptions;

        public static string BingMapDistanceUnit => ConfigHelper.GetStringProperty("Product:BingMapDistanceUnit", "VERouteDistanceUnit.Mile");

        /// <summary>
        /// If true and the skin is using altcontent1 it will load the page content for that in the news detail view
        /// </summary>
        public static bool ShowTopContent => ConfigHelper.GetBoolProperty("Product:ShowTopContent", true);

        /// <summary>
        /// If true and the skin is using altcontent2 it will load the page content for that in the news detail view
        /// </summary>
        public static bool ShowBottomContent => ConfigHelper.GetBoolProperty("Product:ShowBottomContent", true);

        /// <summary>
        /// 165 is the max recommended by google
        /// </summary>
        public static int MetaDescriptionMaxLengthToGenerate => ConfigHelper.GetIntProperty("Product:MetaDescriptionMaxLengthToGenerate", 165);

        public static bool UseNoIndexFollowMetaOnLists => ConfigHelper.GetBoolProperty("Product:UseNoIndexFollowMetaOnLists", true);

        public static bool UseHtmlDiff => ConfigHelper.GetBoolProperty("Product:UseHtmlDiff", true);

        public static bool UseImages => ConfigHelper.GetBoolProperty("Product:UseImages", true);

        public static bool EnableShoppingCart => ConfigHelper.GetBoolProperty("Product:EnableShoppingCart", true);

        public static bool EnableWishlist => ConfigHelper.GetBoolProperty("Product:EnableWishlist", false);

        public static int MaximumShoppingCartItems => ConfigHelper.GetIntProperty("Product:MaximumShoppingCartItems", 20);

        public static int MaximumWishlistItems => ConfigHelper.GetIntProperty("Product:MaximumWishlistItems", 20);

        public static int OrderMaximumQuantity => ConfigHelper.GetIntProperty("Product:OrderMaximumQuantity", 1000);

        public static string AllowedQuantities => ConfigHelper.GetStringProperty("Product:AllowedQuantities", string.Empty);

        public static bool DisplayCartAfterAddingProduct => ConfigHelper.GetBoolProperty("Product:DisplayCartAfterAddingProduct", false);

        public static bool DisplayWishlistAfterAddingProduct => ConfigHelper.GetBoolProperty("Product:DisplayWishlistAfterAddingProduct", false);

        public static bool AllowCartItemEditing => ConfigHelper.GetBoolProperty("Product:AllowCartItemEditing", true);

        public static bool MiniShoppingCartEnabled => ConfigHelper.GetBoolProperty("Product:MiniShoppingCartEnabled", true);

        public static bool ShowProductImagesInMiniShoppingCart => ConfigHelper.GetBoolProperty("Product:ShowProductImagesInMiniShoppingCart", true);

        public static bool FilterProductByTopLevelParentZones => ConfigHelper.GetBoolProperty("Product:FilterProductByTopLevelParentZones", false);

        public static bool RelatedProductsTwoWayRelationship => ConfigHelper.GetBoolProperty("Product:RelatedProductsTwoWayRelationship", false);

        public static string WorkingCurrency => ConfigHelper.GetStringProperty("Product:WorkingCurrency", "₫");

        public static string CurrencyFormatting => ConfigHelper.GetStringProperty("Product:CurrencyFormatting", "#,##0");

        public static int CartPageId => ConfigHelper.GetIntProperty("Product:CartPageId", 1);

        public static string WishlistPageUrl => ConfigHelper.GetStringProperty("Product:WishlistPageUrl", "/wishlist");
        public static bool EnableSaveDiscountPayment => ConfigHelper.GetBoolProperty("Discount:EnableSaveDiscountPayment", true);
        public static bool ValidateInventoryEnable => ConfigHelper.GetBoolProperty("Shopping:ValidateInventoryEnable", false);

        //public static bool OnePageCheckoutEnabled
        //{
        //    get { return ConfigHelper.GetBoolProperty("Product:OnePageCheckoutEnabled", false); }
        //}
        public static bool AnonymousCheckoutAllowed => ConfigHelper.GetBoolProperty("Product:AnonymousCheckoutAllowed", true);

        public static string OrderCodeDateFormat => ConfigHelper.GetStringProperty("Product:OrderCodeDateFormat", "yyMMdd");

        public static int OrderCodeMinimumLength => ConfigHelper.GetIntProperty("Product:OrderCodeMinimumLength", 3);

        public static string OrderAddressRequiredFields => ConfigHelper.GetStringProperty("Product:OrderAddressRequiredFields", "Address_FirstName|Address_Address|Address_Phone");

        public static bool EnableComparing => ConfigHelper.GetBoolProperty("Product:EnableComparing", false);

        public static int MaximumCompareItems => ConfigHelper.GetIntProperty("Product:MaximumCompareItems", 3);

        public static bool ShowCustomFieldInDetailsPage => ConfigHelper.GetBoolProperty("Product:ShowCustomFieldInDetailsPage", true);

        public static bool ShowCustomFieldInCatalogPages => ConfigHelper.GetBoolProperty("Product:ShowCustomFieldInCatalogPages", false);

        public static bool EnableShoppingCartAttributes => ConfigHelper.GetBoolProperty("Product:EnableShoppingCartAttributes", false);

        public static bool EnableAttributesPriceAdjustment => ConfigHelper.GetBoolProperty("Product:EnableAttributesPriceAdjustment", false);

        public static bool RecentlyViewedProductsEnabled => ConfigHelper.GetBoolProperty("Product:RecentlyViewedProductsEnabled", false);

        public static int RecentlyViewedProductCount => ConfigHelper.GetIntProperty("Product:RecentlyViewedProductCount", 6);

        public static string ProductDetailPageMessages => ConfigHelper.GetStringProperty("Product:DetailPageMessages", string.Empty);
        public static string ShoppingPageMessages => ConfigHelper.GetStringProperty("Product:ShoppingPageMessages", string.Empty);

        public static bool EnableProductSoldOutLetter => ConfigHelper.GetBoolProperty("Product:EnableProductSoldOutLetter", false);

        public static bool EnableProductZone => ConfigHelper.GetBoolProperty("Product:EnableProductZone", false);

        public static bool AutoUpdateCommentStats => ConfigHelper.GetBoolProperty("Product:AutoUpdateCommentStats", false);
        public static bool EnableProductJsonSchema => ConfigHelper.GetBoolProperty("Product:EnableProductJsonSchema", false);
    }
}
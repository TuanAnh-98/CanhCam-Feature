using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public static class PromotionsHelper
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PromotionsHelper));

        public static string DisableDiscountTypes = ConfigHelper.GetStringProperty("Promotion:DisableDiscountTypes", "2;4;16;32");

        #region Discount

        public static void PopulateDiscountType(DropDownList ddType, bool addAll)
        {
            ddType.Items.Clear();

            if (addAll)
                ddType.Items.Add(new ListItem(ResourceHelper.GetResourceString("ProductResources", "DiscountTypeAll"), "-10"));

            var lstDisable = DisableDiscountTypes.Split(';');
            foreach (var item in typeof(DiscountType).GetFields())
            {
                if (item.FieldType == typeof(DiscountType) && !lstDisable.Contains(item.GetRawConstantValue().ToString()))
                    ddType.Items.Add(new ListItem { Text = GetDiscountTypeResources(Convert.ToInt32(item.GetRawConstantValue().ToString())), Value = item.GetRawConstantValue().ToString() });
            }
        }

        public static string GetDiscountTypeResources(int type)
        {
            switch ((DiscountType)type)
            {
                case DiscountType.ByCatalog:
                    return Resources.ProductResources.DiscountTypeByCatalog;

                case DiscountType.ByOrderRange:
                    return Resources.ProductResources.DiscountTypeByOrderRange;

                case DiscountType.ByProductRange:
                    return Resources.ProductResources.DiscountTypeByProductRange;

                case DiscountType.Coupon:
                    return Resources.ProductResources.DiscountTypeCoupon;

                case DiscountType.ComboSale:
                    return Resources.ProductResources.DiscountTypeComboSale;

                case DiscountType.PaymentMethod:
                    return Resources.ProductResources.DiscountTypePaymentMethod;

                case DiscountType.Deal:
                    return Resources.ProductResources.DiscountTypeDeal;
            }

            return string.Empty;
        }

        public static string GetDiscountAmountMessage(decimal discount, decimal originalPrice)
        {
            string result = string.Empty;
            if (discount > 0)
            {
                if (discount > 0)
                    result = "-" + ProductHelper.FormatPrice(discount, true)
                             + "<span class='percent'> (" + CalculatePercentage(discount, originalPrice) + "%)</span>";
            }

            return result;
        }

        public static int CalculatePercentage(decimal discountAmount, decimal originalPrice)
        {
            if (originalPrice == 0)
                return 0;

            int percent = (int)Math.Round(discountAmount / originalPrice * 100);

            if (percent <= 0)
                return 0;
            if (percent >= 100)
                return 100;

            return percent;
        }

        public static string GetDiscountGiffMessage(int siteId, string giftProducts, string giftCustomProducts, bool isCoupon)
        {
            string result = string.Empty;
            if (giftProducts.Length > 0
                || giftCustomProducts.Length > 0)
            {
                result += giftCustomProducts;

                if (isCoupon && giftProducts.Length > 0)
                {
                    var lstGiftProducts = Product.GetPageAdv(pageNumber: 1, pageSize: 10, siteId: siteId, productIds: giftProducts);
                    if (lstGiftProducts.Count > 0)
                        foreach (Product productTmp in lstGiftProducts)
                        {
                            productTmp.OpenInNewWindow = true;
                            if (result.Length > 0)
                                result += "<br />";
                            result += ProductHelper.BuildProductLink(productTmp);
                        }
                }

                if (result.Length > 0)
                    result = string.Format("{0}", result);
            }

            return result;
        }

        public static List<int> GetProductIDsByGiftProducts(string giftProducts)
        {
            List<int> result = new List<int>();
            if (giftProducts.Length > 0)
            {
                giftProducts
                .SplitOnCharAndTrim(';')
                .ForEach(qtyStr =>
                {
                    if (int.TryParse(qtyStr.Trim(), out int qty))
                        result.Add(qty);
                });
            }

            return result;
        }

        #region ComboSale rules Xml Parse

        public static string AddComboSaleRules(string comboSaleRulesXml, int productId, int quantity, bool usePercentage,
                                               decimal discountAmount)
        {
            string result = string.Empty;
            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                if (string.IsNullOrEmpty(comboSaleRulesXml))
                {
                    var element1 = xmlDoc.CreateElement("Products");
                    xmlDoc.AppendChild(element1);
                }
                else
                    xmlDoc.LoadXml(comboSaleRulesXml);

                var rootElement = (XmlElement)xmlDoc.SelectSingleNode(@"//Products");

                XmlElement productElement = null;
                //find existing
                var nodeList1 = xmlDoc.SelectNodes(@"//Products/Product");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id))
                        {
                            if (id == productId)
                            {
                                productElement = (XmlElement)node1;
                                break;
                            }
                        }
                    }
                }

                //create new one if not found
                if (productElement == null)
                {
                    productElement = xmlDoc.CreateElement("Product");
                    productElement.SetAttribute("ID", productId.ToString());
                    rootElement.AppendChild(productElement);
                }

                var quantityElement = xmlDoc.CreateElement("Qty");
                quantityElement.InnerText = quantity.ToString();
                productElement.AppendChild(quantityElement);

                var discountAmountElement = xmlDoc.CreateElement("DiscountAmount");
                discountAmountElement.InnerText = Convert.ToDouble(discountAmount).ToString();
                productElement.AppendChild(discountAmountElement);

                var usePercentageElement = xmlDoc.CreateElement("UsePercentage");
                usePercentageElement.InnerText = usePercentage.ToString().ToLower();
                productElement.AppendChild(usePercentageElement);

                result = xmlDoc.OuterXml;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return result;
        }

        public static List<Product> ParseComboSaleProducts(int siteId, string comboSaleRulesXml)
        {
            var result = new List<Product>();
            var ids = ParseComboSaleProductIds(comboSaleRulesXml);
            string productIds = string.Join(";", ids.ToArray());

            if (productIds.Length > 0)
                result = Product.GetPageAdv(pageNumber: 1, pageSize: 100, siteId: siteId, productIds: productIds);

            return result;
        }

        public static List<int> ParseComboSaleProductIds(string comboSaleRulesXml)
        {
            var ids = new List<int>();
            if (String.IsNullOrEmpty(comboSaleRulesXml))
                return ids;

            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                xmlDoc.LoadXml(comboSaleRulesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Products/Product");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id) && !ids.Contains(id))
                            ids.Add(id);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return ids;
        }

        public static List<DiscountAppliedToItem> ParseComboSaleValues(string comboSaleRulesXml)
        {
            var selectedValues = new List<DiscountAppliedToItem>();
            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                xmlDoc.XmlResolver = null;
                xmlDoc.LoadXml(comboSaleRulesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Products/Product");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id))
                        {
                            var item = new DiscountAppliedToItem
                            {
                                ItemId = id
                            };

                            var node2 = node1.SelectSingleNode(@"Qty");
                            if (node2 != null)
                                item.ComboSaleQty = Convert.ToInt32(node2.InnerText.Trim());
                            node2 = node1.SelectSingleNode(@"DiscountAmount");
                            if (node2 != null)
                                item.DiscountAmount = Convert.ToDecimal(node2.InnerText.Trim());
                            node2 = node1.SelectSingleNode(@"UsePercentage");
                            if (node2 != null)
                                item.UsePercentage = Convert.ToBoolean(node2.InnerText.Trim());

                            selectedValues.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return selectedValues;
        }

        public static DiscountAppliedToItem ParseComboSaleValue(string comboSaleRulesXml, int productId)
        {
            try
            {
                var xmlDoc = new XmlDocument
                {
                    XmlResolver = null
                };
                xmlDoc.XmlResolver = null;
                xmlDoc.LoadXml(comboSaleRulesXml);

                var nodeList1 = xmlDoc.SelectNodes(@"//Products/Product");
                foreach (XmlNode node1 in nodeList1)
                {
                    if (node1.Attributes != null && node1.Attributes["ID"] != null)
                    {
                        string str1 = node1.Attributes["ID"].InnerText.Trim();
                        if (int.TryParse(str1, out int id))
                        {
                            if (id == productId)
                            {
                                var item = new DiscountAppliedToItem
                                {
                                    ItemId = id
                                };

                                var node2 = node1.SelectSingleNode(@"Qty");
                                if (node2 != null)
                                    item.ComboSaleQty = Convert.ToInt32(node2.InnerText.Trim());
                                node2 = node1.SelectSingleNode(@"DiscountAmount");
                                if (node2 != null)
                                    item.DiscountAmount = Convert.ToDecimal(node2.InnerText.Trim());
                                node2 = node1.SelectSingleNode(@"UsePercentage");
                                if (node2 != null)
                                    item.UsePercentage = Convert.ToBoolean(node2.InnerText.Trim());

                                return item;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return null;
        }

        #endregion ComboSale rules Xml Parse

        #region ComboSale

        public static decimal GetPriceComboSale(List<DiscountAppliedToItem> lstComboItems, List<ShoppingCartItem> cartList,
                                                int productId, decimal price, int quantity)
        {
            DiscountAppliedToItem discountApplied = null;
            return GetPriceComboSale(lstComboItems, cartList, productId, price, quantity, ref discountApplied);
        }

        public static decimal GetPriceComboSale(List<DiscountAppliedToItem> lstComboItems, List<ShoppingCartItem> cartList,
                                                int productId, decimal price, int quantity, ref DiscountAppliedToItem discountApplied)
        {
            discountApplied = null;
            decimal productPrice = price;

            foreach (DiscountAppliedToItem comboItem1 in lstComboItems)
            {
                var dicComboProduct = PromotionsHelper.ParseComboSaleValues(comboItem1.ComboSaleRules);
                dicComboProduct.Add(comboItem1);

                var comboItem2 = dicComboProduct.Where(x => x.ComboSaleQty <= quantity && x.ItemId == productId).FirstOrDefault();
                if (comboItem2 != null)
                {
                    dicComboProduct.Remove(comboItem2);

                    bool found = true;
                    dicComboProduct.ForEach(comboItem3 =>
                    {
                        var iCount = cartList.Where(cartItem => cartItem.ProductId == comboItem3.ItemId
                                                    && comboItem3.ComboSaleQty <= cartItem.Quantity).Count();
                        if (iCount == 0)
                            found = false;
                    });

                    if (found)
                    {
                        discountApplied = comboItem2;
                        discountApplied.DiscountId = comboItem1.DiscountId;

                        if (comboItem1.ItemId == productId)
                            discountApplied.GiftCustomProducts = comboItem1.GiftCustomProducts;
                        else
                            discountApplied.GiftCustomProducts = comboItem1.GiftProducts;

                        break;
                    }
                }
            }

            if (discountApplied != null)
            {
                if (discountApplied.UsePercentage)
                {
                    productPrice = (productPrice - productPrice * discountApplied.DiscountAmount * (decimal)0.01);
                    discountApplied.DiscountPercentage = (int)Math.Round(discountApplied.DiscountAmount);
                    discountApplied.DiscountCaculator = (discountApplied.DiscountAmount * price) / 100;
                }
                else
                {
                    productPrice = productPrice - discountApplied.DiscountAmount;
                    if (productPrice > 0)
                    {
                        discountApplied.DiscountPercentage = (int)Math.Round(discountApplied.DiscountAmount / price * 100);
                        discountApplied.DiscountCaculator = discountApplied.DiscountAmount;
                    }
                }
            }

            return productPrice;
        }

        #endregion ComboSale

        public static decimal GetItemDiscountTotal(DiscountAppliedToItem discountApplied, int quantity)
        {
            return discountApplied.DiscountCaculator * GetItemDiscountQuantity(discountApplied, quantity);
        }

        public static int GetItemDiscountQuantity(DiscountAppliedToItem discountApplied, int quantity)
        {
            if (discountApplied.DealQty > 0
                && discountApplied.AppliedType != (int)DiscountType.ComboSale
                && discountApplied.AppliedType == (int)DiscountAppliedType.ToProducts)
            {
                int leftQty = discountApplied.DealQty - discountApplied.SoldQty;
                if (leftQty <= 0)
                    return 0;

                int minAllowedQuantity = Math.Min(leftQty, quantity);
                if (discountApplied.DiscountQtyStep > 0)
                    return Math.Min(minAllowedQuantity, discountApplied.DiscountQtyStep);

                return minAllowedQuantity;
            }

            if (
                (discountApplied.DiscountQtyStep <= 0)
                || (discountApplied.DiscountQtyStep > 0 && discountApplied.DiscountQtyStep >= quantity)
            )
                return quantity;

            return discountApplied.DiscountQtyStep;
        }

        public static Dictionary<int, int> ParseProductByGifts(string productGifts)
        {
            var dic = new Dictionary<int, int>();
            try
            {
                var lstParts = productGifts.SplitOnCharAndTrim('+');
                foreach (var part in lstParts)
                {
                    var lstParts2 = part.SplitOnCharAndTrim('x');
                    if (lstParts2.Count == 2)
                    {
                        var productId = -1;
                        var quality = 0;
                        int.TryParse(lstParts2[0], out productId);
                        int.TryParse(lstParts2[1], out quality);
                        if (productId > 0)
                        {
                            if (dic.ContainsKey(productId))
                                dic[productId] += quality;
                            else
                                dic.Add(productId, quality);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
            return dic;
        }

        public static bool CheckOrderHaveDiscountPayment(Order order, List<OrderItem> orderItems, List<Product> products, int paymentMethodId, ref decimal paymentDiscount)
        {
            if (order == null || orderItems == null || orderItems.Count == 0 || products.Count == 0)
                return false;
            var lstPaymentItems = new List<DiscountAppliedToItem>();
            List<DiscountAppliedToItem> lstDiscountItems = DiscountAppliedToItem.GetActive(order.SiteId, (int)DiscountType.PaymentMethod, products);
            log.Info(lstDiscountItems.Count);
            try
            {
                foreach (var sci in orderItems)
                {
                    var product = ProductHelper.GetProductFromList(products, sci.ProductId);
                    if (product != null)
                    {
                        foreach (var item in lstDiscountItems)
                        {
                            if (CartHelper.IsDiscountAppliedItemValid(item, product))
                            {
                                if (item.DiscountType == (int)DiscountType.PaymentMethod)
                                {
                                    if (paymentMethodId > 0 && (";" + item.AppliedForPaymentIDs + ";").Contains(";" + paymentMethodId.ToString() + ";"))
                                    {
                                        if (item.ItemId == -1)
                                            if (!ProductHelper.ContainsDiscount(lstPaymentItems, item.DiscountId))
                                                lstPaymentItems.Add(item);
                                    }
                                }
                            }
                        }
                    }
                }
                if (lstPaymentItems.Count > 0)
                {
                    if (order.OrderDiscount > 0 || !string.IsNullOrEmpty(order.GiftDescription))
                    {
                        if ((lstPaymentItems[0].DiscountShareType & (int)DiscountShareType.AlwaysUse) > 0)
                            paymentDiscount = CartHelper.GetDiscountAmount(order.OrderSubtotal - order.OrderDiscount, lstPaymentItems[0].DiscountAmountParent, lstPaymentItems[0].MaximumDiscountParent, lstPaymentItems[0].UsePercentageParent);
                    }
                    else
                        paymentDiscount = CartHelper.GetDiscountAmount(order.OrderSubtotal, lstPaymentItems[0].DiscountAmountParent, lstPaymentItems[0].MaximumDiscountParent, lstPaymentItems[0].UsePercentageParent);

                    return true;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return false;
        }

        #endregion Discount

        #region Images

        public static string ImagePath(int siteId, int id)
        {
            return "/Data/Sites/" + siteId.ToString() + "/Promotion/" + id.ToString() + "/";
        }

        public static void DeleteFolder(int siteId, int discountId)
        {
            var virtualPath = ImagePath(siteId, discountId);
            var fileSystemPath = HostingEnvironment.MapPath(virtualPath);

            try
            {
                DeleteDirectory(fileSystemPath);
            }
            catch (Exception)
            {
                try
                {
                    System.Threading.Thread.Sleep(0);
                    Directory.Delete(fileSystemPath, false);
                }
                catch (Exception)
                {
                }

                //log.Error(ex);
            }
        }

        public static void DeleteImage(string virtualPath)
        {
            try
            {
                var path = HttpContext.Current.Server.MapPath(virtualPath);
                if (File.Exists(path))
                    File.Delete(path);
            }
            catch (Exception ex)
            {
                //log.Error(ex);
            }
        }

        private static void DeleteDirectory(string fileSystemPath)
        {
            if (Directory.Exists(fileSystemPath))
            {
                string[] files = Directory.GetFiles(fileSystemPath);
                string[] dirs = Directory.GetDirectories(fileSystemPath);

                foreach (string file in files)
                {
                    File.SetAttributes(file, FileAttributes.Normal);
                    File.Delete(file);
                }

                while (Directory.GetFiles(fileSystemPath).Length > 0)
                    System.Threading.Thread.Sleep(10);

                foreach (string dir in dirs)
                    DeleteDirectory(dir);

                Directory.Delete(fileSystemPath, true);
            }
        }

        public static bool VerifyPromotionFolders(string virtualRoot)
        {
            var result = false;

            var originalPath = HttpContext.Current.Server.MapPath(virtualRoot);

            try
            {
                if (!Directory.Exists(originalPath))
                    Directory.CreateDirectory(originalPath);

                result = true;
            }
            catch (UnauthorizedAccessException ex)
            {
                log.Error("Error creating directories in ProductHelper.VerifyProductFolders", ex);
            }

            return result;
        }

        public static string FormatPromotionUrl(string url, int discountId)
        {
            if (url.Length > 0)
            {
                if (url.StartsWith("http"))
                {
                    string siteRoot = WebUtils.GetSiteRoot();
                    return url.Replace("http://~", siteRoot).Replace("https://~", siteRoot.Replace("http:", "https"));
                }

                return SiteUtils.GetNavigationSiteRoot() + url.Replace("~", string.Empty);
            }

            return SiteUtils.GetNavigationSiteRoot() + "/product/promotion.aspx?promotionid=" + discountId.ToInvariantString();
        }

        #endregion Images
    }
}
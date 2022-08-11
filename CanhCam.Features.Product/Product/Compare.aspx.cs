/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-15
/// Last Modified:			2015-08-15

using CanhCam.Business;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class CompareProductsPage : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(CompareProductsPage));

        #region OnInit

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            this.EnsureChildControls();
            base.OnInit(e);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            SecurityHelper.DisableBrowserCache();

            Control c = Master.FindControl("Breadcrumbs");
            if (c != null)
            {
                BreadcrumbsControl crumbs = (BreadcrumbsControl)c;
                crumbs.ForceShowBreadcrumbs = true;
                crumbs.AddedCrumbs
                    = crumbs.ItemWrapperTop + "<a href='" + SiteRoot + "/product/compare.aspx"
                    + "' class='selectedcrumb'>" + ProductResources.ProductCompareTitle
                    + "</a>" + crumbs.ItemWrapperBottom;
            }

            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.ProductCompareTitle);

            if (!Page.IsPostBack)
            {
                if ((Request.UrlReferrer != null) && (hdnReturnUrl.Value.Length == 0))
                {
                    hdnReturnUrl.Value = Request.UrlReferrer.ToString();
                }
            }
        }

        protected void btnClearCompareProductsList_Click(object sender, EventArgs e)
        {
            ProductHelper.ClearCompareProducts();
            SetupRedirect();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            SetupRedirect();
        }

        private void SetupRedirect()
        {
            string returnUrl = CartHelper.LastContinueShoppingPage;
            if (!String.IsNullOrEmpty(returnUrl))
                WebUtils.SetupRedirect(this, returnUrl);
            else
                SiteUtils.RedirectToHomepage();
        }

        protected override void CreateChildControls()
        {
            if (!base.ChildControlsCreated)
            {
                this.GenerateCompareTable();
                base.CreateChildControls();
                base.ChildControlsCreated = true;
            }
        }

        private void btnRemoveFromList_Command(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "Remove")
            {
                ProductHelper.RemoveProductFromCompareList(Convert.ToInt32(e.CommandArgument));
                WebUtils.SetupRedirect(this, "/product/compare.aspx");
            }
        }

        private HtmlTableCell AddCell(HtmlTableRow row, string text)
        {
            var cell = new HtmlTableCell
            {
                InnerHtml = text
            };
            row.Cells.Add(cell);
            return cell;
        }

        protected void GenerateCompareTable()
        {
            this.tblCompareProducts.Rows.Clear();
            this.tblCompareProducts.Width = "100%";
            var compareProducts = ProductHelper.GetCompareProducts();
            if (compareProducts.Count > 0)
            {
                int languageId = WorkingCulture.LanguageId;

                var headerRow = new HtmlTableRow();
                this.AddCell(headerRow, "&nbsp;");
                var productNameRow = new HtmlTableRow();
                this.AddCell(productNameRow, "&nbsp;");
                //var priceRow = new HtmlTableRow();
                //var cell = new HtmlTableCell();
                //cell.InnerText = ProductResources.ProductComparePrice;
                //cell.Align = "left";
                //priceRow.Cells.Add(cell);
                var productDescritionRow = new HtmlTableRow();
                this.AddCell(productDescritionRow, ProductResources.ProductCompareDescription);

                var productIds = new List<int>();
                foreach (var product in compareProducts)
                {
                    if (!productIds.Contains(product.ProductId))
                        productIds.Add(product.ProductId);
                }

                var productProperties = new List<ProductProperty>();
                var customFields = new List<CustomField>();
                var customFieldIds = new List<int>();
                if (productIds.Count > 0)
                {
                    productProperties = ProductProperty.GetPropertiesByProducts(productIds, languageId);
                    foreach (var property in productProperties)
                    {
                        if (!customFieldIds.Contains(property.CustomFieldId))
                            customFieldIds.Add(property.CustomFieldId);
                    }

                    customFields = CustomField.GetActiveByFields(siteSettings.SiteId, Product.FeatureGuid, customFieldIds, languageId);
                    if (customFields.Count > 0)
                        customFields = customFields.Where(s => (s.Options & (int)CustomFieldOptions.EnableComparing) > 0).ToList();
                }

                foreach (var product in compareProducts)
                {
                    var headerCell = new HtmlTableCell();
                    var headerCellDiv = new HtmlGenericControl("div");

                    //var productImagePanel = new HtmlGenericControl("p");
                    //productImagePanel.Attributes.Add("align", "center");

                    var btnRemoveFromList = new Button
                    {
                        ToolTip = ProductResources.ProductCompareRemoveFromList,
                        Text = ProductResources.ProductCompareRemoveFromList,
                        CommandName = "Remove"
                    };
                    btnRemoveFromList.Command += new CommandEventHandler(this.btnRemoveFromList_Command);
                    btnRemoveFromList.CommandArgument = product.ProductId.ToString();
                    btnRemoveFromList.CausesValidation = false;
                    btnRemoveFromList.CssClass = "remove-button";
                    btnRemoveFromList.ID = "btnRemoveFromList" + product.ProductId.ToString();
                    headerCellDiv.Controls.Add(btnRemoveFromList);
                    headerCellDiv.Controls.Add(new HtmlGenericControl("br"));

                    if (product.ImageFile.Length > 0)
                    {
                        var productImage = new HtmlImage
                        {
                            Border = 0,
                            Alt = "Product image"
                        };

                        string imageFolderPath = ProductHelper.MediaFolderPath(product.SiteId, product.ProductId);
                        productImage.Src = Page.ResolveUrl(imageFolderPath + product.ImageFile);

                        var productImageLink = new HyperLink
                        {
                            Text = Server.HtmlEncode(product.Title),
                            NavigateUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId)
                        };
                        productImageLink.Attributes.Add("class", "product-image");
                        productImageLink.Controls.Add(productImage);
                        headerCellDiv.Controls.Add(productImageLink);
                    }

                    headerCell.Controls.Add(headerCellDiv);
                    headerRow.Cells.Add(headerCell);
                    headerRow.Attributes.Add("class", "product-header");
                    this.tblCompareProducts.Rows.Add(headerRow);

                    var productNameCell = new HtmlTableCell();
                    var productLink = new HyperLink
                    {
                        Text = Server.HtmlEncode(product.Title),
                        NavigateUrl = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId)
                    };
                    productLink.Attributes.Add("class", "link");
                    productNameCell.Align = "center";
                    productNameCell.Controls.Add(productLink);
                    productNameRow.Cells.Add(productNameCell);

                    //var priceCell = new HtmlTableCell();
                    //priceCell.Align = "center";
                    //var productVariantCollection = manufacturer.ProductVariants;
                    //if (productVariantCollection.Count > 0)
                    //{
                    //    var productVariant = productVariantCollection[0];
                    //    decimal taxRate = decimal.Zero;
                    //    decimal finalPriceWithoutDiscountBase = this.TaxService.GetPrice(productVariant, PriceHelper.GetFinalPrice(productVariant, false), out taxRate);
                    //    decimal finalPriceWithoutDiscount = this.CurrencyService.ConvertCurrency(finalPriceWithoutDiscountBase, this.CurrencyService.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                    //    priceCell.InnerText = PriceHelper.FormatPrice(finalPriceWithoutDiscount);
                    //}
                    //priceRow.Cells.Add(priceCell);

                    var productDescriptionCell = new HtmlTableCell
                    {
                        //productDescriptionCell.Align = "left";
                        InnerHtml = product.FullContent
                    };
                    productDescritionRow.Cells.Add(productDescriptionCell);
                }
                productNameRow.Attributes.Add("class", "product-name");
                //priceRow.Attributes.Add("class", "productPrice");
                this.tblCompareProducts.Rows.Add(productNameRow);
                //this.tblCompareProducts.Rows.Add(priceRow);

                foreach (CustomField field in customFields)
                {
                    var productRow = new HtmlTableRow();
                    this.AddCell(productRow, Server.HtmlEncode(field.Name)).Align = "left";

                    foreach (var product2 in compareProducts)
                    {
                        var productCell = new HtmlTableCell();
                        foreach (var psa2 in productProperties)
                        {
                            if (psa2.ProductId == product2.ProductId
                                && field.CustomFieldId == psa2.CustomFieldId)
                            {
                                if (productCell.InnerHtml.Length > 0)
                                    productCell.InnerHtml += "<br />";

                                if (psa2.CustomFieldOptionId > 0)
                                    productCell.InnerHtml += (!string.IsNullOrEmpty(psa2.OptionName)) ? Server.HtmlEncode(psa2.OptionName) : "&nbsp;";
                                else
                                    productCell.InnerHtml = psa2.CustomValue;
                            }
                        }

                        productCell.Align = "center";
                        productCell.VAlign = "top";
                        productRow.Cells.Add(productCell);
                    }

                    this.tblCompareProducts.Rows.Add(productRow);
                }

                productDescritionRow.Attributes.Add("class", "product-description");
                this.tblCompareProducts.Rows.Add(productDescritionRow);

                string width = Math.Round((decimal)(75M / compareProducts.Count), 0).ToString() + "%";
                for (int i = 0; i < this.tblCompareProducts.Rows.Count; i++)
                {
                    var row = this.tblCompareProducts.Rows[i];
                    for (int j = 1; j < row.Cells.Count; j++)
                    {
                        //if (j == (row.Cells.Count - 1))
                        //{
                        row.Cells[j].Style.Add("width", width);
                        row.Cells[j].Style.Add("text-align", "center");
                        //}
                        //else
                        //{
                        //    row.Cells[j].Style.Add("width", width);
                        //    row.Cells[j].Style.Add("text-align", "center");
                        //}
                    }
                }
            }
            else
            {
                btnClearCompareProductsList.Visible = false;
                tblCompareProducts.Visible = false;
            }
        }
    }
}
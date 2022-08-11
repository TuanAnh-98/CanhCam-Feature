/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-08-11
/// Last Modified:		    2015-08-11

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Editor;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Resources;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class DiscountEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountEditPage));
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;
        private string imageFolderPath;
        private int discountID = -1;
        protected Discount discount = null;

        private List<DiscountRange> lstDiscountRange = new List<DiscountRange>();

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnUpdateAndNew.Click += new EventHandler(btnUpdateAndNew_Click);
            this.btnUpdateAndClose.Click += new EventHandler(btnUpdateAndClose_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnInsertAndNew.Click += new EventHandler(btnInsertAndNew_Click);
            this.btnInsertAndClose.Click += new EventHandler(btnInsertAndClose_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
            this.btnOrderRangeNewRow.Click += new EventHandler(btnOrderRangeNewRow_Click);

            grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            gridRelated.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(gridRelated_NeedDataSource);
            gridRelated.ItemDataBound += new GridItemEventHandler(gridRelated_ItemDataBound);
            grid.ItemCommand += new GridCommandEventHandler(grid_ItemCommand);

            grid2.NeedDataSource += new GridNeedDataSourceEventHandler(grid2_NeedDataSource);
            gridRelated2.NeedDataSource += new GridNeedDataSourceEventHandler(gridRelated2_NeedDataSource);
            gridRelated2.ItemDataBound += new GridItemEventHandler(gridRelated2_ItemDataBound);

            btnSearch.Click += new EventHandler(btnSearch_Click);
            btnSearch2.Click += new EventHandler(btnSearch2_Click);
            btnAdd.Click += new EventHandler(btnAdd_Click);
            btnAdd2.Click += new EventHandler(btnAdd2_Click);
            btnRemove.Click += new EventHandler(btnRemove_Click);
            btnRemove2.Click += new EventHandler(btnRemove2_Click);
            btnAll.Click += btnAll_Click;
            //btnAll2.Click += btnAll2_Click;

            gridOrder.NeedDataSource += new GridNeedDataSourceEventHandler(gridOrder_NeedDataSource);
            gridOrder.DataBound += new EventHandler(gridOrder_DataBound);

            gridOrderDiscountRange.NeedDataSource += gridOrderDiscountRange_NeedDataSource;
            gridOrderDiscountRange.ItemDataBound += gridOrderDiscountRange_ItemDataBound;
            gridOrderDiscountRange.ItemCommand += gridOrderDiscountRange_ItemCommand;

            //this.ddlAppliedAllProducts.SelectedIndexChanged += ddlAppliedAllProducts_SelectedIndexChanged;

            btnDeleteAttribute.Click += new EventHandler(btnDeleteAttribute_Click);
            btnAttributeUpdate.Click += new EventHandler(btnAttributeUpdate_Click);
            lbAttribute.SelectedIndexChanged += new EventHandler(lbAttribute_SelectedIndexChanged);
            btnAttributeUp.Click += new EventHandler(btnUpDown_Click);
            btnAttributeDown.Click += new EventHandler(btnUpDown_Click);

            SiteUtils.SetupEditor(edBriefContent, AllowSkinOverride, this);
            SiteUtils.SetupEditor(edFullContent, AllowSkinOverride, this);
            SiteUtils.SetupEditor(edAttributeContent, AllowSkinOverride, this);

            gridCoupon.NeedDataSource += gridCoupon_NeedDataSource;
            btnInsertCoupon.Click += btnInsertCoupon_Click;
            btnDeleteCoupon.Click += btnDeleteCoupon_Click;
            btnUpdateCoupon.Click += BtnUpdateCoupon_Click;


            ddlCouponNewType.SelectedIndexChanged += ddlCouponNewType_SelectedIndexChanged;
            btnCouponGenerate.Click += btnCouponGenerate_Click;
            btnProductRelatedKeyword.Click += btnProductRelatedKeyword_Click;
            btnProductRelatedExport.Click += btnProductRelatedExport_Click;
            btnProductRelatedImport.Click += btnProductRelatedImport_Click;
        }

        private void btnProductRelatedImport_Click(object sender, EventArgs e)
        {
            int i = 1;

            try
            {
                if (fupProductRelated.UploadedFiles.Count == 0)
                {
                    message.ErrorMessage = "Vui lòng chọn file excel";
                    return;
                }
                var workbook = new HSSFWorkbook(fupProductRelated.UploadedFiles[0].InputStream, true);
                var worksheet = workbook.GetSheetAt(0);
                if (worksheet == null)
                {
                    message.ErrorMessage = "File excel không hợp lệ";
                    return;
                }

                var appliedType = (int)DiscountAppliedType.ToProducts;
                var yes = false;

                var productCodeColNo = -1;
                var discountColNo = -1;
                var maximumColNo = -1;
                var dealQtyColNo = -1;
                var soldQtyColNo = -1;
                var comboSaleQtyColNo = -1;
                var fromDateColNo = -1;
                var toDateColNo = -1;
                var displayOrderColNo = -1;

                var titleRowNumber = 0;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = ShippingMethodEditPage.GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case productCodeColName:
                            productCodeColNo = columNumber;
                            break;
                        case discountColName:
                            discountColNo = columNumber;
                            break;
                        case maximumColName:
                            maximumColNo = columNumber;
                            break;
                        case dealQtyColName:
                            dealQtyColNo = columNumber;
                            break;
                        case soldQtyColName:
                            soldQtyColNo = columNumber;
                            break;
                        case comboSaleQtyColName:
                            comboSaleQtyColNo = columNumber;
                            break;
                        case fromDateColName:
                            fromDateColNo = columNumber;
                            break;
                        case toDateColName:
                            toDateColNo = columNumber;
                            break;
                        case displayOrderColName:
                            displayOrderColNo = columNumber;
                            break;
                    }
                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));

                var dt = DateTime.Now;

                var sortByItem = ConfigHelper.GetBoolProperty("PromotionSortByItem", false);

                for (i = worksheet.LastRowNum; i >= 1; i--)
                {
                    var dataRow = worksheet.GetRow(i);

                    if (dataRow != null)
                    {
                        var strProductCode = string.Empty;
                        var strDiscountAmmount = string.Empty;
                        var strMaximumDiscount = string.Empty;
                        var strDealQty = string.Empty;
                        var strSoldQty = string.Empty;
                        var strComboSaleQty = string.Empty;
                        var strFromDate = string.Empty;
                        var strToDate = string.Empty;
                        var strDisplayOrder = string.Empty;

                        if (productCodeColNo > -1)
                            strProductCode = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(productCodeColNo)).Trim();
                        if (discountColNo > -1)
                            strDiscountAmmount = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(discountColNo)).Trim();
                        if (maximumColNo > -1)
                            strMaximumDiscount = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(maximumColNo)).Trim();

                        if (discount.DiscountType == (int)DiscountType.Deal)
                        {
                            if (dealQtyColNo > -1)
                                strDealQty = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(dealQtyColNo)).Trim();
                            if (soldQtyColNo > -1)
                                strSoldQty = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(soldQtyColNo)).Trim();
                            if (comboSaleQtyColNo > -1)
                                strComboSaleQty = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(comboSaleQtyColNo)).Trim();
                            if (fromDateColNo > -1)
                                strFromDate = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(fromDateColNo)).Trim();
                            if (toDateColNo > -1)
                                strToDate = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(toDateColNo)).Trim();
                        }

                        if (displayOrderColNo > -1)
                            strDisplayOrder = ShippingMethodEditPage.GetValueFromExcel(dataRow.GetCell(displayOrderColNo)).Trim();

                        if (string.IsNullOrEmpty(strProductCode)) continue;
                        if (string.IsNullOrEmpty(strDiscountAmmount)) continue;
                        if (string.IsNullOrEmpty(strMaximumDiscount)) continue;

                        var product = Product.GetByCode(siteSettings.SiteId, strProductCode);
                        if (product == null || product.ProductId <= 0) continue;

                        var usePercentage = false;
                        if (strDiscountAmmount.Contains("%"))
                        {
                            usePercentage = true;
                            strDiscountAmmount = strDiscountAmmount.Replace("%", string.Empty);
                        }

                        var discountAmmount = decimal.Zero;
                        var maximumDiscount = decimal.Zero;
                        var displayOrder = 0;
                        decimal.TryParse(strDiscountAmmount, out discountAmmount);
                        decimal.TryParse(strMaximumDiscount, out maximumDiscount);
                        if (sortByItem && !string.IsNullOrEmpty(strDisplayOrder))
                            int.TryParse(strDisplayOrder, out displayOrder);

                        //DiscountAppliedToItem.DeleteByItem(product.ProductId, discount.DiscountId, appliedType);

                        var item = new DiscountAppliedToItem
                        {
                            ItemId = product.ProductId,
                            DiscountId = discount.DiscountId,
                            AppliedType = appliedType,
                            UsePercentage = usePercentage,
                            DiscountAmount = discountAmmount,
                            MaximumDiscount = maximumDiscount,
                            CreatedDate = dt
                        };

                        if (discount.DiscountType == (int)DiscountType.Deal)
                        {
                            var dealQty = -1;
                            var soldQty = -1;
                            var comboSaleQty = -1;
                            var fromDate = (DateTime?)null;
                            var toDate = (DateTime?)null;

                            if (!string.IsNullOrEmpty(strDealQty))
                            {
                                if (int.TryParse(strDealQty, out dealQty) && dealQty >= 0)
                                    item.DealQty = dealQty;
                            }
                            if (!string.IsNullOrEmpty(strSoldQty))
                            {
                                if (int.TryParse(strSoldQty, out soldQty) && soldQty >= 0)
                                    item.SoldQty = soldQty;
                            }
                            if (!string.IsNullOrEmpty(strComboSaleQty))
                            {
                                if (int.TryParse(strComboSaleQty, out comboSaleQty) && comboSaleQty >= 0)
                                    item.ComboSaleQty = comboSaleQty;
                            }
                            if (!string.IsNullOrEmpty(strFromDate))
                            {
                                DateTime dateValue;
                                var enUS = new CultureInfo("en-US");
                                if (DateTime.TryParseExact(strFromDate, formatDate, enUS,
                                                               DateTimeStyles.None, out dateValue))
                                    item.FromDate = dateValue;
                            }
                            if (!string.IsNullOrEmpty(strToDate))
                            {
                                DateTime dateValue;
                                var enUS = new CultureInfo("en-US");
                                if (DateTime.TryParseExact(strToDate, formatDate, enUS,
                                                               DateTimeStyles.None, out dateValue))
                                    item.ToDate = dateValue;
                            }
                        }

                        if (sortByItem)
                            item.DisplayOrder = displayOrder;

                        dt = dt.AddMilliseconds(1);

                        item.Save();
                        yes = true;
                    }
                }

                if (yes)
                    DiscountAppliedToItem.DeleteByItem(-appliedType, discount.DiscountId, appliedType);

                gridRelated.Rebind();
            }
            catch (Exception ex)
            {
                message.ErrorMessage = ex.Message;
                log.Error(ex.Message);
            }
        }

        private const string productCodeColName = "ProductCode";
        private const string productNameColName = "ProductName";
        private const string discountColName = "DiscountAmmount";
        private const string maximumColName = "MaximumDiscount";
        private const string dealQtyColName = "DealQty";
        private const string soldQtyColName = "SoldQty";
        private const string comboSaleQtyColName = "MaximumDiscountQty";
        private const string fromDateColName = "FromDate";
        private const string toDateColName = "ToDate";
        private const string displayOrderColName = "SortOrder";
        private const string formatDate = "yyyy-MM-dd HH:mm";

        private void btnProductRelatedExport_Click(object sender, EventArgs e)
        {
            var fileName = "promotion-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            var dt = new DataTable();
            dt.Columns.Add(productCodeColName, typeof(string));
            dt.Columns.Add(productNameColName, typeof(string));
            dt.Columns.Add(discountColName, typeof(string));
            dt.Columns.Add(maximumColName, typeof(string));

            if (discount.DiscountType == (int)DiscountType.Deal)
            {
                dt.Columns.Add(dealQtyColName, typeof(int));
                dt.Columns.Add(soldQtyColName, typeof(int));
                dt.Columns.Add(comboSaleQtyColName, typeof(int));
                dt.Columns.Add(fromDateColName, typeof(string));
                dt.Columns.Add(toDateColName, typeof(string));
            }

            var sortByItem = ConfigHelper.GetBoolProperty("PromotionSortByItem", false);
            if (sortByItem)
                dt.Columns.Add(displayOrderColName, typeof(int));

            var lstItems = DiscountAppliedToItem.GetByDiscount(discount.DiscountId, (int)DiscountAppliedType.ToProducts);
            var productIds = DiscountAppliedToItem.GetItemIdsFromList(lstItems, (int)DiscountAppliedType.ToProducts);
            if (productIds.Length > 0)
            {
                var keywords = string.IsNullOrEmpty(txtProductRelatedKeyword.Text.Trim()) ? null : txtProductRelatedKeyword.Text.Trim();
                var iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, productIds: productIds, keyword: keywords);
                lstProducts = Product.GetPageAdv(pageNumber: 1, pageSize: iCount, siteId: siteSettings.SiteId, productIds: productIds, keyword: keywords);

                var lstProductIds = lstProducts.Select(s => s.ProductId).ToList();
                lstItems = lstItems.Where(s => lstProductIds.Contains(s.ItemId) && s.ItemId != -1).ToList();
            }
            foreach (var item in lstItems)
            {
                var product = ProductHelper.GetProductFromList(lstProducts, item.ItemId);
                if (product != null)
                {
                    var row = dt.NewRow();
                    row[productCodeColName] = product.Code;
                    row[productNameColName] = product.Title;
                    row[discountColName] = Convert.ToDouble(item.DiscountAmount).ToString() + (item.UsePercentage ? "%" : "");
                    row[maximumColName] = Convert.ToDouble(item.MaximumDiscount);

                    if (discount.DiscountType == (int)DiscountType.Deal)
                    {
                        row[dealQtyColName] = item.DealQty;
                        row[soldQtyColName] = item.SoldQty;
                        row[comboSaleQtyColName] = item.ComboSaleQty;
                        if (item.FromDate.HasValue)
                            row[fromDateColName] = item.FromDate.Value.ToString(formatDate);
                        if (item.ToDate.HasValue)
                            row[toDateColName] = item.ToDate.Value.ToString(formatDate);
                    }

                    if (sortByItem)
                        row[displayOrderColName] = Convert.ToDouble(item.DisplayOrder);

                    dt.Rows.Add(row);
                }
            }

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        private void BtnUpdateCoupon_Click(object sender, EventArgs e)
        {
            bool updated = false;
            foreach (GridDataItem data in gridCoupon.Items)
            {
                Guid guid = new Guid(data.GetDataKeyValue("Guid").ToString());
                int limitationTimes = Convert.ToInt32(data.GetDataKeyValue("LimitationTimes"));
                TextBox txtLimitTimes = (TextBox)data.FindControl("txtLimitTimes");
                int.TryParse(txtLimitTimes.Text, out int newLimitTime);
                if (limitationTimes != newLimitTime)
                {
                    DiscountCoupon discountCoupon = new DiscountCoupon(guid);
                    if (discountCoupon != null && discountCoupon.Guid != Guid.Empty)
                    {
                        discountCoupon.LimitationTimes = newLimitTime;
                        discountCoupon.Save();
                        updated = true;
                    }
                }

            }
            if (updated)
                gridCoupon.Rebind();

        }

        private void ddlCouponNewType_SelectedIndexChanged(object sender, EventArgs e)
        {
            divCouponManual.Visible = ddlCouponNewType.SelectedValue == "1";
            divCouponGenerate.Visible = ddlCouponNewType.SelectedValue == "2";
            divCouponImport.Visible = ddlCouponNewType.SelectedValue == "3";
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            SetupScripts();

            if (!Page.IsPostBack)
            {
                PopulatePageList();
                PopulateControls();
                BindAttribute();
                PopulateAttributeControls();
            }
        }

        private void PopulateControls()
        {
            PopulateZoneList(cobZones);
            PopulateZoneList(cobExcludedZones, false, false);
            PromotionsHelper.PopulateDiscountType(ddlDiscountType, false);
            dpStartDate.SelectedDate = DateTime.Now;

            bool loadAllTab = false;
            if (discount != null && discount.DiscountId > 0)
            {
                txtName.Text = discount.Name;
                txtCode.Text = discount.Code;
                txtDiscountAmount.Text = ProductHelper.FormatPrice(discount.DiscountAmount);

                var liItem = ddlDiscountType.Items.FindByValue(discount.DiscountType.ToString());
                if (liItem != null)
                {
                    ddlDiscountType.ClearSelection();
                    liItem.Selected = true;
                }

                liItem = ddlUsePercentage.Items.FindByValue(discount.UsePercentage.ToString().ToLower());
                if (liItem != null)
                {
                    ddlUsePercentage.ClearSelection();
                    liItem.Selected = true;
                }

                cobExcludedZones.ClearSelection();
                if (!string.IsNullOrEmpty(discount.ExcludedZoneIDs))
                {
                    foreach (ListItem li in cobExcludedZones.Items)
                    {
                        if ((";" + discount.ExcludedZoneIDs).Contains(";" + li.Value + ";"))
                            li.Selected = true;
                    }
                }


                dpStartDate.SelectedDate = discount.StartDate;
                dpEndDate.SelectedDate = discount.EndDate;
                txtMinPurchase.Text = ProductHelper.FormatPrice(discount.MinPurchase);

                liItem = ddActiveStatus.Items.FindByValue(discount.IsActive.ToString().ToLower());
                if (liItem != null)
                {
                    ddActiveStatus.ClearSelection();
                    liItem.Selected = true;
                }

                liItem = ddlDiscountQtyStep.Items.FindByValue(discount.DiscountQtyStep.ToString());
                if (liItem != null)
                {
                    ddlDiscountQtyStep.ClearSelection();
                    liItem.Selected = true;
                }

                liItem = ddlPriority.Items.FindByValue(discount.Priority.ToString());
                if (liItem != null)
                {
                    ddlPriority.ClearSelection();
                    liItem.Selected = true;
                }
                txtMaximumDiscount.Text = ProductHelper.FormatPrice(discount.MaximumDiscount);

                PopulateDataByType(discount.DiscountType);

                //chkAlwaysOnDisplay.Checked = discount.AlwaysOnDisplay;
                cobPaymentMethod.ClearSelection();
                foreach (ListItem li in cobPaymentMethod.Items)
                {
                    if ((";" + discount.AppliedForPaymentIDs + ";").Contains(";" + li.Value + ";"))
                        li.Selected = true;
                }

                //liItem = ddlAppliedAllProducts.Items.FindByValue(discount.AppliedAllProducts.ToString().ToLower());
                //if (liItem != null)
                //{
                //    ddlAppliedAllProducts.ClearSelection();
                //    liItem.Selected = true;
                //}
                //PopulateAppliedType();

                if (divShowOption.Visible)
                {
                    chlShowOption.ClearSelection();
                    foreach (ListItem li in chlShowOption.Items)
                    {
                        if ((Convert.ToInt32(li.Value) & discount.Options) > 0)
                            li.Selected = true;
                    }
                }

                chkListShareType.ClearSelection();
                foreach (ListItem li in chkListShareType.Items)
                {
                    if ((Convert.ToInt32(li.Value) & discount.ShareType) > 0)
                        li.Selected = true;
                }

                edBriefContent.Text = discount.BriefContent;
                edFullContent.Text = discount.FullContent;

                if (!string.IsNullOrEmpty(discount.ImageFile))
                {
                    chkDeleteImageFile.Visible = true;
                    imgImageFile.Visible = true;
                    imgImageFile.Src = imageFolderPath + discount.ImageFile;
                }
                if (!string.IsNullOrEmpty(discount.BannerFile))
                {
                    chkDeleteBannerFile.Visible = true;
                    imgBannerFile.Visible = true;
                    imgBannerFile.Src = imageFolderPath + discount.BannerFile;
                }

                txtUrl.Text = discount.Url;
                hdnTitle.Value = txtName.Text;

                var listItem = ddPages.Items.FindByValue(discount.PageId.ToString());
                if (listItem != null)
                {
                    ddPages.ClearSelection();
                    listItem.Selected = true;
                }
                //chkPageID.Checked = discount.PageId > 0;

                if (discount.DiscountType == (int)DiscountType.Coupon && gridOrder.MasterTableView.GetColumn("CouponCode") != null)
                    gridOrder.MasterTableView.GetColumn("CouponCode").Visible = true;

                loadAllTab = true;
            }

            LanguageHelper.PopulateTab(tabLanguage, loadAllTab);
        }

        private void PopulatePageList()
        {
            ddPages.Items.Clear();

            if (displaySettings.HasDetailPage)
            {
                var listPages = PageSettings.GetPageList(siteSettings.SiteId);
                foreach (var pageSettings in listPages)
                {
                    if (!pageSettings.IsPending)
                    {
                        var li = new ListItem(pageSettings.PageName, pageSettings.PageId.ToString());
                        ddPages.Items.Add(li);
                    }
                }
            }

            ddPages.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "PageSettingsChoosePage"), "-1"));
        }

        private void SetupScripts()
        {
            if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa", "<script src=\""
                    + ResolveUrl("~/ClientScript/sarissa/sarissa.js") + "\" type=\"text/javascript\"></script>");
            }

            if (!Page.ClientScript.IsClientScriptBlockRegistered("sarissa_ieemu_xpath"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "sarissa_ieemu_xpath", "<script src=\""
                    + ResolveUrl("~/ClientScript/sarissa/sarissa_ieemu_xpath.js") + "\" type=\"text/javascript\"></script>");
            }

            SetupUrlSuggestScripts(this.txtName.ClientID, this.txtUrl.ClientID, this.hdnTitle.ClientID, this.spnUrlWarning.ClientID);
        }

        private void SetupUrlSuggestScripts(string inputText, string outputText, string referenceText, string warningSpan)
        {
            if (!tabLandingPage.Visible || !divPages.Visible || !divUrl.Visible) return;

            if (!Page.ClientScript.IsClientScriptBlockRegistered("friendlyurlsuggest"))
            {
                this.Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "friendlyurlsuggest", "<script src=\""
                    + ResolveUrl("~/ClientScript/friendlyurlsuggest_v2.js") + "\" type=\"text/javascript\"></script>");
            }

            string focusScript = string.Empty;
            if (discountID == -1) { focusScript = "document.getElementById('" + inputText + "').focus();"; }

            string hookupInputScript = "new UrlHelper( "
                + "document.getElementById('" + inputText + "'),  "
                + "document.getElementById('" + outputText + "'), "
                + "document.getElementById('" + referenceText + "'), "
                + "document.getElementById('" + warningSpan + "'), "
                + "\"" + SiteRoot + "/Product/Services/ProductUrlSuggestService.ashx" + "\""
                + "); " + focusScript;

            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), inputText + "urlscript", hookupInputScript, true);
        }

        #region Populate Zone List

        private void PopulateZoneList(ComboBox cobZones, bool addAll = true, bool selected = true)
        {
            gbSiteMapProvider.PopulateListControl(cobZones, false, Product.FeatureGuid);

            if (addAll && WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
                cobZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), "-1"));

            if (selected && cobZones.Items.Count > 0)
                cobZones.SelectedIndex = 0;
        }

        #endregion Populate Zone List

        private void btnInsert_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx?DiscountID=" + itemId.ToString());
        }

        private void btnInsertAndClose_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/Discounts.aspx");
        }

        private void btnInsertAndNew_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx?DiscountID=" + itemId.ToString());
        }

        private void btnUpdateAndClose_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/Discounts.aspx");
        }

        private void btnUpdateAndNew_Click(object sender, EventArgs e)
        {
            int itemId = SaveData();
            if (itemId > 0)
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/DiscountEdit.aspx");
        }

        private bool IsDateValid(DateTime? fromDate, DateTime? toDate, RadGrid grid)
        {
            foreach (GridDataItem data in grid.Items)
            {
                var dpFromDate = (RadDateTimePicker)data.FindControl("dpFromDate");
                var dpToDate = (RadDateTimePicker)data.FindControl("dpToDate");

                if (dpFromDate.SelectedDate.HasValue && fromDate.HasValue && dpFromDate.SelectedDate.Value < fromDate)
                    return false;

                if (dpToDate.SelectedDate.HasValue && toDate.HasValue && dpToDate.SelectedDate.Value > toDate.Value)
                    return false;
            }

            return true;
        }

        private int SaveData()
        {
            if (!Page.IsValid) return -1;

            try
            {
                var discountType = Convert.ToInt32(ddlDiscountType.SelectedValue);
                if (discountType == (int)DiscountType.Deal)
                {
                    if (!IsDateValid(dpStartDate.SelectedDate, dpEndDate.SelectedDate, gridRelated))
                    {
                        message.ErrorMessage = "Một (số) sản phẩm có Ngày áp dụng không hợp lệ.";
                        return -1;
                    }
                }

                if (discount == null || discount.DiscountId == -1)
                {
                    discount = new Discount
                    {
                        SiteId = siteSettings.SiteId
                    };

                    var siteUser = SiteUtils.GetCurrentSiteUser();
                    if (siteUser != null && siteUser.UserId > 0)
                        discount.CreatedBy = siteUser.LoginName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(discount.ImageFile) && chkDeleteImageFile.Checked)
                    {
                        var newImagePath = VirtualPathUtility.Combine(imageFolderPath, discount.ImageFile);
                        PromotionsHelper.DeleteImage(newImagePath);
                        discount.ImageFile = string.Empty;
                    }
                    if (!string.IsNullOrEmpty(discount.BannerFile) && chkDeleteBannerFile.Checked)
                    {
                        var newImagePath = VirtualPathUtility.Combine(imageFolderPath, discount.BannerFile);
                        PromotionsHelper.DeleteImage(newImagePath);
                        discount.BannerFile = string.Empty;
                    }

                    if (fupImageFile.UploadedFiles.Count > 0)
                    {
                        var file = fupImageFile.UploadedFiles[0];
                        var ext = file.GetExtension();

                        if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                        {
                            PromotionsHelper.VerifyPromotionFolders(imageFolderPath);

                            var newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                            var newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (discount.ImageFile == newFileName)
                            {
                                PromotionsHelper.DeleteImage(newImagePath);
                            }
                            else
                            {
                                int i = 1;
                                while (File.Exists(Server.MapPath(VirtualPathUtility.Combine(imageFolderPath, newFileName))))
                                {
                                    newFileName = i.ToInvariantString() + newFileName;
                                    i += 1;
                                }
                            }

                            newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (!string.IsNullOrEmpty(discount.ImageFile))
                            {
                                var imageVirtualPath = imageFolderPath + discount.ImageFile;
                                PromotionsHelper.DeleteImage(imageVirtualPath);
                            }

                            file.SaveAs(Server.MapPath(newImagePath));

                            discount.ImageFile = newFileName;
                        }
                    }

                    if (fupBannerFile.UploadedFiles.Count > 0)
                    {
                        var file = fupBannerFile.UploadedFiles[0];
                        var ext = file.GetExtension();

                        if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                        {
                            PromotionsHelper.VerifyPromotionFolders(imageFolderPath);

                            var newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                            var newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (discount.BannerFile == newFileName)
                            {
                                PromotionsHelper.DeleteImage(newImagePath);
                            }
                            else
                            {
                                int i = 1;
                                while (File.Exists(Server.MapPath(VirtualPathUtility.Combine(imageFolderPath, newFileName))))
                                {
                                    newFileName = i.ToInvariantString() + newFileName;
                                    i += 1;
                                }
                            }

                            newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (!string.IsNullOrEmpty(discount.BannerFile))
                            {
                                var imageVirtualPath = imageFolderPath + discount.BannerFile;
                                PromotionsHelper.DeleteImage(imageVirtualPath);
                            }

                            file.SaveAs(Server.MapPath(newImagePath));

                            discount.BannerFile = newFileName;
                        }
                    }
                }

                discount.Name = txtName.Text.Trim();
                discount.Code = txtCode.Text.Trim();
                decimal.TryParse(txtDiscountAmount.Text, out decimal discountAmount);
                discount.DiscountAmount = discountAmount;
                discount.StartDate = dpStartDate.SelectedDate;
                discount.EndDate = dpEndDate.SelectedDate;
                discount.DiscountType = discountType;
                discount.UsePercentage = Convert.ToBoolean(ddlUsePercentage.SelectedValue);

                decimal.TryParse(txtMinPurchase.Text, out decimal minPurchase);
                discount.MinPurchase = minPurchase;

                bool.TryParse(ddActiveStatus.SelectedValue, out bool isActive);
                discount.IsActive = isActive;

                int.TryParse(ddlDiscountQtyStep.Text, out int discountQtyStep);
                discount.DiscountQtyStep = discountQtyStep;

                discount.Priority = Convert.ToInt32(ddlPriority.SelectedValue);
                decimal.TryParse(txtMaximumDiscount.Text, out decimal maximumDiscount);
                discount.MaximumDiscount = maximumDiscount;

                //discount.AlwaysOnDisplay = false;
                discount.AppliedForPaymentIDs = string.Empty;
                if (HasDiscountType(discount.DiscountType, DiscountType.PaymentMethod))
                {
                    //discount.AlwaysOnDisplay = chkAlwaysOnDisplay.Checked;
                    var sepa = string.Empty;
                    foreach (var li in cobPaymentMethod.SelectedItems)
                    {
                        discount.AppliedForPaymentIDs += sepa + li.Value;
                        sepa = ";";
                    }
                }
                //discount.AppliedAllProducts = Convert.ToBoolean(ddlAppliedAllProducts.SelectedValue);

                discount.Options = 0;
                if (divShowOption.Visible)
                    discount.Options = chlShowOption.Items.SelectedItemsToBinaryOrOperator();

                discount.ShareType = chkListShareType.Items.SelectedItemsToBinaryOrOperator();

                var oldUrl = string.Empty;
                var newUrl = string.Empty;
                var friendlyUrlString = string.Empty;
                var friendlyUrl = (FriendlyUrl)null;
                if (!IsLanguageTab())
                {
                    //discount.Title = txtTitle.Text.Trim();
                    discount.BriefContent = edBriefContent.Text;
                    discount.FullContent = edFullContent.Text;

                    if (displaySettings.HasDetailPage)
                    {
                        if (txtUrl.Text.Length == 0)
                            txtUrl.Text = "~/" + SiteUtils.SuggestFriendlyUrl(txtName.Text, siteSettings);

                        friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(txtUrl.Text.Replace("~/", String.Empty));

                        if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                            friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);

                        friendlyUrl = new FriendlyUrl(siteSettings.SiteId, friendlyUrlString);

                        if (
                            ((friendlyUrl.FoundFriendlyUrl) && (friendlyUrl.PageGuid != discount.Guid))
                            && (discount.Url != txtUrl.Text.Trim())
                            && (!txtUrl.Text.StartsWith("http"))
                            )
                        {
                            message.ErrorMessage = ProductResources.PageUrlInUseErrorMessage;
                            return -1;
                        }

                        oldUrl = discount.Url.Replace("~/", string.Empty);
                        newUrl = friendlyUrlString;

                        if (txtUrl.Text.Trim().StartsWith("http"))
                            discount.Url = txtUrl.Text.Trim();
                        else if (friendlyUrlString.Length > 0)
                            discount.Url = "~/" + friendlyUrlString;
                        else if (friendlyUrlString.Length == 0)
                            discount.Url = string.Empty;
                    }
                }

                discount.PageId = Convert.ToInt32(ddPages.SelectedValue);
                //discount.PageId = chkPageID.Checked ? 1 : 0;

                if (divExcludedZones.Visible)
                {
                    discount.ExcludedZoneIDs = string.Empty;
                    var sepa = string.Empty;
                    foreach (var li in cobExcludedZones.SelectedItems)
                        discount.ExcludedZoneIDs += li.Value + ";";
                }

                if (discount.Save())
                    SaveContentLanguage(discount.Guid);

                UpdateDiscountApplied(gridRelated2);
                UpdateDiscountApplied(gridRelated);
                UpdateDiscountRange(gridOrderDiscountRange);

                if (displaySettings.HasDetailPage && !IsLanguageTab())
                {
                    if (
                        (oldUrl.Length > 0)
                        && (newUrl.Length > 0)
                        && (!SiteUtils.UrlsMatch(oldUrl, newUrl))
                        )
                    {
                        FriendlyUrl oldFriendlyUrl = new FriendlyUrl(siteSettings.SiteId, oldUrl);
                        if ((oldFriendlyUrl.FoundFriendlyUrl) && (oldFriendlyUrl.PageGuid == discount.Guid))
                        {
                            FriendlyUrl.DeleteUrl(oldFriendlyUrl.UrlId);
                        }
                    }

                    if (
                        ((txtUrl.Text.EndsWith(".aspx")) || siteSettings.DefaultFriendlyUrlPattern == SiteSettings.FriendlyUrlPattern.PageName)
                        && (txtUrl.Text.StartsWith("~/"))
                        )
                    {
                        if (!friendlyUrl.FoundFriendlyUrl)
                        {
                            if ((friendlyUrlString.Length > 0) && (!WebPageInfo.IsPhysicalWebPage("~/" + friendlyUrlString)))
                            {
                                var newFriendlyUrl = new FriendlyUrl
                                {
                                    SiteId = siteSettings.SiteId,
                                    SiteGuid = siteSettings.SiteGuid,
                                    PageGuid = discount.Guid,
                                    Url = friendlyUrlString,
                                    RealUrl = "~/Product/Promotion.aspx"
                                    + "?PromotionID=" + discount.DiscountId.ToString()
                                };

                                newFriendlyUrl.Save();
                            }
                        }
                    }
                }

                if (discountID > 0)
                {
                    LogActivity.Write("Update discount", discount.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                }
                else
                {
                    LogActivity.Write("Create new discount", discount.Name);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }

                return discount.DiscountId;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return -1;
        }

        private void UpdateDiscountApplied(Telerik.Web.UI.RadGrid grid)
        {
            foreach (GridDataItem data in grid.Items)
            {
                var txtDiscountGrid = (TextBox)data.FindControl("txtDiscountAmount");
                var txtMaximumDiscountGrid = (TextBox)data.FindControl("txtMaximumDiscount");
                var ddlDiscountTypeGrid = (DropDownList)data.FindControl("ddlDiscountType");
                var txtDealQty = (TextBox)data.FindControl("txtDealQty");
                var txtSoldQty = (TextBox)data.FindControl("txtSoldQty");
                var txtComboSaleQty = (TextBox)data.FindControl("txtComboSaleQty");
                var txtDisplayOrder = (TextBox)data.FindControl("txtDisplayOrder");
                var dpFromDate = (RadDateTimePicker)data.FindControl("dpFromDate");
                var dpToDate = (RadDateTimePicker)data.FindControl("dpToDate");
                var usePercentage = Convert.ToBoolean(data.GetDataKeyValue("UsePercentage"));
                var discountAmount = Convert.ToDecimal(data.GetDataKeyValue("DiscountAmount"));
                var maximumDiscount = Convert.ToDecimal(data.GetDataKeyValue("MaximumDiscount"));
                var dealQty = Convert.ToInt32(data.GetDataKeyValue("DealQty"));
                var soldQty = Convert.ToInt32(data.GetDataKeyValue("SoldQty"));
                var comboSaleQty = Convert.ToInt32(data.GetDataKeyValue("ComboSaleQty"));
                var displayOrder = Convert.ToInt32(data.GetDataKeyValue("DisplayOrder"));
                var fromDate = (DateTime?)data.GetDataKeyValue("FromDate");
                var toDate = (DateTime?)data.GetDataKeyValue("ToDate");
                var guid = new Guid(data.GetDataKeyValue("Guid").ToString());

                decimal discountAmountNew = discountAmount;
                decimal.TryParse(txtDiscountGrid.Text, out discountAmountNew);

                decimal maximumDiscountNew = maximumDiscount;
                decimal.TryParse(txtMaximumDiscountGrid.Text, out maximumDiscountNew);

                int dealQtyNew = dealQty;
                if (txtDealQty != null)
                    int.TryParse(txtDealQty.Text, out dealQtyNew);

                int soldQtyNew = soldQty;
                if (txtSoldQty != null)
                    int.TryParse(txtSoldQty.Text, out soldQtyNew);

                int comboSaleQtyNew = comboSaleQty;
                if (txtComboSaleQty != null)
                    int.TryParse(txtComboSaleQty.Text, out comboSaleQtyNew);

                int displayOrderNew = displayOrder;
                if (txtDisplayOrder != null)
                    int.TryParse(txtDisplayOrder.Text, out displayOrderNew);

                if (
                    discountAmount != discountAmountNew
                    || maximumDiscount != maximumDiscountNew
                    || (dealQty != dealQtyNew && txtDealQty != null)
                    || (soldQty != soldQtyNew && txtSoldQty != null)
                    || (comboSaleQty != comboSaleQtyNew && txtComboSaleQty != null)
                    || (displayOrder != displayOrderNew && txtDisplayOrder != null)
                    || (ddlDiscountTypeGrid.SelectedIndex == 0 && !usePercentage)
                    || (ddlDiscountTypeGrid.SelectedIndex == 1 && usePercentage)
                    || (dpFromDate != null && fromDate != dpFromDate.SelectedDate)
                    || (dpToDate != null && toDate != dpToDate.SelectedDate)
                )
                {
                    var item = new DiscountAppliedToItem(guid);
                    if (item != null && item.Guid != Guid.Empty)
                    {
                        if (ddlDiscountTypeGrid.SelectedIndex == 0)
                            item.UsePercentage = true;
                        else
                            item.UsePercentage = false;

                        item.DiscountAmount = discountAmountNew;
                        item.MaximumDiscount = maximumDiscountNew;
                        item.DisplayOrder = displayOrderNew;
                        item.DealQty = dealQtyNew;
                        item.SoldQty = soldQtyNew;
                        item.ComboSaleQty = comboSaleQtyNew;
                        if (dpFromDate != null && dpFromDate.SelectedDate.HasValue)
                            item.FromDate = dpFromDate.SelectedDate;
                        if (dpToDate != null && dpToDate.SelectedDate.HasValue)
                            item.ToDate = dpToDate.SelectedDate;

                        item.Save();

                        //LogActivity.Write("Change discount", order.Guid.ToString());
                    }
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (discount != null && discount.DiscountId > -1)
                {
                    PromotionsHelper.DeleteFolder(siteSettings.SiteId, discount.DiscountId);

                    var lstContent = DiscountContent.GetByDiscount(discount.DiscountId);
                    foreach (var c in lstContent)
                        ContentLanguage.DeleteByContent(c.Guid);
                    DiscountContent.DeleteByDiscount(discount.DiscountId);
                    DiscountRange.DeleteByDiscount(discount.DiscountId);
                    DiscountAppliedToItem.DeleteByDiscount(discount.DiscountId);
                    DiscountUsageHistory.DeleteByDiscount(discount.DiscountId);
                    DiscountGift.DeleteByDiscount(discount.DiscountId);
                    DiscountCoupon.DeleteByDiscount(discount.DiscountId);
                    Discount.Delete(discount.DiscountId);

                    LogActivity.Write("Delete discount", discount.Name);

                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                }

                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Promotions/Discounts.aspx");
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, ProductResources.DiscountEditTitle);
            heading.Text = ProductResources.DiscountEditTitle;

            litTabContent.Text = "<a class=\"active\" aria-controls=\"tabContent\" role=\"tab\" data-toggle=\"tab\" href='#tabContent'>" +
                                 ProductResources.DiscountInfoTab + "</a>";
            litTabCoupon.Text = "<a aria-controls=\"" + tabCoupon.ClientID + "\" role=\"tab\" data-toggle=\"tab\" href='#" +
                                 tabCoupon.ClientID + "'>"
                                 + ProductResources.CouponCodeLabel + "</a>";
            litTabLandingPage.Text = "<a aria-controls=\"" + tabLandingPage.ClientID + "\" role=\"tab\" data-toggle=\"tab\" href='#" +
                                 tabLandingPage.ClientID + "'>"
                                 + ProductResources.DiscountLandingPageTab + "</a>";
            litTabHistory.Text = "<a aria-controls=\"" + tabHistory.ClientID + "\" role=\"tab\" data-toggle=\"tab\" href='#" +
                                 tabHistory.ClientID + "'>"
                                 + ProductResources.DiscountUsageHistoryTab + "</a>";

            UIHelper.AddConfirmationDialog(btnDelete, ResourceHelper.GetResourceString("Resource", "DeleteConfirmMessage"));
            UIHelper.AddConfirmationDialog(btnAll, "Bạn có chắc chắn muốn thêm Toàn bộ Sản phẩm? Các sản phẩm đã chọn sẽ bị LOẠI BỎ.");
            //UIHelper.AddConfirmationDialog(btnAll2, "Bạn có chắc chắn muốn thêm Toàn bộ Danh mục? Các Danh mục đã chọn sẽ bị LOẠI BỎ.");
            UIHelper.AddConfirmationDialog(btnDeleteCoupon, "Bạn có chắc chắn muốn XÓA mã được chọn?");

            edBriefContent.WebEditor.Height = Unit.Pixel(300);
            edBriefContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edFullContent.WebEditor.Height = Unit.Pixel(300);
            edFullContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
            edAttributeContent.WebEditor.Height = Unit.Pixel(300);
            edAttributeContent.WebEditor.ToolBar = ToolBar.FullWithTemplates;
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();

            discountID = WebUtils.ParseInt32FromQueryString("DiscountID", discountID);

            if (discountID > 0)
            {
                discount = new Discount(discountID);

                if (
                    discount != null
                    && discount.DiscountId > 0
                    && discount.SiteId == siteSettings.SiteId
                )
                {
                    //if (discount.DiscountType == (int)DiscountType.Deal || discount.DiscountType == (int)DiscountType.Promotion)
                    //	divDiscountQtyStep.Visible = true;

                    imageFolderPath = PromotionsHelper.ImagePath(siteSettings.SiteId, discount.DiscountId);
                }
                else
                    discount = null;
            }

            HideControls();

            divPages.Visible = displaySettings.HasDetailPage;
            divUrl.Visible = displaySettings.HasDetailPage;

            AddClassToBody("discount-edit");
        }

        private void HideControls()
        {
            btnInsert.Visible = false;
            btnInsertAndNew.Visible = false;
            btnInsertAndClose.Visible = false;
            btnUpdate.Visible = false;
            btnUpdateAndNew.Visible = false;
            btnUpdateAndClose.Visible = false;
            btnDelete.Visible = false;

            ulTabs.Visible = false;
            tabHistory.Visible = false;

            if (discount == null)
            {
                btnInsert.Visible = true;
                btnInsertAndNew.Visible = true;
                btnInsertAndClose.Visible = true;
            }
            else if (discount != null && discount.DiscountId > 0)
            {
                btnUpdate.Visible = true;
                btnUpdateAndNew.Visible = true;
                btnUpdateAndClose.Visible = true;

                btnDelete.Visible = true;

                ulTabs.Visible = true;
                tabHistory.Visible = true;

                ddlDiscountType.Enabled = false;
                tabCoupon.Visible = true;
                tabLandingPage.Visible = true;
                tabHistory.Visible = true;
            }
        }

        #region Applied

        private string sZoneId
        {
            get
            {
                if (cobZones.SelectedValue.Length > 0)
                {
                    if (cobZones.SelectedValue == "-1")
                        return string.Empty;

                    return cobZones.SelectedValue;
                }

                return "0";
            }
        }

        private void grid2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (!Page.IsPostBack)
                grid2.DataSource = new List<ZoneSettings>();
            else
            {
                List<ZoneSettings> lstZones = new List<ZoneSettings>();

                foreach (ListItem li in cobZones.Items)
                {
                    if (li.Value != "-1")
                    {
                        ZoneSettings zone = new ZoneSettings
                        {
                            ZoneId = Convert.ToInt32(li.Value),
                            Name = li.Text
                        };

                        lstZones.Add(zone);
                    }
                }

                grid2.DataSource = lstZones;
            }
        }

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            if (!Page.IsPostBack)
                grid.DataSource = new List<Product>();
            else
            {
                bool isApplied = false;
                int iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, zoneIds: sZoneId, keyword: txtTitle.Text.Trim());
                int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
                int maximumRows = isApplied ? iCount : grid.MasterTableView.PageSize;

                grid.VirtualItemCount = iCount;
                grid.AllowCustomPaging = !isApplied;

                grid.DataSource = Product.GetPageAdv(pageNumber: startRowIndex, pageSize: maximumRows, siteId: siteSettings.SiteId, zoneIds: sZoneId, keyword: txtTitle.Text.Trim());
            }
        }

        private void gridRelated_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                var item = (Telerik.Web.UI.GridDataItem)e.Item;
                var litProductCode = (Literal)item.FindControl("litProductCode");
                var litProductName = (Literal)item.FindControl("litProductName");
                var ml = (MediaElement)item.FindControl("ml");
                var ddlDiscountType = (DropDownList)item.FindControl("ddlDiscountType");
                var usePercentage = Convert.ToBoolean(item.GetDataKeyValue("UsePercentage"));
                var productId = Convert.ToInt32(item.GetDataKeyValue("ItemId"));

                if (usePercentage)
                    ddlDiscountType.SelectedIndex = 0;
                else
                    ddlDiscountType.SelectedIndex = 1;

                if (productId == -1)
                    litProductName.Text = "Tất cả Sản phẩm";
                else
                {
                    var product = ProductHelper.GetProductFromList(lstProducts, productId);
                    if (product != null)
                    {
                        litProductCode.Text = product.Code;
                        litProductName.Text = string.Format("<a href='{0}'>{1}</a>", ProductHelper.FormatProductUrl(product.Url, product.ProductId,
                                                            product.ZoneId), product.Title);
                        ml.FileUrl = ProductHelper.GetImageFilePath(siteSettings.SiteId, productId, product.ImageFile, product.ThumbnailFile);
                    }
                }
            }
        }

        private void grid_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (grid.Items.Count <= 1)
                    return;

                int productId = -1;
                int.TryParse(e.CommandArgument.ToString(), out productId);

                if (discount != null && productId > 0 && e.CommandName == "SpecialProduct")
                {
                    if (discount.SpecialProductId == productId)
                        discount.SpecialProductId = -1;
                    else
                        discount.SpecialProductId = productId;

                    discount.Save();

                    grid.Rebind();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private List<Product> lstProducts = new List<Product>();

        private void gridRelated_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            divExcludedZones.Visible = false;

            if (discount != null && discount.DiscountId > 0)
            {
                var lstItems = DiscountAppliedToItem.GetByDiscount(discountID, (int)DiscountAppliedType.ToProducts);
                var productIds = DiscountAppliedToItem.GetItemIdsFromList(lstItems, (int)DiscountAppliedType.ToProducts);
                if (productIds.Length > 0)
                {
                    var keywords = string.IsNullOrEmpty(txtProductRelatedKeyword.Text.Trim()) ? null : txtProductRelatedKeyword.Text.Trim();
                    var iCount = Product.GetCountAdv(siteId: siteSettings.SiteId, productIds: productIds, keyword: keywords);
                    lstProducts = Product.GetPageAdv(pageNumber: 1, pageSize: iCount, siteId: siteSettings.SiteId, productIds: productIds, keyword: keywords);

                    var lstProductIds = lstProducts.Select(s => s.ProductId).ToList();
                    lstItems = lstItems.Where(s => lstProductIds.Contains(s.ItemId) || s.ItemId == -1).ToList();
                }

                gridRelated.DataSource = lstItems;

                if (lstItems.Count == 1 && lstItems[0].ItemId == -1)
                    divExcludedZones.Visible = true;
            }
            else
                gridRelated.DataSource = new List<DiscountAppliedToItem>();
        }

        private void btnProductRelatedKeyword_Click(object sender, EventArgs e)
        {
            gridRelated.Rebind();
        }

        private void gridRelated2_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                var item = (Telerik.Web.UI.GridDataItem)e.Item;
                var litZoneName = (Literal)item.FindControl("litZoneName");
                var ddlDiscountType = (DropDownList)item.FindControl("ddlDiscountType");
                var usePercentage = Convert.ToBoolean(item.GetDataKeyValue("UsePercentage"));
                var zoneId = Convert.ToInt32(item.GetDataKeyValue("ItemId"));

                if (usePercentage)
                    ddlDiscountType.SelectedIndex = 0;
                else
                    ddlDiscountType.SelectedIndex = 1;

                if (zoneId == 0)
                    litZoneName.Text = "Tất cả Danh mục";
                else
                {
                    foreach (ListItem li in cobZones.Items)
                    {
                        if (li.Value == zoneId.ToString())
                        {
                            litZoneName.Text = string.Format("<a href='{0}'>{1}</a>", SiteRoot + "/AdminCP/ZoneSettings.aspx?zoneid=" + zoneId.ToString(), li.Text);
                            break;
                        }
                    }
                }
            }
        }

        private void gridRelated2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (discount != null && discount.DiscountId > 0)
                gridRelated2.DataSource = DiscountAppliedToItem.GetByDiscount(discountID, (int)DiscountAppliedType.ToCategories);
            else
                gridRelated2.DataSource = new List<DiscountAppliedToItem>();
        }

        private IDataReader reader;

        private void gridOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (discount != null && discount.DiscountId > 0)
            {
                reader = DiscountUsageHistory.GetByDiscount(discount.DiscountId);
                gridOrder.DataSource = reader;
            }
            else
                gridOrder.DataSource = new List<DiscountUsageHistory>();
        }

        private void gridOrder_DataBound(object sender, EventArgs e)
        {
            if (reader != null)
                reader.Close();
        }

        private void btnSearch2_Click(object sender, EventArgs e)
        {
            grid2.Rebind();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void btnAdd2_Click(object sender, EventArgs e)
        {
            MoveItems(false, grid2, gridRelated2, "ZoneId", (int)DiscountAppliedType.ToCategories);
        }

        private void btnRemove2_Click(object sender, EventArgs e)
        {
            MoveItems(true, grid2, gridRelated2, "ZoneId", (int)DiscountAppliedType.ToCategories);
        }

        private bool AllowedAddAll()
        {
            if (discount != null
                && discount.DiscountType != (int)DiscountType.ComboSale
                && discount.DiscountType != (int)DiscountType.ByProductRange
                && discount.DiscountType != (int)DiscountType.Deal
                )
                return true;

            return false;
        }

        private bool MoveItems(bool isRemove, Telerik.Web.UI.RadGrid gridSource, Telerik.Web.UI.RadGrid gridDes, string itemKey,
                               int appliedType, bool isAddAll = false)
        {
            if (isAddAll && !AllowedAddAll()) return false;

            if (discount != null && discount.DiscountId > 0)
            {
                if (!isRemove)
                {
                    if (isAddAll)
                    {
                        DiscountAppliedToItem.DeleteByDiscount(discount.DiscountId, appliedType);

                        var item = new DiscountAppliedToItem
                        {
                            ItemId = -appliedType,
                            DiscountId = discount.DiscountId,
                            AppliedType = appliedType
                        };

                        item.Save();

                        if (!string.IsNullOrEmpty(discount.ExcludedZoneIDs))
                        {
                            discount.ExcludedZoneIDs = string.Empty;
                            discount.Save();
                        }
                    }
                    else
                    {
                        if (gridSource.SelectedItems.Count > 0 && appliedType != 0)
                            DiscountAppliedToItem.DeleteByItem(-appliedType, discount.DiscountId, appliedType);

                        foreach (Telerik.Web.UI.GridDataItem data in gridSource.SelectedItems)
                        {
                            var itemId = Convert.ToInt32(data.GetDataKeyValue(itemKey));
                            DiscountAppliedToItem.DeleteByItem(itemId, discount.DiscountId, appliedType);

                            var item = new DiscountAppliedToItem
                            {
                                ItemId = itemId,
                                DiscountId = discount.DiscountId,
                                AppliedType = appliedType
                            };

                            if (discount.DiscountType == (int)DiscountType.ComboSale)
                                item.ComboSaleQty = 1;
                            if (discount.DiscountType == (int)DiscountType.Deal)
                            {
                                item.FromDate = discount.StartDate;
                                item.ToDate = discount.EndDate;
                            }

                            item.Save();
                        }
                    }
                }
                else
                {
                    foreach (Telerik.Web.UI.GridDataItem data in gridDes.SelectedItems)
                    {
                        var guid = new Guid(data.GetDataKeyValue("Guid").ToString());
                        DiscountAppliedToItem.Delete(guid);
                    }
                }
            }

            gridDes.Rebind();

            return true;
        }

        private void btnAll2_Click(object sender, EventArgs e)
        {
            MoveItems(false, grid2, gridRelated2, "ZoneId", (int)DiscountAppliedType.ToCategories, true);
        }

        private void btnAll_Click(object sender, EventArgs e)
        {
            MoveItems(false, grid, gridRelated, "ProductId", (int)DiscountAppliedType.ToProducts, true);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //MoveProducts();
            MoveItems(false, grid, gridRelated, "ProductId", (int)DiscountAppliedType.ToProducts);
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            //MoveProducts(true);
            MoveItems(true, grid, gridRelated, "ProductId", (int)DiscountAppliedType.ToProducts);
        }

        private bool MoveProducts(bool isRemove = false)
        {
            if (discount != null && discount.DiscountId > 0)
            {
                if (!isRemove)
                {
                    foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
                    {
                        int productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                        DiscountAppliedToItem item = new DiscountAppliedToItem
                        {
                            ItemId = productId,
                            DiscountId = discount.DiscountId,
                            AppliedType = (int)DiscountAppliedType.ToProducts
                        };
                        item.Save();
                    }
                }
                else
                {
                    foreach (Telerik.Web.UI.GridDataItem data in gridRelated.SelectedItems)
                    {
                        Guid guid = new Guid(data.GetDataKeyValue("Guid").ToString());
                        DiscountAppliedToItem.Delete(guid);
                    }
                }
            }

            gridRelated.Rebind();

            return true;
        }

        protected String GetSpecialProductImageUrl(int productId)
        {
            if (discount != null && discount.SpecialProductId == productId)
                return ResolveUrl("~/Data/SiteImages/PublishedTrue.png");

            return ResolveUrl("~/Data/SiteImages/PublishedFalse.png");
        }

        #endregion Applied

        private void PopulateDataByType(int type)
        {
            if (type > 0)
            {
                if (HasDiscountType(type, DiscountType.ByOrderRange))
                {
                    //divMaximumDiscount.Visible = true;
                    divOrderDiscountRange.Visible = true;
                    divPriority.Visible = false;
                    //chkListShareType.Visible = false;
                    //litShareType.Visible = true;
                    divShareType.Visible = true;
                }
                else if (HasDiscountType(type, DiscountType.ByCatalog))
                {
                    //divMaximumDiscount.Visible = true;
                    //divAppliedAllProducts.Visible = true;
                    divAppliedToCategories.Visible = true;
                    divAppliedToProducts.Visible = true;
                }
                else if (HasDiscountType(type, DiscountType.ByProductRange))
                {
                    gridRelated.Columns.FindByUniqueName("ProductRange").Visible = true;
                    gridRelated.Columns.FindByUniqueName("DiscountAmount").Visible = false;
                    gridRelated.Columns.FindByUniqueName("MaximumDiscount").Visible = false;
                    gridRelated.Columns.FindByUniqueName("GiftHtml").Visible = false;
                    //divMaximumDiscount.Visible = true;
                    divAppliedToProducts.Visible = true;
                }
                else if (HasDiscountType(type, DiscountType.PaymentMethod))
                {
                    divDiscountAmount.Visible = true;
                    divMaximumDiscount.Visible = true;
                    //divAlwaysOnDisplay.Visible = true;
                    divPaymentMethod.Visible = true;
                    //divMinPurchase.Visible = true;
                    //divAppliedAllProducts.Visible = true;
                    divShareType.Visible = true;
                    divAppliedToCategories.Visible = true;
                    divAppliedToProducts.Visible = true;
                    divExcludedZones.Visible = false;

                    var daCol = gridRelated2.Columns.FindByUniqueName("DiscountAmount");
                    if (daCol != null)
                        daCol.Visible = false;
                    //gridRelated.Columns.FindByUniqueName("DiscountAmount").Visible = false;
                    //gridRelated.Columns.FindByUniqueName("MaximumDiscount").Visible = false;
                    //gridRelated.Columns.FindByUniqueName("GiftHtml").Visible = false;

                    BindPaymentMethod();
                    BindEnum();
                }
                else if (discount.DiscountType == (int)DiscountType.ComboSale)
                {
                    gridRelated.Columns.FindByUniqueName("ComboSaleQty").Visible = true;
                    divAppliedToCategories.Visible = false;
                    divAppliedToProducts.Visible = true;
                }
                else if (discount.DiscountType == (int)DiscountType.Deal)
                {
                    //divMaximumDiscount.Visible = true;
                    gridRelated.Columns.FindByUniqueName("DealDate").Visible = true;
                    gridRelated.Columns.FindByUniqueName("DealQty").Visible = true;
                    gridRelated.Columns.FindByUniqueName("ComboSaleQty").Visible = true;
                    gridRelated.Columns.FindByUniqueName("ComboSaleQty").HeaderText = "SL tối đa";
                    divAppliedToProducts.Visible = true;
                }
                else if (HasDiscountType(type, DiscountType.Coupon))
                {
                    divAppliedToCategories.Visible = true;
                    divAppliedToProducts.Visible = true;
                    divMinPurchase.Visible = true;
                    divDiscountAmount.Visible = true;
                    divMaximumDiscount.Visible = true;
                    liTabCoupon.Visible = true;
                    tabCoupon.Visible = true;
                    divShareType.Visible = true;

                    var li = chkListShareType.Items.FindByValue("1");
                    if (li != null)
                        li.Text = "Sử dụng Mã KM này chung với KM khác";
                }

                btnAll.Visible = divAppliedToProducts.Visible && AllowedAddAll();
                //btnAll2.Visible = divAppliedToCategories.Visible && AllowedAddAll();

                var displayOrderColumn = gridRelated.Columns.FindByUniqueName("DisplayOrder");
                if (displayOrderColumn != null && ConfigHelper.GetBoolProperty("PromotionSortByItem", false))
                    displayOrderColumn.Visible = true;
            }
        }

        //private void ddlAppliedAllProducts_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    PopulateAppliedType();

        //    if (ddlAppliedAllProducts.SelectedValue == "false")
        //    {
        //        gridRelated.Rebind();
        //        gridRelated2.Rebind();
        //    }
        //}

        //private void PopulateAppliedType()
        //{
        //    divAppliedToProducts.Visible = false;
        //    divAppliedToCategories.Visible = false;

        //    if (ddlAppliedAllProducts.SelectedValue == "false")
        //    {
        //        divAppliedToProducts.Visible = true;
        //        divAppliedToCategories.Visible = true;
        //    }
        //}

        private bool HasDiscountType(int type1, DiscountType type2)
        {
            return type1 == (int)type2;
        }

        #region Payment method

        private void BindPaymentMethod()
        {
            var lstPaymentMethods = PaymentMethod.GetByActive(siteSettings.SiteId, 1);
            cobPaymentMethod.DataSource = lstPaymentMethods;
            cobPaymentMethod.DataBind();
        }

        private void BindEnum()
        {
            try
            {
                chlShowOption.DataValueField = "Value";
                chlShowOption.DataTextField = "Name";
                chlShowOption.DataSource = EnumDefined.LoadFromConfigurationXml("product", "discountoption", "value");
                chlShowOption.DataBind();
                if (chlShowOption.Items.Count > 0)
                    divShowOption.Visible = true;
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion Payment method

        #region Discount range

        private void gridOrderDiscountRange_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (discount != null && HasDiscountType(discount.DiscountType, DiscountType.ByOrderRange))
            {
                lstDiscountRange = DiscountRanges;

                if (lstDiscountRange == null)
                    lstDiscountRange = DiscountRange.GetRange(discountID);
                DiscountRanges = lstDiscountRange;

                gridOrderDiscountRange.DataSource = lstDiscountRange;
            }
        }

        private void gridOrderDiscountRange_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                DropDownList ddlDiscountType = (DropDownList)item.FindControl("ddlDiscountType");
                var dt = item.GetDataKeyValue("DiscountType").ToString();
                if (dt == "-1")
                    dt = "1";
                ddlDiscountType.SelectedValue = dt;
            }
        }

        private void gridOrderDiscountRange_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                switch (e.CommandName)
                {
                    case "Delete":
                        SaveTempDiscountRange(gridOrderDiscountRange);
                        lstDiscountRange = DiscountRanges;
                        var itemID = Int32.Parse(e.CommandArgument.ToString());
                        lstDiscountRange.RemoveAll(x => x.ItemID == itemID);
                        DiscountRanges = lstDiscountRange;
                        gridOrderDiscountRange.Rebind();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private void btnOrderRangeNewRow_Click(object sender, EventArgs e)
        {
            if (DiscountRanges == null)
                DiscountRanges = new List<DiscountRange>();
            else
                SaveTempDiscountRange(gridOrderDiscountRange);

            lstDiscountRange = DiscountRanges;

            DiscountRange newRange = new DiscountRange
            {
                DiscountID = discountID,
                ItemID = GenTempDiscountID(lstDiscountRange)
            };
            lstDiscountRange.Add(newRange);

            DiscountRanges = lstDiscountRange;
            gridOrderDiscountRange.Rebind();
        }

        private List<DiscountRange> DiscountRanges
        {
            get
            {
                if (ViewState["DiscountRanges"] != null)
                    return ViewState["DiscountRanges"] as List<DiscountRange>;
                return null;
            }

            set
            {
                ViewState["DiscountRanges"] = value;
            }
        }

        private void SaveTempDiscountRange(Telerik.Web.UI.RadGrid grid)
        {
            lstDiscountRange = DiscountRanges;
            foreach (GridDataItem data in grid.Items)
            {
                var txtFromPrice = (TextBox)data.FindControl("txtFromPrice");
                var txtToPrice = (TextBox)data.FindControl("txtToPrice");
                var txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                var txtMaximumDiscount = (TextBox)data.FindControl("txtMaximumDiscount");
                var ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                int itemID = Convert.ToInt32(data.GetDataKeyValue("ItemID"));
                decimal fromPrice = Decimal.TryParse(txtFromPrice.Text.Trim(), out fromPrice) ? fromPrice : 0;
                decimal toPrice = Decimal.TryParse(txtToPrice.Text.Trim(), out toPrice) ? toPrice : 0;
                decimal discountAmount = Decimal.TryParse(txtDiscountAmount.Text.Trim(), out discountAmount) ? discountAmount : 0;
                decimal maximumDiscount = Decimal.TryParse(txtMaximumDiscount.Text.Trim(), out maximumDiscount) ? maximumDiscount : 0;
                int discountType = Int32.Parse(ddlDiscountType.SelectedValue);

                var discountRange = lstDiscountRange.Where(x => x.ItemID == itemID).FirstOrDefault();
                if (discountRange != null)
                {
                    discountRange.FromPrice = fromPrice;
                    discountRange.ToPrice = toPrice;
                    discountRange.DiscountAmount = discountAmount;
                    discountRange.DiscountType = discountType;
                    discountRange.MaximumDiscount = maximumDiscount;
                }
            }
            DiscountRanges = lstDiscountRange;
        }

        private int GenTempDiscountID(List<DiscountRange> discountRanges)
        {
            int itemID = 0;

            var minItem = discountRanges.Where(x => x.ItemID < 0).OrderBy(x => x.ItemID).FirstOrDefault();
            if (minItem != null)
                itemID = minItem.ItemID - 1;
            else
                itemID = -(discountRanges.Count + 1);

            return itemID;
        }

        private void UpdateDiscountRange(Telerik.Web.UI.RadGrid grid)
        {
            lstDiscountRange = DiscountRanges;

            if (discount == null || discount.DiscountType != (int)DiscountType.ByOrderRange) return;
            if (lstDiscountRange == null) return;

            //If existed discount range is deleted, also delete from database
            var lstOriginalDiscountRanges = DiscountRange.GetRange(discountID);
            foreach (DiscountRange originalDiscountRange in lstOriginalDiscountRanges)
            {
                if (!lstDiscountRange.Exists(x => x.ItemID == originalDiscountRange.ItemID))
                    DiscountRange.Delete(originalDiscountRange.ItemID);
            }

            //Begin update
            foreach (GridDataItem data in grid.Items)
            {
                var txtFromPrice = (TextBox)data.FindControl("txtFromPrice");
                var txtToPrice = (TextBox)data.FindControl("txtToPrice");
                var txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                var txtMaximumDiscount = (TextBox)data.FindControl("txtMaximumDiscount");
                var ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                int itemID = Convert.ToInt32(data.GetDataKeyValue("ItemID"));
                decimal fromPrice = Decimal.TryParse(txtFromPrice.Text.Trim(), out fromPrice) ? fromPrice : 0;
                decimal toPrice = Decimal.TryParse(txtToPrice.Text.Trim(), out toPrice) ? toPrice : 0;
                decimal discountAmount = Decimal.TryParse(txtDiscountAmount.Text.Trim(), out discountAmount) ? discountAmount : 0;
                decimal maximumDiscount = Decimal.TryParse(txtMaximumDiscount.Text.Trim(), out maximumDiscount) ? maximumDiscount : 0;
                int discountType = Int32.Parse(ddlDiscountType.SelectedValue);

                if (fromPrice != Convert.ToDecimal(data.GetDataKeyValue("FromPrice"))
                   || toPrice != Convert.ToDecimal(data.GetDataKeyValue("ToPrice"))
                   || discountAmount != Convert.ToDecimal(data.GetDataKeyValue("DiscountAmount"))
                   || maximumDiscount != Convert.ToDecimal(data.GetDataKeyValue("MaximumDiscount"))
                   || discountType != Convert.ToInt32(data.GetDataKeyValue("DiscountType"))
                //&& !(fromPrice == 0 && toPrice == 0 && discountAmount == 0)
                )
                {
                    DiscountRange discountRange = null;
                    if (itemID > 0)
                        discountRange = new DiscountRange(itemID);
                    else
                        discountRange = new DiscountRange();

                    discountRange.DiscountID = discountID;
                    discountRange.FromPrice = fromPrice;
                    discountRange.ToPrice = toPrice;
                    discountRange.DiscountAmount = discountAmount;
                    discountRange.MaximumDiscount = maximumDiscount;
                    discountRange.DiscountType = discountType;
                    discountRange.Save();
                }
            }
            DiscountRanges = null;
            grid.Rebind();
        }

        //public bool SelectedDiscountType(string eval, string value)
        //{
        //    if (Int32.Parse(eval) < 1 && value == "1")
        //        return true;
        //    if (eval == value)
        //        return true;
        //    return false;
        //}

        #endregion Discount range

        #region Language

        protected void tabLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            edBriefContent.Text = string.Empty;
            edFullContent.Text = string.Empty;
            //btnDeleteLanguage.Visible = false;

            if (discount != null && discount.DiscountId > 0)
            {
                if (e.Tab.Index == 0)
                {
                    edBriefContent.Text = discount.BriefContent;
                    edFullContent.Text = discount.FullContent;
                }
                else
                {
                    ContentLanguage content = new ContentLanguage(discount.Guid, Convert.ToInt32(e.Tab.Value));
                    if (content != null && content.Guid != Guid.Empty)
                    {
                        edBriefContent.Text = content.BriefContent;
                        edFullContent.Text = content.FullContent;

                        //btnDeleteLanguage.Visible = canDelete;
                    }
                }
            }

            //upButton.Update();
        }

        private bool IsLanguageTab()
        {
            if (tabLanguage.Visible && tabLanguage.SelectedIndex > 0)
                return true;

            return false;
        }

        private void SaveContentLanguage(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsLanguageTab())
                return;

            int languageID = -1;
            if (tabLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabLanguage.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);

            if (edBriefContent.Text.Length > 0 || edFullContent.Text.Length > 0)
            {
                content.LanguageId = languageID;
                content.ContentGuid = contentGuid;
                content.SiteGuid = siteSettings.SiteGuid;
                content.BriefContent = edBriefContent.Text.Trim();
                content.FullContent = edFullContent.Text.Trim();

                content.Save();
            }
            else if (content != null && content.ContentGuid != Guid.Empty)
                ContentLanguage.Delete(discount.Guid, languageID);
        }

        //protected void btnDeleteLanguage_Click(object sender, EventArgs e)
        //{
        //    if (!IsLanguageTab())
        //        return;

        //    if (tabLanguage.SelectedIndex > 0)
        //    {
        //        int languageId = Convert.ToInt32(tabLanguage.SelectedTab.Value);

        //        if (languageId > 0 && discount != null && discount.Guid != Guid.Empty)
        //        {
        //            ContentLanguage.Delete(discount.Guid, languageId);
        //            message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

        //            WebUtils.SetupRedirect(this, Request.RawUrl);
        //        }
        //    }
        //}

        #endregion Language

        #region Discount content

        private bool canUpdate = true;
        private bool canDelete = true;

        private string selAttribute = string.Empty;

        private void BindAttribute()
        {
            btnAttributeUp.Visible = false;
            btnAttributeDown.Visible = false;
            btnDeleteAttribute.Visible = false;

            if (discount != null)
            {
                if (!Page.IsPostBack)
                {
                    gbSiteMapProvider.PopulateListControl(cobAttribteZones, false, Product.FeatureGuid);

                    if (discount != null && discount.DiscountId > 0)
                    {
                        ddlLoadType.DataValueField = "Value";
                        ddlLoadType.DataTextField = "Name";
                        ddlLoadType.DataSource = EnumDefined.LoadFromConfigurationXml("product", "discountloadtype", "value");
                        ddlLoadType.DataBind();
                        if (ddlLoadType.Items.Count > 0)
                            divLoadType.Visible = true;
                    }
                }

                lbAttribute.Items.Clear();
                lbAttribute.Items.Add(new ListItem(ProductResources.AttributeNewLabel, ""));
                lbAttribute.DataSource = DiscountContent.GetByDiscount(discount.DiscountId);
                lbAttribute.DataBind();

                var li = lbAttribute.Items.FindByValue(selAttribute);
                if (li != null)
                {
                    lbAttribute.ClearSelection();
                    li.Selected = true;
                }

                LanguageHelper.PopulateTab(tabAttributeLanguage, false);
            }
        }

        private void MoveUpDown(string direction)
        {
            if (discount == null || discount.Guid == Guid.Empty)
                return;

            var listAttribute = DiscountContent.GetByDiscount(discount.DiscountId);
            var attribute = (DiscountContent)null;
            if (lbAttribute.SelectedIndex > 0)
            {
                int delta;

                if (direction == "down")
                    delta = 3;
                else
                    delta = -3;

                attribute = listAttribute[lbAttribute.SelectedIndex - 1];
                attribute.DisplayOrder += delta;

                DiscountContent.ResortContent(listAttribute);

                selAttribute = attribute.ContentId.ToString();

                BindAttribute();
                PopulateAttributeControls();
            }
        }

        private void btnUpDown_Click(Object sender, EventArgs e)
        {
            string direction = ((LinkButton)sender).CommandName;
            MoveUpDown(direction);
        }

        private void lbAttribute_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateAttributeControls();
        }

        private void btnAttributeUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canUpdate)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                if (discount != null && discount.DiscountId > 0 && lbAttribute.SelectedIndex > -1)
                {
                    var attribute = (DiscountContent)null;

                    if (lbAttribute.SelectedValue.Length > 0)
                        attribute = new DiscountContent(Convert.ToInt32(lbAttribute.SelectedValue));

                    bool isUpdate = true;
                    if (attribute == null || attribute.Guid == Guid.Empty)
                    {
                        attribute = new DiscountContent
                        {
                            DiscountId = discount.DiscountId,
                            DisplayOrder = DiscountContent.GetNextSortOrder(discount.DiscountId)
                        };

                        isUpdate = false;
                    }

                    attribute.ZoneIDs = string.Empty;
                    var sepa = string.Empty;
                    foreach (ListItem li in cobAttribteZones.Items)
                    {
                        if (li.Selected)
                        {
                            attribute.ZoneIDs += sepa + li.Value;
                            sepa = ";";
                        }
                    }

                    attribute.LoadType = Convert.ToInt32(ddlLoadType.SelectedValue);

                    if (!string.IsNullOrEmpty(attribute.BannerFile) && chkDeleteAttributeBannerFile.Checked)
                    {
                        var newImagePath = VirtualPathUtility.Combine(imageFolderPath, attribute.BannerFile);
                        PromotionsHelper.DeleteImage(newImagePath);
                        attribute.BannerFile = string.Empty;
                    }

                    if (fupAttributeBannerFile.UploadedFiles.Count > 0)
                    {
                        var file = fupAttributeBannerFile.UploadedFiles[0];
                        var ext = file.GetExtension();

                        if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                        {
                            PromotionsHelper.VerifyPromotionFolders(imageFolderPath);

                            var newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                            var newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (attribute.BannerFile == newFileName)
                            {
                                PromotionsHelper.DeleteImage(newImagePath);
                            }
                            else
                            {
                                int i = 1;
                                while (File.Exists(Server.MapPath(VirtualPathUtility.Combine(imageFolderPath, newFileName))))
                                {
                                    newFileName = i.ToInvariantString() + newFileName;
                                    i += 1;
                                }
                            }

                            newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                            if (!string.IsNullOrEmpty(attribute.BannerFile))
                            {
                                var imageVirtualPath = imageFolderPath + attribute.BannerFile;
                                PromotionsHelper.DeleteImage(imageVirtualPath);
                            }

                            file.SaveAs(Server.MapPath(newImagePath));

                            attribute.BannerFile = newFileName;
                        }
                    }

                    if (!IsAttributeLanguageTab())
                    {
                        attribute.Title = txtAttributeTitle.Text;
                        attribute.Description = edAttributeContent.Text;
                    }

                    if (attribute.Save())
                        SaveAttributeContentLanguage(attribute.Guid);

                    LogActivity.Write("Update discount content", txtAttributeTitle.Text);

                    selAttribute = attribute.ContentId.ToString();

                    BindAttribute();
                    PopulateAttributeControls();

                    //product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);
                    //SiteUtils.QueueIndexing();

                    if (isUpdate)
                        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "UpdateSuccessMessage");
                    else
                        message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "InsertSuccessMessage");
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        private void btnDeleteAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                if (!canDelete)
                {
                    SiteUtils.RedirectToEditAccessDeniedPage();
                    return;
                }

                if (discount != null && lbAttribute.SelectedValue.Length > 0)
                {
                    var contentId = Convert.ToInt32(lbAttribute.SelectedValue);
                    if (contentId > 0)
                    {
                        var content = new DiscountContent(contentId);
                        if (content != null && content.ContentId > 0)
                        {
                            //DiscountContent.DeleteByDiscount(discount.DiscountId);
                            //ContentAttribute.Delete(content.Guid);
                            ContentLanguage.DeleteByContent(content.Guid);
                            DiscountContent.Delete(content.ContentId);
                            LogActivity.Write("Delete discount content", lbAttribute.SelectedItem.Text);
                        }
                    }

                    selAttribute = lbAttribute.Items[lbAttribute.SelectedIndex - 1].Value;

                    BindAttribute();
                    PopulateAttributeControls();
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
            }
        }

        protected void btnDeleteAttributeLanguage_Click(object sender, EventArgs e)
        {
            if (!IsAttributeLanguageTab())
                return;

            if (tabAttributeLanguage.SelectedIndex > 0 && lbAttribute.SelectedValue.Length == 36)
            {
                int languageId = Convert.ToInt32(tabAttributeLanguage.SelectedTab.Value);

                if (languageId > 0)
                {
                    ContentLanguage.Delete(new Guid(lbAttribute.SelectedValue), languageId);
                    message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");

                    PopulateAttributeControls();

                    //product.ContentChanged += new ContentChangedEventHandler(product_ContentChanged);
                    //SiteUtils.QueueIndexing();
                }
            }
        }

        protected void tabAttributeLanguage_TabClick(object sender, Telerik.Web.UI.RadTabStripEventArgs e)
        {
            PopulateAttributeControls();
        }

        private void PopulateAttributeControls()
        {
            btnAttributeUp.Visible = false;
            btnAttributeDown.Visible = false;

            if (lbAttribute.SelectedValue.Length > 0)
            {
                if (lbAttribute.Items.Count > 2)
                {
                    if (lbAttribute.SelectedIndex == 1)
                        btnAttributeDown.Visible = canUpdate;
                    else if (lbAttribute.SelectedIndex == (lbAttribute.Items.Count - 1))
                        btnAttributeUp.Visible = canUpdate;
                    else
                    {
                        btnAttributeDown.Visible = canUpdate;
                        btnAttributeUp.Visible = canUpdate;
                    }
                }

                btnDeleteAttribute.Visible = canDelete;
                btnAttributeUpdate.Text = ResourceHelper.GetResourceString("Resource", "UpdateButton");

                if (tabAttributeLanguage.Visible && tabAttributeLanguage.Tabs.Count == 1)
                    LanguageHelper.PopulateTab(tabAttributeLanguage, lbAttribute.SelectedValue.Length > 0);
            }
            else
            {
                btnDeleteAttribute.Visible = false;
                btnAttributeUpdate.Text = ResourceHelper.GetResourceString("Resource", "InsertButton");

                if (tabAttributeLanguage.Visible && tabAttributeLanguage.Tabs.Count != 1)
                    LanguageHelper.PopulateTab(tabAttributeLanguage, lbAttribute.SelectedValue.Length > 0);
            }

            PopulateDataLanguage(tabAttributeLanguage.SelectedTab);
        }

        private void PopulateDataLanguage(Telerik.Web.UI.RadTab tab)
        {
            cobAttribteZones.ClearSelection();
            ddlLoadType.ClearSelection();
            txtAttributeTitle.Text = string.Empty;
            edAttributeContent.Text = string.Empty;
            btnDeleteAttributeLanguage.Visible = false;

            chkDeleteAttributeBannerFile.Visible = false;
            chkDeleteAttributeBannerFile.Checked = false;
            imgAttributeBannerFile.Visible = false;
            imgAttributeBannerFile.Src = "/Data/SiteImages/1x1.gif";

            if (lbAttribute.SelectedValue.Length > 0)
            {
                var attribute = new DiscountContent(Convert.ToInt32(lbAttribute.SelectedValue));

                if (attribute == null || attribute.ContentId <= 0)
                    return;

                cobAttribteZones.ClearSelection();
                var lstZones = attribute.ZoneIDs.SplitOnCharAndTrim(';');
                if (lstZones.Count > 0)
                {
                    foreach (ListItem li in cobAttribteZones.Items)
                    {
                        if (lstZones.Contains(li.Value))
                            li.Selected = true;
                    }
                }

                var liItem = ddlLoadType.Items.FindByValue(attribute.LoadType.ToString());
                if (liItem != null)
                    liItem.Selected = true;

                if (!string.IsNullOrEmpty(attribute.BannerFile))
                {
                    chkDeleteAttributeBannerFile.Visible = true;
                    imgAttributeBannerFile.Visible = true;
                    imgAttributeBannerFile.Src = imageFolderPath + attribute.BannerFile;
                }

                if (IsAttributeLanguageTab())
                {
                    var content = new ContentLanguage(attribute.Guid, Convert.ToInt32(tab.Value));
                    if (content != null && content.Guid != Guid.Empty)
                    {
                        txtAttributeTitle.Text = content.Title;
                        edAttributeContent.Text = content.FullContent;
                        btnDeleteAttributeLanguage.Visible = true;
                    }
                }
                else
                {
                    txtAttributeTitle.Text = attribute.Title;
                    edAttributeContent.Text = attribute.Description;
                }
            }
        }

        private bool IsAttributeLanguageTab()
        {
            if (tabAttributeLanguage.Visible && tabAttributeLanguage.SelectedIndex > 0)
                return true;

            return false;
        }

        private void SaveAttributeContentLanguage(Guid contentGuid)
        {
            if (contentGuid == Guid.Empty || !IsAttributeLanguageTab())
                return;

            int languageID = -1;
            if (tabAttributeLanguage.SelectedIndex > 0)
                languageID = Convert.ToInt32(tabAttributeLanguage.SelectedTab.Value);

            if (languageID == -1)
                return;

            var content = new ContentLanguage(contentGuid, languageID);

            if (txtAttributeTitle.Text.Length > 0 || edAttributeContent.Text.Length > 0)
            {
                content.LanguageId = languageID;
                content.ContentGuid = contentGuid;
                content.SiteGuid = siteSettings.SiteGuid;
                content.Title = txtAttributeTitle.Text.Trim();
                content.FullContent = edAttributeContent.Text;
                content.Save();
            }
        }

        #endregion Discount content

        #region Coupon

        private void btnDeleteCoupon_Click(object sender, EventArgs e)
        {
            int iRecordDeleted = 0;
            foreach (Telerik.Web.UI.GridDataItem data in gridCoupon.SelectedItems)
            {
                var guid = new Guid(data.GetDataKeyValue("Guid").ToString());

                var coupon = new DiscountCoupon(guid);
                if (coupon != null && coupon.Guid != Guid.Empty)
                {
                    DiscountCoupon.Delete(guid);
                    LogActivity.Write("Delete coupon", coupon.CouponCode);

                    iRecordDeleted += 1;
                }
            }

            if (iRecordDeleted > 0)
            {
                message.SuccessMessage = ResourceHelper.GetResourceString("Resource", "DeleteSuccessMessage");
                gridCoupon.Rebind();
            }
        }

        private void btnInsertCoupon_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            var lstCouponCodes = txtCouponCode.Text.Trim().SplitOnCharAndTrim('\n');

            var limitationTimes = 0;
            int.TryParse(txtLimitationTimes2.Text, out limitationTimes);

            foreach (var couponCode in lstCouponCodes)
            {
                var coupon = DiscountCoupon.GetByCode(couponCode);
                if (coupon != null && coupon.Guid != Guid.Empty)
                    continue;

                coupon = new DiscountCoupon
                {
                    CouponCode = couponCode,
                    DiscountID = discount.DiscountId,
                    LimitationTimes = limitationTimes
                };
                coupon.Save();
            }

            gridCoupon.Rebind();
        }

        private void btnCouponGenerate_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid) return;

            int.TryParse(txtCouponLength.Text, out int couponLength);
            int.TryParse(txtCouponCount.Text, out int couponCount);

            if (couponLength <= 0 || couponCount <= 0) return;

            for (var i = 1; i <= couponCount; i++)
            {
                var couponCode = string.Empty;
                do
                {
                    couponCode = txtCouponPrefix.Text + SiteUser.CreateRandomPassword(couponLength, "ABCDEFGHJKLMNPQRSTWXYZ123456789");
                } while (DiscountCoupon.ExistCode(couponCode));

                var coupon = DiscountCoupon.GetByCode(couponCode);
                if (coupon != null && coupon.Guid != Guid.Empty)
                    return;

                var limitationTimes = 0;
                int.TryParse(txtLimitationTimes.Text, out limitationTimes);

                coupon = new DiscountCoupon
                {
                    CouponCode = couponCode,
                    DiscountID = discount.DiscountId,
                    LimitationTimes = limitationTimes
                };
                coupon.Save();
            }

            gridCoupon.Rebind();
        }

        private void gridCoupon_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (discount == null || discount.DiscountId <= 0) return;

            var isApplied = false;
            var iCount = DiscountCoupon.GetCount(discount.DiscountId, null);

            var startRowIndex = isApplied ? 1 : gridCoupon.CurrentPageIndex + 1;
            var maximumRows = isApplied ? iCount : gridCoupon.PageSize;

            gridCoupon.VirtualItemCount = iCount;
            gridCoupon.AllowCustomPaging = !isApplied;
            gridCoupon.PagerStyle.EnableSEOPaging = false;

            var lst = DiscountCoupon.GetPage(discount.DiscountId, null, startRowIndex, maximumRows);
            gridCoupon.DataSource = lst;

            if (lst.Count > 0) btnDeleteCoupon.Visible = true;
        }

        #endregion Coupon
    }
}
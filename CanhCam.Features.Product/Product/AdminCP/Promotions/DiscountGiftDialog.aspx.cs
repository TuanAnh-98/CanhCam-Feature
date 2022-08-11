/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:			    2015-08-11
/// Last Modified:		    2015-08-11

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class DiscountGiftDialog : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountGiftDialog));
        private DiscountAppliedToItem appliedItem;
        private DiscountRange discountRange;
        private int discountId = -1;
        private string imageFolderPath;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            if (!WebUser.IsAdmin)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            LoadSettings();
            PopulateLabels();

            if (appliedItem == null && discountRange == null)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            if (!Page.IsPostBack)
                PopulateZoneList();

            if (appliedItem != null)
                discountId = appliedItem.DiscountId;
            else
                discountId = discountRange.DiscountID;

            if (discountId > 0)
                imageFolderPath = PromotionsHelper.ImagePath(siteSettings.SiteId, discountId);
        }

        private void PopulateZoneList()
        {
            gbSiteMapProvider.PopulateListControl(ddlZones, false, Product.FeatureGuid);

            if (WebUser.IsAdminOrContentAdmin || SiteUtils.UserIsSiteEditor())
                ddlZones.Items.Insert(0, new ListItem(ResourceHelper.GetResourceString("Resource", "All"), "-1"));
        }

        private void LoadSettings()
        {
            siteSettings = CacheHelper.GetCurrentSiteSettings();

            var appliedGuid = WebUtils.ParseGuidFromQueryString("Guid", Guid.Empty);
            var discountRangeId = WebUtils.ParseInt32FromQueryString("ItemID", -1);

            if (appliedGuid != Guid.Empty)
            {
                appliedItem = new DiscountAppliedToItem(appliedGuid);
                if (appliedItem == null || appliedItem.Guid == Guid.Empty)
                    appliedItem = null;
            }
            else if (discountRangeId > 0)
            {
                discountRange = new DiscountRange(discountRangeId);
                if (discountRange == null || discountRange.ItemID <= 0)
                    discountRange = null;
            }
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
            UIHelper.AddConfirmationDialog(btnDeleteGift, "Bạn có chắn chắn muốn xóa Quà tặng?");
        }

        protected string GetImageFilePath(string imageFile)
        {
            if (string.IsNullOrEmpty(imageFile)) return string.Empty;

            return PromotionsHelper.ImagePath(siteSettings.SiteId, discountId) + imageFile;
        }

        #region Applied

        private string sZoneId
        {
            get
            {
                if (ddlZones.SelectedValue.Length > 0)
                {
                    if (ddlZones.SelectedValue == "-1")
                        return string.Empty;

                    return ddlZones.SelectedValue;
                }

                return "0";
            }
        }

        private List<DiscountGift> GetGiftByDiscount()
        {
            return DiscountGift.GetByDiscount(discountId, appliedItemGuid: (appliedItem != null ? (Guid?)appliedItem.Guid : null), discountRangeID: (discountRange != null ? discountRange.ItemID : -1));
        }

        private void gridGifts_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            if (discountId > 0)
            {
                var lstGifts = GetGiftByDiscount();
                gridGifts.DataSource = lstGifts;

                btnDeleteGift.Visible = lstGifts.Count > 0;
            }
            else
                gridGifts.DataSource = new List<DiscountGift>();
        }

        private void grid_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
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

        private void btnInsertFromSystem_Click(object sender, EventArgs e)
        {
            var rebind = false;

            foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
            {
                var productId = Convert.ToInt32(data.GetDataKeyValue("ProductId"));
                var product = Product.GetTopAdv(top: 1, siteId: siteSettings.SiteId, productIds: productId.ToString()).FirstOrDefault(); // new Product(siteSettings.SiteId, productId)
                if (product != null && product.ProductId > 0)
                {
                    var lstDiscountGifts = GetGiftByDiscount();
                    var findGift = lstDiscountGifts.Where(s => s.ProductID == product.ProductId).FirstOrDefault();

                    if (findGift != null)
                    {
                        //findGift.Quantity += 1;
                        //findGift.Save();
                        //rebind = true;
                    }
                    else
                    {
                        var gift = new DiscountGift
                        {
                            DiscountID = discountId,
                            ProductID = product.ProductId,
                            Name = product.Title,
                            Url = ProductHelper.FormatProductUrl(product.Url, product.ProductId, product.ZoneId)
                        };

                        int.TryParse(txtGiftQuantity.Text, out int quantity);
                        gift.Quantity = quantity;

                        if (appliedItem != null)
                            gift.AppliedItemGuid = appliedItem.Guid;
                        else
                            gift.DiscountRangeID = discountRange.ItemID;

                        if (!string.IsNullOrEmpty(product.ImageFile))
                        {
                            var vp = ProductHelper.GetImageFilePath(siteSettings.SiteId, product.ProductId, product.ImageFile, string.Empty);
                            if (!string.IsNullOrEmpty(vp))
                            {
                                var path = Server.MapPath(vp);
                                if (File.Exists(path))
                                {
                                    PromotionsHelper.VerifyPromotionFolders(imageFolderPath);

                                    var newFileName = product.ImageFile.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                                    var newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                                    if (gift.ImageFile == newFileName)
                                        PromotionsHelper.DeleteImage(newImagePath);
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

                                    if (!string.IsNullOrEmpty(gift.ImageFile))
                                    {
                                        var imageVirtualPath = imageFolderPath + gift.ImageFile;
                                        PromotionsHelper.DeleteImage(imageVirtualPath);
                                    }

                                    File.Copy(path, Server.MapPath(newImagePath));

                                    gift.ImageFile = newFileName;
                                }
                            }
                        }

                        gift.Save();
                    }

                    rebind = true;
                }
            }

            if (rebind)
            {
                GiftHtmlBuilder();
                gridGifts.Rebind();
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (discountId <= 0) return;

                var gift = new DiscountGift
                {
                    DiscountID = discountId,
                    Name = txtGiftName.Text.Trim()
                };

                int.TryParse(txtGiftQuantity.Text, out int quantity);
                gift.Quantity = quantity;

                gift.GiftCount = 1;

                if (appliedItem != null)
                    gift.AppliedItemGuid = appliedItem.Guid;
                else
                    gift.DiscountRangeID = discountRange.ItemID;

                //Save Image
                if (fupImageFile.UploadedFiles.Count > 0)
                {
                    var file = fupImageFile.UploadedFiles[0];
                    var ext = file.GetExtension();

                    if (SiteUtils.IsAllowedUploadBrowseFile(ext, WebConfigSettings.ImageFileExtensions))
                    {
                        PromotionsHelper.VerifyPromotionFolders(imageFolderPath);

                        var newFileName = file.FileName.ToCleanFileName(WebConfigSettings.ForceLowerCaseForUploadedFiles);
                        var newImagePath = VirtualPathUtility.Combine(imageFolderPath, newFileName);

                        if (gift.ImageFile == newFileName)
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

                        if (!string.IsNullOrEmpty(gift.ImageFile))
                        {
                            var imageVirtualPath = imageFolderPath + gift.ImageFile;
                            PromotionsHelper.DeleteImage(imageVirtualPath);
                        }

                        file.SaveAs(Server.MapPath(newImagePath));

                        gift.ImageFile = newFileName;
                    }
                }

                gift.Save();
                gridGifts.Rebind();

                GiftHtmlBuilder();
            }
        }

        private void GiftHtmlBuilder()
        {
            var lstGifts = new List<DiscountGift>();
            if (appliedItem != null)
                lstGifts = DiscountGift.GetByDiscount(discountId, appliedItemGuid: appliedItem.Guid);
            else
                lstGifts = DiscountGift.GetByDiscount(discountId, discountRangeID: discountRange.ItemID);

            var results = string.Empty;
            var results2 = string.Empty;
            var productGifts = string.Empty;
            var productGiftsSepa = string.Empty;
            if (lstGifts.Count > 0)
            {
                var doc = new XmlDocument
                {
                    XmlResolver = null
                };
                doc.XmlResolver = null;
                doc.LoadXml("<GiftList></GiftList>");
                var root = doc.DocumentElement;

                try
                {
                    foreach (var item in lstGifts)
                    {
                        var giftXml = doc.CreateElement("Gift");
                        root.AppendChild(giftXml);

                        XmlHelper.AddNode(doc, giftXml, "Title", item.Name);
                        XmlHelper.AddNode(doc, giftXml, "Quantity", item.Quantity.ToString());
                        XmlHelper.AddNode(doc, giftXml, "Url", item.Url);
                        XmlHelper.AddNode(doc, giftXml, "ProductId", item.ProductID.ToString());
                        if (!string.IsNullOrEmpty(item.ImageFile))
                            XmlHelper.AddNode(doc, giftXml, "ImageUrl", PromotionsHelper.ImagePath(siteSettings.SiteId, discountId) + item.ImageFile);

                        if (item.ProductID > 0)
                        {
                            productGifts += productGiftsSepa + string.Format("{0}x{1}", item.ProductID, item.Quantity);
                            productGiftsSepa = "+";
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                }

                if (File.Exists(Server.MapPath(SiteUtils.GetXsltBasePath("product", "Core_DiscountGiftBuilder.xslt"))))
                    results = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", "Core_DiscountGiftBuilder.xslt"), doc);
                if (File.Exists(Server.MapPath(SiteUtils.GetXsltBasePath("product", "Core_DiscountGiftBuilder2.xslt"))))
                    results2 = XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", "Core_DiscountGiftBuilder2.xslt"), doc);
            }

            if (appliedItem != null)
            {
                appliedItem.ProductGifts = productGifts;
                appliedItem.GiftHtml = lstGifts.Count > 0 ? results : string.Empty;
                appliedItem.GiftDescription = lstGifts.Count > 0 ? results2 : string.Empty;
                appliedItem.Save();
            }
            else
            {
                discountRange.ProductGifts = productGifts;
                discountRange.GiftHtml = lstGifts.Count > 0 ? results : string.Empty;
                discountRange.Save();
            }
        }

        private void btnDeleteGift_Click(object sender, EventArgs e)
        {
            int iRecordDeleted = 0;
            foreach (Telerik.Web.UI.GridDataItem data in gridGifts.SelectedItems)
            {
                var itemId = Convert.ToInt32(data.GetDataKeyValue("GiftID"));
                var gift = new DiscountGift(itemId);

                if (gift != null && gift.GiftID > 0)
                {
                    if (gift.ProductID < 0 && !string.IsNullOrEmpty(gift.ImageFile))
                        PromotionsHelper.DeleteImage(imageFolderPath + gift.ImageFile);

                    if (DiscountGift.Delete(itemId))
                        iRecordDeleted += 1;
                }
            }

            if (iRecordDeleted > 0)
            {
                GiftHtmlBuilder();
                gridGifts.Rebind();
            }
        }

        #endregion Applied

        //private DiscountGift FindDiscountGiftByProductId(List<DiscountGift> lstDiscountGifts, int productID)
        //{
        //    foreach (var gift in lstDiscountGifts)
        //    {
        //        if (gift.ProductID == productID)
        //            return gift;
        //    }

        //    return null;
        //}

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            //ddlRuleType.SelectedIndexChanged += new EventHandler(ddlRuleType_SelectedIndexChanged);

            LoadSettings();
            PopulateLabels();

            this.gridGifts.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(gridGifts_NeedDataSource);
            this.grid.NeedDataSource += grid_NeedDataSource;

            this.btnSearch.Click += new EventHandler(btnSearch_Click);
            this.btnDeleteGift.Click += new EventHandler(btnDeleteGift_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            //this.btnLoadFromSystem.Click += new EventHandler(btnLoadFromSystem_Click);
            this.btnInsertFromSystem.Click += new EventHandler(btnInsertFromSystem_Click);
        }

        #region TreeList

        //private int productId = 0;
        //private void ParentIDCondition()
        //{
        //    if (ddlZones.SelectedValue != "-1"
        //        || txtTitle.Text.Length > 0)
        //        parentID = -1;
        //}

        //protected void treeList_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        //{
        //    bindList(RadDataPager1.CurrentPageIndex + 1);
        //}

        //private void bindList(int pageNumber)
        //{
        //    ParentIDCondition();

        //    if (!Page.IsPostBack)
        //        treeList.DataSource = new List<Product>();
        //    else
        //    {
        //        var lstProduct = Product.GetPageBySearch(pageNumber: pageNumber, pageSize: RadDataPager1.PageSize, siteId: siteSettings.SiteId, zoneIds: sZoneId,
        //            keyword: txtTitle.Text.Trim(), searchProductZone: displaySettings.ShowMultipleZones, parentId: parentID);
        //        treeList.DataSource = lstProduct;
        //    }
        //}

        //private int TotalRowCount
        //{
        //    get
        //    {
        //        if (ViewState["TotalRowCount"] != null)
        //            return (int)ViewState["TotalRowCount"];
        //        var iCount = Product.GetCountBySearch(siteId: siteSettings.SiteId, zoneIds: sZoneId, parentId: 0, keyword: txtTitle.Text.Trim(), searchProductZone: displaySettings.ShowMultipleZones);
        //        ViewState["TotalRowCount"] = iCount;
        //        return iCount;
        //    }
        //    set
        //    {
        //        ViewState["TotalRowCount"] = value;
        //    }
        //}
        //protected void RadDataPager1_TotalRowCountRequest(object sender, RadDataPagerTotalRowCountRequestEventArgs e)
        //{
        //    e.TotalRowCount = TotalRowCount;
        //}
        //protected void RadDataPager1_PageIndexChanged(object sender, RadDataPagerPageIndexChangeEventArgs e)
        //{
        //    bindList(e.NewPageIndex + 1);
        //}

        //protected void treeList_ChildItemsDataBind(object sender, TreeListChildItemsDataBindEventArgs e)
        //{
        //    var id = Convert.ToInt32(e.ParentDataKeyValues["ProductId"].ToString());
        //    var lstChilds = Product.GetPageBySearch(siteId: siteSettings.SiteId, parentId: id, searchProductZone: displaySettings.ShowMultipleZones);

        //    e.ChildItemsDataSource = lstChilds;
        //}

        //protected void treeList_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        //{
        //    if (e.Item is TreeListDataItem)
        //    {
        //        var item = (TreeListDataItem)e.Item;
        //        var parentId = Convert.ToInt32(item.GetParentDataKeyValue("ParentId"));

        //        if (parentId > 0 || parentID == -1)
        //        {
        //            var expandButton = item.FindControl("ExpandCollapseButton");
        //            if (expandButton != null)
        //            {
        //                expandButton.Visible = false;
        //            }
        //        }
        //    }
        //}

        //protected void treeList_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        //{
        //    if (!ProductConfiguration.EnableShoppingCartAttributes)
        //        return;

        //    if (e.Item is TreeListDataItem)
        //    {
        //        var item = (TreeListDataItem)e.Item;
        //        productId = Convert.ToInt32(item.GetDataKeyValue("ProductId"));
        //        if (productId > 0)
        //        {
        //            bool existsChild = Product.ExistsChild(productId);
        //            var expandButton = item.FindControl("ExpandCollapseButton");
        //            if (expandButton != null && !existsChild)
        //            {
        //                expandButton.Visible = false;
        //            }
        //        }
        //    }
        //}

        private void btnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        #endregion TreeList
    }
}
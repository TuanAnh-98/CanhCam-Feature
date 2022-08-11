/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2020-10-30
/// Last Modified:			2020-10-30

using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class PromotionModule : SiteModuleControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PromotionModule));
        private DiscountConfiguration config = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            var doc = GetDataXML();
            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        public XmlDocument GetDataXML()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<PromotionList></PromotionList>");
            var root = doc.DocumentElement;

            try
            {
                XmlHelper.AddNode(doc, root, "ModuleTitle", Title);
                XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);

                if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
                {
                    List<string> lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                    foreach (string item in lstResourceKeys)
                    {
                        XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
                    }
                }

                var pageNumber = WebUtils.ParseInt32FromQueryString("page", 1);
                var pageSize = config.PageSize;

                if (config.LoadType == 0)
                {
                    var lstDiscount = Discount.GetPage(SiteId, -1, -1, 0, -1, -1, -1, -1, pageNumber, pageSize + 1);
                    //var totalRows = Discount.GetCount(SiteId, -1);
                    //var pageUrl = SiteUtils.BuildUrlLeaveOutParam(Request.RawUrl, "pagenumber");
                    //if (pageUrl.Contains("?"))
                    //    pageUrl += "&amp;pagenumber={0}";
                    //else
                    //    pageUrl += "?pagenumber={0}";

                    //pgr.PageURLFormat = pageUrl;
                    //pgr.ShowFirstLast = true;
                    //pgr.PageSize = pageSize;
                    //pgr.ItemCount = totalRows;
                    //pgr.CurrentIndex = pageNumber;
                    //divPager.Visible = (totalRows > pageSize);

                    foreach (var obj in lstDiscount)
                    {
                        var discountXml = doc.CreateElement("Promotion");
                        root.AppendChild(discountXml);

                        XmlHelper.AddNode(doc, discountXml, "PromotionId", obj.DiscountId.ToString());
                        XmlHelper.AddNode(doc, discountXml, "Title", obj.Name);
                        XmlHelper.AddNode(doc, discountXml, "BriefContent", obj.BriefContent);
                        XmlHelper.AddNode(doc, discountXml, "FullContent", obj.FullContent);
                        //XmlHelper.AddNode(doc, discountXml, "EditLink", BuildEditLink(obj.ItemID));

                        var imageFolderPath = PromotionsHelper.ImagePath(siteSettings.SiteId, obj.DiscountId);
                        if (!string.IsNullOrEmpty(obj.ImageFile))
                            XmlHelper.AddNode(doc, discountXml, "ImageUrl", imageFolderPath + obj.ImageFile);
                        if (!string.IsNullOrEmpty(obj.BannerFile))
                            XmlHelper.AddNode(doc, discountXml, "BannerUrl", imageFolderPath + obj.BannerFile);
                        XmlHelper.AddNode(doc, discountXml, "Url", PromotionsHelper.FormatPromotionUrl(obj.Url, obj.DiscountId));

                        var status = 1;
                        if (obj.IsActive)
                        {
                            if (!obj.StartDate.HasValue)
                                obj.StartDate = DateTime.MinValue;
                            if (!obj.EndDate.HasValue)
                                obj.EndDate = DateTime.MaxValue;

                            if (obj.StartDate <= DateTime.Now && DateTime.Now <= obj.EndDate)
                                status = 1;
                            else
                            {
                                if (DateTime.Now < obj.StartDate)
                                    status = 2;
                                if (obj.EndDate < DateTime.Now)
                                    status = -1;
                            }
                        }
                        else
                            status = 0;
                        XmlHelper.AddNode(doc, discountXml, "Status", status.ToString());

                        XmlHelper.AddNode(doc, discountXml, "Type", obj.DiscountType.ToString());
                        if (obj.DiscountType == (int)DiscountType.Deal)
                        {
                            if (obj.StartDate.HasValue)
                                ProductHelper.BuildDealDateXml(doc, discountXml, "DealStart", obj.StartDate.Value);
                            if (obj.EndDate.HasValue)
                                ProductHelper.BuildDealDateXml(doc, discountXml, "DealEnd", obj.EndDate.Value);
                        }
                    }
                }
                else
                {
                    var timeOffset = SiteUtils.GetUserTimeOffset();
                    var timeZone = SiteUtils.GetUserTimeZone();

                    var lstProducts = Product.GetTopAdv(siteId: SiteId, publishStatus: 1, languageId: WorkingCulture.LanguageId, top: pageSize);
                    var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProducts);
                    foreach (Product product in lstProducts)
                    {
                        var productXml = doc.CreateElement("Product");
                        root.AppendChild(productXml);

                        ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset);
                    }

                    XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return doc;
        }

        protected virtual void LoadSettings()
        {
            EnsureConfiguration();

            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }

        private void EnsureConfiguration()
        {
            if (config == null)
                config = new DiscountConfiguration(Settings);
        }
    }
}
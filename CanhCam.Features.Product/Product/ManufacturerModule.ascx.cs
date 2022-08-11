using CanhCam.Business;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: 8a46c6b4-ded9-4dbc-aaac-24ad43350113
    public partial class ManufacturerModule : SiteModuleControl
    {
        private ManufacturerConfiguration config = null;

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
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<ManufacturerList></ManufacturerList>");
            XmlElement root = doc.DocumentElement;

            var rawUrlLeaveOutPageNumber = ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl, false);
            var rawUrlLeaveOutManufacturer = SiteUtils.BuildUrlLeaveOutParam(rawUrlLeaveOutPageNumber, ProductHelper.QueryStringManufacturerParam, false);

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);
            XmlHelper.AddNode(doc, root, "ClearFilterUrl", rawUrlLeaveOutManufacturer);
            XmlHelper.AddNode(doc, root, "ClearAllFilterUrl", WebUtils.GetUrlWithoutQueryString(rawUrlLeaveOutPageNumber));

            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                var lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
            }

            var zoneGuid = (Guid?)null;
            if (config.LoadByZone)
                zoneGuid = CurrentZone.ZoneGuid;
            var lstManufacturers = new List<Manufacturer>();
            if (config.PageSize > 0)
            {
                if (config.ShowPager)
                {
                    var pageNumber = WebUtils.ParseInt32FromQueryString("pagenumber", 1);
                    var totalRows = Manufacturer.GetCount(siteSettings.SiteId, ManufacturerPublishStatus.Published, zoneGuid);
                    lstManufacturers = Manufacturer.GetPage(siteSettings.SiteId, ManufacturerPublishStatus.Published, zoneGuid, pageNumber, config.PageSize);

                    string pageUrl = rawUrlLeaveOutPageNumber;
                    if (pageUrl.Contains("?"))
                        pageUrl += "&amp;pagenumber={0}";
                    else
                        pageUrl += "?pagenumber={0}";

                    pgr.PageURLFormat = pageUrl;
                    pgr.ShowFirstLast = true;
                    pgr.PageSize = config.PageSize;
                    pgr.ItemCount = totalRows;
                    pgr.CurrentIndex = pageNumber;
                    divPager.Visible = (totalRows > config.PageSize);
                }
                else
                    lstManufacturers = Manufacturer.GetPage(siteSettings.SiteId, ManufacturerPublishStatus.Published, zoneGuid, 1, config.PageSize);
            }
            else
                lstManufacturers = Manufacturer.GetAll(siteSettings.SiteId, ManufacturerPublishStatus.Published, zoneGuid, -1);

            if (lstManufacturers.Count > 0)
            {
                //var languageId = WorkingCulture.LanguageId;
                //if (languageId > 0)
                //{
                //    var lstManufacturerGuids = lstManufacturers.Select(s => s.Guid).Distinct().ToList();
                //    var lstContents = ContentLanguage.GetByListContent(string.Join(";", lstManufacturerGuids.ToArray()));
                //}

                var queryString = string.Empty;
                foreach (var m in lstManufacturers)
                {
                    XmlElement xmlElement = doc.CreateElement("Manufacturer");
                    root.AppendChild(xmlElement);

                    XmlHelper.AddNode(doc, xmlElement, "Id", m.ManufacturerId.ToString());
                    XmlHelper.AddNode(doc, xmlElement, "Title", m.Name);
                    XmlHelper.AddNode(doc, xmlElement, "ImageUrl", m.PrimaryImage);
                    XmlHelper.AddNode(doc, xmlElement, "SecondImageUrl", m.SecondImage);
                    XmlHelper.AddNode(doc, xmlElement, "Description", m.Description);
                    XmlHelper.AddNode(doc, xmlElement, "Url", ManufacturerHelper.FormatManufacturerUrl(m.Url, m.ManufacturerId));

                    if (ManufacturerHelper.EnableManufacturerFilterMultipleValues)
                    {
                        XmlHelper.AddNode(doc, xmlElement, "IsActiveFilter", CustomFieldHelper.IsActive(ProductHelper.QueryStringManufacturerParam, m.ManufacturerId.ToString(), (int)CustomFieldFilterType.ByMultipleValues).ToString().ToLower());
                        XmlHelper.AddNode(doc, xmlElement, "UrlFilter", CustomFieldHelper.BuildFilterUrl(rawUrlLeaveOutManufacturer, ProductHelper.QueryStringManufacturerParam, m.ManufacturerId, (int)CustomFieldFilterType.ByMultipleValues, out queryString));
                    }
                    else
                    {
                        XmlHelper.AddNode(doc, xmlElement, "IsActiveFilter", CustomFieldHelper.IsActive(ProductHelper.QueryStringManufacturerParam, m.ManufacturerId.ToString(), (int)CustomFieldFilterType.ByValue).ToString().ToLower());
                        XmlHelper.AddNode(doc, xmlElement, "UrlFilter", CustomFieldHelper.BuildFilterUrl(rawUrlLeaveOutManufacturer, ProductHelper.QueryStringManufacturerParam, m.ManufacturerId, (int)CustomFieldFilterType.ByValue, out queryString));
                    }
                }
            }

            if (ProductHelper.IsAjaxRequest(Request))
            {
                Response.Write(XmlHelper.TransformXML(SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc));
                return;
            }

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
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
                config = new ManufacturerConfiguration(Settings);
        }

        public override bool UserHasPermission()
        {
            if (!Request.IsAuthenticated)
                return false;

            bool hasPermission = false;

            if (ProductPermission.CanManageManufacturer)
            {
                this.LiteralExtraMarkup = "<dd><a class='ActionLink manufacturerlistlink' href='"
                        + SiteRoot
                        + "/Product/AdminCP/Manufacturers.aspx'>" + ProductResources.ManufacturerListLink + "</a></dd>";

                hasPermission = true;
            }

            return hasPermission;
        }
    }
}
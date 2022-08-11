/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-04-08
/// Last Modified:			2014-04-08

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.SearchIndex;
using CanhCam.Web;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading;

namespace CanhCam.Features
{
    public class ProductIndexBuilderProvider : IndexBuilderProvider
    {
        public ProductIndexBuilderProvider()
        { }

        private static readonly ILog log = LogManager.GetLogger(typeof(ProductIndexBuilderProvider));
        private static bool debugLog = log.IsDebugEnabled;

        public override void RebuildIndex(
            ZoneSettings zoneSettings,
            string indexPath)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (zoneSettings == null)
            {
                log.Error("zoneSettings object passed to ProductIndexBuilderProvider.RebuildIndex was null");
                return;
            }

            //don't index pending/unpublished zones
            if (!zoneSettings.IsPublished) { return; }

            log.Info("ProductIndexBuilderProvider indexing zone - " + zoneSettings.Name);

            //try
            //{
            var featureGuid = CanhCam.Business.Product.FeatureGuid;
            var newsFeature = new ModuleDefinition(featureGuid);

            var lstProducts = CanhCam.Business.Product.GetTopAdv(siteId: zoneSettings.SiteId, zoneId: zoneSettings.ZoneId);

            // Language
            string listGuid = zoneSettings.ZoneGuid.ToString();
            string listProductGuid = string.Empty;
            foreach (CanhCam.Business.Product product in lstProducts)
            {
                if (!listGuid.Contains(product.ProductGuid.ToString()))
                {
                    listGuid += ";" + product.ProductGuid.ToString();
                    listProductGuid += ";" + product.ProductGuid.ToString();
                }
            }

            List<ContentLanguage> listContent = ContentLanguage.GetByListContent(listGuid);
            List<Language> listLanguages = LanguageHelper.GetPublishedLanguages();
            string defaultCulture = WebConfigSettings.DefaultLanguageCultureForContent;
            // End Language

            List<ContentAttribute> listAttribute = new List<ContentAttribute>();

            foreach (Language lang in listLanguages)
            {
                if (lang.LanguageCode.ToLower() != defaultCulture.ToLower())
                    listAttribute = ContentAttribute.GetByListContent(listProductGuid, lang.LanguageID);
                else
                    listAttribute = ContentAttribute.GetByListContent(listProductGuid);

                foreach (CanhCam.Business.Product product in lstProducts)
                {
                    CanhCam.SearchIndex.IndexItem indexItem = new CanhCam.SearchIndex.IndexItem
                    {
                        SiteId = zoneSettings.SiteId,
                        ZoneId = zoneSettings.ZoneId,
                        ZoneName = zoneSettings.Name,
                        ViewRoles = zoneSettings.ViewRoles,
                        ZoneViewRoles = zoneSettings.ViewRoles,
                        FeatureId = featureGuid.ToString(),
                        FeatureName = newsFeature.FeatureName,
                        FeatureResourceFile = newsFeature.ResourceFile,

                        ItemGuid = product.ProductGuid,
                        Title = product.Title
                    };

                    string url = product.Url;
                    if (url.Length > 0)
                    {
                        if (url.StartsWith("http"))
                            indexItem.ViewPage = url;
                        else
                            indexItem.ViewPage = url.Replace("~/", string.Empty);
                    }
                    else
                    {
                        indexItem.ViewPage = "Product/ProductDetail.aspx?zoneid="
                            + indexItem.ZoneId.ToInvariantString()
                            + "&ProductID=" + product.ProductId.ToString();
                    }

                    indexItem.PageMetaDescription = product.MetaDescription;
                    indexItem.PageMetaKeywords = product.MetaKeywords;

                    indexItem.CreatedUtc = product.StartDate;
                    indexItem.LastModUtc = product.LastModUtc;

                    //if (indexItem.ViewPage.Length > 0)
                    //{
                    indexItem.UseQueryStringParams = false;
                    //}
                    //else
                    //{
                    //    indexItem.ViewPage = "Product/ProductDetail.aspx";
                    //}
                    indexItem.Content = SecurityHelper.RemoveMarkup(product.FullContent);
                    indexItem.ContentAbstract = SecurityHelper.RemoveMarkup(product.BriefContent);

                    indexItem.IsPublished = product.IsPublished;
                    indexItem.PublishBeginDate = product.StartDate;
                    indexItem.PublishEndDate = product.EndDate;

                    if (product.SubTitle.Length > 0)
                        indexItem.Content += " " + product.SubTitle;

                    // Language
                    indexItem.LanguageCode = defaultCulture;
                    if (lang.LanguageCode.ToLower() != defaultCulture.ToLower())
                    {
                        indexItem.ZoneName = string.Empty;
                        indexItem.LanguageCode = lang.LanguageCode;
                        indexItem.RemoveOnly = true;

                        foreach (ContentLanguage ct in listContent)
                        {
                            if (ct.ContentGuid == zoneSettings.ZoneGuid)
                            {
                                indexItem.ZoneName = ct.Title;
                            }
                            if (ct.ContentGuid == product.ProductGuid)
                            {
                                indexItem.PageMetaDescription = ct.MetaDescription;
                                indexItem.PageMetaKeywords = ct.MetaKeywords;
                                indexItem.Title = ct.Title;
                                indexItem.Content = SecurityHelper.RemoveMarkup(ct.FullContent);
                                indexItem.ContentAbstract = SecurityHelper.RemoveMarkup(ct.BriefContent);

                                if (ct.ExtraText1.Length > 0)
                                    indexItem.Content += " " + ct.ExtraText1;

                                indexItem.ViewPage = ct.Url.Replace("~/", string.Empty);
                                indexItem.RemoveOnly = false;
                            }
                        }
                    }
                    // End Language

                    if (product.Code.Length > 0)
                        indexItem.Content += " " + product.Code;

                    foreach (ContentAttribute attribute in listAttribute)
                    {
                        if (attribute.ContentGuid == product.ProductGuid)
                        {
                            indexItem.Content += " " + attribute.Title + " " + SecurityHelper.RemoveMarkup(attribute.ContentText);
                        }
                    }

                    CanhCam.SearchIndex.IndexHelper.RebuildIndex(indexItem, indexPath);

                    if (debugLog) log.Debug("Indexed " + indexItem.Title);
                }
            }
            //}
            //catch (Exception ex)
            //{
            //    log.Error(ex);
            //}
        }

        public override void ContentChangedHandler(
            object sender,
            ContentChangedEventArgs e)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (sender == null) return;
            if (!(sender is CanhCam.Business.Product)) return;

            CanhCam.Business.Product product = (CanhCam.Business.Product)sender;
            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            product.SiteId = siteSettings.SiteId;
            product.SearchIndexPath = CanhCam.SearchIndex.IndexHelper.GetSearchIndexPath(siteSettings.SiteId);

            if (e.IsDeleted)
            {
                CanhCam.SearchIndex.IndexHelper.RemoveIndexItem(
                        product.ZoneId,
                        product.ProductGuid);
            }
            else
            {
                if (ThreadPool.QueueUserWorkItem(new WaitCallback(IndexItem), product))
                {
                    if (debugLog) log.Debug("ProductIndexBuilderProvider.IndexItem queued");
                }
                else
                {
                    log.Error("Failed to queue a thread for ProductIndexBuilderProvider.IndexItem");
                }

                //IndexItem(news);
            }
        }

        private static void IndexItem(object o)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }
            if (o == null) return;
            if (!(o is CanhCam.Business.Product)) return;

            CanhCam.Business.Product content = o as CanhCam.Business.Product;
            IndexItem(content);
        }

        private static void IndexItem(CanhCam.Business.Product product)
        {
            if (WebConfigSettings.DisableSearchIndex) { return; }

            if (product == null)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error("product object passed to ProductIndexBuilderProvider.IndexItem was null");
                }
                return;
            }

            Guid featureGuid = CanhCam.Business.Product.FeatureGuid;
            ModuleDefinition newsFeature = new ModuleDefinition(featureGuid);

            List<ContentAttribute> listAttribute = new List<ContentAttribute>();
            // Language
            List<Language> listLanguages = LanguageHelper.GetPublishedLanguages();
            string defaultCulture = WebConfigSettings.DefaultLanguageCultureForContent;
            // End Language

            ZoneSettings zoneSettings = new ZoneSettings(product.SiteId, product.ZoneId);
            //don't index pending/unpublished pages
            if (!zoneSettings.IsPublished) { return; }

            foreach (Language lang in listLanguages)
            {
                CanhCam.SearchIndex.IndexItem indexItem = new CanhCam.SearchIndex.IndexItem();
                if (product.SearchIndexPath.Length > 0)
                {
                    indexItem.IndexPath = product.SearchIndexPath;
                }
                indexItem.SiteId = product.SiteId;
                indexItem.ZoneId = zoneSettings.ZoneId;
                indexItem.ZoneName = zoneSettings.Name;
                indexItem.ViewRoles = zoneSettings.ViewRoles;
                indexItem.ZoneViewRoles = zoneSettings.ViewRoles;

                indexItem.PageMetaDescription = product.MetaDescription;
                indexItem.PageMetaKeywords = product.MetaKeywords;
                indexItem.ItemGuid = product.ProductGuid;
                indexItem.Title = product.Title;
                indexItem.Content = product.FullContent;
                indexItem.ContentAbstract = product.BriefContent;
                indexItem.FeatureId = featureGuid.ToString();
                indexItem.FeatureName = newsFeature.FeatureName;
                indexItem.FeatureResourceFile = newsFeature.ResourceFile;

                //indexItem.OtherContent = stringBuilder.ToString();
                indexItem.IsPublished = product.IsPublished;
                indexItem.PublishBeginDate = product.StartDate;
                indexItem.PublishEndDate = product.EndDate;

                indexItem.CreatedUtc = product.StartDate;
                indexItem.LastModUtc = product.LastModUtc;

                if (product.Url.Length > 0)
                {
                    if (product.Url.StartsWith("http"))
                        indexItem.ViewPage = product.Url;
                    else
                        indexItem.ViewPage = product.Url.Replace("~/", string.Empty);
                }
                else
                {
                    indexItem.ViewPage = "Product/ProductDetail.aspx?zoneid="
                        + indexItem.ZoneId.ToInvariantString()
                        + "&ProductID=" + product.ProductId.ToInvariantString()
                        ;
                }

                indexItem.UseQueryStringParams = false;

                if (product.SubTitle.Length > 0)
                    indexItem.Content += " " + product.SubTitle;

                // Language
                string listGuid = zoneSettings.ZoneGuid.ToString()
                                + ";" + product.ProductGuid.ToString();
                List<ContentLanguage> listContent = ContentLanguage.GetByListContent(listGuid);
                indexItem.LanguageCode = defaultCulture;
                if (lang.LanguageCode.ToLower() != defaultCulture.ToLower())
                {
                    indexItem.LanguageCode = lang.LanguageCode;
                    indexItem.RemoveOnly = true;

                    foreach (ContentLanguage ct in listContent)
                    {
                        if (ct.ContentGuid == zoneSettings.PageGuid)
                        {
                            indexItem.ZoneName = ct.Title;
                        }
                        if (ct.ContentGuid == product.ProductGuid)
                        {
                            indexItem.PageMetaDescription = ct.MetaDescription;
                            indexItem.PageMetaKeywords = ct.MetaKeywords;
                            indexItem.Title = ct.Title;
                            indexItem.Content = SecurityHelper.RemoveMarkup(ct.FullContent);
                            indexItem.ContentAbstract = SecurityHelper.RemoveMarkup(ct.BriefContent);

                            if (ct.ExtraText1.Length > 0)
                                indexItem.Content += " " + ct.ExtraText1;

                            indexItem.ViewPage = ct.Url.Replace("~/", string.Empty);
                            indexItem.RemoveOnly = false;
                        }
                    }

                    listAttribute = ContentAttribute.GetByContentAsc(product.ProductGuid, lang.LanguageID);
                }
                else
                {
                    listAttribute = ContentAttribute.GetByContentAsc(product.ProductGuid);
                }
                // End Language

                if (product.Code.Length > 0)
                    indexItem.Content += " " + product.Code;

                foreach (ContentAttribute attribute in listAttribute)
                {
                    indexItem.Content += " " + attribute.Title + " " + SecurityHelper.RemoveMarkup(attribute.ContentText);
                }

                if (product.IsDeleted)
                    indexItem.RemoveOnly = true;

                CanhCam.SearchIndex.IndexHelper.RebuildIndex(indexItem);
            }

            if (debugLog) log.Debug("Indexed " + product.Title);
        }
    }
}
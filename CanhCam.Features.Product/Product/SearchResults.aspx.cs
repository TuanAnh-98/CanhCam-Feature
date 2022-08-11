/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-31
/// Last Modified:			2015-04-01

using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using CanhCam.Web.UI;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    public partial class SearchResults : CmsBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(SearchResults));

        private string query = string.Empty;
        private int pageNumber = 1;
        private int pageSize = ConfigHelper.GetIntProperty("ProductSearchResultsPageSize", 20);
        private int totalHits = 0;
        private int totalPages = 1;
        private bool indexVerified = false;
        private bool isSiteEditor = false;
        private bool queryErrorOccurred = false;
        private TimeZoneInfo timeZone = null;
        private double timeOffset;

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);
            this.btnDoSearch.Click += new EventHandler(btnDoSearch_Click);
            btnRebuildSearchIndex.Click += new EventHandler(btnRebuildSearchIndex_Click);

            if (WebConfigSettings.ShowLeftColumnOnSearchResults) { StyleCombiner.AlwaysShowLeftColumn = true; }
            if (WebConfigSettings.ShowRightColumnOnSearchResults) { StyleCombiner.AlwaysShowRightColumn = true; }
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (SiteUtils.SslIsAvailable()) { SiteUtils.ForceSsl(); }
            SecurityHelper.DisableBrowserCache();

            LoadSettings();

            this.query = string.Empty;

            if (siteSettings == null)
            {
                siteSettings = CacheHelper.GetCurrentSiteSettings();
            }

            PopulateLabels();

            string primarySearchProvider = SiteUtils.GetPrimarySearchProvider();

            switch (primarySearchProvider)
            {
                case "google":
                    pnlInternalSearch.Visible = false;
                    pnlBingSearch.Visible = false;
                    pnlGoogleSearch.Visible = true;
                    gcs.Visible = true;

                    break;

                case "bing":
                    pnlInternalSearch.Visible = false;
                    pnlBingSearch.Visible = true;
                    pnlGoogleSearch.Visible = false;
                    bingSearch.Visible = true;
                    break;

                case "internal":
                default:

                    if (WebConfigSettings.DisableSearchIndex)
                    {
                        WebUtils.SetupRedirect(this, SiteUtils.GetNavigationSiteRoot());
                        return;
                    }

                    pnlInternalSearch.Visible = true;
                    pnlBingSearch.Visible = false;
                    pnlGoogleSearch.Visible = false;
                    SetupInternalSearch();
                    break;
            }

            SetupViewModeControls(false, false);
            LoadAltContent(displaySettings.IncludeTopContent, displaySettings.IncludeBottomContent, displaySettings.PageId);
            LoadSideContent(displaySettings.IncludeLeftContent, displaySettings.IncludeRightContent, displaySettings.PageId);
        }

        private void SetupInternalSearch()
        {
            if (SiteUtils.ShowAlternateSearchIfConfigured())
            {
                string bingApiId = SiteUtils.GetBingApiId();
                string googleCustomSearchId = SiteUtils.GetGoogleCustomSearchId();
                if ((bingApiId.Length > 0) || (googleCustomSearchId.Length > 0)) { spnAltSearchLinks.Visible = true; }

                lnkBingSearch.Visible = (bingApiId.Length > 0);
                lnkGoogleSearch.Visible = (googleCustomSearchId.Length > 0);
            }

            // got here by a cross page postback from another page if Page.PreviousPage is not null
            // this occurs when the search input is used in the skin rather than the search link
            if (Page.PreviousPage != null)
            {
                HandleCrossPagePost();
            }
            else
            {
                DoSearch();
            }

            txtSearchInput.Focus();
        }

        private void HandleCrossPagePost()
        {
            SearchInput previousPageSearchInput = (SearchInput)PreviousPage.Master.FindControl("SearchInput1");
            // try in page if not found in masterpage
            if (previousPageSearchInput == null) { previousPageSearchInput = (SearchInput)PreviousPage.FindControl("SearchInput1"); }

            if (previousPageSearchInput != null)
            {
                TextBox prevSearchTextBox = (TextBox)previousPageSearchInput.FindControl("txtSearch");
                if ((prevSearchTextBox != null) && (prevSearchTextBox.Text.Length > 0))
                {
                    //this.txtSearchInput.Text = prevSearchTextBox.Text;
                    WebUtils.SetupRedirect(this, SiteRoot + "/product/searchresults.aspx?q=" + Server.UrlEncode(prevSearchTextBox.Text));
                    return;
                }
            }

            DoSearch();
        }

        private List<string> GetUserRoles()
        {
            List<string> userRoles = new List<string>();

            userRoles.Add("All Users");
            if (Request.IsAuthenticated)
            {
                SiteUser currentUser = SiteUtils.GetCurrentSiteUser();
                if (currentUser != null)
                {
                    using (IDataReader reader = SiteUser.GetRolesByUser(siteSettings.SiteId, currentUser.UserId))
                    {
                        while (reader.Read())
                        {
                            userRoles.Add(reader["RoleName"].ToString());
                        }
                    }
                }
            }

            return userRoles;
        }

        private void DoSearch()
        {
            if (Page.IsPostBack) { return; }

            if (Request.QueryString.Get("q") == null) { return; }

            query = Request.QueryString.Get("q");

            if (this.query.Length == 0) { return; }

            //txtSearchInput.Text = Server.HtmlEncode(query).Replace("&quot;", "\"") ;
            txtSearchInput.Text = SecurityHelper.SanitizeHtml(query);


            query = query.Replace("\"", string.Empty)
                             .Replace("+", string.Empty)
                             .Replace("&&", string.Empty)
                             .Replace("||", string.Empty)
                             .Replace("!", string.Empty)
                             .Replace("(", string.Empty)
                             .Replace(")", string.Empty)
                             .Replace("{", string.Empty)
                             .Replace("}", string.Empty)
                             .Replace("[", string.Empty)
                             .Replace("]", string.Empty)
                             .Replace("^", string.Empty)
                             .Replace("\"", string.Empty)
                             .Replace("~", string.Empty)
                             .Replace("*", string.Empty)
                             .Replace("?", string.Empty)
                             .Replace(":", string.Empty)
                             .Replace("\\", string.Empty)
                             ;
            var queryToSearch = query;

            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<SearchResults></SearchResults>");
            var root = doc.DocumentElement;

            var lstProducts = Product.GetPageAdv(pageNumber: pageNumber,
                pageSize: pageSize + 1,
                siteId: siteSettings.SiteId, 
                publishStatus: 1,
                languageId: WorkingCulture.LanguageId, 
                keyword: queryToSearch);
            if (lstProducts.Count > 0)
            {
                var hasNextPage = lstProducts.Count > pageSize; 
                if (hasNextPage)
                {
                    lstProducts.RemoveAt(lstProducts.Count - 1);
                    XmlHelper.AddNode(doc, root, "NextPage", (pageNumber + 1).ToString());
                }

                var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProducts);
                foreach (var product in lstProducts)
                {
                    var productXml = doc.CreateElement("Product");
                    root.AppendChild(productXml);

                    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset);
                }
            }

            var totalProducts = Product.GetCountAdv(siteId: siteSettings.SiteId, publishStatus: 1, languageId: WorkingCulture.LanguageId, keyword: queryToSearch);
            XmlHelper.AddNode(doc, root, "TotalProducts", totalProducts.ToString());

            var newsPageSize = 8;
            var lstNews = News.GetPageBySearch2(siteId: siteSettings.SiteId, publishStatus: 1, languageId: WorkingCulture.LanguageId, keyword: queryToSearch, pageNumber: pageNumber, pageSize: newsPageSize + 1);
            if (lstNews.Count > 0)
            {
                var hasNextPage = lstNews.Count > newsPageSize;
                if (hasNextPage)
                {
                    lstNews.RemoveAt(lstNews.Count - 1);
                    XmlHelper.AddNode(doc, root, "NewsNextPage", (pageNumber + 1).ToString());
                }

                foreach (var news in lstNews)
                {
                    var newsXml = doc.CreateElement("News");
                    root.AppendChild(newsXml);

                    NewsUI.NewsHelper.BuildNewsDataXml(doc, newsXml, news, timeZone, timeOffset, string.Empty);
                }
            }

            var lstZoneIds = Product.GetZoneByKeyword(SiteId, query);
            if (lstZoneIds.Count > 0)
            {
                var siteMapDataSource = new SiteMapDataSource
                {
                    SiteMapProvider = "canhcamsite" + siteSettings.SiteId.ToInvariantString()
                };

                var rootNode = siteMapDataSource.Provider.RootNode;
                var allNodes = rootNode.GetAllNodes();

                var lstTopNodes = new List<gbSiteMapNode>();
                foreach (var zoneId in lstZoneIds)
                {
                    foreach (gbSiteMapNode childNode in allNodes)
                    {
                        if (zoneId == childNode.ZoneId && childNode.FeatureGuids.Contains(Product.FeatureGuid.ToString()))
                        {
                            var topNode = SiteUtils.GetTopLevelParentNode(childNode);
                            if (!lstTopNodes.Contains(topNode))
                                lstTopNodes.Add(topNode);

                            //lstTopNodes.Add(childNode);
                        }
                    }
                }

                foreach (gbSiteMapNode gbNode in lstTopNodes)
                {
                    var item = doc.CreateElement("Zone");
                    root.AppendChild(item);

                    XmlHelper.AddNode(doc, item, "ZoneId", gbNode.ZoneId.ToString());
                    XmlHelper.AddNode(doc, item, "Title", gbNode.Title);
                    XmlHelper.AddNode(doc, item, "Url", Page.ResolveUrl(gbNode.Url) + "?q=" + txtSearchInput.Text);
                    XmlHelper.AddNode(doc, item, "ImageUrl", gbNode.PrimaryImage);

                    //var zoneIds = ProductHelper.GetRangeZoneIdsToSemiColonSeparatedString(siteSettings.SiteId, gbNode.ZoneId);
                    //iCount = Product.GetCountByKeyword(siteSettings.SiteId, zoneIds, queryToSearch);
                    //totalPages = CaculateTotalPages(pageSize, iCount);
                    //XmlHelper.AddNode(doc, item, "TotalPages", totalPages.ToString());
                    //XmlHelper.AddNode(doc, item, "TotalProducts", iCount.ToString());

                    //var lstProducts = Product.GetPageByKeyword(siteSettings.SiteId, zoneIds, queryToSearch, 1, pageSize);
                    //var lstDiscountItems = DiscountAppliedToItem.GetActive(siteSettings.SiteId, -1, lstProducts);
                    //foreach (var product in lstProducts)
                    //{
                    //    var productXml = doc.CreateElement("Product");
                    //    item.AppendChild(productXml);

                    //    ProductHelper.BuildProductDataXml(doc, productXml, product, lstDiscountItems, timeZone, timeOffset);
                    //}
                }
            }

            //var lstNews = News.GetPageByKeyword(siteSettings.SiteId, queryToSearch, 1, pageSize);
            //foreach (var news in lstNews)
            //{
            //    var newsXml = doc.CreateElement("News");
            //    root.AppendChild(newsXml);

            //    CanhCam.Web.NewsUI.NewsHelper.BuildNewsDataXml(doc, newsXml, news, timeZone, timeOffset, string.Empty);
            //}

            XmlHelper.AddNode(doc, root, "Keyword", queryToSearch);

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", "ProductSearchResults.xslt"), doc);
        }

        private bool InitIndexIfNeeded()
        {
            if (indexVerified) { return false; }

            indexVerified = true;
            if (!CanhCam.SearchIndex.IndexHelper.VerifySearchIndex(siteSettings))
            {
                this.lblMessage.Text = ResourceHelper.GetResourceString("Resource", "SearchResultsBuildingIndexMessage");
                Thread.Sleep(5000); //wait 5 seconds
                SiteUtils.QueueIndexing();

                return true;
            }

            return false;
        }

        //private void ShowNoResults()
        //{
        //    if (queryErrorOccurred)
        //    {
        //        litNoResults.Text = ResourceHelper.GetResourceString("Resource", "SearchQueryInvalid");
        //    }

        //    divResults.Visible = false;
        //    pnlNoResults.Visible = (txtSearchInput.Text.Length > 0);
        //}

        private void btnDoSearch_Click(object sender, EventArgs e)
        {
            string redirectUrl = SiteRoot + "/Product/SearchResults.aspx?q="
                    + Server.UrlEncode(this.txtSearchInput.Text);

            WebUtils.SetupRedirect(this, redirectUrl);
        }

        private void btnRebuildSearchIndex_Click(object sender, EventArgs e)
        {
            IndexingQueue.DeleteAll();
            CanhCam.SearchIndex.IndexHelper.DeleteSearchIndex(siteSettings);
            CanhCam.SearchIndex.IndexHelper.VerifySearchIndex(siteSettings);

            this.lblMessage.Text = ResourceHelper.GetResourceString("Resource", "SearchResultsBuildingIndexMessage");
            Thread.Sleep(5000); //wait 5 seconds
            SiteUtils.QueueIndexing();
        }

        private void LoadSettings()
        {
            isSiteEditor = WebUser.IsAdminOrContentAdmin || (SiteUtils.UserIsSiteEditor());
            pageNumber = WebUtils.ParseInt32FromQueryString("p", true, 1);
            timeZone = SiteUtils.GetUserTimeZone();
            timeOffset = SiteUtils.GetUserTimeOffset();
        }

        private void PopulateLabels()
        {
            if (siteSettings == null) return;

            Title = SiteUtils.FormatPageTitle(siteSettings, ResourceHelper.GetResourceString("ProductResources", "SearchPageTitle"));
            litSearchTitle.Text = ResourceHelper.GetResourceString("ProductResources", "SearchPageTitle");

            MetaDescription = string.Format(CultureInfo.InvariantCulture,
                ResourceHelper.GetResourceString("Resource", "MetaDescriptionSearchFormat"), siteSettings.SiteName);

            lblMessage.Text = string.Empty;
            btnDoSearch.Text = ResourceHelper.GetResourceString("Resource", "SearchButtonText");

            btnRebuildSearchIndex.Text = ResourceHelper.GetResourceString("Resource", "SearchRebuildIndexButton");
            UIHelper.AddConfirmationDialog(btnRebuildSearchIndex, ResourceHelper.GetResourceString("Resource", "SearchRebuildIndexWarning"));

            divDelete.Visible = (WebConfigSettings.ShowRebuildSearchIndexButtonToAdmins && WebUser.IsAdmin && ViewMode != PageViewMode.View);

            litAltSearchMessage.Text = ResourceHelper.GetResourceString("Resource", "AltSearchPrompt");
            lnkBingSearch.Text = ResourceHelper.GetResourceString("Resource", "SearchThisSiteWithBing");
            lnkBingSearch.NavigateUrl = SiteRoot + "/BingSearch.aspx";
            lnkGoogleSearch.Text = ResourceHelper.GetResourceString("Resource", "SearchThisSiteWithGoogle");
            lnkGoogleSearch.NavigateUrl = SiteRoot + "/GoogleSearch.aspx";

            //this page has no content other than nav
            SiteUtils.AddNoIndexFollowMeta(Page);

            AddClassToBody("searchresults productsearchresults");
        }

        //private static string HighlightKeywords(this string input, string keywords)
        //{
        //    if (input == string.Empty || keywords == string.Empty)
        //    {
        //        return input;
        //    }

        //    string[] sKeywords = keywords.Split(' ');
        //    foreach (string sKeyword in sKeywords)
        //    {
        //        try
        //        {
        //            input = Regex.Replace(input, sKeyword, string.Format("<span class=\"searchterm\">{0}</span>", "$0"), RegexOptions.IgnoreCase);
        //        }
        //        catch
        //        {
        //            //
        //        }
        //    }

        //    return input;
        //}
    }
}
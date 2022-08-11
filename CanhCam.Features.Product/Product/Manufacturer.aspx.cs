using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Web;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ManufacturerPage : CmsBasePage
    {
        #region OnInit

        private static readonly ILog log = LogManager.GetLogger(typeof(ManufacturerPage));

        protected override void OnPreInit(EventArgs e)
        {
            AllowSkinOverride = true;
            base.OnPreInit(e);
        }

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);

            base.OnInit(e);
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            var manufacturerId = WebUtils.ParseInt32FromQueryString("ManufacturerID", -1);
            var manufacturer = new Manufacturer(manufacturerId);
            if (manufacturer == null
                || manufacturer.ManufacturerId < 0
                || !manufacturer.IsPublished
                || manufacturer.IsDeleted
                || manufacturer.PageId < 0)
            {
                if (WebConfigSettings.Custom404Page.Length > 0)
                    Server.Transfer(WebConfigSettings.Custom404Page);
                else
                    Server.Transfer("~/PageNotFound.aspx");

                return;
            }

            CurrentPage = new PageSettings(siteSettings.SiteId, manufacturer.PageId);
            if (CurrentPage == null || CurrentPage.PageId < 0 || CurrentPage.IsDeleted)
                return;

            if (CurrentPage.BodyCssClass.Length > 0)
                AddClassToBody(CurrentPage.BodyCssClass);

            var languageId = WorkingCulture.LanguageId;
            var title = string.Empty;
            if (manufacturer.MetaTitle.Length > 0)
            {
                Title = manufacturer.MetaTitle;
                title = manufacturer.MetaTitle;
            }
            else
            {
                Title = SiteUtils.FormatPageTitle(siteSettings, manufacturer.Name);
                title = manufacturer.Name;
            }

            if (manufacturer.MetaKeywords.Length > 0)
                MetaKeywordCsv = manufacturer.MetaKeywords;
            else if (siteSettings.MetaKeyWords.Length > 0)
                MetaKeywordCsv = siteSettings.GetMetaKeyWords(languageId);

            if (manufacturer.MetaDescription.Length > 0)
                MetaDescription = manufacturer.MetaDescription;
            else if (siteSettings.MetaDescription.Length > 0)
                MetaDescription = manufacturer.Name.Trim() + " " + siteSettings.GetMetaDescription(languageId);

            if (title.Length > 0)
                AdditionalMetaMarkup += "\n<meta property=\"og:title\" content=\"" + title + "\" />";
            if (MetaDescription.Length > 0)
                AdditionalMetaMarkup += "\n<meta property=\"og:description\" content=\"" + MetaDescription + "\" />";
            AdditionalMetaMarkup += "\n<meta property=\"og:site_name\" content=\"" + siteSettings.SiteName + "\" />";

            if (title.Length > 0)
                AdditionalMetaMarkup += "\n<meta itemprop=\"name\" content=\"" + title + "\" />";
            if (MetaDescription.Length > 0)
                AdditionalMetaMarkup += "\n<meta itemprop=\"description\" content=\"" + MetaDescription + "\" />";

            if (siteSettings.MetaAdditional.Length > 0)
                AdditionalMetaMarkup += siteSettings.MetaAdditional;

            var isAdmin = WebUser.IsAdmin;
            var isContentAdmin = false;
            var isSiteEditor = false;
            if (!isAdmin)
            {
                isContentAdmin = WebUser.IsContentAdmin;
                isSiteEditor = SiteUtils.UserIsSiteEditor();
            }

            bool forceShowViewMode = false;
            bool forceShowWorkflow = false;
            CurrentPage.RefreshModules();
            if (CurrentPage.Modules.Count == 0)
            {
                SetupViewModeControls(forceShowViewMode, forceShowWorkflow);
                return;
            }

            foreach (Module module in CurrentPage.Modules)
            {
                if (!ModuleIsVisible(module)) { continue; }

                if ((!WebUser.IsInRoles(module.ViewRoles))
                    && (!isContentAdmin)
                    && (!isSiteEditor))
                {
                    continue;
                }

                if ((module.ViewRoles == "Admins;") && (!isAdmin)) { continue; }

                Control parent = this.MPContent;

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "leftpane"))
                    parent = this.MPLeftPane;
                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "rightpane"))
                    parent = this.MPRightPane;

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent1"))
                {
                    if (AltPane1 != null)
                        parent = this.AltPane1;
                    else
                        parent = this.MPContent;
                }

                if (StringHelper.IsCaseInsensitiveMatch(module.PaneName, "altcontent2"))
                {
                    if (AltPane2 != null)
                        parent = this.AltPane2;
                    else
                        parent = this.MPContent;
                }

                if (!ShouldShowModule(module)) { continue; }

                if ((module.CacheTime == 0) || (WebConfigSettings.DisableContentCache))
                {
                    try
                    {
                        var c = Page.LoadControl("~/" + module.ControlSource);
                        if (c == null) { continue; }

                        if (c is SiteModuleControl)
                        {
                            var siteModule = (SiteModuleControl)c;
                            siteModule.SiteId = siteSettings.SiteId;
                            siteModule.ModuleConfiguration = module;

                            if (Request.IsAuthenticated)
                            {
                                if (!forceShowViewMode && siteModule.UserHasPermission())
                                    forceShowViewMode = true;

                                if (
                                    forceShowViewMode
                                    && WebConfigSettings.EnableContentWorkflow
                                    && siteSettings.EnableContentWorkflow
                                    && (siteModule is IWorkflow)
                                    )
                                {
                                    forceShowWorkflow = forceShowViewMode;
                                }
                            }
                        }

                        parent.Controls.Add(c);
                    }
                    catch (HttpException ex)
                    {
                        log.Error("failed to load control " + module.ControlSource, ex);
                    }
                }
                else
                {
                    var siteModule = new CachedSiteModuleControl
                    {
                        SiteId = siteSettings.SiteId,
                        ModuleConfiguration = module
                    };
                    parent.Controls.Add(siteModule);
                }

                parent.Visible = true;
                parent.Parent.Visible = true;
            } //end foreach

            AddClassToBody("brand-detail-page");
        }
    }
}
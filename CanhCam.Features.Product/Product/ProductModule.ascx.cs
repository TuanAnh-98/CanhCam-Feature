/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-06-23
/// Last Modified:			2014-06-23

using Resources;
using System;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductModule : SiteModuleControl
    {
        protected ProductConfiguration config = new ProductConfiguration();

        private bool forceLoadDetail = false;

        public bool ForceLoadDetail
        {
            get { return forceLoadDetail; }
            set { forceLoadDetail = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadSettings();
            PopulateLabels();
            PopulateControls();
        }

        private void PopulateControls()
        {
            if (config.LoadFirstProduct || forceLoadDetail)
            {
                Control c = Page.LoadControl("~/Product/Controls/ProductViewControl.ascx");
                if (c == null) { return; }

                if (c is ProductViewControl)
                {
                    ProductViewControl productView = (ProductViewControl)c;
                    productView.module = this.ModuleConfiguration;
                    productView.Config = config;
                }

                placeHolder.Controls.Add(c);
            }
            else
            {
                Control c = Page.LoadControl("~/Product/Controls/ProductListControl.ascx");
                if (c == null) { return; }

                if (c is ProductListControl)
                {
                    ProductListControl productList = (ProductListControl)c;
                    productList.ModuleId = ModuleId;
                    productList.Config = config;
                    productList.SiteRoot = SiteRoot;
                    productList.ImageSiteRoot = ImageSiteRoot;
                    productList.ModuleTitle = this.Title;
                }

                placeHolder.Controls.Add(c);

                if (!Page.IsPostBack)
                    CartHelper.LastContinueShoppingPage = ProductHelper.BuildFilterUrlLeaveOutPageNumber(Request.RawUrl);
            }

            if (forceLoadDetail)
            {
                CmsBasePage basePage = Page as CmsBasePage;
                if (basePage != null)
                {
                    basePage.LoadSideContent(config.ShowLeftContent, config.ShowRightContent, config.ShowHiddenContents);
                    basePage.LoadAltContent(ProductConfiguration.ShowTopContent, ProductConfiguration.ShowBottomContent, config.ShowHiddenContents);
                }
            }
        }

        protected virtual void PopulateLabels()
        {
        }

        protected virtual void LoadSettings()
        {
            config = new ProductConfiguration(Settings);

            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }

        public override bool UserHasPermission()
        {
            if (!Request.IsAuthenticated)
                return false;

            bool hasPermission = false;

            if (ProductPermission.CanCreate)
            {
                this.EditUrl = SiteRoot + "/Product/AdminCP/ProductEdit.aspx?start=" + CurrentZone.ZoneId;
                this.EditText = ProductResources.ProductInsertLink;

                hasPermission = true;
            }

            if (ProductPermission.CanViewList)
            {
                this.LiteralExtraMarkup =
                        "<dd><a class='ActionLink productlistlink' href='"
                        + SiteRoot
                        + "/Product/AdminCP/ProductList.aspx?start=" + CurrentZone.ZoneId + "'><i class='fa fa-list'></i> "
                        + ProductResources.ProductListLink + "</a></dd>";

                hasPermission = true;
            }

            return hasPermission;
        }
    }
}
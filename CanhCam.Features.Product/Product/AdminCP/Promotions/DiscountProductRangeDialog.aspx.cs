using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class DiscountProductRangeDialog : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DiscountProductRangeDialog));

        private int discountID = 0;
        private int productID = 0;

        private List<DiscountRange> lstDiscountRange = new List<DiscountRange>();

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            gridOrderDiscountRange.NeedDataSource += gridOrderDiscountRange_NeedDataSource;
            gridOrderDiscountRange.ItemDataBound += gridOrderDiscountRange_ItemDataBound;
            gridOrderDiscountRange.ItemCommand += gridOrderDiscountRange_ItemCommand;

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnOrderRangeNewRow.Click += new EventHandler(btnOrderRangeNewRow_Click);
        }

        #endregion OnInit

        protected void Page_Load(object sender, EventArgs e)
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

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void LoadSettings()
        {
            productID = WebUtils.ParseInt32FromQueryString("ProductID", productID);
            discountID = WebUtils.ParseInt32FromQueryString("DiscountID", discountID);
        }

        private void PopulateLabels()
        {
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
        }

        private void PopulateControls()
        {
        }

        private List<DiscountRange> ProductRanges
        {
            get
            {
                if (ViewState["ProductRanges"] != null)
                    return ViewState["ProductRanges"] as List<DiscountRange>;
                return null;
            }

            set
            {
                ViewState["ProductRanges"] = value;
            }
        }

        #region Click Events

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            lstDiscountRange = ProductRanges;

            //If existed discount range is deleted, also delete from database
            var lstOriginalDiscountRanges = DiscountRange.GetRange(discountID, productID);
            foreach (DiscountRange originalDiscountRange in lstOriginalDiscountRanges)
            {
                if (!lstDiscountRange.Exists(x => x.ItemID == originalDiscountRange.ItemID))
                    DiscountRange.Delete(originalDiscountRange.ItemID);
            }

            //Begin update
            foreach (GridDataItem data in gridOrderDiscountRange.Items)
            {
                var txtFromQuantity = (TextBox)data.FindControl("txtFromQuantity");
                var txtToQuantity = (TextBox)data.FindControl("txtToQuantity");
                var txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                var txtMaximumDiscount = (TextBox)data.FindControl("txtMaximumDiscount");
                var ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                var itemID = Convert.ToInt32(data.GetDataKeyValue("ItemID"));
                decimal fromQuantity = Decimal.TryParse(txtFromQuantity.Text.Trim(), out fromQuantity) ? fromQuantity : 0;
                decimal toQuantity = Decimal.TryParse(txtToQuantity.Text.Trim(), out toQuantity) ? toQuantity : 0;
                decimal discountAmount = Decimal.TryParse(txtDiscountAmount.Text.Trim(), out discountAmount) ? discountAmount : 0;
                decimal maximumDiscount = Decimal.TryParse(txtMaximumDiscount.Text.Trim(), out maximumDiscount) ? maximumDiscount : 0;
                int discountType = Int32.Parse(ddlDiscountType.SelectedValue);

                if (
                   //fromQuantity != Convert.ToDecimal(data.GetDataKeyValue("FromPrice"))
                   //|| toQuantity != Convert.ToDecimal(data.GetDataKeyValue("ToPrice"))
                   //|| discountAmount != Convert.ToDecimal(data.GetDataKeyValue("DiscountAmount"))
                   //|| discountType != Convert.ToInt32(data.GetDataKeyValue("DiscountType"))
                   //|| maximumDiscount != Convert.ToDecimal(data.GetDataKeyValue("MaximumDiscount"))
                   //&& 
                   !(fromQuantity == 0 && toQuantity == 0 && discountAmount == 0)
                )
                {
                    DiscountRange discountRange = null;
                    if (itemID > 0)
                        discountRange = new DiscountRange(itemID);
                    else
                        discountRange = new DiscountRange();

                    discountRange.ProductID = productID;
                    discountRange.DiscountID = discountID;
                    discountRange.FromPrice = fromQuantity;
                    discountRange.ToPrice = toQuantity;
                    discountRange.DiscountAmount = discountAmount;
                    discountRange.MaximumDiscount = maximumDiscount;
                    discountRange.DiscountType = discountType;
                    discountRange.Save();
                }
            }
            ProductRanges = null;
            gridOrderDiscountRange.Rebind();
        }

        private void btnOrderRangeNewRow_Click(object sender, EventArgs e)
        {
            if (ProductRanges == null)
                ProductRanges = new List<DiscountRange>();
            else
                SaveTempDiscountRange(gridOrderDiscountRange);

            lstDiscountRange = ProductRanges;

            DiscountRange newRange = new DiscountRange
            {
                ProductID = productID,
                DiscountID = discountID,
                ItemID = GenTempDiscountID(lstDiscountRange)
            };
            lstDiscountRange.Add(newRange);

            ProductRanges = lstDiscountRange;
            gridOrderDiscountRange.Rebind();
        }

        #endregion Click Events

        #region Grid

        private void gridOrderDiscountRange_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            lstDiscountRange = ProductRanges;
            if (lstDiscountRange == null)
                lstDiscountRange = DiscountRange.GetRange(discountID, productID);
            ProductRanges = lstDiscountRange;

            gridOrderDiscountRange.DataSource = lstDiscountRange;
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
                        lstDiscountRange = ProductRanges;
                        var itemID = Int32.Parse(e.CommandArgument.ToString());
                        lstDiscountRange.RemoveAll(x => x.ItemID == itemID);
                        ProductRanges = lstDiscountRange;
                        gridOrderDiscountRange.Rebind();
                        break;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        #endregion Grid

        private void SaveTempDiscountRange(Telerik.Web.UI.RadGrid grid)
        {
            lstDiscountRange = ProductRanges;
            foreach (GridDataItem data in grid.Items)
            {
                var txtFromQuantity = (TextBox)data.FindControl("txtFromQuantity");
                var txtToQuantity = (TextBox)data.FindControl("txtToQuantity");
                var txtDiscountAmount = (TextBox)data.FindControl("txtDiscountAmount");
                var txtMaximumDiscount = (TextBox)data.FindControl("txtMaximumDiscount");
                var ddlDiscountType = (DropDownList)data.FindControl("ddlDiscountType");

                var itemID = Convert.ToInt32(data.GetDataKeyValue("ItemID"));
                decimal fromQuantity = Decimal.TryParse(txtFromQuantity.Text.Trim(), out fromQuantity) ? fromQuantity : 0;
                decimal toQuantity = Decimal.TryParse(txtToQuantity.Text.Trim(), out toQuantity) ? toQuantity : 0;
                decimal discountAmount = Decimal.TryParse(txtDiscountAmount.Text.Trim(), out discountAmount) ? discountAmount : 0;
                decimal maximumDiscount = Decimal.TryParse(txtMaximumDiscount.Text.Trim(), out maximumDiscount) ? maximumDiscount : 0;
                int discountType = Int32.Parse(ddlDiscountType.SelectedValue);

                var discountRange = lstDiscountRange.Where(x => x.ItemID == itemID).FirstOrDefault();
                if (discountRange != null)
                {
                    discountRange.FromPrice = fromQuantity;
                    discountRange.ToPrice = toQuantity;
                    discountRange.DiscountAmount = discountAmount;
                    discountRange.MaximumDiscount = maximumDiscount;
                    discountRange.DiscountType = discountType;
                }
            }

            ProductRanges = lstDiscountRange;
        }

        private int GenTempDiscountID(List<DiscountRange> discountRanges)
        {
            int itemID = 0;

            DiscountRange minItem = discountRanges.Where(x => x.ItemID < 0).OrderBy(x => x.ItemID).FirstOrDefault();
            if (minItem != null)
                itemID = minItem.ItemID - 1;
            else
                itemID = -(discountRanges.Count + 1);

            return itemID;
        }
    }
}
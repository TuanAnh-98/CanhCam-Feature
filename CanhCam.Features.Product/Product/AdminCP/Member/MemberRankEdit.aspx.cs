using CanhCam.Business;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class MemberRankEditPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(MemberRankEditPage));
        private int rankId = -1;
        private MemberRank memberRank = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            LoadParams();
            LoadSettings();
            PopulateLabels();

            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void LoadParams()
        {
            rankId = WebUtils.ParseInt32FromQueryString("ID", rankId);
        }

        private void LoadSettings()
        {
            if (rankId > 0)
                memberRank = new MemberRank(rankId);
            if (memberRank != null)
                btnInsert.Visible = true;
            else
            {
                btnUpdate.Visible = false;
                btnDelete.Visible = false;
            }
            HideControls();
        }

        private void HideControls()
        {
            btnInsert.Visible = false;
            btnUpdate.Visible = false;
            btnDelete.Visible = false;
            if (memberRank == null)
            {
                btnInsert.Visible = true;
            }
            else if (memberRank != null && memberRank.Id > 0)
            {
                btnUpdate.Visible = true;
                btnDelete.Visible = true;
            }
        }

        private void PopulateLabels()
        {
            heading.Text = ProductResources.MemberRankEditAdminTitle;
            Page.Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);

            UIHelper.AddConfirmationDialog(btnDelete, ProductResources.ProductDeleteWarning);
        }

        private void PopulateControls()
        {
            if (memberRank != null)
            {
                ddlRankOrder.Items.Add(new ListItem(memberRank.RankOrder.ToString(), memberRank.RankOrder.ToString()));
                txtDescription.Text = memberRank.Description;
                txtName.Text = memberRank.Name;

                txtPoint.Text = memberRank.Point > 0 ? memberRank.Point.ToString() : string.Empty;
                txtDiscountPercent.Text = memberRank.DiscountPercent > 0 ? memberRank.DiscountPercent.ToString() : string.Empty;
            }
            else
            {
                var maxMemberRank = new MemberRank(WorkingCulture.LanguageId, MemberHelper.GetMaxRank());
                //if (maxMemberRank.Id > 0 && maxTierPrice.Range != 0)
                if (maxMemberRank.Id > 0)
                    ddlRankOrder.Items.Add(new ListItem((maxMemberRank.RankOrder + 1).ToString(), (maxMemberRank.RankOrder + 1).ToString()));
                else if (maxMemberRank.Id == -1)
                {
                    ddlRankOrder.Items.Add(new ListItem("1", "1"));
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (memberRank != null)
            {
                var isDeleted = MemberRank.Delete(memberRank.Id);
                if (isDeleted)
                {
                    //TTCEHelper.ReorderTierPriceList();
                    MemberHelper.ReorderMemberRankOrderList();
                    MemberHelper.RearrangeMemberPointRange();

                    //TTCEHelper.UpdateAllTierPrice();
                    WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/MemberRank.aspx");
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (memberRank != null)
            {
                //tierPrice.Tier = Convert.ToInt32(txtTierOrder.Text);
                if (ddlRankOrder.Items.Count == 0)
                {
                    message.ErrorMessage = "Cập nhật không thành công";
                    return;
                }

                memberRank.RankOrder = Convert.ToInt32(ddlRankOrder.SelectedValue);

                memberRank.Description = txtDescription.Text;
                if (!string.IsNullOrEmpty(txtName.Text))
                    memberRank.Name = txtName.Text;
                else
                {
                    message.ErrorMessage = "Cập nhật không thành công";
                    return;
                }
                // write a function to check the adjacent point ranks

                Int32.TryParse(txtPoint.Text.Trim(), out int point);
                if (point > 0)
                    memberRank.Point = point;
                else
                {
                    message.ErrorMessage = "Cập nhật không thành công";
                    return;
                }

                decimal.TryParse(txtDiscountPercent.Text.Trim(), out decimal discountPercentage);
                if (discountPercentage > 0)
                    memberRank.DiscountPercent = discountPercentage;
                else
                {
                    message.ErrorMessage = "Cập nhật không thành công";
                    return;
                }

                //if (!ProductHelper.CheckValidRankPoint(memberRank))
                //{
                //    message.ErrorMessage = "Cập nhật không thành công";
                //    return;
                //}

                var save = memberRank.Save();
                MemberHelper.ReorderMemberRankOrderList();
                MemberHelper.RearrangeMemberPointRange();

                if (save) message.SuccessMessage = "Cập nhật thành công";
                else message.ErrorMessage = "Cập nhật không thành công";
            }
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            memberRank = new MemberRank();
            if (ddlRankOrder.Items.Count == 0)
            {
                message.ErrorMessage = "Cập nhật không thành công";
                return;
            }
            memberRank.RankOrder = Convert.ToInt32(ddlRankOrder.SelectedValue);

            memberRank.Description = txtDescription.Text;
            //memberRank.Name = txtName.Text;

            if (!string.IsNullOrEmpty(txtName.Text))
                memberRank.Name = txtName.Text;
            else
            {
                message.ErrorMessage = "Cập nhật không thành công";
                return;
            }

            Int32.TryParse(txtPoint.Text.Trim(), out int point);
            memberRank.Point = point;

            decimal.TryParse(txtDiscountPercent.Text.Trim(), out decimal discountPercentage);
            if (discountPercentage > 0)
                memberRank.DiscountPercent = discountPercentage;
            else
            {
                message.ErrorMessage = "Cập nhật không thành công";
                return;
            }

            //if (!ProductHelper.CheckValidRankPoint(memberRank))
            //{
            //    message.ErrorMessage = "Cập nhật không thành công";
            //    return;
            //}

            var tpId = memberRank.Save();

            if (tpId)
            {
                MemberHelper.RearrangeMemberPointRange();
                WebUtils.SetupRedirect(this, SiteRoot + "/Product/AdminCP/Member/MemberRankEdit.aspx?Id=" + memberRank.Id);
                message.SuccessMessage = "Thêm mới thành công";
                return;
            }
            else
            {
                message.ErrorMessage = "Thêm mới thất bại";
                return;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);

            this.btnUpdate.Click += new EventHandler(btnUpdate_Click);
            this.btnInsert.Click += new EventHandler(btnInsert_Click);
            this.btnDelete.Click += new EventHandler(btnDelete_Click);
        }
    }
}
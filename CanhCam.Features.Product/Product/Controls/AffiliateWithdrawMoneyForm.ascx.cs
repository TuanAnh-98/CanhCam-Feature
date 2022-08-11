using CanhCam.Business;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CanhCam.Web.ProductUI
{
    public partial class AffiliateWithdrawMoneyForm : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AffiliateWithdrawMoneyForm));
        int siteUtils = SiteUtils.GetCurrentSiteUser().UserId;
        UserAddress userAddress;
        AffiliatePayment affiliatePayment;


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = true;
            this.btnSubmit.Click += BtnSubmit_Click;
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                userAddress = UserAddress.GetByUser(siteUtils).First();
                userAddress.Email = txtEmail.Text;
                userAddress.FirstName = txtName.Text;
                userAddress.CreatedDate = DateTime.Parse(txtDateBirth.Text);
                userAddress.Street = txtAddress.Text;
                userAddress.Address = txtCity.Text;
                userAddress.Company = txtCompany.Text;
                userAddress.Save();

                affiliatePayment = new AffiliatePayment();
                affiliatePayment.AffUser = siteUtils;
                affiliatePayment.AffGuid = Guid.NewGuid();
                affiliatePayment.AffBankUserName = txtBankUserName.Text;
                affiliatePayment.AffBankNumber = txtBankNumber.Text;
                affiliatePayment.AffBankName = txtBankName.Text;
                affiliatePayment.AffBankBranch = txtBankBranch.Text;
                affiliatePayment.WithdrawMoney = Convert.ToDecimal(txtWithdrawMoney.Text);
                affiliatePayment.Save();

                PopulateControls();
                message.SuccessMessage = "Insert Success!";
            }
            catch
            {
                message.ErrorMessage = "Insert Fail!";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack) { PopulateControls(); }
        }

        private void PopulateControls()
        {
            userAddress = UserAddress.GetByUser(siteUtils).First();
            if (userAddress.UserId > 0)
            {
                txtEmail.Text = userAddress.Email;
                txtName.Text = userAddress.FirstName + " " + userAddress.LastName;
                txtSex.Text = "Nam";
                txtDateBirth.Text = userAddress.CreatedDate.ToString();
                txtAddress.Text = userAddress.Street;
                txtCity.Text = userAddress.Address;
                txtCompany.Text = userAddress.Company;
            }
            else
            {
                txtEmail.Text = "";
                txtName.Text = "";
                txtSex.Text = "";
                txtDateBirth.Text = "";
                txtAddress.Text = "";
                txtCity.Text = "";
                txtCompany.Text = "";
            }
            affiliatePayment = new AffiliatePayment(siteUtils);
            if (affiliatePayment.AffUser > 0)
            {
                txtBankUserName.Text = affiliatePayment.AffBankUserName;
                txtBankNumber.Text = affiliatePayment.AffBankNumber;
                txtBankName.Text = affiliatePayment.AffBankName;
                txtBankBranch.Text = affiliatePayment.AffBankBranch;
                
            }
            else
            {
                txtBankUserName.Text = "";
                txtBankNumber.Text = "";
                txtBankName.Text = "";
                txtBankBranch.Text = "";
            }

        }
    }
}
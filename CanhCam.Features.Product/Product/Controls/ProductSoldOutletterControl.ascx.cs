using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Net;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Text;
using System.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductSoldOutletterControl : UserControl
    {
        #region Properties

        private static readonly ILog log = LogManager.GetLogger(typeof(ProductSoldOutletterControl));
        private Product product = null;

        public Product Product
        {
            get { return product; }
            set
            {
                product = value;
            }
        }

        #endregion Properties

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            this.Load += new EventHandler(this.Page_Load);
            this.btnSubmit.Click += new EventHandler(BtnSubmit_Click);
            base.OnInit(e);
        }

        private void BtnSubmit_Click(object sender, EventArgs e)
        {
            Page.Validate("ProductSoldOutletter");
            if (!Page.IsValid)
                return;

            SiteSettings siteSettings = CacheHelper.GetCurrentSiteSettings();
            //Try get Product by url
            if (product == null)
            {
                var productId = WebUtils.ParseInt32FromQueryString("ProductID", -1);
                product = new Product(siteSettings.SiteId, productId);
            }
            if (product == null)
                return;
            int.TryParse(txtQuantity.Text, out int quantity);

            ProductSoldOutLetter letter = new ProductSoldOutLetter()
            {
                CreateDate = DateTime.Now,
                Email = txtEmail.Text,
                IpAddress = SiteUtils.GetIP4Address(),
                Phone = txtPhoneNumber.Text,
                ProductID = product.ProductId,
                ProductName = product.Title,
                Quantity = quantity,
                FullName = txtFullName.Text,
                Url = this.Request.RawUrl
            };
            var user = SiteUtils.GetCurrentSiteUser();
            if (user != null)
                letter.UserId = user.UserId;

            if (letter.Save())
            {
                this.pnlfrmLetter.Visible = false;
                this.lblMessage.Text = MessageTemplate.GetMessage("ProductSoldOutLetterThankYouMessage", ProductResources.ThankYouLabel);
                SendMail(siteSettings, letter, product, letter.Email, "ProductSoldOutLetterCustomer");
                SendMail(siteSettings, letter, product, "", "ProductSoldOutLetterManager");
                WebTaskManager.StartOrResumeTasks();
            }
        }

        private void SendMail(SiteSettings siteSettings, ProductSoldOutLetter letter, Product product, string toEmail, string templateSystemCode)
        {
            EmailTemplate template = EmailTemplate.Get(product.SiteId, templateSystemCode, WorkingCulture.LanguageId);
            string subjectEmail = template.Subject.Replace("{SiteName}", siteSettings.SiteName);

            StringBuilder messageEmail = new StringBuilder();
            messageEmail.Append(template.HtmlBody);
            messageEmail.Replace("{SiteName}", siteSettings.SiteName);
            messageEmail.Replace("{FullName}", letter.FullName);
            messageEmail.Replace("{Email}", letter.Email);
            messageEmail.Replace("{IpAddress}", letter.IpAddress);
            messageEmail.Replace("{Phone}", letter.Phone);
            messageEmail.Replace("{ProductName}", letter.ProductName);
            messageEmail.Replace("{Quantity}", letter.Quantity.ToString());
            string siteRoot = SiteUtils.GetNavigationSiteRoot();
            messageEmail.Replace("{Url}", siteRoot + letter.Url);
            messageEmail.Replace("{CreateDate}", letter.CreateDate.ToString("dd-MM-yyyy hh:mm"));

            SmtpSettings smtpSettings = SiteUtils.GetSmtpSettings();
            EmailMessageTask messageTask = new EmailMessageTask(smtpSettings)
            {
                EmailFrom = siteSettings.DefaultEmailFromAddress,
                EmailTo = toEmail + (template.ToAddresses.Length == 0 ? string.Empty : "," + template.ToAddresses),
                EmailCc = (template.CcAddresses.Length == 0 ? string.Empty : "," + template.CcAddresses),
                EmailBcc = (template.BccAddresses.Length == 0 ? string.Empty : "," + template.BccAddresses),
                EmailReplyTo = template.ReplyToAddress,
                EmailFromAlias = template.FromName,
                UseHtml = true,
                SiteGuid = siteSettings.SiteGuid,
                Subject = subjectEmail,
                HtmlBody = messageEmail.ToString()
            };
            messageTask.QueueTask();
        }

        #endregion OnInit

        private void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                PopulateControls();
        }

        protected virtual void PopulateControls()
        {
            litTitle.Text = "Liên hệ khi có hàng \"" + product.Title + "\"";
        }
    }
}
using CanhCam.Business;
using CanhCam.Net;
using CanhCam.Web.Framework;
using log4net;
using Resources;
using System;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ProductSoldOutLetterPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ProductSoldOutLetterPage));
        private RadGridSEOPersister gridPersister;
        protected Double timeOffset = 0;
        protected TimeZoneInfo timeZone = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }
            //if (!WebUser.IsAdmin && !FormPermission.CanManageFormRegisterBorrow)
            //{
            //    SiteUtils.RedirectToEditAccessDeniedPage();
            //    return;
            //}

            LoadSettings();
        }

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.grid.NeedDataSource += new Telerik.Web.UI.GridNeedDataSourceEventHandler(grid_NeedDataSource);
            this.grid.ItemDataBound += Grid_ItemDataBound;
            gridPersister = new RadGridSEOPersister(grid);
            btnUpdate.Click += BtnUpdate_Click;
            btnExport.Click += new EventHandler(BtnExport_Click);
            btnDetele.Click += new EventHandler(BtnDetele_Click);
            this.btnSendMail.Click += new EventHandler(BtnSendMail_Click);
            btnSearch.Click += new EventHandler(BtnSearch_Click);
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            bool updated = false;
            foreach (GridDataItem item in grid.SelectedItems)
            {
                int rowId = Convert.ToInt32(item.GetDataKeyValue("RowId"));
                bool isContacted = Convert.ToBoolean(item.GetDataKeyValue("IsContacted"));
                var cbIsContacted = (CheckBox)item.FindControl("cbIsContacted");
                if (cbIsContacted.Checked != isContacted)
                {
                    ProductSoldOutLetter letter = new ProductSoldOutLetter(rowId);
                    if (letter != null && letter.RowId > 0)
                    {
                        letter.IsContacted = cbIsContacted.Checked;
                        updated = letter.Save();
                    }
                }
            }
            if (updated)
            {
                grid.Rebind();
                message.SuccessMessage = "Updated";
            }
        }

        private void Grid_ItemDataBound(object sender, GridItemEventArgs e)
        {
            if (e.Item is Telerik.Web.UI.GridDataItem)
            {
                Telerik.Web.UI.GridDataItem item = (Telerik.Web.UI.GridDataItem)e.Item;
                string color = "#ffc10745";
                int productID = Convert.ToInt32(item.GetDataKeyValue("ProductID"));
                int quantity = Convert.ToInt32(item.GetDataKeyValue("Quantity"));
                int status = Convert.ToInt32(item.GetDataKeyValue("Status"));
                var litProductStockQuantity = (Literal)item.FindControl("litProductStockQuantity");
                Product product = new Product(siteSettings.SiteId, productID);
                bool isDeleted = false;
                if (product != null)
                {
                    if (product.StockQuantity >= quantity)
                    {
                        color = "#28a7459e";
                    }
                    litProductStockQuantity.Text = product.StockQuantity.ToString();
                    if (product.IsDeleted)
                        isDeleted = true;
                }
                else
                    isDeleted = true;

                if (isDeleted)
                    color = "#ff0707";

                e.Item.Attributes.Add("style", "background-color: " + color + ";");

            }
        }

        private void LoadSettings()
        {
            timeOffset = SiteUtils.GetUserTimeOffset();
            timeZone = SiteUtils.GetUserTimeZone();
            Title = SiteUtils.FormatPageTitle(siteSettings, "Danh sách thông báo khi có hàng");
            heading.Text = "Danh sách thông báo khi có hàng";
        }

        private void BtnSendMail_Click(object sender, EventArgs e)
        {
            bool sended = false;
            StringBuilder stringBuilder = new StringBuilder();

            foreach (Telerik.Web.UI.GridDataItem data in grid.SelectedItems)
            {
                int rowId = Convert.ToInt32(data.GetDataKeyValue("RowId"));
                int productID = Convert.ToInt32(data.GetDataKeyValue("ProductID"));
                int quantity = Convert.ToInt32(data.GetDataKeyValue("Quantity"));
                var litProductStockQuantity = (Literal)data.FindControl("litProductStockQuantity");
                Product product = new Product(siteSettings.SiteId, productID);

                ProductSoldOutLetter letter = new ProductSoldOutLetter(rowId);
                if (letter != null)
                {
                    if (product.StockQuantity >= quantity)
                    {

                        SendMail(letter, letter.Email, "ProductInStockMailTemplate");
                        letter.Status = (int)ProductSoldOutLetterStatus.Sented;
                        letter.Save();
                        sended = true;
                    }
                    else
                    {
                        stringBuilder.Append("Không đủ hàng cho đơn của " + letter.Email + "<br/>");
                    }
                }
            }
            if (sended)
            {
                message.SuccessMessage = ProductResources.MailSuccessfullySentLabel;
                WebTaskManager.StartOrResumeTasks();
                grid.Rebind();
            }
            if (stringBuilder.Length > 0)
                message.WarningMessage = stringBuilder.ToString();
        }

        protected string GetStatus(int status)
        {
            switch (status)
            {
                case (int)ProductSoldOutLetterStatus.Sented:
                    return "Đã gửi";

                default:
                    return "Chưa gửi";
            }
        }

        private void SendMail(ProductSoldOutLetter letter, string toEmail, string templateSystemCode)
        {
            EmailTemplate template = EmailTemplate.Get(siteSettings.SiteId, templateSystemCode, WorkingCulture.LanguageId);
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
            messageEmail.Replace("{IsContacted}", letter.IsContacted ? "Đã liên hệ" : "Chưa liên hệ");

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

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            grid.Rebind();
        }

        private void BtnDetele_Click(object sender, EventArgs e)
        {
            bool deleted = false;
            foreach (GridDataItem item in grid.SelectedItems)
            {
                int newsId = Convert.ToInt32(item.GetDataKeyValue("RowId"));
                if (ProductSoldOutLetter.Delete(newsId))
                    deleted = true;
            }
            if (deleted)
            {
                grid.Rebind();
                message.SuccessMessage = "Xóa thành công";
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            DataTable dt = GetDataForExport();

            string fileName = "ProductSoldOutLetter-data-export-" + DateTimeHelper.GetDateTimeStringForFileName() + ".xls";

            ExportHelper.ExportDataTableToExcel(HttpContext.Current, dt, fileName);
        }

        private DataTable GetDataForExport()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("STT", typeof(int));
            dt.Columns.Add("Email", typeof(string));
            dt.Columns.Add("Tên Sản phẩm", typeof(string));
            dt.Columns.Add("Đường dẫn đăng ký", typeof(string));
            dt.Columns.Add("Ngày đăng ký", typeof(string));

            var lst = ProductSoldOutLetter.GetAll();
            int i = 1;
            foreach (ProductSoldOutLetter item in lst)
            {
                DataRow row = dt.NewRow();
                row["STT"] = i;
                row["Email"] = item.Email;
                row["Tên Sản phẩm"] = item.ProductName;
                row["Đường dẫn đăng ký"] = item.Url;
                row["Ngày đăng ký"] = item.CreateDate.ToString("dd/MM/yyyy hh:mm tt");
                i += 1;
                dt.Rows.Add(row);
            }
            return dt;
        }

        #endregion OnInit

        #region "RadGrid Event"

        private void grid_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            DateTime? fromdate = null;
            DateTime? todate = null;

            if (dpStartDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpStartDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 0, 0, 0);

                if (timeZone != null)
                    fromdate = localTime.ToUtc(timeZone);
                else
                    fromdate = localTime.AddHours(-timeOffset);
            }
            if (dpEndDate.Text.Trim().Length > 0)
            {
                DateTime localTime = DateTime.Parse(dpEndDate.Text);
                localTime = new DateTime(localTime.Year, localTime.Month, localTime.Day, 23, 59, 59);

                if (timeZone != null)
                    todate = localTime.ToUtc(timeZone);
                else
                    todate = localTime.AddHours(-timeOffset);
            }

            grid.PagerStyle.EnableSEOPaging = false;
            bool isApplied = gridPersister.IsAppliedSortFilterOrGroup;
            int iCount = ProductSoldOutLetter.GetCountBySearch(fromdate, todate, txtTitle.Text.Trim());
            int startRowIndex = isApplied ? 1 : grid.CurrentPageIndex + 1;
            int maximumRows = isApplied ? iCount : grid.PageSize;
            grid.VirtualItemCount = iCount;
            grid.AllowCustomPaging = !isApplied;
            grid.DataSource = ProductSoldOutLetter.GetPageBySearch(fromdate, todate, txtTitle.Text.Trim(), startRowIndex, maximumRows);
        }

        #endregion "RadGrid Event"
    }
}
using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Linq;
using System.Text;

namespace CanhCam.Web.ProductUI
{
    public partial class DeleteRedirectUrls : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(DeleteRedirectUrls));
        private SiteUser siteUser;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.IsAuthenticated)
            {
                SiteUtils.RedirectToLoginPage(this);
                return;
            }

            LoadSettings();

            if (!WebUser.IsAdmin && !ProductPermission.CanImport)
            {
                SiteUtils.RedirectToEditAccessDeniedPage();
                return;
            }

            PopulateLabels();
            if (!Page.IsPostBack)
                PopulateControls();
        }

        private void PopulateControls()
        {
        }

        private void PopulateLabels()
        {
            heading.Text = breadcrumb.CurrentPageTitle;
            Title = SiteUtils.FormatPageTitle(siteSettings, heading.Text);
        }

        private void LoadSettings()
        {
            siteUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("admin-importproducts");
        }

        #region Import


        private const string OldUrlColumnName = "OldUrl";
        private const string NewUrlColumnName = "NewUrl";

        private void btnGetData_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            if (fileUpload.UploadedFiles.Count == 0)
            {
                message.ErrorMessage = "Vui lòng nhập tập tin.";
                return;
            }

            int i = 1;
            try
            {
                var workbook = new HSSFWorkbook(fileUpload.UploadedFiles[0].InputStream, true);
                var worksheet = workbook.GetSheetAt(0);
                if (worksheet == null)
                    return;

                var errorMessage = string.Empty;


                int oldUrlColumnNo = -1;
                int newUrlColumnNo = -1;
                var titleRowNumber = 0;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case OldUrlColumnName:
                            oldUrlColumnNo = columNumber;
                            break;

                        case NewUrlColumnName:
                            newUrlColumnNo = columNumber;
                            break;
                    }

                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));
                int count = 0;
                StringBuilder builder = new StringBuilder();
                var redirects = RedirectInfo.GetPage(siteSettings.SiteId, 1, 214748);
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string oldUrl = GetValueFromExcel(dataRow.GetCell(oldUrlColumnNo)).Trim();
                        string newUrl = GetValueFromExcel(dataRow.GetCell(newUrlColumnNo)).Trim();
                        var urls = redirects.Where(z => z.OldUrl == GetPathFromUrl(oldUrl) && z.NewUrl == GetPathFromUrl(newUrl));
                        foreach (var item in urls)
                        {
                            if (RedirectInfo.Delete(item.RowGuid))
                            {
                                builder.Append($"<p>Old Url : {item.OldUrl}   =>  New Url  : {item.NewUrl}</p>");
                                count++;
                            }
                        }

                    }
                }

                if (count > 0)
                {
                    message.SuccessMessage = "<p><strong>Imported " + count + " Urls</strong</p>";
                    litResult.Text = builder.ToString();
                }

                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }

        private string GetPathFromUrl(string url)
        {


            bool result = Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
            {
                return uriResult.AbsolutePath.Substring(1);//remove start Start '/'
            }
            if (url.StartsWith("/"))
                url = url.Substring(1);
            return url;
        }

        public static string GetValueFromExcel(ICell obj)
        {
            if (obj == null)
                return string.Empty;

            switch (obj.CellType)
            {
                case CellType.String:
                    return obj.StringCellValue;

                case CellType.Numeric:
                    if (DateUtil.IsCellDateFormatted(obj))
                    {
                        if (obj.DateCellValue != null)
                        {
                            return obj.DateCellValue.ToString("yyyy/MM/dd HH:mm:00");
                        }

                        return string.Empty;
                    }

                    return obj.NumericCellValue.ToString();

                default:
                    obj.SetCellType(CellType.String);
                    return obj.StringCellValue;
            }
        }

        public static ICell CreateCell(ref IRow dataRow, int column, object value, ICellStyle style, IFont font)
        {
            ICell cell = dataRow.CreateCell(column);
            if (value != null)
                cell.SetCellValue(value.ToString());
            else
                cell.SetCellValue(string.Empty);

            if (style != null)
                cell.CellStyle = style;
            if (font != null)
                cell.CellStyle.SetFont(font);

            return cell;
        }

        #endregion Import

        #region OnInit

        override protected void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnGetData.Click += new EventHandler(btnGetData_Click);
        }

        #endregion OnInit
    }
}
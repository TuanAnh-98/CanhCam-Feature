using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.FileSystem;
using CanhCam.SearchIndex;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportManufacturers : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportManufacturers));
        private IFileSystem fileSystem = null;
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
            var p = FileSystemManager.Providers[WebConfigSettings.FileSystemProvider];
            if (p != null) { fileSystem = p.GetFileSystem(); }

            siteUser = SiteUtils.GetCurrentSiteUser();

            AddClassToBody("admin-importproducts");
        }

        #region Import

        private const string NameColumnName = "Name";

        private void BtnGetData_Click(object sender, EventArgs e)
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

                int nameColumnNo = -1;
                var titleRowNumber = 0;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case NameColumnName:
                            nameColumnNo = columNumber;
                            break;
                    }

                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));
                int count = 0;
                List<int> productsUpdated = new List<int>();
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string name = GetValueFromExcel(dataRow.GetCell(nameColumnNo)).Trim();
                        if (string.IsNullOrEmpty(name)) continue;
                        Manufacturer manufacturer = new Manufacturer
                        {
                            Name = name
                        };
                        var friendlyUrlString = string.Empty;
                        string url = "~/" + SiteUtils.SuggestFriendlyUrl(name, siteSettings);
                        manufacturer.Url = url;
                        manufacturer.PageId = 1051;
                        manufacturer.IsPublished = true;

                        friendlyUrlString = SiteUtils.RemoveInvalidUrlChars(url.Replace("~/", String.Empty));

                        if ((friendlyUrlString.EndsWith("/")) && (!friendlyUrlString.StartsWith("http")))
                            friendlyUrlString = friendlyUrlString.Substring(0, friendlyUrlString.Length - 1);
                        if (manufacturer.Save())
                        {
                            FriendlyUrl newFriendlyUrl = new FriendlyUrl
                            {
                                SiteId = siteSettings.SiteId,
                                SiteGuid = siteSettings.SiteGuid,
                                PageGuid = manufacturer.Guid,
                                Url = friendlyUrlString,
                                RealUrl = "~/Product/Manufacturer.aspx"
                                    + "?ManufacturerID=" + manufacturer.ManufacturerId.ToInvariantString()
                            };
                            newFriendlyUrl.Save();
                        }
                    }
                }

                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }

        private void product_ContentChanged(object sender, ContentChangedEventArgs e)
        {
            IndexBuilderProvider indexBuilder = IndexBuilderManager.Providers["ProductIndexBuilderProvider"];
            if (indexBuilder != null)
            {
                indexBuilder.ContentChangedHandler(sender, e);
            }
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(this.Page_Load);

            this.btnGetData.Click += new EventHandler(BtnGetData_Click);
        }

        #endregion OnInit
    }
}
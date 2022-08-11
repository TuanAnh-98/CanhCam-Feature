using CanhCam.Business;
using CanhCam.Business.WebHelpers;
using CanhCam.Web.Framework;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using Telerik.Web.UI;

namespace CanhCam.Web.ProductUI
{
    public partial class ImportProductsMetaPage : CmsNonBasePage
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImportProductsMetaPage));

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
            AddClassToBody("admin-importproducts");
        }

        #region Import

        private const string ProductItemNoColumnName = "ItemNo";
        private const string MetaTitleColumnName = "MetaTitle";
        private const string MetaDescriptionColumnName = "MetaDescription";
        private const string MetaKeywordsColumnName = "MetaKeywords";
        private const string AdditionalMetaTagsColumnName = "AdditionalMetaTags";

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

                int productItemNoColumnNo = -1;
                int metaTitleColumnNo = -1;
                int metaDescriptionColumnNo = -1;
                int metaKeywordsColumnNo = -1;
                int additionalMetaTagsColumnNo = -1;

                var titleRowNumber = 1;
                var columNumber = 0;
                var columName = string.Empty;
                do
                {
                    columName = GetValueFromExcel(worksheet.GetRow(titleRowNumber).GetCell(columNumber)).Trim();
                    switch (columName)
                    {
                        case ProductItemNoColumnName:
                            productItemNoColumnNo = columNumber;
                            break;

                        case MetaTitleColumnName:
                            metaTitleColumnNo = columNumber;
                            break;

                        case MetaDescriptionColumnName:
                            metaDescriptionColumnNo = columNumber;
                            break;

                        case MetaKeywordsColumnName:
                            metaKeywordsColumnNo = columNumber;
                            break;

                        case AdditionalMetaTagsColumnName:
                            additionalMetaTagsColumnNo = columNumber;
                            break;
                    }
                    columNumber += 1;
                } while (!string.IsNullOrEmpty(columName));

                var existsColumnName = "Exists";
                var dt = new DataTable();
                dt.Columns.Add(ProductItemNoColumnName, typeof(string));
                dt.Columns.Add(MetaTitleColumnName, typeof(string));
                dt.Columns.Add(MetaDescriptionColumnName, typeof(string));
                dt.Columns.Add(MetaKeywordsColumnName, typeof(string));
                dt.Columns.Add(AdditionalMetaTagsColumnName, typeof(string));
                dt.Columns.Add(existsColumnName, typeof(bool));
                for (i = titleRowNumber + 1; i <= worksheet.LastRowNum; i++)
                {
                    var dataRow = worksheet.GetRow(i);
                    if (dataRow != null)
                    {
                        string productItemNo = GetValueFromExcel(dataRow.GetCell(productItemNoColumnNo)).Trim();
                        string metaTitle = GetValueFromExcel(dataRow.GetCell(metaTitleColumnNo)).Trim();
                        string metaDescription = GetValueFromExcel(dataRow.GetCell(metaDescriptionColumnNo)).Trim();
                        string metaKeyword = GetValueFromExcel(dataRow.GetCell(metaKeywordsColumnNo)).Trim();
                        string additionalMetaTags = GetValueFromExcel(dataRow.GetCell(additionalMetaTagsColumnNo)).Trim();

                        if (string.IsNullOrEmpty(productItemNo))
                            continue;

                        var row = dt.NewRow();
                        row[ProductItemNoColumnName] = productItemNo;
                        row[MetaTitleColumnName] = metaTitle;
                        row[MetaDescriptionColumnName] = metaDescription;
                        row[MetaKeywordsColumnName] = metaKeyword;
                        row[AdditionalMetaTagsColumnName] = additionalMetaTags;
                        var product = Product.GetByCode(siteSettings.SiteId, productItemNo);
                        if (product != null)
                            row[existsColumnName] = true;
                        else
                            row[existsColumnName] = false;
                        dt.Rows.Add(row);
                    }
                }

                btnGetData.Visible = false;
                btnImport.Visible = true;
                grid.DataSource = dt;
                grid.DataBind();

                message.InfoMessage = "Số sản phẩm đọc được: " + dt.Rows.Count;
                if (errorMessage.Length > 0)
                    message.ErrorMessage = errorMessage;
            }
            catch (Exception ex)
            {
                message.ErrorMessage = "Lỗi đọc dữ liệu dòng: " + i.ToString() + "<br />" + ex.Message;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
                return;

            try
            {
                var numberProductsImported = 0;
                foreach (GridDataItem data in grid.Items)
                {
                    string productItemNo = data.GetDataKeyValue(ProductItemNoColumnName).ToString();
                    string metaTitle = data.GetDataKeyValue(MetaTitleColumnName).ToString();
                    string metaDescription = data.GetDataKeyValue(MetaDescriptionColumnName).ToString();
                    string metaKeywords = data.GetDataKeyValue(MetaKeywordsColumnName).ToString();
                    string additionalMetaTags = data.GetDataKeyValue(AdditionalMetaTagsColumnName).ToString();

                    if (!string.IsNullOrEmpty(productItemNo))
                    {
                        Product product = Product.GetByCode(siteSettings.SiteId, productItemNo);
                        if (product != null && product.ProductId > 0)
                        {
                            if (metaTitle.ToLower().Trim() != "noedit")
                                product.MetaTitle = metaTitle;
                            if (metaDescription.ToLower().Trim() != "noedit")
                                product.MetaDescription = metaDescription;
                            if (metaKeywords.ToLower().Trim() != "noedit")
                                product.MetaKeywords = metaKeywords;
                            if (additionalMetaTags.ToLower().Trim() != "noedit")
                                product.AdditionalMetaTags = additionalMetaTags;
                            product.Save();
                            numberProductsImported++;
                        }
                    }
                }

                message.SuccessMessage = string.Format("Đã import {0} sản phẩm thành công", numberProductsImported);
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        public static CustomField GetCustomFieldByName(List<CustomField> lstCustomFields, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                foreach (CustomField field in lstCustomFields)
                {
                    if (StringHelper.IsCaseInsensitiveMatch(field.Name, name))
                        return field;
                }

            return null;
        }

        public static CustomFieldOption GetCustomFieldOptionByName(List<CustomFieldOption> lstCustomFieldOptions, string name)
        {
            if (!string.IsNullOrWhiteSpace(name))
                foreach (CustomFieldOption option in lstCustomFieldOptions)
                {
                    if (StringHelper.IsCaseInsensitiveMatch(option.Name, name))
                        return option;
                }

            return null;
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

            this.btnImport.Click += new EventHandler(btnImport_Click);
            this.btnGetData.Click += new EventHandler(btnGetData_Click);
        }

        #endregion OnInit
    }
}
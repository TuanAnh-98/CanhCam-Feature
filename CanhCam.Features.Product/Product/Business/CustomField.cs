// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
// Created:					2014-07-24
// Last Modified:			2014-07-24

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum CustomFieldDataType
    {
        Text = 1,
        SelectBox = 2,
        CheckBox = 3
    }

    //public enum CustomFieldDataType
    //{
    //    /// <summary>
    //    /// Dropdown list
    //    /// </summary>
    //    DropdownList = 1,
    //    /// <summary>
    //    /// Radio list
    //    /// </summary>
    //    RadioList = 2,
    //    /// <summary>
    //    /// Checkboxes
    //    /// </summary>
    //    Checkboxes = 3,
    //    /// <summary>
    //    /// TextBox
    //    /// </summary>
    //    TextBox = 4,
    //    /// <summary>
    //    /// Multiline textbox
    //    /// </summary>
    //    MultilineTextbox = 10,
    //    /// <summary>
    //    /// Datepicker
    //    /// </summary>
    //    Datepicker = 20,
    //    /// <summary>
    //    /// File upload control
    //    /// </summary>
    //    FileUpload = 30,
    //    /// <summary>
    //    /// Color squares
    //    /// </summary>
    //    ColorSquares = 40,
    //    /// <summary>
    //    /// Read-only checkboxes
    //    /// </summary>
    //    ReadonlyCheckboxes = 50,
    //}

    public enum CustomFieldType
    {
        Normal = -1,
        Color = 1,
        //Number = 2
        //PriceRange = 3
    }

    public enum CustomFieldFilterType
    {
        NotAllowFiltering = 0,
        ByValue = 1,
        ByMultipleValues = 2
    }

    public enum CustomFieldOptions
    {
        ShowInCatalogPages = 1,
        ShowInProductDetailsPage = 2,
        EnableShoppingCart = 4,
        EnableComparing = 8,
        EnableGenerateInTitle = 16
    }

    public class CustomField
    {
        #region Constructors

        public CustomField()
        { }

        public CustomField(int customFieldId)
        {
            this.GetCustomField(customFieldId);
        }

        #endregion Constructors

        #region Private Properties

        private int customFieldID = -1;
        private int siteID = -1;
        private string name = string.Empty;
        private string displayName = string.Empty;
        private int dataType = -1;
        private int fieldType = -1;
        private int filterType = -1;
        private bool isEnabled = true;
        private bool isRequired = false;
        private string validationExpression = string.Empty;
        private string invalidMessage = string.Empty;
        private bool allowComparing = false;
        private int displayOrder = 0;
        private Guid featureGuid = Guid.Empty;
        private Guid guid = Guid.Empty;
        private int options = 0;

        #endregion Private Properties

        #region Public Properties

        public int CustomFieldId
        {
            get { return customFieldID; }
            set { customFieldID = value; }
        }

        public int SiteId
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string DisplayName
        {
            get { return displayName; }
            set { displayName = value; }
        }

        public int DataType
        {
            get { return dataType; }
            set { dataType = value; }
        }

        public int FieldType
        {
            get { return fieldType; }
            set { fieldType = value; }
        }

        public int FilterType
        {
            get { return filterType; }
            set { filterType = value; }
        }

        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public bool IsRequired
        {
            get { return isRequired; }
            set { isRequired = value; }
        }

        public string ValidationExpression
        {
            get { return validationExpression; }
            set { validationExpression = value; }
        }

        public string InvalidMessage
        {
            get { return invalidMessage; }
            set { invalidMessage = value; }
        }

        public bool AllowComparing
        {
            get { return allowComparing; }
            set { allowComparing = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int Options
        {
            get { return options; }
            set { options = value; }
        }

        public bool IsNonCombinable
        {
            get
            {
                if (this.DataType == (int)CustomFieldDataType.Text)
                    //this.DataType == CustomFieldDataType.MultilineTextbox ||
                    //this.DataType == CustomFieldDataType.Datepicker ||
                    //this.DataType == CustomFieldDataType.FileUpload)
                    return false;

                return true;
            }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of CustomField.
        /// </summary>
        /// <param name="customFieldID"> customFieldID </param>
        private void GetCustomField(
            int customFieldID)
        {
            using (IDataReader reader = DBCustomField.GetOne(
                customFieldID))
            {
                if (reader.Read())
                {
                    this.customFieldID = Convert.ToInt32(reader["CustomFieldID"]);
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.name = reader["Name"].ToString();
                    this.displayName = reader["DisplayName"].ToString();
                    this.dataType = Convert.ToInt32(reader["DataType"]);
                    this.fieldType = Convert.ToInt32(reader["FieldType"]);
                    this.filterType = Convert.ToInt32(reader["FilterType"]);
                    this.isEnabled = Convert.ToBoolean(reader["IsEnabled"]);
                    this.isRequired = Convert.ToBoolean(reader["IsRequired"]);
                    this.validationExpression = reader["ValidationExpression"].ToString();
                    this.invalidMessage = reader["InvalidMessage"].ToString();
                    this.allowComparing = Convert.ToBoolean(reader["AllowComparing"]);
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                    this.guid = new Guid(reader["Guid"].ToString());
                    if (reader["Options"] != DBNull.Value)
                        this.options = Convert.ToInt32(reader["Options"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of CustomField. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();

            newID = DBCustomField.Create(
                this.siteID,
                this.name,
                this.displayName,
                this.dataType,
                this.fieldType,
                this.filterType,
                this.isEnabled,
                this.isRequired,
                this.validationExpression,
                this.invalidMessage,
                this.allowComparing,
                this.displayOrder,
                this.featureGuid,
                this.guid,
                this.options);

            this.customFieldID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBCustomField.Update(
                this.customFieldID,
                this.siteID,
                this.name,
                this.displayName,
                this.dataType,
                this.fieldType,
                this.filterType,
                this.isEnabled,
                this.isRequired,
                this.validationExpression,
                this.invalidMessage,
                this.allowComparing,
                this.displayOrder,
                this.featureGuid,
                this.guid,
                this.options);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.customFieldID > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Create();
            }
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int customFieldID)
        {
            return DBCustomField.Delete(customFieldID);
        }

        private static List<CustomField> LoadListFromReader(IDataReader reader)
        {
            List<CustomField> customFieldList = new List<CustomField>();
            try
            {
                while (reader.Read())
                {
                    CustomField customField = new CustomField
                    {
                        customFieldID = Convert.ToInt32(reader["CustomFieldID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        name = reader["Name"].ToString(),
                        displayName = reader["DisplayName"].ToString(),
                        dataType = Convert.ToInt32(reader["DataType"]),
                        fieldType = Convert.ToInt32(reader["FieldType"]),
                        filterType = Convert.ToInt32(reader["FilterType"]),
                        isEnabled = Convert.ToBoolean(reader["IsEnabled"]),
                        isRequired = Convert.ToBoolean(reader["IsRequired"]),
                        validationExpression = reader["ValidationExpression"].ToString(),
                        invalidMessage = reader["InvalidMessage"].ToString(),
                        allowComparing = Convert.ToBoolean(reader["AllowComparing"]),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        featureGuid = new Guid(reader["FeatureGuid"].ToString()),
                        guid = new Guid(reader["Guid"].ToString())
                    };
                    if (reader["Options"] != DBNull.Value)
                        customField.options = Convert.ToInt32(reader["Options"]);
                    customFieldList.Add(customField);
                }
            }
            finally
            {
                reader.Close();
            }

            return customFieldList;
        }

        public static List<CustomField> GetByFeature(int siteId, Guid featureGuid)
        {
            IDataReader reader = DBCustomField.GetByFeature(siteId, featureGuid);
            return LoadListFromReader(reader);
        }

        public static List<CustomField> GetActive(int siteId, Guid featureGuid)
        {
            return GetActive(siteId, featureGuid, -1);
        }

        public static List<CustomField> GetActive(int siteId, Guid featureGuid, int languageId = -1)
        {
            IDataReader reader = DBCustomField.GetActive(siteId, featureGuid, languageId);
            return LoadListFromReader(reader);
        }

        public static List<CustomField> GetActiveForCart(int siteId, Guid featureGuid)
        {
            var lstCustomFields = GetActive(siteId, featureGuid, -1);

            List<CustomField> lst = new List<CustomField>();

            foreach (CustomField field in lstCustomFields)
                if (
                    (field.options & (int)CustomFieldOptions.EnableShoppingCart) > 0
                    && field.IsNonCombinable
                    )
                    lst.Add(field);

            return lst;
        }

        public static List<CustomField> GetActiveByZone(int siteId, Guid featureGuid, Guid zoneGuid)
        {
            return GetActiveByZone(siteId, featureGuid, zoneGuid, -1);
        }

        public static List<CustomField> GetActiveByZone(int siteId, Guid featureGuid, Guid zoneGuid, int languageId = -1)
        {
            IDataReader reader = DBCustomField.GetActiveByZone(siteId, featureGuid, zoneGuid, languageId);
            return LoadListFromReader(reader);
        }

        public static List<CustomField> GetActiveByFields(int siteId, Guid featureGuid, List<int> fieldIds)
        {
            return GetActiveByFields(siteId, featureGuid, fieldIds, -1);
        }

        public static List<CustomField> GetActiveByFields(int siteId, Guid featureGuid, List<int> fieldIds, int languageId = -1)
        {
            if (fieldIds.Count > 0)
                return LoadListFromReader(DBCustomField.GetActiveByFields(siteId, featureGuid, string.Join(";", fieldIds.ToArray()), languageId));

            return new List<CustomField>();
        }

        public static List<CustomField> GetByOption(List<CustomField> lstCustomFields, CustomFieldOptions option)
        {
            List<CustomField> lst = new List<CustomField>();

            foreach (CustomField field in lstCustomFields)
                if ((field.options & (int)option) > 0)
                    lst.Add(field);

            return lst;
        }

        public static CustomField GetOneFromList(List<CustomField> lstCustomFields, int customFieldId)
        {
            foreach (CustomField field in lstCustomFields)
            {
                if (field.customFieldID == customFieldId)
                {
                    return field;
                }
            }

            return null;
        }

        #endregion Static Methods
    }
}
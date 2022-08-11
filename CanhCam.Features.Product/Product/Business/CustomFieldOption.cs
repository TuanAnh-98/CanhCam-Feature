// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
// Created:					2014-07-23
// Last Modified:			2014-07-23

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum CustomFieldOptionType
    {
        Color = 1,
        Picture = 2
    }

    public class CustomFieldOption : IComparable
    {
        #region Constructors

        public CustomFieldOption()
        { }

        public CustomFieldOption(
            int customFieldOptionID)
        {
            this.GetCustomFieldOption(
                customFieldOptionID);
        }

        #endregion Constructors

        #region Private Properties

        private int customFieldOptionID = -1;
        private int customFieldID = -1;
        private string name = string.Empty;
        private int optionType = -1;
        private string optionColor = string.Empty;
        private decimal fromValue;
        private decimal toValue;
        private int displayOrder = 999;
        private Guid guid = Guid.Empty;
        private string customFieldName = string.Empty;
        private Guid featureGuid = Guid.Empty;

        #endregion Private Properties

        #region Public Properties

        public int CompareTo(object value)
        {
            if (value == null) return 1;

            int compareOrder = ((CustomFieldOption)value).DisplayOrder;

            if (this.DisplayOrder == compareOrder) return 0;
            if (this.DisplayOrder < compareOrder) return -1;
            if (this.DisplayOrder > compareOrder) return 1;

            return 0;
        }

        public int CustomFieldOptionId
        {
            get { return customFieldOptionID; }
            set { customFieldOptionID = value; }
        }

        public int CustomFieldId
        {
            get { return customFieldID; }
            set { customFieldID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int OptionType
        {
            get { return optionType; }
            set { optionType = value; }
        }

        public string OptionColor
        {
            get { return optionColor; }
            set { optionColor = value; }
        }

        public decimal FromValue
        {
            get { return fromValue; }
            set { fromValue = value; }
        }

        public decimal ToValue
        {
            get { return toValue; }
            set { toValue = value; }
        }

        public int DisplayOrder
        {
            get { return displayOrder; }
            set { displayOrder = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public string CustomFieldName
        {
            get { return customFieldName; }
            set { customFieldName = value; }
        }

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        #endregion Public Properties

        #region Private Methods

        private void GetCustomFieldOption(
            int customFieldOptionID)
        {
            using (IDataReader reader = DBCustomFieldOption.GetOne(
                customFieldOptionID))
            {
                if (reader.Read())
                {
                    this.customFieldOptionID = Convert.ToInt32(reader["CustomFieldOptionID"]);
                    this.customFieldID = Convert.ToInt32(reader["CustomFieldID"]);
                    this.name = reader["Name"].ToString();
                    this.optionType = Convert.ToInt32(reader["OptionType"]);
                    this.optionColor = reader["OptionColor"].ToString();
                    this.fromValue = Convert.ToDecimal(reader["FromValue"]);
                    this.toValue = Convert.ToDecimal(reader["ToValue"]);
                    this.displayOrder = Convert.ToInt32(reader["DisplayOrder"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                }
            }
        }

        private bool Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();

            newID = DBCustomFieldOption.Create(
                this.customFieldID,
                this.name,
                this.optionType,
                this.optionColor,
                this.fromValue,
                this.toValue,
                this.displayOrder,
                this.guid);

            this.customFieldOptionID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBCustomFieldOption.Update(
                this.customFieldOptionID,
                this.customFieldID,
                this.name,
                this.optionType,
                this.optionColor,
                this.fromValue,
                this.toValue,
                this.displayOrder,
                this.guid);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.customFieldOptionID > 0)
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

        public static bool Delete(int customFieldOptionID)
        {
            return DBCustomFieldOption.Delete(customFieldOptionID);
        }

        public static bool DeleteCustomField(int customFieldID)
        {
            return DBCustomFieldOption.DeleteCustomField(customFieldID);
        }

        private static List<CustomFieldOption> LoadListFromReader(IDataReader reader, bool loadCustomFieldName)
        {
            List<CustomFieldOption> customFieldOptionList = new List<CustomFieldOption>();
            try
            {
                while (reader.Read())
                {
                    CustomFieldOption customFieldOption = new CustomFieldOption
                    {
                        customFieldOptionID = Convert.ToInt32(reader["CustomFieldOptionID"]),
                        customFieldID = Convert.ToInt32(reader["CustomFieldID"]),
                        name = reader["Name"].ToString(),
                        optionType = Convert.ToInt32(reader["OptionType"]),
                        optionColor = reader["OptionColor"].ToString(),
                        fromValue = Convert.ToDecimal(reader["FromValue"]),
                        toValue = Convert.ToDecimal(reader["ToValue"]),
                        displayOrder = Convert.ToInt32(reader["DisplayOrder"]),
                        guid = new Guid(reader["Guid"].ToString())
                    };

                    if (loadCustomFieldName)
                    {
                        customFieldOption.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                        customFieldOption.customFieldName = reader["CustomFieldName"].ToString();
                    }

                    customFieldOptionList.Add(customFieldOption);
                }
            }
            finally
            {
                reader.Close();
            }

            return customFieldOptionList;
        }

        public static List<CustomFieldOption> GetByCustomField(int customFieldId)
        {
            return GetByCustomField(customFieldId, -1);
        }

        public static List<CustomFieldOption> GetByCustomField(int customFieldId, int languageId)
        {
            IDataReader reader = DBCustomFieldOption.GetByCustomField(customFieldId, languageId);
            return LoadListFromReader(reader, false);
        }

        public static List<CustomFieldOption> GetByOptionIds(int siteId, string optionIds, int languageId = -1)
        {
            IDataReader reader = DBCustomFieldOption.GetByOptionIds(siteId, optionIds, languageId);
            return LoadListFromReader(reader, true);
        }

        /// <summary>
        /// gets the next sort rank
        /// </summary>
        /// <param name="customFieldId"></param>
        /// <returns>int</returns>
        public static int GetMaxSortOrder(int customFieldId)
        {
            int nextSort = DBCustomFieldOption.GetMaxSortOrder(customFieldId) + 2;

            return nextSort;
        }

        public static void ResortOptions(List<CustomFieldOption> optionList)
        {
            int i = 1;
            optionList.Sort();

            foreach (CustomFieldOption m in optionList)
            {
                // number the items 1, 3, 5, etc. to provide an empty order
                // number when moving items up and down in the list.
                m.DisplayOrder = i;
                m.Save();

                i += 2;
            }
        }

        #endregion Static Methods
    }
}
/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-25
/// Last Modified:			2014-08-25

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class TagItem
    {
        #region Constructors

        public TagItem()
        { }

        //public TagItem(Guid guid)
        //{
        //    this.GetTagItem(guid);
        //}

        #endregion Constructors

        #region Private Properties

        private Guid guid = Guid.Empty;
        private int tagID = -1;
        private Guid itemGuid = Guid.Empty;
        private string tag = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int TagId
        {
            get { return tagID; }
            set { tagID = value; }
        }

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }

        public string TagText
        {
            get { return tag; }
            set { tag = value; }
        }

        #endregion Public Properties

        //#region Private Methods

        //private void GetTagItem(Guid guid)
        //{
        //    using (IDataReader reader = DBTagItem.GetOne(guid))
        //    {
        //        if (reader.Read())
        //        {
        //            this.guid = new Guid(reader["Guid"].ToString());
        //            this.tagID = Convert.ToInt32(reader["TagID"]);
        //            this.itemGuid = new Guid(reader["ItemGuid"].ToString());
        //        }
        //    }
        //}

        //private bool Create()
        //{
        //    this.guid = Guid.NewGuid();

        //    int rowsAffected = DBTagItem.Create(
        //        this.guid,
        //        this.tagID,
        //        this.itemGuid);

        //    return (rowsAffected > 0);
        //}

        //private bool Update()
        //{
        //    return DBTagItem.Update(
        //        this.guid,
        //        this.tagID,
        //        this.itemGuid);
        //}

        //#endregion

        //#region Public Methods

        ///// <summary>
        ///// Saves this instance of TagItem. Returns true on success.
        ///// </summary>
        ///// <returns>bool</returns>
        //public bool Save()
        //{
        //    if (this.guid != Guid.Empty)
        //    {
        //        return Update();
        //    }
        //    else
        //    {
        //        return Create();
        //    }
        //}

        //#endregion

        #region Static Methods

        public static bool Create(int tagId, Guid itemGuid)
        {
            int rowsAffected = DBTagItem.Create(
                Guid.NewGuid(),
                tagId,
                itemGuid);

            return (rowsAffected > 0);
        }

        public static bool Delete(Guid guid)
        {
            return DBTagItem.Delete(guid);
        }

        public static bool DeleteByItem(Guid itemGuid)
        {
            return DBTagItem.DeleteByItem(itemGuid);
        }

        public static bool DeleteByTag(int tagId)
        {
            return DBTagItem.DeleteByTag(tagId);
        }

        private static List<TagItem> LoadListFromReader(IDataReader reader)
        {
            List<TagItem> tagItemList = new List<TagItem>();
            try
            {
                while (reader.Read())
                {
                    TagItem tagItem = new TagItem
                    {
                        guid = new Guid(reader["Guid"].ToString()),
                        tagID = Convert.ToInt32(reader["TagID"]),
                        itemGuid = new Guid(reader["ItemGuid"].ToString()),
                        tag = reader["Tag"].ToString()
                    };
                    tagItemList.Add(tagItem);
                }
            }
            finally
            {
                reader.Close();
            }

            return tagItemList;
        }

        public static List<TagItem> GetByItem(Guid itemGuid, int languageId = -1)
        {
            IDataReader reader = DBTagItem.GetByItem(itemGuid, languageId);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
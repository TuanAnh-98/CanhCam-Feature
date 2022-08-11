/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2014-08-25
/// Last Modified:			2014-08-25

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class Tag
    {
        #region Constructors

        public Tag()
        { }

        public Tag(int tagId)
        {
            this.GetTag(tagId, -1);
        }

        public Tag(int tagId, int languageId)
        {
            this.GetTag(tagId, languageId);
        }

        #endregion Constructors

        #region Private Properties

        private int tagID = -1;
        private Guid siteGuid = Guid.Empty;
        private Guid featureGuid = Guid.Empty;
        private string tag = string.Empty;
        private int itemCount = 0;
        private Guid guid = Guid.Empty;
        private DateTime createdUtc = DateTime.UtcNow;
        private Guid createdBy = Guid.Empty;

        #endregion Private Properties

        #region Public Properties

        public int TagId
        {
            get { return tagID; }
            set { tagID = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public Guid FeatureGuid
        {
            get { return featureGuid; }
            set { featureGuid = value; }
        }

        public string TagText
        {
            get { return tag; }
            set { tag = value; }
        }

        public int ItemCount
        {
            get { return itemCount; }
            set { itemCount = value; }
        }

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public DateTime CreatedUtc
        {
            get { return createdUtc; }
            set { createdUtc = value; }
        }

        public Guid CreatedBy
        {
            get { return createdBy; }
            set { createdBy = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of Tag.
        /// </summary>
        /// <param name="tagID"> tagID </param>
        private void GetTag(int tagId, int languageId)
        {
            using (IDataReader reader = DBTag.GetOne(tagId, languageId))
            {
                if (reader.Read())
                {
                    this.tagID = Convert.ToInt32(reader["TagID"]);
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.featureGuid = new Guid(reader["FeatureGuid"].ToString());
                    this.tag = reader["Tag"].ToString();
                    this.itemCount = Convert.ToInt32(reader["ItemCount"]);
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.createdUtc = Convert.ToDateTime(reader["CreatedUtc"]);
                    this.createdBy = new Guid(reader["CreatedBy"].ToString());
                }
            }
        }

        private int Create()
        {
            int newID = 0;
            this.guid = Guid.NewGuid();

            newID = DBTag.Create(
                this.siteGuid,
                this.featureGuid,
                this.tag,
                this.itemCount,
                this.guid,
                this.createdUtc,
                this.createdBy);

            this.tagID = newID;

            return newID;
        }

        private int Update()
        {
            if (DBTag.Update(
                this.tagID,
                this.siteGuid,
                this.featureGuid,
                this.tag,
                this.itemCount,
                this.guid,
                this.createdUtc,
                this.createdBy))
                return this.tagID;

            return 0;
        }

        #endregion Private Methods

        #region Public Methods

        public int Save()
        {
            if (this.tagID > 0)
                return this.Update();
            else
                return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        public static bool UpdateItemCount(int tagId)
        {
            return DBTag.UpdateItemCount(tagId);
        }

        public static bool Delete(int tagId)
        {
            return DBTag.Delete(tagId);
        }

        public static bool DeleteBySite(Guid siteGuid)
        {
            return DBTag.DeleteBySite(siteGuid);
        }

        public static int GetCount(Guid siteGuid, Guid? featureGuid, string keyword, int languageId = -1)
        {
            return DBTag.GetCount(siteGuid, featureGuid, keyword, languageId);
        }

        public static List<Tag> LoadListFromReader(IDataReader reader)
        {
            List<Tag> tagList = new List<Tag>();
            try
            {
                while (reader.Read())
                {
                    Tag tag = new Tag
                    {
                        tagID = Convert.ToInt32(reader["TagID"]),
                        siteGuid = new Guid(reader["SiteGuid"].ToString()),
                        featureGuid = new Guid(reader["FeatureGuid"].ToString()),
                        tag = reader["Tag"].ToString(),
                        itemCount = Convert.ToInt32(reader["ItemCount"]),
                        guid = new Guid(reader["Guid"].ToString()),
                        createdUtc = Convert.ToDateTime(reader["CreatedUtc"]),
                        createdBy = new Guid(reader["CreatedBy"].ToString())
                    };
                    tagList.Add(tag);
                }
            }
            finally
            {
                reader.Close();
            }

            return tagList;
        }

        public static List<Tag> GetPage(Guid siteGuid, Guid? featureGuid, string keyword, int languageId, int pageNumber, int pageSize)
        {
            IDataReader reader = DBTag.GetPage(siteGuid, featureGuid, keyword, languageId, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static List<Tag> GetTagCloud(Guid siteGuid, Guid? featureGuid, int languageId, int top)
        {
            IDataReader reader = DBTag.GetTagCloud(siteGuid, featureGuid, languageId, top);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
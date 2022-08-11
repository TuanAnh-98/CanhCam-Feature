// Author:					cc
// Created:					2020-6-17
// Last Modified:			2020-6-17

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class MemberRank
    {
        #region Constructors

        public MemberRank()
        { }

        public MemberRank(
            int id)
        {
            this.GetMemberRank(
                id);
        }

        public MemberRank(int languageId, int rankOrder)
        {
            this.GetMemberRankByRankOrder(rankOrder);
        }

        public MemberRank(SiteUser user, int point)
        {
            this.GetOneByPoint(point);
        }

        #endregion Constructors

        #region Private Properties

        private int id = -1;
        private string name = string.Empty;
        private int rankOrder = -1;
        private int point = -1;
        private decimal discountPercent;
        private string description = string.Empty;
        private string note = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public int RankOrder
        {
            get { return rankOrder; }
            set { rankOrder = value; }
        }

        public int Point
        {
            get { return point; }
            set { point = value; }
        }

        public decimal DiscountPercent
        {
            get { return discountPercent; }
            set { discountPercent = value; }
        }

        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        public string Note
        {
            get { return note; }
            set { note = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of MemberRank.
        /// </summary>
        /// <param name="id"> id </param>
        private void GetMemberRank(
            int id)
        {
            using (IDataReader reader = DBMemberRank.GetOne(
                id))
            {
                if (reader.Read())
                {
                    this.id = Convert.ToInt32(reader["Id"]);
                    this.name = reader["Name"].ToString();
                    this.rankOrder = Convert.ToInt32(reader["RankOrder"]);
                    this.point = Convert.ToInt32(reader["Point"]);
                    this.discountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                    this.description = reader["Description"].ToString();
                    this.note = reader["Note"].ToString();
                }
            }
        }

        private void GetMemberRankByRankOrder(int rankOrder)
        {
            using (IDataReader reader = DBMemberRank.GetOneByRankOrder(rankOrder))
            {
                if (reader.Read())
                {
                    this.id = Convert.ToInt32(reader["Id"]);
                    this.name = reader["Name"].ToString();
                    this.rankOrder = Convert.ToInt32(reader["RankOrder"]);
                    this.point = Convert.ToInt32(reader["Point"]);
                    this.discountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                    this.description = reader["Description"].ToString();
                    this.note = reader["Note"].ToString();
                }
            }
        }

        private void GetOneByPoint(int point)
        {
            using (IDataReader reader = DBMemberRank.GetOneByPoint(point))
            {
                if (reader.Read())
                {
                    this.id = Convert.ToInt32(reader["Id"]);
                    this.name = reader["Name"].ToString();
                    this.rankOrder = Convert.ToInt32(reader["RankOrder"]);
                    this.point = Convert.ToInt32(reader["Point"]);
                    this.discountPercent = Convert.ToDecimal(reader["DiscountPercent"]);
                    this.description = reader["Description"].ToString();
                    this.note = reader["Note"].ToString();
                }
            }
        }

        /// <summary>
        /// Persists a new instance of MemberRank. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBMemberRank.Create(
                this.name,
                this.rankOrder,
                this.point,
                this.discountPercent,
                this.description,
                this.note);

            this.id = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of MemberRank. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBMemberRank.Update(
                this.id,
                this.name,
                this.rankOrder,
                this.point,
                this.discountPercent,
                this.description,
                this.note);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of MemberRank. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.id > 0)
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

        /// <summary>
        /// Deletes an instance of MemberRank. Returns true on success.
        /// </summary>
        /// <param name="id"> id </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int id)
        {
            return DBMemberRank.Delete(
                id);
        }

        /// <summary>
        /// Gets a count of MemberRank.
        /// </summary>
        public static int GetCount()
        {
            return DBMemberRank.GetCount();
        }

        private static List<MemberRank> LoadListFromReader(IDataReader reader)
        {
            List<MemberRank> memberRankList = new List<MemberRank>();
            try
            {
                while (reader.Read())
                {
                    MemberRank memberRank = new MemberRank
                    {
                        id = Convert.ToInt32(reader["Id"]),
                        name = reader["Name"].ToString(),
                        rankOrder = Convert.ToInt32(reader["RankOrder"]),
                        point = Convert.ToInt32(reader["Point"]),
                        discountPercent = Convert.ToDecimal(reader["DiscountPercent"]),
                        description = reader["Description"].ToString(),
                        note = reader["Note"].ToString()
                    };
                    memberRankList.Add(memberRank);
                }
            }
            finally
            {
                reader.Close();
            }

            return memberRankList;
        }

        /// <summary>
        /// Gets an IList with all instances of MemberRank.
        /// </summary>
        public static List<MemberRank> GetAll()
        {
            IDataReader reader = DBMemberRank.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of MemberRank.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<MemberRank> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBMemberRank.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
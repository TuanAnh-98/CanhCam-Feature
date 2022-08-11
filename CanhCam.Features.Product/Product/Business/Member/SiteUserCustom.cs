// Author:					t
// Created:					2020-4-16
// Last Modified:			2020-4-16

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class SiteUserCustom
    {
        #region Constructors

        public SiteUserCustom()
        { }

        public SiteUserCustom(
            int userID)
        {
            this.GetSiteUserCustom(
                userID);
        }

        #endregion Constructors

        #region Private Properties

        private int userID = -1;
        private int siteID = -1;
        private string name = string.Empty;
        private string loginName = string.Empty;
        private string email = string.Empty;
        private string phone = string.Empty;

        // string loweredEmail = string.Empty;
        //private string passwordQuestion = string.Empty;
        //private string passwordAnswer = string.Empty;
        //private string gender = string.Empty;
        //private bool profileApproved = false;
        //private Guid registerConfirmGuid = Guid.Empty;
        //private bool approvedForForums = false;
        //private bool trusted = false;
        private bool displayInMemberList = false;

        //private string webSiteURL = string.Empty;
        //private string country = string.Empty;
        //private string state = string.Empty;
        //private string occupation = string.Empty;
        //private string interests = string.Empty;
        //private string mSN = string.Empty;
        //private string yahoo = string.Empty;
        //private string aIM = string.Empty;
        //private string iCQ = string.Empty;
        private int totalPosts = 0;

        //private string avatarUrl = string.Empty;
        //private int timeOffsetHours = -1;
        //private string signature = string.Empty;
        //private DateTime dateCreated = DateTime.Now;
        private Guid userGuid = Guid.Empty;

        //private string skin = string.Empty;
        //private bool isDeleted = false;
        //private DateTime lastActivityDate = DateTime.Now;
        //private DateTime lastLoginDate = DateTime.Now;
        //private DateTime lastPasswordChangedDate = DateTime.Now;
        //private DateTime lastLockoutDate = DateTime.Now;
        //private int failedPasswordAttemptCount = -1;
        //private DateTime failedPwdAttemptWindowStart = DateTime.Now;
        //private int failedPwdAnswerAttemptCount = -1;
        //private DateTime failedPwdAnswerWindowStart = DateTime.Now;
        //private bool isLockedOut = false;
        //private string mobilePIN = string.Empty;
        //private string passwordSalt = string.Empty;
        //private string comment = string.Empty;
        //private string openIDURI = string.Empty;
        //private string windowsLiveID = string.Empty;
        private Guid siteGuid = Guid.Empty;

        //private decimal totalRevenue;
        private string firstName = string.Empty;

        private string lastName = string.Empty;
        //private string pwd = string.Empty;
        //private bool mustChangePwd = false;
        //private string newEmail = string.Empty;
        //private string editorPreference = string.Empty;
        //private Guid emailChangeGuid = Guid.Empty;
        //private string timeZoneId = string.Empty;
        //private Guid passwordResetGuid = Guid.Empty;
        //private bool rolesChanged = false;
        //private string authorBio = string.Empty;

        //Extra properties
        private int totalOrdersBought = 0;

        private decimal totalMoney = 0;
        private decimal totalConfirmedMoney = 0;
        private decimal totalUnconfirmedMoney = 0;

        #endregion Private Properties

        #region Public Properties

        public int UserID
        {
            get { return userID; }
            set { userID = value; }
        }

        public int SiteID
        {
            get { return siteID; }
            set { siteID = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string LoginName
        {
            get { return loginName; }
            set { loginName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public bool DisplayInMemberList
        {
            get { return displayInMemberList; }
            set { displayInMemberList = value; }
        }

        public int TotalPosts
        {
            get { return totalPosts; }
            set { totalPosts = value; }
        }

        public Guid UserGuid
        {
            get { return userGuid; }
            set { userGuid = value; }
        }

        public Guid SiteGuid
        {
            get { return siteGuid; }
            set { siteGuid = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        //Extra properties
        public int TotalOrdersBought
        {
            get { return totalOrdersBought; }
            set { totalOrdersBought = value; }
        }

        public decimal TotalMoney
        {
            get { return totalMoney; }
            set { totalMoney = value; }
        }

        public decimal TotalConfirmedMoney
        {
            get { return totalConfirmedMoney; }
            set { totalConfirmedMoney = value; }
        }

        public decimal TotalUnconfirmedMoney
        {
            get { return totalUnconfirmedMoney; }
            set { totalUnconfirmedMoney = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of User.
        /// </summary>
        /// <param name="userID"> userID </param>
        private void GetSiteUserCustom(
            int userID)
        {
            using (IDataReader reader = DBSiteUserCustom.GetCustomerByID(
                userID))
            {
                if (reader.Read())
                {
                    this.userID = Convert.ToInt32(reader["UserID"]);
                    this.siteID = Convert.ToInt32(reader["SiteID"]);
                    this.name = reader["Name"].ToString();
                    this.loginName = reader["LoginName"].ToString();
                    this.email = reader["Email"].ToString();
                    if (reader["Phone"] != DBNull.Value)
                        this.phone = reader["Phone"].ToString();
                    else this.phone = String.Empty;
                    this.displayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]);
                    this.totalPosts = Convert.ToInt32(reader["TotalPosts"]);
                    this.userGuid = new Guid(reader["UserGuid"].ToString());
                    this.siteGuid = new Guid(reader["SiteGuid"].ToString());
                    this.firstName = reader["FirstName"].ToString();
                    this.lastName = reader["LastName"].ToString();
                    //Extra properties
                    if (reader["TotalOrdersBought"] != DBNull.Value)
                        this.totalOrdersBought = Convert.ToInt32(reader["TotalOrdersBought"].ToString());
                    if (reader["TotalMoney"] != DBNull.Value)
                        this.totalMoney = Convert.ToDecimal(reader["TotalMoney"].ToString());
                    if (reader["TotalConfirmedMoney"] != DBNull.Value)
                        this.totalConfirmedMoney = Convert.ToDecimal(reader["TotalConfirmedMoney"].ToString());
                    if (reader["TotalUnconfirmedMoney"] != DBNull.Value)
                        this.totalUnconfirmedMoney = Convert.ToDecimal(reader["TotalUnconfirmedMoney"].ToString());
                }
            }
        }

        /// <summary>
        /// Persists a new instance of User. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBSiteUserCustom.Create(
                this.siteID,
                this.name,
                this.loginName,
                this.email,
                this.phone,
                this.displayInMemberList,
                this.totalPosts,
                this.userGuid,
                this.siteGuid,
                this.firstName,
                this.lastName,
                //Extra properties
                this.totalOrdersBought,
                this.totalMoney,
                this.totalConfirmedMoney,
                this.totalUnconfirmedMoney
                );

            this.userID = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of User. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBSiteUserCustom.Update(
                this.userID,
                this.siteID,
                this.name,
                this.loginName,
                this.email,
                this.phone,
                this.displayInMemberList,
                this.totalPosts,
                this.userGuid,
                this.siteGuid,
                this.firstName,
                this.lastName,
                //Extra properties
                this.totalOrdersBought,
                this.totalMoney,
                this.totalConfirmedMoney,
                this.totalUnconfirmedMoney
                );
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of User. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.userID > 0)
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
        /// Deletes an instance of User. Returns true on success.
        /// </summary>
        /// <param name="userID"> userID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int userID)
        {
            return DBSiteUserCustom.Delete(
                userID);
        }

        /// <summary>
        /// Gets a count of User.
        /// </summary>
        public static int GetCount()
        {
            return DBSiteUserCustom.GetCount();
        }

        private static List<SiteUserCustom> LoadListFromReader(IDataReader reader)
        {
            List<SiteUserCustom> userList = new List<SiteUserCustom>();
            try
            {
                while (reader.Read())
                {
                    SiteUserCustom user = new SiteUserCustom
                    {
                        userID = Convert.ToInt32(reader["UserID"]),
                        siteID = Convert.ToInt32(reader["SiteID"]),
                        name = reader["Name"].ToString(),
                        loginName = reader["LoginName"].ToString(),
                        email = reader["Email"].ToString(),
                        //if (reader["Phone"] != DBNull.Value)
                        //    user.phone = reader["Phone"].ToString();
                        //else user.phone = String.Empty;
                        displayInMemberList = Convert.ToBoolean(reader["DisplayInMemberList"]),
                        totalPosts = Convert.ToInt32(reader["TotalPosts"]),
                        userGuid = new Guid(reader["UserGuid"].ToString()),
                        siteGuid = new Guid(reader["SiteGuid"].ToString()),
                        firstName = reader["FirstName"].ToString(),
                        lastName = reader["LastName"].ToString()
                    };
                    //Extra properties
                    if (reader["TotalOrdersBought"] != DBNull.Value)
                        user.totalOrdersBought = Convert.ToInt32(reader["TotalOrdersBought"].ToString());
                    if (reader["TotalMoney"] != DBNull.Value)
                        user.totalMoney = Convert.ToDecimal(reader["TotalMoney"].ToString());
                    if (reader["TotalConfirmedMoney"] != DBNull.Value)
                        user.totalConfirmedMoney = Convert.ToDecimal(reader["TotalConfirmedMoney"].ToString());
                    if (reader["TotalUnconfirmedMoney"] != DBNull.Value)
                        user.totalUnconfirmedMoney = Convert.ToDecimal(reader["TotalUnconfirmedMoney"].ToString());
                    userList.Add(user);
                }
            }
            finally
            {
                reader.Close();
            }

            return userList;
        }

        /// <summary>
        /// Gets an IList with all instances of User.
        /// </summary>
        public static List<SiteUserCustom> GetAll()
        {
            IDataReader reader = DBSiteUserCustom.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of User.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<SiteUserCustom> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBSiteUserCustom.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static int GetCustomerCountBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword)
        {
            return DBSiteUserCustom.GetCustomerCountBySearch(startDate, endDate, nameKeyword, emailKeyword, phoneKeyword);
        }

        public static List<SiteUserCustom> GetCustomerBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword, int pageNumber, int pageSize)
        {
            IDataReader reader = DBSiteUserCustom.GetCustomerBySearch(startDate, endDate, nameKeyword, emailKeyword, phoneKeyword, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static int GetCountBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword, int memberRank)
        {
            return DBSiteUserCustom.GetCountBySearch(startDate, endDate, nameKeyword, emailKeyword, phoneKeyword, memberRank);
        }

        public static List<SiteUserCustom> GetPageBySearch(DateTime? startDate, DateTime? endDate, string nameKeyword, string emailKeyword, string phoneKeyword, int memberRank, int pageNumber, int pageSize)
        {
            IDataReader reader = DBSiteUserCustom.GetPageBySearch(startDate, endDate, nameKeyword, emailKeyword, phoneKeyword, memberRank, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static int GetCountByAdminSearch(int siteId, string keyword)
        {
            return DBSiteUserCustom.GetCountByAdminSearch(siteId, keyword);
        }

        public static List<SiteUserCustom> GetPageByAdminSearch(int siteId, string keyword, int pageNumber, int pageSize)
        {
            IDataReader reader = DBSiteUserCustom.GetPageByAdminSearch(siteId, keyword, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static int GetCountByMemberRank(int siteId, string keyword, int memberRank)
        {
            return DBSiteUserCustom.GetCountByMemberRank(siteId, keyword, memberRank);
        }

        public static List<SiteUserCustom> GetPageByMemberRank(int siteId, string keyword, int memberRank, int pageNumber, int pageSize)
        {
            IDataReader reader = DBSiteUserCustom.GetPageByMemberRank(siteId, keyword, memberRank, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
// Author:					Canh cam
// Created:					2020-11-3
// Last Modified:			2020-11-3

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public enum ProductSoldOutLetterStatus
    {
        UnSent = 0,
        Sented = 10,
    }

    public class ProductSoldOutLetter
    {
        #region Constructors

        public ProductSoldOutLetter()
        { }

        public ProductSoldOutLetter(
            int rowId)
        {
            this.GetProductSoldOutLetter(
                rowId);
        }

        #endregion Constructors

        #region Private Properties

        private int rowId = -1;
        private int productID = -1;
        private string productName = string.Empty;
        private int quantity = 1;
        private string url = string.Empty;
        private string email = string.Empty;
        private string fullName = string.Empty;
        private string phone = string.Empty;
        private int userId = -1;
        private int status = 0;
        private DateTime createDate = DateTime.Now;
        private string ipAddress = string.Empty;
        private bool isContacted = false;
        #endregion Private Properties

        #region Public Properties

        public int RowId
        {
            get { return rowId; }
            set { rowId = value; }
        }

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string FullName
        {
            get { return fullName; }
            set { fullName = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public int UserId
        {
            get { return userId; }
            set { userId = value; }
        }

        public int Status
        {
            get { return status; }
            set { status = value; }
        }

        public DateTime CreateDate
        {
            get { return createDate; }
            set { createDate = value; }
        }

        public string IpAddress
        {
            get { return ipAddress; }
            set { ipAddress = value; }
        }
        public bool IsContacted
        {
            get { return isContacted; }
            set { isContacted = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of ProductSoldOutLetter.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        private void GetProductSoldOutLetter(
            int rowId)
        {
            using (IDataReader reader = DBProductSoldOutLetter.GetOne(
                rowId))
            {
                if (reader.Read())
                {
                    this.rowId = Convert.ToInt32(reader["RowId"]);
                    this.productID = Convert.ToInt32(reader["ProductID"]);
                    this.productName = reader["ProductName"].ToString();
                    this.quantity = Convert.ToInt32(reader["Quantity"].ToString());
                    this.url = reader["Url"].ToString();
                    this.email = reader["Email"].ToString();
                    this.fullName = reader["FullName"].ToString();
                    this.phone = reader["Phone"].ToString();
                    this.userId = Convert.ToInt32(reader["UserId"]);
                    this.status = Convert.ToInt32(reader["Status"]);
                    this.createDate = Convert.ToDateTime(reader["CreateDate"]);
                    this.ipAddress = reader["IpAddress"].ToString();
                    if (reader["IsContacted"] != DBNull.Value)
                        this.isContacted = Convert.ToBoolean(reader["IsContacted"].ToString());
                }
            }
        }

        /// <summary>
        /// Persists a new instance of ProductSoldOutLetter. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBProductSoldOutLetter.Create(
                this.productID,
                this.productName,
                this.quantity,
                this.url,
                this.email,
                this.fullName,
                this.phone,
                this.userId,
                this.status,
                this.createDate,
                this.ipAddress,
                this.isContacted);

            this.rowId = newID;

            return (newID > 0);
        }

        /// <summary>
        /// Updates this instance of ProductSoldOutLetter. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBProductSoldOutLetter.Update(
                this.rowId,
                this.productID,
                this.productName,
                this.quantity,
                this.url,
                this.email,
                this.fullName,
                this.phone,
                this.userId,
                this.status,
                this.createDate,
                this.ipAddress,
                this.isContacted);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of ProductSoldOutLetter. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowId > 0)
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
        /// Deletes an instance of ProductSoldOutLetter. Returns true on success.
        /// </summary>
        /// <param name="rowId"> rowId </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowId)
        {
            return DBProductSoldOutLetter.Delete(
                rowId);
        }

        /// <summary>
        /// Gets a count of ProductSoldOutLetter.
        /// </summary>
        public static int GetCount()
        {
            return DBProductSoldOutLetter.GetCount();
        }

        private static List<ProductSoldOutLetter> LoadListFromReader(IDataReader reader)
        {
            List<ProductSoldOutLetter> productSoldOutLetterList = new List<ProductSoldOutLetter>();
            try
            {
                while (reader.Read())
                {
                    ProductSoldOutLetter productSoldOutLetter = new ProductSoldOutLetter
                    {
                        rowId = Convert.ToInt32(reader["RowId"]),
                        productID = Convert.ToInt32(reader["ProductID"]),
                        productName = reader["ProductName"].ToString(),
                        quantity = Convert.ToInt32(reader["Quantity"].ToString()),
                        url = reader["Url"].ToString(),
                        email = reader["Email"].ToString(),
                        fullName = reader["FullName"].ToString(),
                        phone = reader["Phone"].ToString(),
                        userId = Convert.ToInt32(reader["UserId"]),
                        status = Convert.ToInt32(reader["Status"]),
                        createDate = Convert.ToDateTime(reader["CreateDate"]),
                        ipAddress = reader["IpAddress"].ToString()
                    };
                    if (reader["IsContacted"] != DBNull.Value)
                        productSoldOutLetter.isContacted = Convert.ToBoolean(reader["IsContacted"].ToString());
                    productSoldOutLetterList.Add(productSoldOutLetter);
                }
            }
            finally
            {
                reader.Close();
            }

            return productSoldOutLetterList;
        }

        /// <summary>
        /// Gets an IList with all instances of ProductSoldOutLetter.
        /// </summary>
        public static List<ProductSoldOutLetter> GetAll()
        {
            IDataReader reader = DBProductSoldOutLetter.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of ProductSoldOutLetter.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<ProductSoldOutLetter> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBProductSoldOutLetter.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static int GetCountBySearch(DateTime? startDate = null, DateTime? endDate = null, string keyword = null)
        {
            return DBProductSoldOutLetter.GetCountySearch(startDate, endDate, keyword);
        }

        public static List<ProductSoldOutLetter> GetPageBySearch(DateTime? startDate = null, DateTime? endDate = null, string keyword = null, int pageNumber = 1, int pageSize = 21474836)
        {
            IDataReader reader = DBProductSoldOutLetter.GetPageBySearch(startDate, endDate, keyword, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        #endregion Static Methods
    }
}
// Author:					Canh cam
// Created:					2021-2-17
// Last Modified:			2021-2-17

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class Voucher
    {
        #region Constructors

        public Voucher()
        { }

        public Voucher(
            Guid itemGuid)
        {
            this.GetVoucher(
                itemGuid);
        }

        public Voucher(
            string code)
        {
            this.GetVoucher(
                code);
        }

        #endregion Constructors

        #region Private Properties

        private Guid itemGuid = Guid.Empty;
        private string voucherCode = string.Empty;
        private int useCount = 0;
        private int limitationTimes = 1;
        private decimal minimumOrderAmount;
        private decimal amount;
        private bool usePercentage = false;
        private decimal maxAmount;
        private DateTime startDate = DateTime.Now;
        private DateTime endDate = DateTime.MaxValue;
        private DateTime createdDate = DateTime.Now;
        private string orderCodesUsed = string.Empty;

        #endregion Private Properties

        #region Public Properties

        public Guid ItemGuid
        {
            get { return itemGuid; }
            set { itemGuid = value; }
        }

        public string VoucherCode
        {
            get { return voucherCode; }
            set { voucherCode = value; }
        }

        public int UseCount
        {
            get { return useCount; }
            set { useCount = value; }
        }

        public int LimitationTimes
        {
            get { return limitationTimes; }
            set { limitationTimes = value; }
        }

        public decimal MinimumOrderAmount
        {
            get { return minimumOrderAmount; }
            set { minimumOrderAmount = value; }
        }

        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        public bool UsePercentage
        {
            get { return usePercentage; }
            set { usePercentage = value; }
        }

        public decimal MaxAmount
        {
            get { return maxAmount; }
            set { maxAmount = value; }
        }

        public DateTime StartDate
        {
            get { return startDate; }
            set { startDate = value; }
        }

        public DateTime EndDate
        {
            get { return endDate; }
            set { endDate = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public string OrderCodesUsed
        {
            get { return orderCodesUsed; }
            set { orderCodesUsed = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of Voucher.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>

        private void GetVoucher(
            Guid itemGuid)
        {
            PopulateVoucher(this, DBVoucher.GetOne(itemGuid));
        }

        private void GetVoucher(
            string code)
        {
            PopulateVoucher(this, DBVoucher.GetOneByCode(code));
        }

        public static void PopulateVoucher(Voucher voucher, IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    voucher.itemGuid = new Guid(reader["ItemGuid"].ToString());
                    voucher.voucherCode = reader["VoucherCode"].ToString();
                    voucher.useCount = Convert.ToInt32(reader["UseCount"]);
                    voucher.limitationTimes = Convert.ToInt32(reader["LimitationTimes"]);
                    voucher.minimumOrderAmount = Convert.ToDecimal(reader["MinimumOrderAmount"]);
                    voucher.amount = Convert.ToDecimal(reader["Amount"]);
                    voucher.usePercentage = Convert.ToBoolean(reader["UsePercentage"]);
                    voucher.maxAmount = Convert.ToDecimal(reader["MaxAmount"]);
                    voucher.startDate = Convert.ToDateTime(reader["StartDate"]);
                    voucher.endDate = Convert.ToDateTime(reader["EndDate"]);
                    voucher.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    if (reader["OrderCodesUsed"] != DBNull.Value)
                        voucher.orderCodesUsed = reader["OrderCodesUsed"].ToString();
                }
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Persists a new instance of Voucher. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.itemGuid = Guid.NewGuid();

            int rowsAffected = DBVoucher.Create(
                this.itemGuid,
                this.voucherCode,
                this.useCount,
                this.limitationTimes,
                this.minimumOrderAmount,
                this.amount,
                this.usePercentage,
                this.maxAmount,
                this.startDate,
                this.endDate,
                this.createdDate, this.orderCodesUsed);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Updates this instance of Voucher. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBVoucher.Update(
                this.itemGuid,
                this.voucherCode,
                this.useCount,
                this.limitationTimes,
                this.minimumOrderAmount,
                this.amount,
                this.usePercentage,
                this.maxAmount,
                this.startDate,
                this.endDate,
                this.createdDate, this.orderCodesUsed);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of Voucher. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.itemGuid != Guid.Empty)
            {
                return Update();
            }
            else
            {
                return Create();
            }
        }

        #endregion Public Methods

        #region Static Methods

        /// <summary>
        /// Deletes an instance of Voucher. Returns true on success.
        /// </summary>
        /// <param name="itemGuid"> itemGuid </param>
        /// <returns>bool</returns>
        public static bool Delete(
            Guid itemGuid)
        {
            return DBVoucher.Delete(
                itemGuid);
        }

        /// <summary>
        /// Gets a count of Voucher.
        /// </summary>
        public static int GetCount()
        {
            return DBVoucher.GetCount();
        }

        private static List<Voucher> LoadListFromReader(IDataReader reader)
        {
            List<Voucher> voucherList = new List<Voucher>();
            try
            {
                while (reader.Read())
                {
                    Voucher voucher = new Voucher
                    {
                        itemGuid = new Guid(reader["ItemGuid"].ToString()),
                        voucherCode = reader["VoucherCode"].ToString(),
                        useCount = Convert.ToInt32(reader["UseCount"]),
                        limitationTimes = Convert.ToInt32(reader["LimitationTimes"]),
                        minimumOrderAmount = Convert.ToDecimal(reader["MinimumOrderAmount"]),
                        amount = Convert.ToDecimal(reader["Amount"]),
                        usePercentage = Convert.ToBoolean(reader["UsePercentage"]),
                        maxAmount = Convert.ToDecimal(reader["MaxAmount"]),
                        startDate = Convert.ToDateTime(reader["StartDate"]),
                        endDate = Convert.ToDateTime(reader["EndDate"]),
                        createdDate = Convert.ToDateTime(reader["CreatedDate"])
                    };
                    if (reader["OrderCodesUsed"] != DBNull.Value)
                        voucher.orderCodesUsed = reader["OrderCodesUsed"].ToString();
                    voucherList.Add(voucher);
                }
            }
            finally
            {
                reader.Close();
            }

            return voucherList;
        }

        /// <summary>
        /// Gets an IList with all instances of Voucher.
        /// </summary>
        public static List<Voucher> GetAll()
        {
            IDataReader reader = DBVoucher.GetAll();
            return LoadListFromReader(reader);
        }

        /// <summary>
        /// Gets an IList with page of instances of Voucher.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<Voucher> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            IDataReader reader = DBVoucher.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }

        public static List<Voucher> GetPageByAdv(string keyword, int type, int pageNumber, int pageSize)
        {
            IDataReader reader = DBVoucher.GetPageByAdv(keyword, type, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }
        public static int  GetCountByAdv(string keyword, int type)
        {
            return DBVoucher.GetCountByAdv(keyword, type);
        }
        #endregion Static Methods
    }
}
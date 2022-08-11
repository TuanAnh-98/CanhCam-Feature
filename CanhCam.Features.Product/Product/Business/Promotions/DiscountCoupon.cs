/// Author:					Tran Quoc Vuong
/// Created:				2020-11-09
/// Last Modified:			2020-11-09

using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    [Serializable]
    public class DiscountCoupon
    {
        #region Constructors

        public DiscountCoupon()
        { }

        public DiscountCoupon(Guid guid)
        {
            this.GetDiscountCoupon(guid);
        }

        #endregion Constructors

        #region Private Properties

        private Guid guid = Guid.Empty;
        private int discountID = -1;
        private string couponCode = string.Empty;
        private int useCount = 0;
        private int limitationTimes = 0;
        private int maximumQtyDiscount = 0;
        private int maximumQtyRequired = 0;
        private DateTime createdDate = DateTime.UtcNow;
        private Discount discount = null;

        private decimal discountAmount = decimal.Zero;

        #endregion Private Properties

        #region Public Properties

        public Guid Guid
        {
            get { return guid; }
            set { guid = value; }
        }

        public int DiscountID
        {
            get { return discountID; }
            set { discountID = value; }
        }

        public string CouponCode
        {
            get { return couponCode; }
            set { couponCode = value; }
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

        public int MaximumQtyDiscount
        {
            get { return maximumQtyDiscount; }
            set { maximumQtyDiscount = value; }
        }

        public int MaximumQtyRequired
        {
            get { return maximumQtyRequired; }
            set { maximumQtyRequired = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public Discount Discount { get => discount; set => discount = value; }
        public decimal DiscountAmount { get => discountAmount; set => discountAmount = value; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of DiscountCoupon.
        /// </summary>
        /// <param name="guid"> guid </param>
        private void GetDiscountCoupon(Guid guid)
        {
            using (IDataReader reader = DBDiscountCoupon.GetOne(
                guid))
            {
                if (reader.Read())
                {
                    this.guid = new Guid(reader["Guid"].ToString());
                    this.discountID = Convert.ToInt32(reader["DiscountID"]);
                    this.couponCode = reader["CouponCode"].ToString();
                    this.useCount = Convert.ToInt32(reader["UseCount"]);
                    this.limitationTimes = Convert.ToInt32(reader["LimitationTimes"]);
                    this.maximumQtyDiscount = Convert.ToInt32(reader["MaximumQtyDiscount"]);
                    this.maximumQtyRequired = Convert.ToInt32(reader["MaximumQtyRequired"]);
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of DiscountCoupon. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            this.guid = Guid.NewGuid();

            int rowsAffected = DBDiscountCoupon.Create(
                this.guid,
                this.discountID,
                this.couponCode,
                this.useCount,
                this.limitationTimes,
                this.maximumQtyDiscount,
                this.maximumQtyRequired,
                this.createdDate);

            return (rowsAffected > 0);
        }

        /// <summary>
        /// Updates this instance of DiscountCoupon. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {
            return DBDiscountCoupon.Update(
                this.guid,
                this.discountID,
                this.couponCode,
                this.useCount,
                this.limitationTimes,
                this.maximumQtyDiscount,
                this.maximumQtyRequired,
                this.createdDate);
        }

        #endregion Private Methods

        #region Public Methods

        /// <summary>
        /// Saves this instance of DiscountCoupon. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.guid != Guid.Empty)
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

        public static bool Delete(Guid guid)
        {
            return DBDiscountCoupon.Delete(guid);
        }

        public static bool DeleteByDiscount(int discountID)
        {
            return DBDiscountCoupon.DeleteByDiscount(discountID);
        }

        /// <summary>
        /// Gets a count of DiscountCoupon.
        /// </summary>
        public static int GetCount(int discountId, string couponCode)
        {
            return DBDiscountCoupon.GetCount(discountId, !string.IsNullOrWhiteSpace(couponCode) ? couponCode : null);
        }

        private static List<DiscountCoupon> LoadListFromReader(IDataReader reader)
        {
            var discountCouponList = new List<DiscountCoupon>();
            try
            {
                while (reader.Read())
                {
                    DiscountCoupon discountCoupon = new DiscountCoupon
                    {
                        guid = new Guid(reader["Guid"].ToString()),
                        discountID = Convert.ToInt32(reader["DiscountID"]),
                        couponCode = reader["CouponCode"].ToString(),
                        useCount = Convert.ToInt32(reader["UseCount"]),
                        limitationTimes = Convert.ToInt32(reader["LimitationTimes"]),
                        maximumQtyDiscount = Convert.ToInt32(reader["MaximumQtyDiscount"]),
                        maximumQtyRequired = Convert.ToInt32(reader["MaximumQtyRequired"]),
                        createdDate = Convert.ToDateTime(reader["CreatedDate"])
                    };
                    discountCouponList.Add(discountCoupon);
                }
            }
            finally
            {
                reader.Close();
            }

            return discountCouponList;
        }

        public static List<DiscountCoupon> GetPage(int discountId, string couponCode, int pageNumber, int pageSize)
        {
            var reader = DBDiscountCoupon.GetPage(discountId, !string.IsNullOrWhiteSpace(couponCode) ? couponCode : null, pageNumber, pageSize);
            return LoadListFromReader(reader);
        }

        public static DiscountCoupon GetByCode(string couponCode)
        {
            var reader = DBDiscountCoupon.GetByCode(couponCode);
            var lst = LoadListFromReader(reader);
            if (lst.Count > 0)
                return lst[0];

            return null;
        }

        public static bool UpdateUseCount(string couponCode)
        {
            return DBDiscountCoupon.UpdateUseCount(couponCode);
        }

        public static bool ExistCode(string couponCode)
        {
            return DBDiscountCoupon.ExistCode(couponCode);
        }

        #endregion Static Methods
    }
}
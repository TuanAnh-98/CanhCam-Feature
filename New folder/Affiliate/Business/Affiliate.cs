
// Author:					Tran Quoc Vuong
// Created:					2022-8-4
// Last Modified:			2022-8-4

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

	public class Affiliate
	{

		#region Constructors

		public Affiliate()
		{ }


		public Affiliate(
			int affiliateID)
		{
			this.GetAffiliate(
				affiliateID);
		}

		#endregion

		#region Private Properties

		private int affiliateID = -1;
		private string orderCode = string.Empty;
		private int affiliateUserID = -1;
		private int userID = -1;
		private int productID = -1;
		private string productName = string.Empty;
		private decimal productPrice;
		private decimal commission;
		private DateTime dateBuy = DateTime.Now;
		private bool status = false;
		private string extracol3 = string.Empty;

		#endregion

		#region Public Properties

		public int AffiliateID
		{
			get { return affiliateID; }
			set { affiliateID = value; }
		}
		public string OrderCode
		{
			get { return orderCode; }
			set { orderCode = value; }
		}
		public int AffiliateUserID
		{
			get { return affiliateUserID; }
			set { affiliateUserID = value; }
		}
		public int UserID
		{
			get { return userID; }
			set { userID = value; }
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
		public decimal ProductPrice
		{
			get { return productPrice; }
			set { productPrice = value; }
		}
		public decimal Commission
		{
			get { return commission; }
			set { commission = value; }
		}
		public DateTime DateBuy
		{
			get { return dateBuy; }
			set { dateBuy = value; }
		}
		public bool Status
		{
			get { return status; }
			set { status = value; }
		}
		public string Extracol3
		{
			get { return extracol3; }
			set { extracol3 = value; }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of Affiliate.
		/// </summary>
		/// <param name="affiliateID"> affiliateID </param>
		private void GetAffiliate(
			int affiliateID)
		{
			using (IDataReader reader = DBAffiliate.GetOne(
				affiliateID))
			{
				if (reader.Read())
				{
					this.affiliateID = Convert.ToInt32(reader["AffiliateID"]);
					this.orderCode = reader["OrderCode"].ToString();
					this.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					this.userID = Convert.ToInt32(reader["UserID"]);
					this.productID = Convert.ToInt32(reader["ProductID"]);
					this.productName = reader["ProductName"].ToString();
					this.productPrice = Convert.ToDecimal(reader["ProductPrice"]);
					this.commission = Convert.ToDecimal(reader["Commission"]);
					this.dateBuy = Convert.ToDateTime(reader["DateBuy"]);
					this.status = Convert.ToBoolean(reader["Status"]);
					this.extracol3 = reader["extracol3"].ToString();
				}
			}

		}

		/// <summary>
		/// Persists a new instance of Affiliate. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			int newID = 0;

			newID = DBAffiliate.Create(
				this.orderCode,
				this.affiliateUserID,
				this.userID,
				this.productID,
				this.productName,
				this.productPrice,
				this.commission,
				this.dateBuy,
				this.status,
				this.extracol3);

			this.affiliateID = newID;

			return (newID > 0);

		}


		/// <summary>
		/// Updates this instance of Affiliate. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBAffiliate.Update(
				this.affiliateID,
				this.orderCode,
				this.affiliateUserID,
				this.userID,
				this.productID,
				this.productName,
				this.productPrice,
				this.commission,
				this.dateBuy,
				this.status,
				this.extracol3);

		}





		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of Affiliate. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.affiliateID > 0)
			{
				return this.Update();
			}
			else
			{
				return this.Create();
			}
		}




		#endregion

		#region Static Methods

		/// <summary>
		/// Deletes an instance of Affiliate. Returns true on success.
		/// </summary>
		/// <param name="affiliateID"> affiliateID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int affiliateID)
		{
			return DBAffiliate.Delete(
				affiliateID);
		}


		/// <summary>
		/// Gets a count of Affiliate. 
		/// </summary>
		public static int GetCount()
		{
			return DBAffiliate.GetCount();
		}

		private static List<Affiliate> LoadListFromReader(IDataReader reader)
		{
			List<Affiliate> affiliateList = new List<Affiliate>();
			try
			{
				while (reader.Read())
				{
					Affiliate affiliate = new Affiliate();
					affiliate.affiliateID = Convert.ToInt32(reader["AffiliateID"]);
					affiliate.orderCode = reader["OrderCode"].ToString();
					affiliate.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					affiliate.userID = Convert.ToInt32(reader["UserID"]);
					affiliate.productID = Convert.ToInt32(reader["ProductID"]);
					affiliate.productName = reader["ProductName"].ToString();
					affiliate.productPrice = Convert.ToDecimal(reader["ProductPrice"]);
					affiliate.commission = Convert.ToDecimal(reader["Commission"]);
					affiliate.dateBuy = Convert.ToDateTime(reader["DateBuy"]);
					affiliate.status = Convert.ToBoolean(reader["Status"]);
					affiliate.extracol3 = reader["extracol3"].ToString();
					affiliateList.Add(affiliate);

				}
			}
			finally
			{
				reader.Close();
			}

			return affiliateList;

		}

		/// <summary>
		/// Gets an IList with all instances of Affiliate.
		/// </summary>
		public static List<Affiliate> GetAll()
		{
			IDataReader reader = DBAffiliate.GetAll();
			return LoadListFromReader(reader);

		}

		/// <summary>
		/// Gets an IList with page of instances of Affiliate.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static List<Affiliate> GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliate.GetPage(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}

		public static List<Affiliate> GetPage(int pageNumber, int pageSize, out int totalPages, int userid)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliate.GetPage(pageNumber, pageSize, out totalPages, userid);
			return LoadListFromReader(reader);
		}



		#endregion

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByAffiliateID(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.AffiliateID.CompareTo(affiliate2.AffiliateID);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByOrderCode(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.OrderCode.CompareTo(affiliate2.OrderCode);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByAffiliateUserID(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.AffiliateUserID.CompareTo(affiliate2.AffiliateUserID);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByUserID(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.UserID.CompareTo(affiliate2.UserID);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByProductID(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.ProductID.CompareTo(affiliate2.ProductID);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByProductName(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.ProductName.CompareTo(affiliate2.ProductName);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByDateBuy(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.DateBuy.CompareTo(affiliate2.DateBuy);
		}
		/// <summary>
		/// Compares 2 instances of Affiliate.
		/// </summary>
		public static int CompareByextracol3(Affiliate affiliate1, Affiliate affiliate2)
		{
			return affiliate1.Extracol3.CompareTo(affiliate2.Extracol3);
		}

		#endregion


	}

}

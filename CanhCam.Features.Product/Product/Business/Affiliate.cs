
// Author:					Tran Quoc Vuong
// Created:					2022-8-8
// Last Modified:			2022-8-8

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
			int affiliateUserID)
		{
			this.GetAffiliate(
				affiliateUserID);
		}

		#endregion

		#region Private Properties

		private int affiliateID = -1;
		private int orderID = -1;
		private int affiliateUserID = -1;
		private DateTime dateCreate = DateTime.Now;
		private string extracol = string.Empty;
		private string extracol1 = string.Empty;
		private string extracol2 = string.Empty;
		private string orderCode = string.Empty;
		private decimal orderSubTotal = decimal.Zero;
		private int orderStatus;
		private string productName = string.Empty;
		private decimal price = decimal.Zero;
		private int quantity = 0;

		#endregion

		#region Public Properties

		public int AffiliateID
		{
			get { return affiliateID; }
			set { affiliateID = value; }
		}
		public int OrderID
		{
			get { return orderID; }
			set { orderID = value; }
		}
		public int AffiliateUserID
		{
			get { return affiliateUserID; }
			set { affiliateUserID = value; }
		}
		public DateTime DateCreate
		{
			get { return dateCreate; }
			set { dateCreate = value; }
		}
		public string Extracol
		{
			get { return extracol; }
			set { extracol = value; }
		}
		public string Extracol1
		{
			get { return extracol1; }
			set { extracol1 = value; }
		}
		public string Extracol2
		{
			get { return extracol2; }
			set { extracol2 = value; }
		}
		public string OrderCode
		{
			get { return orderCode; }
			set { orderCode = value; }
		}
		public string ProductName
		{
			get { return productName; }
			set { productName = value; }
		}
		public decimal Price
		{
			get { return price; }
			set { price = value; }
		}
		public decimal OrderSubTotal
		{
			get { return orderSubTotal; }
			set { orderSubTotal = value; }
		}
		public int Quantity
		{
			get { return quantity; }
			set { quantity = value; }
		}
		public int OrderStatus
		{
			get { return orderStatus; }
			set { orderStatus = value; }
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
					this.orderID = Convert.ToInt32(reader["OrderID"]);
					this.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					this.dateCreate = Convert.ToDateTime(reader["DateCreate"]);
					this.extracol = reader["Extracol"].ToString();
					this.extracol1 = reader["Extracol1"].ToString();
					this.extracol2 = reader["Extracol2"].ToString();
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
				this.orderID,
				this.affiliateUserID,
				this.dateCreate,
				this.extracol,
				this.extracol1,
				this.extracol2);

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
				this.orderID,
				this.affiliateUserID,
				this.dateCreate,
				this.extracol,
				this.extracol1,
				this.extracol2);

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
					affiliate.orderID = Convert.ToInt32(reader["OrderID"]);
					affiliate.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					affiliate.dateCreate = Convert.ToDateTime(reader["DateCreate"]);
					affiliate.extracol = reader["Extracol"].ToString();
					affiliate.extracol1 = reader["Extracol1"].ToString();
					affiliate.extracol2 = reader["Extracol2"].ToString();
					affiliateList.Add(affiliate);

				}
			}
			finally
			{
				reader.Close();
			}

			return affiliateList;

		}

		private static List<Affiliate> LoadListFromReaderCustom(IDataReader reader)
		{
			List<Affiliate> affiliateList = new List<Affiliate>();
			try
			{
				while (reader.Read())
				{
					Affiliate affiliate = new Affiliate();
					affiliate.affiliateID = Convert.ToInt32(reader["AffiliateID"]);
					affiliate.orderID = Convert.ToInt32(reader["OrderID"]);
					affiliate.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					affiliate.dateCreate = Convert.ToDateTime(reader["DateCreate"]);
					affiliate.extracol = reader["Extracol"].ToString();
					affiliate.extracol1 = reader["Extracol1"].ToString();
					affiliate.extracol2 = reader["Extracol2"].ToString();
					affiliate.orderCode = reader["OrderCode"].ToString();
					affiliate.productName = reader["Title"].ToString();
					affiliate.price = Convert.ToDecimal(reader["Price"]);
					affiliate.orderSubTotal = Convert.ToDecimal(reader["OrderSubTotal"]);
					affiliate.quantity = Convert.ToInt32(reader["Quantity"]);
					affiliate.orderStatus = Convert.ToInt32(reader["OrderStatus"]);
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

		public static List<Affiliate> GetPageAchieve(int pageNumber, int pageSize, out int totalPages)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliate.GetPageAchieve(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}

		public static List<Affiliate> GetPage(int pageNumber, int pageSize, out int totalPages, int userID)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliate.GetPage(pageNumber, pageSize, out totalPages, userID);
			return LoadListFromReaderCustom(reader);
		}



		#endregion

		


	}

}


// Author:					Tran Quoc Vuong
// Created:					2022-8-1
// Last Modified:			2022-8-1

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

	public class AffiliateUser
	{

		#region Constructors

		public AffiliateUser()
		{ }


		public AffiliateUser(
			int affiID)
		{
			this.GetAffiliateUser(
				affiID);
		}

		#endregion

		#region Private Properties

		private int affiID = -1;
		private int affiliateUserID = -1;
		private decimal totalCommission;
		private decimal commissionWait;
		private decimal commissionPay;
		private int totalOrder = -1;
		private bool statusPayment = false;
		private string percentCommission = string.Empty;
		private string extracol1 = string.Empty;
		private string extracol2 = string.Empty;

		#endregion

		#region Public Properties

		public int AffiID
		{
			get { return affiID; }
			set { affiID = value; }
		}
		public int AffiliateUserID
		{
			get { return affiliateUserID; }
			set { affiliateUserID = value; }
		}
		public decimal TotalCommission
		{
			get { return totalCommission; }
			set { totalCommission = value; }
		}
		public decimal CommissionWait
		{
			get { return commissionWait; }
			set { commissionWait = value; }
		}
		public decimal CommissionPay
		{
			get { return commissionPay; }
			set { commissionPay = value; }
		}
		public int TotalOrder
		{
			get { return totalOrder; }
			set { totalOrder = value; }
		}
		public bool StatusPayment
		{
			get { return statusPayment; }
			set { statusPayment = value; }
		}
		public string PercentCommission
		{
			get { return percentCommission; }
			set { percentCommission = value; }
		}
		public string Extracol1
		{
			get { return Extracol1; }
			set { extracol1 = value; }
		}
		public string Extracol2
		{
			get { return extracol2; }
			set { extracol2 = value; }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of AffiliateUser.
		/// </summary>
		/// <param name="affiID"> affiID </param>
		private void GetAffiliateUser(
			int affiID)
		{
			using (IDataReader reader = DBAffiliateUser.GetOne(
				affiID))
			{
				if (reader.Read())
				{
					this.affiID = Convert.ToInt32(reader["AffiID"]);
					this.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					this.totalCommission = Convert.ToDecimal(reader["TotalCommission"]);
					this.commissionWait = Convert.ToDecimal(reader["CommissionWait"]);
					this.commissionPay = Convert.ToDecimal(reader["CommissionPay"]);
					this.totalOrder = Convert.ToInt32(reader["TotalOrder"]);
					this.statusPayment = Convert.ToBoolean(reader["StatusPayment"]);
					this.percentCommission = reader["PercentCommission"].ToString();
					this.extracol1 = reader["extracol1"].ToString();
					this.extracol2 = reader["extracol2"].ToString();
				}
			}

		}

		/// <summary>
		/// Persists a new instance of AffiliateUser. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			int newID = 0;

			newID = DBAffiliateUser.Create(
				this.affiliateUserID,
				this.totalCommission,
				this.commissionWait,
				this.commissionPay,
				this.totalOrder,
				this.statusPayment,
				this.percentCommission,
				this.extracol1,
				this.extracol2);

			this.affiID = newID;

			return (newID > 0);

		}


		/// <summary>
		/// Updates this instance of AffiliateUser. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBAffiliateUser.Update(
				this.affiID,
				this.affiliateUserID,
				this.totalCommission,
				this.commissionWait,
				this.commissionPay,
				this.totalOrder,
				this.statusPayment,
				this.percentCommission,
				this.extracol1,
				this.extracol2);

		}





		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of AffiliateUser. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.affiID > 0)
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
		/// Deletes an instance of AffiliateUser. Returns true on success.
		/// </summary>
		/// <param name="affiID"> affiID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int affiID)
		{
			return DBAffiliateUser.Delete(
				affiID);
		}


		/// <summary>
		/// Gets a count of AffiliateUser. 
		/// </summary>
		public static int GetCount()
		{
			return DBAffiliateUser.GetCount();
		}

		private static List<AffiliateUser> LoadListFromReader(IDataReader reader)
		{
			List<AffiliateUser> affiliateUserList = new List<AffiliateUser>();
			try
			{
				while (reader.Read())
				{
					AffiliateUser affiliateUser = new AffiliateUser();
					affiliateUser.affiID = Convert.ToInt32(reader["AffiID"]);
					affiliateUser.affiliateUserID = Convert.ToInt32(reader["AffiliateUserID"]);
					affiliateUser.totalCommission = Convert.ToDecimal(reader["TotalCommission"]);
					affiliateUser.commissionWait = Convert.ToDecimal(reader["CommissionWait"]);
					affiliateUser.commissionPay = Convert.ToDecimal(reader["CommissionPay"]);
					affiliateUser.totalOrder = Convert.ToInt32(reader["TotalOrder"]);
					affiliateUser.statusPayment = Convert.ToBoolean(reader["StatusPayment"]);
					affiliateUser.percentCommission = reader["PercentCommission"].ToString();
					affiliateUser.extracol1 = reader["extracol1"].ToString();
					affiliateUser.extracol2 = reader["extracol2"].ToString();
					affiliateUserList.Add(affiliateUser);

				}
			}
			finally
			{
				reader.Close();
			}

			return affiliateUserList;

		}

		/// <summary>
		/// Gets an IList with all instances of AffiliateUser.
		/// </summary>
		public static List<AffiliateUser> GetAll()
		{
			IDataReader reader = DBAffiliateUser.GetAll();
			return LoadListFromReader(reader);

		}

		/// <summary>
		/// Gets an IList with page of instances of AffiliateUser.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static List<AffiliateUser> GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliateUser.GetPage(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}



		#endregion

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of AffiliateUser.
		/// </summary>
		public static int CompareByAffiID(AffiliateUser affiliateUser1, AffiliateUser affiliateUser2)
		{
			return affiliateUser1.AffiID.CompareTo(affiliateUser2.AffiID);
		}
		/// <summary>
		/// Compares 2 instances of AffiliateUser.
		/// </summary>
		public static int CompareByAffiliateUserID(AffiliateUser affiliateUser1, AffiliateUser affiliateUser2)
		{
			return affiliateUser1.AffiliateUserID.CompareTo(affiliateUser2.AffiliateUserID);
		}
		/// <summary>
		/// Compares 2 instances of AffiliateUser.
		/// </summary>
		public static int CompareByTotalOrder(AffiliateUser affiliateUser1, AffiliateUser affiliateUser2)
		{
			return affiliateUser1.TotalOrder.CompareTo(affiliateUser2.TotalOrder);
		}
		/// <summary>
		/// Compares 2 instances of AffiliateUser.
		/// </summary>
		public static int CompareByextracol(AffiliateUser affiliateUser1, AffiliateUser affiliateUser2)
		{
			return affiliateUser1.percentCommission.CompareTo(affiliateUser2.percentCommission);
		}
		/// <summary>
		/// Compares 2 instances of AffiliateUser.
		/// </summary>
		public static int CompareByextracol1(AffiliateUser affiliateUser1, AffiliateUser affiliateUser2)
		{
			return affiliateUser1.extracol1.CompareTo(affiliateUser2.extracol1);
		}
		/// <summary>
		/// Compares 2 instances of AffiliateUser.
		/// </summary>
		public static int CompareByextracol2(AffiliateUser affiliateUser1, AffiliateUser affiliateUser2)
		{
			return affiliateUser1.extracol2.CompareTo(affiliateUser2.extracol2);
		}

		#endregion


	}

}


// Author:					Tran Quoc Vuong
// Created:					2022-8-9
// Last Modified:			2022-8-9

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

	public class AffiliatePayment
	{

		#region Constructors

		public AffiliatePayment()
		{ }


		public AffiliatePayment(
			int AffUser)
		{
			this.GetAffiliatePayment(
				AffUser);
		}

		public AffiliatePayment(
			Guid AffGuid)
		{
			this.GetAffiliatePayment(
				AffGuid);
		}

		#endregion

		#region Private Properties

		private int affID = -1;
		private Guid affGuid = Guid.Empty;
		private int affUser = -1;
		private string affBankUserName = string.Empty;
		private string affBankNumber = string.Empty;
		private string affBankName = string.Empty;
		private string affBankBranch = string.Empty;
		private decimal withdrawMoney;
		private DateTime dateRequire = DateTime.Now;
		private bool status = false;
		private DateTime? dateRespond = null;
		private string extraCol = string.Empty;

		#endregion

		#region Public Properties

		public int AffID
		{
			get { return affID; }
			set { affID = value; }
		}
		public Guid AffGuid
		{
			get { return affGuid; }
			set { affGuid = value; }
		}
		public int AffUser
		{
			get { return affUser; }
			set { affUser = value; }
		}
		public string AffBankUserName
		{
			get { return affBankUserName; }
			set { affBankUserName = value; }
		}
		public string AffBankNumber
		{
			get { return affBankNumber; }
			set { affBankNumber = value; }
		}
		public string AffBankName
		{
			get { return affBankName; }
			set { affBankName = value; }
		}
		public string AffBankBranch
		{
			get { return affBankBranch; }
			set { affBankBranch = value; }
		}
		public decimal WithdrawMoney
		{
			get { return withdrawMoney; }
			set { withdrawMoney = value; }
		}
		public DateTime DateRequire
		{
			get { return dateRequire; }
			set { dateRequire = value; }
		}
		public bool Status
		{
			get { return status; }
			set { status = value; }
		}
		public DateTime? DateRespond
		{
			get { return dateRespond; }
			set { dateRespond = value; }
		}
		public string ExtraCol
		{
			get { return extraCol; }
			set { extraCol = value; }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of AffiliatePayment.
		/// </summary>
		/// <param name="affID"> affID </param>
		private void GetAffiliatePayment(
			int AffUser)
		{
			using (IDataReader reader = DBAffiliatePayment.GetOne(
				AffUser))
			{
				if (reader.Read())
				{
					this.affID = Convert.ToInt32(reader["AffID"]);
					this.affGuid = new Guid(reader["AffGuid"].ToString());
					this.affUser = Convert.ToInt32(reader["AffUser"]);
					this.affBankUserName = reader["AffBankUserName"].ToString();
					this.affBankNumber = reader["AffBankNumber"].ToString();
					this.affBankName = reader["AffBankName"].ToString();
					this.affBankBranch = reader["AffBankBranch"].ToString();
					this.withdrawMoney = Convert.ToDecimal(reader["WithdrawMoney"]);
					this.dateRequire = Convert.ToDateTime(reader["DateRequire"]);
					this.status = Convert.ToBoolean(reader["Status"]);
					if (reader["DateRespond"] != DBNull.Value)
						this.dateRespond = Convert.ToDateTime(reader["DateRespond"].ToString());
					this.extraCol = reader["ExtraCol"].ToString();
				}
				
			}

		}

		private void GetAffiliatePayment(
			Guid AffGuid)
		{
			using (IDataReader reader = DBAffiliatePayment.GetOne(
				AffGuid))
			{
				if (reader.Read())
				{
					this.affID = Convert.ToInt32(reader["AffID"]);
					this.affGuid = new Guid(reader["AffGuid"].ToString());
					this.affUser = Convert.ToInt32(reader["AffUser"]);
					this.affBankUserName = reader["AffBankUserName"].ToString();
					this.affBankNumber = reader["AffBankNumber"].ToString();
					this.affBankName = reader["AffBankName"].ToString();
					this.affBankBranch = reader["AffBankBranch"].ToString();
					this.withdrawMoney = Convert.ToDecimal(reader["WithdrawMoney"]);
					this.dateRequire = Convert.ToDateTime(reader["DateRequire"]);
					this.status = Convert.ToBoolean(reader["Status"]);
					if (reader["DateRespond"] != DBNull.Value)
						this.dateRespond = Convert.ToDateTime(reader["DateRespond"].ToString());
					this.extraCol = reader["ExtraCol"].ToString();
				}

			}

		}

		/// <summary>
		/// Persists a new instance of AffiliatePayment. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			int newID = 0;

			newID = DBAffiliatePayment.Create(
				this.affGuid,
				this.affUser,
				this.affBankUserName,
				this.affBankNumber,
				this.affBankName,
				this.affBankBranch,
				this.withdrawMoney,
				this.dateRequire,
				this.status,
				this.dateRespond,
				this.extraCol);

			this.affID = newID;

			return (newID > 0);

		}


		/// <summary>
		/// Updates this instance of AffiliatePayment. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBAffiliatePayment.Update(
				this.affID,
				this.affGuid,
				this.affUser,
				this.affBankUserName,
				this.affBankNumber,
				this.affBankName,
				this.affBankBranch,
				this.withdrawMoney,
				this.dateRequire,
				this.status,
				this.dateRespond,
				this.extraCol);

		}





		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of AffiliatePayment. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.affID > 0)
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
		/// Deletes an instance of AffiliatePayment. Returns true on success.
		/// </summary>
		/// <param name="affID"> affID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int affID)
		{
			return DBAffiliatePayment.Delete(
				affID);
		}


		/// <summary>
		/// Gets a count of AffiliatePayment. 
		/// </summary>
		public static int GetCount()
		{
			return DBAffiliatePayment.GetCount();
		}

		private static List<AffiliatePayment> LoadListFromReader(IDataReader reader)
		{
			List<AffiliatePayment> affiliatePaymentList = new List<AffiliatePayment>();
			try
			{
				while (reader.Read())
				{
					AffiliatePayment affiliatePayment = new AffiliatePayment();
					affiliatePayment.affID = Convert.ToInt32(reader["AffID"]);
					affiliatePayment.affGuid = new Guid(reader["AffGuid"].ToString());
					affiliatePayment.affUser = Convert.ToInt32(reader["AffUser"]);
					affiliatePayment.affBankUserName = reader["AffBankUserName"].ToString();
					affiliatePayment.affBankNumber = reader["AffBankNumber"].ToString();
					affiliatePayment.affBankName = reader["AffBankName"].ToString();
					affiliatePayment.affBankBranch = reader["AffBankBranch"].ToString();
					affiliatePayment.withdrawMoney = Convert.ToDecimal(reader["WithdrawMoney"]);
					affiliatePayment.dateRequire = Convert.ToDateTime(reader["DateRequire"]);
					affiliatePayment.status = Convert.ToBoolean(reader["Status"]);
					if (reader["DateRespond"] != DBNull.Value)
						affiliatePayment.dateRespond = Convert.ToDateTime(reader["DateRespond"].ToString());
					affiliatePayment.extraCol = reader["ExtraCol"].ToString();
					affiliatePaymentList.Add(affiliatePayment);

				}
			}
			finally
			{
				reader.Close();
			}

			return affiliatePaymentList;

		}

		/// <summary>
		/// Gets an IList with all instances of AffiliatePayment.
		/// </summary>
		public static List<AffiliatePayment> GetAll()
		{
			IDataReader reader = DBAffiliatePayment.GetAll();
			return LoadListFromReader(reader);

		}

		/// <summary>
		/// Gets an IList with page of instances of AffiliatePayment.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static List<AffiliatePayment> GetPage(int pageNumber, int pageSize, out int totalPages,int userID)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliatePayment.GetPage(pageNumber, pageSize, out totalPages, userID);
			return LoadListFromReader(reader);
		}

		public static List<AffiliatePayment> GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			totalPages = 1;
			IDataReader reader = DBAffiliatePayment.GetPage(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}
		#endregion


	}

}

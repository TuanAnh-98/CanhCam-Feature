
// Author:					Tran Quoc Vuong
// Created:					2022-7-4
// Last Modified:			2022-7-4

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

	public class PhongBan
	{

		#region Constructors

		public PhongBan()
		{ }


		public PhongBan(
			int itemId)
		{
			this.GetPhongBan(
				itemId);
		}

		#endregion

		#region Private Properties

		private int itemId = -1;
		private string title = string.Empty;
		private string code = string.Empty;

		#endregion

		#region Public Properties

		public int ItemId
		{
			get { return itemId; }
			set { itemId = value; }
		}
		public string Title
		{
			get { return title; }
			set { title = value; }
		}
		public string Code
		{
			get { return code; }
			set { code = value; }
		}

		#endregion

		#region Private Methods

		/// <summary>
		/// Gets an instance of PhongBan.
		/// </summary>
		/// <param name="itemId"> itemId </param>
		private void GetPhongBan(
			int itemId)
		{
			using (IDataReader reader = DBPhongBan.GetOne(
				itemId))
			{
				if (reader.Read())
				{
					this.itemId = Convert.ToInt32(reader["ItemId"]);
					this.title = reader["Title"].ToString();
					this.code = reader["Code"].ToString();
				}
			}

		}

		/// <summary>
		/// Persists a new instance of PhongBan. Returns true on success.
		/// </summary>
		/// <returns></returns>
		private bool Create()
		{
			int newID = 0;

			newID = DBPhongBan.Create(
				this.title,
				this.code);

			this.itemId = newID;

			return (newID > 0);

		}


		/// <summary>
		/// Updates this instance of PhongBan. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		private bool Update()
		{

			return DBPhongBan.Update(
				this.itemId,
				this.title,
				this.code);

		}





		#endregion

		#region Public Methods

		/// <summary>
		/// Saves this instance of PhongBan. Returns true on success.
		/// </summary>
		/// <returns>bool</returns>
		public bool Save()
		{
			if (this.itemId > 0)
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
		/// Deletes an instance of PhongBan. Returns true on success.
		/// </summary>
		/// <param name="itemId"> itemId </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int itemId)
		{
			return DBPhongBan.Delete(
				itemId);
		}


		/// <summary>
		/// Gets a count of PhongBan. 
		/// </summary>
		public static int GetCount()
		{
			return DBPhongBan.GetCount();
		}

		private static List<PhongBan> LoadListFromReader(IDataReader reader)
		{
			List<PhongBan> phongBanList = new List<PhongBan>();
			try
			{
				while (reader.Read())
				{
					PhongBan phongBan = new PhongBan();
					phongBan.itemId = Convert.ToInt32(reader["ItemId"]);
					phongBan.title = reader["Title"].ToString();
					phongBan.code = reader["Code"].ToString();
					phongBanList.Add(phongBan);

				}
			}
			finally
			{
				reader.Close();
			}

			return phongBanList;

		}

		/// <summary>
		/// Gets an IList with all instances of PhongBan.
		/// </summary>
		public static List<PhongBan> GetAll()
		{
			IDataReader reader = DBPhongBan.GetAll();
			return LoadListFromReader(reader);

		}

		/// <summary>
		/// Gets an IList with page of instances of PhongBan.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static List<PhongBan> GetPage(int pageNumber, int pageSize, out int totalPages)
		{
			totalPages = 1;
			IDataReader reader = DBPhongBan.GetPage(pageNumber, pageSize, out totalPages);
			return LoadListFromReader(reader);
		}



		#endregion

		#region Comparison Methods

		/// <summary>
		/// Compares 2 instances of PhongBan.
		/// </summary>
		public static int CompareByItemId(PhongBan phongBan1, PhongBan phongBan2)
		{
			return phongBan1.ItemId.CompareTo(phongBan2.ItemId);
		}
		/// <summary>
		/// Compares 2 instances of PhongBan.
		/// </summary>
		public static int CompareByTitle(PhongBan phongBan1, PhongBan phongBan2)
		{
			return phongBan1.Title.CompareTo(phongBan2.Title);
		}
		/// <summary>
		/// Compares 2 instances of PhongBan.
		/// </summary>
		public static int CompareByCode(PhongBan phongBan1, PhongBan phongBan2)
		{
			return phongBan1.Code.CompareTo(phongBan2.Code);
		}

		#endregion


	}

}

// Author:					Tran Quoc Vuong
// Created:					2022-8-8
// Last Modified:			2022-8-8

using System;
using System.Data;

namespace CanhCam.Data
{

	public static class DBAffiliate
	{

		/// <summary>
		/// Inserts a row in the gb_Affiliate table. Returns new integer id.
		/// </summary>
		/// <param name="orderID"> orderID </param>
		/// <param name="affiliateUserID"> affiliateUserID </param>
		/// <param name="dateCreate"> dateCreate </param>
		/// <param name="extracol"> extracol </param>
		/// <param name="extracol1"> extracol1 </param>
		/// <param name="extracol2"> extracol2 </param>
		/// <returns>int</returns>
		public static int Create(
			int orderID,
			int affiliateUserID,
			DateTime dateCreate,
			string extracol,
			string extracol1,
			string extracol2)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Affiliate_Insert", 6);
			sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
			sph.DefineSqlParameter("@DateCreate", SqlDbType.DateTime, ParameterDirection.Input, dateCreate);
			sph.DefineSqlParameter("@Extracol", SqlDbType.NChar, 10, ParameterDirection.Input, extracol);
			sph.DefineSqlParameter("@Extracol1", SqlDbType.NChar, 10, ParameterDirection.Input, extracol1);
			sph.DefineSqlParameter("@Extracol2", SqlDbType.NChar, 10, ParameterDirection.Input, extracol2);
			int newID = Convert.ToInt32(sph.ExecuteScalar());
			return newID;
		}


		/// <summary>
		/// Updates a row in the gb_Affiliate table. Returns true if row updated.
		/// </summary>
		/// <param name="affiliateID"> affiliateID </param>
		/// <param name="orderID"> orderID </param>
		/// <param name="affiliateUserID"> affiliateUserID </param>
		/// <param name="dateCreate"> dateCreate </param>
		/// <param name="extracol"> extracol </param>
		/// <param name="extracol1"> extracol1 </param>
		/// <param name="extracol2"> extracol2 </param>
		/// <returns>bool</returns>
		public static bool Update(
			int affiliateID,
			int orderID,
			int affiliateUserID,
			DateTime dateCreate,
			string extracol,
			string extracol1,
			string extracol2)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Affiliate_Update", 7);
			sph.DefineSqlParameter("@AffiliateID", SqlDbType.Int, ParameterDirection.Input, affiliateID);
			sph.DefineSqlParameter("@OrderID", SqlDbType.Int, ParameterDirection.Input, orderID);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
			sph.DefineSqlParameter("@DateCreate", SqlDbType.DateTime, ParameterDirection.Input, dateCreate);
			sph.DefineSqlParameter("@Extracol", SqlDbType.NChar, 10, ParameterDirection.Input, extracol);
			sph.DefineSqlParameter("@Extracol1", SqlDbType.NChar, 10, ParameterDirection.Input, extracol1);
			sph.DefineSqlParameter("@Extracol2", SqlDbType.NChar, 10, ParameterDirection.Input, extracol2);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Deletes a row from the gb_Affiliate table. Returns true if row deleted.
		/// </summary>
		/// <param name="affiliateID"> affiliateID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int affiliateID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Affiliate_Delete", 1);
			sph.DefineSqlParameter("@AffiliateID", SqlDbType.Int, ParameterDirection.Input, affiliateID);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Gets an IDataReader with one row from the gb_Affiliate table.
		/// </summary>
		/// <param name="affiliateID"> affiliateID </param>
		public static IDataReader GetOne(
			int affiliateID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Affiliate_SelectOneByAffiliateUserID", 1);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateID);
			return sph.ExecuteReader();

		}

		/// <summary>
		/// Gets a count of rows in the gb_Affiliate table.
		/// </summary>
		public static int GetCount()
		{

			return Convert.ToInt32(SqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_Affiliate_GetCount",
				null));

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the gb_Affiliate table.
		/// </summary>
		public static IDataReader GetAll()
		{

			return SqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_Affiliate_SelectAll",
				null);

		}

		/// <summary>
		/// Gets a page of data from the gb_Affiliate table.
		/// </summary>
		/// <param name="pageNumber">The page number.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="totalPages">total pages</param>
		public static IDataReader GetPage(
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			totalPages = 1;
			int totalRows
				= GetCount();

			if (pageSize > 0) totalPages = totalRows / pageSize;

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				int remainder;
				Math.DivRem(totalRows, pageSize, out remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Affiliate_SelectPage", 2);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

		public static IDataReader GetPageAchieve(
			int pageNumber,
			int pageSize,
			out int totalPages)
		{
			totalPages = 1;
			int totalRows
				= GetCount();

			if (pageSize > 0) totalPages = totalRows / pageSize;

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				int remainder;
				Math.DivRem(totalRows, pageSize, out remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Affiliate_SelectPageAchieve", 2);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

		public static IDataReader GetPage(
			int pageNumber,
			int pageSize,
			out int totalPages,
			int userID)
		{
			totalPages = 1;
			int totalRows
				= GetCount();

			if (pageSize > 0) totalPages = totalRows / pageSize;

			if (totalRows <= pageSize)
			{
				totalPages = 1;
			}
			else
			{
				int remainder;
				Math.DivRem(totalRows, pageSize, out remainder);
				if (remainder > 0)
				{
					totalPages += 1;
				}
			}

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Affiliate_SelectPageByUserID", 3);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, userID);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

	}

}

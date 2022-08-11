// Author:					Tran Quoc Vuong
// Created:					2022-8-4
// Last Modified:			2022-8-4

using System;
using System.Data;

namespace CanhCam.Data
{

	public static class DBAffiliate
	{

		/// <summary>
		/// Inserts a row in the gb_Affiliate table. Returns new integer id.
		/// </summary>
		/// <param name="orderCode"> orderCode </param>
		/// <param name="affiliateUserID"> affiliateUserID </param>
		/// <param name="userID"> userID </param>
		/// <param name="productID"> productID </param>
		/// <param name="productName"> productName </param>
		/// <param name="productPrice"> productPrice </param>
		/// <param name="commission"> commission </param>
		/// <param name="dateBuy"> dateBuy </param>
		/// <param name="status"> status </param>
		/// <param name="extracol3"> extracol3 </param>
		/// <returns>int</returns>
		public static int Create(
			string orderCode,
			int affiliateUserID,
			int userID,
			int productID,
			string productName,
			decimal productPrice,
			decimal commission,
			DateTime dateBuy,
			bool status,
			string extracol3)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Affiliate_Insert", 10);
			sph.DefineSqlParameter("@OrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderCode);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
			sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
			sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
			sph.DefineSqlParameter("@ProductName", SqlDbType.NVarChar, 255, ParameterDirection.Input, productName);
			sph.DefineSqlParameter("@ProductPrice", SqlDbType.Decimal, ParameterDirection.Input, productPrice);
			sph.DefineSqlParameter("@Commission", SqlDbType.Decimal, ParameterDirection.Input, commission);
			sph.DefineSqlParameter("@DateBuy", SqlDbType.DateTime, ParameterDirection.Input, dateBuy);
			sph.DefineSqlParameter("@Status", SqlDbType.Bit, ParameterDirection.Input, status);
			sph.DefineSqlParameter("@extracol3", SqlDbType.NChar, 10, ParameterDirection.Input, extracol3);
			int newID = Convert.ToInt32(sph.ExecuteScalar());
			return newID;
		}


		/// <summary>
		/// Updates a row in the gb_Affiliate table. Returns true if row updated.
		/// </summary>
		/// <param name="affiliateID"> affiliateID </param>
		/// <param name="orderCode"> orderCode </param>
		/// <param name="affiliateUserID"> affiliateUserID </param>
		/// <param name="userID"> userID </param>
		/// <param name="productID"> productID </param>
		/// <param name="productName"> productName </param>
		/// <param name="productPrice"> productPrice </param>
		/// <param name="commission"> commission </param>
		/// <param name="dateBuy"> dateBuy </param>
		/// <param name="status"> status </param>
		/// <param name="extracol3"> extracol3 </param>
		/// <returns>bool</returns>
		public static bool Update(
			int affiliateID,
			string orderCode,
			int affiliateUserID,
			int userID,
			int productID,
			string productName,
			decimal productPrice,
			decimal commission,
			DateTime dateBuy,
			bool status,
			string extracol3)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Affiliate_Update", 11);
			sph.DefineSqlParameter("@AffiliateID", SqlDbType.Int, ParameterDirection.Input, affiliateID);
			sph.DefineSqlParameter("@OrderCode", SqlDbType.NVarChar, 50, ParameterDirection.Input, orderCode);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
			sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
			sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
			sph.DefineSqlParameter("@ProductName", SqlDbType.NVarChar, 255, ParameterDirection.Input, productName);
			sph.DefineSqlParameter("@ProductPrice", SqlDbType.Decimal, ParameterDirection.Input, productPrice);
			sph.DefineSqlParameter("@Commission", SqlDbType.Decimal, ParameterDirection.Input, commission);
			sph.DefineSqlParameter("@DateBuy", SqlDbType.DateTime, ParameterDirection.Input, dateBuy);
			sph.DefineSqlParameter("@Status", SqlDbType.Bit, ParameterDirection.Input, status);
			sph.DefineSqlParameter("@extracol3", SqlDbType.NChar, 10, ParameterDirection.Input, extracol3);
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
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Affiliate_SelectOne", 1);
			sph.DefineSqlParameter("@AffiliateID", SqlDbType.Int, ParameterDirection.Input, affiliateID);
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

		public static IDataReader GetPage(
			int pageNumber,
			int pageSize,
			out int totalPages,
			int userid)
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
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userid);
			return sph.ExecuteReader();

		}

	}

}

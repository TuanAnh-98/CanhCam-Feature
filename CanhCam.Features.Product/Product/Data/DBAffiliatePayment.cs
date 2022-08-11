// Author:					Tran Quoc Vuong
// Created:					2022-8-9
// Last Modified:			2022-8-9

using System;
using System.Data;

namespace CanhCam.Data
{

	public static class DBAffiliatePayment
	{

		/// <summary>
		/// Inserts a row in the gb_AffiliatePayment table. Returns new integer id.
		/// </summary>
		/// <param name="affGuid"> affGuid </param>
		/// <param name="affUser"> affUser </param>
		/// <param name="affBankUserName"> affBankUserName </param>
		/// <param name="affBankNumber"> affBankNumber </param>
		/// <param name="affBankName"> affBankName </param>
		/// <param name="affBankBranch"> affBankBranch </param>
		/// <param name="withdrawMoney"> withdrawMoney </param>
		/// <param name="dateRequire"> dateRequire </param>
		/// <param name="status"> status </param>
		/// <param name="dateRespond"> dateRespond </param>
		/// <param name="extraCol"> extraCol </param>
		/// <returns>int</returns>
		public static int Create(
			Guid affGuid,
			int affUser,
			string affBankUserName,
			string affBankNumber,
			string affBankName,
			string affBankBranch,
			decimal withdrawMoney,
			DateTime dateRequire,
			bool status,
			DateTime? dateRespond,
			string extraCol)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_AffiliatePayment_Insert", 11);
			sph.DefineSqlParameter("@AffGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, affGuid);
			sph.DefineSqlParameter("@AffUser", SqlDbType.Int, ParameterDirection.Input, affUser);
			sph.DefineSqlParameter("@AffBankUserName", SqlDbType.NVarChar, 50, ParameterDirection.Input, affBankUserName);
			sph.DefineSqlParameter("@AffBankNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, affBankNumber);
			sph.DefineSqlParameter("@AffBankName", SqlDbType.NVarChar, 50, ParameterDirection.Input, affBankName);
			sph.DefineSqlParameter("@AffBankBranch", SqlDbType.NVarChar, 255, ParameterDirection.Input, affBankBranch);
			sph.DefineSqlParameter("@WithdrawMoney", SqlDbType.Decimal, ParameterDirection.Input, withdrawMoney);
			sph.DefineSqlParameter("@DateRequire", SqlDbType.DateTime, ParameterDirection.Input, dateRequire);
			sph.DefineSqlParameter("@Status", SqlDbType.Bit, ParameterDirection.Input, status);
			sph.DefineSqlParameter("@DateRespond", SqlDbType.DateTime, ParameterDirection.Input, dateRespond);
			sph.DefineSqlParameter("@ExtraCol", SqlDbType.NChar, 10, ParameterDirection.Input, extraCol);
			int newID = Convert.ToInt32(sph.ExecuteScalar());
			return newID;
		}


		/// <summary>
		/// Updates a row in the gb_AffiliatePayment table. Returns true if row updated.
		/// </summary>
		/// <param name="affID"> affID </param>
		/// <param name="affGuid"> affGuid </param>
		/// <param name="affUser"> affUser </param>
		/// <param name="affBankUserName"> affBankUserName </param>
		/// <param name="affBankNumber"> affBankNumber </param>
		/// <param name="affBankName"> affBankName </param>
		/// <param name="affBankBranch"> affBankBranch </param>
		/// <param name="withdrawMoney"> withdrawMoney </param>
		/// <param name="dateRequire"> dateRequire </param>
		/// <param name="status"> status </param>
		/// <param name="dateRespond"> dateRespond </param>
		/// <param name="extraCol"> extraCol </param>
		/// <returns>bool</returns>
		public static bool Update(
			int affID,
			Guid affGuid,
			int affUser,
			string affBankUserName,
			string affBankNumber,
			string affBankName,
			string affBankBranch,
			decimal withdrawMoney,
			DateTime dateRequire,
			bool status,
			DateTime? dateRespond,
			string extraCol)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_AffiliatePayment_Update", 12);
			sph.DefineSqlParameter("@AffID", SqlDbType.Int, ParameterDirection.Input, affID);
			sph.DefineSqlParameter("@AffGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, affGuid);
			sph.DefineSqlParameter("@AffUser", SqlDbType.Int, ParameterDirection.Input, affUser);
			sph.DefineSqlParameter("@AffBankUserName", SqlDbType.NVarChar, 50, ParameterDirection.Input, affBankUserName);
			sph.DefineSqlParameter("@AffBankNumber", SqlDbType.NVarChar, 50, ParameterDirection.Input, affBankNumber);
			sph.DefineSqlParameter("@AffBankName", SqlDbType.NVarChar, 50, ParameterDirection.Input, affBankName);
			sph.DefineSqlParameter("@AffBankBranch", SqlDbType.NVarChar, 255, ParameterDirection.Input, affBankBranch);
			sph.DefineSqlParameter("@WithdrawMoney", SqlDbType.Decimal, ParameterDirection.Input, withdrawMoney);
			sph.DefineSqlParameter("@DateRequire", SqlDbType.DateTime, ParameterDirection.Input, dateRequire);
			sph.DefineSqlParameter("@Status", SqlDbType.Bit, ParameterDirection.Input, status);
			sph.DefineSqlParameter("@DateRespond", SqlDbType.DateTime, ParameterDirection.Input, dateRespond);
			sph.DefineSqlParameter("@ExtraCol", SqlDbType.NChar, 10, ParameterDirection.Input, extraCol);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Deletes a row from the gb_AffiliatePayment table. Returns true if row deleted.
		/// </summary>
		/// <param name="affID"> affID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int affID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_AffiliatePayment_Delete", 1);
			sph.DefineSqlParameter("@AffID", SqlDbType.Int, ParameterDirection.Input, affID);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Gets an IDataReader with one row from the gb_AffiliatePayment table.
		/// </summary>
		/// <param name="affID"> affID </param>
		public static IDataReader GetOne(
			int AffUser)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_AffiliatePayment_SelectOneByUserID", 1);
			sph.DefineSqlParameter("@AffUser", SqlDbType.Int, ParameterDirection.Input, AffUser);
			return sph.ExecuteReader();

		}

		public static IDataReader GetOne(
			Guid AffGuid)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_AffiliatePayment_SelectOne", 1);
			sph.DefineSqlParameter("@AffGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, AffGuid);
			return sph.ExecuteReader();

		}

		/// <summary>
		/// Gets a count of rows in the gb_AffiliatePayment table.
		/// </summary>
		public static int GetCount()
		{

			return Convert.ToInt32(SqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_AffiliatePayment_GetCount",
				null));

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the gb_AffiliatePayment table.
		/// </summary>
		public static IDataReader GetAll()
		{

			return SqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_AffiliatePayment_SelectAll",
				null);

		}

		/// <summary>
		/// Gets a page of data from the gb_AffiliatePayment table.
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

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_AffiliatePayment_SelectPage", 2);
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

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_AffiliatePayment_SelectPageByAffUserID", 3);
			sph.DefineSqlParameter("@AffUser", SqlDbType.Int, ParameterDirection.Input, userID);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

	}

}

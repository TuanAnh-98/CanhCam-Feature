// Author:					Tran Quoc Vuong
// Created:					2022-8-1
// Last Modified:			2022-8-1

using System;
using System.Data;

namespace CanhCam.Data
{

	public static class DBAffiliateUser
	{

		/// <summary>
		/// Inserts a row in the gb_AffiliateUser table. Returns new integer id.
		/// </summary>
		/// <param name="affiliateUserID"> affiliateUserID </param>
		/// <param name="totalCommission"> totalCommission </param>
		/// <param name="commissionWait"> commissionWait </param>
		/// <param name="commissionPay"> commissionPay </param>
		/// <param name="totalOrder"> totalOrder </param>
		/// <param name="statusPayment"> statusPayment </param>
		/// <param name="extracol"> extracol </param>
		/// <param name="extracol1"> extracol1 </param>
		/// <param name="extracol2"> extracol2 </param>
		/// <returns>int</returns>
		public static int Create(
			int affiliateUserID,
			decimal totalCommission,
			decimal commissionWait,
			decimal commissionPay,
			int totalOrder,
			bool statusPayment,
			string extracol,
			string extracol1,
			string extracol2)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_AffiliateUser_Insert", 9);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
			sph.DefineSqlParameter("@TotalCommission", SqlDbType.Decimal, ParameterDirection.Input, totalCommission);
			sph.DefineSqlParameter("@CommissionWait", SqlDbType.Decimal, ParameterDirection.Input, commissionWait);
			sph.DefineSqlParameter("@CommissionPay", SqlDbType.Decimal, ParameterDirection.Input, commissionPay);
			sph.DefineSqlParameter("@TotalOrder", SqlDbType.Int, ParameterDirection.Input, totalOrder);
			sph.DefineSqlParameter("@StatusPayment", SqlDbType.Bit, ParameterDirection.Input, statusPayment);
			sph.DefineSqlParameter("@PercentCommission", SqlDbType.NChar, 10, ParameterDirection.Input, extracol);
			sph.DefineSqlParameter("@extracol1", SqlDbType.NChar, 10, ParameterDirection.Input, extracol1);
			sph.DefineSqlParameter("@extracol2", SqlDbType.NChar, 10, ParameterDirection.Input, extracol2);
			int newID = Convert.ToInt32(sph.ExecuteScalar());
			return newID;
		}


		/// <summary>
		/// Updates a row in the gb_AffiliateUser table. Returns true if row updated.
		/// </summary>
		/// <param name="affiID"> affiID </param>
		/// <param name="affiliateUserID"> affiliateUserID </param>
		/// <param name="totalCommission"> totalCommission </param>
		/// <param name="commissionWait"> commissionWait </param>
		/// <param name="commissionPay"> commissionPay </param>
		/// <param name="totalOrder"> totalOrder </param>
		/// <param name="statusPayment"> statusPayment </param>
		/// <param name="extracol"> extracol </param>
		/// <param name="extracol1"> extracol1 </param>
		/// <param name="extracol2"> extracol2 </param>
		/// <returns>bool</returns>
		public static bool Update(
			int affiID,
			int affiliateUserID,
			decimal totalCommission,
			decimal commissionWait,
			decimal commissionPay,
			int totalOrder,
			bool statusPayment,
			string extracol,
			string extracol1,
			string extracol2)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_AffiliateUser_Update", 10);
			sph.DefineSqlParameter("@AffiID", SqlDbType.Int, ParameterDirection.Input, affiID);
			sph.DefineSqlParameter("@AffiliateUserID", SqlDbType.Int, ParameterDirection.Input, affiliateUserID);
			sph.DefineSqlParameter("@TotalCommission", SqlDbType.Decimal, ParameterDirection.Input, totalCommission);
			sph.DefineSqlParameter("@CommissionWait", SqlDbType.Decimal, ParameterDirection.Input, commissionWait);
			sph.DefineSqlParameter("@CommissionPay", SqlDbType.Decimal, ParameterDirection.Input, commissionPay);
			sph.DefineSqlParameter("@TotalOrder", SqlDbType.Int, ParameterDirection.Input, totalOrder);
			sph.DefineSqlParameter("@StatusPayment", SqlDbType.Bit, ParameterDirection.Input, statusPayment);
			sph.DefineSqlParameter("@PercentCommission", SqlDbType.NChar, 10, ParameterDirection.Input, extracol);
			sph.DefineSqlParameter("@extracol1", SqlDbType.NChar, 10, ParameterDirection.Input, extracol1);
			sph.DefineSqlParameter("@extracol2", SqlDbType.NChar, 10, ParameterDirection.Input, extracol2);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Deletes a row from the gb_AffiliateUser table. Returns true if row deleted.
		/// </summary>
		/// <param name="affiID"> affiID </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int affiID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_AffiliateUser_Delete", 1);
			sph.DefineSqlParameter("@AffiID", SqlDbType.Int, ParameterDirection.Input, affiID);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Gets an IDataReader with one row from the gb_AffiliateUser table.
		/// </summary>
		/// <param name="affiID"> affiID </param>
		public static IDataReader GetOne(
			int affiID)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_AffiliateUser_SelectOne", 1);
			sph.DefineSqlParameter("@AffiID", SqlDbType.Int, ParameterDirection.Input, affiID);
			return sph.ExecuteReader();

		}

		/// <summary>
		/// Gets a count of rows in the gb_AffiliateUser table.
		/// </summary>
		public static int GetCount()
		{

			return Convert.ToInt32(SqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_AffiliateUser_GetCount",
				null));

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the gb_AffiliateUser table.
		/// </summary>
		public static IDataReader GetAll()
		{

			return SqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_AffiliateUser_SelectAll",
				null);

		}

		/// <summary>
		/// Gets a page of data from the gb_AffiliateUser table.
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

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_AffiliateUser_SelectPage", 2);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

	}

}

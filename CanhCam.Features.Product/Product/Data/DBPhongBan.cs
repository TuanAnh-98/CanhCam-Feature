// Author:					Tran Quoc Vuong
// Created:					2022-7-4
// Last Modified:			2022-7-4

using System;
using System.Data;

namespace CanhCam.Data
{

	public static class DBPhongBan
	{

		/// <summary>
		/// Inserts a row in the gb_PhongBan table. Returns new integer id.
		/// </summary>
		/// <param name="title"> title </param>
		/// <param name="code"> code </param>
		/// <returns>int</returns>
		public static int Create(
			string title,
			string code)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_PhongBan_Insert", 2);
			sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
			sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, code);
			int newID = Convert.ToInt32(sph.ExecuteScalar());
			return newID;
		}


		/// <summary>
		/// Updates a row in the gb_PhongBan table. Returns true if row updated.
		/// </summary>
		/// <param name="itemId"> itemId </param>
		/// <param name="title"> title </param>
		/// <param name="code"> code </param>
		/// <returns>bool</returns>
		public static bool Update(
			int itemId,
			string title,
			string code)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_PhongBan_Update", 3);
			sph.DefineSqlParameter("@ItemId", SqlDbType.Int, ParameterDirection.Input, itemId);
			sph.DefineSqlParameter("@Title", SqlDbType.NVarChar, 255, ParameterDirection.Input, title);
			sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, code);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Deletes a row from the gb_PhongBan table. Returns true if row deleted.
		/// </summary>
		/// <param name="itemId"> itemId </param>
		/// <returns>bool</returns>
		public static bool Delete(
			int itemId)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_PhongBan_Delete", 1);
			sph.DefineSqlParameter("@ItemId", SqlDbType.Int, ParameterDirection.Input, itemId);
			int rowsAffected = sph.ExecuteNonQuery();
			return (rowsAffected > 0);

		}

		/// <summary>
		/// Gets an IDataReader with one row from the gb_PhongBan table.
		/// </summary>
		/// <param name="itemId"> itemId </param>
		public static IDataReader GetOne(
			int itemId)
		{
			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_PhongBan_SelectOne", 1);
			sph.DefineSqlParameter("@ItemId", SqlDbType.Int, ParameterDirection.Input, itemId);
			return sph.ExecuteReader();

		}

		/// <summary>
		/// Gets a count of rows in the gb_PhongBan table.
		/// </summary>
		public static int GetCount()
		{

			return Convert.ToInt32(SqlHelper.ExecuteScalar(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_PhongBan_GetCount",
				null));

		}

		/// <summary>
		/// Gets an IDataReader with all rows in the gb_PhongBan table.
		/// </summary>
		public static IDataReader GetAll()
		{

			return SqlHelper.ExecuteReader(
				ConnectionString.GetReadConnectionString(),
				CommandType.StoredProcedure,
				"gb_PhongBan_SelectAll",
				null);

		}

		/// <summary>
		/// Gets a page of data from the gb_PhongBan table.
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

			SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_PhongBan_SelectPage", 2);
			sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
			sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
			return sph.ExecuteReader();

		}

	}

}

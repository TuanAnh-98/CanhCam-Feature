using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProductZone
    {
        /// <summary>
        /// Inserts a row in the gb_ProductZone table. Returns new integer id.
        /// </summary>
        /// <returns>int</returns>
        public static int Create(int productID, int zoneID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductZone_Insert", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        /// <summary>
        /// Updates a row in the gb_ProductZone table. Returns true if row updated.
        /// </summary>
        /// <param name="productID"> productID </param>
        /// <param name="zoneID"> zoneID </param>
        /// <returns>bool</returns>
        public static bool Update(
            int productID, int zoneID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductZone_Update", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Deletes a row from the gb_ProductZone table. Returns true if row deleted.
        /// </summary>
        /// <param name="productID"> productID </param>
        /// <param name="zoneID"> zoneID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int productID, int zoneID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductZone_Delete", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool DeleteByProduct(int productID)
        {
            var sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductZone_DeleteByProduct", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        /// <summary>
        /// Gets an IDataReader with one row from the gb_ProductZone table.
        /// </summary>
        /// <param name="productID"> productID </param>
        /// <param name="zoneID"> zoneID </param>
        public static IDataReader GetOne(
            int productID, int zoneID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductZone_SelectOne", 2);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@ZoneID", SqlDbType.Int, ParameterDirection.Input, zoneID);
            return sph.ExecuteReader();
        }

        public static IDataReader SelectAllByProductID(
            int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductZone_SelectAllByProductID", 1);
            sph.DefineSqlParameter("@ProductId", SqlDbType.Int, ParameterDirection.Input, productId);
            return sph.ExecuteReader();
        }
    }
}
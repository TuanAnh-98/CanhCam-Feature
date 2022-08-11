/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-11-17
/// Last Modified:			2015-11-17

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBProductVariant
    {
        public static int Create(
            int productID,
            string code,
            string name,
            int displayOrder,
            decimal price,
            int stockQuantity,
            int soldQuantity,
            DateTime? availableDate,
            string attributesXml,
            Guid guid,
            bool isDeleted,
            DateTime createdOn)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductVariant_Insert", 12);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            sph.DefineSqlParameter("@SoldQuantity", SqlDbType.Int, ParameterDirection.Input, soldQuantity);
            sph.DefineSqlParameter("@AvailableDate", SqlDbType.DateTime, ParameterDirection.Input, availableDate);
            sph.DefineSqlParameter("@AttributesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributesXml);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int productVariantID,
            int productID,
            string code,
            string name,
            int displayOrder,
            decimal price,
            int stockQuantity,
            int soldQuantity,
            DateTime? availableDate,
            string attributesXml,
            Guid guid,
            bool isDeleted,
            DateTime createdOn)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductVariant_Update", 13);
            sph.DefineSqlParameter("@ProductVariantID", SqlDbType.Int, ParameterDirection.Input, productVariantID);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productID);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 50, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Price", SqlDbType.Decimal, ParameterDirection.Input, price);
            sph.DefineSqlParameter("@StockQuantity", SqlDbType.Int, ParameterDirection.Input, stockQuantity);
            sph.DefineSqlParameter("@SoldQuantity", SqlDbType.Int, ParameterDirection.Input, soldQuantity);
            sph.DefineSqlParameter("@AvailableDate", SqlDbType.DateTime, ParameterDirection.Input, availableDate);
            sph.DefineSqlParameter("@AttributesXml", SqlDbType.NVarChar, -1, ParameterDirection.Input, attributesXml);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@CreatedOn", SqlDbType.DateTime, ParameterDirection.Input, createdOn);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int productVariantID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_ProductVariant_Delete", 1);
            sph.DefineSqlParameter("@ProductVariantID", SqlDbType.Int, ParameterDirection.Input, productVariantID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int productVariantID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductVariant_SelectOne", 1);
            sph.DefineSqlParameter("@ProductVariantID", SqlDbType.Int, ParameterDirection.Input, productVariantID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByProduct(int productId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_ProductVariant_SelectByProduct", 1);
            sph.DefineSqlParameter("@ProductID", SqlDbType.Int, ParameterDirection.Input, productId);
            return sph.ExecuteReader();
        }
    }
}
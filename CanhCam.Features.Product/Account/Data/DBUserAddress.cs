/// Author:					Tran Quoc Vuong
/// Created:				2017-02-28
/// Last Modified:			2017-02-28

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBUserAddress
    {
        public static int Create(
            int userID,
            string firstName,
            string lastName,
            string email,
            string address,
            string company,
            string phone,
            string mobile,
            string fax,
            string street,
            string ward,
            Guid wardGuid,
            Guid districtGuid,
            Guid provinceGuid,
            Guid countryGuid,
            string customAttributes,
            DateTime createdDate,
            bool isDefault)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserAddress_Insert", 18);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Address", SqlDbType.NVarChar, 255, ParameterDirection.Input, address);
            sph.DefineSqlParameter("@Company", SqlDbType.NVarChar, 255, ParameterDirection.Input, company);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@Mobile", SqlDbType.NVarChar, 255, ParameterDirection.Input, mobile);
            sph.DefineSqlParameter("@Fax", SqlDbType.NVarChar, 255, ParameterDirection.Input, fax);
            sph.DefineSqlParameter("@Street", SqlDbType.NVarChar, 255, ParameterDirection.Input, street);
            sph.DefineSqlParameter("@Ward", SqlDbType.NVarChar, 255, ParameterDirection.Input, ward);
            sph.DefineSqlParameter("@WardGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, wardGuid);
            sph.DefineSqlParameter("@DistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, districtGuid);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@CustomAttributes", SqlDbType.NVarChar, -1, ParameterDirection.Input, customAttributes);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@IsDefault", SqlDbType.Bit, ParameterDirection.Input, isDefault);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int addressID,
            int userID,
            string firstName,
            string lastName,
            string email,
            string address,
            string company,
            string phone,
            string mobile,
            string fax,
            string street,
            string ward,
            Guid wardGuid,
            Guid districtGuid,
            Guid provinceGuid,
            Guid countryGuid,
            string customAttributes,
            DateTime createdDate,
            bool isDefault)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserAddress_Update", 19);
            sph.DefineSqlParameter("@AddressID", SqlDbType.Int, ParameterDirection.Input, addressID);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@FirstName", SqlDbType.NVarChar, 255, ParameterDirection.Input, firstName);
            sph.DefineSqlParameter("@LastName", SqlDbType.NVarChar, 255, ParameterDirection.Input, lastName);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@Address", SqlDbType.NVarChar, 255, ParameterDirection.Input, address);
            sph.DefineSqlParameter("@Company", SqlDbType.NVarChar, 255, ParameterDirection.Input, company);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@Mobile", SqlDbType.NVarChar, 255, ParameterDirection.Input, mobile);
            sph.DefineSqlParameter("@Fax", SqlDbType.NVarChar, 255, ParameterDirection.Input, fax);
            sph.DefineSqlParameter("@Street", SqlDbType.NVarChar, 255, ParameterDirection.Input, street);
            sph.DefineSqlParameter("@Ward", SqlDbType.NVarChar, 255, ParameterDirection.Input, ward);
            sph.DefineSqlParameter("@WardGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, wardGuid);
            sph.DefineSqlParameter("@DistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, districtGuid);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@CountryGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, countryGuid);
            sph.DefineSqlParameter("@CustomAttributes", SqlDbType.NVarChar, -1, ParameterDirection.Input, customAttributes);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@IsDefault", SqlDbType.Bit, ParameterDirection.Input, isDefault);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int addressID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserAddress_Delete", 1);
            sph.DefineSqlParameter("@AddressID", SqlDbType.Int, ParameterDirection.Input, addressID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int addressID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserAddress_SelectOne", 1);
            sph.DefineSqlParameter("@AddressID", SqlDbType.Int, ParameterDirection.Input, addressID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByUser(int userId, int isDefault)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_UserAddress_SelectByUser", 2);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userId);
            sph.DefineSqlParameter("@IsDefault", SqlDbType.Int, ParameterDirection.Input, isDefault);
            return sph.ExecuteReader();
        }

        public static bool UpdateDefaultAddress(int userID, int addressID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_UserAddress_UpdateDefaultAddress", 2);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@AddressID", SqlDbType.Int, ParameterDirection.Input, addressID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }
    }
}
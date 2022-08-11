/// Author:					Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-08-18
/// Last Modified:			2015-08-18

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBPaymentMethod
    {
        public static int Create(
            int siteID,
            int paymentProvider,
            string name,
            string description,
            int displayOrder,
            bool isActive,
            decimal additionalFee,
            bool usePercentage,
            bool freeOnOrdersOverXEnabled,
            decimal freeOnOrdersOverXValue,
            bool useSandbox,
            string businessEmail,
            string securePass,
            string hashcode,
            string merchantId,
            string merchantSiteCode,
            string accessCode,
            Guid guid,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_PaymentMethod_Insert", 19);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@PaymentProvider", SqlDbType.Int, ParameterDirection.Input, paymentProvider);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@AdditionalFee", SqlDbType.Decimal, ParameterDirection.Input, additionalFee);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@FreeOnOrdersOverXEnabled", SqlDbType.Bit, ParameterDirection.Input, freeOnOrdersOverXEnabled);
            sph.DefineSqlParameter("@FreeOnOrdersOverXValue", SqlDbType.Decimal, ParameterDirection.Input, freeOnOrdersOverXValue);
            sph.DefineSqlParameter("@UseSandbox", SqlDbType.Bit, ParameterDirection.Input, useSandbox);
            sph.DefineSqlParameter("@BusinessEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, businessEmail);
            sph.DefineSqlParameter("@SecurePass", SqlDbType.NVarChar, 255, ParameterDirection.Input, securePass);
            sph.DefineSqlParameter("@Hashcode", SqlDbType.NVarChar, 255, ParameterDirection.Input, hashcode);
            sph.DefineSqlParameter("@MerchantId", SqlDbType.NVarChar, 255, ParameterDirection.Input, merchantId);
            sph.DefineSqlParameter("@MerchantSiteCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, merchantSiteCode);
            sph.DefineSqlParameter("@AccessCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, accessCode);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int paymentMethodID,
            int siteID,
            int paymentProvider,
            string name,
            string description,
            int displayOrder,
            bool isActive,
            decimal additionalFee,
            bool usePercentage,
            bool freeOnOrdersOverXEnabled,
            decimal freeOnOrdersOverXValue,
            bool useSandbox,
            string businessEmail,
            string securePass,
            string hashcode,
            string merchantId,
            string merchantSiteCode,
            string accessCode,
            Guid guid,
            bool isDeleted)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_PaymentMethod_Update", 20);
            sph.DefineSqlParameter("@PaymentMethodID", SqlDbType.Int, ParameterDirection.Input, paymentMethodID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@PaymentProvider", SqlDbType.Int, ParameterDirection.Input, paymentProvider);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@IsActive", SqlDbType.Bit, ParameterDirection.Input, isActive);
            sph.DefineSqlParameter("@AdditionalFee", SqlDbType.Decimal, ParameterDirection.Input, additionalFee);
            sph.DefineSqlParameter("@UsePercentage", SqlDbType.Bit, ParameterDirection.Input, usePercentage);
            sph.DefineSqlParameter("@FreeOnOrdersOverXEnabled", SqlDbType.Bit, ParameterDirection.Input, freeOnOrdersOverXEnabled);
            sph.DefineSqlParameter("@FreeOnOrdersOverXValue", SqlDbType.Decimal, ParameterDirection.Input, freeOnOrdersOverXValue);
            sph.DefineSqlParameter("@UseSandbox", SqlDbType.Bit, ParameterDirection.Input, useSandbox);
            sph.DefineSqlParameter("@BusinessEmail", SqlDbType.NVarChar, 255, ParameterDirection.Input, businessEmail);
            sph.DefineSqlParameter("@SecurePass", SqlDbType.NVarChar, 255, ParameterDirection.Input, securePass);
            sph.DefineSqlParameter("@Hashcode", SqlDbType.NVarChar, 255, ParameterDirection.Input, hashcode);
            sph.DefineSqlParameter("@MerchantId", SqlDbType.NVarChar, 255, ParameterDirection.Input, merchantId);
            sph.DefineSqlParameter("@MerchantSiteCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, merchantSiteCode);
            sph.DefineSqlParameter("@AccessCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, accessCode);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(int paymentMethodId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_PaymentMethod_Delete", 1);
            sph.DefineSqlParameter("@PaymentMethodID", SqlDbType.Int, ParameterDirection.Input, paymentMethodId);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(int paymentMethodId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_PaymentMethod_SelectOne", 1);
            sph.DefineSqlParameter("@PaymentMethodID", SqlDbType.Int, ParameterDirection.Input, paymentMethodId);
            return sph.ExecuteReader();
        }

        public static IDataReader GetByActive(int siteId, int activeStatus, int languageId)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_PaymentMethod_SelectByActive", 3);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@ActiveStatus", SqlDbType.Int, ParameterDirection.Input, activeStatus);
            sph.DefineSqlParameter("@LanguageID", SqlDbType.Int, ParameterDirection.Input, languageId);

            return sph.ExecuteReader();
        }
    }
}
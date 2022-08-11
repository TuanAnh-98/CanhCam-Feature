// Author:					t
// Created:					2020-5-4
// Last Modified:			2020-5-4

using System;
using System.Data;

namespace CanhCam.Data
{
    public static class DBStore
    {
        public static int Create(
            int siteID,
            string name,
            string code,
            int displayOrder,
            int options,
            string address,
            string phone,
            string fax,
            string email,
            string contactPerson,
            string map,
            string description,
            bool isPublished,
            string latitude,
            string longitude,
            string provinceGuids,
            string districtGuids,
            int priority,
            Guid guid,
            string aPIParams,
            string orderUserIDs,
            DateTime createdDate,
            bool isDeleted,
            string aPIDistrictCode,
            Guid provinceGuid,
            Guid districtGuid,
            string aPIEndpoint)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Store_Insert", 27);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@Address", SqlDbType.NVarChar, 255, ParameterDirection.Input, address);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@Fax", SqlDbType.NVarChar, 255, ParameterDirection.Input, fax);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@ContactPerson", SqlDbType.NVarChar, 255, ParameterDirection.Input, contactPerson);
            sph.DefineSqlParameter("@Map", SqlDbType.NVarChar, -1, ParameterDirection.Input, map);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@Latitude", SqlDbType.NVarChar, 50, ParameterDirection.Input, latitude);
            sph.DefineSqlParameter("@Longitude", SqlDbType.NVarChar, 50, ParameterDirection.Input, longitude);
            sph.DefineSqlParameter("@ProvinceGuids", SqlDbType.NVarChar, -1, ParameterDirection.Input, provinceGuids);
            sph.DefineSqlParameter("@DistrictGuids", SqlDbType.NVarChar, -1, ParameterDirection.Input, districtGuids);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@APIParams", SqlDbType.NVarChar, -1, ParameterDirection.Input, aPIParams);
            sph.DefineSqlParameter("@OrderUserIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, orderUserIDs);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@APIDistrictCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, aPIDistrictCode);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@DistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, districtGuid);
            sph.DefineSqlParameter("@APIEndpoint", SqlDbType.NVarChar, -1, ParameterDirection.Input, aPIEndpoint);
            int newID = Convert.ToInt32(sph.ExecuteScalar());
            return newID;
        }

        public static bool Update(
            int storeID,
            int siteID,
            string name,
            string code,
            int displayOrder,
            int options,
            string address,
            string phone,
            string fax,
            string email,
            string contactPerson,
            string map,
            string description,
            bool isPublished,
            string latitude,
            string longitude,
            string provinceGuids,
            string districtGuids,
            int priority,
            Guid guid,
            string aPIParams,
            string orderUserIDs,
            DateTime createdDate,
            bool isDeleted,
            string aPIDistrictCode,
            Guid provinceGuid,
            Guid districtGuid,
            string aPIEndpoint)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Store_Update", 28);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteID);
            sph.DefineSqlParameter("@Name", SqlDbType.NVarChar, 255, ParameterDirection.Input, name);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 50, ParameterDirection.Input, code);
            sph.DefineSqlParameter("@DisplayOrder", SqlDbType.Int, ParameterDirection.Input, displayOrder);
            sph.DefineSqlParameter("@Options", SqlDbType.Int, ParameterDirection.Input, options);
            sph.DefineSqlParameter("@Address", SqlDbType.NVarChar, 255, ParameterDirection.Input, address);
            sph.DefineSqlParameter("@Phone", SqlDbType.NVarChar, 255, ParameterDirection.Input, phone);
            sph.DefineSqlParameter("@Fax", SqlDbType.NVarChar, 255, ParameterDirection.Input, fax);
            sph.DefineSqlParameter("@Email", SqlDbType.NVarChar, 255, ParameterDirection.Input, email);
            sph.DefineSqlParameter("@ContactPerson", SqlDbType.NVarChar, 255, ParameterDirection.Input, contactPerson);
            sph.DefineSqlParameter("@Map", SqlDbType.NVarChar, -1, ParameterDirection.Input, map);
            sph.DefineSqlParameter("@Description", SqlDbType.NVarChar, -1, ParameterDirection.Input, description);
            sph.DefineSqlParameter("@IsPublished", SqlDbType.Bit, ParameterDirection.Input, isPublished);
            sph.DefineSqlParameter("@Latitude", SqlDbType.NVarChar, 50, ParameterDirection.Input, latitude);
            sph.DefineSqlParameter("@Longitude", SqlDbType.NVarChar, 50, ParameterDirection.Input, longitude);
            sph.DefineSqlParameter("@ProvinceGuids", SqlDbType.NVarChar, -1, ParameterDirection.Input, provinceGuids);
            sph.DefineSqlParameter("@DistrictGuids", SqlDbType.NVarChar, -1, ParameterDirection.Input, districtGuids);
            sph.DefineSqlParameter("@Priority", SqlDbType.Int, ParameterDirection.Input, priority);
            sph.DefineSqlParameter("@Guid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, guid);
            sph.DefineSqlParameter("@APIParams", SqlDbType.NVarChar, -1, ParameterDirection.Input, aPIParams);
            sph.DefineSqlParameter("@OrderUserIDs", SqlDbType.NVarChar, -1, ParameterDirection.Input, orderUserIDs);
            sph.DefineSqlParameter("@CreatedDate", SqlDbType.DateTime, ParameterDirection.Input, createdDate);
            sph.DefineSqlParameter("@IsDeleted", SqlDbType.Bit, ParameterDirection.Input, isDeleted);
            sph.DefineSqlParameter("@APIDistrictCode", SqlDbType.NVarChar, 255, ParameterDirection.Input, aPIDistrictCode);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@DistrictGuid", SqlDbType.UniqueIdentifier, ParameterDirection.Input, districtGuid);
            sph.DefineSqlParameter("@APIEndpoint", SqlDbType.NVarChar, -1, ParameterDirection.Input, aPIEndpoint);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static bool Delete(
            int storeID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetWriteConnectionString(), "gb_Store_Delete", 1);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            int rowsAffected = sph.ExecuteNonQuery();
            return (rowsAffected > 0);
        }

        public static IDataReader GetOne(
            int storeID)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_SelectOne", 1);
            sph.DefineSqlParameter("@StoreID", SqlDbType.Int, ParameterDirection.Input, storeID);
            return sph.ExecuteReader();
        }

        public static IDataReader GetOne(
            string storeCode)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_SelectOneByCode", 1);
            sph.DefineSqlParameter("@Code", SqlDbType.NVarChar, 255, ParameterDirection.Input, storeCode);
            return sph.ExecuteReader();
        }

        public static int GetCount()
        {
            return Convert.ToInt32(SqlHelper.ExecuteScalar(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_Store_GetCount",
                null));
        }

        public static IDataReader GetAll()
        {
            return SqlHelper.ExecuteReader(
                ConnectionString.GetReadConnectionString(),
                CommandType.StoredProcedure,
                "gb_Store_SelectAll",
                null);
        }

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
                Math.DivRem(totalRows, pageSize, out int remainder);
                if (remainder > 0)
                {
                    totalPages += 1;
                }
            }

            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_SelectPage", 2);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            return sph.ExecuteReader();
        }

        public static int GetCountBySearch(
            int userID,
            bool isAdmin,
            int siteId,
            string keyword,
            Guid? provinceGuid,
            Guid? districtGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_GetCountBySearch", 6);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, 255, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@DistrictGuid", SqlDbType.UniqueIdentifier, 255, ParameterDirection.Input, districtGuid);
            return Convert.ToInt32(sph.ExecuteScalar());
        }

        public static IDataReader GetPageBySearch(
            int userID,
            bool isAdmin,
            int siteId,
            string keyword,
            int pageNumber,
            int pageSize,
            Guid? provinceGuid,
            Guid? districtGuid)
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_SelectPageBySearch", 8);
            sph.DefineSqlParameter("@UserID", SqlDbType.Int, ParameterDirection.Input, userID);
            sph.DefineSqlParameter("@IsAdmin", SqlDbType.Bit, ParameterDirection.Input, isAdmin);
            sph.DefineSqlParameter("@SiteID", SqlDbType.Int, ParameterDirection.Input, siteId);
            sph.DefineSqlParameter("@Keyword", SqlDbType.NVarChar, 255, ParameterDirection.Input, keyword);
            sph.DefineSqlParameter("@PageNumber", SqlDbType.Int, ParameterDirection.Input, pageNumber);
            sph.DefineSqlParameter("@PageSize", SqlDbType.Int, ParameterDirection.Input, pageSize);
            sph.DefineSqlParameter("@ProvinceGuid", SqlDbType.UniqueIdentifier, 255, ParameterDirection.Input, provinceGuid);
            sph.DefineSqlParameter("@DistrictGuid", SqlDbType.UniqueIdentifier, 255, ParameterDirection.Input, districtGuid);
            return sph.ExecuteReader();
        }

        public static IDataReader GetAllStoreIDs(
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_GetAllStoreIDs", 0);
            return sph.ExecuteReader();
        }

        public static IDataReader GetDefaultStore(
            )
        {
            SqlParameterHelper sph = new SqlParameterHelper(ConnectionString.GetReadConnectionString(), "gb_Store_GetDefaultStore", 0);
            return sph.ExecuteReader();
        }
    }
}
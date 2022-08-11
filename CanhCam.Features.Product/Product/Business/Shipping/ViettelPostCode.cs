
// Author:					 Vu Minh Tri
// Created:					2018-9-7
// Last Modified:			2018-9-7

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

    public class ViettelPostCode
    {

        #region Constructors

        public ViettelPostCode()
        { }


        public ViettelPostCode(
            int rowID)
        {
            this.GetViettelPostCode(
                rowID);
        }


        public ViettelPostCode(
            string geozoneCode)
        {
            this.GetViettelPostCode(
                geozoneCode);
        }
        #endregion

        #region Private Properties

        private int rowID = -1;
        private string geoZoneName = string.Empty;
        private string geoZoneCode = string.Empty;
        private string viettelPostCode = string.Empty;

        #endregion

        #region Public Properties

        public int RowID
        {
            get { return rowID; }
            set { rowID = value; }
        }
        public string GeoZoneName
        {
            get { return geoZoneName; }
            set { geoZoneName = value; }
        }
        public string GeoZoneCode
        {
            get { return geoZoneCode; }
            set { geoZoneCode = value; }
        }
        public string ViettelPostCode2
        {
            get { return viettelPostCode; }
            set { viettelPostCode = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ViettelPostCode.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        private void GetViettelPostCode(
            int rowID)
        {
      
            PopulateViettelPostCode(this, DBViettelPostCode.GetOne(rowID));
        }
        private void GetViettelPostCode(
        string geozoneCode)
        {

            PopulateViettelPostCode(this, DBViettelPostCode.GetOne(geozoneCode));
        }
        public static void PopulateViettelPostCode(ViettelPostCode viettelPostCode, IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    viettelPostCode.rowID = Convert.ToInt32(reader["RowID"]);
                    viettelPostCode.geoZoneCode = reader["GeoZoneCode"].ToString();
                    viettelPostCode.viettelPostCode = reader["ViettelPostCode"].ToString();
                }
            }
            finally
            {
                reader.Close();
            }
        }

        /// <summary>
        /// Persists a new instance of ViettelPostCode. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBViettelPostCode.Create(
                this.geoZoneCode,
                this.viettelPostCode);

            this.rowID = newID;

            return (newID > 0);

        }


        /// <summary>
        /// Updates this instance of ViettelPostCode. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBViettelPostCode.Update(
                this.rowID,
                this.geoZoneCode,
                this.viettelPostCode);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ViettelPostCode. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        public bool Save()
        {
            if (this.rowID > 0)
            {
                return this.Update();
            }
            else
            {
                return this.Create();
            }
        }




        #endregion

        #region Static Methods

        /// <summary>
        /// Deletes an instance of ViettelPostCode. Returns true on success.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowID)
        {
            return DBViettelPostCode.Delete(
                rowID);
        }


        /// <summary>
        /// Gets a count of ViettelPostCode. 
        /// </summary>
        public static int GetCount()
        {
            return DBViettelPostCode.GetCount();
        }

        private static List<ViettelPostCode> LoadListFromReader(IDataReader reader, bool loadName = false)
        {
            List<ViettelPostCode> viettelPostCodeList = new List<ViettelPostCode>();
            try
            {
                while (reader.Read())
                {
                    ViettelPostCode viettelPostCode = new ViettelPostCode
                    {
                        rowID = Convert.ToInt32(reader["RowID"]),
                        geoZoneCode = reader["GeoZoneCode"].ToString(),
                        viettelPostCode = reader["ViettelPostCode"].ToString()
                    };
                    if (loadName)
                        viettelPostCode.geoZoneName = reader["Name"].ToString();
                    viettelPostCodeList.Add(viettelPostCode);

                }
            }
            finally
            {
                reader.Close();
            }

            return viettelPostCodeList;

        }

        /// <summary>
        /// Gets an IList with all instances of ViettelPostCode.
        /// </summary>
        public static List<ViettelPostCode> GetAll()
        {
            IDataReader reader = DBViettelPostCode.GetAll();
            return LoadListFromReader(reader);

        }
        public static List<ViettelPostCode> GetAllByProvince(Guid provinceGuid)
        {
            IDataReader reader = DBViettelPostCode.GetAllByProvince(provinceGuid);
            return LoadListFromReader(reader, true);
        }
        public static List<ViettelPostCode> GetAllProvince(Guid countryGuid)
        {
            IDataReader reader = DBViettelPostCode.GetAllProvinceByCountry(countryGuid);
            return LoadListFromReader(reader, true);
        }
        /// <summary>
        /// Gets an IList with page of instances of ViettelPostCode.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<ViettelPostCode> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBViettelPostCode.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }



        #endregion


    }

}

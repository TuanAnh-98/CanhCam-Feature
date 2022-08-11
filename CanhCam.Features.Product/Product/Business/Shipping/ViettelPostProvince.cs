
// Author:					 Vu Minh Tri
// Created:					2018-9-9
// Last Modified:			2018-9-9

using System;
using System.Collections.Generic;
using System.Data;
using CanhCam.Data;

namespace CanhCam.Business
{

    public class ViettelPostProvince
    {

        #region Constructors

        public ViettelPostProvince()
        { }


        public ViettelPostProvince(
            int rowID)
        {
            this.GetViettelPostProvince(
                rowID);
        }


        public ViettelPostProvince(
            Guid provinceGuid)
        {
            this.GetViettelPostProvince(
                provinceGuid);
        }

        #endregion

        #region Private Properties

        private int rowID = -1;
        private Guid provinceGuid = Guid.Empty;
        private string viettelPostProvinceCode = string.Empty;

        #endregion

        #region Public Properties

        public int RowID
        {
            get { return rowID; }
            set { rowID = value; }
        }
        public Guid ProvinceGuid
        {
            get { return provinceGuid; }
            set { provinceGuid = value; }
        }
        public string ViettelPostProvinceCode
        {
            get { return viettelPostProvinceCode; }
            set { viettelPostProvinceCode = value; }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Gets an instance of ViettelPostProvince.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        private void GetViettelPostProvince(
            int rowID)
        {
            PopulateViettelPostProvince(this, DBViettelPostProvince.GetOne(rowID));
        }
        private void GetViettelPostProvince(
            Guid provinceGuid)
        {
            PopulateViettelPostProvince(this, DBViettelPostProvince.GetOne(provinceGuid));
        }
        public static void PopulateViettelPostProvince(ViettelPostProvince viettelPostProvince, IDataReader reader)
        {
            try
            {
                if (reader.Read())
                {
                    viettelPostProvince.rowID = Convert.ToInt32(reader["RowID"]);
                    viettelPostProvince.provinceGuid = new Guid(reader["ProvinceGuid"].ToString());
                    viettelPostProvince.viettelPostProvinceCode = reader["ViettelPostProvinceCode"].ToString();
                }
            }
            finally
            {
                reader.Close();
            }
        }
        /// <summary>
        /// Persists a new instance of ViettelPostProvince. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBViettelPostProvince.Create(
                this.provinceGuid,
                this.viettelPostProvinceCode);

            this.rowID = newID;

            return (newID > 0);

        }


        /// <summary>
        /// Updates this instance of ViettelPostProvince. Returns true on success.
        /// </summary>
        /// <returns>bool</returns>
        private bool Update()
        {

            return DBViettelPostProvince.Update(
                this.rowID,
                this.provinceGuid,
                this.viettelPostProvinceCode);

        }





        #endregion

        #region Public Methods

        /// <summary>
        /// Saves this instance of ViettelPostProvince. Returns true on success.
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
        /// Deletes an instance of ViettelPostProvince. Returns true on success.
        /// </summary>
        /// <param name="rowID"> rowID </param>
        /// <returns>bool</returns>
        public static bool Delete(
            int rowID)
        {
            return DBViettelPostProvince.Delete(
                rowID);
        }


        /// <summary>
        /// Gets a count of ViettelPostProvince. 
        /// </summary>
        public static int GetCount()
        {
            return DBViettelPostProvince.GetCount();
        }

        private static List<ViettelPostProvince> LoadListFromReader(IDataReader reader)
        {
            List<ViettelPostProvince> viettelPostProvinceList = new List<ViettelPostProvince>();
            try
            {
                while (reader.Read())
                {
                    ViettelPostProvince viettelPostProvince = new ViettelPostProvince
                    {
                        rowID = Convert.ToInt32(reader["RowID"]),
                        provinceGuid = new Guid(reader["ProvinceGuid"].ToString()),
                        viettelPostProvinceCode = reader["ViettelPostProvinceCode"].ToString()
                    };
                    viettelPostProvinceList.Add(viettelPostProvince);

                }
            }
            finally
            {
                reader.Close();
            }

            return viettelPostProvinceList;

        }

        /// <summary>
        /// Gets an IList with all instances of ViettelPostProvince.
        /// </summary>
        public static List<ViettelPostProvince> GetAll()
        {
            IDataReader reader = DBViettelPostProvince.GetAll();
            return LoadListFromReader(reader);

        }

        /// <summary>
        /// Gets an IList with page of instances of ViettelPostProvince.
        /// </summary>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="totalPages">total pages</param>
        public static List<ViettelPostProvince> GetPage(int pageNumber, int pageSize, out int totalPages)
        {
            totalPages = 1;
            IDataReader reader = DBViettelPostProvince.GetPage(pageNumber, pageSize, out totalPages);
            return LoadListFromReader(reader);
        }



        #endregion


    }

}

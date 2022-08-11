using CanhCam.Data;
using System;
using System.Collections.Generic;
using System.Data;

namespace CanhCam.Business
{
    public class UserAddress
    {
        #region Constructors

        public UserAddress()
        { }

        public UserAddress(int addressID)
        {
            this.GetUserAddres(addressID);
        }

        #endregion Constructors

        #region Private Properties

        private int addressID = -1;
        private int userID = -1;
        private string firstName = string.Empty;
        private string lastName = string.Empty;
        private string email = string.Empty;
        private string address = string.Empty;
        private string company = string.Empty;
        private string phone = string.Empty;
        private string mobile = string.Empty;
        private string fax = string.Empty;
        private string street = string.Empty;
        private string ward = string.Empty;
        private Guid wardGuid = Guid.Empty;
        private Guid districtGuid = Guid.Empty;
        private Guid provinceGuid = Guid.Empty;
        private Guid countryGuid = Guid.Empty;
        private string customAttributes = string.Empty;
        private DateTime createdDate = DateTime.UtcNow;
        private bool isDefault = false;

        #endregion Private Properties

        #region Public Properties

        public int AddressId
        {
            get { return addressID; }
            set { addressID = value; }
        }

        public int UserId
        {
            get { return userID; }
            set { userID = value; }
        }

        public string FirstName
        {
            get { return firstName; }
            set { firstName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public string Email
        {
            get { return email; }
            set { email = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Company
        {
            get { return company; }
            set { company = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public string Mobile
        {
            get { return mobile; }
            set { mobile = value; }
        }

        public string Fax
        {
            get { return fax; }
            set { fax = value; }
        }

        public string Street
        {
            get { return street; }
            set { street = value; }
        }

        public string Ward
        {
            get { return ward; }
            set { ward = value; }
        }

        public Guid WardGuid
        {
            get { return wardGuid; }
            set { wardGuid = value; }
        }

        public Guid DistrictGuid
        {
            get { return districtGuid; }
            set { districtGuid = value; }
        }

        public Guid ProvinceGuid
        {
            get { return provinceGuid; }
            set { provinceGuid = value; }
        }

        public Guid CountryGuid
        {
            get { return countryGuid; }
            set { countryGuid = value; }
        }

        public string CustomAttributes
        {
            get { return customAttributes; }
            set { customAttributes = value; }
        }

        public DateTime CreatedDate
        {
            get { return createdDate; }
            set { createdDate = value; }
        }

        public bool IsDefault
        {
            get { return isDefault; }
            set { isDefault = value; }
        }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        /// Gets an instance of UserAddress.
        /// </summary>
        /// <param name="addressID"> addressID </param>
        private void GetUserAddres(
            int addressID)
        {
            using (IDataReader reader = DBUserAddress.GetOne(
                addressID))
            {
                if (reader.Read())
                {
                    this.addressID = Convert.ToInt32(reader["AddressID"]);
                    this.userID = Convert.ToInt32(reader["UserID"]);
                    this.firstName = reader["FirstName"].ToString();
                    this.lastName = reader["LastName"].ToString();
                    this.email = reader["Email"].ToString();
                    this.address = reader["Address"].ToString();
                    this.company = reader["Company"].ToString();
                    this.phone = reader["Phone"].ToString();
                    this.mobile = reader["Mobile"].ToString();
                    this.fax = reader["Fax"].ToString();
                    this.street = reader["Street"].ToString();
                    this.ward = reader["Ward"].ToString();
                    this.wardGuid = new Guid(reader["WardGuid"].ToString());
                    this.districtGuid = new Guid(reader["DistrictGuid"].ToString());
                    this.provinceGuid = new Guid(reader["ProvinceGuid"].ToString());
                    this.countryGuid = new Guid(reader["CountryGuid"].ToString());
                    this.customAttributes = reader["CustomAttributes"].ToString();
                    this.createdDate = Convert.ToDateTime(reader["CreatedDate"]);
                    this.isDefault = Convert.ToBoolean(reader["IsDefault"]);
                }
            }
        }

        /// <summary>
        /// Persists a new instance of UserAddress. Returns true on success.
        /// </summary>
        /// <returns></returns>
        private bool Create()
        {
            int newID = 0;

            newID = DBUserAddress.Create(
                this.userID,
                this.firstName,
                this.lastName,
                this.email,
                this.address,
                this.company,
                this.phone,
                this.mobile,
                this.fax,
                this.street,
                this.ward,
                this.wardGuid,
                this.districtGuid,
                this.provinceGuid,
                this.countryGuid,
                this.customAttributes,
                this.createdDate,
                this.isDefault);

            this.addressID = newID;

            return (newID > 0);
        }

        private bool Update()
        {
            return DBUserAddress.Update(
                this.addressID,
                this.userID,
                this.firstName,
                this.lastName,
                this.email,
                this.address,
                this.company,
                this.phone,
                this.mobile,
                this.fax,
                this.street,
                this.ward,
                this.wardGuid,
                this.districtGuid,
                this.provinceGuid,
                this.countryGuid,
                this.customAttributes,
                this.createdDate,
                this.isDefault);
        }

        #endregion Private Methods

        #region Public Methods

        public bool Save()
        {
            if (this.addressID > 0)
                return this.Update();

            return this.Create();
        }

        #endregion Public Methods

        #region Static Methods

        public static bool Delete(int addressID)
        {
            return DBUserAddress.Delete(addressID);
        }

        private static List<UserAddress> LoadListFromReader(IDataReader reader)
        {
            List<UserAddress> userAddresList = new List<UserAddress>();
            try
            {
                while (reader.Read())
                {
                    UserAddress userAddres = new UserAddress
                    {
                        addressID = Convert.ToInt32(reader["AddressID"]),
                        userID = Convert.ToInt32(reader["UserID"]),
                        firstName = reader["FirstName"].ToString(),
                        lastName = reader["LastName"].ToString(),
                        email = reader["Email"].ToString(),
                        address = reader["Address"].ToString(),
                        company = reader["Company"].ToString(),
                        phone = reader["Phone"].ToString(),
                        mobile = reader["Mobile"].ToString(),
                        fax = reader["Fax"].ToString(),
                        street = reader["Street"].ToString(),
                        ward = reader["Ward"].ToString(),
                        wardGuid = new Guid(reader["WardGuid"].ToString()),
                        districtGuid = new Guid(reader["DistrictGuid"].ToString()),
                        provinceGuid = new Guid(reader["ProvinceGuid"].ToString()),
                        countryGuid = new Guid(reader["CountryGuid"].ToString()),
                        customAttributes = reader["CustomAttributes"].ToString(),
                        createdDate = Convert.ToDateTime(reader["CreatedDate"]),
                        isDefault = Convert.ToBoolean(reader["IsDefault"])
                    };
                    userAddresList.Add(userAddres);
                }
            }
            finally
            {
                reader.Close();
            }

            return userAddresList;
        }

        public static List<UserAddress> GetByUser(int userId, int isDefault = -1)
        {
            return LoadListFromReader(DBUserAddress.GetByUser(userId, isDefault));
        }

        public static bool UpdateDefaultAddress(int userID, int addressID)
        {
            return DBUserAddress.UpdateDefaultAddress(userID, addressID);
        }

        #endregion Static Methods
    }
}
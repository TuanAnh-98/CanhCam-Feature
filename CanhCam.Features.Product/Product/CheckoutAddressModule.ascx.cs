/// Author:			        Tran Quoc Vuong - itqvuong@gmail.com - tqvuong263@yahoo.com
/// Created:				2015-07-24
/// Last Modified:			2015-07-24

using CanhCam.Business;
using CanhCam.Web.AccountUI;
using CanhCam.Web.Framework;
using Resources;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace CanhCam.Web.ProductUI
{
    // Feature Guid: ae721338-5b7b-4852-abbc-2bdef66c2fcc
    public partial class CheckoutAddressModule : SiteModuleControl
    {
        private CheckoutAddressConfiguration config = null;

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
            this.EnableViewState = false;
        }

        protected virtual void Page_Load(object sender, EventArgs e)
        {
            if (CartHelper.GetShoppingCart(SiteId, ShoppingCartTypeEnum.ShoppingCart).Count == 0)
            {
                CartHelper.SetupRedirectToCartPage(this);
                return;
            }

            LoadSettings();
            PopulateControls();
        }

        private void PopulateControls()
        {
            var doc = new XmlDocument
            {
                XmlResolver = null
            };
            doc.LoadXml("<CheckoutAddress></CheckoutAddress>");
            XmlElement root = doc.DocumentElement;

            XmlHelper.AddNode(doc, root, "ModuleTitle", this.Title);
            XmlHelper.AddNode(doc, root, "ZoneTitle", CurrentZone.Name);
            if (ModuleConfiguration.ResourceFileDef.Length > 0 && ModuleConfiguration.ResourceKeyDef.Length > 0)
            {
                List<string> lstResourceKeys = ModuleConfiguration.ResourceKeyDef.SplitOnCharAndTrim(';');

                foreach (string item in lstResourceKeys)
                {
                    XmlHelper.AddNode(doc, root, item, ResourceHelper.GetResourceString(ModuleConfiguration.ResourceFileDef, item));
                }
            }

            XmlHelper.AddNode(doc, root, "FullNameText", ProductResources.CheckoutAddressFullName);
            XmlHelper.AddNode(doc, root, "FirstNameText", ProductResources.CheckoutAddressFirstName);
            XmlHelper.AddNode(doc, root, "LastNameText", ProductResources.CheckoutAddressLastName);
            XmlHelper.AddNode(doc, root, "EmailText", ProductResources.CheckoutAddressEmail);
            XmlHelper.AddNode(doc, root, "AddressText", ProductResources.CheckoutAddress);
            XmlHelper.AddNode(doc, root, "PhoneText", ProductResources.CheckoutAddressPhone);
            XmlHelper.AddNode(doc, root, "MobileText", ProductResources.CheckoutAddressMobile);
            XmlHelper.AddNode(doc, root, "FaxText", ProductResources.CheckoutAddressFax);
            XmlHelper.AddNode(doc, root, "StreetText", ProductResources.CheckoutAddressStreet);
            XmlHelper.AddNode(doc, root, "WardText", ProductResources.CheckoutAddressWard);
            XmlHelper.AddNode(doc, root, "DistrictText", ProductResources.CheckoutAddressDistrict);
            XmlHelper.AddNode(doc, root, "ProvinceText", ProductResources.CheckoutAddressProvince);
            XmlHelper.AddNode(doc, root, "CountryText", ProductResources.CheckoutAddressCountry);
            XmlHelper.AddNode(doc, root, "ContinueText", ProductResources.CheckoutContinue);
            XmlHelper.AddNode(doc, root, "SelectProvinceText", ProductResources.CheckoutSelectProvince);
            XmlHelper.AddNode(doc, root, "SelectDistrictText", ProductResources.CheckoutSelectDistrict);
            XmlHelper.AddNode(doc, root, "ContinueShoppingText", Resources.ProductResources.CartContinueShoppingLabel);
            XmlHelper.AddNode(doc, root, "ContinueShoppingUrl", CartHelper.GetContinueShopping());
            XmlHelper.AddNode(doc, root, "CompanyNameText", ProductResources.CheckoutCompanyName);
            XmlHelper.AddNode(doc, root, "CompanyTaxCodeText", ProductResources.CheckoutCompanyTaxCode);
            XmlHelper.AddNode(doc, root, "CompanyAddressText", ProductResources.CheckoutCompanyAddress);
            XmlHelper.AddNode(doc, root, "OrderNoteText", ProductResources.CheckoutOrderNote);
            XmlHelper.AddNode(doc, root, "CartUrl", CartHelper.GetCartUrl()); 
            if (config.CheckoutNextZoneId > 0)
                XmlHelper.AddNode(doc, root, "NextPageUrl", CartHelper.GetZoneUrl(config.CheckoutNextZoneId));

            var order = CartHelper.GetOrderSession(siteSettings.SiteId);
            var provinceGuid = Guid.Empty;
            var districtGuid = Guid.Empty;
            var shippingProvinceGuid = Guid.Empty;
            var shippingDistrictGuid = Guid.Empty;
            if (order != null)
            {
                // Billing address
                XmlHelper.AddNode(doc, root, "FirstName", order.BillingFirstName);
                XmlHelper.AddNode(doc, root, "LastName", order.BillingLastName);
                XmlHelper.AddNode(doc, root, "Email", order.BillingEmail);
                XmlHelper.AddNode(doc, root, "Address", order.BillingAddress);
                XmlHelper.AddNode(doc, root, "Phone", order.BillingPhone);
                XmlHelper.AddNode(doc, root, "Mobile", order.BillingMobile);
                XmlHelper.AddNode(doc, root, "Fax", order.BillingFax);
                XmlHelper.AddNode(doc, root, "Street", order.BillingStreet);
                XmlHelper.AddNode(doc, root, "Ward", order.BillingWard);
                XmlHelper.AddNode(doc, root, "DistrictGuid", order.BillingDistrictGuid.ToString());

                if (order.BillingProvinceGuid != Guid.Empty)
                {
                    provinceGuid = order.BillingProvinceGuid;
                    XmlHelper.AddNode(doc, root, "ProvinceGuid", order.BillingProvinceGuid.ToString());
                    GeoZone geoZone = new GeoZone(provinceGuid);
                    if (geoZone != null && geoZone.Guid != Guid.Empty)
                        XmlHelper.AddNode(doc, root, "Province", geoZone.Name);
                }
                if (order.BillingDistrictGuid != Guid.Empty)
                {
                    districtGuid = order.BillingDistrictGuid;
                    XmlHelper.AddNode(doc, root, "DistrictGuid", order.BillingDistrictGuid.ToString());
                    GeoZone geoZone = new GeoZone(districtGuid);
                    if (geoZone != null && geoZone.Guid != Guid.Empty)
                        XmlHelper.AddNode(doc, root, "District", geoZone.Name);
                }

                // Shipping address
                XmlHelper.AddNode(doc, root, "ShippingFirstName", order.ShippingFirstName);
                XmlHelper.AddNode(doc, root, "ShippingLastName", order.ShippingLastName);
                XmlHelper.AddNode(doc, root, "ShippingEmail", order.ShippingEmail);
                XmlHelper.AddNode(doc, root, "ShippingAddress", order.ShippingAddress);
                XmlHelper.AddNode(doc, root, "ShippingPhone", order.ShippingPhone);
                XmlHelper.AddNode(doc, root, "ShippingMobile", order.ShippingMobile);
                XmlHelper.AddNode(doc, root, "ShippingOption", order.ShippingOption);
                XmlHelper.AddNode(doc, root, "ShippingDistrictGuid", order.ShippingDistrictGuid.ToString());
                XmlHelper.AddNode(doc, root, "OrderNote", order.OrderNote);

                if (order.ShippingProvinceGuid != Guid.Empty)
                {
                    shippingProvinceGuid = order.ShippingProvinceGuid;
                    XmlHelper.AddNode(doc, root, "ShippingProvinceGuid", order.ShippingProvinceGuid.ToString());
                    var geoZone = new GeoZone(shippingProvinceGuid);
                    if (geoZone != null && geoZone.Guid != Guid.Empty)
                        XmlHelper.AddNode(doc, root, "ShippingProvince", geoZone.Name);
                }
                if (order.ShippingDistrictGuid != Guid.Empty)
                {
                    shippingDistrictGuid = order.ShippingDistrictGuid;
                    XmlHelper.AddNode(doc, root, "ShippingDistrictGuid", order.ShippingDistrictGuid.ToString());
                    var geoZone = new GeoZone(shippingDistrictGuid);
                    if (geoZone != null && geoZone.Guid != Guid.Empty)
                        XmlHelper.AddNode(doc, root, "ShippingDistrict", geoZone.Name);
                }

                XmlHelper.AddNode(doc, root, "CompanyName", order.InvoiceCompanyName.Trim());
                XmlHelper.AddNode(doc, root, "CompanyTaxCode", order.InvoiceCompanyTaxCode.Trim());
                XmlHelper.AddNode(doc, root, "CompanyAddress", order.InvoiceCompanyAddress.Trim());
                XmlHelper.AddNode(doc, root, "OrderNote", order.OrderNote);
            }
            else if (Request.IsAuthenticated)
            {
                var siteUser = SiteUtils.GetCurrentSiteUser();
                if (siteUser != null)
                {
                    var userAddess = AccountHelper.GetDefaultUserAddress(siteUser.UserId);
                    if (userAddess != null)
                    {
                        XmlHelper.AddNode(doc, root, "AddressId", userAddess.AddressId.ToString());
                        XmlHelper.AddNode(doc, root, "FirstName", userAddess.FirstName);
                        XmlHelper.AddNode(doc, root, "LastName", userAddess.LastName);
                        XmlHelper.AddNode(doc, root, "Email", string.IsNullOrEmpty(userAddess.Email) ? siteUser.Email : userAddess.Email);
                        XmlHelper.AddNode(doc, root, "Address", userAddess.Address);
                        XmlHelper.AddNode(doc, root, "Phone", userAddess.Phone);
                        XmlHelper.AddNode(doc, root, "Mobile", userAddess.Mobile);
                        XmlHelper.AddNode(doc, root, "Fax", userAddess.Fax);
                        XmlHelper.AddNode(doc, root, "Street", userAddess.Street);
                        XmlHelper.AddNode(doc, root, "Ward", userAddess.Ward);

                        if (userAddess.CountryGuid != Guid.Empty)
                            XmlHelper.AddNode(doc, root, "CountryGuid", userAddess.CountryGuid.ToString());
                        if (userAddess.ProvinceGuid != Guid.Empty)
                        {
                            provinceGuid = userAddess.ProvinceGuid;
                            XmlHelper.AddNode(doc, root, "ProvinceGuid", userAddess.ProvinceGuid.ToString());
                        }
                        if (userAddess.DistrictGuid != Guid.Empty)
                        {
                            districtGuid = userAddess.DistrictGuid;
                            XmlHelper.AddNode(doc, root, "DistrictGuid", userAddess.DistrictGuid.ToString());
                        }
                        if (userAddess.WardGuid != Guid.Empty)
                            XmlHelper.AddNode(doc, root, "WardGuid", userAddess.DistrictGuid.ToString());
                    }
                    else
                    {
                        XmlHelper.AddNode(doc, root, "AddressId", "-1");

                        // Billing address
                        XmlHelper.AddNode(doc, root, "FirstName", siteUser.FirstName);
                        XmlHelper.AddNode(doc, root, "LastName", siteUser.LastName);
                        XmlHelper.AddNode(doc, root, "Email", siteUser.Email);
                        XmlHelper.AddNode(doc, root, "Phone", siteUser.GetCustomPropertyAsString("Phone"));

                        var userProvinceGuid = siteUser.GetCustomPropertyAsString("Province");
                        if (userProvinceGuid.Length == 36)
                        {
                            provinceGuid = new Guid(userProvinceGuid);
                            XmlHelper.AddNode(doc, root, "ProvinceGuid", userProvinceGuid);
                            GeoZone geoZone = new GeoZone(provinceGuid);
                            if (geoZone != null && geoZone.Guid != Guid.Empty)
                                XmlHelper.AddNode(doc, root, "Province", geoZone.Name);
                        }
                    }

                    var address = UserAddress.GetByUser(siteUser.UserId);
                    if (address.Count > 0)
                    {
                        foreach (var item in address)
                        {
                            XmlElement addressXml = doc.CreateElement("UserAddress");
                            root.AppendChild(addressXml);
                            XmlHelper.AddNode(doc, addressXml, "AddressId", item.AddressId.ToString());
                            XmlHelper.AddNode(doc, addressXml, "FirstName", item.FirstName);
                            XmlHelper.AddNode(doc, addressXml, "LastName", item.LastName);
                            XmlHelper.AddNode(doc, addressXml, "Email", string.IsNullOrEmpty(item.Email) ? siteUser.Email : item.Email);
                            XmlHelper.AddNode(doc, addressXml, "Address", item.Address);
                            XmlHelper.AddNode(doc, addressXml, "Phone", item.Phone);
                            XmlHelper.AddNode(doc, addressXml, "Mobile", item.Mobile);
                            XmlHelper.AddNode(doc, addressXml, "Fax", item.Fax);
                            XmlHelper.AddNode(doc, addressXml, "Street", item.Street);
                            XmlHelper.AddNode(doc, addressXml, "Ward", item.Ward); 

                            if (item.CountryGuid != Guid.Empty)
                                XmlHelper.AddNode(doc, addressXml, "CountryGuid", item.CountryGuid.ToString());
                            if (item.ProvinceGuid != Guid.Empty)
                                XmlHelper.AddNode(doc, addressXml, "ProvinceGuid", item.ProvinceGuid.ToString());
                            if (item.DistrictGuid != Guid.Empty)
                                XmlHelper.AddNode(doc, addressXml, "DistrictGuid", item.DistrictGuid.ToString());
                            if (item.WardGuid != Guid.Empty)
                                XmlHelper.AddNode(doc, addressXml, "WardGuid", item.DistrictGuid.ToString());
                        }
                    }
                }
            }

            var languageId = WorkingCulture.LanguageId;
            RenderProvinces(doc, root, provinceGuid, shippingProvinceGuid, languageId);
            if (provinceGuid != Guid.Empty || shippingProvinceGuid != Guid.Empty)
                RenderDistricts(doc, root, provinceGuid, districtGuid.ToString(), shippingDistrictGuid.ToString(), "Districts", languageId);

            XmlHelper.XMLTransform(xmlTransformer, SiteUtils.GetXsltBasePath("product", ModuleConfiguration.XsltFileName), doc);
        }

        private void RenderProvinces(XmlDocument doc, XmlElement root, Guid guid, Guid shippingGuid, int languageId)
        {
            var lstProvinces = GeoZone.GetByCountry(siteSettings.DefaultCountryGuid, 1, languageId);
            foreach (GeoZone province in lstProvinces)
            {
                XmlElement provinceXml = doc.CreateElement("Provinces");
                root.AppendChild(provinceXml);

                XmlHelper.AddNode(doc, provinceXml, "Title", province.Name);
                XmlHelper.AddNode(doc, provinceXml, "Guid", province.Guid.ToString());

                if (province.Guid == guid)
                    XmlHelper.AddNode(doc, provinceXml, "IsActive", "true");
                if (province.Guid == shippingGuid)
                    XmlHelper.AddNode(doc, provinceXml, "ShippingIsActive", "true");
            }
        }

        private void RenderDistricts(XmlDocument doc, XmlElement root, Guid provinceGuid, string guid, string shippingGuid, string elementName, int languageId)
        {
            var lstDistricts = GeoZone.GetByListParent(provinceGuid.ToString(), 1, languageId);
            foreach (GeoZone district in lstDistricts)
            {
                XmlElement provinceXml = doc.CreateElement(elementName);
                root.AppendChild(provinceXml);

                XmlHelper.AddNode(doc, provinceXml, "Title", district.Name);
                XmlHelper.AddNode(doc, provinceXml, "Guid", district.Guid.ToString());

                if (district.Guid.ToString() == guid)
                    XmlHelper.AddNode(doc, provinceXml, "IsActive", "true");
                if (district.Guid.ToString() == shippingGuid)
                    XmlHelper.AddNode(doc, provinceXml, "ShippingIsActive", "true");
            }
        }

        protected virtual void LoadSettings()
        {
            EnsureConfiguration();

            if (this.ModuleConfiguration != null)
            {
                this.Title = ModuleConfiguration.ModuleTitle;
                this.Description = ModuleConfiguration.FeatureName;
            }
        }

        private void EnsureConfiguration()
        {
            if (config == null)
                config = new CheckoutAddressConfiguration(Settings);
        }
    }
}

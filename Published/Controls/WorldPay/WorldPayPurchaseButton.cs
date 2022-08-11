/// Created:			    2013-01-01
/// Last Modified:		    2013-01-01

using Resources;
using System;
using System.Globalization;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CanhCam.Web.Framework;

namespace CanhCam.Web.UI
{
    public class WorldPayPurchaseButton : Button
    {
        private string amountString = string.Empty;

        private CultureInfo currencyCulture = CultureInfo.CurrentCulture;
        public CultureInfo CurrencyCulture
        {
            get { return currencyCulture; }
            set { currencyCulture = value; }
        }

        private string instId = string.Empty;
        [Themeable(false)]
        public string InstId
        {
            get { return instId; }
            set { instId = value; }
        }

        private string merchantCode = string.Empty;

        /// <summary>
        /// As a general rule, we open one merchant code (account) per currency set that you
        /// process. However, you may need to consider using preferred merchant codes if you
        /// have a number of merchant codes with identical characteristics but where they will
        /// be used for different purposes.
        /// For instance, you may have a merchant code for software sales and another for
        /// hardware sales - so, order details submitted to us for software will need to specify
        /// the software merchant code, and order details submitted for hardware will need to
        /// specify the hardware merchant code.
        /// </summary>
        [Themeable(false)]
        public string MerchantCode
        {
            get { return merchantCode; }
            set { merchantCode = value; }
        }

        private string cartId = string.Empty;
        [Themeable(false)]
        public string CartId
        {
            get { return cartId; }
            set { cartId = value; }
        }

        private decimal amount = 0;
        [Themeable(false)]
        public decimal Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        private string currencyCode = "USD";
        [Themeable(false)]
        public string CurrencyCode
        {
            get { return currencyCode; }
            set { currencyCode = value; }
        }
        

        private bool useTestServer = false;
        [Themeable(false)]
        public bool UseTestServer
        {
            get { return useTestServer; }
            set { useTestServer = value; }
        }

        private string testServerPostbackUrl = "https://secure-test.worldpay.com/wcc/purchase";

        public string TestServerPostbackUrl
        {
            get { return testServerPostbackUrl; }
            set { testServerPostbackUrl = value; }
        }

        private string productionServerPostbackUrl = "https://secure.worldpay.com/wcc/purchase";

        public string ProductionServerPostbackUrl
        {
            get { return productionServerPostbackUrl; }
            set { productionServerPostbackUrl = value; }
        }

        private int authValidFrom = -1;

        /// <summary>
        /// This specifies a time window within
        /// which the purchase must (or must not)
        /// be completed, eg. if the purchase is a
        /// time-limited special offer. Each of these
        /// parameters is a time in milliseconds
        /// since 1 January 1970 GMT - a Java
        /// long date value (as from
        /// System.currentTimeMillis() or Date.getTime()), or 1000* a C
        /// time_t. If from to, then the authorisation must complete between
        /// those two times. If tofrom, then the authorisation must complete either
        /// before the to time or after the from time. Either may be zero or omitted to
        /// give the effect of a simple "not before" or "not after" constraint. If both are zero
        /// or omitted, there are no restrictions on how long a shopper can spend making
        /// their purchase (although our server will time-out their session if it is idle for too long).
        /// </summary>
        [Themeable(false)]
        public int AuthValidFrom
        {
            get { return authValidFrom; }
            set { authValidFrom = value; }
        }

        private int authValidTo = -1;
        [Themeable(false)]
        public int AuthValidTo
        {
            get { return authValidTo; }
            set { authValidTo = value; }
        }

        private string lang = string.Empty;
        /// <summary>
        /// 6 char
        /// The shopper's language choice, as a 2-character ISO 639 code, with optional regionalisation using 2-
        /// character country code separated by hyphen. For example "en-GB" specifies UK English. The shopper
        /// can always choose a language on our pages or via browser preferences but if your site has already
        /// made this choice then you can make things more convenient by submitting it to us.
        /// </summary>
        public string Lang
        {
            get { return lang; }
            set { lang = value; }
        }

        private bool noLanguageMenu = false;
        /// <summary>
        /// needs no value
        /// This suppresses the display of the language menu if you have a choice of languages enabled for your
        /// installation but want the choice to be defined by the value of the lang parameter that you submit. Please
        /// contact your local Technical Support department if you would like this facility enabled on your account.
        /// </summary>
        [Themeable(false)]
        public bool NoLanguageMenu
        {
            get { return noLanguageMenu; }
            set { noLanguageMenu = value; }
        }

        private bool withDelivery = false;
        /// <summary>
        /// needs no value
        /// Displays input fields for delivery address and mandate that they be filled in.
        /// </summary>
        [Themeable(false)]
        public bool WithDelivery
        {
            get { return withDelivery; }
            set { withDelivery = value; }
        }


        private string md5Secret = string.Empty;
        [Themeable(false)]
        public string Md5Secret
        {
            get { return md5Secret; }
            set { md5Secret = value; }
        }

        private string orderDescription = string.Empty;
        [Themeable(false)]
        public string OrderDescription
        {
            get { return orderDescription; }
            set { orderDescription = value; }
        }

        private string customerName = string.Empty;
        [Themeable(false)]
        public string CustomerName
        {
            get { return customerName; }
            set { customerName = value; }
        }

        private string customerEmail = string.Empty;
        [Themeable(false)]
        public string CustomerEmail
        {
            get { return customerEmail; }
            set { customerEmail = value; }
        }

        private string address1 = string.Empty;
        [Themeable(false)]
        public string Address1
        {
            get { return address1; }
            set { address1 = value; }
        }

        private string address2 = string.Empty;
        [Themeable(false)]
        public string Address2
        {
            get { return address2; }
            set { address2 = value; }
        }

        private string address3 = string.Empty;
        [Themeable(false)]
        public string Address3
        {
            get { return address3; }
            set { address3 = value; }
        }

        private string town = string.Empty;
        [Themeable(false)]
        public string Town
        {
            get { return town; }
            set { town = value; }
        }

        private string region = string.Empty;
        [Themeable(false)]
        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        private string postalCode = string.Empty;
        [Themeable(false)]
        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        private string country = string.Empty;
        [Themeable(false)]
        public string Country
        {
            get { return country; }
            set { country = value; }
        }

        private string customerPhone = string.Empty;
        [Themeable(false)]
        public string CustomerPhone
        {
            get { return customerPhone; }
            set { customerPhone = value; }
        }

        private bool fixContact = false;
        /// <summary>
        /// if true locks the contact information in the payment page so the customer cannot change it at the worldpay site
        /// </summary>
        [Themeable(false)]
        public bool FixContact
        {
            get { return fixContact; }
            set { fixContact = value; }
        }

        private bool hideContact = false;
        /// <summary>
        /// if true hides the contact details from the shopper when they reach the payment pages
        /// </summary>
        [Themeable(false)]
        public bool HideContact
        {
            get { return hideContact; }
            set { hideContact = value; }
        }

        private string customData = string.Empty;
        /// <summary>
        /// can be used with md5 hash secret to prevent tampering
        /// for example could use a comma separated string of cartoffer guids
        /// then if the user added items to the cart after leaving the site to world pay
        /// by using 2 browser tabs
        /// we can restore the cart offers to the original value when we get the payment response from world pay
        /// since it will include our custom value
        /// </summary>
        [Themeable(false)]
        public string CustomData
        {
            get { return customData; }
            set { customData = value; }
        }

        //private string providerName = string.Empty;
        ///// <summary>
        ///// the provider name of the feature that will handle the shopper response
        ///// </summary>
        //[Themeable(false)]
        //public string ProviderName
        //{
        //    get { return providerName; }
        //    set { providerName = value; }
        //}

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (useTestServer)
            {
                PostBackUrl = testServerPostbackUrl;
            }
            else
            {
                PostBackUrl = productionServerPostbackUrl;
            }

            if (Text.Length == 0)
            {
                Text = Resource.WorldPayBuyButton;
            }

        }

        private string GetSignatureFields()
        {
            if (customData.Length > 0)
            {
                return "instId:amount:currency:cartId:M_custom";
            }
            else
            {
                return "instId:amount:currency:cartId";
            }

        }

       

        private string GetHash()
        {
            string valueToHash;

            if (customData.Length > 0)
            {
                valueToHash = md5Secret + ":" + instId + ":" + amountString + ":" + currencyCode + ":" + cartId + ":" + customData;
            }
            else
            {
                valueToHash = md5Secret + ":" + instId + ":" + amountString + ":" + currencyCode + ":" + cartId;
                
            }

            return CryptoHelper.CalculateMD5Hash(valueToHash).ToLowerInvariant();
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            if (HttpContext.Current == null) { return; }

            if (instId.Length == 0) { return; }
            if (cartId.Length == 0) { return; }
            if (amount <= 0) { return; }

            amount = Math.Round(amount, 2);

            if (useTestServer)
            {
                writer.Write("<input type=\"hidden\" name=\"testMode\" value=\"100\" />");
            }

            writer.Write("<input type=\"hidden\" name=\"instId\" value=\"" + instId + "\" />");

            if (merchantCode.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"accId1\" value=\"" + merchantCode + "\" />");
            }

            amountString = amount.ToString(currencyCulture);

            writer.Write("<input type=\"hidden\" name=\"cartId\" value=\"" + cartId + "\" />");
            writer.Write("<input type=\"hidden\" name=\"amount\" value=\"" + amountString + "\" />");
            writer.Write("<input type=\"hidden\" name=\"currency\" value=\"" + currencyCode + "\" />");

            if (md5Secret.Length > 0)
            {
                //string valueToHash = md5Secret + ":" + instId + ":" + amountString + ":" + currencyCode + ":" + cartId;
                //string hash = CryptoHelper.CalculateMD5Hash(valueToHash);
                //security params
                writer.Write("<input type=\"hidden\" name=\"signatureFields\" value = \"" + GetSignatureFields() + "\" />");
                writer.Write("<input type=\"hidden\" name=signature value=\"" + GetHash() + "\" />");
            }


            //optional params
            if (orderDescription.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"desc\" value=\"" + HttpUtility.HtmlAttributeEncode(orderDescription) + "\" />");
            }

            if (customData.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"M_custom\" value=\"" + customData + "\" />");
            }

            if (customerEmail.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"email\" value=\"" + HttpUtility.HtmlAttributeEncode(customerEmail) + "\" />");
            }

            
            //This tells our server that the test transaction is authorised. Note that the shopper's name
            // parameter is used to specify the test result.
            //<input type=“hidden” name="name" value="AUTHORISED">

            //If you pass the shopper's billing address details to us when you submit order details,
            // we automatically place them into the billing address fields that the shopper would be
            // required to enter in the payment pages. However, the shopper can change these
            // address details in the payment pages unless you specify that they are fixed data

            if (customerName.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"name\" value=\"" + HttpUtility.HtmlAttributeEncode(customerName) + "\"/>");
            }

            if (address1.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"address1\" value=\"" + HttpUtility.HtmlAttributeEncode(address1) + "\"/>");
            }

            if (address2.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"address2\" value=\"" + HttpUtility.HtmlAttributeEncode(address2) + "\"/>");
            }

            if (address3.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"address3\" value=\"" + HttpUtility.HtmlAttributeEncode(address3) + "\"/>");
            }

            if (town.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"town\" value=\"" + HttpUtility.HtmlAttributeEncode(town) + "\"/>");
            }

            if (region.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"region\" value=\"" + HttpUtility.HtmlAttributeEncode(region) + "\"/>");
            }

            if (postalCode.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"postcode\" value=\"" + HttpUtility.HtmlAttributeEncode(postalCode) + "\"/>");
            }

            if (country.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"country\" value=\"" + HttpUtility.HtmlAttributeEncode(country) + "\"/>");
            }

            if (customerPhone.Length > 0)
            {
                writer.Write("<input type=\"hidden\" name=\"tel\" value=\"" + HttpUtility.HtmlAttributeEncode(customerPhone) + "\"/>");
            }

            
            
            

            //To specify that billing address details are fixed data:
            //use an additional parameter in your order details called fixContact to lock
            // the contact information in the payment page.
            if (fixContact)
            {
                writer.Write("<input type=\"hidden\" name=\"fixContact\" />");
            }

            //You can also hide the contact details from the shopper when they reach the payment
            // pages. This is done using the hideContact parameter
    
            if (hideContact)
            {
                writer.Write("<input type=\"hidden\" name=\"hideContact\" />");
            }

            base.Render(writer);
        }
    }
}
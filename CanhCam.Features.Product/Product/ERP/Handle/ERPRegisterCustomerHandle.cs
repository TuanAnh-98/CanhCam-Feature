using CanhCam.Business;
using CanhCam.Business.WebHelpers.UserRegisteredHandlers;
using log4net;

namespace CanhCam.Web.ProductUI
{
    public class ERPRegisterCustomerHandle : UserRegisteredHandlerProvider
    {
        private static readonly ILog log
           = LogManager.GetLogger(typeof(ERPRegisterCustomerHandle));

        public ERPRegisterCustomerHandle()
        { }

        public override void UserRegisteredHandler(object sender, UserRegisteredEventArgs e)
        {
            if (e == null) return;
            if (e.SiteUser == null) return;
            if (!ERPHelper.EnableERP) return;

            try
            {
                ERPHelper.SysnMember(e.SiteUser);
            }
            catch (System.Exception ex)
            {
                log.Error("Sysn ERP Member Error: " + ex.Message);
            }
        }
    }
}
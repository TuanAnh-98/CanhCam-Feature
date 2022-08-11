using CanhCam.Web.Controls.DatePicker;
using System.Collections.Specialized;

namespace CanhCam.Web.UI
{
    public class RadDatePickerProvider : DatePickerProvider
    {
        public override IDatePicker GetDatePicker()
        {
            return new RadDatePickerAdapter();
        }

        public override void Initialize(
            string name,
            NameValueCollection config)
        {
            base.Initialize(name, config);
            // don't read anything from config
            // here as this would raise an error under Medium Trust

        }
    }
}

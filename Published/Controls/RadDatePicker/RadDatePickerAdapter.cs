/// Created:			    2015-01-21
/// Last Modified:		    2015-07-10

using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using CanhCam.Web.Controls.DatePicker;
using Telerik.Web.UI;
using System.Globalization;

namespace CanhCam.Web.UI
{
    public class RadDatePickerAdapter : IDatePicker
    {
        private RadDateTimePicker control;

        #region Constructors

        public RadDatePickerAdapter()
        {
            InitializeAdapter();
        }

        #endregion

        public string ControlID
        {
            get
            {
                return control.ID;
            }
            set
            {
                control.ID = value;
            }
        }

        public string Text
        {
            get
            {
                if(control.SelectedDate != null)
                    return control.SelectedDate.Value.ToString();

                return string.Empty;
            }
            set
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(value, out dt);

                if(dt != DateTime.MinValue)
                    control.SelectedDate = dt;
            }
        }

        string buttonImageUrl = string.Empty;
        /// <summary>
        /// implemented only to support the interface but not really used for this datepicker
        /// </summary>
        public string ButtonImageUrl
        {
            get
            {

                //return control.ButtonImage;
                return buttonImageUrl;
            }
            set
            {

                //control.ButtonImage = value;
                buttonImageUrl = value;
            }
        }

        public Unit Width
        {
            get
            {
                return control.Width;
            }
            set
            {
                control.Width = value;
            }
        }

        public bool ShowTime
        {
            get
            {
                return control.TimePopupButton.Visible;
            }
            set
            {
                control.TimePopupButton.Visible = value;
            }
        }

        private string clockHours = string.Empty;

        public string ClockHours
        {
            get { return clockHours; }
            set { clockHours = value; }
        }

        private bool showMonthList = false;
        public bool ShowMonthList
        {
            get
            {
                return showMonthList;
            }
            set
            {
                showMonthList = value;
            }
        }

        private bool showYearList = false;
        public bool ShowYearList
        {
            get
            {
                return showYearList;
            }
            set
            {
                showYearList = value;
            }
        }

        private string yearRange = string.Empty;
        public string YearRange
        {
            get
            {
                return yearRange;
            }
            set
            {
                yearRange = value;
            }
        }

        private bool showWeek = false;
        public bool ShowWeek
        {
            get { return showWeek; }
            set { showWeek = value; }
        }

        private string calculateWeek = string.Empty;
        public string CalculateWeek
        {
            get { return calculateWeek; }
            set { calculateWeek = value; }
        }

        private int firstDay = 1;
        public int FirstDay
        {
            get { return firstDay; }
            set { firstDay = value; }
        }


        private void InitializeAdapter()
        {
            control = new RadDateTimePicker();
            control.Skin = "Simple";
            //added 2015-07-10
            CultureInfo us = new CultureInfo("en-US");
            control.Culture = us;
            control.DateInput.Culture = us;
            control.TimeView.Culture = us;
            control.DateInput.DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            if (ShowTime)
            {
                control.DateInput.DateFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " " + us.DateTimeFormat.ShortTimePattern;
            }
        }

        #region Public Methods

        public Control GetControl()
        {
            return control;
        }

        #endregion
    }
}

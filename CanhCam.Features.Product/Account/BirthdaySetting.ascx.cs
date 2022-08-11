using System;
using System.Web.UI.WebControls;
using System.Collections;
using CanhCam.Web.UI;
using System.Web.UI;

namespace CanhCam.Web.AccountUI
{
    public partial class BirthdaySetting : UserControl, ISettingControl
    {
        int year, month;

        #region ISettingControl

        public string GetValue()
        {
            EnsureControls();

            if (ddDay.SelectedValue != "0"
                    && ddMonth.SelectedValue != "0"
                    && ddYear.SelectedValue != "0")
                return ddDay.SelectedValue + "/" + ddMonth.SelectedValue + "/" + ddYear.SelectedValue;

            return string.Empty;
        }

        public void SetValue(string val)
        {
            EnsureControls();

            string[] args = val.Split('/');
            if (args.Length != 3) { return; }

            ListItem item = ddYear.Items.FindByValue(args[2]);
            if (item != null)
            {
                ddYear.ClearSelection();
                item.Selected = true;
            }

            item = ddMonth.Items.FindByValue(args[1]);
            if (item != null)
            {
                ddMonth.ClearSelection();
                item.Selected = true;
            }

            year = Int32.Parse(ddYear.SelectedValue);
            month = Int32.Parse(ddMonth.SelectedValue);
            BindDays(year, month);

            item = ddDay.Items.FindByValue(args[0]);
            if (item != null)
            {
                ddDay.ClearSelection();
                item.Selected = true;
            }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            EnsureControls();
        }

        private void EnsureControls()
        {
            if (ddYear == null)
            {
                ddYear = new DropDownList();

                if (this.Controls.Count == 0) { this.Controls.Add(ddYear); }
            }

            if (ddMonth == null)
            {
                ddMonth = new DropDownList();

                if (this.Controls.Count == 1) { this.Controls.Add(ddMonth); }
            }

            if (ddDay == null)
            {
                ddDay = new DropDownList();

                if (this.Controls.Count == 2) { this.Controls.Add(ddDay); }
            }

            if (ddYear.Items.Count > 0) { return; }

            DateTime tnow = DateTime.Now;
            ArrayList alYear = new ArrayList();
            int i;
            for (i = tnow.Year - 1; i >= tnow.Year - 50; i--)
                alYear.Add(i);
            ArrayList alMonth = new ArrayList();
            for (i = 1; i <= 12; i++)
                alMonth.Add(i);

            ddYear.DataSource = alYear;
            ddYear.DataBind();
            ddYear.Items.Insert(0, new ListItem(Resources.ProductResources.RegisterYearLabel, "0"));
            //ddYear.SelectedValue = tnow.Year.ToString();

            ddMonth.DataSource = alMonth;
            ddMonth.DataBind();
            ddMonth.Items.Insert(0, new ListItem(Resources.ProductResources.RegisterMonthLabel, "0"));
            //ddMonth.SelectedValue = tnow.Month.ToString();

            year = Int32.Parse(ddYear.SelectedValue);
            month = Int32.Parse(ddMonth.SelectedValue);
            BindDays(year, month);
            //ddDay.SelectedValue = tnow.Day.ToString();
        }

        private bool CheckLeap(int year)
        {
            if ((year % 4 == 0) && (year % 100 != 0) || (year % 400 == 0))
                return true;
            else return false;
        }

        //binding every month day
        private void BindDays(int year, int month)
        {
            ddDay.Items.Clear();

            int i;
            ArrayList alDay = new ArrayList();

            switch (month)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    for (i = 1; i <= 31; i++)
                        alDay.Add(i);
                    break;
                case 2:
                    if (CheckLeap(year))
                    {
                        for (i = 1; i <= 29; i++)
                            alDay.Add(i);
                    }
                    else
                    {
                        for (i = 1; i <= 28; i++)
                            alDay.Add(i);
                    }
                    break;
                case 4:
                case 6:
                case 9:
                case 11:
                    for (i = 1; i <= 30; i++)
                        alDay.Add(i);
                    break;
            }

            ddDay.DataSource = alDay;
            ddDay.DataBind();

            ddDay.Items.Insert(0, new ListItem(Resources.ProductResources.RegisterDayLabel, "0"));
        }

        //select year
        public void ddYear_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            year = Int32.Parse(ddYear.SelectedValue);
            month = Int32.Parse(ddMonth.SelectedValue);
            BindDays(year, month);
        }

        //select month
        public void ddMonth_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            year = Int32.Parse(ddYear.SelectedValue);
            month = Int32.Parse(ddMonth.SelectedValue);
            BindDays(year, month);
        }

    }
}
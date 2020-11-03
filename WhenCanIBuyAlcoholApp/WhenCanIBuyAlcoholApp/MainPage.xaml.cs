using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace WhenCanIBuyAlcoholApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();   
        }
        void OnClicked(object sender, System.EventArgs e)
        {

            DateTime birthday = bday.Date;
            int legalAge = 21;
            switch (Location.SelectedItem)
            {
                case "USA":
                    legalAge = 21;
                    break;
                case "Mexico":
                    legalAge = 18;
                    break;
                case "Brazil":
                    legalAge = 21;
                    break;
                case "Australia":
                    legalAge = 18;
                    break;
            }
            DateTime.Compare(birthday, birthday.AddYears(legalAge));
            int daysTill21 = (birthday.AddYears(legalAge) - DateTime.Now).Days;
            var dateSpan = DateTimeSpan.CompareDates(birthday.AddYears(legalAge), DateTime.Now);
            string message = "";

            if (daysTill21 > 0)
            {
                message = $"In {Location.SelectedItem}, You will be able to drink in {dateSpan.Years} years {dateSpan.Months} months {dateSpan.Days} days.";
            }
            else if(daysTill21 == 0)
            {
                message = $"Congratulations! In {Location.SelectedItem}, You can drink today!";
            }
            else
            {
                message = $"In {Location.SelectedItem}, You have been able to drink for {dateSpan.Years} years {dateSpan.Months} months {dateSpan.Days} days.";
            }

            output.Text = $"{message}";

        }
    }

    //I got everything below off of stackOverflow
    //It allows me to return the difference between two dates in years, months, days and more
    public struct DateTimeSpan
    {
        public int Years { get; }
        public int Months { get; }
        public int Days { get; }
        public int Hours { get; }
        public int Minutes { get; }
        public int Seconds { get; }
        public int Milliseconds { get; }

        public DateTimeSpan(int years, int months, int days, int hours, int minutes, int seconds, int milliseconds)
        {
            Years = years;
            Months = months;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }
        enum Phase { Years, Months, Days, Done }

        public static DateTimeSpan CompareDates(DateTime date1, DateTime date2)
        {
            if (date2 < date1)
            {
                var sub = date1;
                date1 = date2;
                date2 = sub;
            }

            DateTime current = date1;
            int years = 0;
            int months = 0;
            int days = 0;

            Phase phase = Phase.Years;
            DateTimeSpan span = new DateTimeSpan();
            int officialDay = current.Day;

            while (phase != Phase.Done)
            {
                switch (phase)
                {
                    case Phase.Years:
                        if (current.AddYears(years + 1) > date2)
                        {
                            phase = Phase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;
                    case Phase.Months:
                        if (current.AddMonths(months + 1) > date2)
                        {
                            phase = Phase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                current = current.AddDays(officialDay - current.Day);
                        }
                        else
                        {
                            months++;
                        }
                        break;
                    case Phase.Days:
                        if (current.AddDays(days + 1) > date2)
                        {
                            current = current.AddDays(days);
                            var timespan = date2 - current;
                            span = new DateTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = Phase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }

            return span;
        }
    }
}

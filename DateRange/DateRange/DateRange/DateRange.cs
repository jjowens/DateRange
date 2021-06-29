using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JO.DateRange
{
    public enum DateIncrement
    {
        minutes,
        days,
        weeks,
        months,
        years
    }

    public class DateRange
    {

        private DateTime _startDate;
        private DateTime _endDate;

        private List<DayOfWeek> _excludeDaysOfWeek;
        private List<int> _excludeDays;
        private List<DayOfWeek> _includeDaysOfWeek;
        private List<int> _includeDays;
        private string _dailyOrWeekly;

        public DateRange(DateTime startDate, DateTime endDate, string DailyOrWeekly = "daily")
        {
            init(startDate, endDate, DailyOrWeekly);
        }

        public DateRange(DateTime startDate, int incrementAmount, DateIncrement dateIncrement, string DailyOrWeekly = "daily")
        {
            DateTime endDate = startDate;

            switch(dateIncrement)
            {
                case DateIncrement.minutes:
                    endDate = startDate.AddMinutes(incrementAmount);
                    break;
                case DateIncrement.days:
                    endDate = startDate.AddDays(incrementAmount);
                    break;
                case DateIncrement.weeks:
                    incrementAmount = incrementAmount * 7;
                    endDate = startDate.AddDays(incrementAmount);
                    break;
                case DateIncrement.months:
                    endDate = startDate.AddMonths(incrementAmount);
                    break;
            }

            init(startDate, endDate, DailyOrWeekly);
        }

        private void init(DateTime startDate, DateTime endDate, string DailyOrWeekly = "daily")
        {
            _startDate = startDate;
            _endDate = endDate;
            _excludeDaysOfWeek = new List<DayOfWeek>();
            _excludeDays = new List<int>();

            _includeDaysOfWeek = new List<DayOfWeek>();
            _includeDays = new List<int>();

            _dailyOrWeekly = (DailyOrWeekly != string.Empty) ? DailyOrWeekly.ToLower().Trim() : "daily";
        }

        public DateRange IgnoreDaysOfWeek(DayOfWeek dayOfWeek)
        {
            _excludeDaysOfWeek.Add(dayOfWeek);

            return this;
        }

        public DateRange IgnoreDaysOfWeekRange(List<DayOfWeek> daysOfWeek)
        {
            _excludeDaysOfWeek.AddRange(daysOfWeek);

            return this;
        }

        public DateRange IgnoreDay(int day)
        {
            _excludeDays.Add(day);

            return this;
        }

        public DateRange IgnoreDays(List<int> days)
        {
            _excludeDays.AddRange(days);

            return this;
        }

        public DateRange IncludeDayOfWeekRange(List<DayOfWeek> daysOfWeek)
        {
            _includeDaysOfWeek.AddRange(daysOfWeek);

            return this;
        }

        public DateRange IncludeDayOfWeek(DayOfWeek dayOfWeek)
        {
            _includeDaysOfWeek.Add(dayOfWeek);

            return this;
        }

        public DateRange IncludeDaysRange(List<int> days)
        {
            _includeDays.AddRange(days);

            return this;
        }

        public DateRange IncludeDays(int days)
        {
            _includeDays.Add(days);

            return this;
        }

        private List<DateTime> createDates()
        {
            List<DateTime> listOfDates = new List<DateTime>();
            TimeSpan ts = _endDate - _startDate;

            int incrementDays = 1;

            switch (_dailyOrWeekly)
            {
                case "daily":
                    incrementDays = 1;
                    break;
                case "weekly":
                    incrementDays = 7;
                    break;
                default:
                    incrementDays = 1;
                    break;
            }

            int totalDays = ts.Days;

            DateTime currentDate = _startDate;
            bool ignoreFlag = false;

            while (currentDate.Date <= _endDate.Date)
            {
                // RUN LOGIC CHECK RULES
                if (_excludeDaysOfWeek.Contains(currentDate.DayOfWeek))
                {
                    ignoreFlag = true;
                }

                if (_excludeDays.Contains(currentDate.Day))
                {
                    ignoreFlag = true;
                }

                if (!ignoreFlag)
                {
                    if (_includeDaysOfWeek.Count > 0)
                    {
                        ignoreFlag = !(_includeDaysOfWeek.Contains(currentDate.DayOfWeek));
                    }


                    if (_includeDays.Count > 0)
                    {
                        ignoreFlag = !(_includeDays.Contains(currentDate.Day));
                    }
                }

                if (!ignoreFlag)
                {
                    listOfDates.Add(currentDate);
                }

                currentDate = currentDate.AddDays(incrementDays);
                ignoreFlag = false;
            }

            return listOfDates;
        }

        public List<DateTime> ToList()
        {
            List<DateTime> listOfDates = createDates();

            return listOfDates;
        }


        public Dictionary<string, List<DateTime>> ToDictionaryGroupByMonth()
        {
            Dictionary<string, List<DateTime>> listOfDates = new Dictionary<string, List<DateTime>>();
            List<DateTime> tempList = new List<DateTime>();

            tempList = createDates();

            List<string> months = new List<string>()
            {
                "", "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
            };

            var q = (from d in tempList
                     group d by months[d.Month] into g
                     select g);

            listOfDates = q.ToDictionary(x => x.Key, x => x.ToList());

            return listOfDates;
        }

        public Dictionary<string, List<DateTime>> ToDictionaryGroupByDayOfWeek()
        {
            Dictionary<string, List<DateTime>> listOfDates = new Dictionary<string, List<DateTime>>();
            List<DateTime> tempList = new List<DateTime>();

            tempList = createDates();

            var q = (from d in tempList
                     group d by d.DayOfWeek.ToString() into g
                     select g);

            listOfDates = q.ToDictionary(x => x.Key, x => x.ToList());

            return listOfDates;
        }



    }
}

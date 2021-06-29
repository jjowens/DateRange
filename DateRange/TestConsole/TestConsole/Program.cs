using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JO.DateRange;

namespace TestConsole
{
    class Program
    {
        private static DateTime _startDate = new DateTime();
        private static DateTime _endDate = new DateTime();
        private static List<DayOfWeek> _excludedaysOfWeek = new List<DayOfWeek>();
        private static List<DayOfWeek> _includedaysOfWeek = new List<DayOfWeek>();
        private static List<int> _excludeDays = new List<int>();
        private static List<int> _includeDays = new List<int>();

        static void Main(string[] args)
        {
            // TEST BASIC RANGE OF DATES
            //getBasicRangeOfDates();

            //getFilterIncludeDaysOfWeek();

            //getFilterExcludeDaysOfWeek();

            //getGroupDates();

            //getGroupDatesFiltered();

            //getGroupDatesByDayOfWeek();

            //testChainDates();

            createDateIncrement();

            Console.ReadLine();
        }

        private static void resetData()
        {
            DateTime tempDate = DateTime.Now;
            _startDate = new DateTime(tempDate.Year, tempDate.Month, 1, 0, 0, 0);
            _endDate = _startDate.AddMonths(1).AddDays(-1);
            _excludedaysOfWeek = new List<DayOfWeek>();
            _includedaysOfWeek = new List<DayOfWeek>();
            _excludeDays = new List<int>();
            _includeDays = new List<int>();
        }



        private static void getBasicRangeOfDates()
        {
            resetData();

            Console.WriteLine("Daily Range of Dates");
            getDates(_startDate, _endDate, false, "daily", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, string.Empty);

            Console.WriteLine("Weekly Range of Dates");
            getDates(_startDate, _endDate, false, "weekly", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, string.Empty);
        }

        private static void getFilterIncludeDaysOfWeek()
        {
            resetData();

            _includedaysOfWeek.Add(DayOfWeek.Tuesday);
            _includedaysOfWeek.Add(DayOfWeek.Thursday);

            Console.WriteLine("Weekly Range of Dates - Should only show {0}", string.Join(",", _includedaysOfWeek.ToArray()));
            getDates(_startDate, _endDate, false, "daily", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, string.Empty);
        }

        private static void getFilterExcludeDaysOfWeek()
        {
            resetData();

            _excludedaysOfWeek.Add(DayOfWeek.Tuesday);
            _excludedaysOfWeek.Add(DayOfWeek.Thursday);

            Console.WriteLine("Weekly Range of Dates - Should exclude {0}", string.Join(",", _excludedaysOfWeek.ToArray()));
            getDates(_startDate, _endDate, false, "daily", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, string.Empty);
        }

        private static void getGroupDates()
        {
            resetData();

            DateTime tempDate = DateTime.Now;
            _startDate = new DateTime(tempDate.Year, tempDate.Month, 1, 0, 0, 0);
            _endDate = _startDate.AddYears(1);

            Console.WriteLine("Group Range of Dates");
            getDates(_startDate, _endDate, true, "daily", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, string.Empty);
        }

        private static void getGroupDatesFiltered()
        {
            resetData();

            DateTime tempDate = DateTime.Now;
            _startDate = new DateTime(tempDate.Year, tempDate.Month, 1, 0, 0, 0);
            _endDate = _startDate.AddYears(1);

            _includedaysOfWeek.Add(DayOfWeek.Monday);
            _includedaysOfWeek.Add(DayOfWeek.Tuesday);

            Console.WriteLine("Group Range of Dates - Filter by {0}", string.Join(",", _includedaysOfWeek.ToArray()));
            getDates(_startDate, _endDate, true, "daily", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, string.Empty);
        }

        private static void getGroupDatesByDayOfWeek()
        {
            resetData();

            DateTime tempDate = DateTime.Now;
            _startDate = new DateTime(tempDate.Year, tempDate.Month, 1, 0, 0, 0);
            _endDate = _startDate.AddYears(1);

            Console.WriteLine("Group Range of Dates by Day of Week");
            getDates(_startDate, _endDate, true, "daily", _excludedaysOfWeek, _excludeDays, _includedaysOfWeek, _includeDays, "daysofweek");
        }

        private static void testChainDates()
        {
            resetData();

            Console.WriteLine("Daily Range of Dates");
            dateChain(_startDate, _endDate);

        }

        private static void createDateIncrement()
        {
            resetData();

            var tempDate = DateTime.Now;
            _startDate = new DateTime(tempDate.Year, tempDate.Month, 1, 0, 0, 0);

            DateRange dateRange = new DateRange(_startDate, 3, DateIncrement.months, "daily");

            var listOfDates = dateRange.ToList();

            Console.WriteLine("Start Date: {0} {1}", _startDate.DayOfWeek, _startDate.ToLongDateString());

            foreach (var d in listOfDates)
            {
                Console.WriteLine("{0} {1}", d.DayOfWeek, d.ToLongDateString());
            }

            Console.WriteLine("");
            Console.WriteLine("");
        }

        static void getDates(DateTime startDate, DateTime endDate,
            bool groupDates,
            string dailyOrWeekly,
            List<DayOfWeek> ignoreDaysOfWeek,
            List<int> ignoreDays,
            List<DayOfWeek> includeDaysOfWeek,
            List<int> includeDays,
            string groupType
            )
        {
            DateRange dateRange = new DateRange(startDate, endDate, dailyOrWeekly);

            if (ignoreDaysOfWeek != null)
            {
                dateRange.IgnoreDaysOfWeekRange(ignoreDaysOfWeek);
            }

            if (ignoreDays != null)
            {
                dateRange.IgnoreDays(ignoreDays);
            }

            if (includeDays != null)
            {
                dateRange.IncludeDaysRange(includeDays);
            }

            if (includeDaysOfWeek != null)
            {
                dateRange.IncludeDayOfWeekRange(includeDaysOfWeek);
            }

            Console.WriteLine("Start Date: {0} {1}", startDate.DayOfWeek, startDate.ToLongDateString());
            Console.WriteLine("End Date: {0} {1}", endDate.DayOfWeek, endDate.ToLongDateString());

            if (groupDates)
            {
                Console.WriteLine("Group by Dates");
                Console.WriteLine("=========================\r\n");

                Dictionary<string, List<DateTime>> groupOfDates = new Dictionary<string, List<DateTime>>();

                if (groupType == "daysofweek")
                {
                    groupOfDates = dateRange.ToDictionaryGroupByDayOfWeek();
                } else
                {
                    groupOfDates = dateRange.ToDictionaryGroupByMonth();
                }

                foreach (var group in groupOfDates)
                {
                    Console.WriteLine("{0}", group.Key);

                    foreach (var d in group.Value)
                    {
                        Console.WriteLine("{0} {1}", d.DayOfWeek, d.ToLongDateString());
                    }

                    Console.WriteLine("");
                }
            }
            else
            {
                var listOfDates = dateRange.ToList();

                foreach (var d in listOfDates)
                {
                    Console.WriteLine("{0} {1}", d.DayOfWeek, d.ToLongDateString());
                }
            }

            Console.WriteLine("");
            Console.WriteLine("");

        }

        /// <summary>
        /// Test chain function
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        static void dateChain(DateTime startDate, DateTime endDate)
        {
            DateRange dateRange = new DateRange(startDate, endDate, "daily");

            Console.WriteLine("Start Date: {0} {1}", startDate.DayOfWeek, startDate.ToLongDateString());
            Console.WriteLine("End Date: {0} {1}", endDate.DayOfWeek, endDate.ToLongDateString());

            var listOfDates = dateRange.IgnoreDaysOfWeek(DayOfWeek.Monday)
                    .IgnoreDaysOfWeek(DayOfWeek.Friday)
                    .ToList();

            foreach (var d in listOfDates)
            {
                Console.WriteLine("{0} {1}", d.DayOfWeek, d.ToLongDateString());
            }

            Console.WriteLine("");
            Console.WriteLine("");

        }


    }
}

using ProductCatalogAdminUiIntegrationTest.Data.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProductCatalogAdminUiIntegrationTest.Data.Utility
{
    public static class SortTimeAgoDateUtility
    {
        public const string JustNowTwoChar = "no";
        public const string MinuteTwoChar = "mi";
        public const string HourTwoChar = "ho";
        public const string DayTwoChar = "da";
        public const string MonthTwoChar = "mo";
        public const string YesterdayTwoChar = "Ye";

        public const string MinuteAgo = "minute ago";
        public const string MinutesAgo = "minutes ago";
        public const string HourAgo = "hour ago";
        public const string HoursAgo = "hours ago";
        public const string DayAgo = "day ago";
        public const string DaysAgo = "days ago";

        public static List<string> SortDateType(List<string> cellsFromColumn, SortDirectionType sortDirection)
        {
            List<string> sortMinutes = new List<string>();
            List<string> sortHours = new List<string>();
            List<string> sortDay = new List<string>();
            List<string> sortMonth = new List<string>();
            List<DateTime> sortDateFormat = new List<DateTime>();
            List<string> justNow = new List<string>();
            List<string> yesterday = new List<string>();

            foreach (var date in cellsFromColumn)
            {
                switch (GetTypeOfDateTypeOrDate(date))
                {
                    case DateType.Month:
                        sortMonth.Add(date);
                        break;
                    case DateType.Day:
                        sortDay.Add(date);
                        break;
                    case DateType.Hour:
                        sortHours.Add(date);
                        break;
                    case DateType.Minute:
                        sortMinutes.Add(date);
                        break;
                    case DateType.Yesterday:
                        yesterday.Add(date);
                        break;
                    case DateType.JustNow:
                        justNow.Add(date);
                        break;
                    default:
                        sortDateFormat.Add(DateTime.Parse(date));
                        break;
                }


            }

            if (sortDirection == SortDirectionType.Ascending)
            {
                return justNow
                    .Concat(SortCellValuesNumberically(sortMinutes, sortDirection, DateType.Minute))
                    .Concat(SortCellValuesNumberically(sortHours, sortDirection, DateType.Hour))
                    .Concat(yesterday)
                    .Concat(SortCellValuesNumberically(sortDay, sortDirection, DateType.Day))
                    .Concat(SortCellValuesNumberically(sortMonth, sortDirection, DateType.Month))
                    .Concat(SortDateTypeCellTextValues(sortDateFormat, sortDirection))
                    .ToList();

            }
            else
            {
                return SortDateTypeCellTextValues(sortDateFormat, sortDirection)
                    .Concat(SortCellValuesNumberically(sortMonth, sortDirection, DateType.Month))
                    .Concat(SortCellValuesNumberically(sortDay, sortDirection, DateType.Day))
                    .Concat(yesterday)
                    .Concat(SortCellValuesNumberically(sortHours, sortDirection, DateType.Hour))
                    .Concat(SortCellValuesNumberically(sortMinutes, sortDirection, DateType.Minute))
                    .Concat(justNow).ToList();
            }


        }

        private static List<string> SortCellValuesNumberically(List<string> cellTextValues, SortDirectionType sortDirection, DateType dateType)
        {
            List<int> convertedCellValuesToInt = new List<int>();
            if (cellTextValues.Count == 0)
            {
                return new List<string>(); 
            }

            foreach (var date in cellTextValues)
            {
                string[] dateSplit = date.Split(" ");
                convertedCellValuesToInt.Add(int.Parse(dateSplit[0]));
            }


            var result = sortDirection == SortDirectionType.Ascending
                ? convertedCellValuesToInt.OrderBy(value => value).ToList()
                : convertedCellValuesToInt.OrderByDescending(value => value).ToList();

            List<string> convertedCellValuesBackToString = new List<string>();

            foreach (var item in convertedCellValuesToInt)
            {
                switch (dateType)
                {
                    case DateType.Minute:
                        if(item == 1 )
                        {
                            convertedCellValuesBackToString.Add($"{item} {MinuteAgo}");
                        } else convertedCellValuesBackToString.Add($"{item} {MinutesAgo}");
                        break;
                    case DateType.Hour:
                        if (item == 1)
                        {
                            convertedCellValuesBackToString.Add($"{item} {HourAgo}");
                        }
                        else convertedCellValuesBackToString.Add($"{item} {HoursAgo}");
                        break;
                    case DateType.Day:
                        if (item == 1)
                        {
                            convertedCellValuesBackToString.Add($"{item} {DayAgo}");
                        }
                        else convertedCellValuesBackToString.Add($"{item} {DaysAgo}");
                        break;
                }


            }
            return convertedCellValuesBackToString;
        }

        private static List<string> SortDateTypeCellTextValues(List<DateTime> cellTextValues, SortDirectionType sortDirection)
        {
            var result = sortDirection == SortDirectionType.Ascending
               ? SortDateAscending(cellTextValues)
               : SortDateDescending(cellTextValues);

            List<string> transformedDatesToStrings = new List<string>();
            foreach (var date in result)
            {
                transformedDatesToStrings.Add(date.ToString("dd MMM, yyyy"));
            }

            return transformedDatesToStrings;
        }

        private static List<DateTime> SortDateAscending(List<DateTime> list)
        {
            list.Sort((a, b) => a.CompareTo(a));
            return list;
        }

        private static List<DateTime> SortDateDescending(List<DateTime> list)
        {
            list.Sort((a, b) => b.CompareTo(b));
            var item = list;
            return list;
        }

        private static string ParseDateStringByTwo(string dateType)
        {
            if(dateType == nameof(DateType.JustNow))
            {
                return dateType;
            }
            var result = dateType.Substring(0, 2);
            return result;


        }

        private static dynamic GetTypeOfDateTypeOrDate(string dateType)
        {
            string[] dateTypeList = dateType.Split(" ");
            if(dateTypeList.Length > 1)
            {
                switch (ParseDateStringByTwo(dateTypeList[1]))
                {
                    case JustNowTwoChar:
                        return DateType.JustNow;
                    case MinuteTwoChar:
                        return DateType.Minute;
                    case HourTwoChar:
                        return DateType.Hour;
                    case DayTwoChar:
                        return DateType.Day;
                    case MonthTwoChar:
                        return DateType.Month;
                    default:
                        return dateType;
                }

            }
            else if (dateTypeList.Length == 1 )
            {
                return DateType.Yesterday;
            }
            else
            {
                return dateType;
            }



        }

    }
}

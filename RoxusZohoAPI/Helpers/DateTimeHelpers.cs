using RoxusZohoAPI.Helpers.Constants;
using System;

namespace RoxusZohoAPI.Helpers
{
    public class DateTimeHelpers
    {
        public static string ConvertDateTimeToPlainString(DateTime dateTime)
        {
            return dateTime.ToString(CommonConstants.PlainDateTimeFormat);
        }

        public static string ConvertDateTimeToString(DateTime dateTime)
        {
            return dateTime.ToString(CommonConstants.DateTimeFormat);
        }

        public static DateTime ConvertStringToDateTime(string dateTimeString)
        {
            return DateTime.ParseExact(dateTimeString, CommonConstants.DateTimeFormat, null);
        }

        public static string ConvertDateTimeByFormat(string dateTimeString, string format)
        {
            DateTime date = DateTime.Parse(dateTimeString);
            return date.ToString(format);
        }

        public static DateTime UnixTimestampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp);
            return dateTime;
        }

        public static DateTime UnixTimestampToDateTime(double unixTimeStamp, bool isSecond = false)
        {
            // Unix timestamp is seconds past epoch
            var dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            if (isSecond)
            {
                dateTime = dateTime.AddSeconds(unixTimeStamp).ToUniversalTime();
            }
            else
            {
                dateTime = dateTime.AddMilliseconds(unixTimeStamp).ToUniversalTime();
            }
            return dateTime;
        }

        public static DateTime AddDaysForLetter(int numberOfDay)
        {
            var newDate = DateTime.UtcNow;
            newDate = newDate.AddDays(numberOfDay);
            while (newDate.DayOfWeek == DayOfWeek.Saturday || newDate.DayOfWeek == DayOfWeek.Sunday)
            {
                newDate = newDate.AddDays(1);
            }
            return newDate;
        }
        
        public static DateTime ConvertLetterDateTime(string letterDate) 
        {
            var letterSplits = letterDate.Split(" ", 2);
            string datePart = letterSplits[0];
            var dateSplits = datePart.Split("-");
            int month = int.Parse(dateSplits[0]);
            int day = int.Parse(dateSplits[1]);
            int year = int.Parse(dateSplits[2]);
            return new DateTime(year, month, day, 0, 0, 0);
        }

        public static DateTime ConvertProjectDateToDateTime(string projectDate)
        {
            string[] dateSplits = projectDate.Split('-');
            int year = int.Parse(dateSplits[2]);
            int month = int.Parse(dateSplits[0]);
            int day = int.Parse(dateSplits[1]);
            var dateTime = new DateTime(year, month, day, 0, 0, 0);
            return dateTime;
        }

        public static int GetMonthsBetween(DateTime from, DateTime to)
        {
            if (from > to) return GetMonthsBetween(to, from);

            var monthDiff = Math.Abs((to.Year * 12 + (to.Month - 1)) - (from.Year * 12 + (from.Month - 1)));

            if (from.AddMonths(monthDiff) > to || to.Day < from.Day)
            {
                return monthDiff - 1;
            }
            else
            {
                return monthDiff;
            }
        }

    }
}

using System;
using System.Collections.Generic;

namespace UtilityToolkit.Extensions
{
    public static class DateTimes
    {
        public const string Format_ShortDateString = "dd MMM yyyy";
        public const string Format_ShortDateTimeString = "dd MM yyyy HH:mm";
        public const string Format_DefaultNullToken = "N/A";

        public static readonly string[] Months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        public static readonly string[] MonthsInDigits = { "01", "02", "03", "04", "05", "06", "07", "08", "09", "10", "11", "12" };

        public static int LocalUtcOffsetMinutes
        {
            get
            {
                return ((int)DateTimeOffset.Now.Offset.TotalMinutes);
            }
        }

        public static double LocalUtcOffsetHours
        {
            get
            {
                return ((int)DateTimeOffset.Now.Offset.TotalHours);
            }
        }

        #region Formatting for Not Nullable DateTimes

        public static string ToShortDateOnlyString(this DateTime dateTime)  //Extension method for nullable DateTime
        {
            return dateTime.ToString(Format_ShortDateString);
        }

        public static string ToShortDateTimeString(this DateTime dateTime)  //Extension method for nullable DateTime
        {
            return dateTime.ToString(Format_ShortDateTimeString);
        }

        #endregion

        #region Formatting for Nullable DateTimes

        public static string ToString(this DateTime? dateTime, string format, string nullToken) //Extension method for nullable DateTime
        {
            return (dateTime.HasValue) ? dateTime.Value.ToString(format) : nullToken;
        }

        public static string ToString(this DateTime? dateTime, string format)  //Extension method for nullable DateTime
        {
            return (dateTime.HasValue) ? dateTime.Value.ToString(format) : Format_DefaultNullToken;
        }

        public static string ToShortDateString(this DateTime? dateTime, string nullToken)
        {
            return (dateTime.HasValue) ? dateTime.Value.ToShortDateString() : nullToken;
        }

        public static string ToShortDateOnlyString(this DateTime? dateTime, string nullToken)  //Extension method for nullable DateTime
        {
            return ToString(dateTime, Format_ShortDateString, nullToken);
        }

        public static string ToShortDateTimeString(this DateTime? dateTime, string nullToken)  //Extension method for nullable DateTime
        {
            return ToString(dateTime, Format_ShortDateTimeString, nullToken);
        }

        public static string ToShortDateOnlyString(this DateTime? dateTime)  //Extension method for nullable DateTime
        {
            return ToString(dateTime, Format_ShortDateString, Format_DefaultNullToken);
        }

        public static string ToShortDateTimeString(this DateTime? dateTime)  //Extension method for nullable DateTime
        {
            return ToString(dateTime, Format_ShortDateTimeString, Format_DefaultNullToken);
        }

        #endregion

        public static DateTime? NullableFromDB(object dataReaderValue)
        {
            return (dataReaderValue == Convert.DBNull) ? null : (DateTime?)dataReaderValue;
        }

        public static DateTime? ParseNullable(string dateTimeString)
        {
            return ParseNullable(dateTimeString, null);
        }

        public static DateTime? ParseNullable(string dateTimeString, DateTime? defaultValue)
        {
            return String.IsNullOrEmpty(dateTimeString) ? defaultValue : DateTime.Parse(dateTimeString);
        }

        public static DateTime? ParseNullable(string dateTimeString, string dateTimeFormat, DateTime? defaultValue)
        {
            return String.IsNullOrEmpty(dateTimeString) ? defaultValue : DateTime.ParseExact(dateTimeString, dateTimeFormat, null);
        }

        public static DateTime? TryParseNullable(string dateTimeString, DateTime? defaultValue)
        {
            try
            {
                return ParseNullable(dateTimeString, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static DateTime? TryParseNullable(string dateTimeString, string dateTimeFormat, DateTime? defaultValue)
        {
            try
            {
                return ParseNullable(dateTimeString, dateTimeFormat, defaultValue);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }

        public static IEnumerable<string> GetYears(int from, int numberOfYears)
        {
            for (int i = 0; i < numberOfYears; i++)
            {
                yield return (from + i).ToString();
            }
        }
    }
}

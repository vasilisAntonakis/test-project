using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace WebAPI.Commons
{
    public static class DateTimeStringConverter
    {
        public static string format = "yyyyMMdd";

        private static bool ParseDateTime(string s, out DateTime dt) =>
            DateTime.TryParseExact(s, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out dt);

        public static DateTime? ToDateTimeNullable(string s)
        {
            DateTime dt;
            if (ParseDateTime(s, out dt))
                return dt;
            return null;
        }

        public static DateTime ToDateTime(string s)
        {
            DateTime dt;
            ParseDateTime(s, out dt);
            return dt;
        }

        public static string ToString(DateTime dt)
        {
            return dt.ToString(format);
        }

        public static string ToString(DateTime? dt)
        {
            if (dt.HasValue)
                return ToString(dt.Value);
            return null;
        }
    }
}
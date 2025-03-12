using System;

namespace HsR.Common.Extenders
{
    public static class DateTimeExtenders
    {
        public static string? ToTimeFormattedString(this DateTime? value)
        {
            return value?.ToString("yyyy-MM-dd HH:mm");
        }

        public static string ToTimeFormattedString(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
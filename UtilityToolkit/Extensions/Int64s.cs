using System;

namespace UtilityToolkit.Extensions
{
    public static class Int64s
    {
        public static long? ParseNullable(string value)
        {
            return ParseNullable(value, null);
        }

        public static long? ParseNullable(string value, long? defaultValue)
        {
            return String.IsNullOrEmpty(value) ? defaultValue : Int64.Parse(value);
        }

        public static long? TryParseNullable(string value)
        {
            try { return ParseNullable(value, null); }
            catch (FormatException) { return null; }
        }

        public static long? TryParseNullable(string value, long? defaultValue)
        {
            try { return ParseNullable(value, defaultValue); }
            catch (FormatException) { return null; }
        }
    }
}

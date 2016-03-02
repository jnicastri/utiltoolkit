using System;

namespace UtilityToolkit.Extensions
{
    public static class Int32s
    {
        // Checks if the given string not null and is a number
        public static bool CheckIfNumeric(string sNumber)
        {
            bool IsNum = true;
            if (string.IsNullOrEmpty(sNumber))
            {
                IsNum = false;
            }
            else
            {
                for (int index = 0; index < sNumber.Length; index++)
                {
                    if (!Char.IsNumber(sNumber[index]))
                    {
                        IsNum = false;
                        break;
                    }
                }
            }
            return IsNum;
        }

        public static string PadZeros(this int value, int cols)
        {
            string formatted = value.ToString();
            while (formatted.Length < cols)
            {
                formatted = String.Format("0{0}", formatted);
            }
            return formatted;
        }

        public static int? ParseNullable(string value)
        {
            return ParseNullable(value, null);
        }

        public static int? ParseNullable(string value, int? defaultValue)
        {
            return String.IsNullOrEmpty(value) ? defaultValue : Int32.Parse(value);
        }

        public static int? TryParseNullable(string value)
        {
            try
            {
                return ParseNullable(value, null);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        public static int? TryParseNullable(string value, int? defaultValue)
        {
            try
            {
                return String.IsNullOrEmpty(value) ? defaultValue : Int32.Parse(value);
            }
            catch (Exception)
            {
                return defaultValue;
            }
        }
    }
}

using System;

namespace UtilityToolkit.Extensions
{
    public static class Bools
    {
        public const string Format_DefaultNo = "No";
        public const string Format_DefaultYes = "Yes";
        public const string Format_ShortNo = "N";
        public const string Format_ShortYes = "Y";

        public static string ToYesNoString(this bool value) //Extension method for bool
        {
            return (value) ? Format_DefaultYes : Format_DefaultNo;
        }

        public static string ToYNString(this bool value) //Extension method for bool
        {
            return (value) ? Format_ShortYes : Format_ShortNo;
        }

        public static bool ParseFromUserInput(string input)
        {
            bool? result = ParseNullableFromUserInput(input);
            if (!result.HasValue) throw new ArgumentOutOfRangeException("input", "was not a recognised boolean string.");
            return result.Value;
        }

        public static bool? ParseNullableFromUserInput(string input)
        {
            if (input == null) return null;
            switch (input.ToLower())
            {
                case "y":
                case "yes":
                case "t":
                case "true":
                    {
                        return true;
                    }
                case "n":
                case "no":
                case "f":
                case "false":
                    {
                        return false;
                    }
                default:
                    {
                        return null;
                    }
            }
        }
    }
}

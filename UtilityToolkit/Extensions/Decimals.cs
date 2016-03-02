using System;
using System.Text.RegularExpressions;

namespace UtilityToolkit.Extensions
{
    public static class Decimals
    {
        public const string Format_TwoDP = "F";
        public const string Format_PercNoDPNoConvert = "###0\\%";
        public const string Format_Currency = "C";
        public const string Format_NoDP = "N0";
        public const string Match_NumberWithExactlyTwoDP = @"^[+-]?(?:\d+\.?\d\d)[\r\n]*$";
        public const string Match_NumberWithMaxTwoDP = @"^[+-]?\d+\.?\d{0,2}$";

        private static readonly Regex regex_TwoDP = new Regex(Match_NumberWithExactlyTwoDP, RegexOptions.Compiled);
        private static readonly Regex regex_MaxTwoDP = new Regex(Match_NumberWithMaxTwoDP, RegexOptions.Compiled);

        public static string ToTwoDPString(this decimal value)
        {
            return value.ToString(Format_TwoDP);
        }

        public static string ToZeroDPString(this decimal value)
        {
            return value.ToString(Format_NoDP);
        }

        public static string ToPercNoDPNoConvert(this decimal value)
        {
            return value.ToString(Format_PercNoDPNoConvert);
        }

        public static string ToGlobalCurrencyString(this decimal value)
        {
            return value.ToString(Format_Currency);
        }

        /// <summary>
        /// Determines wether a string is properly formed currency string with exactly two decimal places. 
        /// Tolerates a currency symbol '$' at the start of the string. 
        /// Does not tolerate whitespace or any other non-digit characters.
        /// </summary>
        /// <param name="currency">The string to check.</param>
        /// <returns>True if 'currency' is a properly formed currency string.</returns>
        public static bool IsProperlyFormedCurrency(string currency)
        {
            return regex_TwoDP.IsMatch(currency.TrimStart('$'));
        }

        /// <summary>
        /// Determines wether a string is properly formed decimal string with exactly two decimal places. 
        /// Does NOT tolerate a currency symbol '$' at the start of the string. 
        /// Does not tolerate whitespace or any other non-digit characters.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if 'value' is a properly formed decimal string with exactly two decimal places.</returns>
        public static bool IsProperlyFormedTwoDP(string value)
        {
            return regex_TwoDP.IsMatch(value);
        }

        /// <summary>
        /// Determines wether a string is properly formed decimal string with a maximum of two decimal places. 
        /// Does NOT tolerate a currency symbol '$' at the start of the string. 
        /// Does not tolerate whitespace or any other non-digit characters.
        /// </summary>
        /// <param name="value">The string to check.</param>
        /// <returns>True if 'value' is a properly formed decimal string with a maximum of two decimal places.</returns>
        public static bool IsProperlyFormedMaxTwoDP(string value)
        {
            return regex_MaxTwoDP.IsMatch(value);
        }

        public static decimal? ParseCurrencyOrPercentage(string value)
        {
            return ParseCurrencyOrPercentage(value, null);
        }

        public static decimal? ParseCurrencyOrPercentage(string value, decimal? defaultValue)
        {
            return ParseNullable(value.Trim('%', '$'), defaultValue);
        }

        public static decimal? ParseNullable(string value)
        {
            return ParseNullable(value, null);
        }

        public static decimal? ParseNullable(string value, decimal? defaultValue)
        {
            return String.IsNullOrEmpty(value) ? defaultValue : decimal.Parse(value);
        }

        public static decimal? TryParseNullable(string value)
        {
            try { return ParseNullable(value, null); }
            catch (FormatException) { return null; }
        }

        public static decimal? TryParseNullable(string value, decimal? defaultValue)
        {
            try { return ParseNullable(value, defaultValue); }
            catch (FormatException) { return null; }
        }
    }
}

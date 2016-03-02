using System;
using System.Collections.Generic;

namespace UtilityToolkit.Extensions
{
    public static class Enums
    {
        //Name - refers to an unchanged enum 'names'
        //Label - refers to a readble version of enum 'names', seperated by camel case, e.g. CamelCase -> 'Camel Case'

        public static string[] GetLabels(Type enumType)
        {
            if (!enumType.IsEnum) throw new ArgumentException("must be an Enum", "enumType");
            return Strings.Transform(Enum.GetNames(enumType), Strings.SeperateCamelCase);
        }

        #region ToLabel

        public static string ToLabel<T>(T enumValue) where T : struct
        {
            return Strings.SeperateCamelCase(Enum.GetName(typeof(T), enumValue));
        }

        #endregion

        #region FromLabel

        public static T FromLabel<T>(string label) where T : struct
        {
            return (T)Enum.Parse(typeof(T), label.Replace(Strings.Space, String.Empty), true);
        }

        public static T FromLabel<T>(string label, bool ignoreCase) where T : struct
        {
            return (T)Enum.Parse(typeof(T), label.Replace(Strings.Space, String.Empty), ignoreCase);
        }

        #endregion

        #region ToName

        public static string ToName<T>(T enumValue) where T : struct
        {
            return Enum.GetName(typeof(T), enumValue);
        }

        #endregion

        #region FromName

        public static T FromName<T>(string name) where T : struct
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        public static T FromName<T>(string name, bool ignoreCase) where T : struct
        {
            return (T)Enum.Parse(typeof(T), name, ignoreCase);
        }

        #endregion

        public static class Nullable
        {
            #region ToLabel

            public static string ToLabel<T>(T? enumValue) where T : struct
            {
                return ToLabel(enumValue, String.Empty);
            }

            public static string ToLabel<T>(T? enumValue, string nullToken) where T : struct
            {
                return (!enumValue.HasValue) ? nullToken : Enums.ToLabel(enumValue.Value);
            }

            #endregion

            #region FromLabel

            public static T? FromLabel<T>(string label) where T : struct
            {
                return (!String.IsNullOrEmpty(label)) ? (T?)Enum.Parse(typeof(T), label.Replace(Strings.Space, String.Empty)) : null;
            }

            public static T? FromLabel<T>(string label, bool ignoreCase) where T : struct
            {
                return (!String.IsNullOrEmpty(label)) ? (T?)Enum.Parse(typeof(T), label.Replace(Strings.Space, String.Empty), ignoreCase) : null;
            }

            public static T? FromLabel<T>(string label, T? nullValue) where T : struct
            {
                return (!String.IsNullOrEmpty(label)) ? (T?)Enum.Parse(typeof(T), label.Replace(Strings.Space, String.Empty)) : nullValue;
            }

            public static T? FromLabel<T>(string label, bool ignoreCase, T? nullValue) where T : struct
            {
                return (!String.IsNullOrEmpty(label)) ? (T?)Enum.Parse(typeof(T), label.Replace(Strings.Space, String.Empty), ignoreCase) : nullValue;
            }

            public static T? TryFromLabel<T>(string label) where T : struct
            {
                try
                {
                    return FromLabel<T>(label);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }

            public static T? TryFromLabel<T>(string label, bool ignoreCase) where T : struct
            {
                try
                {
                    return FromLabel<T>(label, ignoreCase);
                }
                catch (ArgumentException)
                {
                    return null;
                }
            }

            public static T? TryFromLabel<T>(string label, T? nullValue) where T : struct
            {
                try
                {
                    return FromLabel(label, nullValue);
                }
                catch (ArgumentException)
                {
                    return nullValue;
                }
            }

            public static T? TryFromLabel<T>(string label, bool ignoreCase, T? nullValue) where T : struct
            {
                try
                {
                    return FromLabel(label, ignoreCase, nullValue);
                }
                catch (ArgumentException)
                {
                    return nullValue;
                }
            }

            #endregion

            #region ToName

            public static string ToName<T>(T? enumValue) where T : struct
            {
                return ToName(enumValue, String.Empty);
            }

            public static string ToName<T>(T? enumValue, string nullToken) where T : struct
            {
                return (!enumValue.HasValue) ? nullToken : Enums.ToName(enumValue.Value);
            }

            #endregion

            #region FromName

            public static T? FromName<T>(string Name) where T : struct
            {
                return (!String.IsNullOrEmpty(Name)) ? (T?)Enum.Parse(typeof(T), Name) : null;
            }

            public static T? FromName<T>(string Name, bool ignoreCase) where T : struct
            {
                return (!String.IsNullOrEmpty(Name)) ? (T?)Enum.Parse(typeof(T), Name, ignoreCase) : null;
            }

            public static T? FromName<T>(string Name, T? nullValue) where T : struct
            {
                return (!String.IsNullOrEmpty(Name)) ? (T?)Enum.Parse(typeof(T), Name) : nullValue;
            }

            public static T? FromName<T>(string Name, bool ignoreCase, T? nullValue) where T : struct
            {
                return (!String.IsNullOrEmpty(Name)) ? (T?)Enum.Parse(typeof(T), Name, ignoreCase) : nullValue;
            }

            #endregion
        }

        public static IEnumerable<T> AllValues<T>()
        {
            foreach (var value in Enum.GetValues(typeof(T)))
            {
                yield return (T)value;
            }
        }
    }
}

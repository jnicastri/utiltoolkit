using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilityToolkit.Extensions
{
    public static class Strings
    {
        public delegate string StringTransformationHandler<T>(T input);

        public const string Space = " ";
        public const string DefaultDelimiter = ",";
        public const string DefaultNullToken = "N/A";
        public const string EntireMatch = "$&";

        public const string EmailRegEx = "^((([^\\(@,\\\\\"\\[\\]<>;\\s\\.]+\\.)*[^\\(@,"
                    + "\\\\\"\\[\\]<>;\\s\\.]+)|(\\\"([^\"]+)\\\"))@((\\[[0-9]{1,3}\\."
                    + "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|((([a-z0-9]+[a-z0-9\\-]"
                    + "*[a-z0-9]+\\.)|([a-z0-9]+\\.))+([a-z]+[a-z\\-]*[a-z]+)))$";
        public const string PasswordRegEx = "^(?!.*\\s).{4,16}$";
        public const string FilenameRegEx = @"^[\w0-9&#228;&#196;&#246;&#214;&#252;&#220;&#223;\-_]+\.[a-zA-Z0-9]{2,6}$";
        public const string UrlUnSafePart = "[^a-zA-Z0-9_-]";
        public const string UrlUnSafePath = "[^a-zA-Z0-9_-/]";

        public static readonly char[] UpperCaseAlphabet = new[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public static readonly char[] LowerCaseAlphabet = new[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        /// <summary>
        /// Splits a string into groups of digits, matching sequential digits seperated by any character that is not a digit,
        /// e.g. '111;222,333 444\n555' matches: '111', '222', '333', '444', '555'.
        /// </summary>
        public static readonly Regex SplitDigitsRegex = new Regex(@"\d+", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Splits a string at Capitalised letters, e.g. 'CamelCaseString' matches: 'Camel', 'Case', and 'String'.
        /// </summary>
        public static readonly Regex SplitCamelCaseRegex = new Regex("[a-z]+|[A-Z][a-z]*", RegexOptions.Multiline | RegexOptions.Compiled);

        /// <summary>
        /// Returns an list of strings that has had the specified string transformation method applied to each element.
        /// </summary>
        /// <param name="transformation"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] Transform<T>(IEnumerable<T> input, StringTransformationHandler<T> transformation)
        {
            List<string> output = new List<string>();
            foreach (T obj in input)
            {
                output.Add(transformation(obj));
            }
            return output.ToArray();
        }

        public static string Join(IEnumerable collection)
        {
            return Join(DefaultDelimiter, collection);
        }

        public static string Join(string seperator, IEnumerable collection)
        {
            StringBuilder b = new StringBuilder();
            IEnumerator enumerator = collection.GetEnumerator();
            if (enumerator.MoveNext())
            {
                b.Append(enumerator.Current);
                while (enumerator.MoveNext())
                {
                    b.Append(seperator + enumerator.Current);
                }
            }
            return b.ToString().Trim();
        }

        public static string Join(params string[] values)
        {
            return Join(DefaultDelimiter, values);
        }

        public static string Join(string seperator, params string[] values)
        {
            return Join(seperator, (IEnumerable)values);
        }

        public static string Join(string keyValueSeperator, string pairSeperator, IDictionary dictionary)
        {
            StringBuilder b = new StringBuilder();
            IDictionaryEnumerator enumerator = dictionary.GetEnumerator();
            if (enumerator.MoveNext())
            {
                b.Append(string.Format("{0}{1}{2}", enumerator.Key, keyValueSeperator, enumerator.Value));
                while (enumerator.MoveNext())
                {
                    b.Append(string.Format("{0}{1}{2}{3}", pairSeperator, enumerator.Key, keyValueSeperator, enumerator.Value));
                }
            }
            return b.ToString().Trim();
        }

        /// <summary>
        /// Inserts spaces before capitalised letter within a string formatted in CamelCase discrete words.
        /// The return result is trimmed of whitespace at the start and end of the string.
        /// </summary>
        /// <param name="input">The string to seperate.</param>
        /// <returns>'input' with spaces inserted between capitalised words.</returns>
        public static string SeperateCamelCase(string input)
        {
            return SeperateCamelCase(input, Space);
        }

        /// <summary>
        /// Inserts the seperator specified before capitalised letter within a string formatted in CamelCase discrete words.
        /// The return result is trimmed of whitespace at the start and end of the string.
        /// </summary>
        /// <param name="input">The string to seperate.</param>
        /// <param name="seperator">The seperator to insert between capitalised words.</param>
        /// <returns>'input' with 'seperator' inserted between capitalised words.</returns>
        public static string SeperateCamelCase(string input, string seperator)
        {
            return SplitCamelCaseRegex.Replace(input, seperator + EntireMatch).Trim();
        }

        public static string[] SplitCamelCase(string input)
        {
            return GetArrayOfMatches(SplitCamelCaseRegex, input);
        }

        public static string[] SplitDigits(string input)
        {
            return GetArrayOfMatches(SplitDigitsRegex, input);
        }

        public static string[] GetArrayOfMatches(Regex regex, string input)
        {
            MatchCollection matches = regex.Matches(input);
            string[] output = new string[matches.Count];
            int i = 0;
            foreach (Match match in matches)
            {
                output[i++] = match.Value;
            }
            return output;
        }

        public static string GetNewGuidTimestampId()
        {
            Guid guid = Guid.NewGuid();
            string guidTimestampId = guid.ToString().Substring(0, 4).ToUpper() + "-" + DateTime.Now.ToString("HHmmssyyMMdd");
            return guidTimestampId; //17 characters
        }

        /// <summary>
        /// Overloaded method to split string into a list of trimmed strings using a comma
        /// </summary>
        /// <param name="val">String to split</param>
        /// <returns></returns>
        public static List<string> SplitToList(string val)
        {
            return SplitToList(val, new char[] { char.Parse(",") });
        }

        /// <summary>
        /// Splits a delimited string into a list of trimmed strings
        /// </summary>
        /// <param name="val">string to split</param>
        /// <param name="delim">Delimiter to split on</param>
        /// <returns></returns>
        public static List<string> SplitToList(string val, params char[] delim)
        {
            List<string> thisList = new List<string>();
            if (val != null && val.Length > 0)
            {
                string[] tmp = val.Split(delim);
                for (int i = 0; i < tmp.Length; i++)
                {
                    thisList.Add(tmp[i].Trim());
                }
            }
            return thisList;
        }

        #region Validation

        public static bool IsValidCreditCard(string val)
        {
            if (val == null) return false;
            //Regex reg = new Regex(@"^[^~`!@#$%^*()_+={\[}\]|\\:;""'<,>.?/&]+$");
            //return reg.IsMatch(val);
            return true;
        }

        /// <summary>
        /// Checks that a string is only alphanumeric, may contain spaces or the character '-',
        /// for hyphenated names, eg Charles-Williams.
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidHumanName(string val)
        {
            if (val == null) return false;
            Regex reg = new Regex(@"^[^~`!@#$%^*()_+={\[}\]|\\:;""'<,>.?/&]+$");
            return reg.IsMatch(val);
        }

        /// <summary>
        /// Checks that a string is not null and alpha chars only
        /// </summary>
        /// <param name="val">Input string</param>
        /// <returns></returns>
        public static bool IsAlphabetical(string val)
        {
            if (val == null) return false;
            Regex reg = new Regex(@"^[a-zA-Z]+$");
            return reg.IsMatch(val);
        }

        /// <summary>
        /// Checks a string is not null and alpha numeric characters only.
        /// Will not accept decimal values
        /// </summary>
        /// <param name="val">String to check</param>
        /// <returns></returns>
        public static bool IsAlphaNumeric(string val)
        {
            if (val == null) return false;
            Regex reg = new Regex(@"^[a-zA-Z0-9]");
            return reg.IsMatch(val);
        }

        /// <summary>
        /// Checks a string is not null and numeric characters. Will accept
        /// positive or negative floating point values
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsNumeric(string val)
        {
            if (string.IsNullOrEmpty(val)) return false;
            Regex reg = new Regex(@"^[0-9]*$");
            return reg.IsMatch(val);
        }

        public static bool IsValidDate(string val)
        {
            if (val == null) return false;
            Regex reTestDate = new Regex("([1-9]|0[1-9]|[12][0-9]|3[01])[- /.]([1-9]|0[1-9]|1[012])[- /.][0-9]{4}$");
            return reTestDate.IsMatch(val);
        }

        /// <summary>
        /// Checks if a string is a valid email address. (Regex only. Does not check domain.)
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string val)
        {
            if (val == null)
            {
                return false;
            }
            else
            {
                Regex reTestEmail = new Regex(Strings.EmailRegEx);

                return reTestEmail.IsMatch(val);
            }
        }

        public static bool IsValidPassword(string val)
        {
            if (val == null)
            {
                return false;
            }
            else
            {
                Regex reTest = new Regex(Strings.PasswordRegEx);

                return reTest.IsMatch(val);
            }
        }


        public static int IsValidEmailList(string val)
        {
            int iErrors = 0;

            char[] sarDelim = new char[] { ';', ',' };

            string[] sarEmailList = val.Split(sarDelim);

            for (int i = 0; i < sarEmailList.Length; i++)
            {
                if (!IsValidEmail(sarEmailList[i].Trim()))
                    iErrors++;
            }

            return iErrors;
        }

        public static bool IsValidDocument(string filename)
        {
            string f = filename.ToLower();
            if (String.IsNullOrEmpty(filename))
            {
                return false;
            }
            if (f.EndsWith(".doc") || f.EndsWith(".pdf") || f.EndsWith(".docx") || f.EndsWith(".txt"))
            {
                return true;
            }
            else
                return false;
        }

        public static bool IsValidImage(string filename)
        {
            string f = filename.ToLower();
            if (String.IsNullOrEmpty(filename))
            {
                return false;
            }
            if (f.EndsWith(".jpg") || f.EndsWith(".gif") || f.EndsWith(".png"))
            {
                return true;
            }
            else
                return false;
        }

        public static bool IsValidAudio(string filename)
        {
            string f = filename.ToLower();
            if (String.IsNullOrEmpty(filename))
            {
                return false;
            }
            if (f.EndsWith(".mp3"))
            {
                return true;
            }
            else
                return false;
        }

        #endregion

        #region String editing

        public static string GetUrlSafePart(string text)
        {
            Regex re = new Regex(UrlUnSafePart);
            text = text.Replace(" ", "_");
            return re.Replace(text, "");
        }

        public static string GetUrlSafePath(string text)
        {
            Regex re = new Regex(UrlUnSafePath);
            return re.Replace(text, "");
        }

        public static bool IsFilename(string text)
        {
            Regex re = new Regex(FilenameRegEx);
            return re.IsMatch(text);
        }

        public static string Shorten(string text, int length)
        {
            if ((text ?? "").Length > length)
            {
                return text.Substring(0, length) + "...";
            }
            else
                return text ?? "";
        }

        public static string Wrap(string text, int maxLength)
        {
            text = text.Replace("\n", "<br>");
            text = text.Replace("\r", " ");
            text = text.Replace(".", ". ");
            text = text.Replace(">", "> ");
            text = text.Replace("\t", " ");
            text = text.Replace(",", ", ");
            text = text.Replace(";", "; ");
            text = text.Replace(" ", " ");

            string[] Words = text.Split(' ');
            int currentLineLength = 0;
            string textLinesStr = "";
            string currentLine = "";
            bool InTag = false;

            foreach (string currentWord in Words)
            {
                //ignore html
                if (currentWord.Length > 0)
                {

                    if (currentWord.Substring(0, 1) == "<")
                        InTag = true;

                    if (InTag)
                    {
                        //handle filenames inside html tags
                        if (currentLine.EndsWith("."))
                        {
                            currentLine += currentWord;
                        }
                        else
                            currentLine += " " + currentWord;

                        if (currentWord.IndexOf(">") > -1)
                            InTag = false;
                    }
                    else
                    {
                        if (currentLineLength + currentWord.Length + 1 < maxLength)
                        {
                            currentLine += " " + currentWord;
                            currentLineLength += (currentWord.Length + 1);
                        }
                        else
                        {
                            textLinesStr += " " + currentLine;
                            currentLine = currentWord;
                            currentLineLength = currentWord.Length;
                        }
                    }
                }

            }
            if (currentLine != "")
                textLinesStr += " " + currentLine;

            return textLinesStr;
        }

        private static readonly Regex WhitespaceRegex = new Regex(@"[\s]+", RegexOptions.Compiled);
        private static readonly Regex InvalidsRegex = new Regex(@"[^\w]", RegexOptions.Compiled);
        public static string GetTruncatedFilename(string filename, string fileExtension)
        {
            filename = WhitespaceRegex.Replace(filename, "");
            filename = InvalidsRegex.Replace(filename, "");

            if (filename.Length > 40)
                filename = filename.Substring(0, 40);

            filename = filename + fileExtension;

            return filename;
        }

        public static string Mailto(string email)
        {
            return String.Format("mailto:{0}", email);
        }

        public static string Pad(string left, string right, int overallLength)
        {
            int paddingLength = overallLength - left.Length - right.Length;
            return left + Spaces(paddingLength) + right;
        }

        public static string Spaces(int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                builder.Append(Space);
            }
            return builder.ToString();
        }

        #endregion

        #region ToString extenstions

        /// <summary>
        /// Returns the string represenation of the object specified or 'DefaultNullToken' if it is null.
        /// </summary>
        /// <param name="nullable"></param>
        /// <returns></returns>
        public static string ToString(this object nullable)
        {
            return ToString(nullable, DefaultNullToken);
        }

        /// <summary>
        /// Returns the string representation of the object specified or the value specified if it is null.
        /// </summary>
        /// <param name="nullable"></param>
        /// <param name="nullToken"></param>
        /// <returns></returns>
        public static string ToString(this object nullable, string nullToken)
        {
            return (nullable == null) ? nullToken : nullable.ToString();
        }

        public static string Remove(string value, params string[] stringsToRemove)
        {
            foreach (string toRemove in stringsToRemove)
            {
                value = Regex.Replace(value, toRemove, String.Empty);
            }
            return value;
        }

        public static string GetErrorCodeString()
        {
            Guid guid = Guid.NewGuid();
            string errorCode = guid.ToString().Substring(0, 4).ToUpper() + "-" + DateTime.Now.ToString("HHmmssfffyyMMdd");

            return errorCode;
        }

        #endregion
    }
}


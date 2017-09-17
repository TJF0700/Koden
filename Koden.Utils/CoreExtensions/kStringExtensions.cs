#region License
// Copyright (c) 2014 Tim Fischer
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the 'Software'), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED 'AS IS', WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Koden.Utils.Extensions
{
    public static partial class ExtensionMethods
    {
        
        /// <summary>
        /// Replace string.Format() with .With(params[]).
        /// </summary>
        /// <param name="s">The string with formatting codes.</param>
        /// <param name="args">The arguments to replace the format.</param>
        /// <returns>Formatted String</returns>
        public static string kWith(this string s, params object[] args)
        {
            return string.Format(s, args);
        }
        /// <summary>
        /// To the int16.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Int16"/> parsed value or default</returns>
        public static Int16 kToInt16WithDefault(this string valueToParse, Int16 defaultValue)
        {
            Int16 returnValue;
            if (!Int16.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the int32.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Int32"/> parsed value or default</returns>
        public static Int32 kToInt32WithDefault(this string valueToParse, Int32 defaultValue)
        {
            Int32 returnValue;
            if (!Int32.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the int64.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Int64"/> parsed value or default</returns>
        public static Int64 kToInt64WithDefault(this string valueToParse, Int64 defaultValue)
        {
            Int64 returnValue;
            if (!Int64.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the u int16.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="UInt16"/> parsed value or default</returns>
        public static UInt16 kToUInt16WithDefault(this string valueToParse, UInt16 defaultValue)
        {
            UInt16 returnValue;
            if (!UInt16.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the u int32.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="UInt32"/> parsed value or default</returns>
        public static UInt32 kToUInt32WithDefault(this string valueToParse, UInt32 defaultValue)
        {
            UInt32 returnValue;
            if (!UInt32.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the u int64.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="UInt64"/> parsed value or default</returns>
        public static UInt64 kToUInt64WithDefault(this string valueToParse, UInt64 defaultValue)
        {
            UInt64 returnValue;
            if (!UInt64.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// Single or default
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Single"/> parsed value or default</returns>
        public static Single kToSingleWithDefault(this string valueToParse, Single defaultValue)
        {
            Single returnValue;
            if (!Single.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// Double to Default
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Double"/> parsed value or default</returns>
        public static Double kToDoubleWithDefault(this string valueToParse, Double defaultValue)
        {
            Double returnValue;
            if (!Double.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the decimal.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Decimal"/> parsed value or default</returns>
        public static Decimal kToDecimalWithDefault(this string valueToParse, Decimal defaultValue)
        {
            Decimal returnValue;
            if (!Decimal.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To the byte.
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="Byte"/> parsed value or default</returns>
        public static Byte kToByteWithDefault(this string valueToParse, Byte defaultValue)
        {
            Byte returnValue;
            if (!Byte.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }

        /// <summary>
        /// To an SByte with default on fail
        /// </summary>
        /// <param name="valueToParse">The value to parse.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns><see cref="SByte"/> parsed value or default</returns>
        public static SByte kToSByteWithDefault(this string valueToParse, SByte defaultValue)
        {
            SByte returnValue;
            if (!SByte.TryParse(valueToParse, out returnValue))
                returnValue = defaultValue;
            return returnValue;
        }
        /// <summary>
        /// Occurenceses the specified substring within a string (FAST).
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="val">The substring to find.</param>
        /// <returns><see cref="int"/> of occurrences of substring</returns>
        public static int kOccurences(this string str, string val)
        {
            int occurrences = 0;
            int startingIndex = 0;

            while ((startingIndex = str.IndexOf(val, startingIndex)) >= 0)
            {
                ++occurrences;
                ++startingIndex;
            }

            return occurrences;
        }

        /// <summary>
        /// Splices the specified string.
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns><see cref="string"/> with spliced data</returns>
        public static string kSplice(this string str, int start, int length,
            string replacement)
        {
            return str.Substring(0, start) +
                   replacement +
                   str.Substring(start + length);
        }

        /// <summary>
        /// Stuffs the specified string.
        /// </summary>
        /// <param name="str">The original string.</param>
        /// <param name="start">The start.</param>
        /// <param name="length">The length.</param>
        /// <param name="replacement">The replacement.</param>
        /// <returns><see cref="string"/> with stuffed data</returns>
        public static string kStuff(this string str, int start, int length, string replacement)
        {
            return str.Substring(0, start) +
                   replacement +
                   str.Substring(start + length);
        }

        ///// <summary>
        ///// Determines whether [contains] [the specified original].
        ///// </summary>
        ///// <param name="original">The original.</param>
        ///// <param name="value">The value.</param>
        ///// <param name="comparisionType">Type of the comparision.</param>
        ///// <returns></returns>
        //public static bool Contains(this string original, string value, StringComparison comparisionType)
        //{
        //    if (String.IsNullOrEmpty(value) || String.IsNullOrEmpty(original)) return false;
        //    return original.IndexOf(value, comparisionType) >= 0;
        //}

        /// <summary>
        /// Removes/fixes single quotes from strings used in SQL Queries.
        /// </summary>
        /// <param name="original">The original.</param>
        /// <returns><see cref="string"/> with double '' replacements</returns>
        public static string kToSafeSQL(this string original)
        {
            return String.IsNullOrEmpty(original) ? "" : original.Replace("'", "''");
        }

        /// <summary>
        /// Pads the center of a string.
        /// </summary>
        /// <param name="s">The sring for the center.</param>
        /// <param name="width">The total width of final string.</param>
        /// <param name="c">The character to pad with.</param>
        /// <returns><see cref="string"/> with center padded to max width</returns>
        public static string kPadCenter(this string s, int width, char c)
        {
            if (s == null || width <= s.Length) return s;

            int padding = width - s.Length;
            return s.PadLeft(s.Length + padding / 2, c).PadRight(width, c);
        }

        /// <summary>
        /// Removes the HTML tags from a string.
        /// </summary>
        /// <param name="original">The original string.</param>
        /// <returns><see cref="string"/> with HTML tags removed</returns>
        public static string kRemoveHTML(this string original)
        {
            var step1 = Regex.Replace(original, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        /// <summary>
        /// Determines whether the specified the value is numeric.
        /// </summary>
        /// <param name="theValue">The value.</param>
        /// <returns><c>True</c> if string is numeric, else <c>false</c></returns>
        public static bool kIsNumeric(this string theValue)
        {
            long retNum;
            return long.TryParse(theValue, System.Globalization.NumberStyles.Integer, System.Globalization.NumberFormatInfo.InvariantInfo, out retNum);
        }

        /// <summary>
        /// Truncates a string to maxLength.  Returns string if less than maxlength or null
        /// </summary>
        /// <param name="source"></param>
        /// <param name="maxLength"></param>
        /// <returns> Returns string if less than maxlength or null</returns>
        public static string kTruncate(this string source, int maxLength)
        {
            return source?.Substring(0, Math.Min(source.Length, maxLength));

        }

        /// <summary>
        /// compare strings ignoring case.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <param name="useOrdinalComparison">if set to <c>true</c> [use ordinal comparison].</param>
        /// <returns><c>True</c> if string equals other, else <c>false</c></returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static bool kEqualsIgnoreCase(this string source, string other, bool useOrdinalComparison = true)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            return string.Equals(source, other, useOrdinalComparison ? StringComparison.OrdinalIgnoreCase : StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether string [contains] [the specified value].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// source
        /// or
        /// value
        /// </exception>
        public static bool kContains(this string source, string value, StringComparison comparisonType)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            // .NET string functions consider an empty string as always contained into another string, so only null values are rejected
            if (value == null) throw new ArgumentNullException(nameof(value));

            return source.IndexOf(value, comparisonType) > -1;
        }

        /// <summary>
        /// Determines whether string [contains] [the specified other].
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <param name="options">The options.</param>
        /// <param name="culture">The culture.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified other]; otherwise, <c>false</c>.
        /// </returns>
        public static bool kContains(this string source, string other, CompareOptions options, CultureInfo culture = null)
            => kIndexOf(source, other, options, culture) > -1;

        /// <summary>
        /// checks for index of specified string - case insenstive.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <param name="options">The options.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static int kIndexOf(this string source, string other, CompareOptions options, CultureInfo culture = null)
            => (culture ?? CultureInfo.CurrentCulture).CompareInfo.IndexOf(source, other, options);

        /// <summary>
        /// checks for Last index of specified string - case insenstive.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <param name="options">The options.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static int kLastIndexOf(this string source, string other, CompareOptions options, CultureInfo culture = null)
            => (culture ?? CultureInfo.CurrentCulture).CompareInfo.LastIndexOf(source, other, options);

        /// <summary>
        /// Check if strings are same.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <param name="options">The options.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static bool kEqualsTo(this string source, string other, CompareOptions options, CultureInfo culture = null)
            => kCompareTo(source, other, options, culture) == 0;

        /// <summary>
        /// Compares to.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="other">The other.</param>
        /// <param name="options">The options.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static int kCompareTo(this string source, string other, CompareOptions options, CultureInfo culture = null)
            => (culture ?? CultureInfo.CurrentCulture).CompareInfo.Compare(source, other, options);

        /// <summary>
        /// Removes the diacritics and recompose.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fullCompatibilityDecomposition">if set to <c>true</c> [full compatibility decomposition].</param>
        /// <param name="customConvert">The custom convert.</param>
        /// <returns></returns>
        public static string kRemoveDiacriticsAndRecompose(this string source, bool fullCompatibilityDecomposition = false, Func<char, char> customConvert = null)
        {
            return new string(kRemoveDiacritics(source, fullCompatibilityDecomposition, customConvert).ToArray())
                .Normalize(NormalizationForm.FormC);
        }

        // 
        /// <summary>
        /// Removes the diacritics.
        /// based on http://stackoverflow.com/a/3769995/852829
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="fullCompatibilityDecomposition">if set to <c>true</c> [full compatibility decomposition].</param>
        /// <param name="customConvert">The custom convert.</param>
        /// <returns>String with removed diacritics</returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static IEnumerable<char> kRemoveDiacritics(this string source, bool fullCompatibilityDecomposition = false, Func<char, char> customConvert = null)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            if (source.Length <= 0)
                return source;
            else
            {
                var output = source
                    .Normalize(fullCompatibilityDecomposition ? NormalizationForm.FormKD : NormalizationForm.FormD)
                    .Where(
                        c =>
                        {
                            switch (CharUnicodeInfo.GetUnicodeCategory(c))
                            {
                                case UnicodeCategory.NonSpacingMark:
                                case UnicodeCategory.SpacingCombiningMark:
                                case UnicodeCategory.EnclosingMark:
                                    return false;
                                default:
                                    return true;
                            }
                        });

                if (customConvert != null)
                    output = output.Select(c => customConvert(c));

                return output;
            }
        }


        #region Convert Word to ProperCase
        /// <summary>Return Converted Word</summary>
        /// <param name="theWord">Word to Convert</param>
        /// <returns>Converted Word</returns>
        public static string kProperWord(this string theWord)
        {
            // Shouldn't be null, but check anyway
            theWord = NoNull(theWord);

            // Skip zero length or any MIXED Case Words on the Assumption that 
            // Mixed Case Words could be right.
            if (theWord.Length > 0 && (theWord == theWord.ToUpper() || theWord == theWord.ToLower()))
            {
                // Start by Checking for Words that shouldn't be changed.
                foreach (var skip in NoChangeWords)
                    if (string.Compare(theWord, skip, true) == 0) return skip;

                // Handle Single Character Words that were no in Skip Section
                if (theWord.Length == 1) { theWord = theWord.ToUpper(); return theWord; }

                // If Word not in List of Words not To Change, CAP first Character
                theWord = theWord.Substring(0, 1).ToUpper() + theWord.Substring(1).ToLower();

                // Check for the SpecialWords 
                theWord = CheckSpecialWords(theWord);

                // Check for Hyphenated Words
                theWord = ChkHyphen(theWord);

            }
            return theWord;
        }
        #endregion
        #region Convert Sentence to ProperCase
        /// <summary>
        /// Convert Sentence to ProperCase
        /// </summary>
        /// <param name="sentence">Sentence to Convert</param>
        /// <returns>Proper Case Sentence</returns>
        public static string kProperSentence(this string sentence)
        {
            sentence = NoNull(sentence);

            var sb = new StringBuilder();
            var words = sentence.Split(' ');


            var i = 0;
            foreach (var word in words)
            {
                // word is Readonly so use a copy of word 
                var theWord = word;
                // Store any Ending Puncutation Characters
                var endChar = string.Empty;
                if (theWord.Length > 1 && Char.IsPunctuation(theWord[theWord.Length - 1]))
                {
                    endChar = theWord.Substring(theWord.Length - 1, 1);
                    theWord = theWord.Substring(0, theWord.Length - 1);
                }
                // Cap an Starting Word like 'a' 
                if (theWord.Length == 1 && i == 0) sb.Append(word.ToUpper());
                else if (i > 0 && IsSkipWord(theWord)) sb.Append(theWord);
                else sb.Append(kProperWord(theWord));
                sb.Append(endChar);
                sb.Append(" ");  // Add back the space
                ++i;      // Increment after 1st word
            }
            return sb.ToString().Trim(); //Trim and Return
        }
        #endregion
    }
}

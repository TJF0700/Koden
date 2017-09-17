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
using System.Text;

namespace Koden.Utils.Extensions
{

    public static partial class ExtensionMethods
    {

        #region Private Class Variables
        /// <summary>Do Not Change these Words</summary>
        private static readonly string[] SkipWords = { "a", "at", "an", "and", "the", "or", "of", "to" };
        /// <summary>Look for Special Words like McDonald, MacDonald or O'Riely</summary>
        private static readonly string[] SpecialWords = { "Mac", "Mc", "O'", "D'" };
        /// <summary>Do Not Change these Words</summary>
        private static readonly string[] NoChangeWords = { "USA", "NSAD", "DOA", "DET", "DSF" };
        #endregion
        #region Private Functions
        /// <summary>
        /// Looks for words that shouldn't be capitalized
        /// They will be skipped if they don't start the sentence. 
        /// </summary>
        /// <param name="theWord">Word to Check</param>
        /// <returns><i>True</i>if a SkipWord</returns>
        private static bool IsSkipWord(IEquatable<string> theWord)
        {
            foreach (var skip in SkipWords) if (theWord.Equals(skip)) return true;
            return false;
        }

        /// <summary>
        /// Looks for Special Situations defined in SpecialWords
        /// above like McDonald or MacDonald
        /// </summary>
        /// <param name="theWord">Word to Check</param>
        /// <returns>Checked Word</returns>
        private static string CheckSpecialWords(string theWord)
        {
            foreach (var sw in SpecialWords)
                if (theWord.StartsWith(sw) && theWord.Length > sw.Length)
                {
                    theWord = theWord.Substring(0, sw.Length)
                      + Char.ToUpper(theWord[sw.Length])
                      + (theWord.Length > sw.Length + 1 ? theWord.Substring(sw.Length + 1) : "");
                }
            return theWord;

        }

        /// <summary>
        /// Checks for a hyphen.
        /// </summary>
        /// <param name="theWord">The word.</param>
        /// <returns><see cref="string"/> with word split at hyphen</returns>
        private static string ChkHyphen(string theWord)
        {
            var sb = new StringBuilder();
            var words = theWord.Split('-');
            var i = words.GetUpperBound(0);
            if (i == 0) return theWord;
            foreach (var word in words)
            {
                sb.Append(kProperWord(word));
                if (i > 0) sb.Append("-");
                --i;
            }
            return sb.ToString();
        }

        /// <summary>We never want a null string</summary>
        /// <param name="theWord">word or sentence</param>
        /// <returns>Checked word or Sentence</returns>
        private static string NoNull(string theWord)
        {
            return String.IsNullOrEmpty(theWord) ? "" : theWord.Trim();
        }

        #endregion
 
    }
}

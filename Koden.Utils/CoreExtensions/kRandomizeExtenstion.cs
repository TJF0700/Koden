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
using Koden.Utils.Models;
using System;
using System.Linq;

namespace Koden.Utils.Extensions
{
    public static partial class ExtensionMethods
    {

        /// <summary>
        /// Places a Caesar shift cipher on a string of a specified number of shifts.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="shift">The number to shift.</param>
        /// <returns>Caesar ciphered string</returns>
        public static string kCaesar(this string source, Int16 shift)
        {
            var maxChar = Convert.ToInt32(char.MaxValue);
            var minChar = Convert.ToInt32(char.MinValue);

            var buffer = source.ToCharArray();

            for (var i = 0; i < buffer.Length; i++)
            {
                var shifted = Convert.ToInt32(buffer[i]) + shift;

                if (shifted > maxChar)
                {
                    shifted -= maxChar;
                }
                else if (shifted < minChar)
                {
                    shifted += maxChar;
                }

                buffer[i] = Convert.ToChar(shifted);
            }

            return new string(buffer);
        }



        /// <summary>
        /// Randomizes the specified source. - used for test data, among other options
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="rType">Type of the randomization (number, text, both, etc).</param>
        /// <param name="seed">The seed.</param>
        /// <returns><see cref="string"/> representing Randomized string</returns>
        public static string kRandomize(this string source, RandomizeType rType, int seed)
        {
            var Size = source.Length;
            string myChar = "AaBbCc DdEeFfGgHhIiJjKkLl MmNnOoPpQqRrSsTt UuVvWwXxYyZz";
            string myNum = "1234567890";
            string finalString = "";
            var rnd = new Random(seed);

            switch (rType)
            {
                case RandomizeType.NumbersOnly:
                    finalString = myNum;
                    break;
                case RandomizeType.TextOnly:
                    finalString = myChar;
                    break;
                case RandomizeType.Either:
                    finalString = myChar + myNum;
                    break;
                default:
                    break;
            }

            var chars = Enumerable.Range(0, Size)
                                        .Select(x => finalString[rnd.Next(0, finalString.Length)]);
            return new string(chars.ToArray());
        }

    }
}

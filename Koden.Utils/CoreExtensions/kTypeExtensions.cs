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
using System.Linq;

namespace Koden.Utils.Extensions
{
    public static partial class ExtensionMethods

    {
        /// <summary>
        /// Throws an ArgumentNullException if the given data item is null.
        /// </summary>
        /// <param name="data">The item to check for nullity.</param>
        /// <param name="name">The name to use when throwing an exception, if necessary</param>
        public static void kThrowIfNull<T>(this T data, string name) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        /// <summary>
        /// Throws an ArgumentNullException if the given data item is null.
        /// No parameter name is specified.
        /// </summary>
        /// <param name="data">The item to check for nullity.</param>
        public static void kThrowIfNull<T>(this T data) where T : class
        {
            if (data == null)
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Coins the toss.
        /// </summary>
        /// <param name="rng">The RNG.</param>
        /// <returns>true if "heads" false if "tails"... in effect</returns>
        public static bool kCoinToss(this Random rng)
        {
            return rng.NextDouble() < .5;
        }

        /// <summary>
        /// Returns a random entry from an array of strings/numebers.
        /// </summary>
        /// <typeparam name="T">number, string, type</typeparam>
        /// <param name="rng">The random number.</param>
        /// <param name="items">The items to choose from.</param>
        /// <returns>Random generic of any Type in a list of arguments</returns>
        public static T kOneOf<T>(this Random rng, params T[] items)
        {
            return items[rng.Next(items.Length)];
        }

        /// <summary>
        /// Determines whether this is an empty guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns><c>True</c> if LGUID is empty, else <c>false</c></returns>
        public static bool kIsEmpty(this Guid guid)
        {
            if (guid == Guid.Empty)
                return true;
            return false;
        }

        /// <summary>
        /// Determines whether the specified variable is contained within the "list".
        /// </summary>
        /// <example>
        /// if(longVariable.IsIn(1,6,9,11))...
        /// if(stringVariable.IsIn("One","Two","Three"))...
        /// </example>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <param name="list">The list.</param>
        /// <returns>
        ///   <c>true</c> if the specified variable is in the list; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">source</exception>
        public static bool kIsIn<T>(this T source, params T[] list)
        {
            if (null == source) throw new ArgumentNullException("source");
            return list.Contains(source);
        }

        /// <summary>
        /// Creates an inclusive range between two values. The default comparer is used
        /// to compare values.
        /// </summary>
        /// <typeparam name="T">Type of the values</typeparam>
        /// <param name="start">Start of range.</param>
        /// <param name="end">End of range.</param>
        /// <returns>An inclusive range between the start point and the end point.</returns>
        public static Range<T> kTo<T>(this T start, T end)
        {
            return new Range<T>(start, end);
        }

        /// <summary>
        /// Returns a RangeIterator over the given range, where the stepping function
        /// is to step by the given number of characters.
        /// </summary>
        /// <param name="range">The range to create an iterator for</param>
        /// <param name="step">How many characters to step each time</param>
        /// <returns>A RangeIterator with a suitable stepping function</returns>
        public static RangeIterator<char> kStepChar(this Range<char> range, int step)
        {
            return range.Step(c => (char)(c + step));
        }
    }
}

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

namespace Koden.Utils.Extensions
{
    /// <summary>
    /// Extensions to IComparer
    /// </summary>
    public static class ComparerExt
    {
        /// <summary>
        /// Reverses the original comparer; if it was already a reverse comparer,
        /// the previous version was reversed (rather than reversing twice).
        /// In other words, for any comparer X, X==X.Reverse().Reverse().
        /// </summary>
        public static IComparer<T> kReverse<T>(this IComparer<T> original)
        {
            ReverseComparer<T> originalAsReverse = original as ReverseComparer<T>;
            if (originalAsReverse != null)
            {
                return originalAsReverse.OriginalComparer;
            }
            return new ReverseComparer<T>(original);
        }

        /// <summary>
        /// Combines a comparer with a second comparer to implement composite sort
        /// behaviour.
        /// </summary>
        public static IComparer<T> kThenBy<T>(this IComparer<T> firstComparer, IComparer<T> secondComparer)
        {
            return new LinkedComparer<T>(firstComparer, secondComparer);
        }

        /// <summary>
        /// Combines a comparer with a projection to implement composite sort behaviour.
        /// </summary>
        public static IComparer<T> kThenBy<T, TKey>(this IComparer<T> firstComparer, Func<T, TKey> projection)
        {
            return new LinkedComparer<T>(firstComparer, new ProjectionComparer<T, TKey>(projection));
        }
    }
}

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
using Koden.Utils.Extensions;
using System.Collections.Generic;

namespace Koden.Utils
{
    /// <summary>
    /// Comparer to daisy-chain two existing comparers and 
    /// apply in sequence (i.e. sort by x then y)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class LinkedComparer<T> : IComparer<T>
    {
        readonly IComparer<T> primary, secondary;
        /// <summary>
        /// Create a new LinkedComparer
        /// </summary>
        /// <param name="primary">The first comparison to use</param>
        /// <param name="secondary">The next level of comparison if the primary returns 0 (equivalent)</param>
        public LinkedComparer(
            IComparer<T> primary,
            IComparer<T> secondary)
        {
            primary.kThrowIfNull("primary");
            secondary.kThrowIfNull("secondary");
            
            this.primary = primary;
            this.secondary = secondary;
        }

        int IComparer<T>.Compare(T x, T y)
        {
            int result = primary.Compare(x, y);
            return result == 0 ? secondary.Compare(x, y) : result;
        }
    }
}

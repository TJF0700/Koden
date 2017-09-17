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
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Koden.Utils.Extensions
{
    /// <summary>
    /// Adds Default Value if no key exists. 
    /// </summary>
    public static partial class ExtensionMethods
    {

        /// <summary>
        /// Gets the value or default if key doesn't exist.
        /// </summary>
        /// <typeparam name="TKey">The type of the T key.</typeparam>
        /// <typeparam name="TValue">The type of the T value.</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key.</param>
        /// <param name="defRet">The definition ret.</param>
        /// <returns>Dictionary Value</returns>
        public static TValue kGetValueOrDefault<TKey, TValue> (this IDictionary<TKey, TValue> dictionary, TKey key, TValue defRet)
        {
            dictionary.TryGetValue(key, out TValue ret);
            return (ret == null) ? defRet :  ret;
        }
    }
}

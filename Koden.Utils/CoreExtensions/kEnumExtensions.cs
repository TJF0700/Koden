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

        /// <summary>
        /// Determines whether the specified value has flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value has flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool kHasFlag<T>(this System.Enum type, T value)
        {
            try
            {
                return (((int)(object)type & (int)(object)value) == (int)(object)value);
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Determines whether the specified value is flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        ///   <c>true</c> if the specified value is flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool kIsFlag<T>(this System.Enum type, T value)
        {
            try
            {
                return (int)(object)type == (int)(object)value;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Adds the flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>Or'd Flags</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static T kAddFlag<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type | (int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not append value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
        /// <summary>
        /// Removes the flag.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="type">The type.</param>
        /// <param name="value">The value.</param>
        /// <returns>And'd Flafs</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static T kRemoveFlag<T>(this System.Enum type, T value)
        {
            try
            {
                return (T)(object)(((int)(object)type & ~(int)(object)value));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(
                    string.Format(
                        "Could not remove value from enumerated type '{0}'.",
                        typeof(T).Name
                        ), ex);
            }
        }
        #region long extensions
        /// <summary>
        /// Determines whether the specified current long (bitwise) has flag (&amp; Logic).
        /// </summary>
        /// <param name="currentVal">The current value.</param>
        /// <param name="checkVal">The checked value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified current val has flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool kHasFlag(this long currentVal, long checkVal)
        {
            return ((currentVal & checkVal) == checkVal);
        }

        /// <summary>
        /// Determines whether the specified current long (bitwise) is the same as flag (&amp; Logic).
        /// </summary>
        /// <param name="currentVal">The current value.</param>
        /// <param name="checkVal">The checked value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified current val is flag; otherwise, <c>false</c>.
        /// </returns>
        public static bool kIsFlag(this long currentVal, long checkVal)
        {
            return (currentVal == checkVal);
        }
        /// <summary>
        /// Removes the flag.
        /// </summary>
        /// <param name="currentVal">The current value.</param>
        /// <param name="checkVal">The check value.</param>
        /// <returns>long representing And'd flags</returns>
        public static long kRemoveFlag(this long currentVal, long checkVal)
        {
            return (currentVal & ~checkVal);
        }

        /// <summary>
        /// Adds the flag.
        /// </summary>
        /// <param name="currentVal">The current value.</param>
        /// <param name="checkVal">The check value.</param>
        /// <returns>long representing Or'd flags</returns>
        public static long kAddFlag(this long currentVal, long checkVal)
        {
            return (currentVal | checkVal);
        }
        #endregion
    }
}

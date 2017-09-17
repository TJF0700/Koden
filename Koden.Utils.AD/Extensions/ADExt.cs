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
    public static class ADExt
    {
        /// <summary>
        /// Get unique identifier as text.
        /// </summary>
        /// <param name="byteArray">The byte array.</param>
        /// <param name="asHex">if set to <c>true</c> [as hexadecimal].</param>
        /// <returns></returns>
        public static string kGetGUIDText(this byte[] byteArray, bool asHex = false)
        {
            if (asHex)
            {
                var guid = new StringBuilder();
                foreach (byte b in byteArray)
                {
                    guid.Append(b.ToString("x2"));
                }
                return guid.ToString();
            }
            else
            {
                var objectGuid = new Guid(byteArray);
                return objectGuid.ToString();
            }
        }
    }
}

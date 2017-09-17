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
using System.Text;
using System.IO;

namespace Koden.Utils.Extensions
{
    /// <summary>
    /// MemoryStream Extension Methods that provide conversions to and from strings
    /// </summary>
   public static partial class ExtensionMethods
   {
        /// <summary>
        /// Returns the content of the stream as a string
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="encoding"></param>
        /// <returns><see cref="string"/> representing the contents of the MemoryStream</returns>
        public static string kGetAsString(this MemoryStream ms, Encoding encoding)
        {
            return encoding.GetString(ms.ToArray());
        }


        /// <summary>
        /// Returns the content of the stream as a string
        /// </summary>
        /// <param name="ms"></param>
        /// <returns><see cref="string"/> representing the contents of the MemoryStream</returns>
        public static string kGetAsString(this MemoryStream ms)
        {
            return kGetAsString(ms, Encoding.Default);
        }

        /// <summary>
        /// Writes the specified string into the memory stream
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="inputString"></param>
        /// <param name="encoding"></param>
        public static void kWriteString(this MemoryStream ms, string inputString, Encoding encoding)
        {
            byte[] buffer = encoding.GetBytes(inputString);
            ms.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        /// Writes the specified string into the memory stream
        /// </summary>
        /// <param name="ms"></param>
        /// <param name="inputString"></param>
        public static void kWriteString(this MemoryStream ms, string inputString)
        {
            kWriteString(ms, inputString, Encoding.Default);
        }
    }
}

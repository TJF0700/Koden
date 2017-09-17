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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Koden.Utils.Models
{
    /// <summary>
    /// RetVal Message Type:  Error, Success, JSCommand (Javascript Command)
    /// </summary>
    public enum FWMsgType
    {
        /// <summary>
        /// The error
        /// </summary>
        Error = -1,
        /// <summary>
        /// The success
        /// </summary>
        Success = 0,
        /// <summary>
        /// The js command
        /// </summary>
        JSCommand = 1
    }

    /// <summary>
    /// FrameWork Return Value
    /// </summary>
    public class FWRetVal
    {
        /// <summary>
        /// Gets or sets the type of the MSG.
        /// </summary>
        /// <value>The type of the MSG.</value>
        public FWMsgType MsgType { get; set; }

        /// <summary>
        /// Gets or sets the Message string to return.
        /// </summary>
        /// <value>The Message.</value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Status should be forwarded to the "NEXT" page load (for redirects).
        /// </summary>
        /// <value><c>true</c> if [forward status]; otherwise, <c>false</c>.</value>
        public bool ForwardStatus { get; set; }
    }

    /// <summary>
    /// Generic return Value for  an inumerable of records
    /// </summary>
    /// <typeparam name="T">The type of the record</typeparam>
    [Serializable]
    public class FWRetVal<T> : FWRetVal
    {
        /// <summary>
        /// Gets or sets the data, such as a returned Auto Index, etc.
        /// </summary>
        /// <value>The data.</value>
        public IEnumerable<T> Records { get; set; }
        /// <summary>
        /// Gets or sets the record.
        /// </summary>
        /// <value>
        /// The record.
        /// </value>
        public T Record {get;set;}
     }
}

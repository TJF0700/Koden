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
namespace Koden.Utils.Models
{

    /// <summary>
    /// Determines how date time values are rounded
    /// </summary>
    public enum RoundingDirection
    {
        /// <summary>
        /// The round up
        /// </summary>
        RoundUp,
        /// <summary>
        /// The round down
        /// </summary>
        RoundDown,
        /// <summary>
        /// The round
        /// </summary>
        Round
    }
    /// <summary>
    /// Type of randomization
    /// </summary>
    public enum RandomizeType
    {
        /// <summary>
        /// The numbers only
        /// </summary>
        NumbersOnly,
        /// <summary>
        /// The text only
        /// </summary>
        TextOnly,
        /// <summary>
        /// The either
        /// </summary>
        Either
    }
    /// <summary>
    /// MessageType to send
    /// </summary>
    public enum EmailMsgType
    {
        /// <summary>
        /// The etl success
        /// </summary>
        ETLSuccess,
        /// <summary>
        /// The etl failure
        /// </summary>
        ETLFailure,
        /// <summary>
        /// The etl debug
        /// </summary>
        ETLDebug,
        /// <summary>
        /// The etl warning
        /// </summary>
        ETLWarning
    }


    /// <summary>
    /// Operations to perform using REST Calls
    /// </summary>
    public enum HTTPOperation
    {
        /// <summary>
        /// GET REST Method
        /// </summary>
        GET,
        /// <summary>
        /// POST REST Method
        /// </summary>
        POST,
        /// <summary>
        /// PUT REST Method
        /// </summary>
        PUT,
        /// <summary>
        /// PATCH REST Method
        /// </summary>
        PATCH,
        /// <summary>
        /// MERGE REST Method
        /// </summary>
        MERGE,
        /// <summary>
        /// DELETE REST Method
        /// </summary>
        DELETE
    }
}

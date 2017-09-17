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
using Newtonsoft.Json;
using System;

namespace Koden.Utils.Logger
{
    /// <summary>
    /// A Log class to store the message and the Date and Time the log entry was created
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets or sets the type of the log <see cref="LogMsgType" /> .
        /// </summary>
        /// <value>The type of the log.</value>
        public LogMsgType LogType { get; set; }
        /// <summary>
        /// Gets or sets the log date.
        /// </summary>
        /// <value>The log date.</value>
        public string LogDate { get; set; }
        /// <summary>
        /// Gets or sets the log time.
        /// </summary>
        /// <value>The log time.</value>
        public string LogTime { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }

        /// <summary>
        /// Gets the log type to HTML.
        /// </summary>
        /// <value>The log type to HTML.</value>
        [JsonIgnore]
        public string ToHTMLRow
        {
            get
            {
                string retVal;
                if (LogType == LogMsgType.Separator)
                {
                    retVal = "<tr><td colspan='3' style='text-align:center'><h2>{0}</h2></td></tr>".kWith(Message);
                }
                else if (LogType == LogMsgType.Empty)
                {
                    retVal = "<tr><td colspan='3'>&nbsp;</td></tr>";
                }
                else
                {
                    retVal = "<tr class='{0}'><td>{0}</td><td class='timeslot'>{1}</td><td>{2}</td></tr>".kWith(
                    LogType.ToString().ToUpper(), LogTime, Message);
                }

                return retVal;
            }

        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LogEntry" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="logType">Type of the log entry.<see cref="LogMsgType" /> </param>
        public LogEntry(string message, LogMsgType logType)
        {
            Message = message;
            LogType = logType;
            LogDate = DateTime.Now.ToString("yyyy-MM-dd");
            LogTime = DateTime.Now.ToString("hh:mm:ss tt");

        }

    }

}


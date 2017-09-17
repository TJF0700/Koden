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
using Koden.Utils.Models;

namespace Koden.Utils.Interfaces
{
    /// <summary>
    /// Interface for Logger
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Gets or sets a value indicating whether to log to the system event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log to event log]; otherwise, <c>false</c>.
        /// </value>
        bool LogToEventLog { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to log information records to system event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log error to event log]; otherwise, <c>false</c>.
        /// </value>
        bool LogInformationToEventLog { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to log warnings to system event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log error to event log]; otherwise, <c>false</c>.
        /// </value>
        bool LogWarningToEventLog { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to log errors to system event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log error to event log]; otherwise, <c>false</c>.
        /// </value>
        bool LogErrorToEventLog { get; set; }
        /// <summary>
        /// Gets or sets the source name for event log.
        /// </summary>
        /// <value>
        /// The source name for event log.
        /// </value>
        string SourceNameForEventLog { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [copy to console].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [copy to console]; otherwise, <c>false</c>.
        /// </value>
        bool CopyToConsole { get; set; }
        /// <summary>
        /// Gets or sets the current log.
        /// </summary>
        /// <value>
        /// The current log.
        /// </value>
        string CurrentLogFile { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether to append to the daily log file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [append to daily log file]; otherwise, <c>false</c>.
        /// </value>
        bool AppendToDayLogFile { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is debug; otherwise, <c>false</c>.
        /// </value>
        bool IsDebug { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is verbose.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is verbose; otherwise, <c>false</c>.
        /// </value>
        bool IsVerbose { get; set; }

        /// <summary>
        /// Debugs the specified message format.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        void Debug(string messageFormat, params object[] parameters);
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Empties the line.
        /// </summary>
        void EmptyLine();
        /// <summary>
        /// Errors the specified message format.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        void Error(string messageFormat, params object[] parameters);
        /// <summary>
        /// Flushes the log.
        /// </summary>
        void FlushLog();
        /// <summary>
        /// Informations the specified message format.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        void Info(string messageFormat, params object[] parameters);
        /// <summary>
        /// Runnings the log.
        /// </summary>
        /// <param name="AsHTML">if set to <c>true</c> [as HTML].</param>
        /// <param name="includeVerbose">if set to <c>true</c> [include verbose].</param>
        /// <returns></returns>
        string RunningLog(bool AsHTML = false, bool includeVerbose = false);
        /// <summary>
        /// To the HTML table.
        /// </summary>
        /// <param name="includeVerbose">if set to <c>true</c> [include verbose].</param>
        /// <returns></returns>
        string ToHTMLTable(bool includeVerbose = false);
        /// <summary>
        /// Separators the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Separator(string message = "");
        /// <summary>
        /// Verboses the specified message format.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        void Verbose(string messageFormat, params object[] parameters);
        /// <summary>
        /// Warnings the specified message format.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        void Warning(string messageFormat, params object[] parameters);
        /// <summary>
        /// Writes the ret value.
        /// </summary>
        /// <param name="fwRetVal">The fw ret value.</param>
        void WriteRetVal(FWRetVal fwRetVal);
    }
}

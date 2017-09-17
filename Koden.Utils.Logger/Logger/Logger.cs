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
using System.IO;
using Koden.Utils.Logger.ConfigFileSections;
using System.Text;
using Koden.Utils.Models;
using System.Diagnostics;
using Koden.Utils.Interfaces;
using Koden.Utils.Extensions;

namespace Koden.Utils.Logger
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed periodically
    /// </summary>
    public class Logger : IDisposable, ILogger
    {
        private static Logger _instance;
        private static Queue<LogEntry> _logQueue;

        private static int _maxLogAge;
        private static int _queueSize;
        private static bool _isVerbose;
        private static bool _isFlushing;
        private static bool _isDebug;
        private static bool _copyToConsole = true;
        private static DateTime _lastFlushed = DateTime.Now;
        private static string _logPath;
        private static bool _logToFile;
        private static bool _appendToDayLogFile;
        private static bool _logToEventLog;
        private static bool _logInformationEL;
        private static bool _logWarningEL;
        private static bool _logErrorEL;
        private static string _sourceNameEL;

        private static bool _useJSON = false;
        private static bool _isDisposed;
        private static RunningLog _runningLog;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private Logger()
        {
        }


        /// <summary>
        /// Creates the instances for the specified log file.
        /// </summary>
        /// <param name="logToFile">if set to <c>true</c> [log to file].</param>
        /// <param name="logToScreen">if set to <c>true</c> [log to screen].</param>
        /// <param name="isDebug">if set to <c>true</c> [is debug].</param>
        /// <param name="isVerbose">if set to <c>true</c> [is verbose].</param>
        /// <param name="appendToDayLog">if set to <c>true</c> [append to daily log].</param>
        /// <param name="useJSON">if set to <c>true</c> [use json] format for file.</param>
        /// <param name="logDir">The log dir.</param>
        /// <param name="logFileName">Name of the log file.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Logging Directory does not exist: " + logDir</exception>
        /// <exception cref="Exception">Logging Directory does not exist: " + logDir</exception>
        public static Logger Instance(bool logToFile, bool logToScreen = false, bool isDebug = false, bool isVerbose = false, bool appendToDayLog = false, bool useJSON = false, string logDir = "", string logFileName = "")
        {

            if (_instance == null)
            {
                _instance = new Logger();
                _isDisposed = false;
                _isFlushing = false;
                _runningLog = new RunningLog();
                _logQueue = new Queue<LogEntry>();
                _isVerbose = isVerbose;
                _isDebug = isDebug;
                _copyToConsole = logToScreen;
                _logToFile = logToFile;
                _useJSON = useJSON;
                _appendToDayLogFile = appendToDayLog;

                try
                {

                    if (_logToFile)
                    {


                        _logPath = logDir + "\\" + logFileName;

                        if (!Directory.Exists(logDir))
                            throw new Exception("Logging Directory does not exist: " + logDir);
                        _runningLog.StartDate = DateTime.Now;

                        if (!_useJSON)
                            _instance.WriteToLog(String.Format("### BEGIN LOG - {0} ###{1}", DateTime.Now, Environment.NewLine), null, LogMsgType.Empty);

                    }

                }
                catch (Exception ex)
                {
                    System.Console.BackgroundColor = ConsoleColor.DarkRed;
                    System.Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("Error (Logging is DISABLED): " + Environment.NewLine + ex.kGetAllMessages());
                    _logToFile = false;

                }
            }
            return _instance;
        }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        /// <param name="logFileName">Name of the log file set programaticaly.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No config section found for KodenGroup/Logger!</exception>
        /// <exception cref="Exception">No config section found for KodenGroup/Logger!</exception>
        public static Logger Instance(string logFileName = "")
        {
            var configSection = new LoggerSection();
            string logDir = "";
            //Only one <configSections> element allowed per config file and if present must be the first child of the root <configuration> element.
            // If the instance is null then create one and init the Queue
            if (_instance == null)
            {
                try
                {

                    configSection = (LoggerSection)System.Configuration.ConfigurationManager.GetSection("KodenGroup/Logger");
                    if (configSection == null)
                        throw new Exception("No config section found for KodenGroup/Logger!");

                    _maxLogAge = configSection.MaxQueueAge;
                    _queueSize = configSection.QueueSize;
                    _useJSON = configSection.UseJSON;
                    _appendToDayLogFile = configSection.AppendToDayLogFile;
                    _logToEventLog = configSection.EventLogging.Enabled;
                    _logInformationEL = configSection.EventLogging.Info;
                    _logWarningEL = configSection.EventLogging.Warning;
                    _logErrorEL = configSection.EventLogging.Error;
                    _sourceNameEL = configSection.EventLogging.SourceName;

                    logDir = configSection.LogDirectory.Path.TrimEnd(new[] { '/', '\\' });

                    if (String.IsNullOrEmpty(logFileName))
                    {
                        string appName = System.IO.Path.GetFileNameWithoutExtension(System.AppDomain.CurrentDomain.FriendlyName);
                        logFileName = configSection.LogFile.FileName.ToLower() == "autogen" ? String.Format("{0:yyyy-MM-dd}_{1}.txt", DateTime.Now, appName) : configSection.LogFile.FileName;
                    }

                }
                catch (Exception ex)
                {
                    System.Console.BackgroundColor = ConsoleColor.DarkRed;
                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.WriteLine("Error (Logging is DISABLED): " + Environment.NewLine + ex.kGetAllMessages());
                    _logToFile = false;
                }
            }

            return Instance(configSection.IsEnabled, configSection.CopyToConsole, configSection.Debug, configSection.Verbose, configSection.UseJSON, configSection.AppendToDayLogFile, logDir, logFileName);

        }

        /// <summary>
        /// Gets or sets the log path and file currently being used.
        /// </summary>
        /// <value>The log path and file.</value>
        public string CurrentLogFile
        {
            get
            {
                return _logPath;
            }
            set
            {
                _logPath = value;
            }
        }

        /// <summary>
        /// Creates a string to contain the HTML representation of the log using a table.
        /// </summary>
        /// <param name="includeVerbose">if set to <c>true</c> [include verbose].</param>
        /// <returns></returns>
        public string ToHTMLTable(bool includeVerbose = false)
        {
            var strHTML = @"
            <style>
                .Debug{background-color:black;color:white;}
                .Info{color:blue;}
                .Warning{background-color:yellow;color:blue;}
                .Error{background-color:darkred;color:white;}
                .Verbose{color:gray;}
                .Success{color:DarkGreen;}
                table.log{font-family: monospace; font-size: 9pt;border-collapse: collapse;border-spacing: 0px;}
                table.log td{padding-right:20px;}
                .timeslot{white-space:nowrap;}
            </style>";
            if (!includeVerbose)
                _runningLog.Entries.RemoveAll(str => str.LogType == LogMsgType.Verbose);

            //            var strTable = "<table class='log'>{0}</table>";
            var loc = new StringBuilder().Append(strHTML);
            loc.Append("<table class='log'>");
            loc.AppendFormat("<tr style='font-size:6pt;color:gray;border:1px;'><td colspan='3'>Verbose: {0}, Debug Included: {1}</td></tr>", includeVerbose, IsDebug);
            foreach (var entry in _runningLog.Entries)
            {
                loc.Append(entry.ToHTMLRow);
            }
            loc.Append("</table>");

            return string.Join("", loc);

        }

        /// <summary>
        /// Gets or sets the running log (for sending just current log via Email, etc).
        /// </summary>
        /// <param name="AsHTML">if set to <c>true</c> newlines are replaced with <br />.</param>
        /// <param name="includeVerbose">if set to <c>true</c> [include verbose] logs.</param>
        /// <value>The running log of the current operation.</value>
        /// <returns></returns>
        public string RunningLog(bool AsHTML = false, bool includeVerbose = false)
        {
            var strHTML = @"
            <style>
            .Debug{background-color:black;color:white;}
            .Info{color:blue;}
            .Warning{background-color:yellow;color:blue;}
            .Error{background-color:darkred;color:white;}
            .Verbose{color:gray;}
            .Success{color:DarkGreen;}
            table.log{font-family: monospace; font-size: 9pt;border-collapse: collapse;border-spacing: 0px;}
            table.log td{padding-right:20px;}
            .timeslot{white-space:nowrap;}
            </style>";
            var strTable = "<table class='log'>{0}</table>";
            var loc = new List<string>();

            if (!includeVerbose)
                _runningLog.Entries.RemoveAll(str => str.LogType == LogMsgType.Verbose);


            //TODO: convert from class to strings here!
            loc.Insert(0,
                "<tr style='font-size:6pt;color:gray;border:1px;'><td colspan='3'>Verbose: {0}, Debug Included: {1}</td></tr>".kWith(includeVerbose, IsDebug));

            var strLog = string.Join("", loc.ToArray());

            return AsHTML == true ? strHTML + String.Format(strTable, strLog) : strLog.kRemoveHTML();
        }
        /// <summary>
        /// Gets or sets a value indicating whether to log to event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log to event log]; otherwise, <c>false</c>.
        /// </value>
        public bool LogToEventLog
        {
            get
            {
                return _logToEventLog;
            }
            set
            {
                _logToEventLog = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether log information records to event log.  (Probably not a good idea)
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log information to event log]; otherwise, <c>false</c>.
        /// </value>
        public bool LogInformationToEventLog
        {
            get
            {
                return _logInformationEL;
            }
            set
            {
                _logInformationEL = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether log warnings to event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log warning to event log]; otherwise, <c>false</c>.
        /// </value>
        public bool LogWarningToEventLog
        {
            get
            {
                return _logWarningEL;
            }
            set
            {
                _logWarningEL = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether  to log errors to event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log error to event log]; otherwise, <c>false</c>.
        /// </value>
        public bool LogErrorToEventLog
        {
            get
            {
                return _logErrorEL;
            }
            set
            {
                _logErrorEL = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating the source name to use for event log logging.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [source name for event log]; otherwise, <c>false</c>.
        /// </value>
        public string SourceNameForEventLog
        {
            get
            {
                return _sourceNameEL;
            }
            set
            {
                _sourceNameEL = value;
            }
        }
        /// <summary>
        /// Gets or sets whether to write to the console.
        /// </summary>
        /// <value>true or false.</value>
        public bool CopyToConsole
        {
            get
            {
                return _copyToConsole;
            }
            set
            {
                _copyToConsole = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to write logs to a file.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool IsEnabled
        {
            get
            {
                return _logToFile;
            }
            set
            {
                _logToFile = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to append to daily log file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [append to day log file]; otherwise, <c>false</c>.
        /// </value>
        public bool AppendToDayLogFile
        {
            get
            {
                return _appendToDayLogFile;
            }
            set
            {
                _appendToDayLogFile = value;
            }
        }
        /// <summary>
        /// Gets or sets whether the logger is in Verbose mode.
        /// </summary>
        /// <value>true or false</value>
        public bool IsVerbose
        {
            get
            {
                return _isVerbose;
            }
            set
            {
                _isVerbose = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is in debug mode.
        /// </summary>
        /// <value><c>true</c> if this instance is debug; otherwise, <c>false</c>.</value>
        public bool IsDebug
        {
            get
            {
                return _isDebug;
            }
            set
            {
                _isDebug = value;
            }
        }

        /// <summary>
        /// Writes a Separator line to the LogFile.
        /// ==============================  yyyy-MM-dd hh:mm:ss ============================
        /// </summary>
        public void Separator(string message = "")
        {
            this.WriteToLog(message, null, LogMsgType.Separator);
        }

        /// <summary>
        /// Writes a Separator line to the LogFile.
        /// ==============================  yyyy-MM-dd hh:mm:ss ============================
        /// </summary>
        public void EmptyLine()
        {
            this.WriteToLog(Environment.NewLine, null, LogMsgType.Empty);
        }

        /// <summary>
        /// Parses FWRetVal and writes to log - quick way to write either Error or Success.
        /// </summary>
        /// <param name="fwRetVal">The FWRetVal.</param>
        public void WriteRetVal(FWRetVal fwRetVal)
        {
            if (fwRetVal.MsgType == FWMsgType.Error)
                this.Error(fwRetVal.Value);
            if (fwRetVal.MsgType == FWMsgType.Success)
                this.Info(fwRetVal.Value);
        }

        /// <summary>
        /// Logs an informational message only if IsVerbose is true.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters (handles string.format functions).</param>
        public void Verbose(string messageFormat, params object[] parameters)
        {
            if (IsVerbose)
                WriteToLog(messageFormat, parameters, LogMsgType.Verbose);
        }

        /// <summary>
        /// Logs an informational message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        public void Info(string messageFormat, params object[] parameters)
        {
            WriteToLog(messageFormat, parameters, LogMsgType.Information);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters (handles string.format functions).</param>
        public void Warning(string messageFormat, params object[] parameters)
        {
            WriteToLog(messageFormat, parameters, LogMsgType.Warning);
        }

        /// <summary>
        /// Logs an Error message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters (handles string.format functions).</param>
        public void Error(string messageFormat, params object[] parameters)
        {
            WriteToLog(messageFormat, parameters, LogMsgType.Error);
        }

        /// <summary>
        /// Logs a Debug message.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters (handles string.format functions).</param>
        public void Debug(string messageFormat, params object[] parameters)
        {
            if (IsDebug)
                WriteToLog(messageFormat, parameters, LogMsgType.Debug);
        }

        /// <summary>
        /// Writes to console.
        /// </summary>
        /// <param name="logType">Type of the log.</param>
        /// <param name="message">The message.</param>
        private void WriteToConsole(LogMsgType logType, string message)
        {
            WriteToConsole(logType, message, null);
        }

        /// <summary>
        /// Writes to console.
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="logType">Type of the log.</param>
        private void WriteToConsole(LogMsgType logType, string messageFormat, params object[] parameters)
        {
            string message = (parameters == null || parameters.Length == 0) ? messageFormat : String.Format(messageFormat, parameters);
            switch (logType)
            {
                case LogMsgType.Information:
                    System.Console.ForegroundColor = ConsoleColor.Cyan;
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogMsgType.Debug:
                    System.Console.ForegroundColor = ConsoleColor.Black;
                    System.Console.BackgroundColor = ConsoleColor.White;
                    break;
                case LogMsgType.Warning:
                    System.Console.ForegroundColor = ConsoleColor.Blue;
                    System.Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                case LogMsgType.Error:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case LogMsgType.Verbose:
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    break;
                case LogMsgType.Success:
                    System.Console.ForegroundColor = ConsoleColor.Yellow;
                    System.Console.BackgroundColor = ConsoleColor.Green;
                    break;
                default:
                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.BackgroundColor = ConsoleColor.Black;
                    break;
            }
            System.Console.WriteLine(message, parameters);
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="messageFormat">The message format.</param>
        /// <param name="parameters">The parameters (handles string.format functions).</param>
        /// <param name="logMsgType">Type of the log message - debug will show in logfile if "verbose" is enabled.</param>
        private void WriteToLog(string messageFormat, object[] parameters, LogMsgType logMsgType = LogMsgType.Information)
        {
            string message = (parameters == null || parameters.Length == 0) ? messageFormat : String.Format(messageFormat, parameters);

            // skip debug messages unless verbose
            if ((!IsVerbose && logMsgType == LogMsgType.Verbose) || (!IsDebug && logMsgType == LogMsgType.Debug))
                return;

            // Lock the queue while writing to prevent contention for the log file
            lock (_logQueue)
            {
                // Create the entry and push to the Queue
                LogEntry logEntry = new LogEntry(message, logMsgType);
                _logQueue.Enqueue(logEntry);

                // If we have reached the Queue Size then flush the Queue
                if (_logQueue.Count >= _queueSize || DoPeriodicFlush())
                {
                    FlushLog();
                }
            }

            //Write to Event Log

            if (_logToEventLog)
            {
                if (!EventLog.SourceExists(_sourceNameEL)) EventLog.CreateEventSource(_sourceNameEL, "Application");
                if (_logInformationEL && logMsgType == LogMsgType.Information)
                    EventLog.WriteEntry(_sourceNameEL, message, EventLogEntryType.Information, 4);
                else
                if (_logWarningEL && logMsgType == LogMsgType.Warning)
                    EventLog.WriteEntry(_sourceNameEL, message, EventLogEntryType.Warning, 2);
                else
                if (_logErrorEL && logMsgType == LogMsgType.Error)
                    EventLog.WriteEntry(_sourceNameEL, message, EventLogEntryType.Error, 1);
            }

            //Copy to Console
            if (_copyToConsole)
            {
                if (logMsgType == LogMsgType.Separator)
                    WriteToConsole(logMsgType, Environment.NewLine + message.kPadCenter(78, '='));
                else if (logMsgType == LogMsgType.Empty)

                    WriteToConsole(logMsgType, message);
                else
                    WriteToConsole(logMsgType, "{0}\t{1}", logMsgType.ToString(), message.kRemoveHTML());
            }

        }

        //private void WriteToConsole(LogMsgType logMsgType, string messageFormat, params object[] parameters)
        //{
        //        string message = (parameters == null || parameters.Length == 0) ? messageFormat : String.Format(messageFormat, parameters);
        //        switch (logMsgType)
        //        {
        //            case LogMsgType.Information:
        //                System.Console.ForegroundColor = ConsoleColor.Cyan;
        //                System.Console.BackgroundColor = ConsoleColor.Black;
        //                break;
        //            case LogMsgType.Debug:
        //                System.Console.ForegroundColor = ConsoleColor.Black;
        //                System.Console.BackgroundColor = ConsoleColor.White;
        //                break;
        //            case LogMsgType.Warning:
        //                System.Console.ForegroundColor = ConsoleColor.Blue;
        //                System.Console.BackgroundColor = ConsoleColor.Yellow;
        //                break;
        //            case LogMsgType.Error:
        //                System.Console.ForegroundColor = ConsoleColor.White;
        //                System.Console.BackgroundColor = ConsoleColor.DarkRed;
        //                break;
        //            case LogMsgType.Verbose:
        //                System.Console.ForegroundColor = ConsoleColor.Gray;
        //                System.Console.BackgroundColor = ConsoleColor.Black;
        //                break;
        //            case LogMsgType.Success:
        //                System.Console.ForegroundColor = ConsoleColor.Yellow;
        //                System.Console.BackgroundColor = ConsoleColor.Green;
        //                break;
        //            default:
        //                System.Console.ForegroundColor = ConsoleColor.White;
        //                System.Console.BackgroundColor = ConsoleColor.Black;
        //                break;
        //        }
        //        System.Console.WriteLine(message, parameters);
        //    }

        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - _lastFlushed;
            if (logAge.TotalSeconds >= _maxLogAge)
            {
                _lastFlushed = DateTime.Now;
                FlushLog();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        public void FlushLog()
        {

            if (_isFlushing) return;

            var fileString = "";
            //   var runningString = "";

            _isFlushing = true;
            while (_logQueue.Count > 0)
            {

                LogEntry entry = _logQueue.Dequeue();
                _runningLog.Entries.Add(entry);
                if (!_useJSON)
                {
                    // runningString = entry.ToHTMLRow;

                    if (entry.LogType == LogMsgType.Separator)
                    {
                        fileString = Environment.NewLine + entry.Message.kPadCenter(78, '=') + Environment.NewLine;
                    }
                    else if (entry.LogType == LogMsgType.Empty)
                    {
                        fileString = entry.Message;
                    }
                    else
                    {
                        fileString = "{0}:\t{1}\t {2}</span>".kWith(entry.LogType.ToString().PadRight(9), entry.LogTime, entry.Message);
                    }

                    if (_logToFile)
                    {
                        using (FileStream fs = File.Open(_logPath, FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter log = new StreamWriter(fs))
                            {
                                log.WriteLine(fileString.kRemoveHTML());
                            }
                        }
                    }
                }
                else
                    _runningLog.kSaveToJsonFile(_logPath, _appendToDayLogFile);
            }
            _isFlushing = false;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Logger"/> class.
        /// </summary>
        ~Logger()
        {
            if (_logToFile) Dispose();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                if (_logToFile)
                {
                    if (!_useJSON) WriteToLog(Environment.NewLine + "### END LOGGING ###", null, LogMsgType.Empty);
                    FlushLog();
                }
                _isDisposed = true;
            }
        }
    }

    /// <summary>
    /// Different log message types.
    /// </summary>
    public enum LogMsgType
    {
        /// <summary>
        ///     An error event. This indicates a significant problem the user should know about;
        ///     usually a loss of functionality or data.
        /// </summary>
        Error = 1,

        /// <summary>
        ///     A warning event. This indicates a problem that is not immediately significant,
        ///     but that may signify conditions that could cause future problems.
        /// </summary>
        Warning = 2,
        /// <summary>
        ///     An information event. This indicates a significant, successful operation.
        /// </summary>
        Information = 4,
        /// <summary>
        ///     A success audit event. This indicates a security event that occurs when an audited
        ///     access attempt is successful; for example, logging on successfully.
        /// </summary>
        SuccessAudit = 8,
        /// <summary>
        ///     A failure audit event. This indicates a security event that occurs when an audited
        ///     access attempt fails; for example, a failed attempt to open a file.
        /// </summary>
        FailureAudit = 16,

        /// <summary>
        /// A debug message - used during development
        /// </summary>
        Debug = 32,
        /// <summary>
        /// A separator record - in an HTMLT table - a Header
        /// </summary>
        Separator = 64,
        /// <summary>
        /// A verbose record - used when extra logging is needed
        /// </summary>
        Verbose = 128,
        /// <summary>
        /// A success message
        /// </summary>
        Success = 256,
        /// <summary>
        /// An empty line - no Date/Time logged, just the text message or a return
        /// </summary>
        Empty = 512
    }
}

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
using System.Configuration;

namespace Koden.Utils.Logger.ConfigFileSections
{

    /// <summary>
    /// This class adds a custom section to the App.Config/Web.Config files labeled 
    /// </summary>
    /// <example>
    /// This is an example of an App.Config file with the Logger Section defined:
    /// <code lang="xml" xml:space="preserve"> 
    /// <configuration>
    ///   <configSections>
    ///     <sectionGroup name="KodenGroup">
    ///       <section name="Logger" type=" Koden.Utils.ConfigFileSections.LoggerSection,Koden.Utils" allowLocation="true" allowDefinition="Everywhere" />
    ///     </sectionGroup>
    ///   </configSections>
    ///     <startup> 
    ///         <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    ///     </startup>
    ///   <KodenGroup>
    ///     <Logger verbose="true" maxQueueAge="5" queueSize="4" copyToConsole="true">
    ///       <logDirectory path="p:\ssis_logs\TestConsole" />
    ///       <logFile name="autogen" />
    ///     </Logger>
    ///    </KodenGroup>
    /// </configuration>     
    /// </code>
    ///</example>    
    public class LoggerSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets whether to finalize logfile as a JSON file. If set to true, the log file can be parsed as JSON using json.Net.
        /// </summary>
        /// <value>
        /// true or false
        /// </value>
        [ConfigurationProperty("useJSON", DefaultValue = "false", IsRequired = false)]
        public Boolean UseJSON
        {
            get
            {
                return (Boolean)this["useJSON"];
            }
            set
            {
                this["useJSON"] = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to append to daily log file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [append to daily log file]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("appendToDayLogFile", DefaultValue = "false", IsRequired = false)]
        public Boolean AppendToDayLogFile
        {
            get
            {
                return (Boolean)this["appendToDayLogFile"];
            }
            set
            {
                this["appendToDayLogFile"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the verbose level. If set to true, additional Logger information is written based on flags in development.
        /// </summary>
        /// <value>true or false</value>
        [ConfigurationProperty("verbose", DefaultValue = "false", IsRequired = false)]
        public Boolean Verbose
        {
            get
            {
                return (Boolean)this["verbose"];
            }
            set
            {
                this["verbose"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("enabled", DefaultValue = "true", IsRequired = false)]
        public Boolean IsEnabled
        {
            get
            {
                return (Boolean)this["enabled"];
            }
            set
            {
                this["enabled"] = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LoggerSection" /> is in debug mode.
        /// </summary>
        /// <value><c>true</c> if debug; otherwise, <c>false</c>.</value>
        [ConfigurationProperty("debug", DefaultValue = "false", IsRequired = false)]
        public Boolean Debug
        {
            get
            {
                return (Boolean)this["debug"];
            }
            set
            {
                this["debug"] = value;
            }
        }
        /// <summary>
        /// Gets or sets the max log age in seconds.  If log reaches this timespan, it writes to disk even if max queue size hasn't been reached.
        /// </summary>
        /// <value>The max log age.</value>
        [ConfigurationProperty("maxQueueAge", DefaultValue = "5", IsRequired = false)]
        public int MaxQueueAge
        {
            get
            {
                return (int)this["maxQueueAge"];
            }
            set
            {
                this["maxQueueAge"] = value;
            }
        }

        /// <summary>
        /// Config Element - if true, then Logger is shown in console, false is not.
        /// </summary>
        /// <value>true -or- false</value>
        [ConfigurationProperty("copyToConsole", DefaultValue = "true", IsRequired = false)]
        public Boolean CopyToConsole
        {
            get
            {
                return (Boolean)this["copyToConsole"];
            }
            set
            {
                this["copyToConsole"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the size of the queue - number of records before writing to disk.
        /// </summary>
        /// <value>The size of the queue.</value>
        [ConfigurationProperty("queueSize", DefaultValue = "5", IsRequired = false)]
        public int QueueSize
        {
            get
            {
                return (int)this["queueSize"];
            }
            set
            {
                this["queueSize"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the log directory.
        /// </summary>
        /// <value>The log directory path.</value>
        [ConfigurationProperty("logDirectory")]
        public LogDirectoryElement LogDirectory
        {
            get
            {
                return (LogDirectoryElement)this["logDirectory"];
            }
            set
            { this["logDirectory"] = value; }
        }

        /// <summary>
        /// Gets or sets the log file name. Default is yyyy-MM-dd_[AppName].log
        /// </summary>
        /// <value>The log file name -or-  "autogen" will auto create logfile name.</value>
        [ConfigurationProperty("logFile")]
        public LogFileElement LogFile
        {
            get
            {
                return (LogFileElement)this["logFile"];
            }
            set
            { this["logFile"] = value; }
        }

        /// <summary>
        /// Gets or sets the log directory.
        /// </summary>
        /// <value>The log directory path.</value>
        [ConfigurationProperty("EventLogging")]
        public EventLogElement EventLogging
        {
            get
            {
                return (EventLogElement)this["EventLogging"];
            }
            set
            { this["EventLogging"] = value; }
        }
    }
}



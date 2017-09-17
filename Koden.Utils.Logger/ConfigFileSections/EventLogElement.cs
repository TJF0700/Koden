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
using System.Configuration;

namespace Koden.Utils.Logger.ConfigFileSections
{

    //<EventLogging enabled="false" Info="false" Warning ="false" Error="true" SourceName="MyAppName" 
    /// <summary>
    /// Event Log Element in *.config
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class EventLogElement : ConfigurationElement
    {
        /// <summary>
        /// Config Element - if true, then events log to the Application Log of the server.
        /// </summary>
        /// <value>
        /// true -or- false
        /// </value>
        [ConfigurationProperty("enabled", DefaultValue = "false", IsRequired = true)]
        public Boolean Enabled
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
        /// Gets or sets a value indicating whether Information records are logged in the event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if information; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("info", DefaultValue = "false", IsRequired = false)]
        public Boolean Info
        {
            get
            {
                return (Boolean)this["info"];
            }
            set
            {
                this["info"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Warnings are logged in the event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if warning; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("warning", DefaultValue = "false", IsRequired = false)]
        public Boolean Warning
        {
            get
            {
                return (Boolean)this["warning"];
            }
            set
            {
                this["warning"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether Errors are logged in the event log.
        /// </summary>
        /// <value>
        ///   <c>true</c> if error; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("error", DefaultValue = "false", IsRequired = false)]
        public Boolean Error
        {
            get
            {
                return (Boolean)this["error"];
            }
            set
            {
                this["error"] = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the source as it appears in the Event Log.
        /// </summary>
        /// <value>
        /// The name of the source.
        /// </value>
        [ConfigurationProperty("sourceName", DefaultValue = "AppName", IsRequired = true)]
        public String SourceName
        {
            get
            {
                return (String)this["sourceName"];
            }
            set
            {
                this["sourceName"] = value;
            }
        }
    }
}

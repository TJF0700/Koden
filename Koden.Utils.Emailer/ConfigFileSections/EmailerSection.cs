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

namespace Koden.Utils.Emailer.ConfigFileSections
{

    /// <summary>
    /// This class adds a custom section to the App.Config/Web.Config files labeled 
    /// </summary>
    /// <example>
    /// This is an example of an App.Config file:
    /// <code lang="xml" xml:space="preserve"> 
    /// <configuration>
    ///   <configSections>
    ///     <sectionGroup name="KodenGroup">
    ///       <section name="Email" type=" Koden.Utils.ConfigFileSections.EmailzSection,Koden.Utils" allowLocation="true" allowDefinition="Everywhere" />
    ///     </sectionGroup>
    ///   </configSections>
    ///     <startup> 
    ///         <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    ///     </startup>
    ///   <KodenGroup>
    ///     <Email>
    ///       <SMTPServer hostname="SMTP.fmgllc.com" />
    ///       <DebugMsg enabled="false" from="_ETLNotify@fmgllc.com" to="developer@fmgllc.com" attachLog="true" />
    ///       <SuccessMsg enabled="true" from="_ETLNotify@fmgllc.com" to="someone@fmgllc.com;someoneelse@fmgllc.com" attachLog="false" />
    ///       <FailureMsg enabled="true" from="_ETLNotify@fmgllc.com" to="_ETLNotify@fmgllc.com" cc="somone@fmgllc.com" attachLog="true" />
    ///     </Email>
    ///    </KodenGroup>
    /// </configuration>     
    /// </code>
    ///</example>
    public class EmailerSection : ConfigurationSection
    {

        /// <summary>
        /// Gets or sets the SMTP server.
        /// </summary>
        /// <value>
        /// The SMTP server.
        /// </value>
        [ConfigurationProperty("SMTPServer")]
        public SMTPServerElement SMTPServer
        {
            get
            {
                return (SMTPServerElement)this["SMTPServer"];
            }
            set
            { this["SMTPServer"] = value; }
        }


        /// <summary>
        /// Gets or sets the debug MSG.
        /// </summary>
        /// <value>
        /// The debug MSG.
        /// </value>
        [ConfigurationProperty("DebugMsg")]
        public MsgElement DebugMsg
        {
            get
            {
                return (MsgElement)this["DebugMsg"];
            }
            set
            { this["DebugMsg"] = value; }
        }

        /// <summary>
        /// Gets or sets the success MSG.
        /// </summary>
        /// <value>
        /// The success MSG.
        /// </value>
        [ConfigurationProperty("SuccessMsg")]
        public MsgElement SuccessMsg
        {
            get
            {
                return (MsgElement)this["SuccessMsg"];
            }
            set
            { this["SuccessMsg"] = value; }
        }

        /// <summary>
        /// Gets or sets the failure MSG.
        /// </summary>
        /// <value>
        /// The failure MSG.
        /// </value>
        [ConfigurationProperty("FailureMsg")]
        public MsgElement FailureMsg
        {
            get
            {
                return (MsgElement)this["FailureMsg"];
            }
            set
            { this["FailureMsg"] = value; }
        }


    }


    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class SMTPServerElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name of the host.
        /// </summary>
        /// <value>
        /// The name of the host.
        /// </value>
        [ConfigurationProperty("hostname", IsRequired = true)]
        public String HostName
        {
            get
            {
                return (String)this["hostname"];
            }
            set
            {
                this["hostname"] = value;
            }
        }

    }
    /// <summary>
    /// Defines the LogDirectory Element for the configuration file
    /// </summary>
    public class MsgElement : ConfigurationElement
    {


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="MsgElement"/> is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
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
        /// Gets or sets who message is from.
        /// </summary>
        /// <value>
        /// email address
        /// </value>
        [ConfigurationProperty("from", IsRequired = false)]
        public String From
        {
            get
            {
                return (String)this["from"];
            }
            set
            {
                this["from"] = value;
            }
        }

        /// <summary>
        /// Gets or sets who message is to.
        /// </summary>
        /// <value>
        /// email address
        /// </value>
        [ConfigurationProperty("to", IsRequired = false)]
        public String To
        {
            get
            {
                return (String)this["to"];
            }
            set
            {
                this["to"] = value;
            }
        }

        /// <summary>
        /// Gets or sets who message is to.
        /// </summary>
        /// <value>
        /// email address
        /// </value>
        [ConfigurationProperty("cc", IsRequired = false)]
        public String CC
        {
            get
            {
                return (String)this["cc"];
            }
            set
            {
                this["cc"] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [attach log].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [attach log]; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("attachLog", DefaultValue = "false", IsRequired = false)]
        public Boolean AttachLog
        {
            get
            {
                return (Boolean)this["attachLog"];
            }
            set
            {
                this["attachLog"] = value;
            }
        }
    }


}



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
using System.Linq;

namespace Koden.Utils.Exchange.ConfigFileSections
{
    /// <summary>
    /// Mailbox element in *.config file
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class MailboxElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The "id" of the element within the config file
        /// </value>
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
        /// <summary>
        /// Gets or sets the host.
        /// </summary>
        /// <value>
        /// The host.
        /// </value>
        [ConfigurationProperty("host", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }
        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        /// <value>
        /// The userid.
        /// </value>
        [ConfigurationProperty("userid", DefaultValue = "abhi", IsKey = false, IsRequired = false)]
        public string userid
        {
            get { return (string)base["userid"]; }
            set { base["userid"] = value; }
        }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [ConfigurationProperty("password", DefaultValue = "password", IsKey = false, IsRequired = false)]
        public string password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }


        /// <summary>
        /// Gets or sets the subject to monitor.
        /// </summary>
        /// <value>
        /// The subject to monitor.
        /// </value>
        [ConfigurationProperty("subjectToMonitor", DefaultValue = ".", IsKey = false, IsRequired = true)]
        public string subjectToMonitor
        {
            get { return (string)base["subjectToMonitor"]; }
            set { base["subjectToMonitor"] = value; }
        }

        /// <summary>
        /// Gets or sets the attachment extpected.
        /// </summary>
        /// <value>
        /// The attachment extpected.
        /// </value>
        [ConfigurationProperty("AttachmentExtpected", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string AttachmentExtpected
        {
            get { return (string)base["AttachmentExtpected"]; }
            set { base["AttachmentExtpected"] = value; }
        }
        /// <summary>
        /// Gets or sets the storage directory.
        /// </summary>
        /// <value>
        /// The storage directory.
        /// </value>
        [ConfigurationProperty("StorageDirectory", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string StorageDirectory
        {
            get { return (string)base["StorageDirectory"]; }
            set { base["StorageDirectory"] = value; }
        }
        /// <summary>
        /// Gets or sets the extension expected to find on attached files.
        /// </summary>
        /// <value>
        /// The extension expected.
        /// </value>
        [ConfigurationProperty("ExtensionExpected", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string ExtensionExpected
        {
            get { return (string)base["ExtensionExpected"]; }
            set { base["ExtensionExpected"] = value; }
        }

    }
}

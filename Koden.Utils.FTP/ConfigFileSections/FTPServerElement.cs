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

namespace Koden.Utils.FTPClient.ConfigFileSections
{
    /// <summary>
    /// FTP Server Element
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class FTPServerElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
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
        [ConfigurationProperty("host", DefaultValue = "", IsKey = false, IsRequired = true)]
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
        /// Gets or sets the localdir.
        /// </summary>
        /// <value>
        /// The localdir.
        /// </value>
        [ConfigurationProperty("localdir", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string localdir
        {
            get { return (string)base["localdir"]; }
            set { base["localdir"] = value; }
        }

        /// <summary>
        /// Gets or sets the remotedir.
        /// </summary>
        /// <value>
        /// The remotedir.
        /// </value>
        [ConfigurationProperty("remotedir", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string remotedir
        {
            get { return (string)base["remotedir"]; }
            set { base["remotedir"] = value; }
        }
        /// <summary>
        /// Gets or sets the retry count.
        /// </summary>
        /// <value>
        /// The retry count.
        /// </value>
        [ConfigurationProperty("retryCount", DefaultValue = 3, IsKey = false, IsRequired = false)]
        public int retryCount
        {
            get { return (int) base["retryCount"]; }
            set { base["retryCount"] = value; }
        }

        /// <summary>
        /// Gets or sets the retry wait seconds.
        /// </summary>
        /// <value>
        /// The retry wait seconds.
        /// </value>
        [ConfigurationProperty("retryWaitSeconds", DefaultValue = 61, IsKey = false, IsRequired = false)]
        public int retryWaitSeconds
        {
            get { return (int) base["retryWaitSeconds"]; }
            set { base["retryWaitSeconds"] = value; }
        }
    }
}

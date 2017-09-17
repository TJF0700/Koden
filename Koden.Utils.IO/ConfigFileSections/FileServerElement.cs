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

namespace Koden.Utils.IO.ConfigFileSections
{
    /// <summary>
    /// A FileServer element in the web.config file
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class FileServerElement : ConfigurationElement
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
        /// Gets or sets the sharename.
        /// </summary>
        /// <value>
        /// The sharename.
        /// </value>
        [ConfigurationProperty("sharename", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string sharename
        {
            get { return (string)base["sharename"]; }
            set { base["sharename"] = value; }
        }

        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        /// <value>
        /// The userid.
        /// </value>
        [ConfigurationProperty("userid", DefaultValue = "none", IsKey = false, IsRequired = false)]
        public string userid
        {
            get { return (string)base["userid"]; }
            set { base["userid"] = value; }
        }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>
        /// The domain.
        /// </value>
        [ConfigurationProperty("domain", DefaultValue = "local", IsKey = false, IsRequired = false)]
        public string domain
        {
            get { return (string)base["domain"]; }
            set { base["domain"] = value; }
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
        /// Gets or sets a value indicating whether this <see cref="FileServerElement" /> is autologin.
        /// </summary>
        /// <value>
        ///   <c>true</c> if autologin; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("autologin", DefaultValue = "true", IsKey = false, IsRequired = false)]
        public bool autologin
        {
            get { return (bool)base["autologin"]; }
            set { base["autologin"] = value; }
        }
    }
}

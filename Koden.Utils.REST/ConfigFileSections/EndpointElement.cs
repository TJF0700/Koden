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

namespace Koden.Utils.REST.ConfigFileSections
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class EndpointElement : ConfigurationElement
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
        /// Gets or sets the root URI.
        /// </summary>
        /// <value>
        /// The root URI.
        /// </value>
        [ConfigurationProperty("rootURI", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string rootURI
        {
            get { return (string)base["rootURI"]; }
            set { base["rootURI"] = value; }
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
        /// Gets or sets the user key.
        /// </summary>
        /// <value>
        /// The user key.
        /// </value>
        [ConfigurationProperty("userKey", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string userKey
        {
            get { return (string) base["userKey"]; }
            set { base["userKey"] = value; }
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
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>
        /// The type of the authentication.
        /// </value>
        [ConfigurationProperty("authType", DefaultValue = "ANONYMOUS", IsKey = false, IsRequired = false)]
        public string authType
        {
            get { return (string)base["authType"]; }
            set { base["authType"] = value; }
        }

        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        /// <value>
        /// The time out.
        /// </value>
        [ConfigurationProperty("timeOut", DefaultValue = "30", IsKey = false, IsRequired = false)]
        public int timeOut
        {
            get { return (int)base["timeOut"]; }
            set { base["timeOut"] = value; }
        }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="EndpointElement"/> is XML.
        /// </summary>
        /// <value>
        ///   <c>true</c> if XML; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("XML", DefaultValue = "false", IsKey = false, IsRequired = true)]
        public bool XML
        {
            get { return (bool)base["XML"]; }
            set { base["XML"] = value; }
        }
        /// <summary>
        /// Gets or sets the queries.
        /// </summary>
        /// <value>
        /// The queries.
        /// </value>
        [ConfigurationProperty("Queries")]
        public QueryAppearanceCollection Queries
        {
            get { return ((QueryAppearanceCollection)(base["Queries"])); }
            set { base["Queries"] = value; }
        }


    }
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class EndPointQueryElement : ConfigurationElement
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
        /// Gets or sets the queryoption.
        /// </summary>
        /// <value>
        /// The queryoption.
        /// </value>
        [ConfigurationProperty("queryoption", DefaultValue = "", IsKey = false, IsRequired = false)]
        public string queryoption
        {
            get { return (string)base["queryoption"]; }
            set { base["queryoption"] = value; }
        }

        /// <summary>
        /// Gets or sets the records per page.
        /// </summary>
        /// <value>
        /// The records per page.
        /// </value>
        [ConfigurationProperty("recordsPerPage", DefaultValue = "", IsKey = false, IsRequired = false)]
        public int? recordsPerPage
        {
            get { return (int?)base["recordsPerPage"]; }
            set { base["recordsPerPage"] = value; }
        }
    }
}

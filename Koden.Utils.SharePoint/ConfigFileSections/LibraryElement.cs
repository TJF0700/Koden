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

namespace Koden.Utils.SharePoint.ConfigFileSections
{
    public class LibraryElement : ConfigurationElement
    {
        [ConfigurationProperty("name", DefaultValue = "", IsKey = true, IsRequired = true)]
        public string name
        {
            get { return (string)base["name"]; }
            set { base["name"] = value; }
        }
        [ConfigurationProperty("host", DefaultValue = "", IsKey = false, IsRequired = true)]
        public string host
        {
            get { return (string)base["host"]; }
            set { base["host"] = value; }
        }

        [ConfigurationProperty("userid", DefaultValue = "abhi", IsKey = false, IsRequired = false)]
        public string userid
        {
            get { return (string)base["userid"]; }
            set { base["userid"] = value; }
        }

        [ConfigurationProperty("password", DefaultValue = "password", IsKey = false, IsRequired = false)]
        public string password
        {
            get { return (string)base["password"]; }
            set { base["password"] = value; }
        }


        [ConfigurationProperty("sitename", DefaultValue = ".", IsKey = false, IsRequired = true)]
        public string sitename
        {
            get { return (string)base["sitename"]; }
            set { base["sitename"] = value; }
        }

        [ConfigurationProperty("library", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string library
        {
            get { return (string)base["library"]; }
            set { base["library"] = value; }
        }

        [ConfigurationProperty("basefolder", DefaultValue = ".", IsKey = false, IsRequired = false)]
        public string basefolder
        {
            get { return (string)base["basefolder"]; }
            set { base["basefolder"] = value; }
        }

        [ConfigurationProperty("retryCount", DefaultValue = 3, IsKey = false, IsRequired = false)]
        public int retryCount
        {
            get { return (int) base["retryCount"]; }
            set { base["retryCount"] = value; }
        }

        [ConfigurationProperty("retryWaitSeconds", DefaultValue = 61, IsKey = false, IsRequired = false)]
        public int retryWaitSeconds
        {
            get { return (int) base["retryWaitSeconds"]; }
            set { base["retryWaitSeconds"] = value; }
        }
    }
}

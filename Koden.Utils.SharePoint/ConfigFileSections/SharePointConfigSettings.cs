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
using System.Configuration;
using System.Linq;

namespace Koden.Utils.SharePoint.ConfigFileSections
{

    public class SharePointConfigSettings
    {
        private SharePointSection LibraryAppearanceConfiguration
        {
            get
            {
                return (SharePointSection)ConfigurationManager.GetSection("KodenGroup/SharePoint");
            }
        }

        private LibraryAppearanceCollection LibraryAppearances
        {
            get
            {
                return this.LibraryAppearanceConfiguration.LibraryElement;
            }
        }

        /// <summary>
        /// Gets an Enumerable list of SharePoint Libraries defined in config section.
        /// </summary>
        /// <value>
        /// The sp libraries.
        /// </value>
        public IEnumerable<LibraryElement> Libraries
        {
            get
            {
                foreach (LibraryElement selement in this.LibraryAppearances)
                {
                    if (selement != null)
                        yield return selement;
                }
            }
        }

        /// <summary>
        /// Gets the configuration for a specific Library by "name".
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public LibraryElement Library(string name)
        {
            
            return this.Libraries.Where(m=>m.name.ToLower() == name.ToLower()).FirstOrDefault();
        }

    }

}

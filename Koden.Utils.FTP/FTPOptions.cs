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

namespace Koden.Utils.FTPClient
{
    /// <summary>
    /// Options for ALL FTP methods
    /// </summary>
    public class FTPOptions
    {
        /// <summary>
        /// The filter type
        /// </summary>
        public FTPFilterType FilterType;
        /// <summary>
        /// The filter text - data is string "10/1/2015", etc.  DateFilterToday needs nothing in filterText.
        /// </summary>
        public string FilterText;
        /// <summary>
        /// The keep original date time the file was last accessed on server
        /// </summary>
        public Boolean KeepOrigDateTime;
        /// <summary>
        /// Delete the file after retrieval from server
        /// </summary>
        public Boolean DeleteAfterRetrieve;
        /// <summary>
        /// Force overwrite of local file if it exists, otherwise skip
        /// </summary>
        public Boolean ForceOverwrite;
    }
}

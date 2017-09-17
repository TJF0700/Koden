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
using Koden.Utils.Interfaces;
using System.Collections.Generic;

namespace Koden.Utils.REST
{
    /// <summary>
    /// 
    /// </summary>
    public class RESTHeader
    {
        /// <summary>
        /// Gets or sets the uri to the root endpoint for REST API.
        /// </summary>
        /// <value>The host.</value>
        public string rootURI { get; set; }

        /// Gets or sets the name of the user.
        /// <summary>
        /// Gets or Sets the User ID
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserId { get; set; }
        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        public string Password { get; set; }
        /// <summary>
        /// Gets or sets the user key (Encrypted Userid/password combo).
        /// </summary>
        /// <value>The user key.</value>
        public string UserKey { get; set; }
        /// <summary>
        /// Gets or sets the type of the authentication.
        /// </summary>
        /// <value>
        /// The type of the authentication.
        /// </value>
        public string AuthType { get; set; }

        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>The logger instance.</value>
        public ILogger LoggerInstance { get; set; }

        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        /// <value>
        /// The time out.
        /// </value>
        public int TimeOut { get; set; }
        /// <summary>
        /// Gets or sets whether or not to return XML.
        /// </summary>
        /// <value>true - reutrns XML, false returns JSON.</value>
        public bool XML { get; set; }

       
    }
}

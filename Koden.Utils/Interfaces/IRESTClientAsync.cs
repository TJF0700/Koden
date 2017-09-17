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
#region License
// Copyright (c) 2014 Tim Fischer
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion
using System.Collections.Generic;
using System.Threading.Tasks;
using Koden.Utils.REST.Models;
using Koden.Utils.Models;

namespace Koden.Utils.REST
{
    /// <summary>
    /// Interface to Asynchronous REST Client
    /// </summary>
    public interface IRESTClientAsync
    {
        /// <summary>
        /// Gets or sets the additional headers.
        /// </summary>
        /// <value>
        /// The additional headers.
        /// </value>
        Dictionary<string, string> AdditionalHeaders { get; set; }
        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        string ContentType { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [log enabled].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [log enabled]; otherwise, <c>false</c>.
        /// </value>
        bool LogEnabled { get; set; }

        /// <summary>
        /// Gets or sets the method (GET,POST,PUT, etc).
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        RESTOperation Method { get; set; }
        /// <summary>
        /// Gets or sets the data to POST to an API.
        /// </summary>
        /// <value>
        /// The post data.
        /// </value>
        string PostData { get; set; }
        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        /// <value>
        /// The time out.
        /// </value>
        int TimeOut { get; set; }

        /// <summary>
        /// Calls the API using token asynchronously.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="apiMethod">The API method.</param>
        /// <param name="loginToken">The login token.</param>
        /// <param name="returnJSON">if set to <c>true</c> [return json].</param>
        /// <returns></returns>
        Task<T> CallAPIUsingTokenAsync<T>(string endpoint, string apiMethod, LoginTokenResult loginToken, bool returnJSON);
        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Does the request asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        Task<string> DoRequestAsync(string parameters);
        /// <summary>
        /// Does the request asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <param name="timeOut">The time out.</param>
        /// <param name="authtype">The authtype.</param>
        /// <returns></returns>
        Task<string> DoRequestAsync(string parameters, string userID, string password, int timeOut, string authtype);
        /// <summary>
        /// Gets the API.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="apiMethod">The API method.</param>
        /// <param name="loginToken">The login token.</param>
        /// <param name="returnJSON">if set to <c>true</c> [return json].</param>
        /// <returns></returns>
        Task<T> CallAPIUsingToken<T>(string endpoint, string apiMethod, LoginTokenResult loginToken, bool returnJSON);
        /// <summary>
        /// Gets the login token asynchronously.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        Task<LoginTokenResult> GetLoginTokenAsync(string endpoint, string userID, string password);
    }
}

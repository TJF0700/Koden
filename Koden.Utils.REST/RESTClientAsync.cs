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

using Koden.Utils.Interfaces;
using Koden.Utils.Models;
using Koden.Utils.REST.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;



namespace Koden.Utils.REST
{

    /// <summary>
    /// Asynchronously make REST Calls
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class RESTClientAsync : IDisposable, IRESTClientAsync
    {

        private int timeOut = 30;
        private string _rootURI { get; set; }

        private string _UserId { get; set; }
        private string _UserKey { get; set; }
        private string _Password { get; set; }
        private string _AuthType { get; set; }
        private string _postData;
        private ILogger _loggerInstance = null;
        private Boolean _logEnabled = false;

        private Boolean _XML = false;


        private static RESTOperation _method = RESTOperation.GET;
        private string _contentType = "text/xml";

        /// <summary>
        /// Gets or sets the type of the content.
        /// </summary>
        /// <value>
        /// The type of the content.
        /// </value>
        public string ContentType
        {
            get { return _contentType; }
            set { _contentType = value; }
        }
        /// <summary>
        /// Gets or sets the method (POST,GET,PUT, etc).
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public RESTOperation Method
        {
            get { return _method; }
            set { _method = value; }
        }

        /// <summary>
        /// Gets or sets the post data.
        /// </summary>
        /// <value>
        /// The post data.
        /// </value>
        public string PostData
        {
            get { return _postData; }
            set { _postData = value; }
        }

        /// <summary>
        /// Gets or sets the time out.
        /// </summary>
        /// <value>
        /// The time out.
        /// </value>
        public int TimeOut
        {
            get
            {
                return this.timeOut == 0 ? 10 : this.timeOut;
            }
            set
            {
                this.timeOut = value;
            }
        }

        /// <summary>
        /// Gets or sets any additional headers.
        /// </summary>
        /// <value>
        /// The additional headers string,string dictionary object.
        /// </value>
        public Dictionary<string, string> AdditionalHeaders { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether logging is enabled.
        /// </summary>
        /// <value><c>true</c> if [log enabled]; otherwise, <c>false</c>.</value>
        public bool LogEnabled
        {
            get
            {
                return this._logEnabled;
            }
            set
            {
                this._logEnabled = value;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RESTClientAsync"/> class.
        /// </summary>
        /// <param name="restHeader">The rest header.</param>
        /// <param name="postData">The post data.</param>
        public RESTClientAsync(RESTHeader restHeader, string postData = "")
        {
            _loggerInstance = restHeader.LoggerInstance;
            if (_loggerInstance != null) _logEnabled = true;
            if (_logEnabled)
            {
                _loggerInstance.Info("Instantiating REST Module:");
                _loggerInstance.Info("\tEndpoint URI: {0}", restHeader.rootURI);
                _loggerInstance.Info("\tUserID: {0}", restHeader.UserId);
                _loggerInstance.Info("\tUserKey: {0}", restHeader.UserKey);
                _loggerInstance.Info("\tTimeOut: {0}", restHeader.TimeOut);
                _loggerInstance.Info("\tContentType: {0}", restHeader.XML == true ? "XML" : "JSON");

            }
            _rootURI = restHeader.rootURI;
            _UserId = restHeader.UserId;
            _UserKey = restHeader.UserKey;
            _Password = restHeader.Password;
            _AuthType = restHeader.AuthType;
            _XML = restHeader.XML;
            TimeOut = restHeader.TimeOut;
            PostData = postData;

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RESTClientAsync"/> class.
        /// </summary>
        public RESTClientAsync()
        {
            _rootURI = "";
            Method = RESTOperation.GET;
            PostData = "";
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="RESTClientAsync"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="XML">if set to <c>true</c> [XML].</param>
        public RESTClientAsync(string endpoint, bool XML = false)
        {
            _rootURI = endpoint;
            Method = RESTOperation.GET;
            _XML = XML;
            PostData = "";
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="RESTClientAsync"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="method">The method.</param>
        /// <param name="XML">if set to <c>true</c> [XML].</param>
        public RESTClientAsync(string endpoint, RESTOperation method, bool XML = false)
        {
            _rootURI = endpoint;
            Method = method;
            _XML = XML;
            PostData = "";
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RESTClientAsync"/> class.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="method">The method.</param>
        /// <param name="postData">The post data.</param>
        /// <param name="XML">if set to <c>true</c> [XML].</param>
        public RESTClientAsync(string endpoint, RESTOperation method, string postData, bool XML = false)
        {
            _rootURI = endpoint;
            Method = method;
            _XML = XML;
            PostData = postData;
        }


        /// <summary>
        /// Gets the data from the API endpoint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="apiMethod">The API method.</param>
        /// <param name="loginToken">The login token.</param>
        /// <param name="returnJSON">if set to <c>true</c> returns JSON else returns XML.</param>
        /// <returns></returns>
        public async Task<T> CallAPIUsingTokenAsync<T>(string endpoint, string apiMethod, LoginTokenResult loginToken, bool returnJSON)
        {
            _rootURI = endpoint;
            Method = RESTOperation.GET;
            AdditionalHeaders = new Dictionary<string, string>();
            AdditionalHeaders.Add("Authorization", "Bearer " + loginToken.AccessToken);

            if (returnJSON) ContentType = "application/json";
            else ContentType = "text/xml";

            T results;

            var jsonRetVal = await DoRequestAsync(apiMethod);
            results = JsonConvert.DeserializeObject<T>(jsonRetVal);

            return results;
        }

        /// <summary>
        /// Makes the request asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <param name="timeOut">The time out.</param>
        /// <param name="authtype">The authtype.</param>
        /// <returns></returns>
        public async Task<string> DoRequestAsync(string parameters, string userID, string password, int timeOut, string authtype)
        {
            this._Password = password;
            this._UserId = userID;
            this._AuthType = authtype;
            this.TimeOut = timeOut;
            return await DoRequestAsync(parameters);
        }

        /// <summary>
        /// Makes the request asynchronously.
        /// </summary>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        /// <exception cref="ApplicationException"></exception>
        public async Task<string> DoRequestAsync(string parameters)
        {
            if (_logEnabled) _loggerInstance.Verbose("Creating web request to: {0}", _rootURI + parameters);
            HttpWebRequest request;
            string responseValue = string.Empty;

            try
            {
                request = (HttpWebRequest)WebRequest.Create(_rootURI + parameters);
                request = ModifyHeaders(request);


                if (!string.IsNullOrEmpty(PostData) && (Method == RESTOperation.POST || Method == RESTOperation.PUT || Method == RESTOperation.PATCH || Method == RESTOperation.MERGE))
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(PostData);
                    request.ContentLength = bytes.Length;

                    using (var writeStream = request.GetRequestStream())
                    {
                        writeStream.Write(bytes, 0, bytes.Length);
                        writeStream.Flush();
                        writeStream.Close();
                    }
                }

                var validStatus = new List<HttpStatusCode> { HttpStatusCode.OK, HttpStatusCode.Created, HttpStatusCode.Accepted, HttpStatusCode.NoContent };
                if (_logEnabled) _loggerInstance.Debug("Contacting Endpoint...");
                using (var response = (HttpWebResponse)await request.GetResponseAsync())
                {
                    if (!validStatus.Contains(response.StatusCode))
                    {
                        string message = String.Format("Koden Exception - Request Issues. Received HTTP {0}:\n{1}", response.StatusCode, response.StatusDescription.Replace("{", "\\{").Replace("}", "\\}"));
                        if (_logEnabled) _loggerInstance.Error(message);
                        throw new ApplicationException(message);
                    }

                    responseValue = await GetHTTPResponseAsync(response);
                    response.Close();
                }
                if (_logEnabled) _loggerInstance.Debug("Response Received!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                request = null;
            }
            return responseValue;
        }

        private HttpWebRequest ModifyHeaders(HttpWebRequest request)
        {
            if (!String.IsNullOrEmpty(_UserId))
            {
                switch (_AuthType.ToUpper())
                {
                    case "PASSWORD":  //JWT Token/AuthBearer
                        if (_logEnabled) _loggerInstance.Debug("Authenticating using password/JWT Bearer Token (user:password): {0}", Convert.ToBase64String(Encoding.Default.GetBytes(_UserId + ":" + _Password)));
                        request.Headers["Authorization"] = "Bearer " + Convert.ToBase64String(Encoding.Default.GetBytes(_UserId + ":" + _Password));

                        break;
                    case "BASIC":

                        if (_logEnabled) _loggerInstance.Debug("Authenticating using BASIC (user:password): {0}", Convert.ToBase64String(Encoding.Default.GetBytes(_UserId + ":" + _Password)));
                        request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(_UserId + ":" + _Password));
                        break;
                    case "NTLM":
                        if (_UserId.Equals("Current", StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (_logEnabled) _loggerInstance.Debug("Authenticating using NTLM (Current User)");
                            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                            request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                        }
                        else
                        {
                            if (_logEnabled) _loggerInstance.Debug("Authenticating using NTLM (user:password): {0}/********", _UserId);
                            var netCreds = new NetworkCredential(_UserId, _Password);
                            CredentialCache myCache = new CredentialCache
                            {
                                { new Uri(_rootURI), _AuthType, netCreds }
                            };

                            request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.MutualAuthRequested;
                            request.Credentials = myCache;
                        }

                        break;
                }
            }
            else
                if (!string.IsNullOrEmpty(_UserKey))
            {
                if (_logEnabled) _loggerInstance.Debug("Authenticating using BASIC (userKey): {0}", _UserKey);
                request.Headers["Authorization"] = "Basic " + _UserKey;
            }

            request.ContentLength = 0;
            request.ContentType = ContentType;
            request.Accept = ContentType;
            request.Timeout = TimeOut * 1000;
            request.Method = Method.ToString();
            request.KeepAlive = false;
            foreach (var item in AdditionalHeaders)
            {
                request.Headers.Add(item.Key, item.Value);
            }

            request.ServicePoint.ConnectionLeaseTimeout = TimeOut * 1000;
            request.ServicePoint.MaxIdleTime = TimeOut * 1000;

            return request;
        }

        /// <summary>
        /// Gets the login token.
        /// </summary>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="userID">The user identifier.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public async Task<LoginTokenResult> GetLoginTokenAsync(string endpoint, string userID, string password)
        {
            _rootURI = endpoint;
            Method = RESTOperation.POST;
            AdditionalHeaders = new Dictionary<string, string>();
            AdditionalHeaders.Add("Audience", "Any");
            ContentType = "application/x-www-form-urlencoded";
            PostData = string.Format("username={0}&password={1}&grant_type=password",
                userID, password);
            dynamic results;

            var jsonRetVal = await DoRequestAsync("/oauth2/token");
            results = JsonConvert.DeserializeObject<LoginTokenResult>(jsonRetVal);

            return results;
        }

        /// <summary>
        /// Gets the data from the api endpoint.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="apiMethod">The API method.</param>
        /// <param name="loginToken">The login token.</param>
        /// <param name="returnJSON">if set to <c>true</c> returns JSON else returns XML.</param>
        /// <returns></returns>
        public async Task<T> CallAPIUsingToken<T>(string endpoint, string apiMethod, LoginTokenResult loginToken, bool returnJSON)
        {
            _rootURI = endpoint;
            Method = RESTOperation.GET;
            AdditionalHeaders = new Dictionary<string, string>();
            AdditionalHeaders.Add("Authorization", "Bearer " + loginToken.AccessToken);

            if (returnJSON) ContentType = "application/json";
            else ContentType = "text/xml";

            T results;

            var jsonRetVal = await DoRequestAsync(apiMethod);
            results = JsonConvert.DeserializeObject<T>(jsonRetVal);

            return results;
        }

        /// <summary>
        /// Gets the HTTP response asynchronously.
        /// </summary>
        /// <param name="endpointResponse">The endpoint response.</param>
        /// <returns></returns>
        private async Task<string> GetHTTPResponseAsync(HttpWebResponse endpointResponse)
        {
            string retVal = String.Empty;
            var content = new MemoryStream();

            // grab the response
            using (var responseStream = endpointResponse.GetResponseStream())
            {

                await responseStream.CopyToAsync(content);

                retVal = Encoding.UTF8.GetString(content.ToArray());
            }

            return retVal;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _loggerInstance.FlushLog();
        }
    } // class
}

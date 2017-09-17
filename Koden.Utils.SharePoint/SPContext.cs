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
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using Koden.Utils.Extensions;
using Koden.Utils.Interfaces;

namespace Koden.Utils.SharePoint
{
    public partial class SPContext : IDisposable
    {

        #region "Properties"

        internal string SharePointURL { get; set; }
        internal string SiteName { get; set; }
        internal string LibraryName { get; set; }
        internal int RetryCount { get; set; }
        internal int RetryWaitSeconds { get; set; }
        internal string _securityDigest { get; set; }
        internal ILogger loggerInstance = null;
        internal bool logEnabled = false;
        internal ICredentials credentials { get; set; }
        internal int _requestCount = 0;

        public int RequestCount { get { return _requestCount; } }

        public void Dispose()
        {
            _securityDigest = null;
        }

        /// <summary>
        /// Gets or sets the logger instance to be used by the File class.
        /// </summary>
        /// <value>The logger instance.</value>
        public ILogger LoggerInstance
        {
            get
            {
                return loggerInstance;
            }
            set
            {
                loggerInstance = value;
                logEnabled = (loggerInstance != null) ? true : false;
            }
        }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="SPContext"/> class.
        /// </summary>
        /// <param name="spHeader">The sp header.</param>
        public SPContext(SPHeader spHeader)
        {
            if (logEnabled) loggerInstance.Info("Instantiating SharePoint Module");
            SharePointURL = spHeader.Host;
            SiteName = spHeader.Site;
            LibraryName = spHeader.Library;
            RetryCount = spHeader.RetryCount;
            RetryWaitSeconds = spHeader.RetryWaitSeconds;
            LoggerInstance = spHeader.LoggerInstance;

            //CredentialCache credNewCache = new CredentialCache();
            //credNewCache.Add(new Uri(uri), "Negotiate", CredentialCache.DefaultNetworkCredentials);

            credentials = CredentialCache.DefaultCredentials;
            GetSecurityDigestForSite();
        }


        #region "Utils"
        /// <summary>
        /// Builds the payload.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private string BuildPayload(string objectType, Dictionary<string, string> properties)
        {
            //build post body
            StringBuilder sb = new StringBuilder();
            properties.Keys.ToList().ForEach(k => sb.AppendFormat("'{0}': {1}, ", k, properties[k]));
            //return
            return string.Format("{{ '__metadata': {{ 'type': '{0}' }}, {1} }}", objectType, sb.Remove(sb.Length - 2, 2).ToString());

        }
        /// <summary>
        /// Builds the payload.
        /// </summary>
        /// <param name="objectType">Type of the object.</param>
        /// <param name="properties">The properties.</param>
        /// <returns></returns>
        private string BuildPayload(ObjectType objectType, Dictionary<string, string> properties)
        {
            //initialization
            string type = string.Empty;
            //determine object type
            switch (objectType)
            {
                //list
                case ObjectType.List:
                    type = "SP.List";
                    break;
                case ObjectType.Folder:
                    type = "SP.Folder";
                    break;
                case ObjectType.Item:
                    type = "SP.Item";
                    break;
                case ObjectType.File:
                    type = "SP.File";
                    break;
            }
            return BuildPayload(type, properties);
        }

        /// <summary>
        /// Contacts SharePoint and does the request.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="uri">The URI.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="additionalHeaders">Any additional headers.</param>
        /// <returns></returns>
        internal string DoRequest(
            Operation operation,
            string uri,
            SPPayloadContent payload,
            Dictionary<string, string> additionalHeaders)
        {
            HttpWebRequest request;
            HttpWebResponse httpWebResponse;
            int retryAttempts = 1;
            string retVal;



            while (retryAttempts <= RetryCount)
            {
                try
                {


                    request = (HttpWebRequest)WebRequest.Create(uri);
                    _requestCount++;
                    request.kSetupRequestHeaderByOperation(operation, additionalHeaders);
                    //set auth
                    if (!string.IsNullOrWhiteSpace(_securityDigest))
                    {
                        request.Headers.Add("X-RequestDigest", _securityDigest);
                        request.Credentials = CredentialCache.DefaultCredentials.GetCredential(new Uri(uri), "NTLM");
                        if (credentials == null)
                            LoggerInstance.Warning("LOST CREDENTIALS!");

                    }
                    else
                    {
                        request.UseDefaultCredentials = true;
                        LoggerInstance.Warning("Using Default Credentials!");
                    }

                    //handle post
                    if (request.Method.Equals("POST") && payload != null)
                    {

                        byte[] postByte = Encoding.UTF8.GetBytes(payload.Content);
                        request.ContentLength = postByte.Length;
                        Stream postStreamBody = request.GetRequestStream();
                        postStreamBody.Write(postByte, 0, postByte.Length);
                        postStreamBody.Close();

                    }



                    //make the call
                    // var webResponse = (WebResponse) request.GetResponse();
                    httpWebResponse = (HttpWebResponse)request.GetResponse();
                    if (httpWebResponse.StatusCode == HttpStatusCode.NoContent || httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        retVal = GetHTTPResponse(httpWebResponse);
                        return String.IsNullOrEmpty(retVal) ? "OK" : retVal;
                    }
                    return "OK";

                }
                catch (WebException we)
                {

                    if (we.Status == WebExceptionStatus.ReceiveFailure)
                    {
                        LoggerInstance.Warning("REST request exceeded usage limits. Sleeping for {0} seconds before retrying. Attemp #{1}", RetryWaitSeconds, retryAttempts);
                        //Add delay.
                        Thread.Sleep(RetryWaitSeconds * 1000);
                        retryAttempts++;
                    }
                    else
                    {
                        throw we;
                    }
                }
            }
            return "MaxRetryFailed";
        }

        /// <summary>
        /// Does the request.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="scope">The scope.</param>
        /// <param name="queryOp">The query op.</param>
        /// <param name="queryOpParameters">The query op parameters.</param>
        /// <param name="payload">The payload.</param>
        /// <param name="additionalHeaders">The additional headers.</param>
        /// <returns></returns>
        internal string DoRequest(
            Operation operation,
            Scope scope,
            string queryOp,
            string queryOpParameters,
            SPPayloadContent payload,
            Dictionary<string, string> additionalHeaders)
        {
            //initialization
            string middlePart = string.Empty;
            switch (scope)
            {
                //web
                case Scope.Web:
                    middlePart = "web";
                    break;
                //site
                case Scope.Site:
                    middlePart = "site";
                    break;
                //list
                case Scope.Lists:
                    middlePart = "lists";
                    break;
                case Scope.Folders:
                    middlePart = "Web/folders";
                    break;
            }
            //build request
            var digestRequest =
                String.Format("{0}/{1}/_api/{2}{3}{4}",
                SharePointURL,
                SiteName,
                middlePart,
                queryOp == String.Empty ? queryOp : "/" + queryOp,
                queryOpParameters == String.Empty ? queryOpParameters : "/" + queryOpParameters);
            if (logEnabled) loggerInstance.Debug("digestRequest: {0}", digestRequest);
            string retVal;
            HttpWebRequest request;
            HttpWebResponse httpWebResponse;
            int retryAttempts = 1;

            string uri;

            while (retryAttempts <= RetryCount)
            {
                try
                {

                    request = (HttpWebRequest)WebRequest.Create(digestRequest);
                    _requestCount++;
                    request.kSetupRequestHeaderByOperation(operation, additionalHeaders);
                    uri = string.Format("{0}/{1}", SharePointURL, SiteName);
                    //set auth
                    if (!string.IsNullOrWhiteSpace(_securityDigest))
                    {
                        request.Headers.Add("X-RequestDigest", _securityDigest);
                        request.Credentials = CredentialCache.DefaultNetworkCredentials.GetCredential(new Uri(uri), "NTLM");
                        if (credentials == null)
                            LoggerInstance.Warning("LOST CREDENTIALS!");

                    }
                    else
                    {
                        request.UseDefaultCredentials = true;
                        LoggerInstance.Warning("Using Default Credentials!");
                    }

                    //handle post
                    if (request.Method.Equals("POST", StringComparison.InvariantCultureIgnoreCase) && payload != null)
                    {
                        byte[] bytes = null;
                        if (payload.PayloadType == PayloadType.Content)
                        {
                            bytes = Encoding.UTF8.GetBytes(payload.Content);
                            request.ContentLength = bytes.Length;
                        }
                        else if (payload.PayloadType == PayloadType.File)
                        {
                            request.Headers.Add("binaryStringRequestBody", "true");
                            bytes = File.ReadAllBytes(payload.Content);
                            request.ContentLength = bytes.Length;
                            request.KeepAlive = true;
                            // LoggerInstance.Info("File Size: {0}", bytes.Length);
                        }


                        using (var writeStream = request.GetRequestStream())
                        {
                            writeStream.Write(bytes, 0, bytes.Length);
                        }
                    }

                    //make the call
                    // var webResponse = (WebResponse) request.GetResponse();
                    httpWebResponse = (HttpWebResponse)request.GetResponse();
                    if (httpWebResponse.StatusCode == HttpStatusCode.NoContent || httpWebResponse.StatusCode == HttpStatusCode.OK)
                    {
                        retVal = GetHTTPResponse(httpWebResponse);
                        return String.IsNullOrEmpty(retVal) ? "OK" : retVal;
                    }
                    return "OK";

                }
                catch (WebException we)
                {

                    if (we.Status == WebExceptionStatus.ReceiveFailure)
                    {
                        LoggerInstance.Warning("REST request exceeded usage limits. Sleeping for {0} seconds before retrying. Attemp #{1}", RetryWaitSeconds, retryAttempts);
                        //Add delay.
                        Thread.Sleep(RetryWaitSeconds * 1000);
                        retryAttempts++;
                    }
                    else
                    {
                        throw we;
                    }
                }

            }
            return "MaxRetryFailed";
        }



        /// <summary>
        /// Gets the HTTP response.
        /// </summary>
        /// <param name="endpointResponse">The endpoint response.</param>
        /// <returns></returns>
        private String GetHTTPResponse(HttpWebResponse endpointResponse)
        {
            string retVal = String.Empty;
            // grab the response
            using (var responseStream = endpointResponse.GetResponseStream())
            {
                if (responseStream != null)
                    using (var reader = new StreamReader(responseStream))
                    {
                        retVal = reader.ReadToEnd();
                    }
            }

            return retVal;
        }

        /// <summary>
        /// Gets the security digest for the site.
        /// </summary>
        internal void GetSecurityDigestForSite()
        {
            GetSecurityDigestForSite(SiteName);
        }

        /// <summary>
        /// Gets the security form digest from a null call to the SharePoint site.
        /// </summary>
        /// <param name="postStream">The post stream.</param>
        /// <returns></returns>
        internal void GetSecurityDigestForSite(string site)
        {
            try
            {
                String results;
                var credCache = new CredentialCache();

                string formdigestRequest = String.Format("{0}/{1}/_api/contextinfo", SharePointURL, site);
                if (logEnabled) loggerInstance.Info("Getting Security Digest for Site {0}", formdigestRequest);

                //credCache.Add(new Uri(formdigestRequest), "Negotiate", CredentialCache.DefaultNetworkCredentials);
                var spRequest = (HttpWebRequest)WebRequest.Create(formdigestRequest);
                _requestCount++;
                //spRequest.Credentials = credentials;
                spRequest.UseDefaultCredentials = true;
                spRequest.Method = "POST";
                spRequest.Accept = "application/json;odata=verbose";
                spRequest.ContentLength = 0;

                var endpointResponse = (HttpWebResponse)spRequest.GetResponse();
                results = GetHTTPResponse(endpointResponse);

                var jsonResult = JsonConvert.DeserializeObject<RootobjectContextWebInfo>(results);
                _securityDigest = jsonResult.d.GetContextWebInformation.FormDigestValue;
            }
            catch (Exception ex)
            {
                if (logEnabled) loggerInstance.Error("Failed to get Security Digest: {0}", ex.kGetAllMessages());
            }
        }
        #endregion
    }


}

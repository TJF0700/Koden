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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Koden.Utils.SharePoint
{
    /// <summary>
    /// SharePoint context for RESTful calls - Item class
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class SPContext
    {

        #region "Items"

        public List<T> Item_GetItems<T>(string listName, string queryOpParameters = "")
        {
            //Single List http://server/site/_api/lists/getbytitle(‘listname’)/items 
            var queryOp =
                 String.Format("getbytitle('{0}')/items{1}", listName, queryOpParameters);

            var retVal = DoRequest(Operation.READ, Scope.Lists, queryOp, "", null, null);

            JObject o = JObject.Parse(retVal);
            IList<JToken> results = o["d"]["results"].Children().ToList();
            var finalResults = new List<T>();

            foreach (var result in results)
            {
               var searchResult = JsonConvert.DeserializeObject<T>(result.ToString());
                finalResults.Add(searchResult);
            }

            return finalResults; 

        }

        public string Item_GetItems(string listName, string queryOpParameters="")
        {

            //Single List http://server/site/_api/lists/getbytitle(‘listname’)/items 

                var queryOp =
                     String.Format("getbytitle('{0}')/items{1}", listName,queryOpParameters);

                var retVal = DoRequest(Operation.READ, Scope.Lists, queryOp, "", null, null);

                JObject o = JObject.Parse(retVal);
                o.AddAnnotation(new HashSet<string>());
                o.PropertyChanged += (sender, args) => o.Annotation<HashSet<string>>().Add(args.PropertyName);

                IList<JToken> results = o["d"]["results"].Children().ToList();



                return ""; // JsonConvert.DeserializeObject<DefaultSPFolderList>(retVal).d.results;
        }
        /// <summary>
        /// Get all custom fields for an Item.
        /// </summary>
        /// <param name="digestRequest">The digest request.</param>
        /// <returns></returns>
        public bool Item_GetAllCustomFields(string digestRequest)
        {
            string retVal;
            var credNewCache = new CredentialCache();

            //  digestRequest ="ht_tp://repository-dev.fmgllc.com/sites/EMRDown/_api/Web/Lists(guid'78e9a260-3297-44e9-a60d-233d889f169d')/Items(7828)/Fields";

            digestRequest += "/Fields?$select=ListItemAllFields,*&$expand=ListItemAllFields";

            var spNewRequest = (HttpWebRequest)HttpWebRequest.Create(digestRequest);
            credNewCache.Add(new Uri(digestRequest), "NTLM", CredentialCache.DefaultNetworkCredentials);
            spNewRequest.Credentials = credNewCache;
            spNewRequest.Method = "GET";
            spNewRequest.Accept = "application/json;odata=verbose";

            var webResponse = (HttpWebResponse)spNewRequest.GetResponse();
            retVal = GetHTTPResponse(webResponse);

            return true;
        }

        /// <summary>
        /// Get information about an Item.
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="libraryName">Name of the library.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="includAllItemFields">if set to <c>true</c> [includ all item fields].</param>
        /// <returns>DefaultSPItemInfo</returns>
        public DefaultSPItemInfo Item_GetInfo(string siteName, string libraryName, string folderName, string itemName, bool includAllItemFields)
        {
            string opQueryParams = "";
            string retVal;
            var opQuery =
                String.Format("GetFileByServerRelativeUrl('/{0}/{1}{2}{3}')",
                    siteName,
                    libraryName,
                    folderName == String.Empty ? folderName : "/" + folderName,
                    itemName == String.Empty ? itemName : "/" + itemName);

            try
            {
                retVal = DoRequest(Operation.READ, Scope.Web, opQuery, opQueryParams, null, null);
            }
            catch (Exception ex)
            {
                var jsonRet = new DefaultSPItemInfo
                {
                    d = new SPItemInfo { Exists = false, Title = ex.Message.ToString()}
                };
                return jsonRet;
            }

            if (includAllItemFields) // call again to get extended properties once we now item exists
            {
                opQueryParams = "?$select=ListItemAllFields,*&$expand=ListItemAllFields";
                retVal = DoRequest(Operation.READ, Scope.Web, opQuery, opQueryParams, null, null);
            }

            return JsonConvert.DeserializeObject<DefaultSPItemInfo>(retVal);
        }

        public bool Item_UpdateMetadata(string siteName, string libraryName, string folderName, string itemName, string newTitle)
        {
            DefaultSPItemInfo itemInfo = new DefaultSPItemInfo();
            itemInfo = Item_GetInfo(siteName, libraryName, folderName, itemName, true);
            Item_UpdateMetadata(itemInfo, newTitle);
            return true;
        }
        public bool Item_UpdateMetadata(DefaultSPItemInfo itemInfo, string newTitle)
        {
            string retVal;
            var credNewCache = new CredentialCache();

            // 1st request to get the context information
            GetSecurityDigestForSite();

            //actually upload file
            var digestRequest = itemInfo.d.__metadata.uri;

            HttpWebRequest spNewRequest = (HttpWebRequest)HttpWebRequest.Create(digestRequest);
            credNewCache.Add(new Uri(digestRequest), "NTLM", CredentialCache.DefaultNetworkCredentials);
            spNewRequest.Credentials = credNewCache;
            spNewRequest.Method = "POST";
            spNewRequest.Accept = "application/json;odata=verbose";
            spNewRequest.ContentType = "application/json;odata=verbose";
            spNewRequest.Headers.Add("X-RequestDigest", _securityDigest);
            spNewRequest.Headers.Add("IF-MATCH", "*");
            spNewRequest.Headers.Add("X-HTTP-Method", "MERGE");
            //            var listPostBody = String.Format("{{'__metadata':{{'type':'{0}'}},'Center Name':'{1}'}}", itemInfo.d.__metadata.type, newTitle);
            var listPostBody = String.Format("{{'__metadata':{{'type':'{0}'}}}}", itemInfo.d.__metadata.type, newTitle);

            // For Content Length
            byte[] postByte = Encoding.UTF8.GetBytes(listPostBody);
            spNewRequest.ContentLength = postByte.Length;
            Stream postStreamBody = spNewRequest.GetRequestStream();
            postStreamBody.Write(postByte, 0, postByte.Length);
            postStreamBody.Close();

            var webResponse = (HttpWebResponse)spNewRequest.GetResponse();
            retVal = GetHTTPResponse(webResponse);

            var jsonResult = JsonConvert.DeserializeObject<DefaultSPItemInfo>(retVal);
            return jsonResult.d.Exists;
            //return "true";
        }

        /// <summary>
        /// Deletes an item in a list
        /// </summary>
        /// <param name="siteName">Name of the site.</param>
        /// <param name="libraryName">Name of the library.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="itemTitle">The item title.</param>
        /// <returns></returns>
        internal string Item_Delete(string siteName, string libraryName, string folderName, string itemTitle)
        {
            DefaultSPItemInfo itemInfo = new DefaultSPItemInfo();
            itemInfo = Item_GetInfo(siteName, libraryName, folderName, itemTitle, false);
            if(itemInfo.d.Exists)
                return Item_Delete(itemInfo);
            
            return "No such item exists!";
        }

        /// <summary>
        /// Deletes an item in a list
        /// </summary>
        /// <param name="itemInfo">The item information.</param>
        /// <returns></returns>
        internal string Item_Delete(DefaultSPItemInfo itemInfo)
        {
            return DoRequest(Operation.DELETE, itemInfo.d.__metadata.uri, null, null);
        }


        /// <summary>
        /// Checks if an item exists.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="itemTitle">The item title.</param>
        /// <returns></returns>
        internal bool Item_Exists(string folderName, string itemTitle)
        {

            try
            {
             var retVal =  Item_GetInfo(SiteName, LibraryName, folderName, itemTitle, false);
                return retVal.d.Exists;
            }
            catch (WebException ex)
            {
                if (ex.Status == WebExceptionStatus.ProtocolError && ex.Response != null)
                {
                    var resp = (HttpWebResponse)ex.Response;
                    if (resp.StatusCode == HttpStatusCode.NotFound)
                    {
                        return false;
                    }
                }
                return false;
            }

        }
        #endregion
    }
}

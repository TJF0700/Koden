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
using System.Net;
using Newtonsoft.Json;

namespace Koden.Utils.SharePoint
{
    /// <summary>
    /// SharePoint context for RESTful calls - Folder class
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class SPContext
    {

        #region "Folders"
        /// <summary>
        /// Rename a Folder.
        /// </summary>
        /// <param name="curFolder">The current folder.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public bool Folder_Rename(string curFolder, string newFolderName)
        {
            //Check if Folder exists
            if (Folder_Exists(curFolder))
            {
                //Get the folder details
                var curFoldInfo = Folder_GetInfo(curFolder, String.Empty, "ListItemAllFields,FileLeafRef");

                var properties = new Dictionary<string, string>();
                properties.Add("Title", String.Format("'{0}'", newFolderName));
                properties.Add("FileLeafRef", String.Format("'{0}'", newFolderName));

                var plContent = BuildPayload(curFoldInfo.d.ListItemAllFields.__metadata.type, properties);
                var payload = new SPPayloadContent { Content = plContent, PayloadType = PayloadType.Content };
                //ACtually rename the folder using content payload
                var retVal = DoRequest(Operation.UPDATE, curFoldInfo.d.ListItemAllFields.__metadata.uri, payload, null);

                //Check to see if newly renamed folder exists
                return Folder_Exists(LibraryName, newFolderName);
            }

            return false;
        }


        /// <summary>
        /// Returns a list of Folders that exist within a library folder
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public jsonFolder[] Folder_GetFolders(string folderName)
        {

            if (Folder_Exists(folderName))
            {
                var queryOp =
                     String.Format("GetFolderByServerRelativeUrl('{0}/{1}')/Folders", LibraryName, folderName);

                var retVal = DoRequest(Operation.READ, Scope.Web, queryOp, "", null, null);
                return JsonConvert.DeserializeObject<DefaultSPFolderList>(retVal).d.results;
            }

            return null;
        }

        /// <summary>
        /// Checks if Folder Exists.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <returns></returns>
        public bool Folder_Exists(string folderName)
        {
            return Folder_Exists(LibraryName, folderName);
        }

        private bool Folder_Exists(string libName, string folderName)
        {

            try
            {
                Folder_GetInfo(folderName, String.Empty, "ListItemAllFields");
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
            return true;
        }

        /// <summary>
        /// Folder_s the get information.
        /// </summary>
        /// <param name="SiteName">Name of the site.</param>
        /// <param name="LibraryName">Name of the library.</param>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="strProperty">The string property.</param>
        /// <param name="populateSubs">The populate subs.</param>
        /// <returns></returns>
        public DefaultSPFolderInfo Folder_GetInfo(string folderName, string strProperty, string populateSubs)
        {

            string opQueryParams = String.Empty;

            folderName = folderName == "/" ? "" : "/" + folderName; // handle root folder

            if (folderName.Equals("/")) folderName = "";
            var opQuery =
                String.Format("GetFolderByServerRelativeUrl('/{0}/{1}{2}')", SiteName, LibraryName, folderName);

            if (!String.IsNullOrEmpty(strProperty)) opQuery += "/" + strProperty;

            if (!String.IsNullOrEmpty(populateSubs)) // call again to get extended properties once we now item exists
            {
                opQueryParams = String.Format("?$select={0},*&$expand={0}", populateSubs);
            }

            var retVal = DoRequest(Operation.READ, Scope.Web, opQuery, opQueryParams, null, null);

            return JsonConvert.DeserializeObject<DefaultSPFolderInfo>(retVal);
        }

        /// <summary>
        /// Create a folder in the spHeader speciified library.
        /// </summary>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        public string Folder_Create(string newFolderName)
        {
            return Folder_Create(SiteName, LibraryName, newFolderName);
        }

        /// <summary>
        /// Create a folder in the spHeader speciified library.
        /// </summary>
        /// <param name="SiteName">Name of the site.</param>
        /// <param name="LibraryName">Name of the library.</param>
        /// <param name="newFolderName">New name of the folder.</param>
        /// <returns></returns>
        private string Folder_Create(string SiteName, string LibraryName, string newFolderName)
        {
            var properties = new Dictionary<string, string>();
            properties.Add("ServerRelativeUrl",
                String.Format("'/{0}/{1}/{2}'", SiteName, LibraryName, newFolderName));
            var plContent = BuildPayload(ObjectType.Folder, properties);

            var payload = new SPPayloadContent { Content = plContent, PayloadType = PayloadType.Content };

            return DoRequest(Operation.CREATE, Scope.Folders, String.Empty, String.Empty, payload, null);
        }

        #endregion
    }
}

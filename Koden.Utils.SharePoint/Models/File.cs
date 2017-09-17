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
using System.IO;
using Koden.Utils.Models;
using Newtonsoft.Json;
using Koden.Utils.Extensions;

namespace Koden.Utils.SharePoint
{
    /// <summary>
    /// SharePoint context for RESTful calls - File class
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class SPContext
    {

        #region "Files"

        /// <summary>
        /// Deletes the file.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public FWRetVal DeleteFile(string folderName, string fileName)
        {
            var retVal = Item_Delete(SiteName, LibraryName, folderName, fileName);

            if (retVal == "OK")
            {
            if(logEnabled) LoggerInstance.Verbose("Successfully deleted file {0}/{1}", folderName, fileName);
                return new FWRetVal { MsgType = FWMsgType.Success, Value = String.Format("Successfully deleted file {0}/{1}", folderName, fileName) };
            }

            if (logEnabled) LoggerInstance.Verbose("Failed to delete file {0}/{1}: {2} ", folderName, fileName, retVal);
            return new FWRetVal { MsgType = FWMsgType.Error, Value = String.Format("Failed to delete file {0}/{1}: {2} ", folderName, fileName, retVal) };
        }

        /// <summary>
        /// File_s the upload to library.
        /// </summary>
        /// <param name="folderName">Name of the folder.</param>
        /// <param name="fileToUpload">The file to upload.</param>
        /// <param name="saveAsName">Name of the save as.</param>
        /// <param name="strTitle">The STR title.</param>
        /// <returns></returns>
        public DefaultSPItemInfo PutFile(string folderName, string fileToUpload, string saveAsName, string strTitle)
        {
            return PutFile(LibraryName, folderName, fileToUpload, saveAsName, strTitle);
        }

        /// <summary>
        /// File_s the upload to library.
        /// </summary>
        /// <param name="libraryName">Name of the SP document library.</param>
        /// <param name="folderName">Name of the folder. - empty/null if none</param>
        /// <param name="fileToUpload">The file to upload. path\filename.ext</param>
        /// <param name="saveAsName">Name of the save as. empty/null if same as uploaded name</param>
        /// <param name="strTitle">The title to give it in the site if applicable.</param>
        /// <returns></returns>
        public DefaultSPItemInfo PutFile(string libName, string folderName, string fileToUpload, string saveAsName, string strTitle)
        {
            if (logEnabled) LoggerInstance.Info("Uploading file {0} to {1}/{2}/{3}", fileToUpload, libName, folderName, saveAsName);

            var finalFolder = libName;
            if (!String.IsNullOrEmpty(folderName)) finalFolder += "/" + folderName;
            if (String.IsNullOrEmpty(saveAsName)) saveAsName = Path.GetFileName(fileToUpload);

            var queryOp =
                   String.Format("GetFolderByServerRelativeUrl('{0}')", finalFolder);

            var queryOpParam =
                 String.Format("Files/add(overwrite=true, url='{0}')", saveAsName);

            var payload = new SPPayloadContent { Content = fileToUpload, PayloadType = PayloadType.File };

            try
            {
                var retVal = DoRequest(Operation.UPLOAD, Scope.Web, queryOp, queryOpParam, payload, null);
                return JsonConvert.DeserializeObject<DefaultSPItemInfo>(retVal);
            }
            catch (Exception ex)
            {
                var jsonRet = new DefaultSPItemInfo
                {
                    d = new SPItemInfo { Exists = false, Title = ex.Message.ToString()}
                };

                if (logEnabled)
                {
                    LoggerInstance.Error("Failed to upload file {0}: {1}", fileToUpload, ex.kGetAllMessages());
                }

                return jsonRet;
            }
        }



        #endregion
    }
}

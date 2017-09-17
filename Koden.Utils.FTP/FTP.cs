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
using Koden.Utils.Models;
using FluentFTP;
using Koden.Utils.Interfaces;
using Koden.Utils.Extensions;

namespace Koden.Utils.FTPClient
{
    /// <summary>
    /// This is made static because it simply wraps System.Net.FtpClient and standardizes on calls
    /// The disposal of instatiated FTP calls is handled within this class.
    /// </summary>
    public static class FTP
    {
       
        private static List<string> _files = null;
        private static ILogger loggerInstance = null;
        private static Boolean logEnabled = false;

         
        /// <summary>
        /// Gets or sets the logger instance to be used by the File class.
        /// </summary>
        /// <value>The logger instance.</value>
        public static ILogger LoggerInstance
        {
            get
            {
                return loggerInstance;
            }
            set
            {
                loggerInstance = value;
                logEnabled = true;
            }
        }




        /// <summary>
        /// Check if File Exists on remote FTP Server
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="serverFile">The server file.</param>
        /// <returns></returns>
        public static FWRetVal FileExists(FTPHeader ftpHeader, string serverFile)
        {
            return FileExists(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password, serverFile);
        }

        /// <summary>
        /// Check if File exists.
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">Name of the STR user.</param>
        /// <param name="strPassword">The STR password.</param>
        /// <param name="serverFile">The server file.</param>
        /// <returns></returns>
        public static FWRetVal FileExists(string strHost, string strUserName, string strPassword, string serverFile)
        {
            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Remote File Exists: " + serverFile };

            using (FtpClient ftpClient = new FtpClient())
            {
                ftpClient.Host = strHost;
                ftpClient.Credentials = new NetworkCredential(strUserName, strPassword);

                if (FileExists(ftpClient, serverFile)) return retVal;
            }

            return new FWRetVal { MsgType = FWMsgType.Error, Value = "Remote File Does NOT Exists: " + serverFile };
        }

        private static bool FileExists(FtpClient ftpClient, string serverFile)
        {
            return ftpClient.FileExists(serverFile);
        }

        /// <summary>
        /// Uploads a file to the remote FTP Server.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="localFile">The local file.</param>
        /// <param name="serverDest">The server file.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns></returns>
        public static FWRetVal PutFile(FTPHeader ftpHeader, string localFile, string serverDest, bool overwrite = false)
        {
            return PutFile(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password, localFile, serverDest, overwrite);
        }

        /// <summary>
        /// Uploads a file to the remote FTP Server.
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">UserName to use</param>
        /// <param name="strPassword">Password to use.</param>
        /// <param name="localFile">The local file (Path and filename).</param>
        /// <param name="serverDest">The server file (Path and filename).</param>
        /// <param name="overwrite">overwrite?</param>
        /// <returns></returns>
        public static FWRetVal PutFile(string strHost, string strUserName,
            string strPassword, string localFile, string serverDest, bool overwrite = false)
        {
            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Uploaded File from {0} to {1}" };

            using (FtpClient ftpClient = new FtpClient())
            {
                if (!OpenConnection(ftpClient, strHost, strUserName, strPassword, retVal)) return retVal;
                try
                {
                    using (var fileStream = File.OpenRead(localFile))
                    using (var ftpStream = ftpClient.OpenWrite(serverDest))
                    {
                        var buffer = new byte[8 * 1024];
                        int count;
                        while ((count = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            ftpStream.Write(buffer, 0, count);
                        }
                    }
                    retVal.Value = String.Format(retVal.Value, localFile, serverDest);
                    if (loggerInstance != null) loggerInstance.Verbose(retVal.Value);
                    return retVal;
                }
                catch (Exception ex)
                {
                    retVal.MsgType = FWMsgType.Error;
                    retVal.Value = String.Format("Error: unable to upload file: ({0})", ex.kGetAllMessages());
                    if (loggerInstance != null) loggerInstance.Error(retVal.Value);
                    return retVal;
                }
            }

        }

        /// <summary>
        /// Gets a file from the remote FTP server.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="serverFile">The server file.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <param name="deleteAfterRetrieve">The delete after retrieve.</param>
        /// <returns></returns>
        public static FWRetVal GetFile(FTPHeader ftpHeader, string serverPath, string serverFile, string localDestPath, bool deleteAfterRetrieve = false)
        {
            return GetFile(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password, serverPath, serverFile, localDestPath, deleteAfterRetrieve);
        }

        /// <summary>
        /// Gets a file from the remote FTP server.
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">Name of theuser.</param>
        /// <param name="strPassword">The password.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="serverFile">The server file.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <param name="deleteAfterRetrieve">The delete after retrieve.</param>
        /// <returns></returns>
        public static FWRetVal GetFile(string strHost, string strUserName, string strPassword,
            string serverPath, string serverFile, string localDestPath, bool deleteAfterRetrieve = false)
        {

            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Downloaded File {0} to {1}" };

            using (var ftpClient = new FtpClient())
            {
                if (!OpenConnection(ftpClient, strHost, strUserName, strPassword, retVal)) return retVal;


                var serverFileFull = serverPath + "/" + serverFile;
                if (loggerInstance != null) loggerInstance.Verbose("Attempting to get file {0}", serverFileFull);
                var destinationPath = Path.Combine(localDestPath, serverFile);

                if (FileExists(ftpClient, serverFileFull))
                {
                    using (var ftpStream = ftpClient.OpenRead(serverFileFull))
                    using (var fileStream = File.Create(destinationPath, (int) ftpStream.Length))
                    {
                        var buffer = new byte[8 * 1024];
                        int count;
                        while ((count = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, count);
                        }

                        // In this example, we're deleting the file after downloading it.
                        if (deleteAfterRetrieve) ftpClient.DeleteFile(serverFileFull);
                    }
                    retVal.Value = String.Format(retVal.Value, serverFile, destinationPath);
                    if (loggerInstance != null) loggerInstance.Verbose(retVal.Value);
                    return retVal;
                }
                if (loggerInstance != null) loggerInstance.Error("File not found on server: ", serverFileFull);
            }


            return new FWRetVal { MsgType = FWMsgType.Error, Value = "Remote File Does NOT Exists: " + serverFile };
        }

        /// <summary>
        /// Gets all the files in a remote directory with an optional filter on the extension.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="serverPath">The Server Path to get files from</param>
        /// <param name="localDestPath">The local dest path to save files to.</param>
        /// <param name="filterType">Type of the filter (None, StartsWith, etc).</param>
        /// <param name="filter">The filter. - (do not use * or %)</param>
        /// <param name="keepOrigDateTime">if set to <c>true</c> [keep original date time].</param>
        /// <param name="deleteAfterRetrieve">if set to <c>true</c> [delete after retrieve].</param>
        /// <returns></returns>
        public static FWRetVal GetFiles(FTPHeader ftpHeader, string serverPath,
                                        string localDestPath, FTPFilterType filterType, string filter = null,
                                        bool keepOrigDateTime = false, bool deleteAfterRetrieve = false)
        {
            return GetFiles(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password, serverPath, localDestPath, filterType, filter, keepOrigDateTime, deleteAfterRetrieve);
        }

        /// <summary>
        /// Gets all the files in a remote directory with an optional filter
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">UserName to use</param>
        /// <param name="strPassword">Password to use</param>
        /// <param name="serverPath">The Server Path to get files from</param>
        /// <param name="localDestPath">The local dest path to save files to.</param>
        /// <param name="filterType">Type of the filter (None, StartsWith, etc).</param>
        /// <param name="filterText">The filter. - (do not use * or %)</param>
        /// <param name="keepOrigDateTime">if set to <c>true</c> [keep original date time].</param>
        /// <param name="deleteAfterRetrieve">if set to <c>true</c> [delete after retrieve].</param>
        /// <returns></returns>
        public static FWRetVal GetFiles(string strHost, string strUserName,
                                        string strPassword, string serverPath,
                                        string localDestPath, FTPFilterType filterType, string filterText = null,
                                        bool keepOrigDateTime = false, bool deleteAfterRetrieve = false)
        {
            StringBuilder sbFiles = new StringBuilder();
            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Downloaded files: {0}" };

            using (var ftpClient = new FtpClient())
            {
                if (loggerInstance != null) loggerInstance.Verbose("Attempting to connect to '{0}'", strHost);
                if (!OpenConnection(ftpClient, strHost, strUserName, strPassword, retVal)) return retVal;
                if (loggerInstance != null) loggerInstance.Verbose("Connected to '{0}' using FTP", strHost);

                IEnumerable<FtpListItem> fileList = FilterFiles(ftpClient, serverPath, filterType, filterText);
                

                foreach (var ftpListItem in fileList)
                {
                    var destinationPath = Path.Combine(localDestPath, ftpListItem.Name);

                    using (var ftpStream = ftpClient.OpenRead(ftpListItem.FullName))
                    using (var fileStream = File.Create(destinationPath, (int) ftpStream.Length))
                    {
                        var buffer = new byte[8 * 1024];
                        int count;
                        while ((count = ftpStream.Read(buffer, 0, buffer.Length)) > 0)
                        {
                            fileStream.Write(buffer, 0, count);
                        }
                        if (loggerInstance != null) loggerInstance.Verbose("Downloaded file {0} from {1} into {2}", ftpListItem.Name, serverPath, destinationPath);
                        sbFiles.Append(ftpListItem.Name + ",");
                        // In this example, we're deleting the file after downloading it.
                        if (deleteAfterRetrieve) ftpClient.DeleteFile(ftpListItem.FullName);
                    }
                }
                retVal.Value = sbFiles.ToString().TrimEnd(',');
                return retVal;
            }


        }

        /// <summary>
        /// Opens the connection to the FTP server with supplied credentials.
        /// </summary>
        /// <param name="ftpClient">The FTP client.</param>
        /// <param name="strHost">The STR host.</param>
        /// <param name="strUserName">Name of the STR user.</param>
        /// <param name="strPassword">The STR password.</param>
        /// <param name="retVal">The ret val.</param>
        /// <returns></returns>
        private static bool OpenConnection(FtpClient ftpClient, string strHost, string strUserName, string strPassword, FWRetVal retVal)
        {
            try
            {
                ftpClient.Host = strHost;
                ftpClient.Credentials = new NetworkCredential(strUserName, strPassword);
                ftpClient.Connect();
                if (loggerInstance != null)
                    loggerInstance.Verbose("Connected to host: {0} ", ftpClient.Host);
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Could not connect to host: {0}", ex.kGetAllMessages());
                if (loggerInstance != null) loggerInstance.Error(retVal.Value);
                return false;
            }

            return true;
        }
        #region "CheckForFiles"
        /// <summary>
        /// Checks for files.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="ftpOptions">The FTP options.</param>
        /// <param name="serverPath">The server path.</param>
        /// <returns></returns>
        public static FWRetVal<string> CheckForFiles(FTPHeader ftpHeader, FTPOptions ftpOptions,
           string serverPath)
        {
            StringBuilder sbFiles = new StringBuilder();
            _files = new List<string>();
            int fileCount = 0;

            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Downloaded files: {0}" };


                try
            {
                using (var ftpClient = new FtpClient())
                {
                   
                  //  ftpClient = new SftpClient(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password);
                    ftpClient.Connect();
                    if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", ftpHeader.Host);
                    if (!OpenConnection(ftpClient, ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password, retVal)) return retVal;
                    if (logEnabled)
                    {
                        loggerInstance.Verbose("Connected to '{0}' using FTP\n", ftpHeader.Host);
                        loggerInstance.Separator("FTP Options Used");
                        loggerInstance.Info("\n\tFilterType={0}\n\tFilterText={1}\n\tForceOverwrite={2}\n\tKeepOrigDateTime={3}\n\tDeleteAfterRetrieve={4}\n",
                            ftpOptions.FilterType, ftpOptions.FilterText, ftpOptions.ForceOverwrite, ftpOptions.KeepOrigDateTime, ftpOptions.DeleteAfterRetrieve);
                    }

                    var fileList = FilterFiles(ftpClient, serverPath, ftpOptions.FilterType, ftpOptions.FilterText);

                    if (logEnabled) loggerInstance.Separator("Begin File Downloads");
                    foreach (var ftpListItem in fileList)
                    {
                        if (ftpListItem.Name.EndsWith(".")) continue;
                        _files.Add(ftpListItem.Name);
                        fileCount++;

                    }
                    if (_files.Count > 0) retVal.Value = "Files Found on remote host";
                    else retVal.Value = "No files found on remote host";
                    retVal.Record = fileCount.ToString();
                    retVal.Records = _files;
                }

                return retVal;
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Unable to find files from {0}: {1}", serverPath, ex.kGetAllMessages());
                if (logEnabled) loggerInstance.Error(retVal.Value);
                return retVal;
            }

        }
        /// <summary>
        /// Gets all the files in a remote directory with an optional filter on the extension.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="filterType">Type of the filter (None, StartsWith, etc).</param>
        /// <param name="filterText">The filter. - (do not use * or %)</param>
        /// <returns>
        /// FWRetVal string the "Record" contains number of files copied
        /// </returns>
        public static FWRetVal<string> CheckForFiles(FTPHeader ftpHeader, string serverPath,
             FTPFilterType filterType = FTPFilterType.None,
            string filterText = null)
        {
            return CheckForFiles(new FTPHeader { Host = ftpHeader.Host, UserName = ftpHeader.UserName, Password = ftpHeader.Password },
                new FTPOptions
                {
                    FilterType = filterType,
                    FilterText = filterText,
                    DeleteAfterRetrieve = false,
                    KeepOrigDateTime = false
                },
                serverPath);

        }

        /// <summary>
        /// Gets all the files in a remote directory with an optional filter on the extension.
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">UserName to use</param>
        /// <param name="strPassword">Password to use</param>
        /// <param name="serverPath">The Server Path to get files from</param>
        /// <param name="filterType">Type of the filter (None, StartsWith, etc).</param>
        /// <param name="filterText">The filter. - (do not use * or %)</param>
        /// <returns>
        /// FWRetVal int the "Record" contains number of files copied
        /// </returns>
        public static FWRetVal<string> CheckForFiles(string strHost, string strUserName,
            string strPassword, string serverPath,
            FTPFilterType filterType, string filterText = null)
        {

            return CheckForFiles(new FTPHeader { Host = strHost, UserName = strUserName, Password = strPassword },
                            new FTPOptions
                            {
                                FilterType = filterType,
                                FilterText = filterText,
                                ForceOverwrite = false,
                                DeleteAfterRetrieve = false,
                                KeepOrigDateTime = false
                            },
                            serverPath);

        }
        #endregion
        /// <summary>
        /// Filters the files based on selected filtertype.
        /// </summary>
        /// <param name="ftpClient">The FTP client.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filterText">The filter.</param>
        /// <returns></returns>
        private static IEnumerable<FtpListItem> FilterFiles(FtpClient ftpClient, string serverPath, FTPFilterType filterType, string filterText)
        {
            //  IEnumerable<FileInfo> fileList = sDir.GetFiles();
            filterText = filterText.ToLower();
            IEnumerable<FtpListItem> fileList = ftpClient.GetListing(serverPath, FtpListOption.Modify | FtpListOption.Size);
            if (filterType != FTPFilterType.None)
            {
                switch (filterType)
                {
                    case FTPFilterType.StartsWith:
                        fileList = fileList.Where(ftpListItem => ftpListItem.FullName.ToLower().StartsWith(filterText));
                        break;
                    case FTPFilterType.EndsWith:
                        fileList = fileList.Where(ftpListItem => ftpListItem.FullName.ToLower().EndsWith(filterText));
                        break;
                    case FTPFilterType.Contains:
                        fileList = fileList.Where(ftpListItem => ftpListItem.FullName.ToLower().Contains(filterText));
                        break;
                    case FTPFilterType.DateFilterToday:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Modified.ToShortDateString() == DateTime.Now.ToShortDateString());
                        break;
                    case FTPFilterType.DateBefore:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Modified <= Convert.ToDateTime(filterText));
                        break;
                    case FTPFilterType.DateAfter:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Modified >= Convert.ToDateTime(filterText));
                        break;
                    case FTPFilterType.Equals:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Name.ToLower() == filterText);
                        break;
                    case FTPFilterType.DateEquals:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Modified.ToShortDateString() == Convert.ToDateTime(filterText).ToShortDateString());
                        break;
                    default:
                        break;
                }
            }
            return fileList;
        }

    }


}

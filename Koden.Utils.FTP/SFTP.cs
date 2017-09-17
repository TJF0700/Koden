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
using System.Linq;
using System.Text;
using Koden.Utils.Models;
using Renci.SshNet;
using System.IO;
using Renci.SshNet.Sftp;
using Koden.Utils.Interfaces;
using Koden.Utils.Extensions;

namespace Koden.Utils.FTPClient
{
    /// <summary>
    /// This is made static because it simply wraps Renci.SshNet Nuget Package and standardizes on calls
    /// The disposal of instatiated FTP calls is handled within this class.
    /// </summary>
    public static class SFTP
    {
        internal static readonly object threadLock = new object();
        internal static int _fileCount = 0;
        internal static List<string> _files = null;
        internal static ILogger loggerInstance = null;
        internal static bool logEnabled = false;

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
        /// Files the exists.
        /// </summary>
        /// <param name="strHost">The string host.</param>
        /// <param name="strUserName">Name of the string user.</param>
        /// <param name="strPassword">The string password.</param>
        /// <param name="serverFile">The server file.</param>
        /// <returns></returns>
        public static FWRetVal FileExists(string strHost, string strUserName,
                                    string strPassword, string serverFile)
        {
            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Remote File Exists: " + serverFile };

            SftpClient ftpClient = new SftpClient(strHost, strUserName, strPassword);
            try
            {
                ftpClient.Connect();
                SftpFile chkFile = ftpClient.Get(serverFile);
                if (chkFile != null)
                {
                    return retVal;
                }

            }
            catch (Exception ex)
            {

                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Remote File Does NOT exist: {0}", serverFile);

                if (!ex.Message.kContains("File not found", StringComparison.OrdinalIgnoreCase))
                {
                    retVal.Value = ex.kGetAllMessages();
                    if (logEnabled) loggerInstance.Error(retVal.Value);
                }
            }
            finally
            {
                ftpClient.Disconnect();
                ftpClient.Dispose();

            }
            return retVal;
        }


        /// <summary>
        /// Download a file from the host to a local folder and filename
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
        /// Download a file from the host to a local folder and filename
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">Name of the user.</param>
        /// <param name="strPassword">The password.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="serverFile">The file on the server.</param>
        /// <param name="localDestPath">The local destination path (do not include filename).</param>
        /// <param name="deleteAfterRetrieval">delete after retrieval?</param>
        /// <returns></returns>
        public static FWRetVal GetFile(string strHost, string strUserName,
            string strPassword, string serverPath, string serverFile, string localDestPath, bool deleteAfterRetrieval = false)
        {
            var serverFileFull = serverPath + "/" + serverFile;
            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Successfully downloaded file: " + serverFileFull + " and saved it to: " + localDestPath };
            SftpClient ftpClient = default(SftpClient);
            try
            {
                if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", strHost);
                ftpClient = new SftpClient(strHost, strUserName, strPassword);
                ftpClient.Connect();
                if (logEnabled) loggerInstance.Verbose("Connected to '{0}'", strHost);
                if (logEnabled) loggerInstance.Verbose("Attempting to get file {0}", serverFileFull);
                var destinationPath = Path.Combine(localDestPath, serverFile);
                lock (threadLock)
                {
                    using (var dFile = System.IO.File.OpenWrite(destinationPath))
                    {
                        ftpClient.DownloadFile(serverFileFull, dFile);
                    }
                }
                if (deleteAfterRetrieval) ftpClient.DeleteFile(serverFileFull);
                if (logEnabled) loggerInstance.Verbose(retVal.Value);

                return retVal;
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Unable to download file {0}: {1}", serverFileFull, ex.kGetAllMessages());
                if (logEnabled) loggerInstance.Error(retVal.Value);
                return retVal;
            }
            finally
            {
                ftpClient.Disconnect();
                ftpClient.Dispose();
            }


        }
        #region "GetFiles"

        /// <summary>
        /// Gets the files.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="ftpOptions">The FTP options.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <returns></returns>
        public static FWRetVal<string> GetFiles(FTPHeader ftpHeader, FTPOptions ftpOptions,
            string serverPath, string localDestPath)
        {
            StringBuilder sbFiles = new StringBuilder();
            _files = new List<string>();
            int fileCount = 0;

            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Downloaded files: {0}" };

            if (!Directory.Exists(localDestPath))
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = "Local destination directory doesn't exist! " + localDestPath;
                if (loggerInstance != null) loggerInstance.WriteRetVal(retVal);
                return retVal;
            }
            SftpClient ftpClient = default(SftpClient);
            try
            {
                if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", ftpHeader.Host);
                ftpClient = new SftpClient(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password);
                ftpClient.Connect();
                if (logEnabled)
                {
                    loggerInstance.Verbose("Connected to '{0}' using SFTP\n", ftpHeader.Host);
                    loggerInstance.Separator("FTP Options Used");
                    loggerInstance.Info("\n\tFilterType={0}\n\tFilterText={1}\n\tForceOverwrite={2}\n\tKeepOrigDateTime={3}\n\tDeleteAfterRetrieve={4}\n",
                        ftpOptions.FilterType, ftpOptions.FilterText, ftpOptions.ForceOverwrite, ftpOptions.KeepOrigDateTime, ftpOptions.DeleteAfterRetrieve);
                }

                var fileList = FilterFiles(ftpClient, serverPath, ftpOptions.FilterType, ftpOptions.FilterText);

                if (logEnabled) loggerInstance.Separator("Begin File Downloads");
                foreach (var ftpListItem in fileList)
                {


                    if (ftpListItem.Name.EndsWith(".")) continue;
                    var destinationPath = Path.Combine(localDestPath, ftpListItem.Name);

                    lock (threadLock)
                    {
                        using (var dFile = System.IO.File.OpenWrite(destinationPath))
                        {
                            ftpClient.DownloadFile(ftpListItem.FullName, dFile);
                            if (logEnabled) loggerInstance.Verbose("Downloaded file {0} from {1} into {2}", ftpListItem.Name, serverPath, destinationPath);
                            _files.Add(destinationPath);
                            fileCount++;
                        }
                    }

                    if (ftpOptions.KeepOrigDateTime) File.SetCreationTime(destinationPath, ftpListItem.LastWriteTime);
                    // In this example, we're deleting the file after downloading it.
                    if (ftpOptions.DeleteAfterRetrieve) ftpClient.DeleteFile(ftpListItem.FullName);
                }
                if (_files.Count > 0) retVal.Value = "Finished copying files";
                else retVal.Value = "No files found on remote host";
                retVal.Record = fileCount.ToString();
                retVal.Records = _files;
                ftpClient.Disconnect();
                ftpClient.Dispose();
                return retVal;
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Unable to download files from {0}: {1}", serverPath, ex.kGetAllMessages());
                if (logEnabled) loggerInstance.Error(retVal.Value);
                return retVal;
            }

        }
        /// <summary>
        /// Gets all the files in a remote directory with an optional filter on the extension.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <param name="filterType">Type of the filter (None, StartsWith, etc).</param>
        /// <param name="filterText">The filter. - (do not use * or %)</param>
        /// <param name="forceOverwrite">if set to <c>true</c> [force overwrite].</param>
        /// <param name="keepOrigDateTime">if set to <c>true</c> [keep original date time].</param>
        /// <param name="deleteAfterRetrieve">if set to <c>true</c> [delete after retrieve].</param>
        /// <returns>
        /// FWRetVal sring the "Record" contains number of files copied
        /// </returns>
        public static FWRetVal<string> GetFiles(FTPHeader ftpHeader, string serverPath,
            string localDestPath, FTPFilterType filterType = FTPFilterType.None,
            string filterText = null, bool forceOverwrite = false, bool keepOrigDateTime = false, bool deleteAfterRetrieve = false)
        {
            return GetFiles(new FTPHeader { Host = ftpHeader.Host, UserName = ftpHeader.UserName, Password = ftpHeader.Password },
                new FTPOptions
                {
                    FilterType = filterType,
                    FilterText = filterText,
                    ForceOverwrite = forceOverwrite,
                    DeleteAfterRetrieve = deleteAfterRetrieve,
                    KeepOrigDateTime = keepOrigDateTime
                },
                serverPath, localDestPath);

        }

        /// <summary>
        /// Gets all the files in a remote directory with an optional filter on the extension.
        /// </summary>
        /// <param name="strHost">The host.</param>
        /// <param name="strUserName">UserName to use</param>
        /// <param name="strPassword">Password to use</param>
        /// <param name="serverPath">The Server Path to get files from</param>
        /// <param name="localDestPath">The local dest path to save files to.</param>
        /// <param name="filterType">Type of the filter (None, StartsWith, etc).</param>
        /// <param name="filterText">The filter. - (do not use * or %)</param>
        /// <param name="forceOverwrite">if set to <c>true</c> [force overwrite].</param>
        /// <param name="keepOrigDateTime">if set to <c>true</c> touches files with FTP Date/Time and not local Creation Date/Time.</param>
        /// <param name="deleteAfterRetrieve">if set to <c>true</c> [delete after retrieve].</param>
        /// <returns>FWRetVal int the "Record" contains number of files copied</returns>
        public static FWRetVal<string> GetFiles(string strHost, string strUserName,
            string strPassword, string serverPath,
            string localDestPath, FTPFilterType filterType, string filterText = null, bool forceOverwrite = false,
            bool keepOrigDateTime = false, bool deleteAfterRetrieve = false)
        {

            return GetFiles(new FTPHeader { Host = strHost, UserName = strUserName, Password = strPassword },
                            new FTPOptions
                            {
                                FilterType = filterType,
                                FilterText = filterText,
                                ForceOverwrite = forceOverwrite,
                                DeleteAfterRetrieve = deleteAfterRetrieve,
                                KeepOrigDateTime = keepOrigDateTime
                            },
                            serverPath, localDestPath);

        }

        #endregion

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

            SftpClient ftpClient = default(SftpClient);
            try
            {
                if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", ftpHeader.Host);
                ftpClient = new SftpClient(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password);
                ftpClient.Connect();
                if (logEnabled)
                {
                    loggerInstance.Verbose("Connected to '{0}' using SFTP\n", ftpHeader.Host);
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
                ftpClient.Disconnect();
                ftpClient.Dispose();
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
            return CheckForFiles(ftpHeader,
                new FTPOptions
                {
                    FilterType = filterType,
                    FilterText = filterText,
                    DeleteAfterRetrieve = false ,
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
        /// Gets files from SFTP Host recursively.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="ftpOptions">The FTP options.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <returns></returns>
        public static FWRetVal<string> GetFilesRecursively(FTPHeader ftpHeader, FTPOptions ftpOptions,
            string serverPath, string localDestPath)
        {
            StringBuilder sbFiles = new StringBuilder();

            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Downloaded files: {0}" };
            SftpClient ftpClient = default(SftpClient);
            try
            {
                if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", ftpHeader.Host);
                ftpClient = new SftpClient(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password);
                ftpClient.Connect();
                if (logEnabled)
                {
                    loggerInstance.Verbose("Connected to '{0}' using SFTP\n", ftpHeader.Host);
                    loggerInstance.Separator("FTP Options Used");
                    loggerInstance.Info("\n\tFilterType={0}\n\tFilterText={1}\n\tForceOverwrite={2}\n\tKeepOrigDateTime={3}\n\tDeleteAfterRetrieve={4}\n",
                        ftpOptions.FilterType, ftpOptions.FilterText, ftpOptions.ForceOverwrite, ftpOptions.KeepOrigDateTime, ftpOptions.DeleteAfterRetrieve);
                }

                _files = new List<string>();
                retVal = DoRecursiveGet(ftpClient, ftpOptions, localDestPath, serverPath);

                ftpClient.Disconnect();
                ftpClient.Dispose();
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Unable to download files from {0}: {1}", serverPath, ex.kGetAllMessages());
                if (logEnabled) loggerInstance.Error(retVal.Value);
                return retVal;
            }
            retVal.Records = _files;
            return retVal;

        }

        /// <summary>
        /// Gets files from SFTP Host recursively.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <param name="forceOverWrite">if set to <c>true</c> [force over write].</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="keepOrigDateTime">if set to <c>true</c> [keep original date time].</param>
        /// <param name="deleteAfterRetrieve">if set to <c>true</c> [delete after retrieve].</param>
        /// <returns></returns>
        public static FWRetVal<string> GetFilesRecursively(FTPHeader ftpHeader, string serverPath,
                                                    string localDestPath, bool forceOverWrite, FTPFilterType filterType = FTPFilterType.None,
                                                    string filter = null, bool keepOrigDateTime = false, bool deleteAfterRetrieve = false)
        {
            return GetFilesRecursively(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password, serverPath,
                localDestPath, forceOverWrite, filterType, filter, keepOrigDateTime, deleteAfterRetrieve);
        }

        /// <summary>
        /// Gets files from SFTP Host recursively.
        /// </summary>
        /// <param name="strHost">The string host.</param>
        /// <param name="strUserName">Name of the string user.</param>
        /// <param name="strPassword">The string password.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <param name="forceOverWrite">if set to <c>true</c> [force over write].</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="keepOrigDateTime">if set to <c>true</c> [keep original date time].</param>
        /// <param name="deleteAfterRetrieve">if set to <c>true</c> [delete after retrieve].</param>
        /// <returns></returns>
        public static FWRetVal<string> GetFilesRecursively(string strHost, string strUserName,
           string strPassword, string serverPath,
           string localDestPath, bool forceOverWrite, FTPFilterType filterType, string filter = null,
           bool keepOrigDateTime = false, bool deleteAfterRetrieve = false)
        {
            return GetFilesRecursively(new FTPHeader { Host = strHost, UserName = strUserName, Password = strPassword },
                                       new FTPOptions
                                       {
                                           FilterType = filterType,
                                           FilterText = filter,
                                           ForceOverwrite = forceOverWrite,
                                           DeleteAfterRetrieve = deleteAfterRetrieve,
                                           KeepOrigDateTime = keepOrigDateTime
                                       },
                                        serverPath, localDestPath);
        }


        /// <summary>
        /// Does the recursive get.
        /// </summary>
        /// <param name="ftpClient">The FTP client.</param>
        /// <param name="ftpOptions">The FTP options.</param>
        /// <param name="localDestPath">The local dest path.</param>
        /// <param name="serverPath">The server path.</param>
        /// <returns></returns>
        private static FWRetVal<string> DoRecursiveGet(SftpClient ftpClient, FTPOptions ftpOptions, string localDestPath, string serverPath)
        {
            StringBuilder sbFiles = new StringBuilder();

            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Downloaded files: {0}" };
            try
            {
                var dirList = ftpClient.ListDirectory(serverPath).Where(d => d.IsDirectory && !d.FullName.EndsWith("."));
                var tDir = new DirectoryInfo(localDestPath);
                foreach (var dir in dirList)
                {
                    DirectoryInfo targetSubDirectory = tDir.CreateSubdirectory(dir.Name);
                    DoRecursiveGet(ftpClient, ftpOptions, targetSubDirectory.FullName, dir.FullName);
                }

                var fileList = FilterFiles(ftpClient, serverPath, ftpOptions.FilterType, ftpOptions.FilterText);

                foreach (var ftpListItem in fileList)
                {
                    if (ftpListItem.Name.EndsWith(".") || ftpListItem.IsDirectory) continue;
                    var destinationPathAndFile = Path.Combine(localDestPath, ftpListItem.Name);
                    if (logEnabled) loggerInstance.Verbose("Working on file {0}", destinationPathAndFile);
                    if (!ftpOptions.ForceOverwrite && File.Exists(destinationPathAndFile)) continue;

                    lock (threadLock)
                    {
                        using (var dFile = System.IO.File.OpenWrite(destinationPathAndFile))
                        {
                            ftpClient.DownloadFile(ftpListItem.FullName, dFile);
                            if (logEnabled) loggerInstance.Verbose("Downloaded file {0} from {1} into {2}", ftpListItem.Name, serverPath, destinationPathAndFile);
                            _files.Add(destinationPathAndFile);
                        }
                    }
                    //if (keepOrigDateTime) File.SetCreationTime(destinationPath, ftpListItem.LastWriteTime);
                    //// In this example, we're deleting the file after downloading it.
                    //if (deleteAfterRetrieve) ftpClient.DeleteFile(ftpListItem.FullName);
                }
                retVal.Value = "Completed copying files recursively";

                return retVal;
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = String.Format("Unable to download files from {0}: {1}", serverPath, ex.kGetAllMessages());
                if (logEnabled) loggerInstance.Error(retVal.Value);
                return retVal;
            }

        }


        /// <summary>
        /// Filters the files based on selected filtertype.
        /// </summary>
        /// <param name="ftpClient">The FTP client.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filterText">The filter.</param>
        /// <returns></returns>
        private static IEnumerable<SftpFile> FilterFiles(SftpClient ftpClient, string serverPath, FTPFilterType filterType, string filterText)
        {
            //  IEnumerable<FileInfo> fileList = sDir.GetFiles();
            filterText = filterText.ToLower();
            IEnumerable<SftpFile> fileList = ftpClient.ListDirectory(serverPath);
            if (filterType != FTPFilterType.None)
            {
                switch (filterType)
                {
                    case FTPFilterType.StartsWith:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Name.ToLower().StartsWith(filterText));
                        break;
                    case FTPFilterType.EndsWith:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Name.ToLower().EndsWith(filterText));
                        break;
                    case FTPFilterType.Contains:
                        fileList = fileList.Where(ftpListItem => ftpListItem.Name.ToLower().Contains(filterText));
                        break;
                    case FTPFilterType.DateFilterToday:
                        fileList = fileList.Where(ftpListItem => ftpListItem.LastWriteTime.ToShortDateString() == DateTime.Now.ToShortDateString());
                        break;
                    case FTPFilterType.DateBefore:
                        fileList = fileList.Where(ftpListItem => ftpListItem.LastWriteTime <= Convert.ToDateTime(filterText));
                        break;
                    case FTPFilterType.DateAfter:
                        fileList = fileList.Where(ftpListItem => ftpListItem.LastWriteTime >= Convert.ToDateTime(filterText));
                        break;
                    case FTPFilterType.Equals:
                        string[] fList = filterText.Split(':');
                        fileList = fileList.Where(ftpListItem => filterText.Contains(ftpListItem.Name.ToLower()));
                        break;
                    case FTPFilterType.DateEquals:
                        fileList = fileList.Where(ftpListItem => ftpListItem.LastWriteTime.ToShortDateString() == Convert.ToDateTime(filterText).ToShortDateString());
                        break;
                    default:
                        break;
                }
            }
            return fileList;
        }


        /// <summary>
        /// Uploads a file using SFTP Protocol.
        /// </summary>
        /// <param name="ftpHeader">The FTP header.</param>
        /// <param name="localFile">The local file.</param>
        /// <param name="serverDest">The server dest.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">File not found on remote server after upload!</exception>
        public static FWRetVal PutFile(FTPHeader ftpHeader, string localFile, string serverDest)
        {


            var retVal = new FWRetVal { MsgType = FWMsgType.Success, Value = "Successfully uploaded file: " + localFile };

            SftpClient ftpClient = new SftpClient(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password);
            try
            {
                ftpClient.Connect();
                if (logEnabled) loggerInstance.Info("Connected to '{0}' using SFTP\n", ftpHeader.Host);

                if (!string.IsNullOrEmpty(serverDest))
                {
                    ftpClient.ChangeDirectory(serverDest + @"/");
                }
                if (logEnabled) loggerInstance.Verbose("Uploading file: {0} to '{1}'", localFile, serverDest);

                using (var fileStream = new FileStream(localFile, FileMode.Open))
                {
                    ftpClient.BufferSize = 4 * 1024;
                    ftpClient.UploadFile(fileStream, Path.GetFileName(localFile), null);
                }

                // Now check if file actually got uploaded!
                if (ftpClient.Exists(serverDest + @"/" + Path.GetFileName(localFile)))
                {
                    if (logEnabled) loggerInstance.Info("Successfully uploaded file: {0} to '{1}'", localFile, serverDest);
                    return retVal;
                }

                throw new Exception("File not found on remote server after upload!");

            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
                if (logEnabled) loggerInstance.Error(retVal.Value);
            }
            finally
            {
                ftpClient.Disconnect();
                ftpClient.Dispose();
            }

            return retVal;



        }
        /// <summary>
        /// Uploads a file using SFTP protocol and username/password.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password.</param>
        /// <param name="localFile">The local file.</param>
        /// <param name="serverDest">The dest directory.</param>
        /// <returns></returns>
        public static FWRetVal PutFile(string host, string userName,
            string password, string localFile, string serverDest)
        {
            return PutFile(new FTPHeader { Host = host, UserName = userName, Password = password }, localFile, serverDest);

        }

        /// <summary>
        /// Gets List of remote files one level deep on FTP server.
        /// </summary>
        /// <param name="ftpHeader">The FTP Header.</param>
        /// <param name="remoteDir">The remote directory.</param>
        /// <returns>FWRetVal FTPFileInfo "Records" contains file information for each file.</returns>
        public static FWRetVal<FTPFileInfo> GetFilesInfo(FTPHeader ftpHeader, string remoteDir)
        {
            var retVal = new FWRetVal<FTPFileInfo> { MsgType = FWMsgType.Success, Value = "Retrieved file information: {0}" };

            var remoteFiles = new List<FTPFileInfo>();

            var sftpClient = new SftpClient(ftpHeader.Host, ftpHeader.UserName, ftpHeader.Password);

            try
            {
                sftpClient.Connect();

                List<SftpFile> results = sftpClient.ListDirectory(remoteDir).ToList();

                foreach (SftpFile file in results)
                {
                    if (!file.IsDirectory)
                    {
                        remoteFiles.Add(new FTPFileInfo
                        {
                            Name = file.Name,
                            FullName = file.FullName,
                            Extensions = file.Attributes.Extensions,
                            LastAccessTime = file.LastAccessTime,
                            LastAccessTimeUtc = file.LastAccessTimeUtc,
                            LastWriteTime = file.LastWriteTime,
                            LastWriteTimeUtc = file.LastWriteTimeUtc,
                            Length = file.Length
                        });
                    }
                }

                retVal.Records = remoteFiles;
                retVal.Record = null;
            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
                if (logEnabled) loggerInstance.Error(retVal.Value);
            }
            finally
            {
                sftpClient.Disconnect();
                sftpClient.Dispose();
            }

            return retVal;
        }

    }



}

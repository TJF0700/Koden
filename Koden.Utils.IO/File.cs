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
using System.Text;
using System.Threading.Tasks;
using Koden.Utils.Models;
using Koden.Utils.Interfaces;
using Koden.Utils.Extensions;

namespace Koden.Utils.IO
{
    /// <summary>
    /// The File Class
    /// </summary>
    public static class File
    {
        private static int fileCount = 0;
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

        #region "Touch"
        /// <summary>
        /// Touches the specified file. (Modifies the last write to time)
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="dateTime">The date/time to use.</param>
        /// <returns></returns>
        public static FWRetVal Touch(string fileName, DateTime? dateTime = null)
        {
            var retVal = new FWRetVal
            {
                MsgType = FWMsgType.Success,
                Value = String.Format("Successfully touched file {0}. Set DateTime on it to: {1} ", fileName, dateTime)
            };
            if (dateTime == null) dateTime = DateTime.UtcNow;

            try
            {
                FileStream myFileStream = System.IO.File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                myFileStream.Close();
                myFileStream.Dispose();
                System.IO.File.SetLastWriteTimeUtc(fileName, (DateTime) dateTime);

            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.Message;
            }

            return retVal;
        }

        /// <summary>
        /// Touches the files recursively.
        /// </summary>
        /// <param name="sourceDir">The source dir.</param>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static FWRetVal<int> TouchFilesRecursively(string sourceDir, DateTime? dateTime = null)
        {
            var retVal = new FWRetVal<int>
            {
                MsgType = FWMsgType.Success,
                Value = String.Format("Successfully touched all files in {0} recursivley. Set DateTime on it to: {1} ", sourceDir, dateTime)
            };
            if (dateTime == null) dateTime = DateTime.UtcNow;

            if (!System.IO.Directory.Exists(sourceDir))
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = "Source Directory doesn't exist! " + sourceDir;
                if (logEnabled) loggerInstance.Error(retVal.Value);
            }

            var sDir = new DirectoryInfo(sourceDir);
            foreach (DirectoryInfo dir in sDir.GetDirectories())
            {
                TouchFilesRecursively(dir.FullName, dateTime);
            }

            Parallel.ForEach(sDir.GetFiles(),
              filePath =>
              {
                  try
                  {
                      if (logEnabled && !loggerInstance.IsVerbose)
                      {
                          var fName = Path.Combine(sDir.FullName, filePath.Name);
                          FileStream myFileStream = System.IO.File.Open(fName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                          myFileStream.Close();
                          myFileStream.Dispose();
                          System.IO.File.SetLastWriteTimeUtc(fName, (DateTime) dateTime);
                      }
                      else
                      {
                          var fName = Path.Combine(sDir.FullName, filePath.Name);
                          FileStream myFileStream = System.IO.File.Open(fName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                          myFileStream.Close();
                          myFileStream.Dispose();
                          System.IO.File.SetLastWriteTimeUtc(fName, (DateTime) dateTime);
                          fileCount += 1;
                          loggerInstance.Verbose(String.Format("Touched file: {0} with date: {1}", fName, dateTime));

                      }
                  }
                  catch (Exception ex)
                  {
                      retVal.MsgType = FWMsgType.Error;
                      retVal.Value = ex.Message;
                  }
              });

            retVal.Record = fileCount;
            retVal.Value = String.Format(retVal.Value, fileCount);
            fileCount = 0;
            if (logEnabled) loggerInstance.WriteRetVal(retVal);
            return retVal;
        }
        #endregion

        #region "Copy File(s)"
        /// <summary>
        /// Copies the specified source file.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <param name="targetFile">The target file.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <returns></returns>
        public static FWRetVal Copy(string sourceFile, string targetFile, bool overwrite)
        {

            var retVal = new FWRetVal
            {
                MsgType = FWMsgType.Success,
                Value = "Successfully copied file from: " + sourceFile + " to: " + targetFile
            };
            if (!System.IO.File.Exists(sourceFile))
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = "Source File doesn't exist! " + sourceFile;
                if (logEnabled) loggerInstance.WriteRetVal(retVal);
                return retVal;
            }
            try
            {
                System.IO.File.Copy(sourceFile, targetFile, overwrite);

            }
            catch (Exception ex)
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
            }

            if (logEnabled) loggerInstance.WriteRetVal(retVal);
            return retVal;
        }

        /// <summary>
        /// Copies ALL files recursively.
        /// </summary>
        /// <param name="sourceDir">The source directory.</param>
        /// <param name="targetDir">The target directory.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="overwrite">if set to <c>true</c> [overwrite].</param>
        /// <param name="renameMask">The rename mask in the form of "{0}.{1}".</param>
        /// <returns></returns>
        public static FWRetVal<string> CopyRecursively(string sourceDir, string targetDir, FileFilterType filterType = FileFilterType.None, string filter = null, bool overwrite = false, string renameMask = "{0}.{1}")
        {
            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Successfully copied {0} files from: " + sourceDir + " to: " + targetDir };

            if (!System.IO.Directory.Exists(sourceDir))
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = "Source Directory doesn't exist! " + sourceDir;
                if (logEnabled) loggerInstance.WriteRetVal(retVal);
                return retVal;
            }


            var sDir = new DirectoryInfo(sourceDir);
            var tDir = new DirectoryInfo(targetDir);
            foreach (DirectoryInfo dir in sDir.GetDirectories())
            {
                DirectoryInfo targetSubDirectory = tDir.CreateSubdirectory(dir.Name);
                CopyRecursively(dir.FullName, targetSubDirectory.FullName, filterType, filter, overwrite);
            }

            var fileList = FilterFiles(sDir, filterType, filter);
            var targetFile = "";

            Parallel.ForEach(fileList,
            filePath =>
            {
                // this is SUPER FAST!!! But no logging capabilities.  I'm keeping it here for when Verbose is turned off 
                try
                {
                    if (logEnabled && !loggerInstance.IsVerbose)
                    {
                        retVal.Value = "Successfully (Asynchronously) copied files from: " + sourceDir + " to: " + targetDir;
                        Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                        {
                            filePath.CopyTo(Path.Combine(tDir.FullName, filePath.Name), overwrite);
                            fileCount += 1;
                            _files.Add(filePath.Name);
                        });
                    }
                    else
                    {
                        targetFile = String.Format(renameMask, filePath.Name.Replace(filePath.Extension, ""), filePath.Extension);

                        filePath.CopyTo(Path.Combine(tDir.FullName, targetFile), overwrite);
                        fileCount += 1;
                        _files.Add(Path.Combine(tDir.FullName, targetFile));
                        loggerInstance.Verbose(String.Format("Copied file: {0}\\{1} to {2}\\{1}", sDir.FullName, filePath.Name, tDir.FullName));
                    }
                }
                catch (Exception ex)
                {
                    retVal.MsgType = FWMsgType.Error;
                    retVal.Value = ex.Message;
                }
            });


            retVal.Records = _files;
            retVal.Value = String.Format(retVal.Value, fileCount);
            fileCount = 0;
            if (logEnabled) loggerInstance.WriteRetVal(retVal);
            return retVal;
        }
        #endregion
        #region "Move File(s)"
        /// <summary>
        /// Moves  files recursively.
        /// </summary>
        /// <param name="sourceDir">The source directory.</param>
        /// <param name="targetDir">The target directory.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filter">The filter.</param>
        /// <param name="overwrite">overwrite? true or false</param>
        /// <returns></returns>
        public static FWRetVal<int> MoveRecursively(string sourceDir, string targetDir, FileFilterType filterType = FileFilterType.None, string filter = null, bool overwrite = false)
        {
            var retVal = new FWRetVal<int> { MsgType = FWMsgType.Success, Value = "Successfully moved {0} files from: " + sourceDir + " to: " + targetDir };
            if (!System.IO.Directory.Exists(sourceDir))
            {
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = "Source Directory doesn't exist! " + sourceDir;
                if (logEnabled) loggerInstance.Error(retVal.Value);
            }

            var sDir = new DirectoryInfo(sourceDir);
            var tDir = new DirectoryInfo(targetDir);
            foreach (DirectoryInfo dir in sDir.GetDirectories())
            {
                DirectoryInfo targetSubDirectory = tDir.CreateSubdirectory(dir.Name);
                MoveRecursively(dir.FullName, targetSubDirectory.FullName, filterType, filter, overwrite);
            }

            var fileList = FilterFiles(sDir, filterType, filter);

            Parallel.ForEach(fileList,
                filePath =>
                {
                    try
                    {
                        if (logEnabled && !loggerInstance.IsVerbose)
                        {
                            retVal.Value = "Successfully (Asynchronously) moved files from: " + sourceDir + " to: " + targetDir;

                            Task task = System.Threading.Tasks.Task.Factory.StartNew(() =>
                            {
                                filePath.MoveTo(Path.Combine(tDir.FullName, filePath.Name));
                                if (logEnabled) loggerInstance.Verbose(String.Format("Moved file: {0}\\{1} to {2}\\{1}", sDir.FullName, filePath.Name, tDir.FullName));

                            });
                        }
                        else
                        {
                            filePath.MoveTo(Path.Combine(tDir.FullName, filePath.Name));
                            fileCount += 1;
                            loggerInstance.Verbose(String.Format("Moved file: {0}\\{1} to {2}\\{1}", sDir.FullName, filePath.Name, tDir.FullName));
                        }

                    }
                    catch (Exception ex)
                    {
                        retVal.MsgType = FWMsgType.Error;
                        retVal.Value = ex.Message;
                    }

                });

            retVal.Record = fileCount;
            retVal.Value = String.Format(retVal.Value, fileCount);
            fileCount = 0;
            if (logEnabled) loggerInstance.WriteRetVal(retVal);
            return retVal;
        }

        /// <summary>
        /// Checks for files.
        /// </summary>
        /// <param name="fileHeader">The file header.</param>
        /// <param name="serverPath">The server path.</param>
        /// <param name="filterType">Type of the filter.</param>
        /// <param name="filterText">The filter text.</param>
        /// <returns></returns>
        public static FWRetVal<string> CheckForFiles(FileHeader fileHeader, string serverPath,
             FileFilterType filterType = FileFilterType.None,
            string filterText = null)
        {
            StringBuilder sbFiles = new StringBuilder();
            _files = new List<string>();
            IEnumerable<FileInfo> fileList;
            int fileCount = 0;

            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Found files: {0}" };

            try
            {
                if (fileHeader != null && !string.IsNullOrEmpty(fileHeader.UserName) && !string.IsNullOrEmpty(fileHeader.Password))
                {
                    if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", fileHeader.Server);
                    using (NetworkShareAccessor.Access(fileHeader.Server, fileHeader.UserName, fileHeader.Password))
                    {
                        var sDir = new DirectoryInfo(serverPath);
                        fileList = File.FilterFiles(sDir, filterType, filterText);

                        
                    }
                }
                else
                {
                    var sDir = new DirectoryInfo(serverPath);
                    fileList = File.FilterFiles(sDir, filterType, filterText);
                }


                if (logEnabled) loggerInstance.Separator("Begin File check");
                foreach (var fileListItem in fileList)
                {
                    if (fileListItem.Name.EndsWith(".")) continue;
                    _files.Add(fileListItem.Name);
                    fileCount++;
                    //File.Copy(@"C:\Some\File\To\copy.txt", @"\\REMOTE-COMPUTER\My\Shared\Target\file.txt");
                }


                //if (logEnabled) loggerInstance.Verbose("Attempting to connect to '{0}'", fileHeader.Server);
                //if (logEnabled)
                //{
                //    loggerInstance.Verbose("Connected to '{0}' using SFTP\n", fileHeader.Server);
                //    loggerInstance.Separator("File Options Used");
                //    //loggerInstance.Info("\n\tFilterType={0}\n\tFilterText={1}\n\tForceOverwrite={2}\n\tKeepOrigDateTime={3}\n\tDeleteAfterRetrieve={4}\n",
                //    //    ftpOptions.FilterType, ftpOptions.FilterText, ftpOptions.ForceOverwrite, ftpOptions.KeepOrigDateTime, ftpOptions.DeleteAfterRetrieve);
                //}

                //var fileList = FilterFiles(ftpClient, serverPath, ftpOptions.FilterType, ftpOptions.FilterText);

                //if (logEnabled) loggerInstance.Separator("Begin File Downloads");
                //foreach (var ftpListItem in fileList)
                //{
                //    if (ftpListItem.Name.EndsWith(".")) continue;
                //    _files.Add(ftpListItem.Name);
                //    fileCount++;

                //}
                if (_files.Count > 0) retVal.Value = "Files Found on remote host";
                else retVal.Value = "No files found on remote host";
                retVal.Record = fileCount.ToString();
                retVal.Records = _files;
                //ftpClient.Disconnect();
                //ftpClient.Dispose();
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
        private static IEnumerable<FileInfo> FilterFiles(DirectoryInfo sDir, FileFilterType filterType, string filterText)
        {
            IEnumerable<FileInfo> fileList = sDir.GetFiles();
            if (filterType != FileFilterType.None)
            {
                switch (filterType)
                {
                    case FileFilterType.StartsWith:
                        fileList = fileList.Where(fListItem => fListItem.FullName.ToLower().StartsWith(filterText));
                        if (logEnabled) loggerInstance.Verbose("Filtering Files (StartsWith) - '{0}'", filterText);
                        break;
                    case FileFilterType.EndsWith:
                        fileList = fileList.Where(fListItem => fListItem.FullName.ToLower().EndsWith(filterText));
                        if (logEnabled) loggerInstance.Verbose("Filtering Files (EndsWith) - '{0}'", filterText);
                        break;
                    case FileFilterType.Contains:
                        fileList = fileList.Where(fListItem => fListItem.FullName.ToLower().Contains(filterText));
                        if (logEnabled) loggerInstance.Verbose("Filtering Files (Contains) - '{0}'", filterText);
                        break;
                    case FileFilterType.DateFilterToday:
                        fileList = fileList.Where(fListItem => fListItem.LastWriteTime.ToShortDateString() == Convert.ToDateTime(filterText).ToShortDateString());
                        if (logEnabled) loggerInstance.Verbose("Filtering Files (DateFilterToday) - '{0}'", filterText);
                        break;
                    case FileFilterType.DateBefore:
                        fileList = fileList.Where(fListItem => fListItem.LastWriteTime <= Convert.ToDateTime(filterText));
                        if (logEnabled) loggerInstance.Verbose("Filtering Files (DateFilterBefore) - '{0}'", filterText);
                        break;
                    case FileFilterType.DateAfter:
                        fileList = fileList.Where(fListItem => fListItem.LastWriteTime >= Convert.ToDateTime(filterText));
                        if (logEnabled) loggerInstance.Verbose("Filtering Files (DateFilterAfter) - '{0}'", filterText);
                        break;
                    case FileFilterType.Equals:
                        string[] fList = filterText.Split(':');
                        fileList = fileList.Where(fListItem => filterText.Contains(fListItem.Name.ToLower()));
                        break;
                    case FileFilterType.DateEquals:
                        fileList = fileList.Where(fListItem => fListItem.LastWriteTime.ToShortDateString() == Convert.ToDateTime(filterText).ToShortDateString());
                        break;
                    default:
                        break;
                }
            }
            return fileList;
        }

        #endregion
        #region "Delete File(s)"
        /// <summary>
        /// Deletes the specified source file.
        /// </summary>
        /// <param name="sourceFile">The source file.</param>
        /// <returns></returns>
        public static FWRetVal Delete(string sourceFile)
        {
            var retVal = new FWRetVal<int>
            {
                MsgType = FWMsgType.Success,
                Value = "Successfully Deleted file: " + sourceFile
            };

            if (System.IO.File.Exists(sourceFile))
            {
                // Use a try block to catch IOExceptions, to
                // handle the case of the file already being
                // opened by another process.
                try
                {
                    System.IO.File.Delete(sourceFile);
                }
                catch (System.IO.IOException ex)
                {
                    retVal.MsgType = FWMsgType.Error;
                    retVal.Value = ex.kGetAllMessages();

                }
            }

            if (logEnabled) loggerInstance.WriteRetVal(retVal);
            return retVal;
        }
        #endregion


    }
}

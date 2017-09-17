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
using System.Linq;
using System.Net.Mail;
using Koden.Utils.Emailer.ConfigFileSections;
using Koden.Utils.Models;
using Koden.Utils.Interfaces;
using Koden.Utils.Extensions;

namespace Koden.Utils.Emailer
{
    /// <summary>
    /// Emailer class
    /// </summary>
    public class Emailer : IDisposable, IEmailer
    {
        private static Emailer _instance;

        private static string _SMTPServer;
        private static Boolean _isDebugMsgEnabled;
        private static string _debugMsgTo;
        private static string _debugMsgFrom;
        private static string _debugMsgCC;
        private static Boolean _debugAttachLog;
        private static Boolean _isSuccessMsgEnabled;
        private static string _successMsgTo;
        private static string _successMsgFrom;
        private static string _successMsgCC;
        private static Boolean _successAttachLog;
        private static Boolean _isFailureMsgEnabled;
        private static string _failureMsgTo;
        private static string _failureMsgFrom;
        private static string _failureMsgCC;
        private static Boolean _failureAttachLog;
        private static Boolean _isDisposed;
        private static ILogger _loggerInstance = null;
        private static bool _logEnabled;


        /// <summary>
        /// Gets or sets the logger instance to be used by the File class.
        /// </summary>
        /// <value>The logger instance.</value>
        public ILogger LoggerInstance
        {
            get
            {
                return _loggerInstance;
            }
            set
            {
                _loggerInstance = value;
                _logEnabled = (_loggerInstance != null) ? true : false;
            }
        }
        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private Emailer() { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (!_isDisposed)
            {
                _isDisposed = true;
                _instance = null;
                _logEnabled = (_loggerInstance != null) ? true : false;
            }
        }
        /// <summary>
        /// Gets or sets whether to email Debug Messages.
        /// </summary>
        /// <value>true or false.</value>
        public Boolean IsDebugMsgEnabled
        {
            get
            {
                return _isDebugMsgEnabled;
            }
            set
            {
                _isDebugMsgEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to email SUCCESS Messages.
        /// </summary>
        /// <value>true or false.</value>
        public Boolean IsSuccessMsgEnabled
        {
            get
            {
                return _isSuccessMsgEnabled;
            }
            set
            {
                _isSuccessMsgEnabled = value;
            }
        }

        /// <summary>
        /// Gets or sets whether to email SUCCESS Messages.
        /// </summary>
        /// <value>true or false.</value>
        public Boolean IsFailureMsgEnabled
        {
            get
            {
                return _isFailureMsgEnabled;
            }
            set
            {
                _isFailureMsgEnabled = value;
            }
        }
        /// <summary>
        /// An Emailer instance that exposes a single instance
        /// </summary>
        public static Emailer Instance
        {
            get
            {
        
                // If the instance is null then create one and initialize the Queue
                if (_instance == null)
                {
                    EmailerSection configSection;
                    _instance = new Emailer();
                    _isDisposed = false;
                    try
                    {

                        configSection = (EmailerSection)System.Configuration.ConfigurationManager.GetSection("KodenGroup/Emailer");
                        if (configSection == null) throw new Exception("No config section found for KodenGroup/Emailer!");

                        _SMTPServer = configSection.SMTPServer.HostName;

                        _isDebugMsgEnabled = configSection.DebugMsg.Enabled;
                        _debugMsgFrom = configSection.DebugMsg.From;
                        _debugMsgTo = configSection.DebugMsg.To;
                        _debugMsgCC = configSection.DebugMsg.CC;
                        _debugAttachLog = configSection.DebugMsg.AttachLog;

                        _isSuccessMsgEnabled = configSection.SuccessMsg.Enabled;
                        _successMsgFrom = configSection.SuccessMsg.From;
                        _successMsgTo = configSection.SuccessMsg.To;
                        _successMsgCC = configSection.SuccessMsg.CC;
                        _successAttachLog = configSection.SuccessMsg.AttachLog;

                        _isFailureMsgEnabled = configSection.FailureMsg.Enabled;
                        _failureMsgFrom = configSection.FailureMsg.From;
                        _failureMsgTo = configSection.FailureMsg.To;
                        _failureMsgCC = configSection.FailureMsg.CC;
                        _failureAttachLog = configSection.FailureMsg.AttachLog;
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Error (Emailing is DISABLED): \n\r" + ex.kGetAllMessages());
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        /// Sends Email with file attachment, can send log as well.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The Message body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="msgType">Type of the message to send.</param>
        /// <param name="ex">The exception to parse if avail.</param>
        public void SendETLMsg(string subject, string body, string[] attachments, EmailMsgType msgType = EmailMsgType.ETLSuccess, Exception ex = null)
        {

            if (_isSuccessMsgEnabled && msgType == EmailMsgType.ETLSuccess)
                SendMsg(new MailHeader { From = _successMsgFrom, To = _successMsgTo, CC = _successMsgCC, Subject = "SUCCESS - " + subject, Attachments = _successAttachLog == true ? attachments : null }, body);
            if (_isFailureMsgEnabled && msgType == EmailMsgType.ETLFailure)
                SendMsg(new MailHeader { From = _failureMsgFrom, To = _failureMsgTo, CC = _failureMsgCC, Subject = "FAILURE - " + subject, Attachments = _failureAttachLog == true ? attachments : null }, body);

            if (msgType == EmailMsgType.ETLWarning)
            {
                SendMsg(new MailHeader { From = _successMsgFrom, To = _successMsgTo, CC = _successMsgCC, Subject = "WARNING - " + subject, Attachments = _successAttachLog == true ? attachments : null }, body);
            }
            
            if (_isDebugMsgEnabled && msgType == EmailMsgType.ETLDebug )
            {
                if (ex != null) body += "<br/>-------  Exception Info -----<br/>" + ex.kGetAllMessages();
                SendMsg(new MailHeader { From = _debugMsgFrom, To = _debugMsgTo, CC = _debugMsgCC, Subject = "DEBUG - " + subject, Attachments = _debugAttachLog == true ? attachments : null }, body);
            }
        }

        /// <summary>
        /// Sends the debug.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachment(s).</param>
        /// <param name="ex">The ex.</param>
        public void SendDebugMsg(string subject, string body, string[] attachments, Exception ex = null)
        {
            if (_isDebugMsgEnabled)
            {
                if (ex != null)
                {
                    body += "<br/>-------  Exception Info -----<br/>" + ex.kGetAllMessages();
                }

                SendMsg(new MailHeader { From = _debugMsgFrom, To = _debugMsgTo, CC = _debugMsgCC, Subject = "DEBUG - " + subject, Attachments = _debugAttachLog == true ? attachments : null }, body);
            }
        }

        /// <summary>
        /// Sends the specified message via SMTP email.
        /// </summary>
        /// <param name="strFrom">From email address</param>
        /// <param name="strTo">To email address(es), separated by comma or semicolon.</param>
        /// <param name="strCC">CC email address(es), separated by comma or semicolon.</param>
        /// <param name="strSubject">The subject.</param>
        /// <param name="strBody">The body (HTML).</param>
        /// <param name="fileAttachment">File attachment if any.</param>
        /// <param name="attachLog">true or false to attach the file.</param>
        /// <returns></returns>
        private void SendMsg(string strFrom, string strTo, string strCC, string strSubject, string strBody, string fileAttachment, bool attachLog)
        {
            
            if(_logEnabled) _loggerInstance.FlushLog();
            MailMessage mMsg = new MailMessage();
            mMsg.IsBodyHtml = true;
            mMsg.Body = strBody;

            mMsg.From = new MailAddress(strFrom);
            mMsg.To.Add(strTo.Replace(";", ","));
            if (!String.IsNullOrEmpty(strCC)) mMsg.CC.Add(strCC.Replace(";", ","));
            mMsg.Subject = strSubject;
            mMsg.Priority = MailPriority.High;
            SmtpClient sC = new SmtpClient(_SMTPServer);
            

            if (!String.IsNullOrEmpty(fileAttachment) && attachLog)
            {
                System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                contentType.Name = System.IO.Path.GetFileName(fileAttachment);
                mMsg.Attachments.Add(new Attachment(fileAttachment, contentType));
            }

            try
            {
                sC.Send(mMsg);
            }
            catch (Exception ex)
            {
                if (_logEnabled) _loggerInstance.Error("Message failed to send: {0}", ex.kGetAllMessages());
                else
                    throw ex;
            }

        }

        /// <summary>
        /// Sends a message using MailHeader class.
        /// </summary>
        /// <param name="mailHeader">The mail header.</param>
        /// <param name="strBody">The body.</param>
        public void SendMsg(IMailHeader mailHeader, string strBody)
        {
            if (_logEnabled) _loggerInstance.FlushLog();
            MailMessage mMsg = new MailMessage();
            mMsg.IsBodyHtml = true;
            mMsg.Body = DateTime.Now.ToString("G") + "<br>";
            mMsg.Body += strBody;

            mMsg.From = new MailAddress(mailHeader.From);
            mMsg.To.Add(mailHeader.To.Replace(";", ","));
            if (!String.IsNullOrEmpty(mailHeader.CC)) mMsg.CC.Add(mailHeader.CC.Replace(";", ","));
            mMsg.Subject = mailHeader.Subject;
            mMsg.Priority = MailPriority.High;
            SmtpClient sC = new SmtpClient(_SMTPServer);
            

            if (mailHeader.Attachments != null && mailHeader.Attachments.Length > 0)
            {
                foreach (var fileItem in mailHeader.Attachments)
                {
                    System.Net.Mime.ContentType contentType = new System.Net.Mime.ContentType();
                    contentType.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                    contentType.Name = System.IO.Path.GetFileName(fileItem);
                    mMsg.Attachments.Add(new Attachment(fileItem, contentType));
                }
            }

            try
            {
                sC.Send(mMsg);
            }
            catch (Exception ex)
            {
                if (_logEnabled) _loggerInstance.Error("Message failed to send: {0}", ex.kGetAllMessages());
                else
                    throw ex;
            }
        }


        /// <summary>
        /// Sends the MSG.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="strBody">The  body.</param>
        /// <param name="attachments">The attachments.</param>
        public void SendMsg(string from, string to, string cc, string subject, string strBody, string[] attachments = null)
        {
            SendMsg(new MailHeader { From = from, To = to, CC = cc, Subject = subject, Attachments = attachments }, strBody);
        }
    }

 

 
}

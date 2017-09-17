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
using Koden.Utils.Models;
using System;

namespace Koden.Utils.Interfaces
{
    /// <summary>
    /// Used for Emailer
    /// </summary>
    public interface IEmailer
    {
        /// <summary>
        /// Gets or sets a value indicating whether this instance has debug Messages enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has debug Messages enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsDebugMsgEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has failure Messages enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has failure Messages enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsFailureMsgEnabled { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance has success Messages enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance has success Messages enabled; otherwise, <c>false</c>.
        /// </value>
        bool IsSuccessMsgEnabled { get; set; }
        /// <summary>
        /// Gets or sets the logger instance.
        /// </summary>
        /// <value>
        /// The logger instance.
        /// </value>
        ILogger LoggerInstance { get; set; }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        void Dispose();
        /// <summary>
        /// Sends the debug Message.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="ex">Any Exception information found.</param>
        void SendDebugMsg(string subject, string body, string[] attachments, Exception ex = null);
        /// <summary>
        /// Sends the etl MSG.
        /// </summary>
        /// <param name="subject">The subject.</param>
        /// <param name="body">The body.</param>
        /// <param name="attachments">The attachments.</param>
        /// <param name="msgType">Type of the Message.</param>
        /// <param name="ex">Any Exception information found.</param>
        void SendETLMsg(string subject, string body, string[] attachments, EmailMsgType msgType = EmailMsgType.ETLSuccess, Exception ex = null);
        /// <summary>
        /// Sends the Message.
        /// </summary>
        /// <param name="mailHeader">The mail header.</param>
        /// <param name="strBody">The body text.</param>
        void SendMsg(IMailHeader mailHeader, string strBody);
        /// <summary>
        /// Sends an email Message.
        /// </summary>
        /// <param name="from">From.</param>
        /// <param name="to">To.</param>
        /// <param name="cc">The cc.</param>
        /// <param name="subject">The subject.</param>
        /// <param name="strBody">The body text (or html).</param>
        /// <param name="attachments">Any attachments.</param>
        void SendMsg(string from, string to, string cc, string subject, string strBody, string[] attachments = null);
    }
}

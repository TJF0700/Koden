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
using Microsoft.Exchange.WebServices.Data;
using Koden.Utils.Models;
using Koden.Utils.Interfaces;
using Koden.Utils.Extensions;

namespace Koden.Utils.Exchange
{
    /// <summary>
    /// Exchange Web Service instance
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public partial class EWSContext : IDisposable
    {


        #region "Properties"

        private string _esHost { get; set; }
        private string _esUserId { get; set; }
        private string _esPassword { get; set; }
        private ILogger _loggerInstance = null;
        private Boolean _logEnabled = false;
        private ExchangeService _esService = null;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _loggerInstance.FlushLog();
        }

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

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="EWSContext"/> class.
        /// </summary>
        public EWSContext() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="EWSContext" /> class.
        /// </summary>
        /// <param name="esHeader">The es header.</param>
        public EWSContext(ESHeader esHeader)
        {
           
            _esHost = esHeader.Host;
            _esUserId = esHeader.UserId;
            _esPassword = esHeader.Password;
            _loggerInstance = esHeader.LoggerInstance;
            if (_logEnabled) _loggerInstance.Info("Instantiating Exchange Module");
            try
            {
                _esService = Service.ConnectToService(UserDataFromHeader.GetUserData(esHeader), new TraceListener());
            }
            catch (Exception ex)
            {
                if (_logEnabled) _loggerInstance.Error(ex.kGetAllMessages());
                throw new Exception("Unable to Connect To Exchange WebService",ex.InnerException);
            }
        }


        /// <summary>
        /// Sets the message to read.
        /// </summary>
        /// <param name="messageID">The message ID.</param>
        /// <returns>FWRetVal bool</returns>
        public FWRetVal<bool> SetMessageToRead(ItemId messageID)
        {
            var retVal = new FWRetVal<bool> { MsgType = FWMsgType.Success, Value = "Successfully set message to read" };
            try
            {
                EmailMessage message = EmailMessage.Bind(_esService, messageID, new PropertySet(BasePropertySet.IdOnly, EmailMessageSchema.IsRead));

                message.IsRead = true;
                message.Update(ConflictResolutionMode.AlwaysOverwrite, true);
                if (_logEnabled) _loggerInstance.Verbose("Message set to 'Read'");
                retVal.Record = true;
            }
            catch (Exception ex)
            {
                retVal.Record = false;
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
                if (_logEnabled) _loggerInstance.Error(retVal.Value);
            }

            return retVal;

        }


        /// <summary>
        /// Gets message attachments and saves them to a specified directory.
        /// </summary>
        /// <param name="messageID">The message ID.</param>
        /// <param name="storageDirectory">The storage directory to place the files.</param>
        /// <param name="filterExtension">The extension to filter on if looking for a specific file type (sometimes there are images, etc that are irrelevant.</param>
        /// <returns> A generic list of strings with the path and filename of saved attachments.  Use this to work with saved data.</returns>
        public FWRetVal<string> GetMessageAttachments(ItemId messageID, string storageDirectory, string filterExtension = "")
        {

            var retVal = new FWRetVal<string> { MsgType = FWMsgType.Success, Value = "Successfully retrieved attachments" };
            try
            {
                EmailMessage message = EmailMessage.Bind(_esService, messageID, new PropertySet(BasePropertySet.IdOnly, ItemSchema.Attachments));

                var fileList = new List<string>();

                if (message.HasAttachments)
                {
                    foreach (FileAttachment attch in message.Attachments)
                    {

                        if (!String.IsNullOrEmpty(filterExtension))
                        {
                            if (Path.GetExtension(attch.Name).ToUpper() != filterExtension.ToUpper()) continue;

                        }

                        attch.Load(storageDirectory + "\\" + attch.Name);
                        if (_logEnabled) _loggerInstance.Verbose("Saved file attachment: {0}", attch.Name);
                        fileList.Add(storageDirectory + "\\" + attch.Name);
                    }
                    retVal.Record = fileList.Count.ToString();
                    retVal.Records = fileList;
                }
            }
            catch (Exception ex)
            {
                retVal.Record = "0";
                retVal.Records = null;
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
                if (_logEnabled) _loggerInstance.Error(retVal.Value);
            }

            return retVal;
        }

        /// <summary>
        /// Finds items in an Exchange Mailbox.
        /// </summary>
        /// <param name="subjSearch">The subject to filter on.</param>
        /// <param name="bodySearch">The body text to filter on.</param>
        /// <param name="maxItems">The maximum items.</param>
        /// <param name="attachmentRequired">Is an attachment required?</param>
        /// <returns>
        /// A List of Exchange Items that can be worked with individually
        /// FwRetval.Record holds the list of items
        /// </returns>
        public FWRetVal<FindItemsResults<Item>> FindItems(string subjSearch, string bodySearch, int maxItems = 50, bool attachmentRequired = false)
        {

            var retVal = new FWRetVal<FindItemsResults<Item>> { MsgType = FWMsgType.Success, Value = "Successfully retrieved items" };
            try
            {
                // Create a search collection that contains your search conditions.
                List<SearchFilter> searchFilterCollection = new List<SearchFilter>();
                if (!String.IsNullOrEmpty(subjSearch))
                {
                    searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ItemSchema.Subject, subjSearch, ContainmentMode.Substring, ComparisonMode.IgnoreCaseAndNonSpacingCharacters));
                }
                if (!String.IsNullOrEmpty(bodySearch))
                {
                    searchFilterCollection.Add(new SearchFilter.ContainsSubstring(ItemSchema.Body, bodySearch));
                }

                searchFilterCollection.Add(new SearchFilter.IsEqualTo(EmailMessageSchema.IsRead, false));

                // Create the search filter with a logical operator and your search parameters.
                SearchFilter searchFilter = new SearchFilter.SearchFilterCollection(LogicalOperator.And, searchFilterCollection.ToArray());

                // Limit the view to 50 items.
                ItemView view = new ItemView(maxItems)
                {

                    // Limit the property set to the property ID for the base property set, and the subject and item class for the additional properties to retrieve.
                    PropertySet = new PropertySet(BasePropertySet.IdOnly, ItemSchema.Subject,
                    ItemSchema.ItemClass, EmailMessageSchema.IsRead,
                    EmailMessageSchema.HasAttachments),

                    // Setting the traversal to shallow will return all non-soft-deleted items in the specified folder.
                    Traversal = ItemTraversal.Shallow
                };

                // Send the request to search the Inbox and get the results.
                retVal.Record = _esService.FindItems(WellKnownFolderName.Inbox, searchFilter, view);
            }
            catch (Exception ex)
            {
                retVal.Record = null;
                retVal.Records = null;
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
                if (_logEnabled) _loggerInstance.Error(retVal.Value);
            }

            return retVal;
        }

        /// <summary>
        /// Gets the appointments.
        /// </summary>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="maxAppts">The maximum appts.</param>
        /// <returns>
        /// FwRetval.Record holds the list of appointments
        /// </returns>
        public FWRetVal<FindItemsResults<Appointment>> GetAppointments(DateTime startDate, DateTime endDate, int maxAppts = 10)
        {

            var retVal = new FWRetVal<FindItemsResults<Appointment>> { MsgType = FWMsgType.Success, Value = "Successfully retrieved appointments" };
            try
            {
                var calendar =
                CalendarFolder.Bind(_esService, WellKnownFolderName.Calendar, new PropertySet());
                var cView = new CalendarView(startDate, endDate, maxAppts)
                {
                    PropertySet = new PropertySet(
                    AppointmentSchema.Subject,
                    AppointmentSchema.Start,
                    AppointmentSchema.End,
                    AppointmentSchema.Location
                    )
                };

                retVal.Record = calendar.FindAppointments(cView);
            }
            catch (Exception ex)
            {
                retVal.Record = null;
                retVal.Records = null;
                retVal.MsgType = FWMsgType.Error;
                retVal.Value = ex.kGetAllMessages();
                if (_logEnabled) _loggerInstance.Error(retVal.Value);
            }

            return retVal;
        }
    }
}

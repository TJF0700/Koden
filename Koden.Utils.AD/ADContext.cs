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
using System.DirectoryServices;
using System.Security;
using System.Runtime.InteropServices;
using Koden.Utils.Interfaces;
using Koden.Utils.Models;

namespace Koden.Utils.AD
{
    /// <summary>
    /// Context for connecting to Active Directory via LDAP
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public class ADContext : IDisposable
    {

        #region "Properties"

        private string _adLDAPRoot { get; set; }
        private string _adOU { get; set; }
        private string _adUserId { get; set; }
        private string _adPassword { get; set; }
        private ILogger _loggerInstance = null;
        private bool _logEnabled = false;

        private string[] _propsToLoad = new[] {
                "samaccountname",
                "userAccountControl",
                "cn",
                "givenname", // First Name
                "sn",  //Last Name
                "objectGUID",
                "displayname",
                "mail",
                "telephoneNumber",
                "physicalDeliveryOfficeName",
                "mobile",
                "title",
                "department",
                "memberOf",
                "whenCreated",
                "whenChanged"
            };

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private ADContext() { }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_logEnabled) _loggerInstance.FlushLog();
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

        /// <summary>
        /// Gets or sets the properties from LDAP to load.
        /// </summary>
        /// <value>
        /// The props to load.
        /// </value>
        public string[] PropsToLoad
        {
            get
            {
                return _propsToLoad;
            }

            set
            {
                this._propsToLoad = value;
            }
        }

        #endregion


        /// <summary>
        /// Initializes a new instance of the <see cref="ADContext"/> class.
        /// </summary>
        /// <param name="adHeader">The ad header.</param>
        public ADContext(ADHeader adHeader)
        {
            _loggerInstance = adHeader.LoggerInstance;
            if (_loggerInstance != null) _loggerInstance.Info("Instantiating AD Module");
            _adLDAPRoot = adHeader.LDAPRoot;
            _adUserId = adHeader.UserId;
            _adPassword = adHeader.Password;
            _adOU = adHeader.OU;

        }

        /// <summary>
        /// Gets the users in group.s
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <returns></returns>
        public List<GroupObject> GetGroups(string groupName)
        {
            var groups = new List<GroupObject>();
            if (_logEnabled) _loggerInstance.Info("Connecting to AD: {0}", _adLDAPRoot);
            var dirRoot = new DirectoryEntry(String.Format(_adLDAPRoot, _adOU));
            var dirGroup = new DirectoryEntry(String.Format(_adLDAPRoot, ""));

            var filt = "(&(objectCategory=group)(cn=" + groupName + "))";
            
            using (DirectorySearcher grpSrch = new DirectorySearcher(dirGroup, filt))
            {

                var resultCol = SafeFindAll(grpSrch);
                var count = 0;
                foreach (SearchResult item in resultCol)
                {
                    groups.Add(
                        new GroupObject (
                            GetProperty(item, "cn"),
                            GetProperty(item, "groupType"),
                            GetProperty(item, "mail"),
                            GetProperty(item, "description")
                        ));
                    count++;
                    if (count % 200 == 0)
                    {
                        if (_logEnabled) _loggerInstance.Info("At {0} groups...", count);
                    }
                }
            }

            return groups;
        }
        /// <summary>
        /// Gets the users in group.
        /// </summary>
        /// <param name="groupName">Name of the group.</param>
        /// <param name="enabledUsersOnly">if set to <c>true</c> [enabled users only].</param>
        /// <param name="propertiesToLoad">The properties to load.</param>
        /// <returns></returns>
        public List<UserObject> GetUsersInGroup(string groupName, bool enabledUsersOnly = false, string[] propertiesToLoad = null)
        {
            var users = new List<UserObject>();
            if (_logEnabled) _loggerInstance.Info("Connecting to AD: {0}", _adLDAPRoot);
            var dirRoot = new DirectoryEntry(String.Format(_adLDAPRoot, _adOU));
            var dirGroup = new DirectoryEntry(String.Format(_adLDAPRoot, ""));

            string groupCN = "";
            using (DirectorySearcher grpSrch = new DirectorySearcher(dirGroup, "(&(objectCategory=group)(cn=" + groupName + "))"))
            {
                var searchResult = grpSrch.FindOne();
                if (searchResult != null)
                    groupCN = GetProperty(searchResult, "distinguishedName");
                else
                    return users;
            }
            string staticFilter = "(&(sAMAccountType=805306368)(memberOf=" + groupCN + "))";



            if (propertiesToLoad == null) propertiesToLoad = PropsToLoad;


            using (DirectorySearcher srch = new DirectorySearcher(dirRoot, staticFilter, propertiesToLoad))
            {
                srch.SearchScope = SearchScope.Subtree;
                if (enabledUsersOnly)
                {
                    srch.Filter = "(!useraccountcontrol:1.2.840.113556.1.4.803:=2)";
                }

                var resultCol = SafeFindAll(srch);

                if (_logEnabled) _loggerInstance.Info("Begin Finding Users...");
                var count = 0;
                foreach (SearchResult item in resultCol)
                {
                    users.Add(PopulateUserObject(item, null));
                    count++;
                    if (count % 200 == 0)
                    {
                        if (_logEnabled) _loggerInstance.Info("At {0} users...", count);
                    }
                }
            }
            if (_logEnabled) _loggerInstance.Info("Finished finding users");

            return users;

        }

        /// <summary>
        /// Gets all users in an OU.
        /// </summary>
        /// <param name="enabledUsersOnly">if set to <c>true</c> [enabled users only].</param>
        /// <param name="importantGroups">The important groups to look for in the search.</param>
        /// <param name="dtLastChanged">The date object was last changed.</param>
        /// <param name="propertiesToLoad">The properties to load.</param>
        /// <returns></returns>
        public List<UserObject> GetAllUsers(bool enabledUsersOnly = false, string[] importantGroups = null, DateTime? dtLastChanged = null, string[] propertiesToLoad = null)
        {
            var users = new List<UserObject>();
            if (_logEnabled) _loggerInstance.Info("Connecting to AD: {0}", _adLDAPRoot);
            var dirRoot = new DirectoryEntry(String.Format(_adLDAPRoot, _adOU));

            string staticFilter = "(sAMAccountType=805306368)";
            if (dtLastChanged != null)
            {
                string strDate = String.Format("{0:yyyyMMddhhmmss.0Z}", dtLastChanged);
                staticFilter = String.Format("(&(sAMAccountType=805306368)(whenChanged>={0}))", strDate);
            }

            if (propertiesToLoad == null) propertiesToLoad = PropsToLoad;

            using (DirectorySearcher srch = new DirectorySearcher(dirRoot, staticFilter, propertiesToLoad))
            {
                srch.SearchScope = SearchScope.OneLevel;
                srch.PageSize = 1000;
                if (enabledUsersOnly)
                {
                    srch.Filter = "(!useraccountcontrol:1.2.840.113556.1.4.803:=2)";
                }

                var resultCol = SafeFindAll(srch);

                if (_logEnabled) _loggerInstance.Info("Begin Finding Users...");
                var count = 0;
                foreach (SearchResult item in resultCol)
                {
                    users.Add(PopulateUserObject(item, importantGroups));
                    count++;
                    if (count % 200 == 0)
                    {
                        if (_logEnabled) _loggerInstance.Info("At {0} users...", count);
                    }
                }
            }
            if (_logEnabled) _loggerInstance.Info("Finished finding users");

            return users;

        }
        /// <summary>
        /// Gets a user object.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="importantGroups">The important groups.</param>
        /// <param name="propertiesToLoad">The properties to load.</param>
        /// <returns></returns>
        /// <exception cref="DirectoryServicesCOMException">User Object not found in AD
        /// or
        /// AD Error: " + de.Message</exception>
        /// <exception cref="SecurityException">Error Authenticating: " + se.Message</exception>
        /// <exception cref="COMException">AD Error: " + ce.Message</exception>
        public UserObject GetUser(string userID, string[] importantGroups = null, string[] propertiesToLoad = null)
        {
            var userObj = new UserObject();
            if (propertiesToLoad == null) propertiesToLoad = PropsToLoad;

            try
            {
                var dirRoot = new DirectoryEntry(String.Format(_adLDAPRoot, _adOU));
                using (DirectorySearcher srch = new DirectorySearcher(dirRoot, "(&(sAMAccountType=805306368)(SAMAccountName=" + userID + "))", propertiesToLoad))
                {
                    var result = srch.FindOne();
                    if (result == null)
                    {
                        throw new DirectoryServicesCOMException("User Object not found in AD");
                    }
                    return PopulateUserObject(result, importantGroups);
                }

            }
            catch (SecurityException se)
            {
                throw new SecurityException("Error Authenticating: " + se.Message);
            }
            catch (DirectoryServicesCOMException de)
            {
                throw new DirectoryServicesCOMException("AD Error: " + de.Message);
            }
            catch (COMException ce)
            {
                throw new COMException("AD Error: " + ce.Message);
            }

        }


        /// <summary>
        /// Gets the users in group.s
        /// </summary>
        /// <param name="userName">Name of the group.</param>
        /// <param name="propertiesToLoad">The properties to load.</param>
        /// <returns></returns>
        public List<BaseUserObject> GetUsers(string userName, string[] propertiesToLoad = null)
        {
            var users = new List<BaseUserObject>();
            if (_logEnabled) _loggerInstance.Info("Connecting to AD: {0}", _adLDAPRoot);
            var dirRoot = new DirectoryEntry(String.Format(_adLDAPRoot, _adOU));
            var dirUser = new DirectoryEntry(String.Format(_adLDAPRoot, ""));

            var filt = "(&(objectClass=user)(objectCategory=person)(cn=" + userName + "))";



            using (DirectorySearcher userSrch = new DirectorySearcher(dirUser, filt))
            {

                var resultCol = SafeFindAll(userSrch);
                var count = 0;
                foreach (SearchResult item in resultCol)
                {
                    users.Add(new BaseUserObject(GetProperty(item, "samaccountname"), GetProperty(item, "displayname"), GetProperty(item, "title"), GetProperty(item, "givenname"), GetProperty(item, "sn")));
                    count++;
                    if (count % 200 == 0)
                    {
                        if (_logEnabled) _loggerInstance.Info("At {0} users...", count);
                    }
                }
            }

            return users;
        }

        private string GetProperty(SearchResult searchResult, string propertyName)
        {
            if (searchResult.Properties.Contains(propertyName))
            {
                return Convert.ToString(searchResult.Properties[propertyName][0]);
            }
            return null;
        }
        private UserObject PopulateUserObject(SearchResult searchResult, string[] importantGroups)
        {
            var userObj = new UserObject();
            userObj.ObjectGUID = (byte[])searchResult.Properties["objectGUID"][0];

            userObj.UserName = GetProperty(searchResult, "displayname");
            userObj.UPath = GetProperty(searchResult, "adspath");
            userObj.UserID = GetProperty(searchResult, "samaccountname");
            userObj.Mail = GetProperty(searchResult, "mail");
            userObj.Homeloc = GetProperty(searchResult, "physicalDeliveryOfficeName");
            userObj.Phone = GetProperty(searchResult, "telephoneNumber");
            userObj.Mobile = GetProperty(searchResult, "mobile");
            userObj.Title = GetProperty(searchResult, "title");
            userObj.Cn = GetProperty(searchResult, "cn");
            userObj.FirstName = GetProperty(searchResult, "givenname");
            userObj.LastName = GetProperty(searchResult, "sn");
            userObj.ADGroups = GetGroups(searchResult, importantGroups);
            userObj.WhenCreated = Convert.ToDateTime(GetProperty(searchResult, "whenCreated"));
            userObj.WhenLastChanged = Convert.ToDateTime(GetProperty(searchResult, "whenChanged"));
            //DateTime result;
            //string format = "yyyyMMddhhmmss.0Z";
            //result = DateTime.ParseExact(dtLastChanged, format, CultureInfo.InvariantCulture);
            userObj.UserFlags = (AdsUserFlags)Convert.ToInt32(GetProperty(searchResult, "userAccountControl"));
            return userObj;
        }

          private List<string> GetGroups(SearchResult searchResult, string[] importantGroups)
        {
            var retVal = new List<string>();

            foreach (string item in searchResult.Properties["memberOf"])
            {
                var tst = item.Replace("CN=", "").Split(',').First();
                retVal.Add(tst);
            }

            return retVal;
        }

        private IEnumerable<SearchResult> SafeFindAll(DirectorySearcher searcher)
        {
            using (SearchResultCollection results = searcher.FindAll())
            {
                foreach (SearchResult result in results)
                {
                    yield return result;
                }
            }
        }

    }


}

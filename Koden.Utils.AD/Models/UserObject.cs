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
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Koden.Utils.AD
{
    /// <summary>
    /// Active Directory User Flags
    /// </summary>
    [Flags]
    public enum AdsUserFlags
    {
        Script = 1,                  // 0x1
        AccountDisabled = 2,              // 0x2
        HomeDirectoryRequired = 8,           // 0x8 
        AccountLockedOut = 16,             // 0x10
        PasswordNotRequired = 32,           // 0x20
        PasswordCannotChange = 64,           // 0x40
        EncryptedTextPasswordAllowed = 128,      // 0x80
        TempDuplicateAccount = 256,          // 0x100
        NormalAccount = 512,              // 0x200
        InterDomainTrustAccount = 2048,        // 0x800
        WorkstationTrustAccount = 4096,        // 0x1000
        ServerTrustAccount = 8192,           // 0x2000
        PasswordDoesNotExpire = 65536,         // 0x10000
        MnsLogonAccount = 131072,           // 0x20000
        SmartCardRequired = 262144,          // 0x40000
        TrustedForDelegation = 524288,         // 0x80000
        AccountNotDelegated = 1048576,         // 0x100000
        UseDesKeyOnly = 2097152,            // 0x200000
        DontRequirePreauth = 4194304,          // 0x400000
        PasswordExpired = 8388608,           // 0x800000
        TrustedToAuthenticateForDelegation = 16777216, // 0x1000000
        NoAuthDataRequired = 33554432         // 0x2000000
    }

    public class BaseUserObject
    {

        public BaseUserObject(string userID, string userName, string title = "",string firstName="", string lastName="" ) {
            _userID = userID;
            _userName = userName;
            //_cn = cn;
            _firstName = firstName;
            _lastName = lastName;
            _title = title;
        }


        private string _userID;
        private string _userName;
        private string _cn;
        private string _firstName;
        private string _lastName;
        private string _title;

        public string UserID
        {
            get
            {
                return _userID;
            }

            set
            {
                this._userID = value;
            }
        }

        public string UserName
        {
            get
            {
                return _userName;
            }

            set
            {
                this._userName = value;
            }
        }

        public string CN
        {
            get
            {
                return _cn;
            }

            set
            {
                this._cn = value;
            }
        }

        public string FirstName
        {
            get
            {
                return _firstName;
            }

            set
            {
                this._firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return _lastName;
            }

            set
            {
                this._lastName = value;
            }
        }

        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                this._title = value;
            }
        }
    }
    public class UserObject
    {
        private string userID;
        private string userName;
        private byte[] objectGUID; // absolute object in AD
        private string division;
        private string cn;
        private string firstName;
        private string lastName;
        private string phone;
        private string mobile;
        private string homeloc;
        private string title;
        private string uPath; // LDAP Directory path 
        private string mail;
        private AdsUserFlags userFlags;
        private bool isLocalDev;
        private bool isAdmin;
        private bool isDev;
        private IPrincipal principal;
        private List<string> facilities;
        private string[] roles;
        private List<string> aDGroups;
        private DateTime lastLoginCheck;
        private DateTime whenCreated;
        private DateTime? whenLastChanged;
        private string userLevelType;

        public string UserID
        {
            get
            {
                return userID;
            }

            set
            {
                this.userID = value;
            }
        }

        public string UserName
        {
            get
            {
                return userName;
            }

            set
            {
                this.userName = value;
            }
        }

        public byte[] ObjectGUID
        {
            get
            {
                return objectGUID;
            }

            set
            {
                this.objectGUID = value;
            }

        }




        public bool IsDisabled
        {
            get
            {
                return UserFlags.HasFlag(AdsUserFlags.AccountDisabled);
            }
        }

        public string Division
        {
            get
            {
                return division;
            }

            set
            {
                this.division = value;
            }
        }


        public string Cn
        {
            get
            {
                return cn;
            }

            set
            {
                this.cn = value;
            }
        }

        public string FirstName
        {
            get
            {
                return firstName;
            }

            set
            {
                this.firstName = value;
            }
        }

        public string LastName
        {
            get
            {
                return lastName;
            }

            set
            {
                this.lastName = value;
            }
        }

        public string Phone
        {
            get
            {
                return phone;
            }

            set
            {
                this.phone = value;
            }
        }

        public string Mobile
        {
            get
            {
                return mobile;
            }

            set
            {
                this.mobile = value;
            }
        }

        public string Homeloc
        {
            get
            {
                return homeloc;
            }

            set
            {
                this.homeloc = value;
            }
        }

        public string Title
        {
            get
            {
                return title;
            }

            set
            {
                this.title = value;
            }
        }

        public string UPath
        {
            get
            {
                return uPath;
            }

            set
            {
                this.uPath = value;
            }
        }

        public string Mail
        {
            get
            {
                return mail;
            }

            set
            {
                this.mail = value;
            }
        }

        public AdsUserFlags UserFlags
        {
            get
            {
                return userFlags;
            }

            set
            {
                this.userFlags = value;
            }
        }

        public bool IsLocalDev
        {
            get
            {
                return isLocalDev;
            }

            set
            {
                this.isLocalDev = value;
            }
        }

        public bool IsAdmin
        {
            get
            {
                return isAdmin;
            }

            set
            {
                this.isAdmin = value;
            }
        }

        public bool IsDev
        {
            get
            {
                return isDev;
            }

            set
            {
                this.isDev = value;
            }
        }

        public IPrincipal Principal
        {
            get
            {
                return principal;
            }

            set
            {
                this.principal = value;
            }
        }

        public List<string> Facilities
        {
            get
            {
                return facilities;
            }

            set
            {
                this.facilities = value;
            }
        }

        public string[] Roles
        {
            get
            {
                return roles;
            }

            set
            {
                this.roles = value;
            }
        }

        public List<string> ADGroups
        {
            get
            {
                return aDGroups;
            }

            set
            {
                this.aDGroups = value;
            }
        }

        public DateTime LastLoginCheck
        {
            get
            {
                return lastLoginCheck;
            }

            set
            {
                this.lastLoginCheck = value;
            }
        }
        public DateTime WhenCreated
        {
            get
            {
                return whenCreated;
            }

            set
            {
                this.whenCreated = value;
            }
        }

        public DateTime? WhenLastChanged
        {
            get
            {
                return whenLastChanged;
            }

            set
            {
                this.whenLastChanged = value;
            }
        }


        public string UserLevelType
        {
            get
            {
                return userLevelType;
            }

            set
            {
                this.userLevelType = value;
            }
        }


    }
    public class UserObject1
    {
        public string UserID;
        public string UserName;
        public byte[] objectGUID; // absolute object in AD
        public string Division;
        public string EmployeeID;
        public string cn;
        public string FirstName;
        public string LastName;
        public string Phone;
        public string Mobile;
        public string Homeloc;
        public string Title;
        public string UPath; // LDAP Directory path 
        public string mail;
        public AdsUserFlags UserFlags;
        public bool IsLocalDev;
        public bool IsAdmin;
        public bool IsDev;
        public IPrincipal Principal;
        public List<string> Facilities;
        public string[] Roles;
        public List<string> ADGroups;
        public DateTime LastLoginCheck;
        public DateTime? WhenLastChanged;
        public string UserLevelType;

    }
}

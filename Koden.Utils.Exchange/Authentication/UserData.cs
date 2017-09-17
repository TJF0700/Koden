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
using System.Security;
using Microsoft.Exchange.WebServices.Data;

namespace Koden.Utils.Exchange
{
    /// <summary>
    /// User Data Interfacte
    /// </summary>
    public interface IUserData
    {
        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        ExchangeVersion Version { get; }
        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        string UserId { get; }
        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        SecureString Password { get; }
        /// <summary>
        /// Gets or sets the autodiscover URL.
        /// </summary>
        /// <value>
        /// The autodiscover URL.
        /// </value>
        Uri AutodiscoverUrl { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Koden.Utils.Exchange.IUserData" />
    public class UserDataFromConsole : IUserData
    {
        /// <summary>
        /// The user data
        /// </summary>
        public static UserDataFromConsole UserData;

        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <returns></returns>
        public static IUserData GetUserData()
        {
            if (UserData == null)
            {
                GetUserDataFromConsole();
            }

            return UserData;
        }

        internal static void GetUserDataFromConsoleCredUI(ref ExchangeService service)
        {
            CredentialHelper.AppLogin(ref service);
        }

        private static void GetUserDataFromConsole()
        {
            UserData = new UserDataFromConsole();

            Console.Write("Enter email address: ");
            UserData.UserId = Console.ReadLine();

            UserData.Password = new SecureString();

            Console.Write("Enter password: ");

            while (true)
            {
                ConsoleKeyInfo userInput = Console.ReadKey(true);
                if (userInput.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (userInput.Key == ConsoleKey.Escape)
                {
                    return;
                }
                else if (userInput.Key == ConsoleKey.Backspace)
                {
                    if (UserData.Password.Length != 0)
                    {
                        UserData.Password.RemoveAt(UserData.Password.Length - 1);
                    }
                }
                else
                {
                    UserData.Password.AppendChar(userInput.KeyChar);
                    Console.Write("*");
                }
            }

            Console.WriteLine();

            UserData.Password.MakeReadOnly();
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public ExchangeVersion Version { get { return ExchangeVersion.Exchange2013; } }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public SecureString Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the autodiscover URL.
        /// </summary>
        /// <value>
        /// The autodiscover URL.
        /// </value>
        public Uri AutodiscoverUrl
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Added for compatibility with Koden *.config settings and classes
    /// </summary>
    /// <seealso cref="Koden.Utils.Exchange.IUserData" />
    public class UserDataFromHeader : IUserData
    {
        /// <summary>
        /// The user data
        /// </summary>
        public static UserDataFromHeader UserData;

        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <param name="esHeader">The es header.</param>
        /// <returns></returns>
        public static IUserData GetUserData(ESHeader esHeader)
        {
            if (UserData == null)
            {
                GetUserDataFromHeader(esHeader);
            }

            return UserData;
        }

        internal static void GetUserDataFromHeaderCredUI(ref ExchangeService service)
        {
            CredentialHelper.AppLogin(ref service);
        }

        private static void GetUserDataFromHeader(ESHeader esHeader)
        {
            UserData = new UserDataFromHeader();
            UserData.AutodiscoverUrl = new Uri("https://" + esHeader.Host + "/EWS/Exchange.asmx");
            UserData.UserId = esHeader.UserId;
            UserData.Password = ConvertToSecureString(esHeader.Password);

            UserData.Password.MakeReadOnly();
        }

        private static SecureString ConvertToSecureString(string password)
        {
            if (password == null)
                throw new ArgumentNullException("password");

            var securePassword = new SecureString();

            foreach (char c in password)
                securePassword.AppendChar(c);

            securePassword.MakeReadOnly();
            return securePassword;
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public ExchangeVersion Version { get { return ExchangeVersion.Exchange2013; } }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public string UserId
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public SecureString Password
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the autodiscover URL.
        /// </summary>
        /// <value>
        /// The autodiscover URL.
        /// </value>
        public Uri AutodiscoverUrl
        {
            get;
            set;
        }
    }

}

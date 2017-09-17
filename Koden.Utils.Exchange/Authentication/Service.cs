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
using System.Net;
using Microsoft.Exchange.WebServices.Data;

namespace Koden.Utils.Exchange
{
    /// <summary>
    /// 
    /// </summary>
    public static class Service
    {
        static Service()
        {
            CertificateCallback.Initialize();
        }

        // The following is a basic redirection validation callback method. It 
        // inspects the redirection URL and only allows the Service object to 
        // follow the redirection link if the URL is using HTTPS. 
        //
        // This redirection URL validation callback provides sufficient security
        // for development and testing of your application. However, it may not
        // provide sufficient security for your deployed application. You should
        // always make sure that the URL validation callback method that you use
        // meets the security requirements of your organization.
        internal static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials. 
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }

            return result;
        }

        /// <summary>
        /// Connects to exchange service.
        /// </summary>
        /// <param name="traceToFile">if set to <c>true</c> [trace to file].</param>
        /// <returns></returns>
        public static ExchangeService ConnectToService(bool traceToFile)
        {
            // We use this to get the target Exchange version. 
            UserDataFromConsole data = new UserDataFromConsole();

            ExchangeService service = new ExchangeService(data.Version);
            //service.PreAuthenticate = true;

            if (traceToFile)
                service.TraceListener = new TraceListener();
            else
            {
                service.TraceEnabled = true;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnablePrettyPrinting = true;
            }

            UserDataFromConsole.GetUserDataFromConsoleCredUI(ref service);

            return service;
        }

        /// <summary>
        /// Connects to exchange service.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <returns></returns>
        public static ExchangeService ConnectToService(IUserData userData)
        {
            return ConnectToService(userData, null);
        }

        /// <summary>
        /// Connects to exchange service.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <param name="listener">The listener.</param>
        /// <returns></returns>
        public static ExchangeService ConnectToService(IUserData userData, ITraceListener listener)
        {
            ExchangeService service = new ExchangeService(userData.Version);

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            service.Credentials = new NetworkCredential(userData.UserId, userData.Password);

            if (userData.AutodiscoverUrl == null)
            {
                Console.Write(string.Format("Using Autodiscover to find EWS URL for {0}. Please wait... ", userData.UserId));

                service.AutodiscoverUrl(userData.UserId, RedirectionUrlValidationCallback);
                userData.AutodiscoverUrl = service.Url;

                Console.WriteLine("Autodiscover Complete");
            }
            else
            {
                service.Url = userData.AutodiscoverUrl;
            }

            return service;
        }

        /// <summary>
        /// Connects to exchange service with impersonation.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <param name="impersonatedUserSMTPAddress">The impersonated user SMTP address.</param>
        /// <returns></returns>
        public static ExchangeService ConnectToServiceWithImpersonation(
          IUserData userData,
          string impersonatedUserSMTPAddress)
        {
            return ConnectToServiceWithImpersonation(userData, impersonatedUserSMTPAddress, null);
        }

        /// <summary>
        /// Connects to exchange service with impersonation.
        /// </summary>
        /// <param name="userData">The user data.</param>
        /// <param name="impersonatedUserSMTPAddress">The impersonated user SMTP address.</param>
        /// <param name="listener">The listener.</param>
        /// <returns></returns>
        public static ExchangeService ConnectToServiceWithImpersonation(
          IUserData userData,
          string impersonatedUserSMTPAddress,
          ITraceListener listener)
        {
            ExchangeService service = new ExchangeService(userData.Version);

            if (listener != null)
            {
                service.TraceListener = listener;
                service.TraceFlags = TraceFlags.All;
                service.TraceEnabled = true;
            }

            service.Credentials = new NetworkCredential(userData.UserId, userData.Password);

            ImpersonatedUserId impersonatedUserId =
              new ImpersonatedUserId(ConnectingIdType.SmtpAddress, impersonatedUserSMTPAddress);

            service.ImpersonatedUserId = impersonatedUserId;

            if (userData.AutodiscoverUrl == null)
            {
                service.AutodiscoverUrl(userData.UserId, RedirectionUrlValidationCallback);
                userData.AutodiscoverUrl = service.Url;
            }
            else
            {
                service.Url = userData.AutodiscoverUrl;
            }

            return service;
        }
    }
}

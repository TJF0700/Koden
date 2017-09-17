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
using Koden.Utils.SharePoint;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Koden.Utils.Extensions
{
    /// <summary>
    /// Extensions for Koden.Utils - SharePoint 
    /// </summary>
    public static class SPExtensions
    {
        internal static void kSetupRequestHeaderByOperation(this HttpWebRequest request, Operation operation, Dictionary<string, string> additionalHeaders)
        {
            switch (operation)
            {
                case Operation.CREATE:
                    request.Method = "POST";
                    request.Headers.Add("X-HTTP-Method", "POST");
                    break;
                case Operation.READ:
                    request.Method = "GET";
                    request.ContentType = "application/json; odata=verbose";
                    break;
                case Operation.UPDATE:
                    request.Method = "POST";
                    request.Headers.Add("IF-MATCH", "*");
                    request.Headers.Add("X-HTTP-Method", "MERGE");
                    break;
                case Operation.DELETE:
                    request.Method = "POST";
                    request.Headers.Add("IF-MATCH", "*");
                    request.Headers.Add("X-HTTP-Method", "DELETE");
                    request.ContentLength = 0;
                    break;
                case Operation.UPLOAD:
                    request.Method = "POST";
                    request.Headers.Add("X-HTTP-Method", "POST");
                    request.ContentType = "application/json; odata=verbose";
                    request.ContentLength = 0;
                    break;
            }
            request.Accept = "application/json; odata=verbose";
            


            if (additionalHeaders != null)
            {
                additionalHeaders.Keys.ToList().ForEach(k =>
                    request.Headers.Add(k, additionalHeaders[k]));
            }

        }

    }
}

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
using System.Configuration;
using System.Linq;

namespace Koden.Utils
{
    /// <summary>
    /// Config (Work with .config files)
    /// </summary>
    public static class Config
    {

        /// <summary>
        /// Gets the application key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <param name="defReturn">The default value to return if key doesn't exist or is empty.</param>
        /// <returns>String from AppKey in *.config file *or* default</returns>
        public static string GetAppKey(string key, bool toLower = false, string defReturn = "")
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                defReturn = toLower == true ? ConfigurationManager.AppSettings[key].ToLower() : ConfigurationManager.AppSettings[key];
            }
            return defReturn;
        }

        /// <summary>
        /// Updates the app key in app config. (Will not work in WEB.Config!)
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="newValue">The new value.</param>
        /// <returns></returns>
        public static bool UpdateAppKeyInAppConfig(string key, string newValue)
        {
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings[key]))
            {
                Configuration config =  ConfigurationManager.OpenExeConfiguration( ConfigurationUserLevel.PerUserRoamingAndLocal);
                config.AppSettings.Settings.Remove(key);
                config.AppSettings.Settings.Add(key, newValue);
                config.Save();
                return true;
            }
            return false;
        }
    }

}

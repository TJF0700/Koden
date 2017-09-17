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
using Koden.Utils.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Koden.Utils.Extensions
{
    public static partial class ExtensionMethods
    {
        private static string _csvSeparator = ",";

        /// <summary>
        /// Defaults to ","
        /// </summary>
        public static string CSVSeparator
        {
            get { return _csvSeparator; }
            set { _csvSeparator = value; }
        }

        /// <summary>
        /// Populate a class from a JSON formatted file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tClass">The t class.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="createEmpty">if set to <c>true</c> [create empty].</param>
        /// <returns>
        /// Populated class from json representation in File
        /// </returns>
        public static T kLoadFromJsonFile<T>(this T tClass, string fileName = "", bool createEmpty = false)
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) + ".persist";

            try
            {
                if (!File.Exists(fileName))
                {
                    if (createEmpty)
                    {
                        default(T).kSaveToJsonFile(fileName);
                    }
                    else
                    {
                        throw new FileNotFoundException(string.Format("File does not exist: {0}", fileName));
                    }
                }

                return JsonConvert.DeserializeObject<T>(File.ReadAllText(fileName));
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// Loads JSON from string into class(type).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="tClass"></param>
        /// <param name="jsonString">The json string.</param>
        /// <returns>Populated class from json representation in String</returns>
        public static T kLoadFromJsonString<T>(this T tClass, string jsonString)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Saves a class in JSON format to a file
        /// </summary>
        /// <typeparam name="T">The type of the class.</typeparam>
        /// <param name="tClass">The class to persist to file.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="append">append to file? (logging, maybe).</param>
        public static void kSaveToJsonFile<T>(this T tClass, string fileName = "", bool append = false)
        {

            if (String.IsNullOrEmpty(fileName))
                fileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) + ".persist";

            try
            {
                var fMode = append == false ? FileMode.Create : FileMode.Append;
                using (FileStream fs = File.Open(fileName, fMode))
                using (StreamWriter sw = new StreamWriter(fs))
                using (JsonWriter jw = new JsonTextWriter(sw))
                {
                    jw.Formatting = Formatting.Indented;
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(jw, tClass);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Saves an IEnumerable class/model to CSV file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">The objects.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void kSaveToCSVFile<T>(this IEnumerable<T> objects, string fileName = "")
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) + ".csv";

            var fields =
                from mi in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                where new[] { MemberTypes.Field, MemberTypes.Property }.Contains(mi.MemberType)
                let orderAttr = (kColumnOrderAttribute)Attribute.GetCustomAttribute(mi, typeof(kColumnOrderAttribute))
                orderby orderAttr == null ? int.MaxValue : orderAttr.Order, mi.Name
                select mi;

            using (TextWriter output = new StreamWriter(fileName))
            {
                output.WriteLine(QuoteRecord(fields.Select(f => f.Name)));
                foreach (var record in objects)
                {
                    output.WriteLine(QuoteRecord(FormatObject(fields, record)));
                }
            }
        }

        /// <summary>
        /// Saves an IEnumerable class/model to CSV file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects">The objects.</param>
        /// <param name="fileName">Name of the file.</param>
        public static void kSaveToCSVFile<T>(this IList<T> objects, string fileName = "")
        {
            if (String.IsNullOrEmpty(fileName))
                fileName = Path.GetFileNameWithoutExtension(Process.GetCurrentProcess().MainModule.FileName) + ".csv";

            var fields =
                from mi in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                where new[] { MemberTypes.Field, MemberTypes.Property }.Contains(mi.MemberType)
                let orderAttr = (kColumnOrderAttribute)Attribute.GetCustomAttribute(mi, typeof(kColumnOrderAttribute))
                orderby orderAttr == null ? int.MaxValue : orderAttr.Order, mi.Name
                select mi;

            using (TextWriter output = new StreamWriter(fileName))
            {
                output.WriteLine(QuoteRecord(fields.Select(f => f.Name)));
                foreach (var record in objects)
                {
                    output.WriteLine(QuoteRecord(FormatObject(fields, record)));
                }
            }
        }

        /// <summary>
        /// Formats the object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fields">The fields.</param>
        /// <param name="record">The record.</param>
        /// <returns><see cref="IEnumerable&lt;String&gt;"/></returns>
        /// <exception cref="System.Exception">Unhandled case.</exception>
        internal static IEnumerable<string> FormatObject<T>(IEnumerable<MemberInfo> fields, T record)
        {
            foreach (var field in fields)
            {
                if (field is FieldInfo)
                {
                    var fi = (FieldInfo)field;
                    yield return Convert.ToString(fi.GetValue(record));
                }
                else if (field is PropertyInfo)
                {
                    var pi = (PropertyInfo)field;
                    yield return Convert.ToString(pi.GetValue(record, null));
                }
                else
                {
                    throw new Exception("Unhandled case.");
                }
            }
        }



        /// <summary>
        /// Adds Quotes to the record.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns><see cref="string"/> representing joined record with quotes</returns>
        internal static string QuoteRecord(IEnumerable<string> record)
        {
            return String.Join(CSVSeparator, record.Select(field => QuoteField(field)).ToArray());
        }

        /// <summary>
        /// Adds Quotes to the field.
        /// </summary>
        /// <param name="field">The field.</param>
        /// <returns><see cref="string"/> representing joined field with quotes</returns>
        internal static string QuoteField(string field)
        {
            if (String.IsNullOrEmpty(field))
            {
                return "\"\"";
            }
            else if (field.Contains(CSVSeparator) || field.Contains("\"") || field.Contains("\r") || field.Contains("\n"))
            {
                return String.Format("\"{0}\"", field.Replace("\"", "\"\""));
            }
            else
            {
                return field;
            }
        }
    }
}

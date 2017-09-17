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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Koden.Utils.Extensions;

namespace Koden.Utils.Test.ExtensionTests
{
    [TestClass]
    public class DictionaryExtensionTests
    {
        [TestMethod]
        public void TestDefault_String()
        {
            var tstDictString = new Dictionary<string, string>();
            tstDictString.Add("test1", "correct");
            tstDictString.Add("test2", "right");

            var chkDict = tstDictString.kGetValueOrDefault("test1","fail");

            Assert.AreEqual(chkDict, "correct","Found correct key (stringKey)");
            chkDict = tstDictString.kGetValueOrDefault("test3", "MyDefault");
            Assert.AreEqual(chkDict, "MyDefault", "No Key Found, using default 'MyDefault' (stringKey)");


        }

        [TestMethod]
        public void TestDefault_Int()
        {
            var tstDictString = new Dictionary<int, string>();
            tstDictString.Add(1, "correct");
            tstDictString.Add(2, "right");

            var chkDict = tstDictString.kGetValueOrDefault(1, "fail");

            Assert.AreEqual(chkDict, "correct", "Found correct key (intKey)");
            chkDict = tstDictString.kGetValueOrDefault(3, "MyDefault");
            Assert.AreEqual(chkDict, "MyDefault", "No Key Found, using default 'MyDefault' (intKey)");


        }
    }
}

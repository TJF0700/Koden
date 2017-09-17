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
using System.Linq;

namespace Koden.Utils.Test.ExtensionTests
{
    [TestClass]
    public class ForEachExtensionTests
    {
        [TestMethod]
        public void TestExtListForEach()
        {
            var tstList = new List<string>
            {
                "test1",
                "test2"
            };

            tstList.kForEach(s =>  string.Format("{0} works", s) );

            Assert.AreEqual("test1 works", tstList[0]);
            Assert.AreEqual("test2 works", tstList[1]);
        }

        [TestMethod]
        public void TestExtEnumerableForEach()
        {
            var tstList = new List<string>
            {
                "test1",
                "test2"
            }.AsEnumerable();

            tstList.kForEach(s => string.Format("{0} works", s));

            Assert.IsTrue(tstList.Contains("test1 works"));
            Assert.IsTrue(tstList.Contains("test2 works"));
        }
    }
}

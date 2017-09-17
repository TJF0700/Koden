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
using Koden.Utils.Extensions;

namespace Koden.Utils.Test.ExtensionTests
{
    [TestClass]
    public class RandomizeExtensionTests
    {
        [TestMethod]
        public void TestExtRandomize()
        {
            var TstString = "LastName";
            var TstString2 = "123456789";

            TstString = TstString.kRandomize(Models.RandomizeType.TextOnly, 43);
            TstString2 = TstString2.kRandomize(Models.RandomizeType.NumbersOnly, 3);
            Assert.AreNotEqual(TstString, "LastName", "TstString has been randomized");
            Assert.AreNotEqual(TstString2, "123456789", "TstString2 has been randomize");
            Assert.IsTrue(TstString2.kIsNumeric(), "TstString2 is no longer Numeric");
        }

        [TestMethod]
        public void TestExtCeasarShift()
        {
            var TstString = "ABCDEFG";
            TstString = TstString.kCaesar(4);
            Assert.AreEqual("EFGHIJK", TstString,"TstString is NOT shifted correctly at 4");
            TstString = TstString.kCaesar(24);
            Assert.AreEqual("]^_`abc", TstString, "TstString is NOT shifted correctly at 24");

        }
    }
}

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
    [Flags]
    enum TestEnum
    {
        flag0 = 0,
        flag1 = 1,
        flag2 = 2,
        flag3 = 4,
        flag4 = 8,
        flag5 = 16
    }
    [TestClass]
    public class EnumExtensionTests
    {
        [TestMethod]
        public void TestExtEnumHasFlag()
        {
            var tstEnum = TestEnum.flag1 | TestEnum.flag4;

            Assert.IsTrue(tstEnum.kHasFlag(TestEnum.flag4), "tstEnum HAS flag 'flag3'");
            Assert.IsFalse(tstEnum.kHasFlag(TestEnum.flag2), "tstEnum DOES NOT HAVE flag 'flag2'");

        }

        [TestMethod]
        public void TestExtEnumAddFlag()
        {
            var tstEnum = TestEnum.flag1 | TestEnum.flag4;
            tstEnum = tstEnum.kAddFlag(TestEnum.flag5);
            Assert.IsTrue(tstEnum.kHasFlag(TestEnum.flag5), "tstEnum HAS flag 'flag5' now");

        }

        [TestMethod]
        public void TestExtEnumRemoveFlag()
        {
            var tstEnum = TestEnum.flag1 | TestEnum.flag4;
            tstEnum = tstEnum.kRemoveFlag(TestEnum.flag1);
            Assert.IsFalse(tstEnum.kHasFlag(TestEnum.flag1), "tstEnum NO LONGER has flag 'flag1'");

        }

        [TestMethod]
        public void TestExtEnumIsFlag()
        {
            var tstEnum = TestEnum.flag1;
            Assert.IsTrue(tstEnum.kIsFlag(TestEnum.flag1), "tstEnum is flag 'flag1'");

        }

        [TestMethod]
        public void TestExtLongHasFlag()
        {
            long tstLong = 1 | 4;

            Assert.IsTrue(tstLong.kHasFlag(4), "tstLong HAS long '3'");
            Assert.IsFalse(tstLong.kHasFlag(2), "tstLong DOES NOT HAVE long '2'");

        }

        [TestMethod]
        public void TestExtLongAddFlag()
        {
            long tstLong = 1 | 4;
            tstLong = tstLong.kAddFlag(5);
            Assert.IsTrue(tstLong.kHasFlag(5), "tstLong HAS long '5' now");

        }

        [TestMethod]
        public void TestExtLongRemoveFlag()
        {
            long tstLong = 1 | 4;
            tstLong = tstLong.kRemoveFlag(1);
            Assert.IsFalse(tstLong.kHasFlag(1), "tstEnum NO LONGER has long '1'");

        }

        [TestMethod]
        public void TestExtLongIsFlag()
        {
            long tstLong = 1;
            Assert.IsTrue(tstLong.kIsFlag(1), "tstLong is flag '1'");

        }
    }
}

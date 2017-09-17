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
using Koden.Utils.Models;
using Koden.Utils.Extensions;

namespace Koden.Utils.Test.ExtensionTests
{
    [TestClass]
    public class DateTimeExtensionTests
    {
        [TestMethod]
        public void TestQuarterInfo()
        {
            //Q1
            QuarterInfo qtrInfo = new DateTime(2017, 1, 19).kQuarterInfo();
            Assert.AreEqual(1, qtrInfo.Qtr, "Incorrect Quarter Number returned (Q1)");
            Assert.AreEqual("1st Quarter", qtrInfo.ToLongString(qtrInfo.Qtr), "Incorrect Long Format returned (Q1)");
            Assert.AreEqual("Q1", qtrInfo.ToShortString(qtrInfo.Qtr), "Incorrect Short Format returned (Q1)");
            Assert.AreEqual("1st Half", qtrInfo.ToSemiAnnualString(qtrInfo.Qtr), "Incorrect SemiAnnual Format returned (Q1)");
            //Q2
            qtrInfo = new DateTime(2017, 4, 20).kQuarterInfo();
            Assert.AreEqual(2, qtrInfo.Qtr, "Incorrect Quarter Number returned (Q2)");
            Assert.AreEqual("2nd Quarter", qtrInfo.ToLongString(qtrInfo.Qtr), "Incorrect Long Format returned (Q2)");
            Assert.AreEqual("Q2", qtrInfo.ToShortString(qtrInfo.Qtr), "Incorrect Short Format returned (Q2)");
            Assert.AreEqual("1st Half", qtrInfo.ToSemiAnnualString(qtrInfo.Qtr), "Incorrect SemiAnnual Format returned (Q2)");
            //Q3
            qtrInfo = new DateTime(2017, 8, 10).kQuarterInfo();
            Assert.AreEqual(3, qtrInfo.Qtr, "Incorrect Quarter Number returned (3)");
            Assert.AreEqual("3rd Quarter", qtrInfo.ToLongString(qtrInfo.Qtr), "Incorrect Long Format returned (Q3)");
            Assert.AreEqual("Q3", qtrInfo.ToShortString(qtrInfo.Qtr), "Incorrect Short Format returned (Q3)");
            Assert.AreEqual("2nd Half", qtrInfo.ToSemiAnnualString(qtrInfo.Qtr), "Incorrect SemiAnnual Format returned (Q3)");
            //Q4
            qtrInfo = new DateTime(2017, 11, 1).kQuarterInfo();
            Assert.AreEqual(4, qtrInfo.Qtr, "Incorrect Quarter Number returned (Q4)");
            Assert.AreEqual("4th Quarter", qtrInfo.ToLongString(qtrInfo.Qtr), "Incorrect Long Format returned (Q4)");
            Assert.AreEqual("Q4", qtrInfo.ToShortString(qtrInfo.Qtr), "Incorrect Short Format returned (Q4)");
            Assert.AreEqual("2nd Half", qtrInfo.ToSemiAnnualString(qtrInfo.Qtr), "Incorrect SemiAnnual Format returned (Q4)");

        }
        [TestMethod]
        public void TestFiscalInfo()
        {
            //FQ1
            FiscalInfo fisInfo = new DateTime(2017, 7, 19).kFiscalInfo(7, false);
            Assert.AreEqual(1, fisInfo.FiscalQtr, "Incorrect Quarter Number returned");
            Assert.AreEqual("1st Quarter", fisInfo.ToLongString(fisInfo.FiscalQtr), "Incorrect Long Format returned");
            Assert.AreEqual("Q1", fisInfo.ToShortString(fisInfo.FiscalQtr), "Incorrect Short Format returned");
            Assert.AreEqual("1st Half", fisInfo.ToSemiAnnualString(fisInfo.FiscalQtr), "Incorrect SemiAnnual Format returned");
            Assert.AreEqual(new DateTime(2017, 7, 1), fisInfo.FirstDayOfFiscalPeriod, "Incorrect FirstDay of Period (FQ1)");
            Assert.AreEqual(new DateTime(2017, 9, 30), fisInfo.LastDayOfFiscalPeriod, "Incorrect Last Day of Period (FQ1)");
            
            //FQ2
            fisInfo = new DateTime(2017, 11, 15).kFiscalInfo(7);
            Assert.AreEqual(2, fisInfo.FiscalQtr, "Incorrect Quarter Number returned");
            Assert.AreEqual("2nd Quarter", fisInfo.ToLongString(fisInfo.FiscalQtr), "Incorrect Long Format returned");
            Assert.AreEqual("Q2", fisInfo.ToShortString(fisInfo.FiscalQtr), "Incorrect Short Format returned");
            Assert.AreEqual("1st Half", fisInfo.ToSemiAnnualString(fisInfo.FiscalQtr), "Incorrect SemiAnnual Format returned");
            //FQ3
            fisInfo = new DateTime(2018, 1, 15).kFiscalInfo(7);
            Assert.AreEqual(3, fisInfo.FiscalQtr, "Incorrect Quarter Number returned");
            Assert.AreEqual("3rd Quarter", fisInfo.ToLongString(fisInfo.FiscalQtr), "Incorrect Long Format returned");
            Assert.AreEqual("Q3", fisInfo.ToShortString(fisInfo.FiscalQtr), "Incorrect Short Format returned");
            Assert.AreEqual("2nd Half", fisInfo.ToSemiAnnualString(fisInfo.FiscalQtr), "Incorrect SemiAnnual Format returned");
            //FQ4
            fisInfo = new DateTime(2018, 4, 15).kFiscalInfo(7, false);
            Assert.AreEqual(4, fisInfo.FiscalQtr, "Incorrect Quarter Number returned");
            Assert.AreEqual("4th Quarter", fisInfo.ToLongString(fisInfo.FiscalQtr), "Incorrect Long Format returned");
            Assert.AreEqual("Q4", fisInfo.ToShortString(fisInfo.FiscalQtr), "Incorrect Short Format returned");
            Assert.AreEqual("2nd Half", fisInfo.ToSemiAnnualString(fisInfo.FiscalQtr), "Incorrect SemiAnnual Format returned");
            Assert.AreEqual(new DateTime(2018, 4, 1), fisInfo.FirstDayOfFiscalPeriod, "Incorrect FirstDay of Period (FQ4)");
            Assert.AreEqual(new DateTime(2018, 6, 30), fisInfo.LastDayOfFiscalPeriod, "Incorrect Last Day of Period (FQ4)");

        }

        [TestMethod]
        public void TestBetween()
        {
            var blnTest = new DateTime(2017, 6, 1).kBetween(new DateTime(2017,6,2), new DateTime(2017,7,1));
            Assert.AreEqual(false, blnTest, "Failed on not between");

            blnTest = new DateTime(2017, 6, 1).kBetween(new DateTime(2017, 5, 30), new DateTime(2017, 6, 2));
            Assert.AreEqual(true, blnTest, "Failed on between is true");

        }


        [TestMethod]
        public void TestCountDaysBetween()
        {
            var blnCount = new DateTime(2017, 6, 1).kCountDaysBetween(new DateTime(2017, 6, 30));
            Assert.AreEqual(29, blnCount, "Failed on count days between");

            blnCount = new DateTime(2017, 8, 2).kCountDaysBetween(new DateTime(2017, 9, 5));
            Assert.AreEqual(34, blnCount, "Failed on count days between (2)");

        }
    }
}

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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;

namespace Koden.Utils.Test.ExtensionTests
{
    class TstModel
    {
        public string TstString { get; set; }
        public int TstInt { get; set; }
        public Collection<string> TstList { get; set; }
    }
    [TestClass]
    public class PersistenceExtensionTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException), "File not found.")]
        public void TestExtLoadFromJSONFileException()
        {
            string tstFileName = "NotExistFile.persist";
            ClearTestFile(tstFileName);
            var tstLoad = new TstModel();
            tstLoad.kLoadFromJsonFile(tstFileName);
        }

       

        [TestMethod]
        public void TestExtLoadFromJSONFileCreate()
        {
            string tstFileName = "TestFile.persist";
            ClearTestFile(tstFileName);
            var tstLoad = new TstModel();
            tstLoad = tstLoad.kLoadFromJsonFile(tstFileName, true);
            Assert.IsTrue(File.Exists(tstFileName));
        }
        [TestMethod]
        public void TestExtLoadFromJSONFile()
        {
            string tstFileName = "TestFile.persist";
            ClearTestFile(tstFileName);
            var tstLoadCreate = new TstModel
            {
                TstInt = 1,
                TstList = new Collection<string> { "test1", "test2", "test3" },
                TstString = "This is a test string"
            };
            tstLoadCreate.kSaveToJsonFile(tstFileName);
            Assert.IsTrue(File.Exists(tstFileName), "Failed on created test filename");

            //load data from file
            var tstLoad = new TstModel().kLoadFromJsonFile(tstFileName, false);
            Assert.AreEqual(1, tstLoad.TstInt);
            Assert.AreEqual("This is a test string", tstLoad.TstString);
            CollectionAssert.AreEqual(tstLoad.TstList, tstLoadCreate.TstList,"Data is NOT the same after load");
        }





        private void ClearTestFile(string tstFileName)
        {
            if (File.Exists(tstFileName))
            {
                File.Delete(tstFileName);
            }
        }
    }
}

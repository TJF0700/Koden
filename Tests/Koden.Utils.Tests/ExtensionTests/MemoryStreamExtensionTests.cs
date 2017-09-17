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
using System.IO;
using Koden.Utils.Extensions;
using System.Text;

namespace Koden.Utils.Test.ExtensionTests
{
    [TestClass]
    public class MemoryStreamExtensionTests
    {
        [TestMethod]
        public void TestExtMemoryStreamGetAsString()
        {
            using (var test_Stream = new MemoryStream(Encoding.UTF8.GetBytes("This is a test stream")))
            {
                var result = test_Stream.kGetAsString();

                // Assert    
                Assert.AreEqual(result, "This is a test stream");
            }
        }

        [TestMethod]
        public void TestExtMemoryStreamWriteString()
        {
            var test_Stream = new MemoryStream();
            test_Stream.kWriteString("This is a test stream");

            var result = test_Stream.kGetAsString();
            // Assert    
            Assert.AreEqual(result, "This is a test stream");
        }
    }
}


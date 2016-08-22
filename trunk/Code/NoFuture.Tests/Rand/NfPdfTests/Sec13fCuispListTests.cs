﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.NfPdfTests
{
    [TestClass]
    public class Sec13fCuispListTests
    {
        [TestMethod]
        public void TestTryParseCusipLine()
        {
            var testInput = "989701 13 1 ZIONS BANCORPORATION *W EXP 11/14/201";
            var testSubject = new NoFuture.Rand.Data.NfPdf.Sec13FCuispList(null);
            string[] testResultOut = null;
            var testResult = testSubject.TryParseCusipLine(testInput, out testResultOut);
            Assert.IsTrue(testResult);
            Assert.IsNotNull(testResultOut);
            Assert.AreNotEqual(0,testResultOut.Length);
            foreach(var i in testResultOut)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestParseContent()
        {
            //not added to src control - too big for just tests
            var testFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\13flist2016q2.pdf";
            Assert.IsTrue(System.IO.File.Exists(testFile));
            var testInput = System.IO.File.ReadAllBytes(testFile);
            Assert.AreNotEqual(0,testInput.Length);
            var testSubject = new NoFuture.Rand.Data.NfPdf.Sec13FCuispList(null);
            var testResult = testSubject.ParseContent(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            //sample some
            var testContent = new List<string>();
            foreach (var t in testResult)
            {
                var i = t.Issuer as string;
                var ii = t.Issue as string;
                var ck = t.ChkDigit as string;

                var tIn = $"{i}{ii}";
                var tTe = ck;
                testContent.Add($"{tIn} {tTe}");
                
            }
            System.IO.File.AppendAllLines(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\CusipChkDigitTest.txt", testContent);
        }
    }
}
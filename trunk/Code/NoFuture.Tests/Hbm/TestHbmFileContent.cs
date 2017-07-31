﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Hbm;
using NoFuture.Shared;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestHbmFileContent
    {
        public string TestFilePath = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles\localhost\Whatever";
        public const string IdHbmXmlTestFile = "Dbo.TableWithVarcharPk.hbm.xml";
        public const string CompositeIdXmlTestFile = "Dbo.TableWithCompositePk.hbm.xml";
        public const string StoredProcXmlTestFile = "Dbo.MyStoredProc.hbm.xml";


        [TestMethod]
        public void TestCtor()
        {
            var testResult =
                new NoFuture.Hbm.SortingContainers.HbmFileContent(Path.Combine(TestFilePath, IdHbmXmlTestFile));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult.Classname);
            Assert.AreNotEqual(string.Empty, testResult.Namespace);
            Assert.AreNotEqual(string.Empty, testResult.DbSchema);
            Assert.AreNotEqual(string.Empty, testResult.IdName);
            Assert.AreNotEqual(string.Empty, testResult.IdType);
            Assert.IsFalse(testResult.IsCompositeKey);

            testResult =
                new NoFuture.Hbm.SortingContainers.HbmFileContent(Path.Combine(TestFilePath, CompositeIdXmlTestFile));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult.Classname);
            Assert.AreNotEqual(string.Empty, testResult.Namespace);
            Assert.AreNotEqual(string.Empty, testResult.DbSchema);
            Assert.AreNotEqual(string.Empty, testResult.IdName);
            Assert.AreNotEqual(string.Empty, testResult.IdType);
            Assert.IsTrue(testResult.IsCompositeKey);
            Assert.AreNotEqual(0, testResult.CompositeKeyProperties);
            Assert.AreNotEqual(0, testResult.ListProperties.Count);
            foreach (var k in testResult.IdAsSimpleProperties.Keys)
                System.Diagnostics.Debug.WriteLine(string.Join(" : ", new[] { k, testResult.IdAsSimpleProperties[k] }));

            testResult =
                new NoFuture.Hbm.SortingContainers.HbmFileContent(Path.Combine(TestFilePath, StoredProcXmlTestFile));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(string.Empty, testResult.Classname);
            Assert.AreNotEqual(string.Empty, testResult.Namespace);
            Assert.AreNotEqual(string.Empty, testResult.DbSchema);
            Assert.AreNotEqual(string.Empty, testResult.IdName);
            Assert.AreNotEqual(string.Empty, testResult.IdType);
            Assert.IsFalse(testResult.IsCompositeKey);
            Assert.IsNotNull( testResult.SpConstNames);
            Assert.AreNotEqual(0, testResult.SpConstNames);
        }

        [TestMethod]
        public void TestCreateXpath()
        {
            var testResult = NoFuture.Hbm.SortingContainers.HbmFileContent.CreateXpath("hibernate-mapping", "class",
                "id");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("//hbm:hibernate-mapping/hbm:class/hbm:id", testResult);
        }

        [TestMethod]
        public void TestGetColumnDataByPropertyName()
        {
            var testSubject = new NoFuture.Hbm.SortingContainers.HbmFileContent(Path.Combine(TestFilePath, CompositeIdXmlTestFile));
            foreach (var kl in testSubject.IdAsSimpleProperties.Keys)
            {
                System.Diagnostics.Debug.WriteLine(kl);
                var testResult = testSubject.GetColumnDataByPropertyName(kl);
                Assert.IsNotNull(testResult);
                Assert.AreNotEqual(0, testResult.Length);
            }

            foreach (var s in testSubject.SimpleProperties.Keys)
            {
                System.Diagnostics.Debug.WriteLine(s);
                var testResult = testSubject.GetColumnDataByPropertyName(s);
                Assert.IsNotNull(testResult);
                Assert.AreNotEqual(0, testResult.Length);
            }

            foreach (var k in testSubject.FkProperties.Keys)
            {
                var testResult = testSubject.GetColumnDataByPropertyName(k);
                Assert.IsNotNull(testResult);
                Assert.AreNotEqual(0, testResult.Length);
            }

            foreach (var b in testSubject.ListProperties.Keys)
            {
                var testResult = testSubject.GetColumnDataByPropertyName(b);
                Assert.IsNotNull(testResult);
                Assert.AreNotEqual(0, testResult.Length);

            }

            testSubject = new NoFuture.Hbm.SortingContainers.HbmFileContent(Path.Combine(TestFilePath, IdHbmXmlTestFile));
            var simpleTestResult = testSubject.GetColumnDataByPropertyName(testSubject.IdName);
            Assert.IsNotNull(simpleTestResult);
            Assert.AreNotEqual(0, simpleTestResult.Length);
        }
    }
}

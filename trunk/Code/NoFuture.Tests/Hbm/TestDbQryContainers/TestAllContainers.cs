using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Globals;
using NoFuture.Hbm.DbQryContainers.MetadataDump;

namespace NoFuture.Tests.Hbm.TestDbQryContainers
{
    [TestClass]
    public class TestAllContainers
    {
        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Hbm = @"C:\Projects\31g\trunk\temp\code\hbm";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }

        [TestMethod]
        public void TestColumnNames()
        {
            var testSubject = new HbmAllKeys();
            var testResult = testSubject.QueryKeysNames;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult);
            foreach(var n in testResult)
                System.Diagnostics.Debug.WriteLine(n);
        }

        [TestMethod]
        public void TestHbmAllColumns()
        {
            var testSubject = new HbmAllColumns();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
            var testResult = testSubject.Data;
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
        public void TestHbmAllIndex()
        {
            var testSubject = new HbmAllIndex();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
            var testResult = testSubject.Data;
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.Write(testResult.Length);
        }

        [TestMethod]
        public void TestHbmAllKeys()
        {
            var testSubject = new HbmAllKeys();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [TestMethod]
        public void TestHbmAutoIncrement()
        {
            var testSubject = new HbmAutoIncrement();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [TestMethod]
        public void TestHbmConstraints()
        {
            var testSubject = new HbmContraints();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [TestMethod]
        public void TestHbmFlatData()
        {
            var testSubject = new HbmFlatData();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [TestMethod]
        public void TestHbmPrimaryKeys()
        {
            var testSubject = new HbmPrimaryKeys();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }
    }
}

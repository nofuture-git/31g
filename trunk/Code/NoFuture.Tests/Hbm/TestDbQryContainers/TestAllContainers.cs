using System;
using NUnit.Framework;
using NoFuture.Hbm.DbQryContainers.MetadataDump;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Hbm.TestDbQryContainers
{
    [TestFixture]
    public class TestAllContainers
    {
        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = @"C:\Projects\31g\trunk\temp\code\hbm";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }

        [Test]
        public void TestColumnNames()
        {
            var testSubject = new HbmAllKeys();
            var testResult = testSubject.QueryKeysNames;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult);
            foreach(var n in testResult)
                Console.WriteLine(n);
        }

        [Test]
        public void TestHbmAllColumns()
        {
            var testSubject = new HbmAllColumns();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
            var testResult = testSubject.Data;
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestHbmAllIndex()
        {
            var testSubject = new HbmAllIndex();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
            var testResult = testSubject.Data;
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.Write(testResult.Length);
        }

        [Test]
        public void TestHbmAllKeys()
        {
            var testSubject = new HbmAllKeys();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [Test]
        public void TestHbmAutoIncrement()
        {
            var testSubject = new HbmAutoIncrement();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [Test]
        public void TestHbmConstraints()
        {
            var testSubject = new HbmContraints();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [Test]
        public void TestHbmFlatData()
        {
            var testSubject = new HbmFlatData();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }

        [Test]
        public void TestHbmPrimaryKeys()
        {
            var testSubject = new HbmPrimaryKeys();
            Assert.IsNotNull(testSubject.SelectStatement);
            Assert.IsNotNull(testSubject.OutputPath);
        }
    }
}

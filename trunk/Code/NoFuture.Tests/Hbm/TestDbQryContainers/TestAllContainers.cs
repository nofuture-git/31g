using System;
using NoFuture.Hbm.DbQryContainers.MetadataDump;
using NoFuture.Shared.Cfg;
using NUnit.Framework;

namespace NoFuture.Hbm.Tests.TestDbQryContainers
{
    [TestFixture]
    public class TestAllContainers
    {
        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = TestAssembly.GetTestFileDirectory();
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

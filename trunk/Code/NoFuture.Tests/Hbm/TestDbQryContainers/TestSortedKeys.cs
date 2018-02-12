using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NoFuture.Hbm;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared.Cfg;

namespace NoFuture.Tests.Hbm.TestDbQryContainers
{
    [TestFixture]
    public class TestSortedKeys
    {
        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }
        [Test]
        public void TestGetDistinctConstraintNames()
        {
            var testData = new NoFuture.Hbm.DbQryContainers.SortedKeys();
            var testOutput = new List<ColumnMetadata>();
            var testResult = testData.GetKeyManyToOneColumns("dbo.TableWithCompositePk", ref testOutput);
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestConstraintNameComparer()
        {
            var testData = new NoFuture.Hbm.DbQryContainers.SortedKeys();
            var testOutput = new List<ColumnMetadata>();
            var testInput = testData.GetKeyManyToOneColumns("dbo.TableWithCompositePk", ref testOutput);
            foreach(var cd in testOutput)
                Console.WriteLine(cd.constraint_name);
            var testResult = testOutput.Distinct(new ConstraintNameComparer()).ToList();
            Assert.AreEqual(1,testResult.Count);
        }
    }
}

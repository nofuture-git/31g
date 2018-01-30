using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Hbm;
using NoFuture.Hbm.SortingContainers;
using NoFuture.Shared.Cfg;

namespace NoFuture.Tests.Hbm.TestDbQryContainers
{
    [TestClass]
    public class TestSortedKeys
    {
        [TestInitialize]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
        }
        [TestMethod]
        public void TestGetDistinctConstraintNames()
        {
            var testData = new NoFuture.Hbm.DbQryContainers.SortedKeys();
            var testOutput = new List<ColumnMetadata>();
            var testResult = testData.GetKeyManyToOneColumns("dbo.TableWithCompositePk", ref testOutput);
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void TestConstraintNameComparer()
        {
            var testData = new NoFuture.Hbm.DbQryContainers.SortedKeys();
            var testOutput = new List<ColumnMetadata>();
            var testInput = testData.GetKeyManyToOneColumns("dbo.TableWithCompositePk", ref testOutput);
            foreach(var cd in testOutput)
                System.Diagnostics.Debug.WriteLine(cd.constraint_name);
            var testResult = testOutput.Distinct(new ConstraintNameComparer()).ToList();
            Assert.AreEqual(1,testResult.Count);
        }
    }
}

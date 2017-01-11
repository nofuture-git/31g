using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Hbm.DbQryContainers;
using NoFuture.Shared;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestMapping
    {
        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
            NoFuture.Hbm.Mapping.HbmBags = new SortedBags();
            NoFuture.Hbm.Mapping.HbmKeys = new SortedKeys();
            NoFuture.Hbm.Mapping.HbmOneToMany = new SortedOneToMany();
        }

        [TestMethod]
        public void TestGetSingleHbmXml()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithCompositePk");
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [TestMethod]
        public void TestGetSingleHbmXml2()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithFkRefToMulti");
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [TestMethod]
        public void TestGetSingleHbmXml3()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithFkToComposite");
            Assert.IsTrue(System.IO.File.Exists(testResult));
            
        }
        [TestMethod]
        public void TestGetSingleHbmXml4()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithNoPkButWithUqIdx");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [TestMethod]
        public void TestGetSingleHbmXml5()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithNoPkMultipleIx");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [TestMethod]
        public void TestGetSingleHbmXml6()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithNoPkSingleUqIxMutliColumns");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [TestMethod]
        public void TestGetSingleHbmXml7()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithVarcharPk");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        
        [TestMethod]
        [ExpectedException(typeof(Exceptions.ItsDeadJim))]
        public void TestGetSingleHbmXml8()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableIncompatiableWithOrm");
        }

        [TestMethod]
        public void TestGetHbmNamedQueryXml()
        {
            var testResult = NoFuture.Hbm.Mapping.GetHbmNamedQueryXml("NoFuture.TestHbm.Prox", "dbo.MyStoredProc");
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

    }

}

using System;
using NUnit.Framework;
using NoFuture.Hbm.DbQryContainers;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Hbm
{
    [TestFixture]
    public class TestMapping
    {
        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Hbm = TestAssembly.UnitTestsRoot + @"\Hbm\TestFiles";
            NfConfig.SqlServer = "localhost";
            NfConfig.SqlCatalog = "Whatever";
            NoFuture.Hbm.Mapping.HbmBags = new SortedBags();
            NoFuture.Hbm.Mapping.HbmKeys = new SortedKeys();
            NoFuture.Hbm.Mapping.HbmOneToMany = new SortedOneToMany();
        }

        [Test]
        public void TestGetSingleHbmXml()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithCompositePk");
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [Test]
        public void TestGetSingleHbmXml2()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithFkRefToMulti");
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [Test]
        public void TestGetSingleHbmXml3()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithFkToComposite");
            Assert.IsTrue(System.IO.File.Exists(testResult));
            
        }
        [Test]
        public void TestGetSingleHbmXml4()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithNoPkButWithUqIdx");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [Test]
        public void TestGetSingleHbmXml5()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithNoPkMultipleIx");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [Test]
        public void TestGetSingleHbmXml6()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithNoPkSingleUqIxMutliColumns");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [Test]
        public void TestGetSingleHbmXml7()
        {
            var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableWithVarcharPk");
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        
        [Test]
        public void TestGetSingleHbmXml8()
        {
            try
            {
                var testResult = NoFuture.Hbm.Mapping.GetSingleHbmXml("NoFuture.TestHbm", "dbo.TableIncompatiableWithOrm");
            }
            catch (ItsDeadJim)
            {
                Assert.True(true);
            }
        }

        [Test]
        public void TestGetHbmNamedQueryXml()
        {
            var testResult = NoFuture.Hbm.Mapping.GetHbmNamedQueryXml("NoFuture.TestHbm.Prox", "dbo.MyStoredProc");
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

    }

}

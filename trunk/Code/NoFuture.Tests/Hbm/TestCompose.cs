using System;
using NUnit.Framework;

namespace NoFuture.Tests.Hbm
{
    [TestFixture]
    public class TestCompose
    {
        [Test]
        public void TestClassName()
        {
            var testResult = NoFuture.Hbm.Compose.ClassName("dbo.123ProcName", "NoFuture");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("NoFuture.Dbo.Nf123ProcName, NoFuture, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",testResult);
            
            testResult = NoFuture.Hbm.Compose.ClassName("dbo.ShipmentTrackingMaster", "NoFuture");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("NoFuture.Dbo.ShipmentTrackingMaster, NoFuture, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null",testResult);

            testResult = NoFuture.Hbm.Compose.ClassName("StoredProc.dbo.123ProcName", "NoFuture");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);

            testResult = NoFuture.Hbm.Compose.ClassName("dbo.GET CLIENT COORDINATOR DETAILS", "NoFuture");
        }

        [Test]
        public void TestPropertyName()
        {
            var testResult = NoFuture.Hbm.Compose.PropertyName("# of Stations");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("NfofStations",testResult);

            testResult = NoFuture.Hbm.Compose.PropertyName("ExpectedArrivalDate");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("ExpectedArrivalDate",testResult);
        }

        [Test]
        public void TestManyToOnePropertyName()
        {
            var testTypeName =
                "NoFuture.Dbo.AccountDetails, NoFuture, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null";
            var testColumnNames = new[] { "dbo.ProgramMaster.accountId" };
            var testResult = NoFuture.Hbm.Compose.ManyToOnePropertyName(testTypeName, testColumnNames);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual("AccountDetailsByAccountId",testResult);
        }
    }
}

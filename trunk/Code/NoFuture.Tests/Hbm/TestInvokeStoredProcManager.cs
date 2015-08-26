using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Hbm;

namespace NoFuture.Tests.Hbm
{
    [TestClass]
    public class TestInvokeStoredProcManager
    {
        [TestMethod]
        public void TestInvokeStoredProcExeMessage()
        {
            var testSubject = new NoFuture.Hbm.InvokeStoredProcExeMessage()
            {
                StoredProcName = "dbo.TestProc",
                State = InvokeStoredProcExeState.Complete
            };

            var testInput = testSubject.ToString();
            InvokeStoredProcExeMessage testOutput;
            Assert.IsTrue(InvokeStoredProcExeMessage.TryParse(testInput, out testOutput));

            Assert.AreEqual(testSubject.StoredProcName, testOutput.StoredProcName);
            Assert.AreEqual(testSubject.State, testOutput.State);
        }
    }
}

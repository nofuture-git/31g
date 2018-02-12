using System;
using NUnit.Framework;
using NoFuture.Hbm;

namespace NoFuture.Tests.Hbm
{
    [TestFixture]
    public class TestInvokeStoredProcManager
    {
        [Test]
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

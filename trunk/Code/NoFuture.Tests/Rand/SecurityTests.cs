using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class SecurityTests
    {
        [TestMethod]
        public void TestLong()
        {
            var testSubject = new NoFuture.Rand.Data.Sp.Security("013817101");
            testSubject.Push(DateTime.Today.AddHours(8), 25M.ToPecuniam(), 0.5M.ToPecuniam());

            testSubject.Pop(DateTime.Today.AddHours(17), 30M.ToPecuniam(), 0.5M.ToPecuniam());

            Assert.AreEqual(SpStatus.Closed, testSubject.GetStatus(DateTime.Today.AddHours(18)));

            Assert.AreEqual(4.0M, testSubject.GetValueAt(DateTime.Today.AddHours(18)).Amount);

            Assert.AreEqual(Pecuniam.Zero, testSubject.GetValueAt(DateTime.Today));
        }
    }
}

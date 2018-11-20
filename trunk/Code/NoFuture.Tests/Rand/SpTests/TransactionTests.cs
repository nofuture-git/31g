using System;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void TestGetInverse()
        {
            var testSubject = new Transaction(DateTime.Now, 128M.ToPecuniam(), Guid.NewGuid());
            var testResult = testSubject.GetInverse();
            Assert.IsTrue((testSubject.Cash + testResult.Cash).GetRounded() == Pecuniam.Zero);
        }
    }
}

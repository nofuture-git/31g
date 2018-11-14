using System;
using System.Collections.Generic;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class SecurityTests
    {
        [Test]
        public void TestGetValueAt()
        {
            var testSubject = new Security("CUSIPID", 25);
            var n = DateTime.Today;
            var historicData = new List<Tuple<DateTime, decimal>>
            {
                new Tuple<DateTime, decimal>(n.AddDays(-17), 53M),
                new Tuple<DateTime, decimal>(n.AddDays(-19), 51M),
                new Tuple<DateTime, decimal>(n.AddDays(-18), 52M),
                new Tuple<DateTime, decimal>(n.AddDays(-15), 55M),
                new Tuple<DateTime, decimal>(n.AddDays(-12), 58M),
                new Tuple<DateTime, decimal>(n.AddDays(-20), 50M),
                new Tuple<DateTime, decimal>(n.AddDays(-16), 54M),
                new Tuple<DateTime, decimal>(n.AddDays(-16), 999M),
                new Tuple<DateTime, decimal>(n.AddDays(-13), 57M),
                new Tuple<DateTime, decimal>(n.AddDays(-14), 56M),
                new Tuple<DateTime, decimal>(n.AddDays(-11), 59M),
                new Tuple<DateTime, decimal>(n.AddDays(-10), 60M),
            };

            testSubject.SetHistoricData(historicData, CurrencyAbbrev.USD);

            var testResult = testSubject.GetValueAt(n.AddDays(-16));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(999M.ToPecuniam(), testResult);
            Assert.AreEqual(54M.ToPecuniam(), testResult);

            var midDate = n.AddDays(-14).AddHours(8).AddMinutes(12).AddSeconds(45);

            testResult = testSubject.GetValueAt(midDate);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(56M.ToPecuniam(), testResult);

            testResult = testSubject.GetValueAt(n);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(Pecuniam.Zero, testResult);

        }
    }
}

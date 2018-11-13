using System;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class MortgageTests
    {
        [Test]
        public void TestGetEstimatedMarketValueAt()
        {
            var testSubject = GetTestSubject();
            PushHistoryInto(testSubject);
            var testResult = testSubject.GetEstimatedMarketValueAt(null);

            Assert.IsTrue(testResult > Pecuniam.Zero);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestGetEstimatedEquityAt()
        {
            var testSubject = GetTestSubject();
            PushHistoryInto(testSubject);
            var testResult = testSubject.GetEquityAt(null);
            Assert.IsTrue(testResult > Pecuniam.Zero);

            Console.WriteLine(testResult);

        }

        [Test]
        public void TestMonthlyPayment()
        {
            var testSubject = GetTestSubject();
            var testResult = testSubject.MonthlyPayment;
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testResult.GetAbs().Amount, 1990.59M);
        }

        [Test]
        public void TestMinPaymentRate()
        {
            var testSubject = GetTestSubject();
            var testResult = testSubject.MinPaymentRate;
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testResult, 0.002778f);
        }

        private Mortgage GetTestSubject()
        {
            var purchaseDate = new DateTime(2008, 12, 1);
            
            var purchasePrice = 393291.02;
            var rate = 0.02f;
            var addr = new PostalAddress();
            var addrData = new AddressData
            {
                ThoroughfareNumber = "123",
                Locality = "abc",
                PostalCode = "99999",
                ThoroughfareName = "lmnop",
                ThoroughfareType = "st"
            };
            addr.CityArea = new UsCityStateZip(addrData);
            addr.Street = new UsStreetPo(addrData);

            var testSubject = new Mortgage(addr, purchaseDate, rate, purchasePrice.ToPecuniam());

            return testSubject;
        }

        private void PushHistoryInto(Mortgage testSubject)
        {
            var remainingCost = 155504.04;
            var monthlyPayment = 1990.59;
            var dtIncrement = testSubject.Inception.AddMonths(1);
            while (testSubject.GetValueAt(dtIncrement) > remainingCost.ToPecuniam())
            {
                if (dtIncrement > DateTime.Now.AddYears(30))
                    break;
                testSubject.AddNegativeValue(dtIncrement, monthlyPayment.ToPecuniam());
                dtIncrement = dtIncrement.AddMonths(1);
            }
        }
    }
}

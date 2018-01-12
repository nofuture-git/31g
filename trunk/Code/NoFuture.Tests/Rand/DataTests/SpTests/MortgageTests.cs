using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Geo;
using NoFuture.Rand.Geo.US;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class MortgageTests
    {
        [TestMethod]
        public void TestGetEstimatedMarketValueAt()
        {
            var testSubject = GetTestSubject();
            PushHistoryInto(testSubject);
            var testResult = testSubject.GetEstimatedMarketValueAt(null);

            Assert.IsTrue(testResult > Pecuniam.Zero);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetEstimatedEquityAt()
        {
            var testSubject = GetTestSubject();
            PushHistoryInto(testSubject);
            var testResult = testSubject.GetEquityAt(null);
            Assert.IsTrue(testResult > Pecuniam.Zero);

            System.Diagnostics.Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestMonthlyPayment()
        {
            var testSubject = GetTestSubject();
            var testResult = testSubject.MonthlyPayment;
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testResult.Abs.Amount, 1990.59M);
        }

        [TestMethod]
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
                AddressNumber = "123",
                City = "abc",
                PostalCode = "99999",
                StreetName = "lmnop",
                StreetType = "st"
            };
            addr.HomeCityArea = new UsCityStateZip(addrData);
            addr.HomeStreetPo = new UsStreetPo(addrData);

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
                testSubject.Push(dtIncrement, monthlyPayment.ToPecuniam());
                dtIncrement = dtIncrement.AddMonths(1);
            }
        }
    }
}

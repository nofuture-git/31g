using System;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class TradeLineTests
    {
        [Test]
        public void TestGetCurrent()
        {
            var testSubject = new TradeLine(DateTime.Now.AddDays(-370));
            var testBalance = testSubject.Balance;

            var today = DateTime.Today;

            testBalance.AddTransaction(today.AddDays(-360), new Pecuniam(-450.0M));
            testBalance.AddTransaction(today.AddDays(-120), new Pecuniam(-458.0M));
            testBalance.AddTransaction(today.AddDays(-150), new Pecuniam(-457.0M));
            testBalance.AddTransaction(today.AddDays(-240), new Pecuniam(-454.0M));
            testBalance.AddTransaction(today.AddDays(-300), new Pecuniam(-452.0M));
            testBalance.AddTransaction(today.AddDays(-270), new Pecuniam(-453.0M));
            testBalance.AddTransaction(today.AddDays(-180), new Pecuniam(-456.0M));
            testBalance.AddTransaction(today.AddDays(-210), new Pecuniam(-455.0M));
            testBalance.AddTransaction(today.AddDays(-330), new Pecuniam(-451.0M));

            //charges
            testBalance.AddTransaction(today.AddDays(-365), new Pecuniam(8000.0M));
            testBalance.AddTransaction(today.AddDays(-350), new Pecuniam(164.4M));
            testBalance.AddTransaction(today.AddDays(-198), new Pecuniam(165.4M));
            testBalance.AddTransaction(today.AddDays(-24), new Pecuniam(166.4M));
            testBalance.AddTransaction(today.AddDays(-74), new Pecuniam(167.4M));
            testBalance.AddTransaction(today.AddDays(-88), new Pecuniam(168.4M));
            testBalance.AddTransaction(today.AddDays(-92), new Pecuniam(169.4M));
            testBalance.AddTransaction(today.AddDays(-121), new Pecuniam(170.4M));
            testBalance.AddTransaction(today.AddDays(-180), new Pecuniam(171.4M));
            testBalance.AddTransaction(today.AddDays(-142), new Pecuniam(172.4M));
            testBalance.AddTransaction(today.AddDays(-155), new Pecuniam(173.4M));

            testBalance.AddTransaction(today.AddDays(-30), new Pecuniam(-461.0M));
            testBalance.AddTransaction(today.AddDays(-90), new Pecuniam(-459.0M));
            testBalance.AddTransaction(today.AddDays(-60), new Pecuniam(-460.0M));

            var current = testBalance.GetCurrent(DateTime.Today, 0.0f);
            Assert.AreEqual(4223.0M, current.Amount);

            current = testBalance.GetCurrent(DateTime.Today, 1.5f);
            Assert.AreEqual(25704.93M, current.Amount);

        }

        [Test]
        public void TestAveragePerPeriod()
        {
            var dt = DateTime.Today;
            var testSubject = new TradeLine(DateTime.Now.AddDays(-180));
            testSubject.DueFrequency = new TimeSpan(30,0,0,0);
            for (var i = -180; i < 0; i += 30)
            {
                var pastDt = dt.AddDays(i);
                for (var j = 0; j < 3; j++)
                {
                    //should get average around 28 per 30 days
                    testSubject.AddPositiveValue(pastDt.AddDays(j), new Pecuniam(27 + j));
                }
            }

            var testResult = testSubject.AveragePerDueFrequency();
            Console.WriteLine(testResult);

            Assert.AreEqual(28M.ToPecuniam(), testResult);
        }
    }
}

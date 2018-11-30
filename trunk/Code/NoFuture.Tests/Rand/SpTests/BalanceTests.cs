using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class BalanceTests
    {
        [Test]
        public void TestGetCurrent_OnlyOneTransaction()
        {
            var testBalance = new Balance();
            var expected = 10000.ToPecuniam();
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-11), expected);
            var testResult = testBalance.GetCurrent(DateTime.UtcNow, 0f);
            Assert.AreEqual(expected, testResult);
        }

        [Test]
        public void TestGetCurrentNoInterest()
        {
            var testBalance = new Balance();
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-15), new Pecuniam(2000.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-12), new Pecuniam(-451.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-12), new Pecuniam(-101.91M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-12), new Pecuniam(-87.88M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-12), new Pecuniam(-32.47M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-12), new Pecuniam(-16.88M));

            var testResult = testBalance.GetCurrent(DateTime.UtcNow, 0);
            Assert.AreEqual(1309.86M, testResult.Amount);
            Console.WriteLine(testResult.Amount);
        }

        [Test]
        public void TestGetPaymentSum()
        {
            var testSubject = new Balance();
            var dt = DateTime.UtcNow;

            testSubject.AddNegativeValue(dt.AddDays(-360), new Pecuniam(-450.0M));
            testSubject.AddNegativeValue(dt.AddDays(-30), new Pecuniam(-461.0M));
            testSubject.AddNegativeValue(dt.AddDays(-120), new Pecuniam(-458.0M));
            testSubject.AddNegativeValue(dt.AddDays(-150), new Pecuniam(-457.0M));
            testSubject.AddNegativeValue(dt.AddDays(-90), new Pecuniam(-459.0M));
            testSubject.AddNegativeValue(dt.AddDays(-240), new Pecuniam(-454.0M));
            testSubject.AddNegativeValue(dt.AddDays(-60), new Pecuniam(-460.0M));
            testSubject.AddNegativeValue(dt.AddDays(-300), new Pecuniam(-452.0M));
            testSubject.AddNegativeValue(dt.AddDays(-270), new Pecuniam(-453.0M));
            testSubject.AddNegativeValue(dt.AddDays(-180), new Pecuniam(-456.0M));
            testSubject.AddNegativeValue(dt.AddDays(-210), new Pecuniam(-455.0M));
            testSubject.AddNegativeValue(dt.AddDays(-330), new Pecuniam(-451.0M));

            //charges
            testSubject.AddPositiveValue(dt.AddDays(-365), new Pecuniam(8000.0M));
            testSubject.AddPositiveValue(dt.AddDays(-350), new Pecuniam(164.4M));
            testSubject.AddPositiveValue(dt.AddDays(-198), new Pecuniam(165.4M));
            testSubject.AddPositiveValue(dt.AddDays(-24), new Pecuniam(166.4M));
            testSubject.AddPositiveValue(dt.AddDays(-74), new Pecuniam(167.4M));
            testSubject.AddPositiveValue(dt.AddDays(-88), new Pecuniam(168.4M));
            testSubject.AddPositiveValue(dt.AddDays(-92), new Pecuniam(169.4M));
            testSubject.AddPositiveValue(dt.AddDays(-121), new Pecuniam(170.4M));
            testSubject.AddPositiveValue(dt.AddDays(-180), new Pecuniam(171.4M));
            testSubject.AddPositiveValue(dt.AddDays(-142), new Pecuniam(172.4M));
            testSubject.AddPositiveValue(dt.AddDays(-155), new Pecuniam(173.4M));

            var testResult =
                testSubject.GetDebitSum(new Tuple<DateTime, DateTime>(dt.AddDays(-31).Date, DateTime.UtcNow));

            Assert.AreEqual(-461.0M, testResult.Amount);

            testSubject.AddNegativeValue(dt.AddDays(-15), new Pecuniam(-120.0M));

            testResult =
                testSubject.GetDebitSum(new Tuple<DateTime, DateTime>(dt.AddDays(-31).Date, DateTime.UtcNow));

            Assert.AreEqual((-461.0M - 120.0M), testResult.Amount);

            testResult = testSubject.GetDebitSum(new Tuple<DateTime, DateTime>(dt.AddDays(-365), dt));

            Assert.AreEqual(-5466.0M - 120.0M, testResult.Amount);

            testResult = testSubject.GetCreditSum(new Tuple<DateTime, DateTime>(dt.AddDays(-365), dt));

            Assert.AreEqual(9689M, testResult.Amount);

        }


        [Test]
        public void TestTransactionsGetCurrent()
        {
            var testBalance = new Balance();
            //monthly payments

            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-360), new Pecuniam(-450.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-30), new Pecuniam(-461.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-120), new Pecuniam(-458.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-150), new Pecuniam(-457.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-90), new Pecuniam(-459.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-240), new Pecuniam(-454.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-60), new Pecuniam(-460.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-300), new Pecuniam(-452.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-270), new Pecuniam(-453.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-180), new Pecuniam(-456.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-210), new Pecuniam(-455.0M));
            testBalance.AddNegativeValue(DateTime.UtcNow.AddDays(-330), new Pecuniam(-451.0M));

            //charges
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-365), new Pecuniam(8000.0M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-350), new Pecuniam(164.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-198), new Pecuniam(165.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-24), new Pecuniam(166.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-74), new Pecuniam(167.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-88), new Pecuniam(168.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-92), new Pecuniam(169.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-121), new Pecuniam(170.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-180), new Pecuniam(171.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-142), new Pecuniam(172.4M));
            testBalance.AddPositiveValue(DateTime.UtcNow.AddDays(-155), new Pecuniam(173.4M));

            var testResult = testBalance.GetCurrent(DateTime.UtcNow, 0.0875f);

            Assert.IsTrue(testResult.Amount <= 4723.45M || testResult.Amount >= 4723.46M);
            Console.WriteLine(testResult.Amount);

        }


        [Test]
        public void TestTransactionsGetCurrentWithVariableRate()
        {
            //set some past date 
            var testBalance = new Balance();
            var dt = DateTime.UtcNow;
            testBalance.AddPositiveValue(dt.AddDays(-360), new Pecuniam(450.0M));
            //180 day spread
            testBalance.AddPositiveValue(dt.AddDays(-180), new Pecuniam(450.0M));
            //150 day spread
            testBalance.AddPositiveValue(dt.AddDays(-30), new Pecuniam(450.0M));

            //say there was not interest for the first 180 days
            var testVariableRate = new Dictionary<DateTime, float>
            {
                {dt.AddDays(-180), 0.0F},
                {dt.AddDays(-30), 0.055F},
                {dt.AddDays(14), 0.195F}//30 days ago the rate jumped to usury
            };

            var testResult = testBalance.GetCurrent(dt, testVariableRate);

            Assert.AreEqual(1392.68M, testResult.Amount);

        }


        [Test]
        public void TestGetTransactionsFromUpTo()
        {
            //set some past date 
            var testBalance = new Balance();
            var dt = DateTime.Today.ToUniversalTime();
            testBalance.AddPositiveValue(dt.AddDays(-360), new Pecuniam(450.0M));
            testBalance.AddPositiveValue(dt.AddDays(-180), new Pecuniam(450.0M));
            testBalance.AddPositiveValue(dt.AddDays(-30), new Pecuniam(450.0M));

            var testResult = testBalance.GetTransactions(dt.AddDays(-360), dt.AddDays(-180));
            Assert.AreEqual(1, testResult.Count);
            testResult = testBalance.GetTransactions(dt.AddDays(-360), dt.AddDays(-180), true);
            Assert.AreEqual(2, testResult.Count);
            testResult = testBalance.GetTransactions(dt.AddDays(-360), dt.AddDays(-179));
            Assert.AreEqual(2, testResult.Count);
            testResult = testBalance.GetTransactions(dt.AddDays(-360), dt);
            Assert.AreEqual(3, testResult.Count);

        }


        [Test]
        public void TestSort()
        {
            var testBalance = new Balance();
            //monthly payments

            var dt = DateTime.UtcNow;

            var oldestDt = dt.AddDays(-360);
            var newestDt = dt.AddDays(-30);

            testBalance.AddNegativeValue(oldestDt, new Pecuniam(-450.0M));
            testBalance.AddNegativeValue(newestDt, new Pecuniam(-461.0M));
            testBalance.AddNegativeValue(dt.AddDays(-120), new Pecuniam(-458.0M));
            testBalance.AddNegativeValue(dt.AddDays(-150), new Pecuniam(-457.0M));
            testBalance.AddNegativeValue(dt.AddDays(-90), new Pecuniam(-459.0M));
            testBalance.AddNegativeValue(dt.AddDays(-240), new Pecuniam(-454.0M));
            testBalance.AddNegativeValue(dt.AddDays(-60), new Pecuniam(-460.0M));
            testBalance.AddNegativeValue(dt.AddDays(-300), new Pecuniam(-452.0M));
            testBalance.AddNegativeValue(dt.AddDays(-270), new Pecuniam(-453.0M));
            testBalance.AddNegativeValue(dt.AddDays(-180), new Pecuniam(-456.0M));
            testBalance.AddNegativeValue(dt.AddDays(-210), new Pecuniam(-455.0M));
            testBalance.AddNegativeValue(dt.AddDays(-330), new Pecuniam(-451.0M));

            var testResult = testBalance.DataSet.FirstOrDefault();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.AtTime.Date == oldestDt.Date);

            testResult = testBalance.DataSet.LastOrDefault();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.AtTime.Date == newestDt.Date);
        }

        [Test]
        public void TestGetInverse()
        {
            var testBalance = new Balance();
            var dt = DateTime.UtcNow.AddDays(-1);

            for (var i = 0; i <= 10; i++)
            {
                testBalance.AddPositiveValue(dt.AddDays(i*-1), 128M.ToPecuniam());
            }

            var testBalanceValue = testBalance.GetCurrent(DateTime.UtcNow, 0.0f);
            Console.WriteLine(testBalanceValue);

            var testResult = testBalance.GetInverse() as Balance;
            Assert.IsNotNull(testResult);

            Assert.AreEqual(testBalance.TransactionCount, testResult.TransactionCount);
            foreach (var t in testBalance.DataSet)
            {
                
                var dayMatch = testResult.DataSet.FirstOrDefault(tr => DateTime.Equals(t.AtTime, tr.AtTime));
                Assert.IsNotNull(dayMatch);
                var diff = (t.Cash + dayMatch.Cash).GetRounded();
                Console.WriteLine(diff);
                Assert.IsTrue(diff == Pecuniam.Zero);

            }
            Assert.IsTrue(testBalanceValue + testResult.GetCurrent(DateTime.UtcNow, 0.0f) == Pecuniam.Zero);
        }

        [Test]
        public void TestGetDebitsCredits()
        {
            var testBalance = new Balance();
            var dt = DateTime.UtcNow.AddDays(-1);
            var numOfEachEntry = 12;
            for (var i = 0; i < numOfEachEntry; i++)
            {
                testBalance.AddPositiveValue(dt.AddDays(-1 * i), 10M.ToPecuniam());
            }

            for (var i = 0; i < numOfEachEntry; i++)
            {
                testBalance.AddNegativeValue(dt.AddDays(-1 * i), (-10M).ToPecuniam());
            }
            Assert.AreEqual(numOfEachEntry*2, testBalance.TransactionCount);
            var testResult = testBalance.GetCredits();
            Assert.IsNotNull(testResult);
            Assert.AreEqual(numOfEachEntry, testResult.Count);
            Assert.IsTrue(testResult.Sum(x => x.Cash.Amount) > 0);

            testResult = testBalance.GetDebits();
            Assert.IsNotNull(testResult);
            Assert.AreEqual(numOfEachEntry, testResult.Count);
            Assert.IsTrue(testResult.Sum(x => x.Cash.Amount) < 0);

        }

        [Test]
        public void TestGetSumPerDay()
        {
            var testBalance = new Balance();
            var origDt = DateTime.Now.AddDays(-7);
            var dt = origDt;

            testBalance.AddPositiveValue(dt, 1500M.ToPecuniam(), new VocaBase("AssetAccount00"));
            testBalance.AddNegativeValue(dt, 1500M.ToPecuniam(), new VocaBase("LiabilityAccount00"));

            //the next day
            dt = dt.AddDays(1);

            //jagged balance
            testBalance.AddPositiveValue(dt, 150M.ToPecuniam(), new VocaBase("AssetAccount01"));
            testBalance.AddPositiveValue(dt.AddMinutes(30), 250M.ToPecuniam(), new VocaBase("AssetAccount00"));

            //skip two days
            dt = dt.AddDays(4);
            testBalance.AddPositiveValue(dt, 1500M.ToPecuniam(), new VocaBase("AssetAccount00"));
            testBalance.AddNegativeValue(dt.AddMinutes(30), 900M.ToPecuniam(), new VocaBase("LiabilityAccount00"));
            testBalance.AddNegativeValue(dt.AddHours(3), 600M.ToPecuniam(), new VocaBase("LiabilityAccount01"));

            var testResult = testBalance.GetSumPerDay();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            Assert.IsTrue(testResult.ContainsKey(origDt.Date));
            var trAtDate = testResult[origDt.Date];
            Assert.IsNotNull(trAtDate);
            Assert.AreEqual(Pecuniam.Zero, trAtDate);

            Assert.IsTrue(testResult.ContainsKey(origDt.AddDays(1).Date));
            trAtDate = testResult[origDt.AddDays(1).Date];
            Assert.IsNotNull(trAtDate);
            Assert.AreNotEqual(Pecuniam.Zero, trAtDate);

            Assert.IsTrue(testResult.ContainsKey(origDt.AddDays(5).Date));
            trAtDate = testResult[origDt.AddDays(5).Date];
            Assert.IsNotNull(trAtDate);
            Assert.AreEqual(Pecuniam.Zero, trAtDate);

        }
    }
}

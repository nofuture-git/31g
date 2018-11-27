﻿using System;
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

            var testResult = testBalance.Transactions.FirstOrDefault();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.AtTime.Date == oldestDt.Date);

            testResult = testBalance.Transactions.LastOrDefault();
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
            foreach (var t in testBalance.Transactions)
            {
                
                var dayMatch = testResult.Transactions.FirstOrDefault(tr => DateTime.Equals(t.AtTime, tr.AtTime));
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
        public void TestPostBalance()
        {
            var testSubject = new Balance();
            var testInput = new Balance();
            var dt = DateTime.UtcNow;
            var testInputIds = new List<Tuple<Guid, Guid>>();
            for (var i = 0; i < 24; i++)
            {
                var atDt = dt.AddDays((-1 * i));
                var amt = (i * 10M).ToPecuniam();
                if (i % 2 == 0)
                {
                    var creditId = testInput.AddPositiveValue(atDt, amt);
                    testInputIds.Add(new Tuple<Guid, Guid>(testInput.Id, creditId));
                }
                else
                {
                    var debitId = testInput.AddNegativeValue(atDt, amt);
                    testInputIds.Add(new Tuple<Guid, Guid>(testInput.Id, debitId));
                }
            }

            //expect all entries are only in testInput
            var testInputCreditSum = testInput.GetCreditSum(null);
            var testInputDebitSum = testInput.GetDebitSum(null);

            Assert.AreNotEqual(Pecuniam.Zero, testInputCreditSum);
            Assert.AreNotEqual(Pecuniam.Zero, testInputDebitSum);

            var testSubjectCreditSum = testSubject.GetCreditSum(null);
            var testSubjectDebitSum = testSubject.GetDebitSum(null);

            Assert.AreEqual(Pecuniam.Zero, testSubjectCreditSum);
            Assert.AreEqual(Pecuniam.Zero, testSubjectDebitSum);

            //perform transfer
            testSubject.PostBalance(testInput);

            //expect all entries are not in testSubject
            testSubjectCreditSum = testSubject.GetCreditSum(null);
            testSubjectDebitSum = testSubject.GetDebitSum(null);

            //expect all value is present
            Assert.AreEqual(testInputCreditSum, testSubjectCreditSum);
            Assert.AreEqual(testInputDebitSum, testSubjectDebitSum);

            //expect that testInput has been reduced to zero
            testInputCreditSum = testInput.GetCreditSum(null);
            testInputDebitSum = testInput.GetDebitSum(null);

            //expect the input to now have a zero balance
            Assert.AreEqual(Pecuniam.Zero, (testInputCreditSum + testInputDebitSum).GetWholeNumber());

            var testResults = testSubject.GetCredits();
            testResults.AddRange(testSubject.GetDebits());

            //expect to find evidence in the trace of each item
            foreach (var tr in testResults)
            {
                var trace = tr.Trace;
                Assert.IsNotNull(trace);
                Assert.IsTrue(testInputIds.Any(v => v.Item1 == testInput.Id && v.Item2 == trace.UniqueId));
            }

        }

        [Test]
        public void TestTransferCredit()
        {
            var testSource = new Balance();
            var testDest = new Balance();
            var dt = DateTime.UtcNow;
            //now the source has a large balance
            testSource.AddPositiveValue(dt.AddDays(-4), 10000.ToPecuniam(), new VocaBase("initial deposit"));

            var testCreditId = testDest.AddPositiveValue(testSource, 1000.ToPecuniam(), dt.AddDays(-3));

            //expect the balance of source to be 1000 less
            Assert.AreEqual(9000.ToPecuniam(), testSource.GetCurrent(dt, 0f));
            
            //expect the balance of dest to be just this one deposit
            Assert.AreEqual(1000.ToPecuniam(), testDest.GetCurrent(dt, 0f));

            var testRemCashFromSource = testSource.GetDebits().FirstOrDefault();
            Assert.IsNotNull(testRemCashFromSource);
            Assert.AreEqual(1000.ToPecuniam().GetNeg(), testRemCashFromSource.Cash);

            var testAddCashToDest = testDest.GetCredits().FirstOrDefault();
            Assert.IsNotNull(testAddCashToDest);
            Assert.AreEqual(1000.ToPecuniam(), testAddCashToDest.Cash);
            Assert.AreEqual(testCreditId, testAddCashToDest.UniqueId);
        }


    }
}

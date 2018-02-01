﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
            testBalance.AddTransaction(DateTime.Now.AddDays(-15), new Pecuniam(2000.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-12), new Pecuniam(-451.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-12), new Pecuniam(-101.91M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-12), new Pecuniam(-87.88M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-12), new Pecuniam(-32.47M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-12), new Pecuniam(-16.88M));

            var testResult = testBalance.GetCurrent(DateTime.Now, 0);
            Assert.AreEqual(1309.86M, testResult.Amount);
            Console.WriteLine(testResult.Amount);


        }

        [Test]
        public void TestGetPaymentSum()
        {
            var testSubject = new Balance();
            var dt = DateTime.Now;

            testSubject.AddTransaction(dt.AddDays(-360), new Pecuniam(-450.0M));
            testSubject.AddTransaction(dt.AddDays(-30), new Pecuniam(-461.0M));
            testSubject.AddTransaction(dt.AddDays(-120), new Pecuniam(-458.0M));
            testSubject.AddTransaction(dt.AddDays(-150), new Pecuniam(-457.0M));
            testSubject.AddTransaction(dt.AddDays(-90), new Pecuniam(-459.0M));
            testSubject.AddTransaction(dt.AddDays(-240), new Pecuniam(-454.0M));
            testSubject.AddTransaction(dt.AddDays(-60), new Pecuniam(-460.0M));
            testSubject.AddTransaction(dt.AddDays(-300), new Pecuniam(-452.0M));
            testSubject.AddTransaction(dt.AddDays(-270), new Pecuniam(-453.0M));
            testSubject.AddTransaction(dt.AddDays(-180), new Pecuniam(-456.0M));
            testSubject.AddTransaction(dt.AddDays(-210), new Pecuniam(-455.0M));
            testSubject.AddTransaction(dt.AddDays(-330), new Pecuniam(-451.0M));

            //charges
            testSubject.AddTransaction(dt.AddDays(-365), new Pecuniam(8000.0M));
            testSubject.AddTransaction(dt.AddDays(-350), new Pecuniam(164.4M));
            testSubject.AddTransaction(dt.AddDays(-198), new Pecuniam(165.4M));
            testSubject.AddTransaction(dt.AddDays(-24), new Pecuniam(166.4M));
            testSubject.AddTransaction(dt.AddDays(-74), new Pecuniam(167.4M));
            testSubject.AddTransaction(dt.AddDays(-88), new Pecuniam(168.4M));
            testSubject.AddTransaction(dt.AddDays(-92), new Pecuniam(169.4M));
            testSubject.AddTransaction(dt.AddDays(-121), new Pecuniam(170.4M));
            testSubject.AddTransaction(dt.AddDays(-180), new Pecuniam(171.4M));
            testSubject.AddTransaction(dt.AddDays(-142), new Pecuniam(172.4M));
            testSubject.AddTransaction(dt.AddDays(-155), new Pecuniam(173.4M));

            var testResult =
                testSubject.GetDebitSum(new Tuple<DateTime, DateTime>(dt.AddDays(-31).Date, DateTime.Now));

            Assert.AreEqual(-461.0M, testResult.Amount);

            testSubject.AddTransaction(dt.AddDays(-15), new Pecuniam(-120.0M));

            testResult =
                testSubject.GetDebitSum(new Tuple<DateTime, DateTime>(dt.AddDays(-31).Date, DateTime.Now));

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

            testBalance.AddTransaction(DateTime.Now.AddDays(-360), new Pecuniam(-450.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-30), new Pecuniam(-461.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-120), new Pecuniam(-458.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-150), new Pecuniam(-457.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-90), new Pecuniam(-459.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-240), new Pecuniam(-454.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-60), new Pecuniam(-460.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-300), new Pecuniam(-452.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-270), new Pecuniam(-453.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-180), new Pecuniam(-456.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-210), new Pecuniam(-455.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-330), new Pecuniam(-451.0M));

            //charges
            testBalance.AddTransaction(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-350), new Pecuniam(164.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-198), new Pecuniam(165.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-24), new Pecuniam(166.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-74), new Pecuniam(167.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-88), new Pecuniam(168.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-92), new Pecuniam(169.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-121), new Pecuniam(170.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-180), new Pecuniam(171.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-142), new Pecuniam(172.4M));
            testBalance.AddTransaction(DateTime.Now.AddDays(-155), new Pecuniam(173.4M));

            var testResult = testBalance.GetCurrent(DateTime.Now, 0.0875f);

            Assert.IsTrue(testResult.Amount <= 4723.45M || testResult.Amount >= 4723.46M);
            Debug.WriteLine(testResult.Amount);

        }


        [Test]
        public void TestTransactionsGetCurrentWithVariableRate()
        {
            //set some past date 
            var testBalance = new Balance();
            var dt = DateTime.Now;
            testBalance.AddTransaction(dt.AddDays(-360), new Pecuniam(450.0M));
            //180 day spread
            testBalance.AddTransaction(dt.AddDays(-180), new Pecuniam(450.0M));
            //150 day spread
            testBalance.AddTransaction(dt.AddDays(-30), new Pecuniam(450.0M));

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
            testBalance.AddTransaction(dt.AddDays(-360), new Pecuniam(450.0M));
            testBalance.AddTransaction(dt.AddDays(-180), new Pecuniam(450.0M));
            testBalance.AddTransaction(dt.AddDays(-30), new Pecuniam(450.0M));

            var testResult = testBalance.GetTransactionsBetween(dt.AddDays(-360), dt.AddDays(-180));
            Assert.AreEqual(1, testResult.Count);
            testResult = testBalance.GetTransactionsBetween(dt.AddDays(-360), dt.AddDays(-180), true);
            Assert.AreEqual(2, testResult.Count);
            testResult = testBalance.GetTransactionsBetween(dt.AddDays(-360), dt.AddDays(-179));
            Assert.AreEqual(2, testResult.Count);
            testResult = testBalance.GetTransactionsBetween(dt.AddDays(-360), dt);
            Assert.AreEqual(3, testResult.Count);

        }


        [Test]
        public void TestSort()
        {
            var testBalance = new Balance();
            //monthly payments

            var dt = DateTime.Now;

            var oldestDt = dt.AddDays(-360);
            var newestDt = dt.AddDays(-30);

            testBalance.AddTransaction(oldestDt, new Pecuniam(-450.0M));
            testBalance.AddTransaction(newestDt, new Pecuniam(-461.0M));
            testBalance.AddTransaction(dt.AddDays(-120), new Pecuniam(-458.0M));
            testBalance.AddTransaction(dt.AddDays(-150), new Pecuniam(-457.0M));
            testBalance.AddTransaction(dt.AddDays(-90), new Pecuniam(-459.0M));
            testBalance.AddTransaction(dt.AddDays(-240), new Pecuniam(-454.0M));
            testBalance.AddTransaction(dt.AddDays(-60), new Pecuniam(-460.0M));
            testBalance.AddTransaction(dt.AddDays(-300), new Pecuniam(-452.0M));
            testBalance.AddTransaction(dt.AddDays(-270), new Pecuniam(-453.0M));
            testBalance.AddTransaction(dt.AddDays(-180), new Pecuniam(-456.0M));
            testBalance.AddTransaction(dt.AddDays(-210), new Pecuniam(-455.0M));
            testBalance.AddTransaction(dt.AddDays(-330), new Pecuniam(-451.0M));

            var testResult = testBalance.Transactions.FirstOrDefault();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.AtTime.Date == oldestDt.Date);

            testResult = testBalance.Transactions.LastOrDefault();
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.AtTime.Date == newestDt.Date);
        }
    }
}
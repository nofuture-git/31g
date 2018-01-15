﻿using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class SecuredFixedRateLoanTests
    {

        [TestMethod]
        public void TestGetRandomLoanWithHistory()
        {
            Pecuniam minOut;
            var testResult = SecuredFixedRateLoan.GetRandomLoanWithHistory(null, new Pecuniam(8200.94M),
                new Pecuniam(8200.94M + 3942.12M), 0.0557f, 5, out minOut);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.MinPaymentRate > 0);
            Assert.IsTrue(testResult.Rate > 0);
            Assert.AreNotEqual(SpStatus.NoHistory, testResult.CurrentStatus);
            Assert.AreNotEqual(Pecuniam.Zero, testResult.Value);
            Debug.WriteLine("MinPaymentRate     : {0}", testResult.MinPaymentRate);
            Debug.WriteLine("Rate               : {0}", testResult.Rate);
            Debug.WriteLine("CurrentStatus      : {0}", testResult.CurrentStatus);
            Debug.WriteLine("CurrentValue       : {0}", testResult.Value);

            foreach (var t in testResult.Balance.GetTransactionsBetween(null, null, true))
            {
                Debug.WriteLine(string.Join(" ", t.AtTime, t.Cash, t.Fee, t.Description));
            }
        }

        [TestMethod]
        public void TestSecuredFixedRateLoan()
        {
            var testResult = new SecuredFixedRateLoan(null, new DateTime(DateTime.Today.Year, 1, 1), 0.016667f, new Pecuniam(12143.06M));
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Balance);
            Assert.IsFalse(testResult.Balance.IsEmpty);
        }

        [TestMethod]
        public void TestGetRandomLoan()
        {
            Pecuniam minOut;
            var testResult = SecuredFixedRateLoan.GetRandomLoan(null, new Pecuniam(8200.94M),
                new Pecuniam(8200.94M + 3942.12M), 0.0557f, 5, out minOut);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.MinPaymentRate > 0);
            Assert.IsTrue(testResult.Rate > 0);
            Assert.AreNotEqual(SpStatus.NoHistory, testResult.CurrentStatus);
            Assert.AreNotEqual(Pecuniam.Zero, testResult.Value);
        }


        [TestMethod]
        public void TestGetCurrentPayoff_DelinqAlot()
        {
            var testLoan = new SecuredFixedRateLoan(null, new DateTime(2013, 10, 13), 0.016667f, new Pecuniam(1461.62M))
            {
                Rate = 0.08499f
            };
            testLoan.Push(Convert.ToDateTime("2013-11-20 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2013-12-25 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-01-28 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-02-21 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-03-23 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-04-20 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-05-23 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-06-24 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-07-24 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-08-27 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-09-26 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-10-23 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-11-26 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2014-12-21 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-01-19 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-02-25 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-03-22 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-04-21 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-05-18 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-06-19 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-07-28 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-08-27 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-09-22 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-10-22 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-11-27 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2015-12-24 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-01-28 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-02-25 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-03-26 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-04-27 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-05-26 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-06-28 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-07-20 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-08-27 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-09-22 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-10-23 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-11-18 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2016-12-20 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-01-20 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-02-28 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-03-25 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-04-18 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-05-24 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-06-21 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-07-25 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            testLoan.Push(Convert.ToDateTime("2017-08-20 00:00:00.0000"), new Pecuniam(37.26M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            var payoff = testLoan.GetValueAt(new DateTime(2017, 9, 21));
            testLoan.Push(new DateTime(2017, 9, 21), payoff, new Mereo("payoff"), Pecuniam.Zero);
            payoff = testLoan.GetValueAt(new DateTime(2017, 9, 22));

            Debug.WriteLine(payoff);
            Assert.AreEqual(Pecuniam.Zero, payoff);
        }


        [TestMethod]
        public void TestGetCurrentPayoff()
        {
            //intial 
            var startDate = new DateTime(2013, 8, 15);
            var testLoan = new SecuredFixedRateLoan(null, startDate, 0.016667f, new Pecuniam(3978.92M))
            {
                Rate = 0.07299f
            };
            for (var i = 0; i < 48; i++)
            {
                testLoan.Push(startDate.AddMonths(i + 1), new Pecuniam(95.52M), new Mereo("Vehicle Payment"), Pecuniam.Zero);
            }
            var lastPmtDate = new DateTime(2017, 9, 15);
            var payoff = testLoan.GetValueAt(lastPmtDate);
            testLoan.Push(lastPmtDate, payoff, new Mereo(null, "payoff"), Pecuniam.Zero);

            Assert.AreEqual(Pecuniam.Zero, testLoan.Value);

            var fv = testLoan.GetValueAt(new DateTime(2028, 12, 15));
            Debug.WriteLine(fv);
        }
    }
}

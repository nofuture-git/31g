using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class PecuniamTests
    {
        [TestMethod]
        public void TestOps()
        {
            var test00 = Pecuniam.Zero;
            var test01 = new Pecuniam(1.0M);
            var testResult = test00 + test01;
            Assert.AreEqual(1.0M, testResult.Amount);

            Assert.AreEqual(new Pecuniam(2.0M), new Pecuniam(1.0M) + new Pecuniam(1.0M));
            Assert.AreEqual(new Pecuniam(2.0M), new Pecuniam(4.0M) - new Pecuniam(2.0M));
            Assert.AreEqual(new Pecuniam(4.0M), new Pecuniam(4.0M) * new Pecuniam(1.0M));
            Assert.AreEqual(new Pecuniam(4.0M), new Pecuniam(4.0M) / new Pecuniam(1.0M));

            Assert.IsTrue(new Pecuniam(4.0M) > new Pecuniam(3.0M));
            Assert.IsTrue(new Pecuniam(1.0M) < new Pecuniam(3.0M));
            Assert.IsTrue(new Pecuniam(1.0M) == new Pecuniam(1.0M));
            Assert.IsTrue(new Pecuniam(2.0M) != new Pecuniam(1.0M));
        }

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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

        [TestMethod]
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
                testLoan.Push(startDate.AddMonths(i+1), new Pecuniam(95.52M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            }
            var lastPmtDate = new DateTime(2017, 9, 15);
            var payoff = testLoan.GetValueAt(lastPmtDate);
            testLoan.Push(lastPmtDate, payoff, Pecuniam.Zero, WealthBase.GetPaymentNote(null, "payoff"));
            
            Assert.AreEqual(Pecuniam.Zero, testLoan.CurrentValue);

            var fv = testLoan.GetValueAt(new DateTime(2028, 12, 15));
            Debug.WriteLine(fv);
        }

        [TestMethod]
        public void TestGetCurrentPayoff_DelinqAlot()
        {
            var testLoan = new SecuredFixedRateLoan(null, new DateTime(2013,10,13), 0.016667f, new Pecuniam(1461.62M))
            {
                Rate = 0.08499f
            };
            testLoan.Push(Convert.ToDateTime("2013-11-20 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2013-12-25 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-01-28 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-02-21 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-03-23 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-04-20 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-05-23 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-06-24 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-07-24 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-08-27 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-09-26 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-10-23 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-11-26 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2014-12-21 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-01-19 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-02-25 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-03-22 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-04-21 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-05-18 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-06-19 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-07-28 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-08-27 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-09-22 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-10-22 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-11-27 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2015-12-24 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-01-28 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-02-25 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-03-26 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-04-27 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-05-26 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-06-28 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-07-20 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-08-27 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-09-22 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-10-23 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-11-18 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2016-12-20 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-01-20 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-02-28 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-03-25 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-04-18 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-05-24 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-06-21 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-07-25 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            testLoan.Push(Convert.ToDateTime("2017-08-20 00:00:00.0000"), new Pecuniam(37.26M), Pecuniam.Zero, WealthBase.GetPaymentNote(null, "Vehicle Payment"));
            var payoff = testLoan.GetValueAt(new DateTime(2017, 9, 21));
            testLoan.Push(new DateTime(2017, 9, 21), payoff, Pecuniam.Zero, WealthBase.GetPaymentNote(null, "payoff"));
            payoff = testLoan.GetValueAt(new DateTime(2017, 9, 22));

            Debug.WriteLine(payoff);
            Assert.AreEqual(Pecuniam.Zero, payoff);
        }

        [TestMethod]
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
            System.Diagnostics.Debug.WriteLine(testResult.Amount);


        }

        [TestMethod]
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
    }
}

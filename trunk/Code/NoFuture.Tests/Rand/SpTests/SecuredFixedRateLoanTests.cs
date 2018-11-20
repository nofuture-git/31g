using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class SecuredFixedRateLoanTests
    {

        [Test]
        public void TestGetRandomLoanWithHistory()
        {
            var testResult = SecuredFixedRateLoan.RandomSecuredFixedRateLoanWithHistory(new Pecuniam(8200.94M),
                new Pecuniam(8200.94M + 3942.12M), 0.0557f);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.MinPaymentRate > 0);
            Assert.IsTrue(testResult.Rate > 0);
            Assert.IsNotNull(testResult.CurrentStatus);
            Assert.AreNotEqual(SpStatus.NoHistory, testResult.CurrentStatus);
            Assert.AreNotEqual(Pecuniam.Zero, testResult.Value);
            Console.WriteLine("Loan Amount        : {0}", testResult.OriginalBorrowAmount);
            Console.WriteLine("MinPaymentRate     : {0}", testResult.MinPaymentRate);
            Console.WriteLine("Rate               : {0}", testResult.Rate);
            Console.WriteLine("CurrentStatus      : {0}", testResult.CurrentStatus);
            Console.WriteLine("CurrentValue       : {0}", testResult.Value);

            foreach (var t in testResult.Balance.GetTransactionsBetween(null, null, true))
            {
                Console.WriteLine(string.Join(" ", t.AtTime, t.Cash, t.Description));
            }
        }

        [Test]
        public void TestSecuredFixedRateLoan()
        {
            var testResult = new SecuredFixedRateLoan(null, new DateTime(DateTime.Today.Year, 1, 1), new Pecuniam(12143.06M), 0.016667f, 5);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Balance);
            Assert.IsFalse(testResult.Balance.IsEmpty);
        }

        [Test]
        public void TestGetRandomLoan()
        {
            var testResult = SecuredFixedRateLoan.RandomSecuredFixedRateLoan(new Pecuniam(8200.94M),
                new Pecuniam(8200.94M + 3942.12M), 0.0557f, 5, null);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.MinPaymentRate > 0);
            Assert.IsTrue(testResult.Rate > 0);
            Assert.AreNotEqual(SpStatus.NoHistory, testResult.CurrentStatus);
            Assert.AreNotEqual(Pecuniam.Zero, testResult.Value);
            var calcMinPayment = testResult.GetMinPayment(testResult.Inception.AddDays(30));
            Console.WriteLine(calcMinPayment);
        }


        [Test]
        public void TestGetCurrentPayoff_DelinqAlot()
        {
            var testLoan = new SecuredFixedRateLoan(null, new DateTime(2013, 10, 13), 0.016667f, new Pecuniam(1461.62M))
            {
                Rate = 0.08499f
            };
            testLoan.AddNegativeValue(Convert.ToDateTime("2013-11-20 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2013-12-25 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-01-28 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-02-21 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-03-23 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-04-20 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-05-23 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-06-24 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-07-24 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-08-27 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-09-26 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-10-23 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-11-26 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2014-12-21 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-01-19 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-02-25 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-03-22 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-04-21 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-05-18 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-06-19 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-07-28 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-08-27 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-09-22 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-10-22 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-11-27 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2015-12-24 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-01-28 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-02-25 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-03-26 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-04-27 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-05-26 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-06-28 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-07-20 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-08-27 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-09-22 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-10-23 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-11-18 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2016-12-20 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-01-20 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-02-28 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-03-25 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-04-18 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-05-24 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-06-21 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-07-25 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            testLoan.AddNegativeValue(Convert.ToDateTime("2017-08-20 00:00:00.0000"), new Pecuniam(37.26M), new VocaBase("Vehicle Payment"));
            var payoff = testLoan.GetValueAt(new DateTime(2017, 9, 21));
            testLoan.AddNegativeValue(new DateTime(2017, 9, 21), payoff, new VocaBase("payoff"));
            payoff = testLoan.GetValueAt(new DateTime(2017, 9, 22));

            Console.WriteLine(payoff);
            Assert.AreEqual(Pecuniam.Zero, payoff);
        }


        [Test]
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
                testLoan.AddNegativeValue(startDate.AddMonths(i + 1), new Pecuniam(95.52M), new VocaBase("Vehicle Payment"));
            }
            var lastPmtDate = new DateTime(2017, 9, 15);
            var payoff = testLoan.GetValueAt(lastPmtDate);
            testLoan.AddNegativeValue(lastPmtDate, payoff, new VocaBase(null, "payoff"));

            Assert.AreEqual(Pecuniam.Zero, testLoan.Value);

            var fv = testLoan.GetValueAt(new DateTime(2028, 12, 15));
            Console.WriteLine(fv);
        }
    }
}

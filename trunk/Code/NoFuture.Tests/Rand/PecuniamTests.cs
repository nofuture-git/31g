using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class PecuniamTests
    {

        [TestMethod]
        public void TestTransactionsGetCurrent()
        {
            var testBalance = new NoFuture.Rand.Balance();
            //monthly payments
            
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-360), new Pecuniam(-450.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-30), new Pecuniam(-461.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-120), new Pecuniam(-458.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-150), new Pecuniam(-457.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-90), new Pecuniam(-459.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-240), new Pecuniam(-454.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-60), new Pecuniam(-460.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-300), new Pecuniam(-452.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-270), new Pecuniam(-453.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-180), new Pecuniam(-456.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-210), new Pecuniam(-455.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-330), new Pecuniam(-451.0M)));

            //charges
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-350), new Pecuniam(164.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-198), new Pecuniam(165.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-24), new Pecuniam(166.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-74), new Pecuniam(167.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-88), new Pecuniam(168.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-92), new Pecuniam(169.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-121), new Pecuniam(170.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-180), new Pecuniam(171.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-142), new Pecuniam(172.4M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-155), new Pecuniam(173.4M)));

            var testResult = testBalance.GetCurrent(DateTime.Now, 0.0875f);

            Assert.AreEqual(4723.45M, testResult.Amount);
            System.Diagnostics.Debug.WriteLine(testResult.Amount);

        }

        [TestMethod]
        public void TestGetCurrentNoInterest()
        {
            var testBalance = new NoFuture.Rand.Balance();
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-15), new Pecuniam(2000.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-12), new Pecuniam(-451.0M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-12), new Pecuniam(-101.91M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-12), new Pecuniam(-87.88M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-12), new Pecuniam(-32.47M)));
            testBalance.Transactions.Add(new Transaction(DateTime.Now.AddDays(-12), new Pecuniam(-16.88M)));

            var testResult = testBalance.GetCurrent(DateTime.Now, 0);
            Assert.AreEqual(1309.86M, testResult.Amount);
            System.Diagnostics.Debug.WriteLine(testResult.Amount);


        }

        [TestMethod]
        public void TestGetPaymentSum()
        {
            var testSubject = new Balance();
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-360), new Pecuniam(-450.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-30), new Pecuniam(-461.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-120), new Pecuniam(-458.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-150), new Pecuniam(-457.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-90), new Pecuniam(-459.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-240), new Pecuniam(-454.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-60), new Pecuniam(-460.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-300), new Pecuniam(-452.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-270), new Pecuniam(-453.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-180), new Pecuniam(-456.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-210), new Pecuniam(-455.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-330), new Pecuniam(-451.0M)));

            //charges
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-350), new Pecuniam(164.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-198), new Pecuniam(165.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-24), new Pecuniam(166.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-74), new Pecuniam(167.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-88), new Pecuniam(168.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-92), new Pecuniam(169.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-121), new Pecuniam(170.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-180), new Pecuniam(171.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-142), new Pecuniam(172.4M)));
            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-155), new Pecuniam(173.4M)));

            var testResult =
                testSubject.GetPaymentSum(new Tuple<DateTime, DateTime>(DateTime.Now.AddDays(-31).Date, DateTime.Now));

            Assert.AreEqual(-461.0M, testResult.Amount);

            testSubject.Transactions.Add(new Transaction(DateTime.Now.AddDays(-15), new Pecuniam(-120.0M)));

            testResult =
                testSubject.GetPaymentSum(new Tuple<DateTime, DateTime>(DateTime.Now.AddDays(-31).Date, DateTime.Now));

            Assert.AreEqual((-461.0M - 120.0M), testResult.Amount);
        }
    }
}

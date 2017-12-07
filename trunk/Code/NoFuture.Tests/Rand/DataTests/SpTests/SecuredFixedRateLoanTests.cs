using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;

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
            System.Diagnostics.Debug.WriteLine("MinPaymentRate     : {0}", testResult.MinPaymentRate);
            System.Diagnostics.Debug.WriteLine("Rate               : {0}", testResult.Rate);
            System.Diagnostics.Debug.WriteLine("CurrentStatus      : {0}", testResult.CurrentStatus);
            System.Diagnostics.Debug.WriteLine("CurrentValue       : {0}", testResult.Value);

            foreach (var t in testResult.Balance.GetTransactionsBetween(null, null, true))
            {
                System.Diagnostics.Debug.WriteLine(string.Join(" ", t.AtTime, t.Cash, t.Fee, t.Description));
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
    }
}

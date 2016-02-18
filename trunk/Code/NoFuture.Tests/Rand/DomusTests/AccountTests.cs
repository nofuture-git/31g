using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Rand.DomusTests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void TestGetRandomBankAccount()
        {
            var testResult = NoFuture.Rand.Domus.Sp.BankAccount.GetRandomBankAccount();

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.RoutingNumber);
            Assert.IsNotNull(testResult.AccountNumber);

            System.Diagnostics.Debug.WriteLine(testResult.RoutingNumber.Value);
            System.Diagnostics.Debug.WriteLine(testResult.AccountNumber.Value);

            
            testResult = NoFuture.Rand.Domus.Sp.BankAccount.GetRandomBankAccount();
            Assert.IsNotNull(testResult.RoutingNumber);
            Assert.IsNotNull(testResult.AccountNumber);
            Assert.IsNotNull(testResult.Bank);

            System.Diagnostics.Debug.WriteLine(testResult.RoutingNumber.Value);
            System.Diagnostics.Debug.WriteLine(testResult.AccountNumber.Value);
            System.Diagnostics.Debug.WriteLine(testResult.Bank.Name);
            
        }
    }
}

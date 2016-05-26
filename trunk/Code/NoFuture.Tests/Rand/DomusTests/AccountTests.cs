using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Tests.Rand.DomusTests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void TestGetRandomBankAccount()
        {
            var testResult = BankAccount.GetRandomBankAccount();

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.AccountNumber);

            System.Diagnostics.Debug.WriteLine(testResult.AccountNumber.Value);

            
            testResult = BankAccount.GetRandomBankAccount();
            Assert.IsNotNull(testResult.AccountNumber);
            Assert.IsNotNull(testResult.Bank);

            System.Diagnostics.Debug.WriteLine(testResult.AccountNumber.Value);
            System.Diagnostics.Debug.WriteLine(testResult.Bank.Name);
            
        }
    }
}

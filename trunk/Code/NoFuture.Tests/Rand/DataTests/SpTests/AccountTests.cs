using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void TestGetRandomBankAccount()
        {
            var p = new American(Etx.GetWorkingAdultBirthDate(), Gender.Female);
            var testResult = DepositAccount.GetRandomCheckingAcct(p);

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);

            
            testResult = DepositAccount.GetRandomCheckingAcct(p);
            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);
            
        }

        [TestMethod]
        public void TestIsPin()
        {
            var testSubject = new CheckingAccount(null, DateTime.Today.AddDays(-65),
                new Tuple<ICreditCard, string>(
                    CreditCard.GetRandomCreditCard(new American(Etx.GetWorkingAdultBirthDate(), Gender.Female)),
                    "8745"));

            Assert.IsTrue(testSubject.IsPin("8745"));

        }
    }
}

using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void TestGetRandomBankAccount()
        {
            var p = new American(Etx.RandomAdultBirthDate(), Gender.Female);
            var testResult = DepositAccount.RandomCheckingAccount(p);

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);

            
            testResult = DepositAccount.RandomCheckingAccount(p);
            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);
            
        }

        [Test]
        public void TestIsPin()
        {
            var testSubject = new CheckingAccount(null, DateTime.Today.AddDays(-65),
                new Tuple<ICreditCard, string>(
                    CreditCard.RandomCreditCard(new American(Etx.RandomAdultBirthDate(), Gender.Female)),
                    "8745"));

            Assert.IsTrue(testSubject.IsPin("8745"));

        }
    }
}

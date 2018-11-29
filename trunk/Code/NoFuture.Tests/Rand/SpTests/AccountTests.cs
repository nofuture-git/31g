using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Cc;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        public void TestGetRandomBankAccount()
        {
            var p = American.RandomAmerican(Etx.RandomAdultBirthDate(), Gender.Female);
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
            var testSubject = new CheckingAccount("ABC", DateTime.Today.AddDays(-65),
                new Tuple<ICreditCard, string>(
                    CreditCard.RandomCreditCard(American.RandomAmerican(Etx.RandomAdultBirthDate(), Gender.Female)),
                    "8745"));

            Assert.IsTrue(testSubject.IsPin("8745"));

        }

        [Test]
        public void TestAccount_Equation()
        {
            var dt = DateTime.UtcNow;

            //debits add cash, credits reduce cash
            var assets = new Account(new AccountId("assets"), dt.AddDays(-2), KindsOfAccounts.Asset, false);
            assets.Debit(dt.AddDays(-1), 10000m.ToPecuniam(), new VocaBase("first debit"));
            Console.WriteLine(assets.Value);
            Assert.AreEqual(10000m.ToPecuniam(), assets.Value);

            var liabilities = new Account(new AccountId("liabilities"),  dt.AddDays(-2), KindsOfAccounts.Asset, true);
            liabilities.Credit(dt.AddDays(-1), 10000M.ToPecuniam(), new VocaBase("first credit"));
            Console.WriteLine(liabilities.Value);
            Assert.AreEqual(10000m.ToPecuniam(), liabilities.Value);
        }
    }
}

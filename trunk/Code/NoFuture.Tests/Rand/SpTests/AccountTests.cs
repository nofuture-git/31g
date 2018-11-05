﻿using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Domus.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Cc;
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
    }
}

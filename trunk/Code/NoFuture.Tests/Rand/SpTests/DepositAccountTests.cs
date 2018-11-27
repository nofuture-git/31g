using System;
using System.Collections;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Cc;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class DepositAccountTests
    {
        [Test]
        public void TestTransferFundsInBankAccounts()
        {
            var savingAcct = DepositAccount.RandomSavingAccount();
            var checkingAccount = DepositAccount.RandomCheckingAccount();
            //initial deposit
            checkingAccount.AddPositiveValue(DateTime.Today.AddDays(-14), 1000.0D.ToPecuniam());

            savingAcct.AddPositiveValue(checkingAccount, 100.ToPecuniam(), DateTime.Today.AddDays(-1));

            var checkingAcctValue = checkingAccount.Value;
            var savingAcctValue = savingAcct.Value;
            Console.WriteLine(checkingAcctValue);
            Console.WriteLine(savingAcctValue);

            Assert.AreEqual(900D.ToPecuniam(), checkingAcctValue);
            Assert.AreEqual(100D.ToPecuniam(), savingAcctValue);
        }

        [Test]
        public void TestCtor()
        {
            //have some data from another external source
            var someData = new Hashtable
            {
                {"first-name", "Judy"},
                {"middle-initial", "B"},
                {"last-name", "Workin"},
                {"title", ""},
                {"suffix", ""},
                {"address", new Hashtable
                    {
                        {"type", "HOME"},
                        {"line-00", "1022 Chesterfield Dr."},
                        {"line-01", "Apt #252"},
                        {"city", "Cambridge"},
                        {"state", "MA"},
                        {"zip-code", "02494"}
                    }
                },
                {"home-phone", ""},
                {"mobile-phone", "7745541255"},
                {"bank-account", new Hashtable
                    {
                        {"routing-number", "024581253"},
                        {"account-number", "004451241"}
                    }
                }
            };

            var fullName = someData["first-name"] + " " + someData["last-name"];
            var someVisaCardNum = Sp.Cc.VisaCc.RandomVisaNumber();
            var someVisaCard = Sp.Cc.CreditCard.RandomCreditCard(someVisaCardNum.ToString(), fullName);
            var accountId = ((Hashtable)someData["bank-account"])["account-number"].ToString();

            var testResult =
                new CheckingAccount(DateTime.Today, new Tuple<ICreditCard, string>(someVisaCard, "8451"))
                {
                    Id = new AccountId(accountId)
                };

            Assert.IsNotNull(testResult.Id);
            Assert.AreNotEqual("",testResult.Id.Value);
            Assert.AreEqual(DateTime.Today, testResult.Inception);
            Assert.AreEqual(0.ToPecuniam(), testResult.Value);
            Assert.IsTrue(testResult.IsPin("8451"));

            Console.WriteLine(testResult);

        }
    }
}

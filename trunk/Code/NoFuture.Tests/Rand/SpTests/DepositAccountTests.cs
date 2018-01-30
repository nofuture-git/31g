using System;
using NoFuture.Rand.Sp;
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

            checkingAccount.AddPositiveValue(DateTime.Today.AddDays(-14), 1000.0D.ToPecuniam());

            DepositAccount.TransferFundsInBankAccounts(checkingAccount, savingAcct, 100D.ToPecuniam(), DateTime.Today.AddDays(-1));

            var checkingAcctValue = checkingAccount.Value;
            var savingAcctValue = savingAcct.Value;
            System.Diagnostics.Debug.WriteLine(checkingAcctValue);
            System.Diagnostics.Debug.WriteLine(savingAcctValue);

            Assert.AreEqual(900D.ToPecuniam(), checkingAcctValue);
            Assert.AreEqual(100D.ToPecuniam(), savingAcctValue);


        }
    }
}

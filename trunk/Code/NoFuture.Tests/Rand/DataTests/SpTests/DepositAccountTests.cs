using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class DepositAccountTests
    {
        [TestMethod]
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

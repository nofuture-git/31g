using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Types;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand.DomusTests
{
    [TestClass]
    public class AccountTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }
        [TestMethod]
        public void TestGetRandomBankAccount()
        {
            var p = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testResult = CheckingAccount.GetRandomCheckingAcct(p);

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);

            
            testResult = CheckingAccount.GetRandomCheckingAcct(p);
            Assert.IsNotNull(testResult.Id);
            Assert.IsNotNull(testResult.Bank);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);
            System.Diagnostics.Debug.WriteLine(testResult.Bank.Name);
            
        }

        [TestMethod]
        public void TestIsPin()
        {
            var testSubject = new CheckingAccount(null, DateTime.Today.AddDays(-65),
                new Tuple<ICreditCard, string>(
                    CreditCard.GetRandomCreditCard(new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female)),
                    "8745"));

            Assert.IsTrue(testSubject.IsPin("8745"));

        }
    }
}

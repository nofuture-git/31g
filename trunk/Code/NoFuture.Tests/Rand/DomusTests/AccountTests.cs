using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data;
using NoFuture.Rand.Data.Endo;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.DomusTests
{
    [TestClass]
    public class AccountTests
    {
        [TestMethod]
        public void TestGetRandomBankAccount()
        {
            var p = new NorthAmerican(UsState.GetWorkingAdultBirthDate(), Gender.Female);
            var testResult = AmericanUtil.GetRandomCheckingAcct(p);

            Assert.IsNotNull(testResult);

            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);

            
            testResult = AmericanUtil.GetRandomCheckingAcct(p);
            Assert.IsNotNull(testResult.Id);

            System.Diagnostics.Debug.WriteLine(testResult.Id.Value);
            
        }

        [TestMethod]
        public void TestIsPin()
        {
            var testSubject = new CheckingAccount(null, DateTime.Today.AddDays(-65),
                new Tuple<ICreditCard, string>(
                    AmericanUtil.GetRandomCreditCard(new NorthAmerican(UsState.GetWorkingAdultBirthDate(), Gender.Female)),
                    "8745"));

            Assert.IsTrue(testSubject.IsPin("8745"));

        }
    }
}

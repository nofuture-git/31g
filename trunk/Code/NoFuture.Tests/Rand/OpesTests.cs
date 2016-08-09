using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class OpesTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }
        [TestMethod]
        public void TestGetFactor()
        {
            var testResult = NorthAmericanWealth.GetFactor(NorthAmericanWealth.FactorTables.HomeDebt,
                (OccidentalEdu.Bachelor | OccidentalEdu.Grad), NorthAmericanRace.Asian, AmericanRegion.West, 38,
                Gender.Male, MaritialStatus.Single);
            Assert.AreNotEqual(0.0D, testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetFactorBaseValue()
        {
            var testResult = NorthAmericanWealth.GetFactorBaseValue(NorthAmericanWealth.FactorTables.VehicleDebt);
            Assert.AreEqual(10000.0D, testResult);

            testResult = NorthAmericanWealth.GetFactorBaseValue(NorthAmericanWealth.FactorTables.HomeEquity);
            Assert.AreEqual(80000.0D, testResult);
        }

        [TestMethod]
        public void TestGetRandomRent()
        {
            var amer = Person.American();
            var testSubject = new NorthAmericanWealth(amer);
            
            testSubject.GetRandomRent();
            var rent = testSubject.HomeDebt.FirstOrDefault() as Rent;
            Assert.IsNotNull(rent);
        }

        [TestMethod]
        public void TestGetRandomHomeLoan()
        {
            var amer = Person.American();
            var testSubject = new NorthAmericanWealth(amer);

            testSubject.GetRandomHomeLoan();

            var homeLoan = testSubject.HomeDebt.FirstOrDefault() as FixedRateLoan;
            Assert.IsNotNull(homeLoan);

            System.Diagnostics.Debug.WriteLine(homeLoan.TradeLine.Balance.ToString());
        }

        [TestMethod]
        public void TestGetRandomCcDebt()
        {
            var amer = Person.American();
            var testSubject = new NorthAmericanWealth(amer);

            testSubject.GetRandomCcDebt();

            Assert.IsTrue(testSubject.CreditCardDebt.Any());

            var testResult = testSubject.CreditCardDebt.First() as CreditCardAccount;
            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(testResult.Cc.CardHolderSince);
            System.Diagnostics.Debug.WriteLine(testResult.Max);

            System.Diagnostics.Debug.WriteLine(testResult.TradeLine.Balance.ToString());

        }

        [TestMethod]
        public void TestGetPaycheck()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new NorthAmericanWealth(amer);
            System.Diagnostics.Debug.WriteLine(string.Join(" ", amer.Age, amer.MaritialStatus, amer.Education, amer.Race));

            var testResult = testSubject.GetPaycheck(null);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Amount > 0.0M);
        }
    }
}

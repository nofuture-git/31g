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

            foreach (var t in rent.TradeLine.Balance.Transactions)
            {
                System.Diagnostics.Debug.WriteLine(t);
            }
        }

        [TestMethod]
        public void TestGetRandomHomeLoan()
        {
            var amer = Person.American();
            var testSubject = new NorthAmericanWealth(amer);

            testSubject.GetRandomHomeLoan();

            var homeLoan = testSubject.HomeDebt.FirstOrDefault() as FixedRateLoan;
            Assert.IsNotNull(homeLoan);

            foreach (var t in homeLoan.TradeLine.Balance.Transactions)
            {
                System.Diagnostics.Debug.WriteLine(t);
            }
        }

        [TestMethod]
        public void TestGetRandomCcDebt()
        {
            var amer = Person.American();
            var testSubject = new NorthAmericanWealth(amer);

            testSubject.GetRandomCcDebt();

            Assert.IsTrue(testSubject.CreditCardDebt.Any());

            var testResult = testSubject.CreditCardDebt.First() as CreditCard;
            Assert.IsNotNull(testResult);

            System.Diagnostics.Debug.WriteLine(testResult.CardHolderSince);
            System.Diagnostics.Debug.WriteLine(testResult.Max);

            foreach (var t in testResult.TradeLine.Balance.Transactions)
            {
                System.Diagnostics.Debug.WriteLine(t);
            }
        }

    }
}

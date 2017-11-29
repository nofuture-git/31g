﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Enums;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Cc;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests
{
    [TestClass]
    public class WealthBaseTests
    {

        [TestMethod]
        public void TestGetFactor()
        {
            var testResult = NorthAmericanFactors.GetFactor(FactorTables.HomeDebt,
                (OccidentalEdu.Bachelor | OccidentalEdu.Grad), NorthAmericanRace.Asian, AmericanRegion.West, 38,
                Gender.Male, MaritialStatus.Single);
            Assert.AreNotEqual(0.0D, testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetFactorBaseValue()
        {
            var testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.VehicleDebt);
            Assert.AreEqual(10000.0D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.HomeEquity);
            Assert.AreEqual(80000.0D, testResult);
        }

        [TestMethod]
        public void TestGetRandomRent()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new NorthAmericanWealth(amer, true);
            testSubject.AddRent();
            var rent = testSubject.HomeDebt.FirstOrDefault() as Rent;
            Assert.IsNotNull(rent);

            System.Diagnostics.Debug.WriteLine(rent.TradeLine.Balance.ToString());
        }

        [TestMethod]
        public void TestGetRandomHomeLoan()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new NorthAmericanWealth(amer);
            testSubject.AddMortgage();

            var homeLoan = testSubject.HomeDebt.FirstOrDefault() as FixedRateLoan;
            Assert.IsNotNull(homeLoan);

            System.Diagnostics.Debug.WriteLine(homeLoan.TradeLine.Balance.ToString());
        }

        [TestMethod]
        public void TestGetRandomCcDebt()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new NorthAmericanWealth(amer);
            testSubject.AddSingleCcDebt();

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

            var testResult = testSubject.GetYearlyIncome(null,1.0);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Amount > 0.0M);
        }

        [TestMethod]
        public void TestGetRandomVehicle()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new NorthAmericanWealth(amer);
            System.Diagnostics.Debug.WriteLine(string.Join(" ", amer.Age, amer.MaritialStatus, amer.Education, amer.Race));

            var testResult = testSubject.AddVehicleLoan();
            Assert.IsNotNull(testResult);

            if (testSubject.VehicleDebt.Any())
            {
                
                var testResultSfrl = testSubject.VehicleDebt.First() as NoFuture.Rand.Data.Sp.SecuredFixedRateLoan;

                Assert.IsNotNull(testResultSfrl);

                System.Diagnostics.Debug.WriteLine(testResultSfrl.Description);

                Assert.IsNotNull( testResultSfrl.PropertyId);
            }
        }

        [TestMethod]
        public void TestCreateRandomAmericanOpes()
        {
            var amer = new NorthAmerican(NAmerUtil.GetWorkingAdultBirthDate(), Gender.Female);
            var testSubject = new NorthAmericanWealth(amer);
            System.Diagnostics.Debug.WriteLine(string.Join(" ", amer.Age, amer.MaritialStatus, amer.Education, amer.Race));

            testSubject.CreateRandomAmericanOpes();

            Assert.IsTrue(testSubject.HomeDebt.Any());

            Assert.IsTrue(testSubject.CheckingAccounts.Any());
            Assert.IsTrue(testSubject.SavingAccounts.Any());

            System.Diagnostics.Debug.WriteLine(testSubject.FinancialData.ToString());
        }

        [TestMethod]
        public void TestGetXmlEduName()
        {
            var testResult = NorthAmericanFactors.GetXmlEduName(OccidentalEdu.Bachelor);
            Assert.AreEqual("Associate",testResult);
            testResult = NorthAmericanFactors.GetXmlEduName(OccidentalEdu.Bachelor | OccidentalEdu.Grad);
            Assert.AreEqual("Bachelor", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestSecuredFixedRateLoan()
        {
            var testResult = new SecuredFixedRateLoan(null, new DateTime(DateTime.Today.Year,1,1), 0.016667f, new Pecuniam(12143.06M));
            Assert.IsNotNull(testResult.TradeLine);
            Assert.IsNotNull(testResult.TradeLine.Balance);
            Assert.IsFalse(testResult.TradeLine.Balance.IsEmpty);
        }

        [TestMethod]
        public void TestGetRandomLoanWithHistory()
        {
            Pecuniam minOut;
            var testResult = SecuredFixedRateLoan.GetRandomLoanWithHistory(null, new Pecuniam(8200.94M),
                new Pecuniam(8200.94M + 3942.12M), 0.0557f, 5, out minOut);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.MinPaymentRate > 0);
            Assert.IsTrue(testResult.Rate > 0);
            Assert.IsNotNull(testResult.TradeLine);
            Assert.AreNotEqual(SpStatus.NoHistory, testResult.CurrentStatus);
            Assert.AreNotEqual(Pecuniam.Zero, testResult.Value);
            System.Diagnostics.Debug.WriteLine("MinPaymentRate     : {0}", testResult.MinPaymentRate);
            System.Diagnostics.Debug.WriteLine("Rate               : {0}", testResult.Rate);
            System.Diagnostics.Debug.WriteLine("TradeLine          : {0}", testResult.TradeLine);
            System.Diagnostics.Debug.WriteLine("CurrentStatus      : {0}", testResult.CurrentStatus);
            System.Diagnostics.Debug.WriteLine("CurrentValue       : {0}", testResult.Value);

            foreach (var t in testResult.TradeLine.Balance.GetTransactionsBetween(null, null, true))
            {
                System.Diagnostics.Debug.WriteLine(string.Join(" ", t.AtTime, t.Cash, t.Fee, t.Description));
            }
        }

        [TestMethod]
        public void TestGetIncomeItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetIncomeItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach(var i in testResult)
                System.Diagnostics.Debug.WriteLine($"{i.Name} {i.GetName(KindsOfNames.Group)}");
        }

        [TestMethod]
        public void TestGetDeductionItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetDeductionItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestGetExpenseItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetExpenseItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }

        [TestMethod]
        public void TestGetAssetItemNames()
        {
            var testResult = NoFuture.Rand.Domus.Opes.WealthBase.GetAssetItemNames();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var i in testResult)
                System.Diagnostics.Debug.WriteLine(i);
        }
    }
}
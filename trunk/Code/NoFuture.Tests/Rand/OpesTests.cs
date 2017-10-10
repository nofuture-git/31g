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
            var testResult = NorthAmericanWealth.GetXmlEduName(OccidentalEdu.Bachelor);
            Assert.AreEqual("Associate",testResult);
            testResult = NorthAmericanWealth.GetXmlEduName(OccidentalEdu.Bachelor | OccidentalEdu.Grad);
            Assert.AreEqual("Bachelor", testResult);
            System.Diagnostics.Debug.WriteLine(testResult.ToString());
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
            var testResult = SecuredFixedRateLoan.GetRandomLoanWithHistory(null, null, new Pecuniam(8200.94M),
                new Pecuniam(8200.94M + 3942.12M), 0.0557f, 5, out minOut);
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.MinPaymentRate > 0);
            Assert.IsTrue(testResult.Rate > 0);
            Assert.IsNotNull(testResult.TradeLine);
            Assert.AreNotEqual(SpStatus.NoHistory, testResult.CurrentStatus);
            Assert.AreNotEqual(Pecuniam.Zero, testResult.CurrentValue);
            System.Diagnostics.Debug.WriteLine("MinPaymentRate     : {0}", testResult.MinPaymentRate);
            System.Diagnostics.Debug.WriteLine("Rate               : {0}", testResult.Rate);
            System.Diagnostics.Debug.WriteLine("TradeLine          : {0}", testResult.TradeLine);
            System.Diagnostics.Debug.WriteLine("CurrentStatus      : {0}", testResult.CurrentStatus);
            System.Diagnostics.Debug.WriteLine("CurrentValue       : {0}", testResult.CurrentValue);

            foreach (var t in testResult.TradeLine.Balance.GetTransactionsBetween(null, null, true))
            {
                System.Diagnostics.Debug.WriteLine(string.Join(" ", t.AtTime, t.Cash, t.Fee, t.Description));
            }
        }
    }
}

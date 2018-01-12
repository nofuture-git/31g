using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus.Opes;
using NoFuture.Rand.Domus.Opes.US;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class AmericanFactorsTests
    {
        [TestMethod]
        public void TestGetRandomFactorValue()
        {
            var stdDev = 0.2685D;
            var testResult = AmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount, 1.0D, stdDev);
            var isInRange = testResult >= 1000D - (1000 * stdDev * 3) && testResult <= 1000D + (1000 * stdDev * 3);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(isInRange);

            stdDev = 0.0485D;
            testResult = AmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity, 1.0D, stdDev);
            isInRange = testResult >= 81000D - (81000 * stdDev * 3) && testResult <= 81000D + (81000 * stdDev * 3);
            Assert.IsTrue(isInRange);
        }

        [TestMethod]
        public void TestGetFactorBaseValue()
        {
            var testResult = AmericanFactors.GetFactorBaseValue(FactorTables.CheckingAccount);
            Assert.AreEqual(1000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.SavingsAccount);
            Assert.AreEqual(4625D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.NetWorth);
            Assert.AreEqual(25116D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.VehicleEquity);
            Assert.AreEqual(6988D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.HomeEquity);
            Assert.AreEqual(81000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.VehicleDebt);
            Assert.AreEqual(10000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.HomeDebt);
            Assert.AreEqual(117000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.CreditCardDebt);
            Assert.AreEqual(3500D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(FactorTables.OtherDebt);
            Assert.AreEqual(10000D, testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestGetFactor()
        {
            var testFactor = FactorTables.HomeEquity;
            var edu = OccidentalEdu.Grad | OccidentalEdu.Bachelor; //1.16049
            var race = NorthAmericanRace.Hispanic; //0.617280
            var region = AmericanRegion.Midwest; //0.76515
            var maritalStatus = MaritialStatus.Married;
            var age = 52;//0.86420

            var testResult = AmericanFactors.GetFactor(testFactor, edu, race, region, age, Gender.Male,
                maritalStatus);

            var expectedResult = Math.Round((1.16049D + 0.61728D + 0.76515D + 0.8642D)/4, 5);

            Assert.AreEqual(expectedResult, testResult);
            System.Diagnostics.Debug.WriteLine(testResult);

        }
    }
}

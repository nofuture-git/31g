using System;
using NoFuture.Rand.Gov;
using NoFuture.Rand.Gov.US;
using NoFuture.Rand.Opes.US;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
{
    [TestFixture]
    public class AmericanFactorsTests
    {
        [Test]
        public void TestGetRandomFactorValue()
        {
            var stdDev = 0.2685D;
            var testResult = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.CheckingAccount, 1.0D, stdDev);
            var isInRange = testResult >= 1000D - (1000 * stdDev * 3) && testResult <= 1000D + (1000 * stdDev * 3);
            Console.WriteLine(testResult);
            Assert.IsTrue(isInRange);

            stdDev = 0.0485D;
            testResult = AmericanFactors.GetRandomFactorValue(AmericanFactorTables.HomeEquity, 1.0D, stdDev);
            isInRange = testResult >= 81000D - (81000 * stdDev * 3) && testResult <= 81000D + (81000 * stdDev * 3);
            Console.WriteLine(testResult);
            Assert.IsTrue(isInRange);
        }

        [Test]
        public void TestGetFactorBaseValue()
        {
            var testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.CheckingAccount);
            Assert.AreEqual(1000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.SavingsAccount);
            Assert.AreEqual(4625D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.NetWorth);
            Assert.AreEqual(25116D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.VehicleEquity);
            Assert.AreEqual(6988D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.HomeEquity);
            Assert.AreEqual(81000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.VehicleDebt);
            Assert.AreEqual(10000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.HomeDebt);
            Assert.AreEqual(117000D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.CreditCardDebt);
            Assert.AreEqual(3500D, testResult);

            testResult = AmericanFactors.GetFactorBaseValue(AmericanFactorTables.OtherDebt);
            Assert.AreEqual(10000D, testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [Test]
        public void TestGetFactor()
        {
            var testFactor = AmericanFactorTables.HomeEquity;
            var edu = OccidentalEdu.Grad | OccidentalEdu.Bachelor; //1.16049
            var race = NorthAmericanRace.Hispanic; //0.617280
            var region = AmericanRegion.Midwest; //0.76515
            var maritalStatus = MaritalStatus.Married;
            var age = 52;//0.86420

            var testResult = AmericanFactors.GetFactor(testFactor, edu, race, region, age, Gender.Male,
                maritalStatus);

            var expectedResult = Math.Round((1.16049D + 0.61728D + 0.76515D + 0.8642D)/4, 5);

            Assert.AreEqual(expectedResult, testResult);
            System.Diagnostics.Debug.WriteLine(testResult);

        }
    }
}

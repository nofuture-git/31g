using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanFactorsTests
    {
        [TestMethod]
        public void TestGetRandomFactorValue()
        {
            var stdDev = 0.2685D;
            var testResult = NorthAmericanFactors.GetRandomFactorValue(FactorTables.CheckingAccount, 1.0D, stdDev);
            var isInRange = testResult >= 600D - (600 * stdDev * 3) && testResult <= 600D + (600 * stdDev * 3);
            Assert.IsTrue(isInRange);

            stdDev = 0.0485D;
            testResult = NorthAmericanFactors.GetRandomFactorValue(FactorTables.HomeEquity, 1.0D, stdDev);
            isInRange = testResult >= 80000D - (80000 * stdDev * 3) && testResult <= 80000D + (80000 * stdDev * 3);
            Assert.IsTrue(isInRange);
        }

        [TestMethod]
        public void TestGetFactorBaseValue()
        {
            var testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.CheckingAccount);
            Assert.AreEqual(600D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.SavingsAccount);
            Assert.AreEqual(2450D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.NetWorth);
            Assert.AreEqual(68828D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.VehicleEquity);
            Assert.AreEqual(6824D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.HomeEquity);
            Assert.AreEqual(80000D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.VehicleDebt);
            Assert.AreEqual(10000D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.HomeDebt);
            Assert.AreEqual(117000D, testResult);

            testResult = NorthAmericanFactors.GetFactorBaseValue(FactorTables.CreditCardDebt);
            Assert.AreEqual(3500D, testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}

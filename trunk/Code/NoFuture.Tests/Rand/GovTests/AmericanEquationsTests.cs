using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Gov;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestClass]
    public class AmericanEquationsTests
    {
        [TestMethod]
        public void TestHealthInsuranceCostPerPerson()
        {
            var testSubject = AmericanEquations.HealthInsuranceCostPerPerson;
            var testResult = testSubject.SolveForY(2015);
            Assert.AreEqual(10084.6675, testResult);
            System.Diagnostics.Debug.WriteLine(testSubject);
        }

        [TestMethod]
        public void TestFederalIncomeTaxRate()
        {
            var testSubject = AmericanEquations.FederalIncomeTaxRate;
            var testResult = testSubject.SolveForY(115725D);
            Assert.AreEqual(0.27075D, testResult);
            System.Diagnostics.Debug.WriteLine(testSubject);

        }
    }
}

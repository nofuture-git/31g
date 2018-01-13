using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Gov.US;
using NoFuture.Util.Core;

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
        [TestMethod]
        public void TestEquations()
        {
            var baseDob = new DateTime(2016, 2, 16, 15, 55, 02);
            for (var i = 0; i < 15; i++)
            {
                var dob = baseDob.AddYears((25 + i) * -1);
                var marriageEq = AmericanEquations.FemaleAge2FirstMarriage.SolveForY(dob.ToDouble());
                var firstBornEq = AmericanEquations.FemaleAge2FirstChild.SolveForY(dob.Year);

                Assert.IsTrue(marriageEq > 23 && marriageEq <= 28);
                Assert.IsTrue(firstBornEq > 25 && firstBornEq < 30);
                Debug.WriteLine($"dob: {dob};  marriageAge: {marriageEq}; firstBornAge: {firstBornEq};");

            }
        }
    }
}

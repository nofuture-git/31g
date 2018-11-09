using System;
using System.Diagnostics;
using NUnit.Framework;
using NoFuture.Rand.Gov.US;
using NoFuture.Util.Core;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestFixture]
    public class AmericanEquationsTests
    {
        [Test]
        public void TestHealthInsuranceCostPerPerson()
        {
            var testSubject = AmericanEquations.HealthInsuranceCostPerPerson;
            var testResult = testSubject.SolveForY(2015);
            Assert.AreEqual(10084.6675, testResult);
            Console.WriteLine(testSubject);
        }

        [Test]
        public void TestFederalIncomeTaxRate()
        {
            var testSubject = AmericanEquations.FederalIncomeTaxRate;
            var testResult = testSubject.SolveForY(115725D);
            Assert.AreEqual(0.27075D, testResult);
            Console.WriteLine(testSubject);

        }
        [Test]
        public void TestEquations()
        {
            var baseDob = new DateTime(2016, 2, 16, 15, 55, 02);
            for (var i = 0; i < 15; i++)
            {
                var dob = baseDob.AddYears((25 + i) * -1);
                var marriageEq = AmericanEquations.FemaleAge2FirstMarriage.SolveForY(dob.ToDouble());
                var firstBornEq = AmericanEquations.FemaleAge2FirstChild.SolveForY(dob.Year);
                Debug.WriteLine($"dob: {dob};  marriageAge: {marriageEq}; firstBornAge: {firstBornEq};");

                Assert.IsTrue(marriageEq > 18 && marriageEq <= 38);
                Assert.IsTrue(firstBornEq > 18 && firstBornEq < 38);

            }
        }

        [Test]
        public void TestGetChildSupportMonthlyCostEquation()
        {
            for (var i = 1; i <= 5; i++)
            {
                var prev = 0.0D;
                for (var j = 2000; j < 55000; j += 500)
                {
                    var t = AmericanEquations.GetChildSupportMonthlyCostEquation(i);
                    var current = t.SolveForY(j);
                    //assert it never goes down as income goes up
                    if(current < prev)
                        Console.WriteLine($"i {i}, j {j}, current {current}, prev {prev}");
                    Assert.IsTrue(current >= prev);
                    prev = current;
                }
            }
        }
    }
}

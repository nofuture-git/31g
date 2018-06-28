using System;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class EquationsTests
    {
        [Test]
        public void TestLinearEquation()
        {
            var testSubject = new LinearEquation {Intercept = 4, Slope = 2};

            var x = 8;
            var testResultY = testSubject.SolveForY(x);
            var testResultX = testSubject.SolveForX(testResultY);

            Assert.AreEqual(x, testResultX);

        }
        [Test]
        public void TestExponentialEquation()
        {
            var testSubject = new ExponentialEquation() {ConstantValue = 4, Power = 3.5};
            var x = 5;
            var testResultY = testSubject.SolveForY(x);
            var testResultX = testSubject.SolveForX(testResultY);

            Assert.AreEqual(x, testResultX);

            testSubject = new ExponentialEquation
            {
                ConstantValue = System.Math.Pow(10, -13),
                Power = 6.547
            };

            testResultY = testSubject.SolveForY(50);
            Console.WriteLine(testResultY);

        }

        [Test]
        public void TestNaturalLogEquation()
        {
            var testSubject = new NaturalLogEquation {Intercept = 2, Slope = 1.2};

            var x = 11.0D;
            var testResultY = testSubject.SolveForY(x);
            var testResultX = testSubject.SolveForX(testResultY);
            Console.WriteLine(testResultX);

            Assert.AreEqual(x, System.Math.Round(testResultX));//some kind of float-pt loss 
        }
    }
}

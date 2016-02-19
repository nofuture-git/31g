using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class EquationsTests
    {
        [TestMethod]
        public void TestLinearEquation()
        {
            var testSubject = new NoFuture.Util.Math.LinearEquation {Intercept = 4, Slope = 2};

            var x = 8;
            var testResultY = testSubject.SolveForY(x);
            var testResultX = testSubject.SolveForX(testResultY);

            Assert.AreEqual(x, testResultX);

        }
        [TestMethod]
        public void TestExponentialEquation()
        {
            var testSubject = new NoFuture.Util.Math.ExponentialEquation() {ConstantValue = 4, Power = 3.5};
            var x = 5;
            var testResultY = testSubject.SolveForY(x);
            var testResultX = testSubject.SolveForX(testResultY);

            Assert.AreEqual(x, testResultX);

            testSubject = new NoFuture.Util.Math.ExponentialEquation
            {
                ConstantValue = System.Math.Pow(10, -13),
                Power = 6.547
            };

            testResultY = testSubject.SolveForY(50);
            System.Diagnostics.Debug.WriteLine(testResultY);

        }

        [TestMethod]
        public void TestNaturalLogEquation()
        {
            var testSubject = new NoFuture.Util.Math.NaturalLogEquation {Intercept = 2, Slope = 1.2};

            var x = 11.0D;
            var testResultY = testSubject.SolveForY(x);
            var testResultX = testSubject.SolveForX(testResultY);
            System.Diagnostics.Debug.WriteLine(testResultX);

            Assert.AreEqual(x, Math.Round(testResultX));//some kind of float-pt loss 
        }
    }
}

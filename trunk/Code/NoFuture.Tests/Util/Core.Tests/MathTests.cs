using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NoFuture.Shared.Core;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class MathTests
    {
        [Test]
        public void TestRomanNumberConversion()
        {
            var testSubject = "XLIX";
            var testResult = testSubject.ParseRomanNumeral();
            Assert.AreEqual(49,testResult);
        }

        [Test]
        public void TestLinearEquation()
        {
            //y = 0.1056x - 181.45
            var myEq = new LinearEquation {Intercept = -181.45, Slope = 0.1056};
            var dob = 1974.477451;
            var x = myEq.SolveForY(dob);
            Console.WriteLine(x);
            Assert.IsTrue(x > 27 && x < 28);

            var y = myEq.SolveForX(27.0548188256);
            Console.WriteLine(y);
            Assert.AreEqual(dob, y);

        }

        [Test]
        public void TestPerDiemInterest()
        {
            var testResult = 1541M.PerDiemInterest(0.13f, 30);
            Assert.AreEqual(1557.54M,testResult);

            testResult = 36000M.PerDiemInterest(0.035f, Constants.TropicalYear.TotalDays*5);
            Assert.AreEqual(42884.5M, testResult);

            testResult = 36000M.PerDiemInterest(0f, Constants.TropicalYear.TotalDays*5);
            Assert.AreEqual(36000M, testResult);

            testResult = 48000M.PerDiemInterest(-0.15f, Constants.TropicalYear.TotalDays*20);

            Console.WriteLine(testResult);

            testResult = 900M.PerDiemInterest(0.055F, 150);

            Console.WriteLine(testResult);
        }

        [Test]
        public void TestLnEq()
        {
            var testSubject = new NaturalLogEquation { Slope = 7.123, Intercept = -55.44 };
            const double X_IN = 1985;
            var solveY = testSubject.SolveForY(X_IN);

            var solveX = testSubject.SolveForX(solveY);
            Assert.AreEqual(System.Math.Round(X_IN), System.Math.Round(solveX));
        }

        [Test]
        public void TestNormalDist()
        {
            var testSubject = new NormalDistEquation() {Mean = 0, StdDev = 1};
            Console.WriteLine(string.Format("{0}\t{1}","x","f(x)"));
            for (var i = 0; i <= 30; i++)
            {
                for (var j = 0; j < 10; j++)
                {
                    var z = i*0.1 + j*0.01;
                    var testResult = testSubject.SolveForY(z);
                    //var testResult = (z - testSubject.Mean)/testSubject.StdDev;
                    //System.Diagnostics.Debug.Write(string.Format(" {0} ", z));
                    Console.WriteLine(string.Format("{0}\t{1}", z, testResult));
                }
                
            }
        }

        [Test]
        public void TestNormalDistGetZScore()
        {
            var testSubject = new NormalDistEquation { Mean = 0, StdDev = 1 };

            var testResult = testSubject.GetZScoreFor(0);
            Assert.IsTrue(testResult >= 0.499);

            testResult = testSubject.GetZScoreFor(1);
            Assert.IsTrue(testResult >= 0.34134);
        }
    }
}




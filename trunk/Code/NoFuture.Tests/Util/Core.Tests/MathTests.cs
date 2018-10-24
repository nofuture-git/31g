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
        public void TestMean()
        {
            var testInput = new double[]
            {
                0.944179428249681, 0.311241151444261, 0.0320733543634756, 0.484716168830505, 0.708686689710564,
                0.613101829594514, 0.357609613965084, 0.80961309317947, 0.317703485171173, 0.645854210316136,
                0.560796459932251, 0.0840445571039079, 0.251447376446541, 0.280716611668801, 0.751935856301308,
                0.617214414578497, 0.347298655355023, 0.3049458345887, 0.832622998781792, 0.767974667608726,
                0.802398868744447, 0.209049347885442, 0.707323246033547, 0.530966512174796, 0.72795933379231,
                0.628821779800962, 0.904752576679342, 0.05582945843033, 0.352612592444109, 0.644451619425999,
                0.11124615888635, 0.112555048015227
            };

            var testResult = testInput.Mean();
            Assert.IsTrue(System.Math.Abs(0.494116969 - testResult) < 0.00000001);
        }

        [Test]
        public void TestStdDev()
        {
            var testInput = new double[]
            {
                0.944179428249681, 0.311241151444261, 0.0320733543634756, 0.484716168830505, 0.708686689710564,
                0.613101829594514, 0.357609613965084, 0.80961309317947, 0.317703485171173, 0.645854210316136,
                0.560796459932251, 0.0840445571039079, 0.251447376446541, 0.280716611668801, 0.751935856301308,
                0.617214414578497, 0.347298655355023, 0.3049458345887, 0.832622998781792, 0.767974667608726,
                0.802398868744447, 0.209049347885442, 0.707323246033547, 0.530966512174796, 0.72795933379231,
                0.628821779800962, 0.904752576679342, 0.05582945843033, 0.352612592444109, 0.644451619425999,
                0.11124615888635, 0.112555048015227
            };

            var testResult = testInput.StdDev();
            Console.WriteLine(testResult);
            Assert.IsTrue(System.Math.Abs(0.27114666 - testResult) < 0.00000001);
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




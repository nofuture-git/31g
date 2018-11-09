using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Rand.Core;
using NoFuture.Util.Core;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.CoreTests
{
    [TestFixture]
    public class RLinearEquationTests
    {
        [Test]
        public void TestSolveForY()
        {
            var testStdDev = 2.5D;
            var testSubject = new RLinearEquation(0.1056, -181.45) {StdDev = testStdDev };
            var testCtrl = new LinearEquation(0.1056, -181.45);
            for (var i = 16; i < 64; i++)
            {
                var x = DateTime.Today.AddYears(-1 * i).ToDouble();
                var testAvg = testCtrl.SolveForY(x);
                var testResult = testSubject.SolveForY(DateTime.Today.AddYears(-1 * i).ToDouble());

                var expectedFarLeft = (testAvg - testStdDev * 3.25);
                var expectedFarRight = (testAvg + testStdDev * 3.25);

                var tr = expectedFarLeft <= testResult && testResult <= expectedFarRight;
                if(!tr)
                    Console.WriteLine($"{expectedFarLeft} <= {testResult} <= {expectedFarRight}" );
                Assert.IsTrue(tr);

            }
        }

        [Test]
        public void TestSolveForY_WithRange()
        {
            var testStdDev = 2.5D;
            var testSubject = new RLinearEquation(0.1056, -181.45) { StdDev = testStdDev, MinX = 1914D, MaxX = 2055D};
            var testCtrl = new LinearEquation(0.1056, -181.45);

            var testResult = testSubject.SolveForY(800);
            //the true equation will produce nonsense results for the distant past
            var testUnconstrained = testCtrl.SolveForY(800);
            Console.WriteLine($"{testUnconstrained} < {testResult}");
            Assert.IsTrue(testResult > testUnconstrained);

            //likewise for distant future
            testResult = testSubject.SolveForY(3030);
            testUnconstrained = testCtrl.SolveForY(3030);
            Console.WriteLine($"{testUnconstrained} > {testResult}");
            Assert.IsTrue(testResult < testUnconstrained);
        }
    }
}

using System;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class TestLinearEquation
    {
        [Test]
        public void TestNfVectorEquals()
        {
            var testSubject1 = new LinearEquation(2.25, 5.375);
            var testSubject2 = new LinearEquation(2.25, 5.375);
            Assert.IsTrue(testSubject1.Equals(testSubject2));

            Assert.IsTrue(testSubject1 == testSubject2);

            Assert.IsFalse(testSubject1 != testSubject2);
        }

        [Test]
        public void TestNfVectorClone()
        {
            var testSubject = new LinearEquation(2.25, 5.375);
            var testResult = testSubject.Clone() as LinearEquation;
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testSubject[0], testResult[0]);
            Assert.AreEqual(testSubject[1], testResult[1]);
        }

        [Test]
        public void TestEuclideanNorm()
        {
            var testSubject = new LinearEquation(2.25, 5.375);
            var testResult = testSubject.EuclideanNorm;
            Assert.IsTrue(System.Math.Abs(5.826931 - testResult) < 0.0000001);
        }

        [Test]
        public void TestNormalized()
        {
            var testSubject = new LinearEquation(2.25, 5.375);
            var testResult = testSubject.Normalized;
            var testExpect = new LinearEquation(0.3861381, 0.9224410);
            Assert.IsTrue(testExpect.Equals(testResult));
        }

        [Test]
        public void TestDistance()
        {
            var testSubject00 = new LinearEquation(-1,2);
            var testSubject01 = new LinearEquation(1, 0);
            var testResult = testSubject00.GetDistance(testSubject01);
            Console.WriteLine(testResult);
            Assert.IsTrue(2.828427 - testResult < 0.0001);
        }

        [Test]
        public void TestGetDotProduct()
        {
            var testSubject00 = new LinearEquation(-1, 2);
            var testSubject01 = new LinearEquation(1, 3);

            var testResult = testSubject00.GetDotProduct(testSubject01);
            Console.WriteLine(testResult);
            Assert.AreEqual(5,testResult);
        }

        [Test]
        public void TestGetCosTheta()
        {
            var testSubject00 = new LinearEquation(-1, 2);
            var testSubject01 = new LinearEquation(1, 3);

            var testResult = testSubject00.GetCosTheta(testSubject01);
            Console.WriteLine(testResult);
            Assert.IsTrue(0.707106781 - testResult < 0.000001);
        }

        [Test]
        public void TestGetAngle()
        {
            var testSubject00 = new LinearEquation(-1, 2);
            var testSubject01 = new LinearEquation(1, 3);

            var testResult = testSubject00.GetAngle(testSubject01);
            Console.WriteLine(testResult);
            Assert.AreEqual(45, testResult);
        }

        [Test]
        public void TestGetOrthoProj()
        {
            var testSubject00 = new LinearEquation(-1, 2);
            var testSubject01 = new LinearEquation(1, 3);

            var testResult = testSubject00.GetOrthoProj(testSubject01);
            Console.WriteLine(testResult);
            Assert.IsTrue(new LinearEquation(-1,2).Equals(testResult));
        }

        [Test]
        public void TestGetImplicitCoeffs()
        {
            var p = new LinearEquation(2, 2);
            var q = new LinearEquation(6, 4);
            var testResult = LinearEquation.GetImplicitCoeffs(p, q);
            Console.WriteLine(testResult);
            Assert.AreEqual(new Tuple<double, double, double>(-2, 4, -4), testResult);
        }

        [Test]
        public void TestGetLineFromVectors()
        {
            //treating these like points 
            var p = new LinearEquation(2,2);
            var q = new LinearEquation(6,4);
            var testResult = LinearEquation.GetLineFromVectors(p, q);
            Console.WriteLine(testResult);
            Assert.IsTrue(new LinearEquation(0.5, 1).Equals(testResult));
        }

        [Test]
        public void TestGetLineFromVectors_Points()
        {
            var p = new Tuple<double, double>(-1, 3);
            var q = new Tuple<double, double>(2, -1);
            var testResult = LinearEquation.GetLineFromVectors(p, q);
            Console.WriteLine(testResult);
            Assert.IsTrue(new LinearEquation(-1.333333, 1.666667).Equals(testResult));
        }

        [Test]
        public void TestGetReciprocal()
        {
            //treat these as points 
            var p = new LinearEquation(-1, 3);
            var q = new LinearEquation(2, -1);
            var r = new LinearEquation(5, 5);

            //the line determined by points p & q
            var ln = LinearEquation.GetLineFromVectors(p, q);
            Console.WriteLine(ln);
            Assert.IsTrue(new LinearEquation(-1.333333, 1.666667).Equals(ln));
            var recipLn = ln.GetReciprocal(r);
            Console.WriteLine(recipLn);
            Assert.IsTrue(new LinearEquation(0.75, 1.25).Equals(recipLn));
        }

        [Test]
        public void TestGetIntersect()
        {
            var r = new LinearEquation(0.75, 1.25);

            //the line determined by points p & q
            var ln = new LinearEquation(-1.333333, 1.666667);
            var intesect = ln.GetIntersect(r);
            Console.WriteLine(intesect);

            Assert.IsTrue(new LinearEquation(0.2, 1.4).Equals(intesect));
        }

        [Test]
        public void TestGetRotationMatrix()
        {
            var testResult = LinearEquation.GetRotationMatrix(62.3);

            Assert.IsTrue(System.Math.Abs(testResult[0, 0] - 0.4648420) < 0.00001);
            Assert.IsTrue(System.Math.Abs(testResult[0, 1] - -0.8853936) < 0.00001);
            Assert.IsTrue(System.Math.Abs(testResult[1, 0] - 0.8853936) < 0.00001);
            Assert.IsTrue(System.Math.Abs(testResult[1, 1] - 0.4648420) < 0.00001);
        }

        [Test]
        public void TestGetRotation()
        {
            var testSubject = new LinearEquation(2, 4);
            var testResult = testSubject.GetRotation(62.3);
            Console.WriteLine(testResult);

            Assert.IsTrue(new LinearEquation(-2.611890, 3.630155).Equals(testResult));
        }

        [Test]
        public void TestGetShear()
        {
            var testSubject = new LinearEquation(2,5);
            var testResult = testSubject.GetShear();
            Console.WriteLine(testResult);
            Assert.IsTrue(new LinearEquation(2,0).Equals(testResult));
        }

        [Test]
        public void TestGetProjection()
        {
            var testSubject = new LinearEquation(2, 5);
            var testResult = testSubject.GetProjection(45);
            Console.WriteLine(testResult);
            Assert.IsTrue(new LinearEquation(4.317952, 4.317952).Equals(testResult));
        }
    }
}

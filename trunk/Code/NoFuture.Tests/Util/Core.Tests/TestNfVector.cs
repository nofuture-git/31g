using System;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class TestNfVector
    {
        [Test]
        public void TestNfVectorEquals()
        {
            var testSubject1 = new NfVector(2.25, 5.375);
            var testSubject2 = new NfVector(2.25, 5.375);
            Assert.IsTrue(testSubject1.Equals(testSubject2));

            Assert.IsTrue(testSubject1 == testSubject2);

            Assert.IsFalse(testSubject1 != testSubject2);
        }

        [Test]
        public void TestNfVectorClone()
        {
            var testSubject = new NfVector(2.25, 5.375);
            var testResult = testSubject.Clone();
            Assert.AreEqual(testSubject[0], testResult[0]);
            Assert.AreEqual(testSubject[1], testResult[1]);
        }

        [Test]
        public void TestEuclideanNorm()
        {
            var testSubject = new NfVector(2.25, 5.375);
            var testResult = testSubject.EuclideanNorm;
            Assert.IsTrue(System.Math.Abs(5.826931 - testResult) < 0.0000001);
        }

        [Test]
        public void TestNormalized()
        {
            var testSubject = new NfVector(2.25, 5.375);
            var testResult = testSubject.Normalized;
            var testExpect = new NfVector(0.3861381, 0.9224410);
            Assert.IsTrue(testExpect.Equals(testResult));
        }

        [Test]
        public void TestDistance()
        {
            var testSubject00 = new NfVector(-1,2);
            var testSubject01 = new NfVector(1, 0);
            var testResult = testSubject00.GetDistance(testSubject01);
            Console.WriteLine(testResult);
            Assert.IsTrue(2.828427 - testResult < 0.0001);
        }

        [Test]
        public void TestGetDotProduct()
        {
            var testSubject00 = new NfVector(-1, 2);
            var testSubject01 = new NfVector(1, 3);

            var testResult = testSubject00.GetDotProduct(testSubject01);
            Console.WriteLine(testResult);
            Assert.AreEqual(5,testResult);
        }

        [Test]
        public void TestGetCosTheta()
        {
            var testSubject00 = new NfVector(-1, 2);
            var testSubject01 = new NfVector(1, 3);

            var testResult = testSubject00.GetCosTheta(testSubject01);
            Console.WriteLine(testResult);
            Assert.IsTrue(0.707106781 - testResult < 0.000001);
        }

        [Test]
        public void TestGetAngle()
        {
            var testSubject00 = new NfVector(-1, 2);
            var testSubject01 = new NfVector(1, 3);

            var testResult = testSubject00.GetAngle(testSubject01);
            Console.WriteLine(testResult);
            Assert.AreEqual(45, testResult);
        }

        [Test]
        public void TestGetOrthoProj()
        {
            var testSubject00 = new NfVector(-1, 2);
            var testSubject01 = new NfVector(1, 3);

            var testResult = testSubject00.GetOrthoProj(testSubject01);
            Console.WriteLine(testResult);
            Assert.IsTrue(new NfVector(-1,2).Equals(testResult));
        }
    }
}

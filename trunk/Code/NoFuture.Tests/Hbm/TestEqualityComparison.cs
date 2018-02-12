using System;
using System.Text;
using NUnit.Framework;

namespace NoFuture.Tests.Hbm
{
    [TestFixture]
    public class TestEqualityComparison
    {
        public const string MY_TEST_STRING = "myTestString";
        [Test]
        public void TestHowStringHashcodeWorks()
        {
            var testString00 = MY_TEST_STRING;
            var testString01 = "myTestString";
            var testString02 = new StringBuilder();
            foreach (var c in MY_TEST_STRING.ToCharArray())
            {
                testString02.Append(c);
            }

            Assert.AreEqual(testString00.GetHashCode(), testString01.GetHashCode());
            Assert.AreEqual(testString01.GetHashCode(), testString02.ToString().GetHashCode());
        }

        [Test]
        public void TestHowTypeHashcodeWorks()
        {
            var testType00 = typeof (int);
            var testType01 = 11;

            Assert.AreEqual(testType00.GetHashCode(), testType01.GetType().GetHashCode());

            Assert.IsTrue(testType00 == testType01.GetType());
        }

        [Test]
        public void TestHowTupleHashcodeWorks()
        {
            var testTuple00 = new Tuple<string, Type>(MY_TEST_STRING, typeof (int));
            var testTuple01 = new Tuple<string, Type>("myTestString", typeof (int));

            Assert.AreEqual(testTuple00.GetHashCode(), testTuple01.GetHashCode());
        }

        [Test]
        public void TestHowListOfTupleHashcodeWorks()
        {
            var testList00 = new []
            {
                new Tuple<string, Type>(MY_TEST_STRING, typeof (int)),
                new Tuple<string, Type>("myTestString", typeof (int))
            };
            var testList01 = new []
            {
                new Tuple<string, Type>(MY_TEST_STRING, typeof (int)),
                new Tuple<string, Type>("myTestString", typeof (int))
            };

            Assert.AreNotEqual(testList00.GetHashCode(), testList01.GetHashCode());
        }
    }
}

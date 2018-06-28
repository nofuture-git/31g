using System;
using System.Linq;
using NoFuture.Util.Core.Algo;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests.Algo
{
    [TestFixture]
    public class TestStringDist
    {
        [Test]
        public void TestJaroWinklerDistance()
        {
            var testResult = StringDist.JaroWinklerDistance("test", "test");
            Assert.AreEqual(1D, System.Math.Round(testResult));

            testResult = StringDist.JaroWinklerDistance("kitty", "kitten");
            Assert.IsTrue(testResult - 0.893 < 0.001);

            testResult = StringDist.JaroWinklerDistance("kitty", "kite");
            Assert.IsTrue(testResult - 0.848 < 0.001);
            Console.WriteLine(testResult);

            testResult = StringDist.JaroWinklerDistance(null, null);
            Assert.AreEqual(1.0, testResult);
        }

        [Test]
        public void TestLevenshteinDistance()
        {
            var testResult = StringDist.LevenshteinDistance("kitten", "sitting");
            Assert.AreEqual(3D, testResult);
            testResult = StringDist.LevenshteinDistance("Saturday", "Sunday");
            Assert.AreEqual(3D, testResult);
            testResult = StringDist.LevenshteinDistance("Brian", "Brain");
            Assert.AreEqual(2D, testResult);

            Console.WriteLine(StringDist.LevenshteinDistance("kitty", "kitten"));
            Console.WriteLine(StringDist.LevenshteinDistance("kitty", "kite"));

            //testResult = Etc.LevenshteinDistance("sword", "swath", true);
            //Console.WriteLine(testResult);
        }

        [Test]
        public void TestShortestDistance()
        {
            var testIn = "kitty";
            var testCompare = new[] { "kitten", "cat", "kite", "can", "kool" };

            var testResult = StringDist.ShortestDistance(testIn, testCompare);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(2, testResult.Length);
            Assert.IsTrue(testResult.Contains("kitten"));
            Assert.IsTrue(testResult.Contains("kite"));

            testIn = "LeRoy";
            testCompare = new[] { "Lee", "Roy", "L.R." };
            testResult = StringDist.ShortestDistance(testIn, testCompare);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(1, testResult.Length);
            Assert.AreEqual("Roy", testResult[0]);

        }
    }
}

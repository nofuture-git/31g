using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class EtxTests
    {

        [TestMethod]
        public void TestRandomValueInNormalDist()
        {
            var mean = 500;
            var stdDev = 100;

            for (var i = 0; i < 256; i++)
            {
                var testResult = NoFuture.Rand.Etx.RandomValueInNormalDist(mean, stdDev);

                System.Diagnostics.Debug.WriteLine(testResult);
                
            }
        }

        [TestMethod]
        public void TestGetRandomRChars()
        {
            var testResult = NoFuture.Rand.Etx.GetRandomRChars();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.IsTrue(testResult.Length >= 5);
            Assert.IsTrue(testResult.Length <= 15);

            var example = new System.Text.StringBuilder();
            foreach (var r in testResult)
                example.Append(r.Rand);

            System.Diagnostics.Debug.WriteLine(example);

            testResult = NoFuture.Rand.Etx.GetRandomRChars(true);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.IsTrue(testResult.Length >= 5);
            Assert.IsTrue(testResult.Length <= 15);

            example = new System.Text.StringBuilder();
            foreach (var r in testResult)
                example.Append(r.Rand);

            Assert.IsTrue(example.ToString().ToCharArray().All(char.IsNumber));

            System.Diagnostics.Debug.WriteLine(example);
            
        }

        [TestMethod]
        public void TestRandomDouble()
        {
            var testResult = NoFuture.Rand.Etx.RationalNumber(0, 3);
            Assert.IsTrue(testResult >= 0);
            Assert.IsTrue(testResult < 4);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestDiscreteRange()
        {
            var testResult = NoFuture.Rand.Etx.DiscreteRange(null);

            //returns null 
            Assert.IsNull(testResult);

            //handles only one thing
            testResult = NoFuture.Rand.Etx.DiscreteRange(new Dictionary<string, double> {{"onlyOne", 12}});
            Assert.AreEqual("onlyOne", testResult);

            var testInput = new Dictionary<string, double>()
            {
                {"small", 2.5},
                {"expected", 95.0},
                {"x-small", 1.0},
                {"another-small", 1.5}
            };

            var testRsltCount = 0.0D;
            for (var i = 0; i < 100; i++)
            {
                testResult = NoFuture.Rand.Etx.DiscreteRange(testInput);
                if (testResult == "expected")
                    testRsltCount += 1;
            }

            var aggTestResult = testRsltCount/100.0D;
            System.Diagnostics.Debug.WriteLine(aggTestResult);
            Assert.IsTrue(0.925 <= aggTestResult && aggTestResult <= 0.975);
        }
    }
}

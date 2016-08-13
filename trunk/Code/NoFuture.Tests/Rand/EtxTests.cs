using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Domus;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class EtxTests
    {
        [TestInitialize]
        public void Init()
        {
            BinDirectories.DataRoot = @"C:\Projects\31g\trunk\bin\Data\Source";
        }
        [TestMethod]
        public void TestExtWord()
        {
            var testResult = NoFuture.Rand.Etx.Word();
            Assert.AreNotEqual(string.Empty, testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestRandomHttpUri()
        {
            for (var i = 0; i < 10; i++)
            {
                var testResult = NoFuture.Rand.Etx.RandomHttpUri();
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                System.Diagnostics.Debug.WriteLine(testResult);
            }

            for (var i = 0; i < 10; i++)
            {
                var testResult = NoFuture.Rand.Etx.RandomHttpUri(false, true);
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }

        [TestMethod]
        public void TestRandomEmailUri()
        {
            var testResult = NoFuture.Rand.Etx.RandomEmailUri();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            System.Diagnostics.Debug.WriteLine( new System.Uri("mailto:" + testResult));

        }

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
            //handles only one thing
            var testResult = NoFuture.Rand.Etx.DiscreteRange(new Dictionary<string, double> {{"onlyOne", 12}});
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

            testInput = new Dictionary<string, double>()
            {
                {".com", 1.0},
                {".net", 1.0},
                {".edu", 1.0},
                {".org", 1.0}
            };
            testResult = NoFuture.Rand.Etx.DiscreteRange(testInput);
            Assert.IsTrue(new[] { ".com" , ".net", ".edu", ".org" }.Contains(testResult));
        }

        [TestMethod]
        public void TestRandomPortions()
        {
            var testResults = NoFuture.Rand.Etx.RandomPortions(6);

            Assert.IsNotNull(testResults);
            Assert.AreEqual(6, testResults.Length);
            var testResultsSum = testResults.Sum();
            Assert.IsTrue(testResultsSum > 0.99D && testResultsSum < 1.01D);
        }
    }
}

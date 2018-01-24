﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NoFuture.Rand.Core;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests
{
    [TestFixture]
    public class EtxTests
    {
        [Test]
        public void TestExtWord()
        {
            var testResult = Etx.RandomWord();
            Assert.AreNotEqual(string.Empty, testResult);

            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [Test]
        public void TestIntNumber()
        {
            //handles in a range
            for (var i = 0; i < 100; i++)
            {
                var testResult = Etx.RandomInteger(10, 20);
                Assert.IsTrue(testResult >= 10);
                Assert.IsTrue(testResult <= 20);
            }

            //handles negative numbers
            for (var i = 0; i < 16; i++)
            {
                var testResult = Etx.RandomInteger(-90, -1);
                Assert.IsTrue(testResult < 0);
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }


        [Test]
        public void TestRandomValueInNormalDist()
        {
            var mean = 500;
            var stdDev = 100;

            var trCounts = new Dictionary<Tuple<double, double>, int>
            {
                {new Tuple<double, double>(0, 100), 0},
                {new Tuple<double, double>(100, 200), 0},
                {new Tuple<double, double>(200, 300), 0},
                {new Tuple<double, double>(300, 400), 0},
                {new Tuple<double, double>(400, 500), 0},
                {new Tuple<double, double>(500, 600), 0},
                {new Tuple<double, double>(600, 700), 0},
                {new Tuple<double, double>(700, 800), 0},
                {new Tuple<double, double>(800, 900), 0},
                {new Tuple<double, double>(900, 1000), 0}
            };


            for (var i = 0; i < 256; i++)
            {
                var testResult = Etx.RandomValueInNormalDist(mean, stdDev);

                var trCat = trCounts.Keys.FirstOrDefault(x => x.Item1 <= testResult && testResult < x.Item2);

                if(trCat == null)
                    System.Diagnostics.Debug.WriteLine(testResult);

                Assert.IsNotNull(trCat);
                trCounts[trCat] += 1;
            }

            foreach (var tr in trCounts.Keys)
            {
                System.Diagnostics.Debug.WriteLine($"{tr} {trCounts[tr]}");
            }
        }

        [Test]
        public void TestRandomValueInNormalDist_SmallRange()
        {
            for (var i = 0; i < 256; i++)
            {
                var smallDblRng = Etx.RandomValueInNormalDist(0.7889, 0.025);
                System.Diagnostics.Debug.WriteLine(smallDblRng);
                Assert.IsTrue(smallDblRng < 0.87);
            }
        }

        [Test]
        public void TestGetRandomRChars()
        {
            var testResult = Etx.RandomRChars();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.IsTrue(testResult.Length >= 5);
            Assert.IsTrue(testResult.Length <= 15);

            var example = new System.Text.StringBuilder();
            foreach (var r in testResult)
                example.Append(r.Rand);

            System.Diagnostics.Debug.WriteLine(example);

            testResult = Etx.RandomRChars(true);
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

        [Test]
        public void TestRandomDouble()
        {
            var testResult = Etx.RandomDouble(0, 3);
            Assert.IsTrue(testResult >= 0);
            Assert.IsTrue(testResult < 4);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [Test]
        public void TestRandomDouble_WithDoubles()
        {
            var testResult = Etx.RandomDouble(0.7139, 0.7889);
            Assert.IsTrue(testResult >= 0.7139);
            Assert.IsTrue(testResult <= 0.7889);

            //handles negative numbers
            testResult = Etx.RandomDouble(-0.99, -0.01);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(testResult < 0);
        }

        [Test]
        public void TestDiscreteRange()
        {
            //handles only one thing
            var testResult = Etx.RandomPickOne(new Dictionary<string, double> {{"onlyOne", 12}});
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
                testResult = Etx.RandomPickOne(testInput);
                if (testResult == "expected")
                    testRsltCount += 1;
            }

            var aggTestResult = testRsltCount/100.0D;
            System.Diagnostics.Debug.WriteLine(aggTestResult);
            Assert.IsTrue(0.89 <= aggTestResult);

            testInput = new Dictionary<string, double>()
            {
                {".com", 1.0},
                {".net", 1.0},
                {".edu", 1.0},
                {".org", 1.0}
            };
            testResult = Etx.RandomPickOne(testInput);
            Assert.IsTrue(new[] { ".com" , ".net", ".edu", ".org" }.Contains(testResult));
        }

        [Test]
        public void TestDiscreteRangeEqProb()
        {
            var discreteRange = new[] {24, 18, 12, 6};
            for (var i = 0; i < 12; i++)
            {
                var testResult = Etx.RandomPickOne(discreteRange);
                Assert.IsTrue(discreteRange.Contains(testResult));
            }
        }

        [Test]
        public void TestRandomPortions()
        {
            var testResults = Etx.RandomPortions(6);

            Assert.IsNotNull(testResults);
            Assert.AreEqual(6, testResults.Length);
            var testResultsSum = testResults.Sum();
            Assert.IsTrue(testResultsSum > 0.99D && testResultsSum < 1.01D);
        }
        [Test]
        public void TestDiminishingPortions()
        {
            var testResults = Etx.RandomDiminishingPortions(12);

            Assert.IsNotNull(testResults);
            Assert.AreEqual(12, testResults.Length);
            var testResultsSum = testResults.Sum();
            Assert.IsTrue(testResultsSum > 0.99D && testResultsSum < 1.01D);

            Assert.IsTrue(testResults.Last() == testResults.Min());

            Assert.IsTrue(testResults.First() == testResults.Max());

            testResults = Etx.RandomDiminishingPortions(12, -10.0);

            foreach (var t in testResults)
                System.Diagnostics.Debug.WriteLine(t);
        }

        [Test]
        public void TestRandShuffle()
        {
            var testInput = new[] {"", ".", "-", "_", "+", "=", "--", "__"};

            var testResult = Etx.RandomShuffle(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testInput.Length, testResult.Length);
            for (var i = 0; i < testInput.Length - 1; i++)
            {
                System.Diagnostics.Debug.WriteLine($"{testInput[i]} {testResult[i]}" );
            }
        }

        [Test]
        public void TestTryAtOrAbove()
        {
            var runningCount = new List<int>();
            var totalCount = 100;
            for (var i = 0; i < totalCount; i++)
            {
                var testRslt = Etx.RandomRollAboveOrAt(95, Etx.Dice.OneHundred) ? 1 : 0;
                runningCount.Add(testRslt);
            }

            var totalTestRslt = runningCount.Sum();
            System.Diagnostics.Debug.WriteLine(totalTestRslt);
            Assert.IsTrue(totalTestRslt < 15);
        }

        [Test]
        public void TestTryAtOrBelow()
        {
            var runningCount = new List<int>();
            var totalCount = 100;
            for (var i = 0; i < totalCount; i++)
            {
                var testRslt = Etx.RandomRollBelowOrAt(95, Etx.Dice.OneHundred) ? 1 : 0;
                runningCount.Add(testRslt);
            }

            var totalTestRslt = runningCount.Sum();
            System.Diagnostics.Debug.WriteLine(totalTestRslt);
            Assert.IsTrue(totalTestRslt > 90);
        }


        [Test]
        public void TestEnglishWords()
        {
            var testResult = Core.Etx.EnglishWords;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            var testResultItem = testResult.FirstOrDefault(x => x.Item1 == "it");
            Assert.IsNotNull(testResultItem);

            Assert.AreEqual(1386, testResultItem.Item2);
        }

        [Test]
        public void TestListRandomFactories()
        {
            var testResult = Etx.ListRandomFactories();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine(t.Name);
        }
    }
}

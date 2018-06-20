using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Util.Core.Algo;
using NUnit.Framework;

namespace NoFuture.Tests.Util
{
    [TestFixture]
    public class TestStringDist
    {
        [Test]
        public void TestJaroWinklerDistance()
        {
            var testResult = StringDist.JaroWinklerDistance("test", "test");
            Assert.AreEqual(1D, Math.Round(testResult));

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

    [TestFixture]
    public class TestHuffmanCoding
    {
        [Test]
        public void TestComparer()
        {
            var testInput = new List<HuffmanNode>
            {
                new HuffmanNode("A", 55),
                new HuffmanNode("B", 44),
                new HuffmanNode("C", 48)
            };
            var testSubject = new NodeComparer();
            testInput.Sort(testSubject);

            Assert.IsTrue(testInput[0].Word == "B");
            Assert.IsTrue(testInput[1].Word == "C");
            Assert.IsTrue(testInput[2].Word == "A");
        }

        [Test]
        public void TestBuildTree()
        {
            var leafs = new List<HuffmanNode>
            {
                new HuffmanNode("A", 3),
                new HuffmanNode("B", 3),
                new HuffmanNode("C", 2),
                new HuffmanNode("D", 1),
                new HuffmanNode("E", 1),
                new HuffmanNode("F", 1),
                new HuffmanNode("G", 1),
                new HuffmanNode("H", 1),
            };
            var totalCount = leafs.Sum(l => l.Count);
            var testing = new HuffmanEncoding(leafs);
            testing.BuildTree();
            Assert.IsNotNull(testing.RootNode);
            Assert.AreEqual(totalCount, testing.RootNode.Count);
            Assert.IsNotNull(testing.RootNode.Right);
            Assert.IsNotNull(testing.RootNode.Left);
            Console.WriteLine(testing.RootNode.Left.Count);
            Console.WriteLine(testing.RootNode.Right.Count);

        }

        [Test]
        public void TestPushEncoding()
        {
            var leafs = new List<HuffmanNode>
            {
                new HuffmanNode("A", 3),
                new HuffmanNode("B", 3),
                new HuffmanNode("C", 2),
                new HuffmanNode("D", 1),
                new HuffmanNode("E", 1),
                new HuffmanNode("F", 1),
                new HuffmanNode("G", 1),
                new HuffmanNode("H", 1),
            };
            var testing = new HuffmanEncoding(leafs);
            testing.BuildTree();
            testing.PushEncoding();
            Assert.AreEqual("111", testing.RootNode.Right.Right.Right.GetEncodingString());
        }

        [Test]
        public void TestGetLeafs()
        {
            var leafs = new List<HuffmanNode>
            {
                new HuffmanNode("A", 3),
                new HuffmanNode("B", 3),
                new HuffmanNode("C", 2),
                new HuffmanNode("D", 1),
                new HuffmanNode("E", 1),
                new HuffmanNode("F", 1),
                new HuffmanNode("G", 1),
                new HuffmanNode("H", 1),
            };
            var origLen = leafs.Count;
            var testing = new HuffmanEncoding(leafs);
            testing.BuildTree();
            testing.PushEncoding();
            leafs = testing.GetLeafs();
            Assert.AreEqual(origLen, leafs.Count);
            foreach (var l in leafs)
                Console.WriteLine(l.GetEncodingString());
        }
    }
}

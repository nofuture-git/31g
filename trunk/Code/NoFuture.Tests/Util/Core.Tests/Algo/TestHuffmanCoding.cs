using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util.Core.Algo;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests.Algo
{

    [TestFixture]
    public class TestHuffmanCoding
    {
        [Test]
        public void TestCountComparer()
        {
            var testInput = new List<HuffmanNode>
            {
                new HuffmanNode("A", 55),
                new HuffmanNode("B", 44),
                new HuffmanNode("C", 48)
            };
            var testSubject = new HuffmanNodeCountComparer();
            testInput.Sort(testSubject);

            Assert.IsTrue(testInput[0].Word == "B");
            Assert.IsTrue(testInput[1].Word == "C");
            Assert.IsTrue(testInput[2].Word == "A");
        }

        [Test]
        public void TestIndexComparer()
        {
            var testInput = new List<HuffmanNode>
            {
                new HuffmanNode("A", 55,1),
                new HuffmanNode("B", 44,8),
                new HuffmanNode("C", 48,4)
            };
            var testSubject = new HuffmanNodeIndexComparer();
            testInput.Sort(testSubject);

            Assert.IsTrue(testInput[0].Word == "A");
            Assert.IsTrue(testInput[1].Word == "C");
            Assert.IsTrue(testInput[2].Word == "B");

        }

        [Test]
        public void TestBuildTree()
        {
            var leafs = new Dictionary<string, int>
            {
                {"A", 3},
                {"B", 3},
                {"C", 2},
                {"D", 1},
                {"E", 1},
                {"F", 1},
                {"G", 1},
                {"H", 1},
            };
            var totalCount = leafs.Sum(l => l.Value);
            var testing = new HuffmanEncoding(leafs);
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
            var leafs = new Dictionary<string, int>
            {
                {"A", 3},
                {"B", 3},
                {"C", 2},
                {"D", 1},
                {"E", 1},
                {"F", 1},
                {"G", 1},
                {"H", 1},
            };
            var testing = new HuffmanEncoding(leafs);
            Assert.AreEqual("111", testing.RootNode.Right.Right.Right.GetEncodingString());
        }

        [Test]
        public void TestGetLeafs()
        {
            var t = new Dictionary<string, int>
            {
                {"A", 3},
                {"B", 3},
                {"C", 2},
                {"D", 1},
                {"E", 1},
                {"F", 1},
                {"G", 1},
                {"H", 1},
            };
            var origLen = t.Count;
            var testing = new HuffmanEncoding(t);
            var leafs = testing.GetLeafs();
            Assert.AreEqual(origLen, leafs.Count);
            var counter = 0;
            foreach (var k in t.Keys)
            {
                var node = leafs.FirstOrDefault(l => l.Word == k);
                Assert.IsNotNull(node);
                Assert.AreEqual(t[k], node.Count);
                Assert.AreEqual(counter, node.Index);
                counter += 1;
            }
            foreach (var l in leafs)
                Console.WriteLine(l.GetEncodingString());
        }

        [Test]
        public void TestGetNodeByPath()
        {
            var t = new Dictionary<string, int>
            {
                {"A", 3},
                {"B", 3},
                {"C", 2},
                {"D", 1},
                {"E", 1},
                {"F", 1},
                {"G", 1},
                {"H", 1},
            };
            var testing = new HuffmanEncoding(t);
            foreach (var leaf in testing.GetLeafs())
            {
                Console.WriteLine(string.Join(" ", leaf.Word, leaf.GetEncodingString()));
            }

            var testPath = testing.GetNodeByWord("F").Encoding;
            var testResult = testing.GetNodeByPath(testPath);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("F", testResult.Word);
        }

        [Test]
        public void TestGetLeafByWord()
        {
            var t = new Dictionary<string, int>
            {
                {"A", 3},
                {"B", 3},
                {"C", 2},
                {"D", 1},
                {"E", 1},
                {"F", 1},
                {"G", 1},
                {"H", 1},
            };
            var testing = new HuffmanEncoding(t);
            var testResult = testing.GetNodeByWord("A");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("A", testResult.Word);
            Assert.AreEqual(3, testResult.Count);

            testResult = testing.GetNodeByWord("H");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("H", testResult.Word);
            Assert.AreEqual(1, testResult.Count);
        }
    }
}

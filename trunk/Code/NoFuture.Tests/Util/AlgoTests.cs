using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Util.Core.Algo;
using NoFuture.Util.Core.Math;
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

            var testPath = testing.GetLeafByWord("F").Encoding;
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
            var testResult = testing.GetLeafByWord("A");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("A", testResult.Word);
            Assert.AreEqual(3, testResult.Count);

            testResult = testing.GetLeafByWord("H");
            Assert.IsNotNull(testResult);
            Assert.AreEqual("H", testResult.Word);
            Assert.AreEqual(1, testResult.Count);
        }
    }

    [TestFixture]
    public class TestWord2Vec
    {
        [Test]
        public void TestGetRandomWindowStartEnd()
        {
            var testing = new Word2Vec();
            var testResult = testing.GetRandomWindowStartEnd();
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.IsTrue(testResult.Item2 > testResult.Item1);

            testing.Window = 1;
            testResult = testing.GetRandomWindowStartEnd();
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestGetRandomIndicesAroundSentencePosition()
        {
            var testing = new Word2Vec();
            var testResult = testing.GetRandomIndicesAroundPosition(28, new Tuple<int, int>(2, 9));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            Assert.AreEqual(25, testResult[0]);
            Assert.AreEqual(26, testResult[1]);
            Assert.AreEqual(27, testResult[2]);
            Assert.AreEqual(29, testResult[3]);
            Assert.AreEqual(30, testResult[4]);
            Assert.AreEqual(31, testResult[5]);

            //test if we just want one-to-the-left and one-to-the-right
            testing.Window = 1;
            //since this is the first word there is only one to the right
            testResult = testing.GetRandomIndicesAroundPosition(0, new Tuple<int, int>(0, 2));
            Assert.IsNotNull(testResult);
            Assert.AreEqual(1, testResult.Length);
            Assert.AreEqual(1, testResult[0]);

            testResult = testing.GetRandomIndicesAroundPosition(1, new Tuple<int, int>(0, 2));
            Assert.IsNotNull(testResult);
            Assert.AreEqual(2, testResult.Length);
            Assert.AreEqual(0, testResult[0]);
            Assert.AreEqual(2, testResult[1]);
        }

        [Test]
        public void TestBuildVocab()
        {
            var textPath = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\HPBook3.txt";
            Console.WriteLine(textPath);

            Assert.IsTrue(System.IO.File.Exists(textPath));
            var text = System.IO.File.ReadAllText(textPath);
            Assert.IsNotNull(text);
            var testing = new Word2Vec();
            testing.BuildVocab(text);
            Assert.IsNotNull(testing.Vocab);
            var testResultLeafs = testing.Vocab.GetLeafs();
            Assert.IsNotNull(testResultLeafs);
            Assert.AreNotEqual(0, testResultLeafs.Count);

        }

        [Test]
        public void TestGetSampleSentence_Simple()
        {
            var corpus = @"The dog saw a cat. The dog chased a cat. The cat climbed a tree";

            var testing = new Word2Vec
            {
                Sample = 0,
                Size = 3,
                Window = 1
            };
            testing.BuildVocab(corpus);
            //TODO - once implementation is figured out
            var testResult = testing.GetContextNodes(2);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Console.WriteLine(string.Join(", ", testResult.Select(t => t.Word)));

        }

        [Test]
        public void TestGetSampleSentence()
        {
            var dictPath = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\HPBook3Dict.json";
            Assert.IsTrue(System.IO.File.Exists(dictPath));
            var dictText = System.IO.File.ReadAllText(dictPath);
            Assert.IsNotNull(dictText);
            var dict = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, int>>(dictText);
            Assert.IsNotNull(dict);
            Assert.IsFalse(dict.Count == 0);

            var testing = new Word2Vec();
            testing.Sample = 0;
            testing.BuildVocab(dict);
            var corpus =
                System.IO.File.ReadAllText(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\HPBook3.txt");
            testing.AssignCorpus(corpus);
            var testResult = testing.GetContextNodes(8);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            Console.WriteLine(string.Join(", ", testResult.Select(t => t.Word)));

        }

        [Test]
        public void TestReadNextNode()
        {
            var corpus = @"The dog saw a cat. The dog chased a cat. The cat climbed a tree";

            var testing = new Word2Vec
            {
                Sample = 0,
                Size = 3,
                Window = 1
            };
            testing.BuildVocab(corpus);

            var vocab = testing.Vocab;
            var leafs = vocab.GetLeafs();
            for (var i = 0; i < 4; i++)
            {
                var b = testing.ReadNextWord();
                Assert.IsNotNull(b);
                Assert.IsNotNull(b.Item1);
                Assert.IsNotNull(b.Item2);
                Assert.AreNotEqual(0, b.Item2.Count);
            }

            testing.CurrentCorpusPosition = 25;
            var testResultNull = testing.ReadNextWord();
            Assert.IsNull(testResultNull);

        }

        [Test]
        public void TestMakeSenseOfWord2Vec()
        {
            var corpus = @"The dog saw a cat. The dog chased a cat. The cat climbed a tree";

            var testing = new Word2Vec
            {
                Sample = 0,
                Size = 3,
                Window =  1
            };
            testing.BuildVocab(corpus);

            var vocab = testing.Vocab;
            var leafs = vocab.GetLeafs();
            Console.WriteLine($"{"Words",-12}Index");

            foreach (var leaf in leafs)
            {
                Console.WriteLine($"{leaf.Word,-12}{leaf.Index}");
            }

            var wi = Matrix.RandomMatrix(8, 3);
            Console.WriteLine("Initial WI");
            Console.WriteLine(wi.Print());

            var wo = Matrix.RandomMatrix(3, 8);
            Console.WriteLine("Initial W0");
            Console.WriteLine(wo.Print());

            var readWord = testing.ReadNextWord();
            while (readWord != null)
            {
                
                var targetOneHot = testing.ToOneHot(readWord.Item1);

                foreach (var contextWord in readWord.Item2)
                {
                    var contextOneHot = testing.ToOneHot(contextWord);

                    var pr = testing.GetOutputs(contextOneHot);


                    /*
                     * This function takes input as a one-hot encoded word vector and this vector,
                     * as well as a weighted sum is passed to a hidden layer.  By using the
                     * activation function, which is the sigmoid function in this case, output
                     * is generated from the hidden layer and this output is passed to the next layer,
                     * which is the output layer.
                     *
                     */
                    //L2

                    //backpropagation - this is where it gets unclear 
                    //where is the gradient applied?
                    //wi.AddAtRow(errVector.Flatten(), contextWord.Index);
                    //wo.AddAtColumn(errVector.Flatten(), contextWord.Index);
                }

                Console.WriteLine($"WI after word {testing.CurrentCorpusPosition}");
                Console.WriteLine(wi.Print());

                Console.WriteLine($"W0 after word {testing.CurrentCorpusPosition}");
                Console.WriteLine(wo.Print());

                readWord = testing.ReadNextWord();
            }
        }

        [Test]
        public void TestWord2VecMath()
        {


        }
    }
}


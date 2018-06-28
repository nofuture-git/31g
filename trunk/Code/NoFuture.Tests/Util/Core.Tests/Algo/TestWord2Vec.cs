using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NoFuture.Util.Core.Algo;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests.Algo
{

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
            var dictPath = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Util\Core.Tests\HPBook3Dict.json";
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
                System.IO.File.ReadAllText(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Util\Core.Tests\HPBook3.txt");
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
                Assert.IsNotNull(b.CurrentWord);
                Assert.IsNotNull(b.ContextWords);
                Assert.AreNotEqual(0, b.ContextWords.Count);
            }

            testing.CurrentCorpusPosition = 25;
            var testResultNull = testing.ReadNextWord();
            Assert.IsNull(testResultNull);

        }

        [Test]
        public void TestToVector()
        {
            var corpus = @"The dog saw a cat. The dog chased a cat. The cat climbed a tree";

            var testing = new Word2Vec
            {
                Sample = 0,
                Size = 3,
                Window = 1
            };
            testing.BuildVocab(corpus);
            var word00 = testing.Vocab.GetNodeByWord("dog");
            var word01 = testing.Vocab.GetNodeByWord("cat");

            var testResult = testing.ToVector(word00);
            for (var j = 0; j < testResult.CountOfColumns(); j++)
            {
                if (j == word00.Index)
                {
                    Assert.AreEqual(1D, testResult[0, j]);
                }
                else
                {
                    Assert.AreEqual(0D, testResult[0, j]);
                }
            }

            testResult = testing.ToVector(new List<HuffmanNode> { word00, word01 });
            for (var j = 0; j < testResult.CountOfColumns(); j++)
            {
                if (j == word00.Index || j == word01.Index)
                {
                    Assert.IsTrue(System.Math.Abs(testResult[0, j] - 0.5) < 0.0001);
                }
                else
                {
                    Assert.AreEqual(0D, testResult[0, j]);
                }
            }
        }

        [Test]
        public void TestFeedFoward()
        {
            var testSubject = Word2VecTestClass.GetFruitAndJuicePreset();

            var initVector = new double[1, testSubject.WI.CountOfRows()];
            var testResult = testSubject.FeedFoward(null, initVector);

            var nextWord = testSubject.ReadNextWord();
            Console.WriteLine(nextWord.CurrentWord.Word);
            foreach (var f in nextWord.ContextWords)
            {
                Console.WriteLine(f.Word);
            }

            testResult = testSubject.FeedFoward(nextWord);
            Console.WriteLine(testResult.Print());
            var expect = new[]
            {
                0.12564016936189557,
                0.12439491130889098,
                0.12421460746660097,
                0.12525675156413787,
                0.12494957008987123,
                0.12491166410164599,
                0.1252102034070606,
                0.12542212269989678
            };

            for (var i = 0; i < expect.Length; i++)
            {
                var tr = testResult[0, i];
                var ex = expect[i];
                Assert.IsTrue(System.Math.Abs(tr - ex) < 0.000001);
            }
        }

        [Test]
        public void TestBackpropagate()
        {
            var WI = new[,] {
                {0.0368690499741906,0.0135390551358178,-0.0377502766147956,0.0336466886725494,-0.0270974517460435},
                {0.016440156808328,0.0169175251931499,0.0922190173492855,-0.031504701977365,-0.0114259869379112},
                {-0.0127991043556477,0.0487416296958652,-0.0701314578625054,-0.0536020711779604,-0.0154984786713023},
                {0.0657846277420338,0.00954156336818894,-0.0136355384782122,-0.0162582306732695,0.0366232909898382},
                {0.0211230152850612,0.0597162170148065,0.057514813056921,-0.0456053966403033,0.0368811796125402},
                {0.0958493649940236,-0.0770609196168654,-0.0213859166583912,-0.0380667455206005,-0.059526237547177},
                {0.0255941354323151,-0.0347667583891967,0.0813340035180254,-0.0742663991517696,0.064415111748695},
                {-0.09638258432801,0.0478759082722831,-0.00176863414317772,0.0383402774754634,-0.048905799700369}
            };

            var WO = new[,] {
                {-0.0702967214259769,0.0859662250550306,-0.0469181300825058,0.0750174499931826,-0.0862165990221391,-0.025250033999444,0.0277256570419882,0.0687815535668198},
                {-0.0643791042102404,-0.0336792340193313,-0.0982879727139547,0.011368115391288,0.0984938144676824,0.0220246880883466,-0.00235211108920729,0.021364244130144},
                {-0.0938820250303866,0.0536523073695844,0.015810846870677,0.0204317684846147,0.0960061897505104,0.0433280812312514,0.0465293434199548,0.0599207696318258},
                {0.0455908204175489,0.0266330542632533,0.0242098380458587,-0.0212760037375968,0.0244631621634882,0.0349988715420472,-0.0567920621283315,-0.0829100279057911},
                {-0.0940704868147478,-0.0859107999996798,0.093844279644007,-0.0646651228725282,0.000752150221146719,-0.0685755731391607,-0.0400362076889892,-0.0787345594627478}
            };
            var expectedOutput = new double[,] { { 1, 0, 0, 0, 0, 0, 0, 0 } };
            var oneHot = new double[,] { { 0, 0, 1, 0, 0, 0, 0, 0 } };
            var actualOutput = oneHot.DotProduct(WI).DotProduct(WO).GetSoftmax();

            var testSubject = Word2VecTestClass.GetFruitAndJuicePreset();
            testSubject.WI = WI;
            testSubject.WO = WO;
            testSubject.Backpropagate(oneHot, actualOutput, expectedOutput);

            var expectedWI = new[,]{
                    {0.0368690499741906, 0.0135390551358178, -0.0377502766147956, 0.0336466886725494, -0.0270974517460435},
                    {0.016440156808328, 0.0169175251931499, 0.0922190173492855, -0.031504701977365, -0.0114259869379112},
                    {-0.027578790001260865, 0.03698981982855939, -0.09493812869218567, -0.04434431484927983, -0.02585083273222276},
                    {0.0657846277420338, 0.00954156336818894, -0.0136355384782122, -0.0162582306732695, 0.0366232909898382},
                    {0.0211230152850612, 0.0597162170148065, 0.057514813056921, -0.0456053966403033, 0.0368811796125402},
                    {0.0958493649940236, -0.0770609196168654, -0.0213859166583912, -0.0380667455206005, -0.059526237547177},
                    {0.0255941354323151, -0.0347667583891967, 0.0813340035180254, -0.0742663991517696, 0.064415111748695},
                    {-0.09638258432801, 0.0478759082722831, -0.00176863414317772, 0.0383402774754634, -0.048905799700369}
            };

            var expectedWO = new[,] {
                {-0.0725349259693216,0.0862846537452614,-0.0466001629378136,0.0753380848400864,-0.0858967505047844,-0.0249302825146291,0.028046172733948,0.0691026117342084},
                {-0.05585555959306,-0.0348918761599449,-0.0994988571939456,0.0101470717509588,0.0972757653324868,0.0208070084730793,-0.00357270096292947,0.0201415883980825},
                {-0.106146050954199,0.0553971066657383,0.0175531171725673,0.0221886562034776,0.0977587688524496,0.0450801286527474,0.0482855782407944,0.0616799768944566},
                {0.0362173208421463,0.0279666192412844,0.0255414700920121,-0.0199331994750246,0.0258026733134108,0.036337976324074,-0.0554497568812851,-0.081565450796141},
                {-0.0967807362519854,-0.0855252136237319,0.0942293071329041,-0.0642768650540175,0.00113945587055197,-0.0681883849867855,-0.0396480941556025,-0.078345789044033}
            };

            Assert.IsTrue(Matrix.AreEqual(testSubject.WI, expectedWI));
            Assert.IsTrue(Matrix.AreEqual(testSubject.WO, expectedWO));

        }

        [Test]
        public void TestFeedFowardCbow()
        {
            var testSubject = Word2VecTestClass.GetFruitAndJuiceCbow();
            var nextWord = testSubject.ReadNextWord();
            Console.WriteLine(nextWord.CurrentWord.Word);
            foreach (var f in nextWord.ContextWords)
            {
                Console.WriteLine(f.Word);
            }

        }

        [Test]
        public void TestGetTestData()
        {
            var testInput = "drink^juice|apple,eat^apple|orange,drink^juice|rice,drink^milk|juice,drink^rice|milk,drink^milk|water,orange^apple|juice,apple^drink|juice,rice^drink|milk,milk^water|drink,water^juice|drink,juice^water|drink";
            var testResult = Word2VecTestClass.GetTestDataTriple(testInput);
            Assert.AreNotEqual(0, testResult.Count);
            var expected = new List<string[]>
            {
                new [] {"apple", "drink", "juice"},
                new [] {"orange", "eat", "apple"},
                new [] {"rice", "drink", "juice"},
                new [] {"juice", "drink", "milk"},
                new [] {"milk", "drink", "rice"},
                new [] {"water", "drink", "milk"},
                new [] {"juice", "orange", "apple"},
                new [] {"juice", "apple", "drink"},
                new [] {"milk", "rice", "drink"},
                new [] {"drink", "milk", "water"},
                new [] {"drink", "water", "juice"},
                new [] {"drink", "juice", "water"}
            };
            for (var i = 0; i < testResult.Count; i++)
            {
                var ex = expected[i];
                var tr = testResult[i];
                Assert.AreEqual(ex[0], tr.Item1);
                Assert.AreEqual(ex[1], tr.Item2);
                Assert.AreEqual(ex[2], tr.Item3);
            }
        }

        [Test]
        public void TestGetTestDataVocab()
        {
            var testInput = "drink^juice|apple,eat^apple|orange,drink^juice|rice,drink^milk|juice,drink^rice|milk,drink^milk|water,orange^apple|juice,apple^drink|juice,rice^drink|milk,milk^water|drink,water^juice|drink,juice^water|drink";
            var testInter = Word2VecTestClass.GetTestDataTriple(testInput);
            var testResult = Word2VecTestClass.GetTestDataVocabTriple(testInter);
            Assert.IsNotNull(testResult);
            foreach (var t in testResult)
            {
                Console.WriteLine(String.Join(", ", t.Key, t.Value));
            }
        }

        [Test]
        public void TestInitTest()
        {
            var testInput = "drink^juice|apple,eat^apple|orange,drink^juice|rice,drink^milk|juice,drink^rice|milk,drink^milk|water,orange^apple|juice,apple^drink|juice,rice^drink|milk,milk^water|drink,water^juice|drink,juice^water|drink";
            var testSubject = new Word2VecTestClass();
            testSubject.InitTest(testInput);
            //"apple", "drink", "juice"
            var testResult = testSubject.ReadNextWord();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.CurrentWord);
            Assert.IsNotNull(testResult.ContextWords);
            Assert.AreEqual(2, testResult.ContextWords.Count);
            Assert.AreEqual("apple", testResult.CurrentWord.Word);
            Assert.IsTrue(testResult.ContextWords.Any(v => v.Word == "drink"));
            Assert.IsTrue(testResult.ContextWords.Any(v => v.Word == "juice"));
        }
    }

    public class Word2VecTestClass : Word2Vec
    {
        private List<Tuple<string, string, string>> _testCorpus;

        public void InitTest(string testData)
        {
            _testCorpus = testData.Contains("^") ? GetTestDataTriple(testData) : GetTestDataDouble(testData);
            var dict = GetTestDataVocabTriple(_testCorpus);
            BuildVocab(dict);
            InitWiWo();
        }

        public static List<Tuple<string, string, string>> GetTestDataDouble(string someData)
        {
            someData = someData ?? "";
            var pairs = someData.Split(',');
            var dataOut = new List<Tuple<string, string, string>>();
            for (var i = 0; i < pairs.Length; i++)
            {
                var kj = pairs[i].Trim().Split('|');
                dataOut.Add(new Tuple<string, string, string>(kj[0], kj[1], null));
            }

            return dataOut;
        }

        public static List<Tuple<string, string, string>> GetTestDataTriple(string someData)
        {
            someData = someData ?? "";
            var pairs = someData.Split(',');
            var dataOut = new List<Tuple<string, string, string>>();
            for (var i = 0; i < pairs.Length; i++)
            {
                var kj = pairs[i].Trim().Split('|');
                var t = kj.First(c => !c.Contains("^"));
                var vs = kj.First(c => c.Contains("^")).Split('^');
                dataOut.Add(new Tuple<string, string, string>(t, vs[0], vs[1]));
            }

            return dataOut;
        }

        public static Dictionary<string, int> GetTestDataVocabTriple(List<Tuple<string, string, string>> data)
        {
            var serial = new List<string>();
            foreach (var w in data)
            {
                serial.Add(w.Item1);
                serial.Add(w.Item2);
                if (w.Item3 != null)
                    serial.Add(w.Item3);
            }

            var words = serial.Distinct();
            var dict = new Dictionary<string, int>();
            foreach (var w in words)
            {
                var c = serial.Count(v => string.Equals(w, v));
                dict.Add(w, c);
            }

            return dict;
        }

        protected internal override HuffmanNode GetTargetNode(int? fromCorpusPosition = null)
        {
            var w = _testCorpus[CurrentCorpusPosition];
            return Vocab.GetNodeByWord(w.Item1);
        }

        protected internal override List<HuffmanNode> GetContextNodes(int? fromCorpusPosition = null, Tuple<int, int> windowRange = null)
        {
            var w = _testCorpus[CurrentCorpusPosition];
            var l1 = Vocab.GetNodeByWord(w.Item2);
            if (w.Item3 == null)
            {
                return new List<HuffmanNode> { l1 };
            }
            var l2 = Vocab.GetNodeByWord(w.Item3);
            return new List<HuffmanNode> { l1, l2 };
        }

        public static Word2VecTestClass GetFruitAndJuiceCbow()
        {
            var testInput =
                "drink^juice|apple,eat^apple|orange,drink^juice|rice,drink^milk|juice,drink^rice|milk,drink^milk|water,orange^apple|juice,apple^drink|juice,rice^drink|milk,milk^water|drink,water^juice|drink,juice^water|drink";
            var testSubject = new Word2VecTestClass
            {
                Size = 5,
                Sample = 0,
                Alpha = 0.2D,
                IsCbow = true,
                IsDebugMode = true
            };

            testSubject.InitTest(testInput);

            return InitTestClassVectors(testSubject);

        }

        public static Word2VecTestClass GetFruitAndJuicePreset()
        {
            var testInput = "eat|apple,eat|orange,eat|rice,drink|juice,drink|milk,drink|water,orange|juice,apple|juice,rice|milk,milk|drink,water|drink,juice|drink";


            var testSubject = new Word2VecTestClass
            {
                Size = 5,
                Sample = 0,
                Alpha = 0.2D,
                IsCbow = false,
                IsDebugMode = true
            };

            testSubject.InitTest(testInput);

            return InitTestClassVectors(testSubject);

        }

        internal static Word2VecTestClass InitTestClassVectors(Word2VecTestClass testSubject)
        {
            testSubject.WI = new[,] {
                {0.0368690499741906,0.0135390551358178,-0.0377502766147956,0.0336466886725494,-0.0270974517460435},
                {0.016440156808328,0.0169175251931499,0.0922190173492855,-0.031504701977365,-0.0114259869379112},
                {-0.0127991043556477,0.0487416296958652,-0.0701314578625054,-0.0536020711779604,-0.0154984786713023},
                {0.0657846277420338,0.00954156336818894,-0.0136355384782122,-0.0162582306732695,0.0366232909898382},
                {0.0211230152850612,0.0597162170148065,0.057514813056921,-0.0456053966403033,0.0368811796125402},
                {0.0958493649940236,-0.0770609196168654,-0.0213859166583912,-0.0380667455206005,-0.059526237547177},
                {0.0255941354323151,-0.0347667583891967,0.0813340035180254,-0.0742663991517696,0.064415111748695},
                {-0.09638258432801,0.0478759082722831,-0.00176863414317772,0.0383402774754634,-0.048905799700369}
            };

            testSubject.WO = new[,] {
                {-0.0702967214259769,0.0859662250550306,-0.0469181300825058,0.0750174499931826,-0.0862165990221391,-0.025250033999444,0.0277256570419882,0.0687815535668198},
                {-0.0643791042102404,-0.0336792340193313,-0.0982879727139547,0.011368115391288,0.0984938144676824,0.0220246880883466,-0.00235211108920729,0.021364244130144},
                {-0.0938820250303866,0.0536523073695844,0.015810846870677,0.0204317684846147,0.0960061897505104,0.0433280812312514,0.0465293434199548,0.0599207696318258},
                {0.0455908204175489,0.0266330542632533,0.0242098380458587,-0.0212760037375968,0.0244631621634882,0.0349988715420472,-0.0567920621283315,-0.0829100279057911},
                {-0.0940704868147478,-0.0859107999996798,0.093844279644007,-0.0646651228725282,0.000752150221146719,-0.0685755731391607,-0.0400362076889892,-0.0787345594627478}
            };

            testSubject.Vocab.GetNodeByWord("apple").Index = 0;
            testSubject.Vocab.GetNodeByWord("drink").Index = 1;
            testSubject.Vocab.GetNodeByWord("eat").Index = 2;
            testSubject.Vocab.GetNodeByWord("juice").Index = 3;
            testSubject.Vocab.GetNodeByWord("milk").Index = 4;
            testSubject.Vocab.GetNodeByWord("orange").Index = 5;
            testSubject.Vocab.GetNodeByWord("rice").Index = 6;
            testSubject.Vocab.GetNodeByWord("water").Index = 7;
            return testSubject;

            /* //javascript arrays

var wi = [
[0.0368690499741906,0.0135390551358178,-0.0377502766147956,0.0336466886725494,-0.0270974517460435],
[0.016440156808328,0.0169175251931499,0.0922190173492855,-0.031504701977365,-0.0114259869379112],
[-0.0127991043556477,0.0487416296958652,-0.0701314578625054,-0.0536020711779604,-0.0154984786713023],
[0.0657846277420338,0.00954156336818894,-0.0136355384782122,-0.0162582306732695,0.0366232909898382],
[0.0211230152850612,0.0597162170148065,0.057514813056921,-0.0456053966403033,0.0368811796125402],
[0.0958493649940236,-0.0770609196168654,-0.0213859166583912,-0.0380667455206005,-0.059526237547177],
[0.0255941354323151,-0.0347667583891967,0.0813340035180254,-0.0742663991517696,0.064415111748695],
[-0.09638258432801,0.0478759082722831,-0.00176863414317772,0.0383402774754634,-0.048905799700369]
];

var wo =  [
[-0.0702967214259769,0.0859662250550306,-0.0469181300825058,0.0750174499931826,-0.0862165990221391,-0.025250033999444,0.0277256570419882,0.0687815535668198],
[-0.0643791042102404,-0.0336792340193313,-0.0982879727139547,0.011368115391288,0.0984938144676824,0.0220246880883466,-0.00235211108920729,0.021364244130144],
[-0.0938820250303866,0.0536523073695844,0.015810846870677,0.0204317684846147,0.0960061897505104,0.0433280812312514,0.0465293434199548,0.0599207696318258],
[0.0455908204175489,0.0266330542632533,0.0242098380458587,-0.0212760037375968,0.0244631621634882,0.0349988715420472,-0.0567920621283315,-0.0829100279057911],
[-0.0940704868147478,-0.0859107999996798,0.093844279644007,-0.0646651228725282,0.000752150221146719,-0.0685755731391607,-0.0400362076889892,-0.0787345594627478]
];
 
             */
        }


    }
}

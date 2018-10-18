using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NoFuture.Util.DotNetMeta.TokenId;
using NoFuture.Util.DotNetMeta.TokenName;
using NoFuture.Util.DotNetMeta.TokenType;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests
{
    public class TestMetadataTokenName
    {

        [Test]
        public void TestBuildMetadataTokenName()
        {
            var tokenData = TestInit.GetTokenData();
            var testResult = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0 ,testResult.Items.Length);
        }

        [Test]
        public void TestSelectDistinct()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            Assert.IsNotNull(testSubject?.Items);

            var testResult = testSubject.SelectDistinctShallowArray();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Console.WriteLine(testResult.Length);

            foreach (var t in testResult.Take(30))
                Console.WriteLine(t);
        }

        [Test]
        public void TestSelectDistinct_WithTypes()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);

            var testResult = testSubject.SelectDistinct(tokenData.Item4.GetAsRoot());
            Assert.IsNotNull(testResult?.Items);
            Assert.AreNotEqual(0,testResult.Items.Length);
            Console.WriteLine(testResult.Items.Length);

        }

        [Test]
        public void TestSelectDistinct_ByMethod()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1);
            var testResult = testSubject.SelectDistinct("TestBegin", "Index");
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0, testResult.Items.Length);
        }

        [Test]
        public void TestBuildWithTokenTypes()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = MetadataTokenName.BuildMetadataTokenName(tokenData.Item3.GetAsRoot(),
                tokenData.Item2.GetAsRoot(), tokenData.Item1, tokenData.Item4.GetAsRoot());
            Assert.IsNotNull(testSubject);
        }

        [Test]
        public void TestGetUnion()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = tokenData.Item3.GetAsRoot();
            var testInput = testSubject.SelectDistinct("OptumController", "Index");
            var testInput2 = testSubject.SelectDistinct("OptumController", "GetInitSsoToken");

            var trIds = testInput.Items.Select(t => t.Id).ToList();
            var tr2Ids = testInput2.Items.Select(t => t.Id).ToList();

            var trNames = testInput.Items.Select(t => t.Name).ToList();
            var tr2Names = testInput2.Items.Select(t => t.Name).ToList();

            Console.WriteLine($"Total id count {trIds.Count + tr2Ids.Count}" );
            trIds.AddRange(tr2Ids);
            var distinctIds = trIds.Distinct().ToList();
            Console.WriteLine($"Distinct id count {distinctIds.Count}" );

            Console.WriteLine($"Total name count {trNames.Count  + tr2Names.Count}");
            trNames.AddRange(tr2Names);
            var distinctNames = trNames.Distinct().ToList();
            Console.WriteLine($"Distinct name count {distinctNames.Count}");

            var testResult = testInput.GetUnion(testInput2);
            Assert.AreNotEqual(0 , testResult.Items.Length);
        }

        [Test]
        public void TestSelectByRegex()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = tokenData.Item3.GetAsRoot();
            var testResult = testSubject.SelectByRegex(@"\.OptumController");
            Assert.IsNotNull(testResult?.Items);
            Assert.AreNotEqual(0, testResult.Items.Length);
            foreach(var t in testResult.Items)
                Console.WriteLine(t.Name);

        }

        [Test]
        public void TestGetTrace()
        {
            var tokenData = TestInit.GetTokenData();
            var testSubject = tokenData.Item3.GetAsRoot();
            var testResult = testSubject.GetTrace(@"\:\:RegisterParticipant");
            Assert.IsNotNull(testResult);
            var asJson = JsonConvert.SerializeObject(testResult, Formatting.Indented);
            Console.WriteLine(asJson);

        }

        [Test]
        public void TestIterateTree()
        {
            /*
             *                                 root 
             *                                  o 
             *                                  |
             *                    o-----------o-+----------o
             *                   (A)          |            |
             *                                |            |
             *          o-----------o-------o-+----o    o--+------o
             *          |          (C)      |     (H)  (I)        |
             *          |                   |                     |
             *          o          o------o-+----o          o-----+------o
             *          |          |     (F)     |          |           (M)
             *          |          |             |          |
             *          o       o--+---o         o       o--+-o----o
             *         (B)     (D)    (E)        (G)    (J)  (K)  (L)
             *
             */

            var testInput = new MetadataTokenName
            {
                Name = "root",
                Items = new[]
                {
                    new MetadataTokenName{Name = "(A) [0]"},
                    new MetadataTokenName
                    {
                        Name = "[1]",
                        Items =  new []
                        {
                            new MetadataTokenName
                            {
                                Name = "[1,0]",
                                Items = new []
                                {
                                    new MetadataTokenName
                                    {
                                        Name = "[1,0,0]",
                                        Items = new []
                                        {
                                            new MetadataTokenName{Name = "(B) [1,0,0,0]"}
                                        }
                                    }
                                }
                            },
                            new MetadataTokenName
                            {
                                Name = "(C) [1,1]"
                            },
                            new MetadataTokenName
                            {
                                Name = "[1,2]",
                                Items = new []
                                {
                                    new MetadataTokenName
                                    {
                                        Name = "[1,2,0]",
                                        Items = new []
                                        {
                                            new MetadataTokenName{Name = "(D) [1,2,0,0]"},
                                            new MetadataTokenName {Name = "(E) [1,2,0,1]"}
                                        }
                                    },
                                    new MetadataTokenName
                                    {
                                        Name = "(F) [1,2,1]"
                                    },
                                    new MetadataTokenName
                                    {
                                        Name = "[1,2,2]",
                                        Items = new []
                                        {
                                            new MetadataTokenName{Name = "(G) [1,2,2,0]"}
                                        }
                                    }
                                }
                            },
                            new MetadataTokenName
                            {
                                Name = "(H) [1,3]"
                            }
                        }
                    },
                    new MetadataTokenName
                    {
                        Name = "[2]",
                        Items = new []
                        {
                            new MetadataTokenName
                            {
                                Name = "(I) [2,0]"
                            },
                            new MetadataTokenName
                            {
                                Name = "[2,1]",
                                Items = new []
                                {
                                    new MetadataTokenName
                                    {
                                        Name = "[2,1,0]",
                                        Items = new []
                                        {
                                            new MetadataTokenName{Name = "(J) [2,1,0,0]"},
                                            new MetadataTokenName{Name = "(K) [2,1,0,1]"},
                                            new MetadataTokenName{Name = "(L) [2,1,0,2]"},
                                        }
                                    },
                                    new MetadataTokenName
                                    {
                                        Name = "(M) [2,1,1]"
                                    }
                                }
                            }
                        }
                    }
                }
            };

            Func<MetadataTokenName, bool> testSearch = (v) =>
            {
                if (v == null)
                    return false;
                var rslt = !string.IsNullOrWhiteSpace(v.Name) && v.Name.StartsWith("(");
                Console.WriteLine(v.Name);
                return rslt;
            };

            testInput.IterateTree(testSearch, AccumulateItems);

            for (var i = 0; i < _accum.Count; i++)
            {
                var popAtI = _accum.Pop();

                switch (i)
                {
                    case 0:
                        Assert.AreEqual("(M) [2,1,1]", popAtI.Name);
                        break;
                    case 1:
                        Assert.AreEqual("(L) [2,1,0,2]", popAtI.Name);
                        break;
                    case 2:
                        Assert.AreEqual("(K) [2,1,0,1]", popAtI.Name);
                        break;
                    case 3:
                        Assert.AreEqual("(J) [2,1,0,0]", popAtI.Name);
                        break;
                    case 4:
                        Assert.AreEqual("(I) [2,0]", popAtI.Name);
                        break;
                }
            }
        }

        private static Stack<MetadataTokenName> _accum = new Stack<MetadataTokenName>();
        private static MetadataTokenName AccumulateItems(MetadataTokenName something)
        {
            if (something == null)
                return null;
            _accum.Push(something);
            return something;
        }
    }
}

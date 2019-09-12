using System;
using System.Collections.Generic;
using System.Linq;
using NoFuture.Tokens.DotNetMeta.TokenName;
using NoFuture.Tokens.DotNetMeta.TokenType;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests.TokenNameTest
{
    public class TestMetadataTokenName
    {
        public static MetadataTokenName GetTestMetadataTokenNameTree()
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
            return testInput;
        }

        [Test]
        public void TestIterateTree()
        {


            Func<MetadataTokenName, bool> testSearch = (v) =>
            {
                if (v == null)
                    return false;
                var rslt = !string.IsNullOrWhiteSpace(v.Name) && v.Name.StartsWith("(");
                Console.WriteLine(v.Name);
                return rslt;
            };

            var testInput = GetTestMetadataTokenNameTree();
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

        [Test]
        public void TestIterateTree_BreakFromSelfRefLoop()
        {
            var testInput = GetTestMetadataTokenNameTree();

            //what if (G)'s parent's parent was present in its child items
            var pGpParentParent = testInput.Items[1].Items[2];
            Assert.IsTrue(pGpParentParent.Name == "[1,2]");

            var pGp = testInput.Items[1].Items[2].Items[2].Items[0];
            Assert.IsTrue(pGp.Name.StartsWith("(G)"));

            //(G)'s grandparent is also its child - nasty!
            pGp.Items = new[]
            {
                pGpParentParent
            };

            Func<MetadataTokenName, bool> testSearch = (v) =>
            {
                if (v == null)
                    return false;
                var rslt = !string.IsNullOrWhiteSpace(v.Name) && v.Name.StartsWith("(");
                Console.WriteLine(v.Name);
                return rslt;
            };

            _accum.Clear();

            testInput.IterateTree(testSearch, AccumulateItems);
            Assert.IsNotNull(_accum);
            Assert.AreEqual(13, _accum.Count);

            testInput = GetTestMetadataTokenNameTree();

            //what if (G) is its own child
            pGp = testInput.Items[1].Items[2].Items[2].Items[0];
            Assert.IsTrue(pGp.Name.StartsWith("(G)"));

            pGp.Items = new[]
            {
                pGp
            };

            _accum.Clear();
            testInput.IterateTree(testSearch, AccumulateItems);
            Assert.IsNotNull(_accum);
            Assert.AreEqual(13, _accum.Count);

        }

        private static Stack<MetadataTokenName> _accum = new Stack<MetadataTokenName>();
        private static MetadataTokenName AccumulateItems(MetadataTokenName something)
        {
            if (something == null)
                return null;
            _accum.Push(something);
            return something;
        }


        [Test]
        public void TestReassignAnyItemsByName()
        {
            var testInput = GetTestMetadataTokenNameTree();
            var testSearchFor = new MetadataTokenName {Name = "(I) [2,0]" };

            var testReplacement = new MetadataTokenName
            {
                Name = "*new* (I) [2,0]",
                Items = new[]
                {
                    new MetadataTokenName
                    {
                        Name = "*added* [2,0,0]"
                    },
                    new MetadataTokenName
                    {
                        Name = "*added* [2,0,1]",
                        Items =  new []
                        {
                            new MetadataTokenName
                            {
                                Name = "*added* [2,0,1,0]"
                            }
                        }
                    }
                }
            };

            testInput.ReassignAnyItemsByName(testSearchFor, testReplacement);
            var testResultItem = new MetadataTokenName();
            Func<MetadataTokenName, bool> searchFor = (v) => string.Equals(v?.Name, testReplacement.Name);
            Func<MetadataTokenName, MetadataTokenName> getMatch = (v) => testResultItem = v;

            testInput.IterateTree(searchFor, getMatch);

            Assert.IsNotNull(testResultItem);
            Assert.AreEqual("*new* (I) [2,0]", testResultItem.Name);
            Assert.IsNotNull(testResultItem.Items);
            Assert.AreEqual("*added* [2,0,0]", testResultItem.Items[0].Name);
            Assert.AreEqual("*added* [2,0,1]", testResultItem.Items[1].Name);
            Assert.IsNotNull(testResultItem.Items[1].Items);
            Assert.AreEqual("*added* [2,0,1,0]", testResultItem.Items[1].Items[0].Name);
        }

        [Test]
        public void TestGetFirstByVal()
        {
            //get the test tree
            var testInput = GetTestMetadataTokenNameTree();

            //target (M)
            var lookingFor = testInput.Items[2].Items[1].Items[1];
            Assert.IsNotNull(lookingFor);
            Assert.AreEqual("(M) [2,1,1]", lookingFor.Name);

            //get a copy of (M)
            var newCopy = lookingFor.GetShallowCopy();

            //set the original (M) to be ByRef
            testInput.Items[2].Items[1].Items[1].IsByRef = true;

            //have the new copy look like a real ByVal in that its got child items while the former does not
            newCopy.Items = new[]
            {
                new MetadataTokenName
                {
                    Name = "(inner M) [2,1,2,0]"
                }
            };

            //get a collection with all siblings of (M)
            var testInputItems2_1 = new List<MetadataTokenName>();
            testInputItems2_1.AddRange(testInput.Items[2].Items[1].Items);

            //add the new (M) copy with child items
            testInputItems2_1.Add(newCopy);

            //reassign the original collection with this one
            testInput.Items[2].Items[1].Items = testInputItems2_1.ToArray();

            var testResult = testInput.GetFirstByVal(new MetadataTokenName {Name = "(M) [2,1,1]"});

            Assert.IsNotNull(testResult);
            //assert its not ByRef
            Assert.IsFalse(testResult.IsByRef);

            //assert its the new one we just added because its got child items
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0, testResult.Items.Length);
        }

        [Test]
        public void TestGetAllByRefNames()
        {
            var testInput = GetTestMetadataTokenNameTree();

            //(A)
            Assert.IsTrue(testInput.Items[0].Name.StartsWith("(A)"));
            testInput.Items[0].IsByRef = true; 
            //(D)
            Assert.IsTrue(testInput.Items[1].Items[2].Items[0].Items[0].Name.StartsWith("(D)"));
            testInput.Items[1].Items[2].Items[0].Items[0].IsByRef = true;
            //(I)
            Assert.IsTrue(testInput.Items[2].Items[0].Name.StartsWith("(I)"));
            testInput.Items[2].Items[0].IsByRef = true;

            var testResults = new List<MetadataTokenName>();
            testInput.GetAllByRefNames(testResults);

            Assert.IsNotNull(testResults);
            Assert.AreEqual(3, testResults.Count);

            Assert.IsTrue(testResults.Any(v => v.Name.StartsWith("(A)")));
            Assert.IsTrue(testResults.Any(v => v.Name.StartsWith("(D)")));
            Assert.IsTrue(testResults.Any(v => v.Name.StartsWith("(I)")));
        }

        [Test]
        public void TestGetAllDeclNames()
        {
            var testInput = GetTestMetadataTokenNameTree();
            var testTypeName = "MyNamespace.MoreNs.SomeTypeName";
            var testType = new MetadataTokenType {Name = testTypeName };

            //(F)
            Assert.IsTrue(testInput.Items[1].Items[2].Items[1].Name.StartsWith("(F)"));
            testInput.Items[1].Items[2].Items[1].Name = $"{testTypeName}::Method00()";

            //(K)
            Assert.IsTrue(testInput.Items[2].Items[1].Items[0].Items[0].Name.StartsWith("(J)"));
            testInput.Items[2].Items[1].Items[0].Items[0].Name = $"{testTypeName}::SomeOtherMethod(System.String)";

            var testResults = new List<MetadataTokenName>();
            testInput.GetAllDeclNames(testType, testResults);

            Assert.IsNotNull(testResults);
            Assert.AreEqual(2, testResults.Count);
            Assert.IsTrue(testResults.Any(v => v.Name.EndsWith("Method00()")));
            Assert.IsTrue(testResults.Any(v => v.Name.EndsWith("SomeOtherMethod(System.String)")));
        }

        [Test]
        public void TestSelectByRegexRefactor()
        {
            var testInput = GetTestMetadataTokenNameTree();
            var testResult = testInput.SelectByRegex("\x28[A-Z]\x29");
            Assert.IsNotNull(testResult?.Items);
            Assert.AreEqual(13, testResult.Items.Length);

            for (var i = 0x41; i < 0x4E; i++)
            {
                var letter = Convert.ToChar(i).ToString();
                Assert.IsTrue(testResult.Items.Any(v => v.Name.StartsWith($"({letter})")));
            }
        }
    }
}

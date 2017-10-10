﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using System.IO;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds;

namespace NoFuture.Tests.Util.InvokeAsmTests
{
    [TestClass]
    public class GetTokenIdsTests
    {
        [TestMethod]
        public void TestResolveCallOfCall()
        {
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm =
                System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\Iesi.Collections.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\NHibernate.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\NoFuture.Hbm.Sid.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\SomeSecondDll.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\SomethingShared.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\ThirdDll.dll"));
            var tp = new IaaProgram(new[] {"noop"})
            {
                RootAssembly = testAsm,
                AssemblyNameRegexPattern = "(Some|ThirdDll)"
            };
            tp.Init(testAsm);
            tp.AssignAsmIndicies(testAsm);

            var testAsmTypes = testAsm.GetTypes();

            var testTokens = testAsmTypes.Select(x =>  NoFuture.Util.Gia.AssemblyAnalysis.GetMetadataToken(x)).ToArray();

            var dee = testTokens.FirstOrDefault();
            Assert.IsNotNull(dee);

            var tProgram = new IaaProgram(null);
            var tGetTokenIds = new GetTokenIds(tProgram);

            var testDepth = 0;
            foreach (var iToken in testTokens)
            {
                foreach (var tToken in iToken.Items)
                {
                    foreach (var vToken in tToken.Items)
                    {
                        tGetTokenIds.ResolveCallOfCall(vToken, ref testDepth, new Stack<MetadataTokenId>(), null);        
                    }
                }
                
            }
            Assert.IsNotNull(testTokens);

            //got this from ildasm
            var targetTokenType = 0x2000060;

            Assert.IsTrue(testTokens.Any(x => x.Id == targetTokenType));
            var testTokenRslt = testTokens.First(x => x.Id == targetTokenType);
            var tokenPrint = MetadataTokenId.Print(testTokenRslt);
            System.Diagnostics.Debug.WriteLine(tokenPrint);

            var testFlattenedRslt = MetadataTokenId.FlattenToDistinct(testTokenRslt);
            Assert.IsTrue(tokenPrint.Split('\n').Length >= testFlattenedRslt.Length);

            var testTokenNames =
                tProgram.UtilityMethods.ResolveAllTokenNames(testFlattenedRslt);
            Assert.IsNotNull(testTokenNames);
            Assert.AreNotEqual(0, testTokenNames.Count);

            foreach (var tname in testTokenNames)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1} ({2})", tname.RslvAsmIdx,tname.Id.ToString("X4"), tname.Name));
            }
        }

        [TestMethod]
        public void TestFlattenToDistinct()
        {
            var t0 = new MetadataTokenId
            {
                Id = 1,
                RslvAsmIdx = 0,
                Items = new[]
                {
                    new MetadataTokenId
                    {
                        Id = 2,
                        RslvAsmIdx = 0,
                        Items = new[]
                        {
                            new MetadataTokenId {Id = 3, RslvAsmIdx = 0},
                            new MetadataTokenId
                            {
                                Id = 4,
                                RslvAsmIdx = 0,
                                Items = new[]
                                {
                                    new MetadataTokenId {Id = 1, RslvAsmIdx = 1},
                                    new MetadataTokenId {Id = 2, RslvAsmIdx = 1}
                                }
                            },
                            new MetadataTokenId
                            {
                                Id = 5,
                                RslvAsmIdx = 0,
                                Items = new[] {new MetadataTokenId {Id = 1, RslvAsmIdx = 1}}
                            }
                            
                        },
                        
                    },
                    new MetadataTokenId {Id = 6, RslvAsmIdx = 0},
                    new MetadataTokenId {Id = 7, RslvAsmIdx = 0}
                }
            };
            var t1 = new MetadataTokenId
            {
                Id = 8,
                RslvAsmIdx = 0,
                Items = new[]
                {
                    new MetadataTokenId
                    {
                        Id = 9,
                        RslvAsmIdx = 0,
                        Items = new[] {new MetadataTokenId {Id = 1, RslvAsmIdx = 0}}
                    }
                }
            };
            var testSubject = new TokenIds {Tokens = new[] {t0, t1}};
            var testResult = testSubject.FlattenToDistinct();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreEqual(11, testResult.Length);

            testResult = testSubject.FlattenToDistinct(true);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreEqual(11,testResult.Length);

            var t1x0 = testResult.FirstOrDefault(x => x.Id == 1 && x.RslvAsmIdx == 0);
            Assert.IsNotNull(t1x0);
            Assert.IsNotNull(t1x0.Items);
            Assert.AreNotEqual(0,t1x0.Items.Length);
            Assert.AreEqual(3, t1x0.Items.Length);
        }

        [TestMethod]
        public void TestToAdjancencyMatrix()
        {
            var testDataFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\GetTokenIdsData.json";
            Assert.IsTrue(System.IO.File.Exists(testDataFile));
            var testJson =
                System.IO.File.ReadAllText(testDataFile);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testJson));
            var testInput = Newtonsoft.Json.JsonConvert.DeserializeObject<TokenIds>(testJson);
            Assert.IsNotNull(testInput);

            var testResult = testInput.GetAdjancencyMatrix(true);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Item1);
            Assert.IsNotNull(testResult.Item2);
            Assert.IsTrue(testResult.Item1.Any());
            Assert.AreNotEqual(0, testResult.Item2.GetLongLength(0));
            Assert.AreNotEqual(0, testResult.Item2.GetLongLength(1));
            Assert.AreEqual(testResult.Item2.GetLongLength(0), testResult.Item2.GetLongLength(1));
            Assert.AreEqual(testResult.Item1.Keys.Count, testResult.Item2.GetLongLength(1));

            var testResultAdjMatrix = testResult.Item2;
            var sumOfMatrix = 0;
            var viewOutputFile = Path.Combine(Path.GetDirectoryName(testDataFile), "AdjacencyMatrixTr.txt");
            File.WriteAllText(viewOutputFile, "");
            for (var i = 0; i < testResultAdjMatrix.GetLongLength(0); i++)
            {
                var viewLn = new List<int>();
                for (var j = 0; j < testResultAdjMatrix.GetLongLength(1); j++)
                {
                    sumOfMatrix += testResultAdjMatrix[i, j];
                    viewLn.Add(testResultAdjMatrix[i, j]);
                }
                File.AppendAllText(viewOutputFile, string.Join(",",viewLn) + @"
");
            }

            Assert.AreNotEqual(0, sumOfMatrix);
        }
    }
}
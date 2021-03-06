﻿using System;
using System.Collections.Generic;
using NoFuture.Tokens.DotNetMeta.TokenId;
using NoFuture.Tokens.DotNetMeta.TokenRank;
using NUnit.Framework;

namespace NoFuture.Tokens.DotNetMeta.Tests
{
    [TestFixture()]
    public class DpxTests
    {
        [Test]
        public void TestGetDpxAdjGraph()
        {
            var testInput = new List<Tuple<RankedMetadataTokenAsm, RankedMetadataTokenAsm[]>>();

            var testControl = new[,]
            {
                {0, 1, 0, 0},
                {1, 0, 1, 0},
                {0, 1, 0, 0},
                {1, 0, 0, 0}
            };

            const string ASM_NM_PREFIX = "ASM NAME ";
            for (var i = 0; i < testControl.GetLongLength(0); i++)
            {
                var tt = new RankedMetadataTokenAsm { AssemblyName = $"{ASM_NM_PREFIX} {i:00}" };
                var ttList = new List<RankedMetadataTokenAsm>();
                for (var j = 0; j < testControl.GetLongLength(1); j++)
                {
                    if (testControl[i, j] == 1)
                    {
                        ttList.Add(new RankedMetadataTokenAsm { AssemblyName = $"{ASM_NM_PREFIX} {j:00}" });
                    }
                }
                testInput.Add(new Tuple<RankedMetadataTokenAsm, RankedMetadataTokenAsm[]>(tt, ttList.ToArray()));
            }

            var testResult = Dpx.GetDpxAdjGraph(testInput);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(MetadataTokenStatus.Error, testResult.St);
            Assert.IsNotNull(testResult.Graph);
            Assert.IsNotNull(testResult.Asms);

            var trGraph = testResult.Graph;
            Assert.AreEqual(testResult.Asms.Length, trGraph.GetLongLength(0));
            Assert.AreEqual(testControl.GetLongLength(0), trGraph.GetLongLength(0));
            Assert.AreEqual(testControl.GetLongLength(1), trGraph.GetLongLength(1));
            for (var i = 0; i < testControl.GetLongLength(0); i++)
            {
                for (var j = 0; j < testControl.GetLongLength(1); j++)
                {
                    Assert.AreEqual(testControl[i, j], trGraph[i, j]);
                }
            }
        }

    }
}

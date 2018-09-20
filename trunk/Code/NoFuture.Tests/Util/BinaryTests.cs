﻿using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NoFuture.Shared;
using NoFuture.Util.DotNetMeta;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Tests.Util
{
    [TestFixture]
    public class BinaryTests
    {
        [Test]
        public void TestShiftArrayContent()
        {
            var testSource = @"
[
    {
        'column_ordinal':  null,
        'table_name':  'dbo.EntityContacts',
        'column_name':  'dbo.EntityContacts.entityId',
        'schema_name':  'dbo',
        'data_type':  'varchar',
        'string_length':  50,
        'is_nullable':  'false'
    },
    {
        'column_ordinal':  null,
        'table_name':  'dbo.EntityContacts',
        'column_name':  'dbo.EntityContacts.contactId',
";
            var testReceiving = @"
        'schema_name':  'dbo',
        'data_type':  'varchar',
        'string_length':  10,
        'is_nullable':  'false'
    },
    {
        'column_ordinal':  null,
        'table_name':  'dbo.EntityContacts',
        'column_name':  'dbo.EntityContacts.firstName',
        'schema_name':  'dbo',
        'data_type':  'varchar',
        'string_length':  50,
        'is_nullable':  'true'
    }
]
";
            var testRcvBuffer = System.Text.Encoding.UTF8.GetBytes(testReceiving);
            var testSrcBuffer = System.Text.Encoding.UTF8.GetBytes(testSource);
            var testRcvOut = NoFuture.Util.Binary.ByteArray.ShiftArrayContent(testRcvBuffer, ref testSrcBuffer, "},");
            
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(testSrcBuffer));
            Console.WriteLine("----");
            Console.WriteLine(System.Text.Encoding.UTF8.GetString(testRcvOut));
            

        }


        [Test]
        public void TestFromDWord()
        {
            uint testInput = 1999;
            var testOutput = NoFuture.Util.Binary.ByteArray.FromDWord(testInput);
            Assert.IsNotNull(testOutput);
            Assert.IsFalse(testOutput.All(x => x == 0));
            Console.WriteLine(NoFuture.Util.Binary.ByteArray.PrettyPrintByteArray(testOutput));

        }

        [Test]
        public void TestToDWord()
        {
            var testResult = NoFuture.Util.Binary.ByteArray.ToDWord((byte)0xF5, (byte)0xFF, (byte)0xFF, (byte)0xEB);
            Assert.AreEqual(-167772181, testResult);
            Console.WriteLine(testResult.ToString("X4"));
            Console.WriteLine(testResult.ToString());

            testResult = NoFuture.Util.Binary.ByteArray.ToDWord(0xA, 0, 0, 0x15);
            Assert.AreEqual(167772181, testResult);

        }

        [Test]
        public void TestBitwise()
        {
            byte p = 1;
            byte q = 65;


            Dbg.WriteLine((byte) (p & ~p));
            Dbg.WriteLine((byte) (~(p | q)));
            Dbg.WriteLine((byte) (~(~q | p)));
            Dbg.WriteLine((byte) (~p));
            Dbg.WriteLine((byte) (p & ~q));
            Dbg.WriteLine((byte) (~q));
            Dbg.WriteLine((byte) (p ^ q));
            Dbg.WriteLine((byte) (~(p & q)));
            Dbg.WriteLine((byte) (p & q));
            Dbg.WriteLine((byte) (~(p ^ q)));
            Dbg.WriteLine((byte) (q));
            Dbg.WriteLine((byte) (~p | q));
            Dbg.WriteLine((byte) (p));
            Dbg.WriteLine((byte) (~q | p));
            Dbg.WriteLine((byte) (p | q));
            Dbg.WriteLine((byte) (p | ~p));

        }

        [Test]
        public void TestCompressionPropositions()
        {
            var testResult =
                NoFuture.Util.Binary.Compression.Propositions(new[] {true, false, true, true, true, false, false, true});
            Assert.AreEqual(185,testResult);
        }

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
                var tt = new RankedMetadataTokenAsm { AssemblyName = $"{ASM_NM_PREFIX} {i:00}"};
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

            var testResult = NoFuture.Util.Binary.Dpx.GetDpxAdjGraph(testInput);
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
                    Assert.AreEqual(testControl[i,j], trGraph[i,j]);
                }
            }
        }
    }
}

using System;
using System.Linq;
using ikvm.extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;
using Dbg = System.Diagnostics.Debug;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class BinaryTests
    {
        [TestMethod]
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
            
            System.Diagnostics.Debug.WriteLine(System.Text.Encoding.UTF8.GetString(testSrcBuffer));
            System.Diagnostics.Debug.WriteLine("----");
            System.Diagnostics.Debug.WriteLine(System.Text.Encoding.UTF8.GetString(testRcvOut));
            

        }

        [TestMethod]
        public void TestBreakUpFileOverMaxJsonLength()
        {
            var testPath = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Util\TestChunkData\diaSdkData.lines.json";// 

            //var testResults = NoFuture.Util.Etc.TrySplitFileOnMarker(testPath, new Tuple<char?, char?, char?>(null,'}',',') , 1440512);//5762048
            var testResults = NoFuture.Util.Etc.TrySplitFileOnMarker(testPath, null);//5762048
        }

        [TestMethod]
        public void TestFromDWord()
        {
            uint testInput = 1999;
            var testOutput = NoFuture.Util.Binary.ByteArray.FromDWord(testInput);
            Assert.IsNotNull(testOutput);
            Assert.IsFalse(testOutput.All(x => x == 0));
            System.Diagnostics.Debug.WriteLine(NoFuture.Util.Binary.ByteArray.PrettyPrintByteArray(testOutput));

        }

        [TestMethod]
        public void TestToDWord()
        {
            var testResult = NoFuture.Util.Binary.ByteArray.ToDWord((byte)0xF5, (byte)0xFF, (byte)0xFF, (byte)0xEB);
            Assert.AreEqual(-167772181, testResult);
            System.Diagnostics.Debug.WriteLine(testResult.ToString("X4"));
            System.Diagnostics.Debug.WriteLine(testResult.toString());

            testResult = NoFuture.Util.Binary.ByteArray.ToDWord(0xA, 0, 0, 0x15);
            Assert.AreEqual(167772181, testResult);

        }

        [TestMethod]
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
    }
}

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;
using NoFuture.Shared.DiaSdk;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestPdbTargetLine
    {
        public const string TEST_PDBCOMPILAND_FILE = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\testPdbCompiland.json";

        [TestMethod]
        public void TestParseFrom()
        {
            var inputData =
                NoFuture.Shared.DiaSdk.CompilandJsonData.Parse(System.IO.File.ReadAllText(TEST_PDBCOMPILAND_FILE),
                    PdbJsonDataFormat.BackslashDoubled);
            var inputDataAsmTypeName =
                "PlummetToxic.OLP.UI.Design.Common.Common_BillRate, PlummetToxic.OLP.UI.Design, Version=1.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089";

            var testResult = NoFuture.Gen.PdbTargetLine.ParseFrom(inputData, inputDataAsmTypeName);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var t in testResult)
            {
                var data = new string[]
                {
                    t.SymbolFolder, t.SrcFile, t.OwningTypeFullName, t.MemberName,
                    t.IndexId.ToString(CultureInfo.InvariantCulture), t.StartAt.ToString(CultureInfo.InvariantCulture),
                    t.EndAt.ToString(CultureInfo.InvariantCulture)
                };
                System.Diagnostics.Debug.WriteLine(string.Join(", ",data));
            }
        }

        [TestMethod]
        public void TestGetIrregulars()
        {
            var testInput00 = new PdbTargetLine() { StartAt = 68, EndAt = 821, IndexId = 3 };
            var testInput02 = new PdbTargetLine() { StartAt = 192, EndAt = 214, IndexId = 1 };
            var testInput01 = new PdbTargetLine() { StartAt = 228, EndAt = 631, IndexId = 4 };
            var testInput03 = new PdbTargetLine() { StartAt = 634, EndAt = 678, IndexId = 5 };

            var testInput = new List<PdbTargetLine> {testInput00, testInput01, testInput02, testInput03};

            var testResult = PdbTargetLine.GetIrregulars(testInput);

            Assert.IsNotNull( testResult);

            Assert.AreEqual(1, testResult.Count);
            Assert.IsTrue(testInput00.Equals(testResult[0]));
            testInput = new List<PdbTargetLine>
            {
                new PdbTargetLine() {IndexId = 1, StartAt = 192, EndAt = 200},
                new PdbTargetLine() {IndexId = 2, StartAt = 973, EndAt = 994},
                new PdbTargetLine() {IndexId = 3, StartAt = 204, EndAt = 226},
                new PdbTargetLine() {IndexId = 4, StartAt = 68, EndAt = 833},
                new PdbTargetLine() {IndexId = 5, StartAt = 240, EndAt = 643},
                new PdbTargetLine() {IndexId = 6, StartAt = 646, EndAt = 690},

                new PdbTargetLine() {IndexId = 10, StartAt = 693, EndAt = 708},

                new PdbTargetLine() {IndexId = 11, StartAt = 713, EndAt = 810},
                new PdbTargetLine() {IndexId = 16, StartAt = 871, EndAt = 892},
                new PdbTargetLine() {IndexId = 17, StartAt = 896, EndAt = 917},
                new PdbTargetLine() {IndexId = 18, StartAt = 921, EndAt = 942},
                new PdbTargetLine() {IndexId = 19, StartAt = 946, EndAt = 967},
                new PdbTargetLine() {IndexId = 22, StartAt = 998, EndAt = 1019},
                new PdbTargetLine() {IndexId = 23, StartAt = 1023, EndAt = 1044},
                new PdbTargetLine() {IndexId = 24, StartAt = 1048, EndAt = 1069},
                new PdbTargetLine() {IndexId = 25, StartAt = 1073, EndAt = 1074},

                new PdbTargetLine() {IndexId = 15, StartAt = 838, EndAt = 867}
            };
            testResult = PdbTargetLine.GetIrregulars(testInput);
            Assert.AreEqual(1, testResult.Count);
            foreach (var pdbTargetLine in testResult)
            {
                System.Diagnostics.Debug.WriteLine(pdbTargetLine.StartAt);
            }
            /*
             
             
             
             
             
             
             
             
             
             */
        }

        [TestMethod]
        public void TestTupleEquality()
        {
            var tuple00 = new Tuple<int, int>(138, 78);
            var tuple01 = new Tuple<int, int>(138, 78);
            Assert.IsTrue(tuple00.Equals(tuple01));
        }

        [TestMethod]
        public void TestGetSupposedIrregularStartAndEnd()
        {
            var testInput00 = new PdbTargetLine() { StartAt = 68, EndAt = 821, IndexId = 3 };
            var testInput02 = new PdbTargetLine() { StartAt = 192, EndAt = 214, IndexId = 1 };
            var testInput01 = new PdbTargetLine() { StartAt = 228, EndAt = 631, IndexId = 4 };
            var testInput03 = new PdbTargetLine() { StartAt = 634, EndAt = 678, IndexId = 5 };
            
            var testInput = new List<PdbTargetLine> { testInput01, testInput02, testInput03 };

            //test overlaps all 
            var testResult = PdbTargetLine.SpliceIrregularRanges(testInput, testInput00);

            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(" StartAt: '{0}', EndAt: '{1}'", testInput00.StartAt, testInput00.EndAt);
            foreach (var f in testResult)
                System.Diagnostics.Debug.WriteLine(string.Format("({0},{1})", f.Item1, f.Item2));
            Assert.AreEqual(4, testResult.Count);

            //test is wholely within another
            testInput00 = new PdbTargetLine() {StartAt = 194, EndAt = 202, IndexId = 8};
            testResult = PdbTargetLine.SpliceIrregularRanges(testInput, testInput00);
            Assert.IsNull(testResult);

            System.Diagnostics.Debug.WriteLine("-------------------------------");

            //starts within another but has some of its own
            testInput00 = new PdbTargetLine() {StartAt = 194, EndAt = 226, IndexId = 8};
            testResult = PdbTargetLine.SpliceIrregularRanges(testInput, testInput00);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(" StartAt: '{0}', EndAt: '{1}'", testInput00.StartAt, testInput00.EndAt);
            foreach (var f in testResult)
                System.Diagnostics.Debug.WriteLine(string.Format("({0},{1})", f.Item1, f.Item2));
            Assert.AreEqual(1, testResult.Count);

            System.Diagnostics.Debug.WriteLine("-------------------------------");

            //starts in an open space then overlaps
            testInput00 = new PdbTargetLine() {StartAt = 84, EndAt = 204, IndexId = 8};
            testResult = PdbTargetLine.SpliceIrregularRanges(testInput, testInput00);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(" StartAt: '{0}', EndAt: '{1}'", testInput00.StartAt, testInput00.EndAt);
            foreach (var f in testResult)
                System.Diagnostics.Debug.WriteLine(string.Format("({0},{1})", f.Item1, f.Item2));
            Assert.AreEqual(1, testResult.Count);
            System.Diagnostics.Debug.WriteLine("-------------------------------");

            //starts in an open space overlaps then opens again
            testInput00 = new PdbTargetLine() {StartAt = 84, EndAt = 227, IndexId = 8};
            testResult = PdbTargetLine.SpliceIrregularRanges(testInput, testInput00);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(" StartAt: '{0}', EndAt: '{1}'", testInput00.StartAt, testInput00.EndAt);
            foreach (var f in testResult)
                System.Diagnostics.Debug.WriteLine(string.Format("({0},{1})", f.Item1, f.Item2));

        }
    }
}

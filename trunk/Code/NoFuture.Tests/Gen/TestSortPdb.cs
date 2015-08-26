using System;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestSortPdb
    {
        private const string TEST_DLL_FILE = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\DiaSdkTestOverloadedMethods.dll";
        private Assembly _testAsm;
        private NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines _testData;

        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Code = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
            NoFuture.CustomTools.Dia2Dump = @"C:\Projects\31g\trunk\bin\Dia2Dump.exe";
            _testAsm = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(TEST_DLL_FILE));
            var testAllLines = (new NoFuture.Shared.DiaSdk.AllLinesJsonDataFile()).GetData();
            _testData = (NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines)testAllLines;
        }

        [TestMethod]
        public void TestGetSortedPdbDataByFileName()
        {

            var testResult = NoFuture.Gen.SortPdb.GetSortedPdbDataByFileName(_testAsm, false, _testData);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0,testResult.Count);

            foreach (var k in testResult.Keys)
            {
                System.Diagnostics.Debug.WriteLine("----");
                var testPdbTarget = testResult[k];
                Assert.IsNotNull(testPdbTarget);
                Assert.AreNotEqual(0, testPdbTarget.Count);
                System.Diagnostics.Debug.WriteLine(k);
                System.Diagnostics.Debug.WriteLine("");
                foreach (var c in testPdbTarget)
                {
                    System.Diagnostics.Debug.WriteLine(c.OwningTypeFullName);
                    System.Diagnostics.Debug.WriteLine(c.MemberName);
                    System.Diagnostics.Debug.WriteLine(c.IndexId);
                    System.Diagnostics.Debug.WriteLine(c.StartAt);
                    System.Diagnostics.Debug.WriteLine(c.EndAt);
                }
                System.Diagnostics.Debug.WriteLine("----");
            }
        }

        [TestMethod]
        public void TestGetPdbMembers()
        {
            var testResult = NoFuture.Gen.SortPdb.GetPdbMembers(_testAsm, false);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.targets);
            Assert.AreNotEqual(0, testResult.targets.Count);

            foreach (var t in testResult.targets)
            {
                Assert.IsNotNull(t.declaringType);
                Assert.IsNotNull(t.name);
                System.Diagnostics.Debug.WriteLine(t.declaringType);
                System.Diagnostics.Debug.WriteLine(t.name);
            }
            System.Diagnostics.Debug.WriteLine("-----");
            var testResultDistinct = testResult.GetDistinctTargets();
            Assert.IsNotNull(testResultDistinct);
            Assert.AreNotEqual(0,testResultDistinct.Count);
            foreach (var td in testResultDistinct)
            {
                Assert.IsNotNull(td.declaringType);
                Assert.IsNotNull(td.name);
                System.Diagnostics.Debug.WriteLine(td.declaringType);
                System.Diagnostics.Debug.WriteLine(td.name);
            }
        }

    }
}

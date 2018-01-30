using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgTypeSrcCode
    {

        [TestInitialize]
        public void Init()
        {
            NfConfig.TempDirectories.Code = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.TempDirectories.Debug = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.CustomTools.Dia2Dump = TestAssembly.RootBin + @"\Dia2Dump.exe";
            NfConfig.CustomTools.InvokeGetCgType = TestAssembly.RootBin + @"\NoFuture.Gen.InvokeGetCgOfType.exe";
        }
        [TestMethod]
        public void TestCtor()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.Production.Product";
            var testAsm = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll";

            var testResult = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
        }

        [TestMethod]
        public void TestFilterOutLinesNotInMethods()
        {
            var testInputAffrim = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(11, 32),
                new Tuple<int, int>(36, 94),
                new Tuple<int, int>(99, 108),
                new Tuple<int, int>(116, 205)
            };

            var testInputLines = new[] { 13, 94, 112, 157 };
            var testResult = NoFuture.Gen.CgTypeCsSrcCode.FilterOutLinesNotInMethods(testInputLines, testInputAffrim);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreEqual(3, testResult.Length);
            Assert.IsTrue(testResult.Contains(13));
            Assert.IsTrue(testResult.Contains(94));
            Assert.IsTrue(testResult.Contains(157));

        }

        [TestMethod]
        public void TestMyRefactoredLines()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll";

            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);

            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);

            var testCgMember = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == "UsesLocalAndInstanceStuff");
            Assert.IsNotNull(testCgMember);
            var refactoredTestResults = testCgMember.MyRefactoredLines("_refactored",null);
            Assert.IsNotNull(refactoredTestResults);

            foreach (var k in refactoredTestResults.Keys)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Replace lines {0} to {1} with the line '{2}'", k.Item1, k.Item2, refactoredTestResults[k]));
            }

            System.Diagnostics.Debug.WriteLine(string.Join("\n",testCgMember.GetMyCgLines()));
        }

        [TestMethod]
        public void TestMoveMethods()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll";
            var testMethodNames = new List<string> { "UsesLocalAndInstanceStuff", "ddlScreeningLocation_SelectedIndexChanged" };

            var testOutputfile = TestAssembly.UnitTestsRoot + @"\Gen\testRefactorMethods.cs";

            if(File.Exists(testOutputfile))
                File.Delete(testOutputfile);

            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);
            var testMethod00 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[0]);
            Assert.IsNotNull(testMethod00);
            var testMethod01 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[1]);
            Assert.IsNotNull(testMethod01);
            testSubject.CgType.MoveMethods(new MoveMethodsArgs
            {
                SrcFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012\AdventureWorks2012\VeryBadCode\ViewWankathon.cs",
                MoveMembers = new List<CgMember> {testMethod00, testMethod01},
                NewVariableName = "roeiu",
                OutFilePath = testOutputfile,
                OutFileNamespaceAndType = new Tuple<string, string>("AdventureWorks.VeryBadCode", "RefactoredType")
            });

            Assert.IsTrue(File.Exists(testOutputfile));

        }

        [TestMethod]
        public void TestBlankOutMethods()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll";
            var testMethodNames = new List<string> { "MyReversedString", "UsesLocalAndInstanceStuff", "Page_Load" };
            var testSrcFile =
                File.ReadAllLines(
                    TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012\AdventureWorks2012\VeryBadCode\ViewWankathon.cs");

            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);
            var testMethod00 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[1]);
            Assert.IsNotNull(testMethod00);
            var testMethod01 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[2]);
            Assert.IsNotNull(testMethod01);
            var testProp00 = testSubject.CgType.Properties.FirstOrDefault(x => x.Name == testMethodNames[0]);
            Assert.IsNotNull(testProp00);

            NoFuture.Gen.RefactorExtensions.BlankOutMembers(testSrcFile,
                new List<CgMember> {testProp00, testMethod00, testMethod01},
                TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012\AdventureWorks2012\VeryBadCode\TestBlankOutMethods.cs");

        }

        [TestMethod]
        public void TestMyOriginalLines()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll";

            var testMethodName = "ddlScreeningLocation_SelectedIndexChanged";

            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);

            var testMethod00 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodName);
            Assert.IsNotNull(testMethod00);
            var testResult = testMethod00.GetMyOriginalLines();

            Assert.IsNotNull(testResult);

            foreach(var ln in testResult)
                System.Diagnostics.Debug.WriteLine(ln);
        }
    }
}

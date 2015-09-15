using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;
using NoFuture.Gen.LangRules;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgTypeSrcCode
    {

        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Code = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
            NoFuture.TempDirectories.Debug = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
            NoFuture.CustomTools.Dia2Dump = @"C:\Projects\31g\trunk\bin\Dia2Dump.exe";
            NoFuture.CustomTools.InvokeGetCgType = @"C:\Projects\31g\trunk\bin\NoFuture.Gen.InvokeGetCgOfType.exe";
        }
        [TestMethod]
        public void TestCtor()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.Production.Product";
            var testAsm = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012.dll";

            var testResult = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);

            //var testtypeName = "ProgramManagement_ClinicSetup_CreateClinic";
            //var testAsm = @"C:\Projects\Summit\APEX\SummitHealth.CRM.UI.Design\bin\SummitHealth.CRM.UI.Design.dll";

            //var testResult = new NoFuture.Gen.SplitSourceCsFile(testAsm, testtypeName);

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
        public void TestFindCgMethodByLineNumber()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012.dll";

            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);

            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);
            Assert.IsNotNull(testSubject.CgType.TypeFiles);
            Assert.IsNotNull(testSubject.CgType.TypeFiles.FileIndex);
            Assert.IsNotNull(testSubject.CgType.TypeFiles.UsingStatementFile);
            var testResult = testSubject.CgType.FindCgMethodByLineNumber(251);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("Page_Load", testResult.Name);

            var testCgMember = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == "UsesLocalAndInstanceStuff");
            Assert.IsNotNull(testCgMember);
            var refactoredTestResults = testCgMember.MyRefactoredLines("_refactored",null);
            Assert.IsNotNull(refactoredTestResults);

            foreach (var k in refactoredTestResults.Keys)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("Replace lines {0} to {1} with the line '{2}'", k.Item1, k.Item2, refactoredTestResults[k]));
            }

            System.Diagnostics.Debug.WriteLine(string.Join("\n",testCgMember.MyCgLines(false)));
        }

        [TestMethod]
        public void TestMoveMethods()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012.dll";
            var testMethodNames = new List<string> { "UsesLocalAndInstanceStuff", "ddlScreeningLocation_SelectedIndexChanged" };

            const string testOutputfile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\testRefactorMethods.cs";

            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);
            Assert.IsNotNull(testSubject.CgType.TypeFiles);
            var testMethod00 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[0]);
            Assert.IsNotNull(testMethod00);
            var testMethod01 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[1]);
            Assert.IsNotNull(testMethod01);
            testSubject.CgType.TypeFiles.MoveMethods(new List<CgMember>() {testMethod00, testMethod01}, "roeiu",
                testOutputfile,
                new Tuple<string, string>("AdventureWorks.VeryBadCode", "RefactoredType"));

        }

        [TestMethod]
        public void TestBlankOutMethods()
        {
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012.dll";
            var testMethodNames = new List<string> { "UsesLocalAndInstanceStuff", "Page_Load" };


            var testSubject = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
            Assert.IsNotNull(testSubject);
            Assert.IsNotNull(testSubject.CgType);
            Assert.IsNotNull(testSubject.CgType.TypeFiles);
            var testMethod00 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[0]);
            Assert.IsNotNull(testMethod00);
            System.Diagnostics.Debug.WriteLine(testMethod00.MyPdbTargetLine.StartAt);
            System.Diagnostics.Debug.WriteLine(testMethod00.MyPdbTargetLine.EndAt);
            var testMethod01 = testSubject.CgType.Methods.FirstOrDefault(x => x.Name == testMethodNames[1]);
            Assert.IsNotNull(testMethod01);
            System.Diagnostics.Debug.WriteLine(testMethod01.MyPdbTargetLine.StartAt);
            System.Diagnostics.Debug.WriteLine(testMethod01.MyPdbTargetLine.EndAt);
            var enEnd = testMethod01.GetMyEndEnclosure(testSubject.CgType.TypeFiles.OriginalSourceFileContent);
            System.Diagnostics.Debug.WriteLine(string.Format("{0},{1}",enEnd.Item1, enEnd.Item2));
            testSubject.CgType.TypeFiles.BlankOutMethods(new List<CgMember>() { testMethod00, testMethod01 }, @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012\AdventureWorks2012\VeryBadCode\TestBlankOutMethods.cs");

        }
    }
}

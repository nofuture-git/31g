using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgTypeFiles
    {
        private Assembly _testAsm;
        private const string SOME_BASE_DIR = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
        private string _testOutputDir;
        private string _testSrcFile;
        private Type _testType;
        private NoFuture.Gen.CgTypeFiles _testSubject;
        private NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines _testData;
        private Dictionary<string, List<PdbTargetLine>> _testTargets;


        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Code = SOME_BASE_DIR;
            _testOutputDir = Path.Combine(SOME_BASE_DIR, "TestCgTypeFiles");
            _testSrcFile = Path.Combine(SOME_BASE_DIR, "OverloadedMethods.cs");
            NoFuture.CustomTools.Dia2Dump = @"C:\Projects\31g\trunk\bin\Dia2Dump.exe";
            _testAsm = System.Reflection.Assembly.Load(System.IO.File.ReadAllBytes(Path.Combine(SOME_BASE_DIR, "DiaSdkTestOverloadedMethods.dll")));
            _testType = _testAsm.GetType("DiaSdkTestOverloadedMethods.OverloadedMethods");
            _testSubject = new NoFuture.Gen.CgTypeFiles(_testOutputDir, _testSrcFile, _testType.AssemblyQualifiedName);
        }

        [TestMethod]
        public void TestCtor()
        {
            Assert.IsNotNull(_testSubject);
            Assert.IsNotNull(_testSubject.RootPdbFolder);
            Assert.IsNotNull(_testSubject.OriginalSource);
            Assert.IsNotNull(_testSubject.SymbolFolder);
            Assert.IsNotNull(_testSubject.SymbolName);

            Assert.AreEqual(_testSrcFile, _testSubject.OriginalSource);
            Assert.AreEqual(_testOutputDir, _testSubject.RootPdbFolder);

            Assert.IsTrue(Directory.Exists(_testSubject.SymbolFolder));

            Assert.IsNotNull(_testSubject._srcFileContent);
            Assert.AreNotEqual(0, _testSubject._srcFileContent.Length);
        }

        [TestMethod]
        public void TestWriteUsingStatementsToFile()
        {
            _testSubject.SaveUsingStatementsToFile();
            Assert.IsTrue(File.Exists(_testSubject.UsingStatementFile));
        }

        [TestMethod]
        public void TestReadLinesFromOriginalSrc()
        {
            var unfetteredContent = File.ReadAllLines(_testSrcFile);

            Assert.AreEqual(unfetteredContent.Length, _testSubject._srcFileContent.Length);

            var testPdbTarget = new PdbTargetLine {StartAt = 24, EndAt = 29};
            var testResult = _testSubject.ReadLinesFromOriginalSrc(testPdbTarget);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult);
            System.Diagnostics.Debug.WriteLine(string.Join("\n",testResult));
        }

        [TestMethod]
        public void TestWritePdbLinesToFile()
        {
            //not a true unit test but its too complicated to draft these otherwise
            var testAllLines = (new NoFuture.Shared.DiaSdk.AllLinesJsonDataFile()).GetData();
            Assert.IsNotNull(testAllLines);

            _testData = (NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines)testAllLines;
            Assert.IsNotNull(_testData);

            _testTargets = NoFuture.Gen.SortPdb.GetSortedPdbDataByFileName(_testAsm, false, _testData);
            Assert.IsNotNull(_testTargets);
            Assert.AreNotEqual(0, _testTargets);

            var testInput = _testTargets[(_testTargets.Keys.First())];

            _testSubject.SavePdbLinesToFile(testInput);

            var ff = new DirectoryInfo(_testSubject.SymbolFolder);
            var gg = ff.GetFiles(string.Format("*.{0}", NoFuture.Gen.Settings.PdbLinesExtension));
            Assert.AreNotEqual(0,gg);
        }

        [TestMethod]
        public void TestFindPdbTargetLine()
        {
            var testCgMember = new NoFuture.Gen.CgMember()
            {
                Args =
                    new List<CgArg>
                    {
                        new CgArg {ArgName = "notTheSameParam", ArgType = "int"},
                        new CgArg {ArgName = "aStrangeName", ArgType = "string"},
                        new CgArg {ArgName = "c", ArgType = "int"}
                    },
                Name = "OvMethod",
                TypeName = "string"
            };

            PdbTargetLine testOutput;
            var testResult = _testSubject.TryFindPdbTargetLine(testCgMember, out testOutput);
            Assert.IsTrue(testResult);

            System.Diagnostics.Debug.WriteLine(testOutput.StartAt);
            System.Diagnostics.Debug.WriteLine(testOutput.EndAt);

        }

        [TestMethod]
        public void TestFindPdbTargetLineByLineNumber()
        {
            var testSubject =
                NoFuture.Gen.CgTypeFileIndex.ReadIndexFromFile(
                    @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\__nf.gen.directory.xml");

            NoFuture.Gen.PdbTargetLine testResultOut;
            var testResult = testSubject.TryFindPdbTargetLine(4404, out testResultOut);
            Assert.IsTrue(testResult);
            
        }

        [TestMethod]
        public void TestFilterNamespaceImportStmts()
        {
            var testOriginals = new[]
            {
                "using System;",
                "using System.Configuration;",
                "using System.Data;",
                "using System.Data.SqlClient;",
                "using System.Globalization;",
                "using System.Net;",
                "using System.Net.Mail;",
                "using System.Web.UI;",
                "using System.Xml.XPath;",
                "using Resources;",
                "using Wankie.BBT.Common.Constants;",
                "using Wankie.BBT.Common.U_tity;",
                "using Wankie.BBT.DTO.Properties;"
            };

            var testIncludes = new[] {"System*", "Wankie.BBT.Common.*"};

            var testExcludes = new[] {"System.Net*"};

            var testResult = NoFuture.Gen.CgTypeFiles.FilterNamespaceImportStmts(testOriginals, testIncludes,
                testExcludes);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach(var rslt in testResult)
                System.Diagnostics.Debug.WriteLine(rslt);

            System.Diagnostics.Debug.WriteLine("-----");

            testIncludes = new[] {"*"};
            testResult = NoFuture.Gen.CgTypeFiles.FilterNamespaceImportStmts(testOriginals, testIncludes,
                testExcludes);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            foreach (var rslt in testResult)
                System.Diagnostics.Debug.WriteLine(rslt);
        }
    }
}

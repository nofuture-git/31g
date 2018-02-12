using System;
using NUnit.Framework;
using NoFuture.Gen.InvokeDia2Dump;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Gen
{
    [TestFixture]
    public class TestInvokeDia2Dump
    {
        private NoFuture.Gen.InvokeDia2Dump.GetPdbData _testSubject;
        private string TEST_PDB_FILE = TestAssembly.UnitTestsRoot + @"\Gen\DiaSdkTestOverloadedMethods.pdb";

        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Code = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.CustomTools.Dia2Dump = TestAssembly.RootBin + @"\Dia2Dump.exe";
            _testSubject = new GetPdbData(TEST_PDB_FILE);
        }

        [Test]
        public void TestInvokeProcessExe()
        {
            var testSwitch = "-compiland 'DiaSdkTestOverloadedMethods.OverloadedMethods'";

            var testResult = _testSubject.InvokeProcessExe(testSwitch);
            Assert.IsNotNull(testResult);
            
        }

        [Test]
        public void TestUsingSwitchInvocation()
        {
            var testSwitch = "-compiland DiaSdkTestOverloadedMethods.OverloadedMethods";
            //var testSwitch = "-l";
            _testSubject.UsingSwitchInvocation(testSwitch,
               TestAssembly.UnitTestsRoot + @"\Gen\TestUsingSwitchInvocation.json");
        }

        [Test]
        public void TestSingleTypeNamed()
        {
            var testTypeName = "DiaSdkTestOverloadedMethods.OverloadedMethods";
            var testResult = _testSubject.SingleTypeNamed(testTypeName);
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestWithModuleSwitch()
        {
            var testResult = _testSubject.DumpAllModulesToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOf(typeof(NoFuture.Shared.DiaSdk.ModulesSwitch.PdbAllModules), testResultData);
            var testResultStrongType = (NoFuture.Shared.DiaSdk.ModulesSwitch.PdbAllModules)testResultData;
            foreach (var t in testResultStrongType.modules)
            {
                Console.WriteLine(string.Format("{0} : {1}", t.id, t.name));
            }
        }

        [Test]
        public void TestWithGlobalsSwitch()
        {
            var testResult = _testSubject.DumpAllGlobalsToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOf(typeof(NoFuture.Shared.DiaSdk.GlobalsSwtich.PdbAllGlobals),testResultData);
            var testResultStrongType = (NoFuture.Shared.DiaSdk.GlobalsSwtich.PdbAllGlobals)testResultData;
            foreach (var i in testResultStrongType.globals)
            {
                Console.WriteLine(string.Format("name: {0}", i.name));
                Console.WriteLine(string.Format("offset: {0}", i.offset));
                Console.WriteLine(string.Format("relativeVirtualAddress: {0}", i.relativeVirtualAddress));
                Console.WriteLine(string.Format("section: {0}", i.section));
                Console.WriteLine(string.Format("type: {0}", i.type));
                Console.WriteLine("");
            }
        }

        [Test]
        public void TestWithFilesSwitch()
        {
            var testResult = _testSubject.DumpAllFilesToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOf(typeof(NoFuture.Shared.DiaSdk.FilesSwitch.PdbAllFiles),testResultData);
            var testResultStrongType = (NoFuture.Shared.DiaSdk.FilesSwitch.PdbAllFiles)testResultData;
            foreach (var i in testResultStrongType.files)
            {
                Console.WriteLine(string.Format("fileName: {0}", i.fileName));
                Console.WriteLine(string.Format("moduleName: {0}", i.moduleName));
                Console.WriteLine("");
            }
        }

        [Test]
        public void TestWithSymbolsSwitch()
        {
            var testResult = _testSubject.DumpAllSymbolsToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOf(typeof(NoFuture.Shared.DiaSdk.SymbolsSwitch.PdbAllSymbols),testResultData);
            var testResultStrongType = (NoFuture.Shared.DiaSdk.SymbolsSwitch.PdbAllSymbols)testResultData;
            foreach (var i in testResultStrongType.symbols)
            {
                Console.WriteLine(string.Format("fileName: {0}", i.moduleName));
                Console.WriteLine(string.Format("moduleName: {0}", i.moduleName));
                Console.WriteLine("");
            }
        }

        [Test]
        public void TestWithSectionsSwitch()
        {
            var testResult = _testSubject.DumpAllSectionsToFile(null);
            Assert.IsNotNull(testResult);
            var testResultsData = testResult.GetData();
            Assert.IsNotNull(testResultsData);
            Assert.IsInstanceOf(typeof(NoFuture.Shared.DiaSdk.SectionSwitch.PdbAllSections),testResultsData);
            var testResultsStrongType = (NoFuture.Shared.DiaSdk.SectionSwitch.PdbAllSections) testResultsData;
            foreach (var t in testResultsStrongType.sectionContribution)
            {
                Console.WriteLine(string.Format("length: {0}", t.length));
                Console.WriteLine(string.Format("name :{0}", t.name));
                Console.WriteLine(string.Format("offset: {0}", t.offset));
                Console.WriteLine(string.Format("relativeVirtualAddress: {0}", t.relativeVirtualAddress));
                Console.WriteLine(string.Format("section: {0}", t.section));
            }
        }

        [Test]
        public void TestWithLinesSwitch()
        {
            var testResult = _testSubject.DumpAllLinesToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOf(typeof(NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines), testResultData);
            var testResultStrongType = (NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines) testResultData;
            foreach (var s in testResultStrongType.allLines)
            {
                Console.WriteLine(string.Format("length: {0}", s.moduleName));
                foreach (var m in s.moduleSymbols)
                {
                    Console.WriteLine(string.Format("file: {0}", m.file));
                    Console.WriteLine(string.Format("firstLine: {0}", m.firstLine.lineNumber));
                    Console.WriteLine(string.Format("lastLine :{0}", m.lastLine.lineNumber));

                    Console.WriteLine(string.Format("symIndexId: {0}", m.symIndexId));
                    Console.WriteLine(string.Format("symbolName: {0}", m.symbolName));
                    Console.WriteLine(string.Join(",",m.locals));
                }
            }

        }
    }
}

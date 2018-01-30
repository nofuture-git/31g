using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen.InvokeDia2Dump;
using NoFuture.Shared;
using NoFuture.Shared.Core;
using NoFuture.Shared.Core.Cfg;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestInvokeDia2Dump
    {
        private NoFuture.Gen.InvokeDia2Dump.GetPdbData _testSubject;
        private string TEST_PDB_FILE = TestAssembly.UnitTestsRoot + @"\Gen\DiaSdkTestOverloadedMethods.pdb";

        [TestInitialize]
        public void Init()
        {
            NfConfig.TempDirectories.Code = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.CustomTools.Dia2Dump = TestAssembly.RootBin + @"\Dia2Dump.exe";
            _testSubject = new GetPdbData(TEST_PDB_FILE);
        }

        [TestMethod]
        public void TestInvokeProcessExe()
        {
            var testSwitch = "-compiland 'DiaSdkTestOverloadedMethods.OverloadedMethods'";

            var testResult = _testSubject.InvokeProcessExe(testSwitch);
            Assert.IsNotNull(testResult);
            
        }

        [TestMethod]
        public void TestUsingSwitchInvocation()
        {
            var testSwitch = "-compiland DiaSdkTestOverloadedMethods.OverloadedMethods";
            //var testSwitch = "-l";
            _testSubject.UsingSwitchInvocation(testSwitch,
               TestAssembly.UnitTestsRoot + @"\Gen\TestUsingSwitchInvocation.json");
        }

        [TestMethod]
        public void TestSingleTypeNamed()
        {
            var testTypeName = "DiaSdkTestOverloadedMethods.OverloadedMethods";
            var testResult = _testSubject.SingleTypeNamed(testTypeName);
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
        public void TestWithModuleSwitch()
        {
            var testResult = _testSubject.DumpAllModulesToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOfType(testResultData, typeof(NoFuture.Shared.DiaSdk.ModulesSwitch.PdbAllModules));
            var testResultStrongType = (NoFuture.Shared.DiaSdk.ModulesSwitch.PdbAllModules)testResultData;
            foreach (var t in testResultStrongType.modules)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} : {1}", t.id, t.name));
            }
        }

        [TestMethod]
        public void TestWithGlobalsSwitch()
        {
            var testResult = _testSubject.DumpAllGlobalsToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOfType(testResultData, typeof(NoFuture.Shared.DiaSdk.GlobalsSwtich.PdbAllGlobals));
            var testResultStrongType = (NoFuture.Shared.DiaSdk.GlobalsSwtich.PdbAllGlobals)testResultData;
            foreach (var i in testResultStrongType.globals)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("name: {0}", i.name));
                System.Diagnostics.Debug.WriteLine(string.Format("offset: {0}", i.offset));
                System.Diagnostics.Debug.WriteLine(string.Format("relativeVirtualAddress: {0}", i.relativeVirtualAddress));
                System.Diagnostics.Debug.WriteLine(string.Format("section: {0}", i.section));
                System.Diagnostics.Debug.WriteLine(string.Format("type: {0}", i.type));
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        [TestMethod]
        public void TestWithFilesSwitch()
        {
            var testResult = _testSubject.DumpAllFilesToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOfType(testResultData, typeof(NoFuture.Shared.DiaSdk.FilesSwitch.PdbAllFiles));
            var testResultStrongType = (NoFuture.Shared.DiaSdk.FilesSwitch.PdbAllFiles)testResultData;
            foreach (var i in testResultStrongType.files)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("fileName: {0}", i.fileName));
                System.Diagnostics.Debug.WriteLine(string.Format("moduleName: {0}", i.moduleName));
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        [TestMethod]
        public void TestWithSymbolsSwitch()
        {
            var testResult = _testSubject.DumpAllSymbolsToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOfType(testResultData, typeof(NoFuture.Shared.DiaSdk.SymbolsSwitch.PdbAllSymbols));
            var testResultStrongType = (NoFuture.Shared.DiaSdk.SymbolsSwitch.PdbAllSymbols)testResultData;
            foreach (var i in testResultStrongType.symbols)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("fileName: {0}", i.moduleName));
                System.Diagnostics.Debug.WriteLine(string.Format("moduleName: {0}", i.moduleName));
                System.Diagnostics.Debug.WriteLine("");
            }
        }

        [TestMethod]
        public void TestWithSectionsSwitch()
        {
            var testResult = _testSubject.DumpAllSectionsToFile(null);
            Assert.IsNotNull(testResult);
            var testResultsData = testResult.GetData();
            Assert.IsNotNull(testResultsData);
            Assert.IsInstanceOfType(testResultsData, typeof(NoFuture.Shared.DiaSdk.SectionSwitch.PdbAllSections));
            var testResultsStrongType = (NoFuture.Shared.DiaSdk.SectionSwitch.PdbAllSections) testResultsData;
            foreach (var t in testResultsStrongType.sectionContribution)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("length: {0}", t.length));
                System.Diagnostics.Debug.WriteLine(string.Format("name :{0}", t.name));
                System.Diagnostics.Debug.WriteLine(string.Format("offset: {0}", t.offset));
                System.Diagnostics.Debug.WriteLine(string.Format("relativeVirtualAddress: {0}", t.relativeVirtualAddress));
                System.Diagnostics.Debug.WriteLine(string.Format("section: {0}", t.section));
            }
        }

        [TestMethod]
        public void TestWithLinesSwitch()
        {
            var testResult = _testSubject.DumpAllLinesToFile(null);
            Assert.IsNotNull(testResult);
            var testResultData = testResult.GetData();
            Assert.IsNotNull(testResultData);
            Assert.IsInstanceOfType(testResultData, typeof(NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines));
            var testResultStrongType = (NoFuture.Shared.DiaSdk.LinesSwitch.PdbAllLines) testResultData;
            foreach (var s in testResultStrongType.allLines)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("length: {0}", s.moduleName));
                foreach (var m in s.moduleSymbols)
                {
                    System.Diagnostics.Debug.WriteLine(string.Format("file: {0}", m.file));
                    System.Diagnostics.Debug.WriteLine(string.Format("firstLine: {0}", m.firstLine.lineNumber));
                    System.Diagnostics.Debug.WriteLine(string.Format("lastLine :{0}", m.lastLine.lineNumber));

                    System.Diagnostics.Debug.WriteLine(string.Format("symIndexId: {0}", m.symIndexId));
                    System.Diagnostics.Debug.WriteLine(string.Format("symbolName: {0}", m.symbolName));
                    System.Diagnostics.Debug.WriteLine(string.Join(",",m.locals));
                }
            }

        }
    }
}

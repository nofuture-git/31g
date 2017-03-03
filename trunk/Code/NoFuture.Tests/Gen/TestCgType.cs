using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;
using NoFuture.Shared;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgType
    {

        [TestInitialize]
        public void Init()
        {
            NfConfig.TempDirectories.Code = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.TempDirectories.Debug = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.CustomTools.Dia2Dump = TestAssembly.RootBin + @"\Dia2Dump.exe";
            NfConfig.CustomTools.InvokeGetCgType = TestAssembly.RootBin + @"\NoFuture.Gen.InvokeGetCgOfType.exe";
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls");
        }
        [TestMethod]
        public void TestToGraphVizString()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(
                    System.IO.File.ReadAllBytes(TestAssembly.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

            var cgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, "AdventureWorks.Person.Person", false);

            Assert.IsNotNull(cgType);

            var castTestResult = cgType;
            var testResult = castTestResult.ToGraphVizNode();
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            System.Diagnostics.Debug.WriteLine(testResult);

            testResult = castTestResult.ToGraphVizEdge();
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestResolveAllMetadataTokens()
        {
            NfConfig.UseReflectionOnlyLoad = false;
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm =
                    NoFuture.Util.Binary.Asm.NfLoadFrom(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll");

            Assert.IsNotNull(testAsm);

            var testResult = NoFuture.Gen.Etc.GetCgOfType(testAsm, testtypeName, false, true);

            Assert.IsNotNull(testResult);

            var testResultCgMem = testResult.Methods.FirstOrDefault(x => x.Name == "YouGottaGetWidIt");
            Assert.IsNotNull(testResultCgMem);

            Assert.IsNotNull(testResultCgMem.OpCodeCallAndCallvirts);
            Assert.AreNotEqual(0, testResultCgMem.OpCodeCallAndCallvirts.Count);

            foreach (var opCodeCall in testResultCgMem.OpCodeCallAndCallvirts)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0} {1} {2}", opCodeCall.DeclaringTypeAsmName, opCodeCall.Name, opCodeCall.TypeName));
            }
        }

        [TestMethod]
        public void TestFindCgMethodByTokenName()
        {
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm =
                System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\Iesi.Collections.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\NHibernate.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\NoFuture.Hbm.Sid.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\SomeSecondDll.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\SomethingShared.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestAssembly.UnitTestsRoot + @"\ExampleDlls\ThirdDll.dll"));

            const string testTypeName = "AdventureWorks.VeryBadCode.BasicGenerics";

            var testType = testAsm.GetType(testTypeName);
            Assert.IsNotNull(testType);

            var testMethod = testType.GetMember("TakesGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);

            var testAsmIndicies = new AsmIndicies()
            {
                Asms =
                    new[]
                    {
                        new MetadataTokenAsm()
                        {
                            AssemblyName = "AdventureWorks2012, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                            IndexId = 0
                        }
                    }
            };
            var testTokenName = NoFuture.Util.Gia.AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, testAsmIndicies, null);
            Assert.IsNotNull(testTokenName);

            var testCgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, testTypeName, false);
            Assert.IsNotNull(testCgType);

            var testResult = testCgType.FindCgMethodByTokenName(testTokenName);
            Assert.IsNotNull(testResult);
        }
    }
}

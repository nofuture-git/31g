using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NoFuture.Gen;
using NoFuture.Shared.Cfg;
using NoFuture.Util.Binary;
using NoFuture.Util.DotNetMeta;
using NoFuture.Util.DotNetMeta.TokenAsm;

namespace NoFuture.Tests.Gen
{
    [TestFixture]
    public class TestCgType
    {

        [SetUp]
        public void Init()
        {
            NfConfig.TempDirectories.Code = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.TempDirectories.Debug = TestAssembly.UnitTestsRoot + @"\Gen";
            NfConfig.CustomTools.Dia2Dump = TestAssembly.RootBin + @"\Dia2Dump.exe";
            NfConfig.CustomTools.InvokeGetCgType = TestAssembly.RootBin + @"\NoFuture.Gen.InvokeGetCgOfType.exe";
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls");
        }
        [Test]
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
            Console.WriteLine(testResult);

            testResult = castTestResult.ToGraphVizEdge();
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestResolveAllMetadataTokens()
        {
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            
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
                Console.WriteLine(string.Format("{0} {1} {2}", opCodeCall.DeclaringTypeAsmName, opCodeCall.Name, opCodeCall.TypeName));
            }
        }

        [Test]
        public void TestFindCgMemberByTokenName()
        {
            NoFuture.Shared.Cfg.NfConfig.CustomTools.InvokeNfTypeName =
                @"C:\Projects\31g\trunk\bin\NoFuture.Tokens.InvokeNfTypeName.exe";
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
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

            var testAsmIndicies = new AsmIndexResponse()
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
            var testTokenName = AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, testAsmIndicies, null);
            Assert.IsNotNull(testTokenName);

            var testCgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, testTypeName, false);
            Assert.IsNotNull(testCgType);

            var testResult = testCgType.FindCgMemberByTokenName(testTokenName);
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestFindCgMember()
        {
            NoFuture.Shared.Cfg.NfConfig.CustomTools.InvokeNfTypeName =
    @"C:\Projects\31g\trunk\bin\NoFuture.Tokens.InvokeNfTypeName.exe";
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
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

            var testCgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, testTypeName, false);
            Assert.IsNotNull(testCgType);

            var testResult = testCgType.FindCgMember("TakesGenericArg", new []{ "myGenericArg"});
            Assert.IsNotNull(testResult);
            Assert.AreEqual("TakesGenericArg", testResult.Name);
        }
    }
}

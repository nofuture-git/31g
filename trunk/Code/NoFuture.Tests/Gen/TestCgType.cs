using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgType
    {

        [TestInitialize]
        public void Init()
        {
            NoFuture.TempDirectories.Code = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
            NoFuture.TempDirectories.Debug = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
            NoFuture.CustomTools.Dia2Dump = @"C:\Projects\31g\trunk\bin\Dia2Dump.exe";
            NoFuture.CustomTools.InvokeGetCgType = @"C:\Projects\31g\trunk\bin\NoFuture.Gen.InvokeGetCgOfType.exe";
            NoFuture.Shared.Constants.AssemblySearchPath = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls";
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
                    System.IO.File.ReadAllBytes(@"C:\Projects\31g\trunk\bin\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks.dll"));
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
            NoFuture.Shared.Constants.UseReflectionOnlyLoad = false;
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm =
                    NoFuture.Util.Binary.Asm.NfLoadFrom(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012.dll");

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
    }
}

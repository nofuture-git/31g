using System;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class AsmTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var testInputFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\MethodBodyIl02.bin";

            var testInput = System.IO.File.ReadAllBytes(testInputFile);

            var testResult = NoFuture.Util.Binary.Asm.GetOpCodesArgs(testInput, new[] { OpCodes.Callvirt, OpCodes.Call });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var t in testResult)
                System.Diagnostics.Debug.WriteLine(t.ToString("X4"));
        }

        [TestMethod]
        public void TestGetMetadataToken()
        {
            NfConfig.AssemblySearchPaths.Add(TestAssembly.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var myAsm =
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
            var myTestType = myAsm.GetType("AdventureWorks.VeryBadCode.Program");
            var testResult = NoFuture.Util.Gia.AssemblyAnalysis.GetMetadataToken(myTestType);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Items);
            Assert.AreNotEqual(0, testResult.Items.Length);

            foreach (var t in testResult.Items)
            {
                System.Diagnostics.Debug.WriteLine("----");
                System.Diagnostics.Debug.WriteLine(t.Id.ToString("X4"));
                System.Diagnostics.Debug.WriteLine("----");
                foreach (var tt in t.Items)
                {
                    System.Diagnostics.Debug.WriteLine(tt.Id.ToString("X4"));
                }
            }
        }
    }
}

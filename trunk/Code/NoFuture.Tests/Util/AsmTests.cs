using System;
using System.Linq;
using System.Reflection.Emit;
using NUnit.Framework;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Util
{
    [TestFixture]
    public class AsmTests
    {
        [Test]
        public void TestMethod1()
        {
            var testInputFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\MethodBodyIl02.bin";

            var testInput = System.IO.File.ReadAllBytes(testInputFile);

            var testResult = NoFuture.Util.Binary.Asm.GetOpCodesArgs(testInput, new[] { OpCodes.Callvirt, OpCodes.Call });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var t in testResult)
                Console.WriteLine(t.ToString("X4"));
        }

        [Test]
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
                Console.WriteLine("----");
                Console.WriteLine(t.Id.ToString("X4"));
                Console.WriteLine("----");
                foreach (var tt in t.Items)
                {
                    Console.WriteLine(tt.Id.ToString("X4"));
                }
            }
        }
    }
}

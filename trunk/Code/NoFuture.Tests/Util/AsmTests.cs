using System;
using System.Linq;
using System.Reflection.Emit;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class AsmTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var testInputFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\MethodBodyIl02.bin";

            var testInput = System.IO.File.ReadAllBytes(testInputFile);

            var testResult = NoFuture.Util.Binary.Asm.GetOpCodesArgs(testInput, new[] { OpCodes.Callvirt, OpCodes.Call });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach(var t in testResult)
                System.Diagnostics.Debug.WriteLine(t.ToString("X4"));
        }

        [TestMethod]
        public void TestGetMetadataToken()
        {
            var myAsm =
                System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\Iesi.Collections.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\NHibernate.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\NoFuture.Hbm.Sid.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\SomeSecondDll.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\SomethingShared.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\ThirdDll.dll"));
            var myTestType = myAsm.GetType("AdventureWorks.VeryBadCode.Program");
            var testResult = NoFuture.Util.Binary.Asm.GetMetadataToken(myTestType);
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

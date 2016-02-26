using System;
using System.Linq;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Util.Gia
{
    [TestClass]
    public class AssemblyAnalysisTests
    {
        [TestMethod]
        public void TestMapAssemblyWordLeftAndRight()
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

            var testInput = new NoFuture.Util.Gia.Args.AssemblyLeftRightArgs
            {
                Assembly = testAsm,
                Separator = "-",
                MaxDepth = 16,
                TargetWord = "CreditCard"
            };

            var refactoredCode = NoFuture.Util.Gia.AssemblyAnalysis.MapAssemblyWordLeftAndRight(testInput);

            System.IO.File.WriteAllLines(@"C:\Projects\31g\trunk\temp\refactoredLeft.txt", refactoredCode.Item1.Items.Select(x => x.FlName));
        }

        [TestMethod]
        public void TestToMetadataTokenName()
        {
            NoFuture.Shared.Constants.AssemblySearchPaths.Add(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\");
            NoFuture.Shared.Constants.UseReflectionOnlyLoad = false;
            NoFuture.Util.FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm =
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

            var testType = testAsm.GetType("AdventureWorks.VeryBadCode.BasicGenerics");
            Assert.IsNotNull(testType);

            var testMethod = testType.GetMember("TakesGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);

            var testResult = NoFuture.Util.Gia.AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, null, null);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Name);
            System.Diagnostics.Debug.WriteLine(testResult.Name);
            Assert.AreEqual("AdventureWorks.VeryBadCode.BasicGenerics::TakesGenericArg(System.Collections.Generic.List`1[SomeSecondDll.MyFirstMiddleClass])", testResult.Name);

            testMethod = testType.GetMember("TakesThisAsmGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);
            testResult = NoFuture.Util.Gia.AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, null, null);
            System.Diagnostics.Debug.WriteLine(testResult.Name);
            Assert.AreEqual("AdventureWorks.VeryBadCode.BasicGenerics::TakesThisAsmGenericArg(System.Collections.Generic.List`1[AdventureWorks.VeryBadCode.Order])", testResult.Name);

        }
    }
}

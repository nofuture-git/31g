using System;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using NoFuture.Shared;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;
using NoFuture.Util.DotNetMeta;

namespace NoFuture.Tests.Util.Gia
{
    [TestFixture]
    public class AssemblyAnalysisTests
    {
        [Test]
        public void TestMapAssemblyWordLeftAndRight()
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

        }

        [Test]
        public void TestToMetadataTokenName()
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

            var testType = testAsm.GetType("AdventureWorks.VeryBadCode.BasicGenerics");
            Assert.IsNotNull(testType);

            var testMethod = testType.GetMember("TakesGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);

            var testResult = AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, null, null);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Name);
            Console.WriteLine(testResult.Name);
            Assert.AreEqual("AdventureWorks.VeryBadCode.BasicGenerics::TakesGenericArg(System.Collections.Generic.List`1[SomeSecondDll.MyFirstMiddleClass])", testResult.Name);

            testMethod = testType.GetMember("TakesThisAsmGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);
            testResult = AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, null, null);
            Console.WriteLine(testResult.Name);
            Assert.AreEqual("AdventureWorks.VeryBadCode.BasicGenerics::TakesThisAsmGenericArg(System.Collections.Generic.List`1[AdventureWorks.VeryBadCode.Order])", testResult.Name);

        }

        [Test]
        public void TestDoNotCommit()
        {
            var d = @"C:\Projects\We_Nf_Mobile\Refactor\Bfw.Client.Participant\NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds";
            var m = NoFuture.Util.DotNetMeta.Grp.TokenNameResponse.ReadFromFile(d + ".GetTokenNames.json");
            var n = NoFuture.Util.DotNetMeta.Grp.TokenIdResponse.ReadFromFile(d + ".GetTokenIds.json");
            var o = NoFuture.Util.DotNetMeta.Grp.AsmIndicies.ReadFromFile(d + ".GetAsmIndices.json");
            var testSubject = new NoFuture.Util.DotNetMeta.TokenNamesTree(m,n,o);

            var testResult = testSubject.SelectDistinct("OptumController", "Index");
            foreach (var i in testResult.Items)
            {
                Console.WriteLine($"{i.Id}, {i.Name}");
            }
        }
    }
}

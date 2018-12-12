using System;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Cfg;
using NoFuture.Util.Binary;
using NoFuture.Util.DotNetMeta.TokenAsm;
using NUnit.Framework;

namespace NoFuture.Util.DotNetMeta.Tests
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
                    System.IO.File.ReadAllBytes(TestInit.RootBin + @"\NoFuture.Hbm.Sid.dll"));
                testAsm =
                    Assembly.Load(
                        System.IO.File.ReadAllBytes(
                            TestInit.UnitTestsRoot + @"\ExampleDlls\AdventureWorks.dll"));
            }

            Assert.IsNotNull(testAsm);

        }

        [Test]
        public void TestToMetadataTokenName()
        {
            NfConfig.AssemblySearchPaths.Add(TestInit.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm =
                System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\Iesi.Collections.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\NHibernate.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\NoFuture.Hbm.Sid.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\SomeSecondDll.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\SomethingShared.dll"));
            System.Reflection.Assembly.Load(
                    System.IO.File.ReadAllBytes(
                        TestInit.UnitTestsRoot + @"\ExampleDlls\ThirdDll.dll"));

            var testType = testAsm.GetType("AdventureWorks.VeryBadCode.BasicGenerics");
            Assert.IsNotNull(testType);

            var testMethod = testType.GetMember("TakesGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);

            var asmRspn = new AsmIndexResponse
            {
                Asms = new []
                    {new MetadataTokenAsm {AssemblyName = testAsm.GetName().FullName, IndexId = 0}}
            };

            var testResult = AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, asmRspn, null);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Name);
            Console.WriteLine(testResult.Name);
            Assert.AreEqual("AdventureWorks.VeryBadCode.BasicGenerics::TakesGenericArg(System.Collections.Generic.List`1[SomeSecondDll.MyFirstMiddleClass])", testResult.Name);

            testMethod = testType.GetMember("TakesThisAsmGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);
            testResult = AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, asmRspn, null);
            Console.WriteLine(testResult.Name);
            Assert.AreEqual("AdventureWorks.VeryBadCode.BasicGenerics::TakesThisAsmGenericArg(System.Collections.Generic.List`1[AdventureWorks.VeryBadCode.Order])", testResult.Name);

        }
    }
}

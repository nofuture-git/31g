using System;
using System.IO;
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
        public void TestToMetadataTokenName()
        {
            NfConfig.AssemblySearchPaths.Add(TestInit.UnitTestsRoot + @"\ExampleDlls\");
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm = Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"AdventureWorks2012.dll")));
            Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"SomeSecondDll.dll")));
            Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"SomethingShared.dll")));
            Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"ThirdDll.dll")));
            var testType = testAsm.GetType("AdventureWorks.VeryBadCode.Program");
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

        [Test]
        public void TestGetMetadataToken()
        {
            NfConfig.AssemblySearchPaths.Add(TestInit.GetTestFileDirectory());
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var myAsm = Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"AdventureWorks2012.dll")));
            Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"SomeSecondDll.dll")));
            Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"SomethingShared.dll")));
            Assembly.Load(File.ReadAllBytes(TestInit.PutTestFileOnDisk(@"ThirdDll.dll")));
            var myTestType = myAsm.GetType("AdventureWorks.VeryBadCode.Program");
            var testResult = AssemblyAnalysis.GetMetadataToken(myTestType);
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

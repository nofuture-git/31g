using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis;
using TestProgram = NoFuture.Util.Gia.InvokeAssemblyAnalysis.Program;

namespace NoFuture.Tests.Util.InvokeAsmTests
{
    [TestClass]
    public class GetTokenIdsTests
    {
        [TestMethod]
        public void TestResolveCallOfCall()
        {
            NoFuture.Shared.Constants.AssemblySearchPath = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\";
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
            TestProgram.RootAssembly = testAsm;
            TestProgram.ManifestModule = testAsm.ManifestModule;
            TestProgram.AssemblyNameRegexPattern = "(Some|ThirdDll)";
            NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetAsmIndices.Init(testAsm);
            NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetAsmIndices.AssignAsmIndicies(testAsm);

            var testAsmTypes = testAsm.GetTypes();

            var testTokens = testAsmTypes.Select(x =>  NoFuture.Util.Gia.AssemblyAnalysis.GetMetadataToken(x,0)).ToArray();

            var dee = testTokens.FirstOrDefault();
            Assert.IsNotNull(dee);
            System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1}", dee.RswAsmIdx, dee.Id.ToString("X4")));

            var testDepth = 0;
            foreach (var iToken in testTokens)
            {
                foreach (var tToken in iToken.Items)
                {
                    foreach (var vToken in tToken.Items)
                    {
                        NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds.GetTokenIds.ResolveCallOfCall(vToken, ref testDepth);        
                    }
                }
                
            }
            System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1}", dee.RswAsmIdx, dee.Id.ToString("X4")));
            Assert.IsNotNull(testTokens);

            //got these from ildasm
            var targetTokenType = 0x2000060;
            var targetTokenMethod = 0x60005CB;

            Assert.IsTrue(testTokens.Any(x => x.Id == targetTokenType));
            var testTokenRslt = testTokens.First(x => x.Id == targetTokenType);

            System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1}", testTokenRslt.RswAsmIdx, testTokenRslt.Id.ToString("X4")));

            Assert.IsNotNull(testTokenRslt.Items);
            Assert.IsTrue(testTokenRslt.Items.Any(x => x.Id == targetTokenMethod));
            var testTokenMethod = testTokenRslt.Items.First(x => x.Id == targetTokenMethod);

            System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1}",testTokenMethod.RswAsmIdx,testTokenMethod.Id.ToString("X4")));

            System.Diagnostics.Debug.WriteLine("----");

            Assert.IsNotNull(testTokenMethod.Items);
            foreach (var cvToken in testTokenMethod.Items)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1}", cvToken.RswAsmIdx,cvToken.Id.ToString("X4")));

                if (cvToken.Items != null && cvToken.Items.Length > 0)
                {
                    foreach (var ccToken in cvToken.Items)
                    {
                        System.Diagnostics.Debug.WriteLine(string.Format("\t{0}.{1}", ccToken.RswAsmIdx, ccToken.Id.ToString("X4")));

                        if (ccToken.Items != null && ccToken.Items.Length > 0)
                        {
                            foreach (var cdToken in ccToken.Items)
                            {
                                System.Diagnostics.Debug.WriteLine(string.Format("\t\t{0}.{1}", cdToken.RswAsmIdx, cdToken.Id.ToString("X4")));    
                            }
                            
                        }
                    }
                }
            }
        }
    }
}

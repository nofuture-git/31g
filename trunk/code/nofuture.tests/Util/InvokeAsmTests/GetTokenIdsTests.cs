using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis;
using NoFuture.Util.Gia.InvokeAssemblyAnalysis.Cmds;

namespace NoFuture.Tests.Util.InvokeAsmTests
{
    [TestClass]
    public class GetTokenIdsTests
    {
        [TestMethod]
        public void TestResolveCallOfCall()
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
            var tp = new IaaProgram(new[] {"noop"})
            {
                RootAssembly = testAsm,
                AssemblyNameRegexPattern = "(Some|ThirdDll)"
            };
            tp.Init(testAsm);
            tp.AssignAsmIndicies(testAsm);

            var testAsmTypes = testAsm.GetTypes();

            var testTokens = testAsmTypes.Select(x =>  NoFuture.Util.Gia.AssemblyAnalysis.GetMetadataToken(x,0)).ToArray();

            var dee = testTokens.FirstOrDefault();
            Assert.IsNotNull(dee);

            var tProgram = new IaaProgram(null);
            var tGetTokenIds = new GetTokenIds(tProgram);

            var testDepth = 0;
            foreach (var iToken in testTokens)
            {
                foreach (var tToken in iToken.Items)
                {
                    foreach (var vToken in tToken.Items)
                    {
                        tGetTokenIds.ResolveCallOfCall(vToken, ref testDepth, new Stack<MetadataTokenId>(), null);        
                    }
                }
                
            }
            Assert.IsNotNull(testTokens);

            //got this from ildasm
            var targetTokenType = 0x2000060;

            Assert.IsTrue(testTokens.Any(x => x.Id == targetTokenType));
            var testTokenRslt = testTokens.First(x => x.Id == targetTokenType);
            var tokenPrint = MetadataTokenId.Print(testTokenRslt);
            System.Diagnostics.Debug.WriteLine(tokenPrint);

            var testFlattenedRslt = MetadataTokenId.FlattenToDistinct(testTokenRslt);
            Assert.IsTrue(tokenPrint.Split('\n').Length >= testFlattenedRslt.Length);

            var testTokenNames =
                tProgram.UtilityMethods.ResolveAllTokenNames(testFlattenedRslt);
            Assert.IsNotNull(testTokenNames);
            Assert.AreNotEqual(0, testTokenNames.Count);

            foreach (var tname in testTokenNames)
            {
                System.Diagnostics.Debug.WriteLine(string.Format("{0}.{1} ({2})", tname.RslvAsmIdx,tname.Id.ToString("X4"), tname.Name));
            }
        }
    }
}

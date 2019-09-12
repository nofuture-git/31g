using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Shared.Cfg;
using NoFuture.Tokens.DotNetMeta;
using NoFuture.Tokens.DotNetMeta.TokenAsm;
using NoFuture.Util.Binary;
using NUnit.Framework;

namespace NoFuture.Gen.Tests
{
    [TestFixture]
    public class TestCgType
    {

        [Test]
        public void TestToGraphVizString()
        {
            var testAsm = GetAdventureWorks2012();

            Assert.IsNotNull(testAsm);

            var cgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, "AdventureWorks.Person.Person", false);

            Assert.IsNotNull(cgType);

            var castTestResult = cgType;
            var testResult = castTestResult.ToGraphVizNode();
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);

            testResult = castTestResult.ToGraphVizEdge();
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestResolveAllMetadataTokens()
        {
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            
            var testtypeName = "AdventureWorks.VeryBadCode.ViewWankathon";
            var testAsm = GetAdventureWorks2012();

            Assert.IsNotNull(testAsm);

            var testResult = NoFuture.Gen.Etc.GetCgOfType(testAsm, testtypeName, false, true);

            Assert.IsNotNull(testResult);

            var testResultCgMem = testResult.Methods.FirstOrDefault(x => x.Name == "YouGottaGetWidIt");
            Assert.IsNotNull(testResultCgMem);

            Assert.IsNotNull(testResultCgMem.OpCodeCallAndCallvirts);
            Assert.AreNotEqual(0, testResultCgMem.OpCodeCallAndCallvirts.Count);

            foreach (var opCodeCall in testResultCgMem.OpCodeCallAndCallvirts)
            {
                Console.WriteLine(string.Format("{0} {1} {2}", opCodeCall.DeclaringTypeAsmName, opCodeCall.Name, opCodeCall.TypeName));
            }
        }

        [Test]
        public void TestFindCgMemberByTokenName()
        {
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm = GetAdventureWorks2012();

            const string testTypeName = "AdventureWorks.VeryBadCode.BasicGenerics";
            var testType = testAsm.GetType(testTypeName);
            Assert.IsNotNull(testType);

            var testMethod = testType.GetMember("TakesGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);

            var testAsmIndicies = new AsmIndexResponse()
            {
                Asms =
                    new[]
                    {
                        new MetadataTokenAsm()
                        {
                            AssemblyName = "AdventureWorks2012, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null",
                            IndexId = 0
                        }
                    }
            };
            var testTokenName = AssemblyAnalysis.ConvertToMetadataTokenName(testMethod, testAsmIndicies, null);
            Assert.IsNotNull(testTokenName);

            var testCgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, testTypeName, false);
            Assert.IsNotNull(testCgType);

            var testResult = testCgType.FindCgMemberByTokenName(testTokenName);
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestFindCgMember()
        {
            NfConfig.UseReflectionOnlyLoad = false;
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var testAsm = GetAdventureWorks2012();

            const string testTypeName = "AdventureWorks.VeryBadCode.BasicGenerics";
            var testType = testAsm.GetType(testTypeName);
            Assert.IsNotNull(testType);

            var testMethod = testType.GetMember("TakesGenericArg").FirstOrDefault();
            Assert.IsNotNull(testMethod);

            var testCgType = NoFuture.Gen.Etc.GetCgOfType(testAsm, testTypeName, false);
            Assert.IsNotNull(testCgType);

            var testResult = testCgType.FindCgMember("TakesGenericArg", new []{ "myGenericArg"});
            Assert.IsNotNull(testResult);
            Assert.AreEqual("TakesGenericArg", testResult.Name);
        }

        public static Assembly GetAdventureWorks2012()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.LoadFile(PutTestFileOnDisk("SomeSecondDll.dll"));
                PutTestFileOnDisk("SomeSecondDll.pdb");
                Assembly.LoadFile(PutTestFileOnDisk("SomethingShared.dll"));
                PutTestFileOnDisk("SomethingShared.pdb");
                Assembly.LoadFile(PutTestFileOnDisk("ThirdDll.dll"));
                PutTestFileOnDisk("ThirdDll.pdb");
                Assembly.LoadFile(PutTestFileOnDisk("IndependentDll.dll"));
                PutTestFileOnDisk("IndependentDll.pdb");
                PutTestFileOnDisk("AdventureWorks2012.pdb");
                testAsm =
                    Assembly.LoadFile(PutTestFileOnDisk("AdventureWorks2012.dll"));
            }

            return testAsm;
        }

        public static string PutTestFileOnDisk(string embeddedFileName)
        {
            var nfAppData = GetTestFileDirectory();
            var fileOnDisk = Path.Combine(nfAppData, embeddedFileName);
            if (File.Exists(fileOnDisk))
                return fileOnDisk;

            //need this to be another object each time and not just another reference
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.{embeddedFileName}");
            if (liSteam == null)
            {
                Assert.Fail($"Cannot find the embedded file {embeddedFileName}");
            }
            if (!Directory.Exists(nfAppData))
            {
                Directory.CreateDirectory(nfAppData);
            }

            var buffer = new byte[liSteam.Length];
            liSteam.Read(buffer, 0, buffer.Length);
            File.WriteAllBytes(fileOnDisk, buffer);
            System.Threading.Thread.Sleep(50);
            return fileOnDisk;
        }

        public static string GetTestFileDirectory()
        {
            var nfAppData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (String.IsNullOrWhiteSpace(nfAppData) || !Directory.Exists(nfAppData))
                throw new DirectoryNotFoundException("The Environment.GetFolderPath for " +
                                                     "SpecialFolder.ApplicationData returned a bad path.");
            nfAppData = Path.Combine(nfAppData, "NoFuture.Tests");
            return nfAppData;
        }
    }
}

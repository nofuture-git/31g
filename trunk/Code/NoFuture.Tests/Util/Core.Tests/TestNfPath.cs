using System;
using System.IO;
using NUnit.Framework;

namespace NoFuture.Util.Core.Tests
{
    [TestFixture]
    public class TestNfPath
    {
        [Test]
        public void TestTryGetRelPath()
        {
            var testInput = @"admin\SomeFile.fs";
            var testResult = NfPath.TryGetRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsFalse(testResult);
            Assert.AreEqual(@"admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\MyProject\admin\SomeFile.fs";
            testResult = NfPath.TryGetRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\MyProject\AnotherFolder\admin\SomeFile.fs";
            testResult = NfPath.TryGetRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"AnotherFolder\admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\DiffProj\admin\SomeFile.fs";
            testResult = NfPath.TryGetRelPath(@"C:\Projects\MyProject", ref testInput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"..\DiffProj\admin\SomeFile.fs", testInput);

            testInput = @"C:\Projects\SomeBigProj\SomeSubProj.WithDots.MoreDots\SomeSubProj.WithDots.MoreDots.fsproj";
            testResult = NfPath.TryGetRelPath(@"C:\Projects\SomeBigProj\SomeOtherProj.WithDots", ref testInput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"..\SomeSubProj.WithDots.MoreDots\SomeSubProj.WithDots.MoreDots.fsproj", testInput);

            testInput = @"C:\Projects\QuickView\source\Tam.Vmm2\Tam.Vmm2.Web.Lib\..\Tam.Vmm2.Lib.Config\Tam.Vmm2.Lib.Config.csproj";
            testResult = NfPath.TryGetRelPath(@"C:\Projects\We_Nf\We.Cli.Insurgo", ref testInput);
            Assert.IsTrue(testResult);
            Assert.AreEqual(@"..\..\QuickView\source\Tam.Vmm2\Tam.Vmm2.Lib.Config\Tam.Vmm2.Lib.Config.csproj", testInput);

            testInput = @"C:\Projects\31g\trunk\bin\Iesi.Collections.dll";
            testResult =
                NfPath.TryGetRelPath(
                    @"C:\Projects\31g\trunk\Code\NoFuture.Tests\ExampleDlls\AdventureWorks2012\AdventureWorks2012",
                    ref testInput);
            Console.WriteLine(testInput);

        }

        [Test]
        public void TestRemoveRedundantPathLeafs()
        {
            var testInput = @"..\Ind.BusinessLogic.Provider\..\Ind.Lookup\Ind.Lookup_Bin.csproj";
            var testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual(@"..\Ind.Lookup\Ind.Lookup_Bin.csproj", testResult);

            testInput = @"..\SomeSubProj.WithDots.MoreDots\SomeSubProj.WithDots.MoreDots.fsproj";
            testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual(testInput, testResult);

            testInput = @"..\SomeSubProj.WithDots.MoreDots\.\SomeSubProj.WithDots.MoreDots.fsproj";
            testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual(@"..\SomeSubProj.WithDots.MoreDots\SomeSubProj.WithDots.MoreDots.fsproj", testResult);

            
            testInput = @"..\Bfw.Scheduling\Bfw.Scheduling.Scaling\..\..\Bfw.BusinessLogic.Manager.Contract\Bfw.BusinessLogic.Manager.Contract_Bin.csproj";
            testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            Assert.AreEqual(@"..\Bfw.BusinessLogic.Manager.Contract\Bfw.BusinessLogic.Manager.Contract_Bin.csproj", testResult);
        }

        [Test]
        public void TestSafeFilename()
        {
            var testInput = "\"A name in quotes\"";
            var testResult = NfPath.SafeFilename(testInput);

            Assert.AreEqual("A name in quotes", testResult);
        }

        [Test]
        public void TestTryResolveEnvVar()
        {
            var testEnvVar = "windir";
            var expectedResult = @"C:\Windows\Microsoft.NET\Framework";

            var testInput = "$(" + testEnvVar + ")" + @"\Microsoft.NET\Framework";
            var testResultOut = string.Empty;
            var testResult = NfPath.TryResolveEnvVar(testInput, ref testResultOut);

            Assert.IsTrue(testResult);
            Assert.IsTrue(string.Equals(expectedResult, testResultOut, StringComparison.OrdinalIgnoreCase));

            testInput = "$env:" + testEnvVar + @"\Microsoft.NET\Framework";
            testResultOut = string.Empty;
            testResult = NfPath.TryResolveEnvVar(testInput, ref testResultOut);

            Assert.IsTrue(testResult);
            Assert.IsTrue(string.Equals(expectedResult, testResultOut, StringComparison.OrdinalIgnoreCase));

            testInput = "%" + testEnvVar + "%" + @"\Microsoft.NET\Framework";
            testResultOut = string.Empty;
            testResult = NfPath.TryResolveEnvVar(testInput, ref testResultOut);

            Assert.IsTrue(testResult);
            Assert.IsTrue(string.Equals(expectedResult, testResultOut, StringComparison.OrdinalIgnoreCase));

            testResultOut = string.Empty;
            testResult = NfPath.TryResolveEnvVar(System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("windir"), @"\Microsoft.NET\Framework"), ref testResultOut);
            Assert.IsFalse(testResult);

        }

        [Test]
        public void TestHasKnownExtension()
        {
            var testResult =
                NfPath.HasKnownExtension(@"C:\Projects\MyProject\AnotherFolder\admin\SomeFile.fs");
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestIsCodeFileExtension()
        {
            Assert.IsTrue(NfPath.IsCodeFileExtension(".cs"));
            Assert.IsTrue(NfPath.IsCodeFileExtension("MyCode.cs"));
            Assert.IsTrue(NfPath.IsCodeFileExtension(@"C:\Projects\MyCodeFile.cs"));
        }

        [Test]
        public void TestContainsExcludeCodeDirectory()
        {
            Assert.IsTrue(NfPath.ContainsExcludeCodeDirectory(@"C:\Projects\Wanker\Services\WCF\WSXXX\bin\QA3"));
        }
    }
}

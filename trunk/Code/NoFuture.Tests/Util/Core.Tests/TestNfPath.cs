using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

        }

        [Test]
        public void TestRemoveRedundantPathLeafs()
        {
            var testInput = @"..\Ind.BusinessLogic.Provider\..\Ind.Lookup\Ind.Lookup_Bin.csproj";
            var testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
            //Assert.AreEqual(@"..\Ind.Lookup\Ind.Lookup_Bin.csproj", testResult);

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

        }

        [Test]
        public void TestSafeFilename()
        {
            var testInput = "\"A name in quotes\"";
            var testResult = NfPath.SafeFilename(testInput);

            Assert.AreEqual("A name in quotes", testResult);
        }

        [Test]
        public void TestSafeDirectoryName()
        {
            var testInput = "\"<A name in quotes>\"";
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
            
            testResultOut = string.Empty;
            testResult = NfPath.TryResolveEnvVar(@"%SystemRoot%\system32\WindowsPowerShell\v1.0\", ref testResultOut);
            Console.WriteLine(testResult);
            Console.WriteLine(testResultOut);

        }

        [Test]
        public void TestJoinLinesWhere()
        {
            var exampleFileContent = @"Id.
at 815.
The court went on to find that surrogate parenting agreements are not void, but are voidable if they are not in accordance with the state's adoption statutes.
Id.
at 817.
The court then upheld the payment of money in connection with the surrogacy arrangement on the ground that the New York Legislature did not contemplate surrogacy when the baby-selling statute was passed.
Id.
at 818.
";
            var testFilePath = Path.Combine(GetTestFileDirectory(), "TestJoinLinesWhere.txt");
            File.WriteAllText(testFilePath, exampleFileContent);
            System.Threading.Thread.Sleep(50);
            Assert.IsTrue(File.Exists(testFilePath));
            var testLinesCount = File.ReadAllLines(testFilePath).Length;

            Func<string, string, bool> testWith = (s1, s2) =>
            {
                s1 = (s1 ?? string.Empty).Trim();
                s2 = (s2 ?? string.Empty).Trim();
                if (new[] {s1, s2}.Any(string.IsNullOrWhiteSpace))
                    return false;

                var s1p = s1.EndsWith("Id.");
                var s2p = Regex.IsMatch(s2, "at\x20[0-9]");
                return s1p && s2p;
            };

            NoFuture.Util.Core.NfPath.JoinLinesWhere(testFilePath, testWith);

            var testLinesResultCount = File.ReadAllLines(testFilePath).Length;
            Assert.IsTrue(testLinesCount > testLinesResultCount);
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

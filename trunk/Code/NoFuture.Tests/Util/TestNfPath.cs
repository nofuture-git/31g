﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Util;
using NoFuture.Util.Core;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class TestNfPath
    {
        [TestMethod]
        public void TestConvertToCrLf()
        {
            var testFile = TestAssembly.UnitTestsRoot + @"\ExampleDlls\MixLineEndings.xml";
            File.Copy(testFile, Path.Combine(Path.GetDirectoryName(testFile),"MixLineEndings.copy.xml"));

            System.Threading.Thread.Sleep(500);
            NfPath.ConvertToCrLf(testFile);

        }

        [TestMethod]
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
            System.Diagnostics.Debug.WriteLine(testInput);

        }

        [TestMethod]
        public void TestRemoveRedundantPathLeafs()
        {
            var testInput = @"..\Ind.BusinessLogic.Provider\..\Ind.Lookup\Ind.Lookup_Bin.csproj";
            var testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(@"..\Ind.Lookup\Ind.Lookup_Bin.csproj", testResult);

            testInput = @"..\SomeSubProj.WithDots.MoreDots\SomeSubProj.WithDots.MoreDots.fsproj";
            testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(testInput, testResult);

            testInput = @"..\SomeSubProj.WithDots.MoreDots\.\SomeSubProj.WithDots.MoreDots.fsproj";
            testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(@"..\SomeSubProj.WithDots.MoreDots\SomeSubProj.WithDots.MoreDots.fsproj", testResult);

            
            testInput = @"..\Bfw.Scheduling\Bfw.Scheduling.Scaling\..\..\Bfw.BusinessLogic.Manager.Contract\Bfw.BusinessLogic.Manager.Contract_Bin.csproj";
            testResult = NfPath.RemoveRedundantPathLeafs(testInput);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual(@"..\Bfw.BusinessLogic.Manager.Contract\Bfw.BusinessLogic.Manager.Contract_Bin.csproj", testResult);
        }

        [TestMethod]
        public void TestBreakUpFileOverMaxJsonLength()
        {
            var testPath = TestAssembly.UnitTestsRoot + @"\Util\TestChunkData\diaSdkData.lines.json";// 

            var testResults = NfPath.TrySplitFileOnMarker(testPath, null);//5762048
        }

        [TestMethod]
        public void TestSafeFilename()
        {
            var testInput = "\"A name in quotes\"";
            var testResult = NfPath.SafeFilename(testInput);

            Assert.AreEqual("A name in quotes", testResult);
        }

        [TestMethod]
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

        [TestMethod]
        public void TestHasKnownExtension()
        {
            var testResult =
                NfPath.HasKnownExtension(@"C:\Projects\MyProject\AnotherFolder\admin\SomeFile.fs");
            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void TestIsCodeFileExtension()
        {
            Assert.IsTrue(NfPath.IsCodeFileExtension(".cs"));
            Assert.IsTrue(NfPath.IsCodeFileExtension("MyCode.cs"));
            Assert.IsTrue(NfPath.IsCodeFileExtension(@"C:\Projects\MyCodeFile.cs"));
        }

        [TestMethod]
        public void TestContainsExcludeCodeDirectory()
        {
            Assert.IsTrue(NfPath.ContainsExcludeCodeDirectory(@"C:\Projects\Wanker\Services\WCF\WSXXX\bin\QA3"));
        }
    }
}

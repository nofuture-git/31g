﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using NUnit.Framework;
using NoFuture.Read;
using NoFuture.Shared.Cfg;
using NoFuture.Shared.Core;

namespace NoFuture.Tests
{
    [TestFixture]
    public class TestRead
    {
        public string TEST_CSPROJ = TestAssembly.UnitTestsRoot + @"\ExampleDlls\AdventureWorks2012\AdventureWorks2012\DoNotUse_VsProjFileTests.csproj";
        public string TEST00_EXE_CONFIG = TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig00-Copy.xml";
        public string TEST01_EXE_CONFIG = TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig01-Copy.xml";
        public string TEST02_EXE_CONFIG = TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig02-Copy.xml";
        public string TEST03_EXE_CONFIG = TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig03-Copy.xml";

        public string TEST_TRANS00_CONFIG =
           TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestTransformConfig00-Copy.xml";

        public string[] RemoveFiles = new[] { "SeeReadMe.aspx", "SeeReadMe.aspx.cs", "SeeReadMe.aspx.designer.cs" };

        public string[] AddFiles = new[] { "Cw.aspx", "Cw.aspx.cs", "Cw.aspx.designer.cs" };

        public Hashtable Regex2Values;

        [SetUp]
        public void Init()
        {
            InitConfigs();
        }

        public void InitConfigs()
        {
            var regexCatalog = new RegexCatalog();
            Regex2Values = new Hashtable
            {
                {regexCatalog.WindowsRootedPath, @"C:\Projects\"},
                {
                    regexCatalog.EmailAddress,
                    string.Format("{0}@myDomain.net", System.Environment.GetEnvironmentVariable("USERNAME"))
                },
                {regexCatalog.IPv4, "127.0.0.1"},
                {regexCatalog.UrlClassicAmerican, "localhost"}
            };
            File.Copy(TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig00.xml", TEST00_EXE_CONFIG, true);
            File.Copy(TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig01.xml", TEST01_EXE_CONFIG, true);
            File.Copy(TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig02.xml", TEST02_EXE_CONFIG, true);
            File.Copy(TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestAppConfig03.xml", TEST03_EXE_CONFIG, true);
            File.Copy(TestAssembly.UnitTestsRoot + @"\ExampleDlls\TestTransformConfig00.xml", TEST_TRANS00_CONFIG, true);

            System.Threading.Thread.Sleep(500);
        }

        [Test]
        public void TestSwapAllProjRef2BinRef()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.SwapAllProjRef2BinRef(true);
            Assert.AreNotEqual(0, testResults);

            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);
        }

        [Test]
        public void TestUpdateHintPathTo()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.UpdateHintPathTo(@"C:\Projects\31g\trunk\bin");
            Assert.AreNotEqual(0, testResults);
            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);
        }

        [Test]
        public void TestDropNodesNamed()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.DropNodesNamed("VisualStudio");
            Assert.AreNotEqual(0, testResults);

            testResults = testSubject.DropNodesNamed("VisualStudio");
            Assert.AreEqual(0, testResults);

            testResults = testSubject.DropNodesNamed("UseIISExpress");
            Assert.AreNotEqual(0, testResults);
            testResults = testSubject.DropNodesNamed("IISExpressSSLPort");
            Assert.AreNotEqual(0, testResults);
            testResults = testSubject.DropNodesNamed("IISExpressWindowsAuthentication");
            Assert.AreNotEqual(0, testResults);
            testResults = testSubject.DropNodesNamed("IISExpressUseClassicPipelineMode");
            Assert.AreNotEqual(0, testResults);
            testResults = testSubject.DropNodesNamed("IISExpressAnonymousAuthentication");
            Assert.AreNotEqual(0, testResults);
            testResults = testSubject.DropNodesNamed("WcfConfigValidationEnabled");
            Assert.AreNotEqual(0, testResults);

            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);

        }

        [Test]
        public void TestVsProjectTypeGuids()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.VsProjectTypeGuids;
            Assert.AreNotEqual("{349c5851-65df-11da-9384-00065b846f21};{fae04ec0-301f-11d3-bf4b-00c04f79efbc}", testResults);
            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);

        }

        [Test]
        public void TestGetBinRefByGuidComment()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResult = testSubject.GetRefNodeByGuidComment("FB851E8C-6995-4EEB-9B4D-B7C450C39E5C");
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestReduceToOnlyBuildConfig()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.ReduceToOnlyBuildConfig(null);
            Assert.AreNotEqual(0, testResults);

            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);
        }

        [Test]
        public void TestGetListCompileItemNodes()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.GetListCompileItemNodes("HumanResources.");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

        }

        [Test]
        public void TestGetSlnProjectEntry()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResult = NoFuture.Read.Vs.SlnFile.GetSlnProjectEntry(testSubject, TestAssembly.UnitTestsRoot);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);
        }

        [Test]
        public void TestProjectConfigurations()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.ProjectConfigurations;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0,testResults);
            foreach(var t in testResults)
                Console.WriteLine(t);
        }

        [Test]
        public void TestProjectGuid()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResult = testSubject.ProjectGuid;
            Assert.AreEqual("{F5D2874B-B6AB-4FB9-ABF4-0F8B613907D4}", testResult);

        }

        [Test]
        public void TestSaveSlnVer14()
        {
            var testInput = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testInput);

            var testResultFile = Path.GetDirectoryName(TEST_CSPROJ);
            testResultFile = Path.Combine(testResultFile, "TestSaveSlnVer14.sln");

            var testSubject = new NoFuture.Read.Vs.SlnFile(testResultFile);

            var testResult = testSubject.SaveSlnVer14(new[] {testInput});
            Assert.AreNotEqual(0, testResult);

        }

        [Test]
        public void TestGetNet40ProjFile()
        {
            //test file is present
            Assert.IsTrue(File.Exists(TEST_CSPROJ));

            var testDir = Path.GetDirectoryName(TEST_CSPROJ);
            //make a copy of it
            var copyOfTestFile = Path.Combine(testDir, (Path.GetFileNameWithoutExtension(TEST_CSPROJ) + "_Copy.csproj"));
            File.Copy(TEST_CSPROJ, copyOfTestFile, true);

            Thread.Sleep(1000);

            foreach (var remFile in RemoveFiles)
            {
                if(!File.Exists(Path.Combine(testDir, remFile)))
                    File.AppendAllText(Path.Combine(testDir, remFile), "");
            }

            foreach (var addFile in AddFiles)
            {

                if(!File.Exists(Path.Combine(testDir, addFile)))
                    File.AppendAllText(Path.Combine(testDir, addFile), "");
            }

            //assert copy is present
            Assert.IsTrue(File.Exists(copyOfTestFile));

            //get test subject
            var testSubject = new NoFuture.Read.Vs.ProjFile(copyOfTestFile);
            Assert.IsNotNull(testSubject);

            var testProjRef =
                testSubject.GetBinRefByAsmPath(
                    TestAssembly.UnitTestsRoot + @"\ExampleDlls\NHibernate.dll");

            Assert.IsNotNull(testProjRef);
            Assert.IsNotNull(testProjRef.SearchPath);
            Assert.IsNotNull(testProjRef.DllOnDisk);
            Assert.IsNotNull(testProjRef.DllOnDisk.Item2);
            Assert.IsNotNull(testProjRef.HintPath);

            var testCompileItem = testSubject.GetSingleCompileItemNode("SeeReadMe.aspx.cs");
            Assert.IsNotNull(testCompileItem);

            var testContentItem = testSubject.GetSingleContentItemNode("SeeReadMe.aspx");
            Assert.IsNotNull(testContentItem);

            Assert.IsTrue(testSubject.HasExistingIncludeAttr(testCompileItem));

            var testChildNode = testSubject.FirstChildNamed(testCompileItem, "DependentUpon");
            Assert.IsNotNull(testChildNode);

            var testLastCompileItem = testSubject.GetLastCompileItem();
            Assert.IsNotNull(testLastCompileItem);

            var testLastContentItem = testSubject.GetLastContentItem();
            Assert.IsNotNull(testLastContentItem);

            var remItemsOut = new List<string>();
            var testRemove = testSubject.TryRemoveSrcCodeFile("SeeReadMe.aspx", remItemsOut);
            Assert.IsTrue(testRemove);

            var testAdd = testSubject.TryAddSrcCodeFile("Cw.aspx");
            Assert.IsTrue(testAdd);

            var testNewRef = testSubject.AddReferenceNode(TestAssembly.UnitTestsRoot + @"\ExampleDlls\NoFuture.Shared.dll");
            Assert.IsTrue(testNewRef);

            testSubject.Save();

        }

        [Test]
        public void TestExeConfigTransforms00()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST00_EXE_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [Test]
        public void TestExeConfigTransforms01()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST01_EXE_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [Test]
        public void TestExeConfigTransforms02()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST02_EXE_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [Test]
        public void TestExeConfigReadTransform()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST_TRANS00_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [Test]
        public void TestAddAppSettingItem00()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST00_EXE_CONFIG);
            testSubject.AddAppSettingItem("myKey", "myValue", "this is really important");
            testSubject.Save();
        }

        [Test]
        public void TestAddAppSettingItem01()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST03_EXE_CONFIG);//no appSettings node
            testSubject.AddAppSettingItem("myKey", "myValue", "this is really important");
            testSubject.Save();
        }

        [Test]
        public void TestSpliceInXmlNs()
        {
            var testInput = "//docNode/sectionNode/listNode/itemNode[someAttr='applesauce']";

            var testResult = BaseXmlDoc.SpliceInXmlNs(testInput, "Wz");

            Console.WriteLine(testResult);
            Assert.AreEqual("//Wz:docNode/Wz:sectionNode/Wz:listNode/Wz:itemNode[someAttr='applesauce']", testResult);
        }

        [Test]
        public void TestAddNugetPkgRestoreNodes()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.AddNugetPkgRestoreNodes();
            Assert.AreNotEqual(0, testResults);

            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);
        }

        [Test]
        public void TestGetProjectReferences()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(@"C:\Projects\We\source\Bfw.Dal.Transform\Bfw.Dal.Transform.csproj");
            Assert.IsNotNull(testSubject);

            var bldScript = "";
            var testResult = testSubject.GetProjectReferences(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);


            while (testResult.Count > 0)
            {
                var testResultI = testResult.Dequeue();
                Console.WriteLine(testResultI.AssemblyName);
            }

            Console.WriteLine(bldScript);
        }

        [Test]
        public void TestGetCompileItems()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.CompileItems;
            Assert.IsNotNull(testResults);

            Assert.IsTrue(testResults.Any());

            foreach (var t in testResults)
            {
                Console.WriteLine(t);
            }
        }

        [Test]
        public void TestGetContentItems()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.ContentItems;
            Assert.IsNotNull(testResults);

            Assert.IsTrue(testResults.Any());

            foreach (var t in testResults)
            {
                Console.WriteLine(t);
            }
        }

        [Test]
        public void TestGetBinaryReferences()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.BinaryReferences;
            Assert.IsNotNull(testResults);

            Assert.IsTrue(testResults.Any());

            foreach (var t in testResults)
            {
                Console.WriteLine(t);
            }
        }

        [Test]
        public void TestGetAsCompileCmd()
        {
            NfConfig.DotNet.CscCompiler =
                @"C:\Projects\31g\trunk\bin\roslyn-master\Binaries\Release\Exes\csc\net46\csc.exe";
            var testResult = NoFuture.Read.Vs.ProjFile.GetAsPsCompileCmd(TEST_CSPROJ, null, false);
            Assert.IsNotNull(testResult);
            System.IO.File.WriteAllLines(TestAssembly.UnitTestsRoot + @"\TestGetAsCompileCmd.txt", testResult);


        }
    }
}

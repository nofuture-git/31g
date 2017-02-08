using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Read;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace NoFuture.Tests
{
    [TestClass]
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

        [TestInitialize]
        public void Init()
        {
            //InitConfigs();
        }

        public void InitConfigs()
        {
            var regexCatalog = new NoFuture.Shared.RegexCatalog();
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

        [TestMethod]
        public void TestTryReplaceToBinaryRef()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.TryReplaceToBinaryRef(true);
            Assert.AreNotEqual(0, testResults);

            var testRslt = Path.GetDirectoryName(TEST_CSPROJ);
            testRslt = Path.Combine(testRslt, "DoNotUse_VsProjFileTests-COPY.csproj");
            testSubject.SaveAs(testRslt);
        }

        [TestMethod]
        public void TestGetBinRefByGuidComment()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResult = testSubject.GetRefNodeByGuidComment("FB851E8C-6995-4EEB-9B4D-B7C450C39E5C");
            Assert.IsNotNull(testResult);
        }

        [TestMethod]
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

        [TestMethod]
        public void TestGetListCompileItemNodes()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.GetListCompileItemNodes("HumanResources.");
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0, testResults.Count);

        }

        [TestMethod]
        public void TestGetSlnProjectEntry()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResult = NoFuture.Read.Vs.SlnFile.GetSlnProjectEntry(testSubject, TestAssembly.UnitTestsRoot);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }

        [TestMethod]
        public void TestProjectConfigurations()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResults = testSubject.ProjectConfigurations;
            Assert.IsNotNull(testResults);
            Assert.AreNotEqual(0,testResults);
            foreach(var t in testResults)
                System.Diagnostics.Debug.WriteLine(t);
        }

        [TestMethod]
        public void TestProjectGuid()
        {
            var testSubject = new NoFuture.Read.Vs.ProjFile(TEST_CSPROJ);
            Assert.IsNotNull(testSubject);

            var testResult = testSubject.ProjectGuid;
            Assert.AreEqual("{F5D2874B-B6AB-4FB9-ABF4-0F8B613907D4}", testResult);

        }

        [TestMethod]
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

        [TestMethod]
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
            Assert.IsNotNull(testProjRef.AssemblyFullName);
            Assert.IsNotNull(testProjRef.AssemblyName);
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

            var testNewRef = testSubject.TryAddReferenceEntry(TestAssembly.UnitTestsRoot + @"\ExampleDlls\NoFuture.Shared.dll");
            Assert.IsTrue(testNewRef);

            testSubject.Save();

        }

        [TestMethod]
        public void TestExeConfigTransforms00()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST00_EXE_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [TestMethod]
        public void TestExeConfigTransforms01()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST01_EXE_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [TestMethod]
        public void TestExeConfigTransforms02()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST02_EXE_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));
        }

        [TestMethod]
        public void TestExeConfigReadTransform()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST_TRANS00_CONFIG);

            var testResult = testSubject.SplitAndSave(Regex2Values, true);

            Assert.IsNotNull(testResult);
            Assert.IsTrue(System.IO.File.Exists(testResult));

        }
        [TestMethod]
        public void TestAddAppSettingItem00()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST00_EXE_CONFIG);
            testSubject.AddAppSettingItem("myKey", "myValue", "this is really important");
            testSubject.Save();
        }

        [TestMethod]
        public void TestAddAppSettingItem01()
        {
            var testSubject = new NoFuture.Read.Config.ExeConfig(TEST03_EXE_CONFIG);//no appSettings node
            testSubject.AddAppSettingItem("myKey", "myValue", "this is really important");
            testSubject.Save();
        }

        [TestMethod]
        public void TestSpliceInXmlNs()
        {
            var testInput = "//docNode/sectionNode/listNode/itemNode[someAttr='applesauce']";

            var testResult = BaseXmlDoc.SpliceInXmlNs(testInput, "Wz");

            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.AreEqual("//Wz:docNode/Wz:sectionNode/Wz:listNode/Wz:itemNode[someAttr='applesauce']", testResult);
        }
    }
}

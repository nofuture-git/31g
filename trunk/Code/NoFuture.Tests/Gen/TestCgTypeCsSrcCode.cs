using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NoFuture.Shared.Cfg;
using NoFuture.Util.Binary;
using NUnit.Framework;

namespace NoFuture.Gen.Tests
{
    [TestFixture]
    public class TestCgTypeSrcCode
    {

        [SetUp]
        public void Init()
        {
            //put all the binary files in test dir
            TestCgType.GetAdventureWorks2012();
            NfConfig.CustomTools.Dia2Dump = TestCgType.PutTestFileOnDisk("Dia2Dump.exe");
            NoFuture.Shared.Cfg.NfConfig.TempDirectories.Debug = TestCgType.GetTestFileDirectory();

        }
        [Test]
        public void TestCtor()
        {
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.Production.Product";
            var testAsm = TestCgType.GetAdventureWorks2012();
            Assert.IsNotNull(testAsm);

            var testResult = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName);
        }

        [Test]
        public void TestCtor2()
        {
            FxPointers.AddResolveAsmEventHandlerToDomain();
            var testtypeName = "AdventureWorks.Production.Product";
            var testAsm = TestCgType.GetAdventureWorks2012();

            var testFile = TestCgType.PutTestFileOnDisk("Production_Product.eg");
            Assert.IsNotNull(testAsm);
            Assert.IsTrue(File.Exists(testFile));

            var testResult = new NoFuture.Gen.CgTypeCsSrcCode(testAsm, testtypeName, testFile);
            Assert.IsNotNull(testResult.CgType);
            Assert.IsTrue(testResult.CgType.Properties.Any(p => p.GetMyStartEnclosure(null) != null));

        }

        [Test]
        public void TestFilterOutLinesNotInMethods()
        {
            var testInputAffrim = new List<Tuple<int, int>>
            {
                new Tuple<int, int>(11, 32),
                new Tuple<int, int>(36, 94),
                new Tuple<int, int>(99, 108),
                new Tuple<int, int>(116, 205)
            };

            var testInputLines = new[] { 13, 94, 112, 157 };
            var testResult = NoFuture.Gen.CgTypeCsSrcCode.FilterOutLinesNotInMethods(testInputLines, testInputAffrim);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
            Assert.AreEqual(3, testResult.Length);
            Assert.IsTrue(testResult.Contains(13));
            Assert.IsTrue(testResult.Contains(94));
            Assert.IsTrue(testResult.Contains(157));

        }
    }
}

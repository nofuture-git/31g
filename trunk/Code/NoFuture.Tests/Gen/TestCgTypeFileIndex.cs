using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Gen;

namespace NoFuture.Tests.Gen
{
    [TestClass]
    public class TestCgTypeFileIndex
    {
        private const string TestOutputDir = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen";
        [TestMethod]
        public void TestWriteIndexToFile()
        {
            var testSubject = new NoFuture.Gen.CgTypeFileIndex()
            {
                AssemblyQualifiedTypeName =
                    "System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                OriginalSrcCodeFile = @"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\OverloadedMethods.cs"
            };

            var pdb00 = new PdbTargetLine
            {
                EndAt = 20,
                StartAt = 17,
                IndexId = 1,
                MemberName = "OvMethod",
                OwningTypeFullName = "DiaSdkTestOverloadedMethods.OverloadedMethods"
            };

            var pdb01 = new PdbTargetLine
            {
                EndAt = 29,
                StartAt = 24,
                IndexId = 3,
                MemberName = "OvMethod",
                OwningTypeFullName = "DiaSdkTestOverloadedMethods.OverloadedMethods"
            };

            var pdb02 = new PdbTargetLine
            {
                EndAt = 40,
                StartAt = 33,
                IndexId = 4,
                MemberName = "OvMethod",
                OwningTypeFullName = "DiaSdkTestOverloadedMethods.OverloadedMethods"
            };

            var pdb03 = new PdbTargetLine
            {
                EndAt = 50,
                StartAt = 44,
                IndexId = 5,
                MemberName = "OvMethod",
                OwningTypeFullName = "DiaSdkTestOverloadedMethods.OverloadedMethods"
            };
            testSubject.PdbFilesHash.Add(pdb00.GetHashCodeAsString(), pdb00);
            testSubject.PdbFilesHash.Add(pdb01.GetHashCodeAsString(), pdb01);
            testSubject.PdbFilesHash.Add(pdb02.GetHashCodeAsString(), pdb02);
            testSubject.PdbFilesHash.Add(pdb03.GetHashCodeAsString(), pdb03);

            testSubject.WriteIndexToFile(TestOutputDir);

            Assert.IsTrue(File.Exists(Path.Combine(TestOutputDir, NoFuture.Gen.Settings.DefaultFileIndexName)));
        }

        [TestMethod]
        public void TestReadIndexFromFile()
        {
            var testXmlContent = @"<?xml version='1.0' encoding='UTF-8'?>
<noFuture>
  <gen>
    <cgTypeFileIndex>
      <originalSrcCodeFile>C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\OverloadedMethods.cs</originalSrcCodeFile>
      <assemblyQualifiedTypeName>System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</assemblyQualifiedTypeName>
      <pdbFilesHash>
        <entry id='343035373832353334'>
          <pdbTargetLine indexId='1' startAt='17' endAt='20'><![CDATA[OvMethod]]></pdbTargetLine>
        </entry>
        <entry id='343035373832353532'>
          <pdbTargetLine indexId='3' startAt='24' endAt='29'><![CDATA[OvMethod]]></pdbTargetLine>
        </entry>
        <entry id='343035373832353733'>
          <pdbTargetLine indexId='4' startAt='33' endAt='40'><![CDATA[OvMethod]]></pdbTargetLine>
        </entry>
        <entry id='343035373832353935'>
          <pdbTargetLine indexId='5' startAt='44' endAt='50'><![CDATA[OvMethod]]></pdbTargetLine>
        </entry>
      </pdbFilesHash>
    </cgTypeFileIndex>
  </gen>
</noFuture>
";
            var testFile = Path.Combine(TestOutputDir, "testReadIndexFromFile.xml");
            File.WriteAllText(testFile, testXmlContent);

            var testResult = NoFuture.Gen.CgTypeFileIndex.ReadIndexFromFile(testFile);
            Assert.IsNotNull(testResult);
            Assert.AreEqual(@"C:\Projects\31g\trunk\Code\NoFuture.Tests\Gen\OverloadedMethods.cs", testResult.OriginalSrcCodeFile);
            Assert.AreEqual("System.String, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089", testResult.AssemblyQualifiedTypeName);
            Assert.AreNotEqual(0, testResult.PdbFilesHash.Count);
            Assert.AreEqual(4,testResult.PdbFilesHash.Count);


        }
    }
}

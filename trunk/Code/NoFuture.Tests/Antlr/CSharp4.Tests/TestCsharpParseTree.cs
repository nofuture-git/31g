using System;
using System.IO;
using System.Reflection;
using NoFuture.Antlr.CSharp;
using NUnit.Framework;

namespace NoFuture.Antlr.CSharp.Tests
{
    [TestFixture]
    public class TestCsharpParseTree
    {
        [Test]
        public void TestInvokeParse()
        {
            var testFile = PutTestFileOnDisk("Class00.eg");
            if(!File.Exists(testFile))
                Assert.Fail($"missing test file at {testFile}");

            var testResult =
                NoFuture.Antlr.CSharp.CsharpParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            //Assert.AreNotEqual(0, testResult.ClassNames.Count);
            Assert.AreNotEqual(0, testResult.MethodNames);

            foreach(var n in testResult.TypeNames)
                Console.WriteLine(n);
        }


        [Test]
        public void TestCsharpParse()
        {
            var testFile = PutTestFileOnDisk("SimpleExample.eg");

            Assert.IsTrue(System.IO.File.Exists(testFile));

            var testResult = CsharpParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.CatchBlocks);
            Assert.AreNotEqual(0, testResult.CatchBlocks.Count);

            foreach (var c in testResult.CatchBlocks)
            {
                Console.WriteLine(c);
            }
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

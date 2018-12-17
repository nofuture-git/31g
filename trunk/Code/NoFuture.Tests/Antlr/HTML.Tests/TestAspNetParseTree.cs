using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Antlr.HTML.Tests
{
    [TestFixture]
    public class TestAspNetParseTree
    {
        [Test]
        public void TestPoc()
        {
            var testFile = PutTestFileOnDisk("AccountEdit_aspx.eg");

            Assert.IsTrue(System.IO.File.Exists(testFile));

            var testResult = AspNetParseTree.InvokeParse(testFile);

            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Tags2Attrs);
            Assert.AreNotEqual(0,testResult.Tags2Attrs.Keys.Count);
            foreach (var key in testResult.Tags2Attrs.Keys)
            {
                //var attrs = string.Join("|", testResult.DistinctTags[key]);
                Console.WriteLine(key);
            }

            Assert.IsNotNull(testResult.ScriptBodies);
            Assert.AreNotEqual(0,testResult.ScriptBodies.Count);
            foreach (var script in testResult.ScriptBodies)
            {
                Console.WriteLine(script);
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

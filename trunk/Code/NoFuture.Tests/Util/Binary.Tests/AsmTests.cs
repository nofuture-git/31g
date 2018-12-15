using System;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using NUnit.Framework;

namespace NoFuture.Util.Binary.Tests
{
    [TestFixture]
    public class AsmTests
    {
        public static string PutTestFileOnDisk(string embeddedFileName)
        {
            //need this to be another object each time and not just another reference
            var asmName = Assembly.GetExecutingAssembly().GetName().Name;
            var liSteam = Assembly.GetExecutingAssembly().GetManifestResourceStream($"{asmName}.{embeddedFileName}");
            if (liSteam == null)
            {
                Assert.Fail($"Cannot find the embedded file {embeddedFileName}");
            }

            string fileContent = null;
            using (var txtSr = new StreamReader(liSteam))
            {
                fileContent = txtSr.ReadToEnd();
            }
            Assert.IsNotNull(fileContent);

            var nfAppData = GetTestFileDirectory();
            nfAppData = Path.Combine(nfAppData, "NoFuture.Tests");
            if (!Directory.Exists(nfAppData))
            {
                Directory.CreateDirectory(nfAppData);
            }

            var fileOnDisk = Path.Combine(nfAppData, embeddedFileName);
            File.WriteAllText(fileOnDisk, fileContent);
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

        [Test]
        public void TestMethod1()
        {
            var testInputFile = PutTestFileOnDisk(@"MethodBodyIl.bin");

            var testInput = System.IO.File.ReadAllBytes(testInputFile);

            var testResult = NoFuture.Util.Binary.Asm.GetOpCodesArgs(testInput, new[] { OpCodes.Callvirt, OpCodes.Call, OpCodes.Ldftn });

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            foreach (var t in testResult)
                Console.WriteLine(t.ToString("X4"));
        }
    }
}

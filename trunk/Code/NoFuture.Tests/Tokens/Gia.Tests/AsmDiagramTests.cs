using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NoFuture.Tokens.Gia.GraphViz;
using NUnit.Framework;

namespace NoFuture.Tokens.Gia.Tests
{
    [TestFixture]
    public class AsmDiagramTests
    {
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


        private Assembly GetTestAsm()
        {
            var testAsm =
                AppDomain.CurrentDomain.GetAssemblies()
                    .FirstOrDefault(x => x.GetName().FullName.StartsWith("AdventureWorks"));

            if (testAsm == null)
            {
                Assembly.Load(File.ReadAllBytes(PutTestFileOnDisk("ThirdDll.dll")));
                Assembly.Load(File.ReadAllBytes(PutTestFileOnDisk("SomethingShared.dll")));
                Assembly.Load(File.ReadAllBytes(PutTestFileOnDisk("SomeSecondDll.dll")));
                testAsm = Assembly.Load(File.ReadAllBytes(PutTestFileOnDisk("AdventureWorks2012.dll")));
            }
            NoFuture.Shared.Cfg.NfConfig.AssemblySearchPaths.Add(GetTestFileDirectory());
            NoFuture.Util.Binary.FxPointers.AddResolveAsmEventHandlerToDomain();

            return testAsm;
        }

        [Test]
        public void TestGetAsm()
        {
            var testAsm = GetTestAsm();
            Assert.IsNotNull(testAsm);
        }

        [Test]
        public void TestCtor()
        {

            var testAsm = GetTestAsm();
            Assert.IsNotNull(testAsm);
            var testSubject = new AsmDiagram(testAsm);
            
            var testResult = testSubject.ToGraphVizString();

            Assert.IsNotNull(testResult);

            System.IO.File.WriteAllText(GetTestFileDirectory() + @"\AsmDiagramTest.gv", testResult);
        }
    }
}

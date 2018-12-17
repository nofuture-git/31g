using System;
using System.IO;
using System.Reflection;
using NUnit.Framework;

namespace NoFuture.Hbm.Tests
{
    [SetUpFixture]
    public sealed class TestAssembly
    {
        [OneTimeSetUp]
        public static void AssemblyInitialize()
        {
            var rootTestsDir = GetTestFileDirectory();
            var localhost = Path.Combine(rootTestsDir, "localhost");
            var whatever = Path.Combine(localhost, "Whatever");
            var storedProxDir = Path.Combine(whatever, "StoredProc");
            if (!Directory.Exists(localhost))
                Directory.CreateDirectory(localhost);
            if (!Directory.Exists(whatever))
                Directory.CreateDirectory(whatever);
            if (!Directory.Exists(storedProxDir))
                Directory.CreateDirectory(storedProxDir);


            PutTestFileOnDisk("dbo_MyProcUsingMyUdtTable_xsd.eg", Path.Combine(storedProxDir, "dbo.MyProcUsingMyUdtTable.xsd"));
            PutTestFileOnDisk("Dbo_MyStoredProc_hbm.xml", Path.Combine(whatever, "Dbo.MyStoredProc.hbm.xml"));
            PutTestFileOnDisk("dbo_MyStoredProc_xsd.eg", Path.Combine(storedProxDir, "dbo.MyStoredProc.xsd"));
            PutTestFileOnDisk("Dbo_TableWithCompositePk_hbm.xml", Path.Combine(whatever, "Dbo.TableWithCompositePk.hbm.xml"));
            PutTestFileOnDisk("Dbo_TableWithVarcharPk_hbm.xml", Path.Combine(whatever, "Dbo.TableWithVarcharPk.hbm.xml"));
            PutTestFileOnDisk("localhost_Whatever_allIndex.json", Path.Combine(whatever, "localhost.Whatever.allIndex.json"));
            PutTestFileOnDisk("localhost_Whatever_allkeys.json", Path.Combine(whatever, "localhost.Whatever.allkeys.json"));
            PutTestFileOnDisk("localhost_Whatever_autoId.json", Path.Combine(whatever, "localhost.Whatever.autoId.json"));
            PutTestFileOnDisk("localhost_Whatever_bags_hbm.json", Path.Combine(whatever, "localhost.Whatever.bags.hbm.json"));
            PutTestFileOnDisk("localhost_Whatever_cols.json", Path.Combine(whatever, "localhost.Whatever.cols.json"));
            PutTestFileOnDisk("localhost_Whatever_constraints.json", Path.Combine(whatever, "localhost.Whatever.constraints.json"));
            PutTestFileOnDisk("localhost_Whatever_fk_hbm.json", Path.Combine(whatever, "localhost.Whatever.fk.hbm.json"));
            PutTestFileOnDisk("localhost_Whatever_flatData.json", Path.Combine(whatever, "localhost.Whatever.flatData.json"));
            PutTestFileOnDisk("localhost_Whatever_pk.json", Path.Combine(whatever, "localhost.Whatever.pk.json"));
            PutTestFileOnDisk("localhost_Whatever_pk_hbm.json", Path.Combine(whatever, "localhost.Whatever.pk.hbm.json"));
            PutTestFileOnDisk("localhost_Whatever_sp.json", Path.Combine(whatever, "localhost.Whatever.sp.json"));
            PutTestFileOnDisk("localhost_Whatever_uqIdx.json", Path.Combine(whatever, "localhost.Whatever.uqIdx.json"));

        }

        public static string PutTestFileOnDisk(string embeddedFileName, string outputFileName = null)
        {
            var nfAppData = GetTestFileDirectory();
            var fileOnDisk = outputFileName ?? Path.Combine(nfAppData, embeddedFileName);
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

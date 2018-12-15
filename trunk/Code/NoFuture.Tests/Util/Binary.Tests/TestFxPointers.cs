using System;
using System.Diagnostics;
using System.Linq;
using NoFuture.Shared.Cfg;
using NUnit.Framework;

namespace NoFuture.Util.Binary.Tests
{
    [TestFixture]
    public class TestFxPointers
    {
        [Test]
        public void TestResolveAssemblyEventHandler_BadAsmName()
        {
            var arg = new ResolveEventArgs("this-is-not an assembly name",
                AppDomain.CurrentDomain.GetAssemblies().First());

            Assert.Throws<System.IO.FileNotFoundException>(() => FxPointers.ResolveReflectionOnlyAssembly(new object(), arg));
        }

        [Test]
        public void TestResolveAssembly_CoreAsm()
        {
            var asmArg = AppDomain.CurrentDomain.GetAssemblies().First();
            Console.WriteLine(asmArg.Location);
            var arg =
                new ResolveEventArgs(
                    "System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089",
                    AppDomain.CurrentDomain.GetAssemblies().First());
            var testResult = FxPointers.ResolveReflectionOnlyAssembly(new object(), arg);
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestResolveAssembly_InAssemblySearchPath()
        {
            //TODO have to run twice at least 
            MakeSomeAssemblies();
            var sdir = AsmTests.GetTestFileDirectory();
            NfConfig.AssemblySearchPaths.Add(sdir);
            var testAsm2 =
                System.Reflection.Assembly.ReflectionOnlyLoadFrom(System.IO.Path.Combine(sdir, "TestAsm2.dll"));
            var arg = new ResolveEventArgs("TestAsm, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", testAsm2);
            var testResult = FxPointers.ResolveReflectionOnlyAssembly(new object(), arg);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("TestAsm, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null", testResult.FullName);
        }

        public static void MakeSomeAssemblies()
        {

            var tempDirEv = AsmTests.GetTestFileDirectory();
            if (string.IsNullOrWhiteSpace(tempDirEv) || !System.IO.Directory.Exists(tempDirEv))
                System.IO.Directory.CreateDirectory(AsmTests.GetTestFileDirectory());

            var testAsmCsFile = System.IO.Path.Combine(AsmTests.GetTestFileDirectory(), "TestAsm.cs");
            var testAsmCs2File = System.IO.Path.Combine(AsmTests.GetTestFileDirectory(), "TestAsm2.cs");
            if (System.IO.File.Exists(System.IO.Path.ChangeExtension(testAsmCsFile, "dll")) &&
                System.IO.File.Exists(System.IO.Path.ChangeExtension(testAsmCs2File, "dll")))
                return;
            

            var testAsmCs = @"
namespace TestAsm
{
	public class TestClass{
		public string TestProperty {get; set;}
	}
}
";
            var testAsmCs2 = @"
namespace TestAsm2
{
	public class TestClass2{
		public string TestProperty2 {get; set;}
	}
}
";
            System.IO.File.WriteAllText(testAsmCsFile, testAsmCs);
            System.IO.File.WriteAllText(testAsmCs2File, testAsmCs2);

            var cscExe = new System.Diagnostics.Process
            {
                StartInfo = new ProcessStartInfo(@"C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319\csc.exe")
                {
                    Arguments =
                        string.Format(@" /target:library /out:{0} {1}",
                            System.IO.Path.ChangeExtension(testAsmCsFile, "dll"),
                            testAsmCsFile),
                    UseShellExecute = true

                }
               
            };

            cscExe.Start();

            while (!cscExe.HasExited)
            {
                System.Threading.Thread.Sleep(5000);
            }

            cscExe.StartInfo.Arguments = string.Format(@" /target:library /out:{0} {1}",
                System.IO.Path.ChangeExtension(testAsmCs2File, "dll"),
                testAsmCs2File);

            cscExe.Start();
        }
    }
}

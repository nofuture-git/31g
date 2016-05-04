using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Util.NfConsole;

namespace NoFuture.Tests.Util
{
    [TestClass]
    public class TestConsoleCmd
    {
        [TestMethod]
        public void TestParseArgsToKeyValueHash()
        {
            var testcmd = "-connStr=Server=localhost;Database=ApexQA01;Trusted_Connection=True;";
            var testResult = ConsoleCmd.ParseArgKey2StringHash(testcmd);
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.Value.Value);
        }

        [TestMethod]
        public void TestArgHash()
        {
            var testCmd = new[]
            {
                "-connStr=Server=localhost;Database=ApexQA01;Trusted_Connection=True;",
                @"-spmFilePath=C:\ProgramFiles (x86)\Somepath\SomeFile.bin",
                "-mySwitchHere"
            };
            var testResult = ConsoleCmd.ArgHash(testCmd);
            Assert.IsNotNull(testResult);
            foreach(var k in testResult.Keys)
                System.Diagnostics.Debug.WriteLine(string.Format("{0}\n\t\t{1}",k,testResult[k]));
        }
    }
}

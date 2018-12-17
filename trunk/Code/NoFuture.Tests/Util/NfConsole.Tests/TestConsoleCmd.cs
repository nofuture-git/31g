using System;
using NUnit.Framework;

namespace NoFuture.Util.NfConsole.Tests
{
    [TestFixture]
    public class TestConsoleCmd
    {
        [Test]
        public void TestParseArgsToKeyValueHash()
        {
            var testcmd = "-connStr=Server=localhost;Database=ApexQA01;Trusted_Connection=True;";
            var testResult = ConsoleCmd.ParseArgKey2StringHash(testcmd);
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult.Value.Value);
        }

        [Test]
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
                Console.WriteLine(string.Format("{0}\n\t\t{1}",k,testResult[k]));
        }
    }
}

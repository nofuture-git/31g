using System;
using NoFuture.Shared.Cfg;
using NUnit.Framework;

namespace NoFuture.Util.NfType.Tests
{
    [TestFixture]
    public class TypeNameProcessTests
    {
        [Test]
        public void TestInvocation()
        {
            NfConfig.CustomTools.InvokeNfTypeName =
                @"C:\Projects\31g\trunk\Code\NoFuture\bin\NoFuture.Tokens.InvokeNfTypeName.exe";
            var testSubject = new NfTypeNameProcess(null);
            var testResult =
                testSubject.GetNfTypeName(
                    "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL");
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult.AssemblyFullName);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Util.NfType;

namespace NoFuture.Tests.Util.NfTypeTests
{
    [TestClass]
    public class TypeNameProcessTests
    {
        [TestMethod]
        public void TestInvocation()
        {
            NoFuture.Tools.CustomTools.InvokeNfTypeName =
                @"C:\Projects\31g\trunk\Code\NoFuture\bin\NoFuture.Tokens.InvokeNfTypeName.exe";
            var testSubject = new NfTypeNameProcess(null);
            var testResult =
                testSubject.GetNfTypeName(
                    "log4net, Version=1.2.13.0, Culture=neutral, PublicKeyToken=669e0ddf0bb1aa2a, processorArchitecture=MSIL");
            Assert.IsNotNull(testResult);
            System.Diagnostics.Debug.WriteLine(testResult.AssemblyFullName);
        }
    }
}

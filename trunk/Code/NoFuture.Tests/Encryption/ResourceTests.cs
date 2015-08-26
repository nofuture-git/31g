using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Tests.Encryption
{
    [TestClass]
    public class ResourceTests
    {
        [TestMethod]
        public void TestGetEmbeddedSjclJsScript()
        {
            var testResult = NoFuture.Encryption.Sjcl.Resources.SjclJs;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(String.Empty,testResult);
        }

        [TestMethod]
        public void TestJurassicEngineAcceptsEmbeddedScript()
        {
            var testResult = NoFuture.Encryption.Sjcl.Resources.ScriptEngine;
            Assert.IsNotNull(testResult);
        }
       
    }
}

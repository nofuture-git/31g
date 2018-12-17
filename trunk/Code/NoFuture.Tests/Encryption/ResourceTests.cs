using System;
using NUnit.Framework;

namespace NoFuture.Encryption.Tests
{
    [TestFixture]
    public class ResourceTests
    {
        [Test]
        public void TestGetEmbeddedSjclJsScript()
        {
            var testResult = NoFuture.Encryption.Sjcl.Resources.SjclJs;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(String.Empty,testResult);
        }

        [Test]
        public void TestJurassicEngineAcceptsEmbeddedScript()
        {
            var testResult = NoFuture.Encryption.Sjcl.Resources.ScriptEngine;
            Assert.IsNotNull(testResult);
        }
       
    }
}

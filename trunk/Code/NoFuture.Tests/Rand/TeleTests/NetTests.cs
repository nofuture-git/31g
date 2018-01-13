using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestClass]
    public class NetTests
    {
        [TestMethod]
        public void WebmailDomainsTest()
        {
            var testResult = Tele.Net.WebmailDomains;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [TestMethod]
        public void TestRandomUriHost()
        {
            var testResult = Tele.Net.RandomUriHost(false);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Assert.IsTrue(testResult.Contains("."));
            var topDomain = testResult.Split('.').Last();
            Assert.IsNotNull(topDomain);
            Assert.IsFalse(string.IsNullOrWhiteSpace(topDomain));

            System.Diagnostics.Debug.WriteLine(topDomain);

            testResult = Tele.Net.RandomUriHost();
            Assert.IsNotNull(testResult);
            var testResultParts = testResult.Split('.');
            Assert.AreEqual(3, testResultParts.Length);
            System.Diagnostics.Debug.WriteLine(testResult);

        }

        [TestMethod]
        public void TestRandomHttpUri()
        {
            var testResult = Tele.Net.RandomHttpUri();
            Assert.IsNotNull(testResult);
            Assert.AreEqual("http", testResult.Scheme);
            Assert.IsNotNull(testResult.LocalPath);

            testResult = Tele.Net.RandomHttpUri(true, true);
            Assert.IsNotNull(testResult);
            Assert.AreEqual("https", testResult.Scheme);
            Assert.IsNotNull(testResult.LocalPath);
            Assert.IsNotNull(testResult.Query);

        }
    }
}

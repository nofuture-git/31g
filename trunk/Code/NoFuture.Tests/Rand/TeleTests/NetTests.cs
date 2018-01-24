using System;
using System.Linq;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestFixture]
    public class NetTests
    {
        [Test]
        public void WebmailDomainsTest()
        {
            var testResult = Tele.Net.WebmailDomains;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [Test]
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

        }

        [Test]
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


        [Test]
        public void TestRandomHttpUri_00()
        {
            for (var i = 0; i < 10; i++)
            {
                var testResult = NoFuture.Rand.Tele.Net.RandomHttpUri();
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                System.Diagnostics.Debug.WriteLine(testResult);
            }

            for (var i = 0; i < 10; i++)
            {
                var testResult = NoFuture.Rand.Tele.Net.RandomHttpUri(false, true);
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                System.Diagnostics.Debug.WriteLine(testResult);
            }
        }
    }
}

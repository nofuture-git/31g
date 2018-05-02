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

            Console.WriteLine(topDomain);

            testResult = Tele.Net.RandomUriHost();
            Assert.IsNotNull(testResult);

        }

        [Test]
        public void TestRandomHttpUri()
        {
            var testResult = Tele.Net.RandomHttpUri();
            Assert.IsNotNull(testResult);
            var uriTestResult = new Uri(testResult);
            Assert.IsNotNull(uriTestResult);
            Assert.AreEqual("http", uriTestResult.Scheme);
            Assert.IsNotNull(uriTestResult.LocalPath);

            testResult = Tele.Net.RandomHttpUri(true, true);
            Assert.IsNotNull(testResult);
            uriTestResult = new Uri(testResult);
            Assert.IsNotNull(uriTestResult);
            Assert.AreEqual("https", uriTestResult.Scheme);
            Assert.IsNotNull(uriTestResult.LocalPath);
            Assert.IsNotNull(uriTestResult.Query);

        }


        [Test]
        public void TestRandomHttpUri_00()
        {
            for (var i = 0; i < 10; i++)
            {
                var testResult = Tele.Net.RandomHttpUri();
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                Console.WriteLine(testResult);
            }

            for (var i = 0; i < 10; i++)
            {
                var testResult = Tele.Net.RandomHttpUri(false, true);
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                Console.WriteLine(testResult);
            }
        }

        [Test]
        public void TestRandomUsername()
        {
            var testResult = Tele.Net.RandomUsername();
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);

            testResult = Tele.Net.RandomUsername("Julius", "Ceaser");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.ToLower().Contains("ceaser"));
            Console.WriteLine(testResult);
        }
    }
}

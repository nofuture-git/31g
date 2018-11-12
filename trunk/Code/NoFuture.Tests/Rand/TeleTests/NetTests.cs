﻿using System;
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
            var testResult = Tele.NetUri.WebmailDomains;
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);
        }

        [Test]
        public void TestRandomUriHost()
        {
            var testResult = Tele.NetUri.RandomUriHost(false);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult));
            Assert.IsTrue(testResult.Contains("."));
            var topDomain = testResult.Split('.').Last();
            Assert.IsNotNull(topDomain);
            Assert.IsFalse(string.IsNullOrWhiteSpace(topDomain));

            Console.WriteLine(topDomain);

            testResult = Tele.NetUri.RandomUriHost();
            Assert.IsNotNull(testResult);

        }

        [Test]
        public void TestRandomHttpUri()
        {
            var testResult = Tele.NetUri.RandomHttpUri();
            Assert.IsNotNull(testResult);
            var uriTestResult = new Uri(testResult);
            Assert.IsNotNull(uriTestResult);
            Assert.AreEqual("http", uriTestResult.Scheme);
            Assert.IsNotNull(uriTestResult.LocalPath);

            testResult = Tele.NetUri.RandomHttpUri(true, true);
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
                var testResult = Tele.NetUri.RandomHttpUri();
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                Console.WriteLine(testResult);
            }

            for (var i = 0; i < 10; i++)
            {
                var testResult = Tele.NetUri.RandomHttpUri(false, true);
                Assert.IsNotNull(testResult);
                Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.ToString()));
                Console.WriteLine(testResult);
            }
        }

        [Test]
        public void TestRandomUsername()
        {
            var testResult = Tele.NetUri.RandomUsername();
            Assert.IsNotNull(testResult);
            Console.WriteLine(testResult);

            testResult = Tele.NetUri.RandomUsername("Julius", "Ceaser");
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.ToLower().Contains("ceaser"));
            Console.WriteLine(testResult);
        }
    }
}

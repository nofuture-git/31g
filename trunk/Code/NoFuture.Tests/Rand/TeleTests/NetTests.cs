using System;
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
    }
}

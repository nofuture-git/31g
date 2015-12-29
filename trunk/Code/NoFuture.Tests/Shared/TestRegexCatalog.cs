using System;
using System.Collections;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Shared;

namespace NoFuture.Tests.Shared
{
    [TestClass]
    public class TestRegexCatalog
    {
        public Hashtable Regex2Values;
        private NoFuture.Shared.RegexCatalog _testSubject = new RegexCatalog();

        private const string _hardCodedIpV4 = "10.130.55.36";

        [TestInitialize]
        public void Init()
        {
            
            Regex2Values = new Hashtable
            {
                {_testSubject.WindowsRootedPath, @"C:\Projects\"},
                {
                    _testSubject.EmailAddress,
                    string.Format("{0}@myDomain.net", System.Environment.GetEnvironmentVariable("USERNAME"))
                },
                {_hardCodedIpV4, "127.0.0.1"},
                {_testSubject.IPv4, "127.0.0.1"},
                {_testSubject.UrlClassicAmerican, "localhost"}
            };
           
        }

        [TestMethod]
        public void TestIsRegexMatch()
        {
            string testResultOut;
            var testResult = NoFuture.Shared.RegexCatalog.IsRegexMatch("10.130.55.36", _testSubject.IPv4,
                out testResultOut);
            Assert.IsTrue(testResult);

            testResultOut = null;
            testResult = NoFuture.Shared.RegexCatalog.IsRegexMatch("net.tcp://10.130.55.36/MyService.svc", _testSubject.IPv4,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("10.130.55.36", testResultOut);

            testResultOut = null;
            testResult = NoFuture.Shared.RegexCatalog.IsRegexMatch("https://www.TheDomainName.com/MyService.svc", _testSubject.UrlClassicAmerican,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("www.TheDomainName.com", testResultOut);

        }

        [TestMethod]
        public void TestAreAnyRegexMatch()
        {
            var testResult = NoFuture.Shared.RegexCatalog.AreAnyRegexMatch("www.TheDomainName.com",
                Regex2Values.Keys.Cast<string>().ToArray());
            Assert.IsTrue(testResult);

            testResult = RegexCatalog.AreAnyRegexMatch("net.tcp://224.11.89.55:1060/OrNotToWankAdminUIController/",
                Regex2Values.Keys.Cast<string>().ToArray());

            Assert.IsTrue(testResult);
        }

        [TestMethod]
        public void TestAppropriateAllRegex()
        {
            var testResultOut = _hardCodedIpV4;
            NoFuture.Shared.RegexCatalog.AppropriateAllRegex(ref testResultOut, Regex2Values);
            Assert.AreEqual(Regex2Values[_hardCodedIpV4],testResultOut);
        }

        [TestMethod]
        public void TestToRegexExpression()
        {
            var testInput = "Dependent (18 yrs +)";
            var testResult = NoFuture.Shared.RegexCatalog.ToRegexExpression(testInput);

            Assert.IsNotNull(testResult);
            System.Text.RegularExpressions.Regex.IsMatch("Dependents", testResult);
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}

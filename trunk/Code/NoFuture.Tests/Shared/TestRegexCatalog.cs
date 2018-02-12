using System;
using System.Collections;
using System.Linq;
using NUnit.Framework;
using NoFuture.Shared;
using NoFuture.Shared.Core;

namespace NoFuture.Tests.Shared
{
    [TestFixture]
    public class TestRegexCatalog
    {
        public Hashtable Regex2Values;
        private RegexCatalog _testSubject = new RegexCatalog();

        private const string _hardCodedIpV4 = "10.130.55.36";

        [SetUp]
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

        [Test]
        public void TestIsRegexMatch()
        {
            string testResultOut;
            var testResult = RegexCatalog.IsRegexMatch("10.130.55.36", _testSubject.IPv4,
                out testResultOut);
            Assert.IsTrue(testResult);

            testResultOut = null;
            testResult = RegexCatalog.IsRegexMatch("net.tcp://10.130.55.36/MyService.svc", _testSubject.IPv4,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("10.130.55.36", testResultOut);

            testResultOut = null;
            testResult = RegexCatalog.IsRegexMatch("https://www.TheDomainName.com/MyService.svc", _testSubject.UrlClassicAmerican,
                out testResultOut);
            Assert.IsTrue(testResult);
            Assert.AreEqual("www.TheDomainName.com", testResultOut);

        }

        [Test]
        public void TestAreAnyRegexMatch()
        {
            var testResult = RegexCatalog.AreAnyRegexMatch("www.TheDomainName.com",
                Regex2Values.Keys.Cast<string>().ToArray());
            Assert.IsTrue(testResult);

            testResult = RegexCatalog.AreAnyRegexMatch("net.tcp://224.11.89.55:1060/OrNotToWankAdminUIController/",
                Regex2Values.Keys.Cast<string>().ToArray());

            Assert.IsTrue(testResult);

            testResult = RegexCatalog.AreAnyRegexMatch("ContractDocument", new[] {"Contract", "Client", "Location"});
            Assert.IsTrue(testResult);
        }

        [Test]
        public void TestAppropriateAllRegex()
        {
            var testResultOut = _hardCodedIpV4;
            RegexCatalog.AppropriateAllRegex(ref testResultOut, Regex2Values);
            Assert.AreEqual(Regex2Values[_hardCodedIpV4],testResultOut);
        }

        [Test]
        public void TestToRegexExpression()
        {
            var testInput = "Dependent (18 yrs +)";
            var testResult = RegexCatalog.ToRegexExpression(testInput);

            Assert.IsNotNull(testResult);
            System.Text.RegularExpressions.Regex.IsMatch("Dependents", testResult);
            Console.WriteLine(testResult);
        }
    }
}

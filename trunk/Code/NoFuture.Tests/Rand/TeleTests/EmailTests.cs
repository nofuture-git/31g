using System;
using System.Linq;
using NoFuture.Rand.Core.Enums;
using NUnit.Framework;
using NoFuture.Rand.Tele;

namespace NoFuture.Rand.Tests.TeleTests
{
    [TestFixture]
    public class EmailTests
    {
        [Test]
        public void TestRandomEmailUri_UsCommon()
        {
            var testResult = Tele.Email.RandomEmail("booty", true);
            Assert.IsNotNull(testResult);
            var testResultParts = testResult.Value.Split('@');
            Assert.AreEqual(2, testResultParts.Length);
            Assert.IsTrue(NetUri.UsWebmailDomains.Contains(testResultParts[1]));
            Assert.AreEqual("booty", testResultParts[0]);
            Console.WriteLine(testResult);
        }


        [Test]
        public void TestRandomEmailUriPersonal()
        {
            var testResult = Email.RandomEmail(NetUri.RandomUsername("Robert", "Lee"));
            Assert.IsNotNull(testResult);
            Assert.IsTrue(testResult.Value.ToLower().Contains("lee"));
            Console.WriteLine(testResult);

        }

        [Test]
        public void TestRandomEmailUri()
        {
            var testResult = Email.RandomEmail(null);
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Value));
            Console.WriteLine(testResult.ToUri());

        }

        [Test]
        public void TestGetChildishRandomEmail()
        {
            var testResult = Email.RandomChildishEmail();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(string.IsNullOrWhiteSpace(testResult.Value));
            Console.WriteLine(testResult.ToString());

        }

        [Test]
        public void TestToUri()
        {
            var testSubject = new Email {Value = "testUser@someDomain.net"};
            Assert.IsNotNull(testSubject.Value);
            var testResult = testSubject.ToUri();
            Assert.IsNotNull(testResult);
        }

        [Test]
        public void TestToData()
        {
            var testSubject = new Email { Value = "testUser@someDomain.net", Descriptor = KindsOfLabels.Home};
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var k in testResult.Keys)
                Console.WriteLine($"{k}: {testResult[k]}");
        }
    }
}

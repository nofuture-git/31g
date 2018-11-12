using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Gov.US;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.GovTests
{
    [TestFixture]
    public class VitalRecordTests
    {
        [Test]
        public void TestBirthCert()
        {
            var dob = Etx.RandomAdultBirthDate();
            var testSubject = new AmericanBirthCert(dob);
            testSubject.City = "Tempe";
            testSubject.State = "FL";
            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach(var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");
        }

        [Test]
        public void TestDeathCert()
        {
            var testSubject = new AmericanDeathCert(AmericanDeathCert.MannerOfDeath.Natural, "Julius Ceaser");
            testSubject.DateOfDeath = DateTime.Today;

            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);
            foreach (var tr in testResult.Keys)
                Console.WriteLine($"{tr}, {testResult[tr]}");

        }
    }
}

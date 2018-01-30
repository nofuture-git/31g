using System;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class PondusTests
    {
        [Test]
        public void TestEquals()
        {
            var testSubject = new Pondus("test")
            {
                Inception = DateTime.Today.AddYears(-1),
                Terminus = DateTime.Today
            };

            var testCompare = new Pondus("test")
            {
                Inception = DateTime.Today.AddYears(-1),
                Terminus = DateTime.Today
            };

            //same dates and name
            Assert.IsTrue(testSubject.Equals(testCompare));
            testCompare.Terminus = DateTime.Today.AddDays(1);

            //same name, same start date, diff end date
            Assert.IsFalse(testSubject.Equals(testCompare));

            //same dates, diff name
            testCompare.Terminus = DateTime.Today.AddDays(1);
            testCompare.Expectation.Name = "test2";

            Assert.IsFalse(testSubject.Equals(testCompare));

            //null date is diff date
            testCompare.Expectation.Name = "test";
            testCompare.Terminus = null;

            Assert.IsFalse(testSubject.Equals(testCompare));

        }

        [Test]
        public void TestCopyFrom()
        {
            var testSubject = new Pondus("TestCorporation");
            testSubject.Expectation.UpsertName(KindsOfNames.Group, "Company");

            var testSubject2 = new Pondus(testSubject.Expectation);
            Assert.AreEqual(testSubject.Expectation.Name, testSubject2.Expectation.Name);
            var groupName = testSubject2.Expectation.GetName(KindsOfNames.Group);
            Assert.IsNotNull(groupName);

            Assert.AreEqual("Company", groupName);
        }
    }
}

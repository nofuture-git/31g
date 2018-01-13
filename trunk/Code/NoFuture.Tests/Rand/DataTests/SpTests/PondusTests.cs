using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class PondusTests
    {
        [TestMethod]
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
            testCompare.My.Name = "test2";

            Assert.IsFalse(testSubject.Equals(testCompare));

            //null date is diff date
            testCompare.My.Name = "test";
            testCompare.Terminus = null;

            Assert.IsFalse(testSubject.Equals(testCompare));

        }

        [TestMethod]
        public void TestCopyFrom()
        {
            var testSubject = new Pondus("TestCorporation");
            testSubject.My.UpsertName(KindsOfNames.Group, "Company");

            var testSubject2 = new Pondus(testSubject.My);
            Assert.AreEqual(testSubject.My.Name, testSubject2.My.Name);
            var groupName = testSubject2.My.GetName(KindsOfNames.Group);
            Assert.IsNotNull(groupName);

            Assert.AreEqual("Company", groupName);
        }
    }
}

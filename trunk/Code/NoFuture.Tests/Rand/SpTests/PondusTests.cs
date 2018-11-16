﻿using System;
using Newtonsoft.Json;
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
            var testSubject = new NamedReceivable("test")
            {
                Inception = DateTime.Today.AddYears(-1),
                Terminus = DateTime.Today
            };

            var testCompare = new NamedReceivable("test")
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
            testCompare.Name = "test2";

            Assert.IsFalse(testSubject.Equals(testCompare));

            //null date is diff date
            testCompare.Name = "test";
            testCompare.Terminus = null;

            Assert.IsFalse(testSubject.Equals(testCompare));

        }

        [Test]
        public void TestCopyFrom()
        {
            var testSubject = new NamedReceivable("TestCorporation");
            testSubject.AddName(KindsOfNames.Group, "Company");

            var testSubject2 = new NamedReceivable(testSubject);
            Assert.AreEqual(testSubject.Name, testSubject2.Name);
            var groupName = testSubject2.GetName(KindsOfNames.Group);
            Assert.IsNotNull(groupName);

            Assert.AreEqual("Company", groupName);
        }

        [Test]
        public void TestToData()
        {
            var dt = DateTime.Today;
            var testSubject = new NamedReceivable("Test Name");
            testSubject.AddName(KindsOfNames.Group, "Company");
            var cusip = new Cusip().Value;
            testSubject.AddPositiveValue(dt.AddDays(-360), new Security(cusip, 5000));
            testSubject.AddPositiveValue(dt.AddDays(-360), new Pecuniam(500000.0M));
            testSubject.Inception = dt.AddDays(-365);
            testSubject.Expectation.Value = new Pecuniam(800000M);
            

            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);

            var asJson = JsonConvert.SerializeObject(testResult, Formatting.Indented);
            Console.WriteLine(asJson);

        }
    }
}

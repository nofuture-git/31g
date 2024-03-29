﻿using System;
using Newtonsoft.Json;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Sp;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class NamedTradelineTests
    {
        [Test]
        public void TestEquals()
        {
            var testSubject = new NamedTradeline("test")
            {
                Inception = DateTime.Today.AddYears(-1),
                Terminus = DateTime.Today
            };

            var testCompare = new NamedTradeline("test")
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
            var testSubject = new NamedTradeline("TestCorporation");
            testSubject.AddName(KindsOfNames.Group, "Company");

            var testSubject2 = new NamedTradeline(testSubject);
            Assert.AreEqual(testSubject.Name, testSubject2.Name);
            var groupName = testSubject2.GetName(KindsOfNames.Group);
            Assert.IsNotNull(groupName);

            Assert.AreEqual("Company", groupName);
        }

        [Test]
        public void TestToData()
        {
            var dt = DateTime.Today;
            var testSubject = new NamedTradeline("Test Name");
            testSubject.AddName(KindsOfNames.Group, "Company");
            var cusip = new Cusip().Value;
            testSubject.AddPositiveValue(dt.AddDays(-360), new Security(cusip, 5000));
            testSubject.AddPositiveValue(dt.AddDays(-360), new Pecuniam(500000.0M));
            testSubject.Inception = dt.AddDays(-365);
            testSubject.AddPositiveValue(dt.AddDays(-365), new Pecuniam(800000M));
            

            var testResult = testSubject.ToData(KindsOfTextCase.Kabab);

            var asJson = JsonConvert.SerializeObject(testResult, Formatting.Indented);
            Console.WriteLine(asJson);
        }

        [Test]
        public void TestRandomNamedReceivableWithVariedHistory()
        {
            var testResult = NamedTradeline.RandomNamedTradelineWithVariedHistory("firstName", "groupName", 250M.ToPecuniam(), new TimeSpan(30, 0, 0, 0),
                DateTime.Today.AddYears(-1));
            Assert.IsNotNull(testResult);
            Assert.AreEqual("firstName", testResult.GetName(KindsOfNames.Legal));
            Assert.AreEqual("groupName", testResult.GetName(KindsOfNames.Group));
            var testResultAverage = testResult.AveragePerDueFrequency();
            Console.WriteLine(testResultAverage);
            var testResultDiff = System.Math.Abs(250M - testResultAverage.Amount);
            Assert.IsTrue(testResultDiff < 83M);
            Console.WriteLine(testResult.Balance.FirstTransaction);
            Console.WriteLine(testResult.Balance.LastTransaction);
        }

        [Test]
        public void TestRandomNamedReceivalbleWithSteadyHistory()
        {
            var testResult = NamedTradeline.RandomNamedTradelineWithSteadyHistory("firstName", "groupName", 250M.ToPecuniam(), new TimeSpan(30, 0, 0, 0),
                DateTime.Today.AddYears(-1));
            Assert.IsNotNull(testResult);
            Assert.AreEqual("firstName", testResult.GetName(KindsOfNames.Legal));
            Assert.AreEqual("groupName", testResult.GetName(KindsOfNames.Group));
            var testResultAverage = testResult.AveragePerDueFrequency();
            Console.WriteLine(testResultAverage);
            var testResultDiff = System.Math.Abs(250M - testResultAverage.Amount);
            Assert.IsTrue(testResultDiff < 1M);
            Console.WriteLine(testResult.Balance.FirstTransaction);
            Console.WriteLine(testResult.Balance.LastTransaction);

        }

        [Test]
        public void TestRandomNamedReceivalbleWithHistoryToSum()
        {
            var testResult =
                NamedTradeline.RandomNamedTradelineWithHistoryToSum("first name", "group name",
                    120000M.ToPecuniam());
            var testResultValue = testResult.Value;
            Console.WriteLine(testResultValue);
            Assert.AreEqual(120000M.ToPecuniam(), testResultValue);

        }
    }
}

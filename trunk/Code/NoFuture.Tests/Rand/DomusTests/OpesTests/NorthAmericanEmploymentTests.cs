﻿using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanEmploymentTests
    {
        [TestMethod]
        public void TestGetYearsOfServiceInDates()
        {
            //still employed
            var testSubject = new NoFuture.Rand.Domus.Opes.NorthAmericanEmployment(new DateTime(2011,10,5),null, null);

            var testResult = testSubject.GetYearsOfServiceInDates();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(4, testResult.Count);

            testSubject = new NoFuture.Rand.Domus.Opes.NorthAmericanEmployment(new DateTime(2013, 5, 16), new DateTime(2017,8,1), null);
            testResult = testSubject.GetYearsOfServiceInDates();
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(3, testResult.Count);

        }

        [TestMethod]
        public void TestGetPayItemsForRange()
        {
            var testSubject = new NoFuture.Rand.Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null, null);
            testSubject.Occupation = StandardOccupationalClassification.GetById("39-3011");

            var testResult = testSubject.GetPayItemsForRange(55000D.ToPecuniam(), testSubject.FromDate,
                testSubject.FromDate.Value.AddYears(1));

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Length);

            var testPondus = testResult.FirstOrDefault(p => p.Name == "Wages");
            Assert.IsNotNull(testPondus);
            Assert.AreNotEqual(Pecuniam.Zero, testPondus.Value);

            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            testResult = testSubject.GetPayItemsForRange(55000D.ToPecuniam(), testSubject.FromDate,
                testSubject.FromDate.Value.AddYears(1));

            testPondus = testResult.FirstOrDefault(p => p.Name == "Commissions");
            Assert.IsNotNull(testPondus);
            Assert.AreNotEqual(Pecuniam.Zero, testPondus.Value);
        }
    }
}

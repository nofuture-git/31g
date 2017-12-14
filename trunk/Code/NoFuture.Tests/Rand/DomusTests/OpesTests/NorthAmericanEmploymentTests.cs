using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Core.Enums;
using NoFuture.Rand.Data.Endo.Grps;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class NorthAmericanEmploymentTests
    {
        [TestMethod]
        public void TestGetYearsOfServiceInDates()
        {
            //still employed
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011,10,5),null);

            var testResult = testSubject.GetYearsOfServiceInDates(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach(var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(6, testResult.Count);

            testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2013, 5, 16), new DateTime(2017,8,1));
            testResult = testSubject.GetYearsOfServiceInDates(null);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            foreach (var r in testResult)
                System.Diagnostics.Debug.WriteLine(r);

            Assert.AreEqual(4, testResult.Count);

        }

        [TestMethod]
        public void TestGetIncomeName2RandomRates()
        {
            var testSubject = new Domus.Opes.NorthAmericanEmployment(new DateTime(2011, 10, 5), null);
            testSubject.Occupation = StandardOccupationalClassification.GetById("41-2031");
            var testResult = testSubject.GetPayName2RandRates(testSubject.MyOptions);

            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(0, testResult.Count);

            var sumOfRates = 0D;
            foreach (var item in testResult)
            {
                sumOfRates += item.Value;
            }

            Assert.IsTrue(Math.Abs(1D - sumOfRates) < 0.001);
            Assert.IsTrue(Math.Abs(1D - sumOfRates) > -0.001);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    }
}

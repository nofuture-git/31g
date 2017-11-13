using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Tests.Rand
{
    [TestClass]
    public class RentTests
    {
        [TestMethod]
        public void TestCountMonths()
        {
            var dt1 = new DateTime(DateTime.Today.Year, 1, 14);
            var dt2 = new DateTime(DateTime.Today.Year - 1, 12, 18);

            var testResult = Rent.CountOfWholeCalendarMonthsBetween(dt1, dt2);
            Assert.AreEqual(0, testResult);
            testResult = Rent.CountOfWholeCalendarMonthsBetween(dt2, dt1);
            Assert.AreEqual(0, testResult);

            dt2 = new DateTime(DateTime.Today.Year -1, 11, 5);
            testResult = Rent.CountOfWholeCalendarMonthsBetween(dt1, dt2);
            Assert.AreEqual(1,testResult);

            dt2 = new DateTime(DateTime.Today.Year,1,1);
            testResult = Rent.CountOfWholeCalendarMonthsBetween(dt1, dt2);
            Assert.AreEqual(0,testResult);

            dt1 = new DateTime(DateTime.Today.Year, 1, 1);
            dt2 = new DateTime(DateTime.Today.Year + 1, 1, 1);
            testResult = Rent.CountOfWholeCalendarMonthsBetween(dt1, dt2);
            Assert.AreEqual(12, testResult);

            dt1 = new DateTime(DateTime.Today.Year, 5, 5);
            dt2 = new DateTime(DateTime.Today.Year, 11, 1);
            testResult = Rent.CountOfWholeCalendarMonthsBetween(dt1, dt2);
            Assert.AreEqual(5, testResult);
        }

        [TestMethod]
        public void TestGetMinPayment()
        {
            //most straight foward example
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);

            var testSubject = new Rent(null, firstOfYear, 12,new Pecuniam(700), new Pecuniam(0));
            //first rent due immediately
            testSubject.PayRent(firstOfYear, new Pecuniam(-700));

            //15 days later
            var testResult = testSubject.GetMinPayment(firstOfYear.AddDays(15));

            //paid first month so nothing should be due
            Assert.AreEqual(0M, testResult.Amount);

            //move signing back to mid Dec of prev year
            var newSigningDate = new DateTime(DateTime.Today.Year-1, 12, 14);
            testSubject = new Rent(null, newSigningDate, 12, new Pecuniam(700), new Pecuniam(0));

            //pro-rated amount should be due immediately
            testResult = testSubject.GetMinPayment(newSigningDate);
            Assert.AreEqual(new Pecuniam(-396.67M), testResult);

            //should be the same until first of next month
            testResult = testSubject.GetMinPayment(new DateTime(DateTime.Today.Year - 1, 12, 31));
            Assert.AreEqual(new Pecuniam(-396.67M), testResult);

            //first month rent due
            testResult = testSubject.GetMinPayment(firstOfYear);
            Assert.AreEqual(new Pecuniam(-396.67M + -700), testResult);

            //pay prorated amt
            testSubject.PayRent(new DateTime(DateTime.Today.Year - 1, 12, 31), new Pecuniam(-396.67M));

            //should still be due one months
            testResult = testSubject.GetMinPayment(firstOfYear);
            Assert.AreEqual(new Pecuniam(-700), testResult);

            //pay first month at exact same time only as another transaction
            testSubject.PayRent(new DateTime(DateTime.Today.Year - 1, 12, 30), new Pecuniam(-700));

            //since rent is not due until tommorrow - there is a 700 credit 
            testResult = testSubject.GetMinPayment(new DateTime(DateTime.Today.Year - 1, 12, 31));
            Assert.AreEqual(700.0M, testResult.Amount );

            //very next day should be current
            testResult = testSubject.GetMinPayment(new DateTime(DateTime.Today.Year, 1, 1));
            Assert.AreEqual(0M, testResult.Amount);

            //pay some part of next month ahead of due
            testSubject.PayRent(new DateTime(DateTime.Today.Year, 1, 18), new Pecuniam(-300));

            //should register as a credit
            testResult = testSubject.GetMinPayment(new DateTime(DateTime.Today.Year, 1, 18));
            Assert.AreEqual(new Pecuniam(300), testResult);

            //at first of next month rent less the credit should be due
            testResult = testSubject.GetMinPayment(new DateTime(DateTime.Today.Year, 2, 1));
            Assert.AreEqual(new Pecuniam(300-700), testResult);
        }

        [TestMethod]
        public void TestGetExpectedRent()
        {
            //most straight foward example
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);

            var testSubject = new Rent(null, firstOfYear, 12, new Pecuniam(700), new Pecuniam(0));

            var testResult = testSubject.GetExpectedTotalRent(new DateTime(DateTime.Today.Year, 3,1));
            //only two whole months have passed but rent's due on the first
            Assert.AreEqual(new Pecuniam(700*3), testResult);

            //after 15 days only first rent is due
            testResult = testSubject.GetExpectedTotalRent(firstOfYear.AddDays(15));
            Assert.AreEqual(new Pecuniam(700), testResult);

            //back the signing date to middle of Dec
            var newSigningDate = firstOfYear.AddDays(-16);
            testSubject = new Rent(null, newSigningDate, 12, new Pecuniam(700), new Pecuniam(0));

            //this should be only the pro-rated amount, first full month is due tommorrow
            testResult = testSubject.GetExpectedTotalRent(newSigningDate.AddDays(15));
            Assert.AreEqual(new Pecuniam(350), testResult);

            //add another day - this should be pro-rated amount plus first month
            testResult = testSubject.GetExpectedTotalRent(firstOfYear.AddDays(16));
            Assert.AreEqual(new Pecuniam(350 + 700), testResult);

            //move to first of Feb - this should be pro-rated amount plus two months rent
            testResult = testSubject.GetExpectedTotalRent(new DateTime(DateTime.Today.Year, 2, 1));
            Assert.AreEqual(new Pecuniam(350 + 700*2), testResult);

            //middle of march -should be prorated amount plus three months
            testResult = testSubject.GetExpectedTotalRent(new DateTime(DateTime.Today.Year, 3, 15));
            Assert.AreEqual(new Pecuniam(350 + 700*3), testResult);

            //on the last day of march - should still be the same
            testResult = testSubject.GetExpectedTotalRent(new DateTime(DateTime.Today.Year, 4, 1).AddDays(-1));
            Assert.AreEqual(new Pecuniam(350 + 700 * 3), testResult);
        }

        [TestMethod]
        public void TestGetAvgAmericanRentByYear()
        {
            var testResult = Rent.GetAvgAmericanRentByYear(null);
            Assert.IsTrue(testResult.Amount > 0);
            System.Diagnostics.Debug.WriteLine(testResult);
        }
    }
}

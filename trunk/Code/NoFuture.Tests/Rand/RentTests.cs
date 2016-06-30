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

            var testSubject = new Rent(firstOfYear, 12,new Pecuniam(700), new Pecuniam(0));
            //first rent due immediately
            testSubject.PayRent(firstOfYear, new Pecuniam(-700));

            //15 days later
            var testResult = testSubject.GetMinPayment(firstOfYear.AddDays(15));

            //nothing should be due
            Assert.AreEqual(0M, testResult.Amount);

            //signing in mid Dec of prev year
            var newSigningDate = firstOfYear.AddDays(-16);
            testSubject = new Rent(newSigningDate, 12, new Pecuniam(700), new Pecuniam(0));

            //pro-rated amount should be due immediately
            testResult = testSubject.GetMinPayment(newSigningDate);
            Assert.AreEqual(new Pecuniam(-350), testResult);

            //shoudl be the same for next 15 days
            testResult = testSubject.GetMinPayment(newSigningDate.AddDays(15));
            Assert.AreEqual(new Pecuniam(-350), testResult);

            //first month rent due
            testResult = testSubject.GetMinPayment(newSigningDate.AddDays(16));
            Assert.AreEqual(new Pecuniam(-350 + -700), testResult);

            //pay prorated amt
            testSubject.PayRent(newSigningDate.AddDays(16), new Pecuniam(-350));

            //should still be due one months
            testResult = testSubject.GetMinPayment(newSigningDate.AddDays(16));
            Assert.AreEqual(new Pecuniam(-700), testResult);

            //pay first month at exact same time only as another transaction
            testSubject.PayRent(newSigningDate.AddDays(16), new Pecuniam(-700));

            //should be current
            testResult = testSubject.GetMinPayment(newSigningDate.AddDays(16));
            Assert.AreEqual(0M, testResult.Amount );

            //pay some part of next month ahead of due
            testSubject.PayRent(newSigningDate.AddDays(18), new Pecuniam(-300));

            //should register as a credit
            testResult = testSubject.GetMinPayment(newSigningDate.AddDays(18));
            Assert.AreEqual(new Pecuniam(300), testResult);

            //at first of next month rent less the credit should be due
            testResult = testSubject.GetMinPayment(firstOfYear.AddMonths(1));
            Assert.AreEqual(new Pecuniam(300-700), testResult);
        }

        [TestMethod]
        public void TestGetExpectedRent()
        {
            //most straight foward example
            var firstOfYear = new DateTime(DateTime.Today.Year, 1, 1);

            var testSubject = new Rent(firstOfYear, 12, new Pecuniam(700), new Pecuniam(0));

            var testResult = testSubject.GetExpectedTotalRent(new DateTime(DateTime.Today.Year, 3,1));
            //only two whole months have passed but rent's due on the first
            Assert.AreEqual(new Pecuniam(700*2), testResult);

            testResult = testSubject.GetExpectedTotalRent(firstOfYear.AddDays(15));
            Assert.AreEqual(new Pecuniam(700), testResult);

            //back the signing date to middle of Dec
            var newSigningDate = firstOfYear.AddDays(-16);
            testSubject = new Rent(newSigningDate, 12, new Pecuniam(700), new Pecuniam(0));
            testResult = testSubject.GetExpectedTotalRent(newSigningDate.AddDays(15));
            
            //this should be only the pro-rated amount, first full month is due tommorrow
            Assert.AreEqual(new Pecuniam(350), testResult);

            //add another day 
            testResult = testSubject.GetExpectedTotalRent(firstOfYear.AddDays(16));

            //this should be pro-rated amount plus first month
            Assert.AreEqual(new Pecuniam(350 + 700), testResult);

            //move to first of Feb
            testResult = testSubject.GetExpectedTotalRent(firstOfYear.AddMonths(1));
            System.Diagnostics.Debug.WriteLine(firstOfYear.AddMonths(1));
            //this should be pro-rated amount plus two months rent
            Assert.AreEqual(new Pecuniam(350 + 700*2), testResult);

            //move date out to middle of march
            testResult = testSubject.GetExpectedTotalRent(firstOfYear.AddMonths(3).AddDays(15));

            //this should be prorated amount plus three months
            Assert.AreEqual(new Pecuniam(350 + 700*3), testResult);
        }

        [TestMethod]
        public void TestGetDtOfFirstRent()
        {

            System.Diagnostics.Debug.WriteLine(Math.Round(700/30.0 * (16.0 - 1), 2));
        }
    }
}

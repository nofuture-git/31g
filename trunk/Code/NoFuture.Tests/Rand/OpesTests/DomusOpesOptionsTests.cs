using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Opes;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
{
    [TestFixture]
    public class DomusOpesOptionsTests
    {
        [Test]
        public void TestGetClone()
        {
            var testInput = new DomusOpesOptions
            {
                Inception = DateTime.Today.AddYears(-1),
                DueFrequency = new TimeSpan(1,0,0,0),
                Rate =  RandPortions.DiminishingRate.Fast,
                SumTotal = 88000,
                IsVehiclePaidOff = true,
                NumberOfCreditCards = 3,
                NumberOfVehicles = 2,
                IsRenting = true,
                IsPayingChildSupport = true,
                IsPayingSpousalSupport = true
            };
            testInput.AddPossibleZeroOuts("abc", "xyz");
            testInput.ChildrenDobs.Add(DateTime.Today.AddYears(-2));
            testInput.ChildrenDobs.Add(DateTime.Today.AddYears(-4));
            testInput.AddGivenDirectly("name00", "group00", 9900);
            testInput.AddGivenDirectly("name01", "group00", 1100);

            var testResult = testInput.GetClone();

            Assert.IsNotNull(testResult);
            Assert.AreEqual(testInput.Inception, testResult.Inception);
            Assert.AreEqual(testInput.DueFrequency, testResult.DueFrequency);
            Assert.AreEqual(testInput.Rate, testResult.Rate);
            Assert.AreEqual(testInput.SumTotal, testResult.SumTotal);
            Assert.AreEqual(testInput.IsVehiclePaidOff, testResult.IsVehiclePaidOff);
            Assert.AreEqual(testInput.NumberOfCreditCards, testResult.NumberOfCreditCards);
            Assert.AreEqual(testInput.NumberOfVehicles, testResult.NumberOfVehicles);
            Assert.AreEqual(testInput.IsRenting, testResult.IsRenting);
            Assert.AreEqual(testInput.IsPayingChildSupport, testResult.IsPayingChildSupport);
            Assert.AreEqual(testInput.IsPayingSpousalSupport, testResult.IsPayingSpousalSupport);
            Assert.IsTrue(testResult.IsPayingChildSupport);

            Assert.AreEqual(2, testResult.PossibleZeroOutCount);

            Assert.IsNotNull(testResult.GetChildrenAges());
            Assert.AreEqual(2, testResult.GetChildrenAges().Count);

            Assert.IsTrue(testResult.AnyGivenDirectly());
            Assert.AreEqual(2, testResult.GivenDirectlyCount);

            Assert.IsNotNull(testResult.FactorOptions);

            var testInputFr = testInput.FactorOptions;
            var testResultFt = testResult.FactorOptions;

            Assert.AreEqual(testInputFr.DateOfBirth, testResultFt.DateOfBirth);
            Assert.AreEqual(testInputFr.EducationLevel, testResultFt.EducationLevel);
            Assert.AreEqual(testInputFr.Gender, testResultFt.Gender);
            Assert.AreEqual(testInputFr.MaritialStatus, testResultFt.MaritialStatus);
            Assert.AreEqual(testInputFr.Race, testResultFt.Race);
            Assert.AreEqual(testInputFr.Region, testResultFt.Region);

        }
    }
}

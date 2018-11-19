using System;
using NoFuture.Rand.Opes;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.OpesTests
{
    [TestFixture]
    public class OpesOptionsTests
    {
        [Test]
        public void TestGetClone()
        {
            var testInput = new OpesOptions
            {
                Inception = DateTime.Today.AddYears(-1),
                DueFrequency = new TimeSpan(1,0,0,0),
                DerivativeSlope = -0.33,
                SumTotal = 88000.ToPecuniam(),
                IsVehiclePaidOff = true,
                NumberOfCreditCards = 3,
                NumberOfVehicles = 2,
                PossibleZeroOuts = {"abc", "xyz"},
                IsRenting = true,
                IsPayingChildSupport = true,
                IsPayingSpousalSupport = true
            };

            testInput.ChildrenDobs.Add(DateTime.Today.AddYears(-2));
            testInput.ChildrenDobs.Add(DateTime.Today.AddYears(-4));
            testInput.AddGivenDirectly("name00", "group00", 9900.ToPecuniam());
            testInput.AddGivenDirectly("name01", "group00", 1100.ToPecuniam());

            var testResult = testInput.GetClone();

            Assert.IsNotNull(testResult);
            Assert.AreEqual(testInput.Inception, testResult.Inception);
            Assert.AreEqual(testInput.DueFrequency, testResult.DueFrequency);
            Assert.AreEqual(testInput.DerivativeSlope, testResult.DerivativeSlope);
            Assert.AreEqual(testInput.SumTotal, testResult.SumTotal);
            Assert.AreEqual(testInput.IsVehiclePaidOff, testResult.IsVehiclePaidOff);
            Assert.AreEqual(testInput.NumberOfCreditCards, testResult.NumberOfCreditCards);
            Assert.AreEqual(testInput.NumberOfVehicles, testResult.NumberOfVehicles);
            Assert.AreEqual(testInput.IsRenting, testResult.IsRenting);
            Assert.AreEqual(testInput.IsPayingChildSupport, testResult.IsPayingChildSupport);
            Assert.AreEqual(testInput.IsPayingSpousalSupport, testResult.IsPayingSpousalSupport);
            Assert.IsTrue(testResult.IsPayingChildSupport);

            Assert.IsNotNull(testResult.PossibleZeroOuts);
            Assert.AreEqual(2, testResult.PossibleZeroOuts.Count);

            Assert.IsNotNull(testResult.GetChildrenAges());
            Assert.AreEqual(2, testResult.GetChildrenAges().Count);

            Assert.IsNotNull(testResult.GivenDirectly);
            Assert.AreEqual(2, testResult.GivenDirectly.Count);

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

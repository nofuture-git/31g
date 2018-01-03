using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;
using NoFuture.Rand.Data.Sp.Enums;
using NoFuture.Rand.Domus.Opes;

namespace NoFuture.Rand.Tests.DomusTests.OpesTests
{
    [TestClass]
    public class OpesOptionsTests
    {
        [TestMethod]
        public void TestGetClone()
        {
            var testInput = new OpesOptions
            {
                Inception = DateTime.Today.AddYears(-1),
                Interval = Interval.Daily,
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
            testInput.GivenDirectly.Add(new Mereo("name00", "group00"){ExpectedValue = 9900.ToPecuniam()});
            testInput.GivenDirectly.Add(new Mereo("name01", "group00") { ExpectedValue = 1100.ToPecuniam() });

            var testResult = testInput.GetClone();

            Assert.IsNotNull(testResult);
            Assert.AreEqual(testInput.Inception, testResult.Inception);
            Assert.AreEqual(testInput.Interval, testResult.Interval);
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

            Assert.IsNotNull(testResult.ChildrenAges);
            Assert.AreEqual(2, testResult.ChildrenAges.Count);

            Assert.IsNotNull(testResult.GivenDirectly);
            Assert.AreEqual(2, testResult.GivenDirectly.Count);

        }
    }
}

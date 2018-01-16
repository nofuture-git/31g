﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Data.Sp;

namespace NoFuture.Rand.Tests.DataTests.SpTests
{
    [TestClass]
    public class PersonalCreditScoreTests
    {
        [TestMethod]
        public void TestGetAgePenalty()
        {
            var testInput = new PersonalCreditScore(DateTime.Today.AddYears(-36));
            var testResult = testInput.GetAgePenalty(null);

            Assert.IsTrue(testResult >= -2.3D && testResult <= 2.27D);
        }
    }
}
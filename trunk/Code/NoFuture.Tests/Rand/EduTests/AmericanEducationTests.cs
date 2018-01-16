﻿using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NoFuture.Rand.Edu.US;
using NoFuture.Util.Core.Math;

namespace NoFuture.Rand.Tests.EduTests
{
    [TestClass]
    public class AmericanEducationTests
    {
        [TestMethod]
        public void RandomEducationNullArgs()
        {
            var testResult = AmericanEducation.RandomEducation();
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            Assert.IsNotNull(testResult.HighSchool.Item1);
            Debug.WriteLine(testResult.HighSchool);
            Debug.WriteLine(testResult.College);
        }

        [TestMethod]
        public void RandomEducationYoungChild()
        {
            var testResult = AmericanEducation.RandomEducation(DateTime.Now.AddYears(-9), "FL", "32162");
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.HighSchool);
            Assert.IsNull(testResult.HighSchool.Item1);
            Assert.IsNotNull(testResult.College);
            Assert.IsNull(testResult.College.Item1);
        }

        [TestMethod]
        public void TestGetRandomGraduationDate()
        {
            var normDist = new NormalDistEquation {Mean = 4.469, StdDev = 0.5145};
            var atDate = DateTime.Today;

            var minYears = (int)Math.Floor(4.469 - 0.5145 * 3);
            var testResult = AmericanEducation.GetRandomGraduationDate(atDate, normDist);
            System.Diagnostics.Debug.WriteLine(testResult);
            Assert.IsTrue(DateTime.Today.AddYears(minYears*-1) < testResult);
            Assert.IsTrue(new[]{5,12}.Contains(testResult.Month));
        }
    }
}
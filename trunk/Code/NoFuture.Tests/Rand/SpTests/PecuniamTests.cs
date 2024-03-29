﻿using System;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Shared.Core;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class PecuniamTests
    {
        [Test]
        public void TestOps()
        {
            var test00 = Pecuniam.Zero;
            var test01 = new Pecuniam(1.0M);
            var testResult = test00 + test01;
            Assert.AreEqual(1.0M, testResult.Amount);

            Assert.AreEqual(new Pecuniam(2.0M), new Pecuniam(1.0M) + new Pecuniam(1.0M));
            Assert.AreEqual(new Pecuniam(2.0M), new Pecuniam(4.0M) - new Pecuniam(2.0M));
            Assert.AreEqual(new Pecuniam(4.0M), new Pecuniam(4.0M) * new Pecuniam(1.0M));
            Assert.AreEqual(new Pecuniam(4.0M), new Pecuniam(4.0M) / new Pecuniam(1.0M));

            Assert.IsTrue(new Pecuniam(4.0M) > new Pecuniam(3.0M));
            Assert.IsTrue(new Pecuniam(1.0M) < new Pecuniam(3.0M));
            Assert.IsTrue(new Pecuniam(1.0M) == new Pecuniam(1.0M));
            Assert.IsTrue(new Pecuniam(2.0M) != new Pecuniam(1.0M));
        }

        [Test]
        public void TestEquals()
        {
            var test01 = new Pecuniam(138.0M);
            var test02 = new Pecuniam(138.0M);

            Assert.IsTrue(test01.Equals(test02));
        }

        [Test]
        public void TestGetRandPecuniam()
        {
            var testResult = Pecuniam.RandomPecuniam();
            Assert.IsNotNull(testResult);
            Assert.IsFalse(testResult.Equals(Pecuniam.Zero));

            testResult = Pecuniam.RandomPecuniam(10000, 99999);
            Assert.IsNotNull(testResult);
            var testResultValue = testResult.Amount;
            Assert.IsTrue(testResultValue >= 10000M && testResultValue <= 99999M);

            testResult = Pecuniam.RandomPecuniam(10000, 99999, 100);
            Assert.IsNotNull(testResult);
            testResultValue = testResult.Amount;
            Assert.IsTrue(testResultValue >= 10000M && testResultValue <= 99999M);
            Assert.IsTrue(testResult.Amount % 100M == 0);
        }

        [Test]
        public void TestDifferentCurrencyIsException()
        {
            var usd = new Pecuniam(10M);
            var yen = new Pecuniam(10M, CurrencyAbbrev.JPY);

            try
            {
                var shouldBlow = usd + yen;
                Assert.False(false);
            }
            catch
            {
                Assert.True(true);
            }
        }

        [Test]
        public void TestGetTropicalMonth()
        {
            var testResult = PecuniamExtensions.GetTropicalMonth();
            Console.WriteLine($"Days: {testResult.Days}, Hours: {testResult.Hours}, Minutes: {testResult.Minutes}, Seconds: {testResult.Seconds}, Milliseconds: {testResult.Milliseconds}");

            var back2Year = testResult.Ticks * 12L;
            Assert.AreEqual(Constants.TropicalYear.Ticks, back2Year);

        }

        [Test]
        public void TestToInterval()
        {
            var testInput = PecuniamExtensions.GetTropicalMonth();
            var testResult = PecuniamExtensions.ToInterval(testInput);
            Console.WriteLine(testResult.ToString());
            Assert.AreEqual(Interval.Monthly, testResult);

            testInput = Constants.TropicalYear;
            testResult = PecuniamExtensions.ToInterval(testInput);
            Console.WriteLine(testResult.ToString());
            Assert.AreEqual(Interval.Annually, testResult);

        }
    }
}

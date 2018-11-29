using System;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class LedgerTests
    {
        [Test]
        public void TestAdd()
        {
            var dt = DateTime.Today;
            var testSubject = new Ledger();
            var testName00 = "Assets";
            var testResults00 = testSubject.Add(testName00, KindsOfAccounts.Asset, false, 101, dt.AddDays(-12));
            Assert.IsNotNull(testResults00);
            Assert.AreEqual(testName00, testResults00.Id.Value);
            Assert.AreEqual("101", testResults00.Id.Abbrev);

            var testResults01 = testSubject.Add(testName00, KindsOfAccounts.Asset, false);
            Assert.IsNotNull(testResults01);
            Assert.IsTrue(testResults00.Equals(testResults01));

        }

        [Test]
        public void TestGet()
        {
            var dt = DateTime.Today;
            var testSubject = new Ledger();
            var testName00 = "Assets";
            var testResults00 = testSubject.Add(testName00, KindsOfAccounts.Asset, false, 101, dt.AddDays(-12));

            Assert.IsNotNull(testResults00);
            var testResults01 = testSubject.Get(testName00);
            Assert.IsNotNull(testResults01);
            Assert.IsTrue(testResults00.Equals(testResults01));

            var testResults02 = testSubject.Get(101);
            Assert.IsNotNull(testResults02);
            Assert.IsTrue(testResults01.Equals(testResults02));

            var voca = new VocaBase(testName00);
            var testResults03 = testSubject.Get(voca);
            Assert.IsNotNull(testResults03);
            Assert.IsTrue(testResults02.Equals(testResults03));
        }

        [Test]
        public void TestRemove()
        {
            var dt = DateTime.Today;
            var testSubject = new Ledger();
            var testName00 = "Assets";
            var testName01 = "Liabilities";
            var testResults00 = testSubject.Add(testName00, KindsOfAccounts.Asset, false, 101, dt.AddDays(-12));
            Assert.IsNotNull(testResults00);
            var testResults01 = testSubject.Add(testName01, KindsOfAccounts.Asset, false, 112, dt.AddDays(-12));
            Assert.IsNotNull(testResults01);
            var testResults02 = testSubject.Remove(testName01);
            Assert.IsNotNull(testResults02);
            Assert.IsTrue(testResults01.Equals(testResults02));

            Assert.IsNull(testSubject.Get(testName01));

        }

        [Test]
        public void TestPostBalance()
        {
            var testInput = new Balance("Journal-552");
            var dt = DateTime.UtcNow;
            var assets = new VocaBase("Assets");
            var liabilities = new VocaBase("Liabilities");
            var ownersEquity = new VocaBase("Owner's Equity");
            testInput.AddNegativeValue(dt.AddDays(-360), new Pecuniam(-450.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-30), new Pecuniam(-461.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-120), new Pecuniam(-458.0M), ownersEquity);
            testInput.AddNegativeValue(dt.AddDays(-150), new Pecuniam(-457.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-90), new Pecuniam(-459.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-240), new Pecuniam(-454.0M), ownersEquity);
            testInput.AddNegativeValue(dt.AddDays(-60), new Pecuniam(-460.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-300), new Pecuniam(-452.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-270), new Pecuniam(-453.0M), ownersEquity);
            testInput.AddNegativeValue(dt.AddDays(-180), new Pecuniam(-456.0M), assets);
            testInput.AddNegativeValue(dt.AddDays(-210), new Pecuniam(-455.0M), liabilities);
            testInput.AddNegativeValue(dt.AddDays(-330), new Pecuniam(-451.0M), assets);

            //charges
            testInput.AddPositiveValue(dt.AddDays(-365), new Pecuniam(8000.0M), assets);
            testInput.AddPositiveValue(dt.AddDays(-350), new Pecuniam(164.4M), liabilities);
            testInput.AddPositiveValue(dt.AddDays(-198), new Pecuniam(165.4M), ownersEquity);
            testInput.AddPositiveValue(dt.AddDays(-24), new Pecuniam(166.4M), assets);
            testInput.AddPositiveValue(dt.AddDays(-74), new Pecuniam(167.4M), liabilities);
            testInput.AddPositiveValue(dt.AddDays(-88), new Pecuniam(168.4M), ownersEquity);
            testInput.AddPositiveValue(dt.AddDays(-92), new Pecuniam(169.4M), assets);
            testInput.AddPositiveValue(dt.AddDays(-121), new Pecuniam(170.4M), liabilities);
            testInput.AddPositiveValue(dt.AddDays(-180), new Pecuniam(171.4M), ownersEquity);
            testInput.AddPositiveValue(dt.AddDays(-142), new Pecuniam(172.4M), assets);
            testInput.AddPositiveValue(dt.AddDays(-155), new Pecuniam(173.4M), liabilities);

            var testSubject = new Ledger();
            testSubject.PostBalance(testInput);

            var testResult00 = testSubject.Get("Assets");
            Assert.IsNotNull(testResult00);

            var testResult01 = testSubject.Get("Liabilities");
            Assert.IsNotNull(testResult01);

            var testResult02 = testSubject.Get("Owner's Equity");
            Assert.IsNotNull(testResult02);
        }


    }
}

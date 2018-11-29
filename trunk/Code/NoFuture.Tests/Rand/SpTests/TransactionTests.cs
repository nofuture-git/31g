using System;
using System.Linq;
using NoFuture.Rand.Core;
using NoFuture.Rand.Sp;
using NoFuture.Shared.Core;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class TransactionTests
    {
        [Test]
        public void TestGetInverse()
        {
            var testSubject = new Transaction(DateTime.UtcNow, 128M.ToPecuniam());
            var testResult = testSubject.GetInverse();
            Assert.IsTrue((testSubject.Cash + testResult.Cash).GetRounded() == Pecuniam.Zero);
        }

        [Test]
        public void TestGetClone()
        {
            var testSubject = new Transaction(DateTime.UtcNow, 128M.ToPecuniam());
            var testClone = testSubject.Clone();
            var testClone2 = testClone.Clone();
            var testClone3 = testClone2.Clone();

            Assert.IsNotNull(testClone3);
            var testResult = testClone3.Trace;
            Assert.IsNotNull(testResult);

            var trs = new bool[]
                {testSubject.Equals(testClone), testClone.Equals(testClone2), testClone2.Equals(testClone3)};
            Assert.IsTrue(trs.All(x => x == false));

            Console.WriteLine(testClone3.UniqueId);
            var c = 1;
            while (testResult != null)
            {
                Console.WriteLine($"{new string('\t', c)}{testResult.UniqueId}");
                c += 1;
                testResult = testResult.Trace;
            }
        }

        [Test]
        public void TestSplitOnAmount()
        {
            var testSubject = new Transaction(DateTime.UtcNow, 128M.ToPecuniam());
            var testAmount = 56M.ToPecuniam();
            var testResult = testSubject.SplitOnAmount(testAmount);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Item1);
            Assert.IsNotNull(testResult.Item2);
            Assert.AreEqual(testAmount,testResult.Item1.Cash);
            Assert.AreEqual(testSubject.Cash - testAmount, testResult.Item2.Cash);
            Assert.AreEqual(testSubject.Cash, testResult.Item1.Cash + testResult.Item2.Cash);

            Assert.Throws<ArgumentNullException>(() => testSubject.SplitOnAmount(null));
            Assert.Throws<ArgumentNullException>(() => testSubject.SplitOnAmount(Pecuniam.Zero));
            Assert.Throws<WatDaFookIzDis>(() => testSubject.SplitOnAmount(512M.ToPecuniam()));

        }

        [Test]
        public void TestSplitOnPercent()
        {
            var testSubject = new Transaction(DateTime.UtcNow, 128M.ToPecuniam());
            var testAmount = 64M.ToPecuniam();
            var testResult = testSubject.SplitOnPercent(50.0);
            Assert.IsNotNull(testResult);
            Assert.IsNotNull(testResult.Item1);
            Assert.IsNotNull(testResult.Item2);
            Assert.AreEqual(testAmount, testResult.Item1.Cash);
            Assert.AreEqual(testSubject.Cash - testAmount, testResult.Item2.Cash);
            Assert.AreEqual(testSubject.Cash, testResult.Item1.Cash + testResult.Item2.Cash);

            Assert.Throws<ArgumentException>(() => testSubject.SplitOnPercent(0));
            Assert.Throws<ArgumentException>(() => testSubject.SplitOnPercent(300));
        }

        [Test]
        public void TestGetThisAsTraceId()
        {
            var testSubject = new Transaction(DateTime.UtcNow, 8000M.ToPecuniam(), new VocaBase("Owner's Capital"));
            var testResult = testSubject.GetThisAsTraceId();
            Assert.IsNotNull(testResult);
            Assert.AreEqual(testSubject.AtTime, testResult.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResult.UniqueId);
            Assert.AreEqual(testSubject.Description, testResult.Description);

            testResult = testSubject.GetThisAsTraceId(DateTime.UtcNow.AddDays(1));
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(testSubject.AtTime, testResult.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResult.UniqueId);
            Assert.AreEqual(testSubject.Description, testResult.Description);

            var journalName = new VocaBase("Journal J1");
            testResult = testSubject.GetThisAsTraceId(DateTime.UtcNow.AddDays(1), journalName);
            Assert.IsNotNull(testResult);
            Assert.AreNotEqual(testSubject.AtTime, testResult.AtTime);
            Assert.AreEqual(Guid.Empty, testResult.UniqueId);
            Assert.AreEqual(journalName, testResult.Description);

            var testResultTrace = testResult.Trace;
            Assert.IsNotNull(testResultTrace);
            Assert.AreEqual(testSubject.AtTime, testResultTrace.AtTime);
            Assert.AreEqual(testSubject.UniqueId, testResultTrace.UniqueId);
            Assert.AreEqual(testSubject.Description, testResultTrace.Description);
        }
    }
}

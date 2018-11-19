using System;
using NoFuture.Rand.Sp;
using NoFuture.Rand.Sp.Enums;
using NoFuture.Util.Core.Math;
using NUnit.Framework;

namespace NoFuture.Rand.Tests.SpTests
{
    [TestFixture]
    public class LoanTests
    {

        [Test]
        public void TestGetMinPayment()
        {
            var testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);
            AddTransactionsOld(testSubject);
            AddRecentPayments(testSubject);

            var testResult = testSubject.GetMinPayment(DateTime.Now.AddDays(-15).Date);

            Assert.AreEqual(-52.79M, testResult.Amount);

            Console.WriteLine(testResult.Amount);
        }

        [Test]
        public void TestGet30yearBalance()
        {
            var testSubject = new FixedRateLoan(DateTime.Today, 0.0885F, new Pecuniam(150000)) {Rate = 0.05F};
            var testResult = testSubject.GetValueAt(DateTime.Today.AddYears(30));

            Console.WriteLine(testResult);

            var fv = Econ.PerDiemInterest(150000M, 0.03F,
                (DateTime.Today.AddYears(30) - DateTime.Today).TotalDays);

            Console.WriteLine(fv);

            var v = fv/30;
            v = v/12;
            Console.WriteLine(v);
        }

        [Test]
        public void TestGetStatus()
        {
            var testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);
            AddTransactionsOld(testSubject);
            AddRecentPayments(testSubject);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-15), new Pecuniam(-461.0M));
            var testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.Current, testResult);

            testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);

            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.NoHistory, testResult);

            testSubject.Terminus = DateTime.Now.AddDays(-1).Date;

            testSubject.Closure = ClosedCondition.ClosedWithZeroBalance;

            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.Closed, testResult);

            testSubject.Terminus = DateTime.Now.AddDays(1).Date;
            testSubject.Closure = ClosedCondition.ClosedWithZeroBalance;

            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreNotEqual(SpStatus.Closed, testResult);

            testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);
            testSubject.DueFrequency = new TimeSpan(28,0,0,0);
            AddTransactionsOld(testSubject);
            AddRecentPayments(testSubject);

            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.Late, testResult);

            testSubject.DueFrequency = new TimeSpan(45, 0, 0, 0);
            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.Current, testResult);

            //account openned then over paid off - better be current
            testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-360), new Pecuniam(-9000.0M));

            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.Current, testResult);

            //immediate payment didn't cover it cause of per diem interest
            testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F) {Rate = 0.0825f};
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-40), new Pecuniam(8000.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-35), new Pecuniam(-8000.0M));

            testResult = testSubject.GetStatus(DateTime.Now);
            Assert.AreEqual(SpStatus.Late, testResult);
        }

        [Test]
        public void TestGetDelinquency()
        {
            var testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);
            AddTransactionsOld(testSubject);
            AddRecentPayments(testSubject);

            var testResult = testSubject.GetDelinquency(DateTime.Now);
            Assert.IsNull(testResult);

            testSubject = new FixedRateLoan(DateTime.Now.AddYears(-3).Date, 0.0125F);
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M));

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-180 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            var dt = DateTime.Now;

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.HundredAndEighty, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-179 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.Ninety, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-90 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.Ninety, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-89 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.Sixty, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-60 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.Sixty, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-59 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.Thirty, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-30 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.AreEqual(PastDue.Thirty, testResult);

            testSubject.AddNegativeValue(DateTime.Now.AddDays(-29 - testSubject.DueFrequency.Value.TotalDays),
                    new Pecuniam(-461.0M));

            testResult = testSubject.GetDelinquency(dt);
            Assert.IsNull(testResult);
            
        }

        internal void AddRecentPayments(FixedRateLoan testSubject)
        {
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-29), new Pecuniam(-461.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-60), new Pecuniam(-460.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-90), new Pecuniam(-459.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-120), new Pecuniam(-458.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-150), new Pecuniam(-457.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-180), new Pecuniam(-456.0M));
        }

        internal void AddTransactionsOld(FixedRateLoan  testSubject)
        {

            //monthly payments
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-360), new Pecuniam(-450.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-240), new Pecuniam(-454.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-300), new Pecuniam(-452.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-270), new Pecuniam(-453.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-210), new Pecuniam(-455.0M));
            testSubject.AddNegativeValue(DateTime.Now.AddDays(-330), new Pecuniam(-451.0M));

            //charges
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-365), new Pecuniam(8000.0M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-350), new Pecuniam(164.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-198), new Pecuniam(165.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-24), new Pecuniam(166.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-74), new Pecuniam(167.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-88), new Pecuniam(168.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-92), new Pecuniam(169.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-121), new Pecuniam(170.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-180), new Pecuniam(171.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-142), new Pecuniam(172.4M));
            testSubject.AddPositiveValue(DateTime.Now.AddDays(-155), new Pecuniam(173.4M));            
        }
    }
}
